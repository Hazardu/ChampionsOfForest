using System.Collections.Generic;

using ChampionsOfForest.Effects.Sound_Effects;

using UnityEngine;

namespace ChampionsOfForest
{
	public class ItemTemplate
	{
		public enum ItemType
		{
			Shield, Quiver, Weapon, Other, Material, Helmet, Boot, Pants, ChestArmor, ShoulderArmor, Glove, Bracer, Amulet, Ring, SpellScroll
		}

		public delegate void OnItemEquip();

		public delegate void OnItemUnequip();

		public delegate bool OnItemConsume(Item other);

		public List<List<ItemStat>> PossibleStats;
		public int Rarity = 0;                  //rarity to display in different color
		public int ID = 0;                      //the id of an item
		public int StackSize = 1;               //how many items can be placed in one item slot?
		public bool CanConsume = false;             //can you eat this bad boy?
		public ItemType type = ItemType.Weapon;          //determines on which inv slot can this item be placed in

		public enum WeaponModelType { None, GreatSword, LongSword, Hammer, Polearm, Axe, Greatbow };

		public WeaponModelType weaponModel = WeaponModelType.None;
		public OnItemEquip onEquip;
		public OnItemUnequip onUnequip;
		public OnItemConsume onConsume;
		public string name;                 //name of item,
		public string description;          //what is this item basically
		public string lore;                 //some cool story to make this item have any sense, or a place for a joke
		public string uniqueStat;              //what should be displayed in the tooltip of this item
		public int level = 1;                   //level of this item
		public int minLevel = 1;
		public int maxLevel = 1;
		public Texture2D icon;              //icon of this item
		public bool PickUpAll = true;              //should the item be picked one by one, or grab all at once
		public int subtype = 0;

		//Drop settings
		public EnemyProgression.Enemy lootTable = EnemyProgression.Enemy.All;

		public ItemTemplate()
		{
		}

		private readonly int[] commonstatIds = new int[]
		{
			1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,21,22,23,24,25,26,31,34,35,36,37,38,39,40,41,42,43,44,45,46,47,49,50,51,52,53,54,55,56,57,59,60,61,62,63,64,65,66,67
		};

		public ItemTemplate(params int[][] possibleStats)
		{
			PossibleStats = new List<List<ItemStat>>();
			foreach (int[] a in possibleStats)
			{
				List<ItemStat> list = new List<ItemStat>();
				foreach (int statID in a)
				{
					if (statID == 0)
					{
						list.Add(null);
					}
					else if (statID == -1)
					{
						foreach (int c in commonstatIds)
						{
							list.Add(new ItemStat(ItemDataBase.statsById[c]));
						}
					}
					else
					{
						list.Add(new ItemStat(ItemDataBase.statsById[statID]));
					}
				}
				PossibleStats.Add(list);
			}
			ID = ItemDataBase.itemTemplates.Count;
			ItemDataBase.itemTemplates.Add(this);
		}

		public ItemTemplate(params ItemDataBase.Stat[][] possibleStats)
		{
			PossibleStats = new List<List<ItemStat>>();
			foreach (var statRow in possibleStats)
			{
				List<ItemStat> list = new List<ItemStat>();
				foreach (var stat in statRow)
				{
					int statID = (int)stat;
					if (statID == 0)
					{
						list.Add(null);
					}
					else if (statID == -1)
					{
						foreach (int c in commonstatIds)
						{
							list.Add(new ItemStat(ItemDataBase.statsById[c]));
						}
					}
					else
					{
						list.Add(new ItemStat(ItemDataBase.statsById[statID]));
					}
				}
				PossibleStats.Add(list);
			}
			ID = ItemDataBase.itemTemplates.Count;
			ItemDataBase.itemTemplates.Add(this);
		}

		public ItemTemplate(List<List<ItemStat>> possibleStats, int rarity, int StackSize, ItemType itemType, string name, string description, string lore, string tooltip, int minlevel, int maxlevel, Texture2D texture, bool pickupAll = false)
		{
			PossibleStats = possibleStats;
			Rarity = rarity;
			ID = ItemDataBase.itemTemplates.Count;
			this.StackSize = StackSize;
			type = itemType;
			PickUpAll = pickupAll;
			this.name = name;
			this.description = description;
			this.lore = lore;
			this.uniqueStat = tooltip;
			this.minLevel = minlevel;
			this.maxLevel = maxlevel;
			LootsFromAssignDefault();
			ItemDataBase.itemTemplates.Add(this);
			icon = texture;
		}

