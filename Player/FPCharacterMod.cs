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
            ModdedPlayer.basejumpPower = jumpHeight;
        }
        protected override void Update()
        {
            jumpHeight = ModdedPlayer.basejumpPower * ModdedPlayer.instance.JumpPower;
            if (ModdedPlayer.instance.Rooted)
            {
                this.rb.Sleep();
                this.rb.isKinematic = true;
                this.rb.useGravity = false;


                MovementLocked = true;

                CanJump = false;
                allowJump = false;
            }
            if (ModdedPlayer.instance.Stunned)
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
            speed *= ModdedPlayer.instance.MoveSpeed;
        }
        protected override void HandleRunningStaminaAndSpeed()
        {
            base.HandleRunningStaminaAndSpeed();
            speed *= ModdedPlayer.instance.MoveSpeed;

        }
        public override void HandleLanded()
        {
            TheForest.Utils.LocalPlayer.CamFollowHead.stopAllCameraShake();
            this.fallShakeBlock = false;
            base.StopCoroutine("startJumpTimer");
            this.jumpTimerStarted = false;
            float num = 28f;
            bool flag = false;
            if ((TheForest.Utils.LocalPlayer.AnimControl.doShellRideMode || TheForest.Utils.LocalPlayer.AnimControl.flyingGlider) && this.prevVelocityXZ.magnitude > 32f)
            {
                flag = true;
            }
            if (this.prevVelocity > num && !flag && this.allowFallDamage && this.jumpingTimer > 0.75f)
            {
                if (!this.jumpLand && !global::Clock.planecrash)
                {
                    this.jumpCoolDown = true;
                    this.jumpLand = true;
                    float num2 = this.prevVelocity * 0.9f * (this.prevVelocity / 27.5f);
                    int damage = (int)num2 + (int)(ModdedPlayer.instance.MaxHealth * 0.008f * num2);
                    float num3 = 3.8f;
                    if (TheForest.Utils.LocalPlayer.AnimControl.doShellRideMode)
                    {
                        num3 = 5f;
                    }
                    bool flag2 = false;
                    if (this.jumpingTimer > num3 && !TheForest.Utils.LocalPlayer.AnimControl.flyingGlider)
                    {
                        damage = (int)(1000f + ModdedPlayer.instance.MaxHealth);
                        flag2 = true;
                    }
                    if (TheForest.Utils.LocalPlayer.AnimControl.doShellRideMode && !flag2)
                    {
                        damage = 17 + (int)(ModdedPlayer.instance.MaxHealth * 0.13f);
                    }
                    if (TheForest.Utils.LocalPlayer.AnimControl.disconnectFromGlider)
                    {
                        damage = 12 + (int)(ModdedPlayer.instance.MaxHealth * 0.08f);
                        TheForest.Utils.LocalPlayer.SpecialActions.SendMessage("DropGlider", true);
                        this.enforceHighDrag = true;
                        base.Invoke("disableHighDrag", 0.65f);
                    }
                    this.Stats.Hit(damage, true, global::PlayerStats.DamageType.Physical);
                    TheForest.Utils.LocalPlayer.Animator.SetBoolReflected("jumpBool", false);
                    if (this.Stats.Health > 0f)
                    {
                        if (!TheForest.Utils.LocalPlayer.ScriptSetup.pmControl.FsmVariables.GetFsmBool("doingJumpAttack").Value && !TheForest.Utils.LocalPlayer.AnimControl.doShellRideMode)
                        {
                            TheForest.Utils.LocalPlayer.Animator.SetIntegerReflected("jumpType", 1);
                            TheForest.Utils.LocalPlayer.Animator.SetTrigger("landHeavyTrigger");
                            TheForest.Utils.LocalPlayer.Animator.SetBoolReflected("jumpBool", false);
                            this.CanJump = false;
                            TheForest.Utils.LocalPlayer.HitReactions.StartCoroutine("doHardfallRoutine");
                            this.prevMouseXSpeed = TheForest.Utils.LocalPlayer.MainRotator.rotationSpeed;
                            TheForest.Utils.LocalPlayer.MainRotator.rotationSpeed = 0.55f;
                            TheForest.Utils.LocalPlayer.Animator.SetLayerWeightReflected(4, 0f);
                            TheForest.Utils.LocalPlayer.Animator.SetLayerWeightReflected(0, 1f);
                            TheForest.Utils.LocalPlayer.Animator.SetLayerWeightReflected(1, 0f);
                            TheForest.Utils.LocalPlayer.Animator.SetLayerWeightReflected(2, 0f);
                            TheForest.Utils.LocalPlayer.Animator.SetLayerWeightReflected(3, 0f);
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
                TheForest.Utils.LocalPlayer.Animator.SetIntegerReflected("jumpType", 0);
                TheForest.Utils.LocalPlayer.Animator.SetBoolReflected("jumpBool", false);
                TheForest.Utils.LocalPlayer.ScriptSetup.pmControl.SendEvent("toWait");
                this.blockJumpAttack();
            }
            base.CancelInvoke("fallDamageTimer");
            this.allowFallDamage = false;
        }
    }
}
