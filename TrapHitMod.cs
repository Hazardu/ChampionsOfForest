using PathologicalGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TheForest.Utils;
using TheForest;

namespace ChampionsOfForest
{
   public class TrapHitMod :trapHit
    {
        protected override void registerTrapHit(Collider other)
        {
            if (other.GetComponent<global::creepyHitReactions>() && !this.disable && this.trigger.largeSpike)
            {
                this.sendCreepyDamageFromRoot(other);
                if (!this.disable)
                {
                    base.Invoke("disableCollision", 0.8f);
                }
                return;
            }
            if (other.gameObject.CompareTag("enemyCollide") || other.gameObject.CompareTag("playerHitDetect") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("animalCollide"))
            {
                if (other.gameObject.CompareTag("enemyCollide"))
                {
                    global::mutantHitReceiver component = other.transform.GetComponent<global::mutantHitReceiver>();
                    if (component && component.inNooseTrap)
                    {
                        return;
                    }
                    global::explodeDummy component2 = other.GetComponent<global::explodeDummy>();
                    if (component2)
                    {
                        return;
                    }
                }
                if (!this.disable)
                {
                    if (this.trigger.largeSpike)
                    {
                        other.gameObject.SendMessageUpwards("setTrapLookat", base.transform.root.gameObject, SendMessageOptions.DontRequireReceiver);
                        base.gameObject.SendMessage("addTrappedMutant", other.transform.root.gameObject, SendMessageOptions.DontRequireReceiver);
                        other.gameObject.SendMessageUpwards("setCurrentTrap", this.trigger.gameObject, SendMessageOptions.DontRequireReceiver);
                        this.sendCreepyDamage(other);
                        if (other.gameObject.CompareTag("Player"))
                        {
                            other.gameObject.SendMessage("HitFromTrap", 150, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else if (this.trigger.largeDeadfall)
                    {
                        if (other.gameObject.CompareTag("playerHitDetect"))
                        {
                            other.gameObject.SendMessageUpwards("HitFromTrap", 150, SendMessageOptions.DontRequireReceiver);
                        }
                        if (other.gameObject.CompareTag("enemyCollide"))
                        {
                            this.sendCreepyDamage(other);
                        }
                    }
                    if (this.trigger.largeSwingingRock)
                    {
                        if (this.rb.velocity.magnitude > 11f)
                        {
                            other.gameObject.SendMessageUpwards("Hit", 500, SendMessageOptions.DontRequireReceiver);
                            //other.gameObject.SendMessageUpwards("Explosion", -1, SendMessageOptions.DontRequireReceiver);
                            //other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
                            //other.gameObject.SendMessageUpwards("DieTrap", this.trapType, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        other.gameObject.SendMessageUpwards("Hit", 500, SendMessageOptions.DontRequireReceiver);
                        //other.gameObject.SendMessageUpwards("DieTrap", this.trapType, SendMessageOptions.DontRequireReceiver);
                        //if (other.gameObject.CompareTag("enemyCollide") && this.trigger.largeSpike)
                        //{
                        //    Vector3 vector = this.PutOnSpikes(other.transform.root.gameObject);
                        //    other.gameObject.SendMessageUpwards("setPositionAtSpikes", vector, SendMessageOptions.DontRequireReceiver);
                        //}
                    }
                    if (!this.disable && !this.trigger.largeSwingingRock)
                    {
                        base.Invoke("disableCollision", 0.8f);
                    }
                }
            }
        }
    }

    public class TrapTriggerMod : trapTrigger
    {
        public override void TriggerLargeTrap(Collider other)
        {
            if (this.MpClientCheck && other == null && this.largeNoose)
            {
                this.switchNooseRope();
                base.Invoke("EnableCutTrigger", 1.5f);
                this.animator.enabled = true;
                this.animator.SetIntegerReflected("direction", 0);
                this.animator.SetBoolReflected("trapSpringBool", true);
            }
            else if (this.MpHostCheck && this.largeNoose)
            {
                this.SprungTrap();
            }
            if (this.sprung)
            {
                return;
            }
            this.CheckAnimReference();
            bool flag = !BoltNetwork.isRunning || this.MpHostCheck;
            if (this.hitbox && !this.hitbox.activeSelf)
            {
                this.hitbox.SetActive(true);
            }
            if (this.largeSwingingRock)
            {
                this.cutRope.SetActive(false);
                this.swingingRock.SendMessage("enableSwingingRock", false);
                base.Invoke("enableTrapReset", 3f);
            }
            if (this.largeDeadfall)
            {
                this.anim.GetComponent<Animation>().Play("trapFall");
                this.spikeTrapBlockerGo.SetActive(true);
                base.Invoke("enableTrapReset", 3f);
            }
            if (this.largeSpike)
            {
                this.anim.GetComponent<Animation>().Play("trapSpring");
                this.spikeTrapBlockerGo.SetActive(true);
                if (flag && other)
                {
                    other.gameObject.SendMessageUpwards("enableController", SendMessageOptions.DontRequireReceiver);
                    if (other.gameObject.CompareTag("enemyCollide"))
                    {
                        this.mutantSetup = other.transform.root.GetComponentInChildren<global::mutantScriptSetup>();
                        if (this.mutantSetup && !this.mutantSetup.ai.creepy && !this.mutantSetup.ai.creepy_male && !this.mutantSetup.ai.creepy_fat && !this.mutantSetup.ai.creepy_baby)
                        {
                            other.gameObject.SendMessageUpwards("setCurrentTrap", base.gameObject, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                base.Invoke("enableTrapReset", 3f);
            }
            if (this.largeNoose)
            {
                if (flag && other)
                {
                    if (other.gameObject.GetComponentInParent<EnemyProgression>()?.abilities.Count>0) return;//noose traps are too strong against elites
                    //so they no longer work on them 

                    global::mutantHitReceiver component = other.transform.GetComponent<global::mutantHitReceiver>();
                    if (other.gameObject.CompareTag("enemyCollide"))
                    {
                        if (component)
                        {
                            component.inNooseTrap = true;
                            component.DisableWeaponHits(2f);
                        }
                        this.mutantSetup = other.transform.root.GetComponentInChildren<global::mutantScriptSetup>();
                    }
                    this.trappedMutants.Clear();
                    this.trappedMutantMasks.Clear();
                    GameObject gameObject = other.transform.root.gameObject;
                    this.addTrappedMutant(gameObject);
                    global::mutantScriptSetup componentInChildren = other.transform.root.GetComponentInChildren<global::mutantScriptSetup>();
                    if (componentInChildren && componentInChildren.ai && componentInChildren.ai.pale)
                    {
                        this.FixPalePosition(componentInChildren, true);
                    }
                    if (base.transform.InverseTransformPoint(other.transform.position).x > 0f)
                    {
                        this.animator.SetIntegerReflected("direction", 0);
                    }
                    else
                    {
                        this.animator.SetIntegerReflected("direction", 1);
                    }
                    other.gameObject.SendMessageUpwards("setFootPivot", this.nooseFootPivot, SendMessageOptions.DontRequireReceiver);
                    this.animator.enabled = true;
                    this.animator.SetBoolReflected("trapSpringBool", true);
                    if (this.mutantSetup)
                    {
                        if (!this.mutantSetup.ai.creepy && !this.mutantSetup.ai.creepy_male && !this.mutantSetup.ai.creepy_fat && !this.mutantSetup.ai.creepy_baby)
                        {
                            other.gameObject.SendMessageUpwards("setInNooseTrap", this.noosePivot);
                        }
                        other.gameObject.SendMessageUpwards("setCurrentTrap", base.gameObject);
                    }
                }
                if (other)
                {
                    other.gameObject.SendMessageUpwards("DieTrap", 2, SendMessageOptions.DontRequireReceiver);
                }
                this.switchNooseRope();
                base.Invoke("EnableCutTrigger", 1.5f);
                if (base.entity.IsOwner() && this.largeNoose)
                {
                    base.entity.GetState<ITrapLargeState>().CanCutDown = true;
                    base.entity.GetState<ITrapLargeState>().CanReset = false;
                }
                if (other && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerNet")))
                {
                    base.Invoke("enableTrapReset", 2f);
                }
            }
            if (this.hitbox)
            {
                base.Invoke("disableHitbox", 1.5f);
            }
            base.transform.GetComponent<Collider>().enabled = false;
            this.SprungTag = true;
            this.PlayTriggerSFX();
        }
    }
}
