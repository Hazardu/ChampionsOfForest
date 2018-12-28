using Bolt;
using TheForest.Buildings.Creation;
using TheForest.Tools;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class ArrowDamageMod : ArrowDamage
    {


        float BaseDmg;
        protected override void Start()
        {
            BaseDmg = damage;
            base.Start();
            damage = Mathf.RoundToInt((BaseDmg + ModdedPlayer.instance.RangedDamageBonus) * ModdedPlayer.instance.RangedAMP * ModdedPlayer.instance.CritDamageBuff);

        }


        public override void CheckHit(Vector3 position, Transform target, bool isTrigger, Collider targetCollider)
        {
            ModAPI.Console.Write(damage.ToString()+ " Base dmg: "+BaseDmg);
            if (ignoreCollisionEvents(targetCollider) && !target.CompareTag("enemyRoot"))
            {
                return;
            }
            if (!isTrigger)
            {
                Molotov componentInParent = ((Component)base.transform).GetComponentInParent<Molotov>();
                if ((bool)componentInParent)
                {
                    componentInParent.IncendiaryBreak();
                }
            }
            bool headDamage = false;
            if (target.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                FMODCommon.PlayOneshotNetworked(hitWaterEvent, base.transform, FMODCommon.NetworkRole.Any);
            }
            else if (target.CompareTag("SmallTree"))
            {
                FMODCommon.PlayOneshotNetworked(hitBushEvent, base.transform, FMODCommon.NetworkRole.Any);
            }
            if (target.CompareTag("PlaneHull"))
            {
                FMODCommon.PlayOneshotNetworked(hitMetalEvent, base.transform, FMODCommon.NetworkRole.Any);
            }
            if (target.CompareTag("Tree") || target.root.CompareTag("Tree") || target.CompareTag("Target"))
            {
                if (spearType)
                {
                    base.StartCoroutine(HitTree(hit.point - base.transform.forward * 2.1f));
                }
                else if (hitPointUpdated)
                {
                    base.StartCoroutine(HitTree(hit.point - base.transform.forward * 0.35f));
                }
                else
                {
                    base.StartCoroutine(HitTree(base.transform.position - base.transform.forward * 0.35f));
                }
                disableLive();
                if (target.CompareTag("Tree") || target.root.CompareTag("Tree"))
                {
                    TreeHealth component = ((Component)target).GetComponent<TreeHealth>();
                    if (!(bool)component)
                    {
                        component = ((Component)target.root).GetComponent<TreeHealth>();
                    }
                    if ((bool)component)
                    {
                        component.LodTree.AddTreeCutDownTarget(base.gameObject);
                    }
                }
            }
            else if (target.CompareTag("enemyCollide") || target.tag == "lb_bird" || target.CompareTag("animalCollide") || target.CompareTag("Fish") || target.CompareTag("enemyRoot") || target.CompareTag("animalRoot"))
            {
                bool flag = target.tag == "lb_bird" || target.CompareTag("lb_bird");
                bool flag2 = target.CompareTag("Fish");
                bool flag3 = target.CompareTag("animalCollide") || target.CompareTag("animalRoot");
                arrowStickToTarget arrowStickToTarget = ((Component)target).GetComponent<arrowStickToTarget>();
                if (!(bool)arrowStickToTarget)
                {
                    arrowStickToTarget = ((Component)target.root).GetComponentInChildren<arrowStickToTarget>();
                }
                if (!spearType && !flintLockAmmoType && !flag2)
                {
                    if ((bool)arrowStickToTarget && arrowStickToTarget.enabled)
                    {
                        if (flag)
                        {
                            EventRegistry.Achievements.Publish(TfEvent.Achievements.BirdArrowKill, null);
                        }
                        arrowStickToTarget.CreatureType(flag3, flag, flag2);
                        if (BoltNetwork.isRunning)
                        {
                            if ((bool)at && (bool)at._boltEntity && at._boltEntity.isAttached && at._boltEntity.isOwner)
                            {
                                headDamage = arrowStickToTarget.stickArrowToNearestBone(base.transform);
                            }
                        }
                        else
                        {
                            headDamage = arrowStickToTarget.stickArrowToNearestBone(base.transform);
                        }
                    }
                    if ((bool)arrowStickToTarget)
                    {
                        base.Invoke("destroyMe", 0.1f);
                    }
                }
                base.StartCoroutine(HitAi(target, flag || flag3, headDamage));
                ModdedPlayer.instance.DoAreaDamage(target.root, damage);
                ModdedPlayer.instance.DoOnHit();
                if (flag2)
                {
                    base.StartCoroutine(HitFish(target, hit.point - base.transform.forward * 0.35f));
                }
                disableLive();
            }
            else if (target.CompareTag("PlayerNet"))
            {
                if (BoltNetwork.isRunning)
                {
                    BoltEntity boltEntity = ((Component)target).GetComponentInParent<BoltEntity>();
                    if (!(bool)boltEntity)
                    {
                        boltEntity = ((Component)target).GetComponent<BoltEntity>();
                    }
                    if ((bool)boltEntity && ModSettings.FriendlyFire)
                    {
                        HitPlayer HP = HitPlayer.Create(boltEntity, EntityTargets.Everyone);
                        HP.damage = damage;
                        HP.Send();
                        disableLive();
                    }
                }
            }
            else if (target.CompareTag("TerrainMain") && !LocalPlayer.IsInCaves)
            {
                if (ignoreTerrain)
                {
                    ignoreTerrain = false;
                    base.StartCoroutine(RevokeIgnoreTerrain());
                }
                else
                {
                    if (spearType)
                    {
                        if ((bool)bodyCollider)
                        {
                            bodyCollider.isTrigger = true;
                        }
                        base.StartCoroutine(HitStructure(base.transform.position - base.transform.forward * 2.1f, false));
                    }
                    else
                    {
                        Vector3 position2 = base.transform.position - base.transform.forward * -0.8f;
                        float num = Terrain.activeTerrain.SampleHeight(base.transform.position);
                        Vector3 position3 = Terrain.activeTerrain.transform.position;
                        float num2 = num + position3.y;
                        Vector3 position4 = base.transform.position;
                        if (position4.y < num2)
                        {
                            position2.y = num2 + 0.5f;
                        }
                        base.StartCoroutine(HitStructure(position2, false));
                    }
                    disableLive();
                    FMODCommon.PlayOneshotNetworked(hitGroundEvent, base.transform, FMODCommon.NetworkRole.Any);
                }
            }
            else if (target.CompareTag("structure") || target.CompareTag("jumpObject") || target.CompareTag("SLTier1") || target.CompareTag("SLTier2") || target.CompareTag("SLTier3") || target.CompareTag("UnderfootWood"))
            {
                if ((bool)target.transform.parent)
                {
                    if ((bool)((Component)target.transform.parent).GetComponent<StickFenceChunkArchitect>())
                    {
                        return;
                    }
                    if ((bool)((Component)target.transform.parent).GetComponent<BoneFenceChunkArchitect>())
                    {
                        return;
                    }
                }
                if (!isTrigger)
                {
                    if (spearType)
                    {
                        base.StartCoroutine(HitStructure(hit.point - base.transform.forward * 2.1f, true));
                    }
                    else
                    {
                        base.StartCoroutine(HitStructure(hit.point - base.transform.forward * 0.35f, true));
                    }
                    disableLive();
                }
            }
            else if (target.CompareTag("CaveDoor"))
            {
                ignoreTerrain = true;
                Physics.IgnoreCollision(base.GetComponent<Collider>(), ((Component)Terrain.activeTerrain).GetComponent<Collider>(), true);
            }
            else if (flintLockAmmoType && (target.CompareTag("BreakableWood") || target.CompareTag("BreakableRock")))
            {
                target.SendMessage("Hit", 40, SendMessageOptions.DontRequireReceiver);
            }
            if (!Live)
            {
                destroyThisAmmo();
                parent.BroadcastMessage("OnArrowHit", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
