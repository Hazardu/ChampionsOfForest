using Bolt;

using ChampionsOfForest.Player;

using TheForest.Utils;
using TheForest.Utils.Settings;
using TheForest.World;

using UnityEngine;

using Scene = TheForest.Utils.Scene;

namespace ChampionsOfForest.Enemies
{
	public class EnemyHitTriggerMod : enemyWeaponMelee
	{
		public static readonly float poisonDuration = 6;
		public static readonly float stunDuration = 1f;

		private Vector3 originalScale = Vector3.zero;
		private BoltEntity entity;
		private float LastReqTime;

		private void OnEnable()
		{
			if (originalScale != Vector3.zero)
			{
				transform.localScale = originalScale;
			}
		}

		public void SetTriggerScaleForTiny()
		{
			if (originalScale == Vector3.zero)
			{
				originalScale = transform.localScale;
			}
			transform.localScale = originalScale * 5;
		}

		protected override void Update()
		{
			if (GameSetup.IsMpClient)
			{
				if (entity == null)
				{
					entity = gameObject.GetComponentInParent<BoltEntity>();
					if (entity == null)
					{
						entity = gameObject.GetComponent<BoltEntity>();
					}
					if (entity == null)
					{
						entity = gameObject.GetComponentInChildren<BoltEntity>();
					}
				}
				else
				{
					if (ModSettings.DifficultyChoosen)
					{
						if (!EnemyManager.clientEnemies.ContainsKey(entity.networkId.PackedValue))
						{
							if (Time.time - LastReqTime > 20)
							{
								LastReqTime = Time.time;
								using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
								{
									using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
									{
										w.Write(29);
										w.Write(entity.networkId.PackedValue);
										w.Close();
									}
									ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
									answerStream.Close();
								}
							}
						}
					}
				}
			}
			base.Update();
		}

		private EnemyProgression EnemyProg;
		private int hitDamage = 0;
		[ModAPI.Attributes.Priority(100)]
		protected override void OnTriggerEnter(Collider other)
		{
			try
			{
				if (GameSetup.IsMpClient)
				{
					if (entity == null && !EnemyManager.clientEnemies.ContainsKey(entity.networkId.PackedValue))
						return;
					else if (EnemyManager.clientEnemies[entity.networkId.PackedValue].Outdated)
					{
						LastReqTime = Time.time;
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(29);
								w.Write(entity.networkId.PackedValue);
								w.Close();
							}
							ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
							answerStream.Close();
						}
					}
				}

