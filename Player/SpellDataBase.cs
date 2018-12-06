using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Spell bh = new Spell(1, 22, 1, 12, 5, "Black Hole", "Creates a black hole that pulls enemies in and damages them every second");
            bh.active =SpellActions.CreatePlayerBlackHole;
        bh.Bought = true;
        }
    }
    public static class SpellActions
    {

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
           ModdedPlayer.instance. AttackSpeed /= f;
        }

        #region BLACK HOLE
        public static float BLACKHOLE_damage = 50 ;
        public static float BLACKHOLE_duration = 12;
        public static float BLACKHOLE_radius = 50;
        public static float BLACKHOLE_pullforce = 25;
        public static void CreatePlayerBlackHole()
        {
            float damage = (BLACKHOLE_damage + ModdedPlayer.instance.SpellDamageBonus) * ModdedPlayer.instance.SpellAMP;
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 100);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.root != LocalPlayer.Transform.root)
                    {
                        if (Vector3.Distance(hits[i].point, LocalPlayer.Transform.position) > 3)
                        {
                            Network.NetworkManager.SendLine("SC1;" + Math.Round(hits[i].point.x, 5) + ";" + Math.Round(hits[i].point.y, 5) + ";" + Math.Round(hits[i].point.z, 5) + ";" +
                                "f;" + damage + ";" + BLACKHOLE_duration + ";" + BLACKHOLE_radius + ";" + BLACKHOLE_pullforce + ";", Network.NetworkManager.Target.Everyone);
                            return;

                        }
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
            BuffDB.AddBuff(3, 111112, 1 * ModdedPlayer.instance.SpellAMP, 3*ModdedPlayer.instance.SpellAMP);
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
