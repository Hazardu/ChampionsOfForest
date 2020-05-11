using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Player
{
	class BoltPlayerSetupEx : BoltPlayerSetup
	{
		public override void OnEvent(HitPlayer evnt)
		{
			limitDamageTimer = 0;
			base.OnEvent(evnt);
		}
	}
}
