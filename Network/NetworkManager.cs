using System;

using Bolt;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Network
{
	public class NetworkManager : MonoBehaviour
	{
		public enum Target
		{
			OnlyServer, Everyone, Clients, Others
		}

		public delegate void OnGetMessage(byte[] arr);

		public OnGetMessage onGetMessage;
		public static NetworkManager instance;

		/// <summary>
		/// Sets the 'instance'
		/// </summary>
		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
			}
		}

		/// <summary>
		/// Sends a string to all players on the server
		/// </summary>
		/// <param name="s">Content of the message, make sure it ends with ';'</param>
		public static void SendLine(byte[] bytearray, Target target)
		{
			if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
			{
				RecieveLine(bytearray);
			}
			else
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = null;
					switch (target)
					{
						case Target.OnlyServer:
							chatEvent = ChatEvent.Create(GlobalTargets.OnlyServer);
							break;

						case Target.Everyone:
							chatEvent = ChatEvent.Create(GlobalTargets.Everyone);
							break;

						case Target.Clients:
							chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
							break;

						case Target.Others:
							chatEvent = ChatEvent.Create(GlobalTargets.Others);
							break;

						default:
							break;
					}
					chatEvent.Message = EncodeCommand(bytearray);
					chatEvent.Sender = ChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}

		public static void SendText(string text, Target target)
		{
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = null;
					switch (target)
					{
						case Target.OnlyServer:
							chatEvent = ChatEvent.Create(GlobalTargets.OnlyServer);
							break;

						case Target.Everyone:
							chatEvent = ChatEvent.Create(GlobalTargets.Everyone);
							break;

						case Target.Clients:
							chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
							break;

						case Target.Others:
							chatEvent = ChatEvent.Create(GlobalTargets.Others);
							break;

						default:
							break;
					}
					chatEvent.Message = text;
					chatEvent.Sender = ChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}

		public static void SendLine(byte[] bytearray, BoltConnection con)
		{
			if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
			{
				RecieveLine(bytearray);
			}
			else
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = ChatEvent.Create(con);
					chatEvent.Message = EncodeCommand(bytearray);
					chatEvent.Sender = ChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}

		public static byte[] DecodeCommand(string cmd)
		{
			var a = cmd.ToCharArray();
			var b = new byte[a.Length];
			for (int i = 0; i < a.Length; i++)
			{
				b[i] = (byte)a[i];
			}
			return b;
		}

		public static string EncodeCommand(byte[] b)
		{
			string s = string.Empty;
			for (int i = 0; i < b.Length; i++)
			{
				s += (char)b[i];
			}
			return s;
		}

		/// <summary>
		/// Called on recieving a message
		/// </summary>
		/// <param name="s"></param>
		public static void RecieveLine(byte[] array)
		{
			try
			{
				instance.onGetMessage(array);
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}

		public static ulong lastDropID = 10;

		/// <summary>
		/// Sends a command to create a item drop for all players.
		/// </summary>
		/// <param name="item">A reference to the item object. Things like level and stats will be read off it</param>
		/// <param name="pos">Where to spawn the item at</param>
		/// <param name="amount">How many of this item should be spawned</param>
		public static void SendItemDrop(Item item, Vector3 pos, int amount = 1)
		{
			if (item == null)
				return;
			ulong id = lastDropID + 1;
			while (PickUpManager.PickUps.ContainsKey(id))
			{
				id++;
			}
			lastDropID = id;
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(5);
					w.Write(item.ID);
					w.Write(id);
					w.Write(item.level);
					w.Write(amount);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					foreach (ItemStat stat in item.Stats)
					{
						w.Write(stat.StatID);
						w.Write(stat.possibleStatsIndex);
						w.Write(stat.Amount);
					}
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		public static void SendItemToPlayer(Item item, string playerID, int amount = 1)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(26);
					w.Write(playerID);
					w.Write(item.ID);
					w.Write(amount);
					w.Write(item.level);
					foreach (ItemStat stat in item.Stats)
					{
						w.Write(stat.StatID);
						w.Write(stat.possibleStatsIndex);
						w.Write(stat.Amount);
					}
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}
		public static void HitEnemyMagic()
		{
			
		}
		public static void SendHitmarker(Vector3 pos, float amount, Color c)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(20);
					w.Write(amount);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(c.r);
					w.Write(c.g);
					w.Write(c.b);
					w.Write(c.a);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		public static void SendPlayerHitmarker(Vector3 pos, int amount)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(21);
					w.Write(amount);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		/// <summary>
		/// Sends a buff commands to players close to a location
		/// </summary>
		/// <param name="pos">position, where the buff was casted</param>
		/// <param name="range">range of the buff effect</param>
		public static void SendBuff(Vector3 pos, float range, int buffId, int sourceId, float amount, float duration)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(41);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(range);
					w.Write(buffId);
					w.Write(sourceId);
					w.Write(amount);
					w.Write(duration);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		/// <summary>
		/// Send buff to a player with a name
		/// </summary>
		/// <param name="id">Name identificator of the other player</param>
		public static void SendBuff(string id, int buffId, int sourceId, float amount, float duration)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(40);

					w.Write(id);
					w.Write(buffId);
					w.Write(sourceId);
					w.Write(amount);
					w.Write(duration);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}

		/// <summary>
		/// Send buff to everyone, globally
		/// </summary>
		public static void SendBuff(int buffId, int sourceId, float amount, float duration)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(42);
					w.Write(buffId);
					w.Write(sourceId);
					w.Write(amount);
					w.Write(duration);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}
	}
}