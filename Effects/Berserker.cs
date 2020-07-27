using ChampionsOfForest.Player;

using TheForest.Utils;

namespace ChampionsOfForest.Effects
{
	public class Berserker
	{
		public static bool on;
		public static bool active;
		public static float duration = 30f;

		public static void Cast()
		{
			BuffDB.AddBuff(17, 50, 0, 30);
		}

		public static void OnEnable()
		{
			active = true;
			ModdedPlayer.Stats.attackSpeed.Multiply( 1.25f);
			ModdedPlayer.Stats.allDamage.Multiply(1.25f);
			ModdedPlayer.Stats.movementSpeed.Multiply(  1.35f);
			ModdedPlayer.Stats.allDamageTaken.Multiply(  5f);
		}

		public static void OnDisable()
		{
			active = false;
			ModdedPlayer.Stats.attackSpeed.Divide( 1.25f);
			ModdedPlayer.Stats.allDamage.Divide( 1.25f);
			ModdedPlayer.Stats.movementSpeed.Divide( 1.35f);
			ModdedPlayer.Stats.allDamageTaken.Divide( 5f);
			BuffDB.AddBuff(18, 51, LocalPlayer.Stats.Energy, 15);
		}

		public static void Effect()
		{
			if (!active)
				return;
			LocalPlayer.Stats.Energy = ModdedPlayer.Stats.TotalMaxEnergy;
			LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
		}
	}
}