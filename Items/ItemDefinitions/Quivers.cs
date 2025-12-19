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

		public static void AddQuivers()
		{
			//------------------------------------------------------
			//Rarity 0 (White)
			//------------------------------------------------------

			new Quiver()
				.RangedStatSlot(1)
				.Name("")
				.Description("")
				.Rarity(0);

			//------------------------------------------------------
			//Rarity 1 (Green)
			//------------------------------------------------------

			new Quiver()
				.RangedStatSlot(1)
				.DefaultStatSlot(1)
				.Name("")
				.Description("")
				.Rarity(1);

			//------------------------------------------------------
			//Rarity 2 (Blue)
			//------------------------------------------------------

			new Quiver()
				.RangedStatSlot(2)
				.DefaultStatSlot(1)
				.Name("")
				.Description("")
				.Rarity(2);

			//------------------------------------------------------
			//Rarity 3 (Yellow)
			//------------------------------------------------------

			new Quiver()
				.RangedStatSlot(2)
				.DefaultStatSlot(2)
				.Name("")
				.Description("")
				.Rarity(3);

			//------------------------------------------------------
			//Rarity 4 (Red)
			//------------------------------------------------------

			new Quiver()
				.RangedStatSlot(3)
				.DefaultStatSlot(2)
				.Name("")
				.Description("")
				.Rarity(4);

		}
	}
}