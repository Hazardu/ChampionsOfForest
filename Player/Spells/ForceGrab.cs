using UnityEngine;

namespace ChampionsOfForest.Player.Spells
{
	public class ForcePush
	{
		public static void Cast(Vector3 pos, Vector3 dir, float dist)
		{
			var hits = Physics.BoxCastAll(pos, Vector3.one, dir * dist, Quaternion.LookRotation(dir, Vector3.up), dist);
			foreach (var hit in hits)
			{
				if (hit.rigidbody != null)
				{
					//pushing away the rigidbody
				}
				if (BoltNetwork.isServer || !BoltNetwork.isRunning)
				{
					if (hit.transform.CompareTag("enemyCollide"))
					{
						//damaging the enemy
					}
				}
			}
		}
	}
}