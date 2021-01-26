using System;

using Bolt;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UdpKit;

using UnityEngine;

namespace ChampionsOfForest.Network
{
	internal class CoopServerCallbacksMod : CoopServerCallbacks
	{
		public override void OnEvent(PlayerHitEnemy ev)
		{
			//this needed to be changed.
			//the command would send damage using hitReal - pure damage.
			//this made clinets attack ignore armor and deal too much dmg
			if (!this.ValidateSender(ev, global::SenderTypes.Any))
			{
				return;
			}
			if (!ev.Target)
			{
				return;
			}
			if (ev.Hit == 0)
			{
				return;
			}
			try
			{
				if (global::EnemyHealth.CurrentAttacker == null)
				{
					global::EnemyHealth.CurrentAttacker = ev.Target;
				}
				var packed = ev.Target.networkId.PackedValue;
				if (EnemyManager.hostDictionary.ContainsKey(packed))
				{
					var enemy = EnemyManager.hostDictionary[packed];
					if (ev.explosion)
					{
						enemy.HealthScript.Explosion(-1);
					}
					if (ev.Burn)
					{
						enemy.HealthScript.Burn();
					}
					if (ev.getAttackerType == DamageMath.SILENTattackerType)
					{
						//ghost hit

						float damage = BitConverter.ToSingle(BitConverter.GetBytes(ev.Hit), 0);
						enemy.HitPhysicalSilent(damage);
					}
					else if (ev.getAttackerType == DamageMath.SILENTattackerTypeMagic)
					{
						//ghost hit

						float damage = BitConverter.ToSingle(BitConverter.GetBytes(ev.Hit), 0);
						enemy.HitMagic(damage);
					}
					else
					{
						if (ev.Hit > 0)
						{

							float damage = ev.getAttackerType >= DamageMath.CONVERTEDFLOATattackerType ? BitConverter.ToSingle(BitConverter.GetBytes(ev.Hit), 0) : ev.Hit;
							if (ev.getAttackerType >= DamageMath.CONVERTEDFLOATattackerType)
								ev.getAttackerType -= DamageMath.CONVERTEDFLOATattackerType;
							//just in case i ever need this
							//this is how to get the player object which raised the event (ev.RaisedBy.UserData as BoltEntity)
							enemy.HealthScript.getAttackDirection(ev.getAttackerType);
							var attackerGO = (ev.RaisedBy.UserData as BoltEntity).gameObject;
							enemy.setup.search.switchToNewTarget(attackerGO);
							enemy.setup.hitReceiver.getAttackDirection(ev.getAttackDirection);
							enemy.setup.hitReceiver.getCombo(ev.getCombo);
							enemy.HealthScript.takeDamage(ev.takeDamage);
							enemy.HealthScript.setSkinDamage(1);
							enemy.HitPhysical(damage);
							
						}
						else
						{
							ModAPI.Console.Write("The good armor reduction");
							enemy.ReduceArmor(-ev.Hit);
						}
					}
					return;
				}

				//Fuck all of this spaghetti below
				global::lb_Bird component = ev.Target.GetComponent<global::lb_Bird>();
				global::Fish componentInChildren = ev.Target.GetComponentInChildren<global::Fish>();
				Transform transform;
				if (componentInChildren)
				{
					transform = componentInChildren.transform;
				}
				else if (ev.Target.GetComponent<global::animalHealth>())
				{
					transform = ev.Target.transform;
				}
				else if (component)
				{
					transform = component.transform;
				}
				else
				{
					global::EnemyHealth componentInChildren2 = ev.Target.GetComponentInChildren<global::EnemyHealth>();
					if (componentInChildren2)
					{
						transform = componentInChildren2.transform;
					}
					else
					{
						transform = ev.Target.transform.GetChild(0);
					}
				}
				if (ev.getAttacker == 10 && ev.Weapon)
				{
					global::ArrowDamage componentInChildren3 = ev.Weapon.GetComponentInChildren<global::ArrowDamage>();
					if (componentInChildren3.Live)
					{
						global::arrowStickToTarget componentInChildren4 = transform.GetComponentInChildren<global::arrowStickToTarget>();
						Transform target = transform;
						if (componentInChildren4)
						{
							target = componentInChildren4.transform;
						}
						componentInChildren3.CheckHit(Vector3.zero, target, false, transform.GetComponent<Collider>());
					}
				}
				if (ev.explosion)
				{
					transform.SendMessage("Explosion", -1, SendMessageOptions.DontRequireReceiver);
				}
				if (ev.HitHead)
				{
					transform.SendMessage("HitHead", SendMessageOptions.DontRequireReceiver);
				}
				if (ev.getStealthAttack)
				{
					transform.SendMessage("getStealthAttack", SendMessageOptions.DontRequireReceiver);
				}
				float dmg = ev.getAttackerType >= DamageMath.CONVERTEDFLOATattackerType ? BitConverter.ToSingle(BitConverter.GetBytes(ev.Hit), 0) : ev.Hit;
				if (ev.hitFallDown)
				{
					global::mutantHitReceiver componentInChildren5 = transform.GetComponentInChildren<global::mutantHitReceiver>();
					if (componentInChildren5)
					{
						componentInChildren5.sendHitFallDown(dmg);
					}
				}
				else
				{
					if (ev.Hit > 0)
					{
						transform.SendMessage("getAttacker", (ev.RaisedBy.UserData as BoltEntity).gameObject, SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("getAttackerType", ev.getAttackerType, SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("getAttackDirection", ev.getAttackDirection, SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("getCombo", ev.getCombo, SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("takeDamage", ev.takeDamage, SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("setSkinDamage", UnityEngine.Random.Range(0, 3), SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("ApplyAnimalSkinDamage", ev.getAttackDirection, SendMessageOptions.DontRequireReceiver);
						transform.SendMessage("Hit", dmg, SendMessageOptions.DontRequireReceiver);
						if (ev.HitAxe)
						{
							transform.SendMessage("HitAxe", SendMessageOptions.DontRequireReceiver);
						}
						if (ev.Burn)
						{
							transform.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						ModAPI.Console.Write("The bad armor reduction");
						transform.SendMessage("ReduceArmor", -dmg, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			finally
			{
				global::EnemyHealth.CurrentAttacker = null;
			}
		}

		public override void OnEvent(ChatEvent evnt)
		{
			//the host would resend all commands to the clients.
			//this is okay for chat messages
			//but with commands sent directly there is no need. This adds unnecessary latency.
			//additionally all events like experience after a kill would be proc twice for clients.

			if (!this.ValidateSender(evnt, global::SenderTypes.Any))
			{
				return;
			}
			if (evnt.Sender == ChatBoxMod.ModNetworkID)
			{
				return;
			}

			for (int i = 0; i < TheForest.Utils.Scene.SceneTracker.allPlayerEntities.Count; i++)
			{
				if (TheForest.Utils.Scene.SceneTracker.allPlayerEntities[i].source == evnt.RaisedBy)
				{
					if (TheForest.Utils.Scene.SceneTracker.allPlayerEntities[i].networkId == evnt.Sender)
					{
						ChatEvent chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
						chatEvent.Sender = evnt.Sender;
						chatEvent.Message = evnt.Message;
						chatEvent.Send();
					}
					return;
				}
			}
			if (BoltNetwork.isServer && evnt.RaisedBy == null)
			{
				ChatEvent chatEvent2 = ChatEvent.Create(GlobalTargets.AllClients);
				chatEvent2.Sender = evnt.Sender;
				chatEvent2.Message = evnt.Message;
				chatEvent2.Send();
			}
		}

		public override void EntityDetached(BoltEntity entity)
		{
			if (entity != null)
			{
				if (entity.StateIs<IPlayerState>() && TheForest.Utils.Scene.SceneTracker && GameSetup.IsMpServer)
				{
					if (entity.source.udpConnection.IsConnected)
					{
						NetworkManager.SendText("II" + entity.GetState<IPlayerState>().name + " has died", NetworkManager.Target.Everyone);
					}
					else
					{
						ModdedPlayer.instance.SendLeaveMessage(entity.GetState<IPlayerState>().name);
					}
				}
			}
			base.EntityDetached(entity);
		}
		public override void Connected(BoltConnection connection)
		{
			NetworkManager.SendText("IIA champion approaches", NetworkManager.Target.Everyone);

			base.Connected(connection);

		}


	}

	internal class CoopClientCallbacksMod : CoopClientCallbacks
	{
		public override void Disconnected(BoltConnection connection)
			{
			ModAPI.Console.Write("Saving client data to avoid item duping");
			Serializer.EmergencySave();
			base.Disconnected(connection);
	}
}

	public class BoltConnectionEx : BoltConnection
	{
		public BoltConnectionEx(UdpKit.UdpConnection c) : base(c)
		{
		}

		public override void PacketReceived(UdpPacket udpPacket)
		{
			try
			{
				using (Packet packet = PacketPool.Acquire())
				{
					packet.UdpPacket = udpPacket;
					packet.Frame = packet.UdpPacket.ReadIntVB();
					if (packet.Frame > this._remoteFrameActual)
					{
						this._remoteFrameAdjust = true;
						this._remoteFrameActual = packet.Frame;
					}
					this._bitsSecondInAcc += packet.UdpPacket.Size;
					this._packetsReceived++;
					for (int i = 0; i < this._channels.Length; i++)
					{
						this._channels[i].Read(packet);
					}
					this._packetStatsIn.Enqueue(packet.Stats);
				}
			}
			catch (Exception exception)
			{
				BoltLog.Exception(exception);
				ModAPI.Log.Write(exception.ToString());
			}

		}

		public override void Disconnect()
		{
			Serializer.EmergencySave();
			base.Disconnect();
		}
	}
}