namespace ChampionsOfForest.Player
{
	public class BooleanPlayerStat : CPlayerStatBase<bool>
	{
		public bool value;
		private readonly bool default_value;

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