namespace ChampionsOfForest.ExpSources
{
	internal class CutBush2AddExp : CutBush2
	{
		public override void CutDown()
		{
			ModdedPlayer.instance.AddFinalExperience(3);
			base.CutDown();
		}
	}
}