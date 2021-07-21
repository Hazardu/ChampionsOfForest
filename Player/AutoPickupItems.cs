using LibNoise.Unity.Operator;

using TheForest.Items.World;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public static class AutoPickupItems
	{
		private static Effects.SpellAimSphere aimSphere;

		public static void Aim()
		{
			if (aimSphere == null)
			{
				aimSphere = new Effects.SpellAimSphere(new Color(1f, .55f, 0f, 0.5f), radius);
			}
			aimSphere.SetRadius(radius);
			aimSphere.UpdatePosition(LocalPlayer.Transform.position);
		}

		public static void AimEnd()
		{
			aimSphere.Disable();
		}

		public static float radius = 7.5f;

		public static void DoPickup()
		{
			if (LocalPlayer.FpCharacter.PushingSled)
			{
				return;
			}

			int minRarity = 7;
			for (int i = -2; i >-14 ; i--)
			{
				if (Player.Inventory.Instance.ItemSlots[i] == null)
				{
					minRarity = 0;
					break;
				}
				else
				{
					if (Player.Inventory.Instance.ItemSlots[i].Rarity < minRarity)
						minRarity = Player.Inventory.Instance.ItemSlots[i].Rarity;
				}
			}


			RaycastHit[] hit = Physics.SphereCastAll(LocalPlayer.Transform.position, radius, Vector3.one, radius + 1);
			for (int i = 0; i < hit.Length; i++)
			{
				PickUp pu = hit[i].transform.GetComponent<PickUp>();
				if (pu != null)
				{
					Debug.Log("Found pickup in original object");
				}
				if (pu == null)
				{
					pu = hit[i].transform.GetComponentInParent<PickUp>();
					if (pu != null)
					{
						Debug.Log("Found pickup in parent");
					}
				}
				if (pu == null)
				{
					pu = hit[i].transform.GetComponentInChildren<PickUp>();
					if (pu != null)
					{
						Debug.Log("Found pickup in child");
					}
				}

				if (pu != null)
				{
					if (pu._itemId == 57 || pu._itemId == 54 || pu._itemId == 53 || pu._itemId == 42 || pu._itemId == 37 || pu._itemId == 36 || pu._itemId == 33 || pu._itemId == 31 || pu._itemId == 83 || pu._itemId == 91 || pu._itemId == 99 || pu._itemId == 67 || pu._itemId == 89 || pu._itemId == 280 || pu._itemId == 41 || pu._itemId == 56 || pu._itemId == 49 || pu._itemId == 43 || pu._itemId == 262 || pu._itemId == 83 || pu._itemId == 94 || pu._itemId == 99 || pu._itemId == 98 || pu._itemId == 97 || pu._itemId == 178 || pu._itemId == 177 || pu._itemId == 109 || pu._itemId == 307 || pu._itemId == 181 || pu._itemId == 82)
					{
						if (LocalPlayer.Inventory.AddItem(pu._itemId, pu._amount, true, false, pu._properties))
						{
							GameObject.Destroy(pu._destroyTarget);
						}
					}
				}
				else
				{
					var customPickup = hit[i].transform.gameObject.GetComponent<ItemPickUp>();
					if (customPickup != null)
					{
						if(customPickup.item.Rarity>= minRarity)
						customPickup.PickUp();
					}
				}
			}
		}
	}
}