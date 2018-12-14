namespace ChampionsOfForest.Player
{
    public class FPCharacterMod : FirstPersonCharacter
    {
        private static readonly float baseRunSpeed = 12;
        private static readonly float basestrafeSpeed = 6;
        private static readonly float basewalkSpeed = 6;
        private static readonly float basecrouchspeed = 4;

        protected override void Update()
        {
            runSpeed = baseRunSpeed * ModdedPlayer.instance.MoveSpeed;
            strafeSpeed = basestrafeSpeed * ModdedPlayer.instance.MoveSpeed;
            walkSpeed = basewalkSpeed * ModdedPlayer.instance.MoveSpeed;
            crouchSpeed = basecrouchspeed * ModdedPlayer.instance.MoveSpeed;
            allowJump = true;

            if (ModdedPlayer.instance.Stunned)
            {
                runSpeed = 0;
                strafeSpeed = 0;
                walkSpeed = 0;
                crouchSpeed = 0;
                allowJump = false;
            }
            base.Update();
        }
    }
}