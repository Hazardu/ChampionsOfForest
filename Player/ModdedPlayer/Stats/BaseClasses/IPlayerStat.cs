namespace ChampionsOfForest.Player
{
	public interface IPlayerStat<T>
	{
		T GetAmount();
		void Reset();

	}
}