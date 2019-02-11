using Bolt;
using TheForest.UI.Multiplayer;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Network
{
    public class ChatBoxMod : ChatBox
    {
        //UNIQUE ID OF THE FAKE PLAYER
        public static ulong ModSenderPacked = 999999421;

        public static NetworkId ModNetwokrID;

        public static ChatBoxMod instance = null;


       
        protected override void Awake()
        {
            if (instance == null)
            {
                instance = this;
                ModNetwokrID = new NetworkId(ModSenderPacked);

            }

            base.Awake();

        }



        [ModAPI.Attributes.Priority(200)]
        public override void AddLine(NetworkId? playerId, string message, bool system)
        {

            if (playerId == ModNetwokrID)
            {
                if (message.StartsWith("II") && BoltNetwork.isRunning) 
                {
                   base.AddLine(null, "\n\t"+message.Remove(0, 2), true);

                    
                    return;
                }
                NetworkManager.RecieveLine(message);

            }
            else if (message.StartsWith("Hazard, i'm cheating, please give item") && Cheats.Allowed && GameSetup.IsMpServer)
            {
                base.AddLine(playerId, message, system);
                string s = message.Trim("Hazard, i'm cheating, please give item ".ToCharArray());
                if (int.TryParse(s, out int ID))
                {
                    Item item = new Item(ItemDataBase.ItemBases[ID]);
                    NetworkManager.SendItemDrop(item, LocalPlayer.Transform.position);
                }
            }
            else if (message.StartsWith("Hazard, i'm cheating, please give points") && Cheats.Allowed && GameSetup.IsMpServer)
            {
                base.AddLine(playerId, message, system);
                string s = message.Trim("Hazard, i'm cheating, please give points ".ToCharArray());
                if (int.TryParse(s, out int ID))
                {
                    ModdedPlayer.instance.MutationPoints += ID;
                }
            }
            else if (message.StartsWith("Hazard, i'm cheating, please give level") && Cheats.Allowed && GameSetup.IsMpServer)
            {
                base.AddLine(playerId, message, system);
                string s = message.Trim("Hazard, i'm cheating, please give level ".ToCharArray());
                if (int.TryParse(s, out int ID))
                {
                    for (int i = 0; i < ID; i++)
                    {
                        ModdedPlayer.instance.LevelUp();
                    }
                }
            }
            else
            {
                base.AddLine(playerId, message, system);
            }
        }

    }
}
