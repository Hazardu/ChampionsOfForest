using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public class XArrowStickToTargetMod : arrowStickToTarget
    {
        public override bool stickArrowToNearestBone(Transform arrow)
        {

            Transform parent = arrow.parent;
            TheForest.Items.Craft.WeaponStatUpgrade.Types types = (TheForest.Items.Craft.WeaponStatUpgrade.Types)(-1);
            TheForest.Items.Inventory.ItemProperties properties = parent.GetComponent<global::arrowTrajectory>()._pickup.GetComponent<TheForest.Items.World.PickUp>()._properties;
            if (properties != null && properties.ActiveBonus != (TheForest.Items.Craft.WeaponStatUpgrade.Types)(-1))
            {
                types = properties.ActiveBonus;
            }
            int item = 0;
            bool flag = false;
            global::ArrowDamage component = arrow.GetComponent<global::ArrowDamage>();
            if (component.crossbowBoltType)
            {
                flag = true;
            }
            GameObject gameObject;
            if (flag)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fakeBoltPickup, parent.transform.position, parent.transform.rotation);
            }
            else if (types != TheForest.Items.Craft.WeaponStatUpgrade.Types.BoneAmmo)
            {
                if (types != TheForest.Items.Craft.WeaponStatUpgrade.Types.ModernAmmo)
                {
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowPickup, parent.transform.position, parent.transform.rotation);
                    item = 0;
                }
                else
                {
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowModernPickup, parent.transform.position, parent.transform.rotation);
                    item = 2;
                }
            }
            else
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowBonePickup, parent.transform.position, parent.transform.rotation);
                item = 1;
            }
            if(ModdedPlayer.instance.ReusabilityChance > 0.35f || (int)ModSettings.difficulty > 2)
            {
                 float multishotMult =Mathf.Min(14.4f, ModdedPlayer.instance.MultishotCount/0.9f);
                Destroy(gameObject, 15 - multishotMult);

            }
            if (flag)
            {
                item = 3;
            }
            Collider component2 = gameObject.GetComponent<Collider>();
            if (component2)
            {
                component2.enabled = false;
            }
            Transform tip = gameObject.GetComponent<global::fakeArrowSetup>().tip;
            tip.position -= tip.forward*1.4f;
            int number = this.returnNearestJointMidPoint(tip);
            if (this.singleJointMode)
            {
                number = 0;
                Vector3 vector = (gameObject.transform.position - this.baseJoint.position).normalized;
                vector = this.baseJoint.position + vector * 0.2f;
                gameObject.transform.parent = this.baseJoint;
                gameObject.transform.position = vector;
                gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - this.baseJoint.position) * Quaternion.Euler(-90f, 0f, 0f);
            }
            else
            {
                Transform transform = this.stickToJoints[number];
                IEnumerator enumerator = this.stickToJoints[number].GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object obj = enumerator.Current;
                        Transform transform2 = (Transform)obj;
                        if (transform2.GetComponent<MonoBehaviour>()==null)
                        {
                            transform = transform2;
                            break;
                        }
                    }
                }
                finally
                {
                    IDisposable disp;
                    if ((disp = (enumerator as IDisposable)) != null)
                    {
                        disp.Dispose();
                    }
                }
                if (SpellActions.SeekingArrow_ChangeTargetOnHit)
                {
                    SpellActions.SetSeekingArrowTarget(stickToJoints[number]);
                }




                Vector3 vector2 = (this.stickToJoints[number].position + transform.position) / 2f;
                Vector3 vector3 = (gameObject.transform.position - vector2).normalized;
                vector3 = vector2 + vector3 * 0.35f;
                gameObject.transform.parent = this.stickToJoints[number];
                gameObject.transform.position = vector3;
                gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - vector2) * Quaternion.Euler(-90f, 0f, 0f);
            }
            bool result = false;
            if (this.stickToJoints.Length > 0 && this.stickToJoints[number] && this.stickToJoints[number].GetComponent<global::headShotObject>())
            {
                result = true;
            }
            if (!this.stuckArrows.ContainsKey(gameObject.transform))
            {
                this.stuckArrows.Add(gameObject.transform, number);
                this.stuckArrowsTypeList.Add(item);
                global::fakeArrowSetup component3 = gameObject.GetComponent<global::fakeArrowSetup>();
                if (component3 && BoltNetwork.isRunning)
                {
                    component3.storedIndex = this.stuckArrows.Count - 1;
                    component3.entityTarget = base.transform.root.GetComponent<BoltEntity>();
                }
                this.numStuckArrows++;
            }
            if (BoltNetwork.isRunning)
            {
                BoltEntity component4 = parent.GetComponent<BoltEntity>();
                if (component4.isAttached && component4.isOwner)
                {
                    if (this.IsCreature && BoltNetwork.isServer)
                    {
                        base.StartCoroutine(this.SendArrowMPDelayed(gameObject, number, types, flag));
                    }
                    else
                    {
                        this.sendArrowMP(gameObject, number, types, flag);
                    }
                }
            }
            return result;
        }
    }
}
