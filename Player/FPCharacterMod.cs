using TheForest.Utils;

namespace ChampionsOfForest.Player
{
    public class FPCharacterMod : FirstPersonCharacter
    {
        private static readonly float baseRunSpeed = 12;
        private static readonly float basestrafeSpeed = 6;
        private static readonly float basewalkSpeed = 6;
        private static readonly float basecrouchspeed = 3.5f;

        protected override void Update()
        {
            if (runSpeed >= baseRunSpeed)
            {
                runSpeed = baseRunSpeed * ModdedPlayer.instance.MoveSpeed;
            }

            if (strafeSpeed >= basestrafeSpeed)
            {
                strafeSpeed = basestrafeSpeed * ModdedPlayer.instance.MoveSpeed;
            }

            if (walkSpeed >= basewalkSpeed)
            {
                walkSpeed = basewalkSpeed * ModdedPlayer.instance.MoveSpeed;
            }

            crouchSpeed = basecrouchspeed * ModdedPlayer.instance.MoveSpeed;
            allowJump = true;

            if (ModdedPlayer.instance.Rooted)
            {
                runSpeed = 0;
                strafeSpeed = 0;
                walkSpeed = 0;
                speed = 0;
                crouchSpeed = 0;
                allowJump = false;
            }
            if (ModdedPlayer.instance.Stunned)
            {
                Locked = true;
                MovementLocked = true;

                //Locking player movement and rotation without showing cursor
                this.Locked = true;
                this.CanJump = false;

                LocalPlayer.Inventory.StashLeftHand();
                LocalPlayer.Inventory.StashEquipedWeapon(false);
            }
            base.Update();
        }
    }
}