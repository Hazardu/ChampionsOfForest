using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class Inventory : MonoBehaviour
    {
        public static int Height = 8;
        public static int Width = 6;
        public static Inventory Instance
        {
            get;
            private set;
        }
        public int SlotCount;

        public Dictionary<int, Item> ItemList = new Dictionary<int, Item>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            SlotCount = Height * Width;
            ItemList.Clear();
            for (int y = 0; y < Height; y++)
            {


                for (int x = 0; x < Width; x++)
                {
                    ItemList.Add(x + y * Width, null);
                }
            }
            ItemList.Add(-2, null);//helmet
            ItemList.Add(-3, null);//chest
            ItemList.Add(-4, null);//pants
            ItemList.Add(-5, null);//boots
            ItemList.Add(-6, null);//shoulders
            ItemList.Add(-7, null);//gloves
            ItemList.Add(-8, null);//tallisman
            ItemList.Add(-9, null);//bracer
            ItemList.Add(-10, null);//ringR
            ItemList.Add(-11, null);//ringL
            ItemList.Add(-12, null);//mainHand
            ItemList.Add(-13, null);//offhand

        }
        private void Update()
        {
            foreach (KeyValuePair<int, Item> item in ItemList)
            {
                if (item.Value != null)
                {
                    if (item.Value.Equipped)
                    {
                        if (item.Key > -1)
                        {
                            item.Value.Equipped = false;
                            item.Value.OnUnequip();
                        }
                    }
                    else
                    {
                        if (item.Key < -1)
                        {
                            if (item.Value.level <= ModdedPlayer.instance.Level)
                            {
                                item.Value.Equipped = true;
                                item.Value.OnEquip();
                            }
                        }
                    }
                }
            }
        }
        public bool DropItem(int key, int amount = 0)
        {
            if (ItemList[key] != null && ItemList.ContainsKey(key))
            {
                Item i = ItemList[key];
                if (amount == 0)
                {
                    amount = i.Amount;
                }
                Network.NetworkManager.SendItemDrop(i, LocalPlayer.Transform.position + Vector3.up + LocalPlayer.Transform.forward, amount);
                ItemList[key].Amount -= amount;
                if (ItemList[key].Amount <= 0)
                {
                    if (ItemList[key].Equipped) ItemList[key].onUnequip();
                    ItemList[key] = null;
                }
                return true;
            }
            return false;
        }
        public void DropAll()
        {
            foreach (KeyValuePair<int, Item> pair in ItemList)
            {
                if (pair.Value != null)
                {
                    DropItem(pair.Key);
                }
            }
        }


        public bool AddItem(Item item, int amount = 1)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                if (ItemList[i] != null)
                {
                    if (ItemList[i].ID == item.ID)
                    {
                        if (ItemList[i].StackSize >= amount + ItemList[i].Amount)
                        {
                            ItemList[i].Amount += amount;
                            PickedUpNotification(ItemList[i].name);
                            amount = 0;

                        }
                        else if (ItemList[i].StackSize - ItemList[i].Amount > 0)
                        {
                            int extrafit = ItemList[i].StackSize - ItemList[i].Amount;
                            ItemList[i].Amount = ItemList[i].StackSize;
                            amount -= extrafit;
                        }
                        if (amount <= 0)
                        {
                            return true;
                        }
                    }
                }
            }
            for (int i = 0; i < SlotCount; i++)
            {
                if (ItemList[i] == null)
                {
                    ItemList[i] = item;
                    ItemList[i].Amount = amount;
                    PickedUpNotification(ItemList[i].name);
                    return true;
                }
            }
            return false;
        }
        public void PickedUpNotification(string item)
        {

        }

    }
}
