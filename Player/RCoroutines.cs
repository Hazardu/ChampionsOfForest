using System.Collections;

using TheForest.Items.Inventory;
using TheForest.Items.World;
using TheForest.Utils;

using UnityEngine;
using UnityEngine.Events;

namespace ChampionsOfForest.Player
{
	public class RCoroutines : MonoBehaviour
	{
		public static RCoroutines i;

		private void Start()
		{
			i = this;
		}

		//public IEnumerator ProjectileMultihit(Transform target,float dmg,bool headshot, UnityAction dmgScript, int invcationCount )
		//{
		//	for (int i = 0; i < invcationCount; i++)
		//	{
		//		yield return null;
		//		dmgScript.Invoke();
		//		ModdedPlayer.instance.DoAreaDamage(target.root, dmg);
		//		ModdedPlayer.instance.OnHit();
		//		ModdedPlayer.instance.OnHit_Ranged(target);
		//	}
		//}

		public IEnumerator AsyncSendRandomItemDrops(int count, EnemyProgression.Enemy type, long bounty,ModSettings.Difficulty difficulty, Vector3 position)
		{
			for (int i = 0; i < count; i++)
			{
				yield return null;
				yield return null;
				Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(bounty, type,difficulty), position + Vector3.up * (2f + i / 4) + Random.Range(-1, 1) * Vector3.forward + Random.Range(-1, 1) * Vector3.right);
			}
		}

