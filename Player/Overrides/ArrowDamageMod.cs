using Bolt;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;

using TheForest.Buildings.Creation;
using TheForest.Tools;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class ArrowDamageMod : ArrowDamage
	{
		private int BaseDmg = -1;
		private float OutputDmg = 0;
		public int Repetitions;
		private bool ignite;
		public Vector3 startposition;

		protected override void Start()
		{
			if (ModSettings.IsDedicated)
			{
				base.Start();
				return;
			}
			if (BaseDmg < 0)
			{
				BaseDmg = damage;
			}
			base.Start();
			OutputDmg = damage + ModdedPlayer.Stats.rangedFlatDmg;

			if (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed)
			{
				if (ModdedPlayer.Stats.Critted)
					OutputDmg *= ModdedPlayer.Stats.projectileSpeed * ((ModdedPlayer.Stats.critDamage ) + 1);
			}
			else
			{
				OutputDmg *= ModdedPlayer.Stats.RandomCritDamage;
			}
			if (GreatBow.isEnabled)
			{
				OutputDmg += 135;
				//dmg *= 2.75f;
			}
			if (crossbowBoltType)
			{
				OutputDmg = OutputDmg * ModdedPlayer.Stats.perk_crossbowDamageMult;
			}
			else if (flintLockAmmoType)
			{
				OutputDmg = OutputDmg * ModdedPlayer.Stats.perk_bulletDamageMult;
			}
			else if (spearType)
			{
				OutputDmg = OutputDmg * ModdedPlayer.Stats.perk_thrownSpearDamageMult;
			}
			else //if arrow
			{
				OutputDmg = OutputDmg * ModdedPlayer.Stats.perk_bowDamageMult;
			}

			if (ModdedPlayer.Stats.spell_seekingArrow)
			{
				startposition = transform.position;
			}
			if (ModdedPlayer.Stats.spell_bia_AccumulatedDamage > 0)
			{
				//if (bloodInfusedMaterial == null)
				//{
				//    bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
				//    {
				//        EmissionColor = new Color(0.6f, 0, 0),
				//        renderMode = BuilderCore.BuildingData.RenderMode.Fade,
				//        MainColor = Color.red,
				//        Metalic = 0.5f,
				//        Smoothness = 0.6f,
				//    });
				//}
				OutputDmg += ModdedPlayer.Stats.spell_bia_AccumulatedDamage;
				if (ModdedPlayer.Stats.i_HazardCrown)
				{
					if (ModdedPlayer.Stats.i_HazardCrownBonus > 0)
					{
						ModdedPlayer.Stats.i_HazardCrownBonus.Substract(1);
					}
					else
						ModdedPlayer.Stats.spell_bia_AccumulatedDamage.valueAdditive = 0;
				}
				else
				{
					ModdedPlayer.Stats.spell_bia_AccumulatedDamage.Reset();
				}
			}
			ignite = BlackFlame.IsOn;
			//removing this line crashes the game when firing a ranged weapon
			damage = damage;
		}

		public static Material bloodInfusedMaterial;

		protected override void OnTriggerEnter(Collider other)
		{
			base.OnTriggerEnter(other);
		}

		private void Update()
		{
			if (ModdedPlayer.Stats.spell_seekingArrow && Live)
			{
				if (Time.time - ModdedPlayer.Stats.spell_seekingArrowDuration > SpellActions.SeekingArrow_TimeStamp)
				{
					ModdedPlayer.Stats.spell_seekingArrow.value = false;
				}
				if ((transform.position - SpellActions.SeekingArrow_Target.position).sqrMagnitude > 3)
				{
					Vector3 vel = PhysicBody.velocity;
					Vector3 targetvel = SpellActions.SeekingArrow_Target.position - transform.position;
					targetvel.Normalize();
					targetvel *= vel.magnitude;
					PhysicBody.velocity = Vector3.RotateTowards(PhysicBody.velocity, targetvel, Time.deltaTime * 2.3f * ModdedPlayer.Stats.projectileSpeed, 0.25f);
				}
			}
		}

		public override void CheckHit(Vector3 position, Transform target, bool isTrigger, Collider targetCollider)
		{
			if (ignoreCollisionEvents(targetCollider) && !target.CompareTag("enemyRoot"))
			{
				return;
			}
			if (!isTrigger)
			{
				Molotov componentInParent = transform.GetComponentInParent<Molotov>();
				if ((bool)componentInParent)
				{
					componentInParent.IncendiaryBreak();
				}
			}
			bool headDamage = false;
			if (target.gameObject.layer == LayerMask.NameToLayer("Water"))
			{
				FMODCommon.PlayOneshotNetworked(hitWaterEvent, base.transform, FMODCommon.NetworkRole.Any);
			}
			else if (target.CompareTag("SmallTree"))
			{
				FMODCommon.PlayOneshotNetworked(hitBushEvent, base.transform, FMODCommon.NetworkRole.Any);
			}
			if (target.CompareTag("PlaneHull"))
			{
				FMODCommon.PlayOneshotNetworked(hitMetalEvent, base.transform, FMODCommon.NetworkRole.Any);
			}
			if (target.CompareTag("Tree") || target.root.CompareTag("Tree") || target.CompareTag("Target"))
			{
				if (spearType)
				{
					base.StartCoroutine(HitTree(hit.point - base.transform.forward * 2.1f));
				}
				else if (hitPointUpdated)
				{
					base.StartCoroutine(HitTree(hit.point - base.transform.forward * 0.35f));
				}
				else
				{
					base.StartCoroutine(HitTree(base.transform.position - base.transform.forward * 0.35f));
				}
				disableLive();
				if (target.CompareTag("Tree") || target.root.CompareTag("Tree"))
				{
					TreeHealth component = target.GetComponent<TreeHealth>();
					if (!(bool)component)
					{
						component = target.root.GetComponent<TreeHealth>();
					}
					if ((bool)component)
					{
						component.LodTree.AddTreeCutDownTarget(base.gameObject);
					}
				}
			}
			else if (target.CompareTag("enemyCollide") || target.tag == "lb_bird" || target.CompareTag("animalCollide") || target.CompareTag("Fish") || target.CompareTag("enemyRoot") || target.CompareTag("animalRoot"))
			{
				if (crossbowBoltType)
				{
				}
				else if (flintLockAmmoType)
				{
				}
				else if (spearType)
				{
				}
				else
				{
					if (ModdedPlayer.Stats.i_CrossfireQuiver.value)
					{
						if (Time.time - ModdedPlayer.instance._lastCrossfireTime > 10)
						{
							ModdedPlayer.instance._lastCrossfireTime = Time.time;
							float damage1 = 55 + ModdedPlayer.Stats.spellFlatDmg * 1.25f;
							damage1 = damage1 * ModdedPlayer.Stats.TotalMagicDamageMultiplier;
							Vector3 pos = Camera.main.transform.position + Camera.main.transform.right * 5;
							Vector3 dir = transform.position - pos;
							dir.Normalize();
							if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
							{
								MagicArrow.Create(pos, dir, damage1, ModReferences.ThisPlayerID, ModdedPlayer.Stats.spell_magicArrowDuration, ModdedPlayer.Stats.spell_magicArrowDoubleSlow, ModdedPlayer.Stats.spell_magicArrowDmgDebuff);
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
											w.Write(damage1);
											w.Write(ModReferences.ThisPlayerID);
											w.Write(ModdedPlayer.Stats.spell_magicArrowDuration);
											w.Write(ModdedPlayer.Stats.spell_magicArrowDoubleSlow);
											w.Write(ModdedPlayer.Stats.spell_magicArrowDmgDebuff);

											w.Close();
										}
										ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
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
										w.Write(damage1);
										w.Write(ModReferences.ThisPlayerID);
										w.Write(ModdedPlayer.Stats.spell_magicArrowDuration);
										w.Write(ModdedPlayer.Stats.spell_magicArrowDoubleSlow);
										w.Write(ModdedPlayer.Stats.spell_magicArrowDmgDebuff);

										w.Close();
									}
									ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
									answerStream.Close();
								}
							}
						}
					}
				}
				bool flag = target.tag == "lb_bird" || target.CompareTag("lb_bird");
				bool flag2 = target.CompareTag("Fish");
				bool flag3 = target.CompareTag("animalCollide") || target.CompareTag("animalRoot");
				arrowStickToTarget arrowStickToTarget = target.GetComponent<arrowStickToTarget>();
				if (!(bool)arrowStickToTarget)
				{
					arrowStickToTarget = target.root.GetComponentInChildren<arrowStickToTarget>();
				}
				if (!spearType && !flintLockAmmoType && !flag2)
				{
					if (arrowStickToTarget && arrowStickToTarget.enabled)
					{
						if (flag)
						{
							EventRegistry.Achievements.Publish(TfEvent.Achievements.BirdArrowKill, null);
						}
						arrowStickToTarget.CreatureType(flag3, flag, flag2);
						if (SpellActions.SeekingArrow_ChangeTargetOnHit)
							startposition = transform.position;
						if (BoltNetwork.isRunning)
						{
							if (at && at._boltEntity && at._boltEntity.isAttached && at._boltEntity.isOwner)
							{
								headDamage = arrowStickToTarget.stickArrowToNearestBone(base.transform);
							}
						}
						else
						{
							headDamage = arrowStickToTarget.stickArrowToNearestBone(base.transform);
						}
					}
					if ((bool)arrowStickToTarget)
					{
						base.Invoke("destroyMe", 0.1f);
					}
				}
				else
				{
					if (SpellActions.SeekingArrow_ChangeTargetOnHit)
					{
						ModdedPlayer.Stats.spell_seekingArrow.value = true;
						SpellActions.SeekingArrow_Target.gameObject.SetActive(true);
						SpellActions.SeekingArrow_Target.parent = target.transform;
						SpellActions.SeekingArrow_Target.position = new Vector3(target.transform.position.x, transform.position.y - 0.075f, target.transform.position.z);
						SpellActions.SeekingArrow_TimeStamp = Time.time;
						startposition = transform.position;
						SpellActions.SeekingArrow_ChangeTargetOnHit = false;
					}
				}
				if (headDamage && !flintLockAmmoType && ModdedPlayer.Stats.perk_trueAim && ModdedPlayer.Stats.spell_seekingArrow)
				{
					float dist = (startposition - transform.position).sqrMagnitude;
					if (dist >= 3600f)
					{
						OutputDmg *= 4;
						NewHitAi(target, flag || flag3, headDamage);
						ModdedPlayer.instance.DoAreaDamage(target.root, OutputDmg);
						ModdedPlayer.instance.OnHit();
						ModdedPlayer.instance.OnHit_Ranged(target);
						if (ModdedPlayer.Stats.perk_trueAim && dist >= 14400f)
						{
							OutputDmg *= 10;

							NewHitAi(target, flag || flag3, headDamage);
							NewHitAi(target, flag || flag3, headDamage);
							ModdedPlayer.instance.DoAreaDamage(target.root, OutputDmg);
							ModdedPlayer.instance.OnHit();
							ModdedPlayer.instance.OnHit_Ranged(target);
							ModdedPlayer.instance.DoAreaDamage(target.root, OutputDmg);
							ModdedPlayer.instance.OnHit();
							ModdedPlayer.instance.OnHit_Ranged(target);
						}
					}
				}

				NewHitAi(target, flag || flag3, headDamage);
				ModdedPlayer.instance.DoAreaDamage(target.root, OutputDmg);
				ModdedPlayer.instance.OnHit();
				ModdedPlayer.instance.OnHit_Ranged(target);
				BoltEntity be = target.GetComponentInParent<BoltEntity>();
				if (be == null)
				{
					be = target.GetComponent<BoltEntity>();
				}

				if (ModdedPlayer.Stats.perk_fireDmgIncreaseOnHit)
				{
					int myID = 1000 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
					float dmg = 1 + ModdedPlayer.Stats.spellFlatDmg / 3;
					dmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
					dmg *= ModdedPlayer.Stats.fireDamage + 1;
					dmg *= 0.3f;
					if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
					{
						target.GetComponentInParent<EnemyProgression>()?.FireDebuff(myID, dmg, 14);
					}
					else
					{
						if (be != null)
						{
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(27);
									w.Write(be.networkId.PackedValue);
									w.Write(dmg);
									w.Write(14.5f);
									w.Write(1);
									w.Close();
								}
								ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
								answerStream.Close();
							}
						}
					}
				}
				if (ModdedPlayer.Stats.rangedArmorPiercing > 0 && target.gameObject.CompareTag("enemyCollide"))
				{
					if (ModdedPlayer.Stats.perk_thrownSpearExtraArmorReduction && spearType)
					{
						if (BoltNetwork.isClient)
						{
							if (be != null)
							{
								EnemyProgression.ReduceArmor(be, ModdedPlayer.Stats.TotalRangedArmorPiercing * 2 + ModdedPlayer.Stats.TotalMeleeArmorPiercing);
							}
						}
						else
							target.transform.SendMessageUpwards("ReduceArmor", ModdedPlayer.Stats.TotalRangedArmorPiercing*2 + ModdedPlayer.Stats.TotalMeleeArmorPiercing, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
					if (BoltNetwork.isClient)
					{
						if (be != null)
						{
							EnemyProgression.ReduceArmor(be, ModdedPlayer.Stats.TotalRangedArmorPiercing );
						}
					}
					else
					{
						target.transform.SendMessageUpwards("ReduceArmor", ModdedPlayer.Stats.TotalRangedArmorPiercing, SendMessageOptions.DontRequireReceiver);
					}
					}
				}
				if (flag2)
				{
					base.StartCoroutine(HitFish(target, hit.point - base.transform.forward * 0.35f));
				}
				disableLive();
			}
			else if (target.CompareTag("PlayerNet"))
			{
				if (BoltNetwork.isRunning)
				{
					BoltEntity be = target.GetComponentInParent<BoltEntity>();
					if (!(bool)be)
					{
						be = target.GetComponent<BoltEntity>();
					}

					if (be)
					{
						if (ModdedPlayer.Stats.i_ArchangelBow && GreatBow.isEnabled)
						{
							float lifePerSecond = (ModdedPlayer.Stats.healthRecoveryPerSecond) * ModdedPlayer.Stats.allRecoveryMult * (ModdedPlayer.Stats.healthPerSecRate) * 2;
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(39);
									w.Write(be.GetState<IPlayerState>().name);
									w.Write(lifePerSecond);
									w.Write(ModdedPlayer.Stats.TotalMaxHealth * 0.2f);
									w.Close();
								}
								AsyncHit.SendCommandDelayed(1, answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
								answerStream.Close();
							}
							BuffDB.AddBuff(25, 91, lifePerSecond, 10);

						}
						else if (ModSettings.FriendlyFire)
						{
							float dmgUnclamped = this.OutputDmg;
							if (ModdedPlayer.Stats.spell_seekingArrow)
							{
								float dist = Vector3.Distance(target.position, startposition);
								dmgUnclamped *= 1 + dist * ModdedPlayer.Stats.spell_seekingArrow_DamagePerDistance;
							}
							if (spearType)
							{
								if (ModdedPlayer.Stats.perk_thrownSpearhellChance > 0 && Random.value <= ModdedPlayer.Stats.perk_thrownSpearhellChance && OutputDmg > 1)
								{
									var obj = Instantiate(PhysicBody, Camera.main.transform.position + Vector3.up * 2f, Quaternion.LookRotation(Camera.main.transform.forward));
									obj.velocity = PhysicBody.velocity * 1.05f;
									Destroy(obj.gameObject, 30);
								}
							}
							else
							{
								if (headDamage)
								{
									if (ModdedPlayer.Stats.i_EruptionBow && GreatBow.isEnabled)
									{
										//if (GameSetup.IsMultiplayer)
										//{
										BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position, Quaternion.identity);
										//}
									}
								}
							}

							if (headDamage || (flintLockAmmoType && Random.value <= ModdedPlayer.Stats.perk_bulletCritChance) || (spearType && Random.value <= ModdedPlayer.Stats.perk_thrownSpearhellChance))
							{
								headDamage = true;
								dmgUnclamped *= ModdedPlayer.Stats.headShotDamage;
								dmgUnclamped *= SpellActions.FocusOnHeadShot();
								if (ModdedPlayer.Stats.spell_seekingArrow)
								{
									dmgUnclamped *= ModdedPlayer.Stats.spell_seekingArrow_HeadDamage;
								}
							}
							else
							{
								dmgUnclamped *= SpellActions.FocusOnBodyShot();
							}
							if (GreatBow.isEnabled)
								dmgUnclamped *= 1.6f;
							dmgUnclamped *= ModdedPlayer.Stats.RangedDamageMult ;
							if (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize)
								dmgUnclamped *= ModdedPlayer.Stats.projectileSize;
							if (!ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed && ModdedPlayer.Stats.Critted)
								dmgUnclamped *= ModdedPlayer.Stats.critChance + 1;
							DamageMath.DamageClamp(dmgUnclamped, out int sendDamage, out Repetitions);

							HitPlayer HP = HitPlayer.Create(be, EntityTargets.Everyone);
							HP.damage = sendDamage;
							for (int i = 0; i < Repetitions; i++)
							{
								HP.Send();
							}
							disableLive();
						}
					}
				}
			}
			else if (target.CompareTag("TerrainMain") && !LocalPlayer.IsInCaves)
			{
				if (ignoreTerrain)
				{
					ignoreTerrain = false;
					base.StartCoroutine(RevokeIgnoreTerrain());
				}
				else
				{
					if (spearType)
					{
						if ((bool)bodyCollider)
						{
							bodyCollider.isTrigger = true;
						}
						base.StartCoroutine(HitStructure(base.transform.position - base.transform.forward * 2.1f, false));
					}
					else
					{
						Vector3 position2 = base.transform.position - base.transform.forward * -0.8f;
						float num = Terrain.activeTerrain.SampleHeight(base.transform.position);
						Vector3 position3 = Terrain.activeTerrain.transform.position;
						float num2 = num + position3.y;
						Vector3 position4 = base.transform.position;
						if (position4.y < num2)
						{
							position2.y = num2 + 0.5f;
						}
						base.StartCoroutine(HitStructure(position2, false));
					}
					disableLive();
					FMODCommon.PlayOneshotNetworked(hitGroundEvent, base.transform, FMODCommon.NetworkRole.Any);
				}
			}
			else if (target.CompareTag("structure") || target.CompareTag("jumpObject") || target.CompareTag("SLTier1") || target.CompareTag("SLTier2") || target.CompareTag("SLTier3") || target.CompareTag("UnderfootWood"))
			{
				if ((bool)target.transform.parent)
				{
					if ((bool)target.transform.parent.GetComponent<StickFenceChunkArchitect>())
					{
						return;
					}
					if ((bool)target.transform.parent.GetComponent<BoneFenceChunkArchitect>())
					{
						return;
					}
				}
				if (!isTrigger)
				{
					if (spearType)
					{
						base.StartCoroutine(HitStructure(hit.point - base.transform.forward * 2.1f, true));
					}
					else
					{
						base.StartCoroutine(HitStructure(hit.point - base.transform.forward * 0.35f, true));
					}
					disableLive();
				}
			}
			else if (target.CompareTag("CaveDoor"))
			{
				ignoreTerrain = true;
				Physics.IgnoreCollision(base.GetComponent<Collider>(), Terrain.activeTerrain.GetComponent<Collider>(), true);
			}
			else if (flintLockAmmoType && (target.CompareTag("BreakableWood") || target.CompareTag("BreakableRock")))
			{
				target.SendMessage("Hit", 40, SendMessageOptions.DontRequireReceiver);
			}
			if (!Live)
			{
				destroyThisAmmo();
				parent.BroadcastMessage("OnArrowHit", SendMessageOptions.DontRequireReceiver);
			}
		}

		private void NewHitAi(Transform target, bool hitDelay, bool headDamage)
		{
			float dmgUnclamped = this.OutputDmg;
			if (ModdedPlayer.Stats.spell_seekingArrow)
			{
				float dist = Vector3.Distance(target.position, startposition);
				dmgUnclamped *= 1 + dist * ModdedPlayer.Stats.spell_seekingArrow_DamagePerDistance;
			}
			if (spearType)
			{
				if (ModdedPlayer.Stats.perk_thrownSpearhellChance > 0 && Random.value <= ModdedPlayer.Stats.perk_thrownSpearhellChance && OutputDmg > 1)
				{
					var obj = Instantiate(PhysicBody, Camera.main.transform.position + Vector3.up * 2f, Quaternion.LookRotation(Camera.main.transform.forward));
					obj.velocity = PhysicBody.velocity * 1.05f;
					Destroy(obj.gameObject, 30);
				}
			}
			else
			{
				if (headDamage)
				{
					if (ModdedPlayer.Stats.i_EruptionBow && GreatBow.isEnabled)
					{
						//if (GameSetup.IsMultiplayer)
						//{
						BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position, Quaternion.identity);
						//}
					}
				}
			}

			if (headDamage || (flintLockAmmoType && Random.value <= ModdedPlayer.Stats.perk_bulletCritChance) || (spearType && Random.value <= ModdedPlayer.Stats.perk_thrownSpearhellChance))
			{
				headDamage = true;
				dmgUnclamped *= ModdedPlayer.Stats.headShotDamage;
				dmgUnclamped *= SpellActions.FocusOnHeadShot();
				if (ModdedPlayer.Stats.spell_seekingArrow)
				{
					dmgUnclamped *= ModdedPlayer.Stats.spell_seekingArrow_HeadDamage;
				}
			}
			else
			{
				dmgUnclamped *= SpellActions.FocusOnBodyShot();
			}
			if (GreatBow.isEnabled)
				dmgUnclamped *= 1.6f;
			dmgUnclamped *= ModdedPlayer.Stats.RangedDamageMult;
			if (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize)
				dmgUnclamped *= ModdedPlayer.Stats.projectileSize;
			if (!ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed && ModdedPlayer.Stats.Critted)
				dmgUnclamped *= ModdedPlayer.Stats.critChance + 1;
			DamageMath.DamageClamp(dmgUnclamped, out int sendDamage, out Repetitions);

			if (this.PhysicBody)
			{
				this.PhysicBody.velocity = Vector3.zero;
			}

			if (this.spearType)
			{
				this.PhysicBody.isKinematic = false;
				this.PhysicBody.useGravity = true;
				this.disableLive();
				if (this.MyPickUp)
				{
					this.MyPickUp.SetActive(true);
				}
			}
			if (target)
			{
				Vector3 vector = target.transform.root.GetChild(0).InverseTransformPoint(base.transform.position);
				float targetAngle = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				int animalHitDirection = animalHealth.GetAnimalHitDirection(targetAngle);
				BoltEntity componentInParent = target.GetComponentInParent<BoltEntity>();
				if (!componentInParent)
				{
					componentInParent = target.GetComponent<BoltEntity>();
				}

				if (BoltNetwork.isClient && componentInParent)
				{
					ModdedPlayer.instance.OnHitEffectsClient(componentInParent, dmgUnclamped);
					if (ignite)
					{
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(27);
								w.Write(componentInParent.networkId.PackedValue);
								w.Write(Effects.BlackFlame.FireDamageBonus);
								w.Write(20f);
								w.Write(2200);
								w.Close();
							}
							AsyncHit.SendCommandDelayed(3, answerStream.ToArray(), NetworkManager.Target.OnlyServer);
							answerStream.Close();
						}
					}
					if (ModdedPlayer.Stats.spell_focus && headDamage)
					{
						if (ModdedPlayer.Stats.spell_focusBonusDmg == 0)
						{
							//slow enemy by 80%
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(22);
									w.Write(componentInParent.networkId.PackedValue);
									w.Write(ModdedPlayer.Stats.spell_focusSlowAmount);
									w.Write(ModdedPlayer.Stats.spell_focusSlowDuration);
									w.Write(90);
									w.Close();
								}
								AsyncHit.SendCommandDelayed(1, answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
								answerStream.Close();
							}
							//Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.OnlyServer);
						}
					}
					else if (ModdedPlayer.Stats.spell_seekingArrow)
					{
						//slow enemy by 80%
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(22);
								w.Write(componentInParent.networkId.PackedValue);
								w.Write(ModdedPlayer.Stats.spell_seekingArrow_SlowAmount);
								w.Write(ModdedPlayer.Stats.spell_seekingArrow_SlowDuration);
								w.Write(91);
								w.Close();
							}
							AsyncHit.SendCommandDelayed(2, answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
							answerStream.Close();
						}
					}
					if (ignite)
					{
						if (BlackFlame.GiveAfterburn && Random.value < 0.1f)
						{
							int id = 120 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(34);
									w.Write(componentInParent.networkId.PackedValue);
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
					if (hitDelay)
					{
						target.transform.SendMessageUpwards("getClientHitDirection", 6, SendMessageOptions.DontRequireReceiver);
						target.transform.SendMessageUpwards("StartPrediction", SendMessageOptions.DontRequireReceiver);
						BoltEntity component = this.parent.GetComponent<BoltEntity>();
						PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
						playerHitEnemy.Target = componentInParent;
						playerHitEnemy.Weapon = component;
						playerHitEnemy.getAttacker = 10;
						if (target.gameObject.CompareTag("animalRoot"))
						{
							playerHitEnemy.getAttackDirection = animalHitDirection;
						}
						else
						{
							playerHitEnemy.getAttackDirection = 3;
						}
						playerHitEnemy.getAttackerType = 4;
						playerHitEnemy.Hit = sendDamage;
						if ((GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites) || (ignite && Random.value < 0.5f))
							playerHitEnemy.Burn = true;
						AsyncHit.SendPlayerHitEnemy(playerHitEnemy, Repetitions);
					}
					else
					{
						target.transform.SendMessageUpwards("getClientHitDirection", 6, SendMessageOptions.DontRequireReceiver);
						target.transform.SendMessageUpwards("StartPrediction", SendMessageOptions.DontRequireReceiver);
						PlayerHitEnemy playerHitEnemy2 = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
						playerHitEnemy2.Target = componentInParent;
						if (target.gameObject.CompareTag("animalRoot"))
						{
							playerHitEnemy2.getAttackDirection = animalHitDirection;
						}
						else
						{
							playerHitEnemy2.getAttackDirection = 3;
						}
						playerHitEnemy2.getAttackerType = 4;
						if ((ignite && Random.value < 0.5f) || GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites)
							playerHitEnemy2.Burn = true;
						playerHitEnemy2.Hit = sendDamage;
						AsyncHit.SendPlayerHitEnemy(playerHitEnemy2, Repetitions);
					}
				}
				else
				{
					if (target.gameObject.CompareTag("enemyRoot") || target.gameObject.CompareTag("enemyCollide"))
					{
						var ep = target.gameObject.GetComponentInParent<EnemyProgression>();
						if (ep == null)
						{
							ep = target.gameObject.GetComponent<EnemyProgression>();
							if (ep == null)
							{
								ep = target.gameObject.GetComponentInChildren<EnemyProgression>();
							}
						}
						if (ignite)
						{
							ep.FireDebuff(2200, Effects.BlackFlame.FireDamageBonus, 20);
							if (BlackFlame.GiveAfterburn && Random.value < 0.1f)
							{
								if (ep != null)
								{
									int id = 120 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
									ep.DmgTakenDebuff(id, 1.15f, 25);
								}
							}
						}
						ModdedPlayer.instance.OnHitEffectsHost(ep, dmgUnclamped);
						if (ModdedPlayer.Stats.spell_focus && headDamage)
						{
							if (ModdedPlayer.Stats.spell_focusBonusDmg == 0)
							{
								//slow enemy by 80%
								ep.Slow(90, ModdedPlayer.Stats.spell_focusSlowAmount, ModdedPlayer.Stats.spell_focusSlowDuration);
							}
						}
						else if (ModdedPlayer.Stats.spell_seekingArrow)
						{
							ep.Slow(91, ModdedPlayer.Stats.spell_seekingArrow_SlowAmount, ModdedPlayer.Stats.spell_seekingArrow_SlowDuration);
						}
					}
					target.gameObject.SendMessageUpwards("getAttackDirection", 3, SendMessageOptions.DontRequireReceiver);
					target.gameObject.SendMessageUpwards("getAttackerType", 4, SendMessageOptions.DontRequireReceiver);
					GameObject closestPlayerFromPos = TheForest.Utils.Scene.SceneTracker.GetClosestPlayerFromPos(base.transform.position);
					target.gameObject.SendMessageUpwards("getAttacker", closestPlayerFromPos, SendMessageOptions.DontRequireReceiver);
					if (target.gameObject.CompareTag("lb_bird") || target.gameObject.CompareTag("animalRoot") || target.gameObject.CompareTag("enemyRoot") || target.gameObject.CompareTag("PlayerNet"))
					{
						if (target.gameObject.CompareTag("enemyRoot"))
						{
							EnemyHealth componentInChildren = target.GetComponentInChildren<EnemyHealth>();
							if (componentInChildren)
							{
								componentInChildren.getAttackDirection(3);
								componentInChildren.setSkinDamage(2);
								mutantTargetSwitching componentInChildren2 = target.GetComponentInChildren<mutantTargetSwitching>();
								if (componentInChildren2)
								{
									componentInChildren2.getAttackerType(4);
									componentInChildren2.getAttacker(closestPlayerFromPos);
								}
								for (int i = 0; i < Repetitions; i++)
								{
									componentInChildren.Hit(sendDamage);
								}
							if ((ignite && Random.value < 0.5f) || GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites)
								componentInChildren.Burn();
							}
						}
						else
						{
							if (target.gameObject.CompareTag("animalRoot"))
							{
								Repetitions = 1;
								target.gameObject.SendMessage("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
							}
							AsyncHit.SendPlayerHitEnemy(target, Repetitions, sendDamage);
							if ((ignite && Random.value < 0.5f) || GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites)
								target.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
							target.gameObject.SendMessage("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						if (target.gameObject.CompareTag("animalCollide"))
						{
							Repetitions = 1;
							target.gameObject.SendMessageUpwards("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
						}
						AsyncHit.SendPlayerHitEnemy(target, Repetitions, sendDamage);
						if (GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites || (ignite && Random.value < 0.5f))
							target.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
						target.gameObject.SendMessageUpwards("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			if (this.MyPickUp)
			{
				if (ModdedPlayer.Stats.perk_projectileNoConsumeChance >= 1)
				{
					Destroy(this.gameObject);
				}
				else
				{
				this.MyPickUp.SetActive(true);
				
				}
			}
			if (ModdedPlayer.Stats.perk_projectileNoConsumeChance > 0.35f)
			{
				FMODCommon.PlayOneshotNetworked(this.hitAiEvent, base.transform, FMODCommon.NetworkRole.Any);
			}
		}
	}
}