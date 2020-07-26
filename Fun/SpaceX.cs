using Bolt;

using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest.Fun
{
	public class SpaceX : RaftPush
	{
		public override void PushRaft(MoveDirection dir)
		{
			if (dir == RaftPush.MoveDirection.Forward)
			{
				this._direction = ((!this._relativeForce) ? this._raft.transform.forward : base.transform.forward);
			}
			else
			{
				if (dir != RaftPush.MoveDirection.Backward)
				{
					return;
				}
				this._direction = ((!this._relativeForce) ? this._raft.transform.forward : base.transform.forward) * -1f;
			}
			this._direction.y = 0f;
			float num = 1f;
			if (!this._buoyancy.InWater)
			{
				num = ModdedPlayer.Stats.perk_turboRaftOwners >0 ? 1f : 0f;
			}
			this._direction *= this._speed * 2f * num;
			Vector3 velocity = this._rb.velocity;
			Vector3 target = this._direction - velocity;
			Vector3 vector = Vector3.zero;
			Vector3 zero = Vector3.zero;
			vector = Vector3.SmoothDamp(vector, target, ref zero, this._dampSpeed);
			if (this.UseRelativeForce)
			{
				Vector3 position = base.transform.position;
				if (this._buoyancy.OverrideCenterOfMass != null)
				{
					position.y = this._buoyancy.OverrideCenterOfMass.position.y;
				}
				this._rb.AddForceAtPosition(vector * (0.016666f / Time.fixedDeltaTime) * ModdedPlayer.Stats.perk_RaftSpeedMultipier, position, this._forceMode);
			}
			else
			{
				this._rb.AddForce(vector * (0.016666f / Time.fixedDeltaTime) *  ModdedPlayer.Stats.perk_RaftSpeedMultipier, this._forceMode);
			}
			////fuck the limits
			//if (!ModdedPlayer.instance.TurboRaft)
			//{
			//    if (this._rb.velocity.magnitude > this._maxVelocity)
			//    {
			//        Vector3 vector2 = this._rb.velocity.normalized;
			//        vector2 *= this._maxVelocity;
			//        this._rb.velocity = vector2;
			//    }
			//}
		}

		public override void TurnRaft(float axis)
		{
			if (!this._relativeForce || TheForest.Utils.Scene.SceneTracker.allPlayers.Count < 2)
			{
				float target = axis * (this._rotateForce * 2f) * Mathf.Clamp(this._rb.velocity.normalized.magnitude, 0.1f, 1f);
				float num = 0f;
				float num2 = 0f;
				num2 = Mathf.SmoothDamp(num2, target, ref num, this._torqueDamp);
				if (!InWater)
					num2 *= 1.76f;
				this._rb.AddTorque(0f, num2 * (0.016666f / Time.fixedDeltaTime) *  ModdedPlayer.Stats.perk_RaftSpeedMultipier, 0f, ForceMode.Force);
			}
		}

		protected override void Update()
		{
			if ((this._state == RaftPush.States.DriverStanding || this._state == RaftPush.States.Idle) && !this._doingOutOfWorld)
			{
				this.allowDirection = false;
			}
			if (this._state == RaftPush.States.DriverStanding)
			{
				TheForest.Utils.LocalPlayer.AnimControl.standingOnRaft = true;
			}
			else
			{
				TheForest.Utils.LocalPlayer.AnimControl.standingOnRaft = false;
			}
			bool flag = this._isGrabbed && this._state == RaftPush.States.DriverStanding;
			if (flag && BoltNetwork.isRunning && base.state.GrabbedBy[this._oarId] != null)
			{
				flag = false;
			}
			if (!this._canLockIcon.gameObject.activeSelf.Equals(flag))
			{
				this._canLockIcon.gameObject.SetActive(flag);
			}
			if (this._shouldUnlock)
			{
				this._shouldUnlock = false;
				if (BoltNetwork.isRunning)
				{
					RaftGrab raftGrab = RaftGrab.Create(GlobalTargets.OnlyServer);
					raftGrab.OarId = this._oarId;
					raftGrab.Raft = base.GetComponentInParent<BoltEntity>();
					raftGrab.Player = null;
					raftGrab.Send();
				}
				else
				{
					this.offRaft();
				}
				return;
			}
			this._shouldUnlock = false;
			if (this.stickToRaft)
			{
				TheForest.Utils.LocalPlayer.FpCharacter.enabled = false;
				TheForest.Utils.LocalPlayer.AnimControl.controller.useGravity = false;
				TheForest.Utils.LocalPlayer.AnimControl.controller.isKinematic = true;
				Vector3 position = this._driverPos.position;
				position.y += TheForest.Utils.LocalPlayer.AnimControl.playerCollider.height / 2f - TheForest.Utils.LocalPlayer.AnimControl.playerCollider.center.y;
				TheForest.Utils.LocalPlayer.Transform.position = position;
				TheForest.Utils.LocalPlayer.Transform.rotation = this._driverPos.rotation;
				TheForest.Utils.LocalPlayer.Animator.SetLayerWeightReflected(2, 1f);
			}
			if (TheForest.Utils.Input.GetButtonDown("Take") && !this._doingOutOfWorld)
			{
				if (flag)
				{
					if (BoltNetwork.isRunning)
					{
						RaftGrab raftGrab2 = RaftGrab.Create(GlobalTargets.OnlyServer);
						raftGrab2.OarId = this._oarId;
						raftGrab2.Raft = base.GetComponentInParent<BoltEntity>();
						raftGrab2.Player = TheForest.Utils.LocalPlayer.Entity;
						raftGrab2.Send();
					}
					else
					{
						this.onRaft();
					}
				}
				else if (this._state == RaftPush.States.DriverLocked)
				{
					if (BoltNetwork.isRunning)
					{
						RaftGrab raftGrab3 = RaftGrab.Create(GlobalTargets.OnlyServer);
						raftGrab3.OarId = this._oarId;
						raftGrab3.Raft = base.GetComponentInParent<BoltEntity>();
						raftGrab3.Player = null;
						raftGrab3.Send();
					}
					else
					{
						this.offRaft();
					}
				}
			}
			else if (this._state == RaftPush.States.DriverLocked && !this._doingOutOfWorld)
			{
				RaftPush.MoveDirection moveDirection = RaftPush.MoveDirection.None;
				float num = TheForest.Utils.Input.GetAxis("Horizontal");
				this.axisDirection = num;
				this.moveDirection = moveDirection;
				if ((TheForest.Utils.Input.GetButton("Fire1") || TheForest.Utils.Input.GetButton("AltFire")))
				{
					if (this.CheckDistanceFromOceanCollision() || TheForest.Utils.LocalPlayer.AnimControl.doneOutOfWorldRoutine || !TheForest.Utils.LocalPlayer.Inventory.Owns(TheForest.Utils.LocalPlayer.AnimControl._timmyPhotoId, true))
					{
						moveDirection = ((!TheForest.Utils.Input.GetButton("Fire1")) ? RaftPush.MoveDirection.Backward : RaftPush.MoveDirection.Forward);
						this.allowDirection = true;
						this.moveDirection = moveDirection;
						this._driver.enablePaddleOnRaft(true);
					}
					else
					{
						this.allowDirection = false;
						this._driver.enablePaddleOnRaft(false);
						if (!this._doingOutOfWorld)
						{
							base.StartCoroutine(this.outOfWorldRoutine());
						}
					}
				}
				else
				{
					this.allowDirection = false;
					this._driver.enablePaddleOnRaft(false);
				}
			}
			else if (this._state == RaftPush.States.Auto)
			{
				this._direction = TheForest.Utils.Scene.OceanCeto.transform.position - base.transform.position;
				this._direction.Normalize();
				this._raft.GetComponent<Rigidbody>().AddForce(this._direction * this._speed * 5f, ForceMode.Impulse);
				if (this.CheckDistanceFromOceanCollision())
				{
					this._state = RaftPush.States.DriverLocked;
				}
			}
		}
	}
}