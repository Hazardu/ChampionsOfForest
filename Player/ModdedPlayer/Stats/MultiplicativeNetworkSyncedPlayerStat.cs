using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ChampionsOfForest.Player
{
	public class MultiplicativeNetworkSyncedPlayerStat<T> : NumericPlayerStatBase<T>, INetworkSyncedPlayerStat, INetworkStatStorage<T>, IMultiplicativeStat<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		protected Func<T, T, T> mult;
		protected Func<T, T, T> divide;

		public T valueMultiplicative;
		protected T default_valueMultiplicative;

		public MultiplicativeNetworkSyncedPlayerStat(T default_valueMultiplicative, in Func<T, T, T> multFunc, in Func<T, T, T> divideFunc, string formatting = "N0")
		{
			this.default_valueMultiplicative = default_valueMultiplicative;
			this.valueMultiplicative = default_valueMultiplicative;
			base.formatting = formatting;
			mult = multFunc;
			divide = divideFunc;
			StatNetworkIndex = NetworkPlayerStats.NextIndex;
			NetworkPlayerStats.syncedStats.Add(this);
			OtherPlayerValues = new Dictionary<string, T>();
			AddStatToList();
		}

		public T Multiply(T amount)
		{
			valueMultiplicative = mult(amount, valueMultiplicative);
			ValueChanged();

			return valueMultiplicative;
		}
		public T Divide(T amount)
		{
			valueMultiplicative = divide(amount, valueMultiplicative);
			ValueChanged();

			return valueMultiplicative;
		}
		public T GetAmountAfterMultiplying(T chngMult)
		{
			return mult(chngMult, valueMultiplicative);
		}
		public override void Reset()
		{
			valueMultiplicative = default_valueMultiplicative;
			ValueChanged();
		}

		public static T operator *(MultiplicativeNetworkSyncedPlayerStat<T> a, T b)
		{
			return a.mult(a.valueMultiplicative, b);
		}
		public static T operator /(MultiplicativeNetworkSyncedPlayerStat<T> a, T b)
		{
			return a.divide(a.valueMultiplicative, b);
		}

		public override T GetAmount()
		{
			T value = valueMultiplicative;
			foreach (var item in OtherPlayerValues)
			{
				value = mult(value, item.Value);
			}
			return value;
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
			Marshal.StructureToPtr(valueMultiplicative, ptr, false);
			// Copy data from unmanaged memory to managed buffer.
			Marshal.Copy(ptr, bytes, 0, size);
			// Release unmanaged memory.
			//Marshal.FreeHGlobal(ptr);

			writer.Write(StatNetworkIndex);
			writer.Write(ModReferences.ThisPlayerID);
			writer.Write(bytes);
		}
		public void ReceivedOtherPlayerChange(BinaryReader reader)
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