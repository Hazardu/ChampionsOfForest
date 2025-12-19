using System.Collections.Generic;

using ChampionsOfForest.Effects.Sound_Effects;

using UnityEngine;

namespace ChampionsOfForest.Items
{
	public class ItemDefinition
	{
		public class StatSlot
		{
			public float probability;
			public List<ItemStat> options;
			public StatSlot(List<ItemStat> options, float chance = 1)
			{
				this.probability = chance;
				this.options = options;
			}
		}

		public enum ItemType
		{
			Other, Shield, Quiver, Weapon, Material, Helmet, Boot, Pants, ChestArmor, ShoulderArmor, Glove, Bracer, Amulet, Ring, SpellScroll, SocketableGem, Consumable
		}
		public enum Rarity
		{
			Common = 0, Uncommon, Rare, Magic, Legendary, Max
		}
		public enum ItemSubtype 
		{ 
			None, GreatSword, LongSword, Hammer, Polearm, Axe, Greatbow,
			RangedSocketable, MeleeSocketable, MagicSocketable, SupportSocketable, DefenseSocketable, UtilitySocketable,
		};

		public delegate void OnItemUsed();
		public delegate bool OnItemUsedOnAnother(Item other); // what happens when you drag and drop this item onto another


		public int id = 0;                      
		public Rarity rarity = 0;     
		public ItemType type = ItemType.Other;          
		public ItemSubtype subtype = ItemSubtype.None;
		
		public OnItemUsedOnAnother onUsedOnAnotherItemCallback;
		public OnItemUsed onEquipCallback, onUnequipCallback;
		public List<StatSlot> statSlots;
		public int maximumSocketSlots;
		public int minimumEmptySockets;


		// display properties of the item
		public string name;                 
		public string description;          
		public string lore;                 
		public string uniqueStat;              
		public Texture2D icon;
		public int minLevel = 1;
		public int maxLevel = 1;

		public int stackSize = 1;
		public bool PickUpAll = true;          

		//Drop settings
		public EnemyProgression.Enemy lootTable = EnemyProgression.Enemy.All;
		public int lootWeight = DefaultLootWeight; //weight of the item in loot table, used to calculate drop chance
		public const int DefaultLootWeight = 1000;
		public ItemDefinition()
		{
			statSlots = new List<StatSlot>();
		}


		public GlobalSFX.SFX GetInvSound()
		{
			switch (type)
			{
				case ItemType.Shield:
					return GlobalSFX.SFX.Invshiel;
				case ItemType.Quiver:
					return GlobalSFX.SFX.Invlarm;
				case ItemType.Weapon:
					switch (subtype)
					{							
						case ItemSubtype.GreatSword:
							return GlobalSFX.SFX.Invharm;
						case ItemSubtype.LongSword:
							return GlobalSFX.SFX.Invsword;
						case ItemSubtype.Hammer:
							return GlobalSFX.SFX.Invanvl;
						case ItemSubtype.Polearm:
							return GlobalSFX.SFX.Invstaf;
						case ItemSubtype.Axe:
							return GlobalSFX.SFX.Invaxe;
						case ItemSubtype.Greatbow:
							return GlobalSFX.SFX.Invbow;
						default:
							return GlobalSFX.SFX.Invharm;
					}
				case ItemType.Other:
					return GlobalSFX.SFX.Invbody;
				case ItemType.Material:
					return GlobalSFX.SFX.Invrock;
				case ItemType.Helmet:
					return GlobalSFX.SFX.Invcap;
				case ItemType.Boot:
					return GlobalSFX.SFX.Invgrab;	//change
				case ItemType.Pants:
					return GlobalSFX.SFX.Invblst;	//change
				case ItemType.ChestArmor:
					return GlobalSFX.SFX.Invsign;	
				case ItemType.ShoulderArmor:
					return GlobalSFX.SFX.Invharm;	//change
				case ItemType.Glove:
					return GlobalSFX.SFX.Invlarm;	//change
				case ItemType.Bracer:
					return GlobalSFX.SFX.Invbook;	//change
				case ItemType.Amulet:
					return GlobalSFX.SFX.Invring;
				case ItemType.Ring:
					return GlobalSFX.SFX.Invring;
				case ItemType.SpellScroll:
					return GlobalSFX.SFX.Invscrol;
				default:
					return GlobalSFX.SFX.ClickDown;
			}
		}

		public int GetDropSoundID()
		{
			int ID = ((int)GetInvSound()) + 1052;
			return ID < 1070 ? ID : Random.Range(1050, 1052);
		}


		public float GetInvSoundPitch()
		{
			return 1.00f - (float)rarity / 12f;
		}
	}
}