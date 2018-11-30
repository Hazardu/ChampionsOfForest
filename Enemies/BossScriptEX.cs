using TheForest.Utils;

namespace ChampionsOfForest.Enemies
{

    //Removes boss' health cap
    public class BossScriptEX : girlMutantAiManager
    {
        public override void setupHealthParams()
        {
            if (Scene.SceneTracker.allPlayers.Count > 1)
            {
                int num = 0;
                for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
                {
                    if (Scene.SceneTracker.allPlayers[i] && (Scene.SceneTracker.allPlayers[i].transform.position - transform.position).sqrMagnitude < 122500f)
                    {
                        num++;
                    }
                }
                int num2 = setup.health.Health + setup.health.Health / 3 * num;
                EnemyHealthMod mod = (EnemyHealthMod)setup.health;
                num2 *= mod.progression.Level / 5;
                setup.health.Health = num2;
                setup.health.maxHealth = num2;
            }
        }
    }
}
