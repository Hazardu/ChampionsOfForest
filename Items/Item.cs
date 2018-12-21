using System.Collections.Generic;

namespace ChampionsOfForest
{
    public class Item : BaseItem
    {
        public int Amount;
        public bool Equipped;
        public List<ItemStat> Stats = new List<ItemStat>();


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
                            stat.Amount *= 0.7f;
                            break;
                        case 1:
                            stat.Amount *= 1;
                            break;
                        case 2:
                            stat.Amount *= 1.25f;
                            break;
                        case 3:
                            stat.Amount *= 1.6f;
                            break;
                        case 4:
                            stat.Amount *= 2f;
                            break;
                        case 5:
                            stat.Amount *= 2.4f;
                            break;
                        case 6:
                            stat.Amount *= 3f;
                            break;
                        case 7:
                            stat.Amount *= 3.5f;
                            break;
                    }
                    Stats.Add(stat);
                }
            }
        }
        public void OnEquip()
        {
            Equipped = true;
            foreach (ItemStat item in Stats)
            {
                if (item.Amount != 0)
                {
                    item.OnEquip(item.Amount);
                }
            }
            onEquip();
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
            onUnequip();
        }
        public bool OnConsume()
        {
            if (CanConsume)
            {
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
