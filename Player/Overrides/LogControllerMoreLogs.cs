using Bolt;

using TheForest.Items.Special;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class LogControllerMoreLogs : LogControler
	{
		private int additional_logs;

		public override bool Lift()
		{
			return LiftNew();
		}

		public override void RemoveLog(bool equipPrevious)
		{
			if (this._logs > 0)
			{
				Debug.Log("Removing log, _logs:" + _logs + "   extra logs:" + additional_logs);
				if (additional_logs == 0)
				{
					this._logs--;
					this._logsHeld[this._logs].SetActive(false);
					if (this._logs == 0)
					{
						for (int i = 0; i < this._itemCache._equipedAnimVars.Length; i++)
						{
							LocalPlayer.Animator.SetBoolReflected(this._itemCache._equipedAnimVars[i].ToString(), false);
						}
						if (equipPrevious)
						{
							if (!this._player.HasInSlot(TheForest.Items.Item.EquipmentSlot.LeftHand, this._lighterItemId))
							{
								this._player.EquipPreviousUtility(false);
							}
							this._player.EquipPreviousWeaponDelayed();
						}
						base.enabled = false;
					}
				}
				else
				{
					additional_logs--;
					PutDownNew(false,false,false,null);
				}
			}
		}

		public override bool PutDown(bool fake, bool drop, bool equipPrevious, GameObject preSpawned)
		{
			return PutDownNew(fake, drop, equipPrevious, preSpawned);
		}

		private bool PutDownNew(bool fake, bool drop, bool equipPrev, GameObject preSpawned)
		{
			if (additional_logs > 0 && !_infiniteLogHack)
			{
				if (drop)
				{
					additional_logs--;
					Transform heldLog = this._logsHeld[Mathf.Min(this._logs, 1)].transform;
					Vector3 logPosition = heldLog.position + heldLog.forward * -2f;
					Quaternion playerRotation = LocalPlayer.Transform.rotation;
					playerRotation *= Quaternion.AngleAxis(90f, Vector3.up);
					if (LocalPlayer.FpCharacter.PushingSled)
					{
						logPosition += heldLog.forward * -1.25f + heldLog.right * -2f;
					}
					Vector3 rayOrigin = logPosition;
					rayOrigin.y += 3f;
					if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit raycastHit, 5f, this._layerMask))
					{
						logPosition.y = raycastHit.point.y + 2.2f;
					}
					if (_logs == 1)
					{
						logPosition.y += 1f;
					}
					if (BoltNetwork.isRunning)
					{
						DropItem dropItem2 = DropItem.Create(GlobalTargets.OnlyServer);
						dropItem2.PrefabId = BoltPrefabs.Log;
						dropItem2.Position = logPosition;
						dropItem2.Rotation = playerRotation;
						dropItem2.PreSpawned = ((preSpawned != null) ? null : preSpawned.GetComponent<BoltEntity>());
						dropItem2.Send();
					}
					else if ((bool)preSpawned)
					{
						preSpawned.transform.position = logPosition;
						preSpawned.transform.rotation = playerRotation;
					}
					else
					{
						UnityEngine.Object.Instantiate<GameObject>(this._logPrefab, logPosition, playerRotation);
					}
					FMODCommon.PlayOneshotNetworked("event:/player/foley/log_drop_exert", heldLog, FMODCommon.NetworkRole.Any);
				}
				return true;
			}
			else
			{
				return OriginalPutDown(fake, drop, equipPrev, preSpawned);
			}
		}

		public bool OriginalPutDown(bool fake, bool drop, bool equipPrevious, GameObject preSpawned)
		{
			if (this._infiniteLogHack)
			{
				return true;
			}
			if (!fake)
			{
				if (this._logs <= 0)
				{
					return false;
				}
				this.RemoveLog(equipPrevious);
			}
			if (drop)
			{
				bool flag = false;
				if (this._logs == 1)
				{
					flag = true;
				}
				Transform transform = this._logsHeld[Mathf.Min(this._logs, 1)].transform;
				Vector3 vector = transform.position + transform.forward * -2f;
				Quaternion quaternion = TheForest.Utils.LocalPlayer.Transform.rotation;
				quaternion *= Quaternion.AngleAxis(90f, Vector3.up);
				if (TheForest.Utils.LocalPlayer.FpCharacter.PushingSled)
				{
					vector += transform.forward * -1.25f + transform.right * -2f;
				}
				Vector3 origin = vector;
				origin.y += 3f;
				RaycastHit raycastHit;
				if (Physics.Raycast(origin, Vector3.down, out raycastHit, 5f, this._layerMask))
				{
					vector.y = raycastHit.point.y + 2.2f;
				}
				if (flag)
				{
					vector.y += 1f;
				}
				if (BoltNetwork.isRunning)
				{
					DropItem dropItem = DropItem.Create(GlobalTargets.OnlyServer);
					dropItem.PrefabId = BoltPrefabs.Log;
					dropItem.Position = vector;
					dropItem.Rotation = quaternion;
					dropItem.PreSpawned = ((!preSpawned) ? null : preSpawned.GetComponent<BoltEntity>());
					dropItem.Send();
				}
				else if (preSpawned)
				{
					preSpawned.transform.position = vector;
					preSpawned.transform.rotation = quaternion;
				}
				else
				{
					UnityEngine.Object.Instantiate<GameObject>(this._logPrefab, vector, quaternion);
				}
				global::FMODCommon.PlayOneshotNetworked("event:/player/foley/log_drop_exert", transform, global::FMODCommon.NetworkRole.Any);
			}
			this.UpdateLogCount();
			return true;
		}

		public bool LiftNew()
		{
			if (this._logs + additional_logs < ModdedPlayer.Stats.MaxLogs && !LocalPlayer.AnimControl.swimming && !LocalPlayer.FpCharacter.PushingSled && !LocalPlayer.FpCharacter.SailingRaft && !LocalPlayer.AnimControl.carry && !LocalPlayer.AnimControl.useRootMotion)
			{
				if (_logs < 2)
				{
					this._logs++;
					this._logsHeld[this._logs - 1].SetActive(true);
					LocalPlayer.Sfx.PlayWhoosh();

					if (this._logs == 1)
					{
						if (!this._player.HasInSlot(TheForest.Items.Item.EquipmentSlot.LeftHand, this._lighterItemId))
						{
							this._player.MemorizeItem(TheForest.Items.Item.EquipmentSlot.LeftHand);
							this._player.UnequipItemAtSlot(TheForest.Items.Item.EquipmentSlot.LeftHand, false, true, false);
						}
						if (!LocalPlayer.FpCharacter.drinking)
						{
							this._player.MemorizeItem(TheForest.Items.Item.EquipmentSlot.RightHand);
							this._player.UnequipItemAtSlot(TheForest.Items.Item.EquipmentSlot.RightHand, false, true, false);
						}
						for (int i = 0; i < this._itemCache._equipedAnimVars.Length; i++)
						{
							LocalPlayer.Animator.SetBoolReflected(this._itemCache._equipedAnimVars[i].ToString(), true);
						}
						base.enabled = true;
					}
					this.UpdateLogCount();
				}
				else
				{
					LocalPlayer.Sfx.PlayWhoosh();
					additional_logs++;
					this.UpdateLogCount();
				}
				return true;
			}
			this.UpdateLogCount();
			return false;
		}
	}
}