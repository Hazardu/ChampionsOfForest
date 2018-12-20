using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public class ModReferences : MonoBehaviour
    {
        public static ClinetItemPicker ItemPicker => ClinetItemPicker.Instance;
        public static List<GameObject> Players
        {
            get;
                       private set;
        }
        public static int ThisPlayerID
        {
            get;
            private set;
        }
        public static List<BoltEntity> AllPlayerEntities = new List<BoltEntity>();
     
        private void Start()
        {
            if (BoltNetwork.isRunning)
            {
                Players = new List<GameObject>();
                InvokeRepeating("UpdateSetups", 1, 10);
            }
            else
            {
                Players = new List<GameObject>() { LocalPlayer.GameObject };
            }
        }
        /// <summary>
        /// Updates the player setups and changes the static variable accordingly
        /// </summary>
        private void UpdateSetups()
        {
            try
            {
  
                Players.Clear();
                AllPlayerEntities.Clear();
                for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
                {
                    Players.Add(Scene.SceneTracker.allPlayers[i]);
                    if(Scene.SceneTracker.allPlayers[i].transform.root == LocalPlayer.Transform.root)
                    {
                        ThisPlayerID = i;
                    }
                    BoltEntity b = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
                    if (b != null)
                    {
                        AllPlayerEntities.Add(b);

                    }
                }

                
                    ModAPI.Console.Write("ThisPlayerID " + ThisPlayerID);
                    ModAPI.Console.Write("AllPlayerEntities " + AllPlayerEntities.Count);
                    ModAPI.Console.Write("Players " + Players.Count);
                
            }
            catch (System.Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }

        }
    }

}
