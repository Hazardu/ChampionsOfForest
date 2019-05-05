using Bolt;
using ChampionsOfForest.Effects;
using System.Collections;
using TheForest.Buildings.Creation;
using TheForest.Tools;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public class ArrowDamageMod : ArrowDamage
    {
        private int BaseDmg =0;
        private int Repetitions;
        private Vector3 startposition;
        protected override void Start()
        {
            if (ModSettings.IsDedicated)
            {
                base.Start();
                return;
            }
            if (BaseDmg == 0)
                BaseDmg = damage;
           
            base.Start();
            float dmg = (BaseDmg + ModdedPlayer.instance.RangedDamageBonus) * ModdedPlayer.instance.RangedAMP * ModdedPlayer.instance.CritDamageBuff;
            if (crossbowBoltType)
            {
                dmg = dmg * ModdedPlayer.instance.CrossbowDamageMult;
            }
            else if (flintLockAmmoType)
            {
                dmg = dmg * ModdedPlayer.instance.BulletDamageMult;

            }
            else if (spearType)
            {
                dmg = dmg * ModdedPlayer.instance.SpearDamageMult;

            }
            else //if arrow
            {
                dmg = dmg * ModdedPlayer.instance.BowDamageMult;
            }

            damage = (int)dmg;

            if (SpellActions.SeekingArrow) startposition = transform.position;
            

        }

        private void Update()
        {
            if (SpellActions.SeekingArrow && Live)
            {
                if (Time.time - 20 > SpellActions.SeekingArrow_TimeStamp)
                {
                    CotfUtils.Log("Time out");
                    SpellActions.SeekingArrow = false;
                }
                if ((transform.position - SpellActions.SeekingArrow_Target.position).sqrMagnitude > 3)
                {
                    Vector3 vel = PhysicBody.velocity;
                    Vector3 targetvel = SpellActions.SeekingArrow_Target.position - transform.position;
                    targetvel.Normalize();
                    targetvel *= vel.magnitude;
                    PhysicBody.velocity = Vector3.RotateTowards(PhysicBody.velocity, targetvel, Time.deltaTime * 2.5f, 0.25f);
                }
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
                        if (Time.time - ModdedPlayer.instance.LastCrossfireTime > 20)
                        {
                            ModdedPlayer.instance.LastCrossfireTime = Time.time;
                            float damage = 55 + ModdedPlayer.instance.SpellDamageBonus * 1.25f;
                            damage = damage * ModdedPlayer.instance.SpellAMP;
                            Vector3 pos = Camera.main.transform.position + Camera.main.transform.right * 5;
                            Vector3 dir = transform.position - pos;
                            dir.Normalize();
                            if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
                            {
                                MagicArrow.Create(pos, dir, damage, ModReferences.ThisPlayerID, SpellActions.MagicArrowDuration, SpellActions.MagicArrowDoubleSlow, SpellActions.MagicArrowDmgDebuff);
                                if (BoltNetwork.isRunning)
                                {
                                    string s = "SC7;" + System.Math.Round(pos.x, 5) + ";" + System.Math.Round(pos.y, 5) + ";" + System.Math.Round(pos.z, 5) + ";" + System.Math.Round(dir.x, 5) + ";" + System.Math.Round(dir.y, 5) + ";" + System.Math.Round(dir.z, 5) + ";";
                                    s += damage + ";" + ModReferences.ThisPlayerID + ";" + SpellActions.MagicArrowDuration + ";";
                                    if (SpellActions.MagicArrowDoubleSlow)
                                    {
                                        s += "t;";
                                    }
                                    else
                                    {
                                        s += "f;";
                                    }

                                    if (SpellActions.MagicArrowDmgDebuff)
                                    {
                                        s += "t;";
                                    }
                                    else
                                    {
                                        s += "f;";
                                    }

                                    Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
                                }
                            }
                            else if (GameSetup.IsMpClient)
                            {
                                MagicArrow.CreateEffect(pos, dir, SpellActions.MagicArrowDmgDebuff, SpellActions.MagicArrowDuration);
                                string s = "SC7;" + System.Math.Round(pos.x, 5) + ";" + System.Math.Round(pos.y, 5) + ";" + System.Math.Round(pos.z, 5) + ";" + System.Math.Round(dir.x, 5) + ";" + System.Math.Round(dir.y, 5) + ";" + System.Math.Round(dir.z, 5) + ";";
                                s += damage + ";" + ModReferences.ThisPlayerID + ";" + SpellActions.MagicArrowDuration + ";";
                                if (SpellActions.MagicArrowDoubleSlow)
                                {
                                    s += "t;";
                                }
                                else
                                {
                                    s += "f;";
                                }

                                if (SpellActions.MagicArrowDmgDebuff)
                                {
                                    s += "t;";
                                }
                                else
                                {
                                    s += "f;";
                                }

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
                if (SpellActions.SeekingArrow_ChangeTargetOnHit)
                {
                    SpellActions.SeekingArrow = true;
                    SpellActions.SeekingArrow_Target.gameObject.SetActive(true);
                    SpellActions.SeekingArrow_Target.transform.parent = target.transform;
                    SpellActions.SeekingArrow_Target.transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    SpellActions.SeekingArrow_TimeStamp = Time.time;
                    SpellActions.SeekingArrow_ChangeTargetOnHit = false;
                }
                NewHitAi(target, flag || flag3, headDamage);

                ModdedPlayer.instance.DoAreaDamage(target.root, damage);
                ModdedPlayer.instance.OnHit();
                ModdedPlayer.instance.OnHit_Ranged();

                if (ModdedPlayer.instance.SpellAmpFireDmg)
                {
                    int myID = 1000 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
                    float dmg = 1 + ModdedPlayer.instance.SpellDamageBonus / 2;
                    dmg *= ModdedPlayer.instance.SpellAMP;
                    dmg *= ModdedPlayer.instance.FireAmp + 1;
                    if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
                    {
                        target.GetComponentInParent<EnemyProgression>()?.FireDebuff(myID, dmg, 4);
                    }
                    else
                    {
                        BoltEntity e = target.GetComponentInParent<BoltEntity>();
                        if (e != null)
                        {
                            Network.NetworkManager.SendLine("AH" + e.networkId.PackedValue + ";" + dmg + ";" + 4.5f + ";1", Network.NetworkManager.Target.OnlyServer);
                        }
                    }
                }
                if (ModdedPlayer.instance.RangedArmorReduction > 0 && target.gameObject.CompareTag("enemyCollide"))
                {
                    if (BoltNetwork.isClient)
                    {
                        BoltEntity be = target.GetComponentInParent<BoltEntity>();
                        if (be == null) { be = target.GetComponent<BoltEntity>(); }
                        if (be != null)
                        {
                            EnemyProgression.ReduceArmor(be, ModdedPlayer.instance.RangedArmorReduction);
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

        private void NewHitAi(Transform target, bool hitDelay = false, bool headDamage = false)
        {
            
            float dmgUnclamped = this.damage;
            if (SpellActions.SeekingArrow)
            {
                float dist = Vector3.Distance(target.position, startposition);
                dmgUnclamped *= 1 + dist * SpellActions.SeekingArrow_DamagePerDistance;
            }


            if (headDamage || (flintLockAmmoType && Random.value < 0.10)||(spearType&&Random.value < 0.03))
            {
                headDamage = true;
                dmgUnclamped *= ModdedPlayer.instance.HeadShotDamage;
                dmgUnclamped *= SpellActions.FocusOnHeadShot();
                if (SpellActions.SeekingArrow)
                {
                    dmgUnclamped *= 2;
                }
            }
            else
            {
                dmgUnclamped *= SpellActions.FocusOnBodyShot();
            }

            DamageMath.DamageClamp(dmgUnclamped, out int sendDamage, out Repetitions);


            if (this.PhysicBody)
            {
                this.PhysicBody.velocity = Vector3.zero;

                Invoke("SetVelocityZero", 0.03f);//invoke after 30 ms, which is about 2 frames.
            }

            if (this.spearType)
            {
                this.PhysicBody.isKinematic = false;
                this.PhysicBody.useGravity = true;
                this.disableLive();
                if (this.MyPickUp)
                {
                    this.MyPickUp.SetActive(true);
                }
            }
            if (target)
            {
                Vector3 vector = target.transform.root.GetChild(0).InverseTransformPoint(base.transform.position);
                float targetAngle = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
                int animalHitDirection = animalHealth.GetAnimalHitDirection(targetAngle);
                BoltEntity componentInParent = target.GetComponentInParent<BoltEntity>();
                if (!componentInParent)
                {
                    componentInParent= target.GetComponent<BoltEntity>();
                }

          
                
                if (BoltNetwork.isClient && componentInParent)
                {
                    ModdedPlayer.instance.OnHitEffectsClient(componentInParent, dmgUnclamped);
                    if (SpellActions.Focus && headDamage)
                    {
                        if (SpellActions.FocusBonusDmg == 0)
                        {
                            try
                            {

                            //slow enemy by 80%
                            string s = "AC" + componentInParent.networkId.PackedValue + ";" + SpellActions.FocusSlowAmount + ";"+SpellActions.FocusSlowDuration+";" + 90 + ";";
                            Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.OnlyServer);
                            }
                            catch (System.Exception e )
                            {

                                CotfUtils.Log(e.ToString());
                            }

                        }
                    }
                    else if (SpellActions.SeekingArrow)
                    {
                        try
                        {

                            //slow enemy by 80%
                            string s = "AC" + componentInParent.networkId.PackedValue + ";" + SpellActions.SeekingArrow_SlowAmount + ";" + SpellActions.SeekingArrow_SlowDuration + ";" + 91 + ";";
                            Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.OnlyServer);
                        }
                        catch (System.Exception e)
                        {

                            CotfUtils.Log(e.ToString());
                        }
                    }
                    if (hitDelay)
                    {
                        target.transform.SendMessageUpwards("getClientHitDirection", 6, SendMessageOptions.DontRequireReceiver);
                        target.transform.SendMessageUpwards("StartPrediction", SendMessageOptions.DontRequireReceiver);
                        BoltEntity component = this.parent.GetComponent<BoltEntity>();
                        PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
                        playerHitEnemy.Target = componentInParent;
                        playerHitEnemy.Weapon = component;
                        playerHitEnemy.getAttacker = 10;
                        if (target.gameObject.CompareTag("animalRoot"))
                        {
                            playerHitEnemy.getAttackDirection = animalHitDirection;
                        }
                        else
                        {
                            playerHitEnemy.getAttackDirection = 3;
                        }
                        playerHitEnemy.getAttackerType = 4;
                        playerHitEnemy.Hit = sendDamage;
                        for (int i = 0; i < Repetitions; i++)
                        {
                            playerHitEnemy.Send();
                        }
                    }
                    else
                    {
                        target.transform.SendMessageUpwards("getClientHitDirection", 6, SendMessageOptions.DontRequireReceiver);
                        target.transform.SendMessageUpwards("StartPrediction", SendMessageOptions.DontRequireReceiver);
                        PlayerHitEnemy playerHitEnemy2 = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
                        playerHitEnemy2.Target = componentInParent;
                        if (target.gameObject.CompareTag("animalRoot"))
                        {
                            playerHitEnemy2.getAttackDirection = animalHitDirection;
                        }
                        else
                        {
                            playerHitEnemy2.getAttackDirection = 3;
                        }
                        playerHitEnemy2.getAttackerType = 4;

                        playerHitEnemy2.Hit = sendDamage;
                        for (int i = 0; i < Repetitions; i++)
                        {
                            playerHitEnemy2.Send();
                        }
                    }
                }
                else
                {
                    if (target.gameObject.CompareTag("enemyRoot")||target.gameObject.CompareTag("enemyCollide"))
                    {
                        var ep = target.gameObject.GetComponentInParent<EnemyProgression>();
                        if (ep == null)
                        {
                            ep = target.gameObject.GetComponent<EnemyProgression>();
                            if (ep == null)
                            {
                                ep = target.gameObject.GetComponentInChildren<EnemyProgression>();
                            }
                        }
                        ModdedPlayer.instance.OnHitEffectsHost(ep, dmgUnclamped);
                        if (SpellActions.Focus && headDamage)
                        {
                            if (SpellActions.FocusBonusDmg == 0)
                            {
                                try
                                {

                                    //slow enemy by 80%
                                   ep.Slow(90,  SpellActions.FocusSlowAmount, SpellActions.FocusSlowDuration);
                                }
                                catch (System.Exception e)
                                {

                                    CotfUtils.Log(e.ToString());
                                }

                            }
                        }else if (SpellActions.SeekingArrow)
                        {
                            ep.Slow(91, SpellActions.SeekingArrow_SlowAmount, SpellActions.SeekingArrow_SlowDuration);

                        }
                    }
                    target.gameObject.SendMessageUpwards("getAttackDirection", 3, SendMessageOptions.DontRequireReceiver);
                    target.gameObject.SendMessageUpwards("getAttackerType", 4, SendMessageOptions.DontRequireReceiver);
                    GameObject closestPlayerFromPos = TheForest.Utils.Scene.SceneTracker.GetClosestPlayerFromPos(base.transform.position);
                    target.gameObject.SendMessageUpwards("getAttacker", closestPlayerFromPos, SendMessageOptions.DontRequireReceiver);
                    if (target.gameObject.CompareTag("lb_bird") || target.gameObject.CompareTag("animalRoot") || target.gameObject.CompareTag("enemyRoot") || target.gameObject.CompareTag("PlayerNet"))
                    {
                        if (target.gameObject.CompareTag("enemyRoot"))
                        {
                            EnemyHealth componentInChildren = target.GetComponentInChildren<EnemyHealth>();
                            if (componentInChildren)
                            {
                                componentInChildren.getAttackDirection(3);
                                componentInChildren.setSkinDamage(2);
                                mutantTargetSwitching componentInChildren2 = target.GetComponentInChildren<mutantTargetSwitching>();
                                if (componentInChildren2)
                                {
                                    componentInChildren2.getAttackerType(4);
                                    componentInChildren2.getAttacker(closestPlayerFromPos);
                                }
                                for (int i = 0; i < Repetitions; i++)
                                {
                                    componentInChildren.Hit(sendDamage);
                                }
                            }
                        }
                        else
                        {
                            if (target.gameObject.CompareTag("animalRoot"))
                            {
                                target.gameObject.SendMessage("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
                            }
                            for (int i = 0; i < Repetitions; i++)
                            {
                                target.gameObject.SendMessage("Hit", sendDamage, SendMessageOptions.DontRequireReceiver);
                            }
                            target.gameObject.SendMessage("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        if (target.gameObject.CompareTag("animalCollide"))
                        {
                            target.gameObject.SendMessageUpwards("ApplyAnimalSkinDamage", animalHitDirection, SendMessageOptions.DontRequireReceiver);
                        }
                        for (int i = 0; i < Repetitions; i++)
                        {
                            target.gameObject.SendMessageUpwards("Hit", sendDamage, SendMessageOptions.DontRequireReceiver);
                        }
                        target.gameObject.SendMessageUpwards("getSkinHitPosition", base.transform, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            if (this.MyPickUp)
            {
                this.MyPickUp.SetActive(true);
            }
            FMODCommon.PlayOneshotNetworked(this.hitAiEvent, base.transform, FMODCommon.NetworkRole.Any);
        }

        void SetVelocityZero()
        {
            PhysicBody.velocity = Vector3.zero;
        }
    }
}
