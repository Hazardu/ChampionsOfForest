using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bolt;

using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
	public partial class EnemyProgression : MonoBehaviour
	{
		public Dictionary<int, EnemyDebuff> slows;
		public Dictionary<int, EnemyDebuff> dmgTakenDebuffs;
		public Dictionary<int, EnemyDebuff> dmgDealtDebuffs;
		public Dictionary<int, EnemyDebuff> FireDamageDebuffs;
		public Vector3 knockbackDir;
		public float knockbackSpeed;
		public float DebuffDmgMult;
		public float dmgTakenIncrease;
		public const float KnockBackDeacceleration = 100f;


		//Damage over time
		public List<DoT> DamageOverTimeList;

		public class DoT
		{
			/// <summary>
			/// Amount of damage dealt second
			/// </summary>
			public float Amount;

			/// <summary>
			/// Timestamp when stoptime needs to be deleted;
			/// </summary>
			private int Ticks;

			public bool Tick()
			{
				Ticks--;
				return Ticks > 0;
			}

			public DoT(float Damage, float duration)
			{
				Amount = Damage;
				Ticks = Mathf.CeilToInt(duration);
			}
		}
		private float DoTTimestamp;
		private void UpdateDoT()
		{
			if (DoTTimestamp < Time.time)
			{
				if (DamageOverTimeList != null && DamageOverTimeList.Count > 0)
				{
					if (extraHealth > 0)
					{
						float i = Mathf.Min(extraHealth, DoTTotal);
						extraHealth -= i;
						HealthScript.Health -= Mathf.FloorToInt(DoTTotal - i);
						Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, DoTTotal, Color.black);
					}
					else
					{
						HealthScript.Health -= Mathf.FloorToInt(DoTTotal);
						Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, DoTTotal, Color.black);
					}
					for (int i = 0; i < DamageOverTimeList.Count; i++)
					{
						if (!DamageOverTimeList[i].Tick())
							DamageOverTimeList.RemoveAt(i);

					}

					GetTotalDoT();
				}
				DoTTimestamp = Time.time + 1;
			}
		}
		public void DoDoT(float dmg, float duration, DamageType dt = DamageType.Physical)
		{
			if (abilities == null || !abilities.Contains(Abilities.Juggernaut))
			{
				switch (dt)
				{
					case DamageType.Pure:
						DamageOverTimeList.Add(new DoT(dmg, duration));
						break;

					case DamageType.Physical:
						DamageOverTimeList.Add(new DoT(ClampDamage(false, dmg, false), duration));
						break;

					case DamageType.Magical:
						DamageOverTimeList.Add(new DoT(ClampDamage(false, dmg, true), duration));
						break;
				}
				GetTotalDoT();
			}
		}

		private float DoTTotal;

		private void GetTotalDoT()
		{
			DoTTotal = DamageOverTimeList.Sum(x => x.Amount);
		}

		public void AddKnockback(Vector3 dir, float speed)
		{
			knockbackDir += dir;
			knockbackDir.Normalize();
			if (speed > knockbackSpeed)
				knockbackSpeed = speed;
		}
		public void AddKnockbackByDistance(Vector3 dir, float distance)
		{
			var velocity = Mathf.Sqrt(2 * distance * KnockBackDeacceleration);
			knockbackDir += dir;
			knockbackDir.Normalize();
			if (velocity > knockbackSpeed)
				knockbackSpeed = velocity;


		}
		public static void AddKnockbackByDistance(ulong EnemyID, Vector3 dir, float distance)
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(43);
					w.Write(EnemyID);
					w.Write(dir.x);
					w.Write(dir.y);
					w.Write(dir.z);
					w.Write(EnemyID);
					w.Write(distance);
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
				answerStream.Close();
			}
		}

		public void FireDebuff(int source, float amount, float time)
		{
			Debug.LogWarning("Fire debuff, " + source + ", " + amount + ", " + time);
			if (FireDamageDebuffs.ContainsKey(source))
			{
				FireDamageDebuffs[source].duration = Mathf.Max(FireDamageDebuffs[source].duration, time);
				FireDamageDebuffs[source].amount = amount;
			}
			else
			{
				FireDamageDebuffs.Add(source, new EnemyDebuff()
				{
					Source = source,
					amount = amount,
					duration = time,
				});
			}
		}
		public static void Slow(BoltEntity entity, int source, float amount, float time)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(22);
					w.Write(entity.networkId.PackedValue);
					w.Write(amount);
					w.Write(time);
					w.Write(source);
					w.Close();
				}
				Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
				answerStream.Close();
			}
		}
		public void Slow(int source, float amount, float time)
		{
			//source - 20 is snap freeze
			//source - 21 is snap freeze super slow
			//source - 40 is hammer attack
			//source - 41 is magic arrow hit
			//source - 43-60 are bashes
			//source - 61-75 are player hits
			//source - 90 - focus on headshot
			//source - 91 - seeking arrow on body shot;
			//source - 120-135 -afterburn;
			//source - 140 is cataclysm fire
			//source - 141 is cataclysm arcane
			//source - 142 is the fart
			//source - 143 is taunt increase in atkspeed
			//source - 144 is blizzard slow
			if (abilities.Contains(Abilities.Juggernaut))
				return;
			if (slows.ContainsKey(source))
			{
				slows[source].duration = Mathf.Max(slows[source].duration, time);
				slows[source].amount = Mathf.Min(amount, slows[source].amount);
			}
			else
			{
				slows.Add(source, new EnemyDebuff()
				{
					Source = source,
					amount = amount,
					duration = time,
				});
			}
		}


		public void DmgTakenDebuff(int source, float amount, float time)
		{
			if (dmgTakenDebuffs.ContainsKey(source))
			{
				dmgTakenDebuffs[source].duration = Mathf.Max(dmgTakenDebuffs[source].duration, time);
				dmgTakenDebuffs[source].amount = amount;
			}
			else
			{
				dmgTakenDebuffs.Add(source, new EnemyDebuff()
				{
					Source = source,
					amount = amount,
					duration = time,
				});
			}
		}


		public void ReduceArmor(int amount)
		{
			ArmorReduction += amount;
		}


		public static void ReduceArmor(BoltEntity target, int amount)
		{
			if (GameSetup.IsMultiplayer)
			{
				PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
				playerHitEnemy.Target = target;
				playerHitEnemy.Hit = -amount;
				playerHitEnemy.Send();
			}
			else
			{
				EnemyManager.hostDictionary[target.networkId.PackedValue].ArmorReduction += amount;
			}
		}

		public static void ReduceArmor(EnemyProgression target, int amount)
		{
			target.ArmorReduction += amount;
		}

		public void Taunt(GameObject player, in float duration, in float slowAmount)
		{
			Slow(143, slowAmount, duration);
			DmgTakenDebuff(143, 1.5f, duration);
			setup.ai?.resetCombatParams();

			this.setup.pmCombat.enabled = true;
			if (setup.aiManager)
			{
				this.setup.aiManager.setAggressiveCombat();
				this.setup.aiManager.setCaveCombat();   //the most agressive combat mode
			}
			if (setup.pmBrain)
			{
				this.setup.pmBrain.SendEvent("toSetAggressive");
				this.setup.pmBrain.SendEvent("toActivateFSM");
				this.setup.pmBrain.FsmVariables.GetFsmBool("playerIsRed").Value = false;
			}
			setup.worldSearch?.setEncounterType();
			setup.familyFunctions?.SendMessage("TauntFamily", duration);

			setup.search?.GetComponent<Enemies.enemySearchMod>()?.Taunt(player, duration);

		}
	}
}
