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
			ModdedPlayer.Stats.attackSpeed.Multiply( 1.2f);
			ModdedPlayer.Stats.allDamage.Multiply(1.2f);
			ModdedPlayer.Stats.movementSpeed.Multiply(  1.3f);
			ModdedPlayer.Stats.allDamageTaken.Multiply(  2f);
		}

		public static void OnDisable()
		{
			active = false;
			ModdedPlayer.Stats.attackSpeed.Divide( 1.2f);
			ModdedPlayer.Stats.allDamage.Divide( 1.2f);
			ModdedPlayer.Stats.movementSpeed.Divide( 1.3f);
			ModdedPlayer.Stats.allDamageTaken.Divide( 2f);
			BuffDB.AddBuff(18, 51, LocalPlayer.Stats.Energy, 20);
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