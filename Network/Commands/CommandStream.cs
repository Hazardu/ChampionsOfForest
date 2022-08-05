using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using static Ceto.WaveSpectrumBufferCPU;
using static HutongGames.PlayMaker.FsmUtility;
using UnityEngine;
using System.Runtime.InteropServices;

namespace ChampionsOfForest.Network
{
	public class CommandStream : MemoryStream
	{
		BinaryWriter writer;
		public CommandStream(Commands.CommandType commandType) : base()
		{
			writer = new BinaryWriter(this);
			writer.Write((int)commandType);
		}
		public override void Close()
		{
			writer.Close();
			base.Close();
		}
		public void Write(bool value)
		{
			writer.Write(value);
		}
		public void Write(byte value)
		{
			writer.Write(value);
		}
		public void Write(byte[] buffer)
		{
			writer.Write(buffer);

		}
		public void Write(char ch)
		{
			writer.Write(ch);

		}
		public void Write(char[] chars)
		{
			writer.Write(chars);
		}
		public void Write(char[] chars, int index, int count)
		{
			writer.Write(chars, index, count);
		}
		public void Write(decimal value)
		{
			writer.Write(value);
		}
		public void Write(double value)
		{
			writer.Write(value);
		}
		public void Write(short value)
		{
			writer.Write(value);
		}
		public void Write(int value)
		{
			writer.Write(value);
		}
		public void Write(long value)
		{
			writer.Write(value);
		}
		public void Write(sbyte value)
		{
			writer.Write(value);
		}
		public void Write(float value)
		{
			writer.Write(value);
		}
		public void Write(string value)
		{
			writer.Write(value);
		}
		public void Write(ushort value)
		{
			writer.Write(value);
		}
		public void Write(uint value)
		{
			writer.Write(value);

		}
		public void Write(ulong value)
		{
			writer.Write(value);

		}
		public void Write(in Vector3 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
			writer.Write(value.z);
		}
		public void Write(in Vector2 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
		}
		public void Write(in Quaternion value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
		}

		public void Write<T>(T element)
		{
			int size = Marshal.SizeOf(element);
			byte[] arr = new byte[size];
			IntPtr ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(element, ptr, true);
				Marshal.Copy(ptr, arr, 0, size);
				writer.Write(arr, 0, size);
			}
			finally
			{
				Marshal.FreeHGlobal(ptr);
			}

		}

		public void Send(NetworkManager.Target targets)
		{
			Network.NetworkManager.SendCommand(this, targets);
			Close();
		}
		public override string ToString()
		{
			Position = 0;
			using (StreamReader reader = new StreamReader(this))
			{
				return reader.ReadToEnd();
			}
		}

	}
}
