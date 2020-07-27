namespace ChampionsOfForest.Player
{
	internal class BoltPlayerSetupEx : BoltPlayerSetup
	{
		public override void OnEvent(HitPlayer evnt)
		{
			limitDamageTimer = 0;
			base.OnEvent(evnt);
		}
	}
}