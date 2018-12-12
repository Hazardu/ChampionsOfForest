using ChampionsOfForest.Items;
using System.Collections.Generic;
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

        public List<BaseItem> _Item_Bases = new List<BaseItem>();
        public Dictionary<int, BaseItem> ItemBases = new Dictionary<int, BaseItem>();
        public List<ItemStat> statList = new List<ItemStat>();
        public Dictionary<int, ItemStat> Stats = new Dictionary<int, ItemStat>();

        private Dictionary<int, List<int>> ItemRarityGroups = new Dictionary<int, List<int>>();
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
            int randomLevel = Random.Range(Mathf.Max(ModdedPlayer.instance.Level - 8, 1), ModdedPlayer.instance.Level + 10);
            float w = Worth / randomLevel;
            int rarity = 0;

            if (w > 200 && w <= 350)
            {
                rarity = 1;
            }
            else if (w > 350 && w <= 500)
            {
                rarity = 2;

            }
            else if (w > 500 && w <= 625)
            {
                rarity = 3;

            }
            else if (w > 625 && w <= 700)
            {
                rarity = 4;

            }
            else if (w > 700 && w <= 800)
            {
                rarity = 5;

            }
            else if (w > 800 && w <= 950)
            {
                rarity = 6;

            }
            else if (w > 950)
            {
                rarity = 7;

            }
            int increasedLvl = 0;
            while (!Instance.ItemRarityGroups.ContainsKey(rarity) && rarity > 0)
            {
                increasedLvl += 2;
                rarity--;
            }
            int randomID = Instance.ItemRarityGroups[rarity][Random.Range(0, Instance.ItemRarityGroups[rarity].Count)];
            Item item = new Item(Instance.ItemBases[randomID], 1, increasedLvl);
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
            new ItemStat(i, 0.1f, 0.25f, 1.7f, "Life Per Second", 4, StatActions.AddHPRegen, StatActions.RemoveHPRegen, StatActions.AddHPRegen); i++;
            new ItemStat(i, 0.1f, 0.25f, 1.7f, "Energy Per Second", 4, StatActions.AddERegen, StatActions.RemoveERegen, StatActions.AddERegen); i++;
            new ItemStat(i, 0.01f, 0.025f, 1.6f, "Energy Regen %", 5, StatActions.AddEnergyRegenPercent, StatActions.RemoveEnergyRegenPercent, StatActions.AddEnergyRegenPercent); i++;
            new ItemStat(i, 0.01f, 0.025f, 1.6f, "Life Regen %", 5, StatActions.AddHealthRegenPercent, StatActions.RemoveHealthRegenPercent, StatActions.AddHealthRegenPercent); i++;
            new ItemStat(i, 0.005f, 0.02f, 1.5f, "Damage Reduction %", 7, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, StatActions.AddDamageReduction); i++;
            new ItemStat(i, 0.01f, 0.03f, 1.5f, "Critical Hit Chance", 6, StatActions.AddCritChance, StatActions.RemoveCritChance, StatActions.AddCritChance); i++;
            new ItemStat(i, 0.01f, 0.03f, 1.7f, "Critical Hit Damage", 6, StatActions.AddCritDamage, StatActions.RemoveCritDamage, StatActions.AddCritDamage); i++;
            new ItemStat(i, 0.1f, 0.35f, 1.6f, "Life on hit", 6, StatActions.AddLifeOnHit, StatActions.RemoveLifeOnHit, StatActions.AddLifeOnHit); i++;
            new ItemStat(i, 0.005f, 0.02f, 1.3f, "Dodge chance", 7, StatActions.AddDodgeChance, StatActions.RemoveDodgeChance, StatActions.AddDodgeChance); i++;
            new ItemStat(i, 1f, 2f, 1.6f, "Armor", 2, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor); i++;
            new ItemStat(i, 0.5f, 1f, 1.4f, "Resistance to magic", 4, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, StatActions.AddMagicResistance); i++;
            new ItemStat(i, 0.005f, 0.01f, 1.4f, "Attack speed", 6, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, StatActions.AddAttackSpeed); i++;
            new ItemStat(i, 0.02f, 0.06f, 1.1f, "Exp %", 6, StatActions.AddExpFactor, StatActions.RemoveExpFactor, StatActions.AddExpFactor); i++;
            new ItemStat(i, 1f, 2f, 1.1f, "Massacre Duration", 5, StatActions.AddMaxMassacreTime, StatActions.RemoveMaxMassacreTime, StatActions.AddMaxMassacreTime); i++;
            new ItemStat(i, 0.01f, 0.02f, 1.7f, "Spell Damage %", 7, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, StatActions.AddSpellDamageAmplifier); i++;
            new ItemStat(i, 0.01f, 0.02f, 1.7f, "Melee Damage %", 7, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, StatActions.AddMeleeDamageAmplifier); i++;
            new ItemStat(i, 0.01f, 0.02f, 1.7f, "Ranged Damage %", 7, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, StatActions.AddRangedDamageAmplifier); i++;
            new ItemStat(i, 1f, 2f, 1.5f, "Bonus Spell Damage", 5, StatActions.AddSpellDamageBonus, StatActions.RemoveSpellDamageBonus, StatActions.AddSpellDamageBonus); i++;
            new ItemStat(i, 1f, 2f, 1.5f, "Bonus Melee Damage", 5, StatActions.AddMeleeDamageBonus, StatActions.RemoveMeleeDamageBonus, StatActions.AddMeleeDamageBonus); i++;
            new ItemStat(i, 1f, 2f, 1.5f, "Bonus Ranged Damage", 5, StatActions.AddRangedDamageBonus, StatActions.RemoveRangedDamageBonus, StatActions.AddRangedDamageBonus); i++;
            new ItemStat(i, 0.005f, 0.01f, 1.2f, "Energy Per Agility", 7, StatActions.AddEnergyPerAgility, StatActions.RemoveEnergyPerAgility, StatActions.AddEnergyPerAgility); i++;
            new ItemStat(i, 0.05f, 0.1f, 1.2f, "Health Per Vitality", 7, StatActions.AddHealthPerVitality, StatActions.RemoveHealthPerVitality, StatActions.AddHealthPerVitality); i++;
            new ItemStat(i, 0.0001f, 0.0005f, 1.2f, "Spell Damage Per Intelligence", 7, StatActions.AddSpellDamageperInt, StatActions.RemoveSpellDamageperInt, StatActions.AddSpellDamageperInt); i++;
            new ItemStat(i, 0.0001f, 0.0005f, 1.2f, "Damage Per Strenght", 7, StatActions.AddDamagePerStrenght, StatActions.RemoveDamagePerStrenght, StatActions.AddDamagePerStrenght); i++;
            new ItemStat(i, 0.001f, 0.005f, 1.6f, "All Healing %", 7, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, StatActions.AddHealingMultipier); i++;
            new ItemStat(i, 1f, 1f, 0f, "PERMANENT PERK POINTS", 7, null, null, StatActions.PERMANENT_perkPointIncrease); i++;
            new ItemStat(i, 4f, 7f, 1.5f, "EXPERIENCE", 3, null, null, StatActions.PERMANENT_expIncrease); i++;
            new ItemStat(i, 0.05f, 0.15f, 1.1f, "Movement Speed", 5, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, StatActions.AddMoveSpeed); i++;



        }
        public void FillItems()
        {
            int ID = 1;
            new BaseItem(
                new int[][] { new int[] { 1, 2, 3, 4 }, new int[] { 0, 34 } },
                0,              //rarity
                ID,             //ID
                1,              //Stack size
                BaseItem.ItemType.Boot,                                                                                     //item type
                "Broken Shoes",                                                                                               //item name
                "A pair of damaged shoes. Judging by their condition, i can imagine what happened to their owner.",          //description
                "Worn by one of the passengers of the plane that Eric also flew in.",                                        //lore
                "Shoes can provide movement speed bonuses.",                                                                 //tooltip
                1, 4,                                                                                                       //Min and max levels
                Texture2D.whiteTexture                                                                                      //Icon
            );
            ID++;

            new BaseItem(
    new int[][] { new int[] { 1, 2, 3, 4 }, new int[] { 34 } },
    1,              //rarity
    ID,             //ID
    1,              //Stack size
    BaseItem.ItemType.Boot,                                                                                     //item type
    "Worn Shoes",                                                                                               //item name
    "They look used, through are usable.",          //description
    "Worn by one of the passengers of the plane that Eric also flew in.",                                        //lore
    "Shoes can provide movement speed bonuses",                                                                 //tooltip
    3, 8,                                                                                                       //Min and max levels
    Texture2D.whiteTexture                                                                                      //Icon
);
            ID++;

            new BaseItem(
new int[][] { new int[] { 1, 2, 3, 4, 8 }, new int[] { 34 } },
2,              //rarity
ID,             //ID
1,              //Stack size
BaseItem.ItemType.Boot,                                                                                     //item type
"Leather Shoes",                                                                                               //item name
"High quality footwear.",                                                                       //description
"They look unused. Maybe they were too big for the previous owner. Luckily, it's my size.",                                        //lore
"Shoes can provide movement speed bonuses",                                                                 //tooltip
8, 14,                                                                                                       //Min and max levels
Texture2D.whiteTexture                                                                                      //Icon
);
            ID++;

            new BaseItem(
new int[][] { new int[] { 16 }, new int[] { 0, 0, 0, 0, 16, 7 } },
0,                                                                                                      //rarity
ID,                                                                                                      //ID
1,                                                                                                      //Stack size
BaseItem.ItemType.Pants,                                                                                     //item type
"Shorts",                                                                                               //item name
"Cheap pants, offer almost no protection.",                                                                       //description
"They smell, but it's better than walking butt naked.",                                        //lore
"Pants mainly offer armor, sometimes they come with additional stats",                                                                 //tooltip
1, 5,                                                                                                       //Min and max levels
Texture2D.whiteTexture                                                                                      //Icon
);
            ID++;

            new BaseItem(
new int[][] { new int[] { 16 }, new int[] { 16, 7, 1, 2, 3, 4 }, new int[] { 7, 8, 0, 0, 0 } },
0,                                                                                                      //rarity
ID,                                                                                                      //ID
1,                                                                                                      //Stack size
BaseItem.ItemType.Pants,                                                                                     //item type
"Trousers",                                                                                               //item name
"Long pants that offer protection to legs.",                                                                       //description
"They smell, but it's better than walking butt naked.",                                        //lore
"Pants mainly offer armor, sometimes they come with additional stats",                                                                 //tooltip
7, 13,                                                                                                       //Min and max levels
Texture2D.whiteTexture                                                                                      //Icon
);
            ID++;


            new BaseItem(new int[][] { new int[] { 16 }, new int[] { 1, 2, 3, 4 }, new int[] { 7, 8 }, new int[] { 16, 0, 0, 3, 1 } },
2,                                                                                                      //rarity
ID,                                                                                                      //ID
1,                                                                                                      //Stack size
BaseItem.ItemType.ChestArmor,                                                                                     //item type
"Worn jacket",                                                                                               //item name
"Offers plenty of protection for it's level.",                                                                       //description
"They smell, but it's better than walking butt naked.",                                        //lore
"Chest armor offers good protection",                                                                 //tooltip
5, 8,                                                                                                       //Min and max levels
Texture2D.whiteTexture                                                                                      //Icon
);
            ID++;
            new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 }, new int[] { 7, 8 }, new int[] { 16, 0, 0, 3, 1 } },
