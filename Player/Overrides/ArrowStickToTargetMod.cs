using System;
using System.Collections;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class XArrowStickToTargetMod : arrowStickToTarget
	{
		public override bool stickArrowToNearestBone(Transform arrow)
		{
			//Transform parent = arrow.parent;
			//TheForest.Items.Craft.WeaponStatUpgrade.Types types = (TheForest.Items.Craft.WeaponStatUpgrade.Types)(-1);
			//TheForest.Items.Inventory.ItemProperties properties = parent.GetComponent<global::arrowTrajectory>()._pickup.GetComponent<TheForest.Items.World.PickUp>()._properties;
			//if (properties != null && properties.ActiveBonus != (TheForest.Items.Craft.WeaponStatUpgrade.Types)(-1))
			//{
			//	types = properties.ActiveBonus;
			//}
			//int item = 0;
			//bool flag = false;
			//global::ArrowDamage component = arrow.GetComponent<global::ArrowDamage>();
			//if (component.crossbowBoltType)
			//{
			//	flag = true;
			//}
			//GameObject spawnedGO;
			//if (flag)
			//{
			//	spawnedGO = UnityEngine.Object.Instantiate<GameObject>(this.fakeBoltPickup, parent.transform.position, parent.transform.rotation);
			//}
			//else if (types != TheForest.Items.Craft.WeaponStatUpgrade.Types.BoneAmmo)
			//{
			//	if (types != TheForest.Items.Craft.WeaponStatUpgrade.Types.ModernAmmo)
			//	{
			//		spawnedGO = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowPickup, parent.transform.position, parent.transform.rotation);
			//		item = 0;
			//	}
			//	else
			//	{
			//		spawnedGO = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowModernPickup, parent.transform.position, parent.transform.rotation);
			//		item = 2;
			//	}
			//}
			//else
			//{
			//	spawnedGO = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowBonePickup, parent.transform.position, parent.transform.rotation);
			//	item = 1;
			//}

			//if (flag)
			//{
			//	item = 3;
			//}
			//Collider component2 = spawnedGO.GetComponent<Collider>();
			//if (component2)
			//{
			//	component2.enabled = false;
			//}
			//Transform tip = spawnedGO.GetComponent<global::fakeArrowSetup>().tip;
			//tip.position -= tip.forward * 1.4f;
			//int number = this.returnNearestJointMidPoint(tip);
			//if (this.singleJointMode)
			//{
			//	number = 0;
			//	Vector3 vector = (spawnedGO.transform.position - this.baseJoint.position).normalized;
			//	vector = this.baseJoint.position + vector * 0.2f;
			//	spawnedGO.transform.parent = this.baseJoint;
			//	spawnedGO.transform.position = vector;
			//	spawnedGO.transform.rotation = Quaternion.LookRotation(spawnedGO.transform.position - this.baseJoint.position) * Quaternion.Euler(-90f, 0f, 0f);
			//}
			//else
			//{
			//	Transform transform = this.stickToJoints[number];
			//	IEnumerator enumerator = this.stickToJoints[number].GetEnumerator();
			//	try
			//	{
			//		while (enumerator.MoveNext())
			//		{
			//			object obj = enumerator.Current;
			//			Transform transform2 = (Transform)obj;
			//			if (transform2.GetComponent<MonoBehaviour>() == null)
			//			{
			//				transform = transform2;
			//				break;
			//			}
			//		}
			//	}
			//	finally
			//	{
			//		IDisposable disp;
			//		if ((disp = (enumerator as IDisposable)) != null)
			//		{
			//			disp.Dispose();
			//		}
			//	}
			//	if (SpellActions.SeekingArrow_ChangeTargetOnHit)
			//	{
			//		SpellActions.SetSeekingArrowTarget(stickToJoints[number]);
			//	}

			//	Vector3 vector2 = (this.stickToJoints[number].position + transform.position) / 2f;
			//	Vector3 vector3 = (spawnedGO.transform.position - vector2).normalized;
			//	vector3 = vector2 + vector3 * 0.35f;
			//	spawnedGO.transform.parent = this.stickToJoints[number];
			//	spawnedGO.transform.position = vector3;
			//	spawnedGO.transform.rotation = Quaternion.LookRotation(spawnedGO.transform.position - vector2) * Quaternion.Euler(-90f, 0f, 0f);
			//}
			//bool result = false;
			//if (this.stickToJoints.Length > 0 && this.stickToJoints[number] && this.stickToJoints[number].GetComponent<global::headShotObject>())
			//{
			//	result = true;
			//}
			//if (numStuckArrows > 20)
			//{
			//	Destroy(spawnedGO, 1f);
			//	return result;
			//}

			//if (!this.stuckArrows.ContainsKey(spawnedGO.transform))
			//{
			//	this.stuckArrows.Add(spawnedGO.transform, number);
			//	this.stuckArrowsTypeList.Add(item);
			//	global::fakeArrowSetup fakeArrowSetupO = spawnedGO.GetComponent<global::fakeArrowSetup>();
			//	if (fakeArrowSetupO && BoltNetwork.isRunning)
			//	{
			//		fakeArrowSetupO.storedIndex = this.stuckArrows.Count - 1;
			//		fakeArrowSetupO.entityTarget = this.transform.root.GetComponent<BoltEntity>();
			//	}
			//	this.numStuckArrows++;
			//}
			//if (BoltNetwork.isRunning && numStuckArrows < 20)
			//{
			//	BoltEntity component4 = parent.GetComponent<BoltEntity>();
			//	if (component4.isAttached && component4.isOwner)
			//	{
			//		if (this.IsCreature && BoltNetwork.isServer)
			//		{
			//			this.StartCoroutine(this.SendArrowMPDelayed(spawnedGO, number, types, flag));
			//		}
			//		else
			//		{
			//			this.sendArrowMP(spawnedGO, number, types, flag);
			//		}
			//	}
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
			GameObject attached;
			if (flag)
			{
				attached = UnityEngine.Object.Instantiate<GameObject>(this.fakeBoltPickup, parent.transform.position, parent.transform.rotation);
				item = 3;
			}
			else if (types != TheForest.Items.Craft.WeaponStatUpgrade.Types.BoneAmmo)
			{
				if (types != TheForest.Items.Craft.WeaponStatUpgrade.Types.ModernAmmo)
				{
					attached = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowPickup, parent.transform.position, parent.transform.rotation);
					item = 0;
				}
				else
				{
					attached = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowModernPickup, parent.transform.position, parent.transform.rotation);
					item = 2;
				}
			}
			else
			{
				attached = UnityEngine.Object.Instantiate<GameObject>(this.fakeArrowBonePickup, parent.transform.position, parent.transform.rotation);
				item = 1;
			}
		
			Collider component2 = attached.GetComponent<Collider>();
			if (component2)
			{
				component2.enabled = false;
			}
			Transform tip = attached.GetComponent<global::fakeArrowSetup>().tip;
			int num = this.returnNearestJointMidPoint(tip);
			if (this.singleJointMode)
			{
				num = 0;
				Vector3 vector = (attached.transform.position - this.baseJoint.position).normalized;
				vector = this.baseJoint.position + vector * 0.2f;
				attached.transform.parent = this.baseJoint;
				attached.transform.position = vector;
				attached.transform.rotation = Quaternion.LookRotation(attached.transform.position - this.baseJoint.position) * Quaternion.Euler(-90f, 0f, 0f);
			}
			else
			{
				Transform tr = this.stickToJoints[num];
				foreach (Transform transform2 in this.stickToJoints[num])
				{
					if (transform2.GetComponent<MonoBehaviour>() == null)
					{
						tr = transform2;
						break;
					}
				}
				Vector3 vector2 = (this.stickToJoints[num].position + tr.position) / 2f;
				Vector3 vector3 = (attached.transform.position - vector2).normalized;
				vector3 = vector2 + vector3 * 0.35f;
				attached.transform.parent = this.stickToJoints[num];
				attached.transform.position = vector3;
				attached.transform.rotation = Quaternion.LookRotation(attached.transform.position - vector2) * Quaternion.Euler(-90f, 0f, 0f);
				if (SpellActions.SeekingArrow_ChangeTargetOnHit)
				{
					SpellActions.SetSeekingArrowTarget(this.stickToJoints[num]);
				}
			}
			bool isHeadshot = false;
			if (this.stickToJoints.Length > 0 && this.stickToJoints[num] && this.stickToJoints[num].GetComponent<global::headShotObject>())
			{
				isHeadshot = true;
			}
			if (numStuckArrows < 20)
			{

				if (!this.stuckArrows.ContainsKey(attached.transform))
				{
					this.stuckArrows.Add(attached.transform, num);
					this.stuckArrowsTypeList.Add(item);
					global::fakeArrowSetup component3 = attached.GetComponent<global::fakeArrowSetup>();
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
							base.StartCoroutine(this.SendArrowMPDelayed(attached, num, types, flag));
						}
						else
						{
							this.sendArrowMP(attached, num, types, flag);
						}
					}
				}
			}
			else
			{
					Destroy(attached, 1f);
			}
			return isHeadshot;
		}
		public bool checkHeadDamage(Transform arrow)
		{
			int num = this.returnNearestJointMidPoint(arrow);
			if (this.singleJointMode)
			{
				num = 0;
			}
			return (this.stickToJoints.Length > 0 && this.stickToJoints[num] && this.stickToJoints[num].GetComponent<global::headShotObject>());

		}
	}
}