using ChampionsOfForest.Player;

using TheForest.Tools;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	public class ExpEvents
	{
		public static void Initialize()
		{
			EventRegistry.Player.Subscribe(TfEvent.CutTree, OnTreeCut);
		}

		public static void OnTreeCut(object o)
		{
			long xp = 30;
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

		
	}
}