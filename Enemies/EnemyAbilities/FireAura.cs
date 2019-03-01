using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class FireAura : MonoBehaviour
    {
        public static void Cast(GameObject go, float dmg)
        {
            var aura = go.GetComponent<FireAura>();
            if (aura == null)
            {
                aura = go.AddComponent<FireAura>();
            }
            aura.TurnOn(dmg);
        }


       void OnDisable()
        {
            StopAllCoroutines();
        }

        public void TurnOn(float dmg)
        {
            StopAllCoroutines();
            StartCoroutine(FireAuraCooroutine(dmg));
        }

        private IEnumerator FireAuraCooroutine(float dmg)
        {
            while (true)
            {
                if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < 80)
                {
                    LocalPlayer.Stats.Health -= Time.deltaTime * dmg * ModdedPlayer.instance.DamageReductionTotal * (1 - ModdedPlayer.instance.ArmorDmgRed) * (1-ModdedPlayer.instance.MagicResistance);
                }
                yield return null;
            }
        }
    }
}
