using System.Collections.Generic;
using System.IO;

namespace ChampionsOfForest.Player
{
	internal static class NetworkPlayerStats
	{
		private static int index;
		public static void Reset()
		{
			syncedStats.Clear();
			index = 0;
		}
		public static int NextIndex => index++;
		public static List<INetworkSyncedPlayerStat> syncedStats = new List<INetworkSyncedPlayerStat>();

		public static void SendUpdate(int nID)
		{
			if (!BoltNetwork.isRunning)
				return;
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(-1);
					syncedStats[nID].WriteToCommand(w);
					w.Close();
				}
				Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Others);
				answerStream.Close();
			}

		}
		public static void PlayerLeft()
		{
			foreach (var stat in syncedStats)
			{
				stat.PlayerDisconnected();
			}
		}
	}
}