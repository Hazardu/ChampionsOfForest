using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Enemies;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Network
{

	public partial class Commands
	{
		private delegate void CommandReceivedCallback(BinaryReader r);
		private struct RegisteredCommand
		{
			public string name;
			public CommandReceivedCallback callback;
			public RegisteredCommand(string name, CommandReceivedCallback callback)
			{
				this.name = name;
				this.callback = callback;
			}
		}
		private readonly RegisteredCommand[] registeredCommands;

		
		internal void ReceiveCommand(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				using (BinaryReader r = new BinaryReader(stream))
				{
					int cmdIndex = r.ReadInt32();
					try
					{
						registeredCommands[cmdIndex].callback.Invoke(r);
					}
					catch (Exception e)
					{
						ModAPI.Log.Write("Exception while invoking rpc " + registeredCommands[cmdIndex].name + "\n" + e.ToString());
					}
				}
			}

		}
	}
}