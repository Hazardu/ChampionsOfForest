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
                    ModAPI.Log.Write(_Item_Bases[i].name + _Item_Bases[i].ID + " - item added");

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
            ModAPI.Log.Write("SETUP: ITEM DATABASE");

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
                int sum = 0;
                foreach (int a in ModReferences.PlayerLevels.Values)
                {
                    sum += a;
                }
                sum /= ModReferences.PlayerLevels.Values.Count;
                averageLevel = sum;
            }
            else
            {
                averageLevel = ModdedPlayer.instance.Level;
            }
            float w = Worth / averageLevel;
            int rarity = 0;

            if (w > 200 && Random.value < 0.60f)
            {
                rarity = 1;

                if (w > 350 && Random.value < 0.50f)
                {
                    rarity = 2;

                    if (w > 500 && Random.value < 0.50f&&(int)ModSettings.difficulty>=1)
                    {
                        rarity = 3;

                        if (w > 625 && Random.value < 0.5f && (int)ModSettings.difficulty >= 2)
                        {
                            rarity = 4;

                            if (w > 700 && Random.value < 0.50f && (int)ModSettings.difficulty >= 3)
                            {
                                rarity = 5;

                                if (w > 800 && Random.value < 0.4f && (int)ModSettings.difficulty >= 4)
                                {
                                    rarity = 6;

                                    if (w > 950 && Random.value < 0.20f && (int)ModSettings.difficulty >= 5)
                                    {
                                        rarity = 7;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            int increasedLvl = 0;
            while (!ItemRarityGroups.ContainsKey(rarity) && rarity > 0)
            {
                increasedLvl += 2;
                rarity--;
            }

            int[] itemPool = null;
            while (itemPool == null)
            {
                itemPool= ItemRarityGroups[rarity].Where(i => ((ItemBases[i].minLevel) <= averageLevel+5)).ToArray();
                if (itemPool.Length == 0)
                {
                    rarity--;
                    itemPool = null;
                }
                
            }

            int randomID = Random.Range(0, itemPool.Length);

            Item item = new Item(ItemBases[itemPool[randomID]], 1, increasedLvl);


            item.level = Mathf.Max(item.level, Random.Range(averageLevel - 3, averageLevel + 5));
            item.RollStats();
            return item;

        }

        public static void FillStats()
        {
            int i = 1;
            new ItemStat(i, 1, 2, 1.7f, "Strenght", 2, StatActions.AddStrenght, StatActions.RemoveStrenght, StatActions.AddStrenght); i++;
            new ItemStat(i, 1, 2, 1.7f, "Agility", 2, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility); i++;
            new ItemStat(i, 1,2, 1.7f, "Vitality", 2, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality); i++;
            new ItemStat(i, 1,2, 1.7f, "Intelligence", 2, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence); i++;
            new ItemStat(i, 2, 2.5f, 1.6f, "Maximum Life", 3, StatActions.AddHealth, StatActions.RemoveHealth, StatActions.AddHealth); i++;
            new ItemStat(i, 2, 2.5f, 1.6f, "Maximum Energy", 3, StatActions.AddEnergy, StatActions.RemoveEnergy, StatActions.AddEnergy); i++;
            new ItemStat(i, 0.01f, 0.025f, 1.7f, "Life Per Second", 3, StatActions.AddHPRegen, StatActions.RemoveHPRegen, StatActions.AddHPRegen) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.01f, 0.025f, 1.7f, "Stamina Per Second", 3, StatActions.AddStaminaRegen, StatActions.RemoveStaminaRegen, StatActions.AddStaminaRegen) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0001f, 0.0025f, 1.2f, "Stamina Regen %", 3, StatActions.AddStaminaRegenPercent, StatActions.RemoveStaminaRegenPercent, StatActions.AddStaminaRegenPercent) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.0025f, 1.2f, "Life Regen %", 3, StatActions.AddHealthRegenPercent, StatActions.RemoveHealthRegenPercent, StatActions.AddHealthRegenPercent) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.0004f, 0.002f, 1.12f, "Damage Reduction %", 2, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, StatActions.AddDamageReduction) { ValueCap = 0.99f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.0002f, 0.001f, 1.15f, "Critical Hit Chance", 6, StatActions.AddCritChance, StatActions.RemoveCritChance, StatActions.AddCritChance) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.01f, 0.03f, 1.35f, "Critical Hit Damage", 6, StatActions.AddCritDamage, StatActions.RemoveCritDamage, StatActions.AddCritDamage) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.1f, 0.35f, 1.6f, "Life on hit", 6, StatActions.AddLifeOnHit, StatActions.RemoveLifeOnHit, StatActions.AddLifeOnHit); i++;
            new ItemStat(i, 0.001f, 0.002f, 1.1f, "Dodge chance", 5, StatActions.AddDodgeChance, StatActions.RemoveDodgeChance, StatActions.AddDodgeChance) { ValueCap = 0.99f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 2f, 3f, 1.8f, "Armor", 2, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.001f, 0.003f, 1.05f, "Resistance to magic", 5, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, StatActions.AddMagicResistance) { ValueCap = 0.95f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.005f, 0.01f, 1.3f, "Attack speed", 6, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, StatActions.AddAttackSpeed) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.003f, 0.007f, 1.3f, "Exp %", 6, StatActions.AddExpFactor, StatActions.RemoveExpFactor, StatActions.AddExpFactor) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 2f, 1.1f, "Massacre Duration", 6, StatActions.AddMaxMassacreTime, StatActions.RemoveMaxMassacreTime, StatActions.AddMaxMassacreTime); i++;
            new ItemStat(i, 0.002f, 0.006f, 1.5f, "Spell Damage %", 4, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, StatActions.AddSpellDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.002f, 0.006f, 1.5f, "Melee Damage %", 4, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, StatActions.AddMeleeDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.002f, 0.006f, 1.5f, "Ranged Damage %", 4, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, StatActions.AddRangedDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.7f, 0.8f, 1.7f, "Bonus Spell Damage", 4, StatActions.AddSpellDamageBonus, StatActions.RemoveSpellDamageBonus, StatActions.AddSpellDamageBonus); i++;
            new ItemStat(i, 0.35f, 0.5f, 1.7f, "Bonus Melee Damage", 4, StatActions.AddMeleeDamageBonus, StatActions.RemoveMeleeDamageBonus, StatActions.AddMeleeDamageBonus); i++;
            new ItemStat(i, 0.35f, 0.5f, 1.7f, "Bonus Ranged Damage", 4, StatActions.AddRangedDamageBonus, StatActions.RemoveRangedDamageBonus, StatActions.AddRangedDamageBonus); i++;
            new ItemStat(i, 0.0005f, 0.001f, 1.02f, "Energy Per Agility", 7, StatActions.AddEnergyPerAgility, StatActions.RemoveEnergyPerAgility, StatActions.AddEnergyPerAgility) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.005f, 0.008f, 1.02f, "Health Per Vitality", 7, StatActions.AddHealthPerVitality, StatActions.RemoveHealthPerVitality, StatActions.AddHealthPerVitality) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0004f, 0.0009f, 1.02f, "Spell Damage Per Int", 7, StatActions.AddSpellDamageperInt, StatActions.RemoveSpellDamageperInt, StatActions.AddSpellDamageperInt) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0004f, 0.0009f, 1.02f, "Damage Per Strenght", 7, StatActions.AddDamagePerStrenght, StatActions.RemoveDamagePerStrenght, StatActions.AddDamagePerStrenght) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.00025f, 0.001f, 1.5f, "All Healing %", 7, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, StatActions.AddHealingMultipier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.7f/5.5f, 1.6f/5.5f, 0f, "PERMANENT PERK POINTS", 0, null, null, StatActions.PERMANENT_perkPointIncrease); i++;
            new ItemStat(i, 10f, 30f, 2.2f, "EXPERIENCE", 0, null, null, StatActions.PERMANENT_expIncrease); i++;
            new ItemStat(i, 0.00075f, 0.002f, 1.25f, "Movement Speed", 5, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, StatActions.AddMoveSpeed) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.1f, "Melee Weapon Range", 5, f => ModdedPlayer.instance.MeleeRange += f, f => ModdedPlayer.instance.MeleeRange -= f, f => ModdedPlayer.instance.MeleeRange += f) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.1f, "Attack Cost Reduction", 3, f => AddPercentage(ref ModdedPlayer.instance.StaminaAttackCostReduction, f), f => RemovePercentage(ref ModdedPlayer.instance.StaminaAttackCostReduction, f), f => AddPercentage(ref ModdedPlayer.instance.StaminaAttackCostReduction, f)) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.1f, "Spell Cost Reduction", 6, f => AddPercentage(ref ModdedPlayer.instance.SpellCostRatio, f), f => RemovePercentage(ref ModdedPlayer.instance.SpellCostRatio, f), f => ModdedPlayer.instance.SpellCostRatio *= f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.005f, 0.015f, 1.02f, "Spell Cost to Stamina", 6, f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => RemovePercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f)) { ValueCap = 0.95f, DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.6f, 1.2f, 1.6f, "Strenght", 1, StatActions.AddStrenght, StatActions.RemoveStrenght, StatActions.AddStrenght); i++;
            new ItemStat(i, 0.6f, 1.2f, 1.6f, "Agility", 1, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility); i++;
            new ItemStat(i, 0.6f, 1.2f, 1.6f, "Vitality", 1, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality); i++;
            new ItemStat(i, 0.6f, 1.2f, 1.6f, "Intelligence", 1, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence); i++;
            new ItemStat(i, 0.8f, 1.3f, 1.7f, "Armor", 1, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.7f, "Energy Per Second", 6, StatActions.AddEnergyRegen, StatActions.RemoveEnergyRegen, StatActions.AddEnergyRegen) { RoundingCount = 2 }; i++;
            new ItemStat(i, 0.001f, 0.005f, 1.4f, "Maximum Life %", 5, f => ModdedPlayer.instance.MaxHealthPercent += f, f => ModdedPlayer.instance.MaxHealthPercent -= f, f => ModdedPlayer.instance.MaxHealthPercent += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.005f, 1.4f, "Maximum Energy %", 5, f => ModdedPlayer.instance.MaxEnergyPercent += f, f => ModdedPlayer.instance.MaxEnergyPercent -= f, f => ModdedPlayer.instance.MaxEnergyPercent += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.003f, 0.02f, 1.035f, "Cooldown Reduction", 7, f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier /= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f)) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.9f }; i++;
            new ItemStat(i, 0.0004f, 0.0009f, 1.02f, "Ranged Damage Per Agi", 7, f => ModdedPlayer.instance.RangedDamageperAgi += f, f => ModdedPlayer.instance.RangedDamageperAgi += -f, f => ModdedPlayer.instance.RangedDamageperAgi += f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.01f, 0.05f, 1.35f, "Energy on hit", 4, f => ModdedPlayer.instance.EnergyOnHit += f, f => ModdedPlayer.instance.EnergyOnHit += -f, f => ModdedPlayer.instance.EnergyOnHit += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.01f, 1.25f, "Block", 2, f => ModdedPlayer.instance.BlockFactor += f, f => ModdedPlayer.instance.BlockFactor += -f, f => ModdedPlayer.instance.BlockFactor += f) { DisplayAsPercent = true, RoundingCount = 1 }; i++;

            i = 1000;
            new ItemStat(i, 2f, 25f, 0f, "Extra Sticks", 3, f => ModdedPlayer.instance.AddExtraItemCapacity(57, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(57, -Mathf.RoundToInt(f)), null); i++;
            new ItemStat(i, 1f, 11f, 0f, "Extra Rocks", 3, f => ModdedPlayer.instance.AddExtraItemCapacity(53, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(53, -Mathf.RoundToInt(f)), null); i++;

            i = 2000;
            new ItemStat(i, 1f, 4f, 0f, "BLACK HOLE RADIUS", 6, f => SpellActions.BLACKHOLE_radius += f, f => SpellActions.BLACKHOLE_radius += -f, f => SpellActions.BLACKHOLE_radius += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 0.2f, 1f, 0f, "BLACK HOLE DURATION", 6, f => SpellActions.BLACKHOLE_duration += f, f => SpellActions.BLACKHOLE_duration += -f, f => SpellActions.BLACKHOLE_duration += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 2f, 3f, 0f, "BLACK HOLE FORCE", 6, f => SpellActions.BLACKHOLE_pullforce += f, f => SpellActions.BLACKHOLE_pullforce += -f, f => SpellActions.BLACKHOLE_pullforce += f) { RoundingCount = 1 }; i++;
            new ItemStat(i, 0.5f, 1f, 1.7f, "BLACK HOLE DAMAGE", 6, f => SpellActions.BLACKHOLE_damage += f, f => SpellActions.BLACKHOLE_damage += -f, f => SpellActions.BLACKHOLE_damage += f) { RoundingCount = 1 }; i++;

        }
        public static void AddPercentage(ref float variable1, float f)
        {
            variable1 = 1 - (1 - variable1) * f;
        }
        public static void RemovePercentage(ref float variable1, float f)
        {
            variable1 = 1 - (1 - variable1) / f;
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
               new int[] {11,1 },
               new int[] {1, },
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
                };
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
                    icon = Res.ResourceLoader.GetTexture(97),
                };
                new BaseItem(new int[][]
               {
                new int[] {1000,1001},
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
                    icon = Res.ResourceLoader.GetTexture(97),
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

                      })
                {
                    name = "Smokey's Sacred Quiver",
                    description = "SmokeyTheBear please send me a viable decription.",
                    lore = "Smokey was the friend of allmighty Hazard, who can materialize any kind of weapon at the snap of his fingers. Hazard remebered Smokey's favourite playstyle and he gave him this as a gift to purge the sh** out of mutants.",
                    tooltip = "This quiver makes you shoot special projectiles from your bows",
                    Rarity = 7,
                    minLevel = 5,
                    maxLevel = 50,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.Offhand,
                    icon = Res.ResourceLoader.GetTexture(98),
                    onEquip = () => ModdedPlayer.IsSacredArrow = true,
                    onUnequip = () => ModdedPlayer.IsSacredArrow = false,
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
                    description = "The distance of teleport is increased by 40 meters, and teleport now damages everything that you teleported through",
                    lore = "",
                    tooltip = "Provides usefull stat bonuses",
                    Rarity = 7,
                    minLevel = 5,
                    maxLevel = 50,
                    CanConsume = false,
                    StackSize = 1,
                    _itemType = BaseItem.ItemType.ShoulderArmor,
                    icon = Res.ResourceLoader.GetTexture(95),
                    onEquip = () => Player.SpellActions.BlinkRange += 40,
                    onUnequip = () => Player.SpellActions.BlinkRange -= 40,
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
                    Rarity = 7,
                    minLevel = 1,
                    maxLevel = 3,
                    CanConsume = true,
                    StackSize = 100,
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
                    Rarity = 6,
                    minLevel = 1,
                    maxLevel = 6,
                    CanConsume = true,
                    StackSize = 100,
                    _itemType = BaseItem.ItemType.Other,
                    icon = Res.ResourceLoader.GetTexture(105),
                };
            }
            catch (System.Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }
        }
    }
}
