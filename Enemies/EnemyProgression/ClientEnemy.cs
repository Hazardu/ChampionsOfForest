using System.Collections.Generic;

using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class ClientEnemy
	{
		public List<EnemyProgression.Abilities> abilities;
		public float damagemult;
		public float scale;

		public ClientEnemy(ulong id, float damage, float scale, Color color, List<EnemyProgression.Abilities> abilities)
		{
			this.damagemult = damage;
			this.abilities = abilities;
			this.scale = scale;
			if (!EnemyManager.clientEnemies.ContainsKey(id))
			{
				EnemyManager.clientEnemies.Add(id, this);
			}
			else
			{
				EnemyManager.clientEnemies.Remove(id);
				EnemyManager.clientEnemies.Add(id, this);
			}
			var entity = BoltNetwork.FindEntity(new Bolt.NetworkId(id));
			if (entity!=null)
			{
				entity.transform.localScale = Vector3.one * scale;
				try
				{
					entity.GetComponentInChildren<MeshRenderer>().material.color = color;
				}
				catch (System.Exception ex)
				{
					ModAPI.Log.Write("exception: renderer is null");
				}
				if (abilities.Contains(EnemyProgression.Abilities.Gargantuan))
					entity.BroadcastMessage("SetTriggerScale", 2.5f, SendMessageOptions.DontRequireReceiver);
				else if (abilities.Contains(EnemyProgression.Abilities.Tiny))
					entity.BroadcastMessage("SetTriggerScale", 5f, SendMessageOptions.DontRequireReceiver);
				else
					entity.BroadcastMessage("SetTriggerScale", 1.3f, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}