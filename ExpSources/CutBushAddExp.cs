namespace ChampionsOfForest.ExpSources
{
    internal class CutBushAddExp : CutBush
    {
        public override void CutDown()
        {
            ModdedPlayer.instance.AddFinalExperience(3);
            base.CutDown();
        }
    }
}
