using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest
{
	public class CrossBowControllerMod : crossbowController
	{
		protected override void fireProjectile()
		{
			if (ModdedPlayer.Stats.i_SmokeyCrossbowQuiver.value)
			{
				BuffDB.AddBuff(14, 81, 2.5f, 8);
			}
			ChampionsOfForest.COTFEvents.Instance.OnAttackRanged.Invoke();
			ChampionsOfForest.COTFEvents.Instance.OnAttackRangedCrossbow.Invoke();

			StartCoroutine(RCoroutines.i.AsyncCrossbowFire(_ammoId, _ammoSpawnPosGo, _boltProjectile, this));
		}

		public void PublicEnablePickupTrigger(GameObject go)
		{
			StartCoroutine(enablePickupTrigger(go));
		}
	}
}