using System;
using TheForest.Utils;
using UnityEngine;


//More health
//More damage
//As if the worm was weak in the first place
namespace ChampionsOfForest.Enemies
{
    internal class WormHealthMod : wormHealth
    {
        public override void Hit(int damage)
        {
            damage = Mathf.Min(damage, 7);
            base.Hit(damage);
        }

    }

    internal class WormHiveControllerMod : wormHiveController
    {
        protected override void Start()
        {
            base.Start();
            maxRespawnAmount = 800000000;

        }
        protected override void Update()
        {
            this.activeWormWalkers.RemoveAll((GameObject o) => o == null);
            this.activeWormTrees.RemoveAll((GameObject o) => o == null);
            this.activeWormSingle.RemoveAll((GameObject o) => o == null);
            this.activeWormAngels.RemoveAll((GameObject o) => o == null);
            if (this.activeWormWalkers.Count > 0 || this.activeWormAngels.Count > 0 || this.activeWormTrees.Count > 0)
            {
                this.anyFormSpawned = true;
            }
            else
            {
                this.anyFormSpawned = false;
            }
            if (this.activeWormSingle.Count == 0 && this.init)
            {
                if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
                {
                    float multipier = 1;
                    switch (ModSettings.difficulty)
                    {
                        case ModSettings.Difficulty.Hard:
                            multipier = 2.3f;
                            break;
                        case ModSettings.Difficulty.Elite:
                            multipier = 9;
                            break;
                        case ModSettings.Difficulty.Master:
                            multipier = 20;
                            break;
                        case ModSettings.Difficulty.Challenge1:
                            multipier = 70;
                            break;
                        case ModSettings.Difficulty.Challenge2:
                            multipier = 200;
                            break;
                        case ModSettings.Difficulty.Challenge3:
                            multipier = 1000;
                            break;
                        case ModSettings.Difficulty.Challenge4:
                            multipier = 7000;
                            break;
                        case ModSettings.Difficulty.Challenge5:
                            multipier = 50000;
                            break;
                        default:
                            break;
                    }
                    float Exp = UnityEngine.Random.Range(300, 350);
                    if(GameSetup.IsMpServer)
                    Network.NetworkManager.SendLine("KX" + Convert.ToInt64(Exp*multipier / (Mathf.Max(1, 0.8f + ModReferences.Players.Count * 0.2f))) + ";", Network.NetworkManager.Target.Everyone);
                    else
                    {
                        ModdedPlayer.instance.AddKillExperience(Convert.ToInt64(Exp * multipier));
                    }
                    int itemCount = UnityEngine.Random.Range(5, 7);
                    for (int i = 0; i < itemCount; i++)
                    {
                        Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(Exp), LocalPlayer.Transform.position + Vector3.up * 2);
                    }
                }
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }

