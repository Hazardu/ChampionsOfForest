using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Bolt;
using TheForest.Utils;
using TheForest.Utils.Settings;
using TheForest.World;
using Scene = TheForest.Utils.Scene;
using ChampionsOfForest.Player;

namespace ChampionsOfForest.Enemies
{
    public class EnemyHitTriggerMod : enemyWeaponMelee
    {
        protected override void OnTriggerEnter(Collider other)
        {
         
                currState = animator.GetCurrentAnimatorStateInfo(0);
                nextState = animator.GetNextAnimatorStateInfo(0);
                if (currState.tagHash != damagedHash && currState.tagHash != staggerHash && currState.tagHash != hitStaggerHash && currState.tagHash != deathHash && nextState.tagHash != damagedHash && nextState.tagHash != staggerHash && nextState.tagHash != hitStaggerHash && nextState.tagHash != deathHash)
                {
                    if (other.gameObject.CompareTag("trapTrigger"))
                    {
                        other.gameObject.SendMessage("CutRope", SendMessageOptions.DontRequireReceiver);
                    }
                    if (!netPrefab && (bool)LocalPlayer.Animator && LocalPlayer.Animator.GetBool("deathBool"))
                    {
                        return;
                    }
                    if (other.gameObject.CompareTag("playerHitDetect") && mainTrigger)
                    {
                        if (!Scene.SceneTracker.hasAttackedPlayer)
                        {
                            Scene.SceneTracker.hasAttackedPlayer = true;
                            Scene.SceneTracker.Invoke("resetHasAttackedPlayer", (float)Random.Range(120, 240));
                        }
                        targetStats component = ((Component)other.transform.root).GetComponent<targetStats>();
                        if ((bool)component && component.targetDown)
                        {
                            return;
                        }
                        Animator componentInParent = other.gameObject.GetComponentInParent<Animator>();
                        Vector3 position = rootTr.position;
                        position.y += 3.3f;
                        Vector3 direction = other.transform.position - position;
                        if (!Physics.Raycast(position, direction, out hit, direction.magnitude, enemyHitMask, QueryTriggerInteraction.Ignore))
                        {
                            if (!creepy_male && !creepy && !creepy_baby && !creepy_fat && (bool)events && (bool)componentInParent)
                            {
                                bool flag = InFront(other.gameObject);
                                if ((!BoltNetwork.isServer || !netPrefab) && flag && events.parryBool && (componentInParent.GetNextAnimatorStateInfo(1).tagHash == blockHash || componentInParent.GetCurrentAnimatorStateInfo(1).tagHash == blockHash))
                                {
                                    int parryDir = events.parryDir;
                                    BoltSetReflectedShim.SetIntegerReflected(animator, "parryDirInt", parryDir);
                                    if (BoltNetwork.isClient && netPrefab)
                                    {
                                        BoltSetReflectedShim.SetTriggerReflected(animator, "ClientParryTrigger");
                                        hitPrediction.StartParryPrediction();
                                        parryEnemy parryEnemy = parryEnemy.Create(GlobalTargets.OnlyServer);
                                        parryEnemy.Target = ((Component)base.transform.root).GetComponent<BoltEntity>();
                                        parryEnemy.Send();
                                        FMODCommon.PlayOneshot(parryEvent, base.transform);
                                    }
                                    else
                                    {
                                        BoltSetReflectedShim.SetTriggerReflected(animator, "parryTrigger");
                                    }
                                    events.StartCoroutine("disableAllWeapons");
                                    playerHitReactions componentInParent2 = other.gameObject.GetComponentInParent<playerHitReactions>();
                                    if ((Object)componentInParent2 != (Object)null)
                                    {
                                        componentInParent2.enableParryState();
                                    }
                                    FMODCommon.PlayOneshotNetworked(parryEvent, base.transform, FMODCommon.NetworkRole.Server);
                                    events.parryBool = false;
                                    return;
                                }
                            }
                            if ((bool)events)
                            {
                                events.parryBool = false;
                            }
                            other.transform.root.SendMessage("getHitDirection", rootTr.position, SendMessageOptions.DontRequireReceiver);
                            int num = 0;
                            if (maleSkinny || femaleSkinny)
                            {
                                if (pale)
                                {
                                    num = ((!skinned) ? Mathf.FloorToInt(10f * GameSettings.Ai.skinnyDamageRatio) : Mathf.FloorToInt(10f * GameSettings.Ai.skinnyDamageRatio * GameSettings.Ai.skinMaskDamageRatio));
                                }
                                else
                                {
                                    num = Mathf.FloorToInt(13f * GameSettings.Ai.skinnyDamageRatio);
                                    if (maleSkinny && props.regularStick.activeSelf && events.leftHandWeapon)
                                    {
                                        num = Mathf.FloorToInt((float)num * 1.35f);
                                    }
                                }
                            }
                            else if (male && pale)
                            {
                                num = ((!skinned) ? Mathf.FloorToInt(22f * GameSettings.Ai.largePaleDamageRatio) : Mathf.FloorToInt(22f * GameSettings.Ai.largePaleDamageRatio * GameSettings.Ai.skinMaskDamageRatio));
                            }
                            else if (male && !firemanMain)
                            {
                                num = ((!painted) ? Mathf.FloorToInt(20f * GameSettings.Ai.regularMaleDamageRatio) : Mathf.FloorToInt(20f * GameSettings.Ai.regularMaleDamageRatio * GameSettings.Ai.paintedDamageRatio));
                            }
                            else if (female)
                            {
                                num = Mathf.FloorToInt(17f * GameSettings.Ai.regularFemaleDamageRatio);
                            }
                            else if (creepy)
                            {
                                num = ((!pale) ? Mathf.FloorToInt(28f * GameSettings.Ai.creepyDamageRatio) : Mathf.FloorToInt(35f * GameSettings.Ai.creepyDamageRatio));
                            }
                            else if (creepy_male)
                            {
                                num = ((!pale) ? Mathf.FloorToInt(30f * GameSettings.Ai.creepyDamageRatio) : Mathf.FloorToInt(40f * GameSettings.Ai.creepyDamageRatio));
                            }
                            else if (creepy_baby)
                            {
                                num = Mathf.FloorToInt(26f * GameSettings.Ai.creepyBabyDamageRatio);
                            }
                            else if (firemanMain)
                            {
                                num = Mathf.FloorToInt(12f * GameSettings.Ai.regularMaleDamageRatio);
                                if ((bool)events && !enemyAtStructure && !events.noFireAttack)
                                {
                                    if (BoltNetwork.isRunning && netPrefab)
                                    {
                                        other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
                                    }
                                    else
                                    {
                                        other.gameObject.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
                                    }
                                }
                            }
                            if (!female && male)
                            {
                                if (holdingRegularWeapon() && events.leftHandWeapon)
                                {
                                    num += 7;
                                }
                                else if (holdingAdvancedWeapon() && events.leftHandWeapon)
                                {
                                    num += 15;
                                }
                            }
                            if ((bool)setup && setup.health.poisoned)
                            {
                                num = Mathf.FloorToInt((float)num / 1.6f);
                            }
                        EnemyHealthMod healthMod = (EnemyHealthMod)setup.health;
                        //POISON ATTACKS
                     

                        num = Mathf.RoundToInt(num * healthMod.progression.DamageAmp);

                        if (healthMod.progression.abilities.Contains(EnemyProgression.Abilities.Poisonous))
                        {
                            BuffDB.AddBuff(3, 32, num / 4, 3);
                        }

                        PlayerStats component2 = ((Component)other.transform.root).GetComponent<PlayerStats>();
                            if (male || female || creepy_male || creepy_fat || creepy || creepy_baby)
                            {
                                netId component3 = ((Component)other.transform).GetComponent<netId>();
                                if (BoltNetwork.isServer && (bool)component3)
                                {
                                    other.transform.root.SendMessage("StartPrediction", SendMessageOptions.DontRequireReceiver);
                                    return;
                                }
                                if (BoltNetwork.isClient && netPrefab && !(bool)component3)
                                {
                                    other.transform.root.SendMessage("setCurrentAttacker", this, SendMessageOptions.DontRequireReceiver);
                                    other.transform.root.SendMessage("hitFromEnemy", num, SendMessageOptions.DontRequireReceiver);
                                    other.transform.root.SendMessage("StartPrediction", SendMessageOptions.DontRequireReceiver);
                                }
                                else if (BoltNetwork.isServer)
                                {
                                    if (!(bool)component3)
                                    {
                                        other.transform.root.SendMessage("setCurrentAttacker", this, SendMessageOptions.DontRequireReceiver);
                                        other.transform.root.SendMessage("hitFromEnemy", num, SendMessageOptions.DontRequireReceiver);
                                    }
                                }
                                else if (!BoltNetwork.isRunning && (bool)component2)
                                {
                                    component2.setCurrentAttacker(this);
                                    component2.hitFromEnemy(num);
                                }
                            }
                            else if (!netPrefab && (bool)component2)
                            {
                                component2.setCurrentAttacker(this);
                                component2.hitFromEnemy(num);
                            }
                            goto IL_092f;
                        }
                        return;
                    }
                    goto IL_092f;
                }
                return;
            IL_092f:
                if (other.gameObject.CompareTag("enemyCollide") && mainTrigger && (bool)bodyCollider && !enemyAtStructure)
                {
                    setupAttackerType();
                    if ((Object)other.gameObject != (Object)bodyCollider)
                    {
                        other.transform.SendMessageUpwards("getAttackDirection", Random.Range(0, 2), SendMessageOptions.DontRequireReceiver);
                        other.transform.SendMessageUpwards("getCombo", Random.Range(1, 4), SendMessageOptions.DontRequireReceiver);
                        other.transform.SendMessage("getAttackerType", attackerType, SendMessageOptions.DontRequireReceiver);
                        other.transform.SendMessage("getAttacker", rootTr.gameObject, SendMessageOptions.DontRequireReceiver);
                        other.transform.SendMessageUpwards("Hit", 6, SendMessageOptions.DontRequireReceiver);
                        FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
                    }
                }
                if (other.gameObject.CompareTag("BreakableWood") || (other.gameObject.CompareTag("BreakableRock") && mainTrigger))
                {
                    other.transform.SendMessage("Hit", 50, SendMessageOptions.DontRequireReceiver);
                    other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, 50f), SendMessageOptions.DontRequireReceiver);
                    FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
                }
                if (other.gameObject.CompareTag("SmallTree") && !mainTrigger)
                {
                    other.SendMessage("Hit", 2, SendMessageOptions.DontRequireReceiver);
                }
                if (other.gameObject.CompareTag("Fire") && mainTrigger && firemanMain && !events.noFireAttack)
                {
                    other.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                }
                if (other.gameObject.CompareTag("Tree") && mainTrigger && creepy_male)
                {
                    other.SendMessage("Explosion", 5f, SendMessageOptions.DontRequireReceiver);
                    FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
                }
                if (!other.gameObject.CompareTag("structure") && !other.gameObject.CompareTag("SLTier1") && !other.gameObject.CompareTag("SLTier2") && !other.gameObject.CompareTag("SLTier3") && !other.gameObject.CompareTag("jumpObject") && !other.gameObject.CompareTag("UnderfootWood"))
                {
                    return;
                }
                if (!mainTrigger)
                {
                    return;
                }
                getStructureStrength component4 = other.gameObject.GetComponent<getStructureStrength>();
                bool flag2 = false;
                if ((Object)component4 == (Object)null)
                {
                    flag2 = true;
                }
                enemyAtStructure = true;
                int num2 = 0;
                if (!creepy_male && !creepy && !creepy_fat && !creepy_baby)
                {
                    if (!flag2)
                    {
                        num2 = ((maleSkinny || femaleSkinny) ? ((component4._strength == getStructureStrength.strength.weak) ? Mathf.FloorToInt(8f * GameSettings.Ai.regularStructureDamageRatio) : 0) : ((pale || painted || skinned) ? ((component4._strength != getStructureStrength.strength.veryStrong) ? Mathf.FloorToInt(16f * GameSettings.Ai.regularStructureDamageRatio) : 0) : ((component4._strength != getStructureStrength.strength.veryStrong) ? Mathf.FloorToInt(12f * GameSettings.Ai.regularStructureDamageRatio) : 0)));
                        goto IL_0d63;
                    }
                    return;
                }
                num2 = ((!creepy_baby) ? Mathf.FloorToInt(30f * GameSettings.Ai.creepyStructureDamageRatio) : Mathf.FloorToInt(10f * GameSettings.Ai.creepyStructureDamageRatio));
                goto IL_0d63;
            IL_0d63:
                if ((bool)setup && setup.health.poisoned)
                {
                    num2 /= 2;
                }
                other.SendMessage("Hit", num2, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("LocalizedHit", new LocalizedHitData(base.transform.position, (float)num2), SendMessageOptions.DontRequireReceiver);
                FMODCommon.PlayOneshotNetworked(weaponHitEvent, base.transform, FMODCommon.NetworkRole.Server);
            

        }
    }
}
