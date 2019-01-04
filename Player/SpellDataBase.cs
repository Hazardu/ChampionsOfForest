using System;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public static class SpellDataBase
    {
        public static Dictionary<int, Spell> spellDictionary = new Dictionary<int, Spell>();


        public static void Initialize()
        {
            try
            {
                spellDictionary = new Dictionary<int, Spell>();
                FillSpells();
                ModAPI.Log.Write("SETUP: SPELL DB");

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        public static void FillSpells()
        {
            Spell bh = new Spell(1, 22, 15, 50, 90, "Black Hole", "Creates a black hole that pulls enemies in and damages them every second")
            {
                active = SpellActions.CreatePlayerBlackHole,
                Bought = true
            };
            Spell healingDome = new Spell(2, 22, 6, 60, 60, "Healing Dome", "Creates a sphere of vaporized aloe that heals all allies inside. Items can further expand this ability to cleanese debuffs. Scales with healing multipier and spell amplification.")
            {
                active = SpellActions.CreateHealingDome,
              
            };
            new Spell(3, 22, 3, 25, 15, "Blink", "Short distance teleportation")
            {
                active = SpellActions.DoBlink,
                
            };
        }
    }
    public static class SpellActions
    {
        public static float BlinkRange = 15;
        public static void DoBlink()
        {

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, BlinkRange);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.root != LocalPlayer.Transform.root && Vector3.Distance(hit.point, LocalPlayer.Transform.position) > 4)
                {
                    int tries=0;
                    Vector3 hitPoint = hit.point;
                    while (Physics.Raycast(hitPoint, Vector3.up, 2f) && tries < 5)
                    {
                        hitPoint += -Camera.main.transform.forward;
                        tries++;
                    }
                    if(tries < 5)
                    {
                        BlinkTowards(hitPoint);
                        return;

                    }
                   
                }
            }
            Vector3 checkPos = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * BlinkRange;
                if(Physics.Raycast(checkPos+ Vector3.up *2, Vector3.down,out RaycastHit hit1, 10f))
            {

            BlinkTowards(hit1.point + Vector3.up);
                return;
            }
            BlinkTowards(Camera.main.transform.position+ Camera.main.transform.forward * (BlinkRange-1));


        }
        private static void BlinkTowards(Vector3 point)
        {
          Vector3 vel =  LocalPlayer.Rigidbody.velocity;
            LocalPlayer.Transform.root.position = point+Vector3.up;
            LocalPlayer.Rigidbody.velocity = vel * 1.5f;
        }





        public static bool HealingDomeGivesImmunity = true;
        public static void CreateHealingDome()
        {
            Vector3 pos = LocalPlayer.Transform.position;
            float radius = 7.5f;
            float healing = (ModdedPlayer.instance.LifeRegen /4 + 3.5f) * ModdedPlayer.instance.SpellAMP * ModdedPlayer.instance.HealingMultipier;
            string immunity = "0;";
            if (HealingDomeGivesImmunity)
            {
                immunity = "1;";
            }
            float duration = 7;
            Network.NetworkManager.SendLine("SC2;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" + radius + ";" + healing + ";" + immunity + duration + ";", Network.NetworkManager.Target.Everyone);
        }

        public static void BUFF_MultMS(float f)
        {
            ModdedPlayer.instance.MoveSpeed *= f;
        }
        public static void BUFF_DivideMS(float f)
        {
            ModdedPlayer.instance.MoveSpeed /= f;
        }

        public static void BUFF_MultAS(float f)
        {
            ModdedPlayer.instance.AttackSpeed *= f;
        }
        public static void BUFF_DivideAS(float f)
        {
            ModdedPlayer.instance.AttackSpeed /= f;
        }

        #region BLACK HOLE
        public static float BLACKHOLE_damage = 40;
        public static float BLACKHOLE_duration = 7;
        public static float BLACKHOLE_radius = 12;
        public static float BLACKHOLE_pullforce = 10;
        public static void CreatePlayerBlackHole()
        {
            float damage = (BLACKHOLE_damage + ModdedPlayer.instance.SpellDamageBonus) * ModdedPlayer.instance.SpellAMP;
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 100);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.root != LocalPlayer.Transform.root)
                {
                        Network.NetworkManager.SendLine("SC1;" + Math.Round(hits[i].point.x, 5) + ";" + Math.Round(hits[i].point.y, 5) + ";" + Math.Round(hits[i].point.z, 5) + ";" +
                            "f;" + damage + ";" + BLACKHOLE_duration + ";" + BLACKHOLE_radius + ";" + BLACKHOLE_pullforce + ";", Network.NetworkManager.Target.Everyone);
                        return;
                }
            }
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 50;
            Network.NetworkManager.SendLine("SC1;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" +
                       "f;" + damage + ";" + BLACKHOLE_duration + ";" + BLACKHOLE_radius + ";" + BLACKHOLE_pullforce + ";", Network.NetworkManager.Target.Everyone);
        }
        #endregion
        #region CLEAVE
        public static bool IsCleaveEquipped;
        public static bool IsCleaveEmpowered;
        public static float CleaveEmpowerAmount;
        public static void CleavePassive(bool b)
        {
            IsCleaveEquipped = b;
        }
        public static void CleaveActive()
        {
            BuffDB.AddBuff(3, 111112, 1 * ModdedPlayer.instance.SpellAMP, 3 * ModdedPlayer.instance.SpellAMP);
        }
        public static void StartCleaveEmpower(float f)
        {
            CleaveEmpowerAmount = f;
            IsCleaveEmpowered = true;
        }
        public static void StopCleaveEmpower(float f)
        {
            IsCleaveEmpowered = false;
        }
        #endregion


    }
}
