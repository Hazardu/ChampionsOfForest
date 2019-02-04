namespace ChampionsOfForest.ExpSources
{
    internal class CutEffigyMod : CutEffigy
    {
        protected override void CutDown()
        {
            if (!breakEventPlayed)
            {
                long Expamount = 40;
                ModdedPlayer.instance.AddFinalExperience(Expamount);
            }
            base.CutDown();
        }
    }
}
