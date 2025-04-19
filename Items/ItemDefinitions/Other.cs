using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using ChampionsOfForest.Items.ItemTemplates;
using ChampionsOfForest.Player;

using UnityEngine;

using static ChampionsOfForest.Items.ItemDatabase.Stat;

namespace ChampionsOfForest.Items
{

	public static partial class ItemDatabase
	{

		public static void AddConsumables()
		{

			new Heart("Grants 1 additional perk point", () =>
			{
				ModdedPlayer.instance.PermanentBonusPerkPoints += 1;
				ModdedPlayer.instance.MutationPoints += 1;
			})
				.Name("Greater Mutated Heart")
				.Description("This heart is finally still. Before, it used to pump blood brimming with strength into the largest, most grotesque mutants creeping on this peninsula.")
				.SetDropCreepy()
				.Rarity(4)
				.Weight(100);

			new Heart("Grants 1 additional perk point", ModdedPlayer.Respec)
				.Name("Heart of Purity")
				.Description("A mysterious heart of the most innocent creature. Sometimes, the cannibals carry these intact hearts for unknown reasons. According to their strict diet, they don't eat anything that isn't human, so this heart must belong to something else.")
				.Rarity(2);


		}

	}


}