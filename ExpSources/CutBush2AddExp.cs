using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	internal class CutBush2AddExp : CutBush2
	{
		public override void CutDown()
		{
			ModdedPlayer.instance.AddFinalExperience(4);
			if (ModdedPlayer.Stats.perk_doubleStickHarvesting)
			{
				if (sapling)
				{
					Object.Instantiate(MyCut, MyBurstPos.position, MyBurstPos.rotation);
				}
				else
				{
					Object.Instantiate(MyCut, base.transform.position, base.transform.rotation);
				}
			}
			base.CutDown();
		}
	}
}