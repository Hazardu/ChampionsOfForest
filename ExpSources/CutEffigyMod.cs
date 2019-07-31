using TheForest.Utils;
using UnityEngine;

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
                if(!GameSetup.IsMpClient && Random.value < 0.3f + ModReferences.Players.Count * 0.10f )
                {
                    Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(99, EnemyProgression.Enemy.NormalSkinnyMale), transform.position + Vector3.up * (1.75f));

                }
            }
            base.CutDown();
        }
    }
}
