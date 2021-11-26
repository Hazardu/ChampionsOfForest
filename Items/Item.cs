using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Items;
using ChampionsOfForest.Localization;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class Item : BaseItem
	{
		public int Amount;
		public bool Equipped;
		public List<ItemStat> Stats = new List<ItemStat>();

		private Dictionary<int, float> groupedStats;

		private void GroupStats()
		{
			var grouped = new Dictionary<int, List<float>>();
			foreach (var stat in Stats)
			{
				if (grouped.ContainsKey(stat.StatID))
					grouped[stat.StatID].Add(stat.Amount);
				else
					grouped.Add(stat.StatID, new List<float>() { stat.Amount });
			}
			groupedStats = new Dictionary<int, float>(grouped.Count);
			foreach (var group in grouped)
			{
				groupedStats.Add(group.Key, ItemDataBase.StatByID(group.Key).EvaluateTotalIncrease(group.Value));
			}
		}

		public Dictionary<int, float> GetGroupedStats()
		{
			if (Stats.Count == 0)
				return null;
			if (groupedStats == null)
				GroupStats();
			return groupedStats;
		}

		public bool CombineItems(Item other)
		{
			//if other is a socketable item
			if (other.type == ItemType.Material)
			{
				if (other.onConsume != null)
				{
					if (Equipped)
						OnUnequip();

					bool returnval = other.onConsume.Invoke(this);
					OnEquip();

					return returnval;
				}
				else if (this.Stats.Any(x => x.StatID == 3000))
				{
					if (Equipped)
					{
						OnUnequip();
					}

					int statindex = Stats.FindIndex(x => x.StatID == 3000);
					Stats[statindex] = StatActions.GetSocketedStat(other.Rarity, this.type, other.subtype);
					OnEquip();
					return true;
				}
			}
			return false;
		}

		public bool CanPlaceInSlot(in int slotIndex)
		{
			switch (slotIndex)
			{
				case -2:
					if (this.type == ItemType.Helmet)
						return true;
					break;

				case -3:
					if (this.type == ItemType.ChestArmor)
						return true;
					break;

				case -4:
					if (this.type == ItemType.Pants)
						return true;
					break;

				case -5:
					if (this.type == ItemType.Boot)
						return true;
					break;

				case -6:
					if (this.type == ItemType.ShoulderArmor)
						return true;
					break;

				case -7:
					if (this.type == ItemType.Glove)
						return true;
					break;

				case -8:
					if (this.type == ItemType.Amulet)
						return true;
					break;

				case -9:
					if (this.type == ItemType.Bracer)
						return true;
					break;

				case -10:
					if (this.type == ItemType.Ring)
						return true;
					break;

				case -11:
					if (this.type == ItemType.Ring)
						return true;
					break;

				case -12:
					if (this.type == ItemType.Weapon)
						return true;
					break;

				case -13:
					if (this.type == ItemType.Quiver || this.type == ItemType.SpellScroll || this.type == ItemType.Shield)
						return true;
					break;
			}
			return false;
		}

		public int destinationSlotID
		{
			get
			{
				switch (this.type)
				{
					case ItemType.Shield:
						return -13;

					case ItemType.Quiver:
						return -13;

					case ItemType.Weapon:
						return -12;

					case ItemType.Other:
						return -1;

					case ItemType.Material:
						return -1;

					case ItemType.Helmet:
						return -2;

					case ItemType.Boot:
						return -5;

					case ItemType.Pants:
						return -4;

					case ItemType.ChestArmor:
						return -3;

					case ItemType.ShoulderArmor:
						return -6;

					case ItemType.Glove:
						return -7;

					case ItemType.Bracer:
						return -9;

					case ItemType.Amulet:
						return -8;

					case ItemType.Ring:
						return -11;

					case ItemType.SpellScroll:
						return -13;

					default:
						return -1;
				}
			}
		}
		public string TypeName()
		{
			switch (this.type)
			{
				case BaseItem.ItemType.Shield:
					return Translations.Item_1/*og:Shield*/;    //tr
				case BaseItem.ItemType.Quiver:
					return Translations.Item_2/*og:Quiver*/;    //tr
				case BaseItem.ItemType.Weapon:
					return Translations.Item_3/*og:Weapon*/;    //tr
				case BaseItem.ItemType.Other:
					return Translations.Item_4/*og:Other*/; //tr
				case BaseItem.ItemType.Material:
					return Translations.Item_5/*og:Material*/;      //tr
				case BaseItem.ItemType.Helmet:
					return Translations.Item_6/*og:Helmet*/;    //tr
				case BaseItem.ItemType.Boot:
					return Translations.Item_7/*og:Boots*/; //tr
				case BaseItem.ItemType.Pants:
					return Translations.Item_8/*og:Pants*/; //tr
				case BaseItem.ItemType.ChestArmor:
					return Translations.Item_9/*og:Chest armor*/;   //tr
				case BaseItem.ItemType.ShoulderArmor:
					return Translations.Item_10/*og:Shoulder armor*/;    //tr
				case BaseItem.ItemType.Glove:
					return Translations.Item_11/*og:Gloves*/;    //tr
				case BaseItem.ItemType.Bracer:
					return Translations.Item_12/*og:Bracers*/;   //tr
				case BaseItem.ItemType.Amulet:
					return Translations.Item_13/*og:Amulet*/;    //tr
				case BaseItem.ItemType.Ring:
					return Translations.Item_14/*og:Ring*/;  //tr
				case BaseItem.ItemType.SpellScroll:
					return Translations.Item_15/*og:Scroll*/;    //tr		
				default:
					return type.ToString();
			}
		}


		public Item()
		{
		}

		/// <summary>
		/// creates a item based on a BaseItem object, rolls values
		/// </summary>
		public Item(BaseItem b, int amount = 1, int increasedLevel = 0, bool roll = true)
		{
			base.description = b.description;
			base.minLevel = b.minLevel;
			base.maxLevel = b.maxLevel;
			if (increasedLevel != -1)
			{
				base.level = Random.Range(minLevel, maxLevel + 1) + increasedLevel;
			}
			else
			{
				int averageLevel;
				if (GameSetup.IsMultiplayer)
				{
					int sum = ModReferences.PlayerLevels.Values.Sum();
					int count = ModReferences.PlayerLevels.Values.Count;

					if (!ModSettings.IsDedicated)
					{
						sum += ModdedPlayer.instance.level;
						count++;
					}
					else
					{
						//ModAPI.Log.Write("Is dedicated server bool set to true.");
					}
					sum = Mathf.Max(1, sum);
					count = Mathf.Max(1, count);
					sum /= count;
					averageLevel = sum;
				}
				else
				{
					averageLevel = ModdedPlayer.instance.level;
				}
				averageLevel = Mathf.Max(1, averageLevel);
				base.level = averageLevel;
			}
			base.lore = b.lore;
			base.name = b.name;
			base.onEquip = b.onEquip;
			base.onUnequip = b.onUnequip;
			base.PossibleStats = b.PossibleStats;
			base.Rarity = b.Rarity;
			base.uniqueStat = b.uniqueStat;
			base.ID = b.ID;
			base.type = b.type;
			base.StackSize = b.StackSize;
			base.icon = b.icon;
			base.onConsume = b.onConsume;
			base.CanConsume = b.CanConsume;
			base.weaponModel = b.weaponModel;
			base.lootTable = b.lootTable;
			base.subtype = b.subtype;
			Amount = amount;
			Equipped = false;
			Stats = new List<ItemStat>();
			if (roll)
			{
				RollStats();
			}
		}

		public float GetRarityMultiplier()
		{
			switch (Rarity)
			{
				case 0:
					return 0.5f;

				case 1:
					return 0.7f;

				case 2:
					return 1f;

				case 3:
					return 1.4f;

				case 4:
					return 2.3f;

				case 5:
					return 3.4f;

				case 6:
					return 4.5f;

				case 7:
					return 5.6f;
			}
			return 1;
		}

		//rolls 'amount' on every item stat on this object
		public void RollStats()
		{
			groupedStats = null;
			Stats.Clear();
			int i = 0;
			foreach (List<ItemStat> PS in PossibleStats)
			{
				int random = Random.Range(0, PS.Count);
				if (PS[random] != null)
				{
					ItemStat stat = new ItemStat(PS[random], level);
					stat.Amount *= GetRarityMultiplier();
					if (stat.ValueCap != 0)
					{
						stat.Amount = Mathf.Min(stat.Amount, stat.ValueCap);
					}
					stat.possibleStatsIndex = i;
					Stats.Add(stat);
				}
				i++;
			}

			if (this.destinationSlotID < -1 && this.level > 20 && Random.value <= 0.175f)
			{
				var socketAmount = StatActions.GetMaxSocketAmountOnItem(this.type);
				if (socketAmount > 0)
				{
					socketAmount = Random.Range(1, socketAmount + 1);
					for (int j = 0; j < socketAmount; j++)
					{
						Stats.Add(new ItemStat(ItemDataBase.StatByID(3000)));
					}
				}
			}
			SortStats();
		}
		public void SortStats()
		{
			Stats = Stats.OrderBy(x => -x.Rarity).ToList();
		}

		public void OnEquip()
		{
			Equipped = true;
			foreach (ItemStat item in Stats)
			{
				if (item.Amount != 0)
				{
					item.OnEquip?.Invoke(item.Amount);
				}
			}
			onEquip?.Invoke();
		}

		public void OnUnequip()
		{
			Equipped = false;
			foreach (ItemStat item in Stats)
			{
				if (item.Amount != 0)
				{
					item.OnUnequip(item.Amount);
				}
			}
			onUnequip?.Invoke();
		}

		public bool OnConsume()
		{
			if (CanConsume && ModdedPlayer.instance.level >= level)
			{
				onEquip?.Invoke();
				foreach (ItemStat item in Stats)
				{
					if (item.Amount != 0)
					{
						item.OnConsume(item.Amount);
					}
				}
				return true;
			}
			return false;
		}
	}
}