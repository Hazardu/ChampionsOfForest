using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class EnemyFallingFix : mutantAnimatorControl
	{
		protected override void Update()
		{
			if (longFallTimer < Time.time - 1.3f)
			{
				longFallTimer = Time.time - 1.1f;
			}
			base.Update();
		}
	}
}