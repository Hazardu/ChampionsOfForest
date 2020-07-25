using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Player
{
	public interface IPlayerStat<T>
	{
		T GetAmount();
		void Reset();

	}
	public abstract class CPlayerStatBase
	{
		public abstract void Reset();
		public abstract void AddStatToList();
		
	}
	public class CPlayerStatBase<T> : CPlayerStatBase , IPlayerStat<T>
	{
		public static implicit operator T(CPlayerStatBase<T> playerStat) => playerStat.GetAmount();
		public virtual T GetAmount() => throw new NotImplementedException();
		public override void Reset() => Assert.Fail();
		public override void AddStatToList() => ModdedPlayer.instance.allStats.Add(this);
	}
	public class NumericPlayerStatBase<T> : CPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		protected string formatting;
		public string GetFormattedAmount() => GetAmount().ToString(formatting, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
		public T Value => GetAmount();

		
	}
	public class MultiOperationPlayerStat<T> : NumericPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		Func<T, T, T> add;
		Func<T, T, T> mult;

		private T valueAdditive;
		private T default_valueAdditive;
		private T valueMultiplicative;
		private T default_valueMultiplicative;

		public MultiOperationPlayerStat(T default_valueAdditive, T default_valueMultiplicative, Func<T, T, T> addFunc, Func<T, T, T> multFunc,string formatting= "N0")
		{
			this.default_valueAdditive = default_valueAdditive;
			this.valueAdditive = default_valueAdditive;
			this.default_valueMultiplicative = default_valueMultiplicative;
			this.valueMultiplicative = default_valueMultiplicative;
			base.formatting = formatting;
			this.mult = multFunc;
			this.add = addFunc;
			AddStatToList();
		}

		public override void Reset()
		{
			valueAdditive = default_valueAdditive;
			valueMultiplicative = default_valueMultiplicative;
		}
		public override T GetAmount()
		{
			return mult(valueAdditive,valueMultiplicative);
		}
		public T GetAmountAfterAdding(T chngAdd)
		{
			return mult(add(valueAdditive, chngAdd), valueMultiplicative);
		}
		public T GetAmountAfterMultiplying(T chngMult)
		{
			return mult(mult(valueMultiplicative, chngMult), valueAdditive);
		}
		public T Multiply(T amount)
		{
			valueMultiplicative = mult(valueMultiplicative, amount);
			return Value;
		}
		public T Add(T amount)
		{
			valueAdditive = add(valueAdditive, amount);
			return Value;
		}
	}
	public class AdditivePlayerStat<T> : NumericPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		Func<T, T, T> add;
		private T valueAdditive;
		private T default_valueAdditive;

		public AdditivePlayerStat(T default_valueAdditive, Func<T, T, T> addFunc, string formatting = "N0")
		{
			this.default_valueAdditive = default_valueAdditive;
			this.valueAdditive = default_valueAdditive;
			base.formatting = formatting;
			this.add = addFunc;
			AddStatToList();
		}

		public T Add(T amount)
		{
			valueAdditive = add(valueAdditive, amount);
			return Value;
		}
		public T GetAmountAfterAdding(T chngAdd)
		{
			return add(valueAdditive, chngAdd);
		}
		public override void Reset()
		{
			valueAdditive = default_valueAdditive;
		}
		public override T GetAmount() => valueAdditive;
		
		}
	public class MultiplicativePlayerStat<T> : NumericPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		Func<T, T, T> mult;

		private T valueMultiplicative;
		private T default_valueMultiplicative;

		public MultiplicativePlayerStat(T default_valueMultiplicative, Func<T, T, T> multFunc, string formatting = "N0")
		{
			this.default_valueMultiplicative = default_valueMultiplicative;
			this.valueMultiplicative = default_valueMultiplicative;
			base.formatting = formatting;
			mult = multFunc;
			AddStatToList();
		}

		public T Multiply(T amount)
		{
			valueMultiplicative = mult(amount, valueMultiplicative);
			return valueMultiplicative;
		}
		public T GetAmountAfterMultiplying(T chngMult)
		{
			return mult(chngMult, valueMultiplicative);
		}
		public override void Reset()
		{
			valueMultiplicative = default_valueMultiplicative;
		}
		public override T GetAmount() => valueMultiplicative;

	}
	public class BooleanPlayerStat : CPlayerStatBase<bool>
	{
		private bool value;
		private bool default_value;

		public BooleanPlayerStat(bool default_value)
		{
			this.default_value = default_value;
			this.value = default_value;
			AddStatToList();
		}

		public override void Reset()
		{
			value = default_value;
		}
		public override bool GetAmount() => value;
		public void Set(bool newValue) => value = newValue;
	}
}