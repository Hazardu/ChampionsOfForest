using Bolt;
using System.Collections;
using TheForest.UI.Multiplayer;
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
                ModAPI.Log.Write("SETUP: ChatBoxMod created");

            }
            else
            {
                ModAPI.Log.Write("WARNING: ChatBoxMod already defined instance");
            }
            base.Awake();
        }

        [ModAPI.Attributes.Priority(200)]
        public override void AddLine(NetworkId? playerId, string message, bool system)
        {
          
                if (playerId == ModNetwokrID)
                {
                    NetworkManager.RecieveLine(message);

                }
                else
                {
                    base.AddLine(playerId, message, system);
                }
                }
    }
}
