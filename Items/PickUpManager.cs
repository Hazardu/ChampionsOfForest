using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest
{
    public static class PickUpManager
    {

        public static Dictionary<int, ItemPickUp> PickUps = new Dictionary<int, ItemPickUp>();

        /// <summary>
        /// spawns a pickup for the clinet.
        /// </summary>
        public static void SpawnPickUp(Item item, Vector3 pos, int amount = 1, int id = -1)
        {
            try
            {
                GameObject spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
                spawn.AddComponent<Rigidbody>();
                spawn.transform.position = pos;
                ItemPickUp pickup = spawn.AddComponent<ItemPickUp>();
                if (id == -1)
                {
                    id = 0;
                    while (PickUps.ContainsKey(id))
                    {
                        id++;
                    }
                }
                pickup.item = item;
                pickup.amount = amount;
                pickup.ID = id;
                PickUps.Add(id, pickup);
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write("Problem with creating a item pickup " + ex.ToString());
            }
        }
        /// <summary>
        /// removes a pickup with given id for the clinet
        /// </summary>
        public static void RemovePickup(int id)
        {
            if (PickUps.ContainsKey(id))
            {
                PickUps[id].Remove();
                PickUps.Remove(id);
            }
            else
            {
                ModAPI.Log.Write("Item desync happened, no item with given id present in the dictionary");
            }
        }
    }
}
