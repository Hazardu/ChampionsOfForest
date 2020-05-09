using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Player
{
    public class CustomCrafting
    {
        public static CustomCrafting instance;
        public Rerolling rerolling;
        public Reforging reforging;

        public enum CraftMode { Rerolling, Reforging, Repurposing, Upgrading}
        public CraftMode craftMode = CraftMode.Rerolling;

        public static void Init()
        {
            instance = new CustomCrafting();
            instance.rerolling = new Rerolling(instance);
            instance.reforging = new Reforging(instance);
        }
        public CustomCrafting()
        {
            ingredients = new CraftingIngredient[9];
            for (int i = 0; i < 9; i++)
            {
                ingredients[i]=(new CraftingIngredient());
            }
            changedItem = new CraftingIngredient();

        }

        public CraftingIngredient changedItem;
        public CraftingIngredient[] ingredients;

        public void ClearedItem()
        {

        }
        public class Reforging
        {
            public CustomCrafting cc;
            public readonly int IngredientCount = 3;

            public Reforging(CustomCrafting cc)
            {
                this.cc = cc;
            }

            public bool validRecipe
            {
                get
                {
                    if (cc.changedItem.i == null) return false;
                    int itemCount = 0;
                    int rarity = cc.changedItem.i.Rarity;
                    for (int i = 0; i < cc.ingredients.Length; i++)
                    {
                        if (cc.ingredients[i].i != null)
                        {
                            if (cc.ingredients[i].i.Rarity >= rarity)
                            {
                                itemCount++;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return itemCount == IngredientCount;

                }
            }

            public void PerformReforge()
            {
                if (cc.changedItem.i != null)
                {
                    if (validRecipe)
                    {
                        int lvl = cc.changedItem.i.level;
                        var v = ItemDataBase.ItemBases.Where(x => x.Value.ID != cc.changedItem.i.ID && x.Value.Rarity == cc.changedItem.i.Rarity).Select(x => x.Value).ToArray(); ;
                        var ib = v[UnityEngine.Random.Range(0, v.Length)];

                        var newItem = new Item(ib, 1, 0, false)
                        {
                            level = lvl,
                        };
                        newItem.RollStats();
                        Inventory.Instance.ItemSlots[cc.changedItem.pos] = newItem;
                        cc.changedItem.i = newItem;
                        Effects.Sound_Effects.GlobalSFX.Play(3);

                        for (int i = 0; i < cc.ingredients.Length; i++)
                        {
                            cc.ingredients[i].RemoveItem();
                        }
                    }
                }
            }

        }
        public class Rerolling
        {
            public CustomCrafting cc;
            public readonly int IngredientCount =2;

            public Rerolling(CustomCrafting cc)
            {
                this.cc = cc;
            }

            public bool validRecipe
            {
                get
                {
                    if (cc.changedItem.i == null) return false;
                    int itemCount = 0;
                            int rarity = cc.changedItem.i.Rarity;
                    for (int i = 0; i < cc.ingredients.Length; i++)
                    {
                        if (cc.ingredients[i].i != null)
                        {
                            if (cc.ingredients[i].i.Rarity == rarity)
                            {
                                itemCount++;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return itemCount == IngredientCount;
                
                }
            }

            public void PerformReroll()
            {
                if(cc.changedItem.i != null)
                {
                    if (validRecipe)
                    {
                        cc.changedItem.i.RollStats();
                        Effects.Sound_Effects.GlobalSFX.Play(3);

                        for (int i = 0; i < cc.ingredients.Length; i++)
                        {
                            cc.ingredients[i].RemoveItem();
                        }
                    }
                }
            }

        }

        public class CraftingIngredient
        {
            public Item i = null;
            public int pos = -1;
            public void Assign(int index, Item i)
            {
                this.i = i;
                pos = index;
            }
            public void RemoveItem()
            {
                if (Inventory.Instance.ItemSlots.ContainsKey(pos))
                Inventory.Instance.ItemSlots[pos] = null;
                i = null;
                    pos = -1;
            }
            public void Clear()
            {
                i = null;
                pos = -1;
            }

        }
        public static bool isIngredient(int index)
        {
            return instance.ingredients.Any(x => x.pos == index) ||instance.changedItem.pos == index;
        }
        public static bool UpdateIndex(int index,int newIndex)
        {
            for (int i = 0; i < instance.ingredients.Length; i++)
            {
                if (instance.ingredients[i].pos == index)
                {
                    if (newIndex < -1)
                    {
                        instance.ingredients[i].Clear();
                        return true;
                    }
                    instance.ingredients[i].pos = newIndex;
                    return true;
                }
            }
            if (instance.changedItem.pos == index)
            {
                if (newIndex < -1)
                {
                    instance.changedItem.Clear();
                    return true;
                }
                instance.changedItem.pos = newIndex;
                return true;
            }
            return false;
        }
         public static bool ClearIndex(int index)
        {
            for (int i = 0; i < instance.ingredients.Length; i++)
            {
                if (instance.ingredients[i].pos == index)
                {
                    instance.ingredients[i].Clear();
                    return true;
                }
            }
            if (instance.changedItem.pos == index)
            {
                instance.changedItem.Clear();
                return true;
            }
            return false;
        }


    }
}
