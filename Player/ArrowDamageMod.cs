using Bolt;
using ChampionsOfForest.Effects;
using TheForest.Buildings.Creation;
using TheForest.Tools;
using TheForest.Utils;
using System;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class ArrowDamageMod : ArrowDamage
    {
        private float BaseDmg;
        protected override void Start()
        {
            if (ModSettings.IsDedicated)
            {
                base.Start();
                return;
            }
            BaseDmg = damage;
            base.Start();
            damage = Mathf.RoundToInt((BaseDmg + ModdedPlayer.instance.RangedDamageBonus) * ModdedPlayer.instance.RangedAMP * ModdedPlayer.instance.CritDamageBuff);
            if (crossbowBoltType)
            {
                damage = Mathf.RoundToInt(damage * ModdedPlayer.instance.CrossbowDamageMult);
            }
            else if (flintLockAmmoType)
            {
                damage = Mathf.RoundToInt(damage * ModdedPlayer.instance.BulletDamageMult);

            }
            else if (spearType)
            {
                damage = Mathf.RoundToInt(damage * ModdedPlayer.instance.SpearDamageMult);

            }
            else //if arrow
            {
                damage = Mathf.RoundToInt(damage * ModdedPlayer.instance.BowDamageMult);
            }
        }


        public override void CheckHit(Vector3 position, Transform target, bool isTrigger, Collider targetCollider)
        {
            if (ignoreCollisionEvents(targetCollider) && !target.CompareTag("enemyRoot"))
            {
                return;
            }
            if (!isTrigger)
            {
                Molotov componentInParent = transform.GetComponentInParent<Molotov>();
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
                    TreeHealth component = target.GetComponent<TreeHealth>();
                    if (!(bool)component)
                    {
                        component = target.root.GetComponent<TreeHealth>();
                    }
                    if ((bool)component)
                    {
                        component.LodTree.AddTreeCutDownTarget(base.gameObject);
                    }
                }
            }
            else if (target.CompareTag("enemyCollide") || target.tag == "lb_bird" || target.CompareTag("animalCollide") || target.CompareTag("Fish") || target.CompareTag("enemyRoot") || target.CompareTag("animalRoot"))
            {
                if (crossbowBoltType)
                {

                }
                else if (flintLockAmmoType)
                {

                }
                else if (spearType)
                {

                }
                else
                {
                    if (ModdedPlayer.instance.IsCrossfire)
                    {
                        if(Time.time- ModdedPlayer.instance.LastCrossfireTime > 20)
                        {
                            ModdedPlayer.instance.LastCrossfireTime = Time.time;
                            int damage = 55 + (int)(ModdedPlayer.instance.SpellDamageBonus * 1.25f);
                            damage = Mathf.RoundToInt(damage * ModdedPlayer.instance.SpellAMP);
                            damage /= 2;
                            Vector3 pos = Camera.main.transform.position + Camera.main.transform.right*5;
                            Vector3 dir = transform.position - pos;
                            dir.Normalize();
                            if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
                            {
                                MagicArrow.Create(pos, dir, damage, ModReferences.ThisPlayerPacked, SpellActions.MagicArrowDuration, SpellActions.MagicArrowDoubleSlow, SpellActions.MagicArrowDmgDebuff);
                                if (BoltNetwork.isRunning)
                                {
                                    string s = "SC7;" + System.Math.Round(pos.x, 5) + ";" + System.Math.Round(pos.y, 5) + ";" + System.Math.Round(pos.z, 5) + ";" + System.Math.Round(dir.x, 5) + ";" + System.Math.Round(dir.y, 5) + ";" + System.Math.Round(dir.z, 5) + ";";
                                    s += damage + ";" + ModReferences.ThisPlayerPacked + ";" + SpellActions.MagicArrowDuration + ";";
                                    if (SpellActions.MagicArrowDoubleSlow) s += "t;"; else s += "f;";
                                    if (SpellActions.MagicArrowDmgDebuff) s += "t;"; else s += "f;";
                                    Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
                                }
                            }
                            else if (GameSetup.IsMpClient)
                            {
                                MagicArrow.CreateEffect(pos, dir, SpellActions.MagicArrowDmgDebuff, SpellActions.MagicArrowDuration);
                                string s = "SC7;" + System.Math.Round(pos.x, 5) + ";" + System.Math.Round(pos.y, 5) + ";" + System.Math.Round(pos.z, 5) + ";" + System.Math.Round(dir.x, 5) + ";" + System.Math.Round(dir.y, 5) + ";" + System.Math.Round(dir.z, 5) + ";";
                                s += damage + ";" + ModReferences.ThisPlayerPacked + ";" + SpellActions.MagicArrowDuration + ";";
                                if (SpellActions.MagicArrowDoubleSlow) s += "t;"; else s += "f;";
                                if (SpellActions.MagicArrowDmgDebuff) s += "t;"; else s += "f;";
                                Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
                            }
                        }
                    }
                }
                bool flag = target.tag == "lb_bird" || target.CompareTag("lb_bird");
                bool flag2 = target.CompareTag("Fish");
                bool flag3 = target.CompareTag("animalCollide") || target.CompareTag("animalRoot");
                arrowStickToTarget arrowStickToTarget = target.GetComponent<arrowStickToTarget>();
                if (!(bool)arrowStickToTarget)
                {
                    arrowStickToTarget = target.root.GetComponentInChildren<arrowStickToTarget>();
                }
                if (!spearType && !flintLockAmmoType && !flag2)
                {
                    if (arrowStickToTarget && arrowStickToTarget.enabled)
                    {
                        if (flag)
                        {
                            EventRegistry.Achievements.Publish(TfEvent.Achievements.BirdArrowKill, null);
                        }
                        arrowStickToTarget.CreatureType(flag3, flag, flag2);
                        if (BoltNetwork.isRunning)
                        {
                            if (at && at._boltEntity && at._boltEntity.isAttached && at._boltEntity.isOwner)
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
                ModdedPlayer.instance.DoRangedOnHit();

                if (ModdedPlayer.instance.RangedArmorReduction > 0 && target.gameObject.CompareTag("enemyCollide"))
                {
                    if (BoltNetwork.isClient)
                    {
                        BoltEntity be = target.GetComponentInParent<BoltEntity>();
                        if (be == null) { be = target.GetComponent<BoltEntity>(); }
                        if (be != null)
                        {
                            EnemyProgression.ReduceArmor(be, ModdedPlayer.instance.MeleeArmorReduction);
                        }
                    }
                    else
                    {
                        target.transform.SendMessageUpwards("ReduceArmor", ModdedPlayer.instance.MeleeArmorReduction, SendMessageOptions.DontRequireReceiver);
                    }
                }
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
                    BoltEntity boltEntity = target.GetComponentInParent<BoltEntity>();
                    if (!(bool)boltEntity)
                    {
                        boltEntity = target.GetComponent<BoltEntity>();
                    }
                    if (boltEntity && ModSettings.FriendlyFire)
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
                    if ((bool)target.transform.parent.GetComponent<StickFenceChunkArchitect>())
                    {
                        return;
                    }
                    if ((bool)target.transform.parent.GetComponent<BoneFenceChunkArchitect>())
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
                Physics.IgnoreCollision(base.GetComponent<Collider>(), Terrain.activeTerrain.GetComponent<Collider>(), true);
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
