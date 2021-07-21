using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChampionsOfForest
{
	public static partial class ItemDataBase
	{
		public static List<BaseItem> _Item_Bases;
		public static Dictionary<int, BaseItem> ItemBases;
		public static List<ItemStat> statList;
		public static Dictionary<int, ItemStat> Stats;

		private static Dictionary<int, List<int>> ItemRarityGroups;


		//Called from Initializer

		public static void Initialize()
		{
			_Item_Bases = new List<BaseItem>();
			ItemBases = new Dictionary<int, BaseItem>();
			statList = new List<ItemStat>();
			Stats = new Dictionary<int, ItemStat>();
			ItemRarityGroups = new Dictionary<int, List<int>>();
			PopulateStats();
			for (int i = 0; i < statList.Count; i++)
			{
				Stats.Add(statList[i].StatID, statList[i]);
			}
			try
			{
				PopulateItems();
			}
			catch (System.Exception ex)
			{
				CotfUtils.Log("Error with item " + ex.ToString());
			}
			ItemBases.Clear();
			for (int i = 0; i < _Item_Bases.Count; i++)
			{
				try
				{
					ItemBases.Add(_Item_Bases[i].ID, _Item_Bases[i]);
					if (ItemRarityGroups.ContainsKey(_Item_Bases[i].Rarity))
					{
						ItemRarityGroups[_Item_Bases[i].Rarity].Add(_Item_Bases[i].ID);
					}
					else
					{
						ItemRarityGroups.Add(_Item_Bases[i].Rarity, new List<int>() { _Item_Bases[i].ID });
					}
				}
				catch (System.Exception ex)
				{
					ModAPI.Log.Write("Error with adding an item " + ex.ToString());
				}
			}

			//LogInfo();
		}

		/// <summary>
		/// Prints a pretty summary to a log file
		/// </summary>
		public static void LogInfo()
		{
			string s = "There are " + Stats.Count + " stats:\n";
			for (int i = 0; i < 8; i++)
			{
				ItemStat[] stats = statList.Where(a => a.Rarity == i).ToArray();
				s += " • Rarity tier of stat[" + i + "] =  " + stats.Length;
				foreach (ItemStat a in stats)
				{
					s += "\n\t • Stat \"" + a.Name + "  ID [" + a.StatID + "]\"";
				}
				s += "\n";
			}
			s += "\n\n\n There are " + ItemBases.Count + " items:\n";
			for (int i = 0; i < 8; i++)
			{
				BaseItem[] items = _Item_Bases.Where(a => a.Rarity == i).ToArray();
				s += " • Rarity tier of item [" + i + "] =  " + items.Length;
				foreach (BaseItem a in items)
				{
					s += "\n\t • Item \"" + a.name + "    ID [" + a.ID + "]\"";
				}
				s += "\n";
			}

			s += "\n\n\nItem types:";
			System.Array array = System.Enum.GetValues(typeof(BaseItem.ItemType));
			for (int i = 0; i < array.Length; i++)
			{
				BaseItem.ItemType t = (BaseItem.ItemType)array.GetValue(i);
				
				BaseItem[] items = _Item_Bases.Where(a => a.type == t).ToArray();

				s += "\n • Item type: [" + t.ToString() + "] = " + items.Length;
				for (int b = 0; b < 8; b++)
				{
					BaseItem[] items2 = items.Where(a => a.Rarity == b).ToArray();
					s += "\n\t\t • RARITY " + b + " \"" + items2.Length + "\"";
				}

				foreach (BaseItem a in items)
				{
					s += "\n\t • Item \"" + a.name + "    ID [" + a.ID + "]    RARITY [" + a.Rarity + "]\"";
				}
				s += "\n";
			}

			ModAPI.Log.Write(s);


			var f = File.CreateText("items.csv");
			f.WriteLine("ITEMS;" + ItemBases.Count);
			for (int i = 0; i <=(int)BaseItem.ItemType.SpellScroll; i++)
			{
				BaseItem.ItemType t = (BaseItem.ItemType)i;
				var itemsByType = _Item_Bases.Where(a => a.type== t);
				f.WriteLine(t+";" + itemsByType.Count());

				if (t == BaseItem.ItemType.Weapon)
				{
					for (int j = 1; j <= (int)BaseItem.WeaponModelType.Greatbow; j++)
					{
						var weapons = itemsByType.Where(a => a.weaponModel == (BaseItem.WeaponModelType)j).OrderBy(a => a.ID + a.Rarity * 100000);
						f.WriteLine(((BaseItem.WeaponModelType)j) + ";" + weapons.Count());
						f.WriteLine("ID;RARITY;NAME;UNIQUE STAT;MIN LEVEL; MIN STATS QUANTITY;MAX STATS QUANTITY");
						foreach (var weapon in weapons)
						{
							f.WriteLine(weapon.ID + ";" + weapon.Rarity + ";" + weapon.name + ";" + weapon.uniqueStat + " ;" + weapon.minLevel + ";" + weapon.PossibleStats.Where(y => !y.Contains(null)).Count() + ";" + weapon.PossibleStats.Count);

						}
						f.WriteLine(" ");

					}
				}
				else
				{
					itemsByType = itemsByType.OrderBy(a => a.ID + a.Rarity * 100000);
					f.WriteLine("ID;RARITY;NAME;UNIQUE STAT;MIN LEVEL; MIN STATS QUANTITY;MAX STATS QUANTITY");
					foreach (var item in itemsByType)
					{
						f.WriteLine(item.ID + ";" + item.Rarity + ";" + item.name + ";" + item.uniqueStat + " ;" + item.minLevel + ";" + item.PossibleStats.Where(y => !y.Contains(null)).Count() + ";" + item.PossibleStats.Count);

					}
					f.WriteLine(" ");
				}
			}


			f.Close();
		}

		public static void AddItem(BaseItem item)
		{
			_Item_Bases.Add(item);
		}

		public static void AddStat(ItemStat item)
		{
			statList.Add(item);
		}

		public static ItemStat StatByID(int id)
		{
			return ItemDataBase.Stats[id];
		}

		public static BaseItem ItemBaseByName(string name)
		{
			return ItemDataBase.ItemBases.Values.Where(x => x.name == name).First();
		}

		public static void AddPercentage(ref float variable1, float f)
		{
			variable1 += ((1 - variable1) * f);
		}

		public static void RemovePercentage(ref float variable1, float f)
		{
			variable1 -= ((1 - variable1) * f);
		}
	}
}