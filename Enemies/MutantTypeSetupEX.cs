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
