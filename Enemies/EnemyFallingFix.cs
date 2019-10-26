using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest.Enemies
{
    public class EnemyFallingFix : mutantAnimatorControl
    {
        protected override void Update()
        {
            if (this.setup.vis.animEnabled || (BoltNetwork.isRunning && BoltNetwork.isServer))
            {
                this.setup.search.worldPositionTr.position = this.rootTr.position;
            }
            if (this.fullBodyState.tagHash == this.deathHash && this.fullBodyState.normalizedTime > 0.95f && (this.animator.GetBool(TheForest.Utils.Scene.animHash.deathfinalBOOLHash) || this.animator.GetBool(TheForest.Utils.Scene.animHash.stealthDeathBoolHash)) && this.setup.pmBrain.ActiveStateName != "death" && this.setup.pmBrain.ActiveStateName != "deathFinish")
            {
                this.setup.pmBrain.SendEvent("toDeath");
                this.setup.pmCombat.SendEvent("toDeath");
            }
            if (this.ai.male || this.ai.female)
            {
                if (this.animator.GetBool("deathBOOL"))
                {
                    this.deathBoolCheck = true;
                }
                else
                {
                    this.deathBoolCheck = false;
                }
            }
            if (this.animator.enabled)
            {
                if (this.initBool)
                {
                    Vector3 position = this.thisTr.position;
                    if (this.worldUpdateCheck)
                    {
                        position = this.setup.search.worldPositionTr.position;
                    }
                    if (Terrain.activeTerrain)
                    {
                        this.terrainPosY = Terrain.activeTerrain.SampleHeight(position) + Terrain.activeTerrain.transform.position.y;
                    }
                    else
                    {
                        this.terrainPosY = this.rootTr.position.y;
                    }
                }
                if (this.ai.targetAngle > -45f && this.ai.targetAngle < 45f)
                {
                    if (!this.animator.GetBool(this.goForwardHash))
                    {
                        this.animator.SetBool(this.goForwardHash, true);
                    }
                    if (this.animator.GetBool(this.goLeftHash))
                    {
                        this.animator.SetBool(this.goLeftHash, false);
                    }
                    if (this.animator.GetBool(this.goRightHash))
                    {
                        this.animator.SetBool(this.goRightHash, false);
                    }
                    if (this.animator.GetBool(this.goBackHash))
                    {
                        this.animator.SetBool(this.goBackHash, false);
                    }
                }
                else if (this.ai.targetAngle < -45f && this.ai.targetAngle > -110f)
                {
                    if (this.animator.GetBool(this.goForwardHash))
                    {
                        this.animator.SetBool(this.goForwardHash, false);
                    }
                    if (!this.animator.GetBool(this.goLeftHash))
                    {
                        this.animator.SetBool(this.goLeftHash, true);
                    }
                    if (this.animator.GetBool(this.goRightHash))
                    {
                        this.animator.SetBool(this.goRightHash, false);
                    }
                    if (this.animator.GetBool(this.goBackHash))
                    {
                        this.animator.SetBool(this.goBackHash, false);
                    }
                }
                else if (this.ai.targetAngle > 45f && this.ai.targetAngle < 110f)
                {
                    if (this.animator.GetBool(this.goForwardHash))
                    {
                        this.animator.SetBool(this.goForwardHash, false);
                    }
                    if (this.animator.GetBool(this.goLeftHash))
                    {
                        this.animator.SetBool(this.goLeftHash, false);
                    }
                    if (!this.animator.GetBool(this.goRightHash))
                    {
                        this.animator.SetBool(this.goRightHash, true);
                    }
                    if (this.animator.GetBool(this.goBackHash))
                    {
                        this.animator.SetBool(this.goBackHash, false);
                    }
                }
                else if (this.ai.targetAngle > 110f || this.ai.targetAngle < -110f)
                {
                    if (this.animator.GetBool(this.goForwardHash))
                    {
                        this.animator.SetBool(this.goForwardHash, false);
                    }
                    if (this.animator.GetBool(this.goLeftHash))
                    {
                        this.animator.SetBool(this.goLeftHash, false);
                    }
                    if (this.animator.GetBool(this.goRightHash))
                    {
                        this.animator.SetBool(this.goRightHash, false);
                    }
                    if (!this.animator.GetBool(this.goBackHash))
                    {
                        this.animator.SetBool(this.goBackHash, true);
                    }
                }
                if (this.controller.enabled && Time.time - 1f > this.timeSinceEnabled && !this.animator.GetBool(TheForest.Utils.Scene.animHash.onCeilingBoolHash) && !this.animator.GetBool(TheForest.Utils.Scene.animHash.sleepBOOLHash) && !this.animator.GetBool(TheForest.Utils.Scene.animHash.treeBOOLHash) && !this.fsmWallClimb.Value && this.fullBodyState.tagHash != this.hashId.deathTag)
                {
                    if (!this.controller.isGrounded && this.controller.velocity.y < -20f)
                    {
                        if (!this.startedFall)
                        {
                            this.longFallTimer = Time.time;
                            this.animator.SetBool("jumpLandBool", false);
                            this.startedFall = true;
                        }
                        if (this.fullBodyState.tagHash != this.jumpFallHash && Time.time - 0.25f > this.longFallTimer)
                        {
                            this.animator.SetBool("fallingBool", true);
                        }
                    }
                    else if (this.controller.isGrounded && this.startedFall)
                    {
                        if (Time.time - 40 > this.longFallTimer)
                        {
                            this.animator.SetBool("hitGroundBool", true);
                            this.animator.SetBool("fallingBool", false);
                            this.startedFall = false;
                            this.setup.health.Health = 0;
                            this.setup.waterDetect.drowned = true;
                            this.setup.health.Die();
                        }
                        else
                        {
                            this.animator.SetBool("jumpLandBool", true);
                            this.animator.SetBool("fallingBool", false);
                            this.animator.SetBool("hitGroundBool", false);
                            this.startedFall = false;
                        }
                    }
                    else if (Time.time - 60f > this.longFallTimer && !this.controller.isGrounded && this.startedFall)
                    {
                        this.animator.SetBool("hitGroundBool", true);
                        this.animator.SetBool("fallingBool", false);
                        this.startedFall = false;
                        this.setup.health.Health = 0;
                        this.setup.waterDetect.drowned = true;
                        this.setup.health.Die();
                    }
                }
            }
            if (!this.animator.enabled && this.ai.doMove)
            {
                if (this.controller.enabled)
                {
                    this.controller.enabled = false;
                }
                Quaternion rotation = Quaternion.identity;
                this.wantedDir = this.ai.wantedDir;
                Vector3 vector = this.ai.wantedDir;
                if (vector != Vector3.zero && vector.sqrMagnitude > 0f)
                {
                    rotation = Quaternion.LookRotation(vector, Vector3.up);
                }
                this.setup.search.worldPositionTr.rotation = rotation;
                if (this.initBool && !this.fsmInCaveBool.Value)
                {
                    Vector3 position2 = this.setup.search.worldPositionTr.position;
                    this.setup.search.worldPositionTr.position = position2 + this.wantedDir * this.offScreenSpeed * Time.deltaTime;
                    if (Time.time > this.offScreenUpdateTimer)
                    {
                        this.rootTr.position = this.setup.search.worldPositionTr.position;
                        this.thisTr.rotation = this.setup.search.worldPositionTr.rotation;
                        this.offScreenUpdateTimer = Time.time + 1f;
                    }
                }
            }
        }
    }

}
