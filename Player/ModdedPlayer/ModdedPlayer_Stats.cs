using System;
using System.Collections;
using System.Collections.Generic;

using Bolt;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Player.BuffDB;

using Random = UnityEngine.Random;

namespace ChampionsOfForest.Player
{
	public partial class ModdedPlayer : MonoBehaviour
	{
		internal List<CPlayerStatBase> allStats = new List<CPlayerStatBase>();

		public AdditivePlayerStat<int> strength =				new AdditivePlayerStat<int>(1,			(a,b)=>a+b);
		public AdditivePlayerStat<int> intelligence =			new AdditivePlayerStat<int>(1,			(a,b)=>a+b);
		public AdditivePlayerStat<int> agility =				new AdditivePlayerStat<int>(1,			(a,b)=>a+b);
		public AdditivePlayerStat<int> vitality =				new AdditivePlayerStat<int>(1,			(a,b)=>a+b);
	
	}
}
