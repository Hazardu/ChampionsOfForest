using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;
using System;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Player
{
	public static class SpellActions
	{
		public static float BlinkRange = 15;
		public static float BlinkDamage = 0;
		private static SpellAimLine blinkAim;
		public static void DoBlinkAim()
		{
			if (blinkAim == null)
			{
				blinkAim = new SpellAimLine();
			}
			Transform t = Camera.main.transform;
			var hits1 = Physics.RaycastAll(t.position, t.forward, BlinkRange + 1f);
			foreach (var hit in hits1)
			{
				if (!hit.transform.CompareTag("enemyCollide") && hit.transform.root != LocalPlayer.Transform.root)
				{
					blinkAim.UpdatePosition(t.position + Vector3.down * 2, hit.point - t.forward + Vector3.up * 0.25f);
					return;
				}
			}

			blinkAim.UpdatePosition(t.position + Vector3.down * 2, LocalPlayer.Transform.position + t.forward * BlinkRange);
		}
		public static void DoBlink()
		{
			blinkAim?.Disable();

			Transform t = Camera.main.transform;
			Vector3 blinkPoint = Vector3.zero;
			var hits1 = Physics.RaycastAll(t.position, t.forward, BlinkRange + 1f);
			foreach (var hit in hits1)
			{
				if (!hit.transform.CompareTag("enemyCollide") && hit.transform.root != LocalPlayer.Transform.root)
				{
					blinkPoint = hit.point - t.forward + Vector3.up * 0.25f;
					break;
				}
			}
			if (blinkPoint == Vector3.zero)
			{
				blinkPoint = LocalPlayer.Transform.position + t.forward * BlinkRange;
			}
			if (BlinkDamage > 0)
			{
				RaycastHit[] hits = Physics.BoxCastAll(t.position, Vector3.one * 1.2f, blinkPoint - t.position, t.rotation, Vector3.Distance(blinkPoint, t.position) + 1);
				foreach (RaycastHit hit in hits)
				{
					if (hit.transform.CompareTag("enemyCollide"))
					{
						ModAPI.Console.Write("Hit enemy on layer " + hit.transform.gameObject.layer);
						float dmg = BlinkDamage + ModdedPlayer.instance.SpellDamageBonus;
						dmg *= ModdedPlayer.instance.SpellAMP * 3;
						DamageMath.DamageClamp(dmg, out int dmgInt, out int repetitions);
						if (GameSetup.IsMpClient)
						{
							BoltEntity enemyEntity = hit.transform.GetComponentInParent<BoltEntity>();
							if (enemyEntity == null)
								enemyEntity = hit.transform.gameObject.GetComponent<BoltEntity>();

							if (enemyEntity != null)
							{
								PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(enemyEntity);
								playerHitEnemy.hitFallDown = true;
								playerHitEnemy.explosion = true;
								playerHitEnemy.Hit = dmgInt;
								for (int i = 0; i < repetitions; i++)
									playerHitEnemy.Send();
							}
						}
						else
						{
							var v = hit.transform.GetComponentInParent<EnemyProgression>();
							if (v == null)
								v = hit.transform.GetComponent<EnemyProgression>();
							if (v != null)
							{
								for (int i = 0; i < repetitions; i++)
									v.HitMagic(dmgInt);
							}
							else
							{
								hit.transform.SendMessageUpwards("Hit", dmgInt, SendMessageOptions.DontRequireReceiver);
							}
						}
					}
				}
			}

			BlinkTowards(blinkPoint);


		}
		private static void BlinkTowards(Vector3 point)
		{
			Vector3 vel = LocalPlayer.Rigidbody.velocity;
			LocalPlayer.Transform.position = point + Vector3.up;
			vel.y /= 6;
			LocalPlayer.Rigidbody.velocity = vel * 1.5f;
			Effects.Sound_Effects.GlobalSFX.Play(5);
			ModAPI.Console.Write("Teleporting to: " + point);
		}





		public static bool HealingDomeGivesImmunity = false;
		public static bool HealingDomeRegEnergy = false;
		public static float HealingDomeDuration = 10;
		public static void CreateHealingDome()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			float radius = 10f;
			float healing = (ModdedPlayer.instance.LifeRegen * 3 + 13.5f + ModdedPlayer.instance.SpellDamageBonus / 30) * ModdedPlayer.instance.SpellAMP * ModdedPlayer.instance.HealingMultipier;


			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(2);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(radius);
					w.Write(healing);
					w.Write(HealingDomeGivesImmunity);
					w.Write(HealingDomeRegEnergy);
					w.Write(HealingDomeDuration);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		public static void BUFF_MultMS(float f)
		{
			ModdedPlayer.instance.MoveSpeedMult *= f;
		}
		public static void BUFF_DivideMS(float f)
		{
			ModdedPlayer.instance.MoveSpeedMult /= f;
		}

		public static void BUFF_MultAS(float f)
		{
			ModdedPlayer.instance.AttackSpeedMult *= f;
		}
		public static void BUFF_DivideAS(float f)
		{
			ModdedPlayer.instance.AttackSpeedMult /= f;
		}
		#region FLARE

		public static float FlareDamage = 40;
		public static float FlareSlow = 0.4f;
		public static float FlareBoost = 1.35f;
		public static float FlareHeal = 11;
		public static float FlareRadius = 5.5f;
		public static float FlareDuration = 20;
		public static void CastFlare()
		{
			Vector3 dir = LocalPlayer.Transform.position;
			float dmg = FlareDamage + ModdedPlayer.instance.SpellDamageBonus;
			dmg *= ModdedPlayer.instance.SpellAMP * 1.2f;
			float slow = FlareSlow;
			float boost = FlareBoost;
			float duration = FlareDuration;
			float radius = FlareRadius;
			float Healing = FlareHeal + ModdedPlayer.instance.SpellDamageBonus / 20 + (ModdedPlayer.instance.LifeRegen) * ModdedPlayer.instance.HealthRegenPercent;
			Healing *= ModdedPlayer.instance.SpellAMP;
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(3);
					w.Write(dir.x);
					w.Write(dir.y);
					w.Write(dir.z);
					w.Write(false);
					w.Write(dmg);
					w.Write(Healing);
					w.Write(slow);
					w.Write(boost);
					w.Write(duration);
					w.Write(radius);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}
		#endregion
		#region BLACK HOLE
		public static float BLACKHOLE_damage = 40;
		public static float BLACKHOLE_duration = 9;
		public static float BLACKHOLE_radius = 15;
		public static float BLACKHOLE_pullforce = 25;
		public static void CreatePlayerBlackHole()
		{
			float damage = (BLACKHOLE_damage + ModdedPlayer.instance.SpellDamageBonus / 3) * ModdedPlayer.instance.SpellAMP;
			//RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position,Camera.main.transform.forward, 160f);
			//for (int i = 0; i < hits.Length; i++)
			//{
			//    if (hits[i].transform.root != LocalPlayer.Transform.root)
			//{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(1);
					w.Write(LocalPlayer.Transform.root.position.x);
					w.Write(LocalPlayer.Transform.root.position.y);
					w.Write(LocalPlayer.Transform.root.position.z);
					w.Write(false);
					w.Write(damage);
					w.Write(BLACKHOLE_duration);
					w.Write(BLACKHOLE_radius);
					w.Write(BLACKHOLE_pullforce);
					w.Write(ModdedPlayer.instance.SparkOfLightAfterDark ? ModReferences.ThisPlayerID : "");
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
			//        return;
			//    }
			//}
			//Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 10;
			//using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			//{
			//    using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
			//    {
			//        w.Write(3);
			//        w.Write(1);
			//        w.Write(pos.x);
			//        w.Write(pos.y);
			//        w.Write(pos.z);
			//        w.Write(false);
			//        w.Write(damage);
			//        w.Write(BLACKHOLE_duration);
			//        w.Write(BLACKHOLE_radius);
			//        w.Write(BLACKHOLE_pullforce);
			//    w.Close();
			//    }
			//    ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
			//    answerStream.Close();
			//}
		}

		public static void CallbackSparkOfLightAfterDarkness(Vector3 pos)
		{
			CastBallLightning(pos, Vector3.down);
		}

		#endregion

		#region SustainShield
		//TODO make this toggleable
		public static float ShieldPerSecond = 2;
		public static float MaxShield = 40;
		public static float ShieldCastTime;
		public static float ShieldPersistanceLifetime = 20;
		public static void CastSustainShieldActive()
		{
			float max = MaxShield + ModdedPlayer.instance.SpellDamageBonus;
			max *= ModdedPlayer.instance.SpellAMP;
			float gain = ShieldPerSecond + ModdedPlayer.instance.SpellDamageBonus / 20;
			gain *= ModdedPlayer.instance.SpellAMP;
			ModdedPlayer.instance.damageAbsorbAmounts[1] = Mathf.Clamp(ModdedPlayer.instance.damageAbsorbAmounts[1] + Time.deltaTime * gain, 0, max);
			ShieldCastTime = Time.time;
		}
		public static void CastSustainShielPassive(bool on)
		{
			if (!on)
			{
				return;
			}

			if (ModdedPlayer.instance.damageAbsorbAmounts[1] > 0)
			{
				if (ShieldCastTime + ShieldPersistanceLifetime < Time.time)
				{
					float loss = Time.deltaTime * (ShieldPerSecond + ModdedPlayer.instance.SpellDamageBonus / 5) * 5 * ModdedPlayer.instance.SpellDamageBonus;
					ModdedPlayer.instance.damageAbsorbAmounts[1] = Mathf.Max(0, ModdedPlayer.instance.damageAbsorbAmounts[1] - loss);
				}
			}
		}
		#endregion


		#region WarCry
		public static float WarCryRadius = 50, WarCryAtkSpeed = 1.2f, WarCryDamage = 1.2f;
		public static bool WarCryGiveDamage = false;
		public static bool WarCryGiveArmor = false;
		public static int WarCryArmor => ModdedPlayer.instance.Armor / 10;
		public static void CastWarCry()
		{
			float speed = WarCryAtkSpeed + (ModdedPlayer.instance.SpellAMP - 1) / 400;
			speed = Mathf.Min(speed, 1.75f);
			float dmg = WarCryDamage + (ModdedPlayer.instance.SpellAMP - 1) / 400;
			dmg = Mathf.Min(dmg, 1.75f);

			WarCry.GiveEffect(speed, dmg, WarCryGiveDamage, WarCryGiveArmor, WarCryArmor);
			WarCry.SpawnEffect(LocalPlayer.Transform.position, WarCryRadius);
			if (BoltNetwork.isRunning)
			{
				Vector3 pos = LocalPlayer.Transform.position;
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(5);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(WarCryRadius);
						w.Write(speed);
						w.Write(dmg);
						w.Write(WarCryGiveDamage);
						w.Write(WarCryGiveArmor);
						w.Write(WarCryArmor);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
					answerStream.Close();
				}
			}
		}

		#endregion
		public static float PortalDuration = 30;
		public static void CastPortal()
		{
			Vector3 pos = LocalPlayer.Transform.position + LocalPlayer.Transform.forward * 6;
			int id = Portal.GetPortalID();
			try
			{
				Portal.CreatePortal(pos, PortalDuration, id, LocalPlayer.IsInCaves, LocalPlayer.IsInEndgame);

			}
			catch (Exception e)
			{
				ModAPI.Log.Write(e.ToString());

			}

			if (BoltNetwork.isRunning)
			{
				Portal.SyncTransform(pos, PortalDuration, id, LocalPlayer.IsInCaves, LocalPlayer.IsInEndgame);
			}
		}


		public static bool MagicArrowDmgDebuff = false;
		public static bool MagicArrowCrit = false;
		public static bool MagicArrowDoubleSlow = false;
		public static float MagicArrowDuration = 10f;
		public static void CastMagicArrow()
		{
			float damage = 55 + ModdedPlayer.instance.SpellDamageBonus * 3.2f + ModdedPlayer.instance.RangedDamageBonus / 2;
			damage = damage * ModdedPlayer.instance.SpellAMP * 2;
			if (MagicArrowCrit)
				BashBleedDmg *= ModdedPlayer.instance.CritDamageBuff * 4;
			Vector3 pos = Camera.main.transform.position;
			Vector3 dir = Camera.main.transform.forward;
			if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
			{
				MagicArrow.Create(pos, dir, damage, ModReferences.ThisPlayerID, MagicArrowDuration, MagicArrowDoubleSlow, MagicArrowDmgDebuff);
				if (BoltNetwork.isRunning)
				{

					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(3);
							w.Write(7);
							w.Write(pos.x);
							w.Write(pos.y);
							w.Write(pos.z);
							w.Write(dir.x);
							w.Write(dir.y);
							w.Write(dir.z);
							w.Write(damage);
							w.Write(ModReferences.ThisPlayerID);
							w.Write(MagicArrowDuration);
							w.Write(MagicArrowDoubleSlow);
							w.Write(MagicArrowDmgDebuff);
							w.Close();
						}
						ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
						answerStream.Close();
					}
				}
			}
			else if (GameSetup.IsMpClient)
			{
				MagicArrow.CreateEffect(pos, dir, MagicArrowDmgDebuff, MagicArrowDuration);
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(7);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(dir.x);
						w.Write(dir.y);
						w.Write(dir.z);
						w.Write(damage);
						w.Write(ModReferences.ThisPlayerID);
						w.Write(MagicArrowDuration);
						w.Write(MagicArrowDoubleSlow);
						w.Write(MagicArrowDmgDebuff);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
					answerStream.Close();
				}
			}

		}


		public static void ToggleMultishot()
		{
			Multishot.IsOn = !Multishot.IsOn;
			Multishot.localPlayerInstance.SetActive(Multishot.IsOn);
		}


		public static float PurgeRadius = 30;
		public static bool PurgeHeal = false, PurgeDamageBonus = false;
		public static void CastPurge()
		{
			Vector3 pos = LocalPlayer.Transform.position;

			Purge.Cast(pos, PurgeRadius, PurgeHeal, PurgeDamageBonus);

			if (BoltNetwork.isRunning)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(8);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(PurgeRadius);
						w.Write(PurgeHeal);
						w.Write(PurgeDamageBonus);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
					answerStream.Close();
				}
			}
		}

		public static float SnapFreezeDist = 20;
		public static float SnapFloatAmount = 0.2f;
		public static float SnapFreezeDuration = 7f;
		public static void CastSnapFreeze()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			float dmg = 23 + ModdedPlayer.instance.SpellDamageBonus * 1.5f;
			dmg *= ModdedPlayer.instance.SpellAMP * 2;
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(9);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(SnapFreezeDist);
					w.Write(SnapFloatAmount);
					w.Write(SnapFreezeDuration);
					w.Write(dmg);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		public static float BL_Damage = 620;
		public static bool BL_Crit = false;
		public static void CastBallLightning(Vector3 pos, Vector3 speed)
		{
			float dmg = BL_Damage + (9 * ModdedPlayer.instance.SpellDamageBonus);
			dmg *= ModdedPlayer.instance.SpellAMP * 4;
			if (BL_Crit)
				dmg *= ModdedPlayer.instance.CritDamageBuff * 4;


			speed.y = 0;
			speed.Normalize();
			speed *= 6;

			if (BoltNetwork.isClient)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(12);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(speed.x);
						w.Write(speed.y);
						w.Write(speed.z);
						w.Write(dmg);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
			}
			else
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(10);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(speed.x);
						w.Write(speed.y);
						w.Write(speed.z);
						w.Write(dmg);
						w.Write((uint)(BallLightning.lastID + 1));
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
					answerStream.Close();
				}
				BallLightning.lastID++;
			}
		}

		public static void CastBallLightning()
		{
			Vector3 pos = LocalPlayer.Transform.position + LocalPlayer.Transform.forward;
			Vector3 speed = Camera.main.transform.forward;

			CastBallLightning(pos, speed);
		}


		#region Bash
		public static float BashExtraDamage = 1.30f;
		public static float BashDamageBuff = 0f;
		public static float BashSlowAmount = 0.4f;
		public static float BashLifesteal = 0.0f;
		public static bool BashEnabled = false;
		public static float BashBleedChance = 0;
		public static float BashBleedDmg = 0.3f;
		public static float BashDuration = 2;

		public static void BashPassiveEnabled(bool on)
		{
			BashEnabled = on;
			//SpellDataBase.spellDictionary[17].icon = on ? Res.ResourceLoader.GetTexture(132) : Res.ResourceLoader.GetTexture(131);
		}
		public static void Bash(EnemyProgression ep, float dmg)
		{
			if (BashEnabled)
			{
				int id = 43;
				ep.Slow(id, BashSlowAmount, BashDuration);
				ep.DmgTakenDebuff(id, BashExtraDamage, BashDuration);
				if (BashBleedChance > 0 && UnityEngine.Random.value < BashBleedChance)
					ep.DoDoT((int)(dmg * BashBleedDmg), BashDuration);
				if (BashLifesteal > 0)
				{
					LocalPlayer.Stats.HealthTarget += dmg * BashLifesteal;
					LocalPlayer.Stats.Energy += dmg * BashLifesteal;
				}
				if (BashDamageBuff > 0)
				{
					BuffDB.AddBuff(24, 89, BashDamageBuff, 2);
				}
			}

		}
		public static void Bash(ulong enemy, float dmg)
		{
			if (BashEnabled)
			{
				int id = 44 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);

				if (BashLifesteal > 0)
				{
					LocalPlayer.Stats.HealthTarget += dmg * BashLifesteal;
					LocalPlayer.Stats.Energy += dmg * BashLifesteal;

				}

				if (BashDamageBuff > 0)
				{
					BuffDB.AddBuff(24, 89, BashDamageBuff, 2);
				}
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(33);
						w.Write(enemy);
						w.Write(BashDuration);
						w.Write(id);
						w.Write(BashSlowAmount);
						w.Write(BashExtraDamage);
						w.Write(((int)(dmg * BashBleedDmg)));
						w.Write(BashBleedChance);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
			}

		}
		#endregion

		#region Frenzy
		public static int FrenzyMaxStacks = 5, FrenzyStacks = 0;
		public static float FrenzyAtkSpeed = 0.02f, FrenzyDmg = 0.075f;
		public static bool Frenzy;
		public static bool FrenzyMS = false;
		public static bool FurySwipes = false;
		public static void OnFrenzyAttack()
		{
			if (Frenzy)
			{
				if (BuffDB.activeBuffs.ContainsKey(60))
				{
					int frenzyStacks = FrenzyStacks;
					BuffDB.activeBuffs[60].OnEnd(FrenzyStacks);
					FrenzyStacks = Mathf.Min(FrenzyMaxStacks, frenzyStacks + 1);
					BuffDB.activeBuffs[60].amount = FrenzyStacks;
					BuffDB.activeBuffs[60].duration = 4;
					BuffDB.activeBuffs[60].OnStart(FrenzyStacks);
				}
				else
				{
					FrenzyStacks++;
					FrenzyStacks = Mathf.Min(FrenzyMaxStacks, FrenzyStacks);
					BuffDB.AddBuff(19, 60, FrenzyStacks, 4);
				}
			}
		}
		#endregion

		#region Focus
		public static float FocusBonusDmg, FocusOnHS = 1, FocusOnBS = 0.2f, FocusOnAtkSpeed = 1.3f, FocusOnAtkSpeedDuration = 4, FocusSlowAmount = 0.8f, FocusSlowDuration = 4;
		public static bool Focus;

		public static float FocusOnBodyShot()
		{
			if (!Focus)
				return 1;
			if (FocusBonusDmg == 0)
			{
				FocusBonusDmg = FocusOnBS;
				BuffDB.AddBuff(14, 61, FocusOnAtkSpeed, FocusOnAtkSpeedDuration);
				return 1;
			}
			else
			{
				var result = 1f + FocusBonusDmg;
				FocusBonusDmg = 0;
				return result;
			}
		}
		public static float FocusOnHeadShot()
		{
			if (!Focus)
				return 1;
			if (FocusBonusDmg == 0)
			{
				FocusBonusDmg = FocusOnHS;
				return 1;
			}
			else
			{
				var result = 1f + FocusBonusDmg;
				FocusBonusDmg = 0;
				return result;
			}
		}
		#endregion

		#region SeekingArrow
		public static Transform SeekingArrow_Target;
		public static bool SeekingArrow;
		public static bool SeekingArrow_ChangeTargetOnHit;
		public static float SeekingArrow_TimeStamp, SeekingArrow_HeadDamage = 2, SeekingArrow_SlowDuration = 4, SeekingArrow_SlowAmount = 0.8f, SeekingArrow_DamagePerDistance = 0.01f, SeekingArrowDuration = 30;
		public static void SeekingArrow_Initialize()
		{
			SeekingArrow_Target = new GameObject().transform;
			SeekingArrow_Target.gameObject.AddComponent<SeekingArrow>();
			//some more visuals
		}
		public static void SeekingArrow_Active()
		{
			if (SeekingArrow_Target == null)
				SeekingArrow_Initialize();
			SeekingArrow_Target.transform.parent = null;
			SeekingArrow_Target.gameObject.SetActive(false);
			SeekingArrow = false;
			SeekingArrow_TimeStamp = 0;
			SeekingArrow_ChangeTargetOnHit = true;

		}
		public static void SeekingArrow_End()
		{
			SeekingArrow_Target.gameObject.SetActive(false);

		}
		#endregion

		#region Parry
		public static bool Parry;
		public static float ParryDamage = 40, ParryRadius = 3.5f, ParryBuffDuration = 10, ParryHeal = 5, ParryEnergy = 10;
		public static bool ChanceToParryOnHit = false;
		public static bool ParryIgnites = false;
		public static float ParryDmgBonus = 0;
		public static float ParryBuffDamage = 0;
		private static float LastBlockTimestamp = 0;    //used for blocking any instance of damage
		private const float Block_parryTime = 0.6f; //600ms to get hit since starting blocking will cause a parry
		public static void OnBlockSetTimer()
		{
			LastBlockTimestamp = Time.time + Block_parryTime;
		}

		public static bool ParryAnythingIsTimed =>
			 ModdedPlayer.instance.ParryAnything && LastBlockTimestamp < Time.time;


		public static void DoParry(Vector3 parryPos)
		{
			if (Parry)
			{
				BuffDB.AddBuff(6, 61, 1, ParryBuffDuration);
				float dmg = ParryDamage + ModdedPlayer.instance.SpellDamageBonus + ModdedPlayer.instance.MeleeDamageBonus;
				dmg *= ModdedPlayer.instance.SpellDamageAmplifier * 1.2f;

				float heal = ParryHeal + ModdedPlayer.instance.SpellDamageBonus / 6 + ModdedPlayer.instance.LifeRegen + ModdedPlayer.instance.LifeOnHit * 2;
				heal *= ModdedPlayer.instance.HealingMultipier * (1 + ModdedPlayer.instance.HealthRegenPercent);
				LocalPlayer.Stats.HealthTarget += heal;
				ParrySound.Play(parryPos);
				float energy = ParryEnergy * ModdedPlayer.instance.StaminaAndEnergyRegenAmp + ModdedPlayer.instance.EnergyOnHit * 2 + ModdedPlayer.instance.MaxEnergy / 12.5f;
				LocalPlayer.Stats.Energy += energy;
				LocalPlayer.Stats.Stamina += energy;
				if (ParryDmgBonus > 0)
				{
					float f = dmg * ParryDmgBonus;
					ModdedPlayer.instance.ParryCounterStrikeDamage += f;
					BuffDB.AddBuff(23, 88, f, 20);
				}

				if (GameSetup.IsMpClient)
				{
					if (BoltNetwork.isRunning)
					{
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(3);
								w.Write(13);
								w.Write(parryPos.x);
								w.Write(parryPos.y);
								w.Write(parryPos.z);
								w.Write(ParryRadius);
								w.Write(ParryIgnites);
								w.Write(dmg);
								w.Close();
							}
							ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
							answerStream.Close();
						}
					}
				}
				else
				{
					DamageMath.DamageClamp(dmg, out int d, out int r);
					var hits = Physics.SphereCastAll(parryPos, ParryRadius, Vector3.one);
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].transform.CompareTag("enemyCollide"))
						{
							for (int a = 0; a < r; a++)
							{
								hits[i].transform.SendMessageUpwards("Hit", d, SendMessageOptions.DontRequireReceiver);
								if (ParryIgnites)
									hits[i].transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
							}
						}
					}
				}
			}
		}
		#endregion

		#region Cataclysm       
		public static float CataclysmDamage = 24, CataclysmDuration = 12, CataclysmRadius = 5;
		public static bool CataclysmArcane = false;
		public static void CastCataclysm()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			BuffDB.AddBuff(1, 66, 0.1f, 2.5f);
			float dmg = CataclysmDamage + ModdedPlayer.instance.SpellDamageBonus * 0.9f;
			dmg *= ModdedPlayer.instance.SpellAMP;
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(11);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(CataclysmRadius);
					w.Write(dmg);
					w.Write(CataclysmDuration);
					w.Write(CataclysmArcane);
					w.Write(false);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}

		}
		#endregion

		#region Blood Infused Arrow
		public static float BIA_bonusDamage;
		public static float BIA_SpellDmMult = 1.25f;
		public static float BIA_HealthDmMult = 3f;
		public static float BIA_HealthTakenMult = 0.65f;
		public static bool BIA_TripleDmg = false, BIA_Weaken = false;
		public static void CastBloodInfArr()
		{
			float takenHP = LocalPlayer.Stats.Health * BIA_HealthTakenMult;
			if (takenHP > LocalPlayer.Stats.Health - 5)
				takenHP = LocalPlayer.Stats.Health - 5;
			LocalPlayer.Stats.Health -= takenHP;
			LocalPlayer.Stats.HealthTarget -= takenHP;
			BIA_bonusDamage = takenHP * BIA_HealthDmMult;
			BIA_bonusDamage += BIA_SpellDmMult * ModdedPlayer.instance.SpellDamageBonus;
			BIA_bonusDamage *= ModdedPlayer.instance.SpellAMP;
			if (BIA_TripleDmg)
			{
				BIA_bonusDamage *= 3;
				BuffDB.AddBuff(18, 95, ModdedPlayer.instance.MaxEnergy / 16, 8);

			}
			if (ModdedPlayer.instance.IsHazardCrown)
				ModdedPlayer.instance.HazardCrownBonus = 5;
			Effects.Sound_Effects.GlobalSFX.Play(4);

		}
		#endregion

		#region

		public static float fartRadius = 6;
		public static float fartKnockback = 4, fartSlow = 0.6f, fartDebuffDuration = 6f;
		public static float fartDoT = 4;
		public static void FartEffect(float radius,float knockback,float damage,float slow,float duration)
		{
		
		
		}
		public static void RipAFatOne()
		{
			var back = -LocalPlayer.Transform.forward;
			var origin = LocalPlayer.Transform.position;
			if (!LocalPlayer.FpCharacter.Grounded)
			{
				var vel = LocalPlayer.Rigidbody.velocity;
				if (vel.y < 0)
				{
					vel.y = 2;
				}
				else
				{
					vel.y += 2;
				}
				LocalPlayer.Rigidbody.velocity = vel;
				back.y -= 1.5f;
				back.Normalize();
			}
			LocalPlayer.Rigidbody.AddForce(-back * 3, ForceMode.VelocityChange);
			
			float dmg = (ModdedPlayer.instance.SpellDamageBonus + fartDoT) * ModdedPlayer.instance.SpellAMP;
			if (GameSetup.IsMultiplayer)
			{
						System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
						System.IO.BinaryWriter writer = new System.IO.BinaryWriter(memoryStream);
						writer.Write(3);
						writer.Write(14);
						writer.Write(LocalPlayer.FpCharacter.Grounded);
						writer.Write(origin.x);
						writer.Write(origin.y);
						writer.Write(origin.z);	
						writer.Write(back.x);
						writer.Write(back.y);
						writer.Write(back.z);
						writer.Write(dmg);
						writer.Write(fartKnockback);
						writer.Write(fartSlow);
						writer.Write(fartDebuffDuration);
				writer.Close();
				NetworkManager.SendLine(memoryStream.ToArray(),NetworkManager.Target.Others);
				memoryStream.Close();
			}
		}
		#endregion



		#region CorpseExplosion
		//public static float CorpseExpl_HealthTakenMult = 0.10f;
		//public static float CorpseExpl_Radius = 4;
		//public static void CastCorpseExplosion()
		//{
		//   var hits = Physics.SphereCastAll(Camera.main.transform.position, CorpseExpl_Radius, Vector3.one, CorpseExpl_Radius, -10, QueryTriggerInteraction.Collide);
		//    List<Transform> hitTransforms = new List<Transform>();
		//    foreach (var hit in hits)
		//    {
		//        ModAPI.Console.Write("hit" + hit.transform.name + "\t" + hit.transform.tag);
		//        if (hit.transform.CompareTag("enemyBodyPart"))
		//        {
		//            if (!hitTransforms.Contains(hit.transform.root))
		//            {
		//                hitTransforms.Add(hit.transform.root);
		//                if ((hitTransforms.Count+1) * BIA_HealthTakenMult>=1) break;
		//            }
		//        }
		//    }
		//    if (hitTransforms.Count == 0) return;
		//    int takenHP = (int)(LocalPlayer.Stats.Health * CorpseExpl_HealthTakenMult * hitTransforms.Count);
		//    LocalPlayer.Stats.Hit(takenHP, true, PlayerStats.DamageType.Drowning);

		//}
		#endregion

		#region Devour

		public static void CastDevour()
		{

		}
		#endregion

	}

}
