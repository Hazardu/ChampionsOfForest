using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ChampionsOfForest.Network
{
	public class COTFCommand<ParamT> where ParamT : struct
	{
		public int commandIndex;
		public static COTFCommand<ParamT> instance;

		static protected void Init(Type type)
		{
			if (instance == null)
			{
				var o = (COTFCommand<ParamT>)Activator.CreateInstance(type);
				instance = o;
				instance.commandIndex = CommandReader.curr_cmd_index++;
				CommandReader.commandsObjects_dict.Add(instance.commandIndex, type.GetMethod("Received", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
				ModAPI.Log.Write($"command {instance.commandIndex} registered: {type}");
			}
		}

		static byte[] GetBytes(ParamT p)
		{
			int size = Marshal.SizeOf(p);
			byte[] arr = new byte[size];
			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(p, ptr, true);
			Marshal.Copy(ptr, arr, 0, size);
			Marshal.FreeHGlobal(ptr);
			return arr;
		}
		static ParamT GetParam(BinaryReader reader)
		{
			int size = Marshal.SizeOf(default(ParamT));
			IntPtr ptr = Marshal.AllocHGlobal(size);
			byte[] arr = reader.ReadBytes(size);
			Marshal.Copy(arr, 0, ptr, size);
			var param = (ParamT)Marshal.PtrToStructure(ptr, typeof(ParamT));
			Marshal.FreeHGlobal(ptr);
			return param;
		}

		protected virtual void OnSendDataWrite(BinaryWriter w)
		{
		}

		protected virtual void OnReceived(ParamT param, BinaryReader r)
		{
		}

		public static void Send(NetworkManager.Target target, ParamT param)
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(instance.commandIndex);
					w.Write(GetBytes(param));
					instance.OnSendDataWrite(w);
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), target);
				answerStream.Close();
			}
		}
		public static void Send(BoltConnection target_connection, ParamT param)
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(instance.commandIndex);
					w.Write(GetBytes(param));
					instance.OnSendDataWrite(w);
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), target_connection);
				answerStream.Close();
			}
		}
		public static void Received(BinaryReader r)
		{
			var p = GetParam(r);
			instance.OnReceived(p, r);
			r.Close();
		}
	}
}
