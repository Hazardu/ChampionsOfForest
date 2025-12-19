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

		public static void AddBoots()
		{
			//------------------------------------------------------
			//Rarity 0 (White)
			//------------------------------------------------------

			new Boot()
				.DefaultStatSlot(1)
				.StatSlot(new Stat[] { MOVEMENT_SPEED })
				.Name("")
				.Description("")
				.Rarity(0);

			//------------------------------------------------------
			//Rarity 1 (Green)
			//------------------------------------------------------

			new Boot()
				.DefaultStatSlot(2)
				.StatSlot(new Stat[] { MOVEMENT_SPEED })
				.Name("")
				.Description("")
				.Rarity(1);

			//------------------------------------------------------
			//Rarity 2 (Blue)
			//------------------------------------------------------

			new Boot()
				.DefaultStatSlot(3)
				.StatSlot(new Stat[] { MOVEMENT_SPEED })
				.Name("")
				.Description("")
				.Rarity(2);

			//------------------------------------------------------
			//Rarity 3 (Yellow)
			//------------------------------------------------------

			new Boot()
				.DefaultStatSlot(4)
				.StatSlot(new Stat[] { MOVEMENT_SPEED })
				.Name("")
				.Description("")
				.Rarity(3);

			//------------------------------------------------------
			//Rarity 4 (Red)
			//------------------------------------------------------

			new Boot()
				.DefaultStatSlot(5)
				.StatSlot(new Stat[] { MOVEMENT_SPEED })
				.Name("")
				.Description("")
				.Rarity(4);
		
		}
	}

}