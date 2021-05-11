using System.Collections.Generic;

using Bolt;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;

using TheForest.Buildings.Creation;
using TheForest.Tools;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class XBArrowDamageMod : ArrowDamage
	{
		private float OutputDmg = 0;
		private float dmgUnclamped = 1;
		private float damageMult = 0;
		private bool ignite;
		private int pierceCount = 0;
		private bool crit;
		private Transform lastPierced;
		public Vector3 startposition;
		protected override void Start()
		{
			base.Start();
			pierceCount = 0;
			
			//flat damage part
			OutputDmg = damage + ModdedPlayer.Stats.rangedFlatDmg;
			if (ModdedPlayer.Stats.spell_seekingArrow)
			{
				OutputDmg += ModdedPlayer.Stats.spell_seekingArrow_DamageBonus.Value;
			}
			
			if (GreatBow.isEnabled)
			{
				OutputDmg += 140;
				OutputDmg *= 1.75f;
			}

			//damage multiplication
			if (ModdedPlayer.Stats.spell_bia_AccumulatedDamage > 0)
			{
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
			if (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed)
			{
				crit = ModdedPlayer.Stats.Critted;
				if (crit)
				{
					OutputDmg *= (ModdedPlayer.Stats.critDamage) + (ModdedPlayer.Stats.projectileSize - 1) * 2.5f + 1;
				}
			}
			else
			{
				OutputDmg *= ModdedPlayer.Stats.RandomCritDamage;
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
			startposition = transform.position;
			OutputDmg *= ModdedPlayer.Stats.RangedDamageMult;
			if(damageMult != 0f)
			OutputDmg *= damageMult;
			ignite = BlackFlame.IsOn;
		}

		public static Material bloodInfusedMaterial;

		//protected override void OnTriggerEnter(Collider other)
		//{
		//	base.OnTriggerEnter(other);
		//}
		
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
					PhysicBody.velocity = Vector3.RotateTowards(PhysicBody.velocity, targetvel, Time.deltaTime * 2.34f * ModdedPlayer.Stats.projectileSpeed, 0.25f);
				}
			}
		}
		void ForgetLastPierced()
		{
			lastPierced = null;
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
					return;
				}
			}
			bool headDamage = false;
			if (target.gameObject.layer == LayerMask.NameToLayer("Water"))
			{
				FMODCommon.PlayOneshotNetworked(hitWaterEvent, base.transform, FMODCommon.NetworkRole.Any);
				return;
			}
			else if (target.CompareTag("SmallTree"))
			{
				FMODCommon.PlayOneshotNetworked(hitBushEvent, base.transform, FMODCommon.NetworkRole.Any);
				return;

			}
			else if (target.CompareTag("PlaneHull"))
			{
				FMODCommon.PlayOneshotNetworked(hitMetalEvent, base.transform, FMODCommon.NetworkRole.Any);
				return;

			}
			else if (target.CompareTag("Tree") || target.root.CompareTag("Tree") || target.CompareTag("Target"))
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
				return;
			}
			else if (target.CompareTag("enemyCollide") || target.tag == "lb_bird" || target.CompareTag("animalCollide") || target.CompareTag("Fish") || target.CompareTag("enemyRoot") || target.CompareTag("animalRoot"))
			{
				if (lastPierced != null)
				{
					if (target.root == lastPierced)
					{
						Physics.IgnoreCollision(base.GetComponent<Collider>(), targetCollider);
						return;
					}
				}
				bool pierce = false;
				float pierceChance = ModdedPlayer.Stats.projectilePierceChance - pierceCount;
				if (pierceChance > 0)
				{
					if (pierceChance >= 1 || pierceChance < Random.value)
					{
						Physics.IgnoreCollision(base.GetComponent<Collider>(), targetCollider);
						pierceCount++;
						lastPierced = target.root;
						Invoke("ForgetLastPierced", 0.125f);
						pierce = true;
					}
				}
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
						if (Time.time - ModdedPlayer.instance._lastCrossfireTime > 2*ModdedPlayer.Stats.cooldown)
						{
							ModdedPlayer.instance._lastCrossfireTime = Time.time;
							Vector3 pos = Camera.main.transform.position + Camera.main.transform.right * 5;
							Vector3 dir = transform.position - pos;
							dir.Normalize();
							SpellActions.CastMagicArrow(pos,dir);
						}
					}
				}
			
				arrowStickToTarget arrowStickToTarget = target.GetComponent<arrowStickToTarget>();
				if (!(bool)arrowStickToTarget)
				{
					arrowStickToTarget = target.root.GetComponentInChildren<arrowStickToTarget>();
				}

				bool isbird = target.tag == "lb_bird" || target.CompareTag("lb_bird");
				bool isfish = target.CompareTag("Fish");
				bool isanimal = target.CompareTag("animalCollide") || target.CompareTag("animalRoot");
				if (!spearType && !flintLockAmmoType && !isfish)
				{
					if (arrowStickToTarget && arrowStickToTarget.enabled)
					{
						if (isbird)
						{
							EventRegistry.Achievements.Publish(TfEvent.Achievements.BirdArrowKill, null);
						}
						arrowStickToTarget.CreatureType(isanimal, isbird, isfish);
					
						if (BoltNetwork.isRunning)
						{
							if (at && at._boltEntity && at._boltEntity.isAttached && at._boltEntity.isOwner)
							{
								if (pierce)
									headDamage = ((XArrowStickToTargetMod)arrowStickToTarget).checkHeadDamage(transform);
								else
									headDamage = arrowStickToTarget.stickArrowToNearestBone(base.transform);
							}
						}
						else
						{
							if (pierce)
								headDamage = ((XArrowStickToTargetMod)arrowStickToTarget).checkHeadDamage(transform);
							headDamage = arrowStickToTarget.stickArrowToNearestBone(base.transform);
						}
					}
					if ((bool)arrowStickToTarget&& !pierce)
					{
						Destroy(parent.gameObject);
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
					//True aim ability
					float dist = (startposition - transform.position).sqrMagnitude;
					int extraHitCount = 0;
					if (dist >= 3600f)
					{
						OutputDmg *= 1.5f;
						extraHitCount = 1;
						if (ModdedPlayer.Stats.perk_trueAimUpgrade && dist >= 14400f)
						{
							OutputDmg *= 5;
							extraHitCount += 2;
						}
					}
					//UnityEngine.Events.UnityAction action = new UnityEngine.Events.UnityAction(() => this.NewHitAi(target, false, headDamage));
					//async hit with a frame of delay between invocations
					//RCoroutines.i.StartCoroutine(RCoroutines.i.ProjectileMultihit(target, OutputDmg, headDamage, action, extraHitCount));
					for (int i = 0; i < extraHitCount; i++)
					{
						this.NewHitAi(target, false, headDamage);
					}
				}
			


				NewHitAi(target, isbird || isanimal, headDamage);
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
				if (ModdedPlayer.Stats.TotalRangedArmorPiercing > 0 && target.gameObject.CompareTag("enemyCollide"))
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
						else if (EnemyManager.enemyByTransform.ContainsKey(target.root))
						{
							var prog = EnemyManager.enemyByTransform[target.root];
							prog.ReduceArmor( ModdedPlayer.Stats.TotalRangedArmorPiercing*2 + ModdedPlayer.Stats.TotalMeleeArmorPiercing);
						}
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
						else if (EnemyManager.enemyByTransform.ContainsKey(target.root))
						{
							var prog = EnemyManager.enemyByTransform[target.root];
							prog.ReduceArmor(ModdedPlayer.Stats.TotalRangedArmorPiercing);
						}
					}
				}
				if (isfish)
				{
					base.StartCoroutine(HitFish(target, hit.point - base.transform.forward * 0.35f));
				}
				//check piercing
				
					if (pierce)
					{
						return;
					}
				Live = false;
				disableLive();
				DisableFlight();

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
							ChampionsOfForest.COTFEvents.Instance.OnFriendlyFire.Invoke();

							{
								//checking if headshot
								var head = target.root.Find("head");
								if (head == null)
									Debug.Log("Player has no head");
								else
								{
									
									if ((transform.position-head.position).sqrMagnitude < 0.4f)
									headDamage = true;
								}
							}


							float dmgUnclamped = this.OutputDmg;
							float dist = Vector3.Distance(target.position, startposition)/3.28f;
							dmgUnclamped *= 1 + dist * ModdedPlayer.Stats.projectile_DamagePerDistance;
							if (spearType)
							{
								if (ModdedPlayer.Stats.perk_thrownSpearhellChance > 0 && Random.value <= ModdedPlayer.Stats.perk_thrownSpearhellChance && OutputDmg > 1)
								{
									var obj = Instantiate(PhysicBody, Camera.main.transform.position + Vector3.up * 2f, Quaternion.LookRotation(PhysicBody.position-Camera.main.transform.position));
									obj.velocity = PhysicBody.velocity.normalized * 180f * ModdedPlayer.Stats.projectileSpeed;
									Destroy(obj.gameObject, 25);

								}
							}

							if (headDamage || (flintLockAmmoType && Random.value <= ModdedPlayer.Stats.perk_bulletCritChance) || (spearType && Random.value <= ModdedPlayer.Stats.perk_thrownSpearCritChance))
							{

								dmgUnclamped *= ModdedPlayer.Stats.headShotDamage;
								dmgUnclamped *= SpellActions.FocusOnHeadShot();
								if (ModdedPlayer.Stats.spell_seekingArrow)
								{
									dmgUnclamped *= ModdedPlayer.Stats.spell_seekingArrow_HeadDamage;
								}
								ChampionsOfForest.COTFEvents.Instance.OnHeadshot.Invoke(new COTFEvents.HeadshotParams(dmgUnclamped,target,this,!headDamage));
								headDamage = true;

							}
							else
							{
								dmgUnclamped *= SpellActions.FocusOnBodyShot();
							}				
						
							DamageMath.ReduceDamageToSendOverNet(dmgUnclamped, out int sendDamage, out int reps);

							HitPlayer HP = HitPlayer.Create(be, EntityTargets.Everyone);
							HP.damage = sendDamage;
							for (int i = 0; i < reps; i++)
							{
								HP.Send();
							}
							//check piercing
							float pierceChance = ModdedPlayer.Stats.projectilePierceChance - pierceCount;
							if (pierceChance > 0)
							{
								if (pierceChance >= 1 || pierceChance < Random.value)
								{
									Physics.IgnoreCollision(base.GetComponent<Collider>(), targetCollider);
									pierceCount++;
									return;
								}
							}
							disableLive();
							DisableFlight();
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
		private void DisableFlight()
		{
			if (this.PhysicBody)
			{
				this.PhysicBody.velocity = Vector3.zero;
			}

			if (this.spearType)
			{
				this.PhysicBody.isKinematic = false;
				this.PhysicBody.useGravity = true;
				//this.disableLive();
				//if (this.MyPickUp)
				//{
				//	//this.MyPickUp.SetActive(true);
				//}
			}
			else if (this.MyPickUp)
			{
				if (ModdedPlayer.Stats.perk_projectileNoConsumeChance >= 1)
				{
					Destroy(this.gameObject);
					return;
				}
				else
				{
					this.MyPickUp.SetActive(true);
				}
			}
		}
		public void NewHitAi(Transform target, bool hitDelay, bool headDamage)
		{
			dmgUnclamped = this.OutputDmg;
			ModAPI.Console.Write("dmgUnclamped: " + dmgUnclamped);
			if (ModdedPlayer.Stats.spell_seekingArrow)
			{
				dmgUnclamped *= 0.9f;
			}
			{
				float dist = Vector3.Distance(target.position, startposition);
				dmgUnclamped *= 1 + dist * ModdedPlayer.Stats.projectile_DamagePerDistance;
			}
			if (spearType)
			{
				if (ModdedPlayer.Stats.perk_thrownSpearhellChance > 0 && Random.value <= ModdedPlayer.Stats.perk_thrownSpearhellChance && OutputDmg > 1)
				{
					var obj = Instantiate(PhysicBody, Camera.main.transform.position + Vector3.up * 2f, Quaternion.LookRotation(PhysicBody.position - Camera.main.transform.position));
					obj.velocity = PhysicBody.velocity.normalized * 130f * ModdedPlayer.Stats.projectileSpeed;
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

			if (headDamage || (flintLockAmmoType && Random.value <= ModdedPlayer.Stats.perk_bulletCritChance) || (spearType && Random.value <= ModdedPlayer.Stats.perk_thrownSpearCritChance))
			{
				dmgUnclamped *= ModdedPlayer.Stats.headShotDamage;
				dmgUnclamped *= SpellActions.FocusOnHeadShot();
				if (ModdedPlayer.Stats.spell_seekingArrow)
				{
					dmgUnclamped *= ModdedPlayer.Stats.spell_seekingArrow_HeadDamage;
				}
				ChampionsOfForest.COTFEvents.Instance.OnHeadshot.Invoke(new COTFEvents.HeadshotParams(dmgUnclamped, target, this, !headDamage));
				headDamage = true;
			}
			else
			{
				dmgUnclamped *= SpellActions.FocusOnBodyShot();
			}
			{
				var eventContext = new COTFEvents.HitOtherParams(dmgUnclamped, crit, target, this);
				ChampionsOfForest.COTFEvents.Instance.OnHitEnemy.Invoke(eventContext);
				ChampionsOfForest.COTFEvents.Instance.OnHitRanged.Invoke(eventContext);
			}

			if (target)
			{
				Vector3 vector = target.transform.root.GetChild(0).InverseTransformPoint(base.transform.position);
				float targetAngle = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				int animalHitDirection = animalHealth.GetAnimalHitDirection(targetAngle);
				BoltEntity entity = target.GetComponentInParent<BoltEntity>();
				if (!entity)
				{
					entity = target.GetComponent<BoltEntity>();
				}

				if (BoltNetwork.isClient && entity)
				{
					ModdedPlayer.instance.OnHitEffectsClient(entity, dmgUnclamped);
					if (ignite)
					{
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(27);
								w.Write(entity.networkId.PackedValue);
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
									w.Write(entity.networkId.PackedValue);
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
								w.Write(entity.networkId.PackedValue);
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
									w.Write(entity.networkId.PackedValue);
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
						playerHitEnemy.Target = entity;
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
						playerHitEnemy.Hit = DamageMath.GetSendableDamage( dmgUnclamped);
						if ((GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites) || (ignite && Random.value < 0.5f))
						{
							ChampionsOfForest.COTFEvents.Instance.OnIgniteRanged.Invoke();
							playerHitEnemy.Burn = true;
						}
						playerHitEnemy.getAttackerType += 1000000;
						playerHitEnemy.Send();
					}
					else
					{
						target.transform.SendMessageUpwards("getClientHitDirection", 6, SendMessageOptions.DontRequireReceiver);
						target.transform.SendMessageUpwards("StartPrediction", SendMessageOptions.DontRequireReceiver);
						PlayerHitEnemy playerHitEnemy2 = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
						playerHitEnemy2.Target = entity;
						if (target.gameObject.CompareTag("animalRoot"))
						{
							playerHitEnemy2.getAttackDirection = animalHitDirection;
						}
						else
						{
							playerHitEnemy2.getAttackDirection = 3;
						}
						playerHitEnemy2.getAttackerType = 4;
						if ((ignite && Random.value < 0.5f) || (GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites))
						{
							ChampionsOfForest.COTFEvents.Instance.OnIgniteRanged.Invoke();
							playerHitEnemy2.Burn = true;

						}
						playerHitEnemy2.Hit = DamageMath.GetSendableDamage(dmgUnclamped);
						playerHitEnemy2.getAttackerType += DamageMath.CONVERTEDFLOATattackerType;
						playerHitEnemy2.Send();
					}
					goto afterdamage;
				}
				else
				{
					if (target.gameObject.CompareTag("enemyRoot") || target.gameObject.CompareTag("enemyCollide"))
					{
						if (EnemyManager.enemyByTransform.ContainsKey(target.root))
						{
							var ep = EnemyManager.enemyByTransform[target.root];

							if (ignite)
							{
								if ((ignite && Random.value < 0.5f) || GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites)
									ep.HealthScript.Burn();
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
							ep.HealthScript.getAttackDirection(3);
							ep.HitPhysical(dmgUnclamped);
							Debug.Log("HIT PHYSICAL");
							goto afterdamage;
						}
					}
					target.gameObject.SendMessageUpwards("getAttackDirection", 3, SendMessageOptions.DontRequireReceiver);
					target.gameObject.SendMessageUpwards("getAttackerType", 4, SendMessageOptions.DontRequireReceiver);
					GameObject closestPlayerFromPos = TheForest.Utils.Scene.SceneTracker.GetClosestPlayerFromPos(base.transform.position);
					target.gameObject.SendMessageUpwards("getAttacker", closestPlayerFromPos, SendMessageOptions.DontRequireReceiver);
					if (target.gameObject.CompareTag("enemyRoot") || target.gameObject.CompareTag("lb_bird") || target.gameObject.CompareTag("animalRoot"))
					{
							Debug.Log("HIT NORMAL");
						if (target.gameObject.CompareTag("enemyRoot"))
						{
							EnemyHealth targetEnemyHealth = target.GetComponentInChildren<EnemyHealth>();
							if (targetEnemyHealth)
							{
								targetEnemyHealth.getAttackDirection(3);
								targetEnemyHealth.setSkinDamage(2);
								mutantTargetSwitching componentInChildren2 = target.GetComponentInChildren<mutantTargetSwitching>();
								if (componentInChildren2)
								{
									componentInChildren2.getAttackerType(4);
									componentInChildren2.getAttacker(closestPlayerFromPos);
								}

								targetEnemyHealth.Hit((int)Mathf.Min(int.MaxValue,dmgUnclamped));
								
							if ((ignite && Random.value < 0.5f) || GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites)
								targetEnemyHealth.Burn();
							}
						}
						else
						{
							if (target.gameObject.CompareTag("animalRoot"))
							{
								target.gameObject.SendMessage("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
							}
							target.SendMessageUpwards("Hit",(int)Mathf.Min( dmgUnclamped,int.MaxValue/2), SendMessageOptions.DontRequireReceiver);
							if ((ignite && Random.value < 0.5f) || GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites)
								target.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
							target.gameObject.SendMessage("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						if (target.gameObject.CompareTag("animalCollide"))
						{
							target.gameObject.SendMessageUpwards("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
						}
						target.SendMessageUpwards("Hit", (int)Mathf.Min(dmgUnclamped, int.MaxValue / 2), SendMessageOptions.DontRequireReceiver);
						if (GreatBow.isEnabled && ModdedPlayer.Stats.i_greatBowIgnites || (ignite && Random.value < 0.5f))
							target.gameObject.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
						target.gameObject.SendMessageUpwards("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
afterdamage:
			
			if (ModdedPlayer.Stats.perk_projectileNoConsumeChance > 0.35f)
			{
				FMODCommon.PlayOneshotNetworked(this.hitAiEvent, base.transform, FMODCommon.NetworkRole.Any);
			}
		}
		public void ModifyHitDamage(float multipier)
		{
			dmgUnclamped *= multipier;
		}
		public void ModifyStartingDamage(float multipier)
		{
			damageMult = multipier;
		}
	}
}