using ChampionsOfForest.Player;
using UnityEngine;

namespace ChampionsOfForest.Items
{
	internal class StatActions
	{


		public static void AddAllAttributes(float f)
		{
			ModdedPlayer.Stats.strength.Add(Mathf.RoundToInt(f));
			ModdedPlayer.Stats.vitality.Add(Mathf.RoundToInt(f));
			ModdedPlayer.Stats.agility.Add(Mathf.RoundToInt(f));
			ModdedPlayer.Stats.intelligence.Add(Mathf.RoundToInt(f));
		}
	}
}