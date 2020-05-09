using System.Collections.Generic;
using System.Linq;
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
            groupedStats =new Dictionary<int, float>(grouped.Count);
            foreach (var group in grouped)
            {
                groupedStats.Add(group.Key,ItemDataBase.StatByID(group.Key).EvaluateTotalIncrease(group.Value));
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


        public int destinationSlotID {
            get
            {
                switch (this._itemType)
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
            base.level = UnityEngine.Random.Range(minLevel, maxLevel + 1) + increasedLevel;
            base.lore = b.lore;
            base.name = b.name;
            base.onEquip = b.onEquip;
            base.onUnequip = b.onUnequip;
            base.PossibleStats = b.PossibleStats;
            base.Rarity = b.Rarity;
            base.tooltip = b.tooltip;
            base.ID = b.ID;
            base._itemType = b._itemType;
            base.StackSize = b.StackSize;
            base.icon = b.icon;
            base.onConsume = b.onConsume;
            base.CanConsume = b.CanConsume;
            base.weaponModel = b.weaponModel;
            base.LootsFrom = b.LootsFrom;
            Amount = amount;
            Equipped = false;
            Stats = new List<ItemStat>();
            if (roll)
            {
                RollStats();
            }
        }



        //rolls 'amount' on every item stat on this object 
        public void RollStats()
        {
            groupedStats = null;
            Stats.Clear();
            foreach (List<ItemStat> PS in PossibleStats)
            {

                int random = UnityEngine.Random.Range(0, PS.Count);
                if (PS[random] != null)
                {
                    ItemStat stat = new ItemStat(PS[random], level);
                    switch (Rarity)
                    {
                        case 0:
                            stat.Amount *= 0.5f;
                            break;
                        case 1:
                            stat.Amount *= 0.7f;
                            break;
                        case 2:
                            stat.Amount *= 1f;
                            break;
                        case 3:
                            stat.Amount *= 1.4f;
                            break;
                        case 4:
                            stat.Amount *= 2.3f;
                            break;
                        case 5:
                            stat.Amount *= 3.4f;
                            break;
                        case 6:
                            stat.Amount *= 4.5f;
                            break;
                        case 7:
                            stat.Amount *= 5.6f;
                            break;
                    }
                    if (stat.ValueCap != 0)
                    {
                        stat.Amount = Mathf.Min(stat.Amount, stat.ValueCap);
                    }

                    Stats.Add(stat);
                }
            }
            Stats.Sort((y, x) => x.Rarity.CompareTo(y.Rarity));
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
            if (CanConsume && ModdedPlayer.instance.Level >= level)
            {
                onConsume?.Invoke();
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
