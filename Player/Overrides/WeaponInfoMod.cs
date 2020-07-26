using Bolt;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;

using TheForest.Audio;
using TheForest.Buildings.World;
using TheForest.Utils;
using TheForest.World;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest.Player
{
	public class WeaponInfoMod : weaponInfo
	{
		public static bool AlwaysIgnite = false;

		protected override void Update()
		{
			if (mainTriggerScript != null)
			{
				if (PlayerInventoryMod.EquippedModel != BaseItem.WeaponModelType.None && PlayerInventoryMod.EquippedModel != BaseItem.WeaponModelType.Greatbow)
				{
					CustomWeapon cw = PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel];
					setup.pmStamina.FsmVariables.GetFsmFloat("notTiredSpeed").Value = animSpeed * cw.swingspeed;
					setup.pmStamina.FsmVariables.GetFsmFloat("staminaDrain").Value = staminaDrain * -1f * ModdedPlayer.Stats.attackStaminaCost;
					setup.pmStamina.FsmVariables.GetFsmFloat("tiredSpeed").Value = animTiredSpeed * cw.tiredswingspeed;
					setup.pmStamina.FsmVariables.GetFsmFloat("staminaDrain").Value = staminaDrain * -1f * (ModdedPlayer.Stats.attackStaminaCost);
					LocalPlayer.Stats.blockDamagePercent = ModdedPlayer.Stats.block * blockDamagePercent;
				}
				else
				{
					setup.pmStamina.FsmVariables.GetFsmFloat("staminaDrain").Value = staminaDrain * -1f * (ModdedPlayer.Stats.attackStaminaCost);
					setup.pmStamina.FsmVariables.GetFsmFloat("staminaDrain").Value = staminaDrain * -1f * (ModdedPlayer.Stats.attackStaminaCost);
					LocalPlayer.Stats.blockDamagePercent = ModdedPlayer.Stats.block * blockDamagePercent;
				}
				//float ats = ModdedPlayer.instance.AttackSpeed;
				//if (GreatBow.isEnabled) ats /= 10f;
				//if (LocalPlayer.Stats.Stamina > 4)
				//{
				//    animator.speed = ats;
				//}
				//else
				//{
				//    animator.speed =Mathf.Min( 0.5f, ats/2);

				//}
			}

			base.Update();
		}

		protected override void setupMainTrigger()
		{
			if ((bool)mainTriggerScript)
			{
				if (stick)
				{
					mainTriggerScript.stick = true;
				}
				else
				{
					mainTriggerScript.stick = false;
				}
				if (axe)
				{
					mainTriggerScript.axe = true;
				}
				else
				{
					mainTriggerScript.axe = false;
				}
				if (rock)
				{
					mainTriggerScript.rock = true;
				}
				else
				{
					mainTriggerScript.rock = false;
				}
				if (fireStick)
				{
					mainTriggerScript.fireStick = true;
				}
				else
				{
					mainTriggerScript.fireStick = false;
				}
				if (spear)
				{
					mainTriggerScript.spear = true;
				}
				else
				{
					mainTriggerScript.spear = false;
				}
				if (shell)
				{
					mainTriggerScript.spear = true;
				}
				else
				{
					mainTriggerScript.shell = false;
				}
				if (chainSaw)
				{
					mainTriggerScript.chainSaw = true;
				}
				else
				{
					mainTriggerScript.chainSaw = false;
				}
				if (machete)
				{
					mainTriggerScript.machete = true;
				}
				else
				{
					mainTriggerScript.machete = false;
				}
				mainTriggerScript.repairTool = repairTool;
				mainTriggerScript.weaponDamage = WeaponDamage;
				mainTriggerScript.smashDamage = smashDamage;
				mainTriggerScript.smallAxe = smallAxe;
				if (weaponRange == 0f)
				{
					mainTriggerScript.hitTriggerRange.transform.localScale = new Vector3(1f, 1f, 1f * ModdedPlayer.Stats.weaponRange);
				}
				else if (BoltNetwork.isClient)
				{
					mainTriggerScript.hitTriggerRange.transform.localScale = new Vector3(1f, 1f, Mathf.Clamp(weaponRange * ModdedPlayer.Stats.weaponRange, 1f, weaponRange * ModdedPlayer.Stats.weaponRange));
				}
				else
				{
					mainTriggerScript.hitTriggerRange.transform.localScale = new Vector3(1f, 1f, weaponRange * ModdedPlayer.Stats.weaponRange);
				}
			}
		}

		protected override void OnTriggerEnter(Collider other)
		{
			int rep = 1;

			PlayerHitEnemy playerHitEnemy;
			mutantHitReceiver component6;
			if (!other.gameObject.CompareTag("Player") && animator.GetCurrentAnimatorStateInfo(2).tagHash != animControl.deathHash && !(currentWeaponScript == null))
			{
				if (other.CompareTag("hanging") || other.CompareTag("corpseProp"))
				{
					if (animControl.smashBool)
					{
						if (LocalPlayer.Animator.GetFloat("tiredFloat") < 0.35f)
						{
							base.Invoke("spawnSmashWeaponBlood", 0.1f);
						}
						else
						{
							base.Invoke("spawnSmashWeaponBlood", 0.03f);
						}
					}
					else
					{
						spawnWeaponBlood(other, false);
					}
					Mood.HitRumble();
					other.gameObject.SendMessageUpwards("Hit", 0, SendMessageOptions.DontRequireReceiver);
					FauxMpHit(0);
					FMODCommon.PlayOneshotNetworked(currentWeaponScript.fleshHitEvent, base.transform, FMODCommon.NetworkRole.Any);
				}
				if (!ForestVR.Enabled && GetInvalidAttackAngle(other))
				{
					return;
				}
				playerHitEnemy = null;
				if ((mainTrigger || (ForestVR.Enabled && !mainTrigger)) && repairTool)
				{
					RepairTool component = currentWeaponScript.gameObject.GetComponent<RepairTool>();
					if (component && component.IsRepairFocused)
					{
						currentWeaponScript.gameObject.SendMessage("OnRepairStructure", other.gameObject);
						if ((bool)component.FocusedRepairCollider)
						{
							currentWeaponScript.PlaySurfaceHit(component.FocusedRepairCollider, SfxInfo.SfxTypes.HitWood);
						}
					}
					return;
				}
				mutantTargetSwitching component2 = other.transform.GetComponent<mutantTargetSwitching>();
				if ((other.CompareTag("enemyCollide") || other.CompareTag("animalCollide") || other.CompareTag("Fish") || other.CompareTag("EnemyBodyPart")) && (mainTrigger || animControl.smashBool || chainSaw))
				{
					bool flag = false;
					if (component2 && component2.regular)
					{
						flag = true;
					}
					if (animControl.smashBool)
					{
						if (LocalPlayer.Animator.GetFloat("tiredFloat") < 0.35f)
						{
							base.Invoke("spawnSmashWeaponBlood", 0.1f);
						}
						else
						{
							base.Invoke("spawnSmashWeaponBlood", 0.03f);
						}
					}
					else if (!flag)
					{
						spawnWeaponBlood(other, false);
					}
				}
				if (other.gameObject.CompareTag("PlayerNet") && (mainTrigger || (!mainTrigger && (animControl.smashBool || chainSaw))))
				{
					if (!ModSettings.FriendlyFire)
					{
						return;
					}

					BoltEntity component3 = other.GetComponent<BoltEntity>();
					BoltEntity component4 = base.GetComponent<BoltEntity>();
					if (!object.ReferenceEquals(component3, component4) && lastPlayerHit + 0.4f < Time.time)
					{
						other.transform.root.SendMessage("getClientHitDirection", animator.GetInteger("hitDirection"), SendMessageOptions.DontRequireReceiver);
						other.transform.root.SendMessage("StartPrediction", SendMessageOptions.DontRequireReceiver);
						lastPlayerHit = Time.time;
						if (BoltNetwork.isRunning)
						{
							ModdedPlayer.instance.OnHit();
							ModdedPlayer.instance.OnHit_Melee(other.transform);

							DamageMath.DamageClamp(2f * (WeaponDamage + ModdedPlayer.Stats.meleeFlatDmg + SpellActions.GetParryCounterStrikeDmg()) * ModdedPlayer.Stats.MeleeDamageMult* ModdedPlayer.Stats.RandomCritDamage, out int dmg, out int repetitions);

							HitPlayer hitPlayer = HitPlayer.Create(component3, EntityTargets.Everyone);
							hitPlayer.damage = dmg;
							for (int i = 0; i < repetitions; i++)
								hitPlayer.Send();
						}
					}
					return;
				}
				if (BoltNetwork.isClient)
				{
					playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
					playerHitEnemy.Target = other.GetComponentInParent<BoltEntity>();
				}
				if (other.gameObject.CompareTag("enemyHead") && !mainTrigger)
				{
					other.transform.SendMessageUpwards("HitHead", SendMessageOptions.DontRequireReceiver);
					if (playerHitEnemy != null)
					{
						playerHitEnemy.HitHead = true;
					}
				}
				if (other.gameObject.CompareTag("enemyCollide") && !mainTrigger && !animControl.smashBool && !repairTool)
				{
					other.transform.SendMessage("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
				}
				if (other.gameObject.CompareTag("structure") && !repairTool && (!BoltNetwork.isRunning || BoltNetwork.isServer || !BoltNetwork.isClient || !PlayerPreferences.NoDestructionRemote))
				{
					setup.pmNoise.SendEvent("toWeaponNoise");
					Mood.HitRumble();
					other.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
					float damage = WeaponDamage * 4f + ModdedPlayer.Stats.meleeFlatDmg + SpellActions.GetParryCounterStrikeDmg();
					damage *= ModdedPlayer.Stats.RandomCritDamage * ModdedPlayer.Stats.MeleeDamageMult;
					if (tht.atEnemy)
					{
						damage *= 0.125f;
					}
					other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, damage), SendMessageOptions.DontRequireReceiver);
				}
				if (BoltNetwork.isClient && (other.CompareTag("jumpObject") || other.CompareTag("UnderfootWood")) && !repairTool)
				{
					float damage = WeaponDamage + ModdedPlayer.Stats.meleeFlatDmg;
					damage *= ModdedPlayer.Stats.RandomCritDamage * ModdedPlayer.Stats.MeleeDamageMult;
					FauxMpHit(Mathf.CeilToInt(damage * 4f));
				}
				switch (other.gameObject.tag)
				{
					case "jumpObject":
					case "UnderfootWood":
					case "SLTier1":
					case "SLTier2":
					case "SLTier3":
					case "UnderfootRock":
					case "Target":
					case "Untagged":
					case "Block":
						if (!repairTool)
						{
							if (BoltNetwork.isRunning && !BoltNetwork.isServer && BoltNetwork.isClient && PlayerPreferences.NoDestructionRemote)
							{
								break;
							}
							other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, WeaponDamage * 4f), SendMessageOptions.DontRequireReceiver);
							setup.pmNoise.SendEvent("toWeaponNoise");
						}
						break;
				}
				PlaySurfaceHit(other, SfxInfo.SfxTypes.None);
				if (spear && other.gameObject.CompareTag("Fish") && (MyFish == null || !MyFish.gameObject.activeSelf) && (!mainTrigger || ForestVR.Enabled))
				{
					base.transform.parent.SendMessage("GotBloody", SendMessageOptions.DontRequireReceiver);
					FMODCommon.PlayOneshotNetworked(fleshHitEvent, base.transform, FMODCommon.NetworkRole.Any);
					spearedFish.Add(other.gameObject);
					other.transform.parent = base.transform;
					other.transform.position = SpearTip.position;
					other.transform.rotation = SpearTip.rotation;
					MyFish = other.transform.GetComponent<Fish>();
					if (MyFish && MyFish.typeCaveFish)
					{
						other.transform.position = SpearTip2.position;
						other.transform.rotation = SpearTip2.rotation;
					}
					other.SendMessage("DieSpear", SendMessageOptions.DontRequireReceiver);
				}
				if (other.gameObject.CompareTag("hanging") || other.gameObject.CompareTag("corpseProp") || (other.gameObject.CompareTag("BreakableWood") && !mainTrigger))
				{
					Rigidbody component5 = other.GetComponent<Rigidbody>();
					float d = pushForce;
					if (other.gameObject.CompareTag("BreakableWood"))
					{
						d = 4500f;
					}
					if ((bool)component5)
					{
						component5.AddForceAtPosition(playerTr.forward * d * 0.75f * (0.016666f / Time.fixedDeltaTime), base.transform.position, ForceMode.Force);
					}
					if (!(bool)other.gameObject.GetComponent<WeaponHitSfxInfo>() && (other.gameObject.CompareTag("hanging") || other.gameObject.CompareTag("corpseProp")))
					{
						FMODCommon.PlayOneshotNetworked(currentWeaponScript.fleshHitEvent, weaponAudio.transform, FMODCommon.NetworkRole.Any);
					}
				}
				if (spear && !mainTrigger && (other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Ocean")))
				{
					if (!LocalPlayer.ScriptSetup.targetInfo.inYacht)
					{
						PlayGroundHit(waterHitEvent);
						base.StartCoroutine(spawnSpearSplash(other));
					}
					setup.pmNoise.SendEvent("toWeaponNoise");
				}
				if (!spear && !mainTrigger && (other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Ocean")) && !LocalPlayer.ScriptSetup.targetInfo.inYacht)
				{
					PlayGroundHit(waterHitEvent);
				}
				if (other.gameObject.CompareTag("Shell") && !mainTrigger)
				{
					other.gameObject.SendMessage("getAttackerType", 4, SendMessageOptions.DontRequireReceiver);
					other.gameObject.SendMessage("getAttacker", Player, SendMessageOptions.DontRequireReceiver);
					Mood.HitRumble();
					other.transform.SendMessageUpwards("Hit", 1, SendMessageOptions.DontRequireReceiver);
					PlayEvent(currentWeaponScript.shellHitEvent, weaponAudio);
				}
				if (other.gameObject.CompareTag("PlaneHull") && !mainTrigger)
				{
					PlayEvent(currentWeaponScript.planeHitEvent, weaponAudio);
				}
				if (other.gameObject.CompareTag("Tent") && !mainTrigger)
				{
					PlayEvent(currentWeaponScript.tentHitEvent, weaponAudio);
				}
				component6 = other.GetComponent<mutantHitReceiver>();
				if ((other.gameObject.CompareTag("enemyCollide") || other.gameObject.CompareTag("animalCollide")) && mainTrigger && !enemyDelay && !animControl.smashBool)
				{
					ModdedPlayer.instance.OnHit();
					ModdedPlayer.instance.OnHit_Melee(other.transform);
					if (ModdedPlayer.Stats.TotalMeleeArmorPiercing > 0 && other.gameObject.CompareTag("enemyCollide"))
					{
						if (BoltNetwork.isClient)
						{
							EnemyProgression.ReduceArmor(playerHitEnemy.Target, ModdedPlayer.Stats.TotalMeleeArmorPiercing);
						}
						else
						{
							other.gameObject.SendMessageUpwards("ReduceArmor", ModdedPlayer.Stats.TotalMeleeArmorPiercing, SendMessageOptions.DontRequireReceiver);
						}
					}
					if (BoltNetwork.isClient && other.gameObject.CompareTag("enemyCollide"))
					{
						CoopMutantClientHitPrediction componentInChildren = other.transform.root.gameObject.GetComponentInChildren<CoopMutantClientHitPrediction>();
						if ((bool)componentInChildren)
						{
							componentInChildren.getClientHitDirection(animator.GetInteger("hitDirection"));
							componentInChildren.StartPrediction();
						}
					}
					if ((bool)currentWeaponScript)
					{
						currentWeaponScript.transform.parent.SendMessage("GotBloody", SendMessageOptions.DontRequireReceiver);
					}
					Vector3 vector = other.transform.root.GetChild(0).InverseTransformPoint(playerTr.position);
					float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
					other.gameObject.SendMessage("getAttackerType", 4, SendMessageOptions.DontRequireReceiver);
					other.gameObject.SendMessage("getAttacker", Player, SendMessageOptions.DontRequireReceiver);
					if (playerHitEnemy != null)
					{
						playerHitEnemy.getAttackerType = 4;
					}
					animator.SetFloatReflected("connectFloat", 1f);
					base.Invoke("resetConnectFloat", 0.3f);
					if (num < -140f || num > 140f)
					{
						if ((bool)component6)
						{
							component6.takeDamage(1);
						}
						else
						{
							other.transform.SendMessageUpwards("takeDamage", 1, SendMessageOptions.DontRequireReceiver);
						}
						if (playerHitEnemy != null)
						{
							playerHitEnemy.takeDamage = 1;
						}
					}
					else
					{
						if ((bool)component6)
						{
							component6.takeDamage(0);
						}
						else
						{
							other.transform.SendMessageUpwards("takeDamage", 0, SendMessageOptions.DontRequireReceiver);
						}
						if (playerHitEnemy != null)
						{
							playerHitEnemy.takeDamage = 0;
						}
					}
					if (spear || shell || chainSaw)
					{
						other.transform.SendMessageUpwards("getAttackDirection", 3, SendMessageOptions.DontRequireReceiver);
						if (playerHitEnemy != null)
						{
							playerHitEnemy.getAttackDirection = 3;
						}
					}
					else if (axe || rock || stick)
					{
						int integer = animator.GetInteger("hitDirection");
						if (axe)
						{
							if ((bool)component6)
							{
								component6.getAttackDirection(integer);
								component6.getStealthAttack();
							}
							else
							{
								other.transform.SendMessageUpwards("getAttackDirection", integer, SendMessageOptions.DontRequireReceiver);
								other.transform.SendMessageUpwards("getStealthAttack", SendMessageOptions.DontRequireReceiver);
							}
						}
						else if (stick)
						{
							if ((bool)component6)
							{
								component6.getAttackDirection(integer);
							}
							else
							{
								other.transform.SendMessageUpwards("getAttackDirection", integer, SendMessageOptions.DontRequireReceiver);
							}
						}
						else if ((bool)component6)
						{
							component6.getAttackDirection(0);
							component6.getStealthAttack();
						}
						else
						{
							other.transform.SendMessageUpwards("getAttackDirection", 0, SendMessageOptions.DontRequireReceiver);
							other.transform.SendMessageUpwards("getStealthAttack", SendMessageOptions.DontRequireReceiver);
						}
						if (playerHitEnemy != null)
						{
							if (axe)
							{
								playerHitEnemy.getAttackDirection = integer;
							}
							else if (stick)
							{
								playerHitEnemy.getAttackDirection = integer;
							}
							else
							{
								playerHitEnemy.getAttackDirection = 0;
							}
							playerHitEnemy.getStealthAttack = true;
						}
					}
					else
					{
						int integer2 = animator.GetInteger("hitDirection");
						if ((bool)component6)
						{
							component6.getAttackDirection(integer2);
						}
						else
						{
							other.transform.SendMessageUpwards("getAttackDirection", integer2, SendMessageOptions.DontRequireReceiver);
						}
						if (playerHitEnemy != null)
						{
							playerHitEnemy.getAttackDirection = integer2;
						}
					}
					if ((fireStick && Random.value > 0.8f) || AlwaysIgnite || Effects.BlackFlame.IsOn)
					{
						if ((bool)component6)
						{
							component6.Burn();
						}
						else
						{
							other.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
						}
						if (playerHitEnemy != null)
						{
							playerHitEnemy.Burn = true;
						}
					}
					float num2 = WeaponDamage + ModdedPlayer.Stats.meleeFlatDmg + SpellActions.GetParryCounterStrikeDmg();
					float crit = ModdedPlayer.Stats.RandomCritDamage;
					num2 *= crit * ModdedPlayer.Stats.MeleeDamageMult;
					if (component2 && chainSaw && (component2.typeMaleCreepy || component2.typeFemaleCreepy || component2.typeFatCreepy))
					{
						num2 /= 2f;
					}

					EnemyProgression ep = other.GetComponentInParent<EnemyProgression>();
					if (BlackFlame.IsOn && BlackFlame.GiveAfterburn && Random.value < 0.1f)
					{
						if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
						{
							if (ep != null)
							{
								int id = 120 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
								ep.DmgTakenDebuff(id, 1.15f, 25);
							}
							else if (playerHitEnemy != null)
							{
								int id = 120 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
								using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
								{
									using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
									{
										w.Write(34);
										w.Write(playerHitEnemy.Target.networkId.PackedValue);
										w.Write(id);
										w.Write(1.15f);
										w.Write(25f);
										w.Close();
									}
									AsyncHit.SendCommandDelayed(1, answerStream.ToArray(), NetworkManager.Target.OnlyServer);
									answerStream.Close();
								}
							}
						}
					}
					if (ModdedPlayer.Stats.i_HammerStun && PlayerInventoryMod.EquippedModel == BaseItem.WeaponModelType.Hammer)
					{
						if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
						{
							if (ep != null)
							{
								ep.Slow(40, ModdedPlayer.Stats.i_HammerStunAmount, ModdedPlayer.Stats.i_HammerStunDuration);
							}
						}
						else if (playerHitEnemy != null)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(22);
									w.Write(playerHitEnemy.Target.networkId.PackedValue);
									w.Write(ModdedPlayer.Stats.i_HammerStunAmount);
									w.Write(ModdedPlayer.Stats.i_HammerStunDuration);
									w.Write(40);
									w.Close();
								}
								AsyncHit.SendCommandDelayed(2, answerStream.ToArray(), NetworkManager.Target.OnlyServer);
								answerStream.Close();
							}
						}
					}

					if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
					{
						if (ep != null)
						{
							SpellActions.Bash(ep, num2);
						}
					}
					else if (playerHitEnemy != null)
					{
						SpellActions.Bash(playerHitEnemy.Target.networkId.PackedValue, num2);
					}

					if (Effects.BlackFlame.IsOn)
					{
						if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
						{
							other.GetComponentInParent<EnemyProgression>()?.FireDebuff(40, Effects.BlackFlame.FireDamageBonus, 20);
						}
						else if (playerHitEnemy != null)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(27);
									w.Write(playerHitEnemy.Target.networkId.PackedValue);
									w.Write(Effects.BlackFlame.FireDamageBonus);
									w.Write(20f);
									w.Write(1);
									w.Close();
								}
								AsyncHit.SendCommandDelayed(3, answerStream.ToArray(), NetworkManager.Target.OnlyServer);
								answerStream.Close();
							}

							//Network.NetworkManager.SendLine("AH" + playerHitEnemy.Target.networkId.PackedValue + ";" + Effects.BlackFlame.FireDamageBonus + ";" + 20 + ";1;", Network.NetworkManager.Target.OnlyServer);
						}
					}
					if (ModdedPlayer.Stats.perk_fireDmgIncreaseOnHit)
					{
						int myID = 2000 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
						float dmg = 1 + ModdedPlayer.Stats.spellFlatDmg / 3;
						dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
						dmg *= ModdedPlayer.Stats.fireDamage + 1;
						dmg *= 0.3f;

						if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
						{
							other.GetComponentInParent<EnemyProgression>()?.FireDebuff(myID, dmg, 14);
						}
						else if (playerHitEnemy != null)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(27);
									w.Write(playerHitEnemy.Target.networkId.PackedValue);
									w.Write(dmg);
									w.Write(15);
									w.Write(myID);
									w.Close();
								}
								AsyncHit.SendCommandDelayed(4, answerStream.ToArray(), NetworkManager.Target.OnlyServer);
								answerStream.Close();
							}
						}
					}

					if (ModdedPlayer.Stats.perk_danceOfFiregod && Effects.BlackFlame.IsOn)
					{
						num2 *= 1 + LocalPlayer.Rigidbody.velocity.magnitude;
					}

					DamageMath.DamageClamp(num2, out int d, out int a);

					if (hitReactions.kingHitBool || fsmHeavyAttackBool.Value)
					{
						num2 *= ModdedPlayer.Stats.heavyAttackDmg;
						DamageMath.DamageClamp(num2*3, out d, out a);

						if ((bool)component6)
						{
							if (fsmHeavyAttackBool.Value && axe && !smallAxe)
							{
								component6.sendHitFallDown(num2 * 3f);
								if (playerHitEnemy != null)
								{
									playerHitEnemy.Hit = d;
									playerHitEnemy.hitFallDown = true;
									rep = a;
								}
							}
							else
							{
								component6.getCombo(3);
								for (int i = 0; i < a; i++)
								{
									component6.hitRelay(d);
								}
							}
						}
						else
						{
							int animalHitDirection = animalHealth.GetAnimalHitDirection(num);
							other.transform.SendMessageUpwards("getCombo", 3, SendMessageOptions.DontRequireReceiver);
							other.transform.SendMessageUpwards("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
							AsyncHit.SendPlayerHitEnemy(other.transform, a, d);
							//ModdedPlayer.instance.DoAreaDamage(other.transform.root, (int)num2 * 3);

							if (playerHitEnemy != null)
							{
								playerHitEnemy.getAttackDirection = animalHitDirection;
							}
						}
						if (playerHitEnemy != null)
						{
							playerHitEnemy.Hit = d;
							playerHitEnemy.getCombo = 3;
							rep = a;
						}
						Mood.HitRumble();
						FMODCommon.PlayOneshotNetworked(currentWeaponScript.fleshHitEvent, weaponAudio.transform, FMODCommon.NetworkRole.Any);
					}
					else
					{
						if ((bool)component6)
						{
							for (int i = 0; i < a; i++)
							{
								component6.hitRelay(d);
							}
						}
						else
						{
							int animalHitDirection2 = animalHealth.GetAnimalHitDirection(num);
							other.transform.SendMessageUpwards("ApplyAnimalSkinDamage", animalHitDirection2, SendMessageOptions.DontRequireReceiver);
							AsyncHit.SendPlayerHitEnemy(other.transform, a, d);

							//ModdedPlayer.instance.DoAreaDamage(other.transform.root, (int)num2);

							if (playerHitEnemy != null)
							{
								playerHitEnemy.getAttackDirection = animalHitDirection2;
							}
						}
						Mood.HitRumble();
						if (playerHitEnemy != null)
						{
							playerHitEnemy.Hit = d;
							rep = a;
						}
						FMODCommon.PlayOneshotNetworked(currentWeaponScript.fleshHitEvent, weaponAudio.transform, FMODCommon.NetworkRole.Any);
					}
					setup.pmNoise.SendEvent("toWeaponNoise");
					hitReactions.enableWeaponHitState();
					animControl.hitCombo();
					if (!axe && !rock)
					{
						goto IL_1171;
					}
					if (animator.GetBool("smallAxe"))
					{
						goto IL_1171;
					}
					goto IL_1181;
				}
				goto IL_122e;
			}
			return;
		IL_1941:
			if (chainSaw)
			{
				base.StartCoroutine(chainSawClampRotation(0.5f));
			}
			animEvents.cuttingTree = true;
			animEvents.Invoke("resetCuttingTree", 0.5f);
			if (stick || fireStick)
			{
				other.SendMessage("HitStick", SendMessageOptions.DontRequireReceiver);
				setup.pmNoise.SendEvent("toWeaponNoise");
				animator.SetFloatReflected("weaponHit", 1f);
				PlayEvent(treeHitEvent, null);
				if (BoltNetwork.isRunning && base.entity.isOwner)
				{
					FmodOneShot fmodOneShot = FmodOneShot.Create(GlobalTargets.Others, ReliabilityModes.Unreliable);
					fmodOneShot.Position = base.transform.position;
					fmodOneShot.EventPath = CoopAudioEventDb.FindId(treeHitEvent);
					fmodOneShot.Send();
				}
			}
			else if (!Delay)
			{
				Delay = true;
				base.Invoke("ResetDelay", 0.2f);
				SapDice = Random.Range(0, 5);
				setup.pmNoise.SendEvent("toWeaponNoise");
				if (!noTreeCut)
				{
					if (SapDice == 1)
					{
						PlayerInv.GotSap(null);
					}
					if (other.GetType() == typeof(CapsuleCollider))
					{
						base.StartCoroutine(spawnWoodChips());
					}
					else
					{
						base.StartCoroutine(spawnWoodChips());
					}
					other.SendMessage("Hit", treeDamage, SendMessageOptions.DontRequireReceiver);
					Mood.HitRumble();
				}
				PlayEvent(treeHitEvent, null);
				if (BoltNetwork.isRunning && base.entity.isOwner)
				{
					FmodOneShot fmodOneShot2 = FmodOneShot.Create(GlobalTargets.Others, ReliabilityModes.Unreliable);
					fmodOneShot2.Position = base.transform.position;
					fmodOneShot2.EventPath = CoopAudioEventDb.FindId(treeHitEvent);
					fmodOneShot2.Send();
				}
			}
			goto IL_1b46;
		IL_1181:
			if ((bool)component6)
			{
				component6.getCombo(3);
			}
			else
			{
				other.transform.SendMessageUpwards("getCombo", 3, SendMessageOptions.DontRequireReceiver);
			}
			if (playerHitEnemy != null)
			{
				playerHitEnemy.getCombo = 3;
			}
			goto IL_122e;
		IL_122e:
			if ((other.CompareTag("suitCase") || other.CompareTag("metalProp")) && animControl.smashBool)
			{
				other.transform.SendMessage("Hit", smashDamage, SendMessageOptions.DontRequireReceiver);
				Mood.HitRumble();
				if (playerHitEnemy != null)
				{
					playerHitEnemy.Hit = (int)smashDamage;
				}
				if (BoltNetwork.isRunning && other.CompareTag("suitCase"))
				{
					OpenSuitcase openSuitcase = OpenSuitcase.Create(GlobalTargets.Others);
					openSuitcase.Position = base.GetComponent<Collider>().transform.position;
					openSuitcase.Damage = (int)smashDamage;
					openSuitcase.Send();
				}
				if (smashSoundEnabled)
				{
					smashSoundEnabled = false;
					base.Invoke("EnableSmashSound", 0.3f);
					PlayEvent(smashHitEvent, null);
					if (BoltNetwork.isRunning)
					{
						FmodOneShot fmodOneShot3 = FmodOneShot.Create(GlobalTargets.Others, ReliabilityModes.Unreliable);
						fmodOneShot3.EventPath = CoopAudioEventDb.FindId(smashHitEvent);
						fmodOneShot3.Position = base.transform.position;
						fmodOneShot3.Send();
					}
				}
				setup.pmNoise.SendEvent("toWeaponNoise");
				hitReactions.enableWeaponHitState();
				if (other.CompareTag("metalProp"))
				{
					Rigidbody component7 = other.GetComponent<Rigidbody>();
					if ((bool)component7)
					{
						component7.AddForceAtPosition((Vector3.down + LocalPlayer.Transform.forward * 0.2f) * pushForce * 2f * (0.016666f / Time.fixedDeltaTime), base.transform.position, ForceMode.Force);
					}
				}
			}
			if ((other.CompareTag("enemyCollide") || other.CompareTag("lb_bird") || other.CompareTag("animalCollide") || other.CompareTag("Fish") || other.CompareTag("EnemyBodyPart")) && !mainTrigger && !enemyDelay && (animControl.smashBool || chainSaw))
			{
				float num3 = smashDamage + ModdedPlayer.Stats.meleeFlatDmg + SpellActions.GetParryCounterStrikeDmg();

				if (chainSaw && !mainTrigger)
				{
					base.StartCoroutine(chainSawClampRotation(0.25f));
					num3 = (smashDamage + ModdedPlayer.Stats.meleeFlatDmg + SpellActions.GetParryCounterStrikeDmg()) / 2f;
				}
				float crit = ModdedPlayer.Stats.RandomCritDamage;
				num3 *= crit * ModdedPlayer.Stats.MeleeDamageMult;
				if (PlayerInventoryMod.EquippedModel == BaseItem.WeaponModelType.Hammer)
				{
					num3 *= ModdedPlayer.Stats.i_HammerSmashDamageAmp;
				}

				DamageMath.DamageClamp(num3, out int dmg, out int a);

				base.transform.parent.SendMessage("GotBloody", SendMessageOptions.DontRequireReceiver);
				enemyDelay = true;
				base.Invoke("resetEnemyDelay", 0.25f);
				if ((rock || stick || spear || noBodyCut) && !allowBodyCut)
				{
					other.transform.SendMessageUpwards("ignoreCutting", SendMessageOptions.DontRequireReceiver);
				}
				other.transform.SendMessage("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
				other.transform.SendMessage("hitSuitCase", dmg, SendMessageOptions.DontRequireReceiver);
				other.gameObject.SendMessage("getAttacker", Player, SendMessageOptions.DontRequireReceiver);
				other.gameObject.SendMessage("getAttackerType", 4, SendMessageOptions.DontRequireReceiver);
				if (fsmJumpAttackBool.Value && LocalPlayer.FpCharacter.jumpingTimer > 1.2f && !chainSaw)
				{
					other.transform.SendMessageUpwards("Explosion", -1, SendMessageOptions.DontRequireReceiver);
					if (BoltNetwork.isRunning)
					{
						playerHitEnemy.explosion = true;
					}
				}
				else if (!other.gameObject.CompareTag("Fish"))
				{
					if (other.gameObject.CompareTag("animalCollide"))
					{
						Vector3 vector2 = other.transform.root.GetChild(0).InverseTransformPoint(playerTr.position);
						float targetAngle = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
						int animalHitDirection3 = animalHealth.GetAnimalHitDirection(targetAngle);
						other.transform.SendMessageUpwards("ApplyAnimalSkinDamage", animalHitDirection3, SendMessageOptions.DontRequireReceiver);
						other.transform.SendMessageUpwards("Hit", (int)num3, SendMessageOptions.DontRequireReceiver);
						Mood.HitRumble();
						if (playerHitEnemy != null)
						{
							playerHitEnemy.getAttackDirection = animalHitDirection3;
						}
					}
					else
					{
						other.transform.SendMessageUpwards("getAttackDirection", 3, SendMessageOptions.DontRequireReceiver);

						for (int i = 0; i < a; i++)
						{
							other.transform.SendMessageUpwards("Hit", dmg, SendMessageOptions.DontRequireReceiver);
						}
						Mood.HitRumble();
					}
				}
				else if (other.gameObject.CompareTag("Fish") && !spear)
				{
					other.transform.SendMessage("Hit", num3, SendMessageOptions.DontRequireReceiver);
					Mood.HitRumble();
				}
				if (playerHitEnemy != null)
				{
					playerHitEnemy.getAttackerType = 4;
					playerHitEnemy.Hit = dmg;
					rep = a;
				}
				if (axe)
				{
					other.transform.SendMessageUpwards("HitAxe", SendMessageOptions.DontRequireReceiver);
					if (playerHitEnemy != null)
					{
						playerHitEnemy.HitAxe = true;
					}
				}
				if (other.CompareTag("lb_bird") || other.CompareTag("animalCollide"))
				{
					FMODCommon.PlayOneshotNetworked(animalHitEvent, base.transform, FMODCommon.NetworkRole.Any);
				}
				else if (other.CompareTag("enemyCollide"))
				{
					FMODCommon.PlayOneshotNetworked(fleshHitEvent, base.transform, FMODCommon.NetworkRole.Any);
				}
				else if (other.CompareTag("EnemyBodyPart"))
				{
					FMODCommon.PlayOneshotNetworked(hackBodyEvent, base.transform, FMODCommon.NetworkRole.Any);
					FauxMpHit((int)smashDamage);
				}
				setup.pmNoise.SendEvent("toWeaponNoise");
				hitReactions.enableWeaponHitState();
			}
			if (!mainTrigger && (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock")))
			{
				other.transform.SendMessage("Hit", WeaponDamage, SendMessageOptions.DontRequireReceiver);
				Mood.HitRumble();
				other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, WeaponDamage), SendMessageOptions.DontRequireReceiver);
				FauxMpHit((int)WeaponDamage);
			}
			if (other.CompareTag("lb_bird") && !mainTrigger)
			{
				base.transform.parent.SendMessage("GotBloody", SendMessageOptions.DontRequireReceiver);
				other.transform.SendMessage("Hit", WeaponDamage, SendMessageOptions.DontRequireReceiver);
				Mood.HitRumble();
				FMODCommon.PlayOneshotNetworked(animalHitEvent, base.transform, FMODCommon.NetworkRole.Any);
				setup.pmNoise.SendEvent("toWeaponNoise");
				hitReactions.enableWeaponHitState();
				if (playerHitEnemy != null)
				{
					playerHitEnemy.Hit = (int)WeaponDamage;
				}
			}
			if (other.CompareTag("Tree") && !mainTrigger)
			{
				goto IL_1941;
			}
			if (other.CompareTag("MidTree") && !mainTrigger)
			{
				goto IL_1941;
			}
			goto IL_1b46;
		IL_1171:
			if (fsmHeavyAttackBool.Value)
			{
				goto IL_1181;
			}
			if (!hitReactions.kingHitBool)
			{
				if ((bool)component6)
				{
					component6.getCombo(animControl.combo);
				}
				else
				{
					other.transform.SendMessageUpwards("getCombo", animControl.combo, SendMessageOptions.DontRequireReceiver);
				}
				if (playerHitEnemy != null)
				{
					playerHitEnemy.getCombo = animControl.combo;
				}
			}
			goto IL_122e;
		IL_1b46:
			if (other.gameObject.CompareTag("Rope") && ForestVR.Enabled && mainTrigger)
			{
				setup.pmNoise.SendEvent("toWeaponNoise");
				int num4 = DamageAmount;
				other.SendMessage("Hit", 5, SendMessageOptions.DontRequireReceiver);
				Mood.HitRumble();
				PlayEvent(ropeHitEvent, null);
			}
			if ((other.CompareTag("SmallTree") || other.CompareTag("Rope")) && !mainTrigger)
			{
				setup.pmNoise.SendEvent("toWeaponNoise");
				int integer3 = animator.GetInteger("hitDirection");
				other.transform.SendMessage("getAttackDirection", integer3, SendMessageOptions.DontRequireReceiver);
				int num5 = DamageAmount;
				if (chainSaw || machete)
				{
					num5 *= 5;
				}
				other.SendMessage("Hit", num5, SendMessageOptions.DontRequireReceiver);
				Mood.HitRumble();
				if (chainSaw || machete)
				{
					other.SendMessage("Hit", num5, SendMessageOptions.DontRequireReceiver);
				}
				FauxMpHit(num5);
				if (chainSaw || machete)
				{
					FauxMpHit(num5);
				}
				if (!plantSoundBreak)
				{
					if (other.CompareTag("SmallTree"))
					{
						if (!string.IsNullOrEmpty(plantHitEvent))
						{
							FMODCommon.PlayOneshotNetworked(plantHitEvent, base.transform, FMODCommon.NetworkRole.Any);
						}
					}
					else if (other.CompareTag("Rope"))
					{
						PlayEvent(ropeHitEvent, null);
					}
					plantSoundBreak = true;
					base.Invoke("disablePlantBreak", 0.3f);
				}
				if (other.CompareTag("SmallTree"))
				{
					PlayerInv.GotLeaf();
				}
			}
			if (other.CompareTag("fire") && !mainTrigger && fireStick)
			{
				other.SendMessage("startFire");
			}

			if (playerHitEnemy != null && playerHitEnemy.Target && playerHitEnemy.Hit > 0)
			{
				//ModdedPlayer.instance.DoAreaDamage(other.transform.root, playerHitEnemy.Hit);

				if (ForestVR.Enabled && BoltNetwork.isClient)
				{
					playerHitEnemy.getCombo = Random.Range(2, 4);
				}
				for (int i = 0; i < rep; i++)
				{
					playerHitEnemy.Send();
				}
			}
		}
	}
}