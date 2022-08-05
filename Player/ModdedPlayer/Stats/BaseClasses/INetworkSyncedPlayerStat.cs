using System.IO;

using ChampionsOfForest.Network;

namespace ChampionsOfForest.Player
{
	public interface INetworkSyncedPlayerStat
	{
		int StatNetworkIndex
		{
			get; set;
		}
		void ValueChanged();
		void WriteToCommand(BinaryWriter writer);
		void ReceivedOtherPlayerChange(CommandReader reader);
		void PlayerDisconnected();

	}
}