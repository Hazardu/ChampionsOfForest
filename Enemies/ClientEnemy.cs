using System.Collections.Generic;

using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class ClientEnemy
	{
		private ulong id;
		public List<EnemyProgression.Abilities> abilities;
		public float damagemult;
		public float TimeStamp;
		public bool Outdated => TimeStamp + 5 < Time.time;

		public ClientEnemy(ulong id, float damage, List<EnemyProgression.Abilities> abilities)
		{
			this.damagemult = damage;
			this.abilities = abilities;
			this.id = id;
			TimeStamp = Time.time;
			if (!EnemyManager.clientEnemies.ContainsKey(id))
			{
				EnemyManager.clientEnemies.Add(id, this);
			}
			else
			{
				EnemyManager.clientEnemies.Remove(id);

				EnemyManager.clientEnemies.Add(id, this);
			}
		}
	}
}