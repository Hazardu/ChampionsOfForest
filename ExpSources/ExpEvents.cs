using TheForest.Tools;
using TheForest.Utils;

namespace ChampionsOfForest.ExpSources
{
	public class ExpEvents
	{
		public static void Initialize()
		{
			EventRegistry.Player.Subscribe(TfEvent.CutTree, OnTreeCut);
			EventRegistry.Animal.Subscribe(TfEvent.KilledShark, SharkKilled);
		}

		public static void OnTreeCut(object o)
		{
			int xp = 10;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(11);
						w.Write(xp);
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				}
			}
			else
			{
				ModdedPlayer.instance.AddFinalExperience(xp);
			}
		}

		public static void SharkKilled(object o)
		{
			int xp = 300;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(11);
						w.Write(xp);
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				}
			}
			else
			{
				ModdedPlayer.instance.AddFinalExperience(xp);
			}
		}

		//int xp = 12;
		//    if (GameSetup.IsMultiplayer)
		//    {
		//        Network.NetworkManager.SendLine("KY" + xp + ";", Network.NetworkManager.Target.Everyone);
		//    }
		//    else
		//    {
		//        ModdedPlayer.instance.AddFinalExperience(xp);
		//    }
	}
}