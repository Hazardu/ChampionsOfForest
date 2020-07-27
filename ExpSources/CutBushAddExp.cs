using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	internal class CutBushAddExp : CutBush
	{
		public override void CutDown()
		{
			ModdedPlayer.instance.AddFinalExperience(Random.Range(3,6));
			base.CutDown();
		}
	}
}