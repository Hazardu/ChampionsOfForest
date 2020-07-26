using System;

namespace ChampionsOfForest.Player
{
	public class NumericPlayerStatBase<T> : CPlayerStatBase<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
	{
		protected string formatting;
		public string GetFormattedAmount() => GetAmount().ToString(formatting, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
		public T Value => GetAmount();

		public override string ToString() => GetFormattedAmount();

	}
}