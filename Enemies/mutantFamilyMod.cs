using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class mutantFamilyMod : mutantFamilyFunctions
	{
		public float tauntTimestamp;
		public bool isTaunted => tauntTimestamp > Time.time;

		public void TauntFamily(float duration)
		{
			tauntTimestamp = Time.time + duration;
			sendAggressiveCombat(duration + 0.5f);
		}

		protected override void switchToFleeArea()
		{
			if (isTaunted)
				return;
			base.switchToFleeArea();
		}

		protected override void switchToEatMe(GameObject go)
		{
			if (isTaunted)
				return;
			base.switchToEatMe(go);
		}

		public override void sendAllFleeArea()
		{
			if (isTaunted)
				return;
			base.sendAllFleeArea();
		}

		public override void sendFleeArea()
		{
			if (isTaunted)
				return;
			base.sendFleeArea();
		}

		protected override void startRescueEvent()
		{
			if (isTaunted)
				return;
			base.startRescueEvent();
		}

		protected override void setRunAwayFromPlayer()
		{
			if (isTaunted)
				return;
			base.setRunAwayFromPlayer();
		}
	}
}