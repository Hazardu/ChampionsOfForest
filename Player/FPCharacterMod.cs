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
                
                MovementLocked = true;
                CanJump = false;
                allowJump = false;
            }
            if (ModdedPlayer.instance.Stunned)
            {
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
    }
}