using System;
using System.Collections.Generic;
using System.IO;

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
		public enum DamageType
		{
			Pure, Physical, Magical
		}

		//Type of enemy
		public enum EnemyRarity
		{
			Normal, Elite, Miniboss, Boss
		}
		public EnemyRarity _rarity;

		//References to vanilla objects
		public EnemyHealth HealthScript;
		public mutantAI AIScript;
		public BoltEntity entity;
		public mutantScriptSetup setup;


		#region Variables

		public string enemyName;

		public int level;
		public int armor;
		public int armorReduction;
		public double extraHealth;
		public double maxHealth;
		private int baseHealth = 0;
		public double HP => (float) extraHealth + HealthScript.Health;
		public float DamageMult;
		public float BaseDamageMult;
		public float DamageAmp => DamageMult;
		public float DamageTotal => DamageMult * DebuffDmgMult;
		public float FireDmgAmp = 1;
		public float FireDmgBonus;

		public long bounty;

		public float Steadfast = 100;
		private float steadfastCap = 100000;
		public bool setupComplete = false;
		public bool CCimmune = false;
		private bool DualLifeSpend = false;
		public bool OnDieCalled = false;
		public List<Abilities> abilities;
		public Enemy enemyType;
		public float CreationTime;
		public float AnimSpeed = 1;
		public float BaseAnimSpeed = 1;
		private float prerainDmg;
		private int prerainArmor;
		private readonly float agroRange = 1200;

		//cooldowns
		private float abilityCooldown;
		private float freezeauraCD;
		private float blackholeCD;
		private float blinkCD;
		private float shieldingCD;
		private float shieldingON;
		private float chainsCD;
		private float trapSphereCD;
		private float laserTurretCD;
		private float meteorCD;
		private float sunflareCD;
		private float arcaneCataclysmCD;
		private float fireCataclysmCD;

		//Closest player, for detecting if in range to cast abilities
		private GameObject closestPlayer;
		private float closestPlayerMagnitude;
		public Avenger avengerability;
		private float timeOfDeath;
		private Color normalColor;
		private ModSettings.GameDifficulty setupDifficulty;
		public enum Abilities
		{
			Steadfast, BossSteadfast, EliteSteadfast, Blizzard, Radiance, Chains, BlackHole, Trapper, Juggernaut, Gargantuan, Tiny, ExtraDamage, ExtraHealth, Basher, Warp, RainEmpowerment, Shielding, Meteor, Flare, Undead, Laser, Poisonous, Sacrifice, Avenger, FireCataclysm, ArcaneCataclysm
		}

		public enum Enemy
		{
			All = 0,
			RegularArmsy = 0b1,
			PaleArmsy = 0b10,
			RegularVags = 0b100,
			PaleVags = 0b1000,
			Cowman = 0b10000,
			Baby = 0b100000,
			Girl = 0b1000000,
			Worm = 0b10000000,
			Megan = 0b100000000,
			NormalMale = 0b1000000000,
			NormalLeaderMale = 0b10000000000,
			NormalFemale = 0b100000000000,
			NormalSkinnyMale = 0b1000000000000,
			NormalSkinnyFemale = 0b10000000000000,
			PaleMale = 0b100000000000000,
			PaleSkinnyMale = 0b1000000000000000,
			PaleSkinnedMale = 0b10000000000000000,
			PaleSkinnedSkinnyMale = 0b100000000000000000,
			PaintedMale = 0b1000000000000000000,
			PaintedLeaderMale = 0b10000000000000000000,
			PaintedFemale = 0b100000000000000000000,
			Fireman =	0b1000000000000000000000,
		};

		#endregion Variables

		void OnDestroy()
		{
			EnemyManager.enemyByTransform.Remove(transform);
		}

		private void OnEnable()
		{
			OnDieCalled = false;
		}
		void ClampHealth()
		{
			if (HP > maxHealth)
			{
				double diff = HP - maxHealth;
				if (extraHealth > 0)
				{
					double min = Math.Min(extraHealth, diff);
					extraHealth -= min;
					diff -= min;
				}
				HealthScript.Health -= (int)diff;
			}
		}
		private void Update()
		{
			if (!setupComplete)
			{
				if (HealthScript.Health > 0 && ModSettings.DifficultyChosen)
				{
					Setup();
				}
				return;
			}
			else
			{
				ClampHealth();
			}
			if (setup.ai.creepy_boss && !setup.ai.girlFullyTransformed)
			{
				return;
			}
			if (OnDieCalled && HP > 0)
			{
				timeOfDeath -= Time.deltaTime;
				if (timeOfDeath < 0)
				{
					OnDieCalled = false;
				}
			}
			AnimSpeed = BaseAnimSpeed;

			if (knockbackSpeed > 0)
			{
				knockbackSpeed -= Time.deltaTime * KnockBackDeacceleration;
				transform.root.Translate(knockbackDir * knockbackSpeed * Time.deltaTime);
				AnimSpeed = 0;
			}

			FireDmgBonus = 0;
			foreach (EnemyDebuff item in FireDamageDebuffs.Values)
			{
				FireDmgBonus += item.amount;
			}

			int[] FDBKeys = new List<int>(FireDamageDebuffs.Keys).ToArray();
			for (int i = 0; i < FDBKeys.Length; i++)
			{
				int key = FDBKeys[i];

				FireDamageDebuffs[key].duration -= Time.deltaTime;

				if (FireDamageDebuffs[key].duration < 0)
				{
					FireDamageDebuffs.Remove(key);
				}
			}

			int[] DTDKeys = new List<int>(dmgTakenDebuffs.Keys).ToArray();
			int[] DDDKeys = new List<int>(dmgDealtDebuffs.Keys).ToArray();
			DebuffDmgMult = 1;
			dmgTakenIncrease = 1;

			for (int i = 0; i < DTDKeys.Length; i++)
			{
				int key = DTDKeys[i];
				dmgTakenIncrease *= dmgTakenDebuffs[key].amount;
				dmgTakenDebuffs[key].duration -= Time.deltaTime;

				if (dmgTakenDebuffs[key].duration < 0)
				{
					dmgTakenDebuffs.Remove(key);
				}
			}

			for (int i = 0; i < DDDKeys.Length; i++)
			{
				int key = DDDKeys[i];
				DebuffDmgMult *= dmgDealtDebuffs[key].amount;
				dmgDealtDebuffs[key].duration -= Time.deltaTime;

				if (dmgDealtDebuffs[key].duration < 0)
				{
					dmgDealtDebuffs.Remove(key);
				}
			}

			int[] Keys = new List<int>(slows.Keys).ToArray();
			for (int i = 0; i < Keys.Length; i++)
			{
				int key = Keys[i];
				if (!(slows[key].amount < 1 && CCimmune))
				{
					AnimSpeed *= slows[key].amount;
					slows[key].duration -= Time.deltaTime;
				}
				else
				{
					slows[key].duration = -1;
				}
				if (slows[key].duration < 0)
				{
					slows.Remove(key);
				}
			}
			if (extraHealth > 0)
			{
				if (HealthScript.Health < int.MaxValue / 4)
				{
					double d = int.MaxValue / 2 - HealthScript.Health;
					d = Math.Min(d, extraHealth);
					HealthScript.Health += (int)d;
					extraHealth -= d;
				}
			}
			UpdateDoT();
			UpdateAbilities();
			AIScript.animSpeed = AnimSpeed;
			setup.animator.speed = AnimSpeed;
		}





		public bool OnDie()
		{
			try
			{
				DamageOverTimeList.Clear();
				if (GameSetup.IsMpClient)
				{
					return true;
				}
				if (setup.waterDetect.drowned)
				{
					return true;
				}
				if (abilities.Contains(Abilities.Undead))
				{
					if (!DualLifeSpend)
					{
						DualLifeSpend = true;
						extraHealth = maxHealth / 2;
						HealthScript.MySkin.material.color = Color.green;
						prerainDmg *= 2;

						HealthScript.releaseFromTrap();
						OnDieCalled = false;
						return false;
					}
				}

				if (OnDieCalled)
				{
					return true;
				}
				EnemyManager.RemoveEnemy(this);
				if (BoltNetwork.isRunning)
				{
					foreach (EnemyProgression item in EnemyManager.hostDictionary.Values)
					{
						if (item != null && item.gameObject != null && item.gameObject.activeSelf)
						{
							item.gameObject.SendMessage("ThisEnemyDied", this, SendMessageOptions.DontRequireReceiver);
						}
					}
					if (abilities.Contains(Abilities.Sacrifice))
					{
						Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.Sacrifice, 2000);
						foreach (EnemyProgression item in EnemyManager.hostDictionary.Values)
						{
							if (!(item != null && item != this && item.gameObject != null && item.gameObject.activeSelf))
							{
								continue;
							}

							if ((item.transform.position - transform.position).sqrMagnitude > 100)
							{
								continue;
							}

							item.armorReduction = 0;
							item.armor = Mathf.RoundToInt(item.armor* 1.5f);
							item.BaseAnimSpeed *= 1.30f;
							item.BaseDamageMult *= 2f;
							item.extraHealth = (item.maxHealth * 1.15);
							
						}
					}
				}
				else
				{
					foreach (var item in EnemyManager.enemyByTransform)
					{
						if (item.Value != null && item.Value.gameObject != null && item.Value.gameObject.activeSelf)
						{
							if (item.Value.avengerability != null)
								item.Value.avengerability.ThisEnemyDied(this);
						}
					}
					if (abilities.Contains(Abilities.Sacrifice))
					{
						foreach (var item in EnemyManager.enemyByTransform)
						{
							if (item.Value != null && item.Value.gameObject != null && item.Value.gameObject.activeSelf)
							{
								if (!((item.Key.position - transform.position).sqrMagnitude > 4303))
								{
									item.Value.armorReduction = 0;
									item.Value.BaseAnimSpeed *= 1.25f;
									item.Value.BaseDamageMult *= 2f;
									item.Value.extraHealth = item.Value.maxHealth;
								}
							}
						}
					}
				}
				DropLoot();
				if (GameSetup.IsMpServer)
				{
					ModAPI.Console.Write("There are " + ModReferences.Players.Count + " players on the server");
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(10);
							w.Write(Convert.ToInt64(bounty / (Mathf.Max(1, ModReferences.Players.Count))));
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
						answerStream.Close();
					}
				}
				else if (GameSetup.IsSinglePlayer)
				{
					ModdedPlayer.instance.AddKillExperience(bounty);
				}
				OnDieCalled = true;
				timeOfDeath = 120;
				HealthScript.Health = 0;
				extraHealth = 0;
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write("DYING ENEMY EXCEPTION  " + ex);
			}

			return true;
		}
		private void DropLoot()
		{

			if (Random.value <= 0.1f * ModSettings.DropChanceMultiplier * ModdedPlayer.Stats.magicFind_quantity.Value || AIScript.creepy_boss || abilities.Count > 0)
			{
				int itemCount = Random.Range(2, 4);
				if (AIScript.creepy_boss)
				{
					itemCount += 100;
					int pc = ModReferences.Players.Count;
					for (int i = 0; i < pc; i++)
					{
						////Drop megan only amulet
						//Network.NetworkManager.SendItemDrop(new Item(ItemDatabase.itemLookup[80]), LocalPlayer.Transform.position + Vector3.up * 2, ItemPickUp.DropSource.EnemyOnDeath);
					}
				}
				else if (abilities.Count >= 3)
				{
					itemCount += Random.Range(2, 5);
				}
				if (_rarity == EnemyRarity.Boss)
				{
					itemCount += 7;
				}
				else if (_rarity == EnemyRarity.Miniboss)
				{
					itemCount += 3;
				}
				itemCount += Mathf.Clamp(level / 50, 1, 8);
				itemCount = Mathf.RoundToInt(itemCount * ModSettings.DropQuantityMultiplier);
				ModReferences.SendRandomItemDrops(itemCount, enemyType, bounty, setupDifficulty, transform.position);
			}

		}
		float scale = 1; Color color = Color.white;
		private void UpdateVisual()
		{
			float sc = 1;
			Color c = normalColor;
			if (abilities.Contains(Abilities.Avenger))
			{
				sc += 0.1f * avengerability.Stacks;
			}
			if (abilities.Contains(Abilities.RainEmpowerment))
			{
				if (Scene.WeatherSystem.Raining)
				{
					armor = prerainArmor * 5;
					DamageMult = prerainDmg * 5;
					sc *= 1.5f;

					AnimSpeed *= 2;
				}
				else
				{
					armor = prerainArmor;
					DamageMult = prerainDmg;
				}
			}
			armorReduction = Mathf.Min(armorReduction, armor);
			if (abilities.Contains(Abilities.Gargantuan))
			{
				sc *= 2f;
			}
			else if (abilities.Contains(Abilities.Tiny))
			{
				sc *= 0.35f;
			}
			if (abilities.Contains(Abilities.Undead))
			{
				if (DualLifeSpend)
				{
					c = Color.green;
					AnimSpeed *= 1.1f;
					sc *= 1.4f;
				}
			}

			if (abilities.Contains(Abilities.Shielding))
			{
				if (shieldingON > 0)
				{
					shieldingON -= Time.deltaTime;
					c = Color.black;

					if (shieldingON <= 0)
					{
						HealthScript.MySkin.material.color = normalColor;
					}
				}
				if (shieldingCD > 0)
				{
					shieldingCD -= Time.deltaTime;
				}
			}

			HealthScript.MySkin.material.color = c;
			transform.localScale = Vector3.one * sc;
			if (BoltNetwork.isServer && (scale != sc || color != c))
			{
				scale = sc;
				color = c;
				SyncAppearance(entity.networkId.PackedValue);
			}
		}
		internal void SyncAppearance(ulong id)
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(30);
					w.Write(id);
					w.Write(BaseDamageMult);
					w.Write(scale);
					w.Write(color.r);
					w.Write(color.g);
					w.Write(color.b);
					w.Write(color.a);
					foreach (EnemyProgression.Abilities ability in abilities)
					{
						w.Write((int)ability);
					}
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Clients);
				answerStream.Close();
			}
		}
		private void UpdateAbilities()
		{

			if (arcaneCataclysmCD > 0)
				arcaneCataclysmCD -= Time.deltaTime;
			if (fireCataclysmCD > 0)
				fireCataclysmCD -= Time.deltaTime;
			if (trapSphereCD > 0)
				trapSphereCD -= Time.deltaTime;
			if (chainsCD > 0)
				chainsCD -= Time.deltaTime;
			if (laserTurretCD > 0)
				laserTurretCD -= Time.deltaTime;
			if (meteorCD > 0)
				meteorCD -= Time.deltaTime;
			if (blackholeCD > 0)
				blackholeCD -= Time.deltaTime;
			if (blinkCD > 0)
				blinkCD -= Time.deltaTime;
			if (sunflareCD > 0)
				sunflareCD -= Time.deltaTime;
			if (freezeauraCD > 0)
				freezeauraCD -= Time.deltaTime;

			if (abilityCooldown > 0)
			{
				abilityCooldown -= Time.deltaTime;
				return;
			}

			bool inRange = false;
			closestPlayerMagnitude = agroRange;
			foreach (GameObject g in AIScript.allPlayers)
			{
				float f = (g.transform.position - transform.position).sqrMagnitude;
				if (f < agroRange)
				{
					if (f < closestPlayerMagnitude)
					{
						closestPlayer = g;
						closestPlayerMagnitude = f;
					}
					inRange = true;
				}
			}

			UpdateVisual();

			if (inRange)
			{
				if (abilities.Contains(Abilities.Meteor) && meteorCD <= 0)
				{
					SetAbilityCooldown(0, 1);
					Vector3 dir = closestPlayer.transform.position;

					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(17);
							w.Write(dir.x);
							w.Write(dir.y);
							w.Write(dir.z);
							w.Write(Random.Range(-100000, 100000));
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
						answerStream.Close();
					}
					meteorCD = 50f;
					return;
				}
				if (abilities.Contains(Abilities.Flare) && sunflareCD <= 0)
				{
					SetAbilityCooldown(0, 1);
					Vector3 dir = transform.position;
					float dmg = 60 * Mathf.Clamp(level * level / 200, 1, 20000);
					float slow = 0.2f;
					float boost = 1.4f;
					float duration = 20;
					float radius = 7;


					float Healing = dmg / 5;
					dmg *= 2;
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(3);
							w.Write(3);
							w.Write(dir.x);
							w.Write(dir.y);
							w.Write(dir.z);
							w.Write(true);
							w.Write(dmg);
							w.Write(Healing);
							w.Write(slow);
							w.Write(boost);
							w.Write(duration);
							w.Write(radius);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
						answerStream.Close();
					}

					sunflareCD = 120f;
					return;
				}

				if (abilities.Contains(Abilities.Warp))
				{
					if (blinkCD <= 0)
					{
						SetAbilityCooldown(0.5f, 1.5f);
						transform.root.position = closestPlayer.transform.position + transform.forward * -2.5f;
						blinkCD = Random.Range(2, 30);
						Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.Warp);
						return;
					}
				}
				if (abilities.Contains(Abilities.Laser) && laserTurretCD <= 0)
				{
					Vector3 dir = closestPlayer.transform.position;

					laserTurretCD = 110;
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(16);
							w.Write(transform.position.x);
							w.Write(transform.position.y);
							w.Write(transform.position.z);
							w.Write(dir.x);
							w.Write(dir.y);
							w.Write(dir.z);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
						answerStream.Close();
					}
					return;
				}
				if (abilities.Contains(Abilities.Chains) && chainsCD <= 0)
				{
					SetAbilityCooldown(0.0f, 0.5f);

					float duration = Mathf.Clamp(level / 20f, 3f, 10f);


					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(12);
							w.Write(transform.position.x);
							w.Write(transform.position.y);
							w.Write(transform.position.z);
							w.Write(duration);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
						answerStream.Close();
					}
					chainsCD = Random.Range(20, 35);
					return;
				}

				if (abilities.Contains(Abilities.Trapper) && trapSphereCD <= 0)
				{
					if (closestPlayerMagnitude < 80)
					{
						SetAbilityCooldown(0.5f, 1f);

						float radius = 8f;
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(15);
								w.Write(transform.position.x);
								w.Write(transform.position.y);
								w.Write(transform.position.z);
								w.Write(20f);
								w.Write(radius);
								w.Close();
							}
							Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
							answerStream.Close();
						}
						trapSphereCD = 40;
						return;
					}
				}
				if (abilities.Contains(Abilities.ArcaneCataclysm) && arcaneCataclysmCD <= 0)
				{
					if (closestPlayerMagnitude < agroRange / 2)
					{
						SetAbilityCooldown(0.0f, 1f);

						float dmg = 60 * Mathf.Pow(level, 2f);

						float radius = 10 + Mathf.Clamp(level / 20, 1, 10);

						radius -= 1;
						if (BoltNetwork.isRunning)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(3);
									w.Write(11);
									w.Write(transform.position.x);
									w.Write(transform.position.y);
									w.Write(transform.position.z);
									w.Write(radius);
									w.Write(dmg);
									w.Write(15f);
									w.Write(true);
									w.Write(true);
									w.Close();
								}
								Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
								answerStream.Close();
							}
						}
						Effects.Cataclysm.Create(transform.position, radius, dmg, 15, Effects.Cataclysm.TornadoType.Arcane, true);
						arcaneCataclysmCD = 140;
						return;
					}
				}
				if (abilities.Contains(Abilities.FireCataclysm) && fireCataclysmCD <= 0)
				{
					if (closestPlayerMagnitude < agroRange / 2)
					{
						SetAbilityCooldown(0.0f, 1f);

						float dmg = 60 * Mathf.Pow(level, 2f);

						float radius = 10 + Mathf.Clamp(level / 20, 1, 10);

						radius -= 1;
						dmg /= 1.5f;
						if (BoltNetwork.isRunning)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(3);
									w.Write(11);
									w.Write(transform.position.x);
									w.Write(transform.position.y);
									w.Write(transform.position.z);
									w.Write(radius);
									w.Write(dmg);
									w.Write(18f);
									w.Write(false);
									w.Write(true);
									w.Close();
								}
								Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
								answerStream.Close();
							}
						}
						Effects.Cataclysm.Create(transform.position, radius, dmg, 15, Effects.Cataclysm.TornadoType.Fire, true);
						fireCataclysmCD = 170;
						return;
					}
				}

				if (abilities.Contains(Abilities.Blizzard))
				{

					if (freezeauraCD <= 0)
					{
						if (BoltNetwork.isRunning)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(8);
									w.Write(1);
									w.Write(entity.networkId.PackedValue);
									w.Close();
								}
								Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
								answerStream.Close();
							}
						}
						else
						{
							SnowAura sa = new GameObject("Snow").AddComponent<SnowAura>();
							sa.followTarget = transform.root;
						}
						freezeauraCD = Random.Range(60, 100);
						return;
					}
				}
				if (abilities.Contains(Abilities.BlackHole))
				{

					if (blackholeCD <= 0)
					{
						SetAbilityCooldown(1.0f, 3f);

						float damage = Mathf.Pow(level, 1.7f) * 10;
						float duration = 5.5f;
						float radius = 21 + 2 * Mathf.Sqrt(level);
						float pullforce = 30;
						if (BoltNetwork.isRunning)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(3);
									w.Write(1);
									w.Write(transform.position.x);
									w.Write(transform.position.y);
									w.Write(transform.position.z);
									w.Write(true);
									w.Write(damage);
									w.Write(duration);
									w.Write(radius);
									w.Write(pullforce);
									w.Write("");

									w.Close();
								}
								Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
								answerStream.Close();
							}
						}
						else
						{
							BlackHole.CreateBlackHole(transform.position, true, damage, duration, radius, pullforce);
						}
						blackholeCD = Random.Range(50, 70);
					}
				}
			}
		}

		private void SetAbilityCooldown(float min, float max)
		{
			abilityCooldown = Random.Range(min, max);
		}
		private void SendFireAura()
		{
			if (abilities.Contains(Abilities.Radiance))
			{
				float aurDmg = (5 * level + 1) * (Mathf.Max(2, level / 50) + 1.3f);
				FireAura.Cast(gameObject, aurDmg);
				if (BoltNetwork.isRunning)
				{
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(8);
							w.Write(2);
							w.Write(entity.networkId.PackedValue);
							w.Write(aurDmg);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
						answerStream.Close();
					}
				}
			}
		}




	}
}