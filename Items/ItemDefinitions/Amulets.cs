using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using ChampionsOfForest.Items.ItemTemplates;
using ChampionsOfForest.Player;

using static ChampionsOfForest.Items.ItemDatabase.Stat;

namespace ChampionsOfForest.Items
{
	public static partial class ItemDatabase
	{

		public static void AddAmulets()
		{

			//------------------------------------------------------
			//Rarity 0 (White)
			//------------------------------------------------------

			new Amulet()
				.AmuletStatSlot(1)
				.DefenseStatSlot(1)
				.Name("Steel Locket")
				.Description("A a trinklet that boosts defense.")
				.Rarity(0);

			new Amulet()
				.AmuletStatSlot(2)
				.Name("Bronze Locket")
				.Description("")
				.Rarity(0)
				.Weight(5000);

			new Amulet()
				.AmuletStatSlot(1)
				.MagicStatSlot(1)
				.Name("Small Sapphire Pedant")
				.Description("Mages value sapphires for their arcane properites.")
				.Rarity(0);

			

			//------------------------------------------------------
			//Rarity 1 (Green)
			//------------------------------------------------------

			new Amulet()
				.AmuletStatSlot(3)
				.Name("Enchanted Golden Chain")
				.Description("")
				.Rarity(1)
				.Weight(5000);

			new Amulet()
				.AmuletStatSlot(2)
				.RangedStatSlot(1)
				.StatSlot(new Stat[] { LOOT_QUALITY, LOOT_QUANTITY }, probability: 0.10f)
				.Name("Hunter's Charm")
				.Description("An amulet crafted from a beast's fang, said to enhance fortune and precision during hunts.")
				.Rarity(0);

			//------------------------------------------------------
			//Rarity 2 (Blue)
			//------------------------------------------------------

			new Amulet()
				.AmuletStatSlot(4)
				.Name("Champion's Chain")
				.Description("")
				.Rarity(2)
				.Weight(5000);

			//------------------------------------------------------
			//Rarity 3 (Yellow)
			//------------------------------------------------------

			new Amulet()
				.AmuletStatSlot(5)
				.Name("Sun Amulet")
				.Description("")
				.Rarity(3);



			new Amulet()
				.AmuletStatSlot(3)
				.DefenseStatSlot(1)
				.RecoveryStatSlot(1)
				.Name("Flesh Amulet")
				.Description("Obtainable only from mutants.")
				.Rarity(3)
				.SetDropCreepy();

			//------------------------------------------------------
			//Rarity 4 (Red)
			//------------------------------------------------------

			new Amulet()
				.AmuletStatSlot(6)
				.Name("Viridescent Mirror")
				.Description("")
				.Rarity(4)
				.UniqueStat("Magic arrow creates 2 additional projectiles and fires in volleys",
					() => ModdedPlayer.Stats.spell_magicArrowVolleyCount.Add(2),
					() => ModdedPlayer.Stats.spell_magicArrowVolleyCount.Sub(2));

		}
	}
}