		public IEnumerator AsyncCrossbowFire(int _ammoId, GameObject _ammoSpawnPosGo, GameObject _boltProjectile, crossbowController cc)
		{
			int repeats = ModdedPlayer.RangedRepetitions();
			Vector3 updir = _ammoSpawnPosGo.transform.up;
			Vector3 right = _ammoSpawnPosGo.transform.right;
			Vector3 positionOriginal = _ammoSpawnPosGo.transform.position;
			Quaternion rotation = _ammoSpawnPosGo.transform.rotation;
			Vector3 forceUp = Vector3.zero;
			bool noconsume = false;
			if (ModdedPlayer.Stats.perk_projectileNoConsumeChance > 0 && Random.value < ModdedPlayer.Stats.perk_projectileNoConsumeChance)
			{
				noconsume = true;
			}
			for (int i = 0; i < repeats; i++)
			{
				if (noconsume || LocalPlayer.Inventory.RemoveItem(_ammoId, 1, true, true))
				{
					Vector3 position = positionOriginal;
					if (i > 0)
					{
						position += 0.5f * updir * Mathf.Sin((i / 2f) * Mathf.PI) / 3;
						position += 0.4f * right * (((i) % 3) - 2);
					}

					GameObject gameObject = Object.Instantiate(_boltProjectile, position, rotation);
					gameObject.transform.localScale *= ModdedPlayer.Stats.projectileSize;
					gameObject.layer = 19;
					Physics.IgnoreLayerCollision(19, 19, true);
					if (i > 0)
					{
						float dmgPen = Mathf.Pow(ModdedPlayer.Stats.perk_multishotDamagePennalty, i);
						gameObject.SendMessage("ModifyStartingDamage", dmgPen, SendMessageOptions.DontRequireReceiver);
					}
					Rigidbody component = gameObject.GetComponent<Rigidbody>();
					component.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
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
						if (gameObject.activeInHierarchy)
						{
							cc.SendMessage("PublicEnablePickupTrigger", componentInChildren.gameObject);
						}
					}
					if (i == 0)
						forceUp = gameObject.transform.up;
					Vector3 force = 22000f * ModdedPlayer.Stats.projectileSpeed * (0.016666f / Time.fixedDeltaTime) * forceUp;
					if (ModdedPlayer.Stats.spell_bia_AccumulatedDamage > 0)
					{
						gameObject.transform.localScale *= 2f;
						force *= 2f;
						component.useGravity = false;
						if (ModReferences.bloodInfusedMaterial == null)
						{
							ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
							{
								EmissionColor = new Color(0.6f, 0.1f, 0),
								renderMode = BuilderCore.BuildingData.RenderMode.Fade,
								MainColor = Color.red,
								Metalic = 1f,
								Smoothness = 0.9f,
							});
						}
						//gameObject.GetComponent<Renderer>().material = bloodInfusedMaterial;
						var trail = gameObject.AddComponent<TrailRenderer>();
						trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
						trail.material = ModReferences.bloodInfusedMaterial;
						trail.widthMultiplier = 0.55f;
						trail.time = 1.25f;
					}
					component.AddForce(force);
				}
				else
				{
					break;
				}
				if (i % 2 == 1)
					yield return null;
			}
		}

		public IEnumerator AsyncRangedFire(PlayerInventory inv, float _weaponChargeStartTime, InventoryItemView inventoryItemView, InventoryItemView inventoryItemView2, bool noconsume)
		{
			TheForest.Items.Item itemCache = inventoryItemView.ItemCache;
			bool flag = itemCache._maxAmount < 0;
			int repeats = ModdedPlayer.RangedRepetitions();
			Vector3 forceUp = inventoryItemView2._held.transform.up;
			Vector3 right = inventoryItemView2._held.transform.right;
			Vector3 up = inventoryItemView2._held.transform.up;
			Vector3 originalPos = inventoryItemView2._held.transform.position;
			FakeParent component = inventoryItemView2._held.GetComponent<FakeParent>();
			Quaternion rotation = inventoryItemView2._held.transform.rotation;
			TheForest.Items.Item itemCache2 = inventoryItemView2.ItemCache;
			for (int i = 0; i < repeats; i++)
			{
				if (noconsume)
				{
					Vector3 pos = originalPos;
					if (i > 0)
					{
						// pos += 0.5f * up * (i + 1) / 3;
						pos += 0.5f * right * (((i - 1) % 3) - 1);
					}

					GameObject projectileObject = (!(bool)component || component.gameObject.activeSelf) ? Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, pos, rotation) : Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, pos, rotation);

					projectileObject.transform.localScale *= ModdedPlayer.Stats.projectileSize;
					

					try
					{
						projectileObject.transform.GetChild(0).gameObject.layer = 19;
					}
					catch (System.Exception)
					{
						throw;
					}
					projectileObject.layer = 19;
					Physics.IgnoreLayerCollision(19, 19, true);
					if (noconsume)
						GameObject.Destroy(projectileObject, 40f);
					else
					{
						if (i >= 4)
							GameObject.Destroy(projectileObject, 40);         //if spamming arrows, delete 4th and further after really short time
					}
					if ((bool)projectileObject.GetComponent<Rigidbody>())
					{
						if (itemCache.MatchRangedStyle(TheForest.Items.Item.RangedStyle.Shoot))
						{
							projectileObject.GetComponent<Rigidbody>().AddForce(projectileObject.transform.TransformDirection(Vector3.forward * (0.016666f / Time.fixedDeltaTime) * ModdedPlayer.Stats.projectileSpeed * itemCache._projectileThrowForceRange), ForceMode.VelocityChange);
						}
						else
						{
							float num = Time.time - _weaponChargeStartTime;
							if (ForestVR.Enabled)
							{
								projectileObject.GetComponent<Rigidbody>().AddForce(inventoryItemView2._held.transform.up * ModdedPlayer.Stats.projectileSpeed * itemCache._projectileThrowForceRange);
							}
							else
							{
								Vector3 proj_force = forceUp * ModdedPlayer.Stats.projectileSpeed * Mathf.Clamp01(num / itemCache._projectileMaxChargeDuration) * (0.016666f / Time.fixedDeltaTime) * itemCache._projectileThrowForceRange;
								var proj_rb = projectileObject.GetComponent<Rigidbody>();
								proj_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
								var col = projectileObject.GetComponent<CapsuleCollider>();
								if (col)
									col.height *= ModdedPlayer.Stats.projectileSpeed;
								else
									Debug.LogError("No capsule collider on projectile");
								if (GreatBow.isEnabled)
								{
									proj_force *= 1.1f;
									proj_rb.useGravity = false;
								}
								if (ModdedPlayer.Stats.spell_bia_AccumulatedDamage > 0)
								{
									proj_force *= 1.1f;
									proj_rb.useGravity = false;
									if (ModReferences.bloodInfusedMaterial == null)
									{
										ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
										{
											EmissionColor = new Color(0.6f, 0.1f, 0),
											renderMode = BuilderCore.BuildingData.RenderMode.Opaque,
											MainColor = Color.red,
											Metalic = 1f,
											Smoothness = 0.9f,
										});
									}
									var trail = projectileObject.AddComponent<TrailRenderer>();
									trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
									trail.material = ModReferences.bloodInfusedMaterial;
									trail.widthMultiplier = 0.45f;
									trail.time = 1.25f;
									trail.autodestruct = false;
								}
								if (i > 0)
								{
									float dmgPen = 1;
									for (int k = 0; k < i; k++)
									{
										dmgPen *= ModdedPlayer.Stats.perk_multishotDamagePennalty.Value;
									}
									//using XBArrowDamageMod as a typename here results in erros with modapi
									projectileObject.SendMessage("ModifyStartingDamage", dmgPen, SendMessageOptions.DontRequireReceiver);
								}
								proj_rb.AddForce(proj_force);
							}
							if (LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._bowId))
							{
								projectileObject.SendMessage("setCraftedBowDamage", SendMessageOptions.DontRequireReceiver);
							}
						}
						inventoryItemView._held.SendMessage("OnAmmoFired", projectileObject, SendMessageOptions.DontRequireReceiver);
					}
					if (itemCache._attackReleaseSFX != 0)
					{
						LocalPlayer.Sfx.SendMessage(itemCache._attackReleaseSFX.ToString(), SendMessageOptions.DontRequireReceiver);
					}
					Mood.HitRumble();
				}
				else
				{
					if (itemCache._dryFireSFX != 0)
					{
						LocalPlayer.Sfx.SendMessage(itemCache._dryFireSFX.ToString(), SendMessageOptions.DontRequireReceiver);
					}
				}
				if (i % 2 == 1)
					yield return null;
			}
			if (flag)
			{
				inv.UnequipItemAtSlot(itemCache._equipmentSlot, false, false, flag);
			}
			else
			{
				inv.ToggleAmmo(inventoryItemView, true);
			}

			yield break;
		}
	}
}