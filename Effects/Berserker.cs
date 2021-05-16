using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class Berserker
	{
		public static bool on;
		public static bool active;
		private static float castTimestamp;
		private static int setbonusAmount;
		public static void Cast()
		{
			BuffDB.AddBuff(17, 50, 0,ModdedPlayer.Stats.spell_berserkDuration);
		}

		public static void OnEnable()
		{
			active = true;
			ModdedPlayer.Stats.attackSpeed.Multiply( 1.25f);
			ModdedPlayer.Stats.allDamage.Multiply(1.25f);
			ModdedPlayer.Stats.movementSpeed.Multiply(  1.35f);
			ModdedPlayer.Stats.allDamageTaken.Multiply(  5f);
			castTimestamp = Time.time;
			setbonusAmount = 0;
		}

		public static void OnDisable()
		{
			active = false;
			ModdedPlayer.Stats.attackSpeed.Divide( 1.25f);
			ModdedPlayer.Stats.allDamage.Divide( 1.25f);
			ModdedPlayer.Stats.movementSpeed.Divide( 1.35f);
			ModdedPlayer.Stats.allDamageTaken.Divide( 5f);
			if (ModdedPlayer.Stats.i_setcount_BerserkSet < 2)
				BuffDB.AddBuff(18, 51, LocalPlayer.Stats.Energy, 15);
		}

		public static void Effect()
		{
			if (!active)
				return;
			LocalPlayer.Stats.Energy = ModdedPlayer.Stats.TotalMaxEnergy;
			LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
			if (ModdedPlayer.Stats.i_setcount_BerserkSet >= 4)
			{
				float timeSpan = Time.time - castTimestamp;
				int bonus = Mathf.CeilToInt(timeSpan);
				if (bonus > setbonusAmount)
				{
					setbonusAmount = bonus;
					float buff = 0.35f * bonus + 1;
					BuffDB.ForceEndBuff(106);
					BuffDB.AddBuff(9, 106, buff, 3f);
					if (ModdedPlayer.Stats.i_setcount_BerserkSet >= 5)
					{
						if (bonus <= 15)
							BuffDB.ForceEndBuff(107);
						BuffDB.AddBuff(14, 107, 0.3f * bonus, ModdedPlayer.Stats.spell_berserkDuration - bonus);
					}
				}

			}
		}
	}
}