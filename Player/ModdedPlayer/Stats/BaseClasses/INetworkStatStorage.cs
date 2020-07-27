using System.Collections.Generic;

namespace ChampionsOfForest.Player
{
	public interface INetworkStatStorage<T>
	{
		Dictionary<string,T> OtherPlayerValues { get; set; }
	}
}