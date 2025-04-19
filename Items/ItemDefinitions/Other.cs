using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using ChampionsOfForest.Items.ItemTemplates;

using static ChampionsOfForest.Items.ItemDatabase.Stat;

namespace ChampionsOfForest.Items
{

	public static partial class ItemDatabase
	{

		public static void AddConsumables()
		{

			//------------------------------------------------------
			//Rarity 0 (White)
			//------------------------------------------------------

			new Heart()
				.Name("Greater Mutated Heart")
				.Consumable("Grants 1 additional perk point", () => );
					


			//------------------------------------------------------
			//Rarity 1 (Green)
			//------------------------------------------------------



			//------------------------------------------------------
			//Rarity 2 (Blue)
			//------------------------------------------------------



			//------------------------------------------------------
			//Rarity 3 (Yellow)
			//------------------------------------------------------



			//------------------------------------------------------
			//Rarity 4 (Red)
			//------------------------------------------------------

		}

	}


}