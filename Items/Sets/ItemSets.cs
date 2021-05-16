using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChampionsOfForest.Player;

namespace ChampionsOfForest.Items.Sets
{
	public static class AkagiSet
	{
		public static void Equip()
		{
			ModdedPlayer.Stats.i_setcount_AkagisSet.Add(1);
			ModdedPlayer.Stats.spell_snowstormDamageMult.Add(0.5f);
			switch (ModdedPlayer.Stats.i_setcount_AkagisSet.Value)
			{
				case 2:
					ModdedPlayer.Stats.spell_snowstormPullEnemiesIn.value = true;
					break;
				case 3:
					ModdedPlayer.Stats.spell_snowstormMaxCharge.Add(10);
					break;
				case 4:
					ModdedPlayer.Stats.spell_snowstormHitDelay.Multiply(0.5f);
					ModdedPlayer.Stats.spell_snowstormDamageMult.Add(3f);

					break;
			}
		}
		public static void Unequip()
		{
			switch (ModdedPlayer.Stats.i_setcount_AkagisSet.Value)
			{
				case 2:
					ModdedPlayer.Stats.spell_snowstormPullEnemiesIn.value = false;
					break;
				case 3:
					ModdedPlayer.Stats.spell_snowstormMaxCharge.Substract(10);
					break;
				case 4:
					ModdedPlayer.Stats.spell_snowstormHitDelay.Divide(0.5f);
					ModdedPlayer.Stats.spell_snowstormDamageMult.Substract(3f);
					break;
			}
			ModdedPlayer.Stats.i_setcount_AkagisSet.Substract(1);
			ModdedPlayer.Stats.spell_snowstormDamageMult.Substract(0.5f);
		}
	}
	public static class BerserkSet
	{
		public static void Equip()
		{
			ModdedPlayer.Stats.i_setcount_BerserkSet.Add(1);
			ModdedPlayer.Stats.weaponRange.Multiply(0.3f);
			ModdedPlayer.Stats.attackStaminaCost.Multiply(0.5f);
			switch (ModdedPlayer.Stats.i_setcount_BerserkSet.Value)
			{
				case 3:
					ModdedPlayer.Stats.spell_berserkDuration.Add(15);
					break;
			}
		}
		public static void Unequip()
		{
			switch (ModdedPlayer.Stats.i_setcount_BerserkSet.Value)
			{
				case 3:
					ModdedPlayer.Stats.spell_berserkDuration.Substract(15);
					break;
			}
			ModdedPlayer.Stats.weaponRange.Divide(0.3f);
			ModdedPlayer.Stats.attackStaminaCost.Divide(0.5f);
			ModdedPlayer.Stats.i_setcount_BerserkSet.Substract(1);

		}
	}
}
