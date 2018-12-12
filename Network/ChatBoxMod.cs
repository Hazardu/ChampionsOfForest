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

        protected override IEnumerator Start()
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
            return base.Start();
        }
        public override void AddLine(NetworkId? playerId, string message, bool system)
        {
            try
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
            catch (System.Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());
            }
        }

    }
}
