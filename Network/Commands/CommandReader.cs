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
using static UdpKit.UdpLog;

namespace ChampionsOfForest.Network
{

	public class CommandReader : BinaryReader
	{
		public CommandReader(Stream stream) : base(stream)
		{
		}
		public T ReadStruct<T>() where T : struct
		{
			int size = Marshal.SizeOf(typeof(T));
			byte[] bytes = ReadBytes(size);
			IntPtr ptr = IntPtr.Zero;
			ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(bytes, 0, ptr, size);
			object obj = Marshal.PtrToStructure(ptr, typeof(T));
			Marshal.FreeHGlobal(ptr);
			return (T)obj;
		}
		public Vector3 ReadVector3()
		{
			return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
		}
		public Vector2 ReadVector2()
		{
			return new Vector2(ReadSingle(), ReadSingle());
		}
	}


	public partial class Commands
	{
		private delegate void CommandReceivedCallback(CommandReader r);
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
				using (CommandReader r = new CommandReader(stream))
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