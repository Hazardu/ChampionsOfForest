using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ChampionsOfForest.Player
{
	public class AdditiveNetworkSyncedPlayerStat<T> : NumericPlayerStatBase<T>, INetworkSyncedPlayerStat, INetworkStatStorage<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{

		protected Func<T, T, T> add, substract;
		private T valueAdditive;
		protected T default_valueAdditive;

		public AdditiveNetworkSyncedPlayerStat(T default_valueAdditive,in Func<T, T, T> addFunc, in Func<T, T, T> substractFunc, string formatting = "N0")
		{
			this.default_valueAdditive = default_valueAdditive;
			this.valueAdditive = default_valueAdditive;
			base.formatting = formatting;
			this.add = addFunc;
			this.substract = substractFunc;
			StatNetworkIndex = NetworkPlayerStats.NextIndex;
			NetworkPlayerStats.syncedStats.Add(this);
			OtherPlayerValues = new Dictionary<string, T>();
			AddStatToList();
		}

		public T Add(T amount)
		{
			valueAdditive = add(valueAdditive, amount);
			ValueChanged();
			return Value;
		}
		public T Substract(T amount)
		{
			valueAdditive = substract(valueAdditive, amount);
			return Value;
		}
		public T GetAmountAfterAdding(T chngAdd)
		{
			return add(valueAdditive, chngAdd);
		}
		public override void Reset()
		{
			valueAdditive = default_valueAdditive;
			ValueChanged();

		}
		public override T GetAmount()
		{
			T value = valueAdditive;
			foreach (var item in OtherPlayerValues)
			{
				value = add(value, item.Value);
			}
			return value;
		}
		public static T operator +(AdditiveNetworkSyncedPlayerStat<T> a, T b)
		{
			return a.add(a.valueAdditive, b);
		}
		public static T operator -(AdditiveNetworkSyncedPlayerStat<T> a, T b)
		{
			return a.substract(a.valueAdditive, b);
		}

		public Dictionary<string, T> OtherPlayerValues
		{
			get; set;
		}

		public void PlayerDisconnected()
		{
			var keys = OtherPlayerValues.Keys;
			var names = ModReferences.PlayerStates.Select(x => x.name).ToList();
			foreach (var key in keys)
			{
				if (!names.Contains(key))
				{
					OtherPlayerValues.Remove(key);
				}
			}
		}

		public int StatNetworkIndex
		{
			get; set;
		}

		public void ValueChanged() => NetworkPlayerStats.SendUpdate(StatNetworkIndex);
		public void WriteToCommand(BinaryWriter writer) 
			{
			var size = Marshal.SizeOf(typeof(T));
			// Both managed and unmanaged buffers required.
			var bytes = new byte[size];
			var ptr = Marshal.AllocHGlobal(size);
			// Copy object byte-to-byte to unmanaged memory.
			Marshal.StructureToPtr(valueAdditive, ptr, false);
			// Copy data from unmanaged memory to managed buffer.
			Marshal.Copy(ptr, bytes, 0, size);
			// Release unmanaged memory.
			//Marshal.FreeHGlobal(ptr);

			writer.Write(StatNetworkIndex);
			writer.Write(ModReferences.ThisPlayerID);
			writer.Write(bytes);
		}
		public void RecievedOtherPlayerChange(BinaryReader reader)
		{
			string playerName = reader.ReadString();
			var size = Marshal.SizeOf(typeof(T));
			var bytes = reader.ReadBytes(size);
			var ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(bytes, 0, ptr, size);
			var newVal = (T)Marshal.PtrToStructure(ptr, typeof(T));

			//Marshal.FreeHGlobal(ptr);
			if (OtherPlayerValues.ContainsKey(playerName))
			{
				OtherPlayerValues[playerName] = newVal;
			}
			else
			{
				OtherPlayerValues.Add(playerName, newVal);
			}

		}
	}
}