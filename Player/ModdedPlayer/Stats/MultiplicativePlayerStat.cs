using System;

namespace ChampionsOfForest.Player
{
	public class MultiplicativePlayerStat<T> : NumericPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		protected Func<T, T, T> mult;
		protected Func<T, T, T> divide;

		public T valueMultiplicative;
		protected T default_valueMultiplicative;

		public MultiplicativePlayerStat(T default_valueMultiplicative, in Func<T, T, T> multFunc, in Func<T, T, T> divideFunc, string formatting = "N0")
		{
			this.default_valueMultiplicative = default_valueMultiplicative;
			this.valueMultiplicative = default_valueMultiplicative;
			base.formatting = formatting;
			mult = multFunc;
			divide = divideFunc;
			AddStatToList();
		}

		public T Multiply(T amount)
		{
			valueMultiplicative = mult(amount, valueMultiplicative);
			return valueMultiplicative;
		}
		public T Divide(T amount)
		{
			valueMultiplicative = divide(valueMultiplicative, amount);
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
		public static T operator *(MultiplicativePlayerStat<T> a, T b)
		{
			return a.mult(a.valueMultiplicative, b);
		}
		public static T operator /(MultiplicativePlayerStat<T> a, T b)
		{
			return a.divide(a.valueMultiplicative, b);
		}
	}
}