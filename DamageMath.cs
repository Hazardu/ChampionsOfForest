using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest
{
    public static class DamageMath
    {
        /// <summary>
        /// Takes a float input damage and returns a integer that is smaller than int.maxvalue and amount of times it has to be sent.
        /// </summary>
        public static void DamageClamp(float damage, out int outdamage, out int repetitions)
        {
            if(damage < int.MaxValue/5)
            {
                outdamage = Mathf.FloorToInt(damage);
                repetitions = 1;
                return;
            }
            repetitions = Mathf.FloorToInt(damage / ((float)int.MaxValue/5f)) + 1;
            outdamage = Mathf.RoundToInt(damage / repetitions);
        }
    }
}
