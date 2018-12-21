using TheForest.Items.World;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class CrossBowControllerMod : crossbowController
    {
        protected override void fireProjectile()
        {
            if (LocalPlayer.Inventory.RemoveItem(_ammoId, 1, false, true))
            {
                Vector3 position = _ammoSpawnPosGo.transform.position;
                Quaternion rotation = _ammoSpawnPosGo.transform.rotation;
                GameObject gameObject = Object.Instantiate(_boltProjectile, position, rotation);
                Rigidbody component = gameObject.GetComponent<Rigidbody>();
                if (BoltNetwork.isRunning)
                {
                    BoltEntity component2 = gameObject.GetComponent<BoltEntity>();
                    if ((bool)component2)
                    {
                        BoltNetwork.Attach(gameObject);
                    }
                }
                PickUp componentInChildren = gameObject.GetComponentInChildren<PickUp>(true);
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
                Vector3 up = gameObject.transform.up;
                component.AddForce(22000f * (0.016666f / Time.fixedDeltaTime) * up);
            }
        }
    }
}
