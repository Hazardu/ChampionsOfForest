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
            aura.damage = dmg;
            aura.isOn = true;
            CotfUtils.Log("FireAura dmg = " + dmg);
        }

        bool isOn = false;
        float damage;

       void OnDisable()
        {
            isOn = false;
        }

        public void TurnOn()
        {
            isOn = true;
        }

     void Update()
        {
            if (isOn)
            {
                if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < 49)
                {
                    LocalPlayer.Stats.Health -= Time.deltaTime * damage * ModdedPlayer.instance.DamageReductionTotal * (1 - ModdedPlayer.instance.ArmorDmgRed) * (1-ModdedPlayer.instance.MagicResistance);
                }
              
            }
        }
    }
}
