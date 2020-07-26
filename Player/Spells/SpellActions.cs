using System;
using System.Linq;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public static class SpellActions
	{
	
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
						dmg *= ModdedPlayer.instance.TotalSpellAmplification * 3;
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
			BuffDB.AddBuff(4, 97, 1, 0.1f);
		}

	
		private static SpellAimSphere healingDomeaimSphere;

		public static void HealingDomeAim()
		{
			if (healingDomeaimSphere == null)
			{
				healingDomeaimSphere = new Effects.SpellAimSphere(new Color(0f, 1f, 0f, 0.5f), 10f);
			}
			healingDomeaimSphere.UpdatePosition(LocalPlayer.Transform.position);
		}

		public static void HealingDomeAimEnd()
		{
			healingDomeaimSphere.Disable();
		}

		public static void CreateHealingDome()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			float radius = 10f;
			float healing = (ModdedPlayer.Stats.healthRecoveryPerSecond * 3 + 13.5f + ModdedPlayer.instance.SpellDamageBonus / 30) * ModdedPlayer.instance.TotalSpellAmplification * ModdedPlayer.Stats.allRecoveryMult;

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
			ModdedPlayer.Stats.attackSpeed *= f;
		}

		public static void BUFF_DivideAS(float f)
		{
			ModdedPlayer.Stats.attackSpeed /= f;
		}

		#region FLARE

	

		public static void CastFlare()
		{
			Vector3 dir = LocalPlayer.Transform.position;
			float dmg = FlareDamage + ModdedPlayer.instance.SpellDamageBonus;
			dmg *= ModdedPlayer.instance.TotalSpellAmplification * 1.2f;
			float slow = FlareSlow;
			float boost = FlareBoost;
			float duration = FlareDuration;
			float radius = FlareRadius;
			float Healing = FlareHeal + ModdedPlayer.instance.SpellDamageBonus / 20 + (ModdedPlayer.Stats.healthRecoveryPerSecond) * ModdedPlayer.Stats.healthPerSecRate;
			Healing *= ModdedPlayer.instance.TotalSpellAmplification;
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

		#endregion FLARE
		#region BLACK HOLE
		
		private static SpellAimSphere blackholeAim;

		public static void BlackHoleAimEnd()
		{
			blackholeAim.Disable();
		}

		public static void BlackHoleAim()
		{
			if (blackholeAim == null)
			{
				blackholeAim = new SpellAimSphere(new Color(0f, .6f, 0.95f, 0.5f), BLACKHOLE_radius);
			}
			Transform t = Camera.main.transform;

			Vector3 point = Vector3.zero;
			var hits1 = Physics.RaycastAll(t.position, t.forward, 35f);
			foreach (var hit in hits1)
			{
				if (hit.transform.root != LocalPlayer.Transform.root)
				{
					point = hit.point + Vector3.up * 2f;
					break;
				}
			}
			if (point == Vector3.zero)
			{
				point = LocalPlayer.Transform.position + t.forward * 30;
			}
			blackholeAim.SetRadius(BLACKHOLE_radius);
			blackholeAim.UpdatePosition(point);
		}

		public static void CreatePlayerBlackHole()
		{
			float damage = (BLACKHOLE_damage + ModdedPlayer.instance.SpellDamageBonus / 3) * ModdedPlayer.instance.TotalSpellAmplification;
			Transform t = Camera.main.transform;

			Vector3 point = Vector3.zero;
			var hits1 = Physics.RaycastAll(t.position, t.forward, 35f);
			foreach (var hit in hits1)
			{
				if (hit.transform.root != LocalPlayer.Transform.root)
				{
					point = hit.point;
					break;
				}
			}
			if (point == Vector3.zero)
			{
				point = LocalPlayer.Transform.position + t.forward * 30;
			}
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
					w.Write(point.x);
					w.Write(point.y - 1);
					w.Write(point.z);
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

		#endregion BLACK HOLE

		#region SustainShield

		//TODO make this toggleable

		public static float ShieldCastTime;

		public static void CastSustainShieldActive()
		{
			float max = MaxShield + ModdedPlayer.instance.SpellDamageBonus;
			max *= ModdedPlayer.instance.TotalSpellAmplification;
			float gain = ShieldPerSecond + ModdedPlayer.instance.SpellDamageBonus / 20;
			gain *= ModdedPlayer.instance.TotalSpellAmplification;
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

		#endregion SustainShield

		#region WarCry
	
		public static int WarCryArmor => ModdedPlayer.Stats.armor.Value / 10;

		public static void CastWarCry()
		{
			float speed = WarCryAtkSpeed + (ModdedPlayer.instance.TotalSpellAmplification - 1) / 400;
			speed = Mathf.Min(speed, 1.75f);
			float dmg = WarCryDamage + (ModdedPlayer.instance.TotalSpellAmplification - 1) / 400;
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

		#endregion WarCry

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

	
		private static SpellAimSphere arrowAim;

		public static void MagicArrowAimEnd()
		{
			arrowAim.Disable();
		}

		public static void MagicArrowAim()
		{
			if (arrowAim == null)
			{
				arrowAim = new SpellAimSphere(new Color(0f, 1f, 0.55f, 0.5f), 1f);
			}
			Transform t = Camera.main.transform;
			if (Physics.Raycast(t.position + t.forward, t.forward, out RaycastHit hit, 250))
			{
				arrowAim.UpdatePosition(hit.point);
			}
			else
			{
				arrowAim.Disable();
			}
		}

		public static void CastMagicArrow()
		{
			float damage = 55 + ModdedPlayer.instance.SpellDamageBonus * 3.2f + ModdedPlayer.instance.RangedDamageBonus / 2;
			damage = damage * ModdedPlayer.instance.TotalSpellAmplification * 2;
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

	
		private static SpellAimSphere snapFreezeAim;

		public static void SnapFreezeAimEnd()
		{
			snapFreezeAim.Disable();
		}

		public static void SnapFreezeAim()
		{
			if (snapFreezeAim == null)
			{
				snapFreezeAim = new Effects.SpellAimSphere(new Color(1f, .55f, 0f, 0.5f), SnapFreezeDist);
			}
			snapFreezeAim.SetRadius(SnapFreezeDist);
			snapFreezeAim.UpdatePosition(LocalPlayer.Transform.position);
		}

		public static void CastSnapFreeze()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			float dmg = 23 + ModdedPlayer.instance.SpellDamageBonus * 1.5f;
			dmg *= ModdedPlayer.instance.TotalSpellAmplification * 2;
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

		

		public static void CastBallLightning(Vector3 pos, Vector3 speed)
		{
			float dmg = BL_Damage + (9 * ModdedPlayer.instance.SpellDamageBonus);
			dmg *= ModdedPlayer.instance.TotalSpellAmplification * 4;
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
			Vector3 speed = Camera.main.transform.forward * 2;

			CastBallLightning(pos, speed);
		}

		#region Bash
	

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
					BuffDB.AddBuff(24, 89, BashDamageBuff * 0.15f, 2);
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

		#endregion Bash

		#region Frenzy
		

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

		#endregion Frenzy

		#region Focus


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

		#endregion Focus

		#region SeekingArrow
		public static Transform SeekingArrow_Target;
		public static float SeekingArrow_TimeStamp;
		public static bool SeekingArrow_ChangeTargetOnHit;

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

		public static void SetSeekingArrowTarget(Transform bone)
		{
			SpellActions.SeekingArrow = true;
			SpellActions.SeekingArrow_Target.gameObject.SetActive(true);
			SpellActions.SeekingArrow_Target.transform.parent = bone;
			SpellActions.SeekingArrow_Target.transform.position = bone.position;
			SpellActions.SeekingArrow_TimeStamp = Time.time;
			SpellActions.SeekingArrow_ChangeTargetOnHit = false;
		}

		#endregion SeekingArrow

		#region Parry

		private static float LastBlockTimestamp = 0;    //used for blocking any instance of damage
		private const float Block_parryTime = 0.4f; //600ms to get hit since starting blocking will cause a parry
		public static float GetParryCounterStrikeDmg()
		{
			if (ModdedPlayer.Stats.perk_parryCounterStrikeDamage.Value > 0)
			{
				float f = Stats.perk_parryCounterStrikeDamage;
				if (BuffDB.activeBuffs.ContainsKey(88))
				{
					BuffDB.activeBuffs[88].ForceEndBuff(88);
				}
				Stats.perk_parryCounterStrikeDamage.Reset();
				return f;
			}
			return 0;
		}
		public static void OnBlockSetTimer()
		{
			LastBlockTimestamp = Time.time + Block_parryTime;
		}

		public static bool ParryAnythingIsTimed =>
			 ModdedPlayer.instance.ParryAnything && LastBlockTimestamp > Time.time;

		public static void DoParry(Vector3 parryPos)
		{
			if (Parry)
			{
				BuffDB.AddBuff(6, 61, 1, ParryBuffDuration);
				float dmg = ParryDamage + ModdedPlayer.instance.SpellDamageBonus + ModdedPlayer.instance.MeleeDamageBonus;
				dmg *= ModdedPlayer.instance.SpellDamageAmplifier * 1.2f;

				float heal = ParryHeal + ModdedPlayer.instance.SpellDamageBonus / 6 + ModdedPlayer.Stats.healthRecoveryPerSecond + ModdedPlayer.instance.LifeOnHit * 2;
				heal *= ModdedPlayer.Stats.allRecoveryMult * (1 + ModdedPlayer.Stats.healthPerSecRate);
				LocalPlayer.Stats.HealthTarget += heal;
				ParrySound.Play(parryPos);
				float energy = ParryEnergy * ModdedPlayer.instance.StaminaAndEnergyRegenAmp + ModdedPlayer.instance.EnergyOnHit * 2 + ModdedPlayer.Stats.TotalMaxEnergy / 12.5f;
				LocalPlayer.Stats.Energy += energy;
				LocalPlayer.Stats.Stamina += energy;
				if (ParryDmgBonus > 0)
				{
					float f = dmg * ParryDmgBonus;
					ModdedPlayer.instance.Stats.perk_parryCounterStrikeDamage += f;
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

		#endregion Parry

		#region Cataclysm
	
		private static SpellAimSphere cataclysmAim;

		public static void CataclysmAimEnd()
		{
			cataclysmAim.Disable();
		}

		public static void CataclysmAim()
		{
			if (cataclysmAim == null)
			{
				cataclysmAim = new Effects.SpellAimSphere(new Color(1f, 0.0f, 0f, 0.5f), CataclysmRadius);
			}
			cataclysmAim.SetRadius(CataclysmRadius);
			cataclysmAim.UpdatePosition(LocalPlayer.Transform.position);
		}

		public static void CastCataclysm()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			BuffDB.AddBuff(1, 66, 0.1f, 2.5f);
			float dmg = CataclysmDamage + ModdedPlayer.instance.SpellDamageBonus * 0.9f;
			dmg *= ModdedPlayer.instance.TotalSpellAmplification;
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

		#endregion Cataclysm

		#region Blood Infused Arrow


		public static void CastBloodInfArr()
		{
			float takenHP = LocalPlayer.Stats.Health * BIA_HealthTakenMult;
			if (takenHP > LocalPlayer.Stats.Health - 5)
				takenHP = LocalPlayer.Stats.Health - 5;
			LocalPlayer.Stats.Health -= takenHP;
			LocalPlayer.Stats.HealthTarget -= takenHP;
			BIA_bonusDamage = takenHP * BIA_HealthDmMult;
			BIA_bonusDamage += BIA_SpellDmMult * ModdedPlayer.instance.SpellDamageBonus;
			BIA_bonusDamage *= ModdedPlayer.instance.TotalSpellAmplification;
			if (BIA_TripleDmg)
			{
				BIA_bonusDamage *= 3;
				BuffDB.AddBuff(18, 95, ModdedPlayer.Stats.TotalMaxEnergy / 16, 8);
			}
			if (ModdedPlayer.instance.IsHazardCrown)
				ModdedPlayer.instance.HazardCrownBonus = 5;
			Effects.Sound_Effects.GlobalSFX.Play(4);
		}

		#endregion Blood Infused Arrow

		#region

		public static float fartRadius = 30;
		public static float fartKnockback = 2, fartSlow = 0.8f, fartDebuffDuration = 30f, fartBaseDmg = 20f;

		public static void FartEffect(float radius, float knockback, float damage, float slow, float duration)
		{
		}

		public static void RipAFatOne()
		{
			var back = -LocalPlayer.Transform.forward;
			var origin = LocalPlayer.Transform.position;
			LocalPlayer.Stats.Fullness -= 0.5f;
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
				LocalPlayer.Rigidbody.AddForce(-back * 5, ForceMode.VelocityChange);
				float dmg = (ModdedPlayer.instance.SpellDamageBonus + fartBaseDmg) * ModdedPlayer.instance.TotalSpellAmplification / 5;
				if (GameSetup.IsMultiplayer)
				{
					System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
					System.IO.BinaryWriter writer = new System.IO.BinaryWriter(memoryStream);
					writer.Write(3);
					writer.Write(14);
					writer.Write(false);
					writer.Write(origin.x);
					writer.Write(origin.y);
					writer.Write(origin.z);
					writer.Write(back.x);
					writer.Write(back.y);
					writer.Write(back.z);
					writer.Write(fartRadius / 2);
					writer.Write(dmg);
					writer.Write(fartKnockback / 2);
					writer.Write(fartSlow);
					writer.Write(fartDebuffDuration / 2);
					writer.Close();
					NetworkManager.SendLine(memoryStream.ToArray(), NetworkManager.Target.Others);
					memoryStream.Close();
				}
				if (!GameSetup.IsMpClient)
				{
					Effects.TheFartCreator.DealDamageAsHost(origin, back, fartRadius / 2, dmg, fartKnockback / 2, fartSlow, fartDebuffDuration / 2);
				}
				SpellCaster.instance.infos.First(x => x.spell.ID == 24).Cooldown /= 3;
			}
			else
			{
				float dmg = (ModdedPlayer.instance.SpellDamageBonus + fartBaseDmg) * ModdedPlayer.instance.TotalSpellAmplification;
				BuffDB.AddBuff(1, 96, 0.4f, 7.175f);
				TheFartCreator.FartWarmup(fartRadius, dmg, fartKnockback, fartSlow, fartDebuffDuration);
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