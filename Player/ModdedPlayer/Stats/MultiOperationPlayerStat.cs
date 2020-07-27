using System;

namespace ChampionsOfForest.Player
{
	public class MultiOperationPlayerStat<T> : NumericPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		protected Func<T, T, T> add;
		protected Func<T, T, T> substract;
		protected Func<T, T, T> mult;
		protected Func<T, T, T> divide;

		public T valueAdditive;
		protected T default_valueAdditive;
		public T valueMultiplicative;
		protected T default_valueMultiplicative;

		public MultiOperationPlayerStat(T default_valueAdditive, T default_valueMultiplicative, in Func<T, T, T> addFunc, in Func<T, T, T> substractFunc, in Func<T, T, T> multFunc, in Func<T, T, T> divideFunc,string formatting= "N0")
		{
			this.default_valueAdditive = default_valueAdditive;
			this.valueAdditive = default_valueAdditive;
			this.default_valueMultiplicative = default_valueMultiplicative;
			this.valueMultiplicative = default_valueMultiplicative;
			base.formatting = formatting;
			this.mult = multFunc;
			this.divide = divideFunc;
			this.add = addFunc;
			this.substract = substractFunc;
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
		public T Divide(T amount)
		{
			valueMultiplicative = divide(valueMultiplicative, amount);
			return Value;
		}
		public T Add(T amount)
		{
			valueAdditive = add(valueAdditive, amount);
			return Value;
		}
		public T Substract(T amount)
		{
			valueAdditive = substract(valueAdditive, amount);
			return Value;
		}

		public static T operator +(MultiOperationPlayerStat<T> a, T b)
		{
			return a.add(a.valueAdditive, b);
		}
		public static T operator -(MultiOperationPlayerStat<T> a, T b)
		{
			return a.substract(a.valueAdditive, b);
		}
		public static T operator *(MultiOperationPlayerStat<T> a, T b)
		{
			return a.mult(a.valueMultiplicative, b);
		}
		public static T operator /(MultiOperationPlayerStat<T> a, T b)
		{
			return a.divide(a.valueMultiplicative, b);
		}
	}
}