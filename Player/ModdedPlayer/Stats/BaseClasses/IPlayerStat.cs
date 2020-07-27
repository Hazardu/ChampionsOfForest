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
}