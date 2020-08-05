using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Player.Crafting;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class Inventory : MonoBehaviour
	{
		public static int Height = 15;
		public static int Width = 8;

		public static Inventory Instance
		{
			get;
			private set;
		}

		public int SlotCount;

		public Dictionary<int, Item> ItemSlots = new Dictionary<int, Item>();

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
			ItemSlots.Clear();
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					ItemSlots.Add(x + y * Width, null);
				}
			}
			ItemSlots.Add(-2, null);//helmet
			ItemSlots.Add(-3, null);//chest
			ItemSlots.Add(-4, null);//pants
			ItemSlots.Add(-5, null);//boots
			ItemSlots.Add(-6, null);//shoulders
			ItemSlots.Add(-7, null);//gloves
			ItemSlots.Add(-8, null);//tallisman
			ItemSlots.Add(-9, null);//bracer
			ItemSlots.Add(-10, null);//ringR
			ItemSlots.Add(-11, null);//ringL
			ItemSlots.Add(-12, null);//mainHand
			ItemSlots.Add(-13, null);//offhand
		}

		private void Update()
		{
			foreach (KeyValuePair<int, Item> item in ItemSlots)
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
							if (item.Value.level <= ModdedPlayer.instance.level)
							{
								item.Value.Equipped = true;
								item.Value.OnEquip();
							}
						}
					}
				}
			}
		}
		public bool DropItemOnPosition(int key, Vector3 pos)
		{
			if (ItemSlots[key] != null && ItemSlots.ContainsKey(key))
			{
				Item i = ItemSlots[key];

				int amount = i.Amount;
				i.Amount = 1;
				for (int ind = 0; ind < amount; ind++)
				{
					Network.NetworkManager.SendItemDrop(i, pos, 1);

				}

				if (ItemSlots[key].Equipped)
				{
					ItemSlots[key].OnUnequip();
					ItemSlots[key].Equipped = false;
				}
				ItemSlots[key] = null;


				return true;
			}
			return false;
		}
		public bool DropItem(int key, int amount = 0)
		{
			if (ItemSlots[key] != null && ItemSlots.ContainsKey(key))
			{
				Item i = ItemSlots[key];

				if (amount == 0)
				{
					amount = i.Amount;
				}
				else
				{
					amount = Mathf.Min(amount, i.Amount);
				}
				Network.NetworkManager.SendItemDrop(i, LocalPlayer.Transform.position + Vector3.up * 1.5f + LocalPlayer.Transform.forward * 3, amount);
				ItemSlots[key].Amount -= amount;
				if (ItemSlots[key].Amount <= 0)
				{
					if (ItemSlots[key].Equipped)
					{
						ItemSlots[key].OnUnequip();
						ItemSlots[key].Equipped = false;
					}
					ItemSlots[key] = null;
				}

				return true;
			}
			return false;
		}

		public void DropAll()
		{
			StartCoroutine(DropAllCoroutine());
		}

		private IEnumerator DropAllCoroutine()
		{
			Vector3 dropPos = LocalPlayer.Transform.position;
			int itemsDropped = 0;
			var keys = ItemSlots.Keys.ToArray();
			for (int i = 0; i < keys.Length; i++)
			{
				try
				{
					itemsDropped += DropItemOnPosition(keys[i], dropPos) ? 1 : 0;
				}
				catch (System.Exception e)
				{
					Debug.LogError("dropping error\n" + e.Message);
				}
				yield return null;
			}
		}

		public void DropEquipped()
		{
			StartCoroutine(DropEquippedCoroutine());
		}

		private IEnumerator DropEquippedCoroutine()
		{
			int itemsDropped = 0;
			Vector3 dropPos = LocalPlayer.Transform.position;

			var keys = ItemSlots.Keys.ToArray();
			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i] < 0)
				{
					try
					{
						itemsDropped += DropItemOnPosition(keys[i], dropPos) ? 1 : 0;
					}
					catch (System.Exception e)
					{
						Debug.LogError("dropping error\n" + e.Message);
					}
					yield return null;
				}
			}
		}

		public bool HasSpaceFor(Item item, int amount = 1)
		{
			for (int i = 0; i < SlotCount; i++)
			{
				if (ItemSlots[i] == null)
				{
					return true;
				}
			}
			return false;
		}

		public bool AddItem(Item item, int amount = 1)
		{
			for (int i = 0; i < SlotCount; i++)
			{
				if (ItemSlots[i] != null)
				{
					if (ItemSlots[i].ID == item.ID)
					{
						if (ItemSlots[i].StackSize >= amount + ItemSlots[i].Amount)
						{
							ItemSlots[i].Amount += amount;
							PickedUpNotification(ItemSlots[i].name);
							amount = 0;
						}
						else if (ItemSlots[i].StackSize - ItemSlots[i].Amount > 0)
						{
							int extrafit = ItemSlots[i].StackSize - ItemSlots[i].Amount;
							ItemSlots[i].Amount = ItemSlots[i].StackSize;
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
				if (ItemSlots[i] == null)
				{
					ItemSlots[i] = item;
					ItemSlots[i].Amount = amount;
					PickedUpNotification(ItemSlots[i].name);
					return true;
				}
			}
			return false;
		}

		public void PickedUpNotification(string item)
		{
		}

		public static void SwapItems(int a, int b)
		{
			Item backup = Inventory.Instance.ItemSlots[a];
			Inventory.Instance.ItemSlots[a] = Inventory.Instance.ItemSlots[b];
			Inventory.Instance.ItemSlots[b] = backup;
			CustomCrafting.UpdateIndex(a, b);
			CustomCrafting.UpdateIndex(b, a);
		}
	}
}