using TheForest.Items.World;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class CrossBowControllerMod : crossbowController
    {
        protected override void fireProjectile()
        {
            int repeats = 1;
            if (Effects.Multishot.IsOn)
            {
                if (SpellCaster.RemoveStamina(5 * ModdedPlayer.instance.MultishotCount * ModdedPlayer.instance.MultishotCount))
                {
                    repeats += ModdedPlayer.instance.MultishotCount;

                }
                else
                {
                    Effects.Multishot.IsOn = false;
                    Effects.Multishot.localPlayerInstance.SetActive(false);
                }
            }
            for (int i = 0; i < repeats; i++)
            {
                if (LocalPlayer.Inventory.RemoveItem(_ammoId, 1, false, true))
                {
                    Vector3 position = _ammoSpawnPosGo.transform.position;
                    if (i > 0)
                    {
                        position += 0.5f * _ammoSpawnPosGo.transform.up * (i + 1) / 3;
                        position += 0.5f * _ammoSpawnPosGo.transform.right * (((i-1)%3)-1) ;


                    }

                    Quaternion rotation = _ammoSpawnPosGo.transform.rotation;
                    GameObject gameObject = Object.Instantiate(_boltProjectile, position, rotation);
                    gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;
                    gameObject.AddComponent<ProjectileIgnoreCollision>();

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
                    component.AddForce(22000f * ModdedPlayer.instance.ProjectileSpeedRatio * (0.016666f / Time.fixedDeltaTime) * up);
                }
            }
        }
    }
}
