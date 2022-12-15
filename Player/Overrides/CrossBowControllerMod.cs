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
				BuffManager.GiveBuff(14, 81, 2.5f, 8);
			}
			COTFEvents.Instance.OnAttackRanged.Invoke();
			COTFEvents.Instance.OnAttackRangedCrossbow.Invoke();

			StartCoroutine(RCoroutines.Instance.AsyncCrossbowFire(_ammoId, _ammoSpawnPosGo, _boltProjectile, this));
		}

		public void PublicEnablePickupTrigger(GameObject go)
		{
			StartCoroutine(enablePickupTrigger(go));
		}
	}
}