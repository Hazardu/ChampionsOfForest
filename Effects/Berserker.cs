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
			ModdedPlayer.instance.AttackSpeedMult *= 1.2f;
			ModdedPlayer.instance.DamageOutputMult *= 1.2f;
			ModdedPlayer.instance.MoveSpeedMult *= 1.3f;
			ModdedPlayer.instance.DamageReduction *= 2f;
		}

		public static void OnDisable()
		{
			active = false;
			ModdedPlayer.instance.AttackSpeedMult /= 1.2f;
			ModdedPlayer.instance.DamageOutputMult /= 1.2f;
			ModdedPlayer.instance.MoveSpeedMult /= 1.3f;
			ModdedPlayer.instance.DamageReduction /= 2f;
			BuffDB.AddBuff(18, 51, LocalPlayer.Stats.Energy, 20);
		}

		public static void Effect()
		{
			if (!active)
				return;
			LocalPlayer.Stats.Energy = ModdedPlayer.instance.MaxEnergy;
			LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
		}
	}
}