				currState = animator.GetCurrentAnimatorStateInfo(0);
				nextState = animator.GetNextAnimatorStateInfo(0);
				if (currState.tagHash != damagedHash && currState.tagHash != staggerHash && currState.tagHash != hitStaggerHash && currState.tagHash != deathHash && nextState.tagHash != damagedHash && nextState.tagHash != staggerHash && nextState.tagHash != hitStaggerHash && nextState.tagHash != deathHash)
				{
					if (other.gameObject.CompareTag("trapTrigger"))
					{
						other.gameObject.SendMessage("CutRope", SendMessageOptions.DontRequireReceiver);
					}
					if (!netPrefab && LocalPlayer.Animator && LocalPlayer.Animator.GetBool("deathBool"))
					{
						return;
					}
					if (other.gameObject.CompareTag("playerHitDetect") && mainTrigger)
					{
						if (!Scene.SceneTracker.hasAttackedPlayer)
						{
							Scene.SceneTracker.hasAttackedPlayer = true;
							Scene.SceneTracker.Invoke("resetHasAttackedPlayer", Random.Range(120, 240));
						}
						targetStats component = other.transform.root.GetComponent<targetStats>();
						if (component && component.targetDown)
						{
							return;
						}
						Animator componentInParent = other.gameObject.GetComponentInParent<Animator>();
						Vector3 position = rootTr.position;
						position.y += 3.3f;
						Vector3 direction = other.transform.position - position;
						if (!Physics.Raycast(position, direction, out hit, direction.magnitude, enemyHitMask, QueryTriggerInteraction.Ignore))
						{
							bool doParry = SpellActions.ParryAnythingIsTimed;
							if (doParry || (!creepy_male && !creepy && !creepy_baby && !creepy_fat && events))
							{
								if (componentInParent)
								{
									bool flag = InFront(other.gameObject);
									if (doParry || ((!BoltNetwork.isServer || !netPrefab) && flag && events.parryBool && ((componentInParent.GetNextAnimatorStateInfo(1).tagHash == blockHash || componentInParent.GetCurrentAnimatorStateInfo(1).tagHash == blockHash))))
									{
										SpellActions.DoParry(transform.position);
										ModAPI.Console.Write("Parrying successful");

										if ((!creepy_male && !creepy && !creepy_baby && !creepy_fat && events))
										{
											int parryDir = events != null ? events.parryDir : 1;
											BoltSetReflectedShim.SetIntegerReflected(animator, "parryDirInt", parryDir);
											if (BoltNetwork.isClient && netPrefab)
											{
												BoltSetReflectedShim.SetTriggerReflected(animator, "ClientParryTrigger");
												hitPrediction.StartParryPrediction();
												FMODCommon.PlayOneshot(parryEvent, base.transform);
												parryEnemy parryEnemy = parryEnemy.Create(GlobalTargets.OnlyServer);
												parryEnemy.Target = transform.root.GetComponent<BoltEntity>();
												parryEnemy.Send();
											}
											else
											{
												BoltSetReflectedShim.SetTriggerReflected(animator, "parryTrigger");
											}
											events.StartCoroutine("disableAllWeapons");
											playerHitReactions componentInParent2 = other.gameObject.GetComponentInParent<playerHitReactions>();
											if (componentInParent2 != null)
											{
												componentInParent2.enableParryState();
											}
											FMODCommon.PlayOneshotNetworked(parryEvent, base.transform, FMODCommon.NetworkRole.Server);
											events.parryBool = false;
										}
										return;
									}
								}
							}
							if ((bool)events)
							{
								events.parryBool = false;
							}
							other.transform.root.SendMessage("getHitDirection", rootTr.position, SendMessageOptions.DontRequireReceiver);
							int num = 0;
							if (maleSkinny || femaleSkinny)
							{
								if (pale)
								{
									num = ((!skinned) ? Mathf.FloorToInt(10f * GameSettings.Ai.skinnyDamageRatio) : Mathf.FloorToInt(10f * GameSettings.Ai.skinnyDamageRatio * GameSettings.Ai.skinMaskDamageRatio));
								}
								else
								{
									num = Mathf.FloorToInt(13f * GameSettings.Ai.skinnyDamageRatio);
									if (maleSkinny && props.regularStick.activeSelf && events.leftHandWeapon)
									{
										num = Mathf.FloorToInt(num * 1.35f);
									}
								}
							}
							else if (male && pale)
							{
								num = ((!skinned) ? Mathf.FloorToInt(22f * GameSettings.Ai.largePaleDamageRatio) : Mathf.FloorToInt(22f * GameSettings.Ai.largePaleDamageRatio * GameSettings.Ai.skinMaskDamageRatio));
							}
							else if (male && !firemanMain)
							{
								num = ((!painted) ? Mathf.FloorToInt(20f * GameSettings.Ai.regularMaleDamageRatio) : Mathf.FloorToInt(20f * GameSettings.Ai.regularMaleDamageRatio * GameSettings.Ai.paintedDamageRatio));
							}
							else if (female)
							{
								num = Mathf.FloorToInt(17f * GameSettings.Ai.regularFemaleDamageRatio);
							}
							else if (creepy)
							{
								num = ((!pale) ? Mathf.FloorToInt(28f * GameSettings.Ai.creepyDamageRatio) : Mathf.FloorToInt(35f * GameSettings.Ai.creepyDamageRatio));
							}
							else if (creepy_male)
							{
								num = ((!pale) ? Mathf.FloorToInt(60f * GameSettings.Ai.creepyDamageRatio) : Mathf.FloorToInt(120f * GameSettings.Ai.creepyDamageRatio));
							}
							else if (creepy_baby)
							{
								num = Mathf.FloorToInt(26f * GameSettings.Ai.creepyBabyDamageRatio);
							}
							else if (firemanMain)
							{
								num = Mathf.FloorToInt(12f * GameSettings.Ai.regularMaleDamageRatio);
								if (events && !enemyAtStructure && !events.noFireAttack)
								{
									if (BoltNetwork.isRunning && netPrefab)
									{
										other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
									}
									else
									{
										other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
									}
								}
							}
							if (!female && male)
							{
								if (holdingRegularWeapon() && events.leftHandWeapon)
								{
									num += 7;
								}
								else if (holdingAdvancedWeapon() && events.leftHandWeapon)
								{
									num += 15;
								}
							}
							if (setup && setup.health.poisoned)
							{
								num = Mathf.FloorToInt(num / 1.6f);
							}

							//COTF additional code
							try
							{
								if (GameSetup.IsMpClient)
								{
									if (other.transform.root == LocalPlayer.Transform.root)
									{
										var x = EnemyManager.clientEnemies[entity.networkId.PackedValue];
										num = Mathf.RoundToInt(num * x.damagemult);
										if (x.abilities.Contains(EnemyProgression.Abilities.RainEmpowerement))
										{
											if (TheForest.Utils.Scene.WeatherSystem.Raining)
											{
												num *= 5;
											}
										}
										hitDamage = num;
										if (x.abilities.Contains(EnemyProgression.Abilities.Poisonous))
										{
											BuffDB.AddBuff(3, 32, Mathf.Sqrt(num / 10) / 7, poisonDuration);
										}
										if (x.abilities.Contains(EnemyProgression.Abilities.Basher))
										{
											ModdedPlayer.instance.Stun(stunDuration);
										}
										if (ModdedPlayer.Stats.TotalThornsDamage > 0)
										{

											if (ModdedPlayer.Stats.TotalThornsArmorPiercing > 0)
												EnemyProgression.ReduceArmor(entity, ModdedPlayer.Stats.TotalThornsArmorPiercing);
													
											PlayerHitEnemy playerHitEnemy =		PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
											playerHitEnemy.Target = entity;

											//this integer make the attack not stagger the enemy
											playerHitEnemy.getAttackerType = 2000000;

											playerHitEnemy.Hit = DamageMath.GetSendableDamage(ModdedPlayer.Stats.TotalThornsDamage);
											playerHitEnemy.Send();
										}
									}
								}
								else
								{
									if (other.transform.root == LocalPlayer.Transform.root && EnemyManager.enemyByTransform.ContainsKey(this.rootTr))
									{
										if (EnemyProg == null)
										{
											EnemyProg = EnemyManager.enemyByTransform[this.rootTr];
										}
										num = Mathf.RoundToInt(num * EnemyProg.DamageAmp * EnemyProg.DebuffDmgMult);
										BoltEntity bo = other.transform.root.GetComponent<BoltEntity>();
										if (bo == null)
										{
											bo = other.transform.root.GetComponentInChildren<BoltEntity>();
										}
										hitDamage = num;

										//POISON ATTACKS
										if (EnemyProg.abilities.Contains(EnemyProgression.Abilities.Poisonous))
										{
											BuffDB.AddBuff(3, 32, Mathf.Sqrt(num / 10) / 10, poisonDuration);
										}

										//STUN ON HIT
										if (EnemyProg.abilities.Contains(EnemyProgression.Abilities.Basher))
										{
											ModdedPlayer.instance.Stun(stunDuration);
										}

										if (ModdedPlayer.Stats.TotalThornsDamage > 0)
										{
											EnemyProg.HitPhysicalSilent(ModdedPlayer.Stats.TotalThornsDamage);
											if (ModdedPlayer.Stats.TotalThornsArmorPiercing > 0)
												EnemyProg.ReduceArmor( ModdedPlayer.Stats.TotalThornsArmorPiercing);

										}
									}
								}
							}
							catch (System.Exception ex)
							{
								ModAPI.Log.Write(ex.ToString());
							}

							PlayerStats component2 = other.transform.root.GetComponent<PlayerStats>();
							if (male || female || creepy_male || creepy_fat || creepy || creepy_baby)
							{
								netId component3 = other.transform.GetComponent<netId>();
								if (BoltNetwork.isServer && component3)
								{
									other.transform.root.SendMessage("StartPrediction", SendMessageOptions.DontRequireReceiver);
									return;
								}
								if (BoltNetwork.isClient && netPrefab && !(bool)component3)
								{
									other.transform.root.SendMessage("setCurrentAttacker", this, SendMessageOptions.DontRequireReceiver);
									other.transform.root.SendMessage("hitFromEnemy", num, SendMessageOptions.DontRequireReceiver);
									other.transform.root.SendMessage("StartPrediction", SendMessageOptions.DontRequireReceiver);
								}
								else if (BoltNetwork.isServer)
								{
									if (!(bool)component3)
									{
										other.transform.root.SendMessage("setCurrentAttacker", this, SendMessageOptions.DontRequireReceiver);
										other.transform.root.SendMessage("hitFromEnemy", num, SendMessageOptions.DontRequireReceiver);
									}
								}
								else if (!BoltNetwork.isRunning && component2)
								{
									component2.setCurrentAttacker(this);
									component2.hitFromEnemy(num);
								}
							}
							else if (!netPrefab && component2)
							{
								component2.setCurrentAttacker(this);
								component2.hitFromEnemy(num);
							}

							goto IL_092f;
						}
						return;
					}
					goto IL_092f;
				}

