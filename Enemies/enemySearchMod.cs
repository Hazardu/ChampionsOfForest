using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class enemySearchMod : mutantSearchFunctions
	{
		private float tauntEndTimestamp;
		private GameObject tauntingPlayer;
		private bool isTaunted =>  tauntEndTimestamp < Time.time;
		public void Taunt(GameObject go,in float duration)
		{
			tauntEndTimestamp = Time.time + duration;
			tauntingPlayer = go;
			switchToNewTarget(go);
			this.setup.aiManager.setAggressiveCombat();
			this.setup.pmBrain.SendEvent("toSetAggressive");

		}
		public override void updateClosePlayerTarget()
		{
			if (this.currentTarget)
			{
				if (tauntingPlayer)
				{
					if (isTaunted)
					{
						if (currentTarget != tauntingPlayer)
							switchToNewTarget(tauntingPlayer);
						this.setup.aiManager.setAggressiveCombat();
						this.setup.pmBrain.SendEvent("toSetAggressive");
					}
				}
				this.currentTargetDist = Vector3.Distance(this.tr.position, this.currentTarget.transform.position);
			}
			if (!this.currentTarget)
			{
				this.currentTarget = this.currentWaypoint;
			}
		}

		public override void switchToNewTarget(GameObject go)
		{
			if (tauntingPlayer)
			{
				if (go != tauntingPlayer)
				{
					if (!isTaunted)
					{
						tauntingPlayer = null;
					}
					else
					{
						return;
					}
				}
			}
			base.switchToNewTarget(go);
		}
	}
}
