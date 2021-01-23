using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest
{
	public class EnemyHealthMod : EnemyHealth
	{
		public EnemyProgression progression = null;
		public float LastHitTime;
		private const float HitMaxFrequency = 0.05f;

		//creates progression
		protected override void OnEnable()
		{
			Invoke("LateProgressionCreate", 2);

			base.OnEnable();
		}

		//It takes some time for enemies to appear on screen.
		private void LateProgressionCreate()
		{
			try
			{
				if (progression == null)
				{
					progression = transform.root.gameObject.AddComponent<EnemyProgression>();
					progression.HealthScript = this;
					progression.AIScript = ai;
					progression.entity = entity;
					progression.setup = setup;
				}
				progression.setupComplete = false;
				progression.OnDieCalled = false;
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		protected override void Update()
		{
			if (progression == null)
			{
				progression = GetComponent<EnemyProgression>();
			}
			if (setup.waterDetect.drowned && !deadBlock)
			{
				Health = 0;
				progression.extraHealth = 0;
				dieExplode();
			}
			base.Update();
		}

		public override void Detached()
		{
			EnemyManager.RemoveEnemy(progression);
			base.Detached();
		}

		public override void getStealthAttack()
		{
			doStealthKill = false;
		}

		protected override void OnDestroy()
		{
			//if (!ai.creepy_fat)
			//{
			if (Health <= 0)
			{
				progression.OnDie();
			}
			//}
			base.OnDestroy();
		}

		//changes how damage is calculated to include armor and abilities
		public override void Hit(int damage)
		{
			//if (ai.creepy_fat)
			//{
			//base.Hit(damage);
			//return;
			//}
			HitPhysical(damage);
		}

		public void ReduceAr(int value)
		{
			progression.ArmorReduction += value;
			return;
		}

		//Reduced damage
		public void HitPhysical(int damage)
		{
			int dmg = progression.ClampDamage(false, damage);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, dmg, new Color(1, 0.4f, 0, 1f));
			HitReal(dmg);
		}

		//Fire additionally reduces armor
		protected override void HitFire()
		{
			if (this.ai.creepy_boss && !this.ai.girlFullyTransformed)
			{
				return;
			}
			if (!this.deadBlock)
			{
				this.setSkinDamage(UnityEngine.Random.Range(0, 3));
				this.targetSwitcher.attackerType = 4;
				int num = this.douseMult - 1;
				int i = 0;
				if (num < 1)
				{
					num = 1;
				}
				if (this.ai.creepy_boss)
				{
					i = Mathf.CeilToInt(2 * num * TheForest.Utils.Settings.GameSettings.Ai.fireDamageCreepyRatio * (1 + progression.FireDmgAmp));
					Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, i, new Color(1, 0, 0, 1f));

					base.HitFireDamageOnly(i);
				}
				else if (this.ai.creepy || this.ai.creepy_male || this.ai.creepy_fat || this.ai.creepy_baby || this.ai.creepy_boss)
				{
					i = Mathf.CeilToInt(UnityEngine.Random.Range(3, 10) * num * TheForest.Utils.Settings.GameSettings.Ai.fireDamageCreepyRatio * progression.FireDmgAmp + progression.FireDmgBonus * progression.FireDmgAmp);
					Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, i, new Color(1, 0, 0, 1f));
					base.HitReal(i);
				}
				else
				{
					i = Mathf.CeilToInt(UnityEngine.Random.Range(4, 10) * num * TheForest.Utils.Settings.GameSettings.Ai.fireDamageRatio * progression.FireDmgAmp + progression.FireDmgBonus * progression.FireDmgAmp);
					Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, i, new Color(1, 0, 0, 1f));

					base.HitReal(i);
				}
				progression.ArmorReduction += i;
			}
		}

		//pure damage
		public override void HitReal(int damage)
		{
			//Creating a hit marker for every player

			//if (!ai.creepy_fat)
			//{
			if (progression.extraHealth > 0)
			{
				int i = (int)Mathf.Min(progression.extraHealth, (float)damage);
				progression.extraHealth -= i;
				damage -= i;
			}
			//}
			hitBlock = false;
			base.HitReal(damage);
		}

		//Overriden to forbid enemies from getting oneshotted,
		//Instead they take damage
		public override void Explosion(float explodeDist)
		{
			if (this.ai.creepy_boss && !this.ai.girlFullyTransformed)
			{
				return;
			}
			if (!this.explodeBlock)
			{
				if (this.ai.creepy_male || this.ai.creepy || this.ai.creepy_fat || this.ai.creepy_baby || this.ai.creepy_boss)
				{
				
						this.progression.HitPure(750 * (1+ModdedPlayer.Stats.explosionDamage.Value));
					
					if (this.Burnt && this.MySkin && !this.ai.creepy_boss && explodeDist > 0f)
					{
						if (this.setup.propManager && this.setup.propManager.lowSkinnyBody)
						{
							this.setup.propManager.lowSkinnyBody.GetComponent<SkinnedMeshRenderer>().sharedMaterial = this.Burnt;
						}
						if (this.setup.propManager && this.setup.propManager.lowBody)
						{
							this.setup.propManager.lowBody.GetComponent<SkinnedMeshRenderer>().sharedMaterial = this.Burnt;
						}
						this.MySkin.sharedMaterial = this.Burnt;
						this.alreadyBurnt = true;
					}
					this.setSkinDamage(UnityEngine.Random.Range(0, 3));
					if (this.Health <= 0)
					{
						if (this.ai.creepy_boss)
						{
							this.Die();
						}
						else
						{
							this.dieExplode();
						}
					}
					else
					{
						if (UnityEngine.Random.value > 0.75f && this.ai.creepy_boss && !this.animator.GetBool("hitStagger"))
						{
							this.setup.pmCombat.FsmVariables.GetFsmBool("staggered").Value = true;
							this.animator.SetBool("hitStagger", true);
							base.Invoke("resetStagger", 10f);
						}
						this.setup.pmCombat.SendEvent("toHitExplode");
					}
				}
				else if (this.ai.male || this.ai.female)
				{
					if (explodeDist <= 0f)
					{
						this.getAttackDirection(5);
						this.targetSwitcher.attackerType = 4;
						this.animator.SetIntegerReflected("hurtLevelInt", 4);
						this.animator.SetTriggerReflected("damageTrigger");
						this.setSkinDamage(UnityEngine.Random.Range(0, 3));
						this.progression.HitPure(850 * (1 + ModdedPlayer.Stats.explosionDamage.Value));
						Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, 850 * (1 + ModdedPlayer.Stats.explosionDamage.Value), Color.white);

						if (this.Health < 1)
						{
							UnityEngine.Object.Instantiate<GameObject>(this.RagDollExploded, base.transform.position, base.transform.rotation);
							progression.OnDie();
							this.typeSetup.removeFromSpawnAndExplode();
							return;
						}
						this.pmGotHit();
					}
					else if (explodeDist < 10.5f)
					{
						HitReal(800);
						if (this.Health < 1)
						{
							UnityEngine.Object.Instantiate<GameObject>(this.RagDollExploded, base.transform.position, base.transform.rotation);
							progression.OnDie();
							this.typeSetup.removeFromSpawnAndExplode();
						}
					}
					else if (explodeDist > 10.5f && explodeDist < 18f)
					{
						this.getAttackDirection(5);
						this.targetSwitcher.attackerType = 4;
						this.animator.SetIntegerReflected("hurtLevelInt", 4);
						this.animator.SetTriggerReflected("damageTrigger");
						if (this.Burnt && this.MySkin)
						{
							if (this.setup.propManager && this.setup.propManager.lowSkinnyBody)
							{
								this.setup.propManager.lowSkinnyBody.GetComponent<SkinnedMeshRenderer>().sharedMaterial = this.Burnt;
							}
							if (this.setup.propManager && this.setup.propManager.lowBody)
							{
								this.setup.propManager.lowBody.GetComponent<SkinnedMeshRenderer>().sharedMaterial = this.Burnt;
							}
							this.MySkin.sharedMaterial = this.Burnt;
							this.alreadyBurnt = true;
						}
						this.setSkinDamage(UnityEngine.Random.Range(0, 3));
						this.progression.HitPure(350 * (1 + ModdedPlayer.Stats.explosionDamage.Value));

						if (this.Health < 1)
						{
							this.HitReal(10);
							return;
						}
						this.pmGotHit();
					}
					else if (explodeDist > 18f && explodeDist < 25f)
					{
						this.getAttackDirection(3);
						this.targetSwitcher.attackerType = 4;
						this.animator.SetIntegerReflected("hurtLevelInt", 0);
						this.Hit(40);
					}
				}
				else
				{
					HitReal(800);
					if (Health < 1)
					{
						this.setup.pmCombat.enabled = true;
						this.setup.pmCombat.FsmVariables.GetFsmBool("deathFinal").Value = true;
						this.setup.pmCombat.SendEvent("toDeath");
						if (this.familyFunctions)
						{
							this.familyFunctions.cancelEatMeEvent();
							this.familyFunctions.cancelRescueEvent();
						}

						UnityEngine.Object.Instantiate<GameObject>(this.RagDollExploded, base.transform.position, base.transform.rotation);
						progression.OnDie();
						this.typeSetup.removeFromSpawnAndExplode();
					}
				}
				this.explodeBlock = true;
			}
			base.Invoke("resetExplodeBlock", 0.05f);
		}

		#region DieOverride

		//Those functions, along with HitReal
		//can proc the OnDie() of EnemyProgression of this current enemy

		//Doing that, it can either return false, if the enemy is not supposed to die yet
		//as for example he has a affix that revives it after death
		//or it can return true.

		//if true, the die function will be called normally.
		//if false, it will be skipped

		//Dieing of traps is changed, as it gives exp
		//its also changed to no longer oneshot enemies

		protected override void dieExplode()
		{
			//if (ai.creepy_fat)
			//{
			//    base.dieExplode();
			//    return;
			//}
			//Explosives deal 200 pure damage, as of yet, its not scaling with any stat
			HitReal(100);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, 100, Color.white);

			if (progression.OnDie())
			{
				base.dieExplode();
			}
		}

		protected override void DieTrap(int type)
		{
			//if (ai.creepy_fat)
			//{
			//    base.DieTrap(type);
			//    return;
			//}
			//Since the trap doesnt one shot cannibals, it will deal pure damage to them
			HitReal(400);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, 100, new Color(0.7f, 0.7f, 0.4f, 0.5f));

			if (type == 2)
				return;
			if (progression.OnDie())
			{
				base.DieTrap(type);
			}
		}

		public override void Die()
		{
			if (progression.OnDie())
			{
				base.Die();
			}
		}

		#endregion DieOverride



		public override void Burn()
		{
			if (this.ai.creepy_boss && !this.ai.girlFullyTransformed)
			{
				return;
			}
			if (this.ai.fireman_dynamite)
			{
				base.Invoke("doBeltExplosion", 4f);
			}
			if (this.douseMult > 1)
			{
				TheForest.Utils.GameStats.BurntEnemy.Invoke();
				if (this.Burnt && this.MySkin)
				{
					if (this.setup.propManager && this.setup.propManager.lowSkinnyBody)
					{
						this.setup.propManager.lowSkinnyBody.GetComponent<SkinnedMeshRenderer>().sharedMaterial = this.Burnt;
					}
					if (this.setup.propManager && this.setup.propManager.lowBody)
					{
						this.setup.propManager.lowBody.GetComponent<SkinnedMeshRenderer>().sharedMaterial = this.Burnt;
					}
					this.MySkin.sharedMaterial = this.Burnt;
					this.alreadyBurnt = true;
				}
				float num;
				if (this.setup.ai.creepy_fat)
				{
					num = 10f;
				}
				else if (this.setup.ai.fireman || this.setup.ai.creepy_boss)
				{
					num = 5f;
				}
				else
				{
					num = UnityEngine.Random.Range(7f, 14f);
				}
				num *= 1 + ModdedPlayer.Stats.fireDuration;
				if (this.Fire != null && this.Fire.Length > 0)
				{
					foreach (GameObject gameObject in this.Fire)
					{
						if (gameObject && !gameObject.activeSelf)
						{
							gameObject.SetActive(true);
							gameObject.SendMessage("setFireDuration", num + 7f, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
				this.onFire = true;
				if (!this.ai.creepy_boss && this.setup.pmCombat)
				{
					this.setup.pmCombat.FsmVariables.GetFsmBool("onFireBool").Value = true;
				}
				this.targetSwitcher.attackerType = 4;
				int num2 = this.douseMult - 1;
				if (num2 < 1)
				{
					num2 = 1;
				}
				this.Hit(5 * num2);
				if (this.ai.creepy_male || this.ai.creepy || this.ai.creepy_fat || this.ai.creepy_baby)
				{
					if (!this.ai.creepy_boss)
					{
						this.animator.SetIntegerReflected("randInt1", UnityEngine.Random.Range(0, 3));
						this.animator.SetBoolReflected("onFireBool", true);
						this.setup.pmCombat.SendEvent("goToOnFire");
					}
					this.StartOnFireEvent();
				}
				if (!base.IsInvoking("HitFire"))
				{
					base.InvokeRepeating("HitFire", 1f / (1 + ModdedPlayer.Stats.fireTickRate), 1f/(1+ModdedPlayer.Stats.fireTickRate));
				}
				if (!base.IsInvoking("disableBurn"))
				{
					base.Invoke("disableBurn", num);
				}
				else
				{
					CancelInvoke("disableBurn");
					base.Invoke("disableBurn", num);

				}
			}
			else
			{
				this.singeBurn();
			}
		}
	}
}