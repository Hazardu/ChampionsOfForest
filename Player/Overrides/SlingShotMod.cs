using TheForest.Items.World;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class SlingShotMod : slingShotController
	{
		public override void fireProjectile()
		{
			int repeats = ModdedPlayer.RangedRepetitions();
			ChampionsOfForest.COTFEvents.Instance.OnAttackRanged.Invoke();

			for (int i = 0; i < repeats; i++)
			{
				bool noconsume = false;
				if (ModdedPlayer.Stats.perk_projectileNoConsumeChance >= 0 && Random.value < ModdedPlayer.Stats.perk_projectileNoConsumeChance)
				{
					noconsume = true;
				}
				if (noconsume || LocalPlayer.Inventory.RemoveItem(_ammoItemId, 1, false, true))
				{
					Vector3 position = _ammoSpawnPos.transform.position;
					if (i > 0)
					{
						position += 0.5f * _ammoSpawnPos.transform.up * (i + 1) / 3;
						position += 0.5f * _ammoSpawnPos.transform.right * (((i - 1) % 3) - 1);
					}
					Quaternion rotation = _ammoSpawnPos.transform.rotation;
					if (ForestVR.Enabled)
					{
						position = _ammoSpawnPosVR.transform.position;
						rotation = _ammoSpawnPosVR.transform.rotation;
					}
					GameObject gameObject = Object.Instantiate(_Ammo, position, rotation);
					gameObject.transform.localScale *= ModdedPlayer.Stats.projectileSize;

					Rigidbody component = gameObject.GetComponent<Rigidbody>();
					rockSound component2 = gameObject.GetComponent<rockSound>();
					if ((bool)component2)
					{
						component2.slingShot = true;
					}
					if (BoltNetwork.isRunning)
					{
						BoltEntity component3 = gameObject.GetComponent<BoltEntity>();
						if ((bool)component3)
						{
							BoltNetwork.Attach(gameObject);
						}
					}
					PickUp componentInChildren = gameObject.GetComponentInChildren<PickUp>();
					if ((bool)componentInChildren)
					{
						SheenBillboard[] componentsInChildren = gameObject.GetComponentsInChildren<SheenBillboard>();
						SheenBillboard[] array = componentsInChildren;
						foreach (SheenBillboard sheenBillboard in array)
						{
							sheenBillboard.gameObject.SetActive(false);
						}
						componentInChildren.gameObject.SetActive(false);
						if (base.gameObject.activeInHierarchy)
						{
							base.StartCoroutine(enablePickupTrigger(componentInChildren.gameObject));
						}
					}
					Vector3 forward = _ammoSpawnPos.transform.forward;
					if (ForestVR.Enabled)
					{
						forward = _ammoSpawnPosVR.transform.forward;
					}
					component.AddForce(4000f * ModdedPlayer.Stats.projectileSpeed * (0.016666f / Time.fixedDeltaTime) * forward);
				}
			}
		}
	}
}