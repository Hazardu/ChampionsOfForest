using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	class CutBush3AddExp : BushDamage
	{
		protected override void CutDown()
		{
			ModdedPlayer.instance.AddFinalExperience(3);
			if (ModdedPlayer.Stats.perk_doubleStickHarvesting)
			{
				Object.Instantiate(MyCut, MyBurstPos.position, MyBurstPos.rotation);
			}

			base.CutDown();
		}
	}
}