		public ItemTemplate(int[][] possibleStats, int rarity, int StackSize, ItemType itemType, string name, string description, string lore, string tooltip, int minlevel, int maxlevel, Texture2D texture, bool pickupAll = false)
		{
			PossibleStats = new List<List<ItemStat>>();
			foreach (int[] a in possibleStats)
			{
				List<ItemStat> list = new List<ItemStat>();
				foreach (int b in a)
				{
					list.Add(b == 0 ? null : ItemDataBase.statsById[b]);
				}
				PossibleStats.Add(list);
			}
			Rarity = rarity;
			ID = ItemDataBase.itemTemplates.Count;
			this.StackSize = StackSize;
			type = itemType;
			PickUpAll = pickupAll;
			this.name = name;
			this.description = description;
			this.lore = lore;
			this.uniqueStat = tooltip;
			this.minLevel = minlevel;
			this.maxLevel = maxlevel;
			LootsFromAssignDefault();
			ItemDataBase.itemTemplates.Add(this);
			icon = texture;
		}

		public ItemTemplate(int[][] possibleStats, int rarity, int iD, int StackSize, WeaponModelType weaponModel, string name, string description, string lore, string tooltip, int minlevel, int maxlevel, Texture2D texture, bool pickupAll = false)
		{
			PossibleStats = new List<List<ItemStat>>();
			foreach (int[] a in possibleStats)
			{
				List<ItemStat> list = new List<ItemStat>();
				foreach (int b in a)
				{
					list.Add(b == 0 ? null : ItemDataBase.statsById[b]);
				}
				PossibleStats.Add(list);
			}
			Rarity = rarity;
			ID = iD;
			this.StackSize = StackSize;
			type = ItemType.Weapon;
			this.weaponModel = weaponModel;
			PickUpAll = pickupAll;
			this.name = name;
			this.description = description;
			this.lore = lore;
			this.uniqueStat = tooltip;
			this.minLevel = minlevel;
			this.maxLevel = maxlevel;
			LootsFromAssignDefault();
			ItemDataBase.itemTemplates.Add(this);
			icon = texture;
		}

		/// <summary>
		/// set to this by default
		/// </summary>
		private void LootsFromAssignDefault()
		{
			//Lootable from everything
			lootTable = (EnemyProgression.Enemy)0b1111111111111111111111;
		}

		//Sets the item to drop from only a specyfic group of enemies
		public void DropSettings_OnlyArmsy()
		{
			lootTable = EnemyProgression.Enemy.RegularArmsy | EnemyProgression.Enemy.PaleArmsy;
		}

		public void DropSettings_OnlyVags()
		{
			lootTable = EnemyProgression.Enemy.PaleVags | EnemyProgression.Enemy.RegularVags;
		}

		public void DropSettings_OnlyCow()
		{
			lootTable = EnemyProgression.Enemy.Cowman;
		}

		public void DropSettings_OnlyBaby()
		{
			lootTable = EnemyProgression.Enemy.Baby;
		}

		public void DropSettings_OnlyMegan()
		{
			lootTable = EnemyProgression.Enemy.Megan;
		}

		public void DropSettings_OnlyCreepy()
		{
			lootTable = EnemyProgression.Enemy.RegularArmsy | EnemyProgression.Enemy.PaleArmsy | EnemyProgression.Enemy.RegularVags | EnemyProgression.Enemy.PaleVags | EnemyProgression.Enemy.Cowman | EnemyProgression.Enemy.Baby | EnemyProgression.Enemy.Girl | EnemyProgression.Enemy.Worm | EnemyProgression.Enemy.Megan;
		}

		public void DropSettings_OnlyCannibals()
		{
			lootTable =
				EnemyProgression.Enemy.NormalMale | EnemyProgression.Enemy.NormalLeaderMale | EnemyProgression.Enemy.NormalFemale | EnemyProgression.Enemy.NormalSkinnyMale | EnemyProgression.Enemy.NormalSkinnyFemale | EnemyProgression.Enemy.PaleMale | EnemyProgression.Enemy.PaleSkinnyMale | EnemyProgression.Enemy.PaleSkinnedMale | EnemyProgression.Enemy.PaleSkinnedSkinnyMale | EnemyProgression.Enemy.PaintedMale | EnemyProgression.Enemy.PaintedLeaderMale | EnemyProgression.Enemy.PaintedFemale | EnemyProgression.Enemy.Fireman;
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
					switch (weaponModel)
					{							
						case WeaponModelType.GreatSword:
							return GlobalSFX.SFX.Invharm;
						case WeaponModelType.LongSword:
							return GlobalSFX.SFX.Invsword;
						case WeaponModelType.Hammer:
							return GlobalSFX.SFX.Invanvl;
						case WeaponModelType.Polearm:
							return GlobalSFX.SFX.Invstaf;
						case WeaponModelType.Axe:
							return GlobalSFX.SFX.Invaxe;
						case WeaponModelType.Greatbow:
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
			return 1.15f - (float)Rarity / 15f;
		}
	}
}