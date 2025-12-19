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
		public static void AddShields()
		{
			//------------------------------------------------------
			//Rarity 0 (Grey)
			//------------------------------------------------------
			new Shield()
				.ShieldStatSlot(1)
				.StatSlot(new Stat[] { BLOCK })
				.Name("Rusten Battered Shield")
				.Description("Covered in rust and scarred from countless blows, this battered shield has seen better days. It creaks with every movement, offering only the most basic protection.")
				.Rarity(0);

			new Shield()
				.StatSlot(new Stat[] { BLOCK })
				.Name("Cracked Buckler")
				.Description("Cracked and barely holding together, this old buckler offers little more than false hope. Its surface is marred with dents and splits from battles long forgotten.")
			.Rarity(0);
			new Shield()
				.StatSlot(new Stat[] { BLOCK })
				.StatSlot(new Stat[] { STRENGTH, NONE })
				.Name("Shattered Guard")
				.Description("No warior would pick this up and call it a shield.")
				.Rarity(0);

			//------------------------------------------------------
			//Rarity 1 (White)
			//------------------------------------------------------

			new Shield()
				.ShieldStatSlot(2)
				.StatSlot(new Stat[] { BLOCK })
				.Name("Plain Iron Shield")
				.Description("Fuck you Hazard. ~Lama Mega~")
				.Rarity(1);

			new Shield()
				.ShieldStatSlot(1)
				.StatSlot(new Stat[] { BLOCK })
				.StatSlot(new Stat[] { THORNS, BASE_MELEE_DAMAGE })
				.Name("Wooden Buckler")
				.Description("This shield has a precariously sticking out nail.")
				.Rarity(1);

			new Shield()
				.ShieldStatSlot(1)
				.DefaultStatSlot(1)
				.StatSlot(new Stat[] { BLOCK })
				.Name("Footknights's Roundshield")
				.Description("")
				.Rarity(1);

			//------------------------------------------------------
			//Rarity 2 (Dark Blue)
			//------------------------------------------------------

			new Shield()
				.ShieldStatSlot(3)
				.StatSlot(new Stat[] { BLOCK })
				.Name("Runed Iron Bulwark")
				.Description("Fuck you Hazard. ~Lama Mega~")
				.Rarity(1);


			new Shield()
				.ShieldStatSlot(3)
				.StatSlot(new Stat[] { BLOCK })
				.StatSlot(new Stat[] { DODGE_CHANCE })
				.Name("Reinforced Buckler")
				.Description("Light and small shield that allows you to dodge attacks.")
				.Rarity(2);

			new Shield()
				.ShieldStatSlot(3)
				.StatSlot(new Stat[] { BLOCK })
				.StatSlot(new Stat[] { DODGE_CHANCE })
				.Name("Knight's Guard")
				.Description("")
				.Rarity(2);


			//------------------------------------------------------
			//Rarity 3 (Blue)
			//------------------------------------------------------

			new Shield()
			.ShieldStatSlot(3)
			.StatSlot(new Stat[] { BLOCK })
			.StatSlot(new Stat[] { DODGE_CHANCE })
			.Name("1st Blue Shield")
			.Description("")
			.Rarity(3);

			//------------------------------------------------------
			//Rarity 4 (Yellow)
			//------------------------------------------------------

			new Shield()
			.ShieldStatSlot(4)
			.StatSlot(new Stat[] { BLOCK })
			.StatSlot(new Stat[] { DODGE_CHANCE })
			.Name("1st Yellow Shield")
			.Description("")
			.Rarity(4);

			//------------------------------------------------------
			//Rarity 5 (Orange)
			//------------------------------------------------------

			new Shield()
			.ShieldStatSlot(5)
			.StatSlot(new Stat[] { BLOCK })
			.StatSlot(new Stat[] { DODGE_CHANCE })
			.Name("1st Orange Shield")
			.Description("")
			.Rarity(5);

			//------------------------------------------------------
			//Rarity 6 (Green)
			//------------------------------------------------------

			new Shield()
			.ShieldStatSlot(6)
			.StatSlot(new Stat[] { BLOCK })
			.StatSlot(new Stat[] { DODGE_CHANCE })
			.Name("1st Green Shield")
			.Description("")
			.Rarity(6);

			//------------------------------------------------------
			//Rarity 7 (Red)
			//------------------------------------------------------

			new Shield()
			.ShieldStatSlot(7)
			.StatSlot(new Stat[] { BLOCK })
			.StatSlot(new Stat[] { DODGE_CHANCE })
			.Name("1st Red Shield")
			.Description("")
			.Rarity(7);

		}

	}
}


//------------------------------------------------------
//Rarity 0 (White)
//------------------------------------------------------



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