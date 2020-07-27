using System;

namespace ChampionsOfForest.Player
{
	public class CPlayerStatBase<T> : CPlayerStatBase , IPlayerStat<T>
	{
		public static implicit operator T(CPlayerStatBase<T> playerStat) => playerStat.GetAmount();
		public virtual T GetAmount() => throw new NotImplementedException();
		public override void Reset() => Assert.Fail();
		public override void AddStatToList() => ModdedPlayer.instance.allStats.Add(this);
	}
}