    internal class ExplodeMod : Explode
    {
        protected override void RunExplode()
        {
            Vector3 position = base.transform.position;
            Collider[] array = Physics.OverlapSphere(position, this.radius);
            int num = 0;
            int num2 = 0;
            foreach (Collider collider in array)
            {
                bool flag = collider.CompareTag("Fish");
                bool flag2 = !flag && (collider.CompareTag("Tree") || collider.CompareTag("MidTree"));
                if (flag)
                {
                    num++;
                }
                else if (flag2)
                {
                    num2++;
                }
                if (!this.wormDamageSetup || !collider.GetComponent<global::wormHitReceiver>())
                {
                    if (collider.transform.root == LocalPlayer.Transform.root)
                    {
                        int damage = 85;
                        switch (ModSettings.difficulty)
                        {
                            case ModSettings.Difficulty.Hard:
                                damage = 150;
                                break;
                            case ModSettings.Difficulty.Elite:
                                damage = 250;
                                break;
                            case ModSettings.Difficulty.Master:
                                damage = 425;
                                break;
                            case ModSettings.Difficulty.Challenge1:
                                damage = 50000;
                                break;
                            case ModSettings.Difficulty.Challenge2:
                                damage = 250000;
                                break;
                            case ModSettings.Difficulty.Challenge3:
                                damage = 1000000;
                                break;
                            case ModSettings.Difficulty.Challenge4:
                                damage = 5000000;
                                break;
                            case ModSettings.Difficulty.Challenge5:
                                damage = 15000000;
                                break;

                        }
                        LocalPlayer.Stats.Hit(damage, false);
                    }
                    if (BoltNetwork.isClient)
                    {
                        if (collider.CompareTag("playerHitDetect"))
                        {
                            float num3 = Vector3.Distance(base.transform.position, collider.transform.position);
                            collider.SendMessageUpwards("Explosion", num3, SendMessageOptions.DontRequireReceiver);
                            collider.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                        }
                        else if (collider.CompareTag("SmallTree") || flag2 || collider.CompareTag("BreakableWood") || collider.CompareTag("BreakableRock") || flag)
                        {
                            float num4 = Vector3.Distance(base.transform.position, collider.transform.position);
                            if (collider.CompareTag("lb_bird") || flag)
                            {
                                collider.gameObject.SendMessage("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                            }
                            else
                            {
                                collider.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
                            }
                            collider.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                        }
                        else if (collider.CompareTag("structure"))
                        {
                            float distance = Vector3.Distance(base.transform.position, collider.transform.position);
                            collider.gameObject.SendMessage("OnExplode", new global::Explode.Data
                            {
                                distance = distance,
                                explode = this
                            }, SendMessageOptions.DontRequireReceiver);
                        }
                        else if (collider.CompareTag("animalCollide"))
                        {
                            collider.gameObject.SendMessageUpwards("Explosion", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        if (collider.CompareTag("enemyCollide") || collider.CompareTag("animalCollide") || collider.CompareTag("lb_bird") || collider.CompareTag("playerHitDetect") || collider.CompareTag("structure") || collider.CompareTag("SLTier1") || collider.CompareTag("SLTier2") || collider.CompareTag("SLTier3") || flag2 || collider.CompareTag("SmallTree") || collider.CompareTag("BreakableWood") || collider.CompareTag("BreakableRock") || flag || collider.CompareTag("jumpObject") || collider.CompareTag("UnderfootWood") || collider.CompareTag("UnderfootRock") || collider.CompareTag("Target") || collider.CompareTag("dummyExplode"))
                        {
                            float num5 = Vector3.Distance(base.transform.position, collider.transform.position);
                            if (collider.CompareTag("lb_bird") || flag)
                            {
                                collider.gameObject.SendMessage("Explosion", num5, SendMessageOptions.DontRequireReceiver);
                            }
                            else
                            {
                                collider.gameObject.SendMessageUpwards("Explosion", num5, SendMessageOptions.DontRequireReceiver);
                            }
                            collider.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                            if (num5 < this.radius)
                            {
                                collider.gameObject.SendMessage("OnExplode", new global::Explode.Data
                                {
                                    distance = num5,
                                    explode = this
                                }, SendMessageOptions.DontRequireReceiver);
                            }
                        }
                        else if (collider.CompareTag("TripWireTrigger"))
                        {
                            collider.SendMessage("OnTripped", SendMessageOptions.DontRequireReceiver);
                        }
                        if (collider && collider.GetComponent<Rigidbody>())
                        {
                            if (!collider.gameObject.CompareTag("Tree"))
                            {
                                float num6 = 10000f;
                                if (collider.GetComponent<global::logChecker>())
                                {
                                    num6 *= 5.5f;
                                }
                                if (collider.CompareTag("Fish"))
                                {
                                    num6 = 1850f;
                                }
                                if (!collider.CompareTag("Player") && !collider.CompareTag("PlayerNet"))
                                {
                                    collider.GetComponent<Rigidbody>().AddExplosionForce(num6, position, this.radius, 3f, ForceMode.Force);
                                }
                            }
                        }
                    }
                }
            }
            if (num > 0)
            {
                TheForest.Tools.EventRegistry.Achievements.Publish(TheForest.Tools.TfEvent.Achievements.FishDynamited, num);
            }
            if (num2 > 0)
            {
                TheForest.Tools.EventRegistry.Achievements.Publish(TheForest.Tools.TfEvent.Achievements.TreeDynamited, num2);
            }
            if (TheForest.Utils.LocalPlayer.GameObject)
            {
                float num7 = Vector3.Distance(TheForest.Utils.LocalPlayer.Transform.position, base.transform.position);
                TheForest.Utils.LocalPlayer.GameObject.SendMessage("enableExplodeShake", num7, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
