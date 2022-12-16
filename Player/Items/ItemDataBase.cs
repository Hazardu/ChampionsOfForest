using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChampionsOfForest
{
	public partial class ItemDataBase
	{
		public List<ItemTemplate> itemTemplates;
		public List<ItemStat> stats;
		private readonly Dictionary<int, ItemStat> statsById;
		private Dictionary<int, List<ItemTemplate>> itemsByRarities;
		
		//Called from Initializer

		public ItemDataBase()
		{
			itemTemplates = new List<ItemTemplate>();
			stats = new List<ItemStat>();
			PopulateStats();
			statsById = stats.ToDictionary(stat => stat.id);
			for (int i = 0; i < stats.Count; i++)
			{
				statsById.Add(stats[i].id, stats[i]);
			}
			try
			{
				PopulateItems();
			}
			catch (System.Exception ex)
			{
				CotfUtils.Log("Error with item " + ex.ToString());
			}
			itemTemplates.Clear();
			for (int i = 0; i < itemTemplates.Count; i++)
			{
				try
				{
					itemTemplates.Add(itemTemplates[i]);
					if (itemsByRarities.ContainsKey(itemTemplates[i].Rarity))
					{
						itemsByRarities[itemTemplates[i].Rarity].Add(itemTemplates[i].ID);
					}
					else
					{
						itemsByRarities.Add(itemTemplates[i].Rarity, new List<int>() { itemTemplates[i].ID });
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
			string s = "There are " + statsById.Count + " stats:\n";
			for (int i = 0; i < 8; i++)
			{
				ItemStat[] stats = stats.Where(a => a.Rarity == i).ToArray();
				s += " • Rarity tier of stat[" + i + "] =  " + stats.Length;
				foreach (ItemStat a in stats)
				{
					s += "\n\t • Stat \"" + a.name + "  ID [" + a.id + "]\"";
				}
				s += "\n";
			}
			s += "\n\n\n There are " + itemTemplates.Count + " items:\n";
			for (int i = 0; i < 8; i++)
			{
				ItemTemplate[] items = itemTemplates.Where(a => a.Rarity == i).ToArray();
				s += " • Rarity tier of item [" + i + "] =  " + items.Length;
				foreach (ItemTemplate a in items)
				{
					s += "\n\t • Item \"" + a.name + "    ID [" + a.ID + "]\"";
				}
				s += "\n";
			}

			s += "\n\n\nItem types:";
			System.Array array = System.Enum.GetValues(typeof(ItemTemplate.ItemType));
			for (int i = 0; i < array.Length; i++)
			{
				ItemTemplate.ItemType t = (ItemTemplate.ItemType)array.GetValue(i);
				
				ItemTemplate[] items = itemTemplates.Where(a => a.type == t).ToArray();

				s += "\n • Item type: [" + t.ToString() + "] = " + items.Length;
				for (int b = 0; b < 8; b++)
				{
					ItemTemplate[] items2 = items.Where(a => a.Rarity == b).ToArray();
					s += "\n\t\t • RARITY " + b + " \"" + items2.Length + "\"";
				}

				foreach (ItemTemplate a in items)
				{
					s += "\n\t • Item \"" + a.name + "    ID [" + a.ID + "]    RARITY [" + a.Rarity + "]\"";
				}
				s += "\n";
			}

			ModAPI.Log.Write(s);


			var f = File.CreateText("items.csv");
			f.WriteLine("ITEMS;" + itemTemplates.Count);
			for (int i = 0; i <=(int)ItemTemplate.ItemType.SpellScroll; i++)
			{
				ItemTemplate.ItemType t = (ItemTemplate.ItemType)i;
				var itemsByType = itemTemplates.Where(a => a.type== t);
				f.WriteLine(t+";" + itemsByType.Count());

				if (t == ItemTemplate.ItemType.Weapon)
				{
					for (int j = 1; j <= (int)ItemTemplate.WeaponModelType.Greatbow; j++)
					{
						var weapons = itemsByType.Where(a => a.weaponModel == (ItemTemplate.WeaponModelType)j).OrderBy(a => a.ID + a.Rarity * 100000);
						f.WriteLine(((ItemTemplate.WeaponModelType)j) + ";" + weapons.Count());
						f.WriteLine("ID;RARITY;NAME;UNIQUE STAT;MIN LEVEL; MIN STATS QUANTITY;MAX STATS QUANTITY");
						foreach (var weapon in weapons)
						{
							f.WriteLine(weapon.ID + ";" + weapon.Rarity + ";" + weapon.name + ";" + weapon.uniqueStat + " ;" + weapon.minLevel + ";" + weapon.PossibleStats.Count(y => !y.Contains(null)) + ";" + weapon.PossibleStats.Count);

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
						f.WriteLine(item.ID + ";" + item.Rarity + ";" + item.name + ";" + item.uniqueStat + " ;" + item.minLevel + ";" + item.PossibleStats.Count(y => !y.Contains(null)) + ";" + item.PossibleStats.Count);

					}
					f.WriteLine(" ");
				}
			}


			f.Close();
		}

		public static void AddItem(ItemTemplate item)
		{
			itemTemplates.Add(item);
		}

		public static void AddStat(ItemStat item)
		{
			stats.Add(item);
		}

		public static ItemStat StatByID(int id)
		{
			return ItemDataBase.statsById[id];
		}

	
		public static void AddPercentage(ref float variable1, float f)
		{
			variable1 += ((1 - variable1) * f);
		}

		public static void RemovePercentage(ref float variable1, float f)
		{
			variable1 -= ((1 - variable1) * f);
		}

		public ItemTemplate GetItemTemplate(int id)
		{
			return itemTemplates[id];
		}
		public ItemTemplate GetItemTemplate(string name)
		{
			return itemTemplates.First(x => x.name == name);
		}

	}
}