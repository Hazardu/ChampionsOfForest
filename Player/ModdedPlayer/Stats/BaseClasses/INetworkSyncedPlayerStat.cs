using System.IO;

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
		void ReceivedOtherPlayerChange(BinaryReader reader);
		void PlayerDisconnected();

	}
}