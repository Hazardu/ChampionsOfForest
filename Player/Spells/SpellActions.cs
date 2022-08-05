using System;
using System.Linq;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public partial class SpellActions
	{
		public SpellActions()
		{
			instance = this;

			blinkAim = new SpellAimLine();
		}

		private static SpellActions instance;




		public void CastFlare()
		{
			Vector3 dir = LocalPlayer.Transform.position;
			float dmg = ModdedPlayer.Stats.spell_flareDamage + ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_flareDamageScaling;
			dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
			float slow = ModdedPlayer.Stats.spell_flareSlow;
			float boost = ModdedPlayer.Stats.spell_flareBoost;
			float duration = ModdedPlayer.Stats.spell_flareDuration;
			float radius = ModdedPlayer.Stats.spell_flareRadius;
			float Healing = ModdedPlayer.Stats.spell_flareHeal + ModdedPlayer.Stats.spellFlatDmg / 20 + (ModdedPlayer.Stats.healthRecoveryPerSecond * 2) * ModdedPlayer.Stats.healthPerSecRate;
			Healing *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
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
				NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}


		private SpellAimSphere blackholeAim;
		public void BlackHoleAimEnd()
		{
			blackholeAim.Disable();
		}

		public void BlackHoleAim()
		{
			if (blackholeAim == null || !blackholeAim.IsValid)
			{
				blackholeAim = new SpellAimSphere(new Color(0f, .6f, 0.95f, 0.5f), ModdedPlayer.Stats.spell_blackhole_radius);
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
			blackholeAim.SetRadius(ModdedPlayer.Stats.spell_blackhole_radius);
			blackholeAim.UpdatePosition(point);
		}

		public void CreatePlayerBlackHole()
		{
			float damage = (ModdedPlayer.Stats.spell_blackhole_damage + ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_blackhole_damageScaling) * ModdedPlayer.Stats.TotalMagicDamageMultiplier;
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
					w.Write(ModdedPlayer.Stats.spell_blackhole_duration);
					w.Write(ModdedPlayer.Stats.spell_blackhole_radius);
					w.Write(ModdedPlayer.Stats.spell_blackhole_pullforce);
					w.Write(ModdedPlayer.Stats.i_sparkOfLightAfterDark ? ModReferences.ThisPlayerID : "");
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
				answerStream.Close();
			}

		}

		public void CallbackSparkOfLightAfterDarkness(Vector3 pos)
		{
			CastBallLightningVisual(pos, Vector3.down);
		}



		//public float ShieldCastTime;
		//TODO Change sustain shield to use buffs

		public void CastSustainShieldActive()
		{
			float max = ModdedPlayer.Stats.spell_shieldMax + ModdedPlayer.Stats.spellFlatDmg;
			max *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
			float gain = ModdedPlayer.Stats.spell_shieldPerSecond + ModdedPlayer.Stats.spellFlatDmg / 35;
			gain *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
			ModdedPlayer.instance.damageAbsorbAmounts[1] = Mathf.Clamp(ModdedPlayer.instance.damageAbsorbAmounts[1] + Time.deltaTime * gain, 0, max);
			ShieldCastTime = Time.time;
		}

		public void CastSustainShieldPassive(bool on)
		{
			if (!on)
			{
				return;
			}

			if (ModdedPlayer.instance.damageAbsorbAmounts[1] > 0)
			{
				if (ShieldCastTime + ModdedPlayer.Stats.spell_shieldPersistanceLifetime < Time.time)
				{
					float loss = Time.deltaTime * (ModdedPlayer.Stats.spell_shieldPerSecond + ModdedPlayer.Stats.spellFlatDmg / 5) * 5 * ModdedPlayer.Stats.spellFlatDmg;
					ModdedPlayer.instance.damageAbsorbAmounts[1] = Mathf.Max(0, ModdedPlayer.instance.damageAbsorbAmounts[1] - loss);
				}
			}
		}



		public static int WarCryArmor => ModdedPlayer.Stats.armor.Value / 5;

		public static void CastWarCry()
		{
			float speed = ModdedPlayer.Stats.spell_warCryAtkSpeed + (ModdedPlayer.Stats.TotalMagicDamageMultiplier - 1) / 400;
			speed = Mathf.Min(speed, 1.75f);
			float dmg = speed;//ModdedPlayer.Stats.spell_warCryDamage + (ModdedPlayer.Stats.TotalMagicDamageMultiplier - 1) / 400;
							  //dmg = Mathf.Min(dmg, 1.75f);

			WarCry.GiveEffect(speed, dmg, ModdedPlayer.Stats.spell_warCryGiveDamage, ModdedPlayer.Stats.spell_warCryGiveArmor, WarCryArmor, ModdedPlayer.Stats.spell_warCryGiveDamageResistance);
			WarCry.SpawnEffect(LocalPlayer.Transform.position, ModdedPlayer.Stats.spell_warCryRadius);
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
						w.Write(ModdedPlayer.Stats.spell_warCryRadius);
						w.Write(speed);
						w.Write(dmg);
						w.Write(ModdedPlayer.Stats.spell_warCryGiveDamage);
						w.Write(ModdedPlayer.Stats.spell_warCryGiveArmor);
						w.Write(WarCryArmor);
						w.Write(ModdedPlayer.Stats.spell_warCryGiveDamageResistance);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
					answerStream.Close();
				}
			}
		}

		const float portalCastMaxRange = 66;
		public void DoPortalAim()
		{
			if (portalAimLine == null || !portalAimLine.IsValid)
			{
				portalAimLine = new SpellAimLine();
			}
			Transform t = Camera.main.transform;
			var hits1 = Physics.RaycastAll(t.position, t.forward, portalCastMaxRange + 1f);
			foreach (var hit in hits1)
			{
				if (!hit.transform.CompareTag("enemyCollide") && hit.transform.root != LocalPlayer.Transform.root)
				{
					portalAimLine.UpdatePosition(t.position + Vector3.down * 2, hit.point - t.forward + Vector3.up);
					return;
				}
			}

			portalAimLine.UpdatePosition(t.position + Vector3.down * 2, LocalPlayer.Transform.position + t.forward * portalCastMaxRange);
		}
		private SpellAimLine portalAimLine;

		public void CastPortal()
		{
			portalAimLine.Disable();
			Transform t = Camera.main.transform;
			var hits1 = Physics.RaycastAll(t.position, t.forward, portalCastMaxRange + 1f);
			Vector3 pos;
			foreach (var hit in hits1)
			{
				if (!hit.transform.CompareTag("enemyCollide") && hit.transform.root != LocalPlayer.Transform.root)
				{
					pos = hit.point - t.forward * 2.5f + Vector3.up * 2.1f;
					goto portal_postPickingPos;
				}
			}

			pos = LocalPlayer.Transform.position + t.forward * (portalCastMaxRange - 2) + Vector3.up * 2f;
		portal_postPickingPos:

			int id = Portal.GetPortalID();
			try
			{
				Portal.CreatePortal(pos, ModdedPlayer.Stats.spell_portalDuration, id, LocalPlayer.IsInCaves, LocalPlayer.IsInEndgame);
			}
			catch (Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}

			if (BoltNetwork.isRunning)
			{
				if (Portal.BothPortalsActive)
					Portal.SyncBothPortals();
				else
					Portal.SyncTransform(pos, ModdedPlayer.Stats.spell_portalDuration, id, LocalPlayer.IsInCaves, LocalPlayer.IsInEndgame);
			}
		}


		private SpellAimSphere arrowAim;

		public void MagicArrowAimEnd()
		{
			arrowAim.Disable();
		}

		public void MagicArrowAim()
		{
			if (arrowAim == null || !arrowAim.IsValid)
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
		private void SendMagicArrow(Vector3 pos, Vector3 dir, float damage)
		{
			if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
			{
				MagicArrow.Create(pos, dir, damage, ModReferences.ThisPlayerID, ModdedPlayer.Stats.spell_magicArrowDuration, ModdedPlayer.Stats.spell_magicArrowDoubleSlow, ModdedPlayer.Stats.spell_magicArrowDmgDebuff);
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
							w.Write(ModdedPlayer.Stats.spell_magicArrowDuration);
							w.Write(ModdedPlayer.Stats.spell_magicArrowDoubleSlow);
							w.Write(ModdedPlayer.Stats.spell_magicArrowDmgDebuff);
							w.Close();
						}
						NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
						answerStream.Close();
					}
				}
			}
			else if (GameSetup.IsMpClient)
			{
				MagicArrow.CreateEffect(pos, dir, ModdedPlayer.Stats.spell_magicArrowDmgDebuff, ModdedPlayer.Stats.spell_magicArrowDuration);
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
						w.Write(ModdedPlayer.Stats.spell_magicArrowDuration);
						w.Write(ModdedPlayer.Stats.spell_magicArrowDoubleSlow);
						w.Write(ModdedPlayer.Stats.spell_magicArrowDmgDebuff);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
					answerStream.Close();
				}
			}
		}
		public void CastMagicArrow()
		{
			var cam = Camera.main.transform;
			Vector3 pos = cam.position + cam.forward;
			Vector3 dir = cam.forward;
			CastMagicArrow(pos, dir);
		}
		public void CastMagicArrow(Vector3 pos, Vector3 dir)
		{
			float damage = 55 + ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_magicArrowDamageScaling + ModdedPlayer.Stats.rangedFlatDmg / 2;
			damage = damage * ModdedPlayer.Stats.TotalMagicDamageMultiplier * 2;
			if (ModdedPlayer.Stats.spell_magicArrowCrit)
				damage *= ModdedPlayer.Stats.RandomCritDamage;

			SendMagicArrow(pos, dir, damage);
			var cam = Camera.main.transform;

			for (int i = 0; i < ModdedPlayer.Stats.spell_magicArrowVolleyCount; i++)
			{
				SendMagicArrow(pos + (i * cam.right * 0.04f), Vector3.RotateTowards(dir, cam.right, Mathf.PI * i * 0.065f, 0), damage);
				SendMagicArrow(pos - (i * cam.right * 0.04f), Vector3.RotateTowards(dir, -cam.right, Mathf.PI * i * 0.065f, 0), damage);
			}
		}

		public void ToggleMultishot()
		{
			Multishot.IsOn = !Multishot.IsOn;
			Multishot.localPlayerInstance.SetActive(Multishot.IsOn);
		}



		public void CastPurge()
		{
			Vector3 pos = LocalPlayer.Transform.position;

			Purge.Cast(pos, ModdedPlayer.Stats.spell_purgeRadius, ModdedPlayer.Stats.spell_purgeHeal, ModdedPlayer.Stats.spell_purgeDamageBonus);

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
						w.Write(ModdedPlayer.Stats.spell_purgeRadius);
						w.Write(ModdedPlayer.Stats.spell_purgeHeal);
						w.Write(ModdedPlayer.Stats.spell_purgeDamageBonus);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
					answerStream.Close();
				}
			}
		}


		public void CastSnapFreeze()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			float dmg = 23 + ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_snapDamageScaling;
			dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
			if (GameSetup.IsSinglePlayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(9);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(ModdedPlayer.Stats.spell_snapFreezeDist);
						w.Write(ModdedPlayer.Stats.spell_snapFloatAmount);
						w.Write(ModdedPlayer.Stats.spell_snapFreezeDuration);
						w.Write(dmg);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
					answerStream.Close();
				}
			}
			if (!GameSetup.IsMpClient)
			{
				SnapFreeze.CreateEffect(pos, ModdedPlayer.Stats.spell_snapFreezeDist);
				SnapFreeze.HostAction(pos, ModdedPlayer.Stats.spell_snapFreezeDist, ModdedPlayer.Stats.spell_snapFloatAmount, ModdedPlayer.Stats.spell_snapFreezeDuration, dmg);
			}
		}



		public void CastBallLightningVisual(Vector3 pos, Vector3 speed)
		{
			float dmg = ModdedPlayer.Stats.spell_ballLightning_Damage + (ModdedPlayer.Stats.spell_ballLightning_DamageScaling * ModdedPlayer.Stats.spellFlatDmg);
			dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
			if (ModdedPlayer.Stats.spell_ballLightning_Crit)
				dmg *= ModdedPlayer.Stats.RandomCritDamage;

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
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
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
						w.Write(BallLightning.lastID + 1);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
					answerStream.Close();
				}
				BallLightning.lastID++;
			}
		}

		public void CastBallLightning()
		{
			Vector3 pos = LocalPlayer.Transform.position + LocalPlayer.Transform.forward;
			Vector3 speed = Camera.main.transform.forward * 2;

			CastBallLightningVisual(pos, speed);
		}


		public void BashPassiveEnabled(bool on)
		{
			ModdedPlayer.Stats.spell_bashEnabled.value = on;
			//SpellDataBase.spellDictionary[17].icon = on ? Res.ResourceLoader.GetTexture(132) : Res.ResourceLoader.GetTexture(131);
		}
		public void BashActive()
		{
			BuffManager.GiveBuff(13, 101, ModdedPlayer.Stats.spell_bashDamageDebuffAmount, ModdedPlayer.Stats.spell_bashDuration);
		}

		public void Bash(EnemyProgression ep, float dmg)
		{
			if (ModdedPlayer.Stats.spell_bashEnabled)
			{
				int id = 43;
				ep.Slow(id, ModdedPlayer.Stats.spell_bashSlowAmount, ModdedPlayer.Stats.spell_bashDuration);
				ep.DmgTakenDebuff(id, ModdedPlayer.Stats.spell_bashDamageDebuffAmount, ModdedPlayer.Stats.spell_bashDuration);
				if (ModdedPlayer.Stats.spell_bashBleedChance > 0 && UnityEngine.Random.value < ModdedPlayer.Stats.spell_bashBleedChance)
					ep.DoDoT((int)(dmg * ModdedPlayer.Stats.spell_bashBleedDmg), ModdedPlayer.Stats.spell_bashDuration);
				if (ModdedPlayer.Stats.spell_bashLifesteal > 0)
				{
					LocalPlayer.Stats.HealthTarget += dmg * ModdedPlayer.Stats.spell_bashLifesteal;
					LocalPlayer.Stats.Energy += dmg * ModdedPlayer.Stats.spell_bashLifesteal;
				}
				if (ModdedPlayer.Stats.spell_bashDamageBuff > 0)
				{
					BuffManager.GiveBuff(24, 89, ModdedPlayer.Stats.spell_bashDamageBuff * 0.25f, 2);
				}
			}
		}

		public void Bash(ulong enemy, float dmg)
		{
			if (ModdedPlayer.Stats.spell_bashEnabled)
			{
				int id = 44 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);

				if (ModdedPlayer.Stats.spell_bashLifesteal > 0)
				{
					LocalPlayer.Stats.HealthTarget += dmg * ModdedPlayer.Stats.spell_bashLifesteal;
					LocalPlayer.Stats.Energy += dmg * ModdedPlayer.Stats.spell_bashLifesteal;
				}

				if (ModdedPlayer.Stats.spell_bashDamageBuff > 0)
				{
					BuffManager.GiveBuff(24, 89, ModdedPlayer.Stats.spell_bashDamageBuff * 0.25f, 2);
				}
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(33);
						w.Write(enemy);
						w.Write(ModdedPlayer.Stats.spell_bashDuration);
						w.Write(id);
						w.Write(ModdedPlayer.Stats.spell_bashSlowAmount);
						w.Write(ModdedPlayer.Stats.spell_bashDamageDebuffAmount);
						w.Write(((int)(dmg * ModdedPlayer.Stats.spell_bashBleedDmg)));
						w.Write(ModdedPlayer.Stats.spell_bashBleedChance);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
			}
		}



		public void OnFrenzyAttack()
		{
			if (ModdedPlayer.Stats.spell_frenzy)
			{
				if (BuffManager.activeBuffs.ContainsKey(60))
				{
					int frenzyStacks = ModdedPlayer.Stats.spell_frenzyStacks;
					BuffManager.activeBuffs[60].OnEnd(ModdedPlayer.Stats.spell_frenzyStacks);
					ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive = Mathf.Min(ModdedPlayer.Stats.spell_frenzyMaxStacks, frenzyStacks + 1);
					BuffManager.activeBuffs[60].amount = ModdedPlayer.Stats.spell_frenzyStacks;
					BuffManager.activeBuffs[60].duration = 4;
					BuffManager.activeBuffs[60].OnStart(ModdedPlayer.Stats.spell_frenzyStacks);
				}
				else
				{
					ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive++;
					ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive = Mathf.Min(ModdedPlayer.Stats.spell_frenzyMaxStacks, ModdedPlayer.Stats.spell_frenzyStacks);
					BuffManager.GiveBuff(19, 60, ModdedPlayer.Stats.spell_frenzyStacks, 4);
				}
			}
		}




		public float FocusOnBodyShot()
		{
			if (!ModdedPlayer.Stats.spell_focus)
				return 1;
			if (ModdedPlayer.Stats.spell_focusBonusDmg == 0)
			{
				ModdedPlayer.Stats.spell_focusBonusDmg.valueAdditive = ModdedPlayer.Stats.spell_focusOnBS;
				BuffManager.GiveBuff(14, 61, ModdedPlayer.Stats.spell_focusOnAtkSpeed, ModdedPlayer.Stats.spell_focusOnAtkSpeedDuration);
				return 1;
			}
			else
			{
				var result = 1f + ModdedPlayer.Stats.spell_focusBonusDmg;
				ModdedPlayer.Stats.spell_focusBonusDmg.valueAdditive = 0;
				return result;
			}
		}

		public float FocusOnHeadShot()
		{
			if (!ModdedPlayer.Stats.spell_focus)
				return 1;
			if (ModdedPlayer.Stats.spell_focusBonusDmg == 0)
			{
				ModdedPlayer.Stats.spell_focusBonusDmg.valueAdditive = ModdedPlayer.Stats.spell_focusOnHS;
				return 1;
			}
			else
			{
				var result = 1f + ModdedPlayer.Stats.spell_focusBonusDmg;
				ModdedPlayer.Stats.spell_focusBonusDmg.valueAdditive = 0;
				return result;
			}
		}


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
			ModdedPlayer.Stats.spell_seekingArrow.value = false;
			SeekingArrow_TimeStamp = 0;
			SeekingArrow_ChangeTargetOnHit = true;
		}

		public static void SeekingArrow_End()
		{
			SeekingArrow_Target.gameObject.SetActive(false);
		}

		public static void SetSeekingArrowTarget(Transform bone)
		{
			ModdedPlayer.Stats.spell_seekingArrow.value = true;
			SeekingArrow_Target.gameObject.SetActive(true);
			SeekingArrow_Target.transform.parent = bone;
			SeekingArrow_Target.transform.position = bone.position;
			SeekingArrow_TimeStamp = Time.time;
			SeekingArrow_ChangeTargetOnHit = false;
		}



		private static float LastBlockTimestamp = 0;    //used for blocking any instance of damage
		private const float Block_parryTime = 0.3f; //300ms to get hit since starting blocking will cause a parry
		public static float GetParryCounterStrikeDmg()
		{
			if (ModdedPlayer.Stats.perk_parryCounterStrikeDamage.Value > 0)
			{
				float f = ModdedPlayer.Stats.perk_parryCounterStrikeDamage;
				if (BuffManager.activeBuffs.ContainsKey(88))
				{
					BuffManager.activeBuffs[88].ForceEndBuff(88);
				}
				ModdedPlayer.Stats.perk_parryCounterStrikeDamage.Reset();
				return f;
			}
			return 0;
		}
		public static void OnBlockSetTimer()
		{
			LastBlockTimestamp = Time.time + Block_parryTime;
		}

		public static bool ParryAnythingIsTimed =>
			 ModdedPlayer.Stats.perk_parryAnything && LastBlockTimestamp > Time.time;

		public static void DoParry(Vector3 parryPos)
		{
			if (ModdedPlayer.Stats.spell_parry)
			{
				BuffManager.GiveBuff(6, 61, 1, ModdedPlayer.Stats.spell_parryBuffDuration);
				float dmg = ModdedPlayer.Stats.spell_parryDamage + ModdedPlayer.Stats.spellFlatDmg;
				dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier * ModdedPlayer.Stats.spell_parryDamageScaling;

				float heal = ModdedPlayer.Stats.spell_parryHeal + ModdedPlayer.Stats.spellFlatDmg / 6 + ModdedPlayer.Stats.healthRecoveryPerSecond + ModdedPlayer.Stats.healthOnHit * 3;
				heal *= ModdedPlayer.Stats.allRecoveryMult * (1 + ModdedPlayer.Stats.healthPerSecRate);
				LocalPlayer.Stats.HealthTarget += heal;
				ParrySound.Play(parryPos);
				float energy = (ModdedPlayer.Stats.spell_parryEnergy + ModdedPlayer.Stats.energyOnHit * 10) * ModdedPlayer.Stats.TotalEnergyRecoveryMultiplier + +ModdedPlayer.Stats.TotalMaxEnergy / 5.5f;
				LocalPlayer.Stats.Energy += energy;
				LocalPlayer.Stats.Stamina += energy;
				if (ModdedPlayer.Stats.spell_parryDmgBonus > 0)
				{
					float f = dmg * ModdedPlayer.Stats.spell_parryDmgBonus;
					ModdedPlayer.Stats.perk_parryCounterStrikeDamage.Add(f);
					BuffManager.GiveBuff(23, 88, f, 20);
				}
				if (ModdedPlayer.Stats.spell_parryAttackSpeed > 1)
				{
					BuffManager.GiveBuff(14, 105, ModdedPlayer.Stats.spell_parryAttackSpeed.Value, 5);

				}

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
							w.Write(ModdedPlayer.Stats.spell_parryRadius);
							w.Write(ModdedPlayer.Stats.spell_parryIgnites);
							w.Write(dmg);
							w.Close();
						}
						NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
						answerStream.Close();
					}
				}
				{
					var hits = Physics.SphereCastAll(parryPos, ModdedPlayer.Stats.spell_parryRadius, Vector3.one);
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].transform.CompareTag("enemyCollide"))
						{
							if (EnemyManager.enemyByTransform.ContainsKey(hits[i].transform.root))
							{
								var prog = EnemyManager.enemyByTransform[hits[i].transform.root];
								prog.HitMagic(dmg);
								if (ModdedPlayer.Stats.spell_parryIgnites)
									prog.HealthScript.Burn();
							}
						}
					}
				}
			}
		}




		public static void CataclysmAimEnd()
		{
		}

		public static void CataclysmAim()
		{
		}

		public static void CastCataclysm()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			BuffManager.GiveBuff(1, 66, 0.1f, 2.5f);
			float dmg = ModdedPlayer.Stats.spell_cataclysmDamage + ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_cataclysmDamageScaling;
			dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier * ModdedPlayer.Stats.fireDamage;
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(11);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(ModdedPlayer.Stats.spell_cataclysmRadius);
					w.Write(dmg);
					w.Write(ModdedPlayer.Stats.spell_cataclysmDuration);
					w.Write(ModdedPlayer.Stats.spell_cataclysmArcane);
					w.Write(false);
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}




		public static void CastBloodInfArr()
		{
			float takenHP = LocalPlayer.Stats.Health * ModdedPlayer.Stats.spell_bia_HealthTaken;
			if (takenHP > LocalPlayer.Stats.Health - 5)
				takenHP = LocalPlayer.Stats.Health - 5;
			LocalPlayer.Stats.Health -= takenHP;
			LocalPlayer.Stats.HealthTarget -= takenHP;
			ModdedPlayer.Stats.spell_bia_AccumulatedDamage.valueAdditive = (takenHP * ModdedPlayer.Stats.spell_bia_HealthDmMult + ModdedPlayer.Stats.spell_bia_SpellDmMult * ModdedPlayer.Stats.spellFlatDmg) * ModdedPlayer.Stats.TotalMagicDamageMultiplier;
			if (ModdedPlayer.Stats.spell_bia_TripleDmg)
			{
				ModdedPlayer.Stats.spell_bia_AccumulatedDamage.valueAdditive *= 3;
				BuffManager.GiveBuff(18, 95, ModdedPlayer.Stats.TotalMaxEnergy / 100, 10);
			}
			if (ModdedPlayer.Stats.i_HazardCrown)
				ModdedPlayer.Stats.i_HazardCrownBonus.valueAdditive = 5;
			Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.BloodInfusedArrow);
			NetworkManager.SendPlayerHitmarker(LocalPlayer.Transform.position + Vector3.up, (int)takenHP);
		}



		public static float fartRadius = 30;
		public static float fartKnockback = 4, fartSlow = 0.8f, fartDebuffDuration = 50f, fartBaseDmg = 200f;

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
				float dmg = (ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_fartDamageScaling + fartBaseDmg) * ModdedPlayer.Stats.TotalMagicDamageMultiplier;
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
					FartSpell.DealDamageAsHost(origin, back, fartRadius / 2, dmg, fartKnockback / 2, fartSlow, fartDebuffDuration / 2);
				}
				SpellCaster.instance.infos.First(x => x.spell.ID == 24).Cooldown /= 3;
			}
			else
			{
				float dmg = (ModdedPlayer.Stats.spellFlatDmg + fartBaseDmg) * ModdedPlayer.Stats.TotalMagicDamageMultiplier;
				BuffManager.GiveBuff(1, 96, 0.4f, 7.175f);
				FartSpell.FartWarmup(fartRadius, dmg, fartKnockback, fartSlow, fartDebuffDuration);
			}
		}
	}
}