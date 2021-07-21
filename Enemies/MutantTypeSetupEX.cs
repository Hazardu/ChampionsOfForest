using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Enemies
{
	public class MutantTypeSetupEX : mutantTypeSetup
	{
		protected override void killThisEnemy()
		{
			this.setup.waterDetect.drowned = true;
			this.health.HitReal(10000);
		}
		
	}
}
