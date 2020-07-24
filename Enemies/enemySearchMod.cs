using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class enemySearchMod : mutantSearchFunctions
	{
		private float tauntEndTimestamp;
		private GameObject tauntingPlayer;
		private bool isTaunted => tauntEndTimestamp < Time.time;

		public void Taunt(GameObject go, in float duration)
		{
			setup.ai.resetCombatParams();

			tauntEndTimestamp = Time.time + duration;
			tauntingPlayer = go;
			switchToNewTarget(go);
			
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
						{
							switchToNewTarget(tauntingPlayer);
							this.setup.aiManager.setAggressiveCombat();
							this.setup.pmBrain.SendEvent("toSetAggressive");
							this.setup.pmCombat.enabled = true;
							this.setup.aiManager.setCaveCombat();   //the most agressive combat mode
							this.setup.pmBrain.SendEvent("toActivateFSM");
							this.setup.pmBrain.FsmVariables.GetFsmBool("playerIsRed").Value = false;
						}
						if (this.setup.aiManager)
						{
							this.setup.aiManager.flee = false;
						}
						if (this.setup.pmBrain)
						{
						}
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