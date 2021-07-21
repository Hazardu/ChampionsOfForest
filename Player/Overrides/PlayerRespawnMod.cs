using System;
using System.IO;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;

using TheForest.Save;
using TheForest.Utils;

namespace ChampionsOfForest.Player
{
	public class PlayerRespawnMod : PlayerRespawnMP
	{
		protected override void Respawn()
		{
			base.Respawn();
			try
			{
				ModdedPlayer.ResetAllStats();
				CustomBowBase.baseBow = null;
				CustomBowBase.baseBowC = null;
				GreatBow.instance = null;
			}
			catch (Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}

			ModReferences.rightHandTransform = null;

			ModdedPlayer.instance.ExpCurrent = 0;
			ModdedPlayer.instance.NewlyGainedExp = 0;
			ModdedPlayer.instance.MassacreKills = 0;
			ModdedPlayer.instance.MassacreMultipier = 1;
			ModdedPlayer.instance.TimeUntillMassacreReset = 0;
			ModdedPlayer.instance.AfterRespawn();
			BlackFlame.instance.Start();
			if (GameSetup.IsMultiplayer)
			{
				using (MemoryStream answerStream = new MemoryStream())
				{
					using (BinaryWriter w = new BinaryWriter(answerStream))
					{
						w.Write(19);
						w.Write(ModReferences.ThisPlayerID);
						w.Write(ModdedPlayer.instance.level);
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
				}
			}
		}
	}
}