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
        public static ulong ThisPlayerPacked
        {
            get;
            private set;
        }
        public static List<BoltEntity> AllPlayerEntities = new List<BoltEntity>();
        public static Dictionary<ulong,int> PlayerLevels = new Dictionary<ulong, int>();
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
        float LevelRequestCooldown = 10;
        void Update()
        {  if (GameSetup.IsMpServer && BoltNetwork.isRunning)
            {
                LevelRequestCooldown -= Time.deltaTime;
                if (LevelRequestCooldown < 0)
                {
                    LevelRequestCooldown = 60;
                    Network.NetworkManager.SendLine("RLx", Network.NetworkManager.Target.Clinets);

                }
                else if (Players.Count != PlayerLevels.Count)
                {
                    LevelRequestCooldown = 60;

                    Network.NetworkManager.SendLine("RL", Network.NetworkManager.Target.Clinets);

                }
            }
        }
        public static void Host_RequestLevels()
        {
            if(GameSetup.IsMpServer)
            {
                Network.NetworkManager.SendLine("RLx", Network.NetworkManager.Target.Clinets);
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
                    if (Scene.SceneTracker.allPlayers[i].transform.root == LocalPlayer.Transform.root)
                    {
                        ThisPlayerID = i;
                    }
                    BoltEntity b = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
                    if (b != null)
                    {
                        AllPlayerEntities.Add(b);

                    }
                }

                ThisPlayerPacked = LocalPlayer.Entity.networkId.PackedValue;
            }
            catch (System.Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }

        }
    }

}
