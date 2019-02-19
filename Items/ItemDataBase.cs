using ChampionsOfForest.Items;
using ChampionsOfForest.Player;
using System.Collections.Generic;
using System.Linq;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public static class ItemDataBase
    {

        public static List<BaseItem> _Item_Bases;
        public static Dictionary<int, BaseItem> ItemBases;
        public static List<ItemStat> statList;
        public static Dictionary<int, ItemStat> Stats;

        private static Dictionary<int, List<int>> ItemRarityGroups;

        public static float MagicFind = 1;



        //Called from Initializer

        public static void Initialize()
        {

            _Item_Bases = new List<BaseItem>();
            ItemBases = new Dictionary<int, BaseItem>();
            statList = new List<ItemStat>();
            Stats = new Dictionary<int, ItemStat>();
            ItemRarityGroups = new Dictionary<int, List<int>>();
            FillStats();

            Stats.Clear();
            for (int i = 0; i < statList.Count; i++)
            {
                try
                {
                    Stats.Add(statList[i].StatID, statList[i]);
                    ModAPI.Log.Write("[" + statList[i].StatID + "]\t" + statList[i].Name);
                }
                catch (System.Exception ex)
                {
                    ModAPI.Log.Write("Error with adding a stat " + ex.ToString());
                }
            }
            try
            {
                FillItems();
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write("Error with item " + ex.ToString());

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
                BaseItem[] items = _Item_Bases.Where(a => a._itemType == t).ToArray();

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
        public static Item GetRandomItem(float Worth)
        {

            //this needs to be changed to take random value of average of all player levels - and exclude the level of dedicated server.
            int averageLevel = 1;
            if (GameSetup.IsMultiplayer)
            {
                int sum = ModReferences.PlayerLevels.Values.Sum();
                int count = ModReferences.PlayerLevels.Values.Count;

                if (!ModSettings.IsDedicated)
                {
                    sum += ModdedPlayer.instance.Level;
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
                averageLevel = ModdedPlayer.instance.Level;
            }
            averageLevel = Mathf.Max(1, averageLevel);
            float w = Worth / (averageLevel*0.55f);
            w *= MagicFind;
           
            int rarity = 0;

            if ((w > 200 && Random.value < 0.80f) || (int)ModSettings.difficulty > 5 || w > 1500)
            {
                rarity = 1;

                if (w > 350 && Random.value < 0.60f || w > 1700 || (int)ModSettings.difficulty > 6)
                {
                    rarity = 2;

                    if (w > 500 && Random.value < 0.60f && (int)ModSettings.difficulty > 1 || Random.value < (0.09f * (int)ModSettings.difficulty) || w > 2300)
                    {
                        rarity = 3;

                        if (w > 625 && Random.value < 0.6f && (int)ModSettings.difficulty > 2 || Random.value < (0.05f * (int)ModSettings.difficulty) || w > 2900)
                        {
                            rarity = 4;

                            if (w > 700 && Random.value < 0.50f && (int)ModSettings.difficulty > 3 || Random.value < (0.04f * (int)ModSettings.difficulty) || (w > 3500 && (int)ModSettings.difficulty > 2))
                            {
                                rarity = 5;

                                if (w > 800 && Random.value < 0.5f && (int)ModSettings.difficulty > 4 || Random.value < (0.02f * (int)ModSettings.difficulty) || (w > 4600 && (int)ModSettings.difficulty > 3))
                                {
                                    rarity = 6;

                                    if (w > 950 && Random.value < 0.40f && (int)ModSettings.difficulty > 5 || Random.value < (0.01f * (int)ModSettings.difficulty) || (w > 5250 && (int)ModSettings.difficulty > 4))
                                    {
                                        rarity = 7;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            while (!ItemRarityGroups.ContainsKey(rarity) && rarity > 0)
            {
                rarity--;
            }

            int[] itemPool = null;
            while (itemPool == null)
            {
                itemPool = ItemRarityGroups[rarity].Where(i => ((ItemBases[i].minLevel) <= averageLevel + 12)).ToArray();
                if (itemPool.Length == 0)
                {
                    rarity--;
                    itemPool = null;
                }

            }

            int randomID = Random.Range(0, itemPool.Length);

            Item item = new Item(ItemBases[itemPool[randomID]], 1, 0);


            item.level = Mathf.Max(item.level, Random.Range(averageLevel - 4, averageLevel + 1));
            item.RollStats();
            return item;

        }
        public static bool AllowItemDrop(int i, int averageLevel, EnemyProgression.Enemy e)
        {
            if (!ItemBases.ContainsKey(i))
            {
                return true;
            }

            if (ItemBases[i].minLevel <= averageLevel + 12)
            {
                if (ItemBases[i].LootsFrom == null)
                {
                    return true;
                }
                if (ItemBases[i].LootsFrom.Contains(e))
                {
                    return true;
                }
            }
            return false;
        }
        public static Item GetRandomItem(float Worth, EnemyProgression.Enemy killedEnemyType)
        {

            //this needs to be changed to take random value of average of all player levels - and exclude the level of dedicated server.
            int averageLevel = 1;
            if (GameSetup.IsMultiplayer)
            {
                int sum = ModReferences.PlayerLevels.Values.Sum();
                int count = ModReferences.PlayerLevels.Values.Count;

                if (!ModSettings.IsDedicated)
                {
                    sum += ModdedPlayer.instance.Level;
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
                averageLevel = ModdedPlayer.instance.Level;
            }
            averageLevel = Mathf.Max(1, averageLevel);
            float w = Worth / averageLevel;
            int rarity = 0;

            if ((w > 200 && Random.value < 0.80f) || (int)ModSettings.difficulty > 5 || w > 1500)
            {
                rarity = 1;

                if (w > 350 && Random.value < 0.60f || w > 1700 || (int)ModSettings.difficulty > 6)
                {
                    rarity = 2;

                    if (w > 500 && Random.value < 0.60f && (int)ModSettings.difficulty > 1 || Random.value < (0.09f * (int)ModSettings.difficulty) || w > 2300)
                    {
                        rarity = 3;

                        if (w > 625 && Random.value < 0.6f && (int)ModSettings.difficulty > 2 || Random.value < (0.05f * (int)ModSettings.difficulty) || w > 2900)
                        {
                            rarity = 4;

                            if (w > 700 && Random.value < 0.50f && (int)ModSettings.difficulty > 3 || Random.value < (0.04f * (int)ModSettings.difficulty) || (w > 3500 && (int)ModSettings.difficulty > 2))
                            {
                                rarity = 5;

                                if (w > 800 && Random.value < 0.5f && (int)ModSettings.difficulty > 4 || Random.value < (0.02f * (int)ModSettings.difficulty) || (w > 4600 && (int)ModSettings.difficulty > 3))
                                {
                                    rarity = 6;

                                    if (w > 950 && Random.value < 0.40f && (int)ModSettings.difficulty > 5 || Random.value < (0.01f * (int)ModSettings.difficulty) || (w > 5250 && (int)ModSettings.difficulty > 4))
                                    {
                                        rarity = 7;

                                    }
                                }
                            }
                        }
                    }
                }
            }


            int[] itemPool = null;
            while (itemPool == null)
            {
                itemPool = ItemRarityGroups[rarity].Where(i => (AllowItemDrop(i, averageLevel, killedEnemyType))).ToArray();
                if (itemPool.Length == 0)
                {
                    rarity--;
                    itemPool = null;
                }

            }

            int randomID = Random.Range(0, itemPool.Length);

            Item item = new Item(ItemBases[itemPool[randomID]], 1, 0);


            item.level = Mathf.Max(item.level, Random.Range(averageLevel - 4, averageLevel + 1));
            item.RollStats();
            return item;

        }
        public static void RequestMagicFind()
        {
            MagicFind = ModdedPlayer.instance.MagicFindMultipier;
            if (GameSetup.IsMultiplayer)
            {
                Network.NetworkManager.SendLine("AD", Network.NetworkManager.Target.Clinets);
            }
        }


        public static void FillStats()
        {
            int i = 1;
            new ItemStat(i, 1, 2, 1.4f, "Strenght", 2, StatActions.AddStrenght, StatActions.RemoveStrenght, StatActions.AddStrenght); i++;
            new ItemStat(i, 1, 2, 1.4f, "Agility", 2, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility); i++;
            new ItemStat(i, 1, 2, 1.4f, "Vitality", 2, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality); i++;
            new ItemStat(i, 1, 2, 1.4f, "Intelligence", 2, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence); i++;
            new ItemStat(i, 2, 4f, 1.55f, "Maximum Life", 3, StatActions.AddHealth, StatActions.RemoveHealth, StatActions.AddHealth); i++;
            new ItemStat(i, 0.5f, 1f, 1.2f, "Maximum Energy", 3, StatActions.AddEnergy, StatActions.RemoveEnergy, StatActions.AddEnergy); i++;
            new ItemStat(i, 0.01f, 0.025f, 1.4f, "Life Per Second", 3, StatActions.AddHPRegen, StatActions.RemoveHPRegen, StatActions.AddHPRegen) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.01f, 0.025f, 1.4f, "Stamina Per Second", 3, StatActions.AddStaminaRegen, StatActions.RemoveStaminaRegen, StatActions.AddStaminaRegen) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.01f, 0.025f, 0.7f, "Stamina Regen %", 3, StatActions.AddStaminaRegenPercent, StatActions.RemoveStaminaRegenPercent, StatActions.AddStaminaRegenPercent) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.01f, 0.025f, 0.7f, "Life Regen %", 3, StatActions.AddHealthRegenPercent, StatActions.RemoveHealthRegenPercent, StatActions.AddHealthRegenPercent) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.003f, 0.009f, 0.6f, "Damage Reduction %", 4, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, StatActions.AddDamageReduction) { ValueCap = 0.3f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.002f, 0.005f, 0.6f, "Critical Hit Chance", 6, StatActions.AddCritChance, StatActions.RemoveCritChance, StatActions.AddCritChance) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.25f, "Critical Hit Damage", 6, StatActions.AddCritDamage, StatActions.RemoveCritDamage, StatActions.AddCritDamage) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.1f, 0.35f, 1.3f, "Life on hit", 4, StatActions.AddLifeOnHit, StatActions.RemoveLifeOnHit, StatActions.AddLifeOnHit); i++;
            new ItemStat(i, 0.002f, 0.003f, 0.65f, "Dodge chance", 5, StatActions.AddDodgeChance, StatActions.RemoveDodgeChance, StatActions.AddDodgeChance) { ValueCap = 0.3f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 3f, 1.6f, "Armor", 2, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.001f, 0.003f, 0.5f, "Resistance to magic", 5, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, StatActions.AddMagicResistance) { ValueCap = 0.95f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.005f, 0.01f, 0.5f, "Attack speed", 6, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, StatActions.AddAttackSpeed) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.003f, 0.007f, 0.6f, "Exp %", 6, StatActions.AddExpFactor, StatActions.RemoveExpFactor, StatActions.AddExpFactor) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 2f, 0.6f, "Massacre Duration", 6, StatActions.AddMaxMassacreTime, StatActions.RemoveMaxMassacreTime, StatActions.AddMaxMassacreTime); i++;
            new ItemStat(i, 0.002f, 0.003f, 0.6f, "Spell Damage %", 4, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, StatActions.AddSpellDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.002f, 0.003f, 0.6f, "Melee Damage %", 4, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, StatActions.AddMeleeDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.002f, 0.003f, 0.6f, "Ranged Damage %", 4, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, StatActions.AddRangedDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.55f, 0.7f, 1.3f, "Bonus Spell Damage", 4, StatActions.AddSpellDamageBonus, StatActions.RemoveSpellDamageBonus, StatActions.AddSpellDamageBonus); i++;
            new ItemStat(i, 0.55f, 0.7f, 1.3f, "Bonus Melee Damage", 4, StatActions.AddMeleeDamageBonus, StatActions.RemoveMeleeDamageBonus, StatActions.AddMeleeDamageBonus); i++;
            new ItemStat(i, 0.55f, 0.7f, 1.3f, "Bonus Ranged Damage", 4, StatActions.AddRangedDamageBonus, StatActions.RemoveRangedDamageBonus, StatActions.AddRangedDamageBonus); i++;
            new ItemStat(i, 0.0005f, 0.001f, 0.5f, "Energy Per Agility", 7, StatActions.AddEnergyPerAgility, StatActions.RemoveEnergyPerAgility, StatActions.AddEnergyPerAgility) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.005f, 0.008f, 0.5f, "Health Per Vitality", 7, StatActions.AddHealthPerVitality, StatActions.RemoveHealthPerVitality, StatActions.AddHealthPerVitality) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0004f, 0.0009f, 0.5f, "Spell Damage Per Int", 7, StatActions.AddSpellDamageperInt, StatActions.RemoveSpellDamageperInt, StatActions.AddSpellDamageperInt) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0004f, 0.0009f, 0.5f, "Damage Per Strenght", 7, StatActions.AddDamagePerStrenght, StatActions.RemoveDamagePerStrenght, StatActions.AddDamagePerStrenght) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.00025f, 0.001f, 0.6f, "All Healing %", 7, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, StatActions.AddHealingMultipier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.7f / 3.8f, 1.6f / 3.8f, 0f, "PERMANENT PERK POINTS", 6, null, null, StatActions.PERMANENT_perkPointIncrease); i++;
            new ItemStat(i, 100f, 200f, 3.2f, "EXPERIENCE", 5, null, null, StatActions.PERMANENT_expIncrease); i++;
            new ItemStat(i, 0.0025f, 0.007f, 0.5f, "Movement Speed", 5, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, StatActions.AddMoveSpeed) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0002f, 0.0015f, 0.5f, "Melee Weapon Range", 5, f => ModdedPlayer.instance.MeleeRange += f, f => ModdedPlayer.instance.MeleeRange -= f, f => ModdedPlayer.instance.MeleeRange += f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.001f, 0.002f, 0.5f, "Attack Cost Reduction", 3, f => ModdedPlayer.instance.StaminaAttackCost *= 1 - f, f => ModdedPlayer.instance.StaminaAttackCost /= 1 - f, f => ModdedPlayer.instance.StaminaAttackCost *= 1 - f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.001f, 0.002f, 0.5f, "Spell Cost Reduction", 6, f => ModdedPlayer.instance.SpellCostRatio *= 1 - f, f => ModdedPlayer.instance.SpellCostRatio /= 1 - f, f => ModdedPlayer.instance.SpellCostRatio *= 1 - f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.002f, 0.003f, 0.5f, "Spell Cost to Stamina", 6, f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => RemovePercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f)) { ValueCap = 0.95f, DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.6f, 1.2f, 1.3f, "Strenght", 1, StatActions.AddStrenght, StatActions.RemoveStrenght, StatActions.AddStrenght); i++;
            new ItemStat(i, 0.6f, 1.2f, 1.3f, "Agility", 1, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility); i++;
            new ItemStat(i, 0.6f, 1.2f, 1.3f, "Vitality", 1, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality); i++;
            new ItemStat(i, 0.6f, 1.2f, 1.3f, "Intelligence", 1, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence); i++;
            new ItemStat(i, 0.4f, 1.5f, 1.5f, "Armor", 1, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1f, "Energy Per Second", 6, StatActions.AddEnergyRegen, StatActions.RemoveEnergyRegen, StatActions.AddEnergyRegen) { RoundingCount = 2 }; i++;
            new ItemStat(i, 0.001f, 0.005f, 0.5f, "Maximum Life %", 5, f => ModdedPlayer.instance.MaxHealthPercent += f, f => ModdedPlayer.instance.MaxHealthPercent -= f, f => ModdedPlayer.instance.MaxHealthPercent += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.005f, 0.5f, "Maximum Energy %", 5, f => ModdedPlayer.instance.MaxEnergyPercent += f, f => ModdedPlayer.instance.MaxEnergyPercent -= f, f => ModdedPlayer.instance.MaxEnergyPercent += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.002f, 0.004f, 0.5f, "Cooldown Reduction", 7, f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier /= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f)) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.9f }; i++;
            new ItemStat(i, 0.0004f, 0.0009f, 0.5f, "Ranged Damage Per Agi", 7, f => ModdedPlayer.instance.RangedDamageperAgi += f, f => ModdedPlayer.instance.RangedDamageperAgi += -f, f => ModdedPlayer.instance.RangedDamageperAgi += f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.01f, 0.05f, 1.1f, "Energy on hit", 4, f => ModdedPlayer.instance.EnergyOnHit += f, f => ModdedPlayer.instance.EnergyOnHit += -f, f => ModdedPlayer.instance.EnergyOnHit += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 0.01f, 0.035f, 0.8f, "Block", 2, f => ModdedPlayer.instance.BlockFactor += f, f => ModdedPlayer.instance.BlockFactor += -f, f => ModdedPlayer.instance.BlockFactor += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.01f, 0.5f, "Projectile speed", 5, f => ModdedPlayer.instance.ProjectileSpeedRatio += f, f => ModdedPlayer.instance.ProjectileSpeedRatio += -f, f => ModdedPlayer.instance.ProjectileSpeedRatio += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.01f, 0.5f, "Projectile size", 6, f => ModdedPlayer.instance.ProjectileSizeRatio += f, f => ModdedPlayer.instance.ProjectileSizeRatio += -f, f => ModdedPlayer.instance.ProjectileSizeRatio += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.4f, 1.3f, 1.2f, "Melee armor piercing", 5, f => ModdedPlayer.instance.ARreduction_melee += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_melee += -Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_melee += Mathf.RoundToInt(f)); i++;
            new ItemStat(i, 0.4f, 1.3f, 1.2f, "Ranged armor piercing", 5, f => ModdedPlayer.instance.ARreduction_ranged += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_ranged += -Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_ranged += Mathf.RoundToInt(f)); i++;
            new ItemStat(i, 0.2f, 0.6f, 1.2f, "Armor piercing", 5, f => ModdedPlayer.instance.ARreduction_all += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_all += -Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_all += Mathf.RoundToInt(f)); i++; new ItemStat(i, 0.006f, 0.01f, 0.5f, "Magic Find", 5, StatActions.AddMagicFind, StatActions.RemoveMagicFind, StatActions.AddMagicFind) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.33f, 0.66f, 1.3f, "All attributes", 5, StatActions.AddAllStats, StatActions.RemoveAllStats, StatActions.AddAllStats); i++;
            new ItemStat(i, 1 / 5, 1 / 5, 0, "PURGE", 7, null, null, f => ModdedPlayer.Respec()); i++;
            new ItemStat(i, 0.0025f, 0.0075f, 0.8f, "Jump Power", 5, StatActions.AddJump, StatActions.RemoveJump, StatActions.AddJump) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            //Extra carry items
            i = 1000;
            new ItemStat(i, 7f, 10f, 0f, "Extra Sticks", 3, f => ModdedPlayer.instance.AddExtraItemCapacity(57, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(57, -Mathf.RoundToInt(f)), null); i++;
            new ItemStat(i, 3f, 5f, 0f, "Extra Rocks", 3, f => ModdedPlayer.instance.AddExtraItemCapacity(53, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(53, -Mathf.RoundToInt(f)), null); i++;
            new ItemStat(i, 2f, 5f, 0f, "Extra Ropes", 3, f => ModdedPlayer.instance.AddExtraItemCapacity(54, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(43, -Mathf.RoundToInt(f)), null); i++;

            i = 2000;
            new ItemStat(i, 3f, 9f, 0f, "BLACK HOLE RADIUS", 6, f => SpellActions.BLACKHOLE_radius += f, f => SpellActions.BLACKHOLE_radius += -f, f => SpellActions.BLACKHOLE_radius += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 0.4f, 2f, 0f, "BLACK HOLE DURATION", 6, f => SpellActions.BLACKHOLE_duration += f, f => SpellActions.BLACKHOLE_duration += -f, f => SpellActions.BLACKHOLE_duration += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 3f, 0f, "BLACK HOLE FORCE", 6, f => SpellActions.BLACKHOLE_pullforce += f, f => SpellActions.BLACKHOLE_pullforce += -f, f => SpellActions.BLACKHOLE_pullforce += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 0.0125f, 0.025f, 1f, "BLACK HOLE DAMAGE", 6, f => SpellActions.BLACKHOLE_damage += f, f => SpellActions.BLACKHOLE_damage += -f, f => SpellActions.BLACKHOLE_damage += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 1, 1, 0, "HAMMER", 7, f => ModdedPlayer.instance.IsHammerStun = true, f => ModdedPlayer.instance.IsHammerStun = false, null); i++;
            new ItemStat(i, 1, 1.4f, 0, "Snap Freeze Duration", 7, f => SpellActions.SnapFreezeDuration += f, f => SpellActions.SnapFreezeDuration -= f, null); i++;

        }

        public static void FillItems()
        {
            try
            {

                new BaseItem(new int[][]
           {
                new int[] { 34 },
                new int[] {43,0 },
           })
                {
                    name = "Broken Shoes",
                    description = "A pair of damaged shoes. Judging by their condition, i can imagine what happened to their owner.",
                    lore = "Worn by one of the passengers of the plane that Eric also flew in.",
                    tooltip = "Shoes can provide movement speed bonuses.",
                    Rarity = 0,
                    minLevel = 1,
                    maxLevel = 10,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                };
                new BaseItem(new int[][]
                {
                new int[] {34 },
                new int[] {34,0,40,41 },
                new int[] {43 },
                new int[] {43,0 },
                })
                {
                    name = "Old Boots",
                    description = "A pair of old boots. They must have been lying here for ages.",
                    lore = "Found on the Peninsula, but judging by their condition, they belong neither to a plane passenger nor a cannibal.",
                    tooltip = "Shoes can provide movement speed bonuses.",
                    Rarity = 0,
                    minLevel = 10,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                };
                new BaseItem(new int[][]
               {
                new int[] {34 },
                new int[] {34,40,41 },
                new int[] {43,3,2 },
                new int[] {43 },
               })
                {
                    name = "Worn Leather Boots",
                    description = "A pair of leather boots. They look good and have only some scratches.",
                    lore = "They arrived to the Peninsula the same way Eric did. Since they were in a baggage, they avoided a lot of damage.",
                    tooltip = "Shoes can provide movement speed bonuses.",
                    Rarity = 1,
                    minLevel = 5,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                };
                new BaseItem(new int[][]
               {
                new int[] {34 },
                new int[] {3,2 },
                new int[] {43,3,2,1,4 },
                new int[] {43 },
               })
                {
                    name = "New Leather Boots",
                    description = "A pair of leather boots. They are in a very good condition.",
                    lore = "They arrived to the Peninsula the same way Eric did. Eric found them undamaged in their original box. They still had a pricetag - $419,99.",
                    tooltip = "Shoes can provide movement speed bonuses.",
                    Rarity = 2,
                    minLevel = 7,
                    maxLevel = 30,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                };
                new BaseItem(new int[][]
               {
               new int[] {34 },
                new int[] {34,39,41,11 },
                new int[] {16,39,41 },
                new int[] {16,7,8 },
                new int[] {43 },
                })
                {
                    name = "Damaged Army Boots",
                    description = "Sturdy, hard, resistant but damaged boots.",
                    lore = "They look modern, almost too modern for everything here.",
                    tooltip = "Shoes can provide movement speed bonuses.",
                    Rarity = 3,
                    minLevel = 10,
                    maxLevel = 30,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                };
                new BaseItem(new int[][]
               {
                new int[] {34 },
                new int[] {34,3,2,11 },
                new int[] {16,3,2,1,4 },
                new int[] {16,7,8 },
                new int[] {43 },
               })
                {
                    name = "Army Boots",
                    description = "Sturdy, hard, resistant boots.",
                    lore = "They look modern, almost too modern for everything here.",
                    tooltip = "Shoes can provide movement speed bonuses.",
                    Rarity = 4,
                    minLevel = 15,
                    maxLevel = 40,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                };
                new BaseItem(new int[][]
               {
               new int[] {25,22 },
               new int[] {11,1,3,17 },
               new int[] {11,1,3,17 },
               new int[] {11,1 },
               new int[] {1, },
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
                    maxLevel = 40,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Boot,
                    icon = Res.ResourceLoader.GetTexture(85),
                }.DropSettings_OnlyArmsy();
                new BaseItem(new int[][]
          {
                new int[] {42,0 },
                new int[] {40 },
                new int[] {43,0 },
          })
                {
                    name = "Finger Warmer",
                    description = "A little glove to keep your fingers warm and cozy.",
                    lore = "Made of wool.",
                    tooltip = "Gloves offer protection.",
                    Rarity = 0,
                    minLevel = 1,
                    maxLevel = 10,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Glove,
                    icon = Res.ResourceLoader.GetTexture(86),
                };
                new BaseItem(new int[][]
                {
                new int[] {39,40,41,42,43,24,25,26 },
                new int[] {39,40,41,42,43,24,25,26,44 },
                new int[] {43,0,7,0,5,6,8,0,0,0,0,21,22,23,16 },
                new int[] {43,0,7,0,5,6,8,0,0,0,0,21,22,23 },
                })
                {
                    name = "Thick Rubber Glove",
                    description = "A glove that helps get a better grip.",
                    lore = "Made of wool.",
                    tooltip = "Gloves offer protection and stat bonuses.",
                    Rarity = 1,
                    minLevel = 1,
                    maxLevel = 10,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Glove,
                    icon = Res.ResourceLoader.GetTexture(86),
                };
                new BaseItem(new int[][]
                {
                new int[] {39,40,41,42},
                new int[] {39,40,41,42},
                new int[] {1,2,3,4,5,6,7},
                new int[] {0,18,14,0,0,0},
                new int[] {43,0,0,5,6,0,0,21,22,23,16 },
                })
                {
                    name = "Tribal Glove",
                    description = "Offers medicore protection.",
                    lore = "Glove made out of thin bones, some may possibly be from a human.",
                    tooltip = "Gloves offer protection and stat bonuses.",
                    Rarity = 3,
                    minLevel = 10,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Glove,
                    icon = Res.ResourceLoader.GetTexture(86),
                };
                new BaseItem(new int[][]
              {
                new int[] {1,2,4,6,8,9},
                new int[] {1,0},
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
                    minLevel = 10,
                    maxLevel = 25,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Glove,
                    icon = Res.ResourceLoader.GetTexture(86),
                };
                new BaseItem(new int[][]
           {
                new int[] {43,0 },
                new int[] {43,0 },
           })
                {
                    name = "Worn Shorts",
                    description = "Some protection for legs.",
                    lore = "Short, made out of cheap thin fabric, and on top of that they are damaged. But its better than nothing.",
                    tooltip = "Pants provide armor and sometimes they increase carrying capactiy.",
                    Rarity = 0,
                    minLevel = 1,
                    maxLevel = 10,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Pants,
                    icon = Res.ResourceLoader.GetTexture(87),
                };
                new BaseItem(new int[][]
               {
                new int[] {1000,1001,1002},
                new int[] {8,9,0,0,0,0 },
               })
                {
                    name = "Cargo Shorts",
                    description = "No protection at all but they allow to carry more items.",
                    lore = "They are ugly as hell tho",
                    tooltip = "Pants provide armor and sometimes they increase carrying capactiy.",
                    Rarity = 1,
                    minLevel = 1,
                    maxLevel = 10,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Pants,
                    icon = Res.ResourceLoader.GetTexture(87),
                };
                new BaseItem(new int[][]
                        {
                new int[] {5 },
                new int[] {43,16,0,41 },
                new int[] {39,40,41,42,43,44,0,0,0,0,0,0},
                        })
                {
                    name = "Worn Jacket",
                    description = "It's a little torn. ",
                    lore = "This jacket was worn by Preston A. the 34th passenger on the plane. Eric talked to him at the airport. Guy was odd, and now he's dead.",
                    tooltip = "Chest armor increases health and gives armor",
                    Rarity = 0,
                    minLevel = 1,
                    maxLevel = 10,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),

                };
                new BaseItem(new int[][]
               {
                new int[] {5 },
                new int[] {43,16,0,41,3,2,1 },
                new int[] {6,7,8,9,10,16,17,31 },
                new int[] {39,40,41,42,43,44,0,0,0,0,0,0},
               })
                {
                    name = "Jacket",
                    description = "Offers little protection",
                    lore = "This jacket was in a baggage of one of the plane passengers",
                    tooltip = "Chest armor increases health and gives armor",
                    Rarity = 1,
                    minLevel = 4,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),

                };
                new BaseItem(new int[][]
               {
                new int[] {5 },
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
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),
                };
                new BaseItem(new int[][]
               {
                new int[] {5 },
                new int[] {5,3,4,2,1 },
                new int[] {14,0,0,0 },
                new int[] {6,8,9},
                new int[] {6,8,9},
                new int[] {12,13,0,0,0,0,0 },

               })
                {
                    name = "Crocodile Skin Armor",
                    description = "",
                    lore = "",
                    tooltip = "",
                    Rarity = 2,
                    minLevel = 7,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),
                };
                BaseItem baseItem1 = new BaseItem(new int[][]
               {
                new int[] {5 },
                new int[] {16 },
                new int[] {18,17,16},
                new int[] {11},
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
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),
                };
                baseItem1.PossibleStats[1][0].Multipier = 2.5f;
                new BaseItem(new int[][]
               {
                new int[] {5 },
                new int[] {16},
                new int[] {16,43},
                new int[] {16,0,43},
                new int[] {6,8,9},
                new int[] {45,43,39,42},
                new int[] {7,10,11,17,18,31},

               })
                {
                    name = "Bear Skin Armor",
                    description = "",
                    lore = "",
                    tooltip = "",
                    Rarity = 3,
                    minLevel = 7,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
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
                new int[] {16,23,4,5,6},
                new int[] {16,23,4,5,6,0,0,0,0},
               })
                {
                    name = "Archer's Gear",
                    description = "",
                    lore = "",
                    tooltip = "",
                    Rarity = 5,
                    minLevel = 7,
                    maxLevel = 20,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),
                }; new BaseItem(new int[][]
               {
                new int[] {5 },
                new int[] {23,26 },
                new int[] {23,26 },
                new int[] {12,13},
                new int[] {13,23,26},
                new int[] {23,26},
                new int[] {34,2,2},
                new int[] {15,14},
                new int[] {16,23,4,5,6},
                new int[] {45,46},
                new int[] {27,48},

               })
                {
                    name = "Hazard's Gear",
                    description = "",
                    lore = "",
                    tooltip = "",
                    Rarity = 6,
                    minLevel = 20,
                    maxLevel = 30,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
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
                    tooltip = "???????????",
                    Rarity = 7,
                    minLevel = 70,
                    maxLevel = 75,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ChestArmor,
                    icon = Res.ResourceLoader.GetTexture(96),
                };
                new BaseItem(new int[][]
                        {
                new int[] {39,40,41,42,44,8,14,49 },

                        })
                {
                    name = "Rusty Longsword",
                    description = "A long, very heavy sword. Edge got dull over time. Still, it's in a condition that allows me to slice some enemies in half.",
                    lore = "The sword appears to be from medieval ages, through it's not. It was made a lot later. It never was used as a weapon in battles, because it was merely a decoration.",
                    tooltip = "Long swords are slow, have little block and consume a lot of stamina. They have extraordinary weapon range and high damage",
                    Rarity = 3,
                    minLevel = 1,
                    maxLevel = 5,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Weapon,
                    weaponModel = BaseItem.WeaponModelType.LongSword,
                    icon = Res.ResourceLoader.GetTexture(89),
                };
                new BaseItem(new int[][]
              {
                new int[] {25 },
                new int[] {6,49},
                new int[] {22,0,25,1,2,3,4},
                new int[] {1,2,3,4},
                new int[] {39,40,41,42,44,8,18 },
              })
                {
                    name = "Longsword",
                    description = "Sharp and long",
                    lore = "The sword is in perfect contidion.",
                    tooltip = "Long swords are slow, have little block and consume a lot of stamina. They have extraordinary weapon range and high damage",
                    Rarity = 5,
                    minLevel = 10,
                    maxLevel = 15,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Weapon,
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
                new int[] {39,40,41,42,44,8 },
              })
                {
                    name = "Full Metal Sword",
                    description = "It's sooo big...",
                    lore = "A normal human cannot lift this.",
                    tooltip = "Greatswords are giant, incredibly slow and hard hitting.",
                    Rarity = 6,
                    minLevel = 15,
                    maxLevel = 25,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Weapon,
                    weaponModel = BaseItem.WeaponModelType.GreatSword,
                    icon = Res.ResourceLoader.GetTexture(88),
                }; new BaseItem(new int[][]
              {
                new int[] {25 },
                new int[] {25,22 },
                new int[] {49 },
                new int[] {14 },
                new int[] {14 },
                new int[] {14,31,49 },
                new int[] {14,18,49 },
                new int[] {38,36,1,2,3,4,5,6,16 },

              })
                {
                    name = "The Leech",
                    description = "Hey where did my health g- oh it's back...",
                    lore = "",
                    tooltip = "Greatswords are giant, incredibly slow and hard hitting.",
                    Rarity = 6,
                    minLevel = 15,
                    maxLevel = 25,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Weapon,
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
                new int[] {2,23,26,20,16,15},
                new int[] {52,0,0},

                      })
                {
                    name = "Smokey's Sacred Quiver",
                    description = "SmokeyTheBear died because he never used this item.",
                    lore = "Smokey was the friend of allmighty Hazard, who can materialize any kind of weapon at the snap of his fingers. Hazard remebered Smokey's favourite playstyle and he gave him this as a gift to purge the sh** out of mutants.",
                    tooltip = "This quiver makes you shoot special projectiles from your bows",
                    Rarity = 7,
                    minLevel = 5,
                    maxLevel = 50,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Quiver,
                    icon = Res.ResourceLoader.GetTexture(98),
                    onEquip = () => ModdedPlayer.instance.IsSacredArrow = true,
                    onUnequip = () => ModdedPlayer.instance.IsSacredArrow = false,
                };
                new BaseItem(new int[][]
             {
                new int[] {0,42 },
                new int[] {50 },
                new int[] {43,16 },


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
                    _itemType = BaseItem.ItemType.Shield,
                    icon = Res.ResourceLoader.GetTexture(99),

                }; new BaseItem(new int[][]
           {
                new int[] {1,2,3,4,0,42 },
                new int[] {0,42 },
                new int[] {50 },
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
                    _itemType = BaseItem.ItemType.Shield,
                    icon = Res.ResourceLoader.GetTexture(99),

                }; new BaseItem(new int[][]
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
                    maxLevel = 15,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Shield,
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
                    _itemType = BaseItem.ItemType.ShoulderArmor,
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
                    _itemType = BaseItem.ItemType.ShoulderArmor,
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

             })
                {
                    name = "Phase Pauldrons",
                    description = "The distance of teleport is increased by 40 meters, and teleport now hits everything that you teleported through",
                    lore = "",
                    tooltip = "Provides usefull stat bonuses",
                    Rarity = 7,
                    minLevel = 5,
                    maxLevel = 50,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ShoulderArmor,
                    icon = Res.ResourceLoader.GetTexture(95),
                    onEquip = () => { Player.SpellActions.BlinkRange += 40; Player.SpellActions.BlinkDamage += 60; },
                    onUnequip = () => { Player.SpellActions.BlinkRange -= 40; Player.SpellActions.BlinkDamage -= 60; },
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
                    Rarity = 0,
                    minLevel = 1,
                    maxLevel = 3,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Helmet,
                    icon = Res.ResourceLoader.GetTexture(91),
                };
                new BaseItem(new int[][]
                {
                new int[] {2000},
                new int[] {2001},
                new int[] {2002},
                new int[] {2003},
                new int[] {16},
                new int[] {16},
                new int[] {21,6},
                })
                {
                    name = "Hubble's Vision",
                    description = "Wearing this item empowers your black hole spell",
                    lore = "Man, fuck gravity.",
                    tooltip = "I wish ModAPI supported custom shaders. I could make the blackhole look better.",
                    Rarity = 6,
                    minLevel = 10,
                    maxLevel = 30,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Helmet,
                    icon = Res.ResourceLoader.GetTexture(91),

                }; new BaseItem(new int[][]
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
                    _itemType = BaseItem.ItemType.Ring,
                    icon = Res.ResourceLoader.GetTexture(90),
                };
                new BaseItem(new int[][]
                {
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
                    _itemType = BaseItem.ItemType.Ring,
                    icon = Res.ResourceLoader.GetTexture(90),
                };
                new BaseItem(new int[][]
                {
                new int[] {39,40,41,42,43,12,13},

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
                    _itemType = BaseItem.ItemType.Ring,
                    icon = Res.ResourceLoader.GetTexture(90),
                };
                new BaseItem(new int[][]
                          {
                new int[] {39,40,41,42,43},

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
                    _itemType = BaseItem.ItemType.Amulet,
                    icon = Res.ResourceLoader.GetTexture(100),
                };
                new BaseItem(new int[][]
                        {
                new int[] {39,40,41,42,43},

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
                    _itemType = BaseItem.ItemType.Bracer,
                    icon = Res.ResourceLoader.GetTexture(93),
                };
                new BaseItem(new int[][]
               {
                new int[] {39,40,41,42,43},
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
                    _itemType = BaseItem.ItemType.Bracer,
                    icon = Res.ResourceLoader.GetTexture(93),
                };
                new BaseItem(new int[][]
               {
                new int[] {39,40,41,42,43},
                new int[] {16},
                new int[] {5,6,10},

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
                    _itemType = BaseItem.ItemType.Bracer,
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
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Other,
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
                    _itemType = BaseItem.ItemType.Other,
                    icon = Res.ResourceLoader.GetTexture(105),
                };
                new BaseItem(new int[][]
            {
                new int[] {1,2},
                new int[] {1,2},
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
                    _itemType = BaseItem.ItemType.Ring,
                    icon = Res.ResourceLoader.GetTexture(90),
                };

                new BaseItem(new int[][]
                {
                new int[] {1,2,3,4,5,6},
                new int[] {1,2,3,4,5,6},
                new int[] {16 },
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
                    _itemType = BaseItem.ItemType.Amulet,
                    icon = Res.ResourceLoader.GetTexture(101),
                };
                FillKaspitoItems();
                BaseItem BrokenGreatAxe = new BaseItem(new int[][]
            {

                new int[] {25 },
                new int[] {18 },
                new int[] {2004 },

            })
                {
                    name = "Relic Hammer",
                    description = "It's slow and weak.",
                    lore = "",
                    tooltip = "Hammers stun enemies with every hit, but are slow. They also deal high smash damage",
                    Rarity = 2,
                    minLevel = 1,
                    maxLevel = 2,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Weapon,
                    weaponModel = BaseItem.WeaponModelType.Hammer,
                    icon = Res.ResourceLoader.GetTexture(109),
                };
                BrokenGreatAxe.PossibleStats[1][0].Multipier = -5;

                BaseItem GreaterHammer = new BaseItem(new int[][]
            {
                new int[] {25 },
                new int[] {18 },
                new int[] {2004 },
                new int[] {1,3},
                new int[] {53,16},
                new int[] {39,31,43,0,0},
                new int[] {25 ,22,1,12,13,5,6},


            })
                {
                    name = "Black Hammer",
                    description = "It's slow but with enough strenght i can make it a very deadly tool",
                    lore = "",
                    tooltip = "Hammers stun enemies with every hit, but are slow. They also deal high smash damage",
                    Rarity = 4,
                    minLevel = 1,
                    maxLevel = 2,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Weapon,
                    weaponModel = BaseItem.WeaponModelType.Hammer,
                    icon = Res.ResourceLoader.GetTexture(109),
                };
                GreaterHammer.PossibleStats[1][0].Multipier = -4;
                //Item 0/6
                new BaseItem(new int[][]
                {
                    new int[] {23,26},
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
                    _itemType = BaseItem.ItemType.Quiver,
                    icon = Res.ResourceLoader.GetTexture(98),
                };


                //Item 1/6
                new BaseItem(new int[][]
                {
                    new int[] {23,26},
                    new int[] {40,41,42},
                    new int[] {40,0},
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
                    _itemType = BaseItem.ItemType.Quiver,
                    icon = Res.ResourceLoader.GetTexture(98),
                };


                //Item 2/6
                new BaseItem(new int[][]
                {
                    new int[] {26},
                    new int[] {23,2,54},
                    new int[] {18,},
                    new int[] {40,41,16,5,6,40},
                    new int[] {40,0,0,0},
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
                    _itemType = BaseItem.ItemType.Quiver,
                    icon = Res.ResourceLoader.GetTexture(98),
                };


                //Item 3/6
                new BaseItem(new int[][]
                {
                    new int[] {26,23},
                    new int[] {24,21},
                    new int[] {17,16,18,54,51,52},
                    new int[] {2,3,4,15,14,13,12,11,10},
                    new int[] {5,6,47},
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
                    _itemType = BaseItem.ItemType.Quiver,
                    icon = Res.ResourceLoader.GetTexture(98),
                };


                //Item 4/6
                new BaseItem(new int[][]
                {
                    new int[] {23,26},
                    new int[] {23},
                    new int[] {2,3,4},
                    new int[] {34,18,17,16,15,14},
                    new int[] {16,19,23,31,54,51,52},
                    new int[] {2},
                    new int[] {2,3,4,5,6,7,8,9,10},
                    new int[] {2,1,5,6},
                    new int[] {11,2,0,0,0,0,0},
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
                    _itemType = BaseItem.ItemType.Quiver,
                    icon = Res.ResourceLoader.GetTexture(98),
                };


                //Item 5/6
                new BaseItem(new int[][]
                {
                    new int[] {37, 24},
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
                    _itemType = BaseItem.ItemType.SpellScroll,
                    icon = Res.ResourceLoader.GetTexture(110),
                };
                FillAppItems1();

            }
            catch (System.Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }
        }
        public static void AddPercentage(ref float variable1, float f)
        {
            variable1 = variable1 + (1 - variable1) * f;
        }
        public static void RemovePercentage(ref float variable1, float f)
        {
            variable1 = variable1 - (1 - variable1) / f;
        }
        //Items added by Kaspito#4871
        private static void FillKaspitoItems()
        {
            new BaseItem(new int[][]
                {
                new int[] {59 },
                new int[] {21 },
                new int[] {34,0,40,41 },
                new int[] {43,0 },
                new int[] {35,0 },
                })
            {
                name = "Moon Boots",
                description = "A pair of boots from the moon.",
                lore = "It is said that the wearer will not take fall damage while wearing these boots and will jump like on the moon, I wouldn't trust it tough.",
                tooltip = "Shoes can provide movement speed bonuses.",
                Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 10,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Boot,
                icon = Res.ResourceLoader.GetTexture(85), //icon ids, dont worry about that
            };
            new BaseItem(new int[][]
            {
                new int[] {1},
                new int[] {12,13,1},
                new int[] {22,25,1},
                new int[] {50,53,0,1},
                new int[] {0,0,39,1}
            })
            {
                name = "Golden Ring of Strength",
                description = "A Ring of ancient times.",
                lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Rings give base stats to make your stronger.",
                Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 10,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };
            new BaseItem(new int[][]
            {
                new int[] {3},
                new int[] {5,3,41},
                new int[] {7,10,31},
                new int[] {11,17},
                new int[] {14,16},
                new int[] {45,41,0,0}
            })
            {
                name = "Golden Ring of Vitality",
                description = "A Ring of ancient times.",
                lore = "A Golden Ring that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Rings give base stats to make your stronger.",
                Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 10,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
                new int[] {2},
                new int[] {8,9,},
                new int[] {12,13,51, },
                new int[] {12,13,51,0},
                new int[] {15,18,34,36,0},
                new int[] {23,48,54,26},
                new int[] {6,57,0,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };
            new BaseItem(new int[][]
            {
                new int[] {4},
                new int[] {6},
                new int[] {12,13,21,24},
                new int[] {12,13,21,24,0},
                new int[] {19,47,49,0},
                new int[] { 37,38,0},
                new int[] { 37,38,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };
            //Silver Rings---------------------------------------------------------------------------
            new BaseItem(new int[][]
              {
                new int[] {1},
                new int[] {12,13},
                new int[] {22,25,},
                new int[] {35,50,53,0},
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
              {
                new int[] {3},
                new int[] {5, },
                new int[] {7,10,31},
                new int[] {11,17,0},
                new int[] {14,16,0},
                new int[] {45,0,0,0}
              })
            {
                name = "Silver Ring of Vitality",
                description = "A Ring of ancient times.",
                lore = "A Silver Ring that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Rings give base stats to make your stronger.",
                Rarity = 4,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 5,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
              {
                new int[] {2},
                new int[] {8,9, },
                new int[] {12,13,51, },
                new int[] {12,13,51, 57,0},
                new int[] {15,18,34,36,0},
                new int[] {23,48,54,26},
                new int[] {6,57,0,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
              {
                new int[] {4},
                new int[] {6},
                new int[] {12,13,21,24},
                new int[] {12,13,21,24,0},
                new int[] {19,47,49,0},
                new int[] { 57,37,38,0},
                new int[] { 57,37,38,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            //Steel Rings-------------------------------------------------------------------
            new BaseItem(new int[][]
              {
                  new int[] {1},
                  new int[] {12,13},
                  new int[] {22,25, 57,},
                  new int[] {35,50,53,0},
                  new int[] {20,0,0,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
              {
                new int[] {3},
                new int[] {5, },
                new int[] {7,10,31},
                new int[] {11,17,0},
                new int[] {14,16,0},
                new int[] {45,0,0,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
              {
                new int[] {2},
                new int[] {8,9, },
                new int[] {12,13,51, },
                new int[] {12,13,51,57 ,0},
                new int[] {15,18,34,36,0},
                new int[] {23,48,54,26},
                new int[] {6,57,0,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
              {
                new int[] {4},
                new int[] {6},
                new int[] {12,13,21,24},
                new int[] {12,13,21,24,0,0,0,0,0},
                new int[] {19,47,49,0},
                new int[] { 57,37,38,0}
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            //One Ring to rule them all----------------------------------------------------------

            new BaseItem(new int[][]
              {
                new int[] {1},
                new int[] {12,13},
                new int[] {22,25,30,},
                new int[] {35,50,53,0},
                new int[] {20,0},
                new int[] {3},
                new int[] {5,28},
                new int[] {7,10,31},
                new int[] {11,17,0},
                new int[] {14,16,0},
                new int[] {45,0},
                new int[] {2},
                new int[] {8,9,27},
                new int[] {51,52},
                new int[] {15,18,34,36,57},
                new int[] {23,48,54,26},
                new int[] {4},
                new int[] {6},
                new int[] {21,24},
                new int[] {19,47,49,57},
                new int[] {29,37,38,57},
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
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90), //icon ids, dont worry about that
            };

            //Golden Lockets---------------------------------------------------------------------

            new BaseItem(new int[][]
            {
            new int[] {1},
            new int[] {12,13},
            new int[] {22,25,57,},
            new int[] {35,50,53,0},
            new int[] {20,0,0,0}
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
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {3},
            new int[] {5,},
            new int[] {7,10,31},
            new int[] {11,17,0},
            new int[] {14,16,0},
            new int[] {45,0,0,0}
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
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {2},
            new int[] {8,9,},
            new int[] {12,13,51,52},
            new int[] {12,13,51,52,0},
            new int[] {15,18,34,36,0},
            new int[] {23,48,54,26},
            new int[] {6,0,0,0}
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
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {4},
            new int[] {6},
            new int[] {12,13,21,24},
            new int[] {12,13,21,24,0},
            new int[] {19,47,49,0},
            new int[] {57,37,38,0},
            new int[] {4,37,38,0}
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
                _itemType = BaseItem.ItemType.Amulet,
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
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {3},
            new int[] {5,},
            new int[] {7,10,31},
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
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {2},
            new int[] {8,9,},
            new int[] {12,13,51,52},
            new int[] {12,13,51,52,0},
            new int[] {15,18,34,36,0},
            new int[] {23,48,54,26},
            new int[] {6,0,0,0}
            })
            {
                name = "Silver Locket of Agility",
                description = "A Locket of ancient times.",
                lore = "A Silver Locket that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Lockets give base stats to make your stronger.",
                Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 5,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {4},
            new int[] {6},
            new int[] {12,13,21,24},
            new int[] {12,13,21,24,0},
            new int[] {19,47,49,0},
            new int[] {57,37,38,0},
            new int[] {57,37,38,0}
            })
            {
                name = "Silver Locket of Intelligence",
                description = "A Locket of ancient times.",
                lore = "A Silver Locket that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Lockets give base stats to make your stronger.",
                Rarity = 2,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 5,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };


            // Emerald Pendant-----------------------------------------------------------


            new BaseItem(new int[][]
            {
            new int[] {1},
            new int[] {12,13},
            new int[] {22,25,57,},
            new int[] {35,50,53,0},
            new int[] {20,0,0,0}
            })
            {
                name = "Emerald Pendant of Strength",
                description = "A Pendant of ancient times.",
                lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 20,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {3},
            new int[] {5,},
            new int[] {7,10,31},
            new int[] {11,17,0},
            new int[] {14,16,0},
            new int[] {45,0,0,0}
            })
            {
                name = "Emerald Pendant of Vitality",
                description = "A Pendant of ancient times.",
                lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 30,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {2},
            new int[] {8,9,},
            new int[] {12,13,51,52},
            new int[] {12,13,51,52,0},
            new int[] {15,18,34,36,0},
            new int[] {23,48,54,26},
            new int[] {6,0,0,0}
            })
            {
                name = "Emerald Pendant of Agility",
                description = "A Pendant of ancient times.",
                lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 30,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {4},
            new int[] {6},
            new int[] {12,13,21,24},
            new int[] {12,13,21,24,0},
            new int[] {19,47,49,0},
            new int[] {4,37,38,0},
            new int[] {4,37,38,0}
            })
            {
                name = "Emerald Pendant of Intelligence",
                description = "A Pendant of ancient times.",
                lore = "An Emerald Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 5,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 30,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };


            // Diamond Pendant-----------------------------------------------------------


            new BaseItem(new int[][]
            {
            new int[] {1},
            new int[] {12,13},
            new int[] {22,25,1,},
            new int[] {35,50,53,0},
            new int[] {20,0,0,0}
            })
            {
                name = "Diamond Pendant of Strength",
                description = "A Pendant of ancient times.",
                lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 45,
                maxLevel = 60,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {3},
            new int[] {5,},
            new int[] {7,10,31},
            new int[] {11,17,0},
            new int[] {14,16,0},
            new int[] {45,0,0,0}
            })
            {
                name = "Diamond Pendant of Vitality",
                description = "A Pendant of ancient times.",
                lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 45,
                maxLevel = 60,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {2},
            new int[] {8,9,},
            new int[] {12,13,51,52},
            new int[] {12,13,51,52,0},
            new int[] {15,18,34,36,0},
            new int[] {23,48,54,26},
            new int[] {6,0,0,0}
            })
            {
                name = "Diamond Pendant of Agility",
                description = "A Pendant of ancient times.",
                lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 45,
                maxLevel = 60,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };

            new BaseItem(new int[][]
            {
            new int[] {4},
            new int[] {6},
            new int[] {12,13,21,24},
            new int[] {12,13,21,24,0},
            new int[] {19,47,49,0},
            new int[] {57,37,38,0},
            new int[] {57,37,38,0}
            })
            {
                name = "Diamond Pendant of Intelligence",
                description = "A Pendant of ancient times.",
                lore = "A Diamond Pendant that looks simple and elegant, yet it feels powerfull to the touch.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 6,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 45,
                maxLevel = 60,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            };


            //Rare Amulets -----------------------------------------------------------------------------------------


            new BaseItem(new int[][]
            {
            new int[] {1},
            new int[] {1},
            new int[] {1},
            new int[] {12,13},
            new int[] {22,25,30,},
            new int[] {35,50,53,57},
            new int[] {32,33},
            new int[] {20,57}
            })
            {
                name = "Armsy Finger Necklace",
                description = "A Necklace decorated with armsy fingertips.",
                lore = "A Necklace made from the fingertips of an armsy, yeilding it's raw power and strentgh.",
                tooltip = "Necklaces give base stats to make your stronger.",
                Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 40,
                maxLevel = 60,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            }.DropSettings_OnlyArmsy();

            new BaseItem(new int[][]
            {
            new int[] {2},
            new int[] {2},
            new int[] {2},
            new int[] {5,28},
            new int[] {7,10,31},
            new int[] {11,17,57},
            new int[] {14,16,57},
            new int[] {32,33},
            new int[] {45,57}
            })
            {
                name = "Virginia Heart Pendant",
                description = "A Pendant of a petrified Virginia heart.",
                lore = "A Pendant made from a petrified Virginia heart, yeilding it's love and Vitality.",
                tooltip = "Pendants give base stats to make your stronger.",
                Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 20,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            }.DropSettings_OnlyVags();

            new BaseItem(new int[][]
            {
            new int[] {3},
            new int[] {3},
            new int[] {3},
            new int[] {8,9,27},
            new int[] {12,13,51,52},
            new int[] {12,13,51,52,57},
            new int[] {15,18,34,36,57},
            new int[] {23,48,54,26},
            new int[] {32,33},
            new int[] {6,57}
            })
            {
                name = "Cowman Toe Necklace",
                description = "A Necklace decorated with cowman toes.",
                lore = "A Necklace made from the fingertips of an armsy, yeilding it's speed and agility.",
                tooltip = "Necklaces give base stats to make your stronger.",
                Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 20,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            }.DropSettings_OnlyCow();

            new BaseItem(new int[][]
            {
            new int[] {4},
            new int[] {4},
            new int[] {4},
            new int[] {6},
            new int[] {12,13,21,24},
            new int[] {12,13,21,24,0},
            new int[] {19,47,49,0},
            new int[] {29,37,38},
            new int[] {29,37,38,0},
            new int[] {32,33}
            })
            {
                name = "Babyhead Pendant",
                description = "A Pendant of a shrunken babyhead.",
                lore = "A Pendant made from a shrunken babyhead, yeilding it's intellectual potential and imagination.",
                tooltip = "Necklaces give base stats to make your stronger.",
                Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 30,
                maxLevel = 40,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            }.DropSettings_OnlyBaby();


            //Boss drop Amulet----------------------------------------------------------------------------------------


            new BaseItem(new int[][]
            {
            new int[] {1},
            new int[] {1},
            new int[] {12,13},
            new int[] {22,25,30,18},
            new int[] {35,50,53,57,56},
            new int[] {20,57,19,18},
            new int[] {3},
            new int[] {3},
            new int[] {5,28},
            new int[] {7,10,31},
            new int[] {11,17,57},
            new int[] {14,16,57},
            new int[] {45,16,10,11,9,8},
            new int[] {2},
            new int[] {2},
            new int[] {8,9,27},
            new int[] {51,52},
            new int[] {15,18,34,36,57},
            new int[] {23,48,54,26},
            new int[] {4},
            new int[] {4},
            new int[] {6,55,46,54,53},
            new int[] {21,24},
            new int[] {19,47,49,57},
            new int[] {29,37,38,57},
            })
            {
                name = "Megan's Locket",
                description = "The Locket Megan wore.",
                lore = "Megan wore this Locket, it has a picture of her mom in it.",
                tooltip = "lockets give base stats to make your stronger.",
                Rarity = 7,         //range 0-7, 0 is most common, 7 is ultra rare
                minLevel = 75,
                maxLevel = 80,
                CanConsume = false,
                StackSize = 1,     //stacking in inventory like in mc, one means single item
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101), //icon ids, dont worry about that
            }.DropSettings_OnlyMegan();





        }

        //item constructors created using my app
        private static void FillAppItems1()
        {


            //    - ITEMS 
            //OFFHANDS - Quivers and spell scrolls
            //Item 0/6
            new BaseItem(new int[][]
            {
                new int[] {23,26},
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
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(98),
            };


            //Item 1/6
            new BaseItem(new int[][]
            {
                new int[] {23,26},
                new int[] {40,41,42},
                new int[] {40,0},
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
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(98),
            };


            //Item 2/6
            new BaseItem(new int[][]
            {
                new int[] {26},
                new int[] {23,2,54},
                new int[] {18,},
                new int[] {40,41,16,5,6,40},
                new int[] {40,0,0,0},
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
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(98),
            };


            //Item 3/6
            new BaseItem(new int[][]
            {
                new int[] {26,23},
                new int[] {24,21},
                new int[] {17,16,18,54,51,52},
                new int[] {2,3,4,15,14,13,12,11,10},
                new int[] {5,6,47},
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
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(98),
            };


            //Item 4/6
            new BaseItem(new int[][]
            {
                new int[] {23,26},
                new int[] {23},
                new int[] {2,3,4},
                new int[] {34,18,17,16,15,14},
                new int[] {16,19,23,31,54,51,52},
                new int[] {2},
                new int[] {2,3,4,5,6,7,8,9,10},
                new int[] {2,1,5,6},
                new int[] {11,2,0,0,0,0,0},
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
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(98),
            };


            //Item 5/6
            new BaseItem(new int[][]
            {
                new int[] {37, 24},
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
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(110),
            };
            //Created using Hazard's app


            //    - ITEMS 

            //Item 0/7
            new BaseItem(new int[][]
            {
                new int[] {16,43},
                new int[] {43},
                new int[] {43},
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
                _itemType = BaseItem.ItemType.Pants,
                icon = Res.ResourceLoader.GetTexture(87),
            };


            //Item 1/7
            new BaseItem(new int[][]
            {
                new int[] {16},
                new int[] {1,2,3,4},
                new int[] {5,6},
                new int[] {16,43,39,40,41,42},
                new int[] {1000,1001,1002,43,0,0,0},
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
                _itemType = BaseItem.ItemType.Pants,
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
                _itemType = BaseItem.ItemType.Pants,
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
                _itemType = BaseItem.ItemType.Pants,
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
            })
            {
                name = "Hammer Jammers",
                description = "Increases the damage of your hammer smash by 50%, hammer stun duration is increased by 0,3 seconds",
                lore = "",
                tooltip = "",
                Rarity = 7,
                minLevel = 20,
                maxLevel = 28,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Pants,
                icon = Res.ResourceLoader.GetTexture(87),
                onEquip = () => { ModdedPlayer.instance.HammerSmashDamageAmp += 0.5f; ModdedPlayer.instance.HammerStunAmount += 0.3f; },
                onUnequip = () => { ModdedPlayer.instance.HammerSmashDamageAmp -= 0.5f; ModdedPlayer.instance.HammerStunAmount -= 0.3f; },
            };


            //Item 5/7
            new BaseItem(new int[][]
            {
                new int[] {16},
                new int[] {34},
                new int[] {1,2,4,6,7,8},
                new int[] {8,9},
                new int[] {26,23,24,21},
                new int[] {1000, 1001,1002,0,0,0,1,2,4},
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
                _itemType = BaseItem.ItemType.Pants,
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
                new int[] {16,12,13},
                new int[] {16},
            })
            {
                name = "Hexed Pants of Mr M.",
                description = "They look like yoga pants but for a man the size of a wardrobe",
                lore = "Once upon a time there was a man who was in a basement and fed himself with nothing but nuggets. He got so obese that friends and family started worrying. Hazard noticed this man and cursed his pants to force him to excercise.",
                tooltip = "While moving with those pants on, energy regeneration and damage is increased by 20%. While standing still for longer than a second, you loose 1.5% of max health per second.",
                Rarity = 7,
                minLevel = 14,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Pants,
                icon = Res.ResourceLoader.GetTexture(87),
                onEquip = () => ModdedPlayer.instance.HexedPantsOfMrM_Enabled = true,
                onUnequip = () => ModdedPlayer.instance.HexedPantsOfMrM_Enabled = false,
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
                _itemType = BaseItem.ItemType.ShoulderArmor,
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
                _itemType = BaseItem.ItemType.ShoulderArmor,
                icon = Res.ResourceLoader.GetTexture(95),
            };


            //Item 2/6
            BaseItem Heavy_Shoulder_Plates = new BaseItem(new int[][]
            {
                new int[] {34},
                new int[] {18},
                new int[] {16},
                new int[] {16},
                new int[] {16,0,0,0},
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
                _itemType = BaseItem.ItemType.ShoulderArmor,
                icon = Res.ResourceLoader.GetTexture(95),
            };
            Heavy_Shoulder_Plates.PossibleStats[0][0].Multipier = -1;
            Heavy_Shoulder_Plates.PossibleStats[1][0].Multipier = -1;

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
                minLevel = 7,
                maxLevel = 10,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.ShoulderArmor,
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
                minLevel = 19,
                maxLevel = 24,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.ShoulderArmor,
                icon = Res.ResourceLoader.GetTexture(95),
            };


            //Item 5/6
            new BaseItem(new int[][]
            {
                new int[] {11},
                new int[] {1,2,3,4},
                new int[] {16},
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
                minLevel = 20,
                maxLevel = 30,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.ShoulderArmor,
                icon = Res.ResourceLoader.GetTexture(95),
                onEquip = () => ModdedPlayer.instance.DeathPact_Enabled = true,
                onUnequip = () => ModdedPlayer.instance.DeathPact_Enabled = false,
            };
            new BaseItem(new int[][]
            {
                new int[] {56},
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
                minLevel = 10,
                maxLevel = 15,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Ring,
                icon = Res.ResourceLoader.GetTexture(90),
            };
            new BaseItem(new int[][]
                         {
                new int[] {58},

                         })
            {
                name = "Shard of Farket's Heart",
                description = "A object filled with both destructive and creative energy. Poison for some. It will kill a man who eats it.",
                lore = "The Legendary Farket, self proclaimed king of peninsula, over time shifted into insanity. The cursed land of his sent madness upon him. That was the price he had to pay for the unfair power he obtained. This merely a shard of his strongest muscule contains so much power, that it kill anything and force it to come back to life, resulting in it's rebirth. The Prophecy fortold the return of The King. He will reappear as the harbinger of end of the world.",
                tooltip = "Can be consumed by right clicking it. Allows to re-assign all spent mutation points",
                Rarity = 7,
                minLevel = 10,
                maxLevel = 30,
                CanConsume = true,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Other,
                icon = Res.ResourceLoader.GetTexture(105),
            };

            //Created using Hazard's app for 1.0 release

            new BaseItem(new int[][]
            {
new int[] {1,2,3,4,57},
new int[] {16,17,14},
new int[] {50},
new int[] {49,39,40,41,42,45,44},
new int[] {5,6,0,0,},
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
                _itemType = BaseItem.ItemType.Shield,
                icon = Res.ResourceLoader.GetTexture(99),
            };


            //Item 1/5
            new BaseItem(new int[][]
            {
new int[] {2,0},
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
                _itemType = BaseItem.ItemType.Shield,
                icon = Res.ResourceLoader.GetTexture(99),
            };


            //Item 2/5
            new BaseItem(new int[][]
            {
new int[] {16},
new int[] {16,50},
new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
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
                _itemType = BaseItem.ItemType.Shield,
                icon = Res.ResourceLoader.GetTexture(99),
            };


            //Item 3/5
            new BaseItem(new int[][]
            {
new int[] {15,14},
new int[] {2,3,4,1,41,42,57},
new int[] {2,3,4,1,41,42,57},
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
                _itemType = BaseItem.ItemType.Shield,
                icon = Res.ResourceLoader.GetTexture(99),
            };

            new BaseItem(new int[][]
            {
new int[] {18},
new int[] {18},
            })
            {
                name = "Dull Longsword",
                description = "It's round on the edges",
                lore = "",
                tooltip = "",
                Rarity = 0,
                minLevel = 1,
                maxLevel = 3,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Weapon,
                weaponModel = BaseItem.WeaponModelType.LongSword,
                icon = Res.ResourceLoader.GetTexture(89),
            }.PossibleStats[0][0].Multipier = -2;

            new BaseItem(new int[][]
            {
                new int[] {16},
                new int[] {16},
                new int[] {16},
            })
            {
                name = "Iron Horn",
                description = "",
                lore = "",
                tooltip = "Wielder of this item has empowered warcry ability - all allies recieve armor bonus equal to 10% of your armor",
                Rarity = 4,
                minLevel = 10,
                maxLevel = 25,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101),
                onEquip = () => SpellActions.WarCryGiveArmor = true,
                onUnequip = () => SpellActions.WarCryGiveArmor = false,
            };


            //Item 1/5
            new BaseItem(new int[][]
            {
                new int[] {16},
                new int[] {16},
                new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
                new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
                new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
                new int[] {31,7,8,9,10},
                new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,49,50,53,54,55,57},
            })
            {
                name = "The Great Horn",
                description = "",
                lore = "",
                tooltip = "Wielder of this item has empowered warcry ability - all allies recieve armor bonus equal to 10% of your armor",
                Rarity = 7,
                minLevel = 34,
                maxLevel = 50,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Amulet,
                icon = Res.ResourceLoader.GetTexture(101),
                onEquip = () => SpellActions.WarCryGiveArmor = true,
                onUnequip = () => SpellActions.WarCryGiveArmor = false,
            };


            //Item 2/5
            new BaseItem(new int[][]
            {
                new int[] {16},
                new int[] {1},
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
                minLevel = 6,
                maxLevel = 15,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Helmet,
                icon = Res.ResourceLoader.GetTexture(91),
            };


            //Item 3/5
            new BaseItem(new int[][]
            {
                new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
                new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,31,36,37,38,43,44,45,46,47,49,50,53,54,55,57},
            })
            {
                name = "Mask",
                description = "",
                lore = "",
                tooltip = "",
                Rarity = 2,
                minLevel = 5,
                maxLevel = 16,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Helmet,
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
                minLevel = 33,
                maxLevel = 66,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Helmet,
                icon = Res.ResourceLoader.GetTexture(91),
            };
            mask.PossibleStats[2][0].Multipier = -3;
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
                new int[] {24},
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
                _itemType = BaseItem.ItemType.SpellScroll,
                icon = Res.ResourceLoader.GetTexture(110),
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
            })
            {
                name = "Wormhole Stabilizators",
                description = "High-tech gear",
                lore = "Hazard remember to put some fucking lore in here, dont leave it like this!",
                tooltip = "Increases the duration of a portal to 5 minutes",
                Rarity = 7,
                minLevel = 36,
                maxLevel = 50,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Bracer,
                icon = Res.ResourceLoader.GetTexture(94),
                onEquip = () => SpellActions.PortalDuration = 300,
                onUnequip = () => SpellActions.PortalDuration = 30,
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
            })
            {
                name = "Cripplers",
                description = "",
                lore = "",
                tooltip = "Increases the duration of a magic arrow's negative effect by 3 seconds",
                Rarity = 7,
                minLevel = 36,
                maxLevel = 50,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Glove,
                icon = Res.ResourceLoader.GetTexture(86),
                onEquip = () => SpellActions.MagicArrowDuration += 3,
                onUnequip = () => SpellActions.MagicArrowDuration -= 3,
            };

            new BaseItem(new int[][]
            {
                new int[] {24},
                new int[] {26},
                new int[] {21},
                new int[] {23},
                new int[] {2,4,57,16},
                new int[] {6,8,9,44,46},
                new int[] {2},
                new int[] {4},
                new int[] {12,13,14,15,16,18},
            })
            {
                name = "Crossfire",
                description = "Infused with powerful magic. This item is a dangerous tool of destruction.",
                lore = "",
                tooltip = "When hitting an enemy with a projectile, create a magic arrow pointed at the enemy and shoot it without using in energy. This effect may occur once every 20 seconds. The magic arrow deals half the damage and applies stats that a normal magic arrow would.",
                Rarity = 7,
                minLevel = 1,
                maxLevel = 20,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Quiver,
                icon = Res.ResourceLoader.GetTexture(98),
                onEquip = () => ModdedPlayer.instance.IsCrossfire = true,
                onUnequip = () => ModdedPlayer.instance.IsCrossfire = false,
            };

            new BaseItem(new int[][]
            {
                new int[] {44},
                new int[] {44,8},
                new int[] {44,0,0,0},
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
                _itemType = BaseItem.ItemType.SpellScroll,
                icon = Res.ResourceLoader.GetTexture(110),
            };

            new BaseItem(new int[][]
            {
                new int[] {11},
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
                _itemType = BaseItem.ItemType.Helmet,
                icon = Res.ResourceLoader.GetTexture(91),
            };


            //Item 1/2
            new BaseItem(new int[][]
            {
                new int[] {3},
                new int[] {15},
                new int[] {15},
                new int[] {17,16},
                new int[] {17,16},
            })
            {
                name = "Chastity belt",
                description = "Dodge those fukbois",
                lore = "This belt will stop those cheeky cannibals and armsies from getting into your pants",
                tooltip = "Always wear this when sleeping",
                Rarity = 2,
                minLevel = 1,
                maxLevel = 1,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.Pants,
                icon = Res.ResourceLoader.GetTexture(87),
            };
            new BaseItem(new int[][]
           {
                    new int[] {4,2005},
                    new int[] {37,38},
                    new int[] {42,43},
                    new int[] {44,0,49,},
                    new int[] {21,24,0,0,0,0},
           })
            {
                name = "Ice Scroll",
                description ="A spell surrounded by flying shards of ice, contains tramendous power of cold.",
                lore = "Created at the top of the mountain.",
                tooltip = "Spell Scrolls grant magic buffs. Ice Scroll empowers spell - Snap Freeze.",
                Rarity = 4,
                minLevel = 30,
                maxLevel =40,
                CanConsume = false,
                StackSize = 1,
                _itemType = BaseItem.ItemType.SpellScroll,
                icon = Res.ResourceLoader.GetTexture(110),
            };
        }
    }
}