2,                                                                                                      //rarity
ID,                                                                                                      //ID
1,                                                                                                      //Stack size
BaseItem.WeaponModelType.LongSword,                                                                                     //item type
"Dull Sword",                                                                                               //item name
"",                                                                       //description
"",                                        //lore
"",                                                                 //tooltip
1, 3,                                                                                                       //Min and max levels
Texture2D.whiteTexture                                                                                      //Icon
);
            ID++;
            new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 3, ID, 1, BaseItem.ItemType.ShoulderArmor, "Shoulder armor", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 3, ID, 1, BaseItem.ItemType.Shield, "Shield", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 4, ID, 1, BaseItem.ItemType.Ring, "Ring", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 5, ID, 1, BaseItem.ItemType.Pants, "Pants", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 6, ID, 1, BaseItem.ItemType.Helmet, "Helmet", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 7, ID, 1, BaseItem.ItemType.Glove, "Gloves", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 5, ID, 1, BaseItem.ItemType.ChestArmor, "Breastplate", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 4, ID, 1, BaseItem.ItemType.Bracer, "Bracer", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
               new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 3, ID, 1, BaseItem.ItemType.Amulet, "Amulet", "", "", "", 1, 4, Texture2D.whiteTexture);
            ID++;
            new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 3, ID, 1, BaseItem.WeaponModelType.GreatSword, "Great sword", "Description", "Lore", "Tooltip", 1, 4, Texture2D.whiteTexture);
            ID++;
            new BaseItem(new int[][] { new int[] { 25 }, new int[] { 1, 2, 3, 4 } }, 3, ID, 1, BaseItem.WeaponModelType.LongSword, "Long sword", "Description", "Lore", "Tooltip", 1, 4, Texture2D.whiteTexture);
            ID++;


























        }




    }
}
