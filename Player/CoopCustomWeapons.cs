using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public static class CoopCustomWeapons
    {
        private static Dictionary<Transform, Dictionary<int, GameObject>> customWeapons = new Dictionary<Transform, Dictionary<int, GameObject>>();
        private static Dictionary<Transform, GameObject> playerPlaneAxes = new Dictionary<Transform, GameObject>();

        public static void SetWeaponOn(Transform tr, int i)
        {
            try
            {


                if (!playerPlaneAxes.ContainsKey(tr))
                {
                    Transform axe = tr.Find("AxePlaneHeld");
                    if (axe != null)
                    {
                        playerPlaneAxes.Add(tr, axe.gameObject);
                        axe.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("NO PLANE AXE FOR CLIENT FOUND");
                    }
                }
                else
                {
                    playerPlaneAxes[tr].SetActive(false);
                }
            }
            catch (Exception e)
            {

                Debug.Log(e.ToString());
            }

            try
            {


                if (!customWeapons.ContainsKey(tr))
                {
                    Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();
                    foreach (KeyValuePair<BaseItem.WeaponModelType, CustomWeapon> pair in PlayerInventoryMod.customWeapons)
                    {
                        dic.Add((int)pair.Key, pair.Value.CreateClientGameObject(tr));
                    }

                    customWeapons.Add(tr, dic);
                }
                if (customWeapons.ContainsKey(tr))
                {
                    foreach (KeyValuePair<int, GameObject> item in customWeapons[tr])
                    {
                        if (item.Key != i)
                        {
                            item.Value.SetActive(false);
                        }
                        else
                        {
                            item.Value.SetActive(true);

                        }

                    }

                }
                else
                {
                    Debug.Log("NO transform as a key");
                }
            }
            catch (Exception e)
            {

                Debug.Log("Exc 2 \n" + e.ToString());
            }

        }


    }
}