				return;
			IL_092f:
				if (other.gameObject.CompareTag("enemyCollide") && mainTrigger && bodyCollider && !enemyAtStructure)
				{
					setupAttackerType();
					if (other.gameObject != bodyCollider)
					{
						other.transform.SendMessageUpwards("getAttackDirection", Random.Range(0, 2), SendMessageOptions.DontRequireReceiver);
						other.transform.SendMessageUpwards("getCombo", Random.Range(1, 4), SendMessageOptions.DontRequireReceiver);
						other.transform.SendMessage("getAttackerType", attackerType, SendMessageOptions.DontRequireReceiver);
						other.transform.SendMessage("getAttacker", rootTr.gameObject, SendMessageOptions.DontRequireReceiver);
						other.transform.SendMessageUpwards("HitPhysical",Random.Range(30f,50f) * Mathf.Pow(ModdedPlayer.Stats.explosionDamage,1.25f), SendMessageOptions.DontRequireReceiver);
						FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
					}
				}
				if (other.gameObject.CompareTag("BreakableWood") || (other.gameObject.CompareTag("BreakableRock") && mainTrigger))
				{
					other.transform.SendMessage("Hit", 50, SendMessageOptions.DontRequireReceiver);
					other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
					FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
				}
				if (other.gameObject.CompareTag("SmallTree") && !mainTrigger)
				{
					other.SendMessage("Hit", 2, SendMessageOptions.DontRequireReceiver);
				}
				if (other.gameObject.CompareTag("Fire") && mainTrigger && firemanMain && !events.noFireAttack)
				{
					other.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
				}
				if (other.gameObject.CompareTag("Tree") && mainTrigger && creepy_male)
				{
					other.SendMessage("Explosion", 5f, SendMessageOptions.DontRequireReceiver);
					FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
				}
				if (!other.gameObject.CompareTag("structure") && !other.gameObject.CompareTag("SLTier1") && !other.gameObject.CompareTag("SLTier2") && !other.gameObject.CompareTag("SLTier3") && !other.gameObject.CompareTag("jumpObject") && !other.gameObject.CompareTag("UnderfootWood"))
				{
					return;
				}
				if (!mainTrigger)
				{
					return;
				}
				getStructureStrength component4 = other.gameObject.GetComponent<getStructureStrength>();
				bool flag2 = false;
				if (component4 == null)
				{
					flag2 = true;
				}
				enemyAtStructure = true;
				int num2 = 0;
				if (!creepy_male && !creepy && !creepy_fat && !creepy_baby)
				{
					if (!flag2)
					{
						num2 = ((maleSkinny || femaleSkinny) ? ((component4._strength == getStructureStrength.strength.weak) ? Mathf.FloorToInt(8f * GameSettings.Ai.regularStructureDamageRatio) : 0) : ((pale || painted || skinned) ? ((component4._strength != getStructureStrength.strength.veryStrong) ? Mathf.FloorToInt(16f * GameSettings.Ai.regularStructureDamageRatio) : 0) : ((component4._strength != getStructureStrength.strength.veryStrong) ? Mathf.FloorToInt(12f * GameSettings.Ai.regularStructureDamageRatio) : 0)));
						goto IL_0d63;
					}
					return;
				}
				num2 = ((!creepy_baby) ? Mathf.FloorToInt(30f * GameSettings.Ai.creepyStructureDamageRatio) : Mathf.FloorToInt(10f * GameSettings.Ai.creepyStructureDamageRatio));
				goto IL_0d63;
			IL_0d63:
				if (setup && setup.health.poisoned)
				{
					num2 /= 2;
				}
				other.SendMessage("Hit", num2, SendMessageOptions.DontRequireReceiver);
				other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, num2), SendMessageOptions.DontRequireReceiver);
				FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
			}
			catch (System.Exception ee)
			{
				ModAPI.Log.Write(ee.ToString());
			}
		}

	}
}