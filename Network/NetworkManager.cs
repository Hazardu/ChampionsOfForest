using System;
using System.Collections.Generic;
using System.IO;

using Bolt;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Network
{
	public class NetworkManager : MonoBehaviour
	{
		public enum Target
		{
			OnlyServer = 6, Everyone = 2, Clients = 8, Others = 4, Self = 12
		}

		private static NetworkManager instance;

		private Commands cmdHandler;
		private Dictionary<ulong, ModdedClient> moddedClientsDictionary = new Dictionary<ulong, ModdedClient>();

		public static ModdedClient GetModdedClient(ulong id)
		{
			if (instance.moddedClientsDictionary.ContainsKey(id))
				return instance.moddedClientsDictionary[id];
			else
			{
				var entity = BoltNetwork.FindEntity(new NetworkId(id));
				if (entity != null)
				{
					ModdedClient mc = new ModdedClient(entity);
					instance.moddedClientsDictionary.Add(id,mc);
					return mc;
				}
				return null;
			}
		}
		public static int GetPlayerCount => Mathf.Max(1,TheForest.Utils.Scene.SceneTracker.allPlayerEntities.Count);

		/// <summary>
		/// Sets the 'instance'
		/// </summary>
		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				cmdHandler = new Commands();
			}
			else
			{
				Destroy(this);
			}
		}

		internal static void SendCommand(CommandStream cmd, Target target)
		{
			if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
			{
				ReceiveLine(cmd.ToArray());
			}
			else
			{
				if (BoltNetwork.isRunning)
				{
					ChatEvent chatEvent = ChatEvent.Create((GlobalTargets)target);
					chatEvent.Message = cmd.ToString();
					chatEvent.Sender = ChatBoxMod.ModNetworkID;
					chatEvent.Send();
				}
			}
		}
	

		internal static void SendText(string text, Target target)
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

		internal static void SendLine(byte[] bytearray, BoltConnection con)
		{
			if (GameSetup.IsSinglePlayer || !BoltNetwork.isRunning)
			{
				ReceiveLine(bytearray);
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

		private static byte[] DecodeCommand(string cmd)
		{
			var a = cmd.ToCharArray();
			var b = new byte[a.Length];
			for (int i = 0; i < a.Length; i++)
			{
				b[i] = (byte)a[i];
			}
			return b;
		}

		private static string EncodeCommand(byte[] b)
		{
			string s = string.Empty;
			for (int i = 0; i < b.Length; i++)
			{
				s += (char)b[i];
			}
			return s;
		}


		/// <summary>
		/// Called on receiving a message
		/// </summary>
		/// <param name="s"></param>
		internal static void ReceiveLine(byte[] array)
		{
			instance.cmdHandler.ReceiveCommand(array);
		}

	
	}
}