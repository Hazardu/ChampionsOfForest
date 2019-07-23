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
                if(!GameSetup.IsMpClient && Random.value < 0.08f)
                {
                    Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(0, EnemyProgression.Enemy.NormalSkinnyMale), transform.position + Vector3.up * (1.75f));

                }
            }
            base.CutDown();
        }
    }
}
