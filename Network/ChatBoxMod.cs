using Bolt;

using TheForest.UI.Multiplayer;

namespace ChampionsOfForest.Network
{
	public class ChatBoxMod : ChatBox
	{
		//UNIQUE ID OF THE FAKE PLAYER
		public const ulong ModSenderPacked = 999999421;

		public static NetworkId ModNetworkID;

		public static ChatBoxMod instance = null;

		protected override void Awake()
		{
			if (instance == null)
			{
				instance = this;
				ModNetworkID = new NetworkId(ModSenderPacked);
			}

			base.Awake();
		}

		[ModAPI.Attributes.Priority(200)]
		public override void AddLine(NetworkId? playerId, string message, bool system)
		{
			if (playerId == ModNetworkID)
			{
				if (message.StartsWith("II") && BoltNetwork.isRunning)
				{
					base.AddLine(null, "\n\t" + message.Remove(0, 2), true);

					return;
				}
				NetworkManager.RecieveLine(NetworkManager.DecodeCommand(message));
			}
			else
			{
				base.AddLine(playerId, message, system);
			}
		}
	}
}