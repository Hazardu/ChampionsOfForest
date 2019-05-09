using Bolt;
using System;
using System.IO;
using TheForest.UI.Multiplayer;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Network
{
    public class NetworkManager : MonoBehaviour
    {
        public enum Target { OnlyServer, Everyone, Clients, Others }
        public delegate void OnGetMessage(byte[] arr);
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
        public static void SendLine(byte[] bytearray, Target target)
        {
            if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
            {
                RecieveLine(bytearray);
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
                        case Target.Clients:
                            chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
                            break;
                        case Target.Others:
                            chatEvent = ChatEvent.Create(GlobalTargets.Others);
                            break;
                        default:
                            break;
                    }
                    chatEvent.Message = EncodeCommand(bytearray);
                    chatEvent.Sender = ChatBoxMod.ModNetworkID;
                    chatEvent.Send();
                }
            }
        }
        public static void SendLine(byte[] bytearray, BoltConnection con)
        {
            if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
            {
                RecieveLine(bytearray);
            }
            else
            {
                if (BoltNetwork.isRunning)
                {

                    ChatEvent chatEvent = ChatEvent.Create(con);
                    chatEvent.Message = EncodeCommand(bytearray);
                    chatEvent.Sender = ChatBoxMod.ModNetworkID;
                    chatEvent.Send();
                }
            }
        }


        public static byte[] DecodeCommand(string cmd)
        {
            var a = cmd.ToCharArray();
            var b = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = (byte)a[i];
            }
            return b;
        }
        public static string EncodeCommand(byte[] b)
        {
            string s = string.Empty;
            for (int i = 0; i < b.Length; i++)
            {
                s += (char)b[i];
            }
            return s;
        }

        /// <summary>
        /// Called on recieving a message
        /// </summary>
        /// <param name="s"></param>
        public static void RecieveLine(byte[] array)
        {
            try
            {
                instance.onGetMessage(array);
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());
            }
        }


        public static ulong lastDropID = 10;
        /// <summary>
        /// Sends a command to create a item drop for all players.
        /// </summary>
        /// <param name="item">A reference to the item object. Things like level and stats will be read off it</param>
        /// <param name="pos">Where to spawn the item at</param>
        /// <param name="amount">How many of this item should be spawned</param>
        public static void SendItemDrop(Item item, Vector3 pos, int amount = 1)
        {
            ulong id = lastDropID + 1;
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
        public static void SendItemToPlayer(Item item, string playerID, int amount = 1)
        {
            string msg = "AG"+ playerID + ";" + item.ID + ";"+ amount+ ";" + item.level + ";";
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
           public static void SendPlayerHitmarker(Vector3 pos, int amount)
        {
            string msg = "PH" + amount + ";" + pos.x + ";" + pos.y + ";" + pos.z + ";";
            SendLine(msg, Network.NetworkManager.Target.Everyone);

        }
     
    }
}
