using TheForest.Utils;

namespace ChampionsOfForest.Player
{
	public class FPCharacterMod : FirstPersonCharacter
	{
		public static float basewalkSpeed;

		protected override void Start()
		{
			base.Start();
			basewalkSpeed = walkSpeed;
			ModdedPlayer.instance.basejumpPower = jumpHeight;
		}
		protected override void HandleStartJumping()
		{
			COTFEvents.Instance.OnJump.Invoke();
			base.HandleStartJumping();
		}
			
		protected override void Update()
		{
			jumpHeight = ModdedPlayer.instance.basejumpPower * ModdedPlayer.Stats.jumpPower;
			if (ModdedPlayer.Stats.rooted)
			{
				this.rb.Sleep();
				this.rb.isKinematic = true;
				this.rb.useGravity = false;

				MovementLocked = true;

				CanJump = false;
				allowJump = false;
			}
			if (ModdedPlayer.Stats.stunned)
			{
				this.rb.Sleep();
				this.rb.isKinematic = true;
				this.rb.useGravity = false;
				MovementLocked = true;
				Locked = true;
				CanJump = false;
				LocalPlayer.Inventory.StashLeftHand();
				LocalPlayer.Inventory.StashEquipedWeapon(false);
			}

			base.Update();
		}

		protected override void HandleWalkingSpeedOptions()
		{
			base.HandleWalkingSpeedOptions();
			speed *= ModdedPlayer.Stats.mOVEMENT_SPEED;
		}

		protected override void HandleRunningStaminaAndSpeed()
		{
			base.HandleRunningStaminaAndSpeed();
			speed *= ModdedPlayer.Stats.mOVEMENT_SPEED;
			COTFEvents.Instance.OnSprint.Invoke();

		}

		public override void HandleLanded()
		{
			LocalPlayer.CamFollowHead.stopAllCameraShake();
			this.fallShakeBlock = false;
			base.StopCoroutine("startJumpTimer");
			this.jumpTimerStarted = false;
			float num = 28f;
			bool flag = false;
			COTFEvents.Instance.OnLand.Invoke();
			if (ModdedPlayer.Stats.perk_bunnyHop)
			{
				if (ModdedPlayer.Stats.perk_bunnyHopUpgrade)
					BuffDB.AddBuff(5, 87, 1.6f, 0.75f * ModdedPlayer.Stats.jumpPower);
				else
					BuffDB.AddBuff(5, 87, 1.25f, 0.5f * ModdedPlayer.Stats.jumpPower);
			}
			if ((LocalPlayer.AnimControl.doShellRideMode || LocalPlayer.AnimControl.flyingGlider) && this.prevVelocityXZ.magnitude > 32f)
			{
				flag = true;
			}
			if (this.prevVelocity > num && !flag && this.allowFallDamage && this.jumpingTimer > 0.75f)
			{
				if (!this.jumpLand && !Clock.planecrash)
				{
					this.jumpCoolDown = true;
					this.jumpLand = true;
					float num2 = this.prevVelocity * 0.9f * (this.prevVelocity / 27.5f);
					int damage = (int)num2 + (int)(ModdedPlayer.Stats.TotalMaxHealth * 0.008f * num2);
					float num3 = 3.8f;
					if (LocalPlayer.AnimControl.doShellRideMode)
					{
						num3 = 5f;
					}
					bool flag2 = false;
					if (this.jumpingTimer > num3 && !LocalPlayer.AnimControl.flyingGlider)
					{
						damage = (int)(1000f + ModdedPlayer.Stats.TotalMaxHealth);
						flag2 = true;
					}
					if (LocalPlayer.AnimControl.doShellRideMode && !flag2)
					{
						damage = 17 + (int)(ModdedPlayer.Stats.TotalMaxHealth * 0.13f);
					}
					if (LocalPlayer.AnimControl.disconnectFromGlider)
					{
						damage = 12 + (int)(ModdedPlayer.Stats.TotalMaxHealth * 0.08f);
						LocalPlayer.SpecialActions.SendMessage("DropGlider", true);
						this.enforceHighDrag = true;
						base.Invoke("disableHighDrag", 0.65f);
					}
					this.Stats.Hit(damage, true, PlayerStats.DamageType.Physical);
					LocalPlayer.Animator.SetBoolReflected("jumpBool", false);
					if (this.Stats.Health > 0f)
					{
						if (!LocalPlayer.ScriptSetup.pmControl.FsmVariables.GetFsmBool("doingJumpAttack").Value && !LocalPlayer.AnimControl.doShellRideMode)
						{
							LocalPlayer.Animator.SetIntegerReflected("jumpType", 1);
							LocalPlayer.Animator.SetTrigger("landHeavyTrigger");
							LocalPlayer.Animator.SetBoolReflected("jumpBool", false);
							this.CanJump = false;
							LocalPlayer.HitReactions.StartCoroutine("doHardfallRoutine");
							this.prevMouseXSpeed = LocalPlayer.MainRotator.rotationSpeed;
							LocalPlayer.MainRotator.rotationSpeed = 0.55f;
							LocalPlayer.Animator.SetLayerWeightReflected(4, 0f);
							LocalPlayer.Animator.SetLayerWeightReflected(0, 1f);
							LocalPlayer.Animator.SetLayerWeightReflected(1, 0f);
							LocalPlayer.Animator.SetLayerWeightReflected(2, 0f);
							LocalPlayer.Animator.SetLayerWeightReflected(3, 0f);
							base.Invoke("resetAnimSpine", 1f);
						}
						else
						{
							this.jumpLand = false;
							this.jumpCoolDown = false;
						}
					}
					else
					{
						this.jumpCoolDown = false;
						this.jumpLand = false;
					}
				}
				this.blockJumpAttack();
			}
			this.jumping = false;
			base.CancelInvoke("setAnimatorJump");
			if (!this.jumpCoolDown)
			{
				LocalPlayer.Animator.SetIntegerReflected("jumpType", 0);
				LocalPlayer.Animator.SetBoolReflected("jumpBool", false);
				LocalPlayer.ScriptSetup.pmControl.SendEvent("toWait");
				this.blockJumpAttack();
			}
			base.CancelInvoke("fallDamageTimer");
			this.allowFallDamage = false;
		}
	}
}