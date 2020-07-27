using System;

namespace ChampionsOfForest.Player
{
	public class AdditivePlayerStat<T> : NumericPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		protected Func<T, T, T> add,substract;
		public T valueAdditive;
		protected T default_valueAdditive;

		public AdditivePlayerStat(T default_valueAdditive,in Func<T, T, T> addFunc,in Func<T, T, T> substractFunc, string formatting = "N0")
		{
			this.default_valueAdditive = default_valueAdditive;
			this.valueAdditive = default_valueAdditive;
			this.formatting = formatting;
			this.add = addFunc;
			this.substract = substractFunc;
			AddStatToList();
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
		public T GetAmountAfterAdding(T chngAdd)
		{
			return add(valueAdditive, chngAdd);
		}
		public override void Reset()
		{
			valueAdditive = default_valueAdditive;
		}
		public override T GetAmount() => valueAdditive;
		public static T operator +(AdditivePlayerStat<T> a, T b)
		{
			return a.add(a.valueAdditive, b);
		}
		public static T operator -(AdditivePlayerStat<T> a, T b)
		{
			return a.substract(a.valueAdditive, b);
		}
	}
}