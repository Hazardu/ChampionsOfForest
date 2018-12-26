using ChampionsOfForest.Items;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest
{
    public class ItemDataBase
    {
        public static ItemDataBase Instance
        {
            get;
            private set;
        }

        public List<BaseItem> _Item_Bases;
        public Dictionary<int, BaseItem> ItemBases;
        public List<ItemStat> statList;
        public Dictionary<int, ItemStat> Stats;

        private Dictionary<int, List<int>> ItemRarityGroups;
        //Called from Initializer
        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new ItemDataBase();
            }
            else
            {
                return;
            }
            Instance._Item_Bases = new List<BaseItem>();
            Instance.ItemBases = new Dictionary<int, BaseItem>();
            Instance.statList = new List<ItemStat>();
            Instance.Stats = new Dictionary<int, ItemStat>();
            Instance.ItemRarityGroups = new Dictionary<int, List<int>>();
            Instance.FillStats();

            Instance.Stats.Clear();
            for (int i = 0; i < Instance.statList.Count; i++)
            {
                try
                {
                    Instance.Stats.Add(Instance.statList[i].StatID, Instance.statList[i]);
                }
                catch (System.Exception ex)
                {
                    ModAPI.Log.Write("Error with adding a stat " + ex.ToString());
                }
            }
            try
            {
                Instance.FillItems();
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write("Error with item " + ex.ToString());

            }
            Instance.ItemBases.Clear();
            for (int i = 0; i < Instance._Item_Bases.Count; i++)
            {
                try
                {
                    ModAPI.Log.Write(Instance._Item_Bases[i].name + Instance._Item_Bases[i].ID + " - item added");

                    Instance.ItemBases.Add(Instance._Item_Bases[i].ID, Instance._Item_Bases[i]);
                    if (Instance.ItemRarityGroups.ContainsKey(Instance._Item_Bases[i].Rarity))
                    {
                        Instance.ItemRarityGroups[Instance._Item_Bases[i].Rarity].Add(Instance._Item_Bases[i].ID);
                    }
                    else
                    {
                        Instance.ItemRarityGroups.Add(Instance._Item_Bases[i].Rarity, new List<int>() { Instance._Item_Bases[i].ID });

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
            Instance._Item_Bases.Add(item);
        }
        public static void AddStat(ItemStat item)
        {
            Instance.statList.Add(item);
        }

        public static ItemStat StatByID(int id)
        {
            return ItemDataBase.Instance.Stats[id];
        }

        public static Item GetRandomItem(float Worth)
        {
            //this needs to be changed to take random value of average of all player levels - and exclude the level of dedicated server.
            int randomLevel = 1;
            if (GameSetup.IsMultiplayer)
            {
                int sum = 0;
                foreach (int a in ModReferences.PlayerLevels.Values)
                {
                    sum += a;
                }
                sum /= ModReferences.PlayerLevels.Values.Count;
                randomLevel = Random.Range(Mathf.Max(sum - 15, 1), ModdedPlayer.instance.Level + 16);
            }
            else
            {
                randomLevel = Random.Range(Mathf.Max(ModdedPlayer.instance.Level - 5, 1), ModdedPlayer.instance.Level + 10);
            }
            float w = Worth / randomLevel;
            int rarity = 0;

            if (w > 200)
            {
                rarity = 1;
                if (w > 350 && Random.value < 0.95f)
                {
                    rarity = 2;

                    if (w > 500 && Random.value < 0.85f)
                    {
                        rarity = 3;

                        if (w > 625 && Random.value < 0.75f)
                        {
                            rarity = 4;

                            if (w > 700 && Random.value < 0.60f)
                            {
                                rarity = 5;

                                if (w > 800 && Random.value < 0.5f)
                                {
                                    rarity = 6;

                                    if (w > 950 && Random.value < 0.40f)
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
            while (!Instance.ItemRarityGroups.ContainsKey(rarity) && rarity > 0)
            {
                increasedLvl += 2;
                rarity--;
            }
            int randomID = Instance.ItemRarityGroups[rarity][Random.Range(0, Instance.ItemRarityGroups[rarity].Count)];
            Item item = new Item(Instance.ItemBases[randomID], 1, increasedLvl);
            item.level = Mathf.Max(item.level, randomLevel);
            return item;

        }

        public void FillStats()
        {
            int i = 1;
            new ItemStat(i, 1, 1.4f, 1.6f, "Strenght", 2, StatActions.AddStrenght, StatActions.RemoveStrenght, StatActions.AddStrenght); i++;
            new ItemStat(i, 1, 1.4f, 1.6f, "Agility", 2, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility); i++;
            new ItemStat(i, 1, 1.4f, 1.6f, "Vitality", 2, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality); i++;
            new ItemStat(i, 1, 1.4f, 1.6f, "Intelligence", 2, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence); i++;
            new ItemStat(i, 3, 6, 1.7f, "Maximum Life", 3, StatActions.AddHealth, StatActions.RemoveHealth, StatActions.AddHealth); i++;
            new ItemStat(i, 3, 6, 1.7f, "Maximum Energy", 3, StatActions.AddHealth, StatActions.RemoveHealth, StatActions.AddHealth); i++;
            new ItemStat(i, 0.01f, 0.025f, 1.7f, "Life Per Second", 3, StatActions.AddHPRegen, StatActions.RemoveHPRegen, StatActions.AddHPRegen) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.01f, 0.025f, 1.7f, "Stamina Per Second", 3, StatActions.AddStaminaRegen, StatActions.RemoveStaminaRegen, StatActions.AddStaminaRegen) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0001f, 0.0025f, 1.6f, "Stamina Regen %", 5, StatActions.AddStaminaRegenPercent, StatActions.RemoveStaminaRegenPercent, StatActions.AddStaminaRegenPercent) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.0025f, 1.6f, "Life Regen %", 5, StatActions.AddHealthRegenPercent, StatActions.RemoveHealthRegenPercent, StatActions.AddHealthRegenPercent) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.0001f, 0.0005f, 1.5f, "Damage Reduction %", 7, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, StatActions.AddDamageReduction) { ValueCap = 0.99f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.01f, 0.03f, 1.5f, "Critical Hit Chance", 6, StatActions.AddCritChance, StatActions.RemoveCritChance, StatActions.AddCritChance) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.01f, 0.03f, 1.7f, "Critical Hit Damage", 6, StatActions.AddCritDamage, StatActions.RemoveCritDamage, StatActions.AddCritDamage) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.1f, 0.35f, 1.6f, "Life on hit", 6, StatActions.AddLifeOnHit, StatActions.RemoveLifeOnHit, StatActions.AddLifeOnHit); i++;
            new ItemStat(i, 0.0005f, 0.0002f, 1.3f, "Dodge chance", 7, StatActions.AddDodgeChance, StatActions.RemoveDodgeChance, StatActions.AddDodgeChance) {ValueCap = 0.99f, DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 2f, 1.6f, "Armor", 2, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.0005f, 0.001f, 1.4f, "Resistance to magic", 4, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, StatActions.AddMagicResistance) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.005f, 0.01f, 1.4f, "Attack speed", 6, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, StatActions.AddAttackSpeed) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.0003f, 0.0007f, 1.2f, "Exp %", 6, StatActions.AddExpFactor, StatActions.RemoveExpFactor, StatActions.AddExpFactor) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 2f, 1.1f, "Massacre Duration", 5, StatActions.AddMaxMassacreTime, StatActions.RemoveMaxMassacreTime, StatActions.AddMaxMassacreTime); i++;
            new ItemStat(i, 0.001f, 0.002f, 1.7f, "Spell Damage %", 7, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, StatActions.AddSpellDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.002f, 1.7f, "Melee Damage %", 7, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, StatActions.AddMeleeDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 0.001f, 0.002f, 1.7f, "Ranged Damage %", 7, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, StatActions.AddRangedDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 2f, 1.5f, "Bonus Spell Damage", 5, StatActions.AddSpellDamageBonus, StatActions.RemoveSpellDamageBonus, StatActions.AddSpellDamageBonus); i++;
            new ItemStat(i, 1f, 2f, 1.5f, "Bonus Melee Damage", 5, StatActions.AddMeleeDamageBonus, StatActions.RemoveMeleeDamageBonus, StatActions.AddMeleeDamageBonus); i++;
            new ItemStat(i, 1f, 2f, 1.5f, "Bonus Ranged Damage", 5, StatActions.AddRangedDamageBonus, StatActions.RemoveRangedDamageBonus, StatActions.AddRangedDamageBonus); i++;
            new ItemStat(i, 0.005f, 0.01f, 1.2f, "Energy Per Agility", 7, StatActions.AddEnergyPerAgility, StatActions.RemoveEnergyPerAgility, StatActions.AddEnergyPerAgility); i++;
            new ItemStat(i, 0.05f, 0.1f, 1.2f, "Health Per Vitality", 7, StatActions.AddHealthPerVitality, StatActions.RemoveHealthPerVitality, StatActions.AddHealthPerVitality); i++;
            new ItemStat(i, 0.0001f, 0.0005f, 1.2f, "Spell Damage Per Intelligence", 7, StatActions.AddSpellDamageperInt, StatActions.RemoveSpellDamageperInt, StatActions.AddSpellDamageperInt); i++;
            new ItemStat(i, 0.0001f, 0.0005f, 1.2f, "Damage Per Strenght", 7, StatActions.AddDamagePerStrenght, StatActions.RemoveDamagePerStrenght, StatActions.AddDamagePerStrenght); i++;
            new ItemStat(i, 0.0001f, 0.0005f, 1.6f, "All Healing %", 7, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, StatActions.AddHealingMultipier) { DisplayAsPercent = true, RoundingCount = 1 }; i++;
            new ItemStat(i, 1f, 1f, 0f, "PERMANENT PERK POINTS", 7, null, null, StatActions.PERMANENT_perkPointIncrease); i++;
            new ItemStat(i, 4f, 7f, 1.5f, "EXPERIENCE", 7, null, null, StatActions.PERMANENT_expIncrease); i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.1f, "Movement Speed", 5, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, StatActions.AddMoveSpeed) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.1f, "Melee Weapon Range", 5, f => ModdedPlayer.instance.MeleeRange += f, f => ModdedPlayer.instance.MeleeRange -= f, f => ModdedPlayer.instance.MeleeRange += f) { DisplayAsPercent = false, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.1f, "Attack Cost Reduction", 3, f => AddPercentage(ref ModdedPlayer.instance.StaminaAttackCostReduction, f), f => RemovePercentage(ref ModdedPlayer.instance.StaminaAttackCostReduction, f), f => AddPercentage(ref ModdedPlayer.instance.StaminaAttackCostReduction, f)) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.0005f, 0.0015f, 1.3f, "Spell Cost Reduction", 6, f => AddPercentage(ref ModdedPlayer.instance.SpellCostRatio, f), f => RemovePercentage(ref ModdedPlayer.instance.SpellCostRatio, f), f => ModdedPlayer.instance.SpellCostRatio *= f) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 0.005f, 0.015f, 1.3f, "Spell Cost to Stamina", 6, f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => RemovePercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f)) { DisplayAsPercent = true, RoundingCount = 2 }; i++;
            new ItemStat(i, 1, 1.2f, 1.5f, "Strenght", 1, StatActions.AddStrenght, StatActions.RemoveStrenght, StatActions.AddStrenght); i++;
            new ItemStat(i, 1, 1.2f, 1.5f, "Agility", 1, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility); i++;
            new ItemStat(i, 1, 1.2f, 1.5f, "Vitality", 1, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality); i++;
            new ItemStat(i, 1, 1.2f, 1.5f, "Intelligence", 1, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence); i++;
            new ItemStat(i, 1.4f, 2f, 1.5f, "Armor", 1, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.0001f, 0.00015f, 1.3f, "Energy Per Second", 6, StatActions.AddEnergyRegen, StatActions.RemoveEnergyRegen, StatActions.AddEnergyRegen) { RoundingCount = 2 }; i++;

            i = 1000;
            new ItemStat(i, 1f, 20f,0f, "Extra Sticks", 5, f => ModdedPlayer.instance.AddExtraItemCapacity(57,Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(57, -Mathf.RoundToInt(f)), null); i++;
            new ItemStat(i, 1f, 6f, 0f, "Extra Rocks", 5, f => ModdedPlayer.instance.AddExtraItemCapacity(53,Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(53, -Mathf.RoundToInt(f)), null); i++;

        }
        public static void AddPercentage(ref float variable1, float f)
        {
            variable1 = 1 - (1 - variable1) * f;
        }
        public static void RemovePercentage(ref float variable1, float f)
        {
            variable1 = 1 - (1 - variable1) / f;
        }

        public void FillItems()
        {
            FillBoots();
            FillGloves();
            FillPants();
        }

        private void FillPants()
        {
            new BaseItem(new int[][]
           {
                new int[] {43,0 },
                new int[] {43,0 },
           })
            {
                name = "Worn Shorts",
                description = "Some protection for legs.",
                lore = "Shorth, made out of cheap thin fabric, and on top of that they are damaged. But its better than nothing.",
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
                icon = Res.ResourceLoader.GetTexture(87),
            };
        }

        private void FillGloves()
        {
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
        }

        private void FillBoots()
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
        }
    }
}
