using ChampionsOfForest.Player;

using static ChampionsOfForest.ItemDataBase.Stat;

namespace ChampionsOfForest
{
	public static partial class ItemDataBase
	{
		public static void PopulateItems()
		{
			new BaseItem(new int[][]
			{
				new int[] { 34 },
				new int[] {43,0,39,59,67  },
			})
			{
				name = "Broken Flip-Flops",
				description = "A pair of damaged shoes. Judging by their condition, i can imagine what happened to their owner.",
				lore = "Worn by one of the passengers of the plane that Eric also flew in.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
			{
				new int[] {34 },
				new int[] {34,0,40,41 },
				new int[] {43 },
				new int[] {43,0 ,67 },
			})
			{
				name = "Old Boots",
				description = "A pair of old boots. They must have been lying here for ages.",
				lore = "Found on the Peninsula, but judging by their condition, they belong neither to a plane passenger nor a cannibal.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
		   {
				new int[] {34 },
				new int[] {34,40,41 },
				new int[] {43,3,2 },
				new int[] {43,65,67  },
		   })
			{
				name = "Damaged Leather Boots",
				description = "A pair of leather boots. They look good and have only some scratches.",
				lore = "They arrived to the Peninsula the same way Eric did. Since they were in a baggage, they avoided a lot of damage.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
		   {
				new int[] {34 },
				new int[] {3,2 },
				new int[] {43,3,2,1,4 },
				new int[] {43,65,67  },
		   })
			{
				name = "Sturdy Leather Boots",
				description = "A pair of leather boots. They are in a very good condition.",
				lore = "They arrived to the Peninsula the same way Eric did. Eric found them undamaged in their original box. They still had a pricetag - $419,99.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 2,
				minLevel = 7,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
		   {
			   new int[] {34 },
				new int[] {34,39,41,11,57 },
				new int[] {-1},
				new int[] {16,7,8 },
				new int[] {43,65,67  },
			})
			{
				name = "Damaged Army Boots",
				description = "Sturdy, hard, resistant but damaged boots.",
				lore = "They look modern, almost too modern for everything here.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 3,
				minLevel = 4,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
		   {
				new int[] {34 },
				new int[] {34,3,2,11 },
				new int[] {-1},
				new int[] {16,7,8 },
				new int[] {43,65,67  },
		   })
			{
				name = "Army Boots",
				description = "Sturdy, hard, resistant boots.",
				lore = "They look modern, almost too modern for everything here.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 4,
				minLevel = 10,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
		   {
			   new int[] {25,22 },
			   new int[] {11,1,3,17 },
			   new int[] {22,1,3,17 },
			   new int[] {28,1,65 },
			   new int[] {-1 },
			   new int[] {5,6,16,31,7,8,9,10 },
			   new int[] {5,6,16,31,7,8,9,10 },
		   })
			{
				name = "Armsy Skin Footwear",
				description = "Severed armsy legs, with all of their insides removed. All thats left is dried mutated skin.",
				lore = "Armsy, the second heaviest of the mutants needs very resistant skin. It often drags its legs on the ground when it moves. The skin on their legs grew very thick, and has bone tissue mixed with skin tissue.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 6,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			}.DropSettings_OnlyArmsy();
			new BaseItem(new int[][]
	  {
				new int[] {42,0 },
				new int[] {40,41,26,25,67  },
				new int[] {43,65,0 },
	  })
			{
				name = "Finger Warmer",
				description = "A little glove to keep your fingers warm and cozy.",
				lore = "Made of wool.",
				tooltip = "Gloves offer protection.",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
			{
				new int[] {39,40,41,42,43,24,25,26 },
				new int[] {39,40,41,42,43,24,25,26,44 },
				new int[] {43,0,7,0,5,6,8,0,21,22,23,16,67 },
				new int[] {43,0,7,0,5,6,8,0,0,0,0,21,22,23,65,66,67 },
			})
			{
				name = "Thick Rubber Glove",
				description = "A glove that helps get a better grip.",
				lore = "Made of wool.",
				tooltip = "Gloves offer protection and stat bonuses.",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
			{
				new int[] {39,40,41,42},
				new int[] {39,40,41,42},
				new int[] {1,2,3,4,5,6,7},
				new int[] {0,18,14},
				new int[] {-1 },
			})
			{
				name = "Tribal Glove",
				description = "Offers medicore protection.",
				lore = "Glove made out of thin bones, some may possibly be from a human.",
				tooltip = "Gloves offer protection and stat bonuses.",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
		  {
				new int[] {1,2,4,6,8,9},
				new int[] {1,11,65},
				new int[] {21,22,23},
				new int[] {12,13,15},
				new int[] {12,13,24,25,26},
				new int[] {24,25,26,44,35},
			})
			{
				name = "Tribe Leader Glove",
				description = "A glove that offers little protection but a lot of offensive stats.",
				lore = "A glove made of bones, some have engravings of crosses.",
				tooltip = "Gloves offer protection and stat bonuses.",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
	   {
				new int[] {43,0 },
				new int[] {43,39,40,41,42 },
	   })
			{
				name = "Worn Shorts",
				description = "Some protection for legs.",
				lore = "Short, made out of cheap thin fabric, and on top of that they are damaged. But its better than nothing.",
				tooltip = "Pants provide armor and sometimes they increase carrying capactiy.",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};
			new BaseItem(new int[][]
		   {
				new int[] {1000,1001,1002},
				new int[] {1000,1001,1002,1003,1004,0,0,0,0},
				new int[] {8,9,0,0,0,0 },
		   })
			{
				name = "Cargo Shorts",
				description = "No protection at all but they allow to carry more items.",
				lore = "They are ugly as hell tho",
				tooltip = "Pants provide armor and sometimes they increase carrying capactiy.",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};
			new BaseItem(new int[][]
					{
				new int[] {5 },
				new int[] {43,16,0,41 },
				new int[] {39,40,41,42,43,44,0,0,0,0,0,0,1003,1004},
					})
			{
				name = "Passenger's Jacket",
				description = "It's a little torn. ",
				lore = "This jacket was worn by Preston A. the 34th passenger on the plane. Eric talked to him at the airport. Guy was odd, and now he's dead.",
				tooltip = "Chest armor increases health and gives armor",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
		   {
				new int[] {5 },
				new int[] {1,2,3,4,5,6,65},
				new int[] {43,16,0,41,3,2,1 },
				new int[] {6,7,8,9,10,16,17,31, },
				new int[] {39,40,41,42,43,44,0,0,0,0,0,0,1003,1004},
		   })
			{
				name = "Leather Jacket",
				description = "Offers little protection",
				lore = "This jacket was in a baggage of one of the plane passengers",
				tooltip = "Chest armor increases health and gives armor",
				Rarity = 1,
				minLevel = 4,
				maxLevel = 7,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
		   {
				new int[] {5,3,1 },
				new int[] {5 },
				new int[] {16},
				new int[] {7,11 },
		   })
			{
				name = "Boar Skin Armor",
				description = "It's made from a skin of a huge individual. It's heavy and thick, and surely can protect from attacks of weaker enemies.",
				lore = "Boar, one of the animals on the peninsula, is rather rare and it's skin is very durable.",
				tooltip = "Chest armor increases health and gives armor",
				Rarity = 1,
				minLevel = 4,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
		   {
				new int[] {5 },
				new int[] {5,3,4,2,1 },
				new int[] {14,0,0,0 },
				new int[] {6,8,9},
				new int[] {6,8,9},
				new int[] {12,13,0,65,66 },
		   })
			{
				name = "Crocodile Skin Armor",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 2,
				minLevel = 7,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			BaseItem baseItem1 = new BaseItem(new int[][]
		   {
				new int[] {5 },
				new int[] {16 },
				new int[] {18,17,16},
				new int[] {11},
				new int[] {65,0},
				new int[] {12,13,1,2,3,4},
				new int[] {25,22,0},
		   })
			{
				name = "Plate armour",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			baseItem1.PossibleStats[1][0].Multipier = 2.5f;
			new BaseItem(new int[][]
		   {
				new int[] {5 },
				new int[] {65 },
				new int[] {16},
				new int[] {16,43},
				new int[] {16,0,43},
				new int[] {6,8,9},
				new int[] {45,43,39,42},
				new int[] {7,10,11,17,18,31,66},
		   })
			{
				name = "Bear Skin Armor",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 3,
				minLevel = 7,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
		   {
				new int[] {5 },
				new int[] {12,13},
				new int[] {13,23,26},
				new int[] {23,26},
				new int[] {34,2,2},
				new int[] {15,14},
				new int[] {16,23,4,5,6,66},
				new int[] {16,23,4,5,6,0,0,0,0},
		   })
			{
				name = "Archer's Gear",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 7,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
		   {
				new int[] {5 },
				new int[] {23,26,2 },
				new int[] {23,26 },
				new int[] {12,13},
				new int[] {13,23,26},
				new int[] {23,26},
				new int[] {34,2,2},
				new int[] {15,14},
				new int[] {16,23,4,5,6},
				new int[] {45,46,66},
				new int[] {27,48},
		   })
			{
				name = "Hazard's Gear",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 6,
				minLevel = 5,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
		   {
				new int[] {47 },
				new int[] {4,29 },
				new int[] {4,29 },
				new int[] {4,29 },
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
		   })
			{
				name = "Mysterious robe",
				description = "Magic flows through the entirety of this object. It's made out of unknown material",
				lore = "Robe looks like it was created yesterday, but its older than the oldest of mankinds' civilizations. Simply looking at it sends chills down the spine.",
				tooltip = "Allows you to use Arcane Cataclysm. Cataclysm now freezes enemies and makes them take extra damage",
				Rarity = 7,
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
				onEquip = () => ModdedPlayer.Stats.spell_cataclysmArcane.value= true,
				onUnequip = () => ModdedPlayer.Stats.spell_cataclysmArcane.value = false
			};
			new BaseItem(new int[][]
					{
				new int[] {39,40,41,42,44,8,14,49 },
				new int[] {39,40,41,42,44,8,14,49 },
				new int[] {0,62,63,64},
				new int[] {1,0,65},
					})
			{
				name = "Rusty Longsword",
				description = "A long, very heavy sword. Edge got dull over time. Still, it's in a condition that allows me to slice some enemies in half.",
				lore = "The sword appears to be from medieval ages, through it's not. It was made a lot later. It never was used as a weapon in battles, because it was merely a decoration.",
				tooltip = "Long swords are slow, have little block and consume a lot of stamina. They have extraordinary weapon range and high damage",
				Rarity = 3,
				minLevel = 13,
				maxLevel = 15,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.LongSword,
				icon = Res.ResourceLoader.GetTexture(89),
			};
			new BaseItem(new int[][]
		  {
				new int[] {25 },
				new int[] {25 ,62,63,64},
				new int[] {6,49},
				new int[] {22,0,25,1,2,3,4},
				new int[] {1,2,3,4},
				new int[] {39,40,41,42,44,8,18,65 },
		  })
			{
				name = "Longsword",
				description = "Sharp and long",
				lore = "The sword is in perfect contidion.",
				tooltip = "Long swords are slow, have little block and consume a lot of stamina. They have extraordinary weapon range and high damage",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 27,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.LongSword,
				icon = Res.ResourceLoader.GetTexture(89),
			};
			new BaseItem(new int[][]
		  {
				new int[] {25 },
				new int[] {6,49},
				new int[] {22,0,25,1,2,3,4},
				new int[] {1,2,3,4,8},
				new int[] {1,2,3,4,8},
				new int[] {5,6,45,46,16,8},
				new int[] {39,40,41,42,44,8 },
				new int[] {39,40,41,42,44,8,62,63,64 },
		  })
			{
				name = "Full Metal Sword",
				description = "It's sooo big...",
				lore = "A normal human cannot lift this.",
				tooltip = "Greatswords are giant, incredibly slow and hard hitting.",
				Rarity = 6,
				minLevel = 50,
				maxLevel = 52,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.GreatSword,
				icon = Res.ResourceLoader.GetTexture(88),
			};
			new BaseItem(new int[][]
		  {
				new int[] {25,1,3 },
				new int[] {25,22,1,3 },
				new int[] {49 },
				new int[] {14 },
				new int[] {14,1 },
				new int[] {14,31,49 },
				new int[] {14,18,49 },
				new int[] {38,36,1,3,4,5,6,16 ,62,63,64},
		  })
			{
				name = "The Leech",
				description = "Hey where did my health g- oh it's back...",
				lore = "",
				tooltip = "Greatswords are giant, incredibly slow and hard hitting.",
				Rarity = 6,
				minLevel = 60,
				maxLevel = 61,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.GreatSword,
				icon = Res.ResourceLoader.GetTexture(88),
			};
			new BaseItem(new int[][]
			{
				new int[] {1,2,3,4 },
				new int[] {1,2,3,15,4,0,0,0 },
				new int[] {12,13,1,2,3,4,5, },
				new int[] {18,16,23,26,19 },
				new int[] {18,16,23,26 },
				new int[] {34,44,45,46 },
				new int[] {2,23,26},
				new int[] {2,23,26,51},
				new int[] {2,23,26,20,16,15,60},
				new int[] {52,66,60,0,0},
				new int[] {66},
				  })
			{
				name = "Smokey's Sacred Quiver",
				description = "SmokeyTheBear died because he never used this item.",
				lore = "Smokey was the friend of allmighty Hazard, who can materialize any kind of weapon at the snap of his fingers. Hazard remebered Smokey's favourite playstyle and he gave him this as a gift to purge the sh** out of mutants.",
				tooltip = "This quiver makes you operate crossbows at 250% speed",
				Rarity = 7,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
				onEquip = () => ModdedPlayer.Stats.i_SmokeyCrossbowQuiver.value= true,
				onUnequip = () => ModdedPlayer.Stats.i_SmokeyCrossbowQuiver.value = false,
			};
			new BaseItem(new int[][]
		 {
				new int[] {0,42 },
				new int[] {50 },
				new int[] {43,16 },
				new int[] {1,0 },
		 })
			{
				name = "Broken shield",
				description = "",
				lore = "",
				tooltip = "Shields increase your block.",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};
			new BaseItem(new int[][]
	{
				new int[] {1,2,3,4,0,42 },
				new int[] {0,42 },
				new int[] {50 },
				new int[] {1,0 },
				new int[] {43,16 },
				new int[] {43,16,0,0 },
	})
			{
				name = "Shield",
				description = "",
				lore = "",
				tooltip = "Shields increase your block.",
				Rarity = 1,
				minLevel = 3,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};
			new BaseItem(new int[][]
	 {
				new int[] {16},
				new int[] {16},
				new int[] {16},
				new int[] {16,0},
				new int[] {16,0,45,46},
				new int[] {0,42,11 },
				new int[] {50 },
	 })
			{
				name = "Tower Shield",
				description = "",
				lore = "",
				tooltip = "Shields increase your block.",
				Rarity = 3,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};
			new BaseItem(new int[][]
				   {
				new int[] {5,6,7,8,0,0,0},
				new int[] {43},
				   })
			{
				name = "Broken Leather Shoulder Armor",
				description = "S",
				lore = "",
				tooltip = "Provides usefull stat bonuses",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};
			new BaseItem(new int[][]
		 {
				new int[] {5,6,7,8},
				new int[] {43},
		 })
			{
				name = "Leather Shoulder Armor",
				description = "",
				lore = "",
				tooltip = "Provides usefull stat bonuses",
				Rarity = 1,
				minLevel = 2,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};
			new BaseItem(new int[][]
		 {
				new int[] {16},
				new int[] {1,2,3,4},
				new int[] {17},
				new int[] {17},
				new int[] {8,9,49,47},
				new int[] {8,9,49,47},
				new int[] {16,18,11,34},
				new int[] {37,34},
				new int[] {-1},
		 })
			{
				name = "Phase Pauldrons",
				description = "The distance of teleport is increased by 40 meters, and teleport now hits everything that you teleported through",
				lore = "",
				tooltip = "Provides usefull stat bonuses",
				Rarity = 7,
				minLevel = 5,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
				onEquip = () => { ModdedPlayer.Stats.spell_blinkRange.Add(40); ModdedPlayer.Stats.spell_blinkDamage.Add(60); },
				onUnequip = () => { ModdedPlayer.Stats.spell_blinkRange.Substract( 40); ModdedPlayer.Stats.spell_blinkDamage.Substract( 60); },
			};
			new BaseItem(new int[][]
					  {
				new int[] {39,49,5,6,7,8,0,0,0},
				new int[] {43,0},
				new int[] {43},
					  })
			{
				name = "MAGA Cap",
				description = "Wearing this item channels the power of D.Trump to you",
				lore = "... or does it?",
				tooltip = "Provides usefull stat bonuses",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};
			new BaseItem(new int[][]
			{
				new int[] {2000},
				new int[] {2001},
				new int[] {2002},
				new int[] {2003},
				new int[] {16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {1,2,3,4},
				new int[] {21,6},
			})
			{
				name = "Hubble's Vision",
				description = "Wearing this item empowers your black hole spell",
				lore = "Man, fuck gravity.",
				tooltip = "I wish ModAPI supported custom shaders. I could make the blackhole look better. (hahahahahaha, i still managed to add them to the forest!, Seeking arrow's effect uses a custom shader!)",
				Rarity = 6,
				minLevel = 10,
				maxLevel = 11,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};
			new BaseItem(new int[][]
				   {
				new int[] {39,40,41,42,43,12,13},
				   })
			{
				name = "Broken Loop",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			};
			new BaseItem(new int[][]
			{
				new int[] {-1},
				new int[] {39,40,41,42,43,12,13},
			})
			{
				name = "Loop",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			};
			new BaseItem(new int[][]
			{
				new int[] {63},
				new int[] {-1},
				new int[] {39,40,41,42,43,12,13,5,6,7,8,9,12,13,15,16,17,18,10,11},
				new int[] {1,2,3,4,21,22,23,24,25,26,43,12,13},
			})
			{
				name = "Toxic Ring",
				description = "",
				lore = "What the fuck did you just fucking say about me, you little bitch? I'll have you know I graduated top of my class in the Navy Seals, and I've been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I'm the top sniper in the entire US armed forces. You are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out the pathetic little thing you call your life. You're fucking dead, kid. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that's just with my bare hands. Not only am I extensively trained in unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could have known what unholy retribution your little \"clever\" comment was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn't, you didn't, and now you're paying the price, you goddamn idiot. I will shit fury all over you and you will drown in it. You're fucking dead, kiddo.",
				tooltip = "",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			};
			new BaseItem(new int[][]
					  {
				new int[] {39,40,41,42,43},
				new int[] {-1},
					  })
			{
				name = "Scarf",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(100),
			};
			new BaseItem(new int[][]
					{
				new int[] {39,40,41,42,43},
				new int[] {43},
					})
			{
				name = "Damaged Bracer",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new int[][]
		   {
				new int[] {39,40,41,42,43},
				new int[] {43},
				new int[] {16},
		   })
			{
				name = "Worn Bracer",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 1,
				minLevel = 3,
				maxLevel = 10,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new int[][]
		   {
				new int[] {39,40,41,42,43},
				new int[] {16},
				new int[] {-1},
				new int[] {5,6,7,8,9,10},
		   })
			{
				name = "Leather Bracer",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 2,
				minLevel = 4,
				maxLevel = 10,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(94),
			};
			new BaseItem(new int[][]
					  {
				new int[] {32},
					  })
			{
				name = "Greater Mutated Heart",
				description = "",
				lore = "",
				tooltip = "Can be consumed by right clicking it",
				Rarity = 6,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = true,
				StackSize = 100,
				type = BaseItem.ItemType.Other,
				icon = Res.ResourceLoader.GetTexture(105),
			};
			new BaseItem(new int[][]
			{
				new int[] {33},
			})
			{
				name = "Lesser Mutated Heart",
				description = "",
				lore = "",
				tooltip = "Can be consumed by right clicking it",
				Rarity = 5,
				minLevel = 1,
				maxLevel = 6,
				CanConsume = true,
				StackSize = 1,
				type = BaseItem.ItemType.Other,
				icon = Res.ResourceLoader.GetTexture(105),
			};
			new BaseItem(new int[][]
		{
				new int[] {1,2},
				new int[] {1,2,3,5,6},
				new int[] {65},
				new int[] {-1},
				new int[] {-1},
				new int[] {53,54},
		})
			{
				name = "Spiked ring",
				description = "Armor piercing for either melee or ranged weapons",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 10,
				maxLevel = 16,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			};

			new BaseItem(new int[][]
			{
				new int[] {1,2,3,4,5,6},
				new int[] {1,2,3,4,5,6,65},
				new int[] {-1},
				new int[] {-1 },
				new int[] {1,2,3,4,21,22,23,24,25,26,18,16},
				new int[] {55},
			})
			{
				name = "Piercer",
				description = "",
				lore = "",
				tooltip = "Provides stats and armor piercing",
				Rarity = 4,
				minLevel = 11,
				maxLevel = 15,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101),
			};
			new BaseItem(new int[][]
			{
				new int[] {59 },
				new int[] {21 },
				new int[] {34,0,40,41 },
				new int[] {16,34},
				new int[] {12 },
				new int[] {-1 },
			})
			{
				name = "Moon Boots",
				description = "A pair of boots from the moon.",
				lore = "It is said that the wearer will not take fall damage while wearing these boots and will jump like on the moon, I wouldn't trust it tough.",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85), //icon ids, dont worry about that
			};
			new BaseItem(new int[][]
			{
				new int[] {1},
				new int[] {1,57,18,36},
				new int[] {12,13,1},
				new int[] {22,25,1},
				new int[] {22,25,1},
				new int[] {50,53,35},
			})
			{
				name = "Golden Ring of Strength",
				description = "A Ring of ancient times.",
				lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};
			new BaseItem(new int[][]
			{
				new int[] {3},
				new int[] {3,31,6,7,8,9},
				new int[] {5,3,41,45},
				new int[] {7,10,31,14},
				new int[] {14,16,11,17},
				new int[] {65,57,45,46},
			})
			{
				name = "Golden Ring of Vitality",
				description = "A Ring of ancient times.",
				lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
				new int[] {2},
				new int[] {12,13,8,9, },
				new int[] {15,18,34,36},
				new int[] {23,48,54,26,59,55,16},
				new int[] {6,57,2,34,},
				new int[] {52,66,51,2,23}
			})
			{
				name = "Golden Ring of Agility",
				description = "A Ring of ancient times.",
				lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};
			new BaseItem(new int[][]
			{
				new int[] {4},
				new int[] {12,13,21,24,6},
				new int[] {12,13,21,24},
				new int[] {19,47,49},
				new int[] { 37,38,4,24,61,44},
				new int[] { 57,44,6,24,21,47}
			})
			{
				name = "Golden Ring of Intelligence",
				description = "A Ring of ancient times.",
				lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};
			//Silver Rings---------------------------------------------------------------------------
			new BaseItem(new int[][]
			  {
				new int[] {1},
				new int[] {22,25,12,13},
				new int[] {35,50,53},
				new int[] {20,0,0,0}
			  })
			{
				name = "Silver Ring of Strength",
				description = "A Ring of ancient times.",
				lore = "A Silver Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			  {
				new int[] {3},
				new int[] {7,10,31,5, },
				new int[] {11,17,0},
				new int[] {14,16,45},
			  })
			{
				name = "Silver Ring of Vitality",
				description = "A Ring of ancient times.",
				lore = "A Silver Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			  {
				new int[] {2},
				new int[] {12,13,51,8,9, },
				new int[] {15,18,34,36,0},
				new int[] {23,48,54,26,6,57,0,0,0 }
			  })
			{
				name = "Silver Ring of Agility",
				description = "A Ring of ancient times.",
				lore = "A Silver Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			  {
				new int[] {4},
				new int[] { 12, 13, 21, 24,12, 13,21,24,0},
				new int[] {19,47,49,6},
				new int[] { 57,37,38,0},
			  })
			{
				name = "Silver Ring of Intelligence",
				description = "A Ring of ancient times.",
				lore = "A Silver Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			//Steel Rings-------------------------------------------------------------------
			new BaseItem(new int[][]
			  {
				  new int[] {1},
				  new int[] {12,13,65},
				  new int[] {22,25, 57,35,50,53,20},
			  })
			{
				name = "Steel Ring of Strength",
				description = "A Ring of ancient times.",
				lore = "A Steel Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			  {
				new int[] {3},
				new int[] {7,10,31,5,65},
				new int[] {14,16, 45,11,17,0},
			  })
			{
				name = "Steel Ring of Vitality",
				description = "A Ring of ancient times.",
				lore = "A Steel Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			  {
				new int[] {2},
				new int[] {8,9, 12,13,51,57},
				new int[] {23,54,26,59,18,34},
			  })
			{
				name = "Steel Ring of Agility",
				description = "A Ring of ancient times.",
				lore = "A Steel Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 10,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			  {
				new int[] {4},
				new int[] {12,13,21,24,6},
				new int[] {19,47,49,57,37,38,21,24},
			  })
			{
				name = "Steel Ring of Intelligence",
				description = "A Ring of ancient times.",
				lore = "A Steel Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			//One Ring to rule them all----------------------------------------------------------

			new BaseItem(new int[][]
			  {
				new int[] {1,3,2,4},
				new int[] {12,13},
				new int[] {22,25,30,},
				new int[] {35,50,53,20},
				new int[] {5,28},
				new int[] {7,10,31},
				new int[] {11,17,14,16 },
				new int[] {8,9,27,45},
				new int[] {51,52,66},
				new int[] {15,18,34,36,57},
				new int[] {23,48,54,26},
				new int[] {21,24},
				new int[] {19,47,49,57,6},
				new int[] {29,37,38,57},
				new int[] {65},
			  })
			{
				name = "The One Ring To Rule Them All",
				description = "An Ancient magical Ring of great power.",
				lore = "It looks like and ordinay ring, but a strange energy is surrounding it. The Ring is said to have been found inside a volcanic rock by an archeologist, who went mad and isolated himself on the peninsula many years ago. But that's just a fairy tale, ring?",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 20,
				maxLevel = 30,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};

			//Golden Lockets---------------------------------------------------------------------

			new BaseItem(new int[][]
			{
			new int[] {1},
			new int[] {12,13},
			new int[] {22,25,57,},
			new int[] {35,50,53},
			new int[] {65,1,57,47,34,36,18}
			})
			{
				name = "Golden Locket of Strength",
				description = "A Locket of ancient times.",
				lore = "A Golden Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 3,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {3},
			new int[] {5,6,7},
			new int[] {7,10,31},
			new int[] {11,17},
			new int[] {14,16,45},
			})
			{
				name = "Golden Locket of Vitality",
				description = "A Locket of ancient times.",
				lore = "A Golden Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 3,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {2},
			new int[] {12,13,51,52,66,8,9,},
			new int[] {12,13,51,52,66},
			new int[] {23,48,54,26},
			new int[] {57,18,47}
			})
			{
				name = "Golden Locket of Agility",
				description = "A Locket of ancient times.",
				lore = "A Golden Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 3,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {4},
			new int[] {12,13,21,24},
			new int[] {12,13,21,24,19,47,49,},
			new int[] {57,37,38,6},
			new int[] {4,37,38,}
			})
			{
				name = "Golden Locket of Intelligence",
				description = "A Locket of ancient times.",
				lore = "A Golden Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 3,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			//Silver Lockets---------------------------------------------------------------------------

			new BaseItem(new int[][]
			{
			new int[] {1},
			new int[] {12,13},
			new int[] {22,25,57,},
			new int[] {35,50,53,0},
			new int[] {20,0,0,0}
			})
			{
				name = "Silver Locket of Strength",
				description = "A Locket of ancient times.",
				lore = "A Silver Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {3},
			new int[] {7,10,31,5,},
			new int[] {11,17,0},
			new int[] {14,16,0},
			new int[] {45,0,0,0}
			})
			{
				name = "Silver Locket of Vitality",
				description = "A Locket of ancient times.",
				lore = "A Silver Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {2},
			new int[] {12,13,51,52,66,8,9,6,},
			new int[] {12,13,51,52,66,0},
			new int[] {15,18,34,36,0},
			new int[] {23,48,54,26},
			})
			{
				name = "Silver Locket of Agility",
				description = "A Locket of ancient times.",
				lore = "A Silver Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {4},
			new int[] {12,13,21,24},
			new int[] {12,13,21,24,6},
			new int[] {19,47,49,0},
			new int[] {57,37,38,0},
			})
			{
				name = "Silver Locket of Intelligence",
				description = "A Locket of ancient times.",
				lore = "A Silver Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			// Emerald Pendant-----------------------------------------------------------

			new BaseItem(new int[][]
			{
			new int[] {1},
			new int[] {12,13},
			new int[] {22,25,57,1},
			new int[] {35,50,53},
			new int[] {36,65,22,25},
			new int[] {11,18,37,6,8},
			})
			{
				name = "Emerald Pendant of Strength",
				description = "A Pendant of ancient times.",
				lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {3},
			new int[] {7,10,31,5},
			new int[] {11,17},
			new int[] {14,16,57,55},
			new int[] {5,7,10,3,31},
			new int[] {57,65},
			})
			{
				name = "Emerald Pendant of Vitality",
				description = "A Pendant of ancient times.",
				lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {2},
			new int[] {8,9,12,13,51,52,66},
			new int[] {12,13,51,52,66},
			new int[] {15,18,34,36},
			new int[] {23,48,54,26},
			new int[] {52,66,23,26,2},
			new int[] {-1},
			})
			{
				name = "Emerald Pendant of Agility",
				description = "A Pendant of ancient times.",
				lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {4},
			new int[] {29,37,21,24,46,56,19},
			new int[] {12,13,21,24,6},
			new int[] {21,4,47,49},
			new int[] {4,57,47,24},
			new int[] {-1},
			new int[] {-1},
			})
			{
				name = "Emerald Pendant of Intelligence",
				description = "A Pendant of ancient times.",
				lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			// Diamond Pendant-----------------------------------------------------------

			new BaseItem(new int[][]
			{
			new int[] {1},
			new int[] {12,13},
			new int[] {22,25,1,},
			new int[] {35,50,53, },
			new int[] {65,30},
			new int[] {-1},
			new int[] {-1},
		   })
			{
				name = "Diamond Pendant of Strength",
				description = "A Pendant of ancient times.",
				lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 12,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {3},
			new int[] {5,6,7,8,9,10},
			new int[] {7,10,31},
			new int[] {11,17},
			new int[] {14,16},
			new int[] {28,11},
			new int[] {-1},
			new int[] {-1},
			})
			{
				name = "Diamond Pendant of Vitality",
				description = "A Pendant of ancient times.",
				lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 12,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {2},
			new int[] {8,9,12,13,51,52,66},
			new int[] {12,13,51,52,66,15,18,34,36 },
			new int[] {23,48,54,26},
			new int[] {54,52,66,2},
			new int[] {2,18,38},
			new int[] {57,2,6},
			new int[] {-1},
			})
			{
				name = "Diamond Pendant of Agility",
				description = "A Pendant of ancient times.",
				lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 12,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			new BaseItem(new int[][]
			{
			new int[] {4},
			new int[] {12,13,21,24},
			new int[] {12,13,21,24,6,65},
			new int[] {19,47,49,11,5,6,7,8,46,34},
			new int[] {29,4},
			new int[] {21,4,47,49},
			new int[] {4,57,47,24},
			new int[] {-1},
			new int[] {-1},
			})
			{
				name = "Diamond Pendant of Intelligence",
				description = "A Pendant of ancient times.",
				lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 12,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};

			//Rare Amulets -----------------------------------------------------------------------------------------

			var armsyFingerNecklace = new BaseItem(new int[][]
			{
			new int[] {1},
			new int[] {65},
			new int[] {18,11},
			new int[] {-1},
			new int[] {12,13},
			new int[] {22,25,30,},
			new int[] {35,50,53,57},
			new int[] {20,57}
			})
			{
				name = "Armsy Finger Necklace",
				description = "A Necklace decorated with armsy fingertips.",
				lore = "A Necklace made from the fingertips of an armsy, yeilding it's raw power and strentgh.",
				tooltip = "Necklaces give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that

			};
			armsyFingerNecklace.DropSettings_OnlyArmsy();
			armsyFingerNecklace.PossibleStats[0][0].Multipier = 2;
			armsyFingerNecklace.PossibleStats[1][0].Multipier = 2;
			var virginiaHeartPedant = new BaseItem(new int[][]
			{
			new int[] {2},
			new int[] {23},
			new int[] {48},
			new int[] {-1},
			new int[] {5,28},
			new int[] {7,10,31},
			new int[] {11,17,57},
			new int[] {14,16,57},
			new int[] {45,57}
			})
			{
				name = "Virginia Heart Pendant",
				description = "A Pendant of a petrified Virginia heart.",
				lore = "A Pendant made from a petrified Virginia heart, yeilding it's love and Vitality.",
				tooltip = "Pendants give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};
			virginiaHeartPedant.DropSettings_OnlyVags();
			virginiaHeartPedant.PossibleStats[0][0].Multipier = 2;
			virginiaHeartPedant.PossibleStats[1][0].Multipier = 2.25f;

			var cowmanToeNecklace = new BaseItem(new int[][]
			{
			new int[] {3},
			new int[] {31,6},
			new int[] {28},
			new int[] {8,9,27},
			new int[] {12,13,51,52,66},
			new int[] {12,13,51,52,66,57},
			new int[] {15,18,34,36,57},
			new int[] {23,48,54,26},
			new int[] {65,57}
			})
			{
				name = "Cowman Toe Necklace",
				description = "A Necklace decorated with cowman toes.",
				lore = "A Necklace made from the fingertips of an armsy, yeilding it's speed and agility.",
				tooltip = "Necklaces give base stats to make your stronger.",
				Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 20,
				maxLevel = 40,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};
			cowmanToeNecklace.DropSettings_OnlyCow();
			cowmanToeNecklace.PossibleStats[0][0].Multipier = 3;
			cowmanToeNecklace.PossibleStats[1][0].Multipier = 2;
			new BaseItem(new int[][]
			{
			new int[] {47},
			new int[] {4,0},
			new int[] {21,0},
			new int[] {38,31,49,14},
			new int[] {-1},
			new int[] {-1},
			new int[] {-1},
			new int[] {12,13,21,24,6},
			new int[] {19,47,49,6},
			new int[] {29,37,38},
			new int[] {29,37,38,},
			})
			{
				name = "Pendant of Perpetual Rebirth",
				description = "A Pendant of a shrunken babyhead. It's unique legendary ability causes you to decrease a random cooldown by 1 second whenever you hit something with melee or ranged attack.",
				lore = "A pedant of great power, drops only from babies",
				tooltip = "Necklaces give base stats to make your stronger. ",
				Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 30,
				maxLevel = 40,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
				onEquip = () => ModdedPlayer.Stats.i_infinityLoop.value= true,
				onUnequip = () => ModdedPlayer.Stats.i_infinityLoop.value = false,
			}.DropSettings_OnlyBaby();

			//Boss drop Amulet----------------------------------------------------------------------------------------

			new BaseItem(new int[][]
			{
			new int[] {1,2,4},
			new int[] {12,13},
			new int[] {22,25,30,18,5,28},
			new int[] {35,50,53,57,56,20,57,19,18},
			new int[] {7,10,31},
			new int[] {-1},
			new int[] {-1},
			new int[] {45,16,10,11,9,8, 14, 16, 57,11, 17,57},
			new int[] { 51, 52,66,8, 9,27},
			new int[] {15,18,34,36,57},
			new int[] {23,48,54,26},
			new int[] {6,55,46,54,53},
			new int[] {19,47,49,57, 21,24,29,37,38,57},
			})
			{
				name = "Megan's Locket",
				description = "The Locket Megan wore.",
				lore = "Megan wore this Locket, it has a picture of her mom in it.",
				tooltip = "lockets give base stats to make your stronger.",
				Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			}.DropSettings_OnlyMegan();

			BaseItem RelicHammer = new BaseItem(new int[][]
		{
				new int[] {25 },
				new int[] {18 },
				new int[] {2004 },
				new int[] {1,62,63,64 },
		})
			{
				name = "Relic Hammer",
				description = "It's slow and weak.",
				lore = "",
				tooltip = "Hammers stun enemies with every hit, but are slow. They also deal high smash damage",
				Rarity = 2,
				minLevel = 20,
				maxLevel = 22,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Hammer,
				icon = Res.ResourceLoader.GetTexture(109),
			};
			RelicHammer.PossibleStats[1][0].Multipier = -4;

			BaseItem GreaterHammer = new BaseItem(new int[][]
		{
				new int[] {25 },
				new int[] {18 },
				new int[] {2004 },
				new int[] {1,3,62,63,64},
				new int[] {53,16},
				new int[] {39,31,43,0,0},
				new int[] {25 ,22,1,12,13,5,6},
		})
			{
				name = "Black Hammer",
				description = "It's slow but with enough strength i can make it a very deadly tool",
				lore = "",
				tooltip = "Hammers stun enemies with every hit, but are slow. They also deal high smash damage",
				Rarity = 4,
				minLevel = 30,
				maxLevel = 35,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Hammer,
				icon = Res.ResourceLoader.GetTexture(109),
			};
			GreaterHammer.PossibleStats[1][0].Multipier = -3;
			//Item 0/6
			new BaseItem(new int[][]
			{
					new int[] {23,26},
					new int[] {2,6,4},
			})
			{
				name = "Potato Sack",
				description = "Can be used as a quiver",
				lore = "",
				tooltip = "Quivers increase projectile damage",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
			};

			//Item 1/6
			new BaseItem(new int[][]
			{
					new int[] {23,26},
					new int[] {40,41,42},
					new int[] {40,16,60},
					new int[] {2},
			})
			{
				name = "Rabbit Skin Quiver",
				description = "",
				lore = "",
				tooltip = "Quivers increase player's ranged damage",
				Rarity = 1,
				minLevel = 2,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
			};

			//Item 2/6
			new BaseItem(new int[][]
			{
					new int[] {26},
					new int[] {23,2,54},
					new int[] {18,60,61},
					new int[] {40,41,16,5,6,40},
					new int[] {-1},
			})
			{
				name = "Hollow Log",
				description = "It allows for faster drawing of arrow than a cloth quiver",
				lore = "",
				tooltip = "Quiver increase projectile damage",
				Rarity = 2,
				minLevel = 6,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
			};

			//Item 3/6
			new BaseItem(new int[][]
			{
					new int[] {26,23},
					new int[] {24,21},
					new int[] {17,16,18,54,51,52,66},
					new int[] {2,3,4,15,14,13,12,11,10},
					new int[] {5,6,47,60,61},
					new int[] {2,3,4,5,6,7,8,11,12,16,18,37},
			})
			{
				name = "Magic Quiver",
				description = "",
				lore = "",
				tooltip = "This item boosts both magic and ranged damage",
				Rarity = 3,
				minLevel = 6,
				maxLevel = 11,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
			};

			//Item 4/6
			new BaseItem(new int[][]
			{
					new int[] {23,26},
					new int[] {23},
					new int[] {2,3,4},
					new int[] {34,18,17,16,15,14,60,61,55,},
					new int[] {16,19,23,31,54,51,52,66,57},
					new int[] {2,0},
					new int[] {2,3,4,5,6,7,8,9,10},
					new int[] {2,1,5,6},
					new int[] {-1},
			})
			{
				name = "Long Lost Quiver",
				description = "",
				lore = "",
				tooltip = "A quiver that greatly increases ranged damage",
				Rarity = 5,
				minLevel = 12,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
			};

			//Item 5/6
			new BaseItem(new int[][]
			{
					new int[] {37, 24,47},
					new int[] {42,6,17,61},
					new int[] {-1},
					new int[] {4,0},
			})
			{
				name = "Spell Scroll",
				description = "Contains a lot of information on how to properly cast spells to achieve better results",
				lore = "",
				tooltip = "Spell Scrolls grant magic buffs",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
			};

			new BaseItem(new int[][]
			{
				new int[] {16,43},
				new int[] {43,39,40,41,42},
				new int[] {43,39,40,41,42},
				new int[] {43,0,0,0},
			})
			{
				name = "Cloth Pants",
				description = "Offer little protction",
				lore = "",
				tooltip = "They provide armor which gives damage reduction",
				Rarity = 1,
				minLevel = 2,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};

			//Item 1/7
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {1,2,3,4},
				new int[] {5,6},
				new int[] {16,43,39,40,41,42},
				new int[] {1000,1001,1002,1003,1004,43,0,0,0},
			})
			{
				name = "Rough Hide Leggins",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};

			//Item 2/7
			new BaseItem(new int[][]
			{
				new int[] {16,},
				new int[] {1,2,3,4},
				new int[] {5,44,7,8},
				new int[] {6,16,3},
				new int[] {1,2,3,4,11},
				new int[] {17,16,10,9},
				new int[] {16,43},
			})
			{
				name = "Plate Leggins",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 4,
				maxLevel = 10,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};

			//Item 3/7
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {19},
				new int[] {1,2,3,4,5,6,7,8},
				new int[] {39,40,41,42,43},
				new int[] {4},
				new int[] {-1},
			})
			{
				name = "Sage's Robes",
				description = "",
				lore = "",
				tooltip = "Sage's Robes grant bonus to experience gained. This amplifies experience gained.",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};

			//Item 4/7
			new BaseItem(new int[][]
			{
				new int[] {1,2,3,4},
				new int[] {1,5},
				new int[] {16},
				new int[] {22,25},
				new int[] {11,12,13,14,5,6,1,2,3,4},
				new int[] {7,8,9,10,44,45,46,49},
				new int[] {31,1,3,},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "Hammer Jammers",
				description = "Increases the damage of your hammer smash by 250%, hammer stun duration is doubled",
				lore = "",
				tooltip = "",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 28,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
				onEquip = () => { ModdedPlayer.Stats.smashDamage.Multiply( 2.5f); ModdedPlayer.Stats.i_HammerStunAmount.Multiply(2); },
				onUnequip = () => { ModdedPlayer.Stats.smashDamage.Divide(2.5f); ModdedPlayer.Stats.i_HammerStunAmount .Divide(2); },
			};

			//Item 5/7
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {34},
				new int[] {1,2,4,6,7,8},
				new int[] {-1},
				new int[] {26,23,24,21},
				new int[] {1000, 1001,1002, 1003, 1004, 0,0,0,1,2,4},
				new int[] {51,1,2,3,4,55},
			})
			{
				name = "Pirate Pants",
				description = "Those pants are ligh and comfortable. They offer plenty of mobility but lack in protection.",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};

			//Item 6/7
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {1,2,3,4,16,17},
				new int[] {18,34},
				new int[] {1,2,3,4},
				new int[] {5,6,15,16,13,12,11},
				new int[] {8,4,2,9},
				new int[] {22,21,23},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "Hexed Pants of Mr M.",
				description = "They look like yoga pants but for a man the size of a wardrobe",
				lore = "Once upon a time there was a man who was in a basement and fed himself with nothing but nuggets. He got so obese that friends and family started worrying. Hazard noticed this man and cursed his pants to force him to excercise.",
				tooltip = "While moving with those pants on, energy regeneration and damage is increased by 40%. While standing still for longer than a second, you loose 1.5% of max health per second.",
				Rarity = 7,
				minLevel = 14,
				maxLevel = 15,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
				onEquip = () => ModdedPlayer.Stats.i_HexedPantsOfMrM_Enabled.value = true,
				onUnequip = () =>  ModdedPlayer.Stats.i_HexedPantsOfMrM_Enabled.value = false,
			};
			//Created using Hazard's app

			//    - ITEMS

			//Item 0/6
			new BaseItem(new int[][]
			{
new int[] {39,40,41,42,43},
new int[] {39,40,41,42,43},
			})
			{
				name = "Leather Mantle",
				description = "A piece of cloth to give protection from ",
				lore = "",
				tooltip = "Offers bonuses to attributes and armor",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};

			//Item 1/6
			new BaseItem(new int[][]
			{
new int[] {16},
new int[] {16},
new int[] {1,2,3,4,5,6},
new int[] {39,40,41,42,43},
			})
			{
				name = "Shoulder Guards",
				description = "Medium armor piece.",
				lore = "Heavy armor",
				tooltip = "Shoulder pads offer armor and stat bonuses",
				Rarity = 2,
				minLevel = 4,
				maxLevel = 7,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};

			//Item 2/6
			BaseItem Heavy_Shoulder_Plates = new BaseItem(new int[][]
			{
				new int[] {34},
				new int[] {18},
				new int[] {16},
				new int[] {16,65},
				new int[] {1,2,3,4},
				new int[] {1,2,3,4,5,8,9,7,10},
				new int[] {17,10,5,8,9,7,10},
				new int[] {5,45,3},
				new int[] {11},
			})
			{
				name = "Heavy Shoulder Plates",
				description = "Heavy armor piece. They offer great protection at the cost of attack speed and movement speed decrease",
				lore = "",
				tooltip = "Shoulder pads offer armor and stat bonuses",
				Rarity = 4,
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};
			Heavy_Shoulder_Plates.PossibleStats[0][0].Multipier = -1;
			Heavy_Shoulder_Plates.PossibleStats[1][0].Multipier = -1;
			Heavy_Shoulder_Plates.PossibleStats[2][0].Multipier = 3;

			//Item 3/6
			new BaseItem(new int[][]
			{
				new int[] {21,22,23,24,25,26},
				new int[] {16},
				new int[] {1,2,3,4},
				new int[] {1,2,3,4,16,39,40,41,42,43},
				new int[] {1,2,3,4,16,39,40,41,42,43},
			})
			{
				name = "Etched Mantle",
				description = "Those pauldrons empower wearer's combat skill",
				lore = "",
				tooltip = "Offers bonuses to attributes and armor",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};

			//Item 4/6
			new BaseItem(new int[][]
			{
				new int[] {22,25},
				new int[] {1,2,3,4},
				new int[] {16},
				new int[] {12,11,13,14},
				new int[] {5,6},
				new int[] {10,15,16,17,18,19,31,35,36,44,45,46,47,49,50,53,55},
				new int[] {10,15,16,17,18,19,31,35,36,44,45,46,47,49,50,53,55},
				new int[] {53,55},
			})
			{
				name = "Assassins Pauldrons",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 4,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};

			//Item 5/6
			new BaseItem(new int[][]
			{
				new int[] {11},
				new int[] {1,2,3,4},
				new int[] {16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {21,22,23,24,25,26},
				new int[] {5,14,7,10,45},
				new int[] {1,2,3,4},
				new int[] {12,13,15,16,18},
				new int[] {17,19,21,22,23},
				new int[] {37,35,36,38,44,45,47},
				new int[] {1,2,4},
			})
			{
				name = "Death Pact",
				description = "This set of shoulder armor contains a unique ability - every attack you make decreases your health by 7% of max health. For every percent of missing health you gain 3% damage amplification. This damage cannot kill you.",
				lore = "",
				tooltip = "This item will make you a glass cannon.",
				Rarity = 7,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
				onEquip = () => ModdedPlayer.Stats.i_DeathPact_Enabled.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_DeathPact_Enabled.value = false,
			};
			new BaseItem(new int[][]
			{
				new int[] {56},
				new int[] {-1},
				new int[] {-1},
				new int[] {1,2,3,4},
				new int[] {11,12,13,14,15,16,17,18},
				new int[] {11,12,13,14,15,16,17,18},
				new int[] {5,6,7,8,9,10,31},
				new int[] {55,54,53,50},
				new int[] {1,2,3,4,21,22,23,24,25,26},
				new int[] {16,0,0,0,1,2,3,4,0,0,0,0},
			})
			{
				name = "Maximale Qualitöt",
				description = "A platinum ring with the most expensive jewels engraved on it. It's quality is uncomparable.",
				lore = "",
				tooltip = "This item increases magic find - increases the value of dropped items. Magic find does not increase the chance to get a higher tier item.",
				Rarity = 6,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			};
			new BaseItem(new int[][]
						 {
						 })
			{
				name = "Heart of Purity",
				description = "A object filled with both destructive and creative energy. Allows to re-assign all spent mutation points",
				lore = "This powerful relic contains so much power, that it can kill anything and force it to come back to life, resulting in it's rebirth.",
				tooltip = "Can be consumed by right clicking it. ",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = true,
				StackSize = 100,
				type = BaseItem.ItemType.Other,
				icon = Res.ResourceLoader.GetTexture(105),
				onConsume = ModdedPlayer.Respec
			};

			new BaseItem(new int[][]
			{
new int[] {1,2,3,4,57},
new int[] {16,17,14},
new int[] {50,11},
new int[] {49,39,40,41,42,45,44},
new int[] {5,6,9,8,10,12,13,14,},
			})
			{
				name = "Round Shield",
				description = "A sturdy shield made of wood and reinforced with iron.",
				lore = "",
				tooltip = "",
				Rarity = 2,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};

			//Item 1/5
			new BaseItem(new int[][]
			{
new int[] {1,11,5,7},
new int[] {57,2,3,4,5,6,7,8,10,11},
new int[] {39,40,41,42,43,44,45,46},
new int[] {50},
			})
			{
				name = "Old Buckler",
				description = "An old shield.",
				lore = "This item has a lot of scratches that look like they were made by something with sharp claws.",
				tooltip = "Shields increase block and provide usefull stats.",
				Rarity = 1,
				minLevel = 4,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};

			//Item 2/5
			new BaseItem(new int[][]
			{
new int[] {16},
new int[] {16,50},
new int[] {-1},
new int[] {-1},
new int[] {-1},
new int[] {11},
new int[] {39,40,41,42,43,50,57},
new int[] {39,40,41,42,43,50,57},
			})
			{
				name = "Dark Oak Shield",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};

			//Item 3/5
			new BaseItem(new int[][]
			{
new int[] {15,14},
new int[] {2,3,4,1,41,42,57},
new int[] {-1},
new int[] {-1},
new int[] {-1},
new int[] {65,1,16,25},
new int[] {2,4,5,6},
new int[] {16,7,8,22,23,25,26},
			})
			{
				name = "Bone Shield",
				description = "A shield made of bones, held together by thick steel wire.",
				lore = "",
				tooltip = "",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			};

			new BaseItem(new int[][]
			{
new int[] {18},
new int[] {0,0,0,0,62,63,64},
			})
			{
				name = "Dull Longsword",
				description = "It's round on the edges",
				lore = "",
				tooltip = "",
				Rarity = 0,
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.LongSword,
				icon = Res.ResourceLoader.GetTexture(89),
			}.PossibleStats[0][0].Multipier = -3;

			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {65},
			})
			{
				name = "Iron Horn",
				description = "Wielder of this item has empowered warcry ability - all allies recieve armor bonus equal to 10% of your armor",
				lore = "",
				tooltip = "Has a special ability",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101),
				onEquip = () => ModdedPlayer.Stats.spell_warCryGiveArmor.value= true,
				onUnequip = () => ModdedPlayer.Stats.spell_warCryGiveArmor.value = false,
			}.PossibleStats[0][0].Multipier = 2;

			//Item 1/5
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {31,7,8,9,10},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "The Great Horn",
				description = "",
				lore = "",
				tooltip = "Wielder of this item has empowered warcry ability - all allies recieve armor bonus equal to 10% of your armor",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101),
				onEquip = () => ModdedPlayer.Stats.spell_warCryGiveArmor.value = true,
				onUnequip = () => ModdedPlayer.Stats.spell_warCryGiveArmor.value = false,
			}.PossibleStats[0][0].Multipier = 5;

			//Item 2/5
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {1},
				new int[] {65,0},
				new int[] {5,16,18},
				new int[] {21,22,23,0,0,0},
				new int[] {24,25,26,0,0,0},
			})
			{
				name = "Horned Helmet",
				description = "A viking helmet",
				lore = "",
				tooltip = "",
				Rarity = 2,
				minLevel = 2,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};

			//Item 3/5
			new BaseItem(new int[][]
			{
				new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
				new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
				new int[] {-1},
			})
			{
				name = "Mask",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 2,
				minLevel = 1,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};

			//Item 4/5
			BaseItem mask = new BaseItem(new int[][]
			  {
				new int[] {18},
				new int[] {22,23,21},
				new int[] {11},
				new int[] {1,2,3,4,5,6,7,8,9,10,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
				new int[] {1,2,3,4,5,6,7,8,9,10,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
				new int[] {1,2,3,4,5,6,7,8,9,10,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
				new int[] {24,25,26,0,0,0},
				new int[] {29,30,48},
			  })
			{
				name = "Mask of Madness",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 2,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};
			mask.PossibleStats[2][0].Multipier = -4;
			mask.PossibleStats[0][0].Multipier = 1.5f;
			mask.PossibleStats[1][0].Multipier = 2.5f;
			mask.PossibleStats[1][1].Multipier = 2.5f;
			mask.PossibleStats[1][2].Multipier = 2.5f;

			new BaseItem(new int[][]
			 {
				new int[] {47,49,37,38},
				new int[] {42,4},
				new int[] {44},
				new int[] {21},
				new int[] {-1},
			 })
			{
				name = "Old Scroll",
				description = "Contains a lot of information on how to properly cast spells to achieve better results",
				lore = "",
				tooltip = "Spell Scrolls grant magic buffs",
				Rarity = 3,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
			};
			new BaseItem(new int[][]
			{
				new int[] {57},
				new int[] {1,2,3,4},
				new int[] {5,46},
				new int[] {6,45},
				new int[] {21,24,11,12,13,14,15,16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {4,18,7,8,19},
				new int[] {27,28,29,30,48,47},
			})
			{
				name = "Wormhole Stabilizators",
				description = "High-tech gear",
				lore = "Hazard remember to put some fucking lore in here, dont leave it like this!",
				tooltip = "Increases the duration of a portal to 10 minutes",
				Rarity = 7,
				minLevel = 4,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(94),
				onEquip = () => ModdedPlayer.Stats.spell_portalDuration.Add(570),
				onUnequip = () => ModdedPlayer.Stats.spell_portalDuration.Substract(570),
			};
			new BaseItem(new int[][]
			{
				new int[] {57},
				new int[] {1,2,3,4},
				new int[] {5,46},
				new int[] {6,45},
				new int[] {21,24,11,12,13,14,15,16},
				new int[] {16},
				new int[] {17},
				new int[] {4,18,7,8,19},
				new int[] {27,28,29,30,48,47},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "Cripplers",
				description = "",
				lore = "",
				tooltip = "Increases the duration of a magic arrow's negative effect by 10 seconds",
				Rarity = 7,
				minLevel = 3,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
				onEquip = () => ModdedPlayer.Stats.spell_magicArrowDuration.Add( 10),
				onUnequip = () => ModdedPlayer.Stats.spell_magicArrowDuration.Substract(10),
			};

			new BaseItem(new int[][]
			{
				new int[] {24,4},
				new int[] {26,4},
				new int[] {21,2},
				new int[] {23,2,0,0,0,0},
				new int[] {2,4,57,16},
				new int[] {6,8,9,44,46},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {12,13,14,15,16,18},
			})
			{
				name = "Crossfire",
				description = "Infused with powerful magic. This item is a dangerous tool of destruction.",
				lore = "",
				tooltip = "When hitting an enemy with a projectile, create a magic arrow pointed at the enemy and shoot it without using in energy. This effect may occur once every 10 seconds. The magic arrow deals half the damage and applies stats that a normal magic arrow would.",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
				onEquip = () => ModdedPlayer.Stats.i_CrossfireQuiver.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_CrossfireQuiver.value = false,
			};

			new BaseItem(new int[][]
			{
				new int[] {44},
				new int[] {44,8},
				new int[] {44,4,6,9,4},
				new int[] {49,7,0},
				new int[] {-1},
			})
			{
				name = "Scroll of Recovery",
				description = "Recovers health and stamina",
				lore = "",
				tooltip = "Scroll is an offhand",
				Rarity = 1,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
			};

			new BaseItem(new int[][]
			{
				new int[] {11},
				new int[] {-1},
				new int[] {16,15},
				new int[] {37,38},
				new int[] {42,24},
			})
			{
				name = "Tiara",
				description = "A beautiful tiara ",
				lore = "This tiara may not provide much protection, but it sure is pretty",
				tooltip = "Wear it to dazzle your enemies ",
				Rarity = 2,
				minLevel = 5,
				maxLevel = 10,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};

			//Item 1/2
			new BaseItem(new int[][]
			{
				new int[] {-1},
				new int[] {15},
				new int[] {15},
				new int[] {17,16},
				new int[] {17,16},
				new int[] {0,65},
			})
			{
				name = "Chastity belt",
				description = "Dodge those fukbois",
				lore = "This belt will stop those cheeky cannibals and armsies from getting into your pants",
				tooltip = "Always wear this when sleeping",
				Rarity = 2,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};
			new BaseItem(new int[][]
		   {
					new int[] {2005},
					new int[] {-1},
					new int[] {-1},
					new int[] {42,43},
					new int[] {44,0,49,},
					new int[] {21,24,0,0,0,0},
		   })
			{
				name = "Ice Scroll",
				description = "A spell surrounded by flying shards of ice, contains tramendous power of cold.",
				lore = "Created at the top of the mountain.",
				tooltip = "Spell Scrolls grant magic buffs. Ice Scroll empowers spell - Snap Freeze.",
				Rarity = 4,
				minLevel = 30,
				maxLevel = 40,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
			};
			new BaseItem(new int[][]
		   {
					new int[] {2006},
					new int[] {57,1,2,3,4},
					new int[] {34,45,46,15,1,2,3,4,57,11,14,7,10,59},
					new int[] {8,1,2,3,4,9,5,6},
					new int[] {1000,1001,1002},
					new int[] {1000,1001,1002, 1003, 1004, 0,0,0,0,0,0,0},
		   })
			{
				name = "Motorboat Modification Blueprints",
				description = "Sheet of paper that allows to turn any raft into a high speed. Increases carry amount and increases speed of rafts.",
				lore = "Who did this lmao.",
				tooltip = "Consider unlocking a perk to drive them on land.",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
			};
			new BaseItem(new int[][]
			 {
				new int[]{1,2,3,4},
				new int[]{18},
				new int[]{18,0,0,62,63,64},
				new int[] {1,2,3,4,6,55},
			 })
			{
				name = "Axe of Swiftness",
				description = "",
				lore = "",
				tooltip = "A really fast axe",
				Rarity = 3,
				minLevel = 15,
				maxLevel = 17,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Axe,
				icon = Res.ResourceLoader.GetTexture(138),
			}.PossibleStats[1][0].Multipier = 1.6f;
			new BaseItem(new int[][]
			 {
				new int[]{1,26,22},
				new int[]{18},
				new int[]{18,62,63,64},
				new int[] {1,2,3,4,6,55,59,57,34,35,36,14,44,49},
				new int[] {53,22,25,12,13},
			 })
			{
				name = "Severer",
				description = "",
				lore = "",
				tooltip = "A really fast axe",
				Rarity = 4,
				minLevel = 25,
				maxLevel = 25,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Axe,
				icon = Res.ResourceLoader.GetTexture(138),
			}.PossibleStats[1][0].Multipier = 2.5f;

			new BaseItem(new int[][]
			 {
				new int[]{1,26,22},
				new int[]{18},
				new int[]{1,2,3,4,5,6,7,8,9,10,12,13,14,15,16,17,18,20,34,35,36,37,38,44,45,46,47,49,53,54,55,},
				new int[]{62,63,64,0,0,0},
				new int[]{19,56},
				new int[] {1,2,3,4,6,55,59,57,34,35,36,14,44,49},
				new int[] {53,22,25,12,13,1,2,3,4,39,40,41,42,43},
			 })
			{
				name = "Golden Axe of Fortune",
				description = "",
				lore = "",
				tooltip = "A really fast axe",
				Rarity = 5,
				minLevel = 35,
				maxLevel = 36,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Axe,
				icon = Res.ResourceLoader.GetTexture(138),
			}.PossibleStats[1][0].Multipier = 2.5f;
			new BaseItem(new int[][]
					   {
				new int[]{26,22},
				new int[]{63},
				new int[]{1,2,3,4,5,6,7,8,9,10,12,13,14,15,16,17,18,20,34,35,36,37,38,44,45,46,47,49,53,54,55,},
				new int[]{62,63,64,65},
				new int[]{19,56},
				new int[] {26},
				new int[] {53,22,25,12,13,1,2,3,4,39,40,41,42,43},
					   })
			{
				name = "Axe of Misfortune",
				description = "Misfortunate are the ones on the recieving end. They will bleed a lot",
				lore = "",
				tooltip = "A really fast axe",
				Rarity = 5,
				minLevel = 6,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Axe,
				icon = Res.ResourceLoader.GetTexture(138),
			}.PossibleStats[1][0].Multipier = 3;

			new BaseItem(new int[][]
		 {
				new int[] {1,2,3,4},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
		 })
			{
				name = "Golden Ring",
				description = "A Ring of ancient times.",
				lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Rings give base stats to make your stronger.",
				Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
			};
			new BaseItem(new int[][]
		   {
				new int[] {1,2,3,4},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
		   })
			{
				name = "Golden Locket",
				description = "A Locket of ancient times.",
				lore = "A Golden Locket that looks simple and elegant, yet it feels powerfull to the touch.",
				tooltip = "Lockets give base stats to make your stronger.",
				Rarity = 3,         //range 0-7, 0 is most common, 7 is ultra rare
				minLevel = 1,
				maxLevel = 4,
				CanConsume = false,
				StackSize = 1,     //stacking in inventory like in mc, one means single item
				type = BaseItem.ItemType.Amulet,
				icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
			};
			new BaseItem(new int[][]
		   {
			   new int[]{0,39}
		   })
			{
				name = "Dull Axe",
				description = "",
				lore = "",
				tooltip = "A really fast axe",
				Rarity = 0,
				minLevel = 15,
				maxLevel = 24,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Axe,
				icon = Res.ResourceLoader.GetTexture(138),
			};
			new BaseItem(new int[][]
			{
				new int[] {1,2,3,4 },
				new int[] {1,2,3,15,4,0,0,0 },
				new int[] {12,13,1,2,3,4,5, },
				new int[] {18,16,23,26,19 },
				new int[] {18,16,23,26 },
				new int[] {34,44,45,46 },
				new int[] {2,23,26},
				new int[] {2,23,26,51},
				new int[] {2,23,26,20,16,15,60},
				new int[] {52,66,60,0,0},
				new int[] {-1},
				  })
			{
				name = "Precise Adjustments",
				description = "Focus attack speed buff duration is increased by 16 seconds",
				lore = "Sometimes, when missing, there isn't a chance to fire another shot, as the prey is long gone.",
				tooltip = "Use with spell 'Focus'",
				Rarity = 7,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Quiver,
				icon = Res.ResourceLoader.GetTexture(98),
				onEquip = () => ModdedPlayer.Stats.spell_focusOnAtkSpeedDuration.Add( 16),
				onUnequip = () => ModdedPlayer.Stats.spell_focusOnAtkSpeedDuration.Substract(16),
			};
			new BaseItem(new int[][]
		   {
				new int[] {25 },
				new int[] {22,1,18,},
				new int[] {1,2,3,4,57,},
				new int[] {27,28,30},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {15,14,45,7,35,10},
				new int[] {62,63,64, },
				new int[] {53,61 },
		   })
			{
				name = "Rage",
				description = "Increases maximum stacks of frenzy by 10",
				lore = "WRYYYYYYYYYYYYYYYYYY",
				tooltip = "Greatswords are giant, incredibly slow and hard hitting.",
				Rarity = 7,
				minLevel = 6,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.GreatSword,
				icon = Res.ResourceLoader.GetTexture(88),
				onEquip = () => ModdedPlayer.Stats.spell_frenzyMaxStacks.Add( 10),
				onUnequip = () => ModdedPlayer.Stats.spell_frenzyMaxStacks.Substract(10),
			}.PossibleStats[0][0].Multipier = 3;

			BaseItem jaggedRipper = new BaseItem(new int[][]
			{
				new int[] {1,2,3,4,57,},
				new int[]{18},
				new int[] {25 },
				new int[] {62,63,64,  },
				new int[] {53,  },
				new int[] {49,14,  },
				new int[] {35,36,15,12,  },
				new int[] {27,28,30},
				new int[]{18,62,63,64},
				new int[] {65},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "Jagged Ripper",
				description = "Bash has 30% a chance to make enemies to bleed for 30% of damage dealt per second for duration of slow",
				lore = "",
				tooltip = "A legendary axe",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Axe,
				icon = Res.ResourceLoader.GetTexture(138),
				onEquip = () => ModdedPlayer.Stats.spell_bashBleedChance.Add( 0.3f),
				onUnequip = () => ModdedPlayer.Stats.spell_bashBleedChance.Substract(0.3f),
			};
			jaggedRipper.PossibleStats[1][0].Multipier = 2;

			new BaseItem(new int[][]
					  {
						new int[] {25 },
						new int[] {22,},
						new int[] {1,2,3,4,57,},
						new int[] {27,28,30},
						new int[] {-1},
						new int[] {-1},
						new int[] {-1},
						new int[] {-1},
						new int[] {-1},
						new int[] {15,14,45,7,35,10},
						new int[] {62,63,64, },
						new int[] {53,61 },
					  })
			{
				name = "Bloodthirster",
				description = "When bashing an enemy, you have gain 2% of damage dealt as health and energy. Based on damage before armor reduction.",
				lore = "The sword appears to be from medieval ages, through it's not. It was made a lot earlier. The means to reproduce this weapon have been long lost.",
				tooltip = "Long swords are slow, have little block and consume a lot of stamina. They have extraordinary weapon range and high damage",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 5,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.LongSword,
				icon = Res.ResourceLoader.GetTexture(89),
				onEquip = () => ModdedPlayer.Stats.spell_bashLifesteal.Add( 0.02f),
				onUnequip = () => ModdedPlayer.Stats.spell_bashLifesteal.Substract(0.02f),
			}.PossibleStats[0][0].Multipier = 3;
			new BaseItem(new int[][]
		 {
				new int[] {25 },
				new int[] {22 },
				new int[] {2004 },
				new int[] {62,63,64 },
				new int[] {27,28,30,29,48},
				new int[] {1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
		 })
			{
				name = "Frost Giant",
				description = "Insted of slowing enemies on hit, and the duration is twice as long as the previous effect.",
				lore = "",
				tooltip = "This hammer freezes enemies on hit. They also deal high smash damage",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Hammer,
				icon = Res.ResourceLoader.GetTexture(109),
				onEquip = () =>
				{
					ModdedPlayer.Stats.i_HammerStunDuration.Multiply(2) ;
					ModdedPlayer.Stats.i_HammerStunAmount.Multiply(0);
				},
				onUnequip = () =>
				{
					ModdedPlayer.Stats.i_HammerStunDuration.Divide(2);
					ModdedPlayer.Stats.i_HammerStunAmount.Reset();
				},
			}.PossibleStats[0][0].Multipier = 3.25f;

			new BaseItem(new int[][]
			{
			  new int[] {11},
				new int[] {65},
				new int[] {16,17},
				new int[] {1,11,45},
				new int[] {15},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "Alexander's Shield",
				description = "Parry has a chance to be casted when getting it. Requires parry to be equipped",
				lore = "",
				tooltip = "Shields increase your block.",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
				onEquip = () => ModdedPlayer.Stats.spell_chanceToParryOnHit.value = true,
				onUnequip = () => ModdedPlayer.Stats.spell_chanceToParryOnHit.value = false,
			};

			new BaseItem(new int[][]
	 {
			  new int[] {1},
				new int[] {12,3,16,45,46},
				new int[] {13,4,62,64},
				new int[] {22},
				new int[] {25,22},
				new int[] {25},
				new int[] {11,1},
				new int[] {30,1,57},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
	 })
			{
				name = "King Qruies",
				description = "A mighty sword seeking for it's owner",
				lore = "",
				tooltip = "With King Qruies' sword equipped you gain additional melee damage equal to the last instance of physical damage taken.",
				Rarity = 7,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.LongSword,
				icon = Res.ResourceLoader.GetTexture(89),
				onEquip = () => ModdedPlayer.Stats.i_KingQruiesSword.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_KingQruiesSword.value = false,
			};

			new BaseItem(new int[][]
			{
			  new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
			  new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
				new int[] {13,4,62,64},
				new int[] {26,2,34,57,55},
				new int[] {26,23},
				new int[] {18,16},
				new int[] {48,2,55},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			})
			{
				name = "Grip of Sora",
				description = "Look, a porcupine! -Sora",
				lore = "",
				tooltip = "Multishot drains 90% less energy and shoots +4 projectiles. Additional projectiles do not increase the cost of multishot",
				Rarity = 7,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
				onEquip = () => ModdedPlayer.Stats.i_SoraBracers.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_SoraBracers.value = false,
			};
			new BaseItem(new int[][]
		   {
			  new int[] {18},
				new int[] {60,0},
				new int[] {2,40},
				new int[] {12,13,2,40,16,66},
				new int[] {39,40,41,42,43,0,0},
				new int[] {23,26},
				new int[] {-1,0},
		   })
			{
				name = "Ancient Greatbow",
				description = "A massive and slow bow, deals extra damage",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Greatbow,
				icon = Res.ResourceLoader.GetTexture(170),
			}.PossibleStats[0][0].Multipier = -1.55f;

			new BaseItem(new int[][]
		{
			  new int[] {18},
				new int[] {61},
				new int[] {2,0,0,0},
				new int[] {12,13,2,40,16},
				new int[] {39,40,41,42,43,0,0},
				new int[] {23,26,2,66},
				new int[] {23,26},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
		})
			{
				name = "Phoenix's Death",
				description = "Ignites enemies on hit",
				lore = "",
				tooltip = "",
				Rarity = 6,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Greatbow,
				icon = Res.ResourceLoader.GetTexture(170),
				onEquip = () => ModdedPlayer.Stats.i_greatBowIgnites.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_greatBowIgnites.value = false,
			}.PossibleStats[0][0].Multipier = -0.7f;

			new BaseItem(new int[][]
		{
			  new int[] {18},
				new int[] {61,48},
				new int[] {2,3,5},
				new int[] {12,13,2,40,16},
				new int[] {39,40,41,42,43,66},
				new int[] {23,26},
				new int[] {23,26},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
		})
			{
				name = "Soulstring",
				description = "A massive and slow bow",
				lore = "",
				tooltip = "Blood infused arrow now deals additional 20 points of damage per health consumed",
				Rarity = 7,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Greatbow,
				icon = Res.ResourceLoader.GetTexture(170),
				onEquip = () => ModdedPlayer.Stats.spell_bia_HealthDmMult.Add( 20),
				onUnequip = () => ModdedPlayer.Stats.spell_bia_HealthDmMult.Substract(20),
			}.PossibleStats[0][0].Multipier = -0.7f;

			new BaseItem(new int[][]
			{
			  new int[] {18},

				new int[] {2,40,0},
			})
			{
				name = "Greatbow",
				description = "A massive and slow bow, deals extra damage",
				lore = "",
				tooltip = "",
				Rarity = 2,
				minLevel = 25,
				maxLevel = 28,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Greatbow,
				icon = Res.ResourceLoader.GetTexture(170),
			}.PossibleStats[0][0].Multipier = -2.7f;

			new BaseItem(new int[][]
		{
			  new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
			  new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
				new int[] {13,4,62,64},
				new int[] {26,2,34,57,55},
				new int[] {26,23},
				new int[] {26,0,0},
				new int[] {18,16},
				new int[] {30,1,57},
				new int[] {5},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
		})
			{
				name = "Hazardous Crown",
				description = "",
				lore = "",
				tooltip = "After casting bloody arrow, the bonus damage persists for 4 more projectiles.",
				Rarity = 7,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
				onEquip = () => ModdedPlayer.Stats.i_HazardCrown.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_HazardCrown.value = false,
			};
			new BaseItem(new int[][]
				   {
				new int[] {39,40,41,42,43,4},
				new int[] {4,6,24,21,16,3,42,43,49},
				new int[] {0,6,24,21,16,3,47,49,38,17,10,11,9,8,7,6},
				new int[] {37,4},
				new int[] {43,16,17,37,47 },
				   })
			{
				name = "Novice Magic Caster's Bracers",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new int[][]
				   {
				new int[] {2,12,13,23,26,40,43,54},
				new int[] {2,12,13,23,26,40,43,60,62,63,51,52,66},
				new int[] {5,6,7,8,9,10,16,17,26},
				new int[] {37,0,0,0},
				new int[] {43,16,2 },
				   })
			{
				name = "Ranger's Bracers",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new int[][]
				   {
				new int[] {1,16,5,6,18,39,14,22,25,53,62,63,57,45},
				new int[] {2,12,13,22,25,40,43,54},
				new int[] {5,6,7,8,9,10,16,17,26},
				new int[] {37,1,17,18,16},
				new int[] {43,1,65 },
				   })
			{
				name = "Swordsman's Bracers",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new int[][]
				   {
				new int[] {3,5,6,7,8,9,10,11,14,15,16},
				new int[] {7,16,18,0,0},
				new int[] {39,40,41,42,43,31,16},
				new int[] {1,2,3,4,5,57,39,40,41,42,43,31,16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				   })
			{
				name = "Healer's Bracers",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new int[][]
				 {
				new int[] {39,40,41,42,43,4},
				new int[] {4,6,24,21,16,3,42,43,49},
				new int[] {0,6,24,21,16,3,47,49,38,17,10,11,9,8,7,6},
				new int[] {37,0,0,0},
				new int[] {43,4 },
				 })
			{
				name = "Novice Magic Caster's Gloves",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
				   {
				new int[] {2,12,13,23,26,40,43,54},
				new int[] {2,12,13,23,26,40,43,60,62,63,51,52,66},
				new int[] {5,6,7,8,9,10,16,17,26},
				new int[] {37,0,0,0},
				new int[] {43,2 },
				   })
			{
				name = "Ranger's Gloves",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
				   {
				new int[] {1,16,5,6,18,39,14,22,25,53,62,63,57,45},
				new int[] {2,12,13,23,26,40,43,54},
				new int[] {5,6,7,8,9,10,16,17,26},
				new int[] {37,18,7,0},
				new int[] {43,1,65 },
				   })
			{
				name = "Swordsman's Gloves",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 4,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
				   {
				new int[] {3,5,6,7,8,9,10,11,14,15,16},
				new int[] {7,16,18,0,0},
				new int[] {39,40,41,42,43,31,16},
				new int[] {1,2,3,4,5,57,39,40,41,42,43,31,16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				   })
			{
				name = "Healer's Gloves",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
				   {
				new int[] {62,63,64,55,54,53,48,30,29,28,27},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				   })
			{
				name = "Fate Gloves",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 6,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
				   {
				new int[] {62,63,64,55,54,53,48,30,29,28,27},
				new int[] {34},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				   })
			{
				name = "Fate Boots",
				description = "",
				lore = "",
				tooltip = "",
				Rarity = 6,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
				   {
				new int[] {62,63,64,55,54,53,48,30,29,28,27},
				new int[] {34},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				   })
			{
				name = "Greed",
				description = "",
				lore = "",
				tooltip = "Automatically casts wide reach every second",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
				onEquip = () => ModdedPlayer.Stats.i_isGreed.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_isGreed.value = false,
			};
			BaseItem titaniumleggins = new BaseItem(new int[][]
			 {
				new int[] { 16},
				new int[] {31},
				new int[] {1,2,3,4},
				new int[] {5,},
				new int[] {-1,65},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			 })
			{
				name = "Titanium Leggins",
				description = "Heavily armored leg protection. Suffers from the same weaknesses as spartan armor.",
				lore = "",
				tooltip = "",
				Rarity = 6,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};
			titaniumleggins.PossibleStats[0][0].Multipier = 3;
			titaniumleggins.PossibleStats[1][0].Multipier = 1.5f;
			new BaseItem(new int[][]
	  {
				new int[] {42,39,40,41,43,0,0 },
				new int[] {16,24,25,26,5,6,7,8,9,10,11,12,13,14,15,17,18,55,60,61,62,63,64,0,0,0 },
				new int[] {16,24,25,26,5,6,7,8,9,10,11,12,13,14,15,17,18,55,60,61,62,63,64 },
				new int[] {43,0,0,0,16 },
				new int[] {65,0},
	  })
			{
				name = "Iron Gauntlet",
				description = "",
				lore = "",
				tooltip = "Gloves offer protection.",
				Rarity = 2,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};

			new BaseItem(new int[][]
		  {
				new int[] {4,3,6},
				new int[] {21,24,16 },
				new int[] {29,4 },
				new int[] {16,15,17 },
				new int[] {47,49,44,45,46 },
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
		  })
			{
				name = "Magefist",
				description = "Gloves that amplify magic",
				lore = "",
				tooltip = "Spells deal 75% increased damage but their cost is increased by 150%",
				Rarity = 7,
				minLevel = 1,
				maxLevel = 2,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
				onEquip = () => { ModdedPlayer.Stats.spellIncreasedDmg.valueMultiplicative *= 1.75f; ModdedPlayer.Stats.spellCost.valueMultiplicative *= 2.5f; },
				onUnequip = () => { ModdedPlayer.Stats.spellIncreasedDmg.valueMultiplicative /= 1.75f; ModdedPlayer.Stats.spellCost.valueMultiplicative /= 2.5f; }
			};
			new BaseItem(new int[][]
			   {
				new int[] {34 },
				new int[] {34,5,1,2,4,3,2,11 },
				new int[] {16,3,2,1,4 },
				new int[] {16,7,8 },
				new int[] {16, },
				new int[] {-1 },
				new int[] {43 },
			   })
			{
				name = "Armored Boots",
				description = "Heavily armored, resistant to damage boots.",
				lore = "",
				tooltip = "Shoes can provide movement speed bonuses.",
				Rarity = 5,
				minLevel = 10,
				maxLevel = 14,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Boot,
				icon = Res.ResourceLoader.GetTexture(85),
			};
			new BaseItem(new int[][]
			{
				new int[] {16},
				new int[] {16,18,57,53},
				new int[] {16,1,2,3,4,21,22,23,24,25,26,31,15,5,6,7,8,9},
				new int[] {1,2,3,4,57},
				new int[] {16,45,46,0,0,0},
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
			})
			{
				name = "Broken Protector",
				description = "This shield failed to protect those behind it.",
				lore = "",
				tooltip = "Shields increase your block.",
				Rarity = 6,
				minLevel = 5,
				maxLevel = 8,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Shield,
				icon = Res.ResourceLoader.GetTexture(99),
			}.PossibleStats[0][0].Multipier = 2;

			new BaseItem(new int[][]
		   {
					new int[] {4},
					new int[] {6,4,3,44},
					new int[] {21,24},
					new int[] {21,24},
					new int[] {-1,},
					new int[] {-1},
					new int[] {-1},
					new int[] {47,4,5,6,7,61,17,0,0,0,0,2,56,57,49,64},
		   })
			{
				name = "Forbidden Scroll",
				description = "Too powerful to be kept.",
				lore = "",
				tooltip = "Spell Scrolls grant magic buffs",
				Rarity = 6,
				minLevel = 1,
				maxLevel = 1,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
			};

			new BaseItem(new int[][]
		  {
				new int[] {16},
				new int[] {1,2,3,4},
				new int[] {17},
				new int[] {8,9,49,47},
				new int[] {16,18,11,34},
				new int[] {37,34},
				new int[] {-1},
		  })
			{
				name = "Doom Pauldrons",
				description = "Despite the cool name, they are completely normal pair of shoulder armor.",
				lore = "",
				tooltip = "Provides usefull stat bonuses",
				Rarity = 6,
				minLevel = 5,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ShoulderArmor,
				icon = Res.ResourceLoader.GetTexture(95),
			};
			new BaseItem(new int[][]
		  {
				new int[] {16},
				new int[] {1,2,3,4,57,53,54,55},
				new int[] {17,18,11,15},
				new int[] {15},
				new int[] {34},
				new int[] {16,5,6,7,8,9,10,11,12,13,14,15,17,18,59,47,45,46,60},
				new int[] {23,22,30,27,34,44,48,59},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
		  })
			{
				name = "Wind armor",
				description = "Run fast like the wind",
				lore = "",
				tooltip = "Upon dodging an attack, gain 20% movement speed, 35% damage, 2000 armor, and heal for 5% of your maximum health",
				Rarity = 7,
				minLevel = 5,
				maxLevel = 9,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
				onEquip = () => ModdedPlayer.Stats.i_isWindArmor.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_isWindArmor.value = false,
			};
			new BaseItem(new int[][]
			{
				new int[] {11},
				new int[] {22,23,21},
				new int[] {16},
				new int[] {1,2,3,4,5,6,7,8,9,10,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
				new int[] {-1},
				new int[] {-1},
				new int[] {24,25,26,0,0,0},
				//new int[] {29,30,48},
			})
			{
				name = "Crusader Helmet",
				description = "You're talking mad shit for someone within crusading distance",
				lore = "",
				tooltip = "",
				Rarity = 5,
				minLevel = 2,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};

			new BaseItem(new int[][]
		{
				new int[] {1,2,3,4,5,6,57},
				new int[] {-1},
				new int[] {-1},
				new int[] {24,25,26,10,47,0,0,0},
			//new int[] {29,30,48},
		})
			{
				name = "Hood",
				description = "",
				lore = "",
				tooltip = "Hats provide usefull stat bonuses",
				Rarity = 3,
				minLevel = 2,
				maxLevel = 6,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};
			new BaseItem(new int[][]
			{
				new int[] {47 },
				new int[] {4,29 },
				new int[] {4,29 },
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {4,17,6,44,38,21,24,8,9},
				new int[] {2000,2002,49},
				new int[] {2001,4,29,24,21},
				new int[] {2002,4},
				new int[] {2003,-1},
			})
			{
				name = "The Spark of Light in The Darkness",
				description = "Magic Scroll of great quality",
				lore = "Written in a language i canno't understand. Decyphering this text is impossible, so is the full utilization of the scroll.",
				tooltip = "If a black hole hits 5 or more enemies during it's lifetime, a ball lightning is summoned after it ends.",
				Rarity = 7,
				minLevel = 15,
				maxLevel = 20,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.SpellScroll,
				icon = Res.ResourceLoader.GetTexture(110),
				onEquip = () => ModdedPlayer.Stats.i_sparkOfLightAfterDark.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_sparkOfLightAfterDark.value = false
			};
			new BaseItem(new int[][]
		   {
				new int[] {21,22,23,24,25,26,1,2,4},
				new int[] {12,13,11,47},
				new int[] {62,63,64,1000,1001,1002,1003,1004},
				new int[] {61},
				new int[] {5,57},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
		   })
			{
				name = "Purgatory",
				description = "Golden ring with a bone chilling feel about it. This thing will only bring harm, but not to the wearer",
				lore = "Ring made of Netherrite",
				tooltip = "Purge ability bonus damage based on missing health. Up to 300%",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 26,
				CanConsume = false,
				StackSize = 1,
				onEquip = () => ModdedPlayer.Stats.spell_purgeDamageBonus.value = true,
				onUnequip = () => ModdedPlayer.Stats.spell_purgeDamageBonus.value = false,
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			}.DropSettings_OnlyCannibals();

			new BaseItem(new int[][]
		{
			  new int[] {18},
				new int[] {61,48},
				new int[] {2,3,5},
				new int[] {12,13,2,40,16},
				new int[] {39,40,41,42,43,0,0},
				new int[] {23,26},
				new int[] {23,26},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
		})
			{
				name = "Eruption",
				description = "Incarnation of devastation",
				lore = "",
				tooltip = "Headshots cause explosions",
				Rarity = 7,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Greatbow,
				icon = Res.ResourceLoader.GetTexture(170),
				onEquip = () => ModdedPlayer.Stats.i_EruptionBow.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_EruptionBow.value = false,
			}.PossibleStats[0][0].Multipier = -0.6f;

			new BaseItem(new int[][]
		{
			  new int[] {31},
				new int[] {61,48},
				new int[] {2,3,5},
				new int[] {12,13,2,40,16},
				new int[] {39,40,41,42,43,0,0},
				new int[] {23,26},
				new int[] {23,26},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
				new int[] {2,3,4,5,6,11,12,13,15,23,26,51,60,44,49,48},
		})
			{
				name = "Archangel",
				description = "Spread the goodness",
				lore = "",
				tooltip = "Shooting another player causes them to be greatly empowered for 30 seconds",
				Rarity = 7,
				minLevel = 10,
				maxLevel = 12,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Greatbow,
				icon = Res.ResourceLoader.GetTexture(170),
				onEquip = () => ModdedPlayer.Stats.i_ArchangelBow.value = true,
				onUnequip = () => ModdedPlayer.Stats.i_ArchangelBow.value = false,
			}.PossibleStats[0][0].Multipier = 2f;
			new BaseItem(new int[][]
			{
				new int[] {1,4 },
				new int[] {1,3,5,6,49 },
				new int[] {22},
				new int[] {25},
				new int[] {12},
				new int[] {13},
				new int[] {1,12,13,22,25,30,53,57,65 },
				new int[] {-1 },
			})
			{
				name = "The Executioner",
				description = "A sword for decapitating",
				lore = "",
				tooltip = "Critical hit can greatly increase how much damage you deal. ",
				Rarity = 4,
				minLevel = 25,
				maxLevel = 27,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.LongSword,
				icon = Res.ResourceLoader.GetTexture(89),
			};
			new BaseItem(new int[][]
			  {
				new int[] {48 },
				new int[] {23,26,2 },
				new int[] {23,26 },
				new int[] {12,13},
				new int[] {60},
				new int[] {13,23,26},
				new int[] {23,26},
				new int[] {34,2,2,54},
				new int[] {15,14},
				new int[] {16,23,2,4,5,6},
				new int[] {45,46},
				new int[] {27,49},
				new int[] {-1},
			  })
			{
				name = "Moon Cuirass",
				description = "A piece of armor designed for an archer. ",
				lore = "",
				tooltip = "Legendary marksmanship - landing a headshot with an arrow, without the homing effect of seeking arrow, on a distance greater than 120 meters, deals tenfold damage, and hits the enemy two extra times, applying debuffs and reducing armor.",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 22,
				CanConsume = false,
				StackSize = 1,
				onEquip = () => ModdedPlayer.Stats.perk_trueAimUpgrade.value = true,
				onUnequip = () => ModdedPlayer.Stats.perk_trueAimUpgrade.value = false,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};
			new BaseItem(new int[][]
			  {
				new int[] {65 },
				new int[] {1 },
				new int[] {3},
				new int[] {5,65},
				new int[] {5,6,11,10,7,45},
				new int[] {53},
				new int[] {22},
				new int[] {1,3,4,5,31},
				new int[] {28},
				new int[] {63},
				new int[] {16},
				new int[] {-1},
				new int[] {-1},
				new int[] {-1},
			  })
			{
				name = "Thornmail",
				description = "Spiked death on the outside, really comfy on the inside",
				lore = "",
				tooltip = "Thorns deal double damage",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 22,
				CanConsume = false,
				StackSize = 1,
				onEquip = () => ModdedPlayer.Stats.thornsDmgMult.valueMultiplicative *= 2,
				onUnequip = () => ModdedPlayer.Stats.thornsDmgMult.valueMultiplicative /= 2,
				type = BaseItem.ItemType.ChestArmor,
				icon = Res.ResourceLoader.GetTexture(96),
			};

			new BaseItem(new int[][]
						 {
				new int[] {1 },
						 })
			{
				name = "Rusty Polearm",
				description = "Used by the Ubersreik Five",
				lore = "",
				tooltip = "Long range spear-type weapon",
				Rarity = 1,
				minLevel = 10,
				maxLevel = 16,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Polearm,
				icon = Res.ResourceLoader.GetTexture(181),
			};
			new BaseItem(new int[][]
					   {
				new int[] {1 },
				new int[] {25,0 },
				new int[] {25 ,62,63,64},
				new int[] {6,49},
				new int[] {39,40,41,42,44,8,18,65 },
					   })
			{
				name = "Giant Polearm",
				description = "Used by the Sir Kruber",
				lore = "",
				tooltip = "Long range spear-type weapon",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 24,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Polearm,
				icon = Res.ResourceLoader.GetTexture(181),
			};

			//Feathers
			new BaseItem(new int[][] { })
			{
				name = "Crude Feather",
				description = "If equipped on a weapon, increases ranged damage by 10%\n" +
				"If equipped on boots, increases movement speed by 6.5%\n" +
				"If equipped on a helmet, increases critical hit chance by 7.5%\n" +
				"If equipped on an amulet, increases ranged armor piercing by 50\n" +
				"If equipped in other slots, increases agility by 15",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 3,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 1,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(185),
			};
			new BaseItem(new int[][] { })
			{
				name = "Soft Feather",
				description = "If equipped on a weapon, increases ranged damage by 20%\n" +
				"If equipped on boots, increases movement speed by 12.5%\n" +
				"If equipped on a helmet, increases critical hit chance by 12.5%\n" +
				"If equipped on an amulet, increases ranged armor piercing by 100\n" +
				"If equipped in other slots, increases agility by 30",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 1,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(185),
			};
			new BaseItem(new int[][] { })
			{
				name = "Ornate Feather",
				description = "If equipped on a weapon, increases ranged damage by 35%\n" +
					"If equipped on boots, increases movement speed by 18%\n" +
					"If equipped on a helmet, increases critical hit chance by 20%\n" +
					"If equipped on an amulet, increases ranged armor piercing by 200\n" +
					"If equipped in other slots, increases agility by 60",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 5,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 1,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(185),
			};

			new BaseItem(new int[][] { })
			{
				name = "Wonderful Feather",
				description = "If equipped on a weapon, increases ranged damage by 50%\n" +
				   "If equipped on boots, increases movement speed by 24%\n" +
				   "If equipped on a helmet, increases critical hit chance by 25%\n" +
				   "If equipped on an amulet, increases ranged armor piercing by 350\n" +
				   "If equipped in other slots, increases agility by 120",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 6,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 1,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(185),
			};
			new BaseItem(new int[][] { })
			{
				name = "White Crow's Feather",
				description = "If equipped on a weapon, increases ranged damage by 70%\n" +
				"If equipped on boots, increases movement speed by 30%\n" +
				"If equipped on a helmet, increases critical hit chance by 30%\n" +
				"If equipped on an amulet, increases ranged armor piercing by 600\n" +
				"If equipped in other slots, increases agility by 250",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 1,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(185),
			};

			//-------------- Shark teeth

			new BaseItem(new int[][] { })
			{
				name = "Reef Shark",
				description = "If equipped on a weapon, increases melee damage by 10%\n" +
				"If equipped on boots, increases damage reduction by 6.5%\n" +
				"If equipped on a helmet, increases attack speed by 7.5%\n" +
				"If equipped on an amulet, increases melee armor piercing by 50\n" +
				"If equipped in other slots, increases strength by 15",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 3,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 2,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(186),
			};
			new BaseItem(new int[][] { })
			{
				name = "Tiger Shark Tooth",
				description = "If equipped on a weapon, increases melee damage by 20%\n" +
				"If equipped on boots, increases damage reduction by 12.5%\n" +
				"If equipped on a helmet, increases attack speed by 12.5%\n" +
				"If equipped on an amulet, increases melee armor piercing by 100\n" +
				"If equipped in other slots, increases strength by 30",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 2,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(186),
			};
			new BaseItem(new int[][] { })
			{
				name = "Whale Shark Tooth",
				description = "If equipped on a weapon, increases melee damage by 35%\n" +
					"If equipped on boots, increases damage reduction by 18%\n" +
					"If equipped on a helmet, increases attack speed by 20%\n" +
					"If equipped on an amulet, increases melee armor piercing by 200\n" +
					"If equipped in other slots, increases strength by 60",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 5,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 2,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(186),
			};

			new BaseItem(new int[][] { })
			{
				name = "Great White Shark Tooth",
				description = "If equipped on a weapon, increases melee damage by 50%\n" +
				   "If equipped on boots, increases damage reduction by 24%\n" +
				   "If equipped on a helmet, increases attack speed by 25%\n" +
				   "If equipped on an amulet, increases melee armor piercing by 350\n" +
				   "If equipped in other slots, increases strength by 120",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 6,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 2,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(186),
			};
			new BaseItem(new int[][] { })
			{
				name = "Megalodon's Tooth",
				description = "If equipped on a weapon, increases melee damage by 70%\n" +
				"If equipped on boots, increases damage reduction by 30%\n" +
				"If equipped on a helmet, increases attack speed by 30%\n" +
				"If equipped on an amulet, increases melee armor piercing by 600\n" +
				"If equipped in other slots, increases strength by 250",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 2,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(186),
			};

			//------------- Sapphires

			new BaseItem(new int[][] { })
			{
				name = "Uncut Sapphire",
				description = "If equipped on a weapon, increases magic damage by 10%\n" +
			"If equipped on boots, decreases spell cost by 6.5%\n" +
			"If equipped on a helmet, decreases spell cooldown by 7.5%\n" +
			"If equipped on an amulet, increases energy on hit by 1\n" +
			"If equipped in other slots, increases intelligence by 15",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 3,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 3,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(187),
			};
			new BaseItem(new int[][] { })
			{
				name = "Clear Sapphire",
				description = "If equipped on a weapon, increases magic damage by 20%\n" +
				"If equipped on boots, decreases spell cost by 12.5%\n" +
				"If equipped on a helmet, decreases spell cooldown by 12.5%\n" +
				"If equipped on an amulet, increases energy on hit by 2\n" +
				"If equipped in other slots, increases intelligence by 30",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 3,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(187),
			};
			new BaseItem(new int[][] { })
			{
				name = "Shiny Sapphire",
				description = "If equipped on a weapon, increases magic damage by 35%\n" +
					"If equipped on boots, decreases spell cost by 18%\n" +
					"If equipped on a helmet, decreases spell cooldown by 20%\n" +
					"If equipped on an amulet, increases energy on hit by 4\n" +
					"If equipped in other slots, increases intelligence by 60",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 5,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 3,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(187),
			};

			new BaseItem(new int[][] { })
			{
				name = "Enchanted Sapphire",
				description = "If equipped on a weapon, increases magic damage by 50%\n" +
				   "If equipped on boots, decreases spell cost by 24%\n" +
				   "If equipped on a helmet, decreases spell cooldown by 25%\n" +
				   "If equipped on an amulet, increases energy on hit by 7\n" +
				   "If equipped in other slots, increases intelligence by 120",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 6,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 3,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(187),
			};
			new BaseItem(new int[][] { })
			{
				name = "Celestial Sapphire",
				description = "If equipped on a weapon, increases magic damage by 70%\n" +
				"If equipped on boots, decreases spell cost by 30%\n" +
				"If equipped on a helmet, decreases spell cooldown by 30%\n" +
				"If equipped on an amulet, increases energy on hit by 12\n" +
				"If equipped in other slots, increases intelligence by 250",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 3,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(187),
			};

			// -------- Moonstones
			new BaseItem(new int[][] { })
			{
				name = "Uncut Moonstone",
				description = "If equipped on a weapon, increases all healing by 10%\n" +
	 "If equipped on boots, increases magic find by 6.5%\n" +
	 "If equipped on a helmet, increases experience recieved by 7.5%\n" +
	 "If equipped on an amulet, increases life per second by 2\n" +
	 "If equipped in other slots, increases vitality by 15",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 3,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 4,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(188),
			};
			new BaseItem(new int[][] { })
			{
				name = "Clear Moonstone",
				description = "If equipped on a weapon, increases all healing by 20%\n" +
				"If equipped on boots, increases magic find by 12.5%\n" +
				"If equipped on a helmet, increases experience recieved by 12.5%\n" +
				"If equipped on an amulet, increases life per second by 4\n" +
				"If equipped in other slots, increases vitality by 30",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 4,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(188),
			};
			new BaseItem(new int[][] { })
			{
				name = "Shiny Moonstone",
				description = "If equipped on a weapon, increases all healing by 35%\n" +
					"If equipped on boots, increases magic find by 18%\n" +
					"If equipped on a helmet, increases experience recieved by 20%\n" +
					"If equipped on an amulet, increases life per second by 8\n" +
					"If equipped in other slots, increases vitality by 60",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 5,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 4,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(188),
			};

			new BaseItem(new int[][] { })
			{
				name = "Enchanted Moonstone",
				description = "If equipped on a weapon, increases all healing by 50%\n" +
				   "If equipped on boots, increases magic find by 24%\n" +
				   "If equipped on a helmet, increases experience recieved by 25%\n" +
				   "If equipped on an amulet, increases life per second by 14\n" +
				   "If equipped in other slots, increases vitality by 120",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 6,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 4,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(188),
			};
			new BaseItem(new int[][] { })
			{
				name = "Celestial Moonstone",
				description = "If equipped on a weapon, increases all healing by 70%\n" +
				"If equipped on boots, increases magic find by 30%\n" +
				"If equipped on a helmet, increases experience recieved by 30%\n" +
				"If equipped on an amulet, increases life per second by 24\n" +
				"If equipped in other slots, increases vitality by 250",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 4,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(188),
			};

			// ----------------- Ores

			new BaseItem(new int[][] { })
			{
				name = "Lead Ore",
				description = "If equipped on a weapon, increases weapon range by 10%\n" +
  "If equipped on boots, increases resistance to magic by 6.5%\n" +
  "If equipped on a helmet, increases maximum health by 7.5%\n" +
  "If equipped on an amulet, increases thorns by 150\n" +
  "If equipped in other slots, increases armor by 15",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 3,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 5,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(184),
			};
			new BaseItem(new int[][] { })
			{
				name = "Vanadium Ore",
				description = "If equipped on a weapon, increases weapon range by 20%\n" +
				"If equipped on boots, increases resistance to magic by 12.5%\n" +
				"If equipped on a helmet, increases maximum health by 12.5%\n" +
				"If equipped on an amulet, increases thorns by 300\n" +
				"If equipped in other slots, increases armor by 30",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 4,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 5,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(184),
			};
			new BaseItem(new int[][] { })
			{
				name = "Titanium Ore",
				description = "If equipped on a weapon, increases weapon range by 35%\n" +
					"If equipped on boots, increases resistance to magic by 18%\n" +
					"If equipped on a helmet, increases maximum health by 20%\n" +
					"If equipped on an amulet, increases thorns by 600\n" +
					"If equipped in other slots, increases armor by 60",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 5,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 5,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(184),
			};

			new BaseItem(new int[][] { })
			{
				name = "Chromium Ore",
				description = "If equipped on a weapon, increases weapon range by 50%\n" +
				   "If equipped on boots, increases resistance to magic by 24%\n" +
				   "If equipped on a helmet, increases maximum health by 25%\n" +
				   "If equipped on an amulet, increases thorns by 1100\n" +
				   "If equipped in other slots, increases armor by 120",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 6,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 5,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(184),
			};
			new BaseItem(new int[][] { })
			{
				name = "Tungsten Ore",
				description = "If equipped on a weapon, increases weapon range by 70%\n" +
				"If equipped on boots, increases resistance to magic by 30%\n" +
				"If equipped on a helmet, increases maximum health by 30%\n" +
				"If equipped on an amulet, increases thorns by 1800\n" +
				"If equipped in other slots, increases armor by 250",
				lore = "",
				tooltip = "Materials can be put inside empty sockets to add stats to items",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 21,
				CanConsume = false,
				StackSize = 100,
				subtype = 5,
				type = BaseItem.ItemType.Material,
				icon = Res.ResourceLoader.GetTexture(184),
			};

			//This is the new better way of defining items, no longer uses item ids, now uses enum like in C++, the enum is Stats, you can find it in ItemDataBase_StatDefinitons.cs
			new BaseItem(new Stat[][]
			{
				new [] {MELEEDMGFROMSTR},
				new [] {STRENGTH},
				new [] {BASEMELEEDAMAGE},
				new [] {MELEEARMORPIERCING},
				new [] {ATTACKCOSTREDUCTION,ATTACKSPEED},
				new [] {ATTACKSPEED},
				new [] {ALLATTRIBUTES,MELEEWEAPONRANGE},
				new [] {ENERGYONHIT,VITALITY,LIFEONHIT }
			})
			{
				name = "Knife on a stick",
				description = "Kasper named this item, his fault",
				lore = "",
				tooltip = "Long range spear-type weapon",
				Rarity = 5,
				minLevel = 30,
				maxLevel = 34,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Weapon,
				weaponModel = BaseItem.WeaponModelType.Polearm,
				icon = Res.ResourceLoader.GetTexture(181),
			};
			new BaseItem(new Stat[][]
			{
				new [] {STRENGTH},
				new [] {STRENGTH,ALLATTRIBUTES,MELEEARMORPIERCING,MELEEDAMAGEINCREASE,COOLDOWNREDUCTION,SPELLDAMAGEINCREASE},
				new [] {MAXIMUMLIFE,VITALITY,PERCENTMAXIMUMLIFE,ALLATTRIBUTES},
				new [] {MELEEDAMAGEINCREASE,DAMAGEREDUCTION},
				new [] {THORNS},
				new [] {VITALITY,LIFEPERSECOND,LIFEREGENERATION},
				new [] {VITALITY,LIFEPERSECOND,LIFEREGENERATION,THORNS},
				new [] {ALL},
				new [] {ALL},
				new [] {ALL},
				new [] {ENERGYPERSECOND,MAXIMUMENERGY,DODGECHANCE,ARMOR,},
				new [] {STRENGTH,INTELLIGENCE,ARMOR,ARMORPIERCING,THORNS,}
			})
			{
				name = "Fists of Nails",
				description = "Swiss sheese makers",
				lore = "",
				tooltip = "Gain 5 thorns per vitality",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 22,
				CanConsume = false,
				StackSize = 1,
				onEquip = () => ModdedPlayer.Stats.thornsPerVit.Add( 5),
				onUnequip = () => ModdedPlayer.Stats.thornsPerVit.Substract(5),
				type = BaseItem.ItemType.Glove,
				icon = Res.ResourceLoader.GetTexture(86),
			};
			new BaseItem(new int[][]
		   {
				new int[] {1000},
				new int[] {1001},
				new int[] {1002},
				new int[] {1003},
				new int[] {1004},
				new int[] {1,2,3,4,5,6 },
				new int[] {-1 },
				new int[] {-1 },
				new int[] {-1 },
		   })
			{
				name = "Cargo Shorts MK2",
				description = "Deepest pockets out there",
				lore = "Improved cargo pants. Twice as many pockets, and since they didnt fit on the outside, they are inside. They are still ugly as hell tho",
				tooltip = "Pants provide armor and sometimes they increase carrying capactiy.",
				Rarity = 4,
				minLevel = 30,
				maxLevel = 33,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Pants,
				icon = Res.ResourceLoader.GetTexture(87),
			};
			new BaseItem(new Stat[][]
			{
				new [] {INTELLIGENCE,AGILITY},
				new [] {MAGICFIND,SPELLDAMAGEINCREASE,BASESPELLDAMAGE},
				new [] {SPELLDAMAGEINCREASE,BASESPELLDAMAGE},
				new [] {MAXENERGYFROMAGI,SPELLDMGFROMINT,RANGEDDMGFROMAGI},
				new [] {ARMOR},
				new [] {VITALITY,LIFEPERSECOND,LIFEREGENERATION,INTELLIGENCE,AGILITY,STRENGTH,ALLATTRIBUTES},
				new [] {COOLDOWNREDUCTION,SPELLCOSTREDUCTION,SPELLCOSTTOSTAMINA,MOVEMENTSPEED,DAMAGEREDUCTION},
				new [] {ALL},
				new [] {ALL},
				new [] {ALL},
				new [] {DODGECHANCE,ARMOR,BASESPELLDAMAGE,BASERANGEDDAMAGE},
				new [] {BASERANGEDDAMAGE,RANGEDARMORPIERCING,RANGEDDAMAGEINCREASE}
			})
			{
				name = "Aezyn",
				description = "Enchanted with magic as strong as power swing. It's purpose? Hit harder.",
				lore = "",
				tooltip = "Magic arrow damage scaling is increased by 666%",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 22,
				CanConsume = false,
				StackSize = 1,
				onEquip = () => ModdedPlayer.Stats.spell_magicArrowDamageScaling.Add(6.66f),
				onUnequip = () => ModdedPlayer.Stats.spell_magicArrowDamageScaling.Substract(6.66f),
				type = BaseItem.ItemType.Bracer,
				icon = Res.ResourceLoader.GetTexture(93),
			};
			new BaseItem(new Stat[][]
		{
				new [] {INTELLIGENCE,AGILITY},
				new [] {CRITICALHITCHANCE},
				new [] {CRITICALHITDAMAGE},
				new [] {VITALITY,LIFEPERSECOND,LIFEREGENERATION,INTELLIGENCE,AGILITY,STRENGTH,ALLATTRIBUTES},
				new [] {COOLDOWNREDUCTION,SPELLCOSTREDUCTION,SPELLCOSTTOSTAMINA,MOVEMENTSPEED,DAMAGEREDUCTION},
				new [] {ALL},
				new [] {ALL},
				new [] {ALL},
				new [] {ALL},
				new [] {ALL},
				new [] {DODGECHANCE,ARMOR,BASESPELLDAMAGE,BASERANGEDDAMAGE},
				new [] {BASERANGEDDAMAGE,RANGEDARMORPIERCING,RANGEDDAMAGEINCREASE}
		})
			{
				name = "Punny's Reflective Ring",
				description = "Enchanted with magic as strong as power swing. It's purpose? Hit harder.",
				lore = "",
				tooltip = "Magic arrow is shot in volleys. This effect can stack if more of the same item is equipped.",
				Rarity = 7,
				minLevel = 20,
				maxLevel = 22,
				CanConsume = false,
				StackSize = 1,
				onEquip = () => ModdedPlayer.Stats.spell_magicArrowVolleyCount.Add(3),
				onUnequip = () => ModdedPlayer.Stats.spell_magicArrowDamageScaling.Substract(3),
				type = BaseItem.ItemType.Ring,
				icon = Res.ResourceLoader.GetTexture(90),
			};
			new BaseItem(new int[][]
					  {
				new int[] {39,0},
				new int[] {43,0},
				new int[] {67},
					  })
			{
				name = "Eyepatch",
				description = "A wise man once said:",
				lore = "Everyone thinks I'm just a one-eyed bloody monster, god damnit... (sobbing)",
				tooltip = "Explosion damage is also applied when performing jump attacks",
				Rarity = 0,
				minLevel = 1,
				maxLevel = 3,
				CanConsume = false,
				StackSize = 1,
				type = BaseItem.ItemType.Helmet,
				icon = Res.ResourceLoader.GetTexture(91),
			};
		}
	}
}