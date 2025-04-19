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

			ModdedPlayer.instance.ExpCurrent = (long)((double)ModdedPlayer.instance.ExpCurrent * (double)ModSettings.KeptExperienceAfterDeath);
			ModdedPlayer.instance.NewlyGainedExp = (long)((double)ModdedPlayer.instance.ExpCurrent * (double)ModSettings.KeptExperienceAfterDeath);
			if (ModSettings.EndMassacreAfterDeath)
			{
				ModdedPlayer.instance.MassacreKills = 0;
				ModdedPlayer.instance.MassacreMultiplier = 1;
				ModdedPlayer.instance.TimeUntillMassacreReset = 0;
			}
			ModdedPlayer.instance.AfterRespawn();
			BlackFlame.instance.Start();
			if (GameSetup.IsMultiplayer)
			{
				ModdedPlayer.instance.SendPlayerState();
			}
		}
	}
}