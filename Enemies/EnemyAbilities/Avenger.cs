using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Tools;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class Avenger : MonoBehaviour
    {
        private const int Radius = 50;

        int Stacks = 0;
        public EnemyProgression progression;
        public void ThisEnemyDied(EnemyProgression enemyProgression)
        {
            if (Stacks < 10)
            {
                if ((enemyProgression != progression) && (transform.position - enemyProgression.transform.position).sqrMagnitude < Radius * Radius)
                {
                    transform.localScale += Vector3.one * 0.05f;
                    progression.DamageMult *= 1.15f;
                    progression.AnimSpeed *= 1.05f;
                    progression.ArmorReduction /= 2;
                    Stacks++;
                }
            }
        }
    }
}
