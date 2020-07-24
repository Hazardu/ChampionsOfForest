using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class Avenger : MonoBehaviour
	{
		private const int Radius = 50;

		public int Stacks = 0;
		public EnemyProgression progression;

		public void ThisEnemyDied(EnemyProgression enemyProgression)
		{
			if (Stacks < 10)
			{
				if ((enemyProgression != progression) && (transform.position - enemyProgression.transform.position).sqrMagnitude < Radius * Radius)
				{
					transform.localScale += Vector3.one * 0.1f;
					progression.BaseDamageMult *= 1.25f;
					progression.BaseAnimSpeed *= 1.05f;
					progression.ArmorReduction = 0;
					LocalPlayer.Sfx.PlayWokenByEnemies();
					Stacks++;
				}
			}
		}
	}
}