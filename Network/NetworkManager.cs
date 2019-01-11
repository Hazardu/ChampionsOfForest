using Bolt;
using System;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Network
{
    public class NetworkManager : MonoBehaviour
    {
        public enum Target { OnlyServer, Everyone, Clinets, Others }

        public delegate void OnGetMessage(string s);
        public OnGetMessage onGetMessage;
        public static NetworkManager instance;

        /// <summary>
        /// Sets the 'instance'
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// Sends a string to all players on the server
        /// </summary>
        /// <param name="s">Content of the message, make sure it ends with ';'</param>
        public static void SendLine(string s, Target target)
        {
            ModAPI.Log.Write("SEND:" + s);
            if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
            {
                RecieveLine(s);
            }
            else
            {
                if (BoltNetwork.isRunning)
                {

                    ChatEvent chatEvent = null;
                    switch (target)
                    {
                        case Target.OnlyServer:
                            chatEvent = ChatEvent.Create(GlobalTargets.OnlyServer);
                            break;
                        case Target.Everyone:
                            chatEvent = ChatEvent.Create(GlobalTargets.Everyone);
                            break;
                        case Target.Clinets:
                            chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
                            break;
                        case Target.Others:
                            chatEvent = ChatEvent.Create(GlobalTargets.Others);
                            break;
                        default:
                            break;
                    }

                    chatEvent.Message = s;
                    chatEvent.Sender = ChatBoxMod.ModNetwokrID;
                    chatEvent.Send();
                }
            }
        }
        public static void SendLine(string s, BoltConnection con)
        {
            if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
            {
                RecieveLine(s);
            }
            else
            {
                if (BoltNetwork.isRunning)
                {

                    ChatEvent chatEvent = ChatEvent.Create(con);
                    chatEvent.Message = s;
                    chatEvent.Sender = ChatBoxMod.ModNetwokrID;
                    chatEvent.Send();
                }
            }
        }

        /// <summary>
        /// Called on recieving a message
        /// </summary>
        /// <param name="s"></param>
        public static void RecieveLine(string s)
        {
            try
            {
                //For testing purposes
                ModAPI.Log.Write("RECIEVED:" + s);

                instance.onGetMessage(s);
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());
            }
        }


        public static int lastDropID = 0;
        /// <summary>
        /// Sends a command to create a item drop for all players.
        /// </summary>
        /// <param name="item">A reference to the item object. Things like level and stats will be read off it</param>
        /// <param name="pos">Where to spawn the item at</param>
        /// <param name="amount">How many of this item should be spawned</param>
        public static void SendItemDrop(Item item, Vector3 pos, int amount = 1)
        {
            int id = lastDropID + 1;
            while (PickUpManager.PickUps.ContainsKey(id))
            {
                id++;
            }
            lastDropID = id;
            string msg = "CI" + item.ID + ";" + id + ";" + item.level + ";" + amount + ";" + pos.x + ";" + pos.y + ";" + pos.z + ";";
            foreach (ItemStat stat in item.Stats)
            {
                msg += stat.StatID + ";" + stat.Amount + ";";
            }
            SendLine(msg, Network.NetworkManager.Target.Everyone);

        }
        public static void SendHitmarker(Vector3 pos, int amount)
        {
            string msg = "EH" + amount + ";" + pos.x + ";" + pos.y + ";" + pos.z + ";";
            SendLine(msg, Network.NetworkManager.Target.Everyone);

        }

    }
}
