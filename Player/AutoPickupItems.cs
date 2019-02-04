using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Items.World;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public static class AutoPickupItems
    {
        public static float radius = 5;
       public static void DoPickup()
        {
                if (!LocalPlayer.FpCharacter.PushingSled)
                {
                return;
                }
                RaycastHit[] hit = Physics.SphereCastAll(LocalPlayer.Transform.position, radius, Vector3.one, radius + 1);
                for (int i = 0; i < hit.Length; i++)
                {
                    PickUp pu = hit[i].transform.GetComponent<PickUp>();
                    if (pu != null)
                    {
                        if (pu._itemId == 57 || pu._itemId == 54 || pu._itemId == 53 || pu._itemId == 42 || pu._itemId == 37 || pu._itemId == 36 || pu._itemId == 33 || pu._itemId == 31 || pu._itemId == 83 || pu._itemId == 91 || pu._itemId == 99 || pu._itemId == 67 || pu._itemId == 89 || pu._itemId == 280 || pu._itemId == 41 || pu._itemId == 56 || pu._itemId == 49 || pu._itemId == 43 || pu._itemId == 262 || pu._itemId == 83 || pu._itemId == 94 || pu._itemId == 99 || pu._itemId == 98 || pu._itemId == 97 || pu._itemId == 178 || pu._itemId == 177 || pu._itemId == 109||pu._itemId == 307||pu._itemId == 181||pu._itemId == 82)
                            if (LocalPlayer.Inventory.AddItem(pu._itemId, pu._amount, true, false, pu._properties))
                            {
                               GameObject.Destroy(pu._destroyTarget);
                            }

                    }
                }

            }
        }
    }

