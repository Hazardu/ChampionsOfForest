using ChampionsOfForest.Effects;
using System;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public static class SpellActions
    {
        public static float BlinkRange = 15;
        public static float BlinkDamage = 0;
        public static void DoBlink()
        {

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, BlinkRange);
            foreach (RaycastHit hit in hits)
            {
                if (BlinkDamage != 0)
                {
                    if (hit.transform.root.CompareTag("enemyCollide"))
                    {
                        float dmg = BlinkDamage + ModdedPlayer.instance.SpellDamageBonus / 5;
                        dmg *= ModdedPlayer.instance.SpellAMP;
                        int dmgInt = Mathf.RoundToInt(dmg);
                        if (GameSetup.IsMpClient)
                        {
                            BoltEntity enemyEntity = hit.transform.root.GetComponent<BoltEntity>();
                            if (enemyEntity == null)
                            {
                                enemyEntity = hit.transform.root.GetComponentInChildren<BoltEntity>();
                            }

                            if (enemyEntity != null)
                            {
                                PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(enemyEntity);
                                playerHitEnemy.hitFallDown = true;
                                playerHitEnemy.Hit = dmgInt;
                                playerHitEnemy.Send();

                            }
                        }
                        else
                        {
                            hit.transform.SendMessageUpwards("Hit", dmgInt, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                if (hit.transform.root != LocalPlayer.Transform.root && Vector3.Distance(hit.point, LocalPlayer.Transform.position) > 4)
                {
                    int tries = 0;
                    Vector3 hitPoint = hit.point;
                    while (Physics.Raycast(hitPoint, Vector3.up, 2f) && tries < 5)
                    {
                        hitPoint += -Camera.main.transform.forward;
                        tries++;
                    }
                    if (tries < 5)
                    {
                        BlinkTowards(hitPoint);
                        return;

                    }

                }
            }
            Vector3 checkPos = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * BlinkRange;
            if (Physics.Raycast(checkPos + Vector3.up * 2, Vector3.down, out RaycastHit hit1, 10f))
            {

                BlinkTowards(hit1.point + Vector3.up);
                return;
            }
            BlinkTowards(Camera.main.transform.position + Camera.main.transform.forward * (BlinkRange - 1));


        }
        private static void BlinkTowards(Vector3 point)
        {
            Vector3 vel = LocalPlayer.Rigidbody.velocity;
            LocalPlayer.Transform.root.position = point + Vector3.up;
            LocalPlayer.Rigidbody.velocity = vel * 1.5f;
        }





        public static bool HealingDomeGivesImmunity = false;
        public static void CreateHealingDome()
        {
            Vector3 pos = LocalPlayer.Transform.position;
            float radius = 8.5f;
            float healing = (ModdedPlayer.instance.LifeRegen + 13.5f) * ModdedPlayer.instance.SpellAMP * ModdedPlayer.instance.HealingMultipier;
            string immunity = "0;";
            if (HealingDomeGivesImmunity)
            {
                immunity = "1;";
            }
            float duration = 8;
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
            ModdedPlayer.instance.AttackSpeedMult *= f;
        }
        public static void BUFF_DivideAS(float f)
        {
            ModdedPlayer.instance.AttackSpeedMult /= f;
        }
        #region FLARE

        public static float FlareDamage = 10;
        public static float FlareSlow = 0.5f;
        public static float FlareBoost = 1.35f;
        public static float FlareHeal = 5;
        public static float FlareRadius = 4.5f;
        public static float FlareDuration = 8;
        public static void CastFlare()
        {
            Vector3 dir = LocalPlayer.Transform.position;
            float dmg = FlareDamage + ModdedPlayer.instance.SpellDamageBonus / 3;
            dmg *= ModdedPlayer.instance.SpellAMP;
            float slow = FlareSlow;
            float boost = FlareBoost;
            float duration = FlareDuration;
            float radius = FlareRadius;
            float Healing = FlareHeal + ModdedPlayer.instance.SpellDamageBonus / 20 + (ModdedPlayer.instance.LifeRegen / 1.2f) * ModdedPlayer.instance.HealthRegenPercent;
            Healing *= ModdedPlayer.instance.SpellAMP;

            Network.NetworkManager.SendLine("SC3;" + dir.x + ";" + dir.y + ";" + dir.z + ";" + "f;" + dmg + ";" + Healing + ";" + slow + ";" + boost + ";" + duration + ";" + radius + ";", Network.NetworkManager.Target.Everyone);
        }
        #endregion
        #region BLACK HOLE
        public static float BLACKHOLE_damage = 40;
        public static float BLACKHOLE_duration = 7;
        public static float BLACKHOLE_radius = 12;
        public static float BLACKHOLE_pullforce = 10;
        public static void CreatePlayerBlackHole()
        {
            float damage = (BLACKHOLE_damage + ModdedPlayer.instance.SpellDamageBonus / 7) * ModdedPlayer.instance.SpellAMP;
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

        #region SustainShield
        public static float ShieldPerSecond = 1;
        public static float MaxShield = 10;
        public static float ShieldCastTime;
        public static float ShieldPersistanceLifetime = 20;
        public static void CastSustainShieldActive()
        {
            float max = MaxShield + ModdedPlayer.instance.SpellDamageBonus / 2;
            max *= ModdedPlayer.instance.SpellAMP;
            float gain = ShieldPerSecond + ModdedPlayer.instance.SpellDamageBonus / 20;
            gain *= ModdedPlayer.instance.SpellAMP;
            ModdedPlayer.instance.damageAbsorbAmounts[1] = Mathf.Clamp(ModdedPlayer.instance.damageAbsorbAmounts[1] + Time.deltaTime * gain, 0, max);
            ShieldCastTime = Time.time;
        }
        public static void CastSustainShielPassive(bool on)
        {
            if (!on)
            {
                return;
            }

            if (ModdedPlayer.instance.damageAbsorbAmounts[1] > 0)
            {
                if (ShieldCastTime + ShieldPersistanceLifetime < Time.time)
                {
                    float loss = Time.deltaTime * (ShieldPerSecond + ModdedPlayer.instance.SpellDamageBonus / 5) * 5 * ModdedPlayer.instance.SpellDamageBonus;
                    ModdedPlayer.instance.damageAbsorbAmounts[1] = Mathf.Max(0, ModdedPlayer.instance.damageAbsorbAmounts[1] - loss);
                }
            }
        }
        #endregion


        #region WarCry
        public static float WarCryRadius = 50;
        public static bool WarCryGiveDamage = false;
        public static bool WarCryGiveArmor = false;
        public static int WarCryArmor => ModdedPlayer.instance.Armor / 10;
        public static void CastWarCry()
        {
            WarCry.GiveEffect(WarCryGiveDamage, WarCryGiveArmor, WarCryArmor);
            WarCry.SpawnEffect(LocalPlayer.Transform.position, WarCryRadius);
            if (BoltNetwork.isRunning)
            {
                Vector3 pos = LocalPlayer.Transform.position;
                string s = "SC5;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" + WarCryRadius + ";";
                if (WarCryGiveDamage)
                {
                    s += "t;";
                }
                else
                {
                    s += "f;";
                }

                if (WarCryGiveArmor)
                {
                    s += "t;" + WarCryArmor;
                }
                else
                {
                    s += "f;";
                }

                Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
            }
        }

        #endregion
        public static float PortalDuration = 30;
        public static void CastPortal()
        {
            Vector3 pos = LocalPlayer.Transform.position + LocalPlayer.Transform.forward * 6;
            int id = Portal.GetPortalID();
            try
            {
                Portal.CreatePortal(pos, PortalDuration, id, LocalPlayer.IsInCaves, LocalPlayer.IsInEndgame);

            }
            catch (Exception e)
            {
                ModAPI.Log.Write(e.ToString());

            }

            if (BoltNetwork.isRunning)
            {
                Portal.SyncTransform(pos, PortalDuration, id, LocalPlayer.IsInCaves, LocalPlayer.IsInEndgame);
            }
        }


        public static bool MagicArrowDmgDebuff = false;
        public static bool MagicArrowDoubleSlow = false;
        public static float MagicArrowDuration = 3f;
        public static void CastMagicArrow()
        {
            float damage = 55 + ModdedPlayer.instance.SpellDamageBonus * 1.3f;
            damage = damage * ModdedPlayer.instance.SpellAMP;
            Vector3 pos = Camera.main.transform.position;
            Vector3 dir = Camera.main.transform.forward;
            if (GameSetup.IsSinglePlayer || GameSetup.IsMpServer)
            {
                MagicArrow.Create(pos, dir, damage, ModReferences.ThisPlayerID, MagicArrowDuration, MagicArrowDoubleSlow, MagicArrowDmgDebuff);
                if (BoltNetwork.isRunning)
                {
                    string s = "SC7;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" + Math.Round(dir.x, 5) + ";" + Math.Round(dir.y, 5) + ";" + Math.Round(dir.z, 5) + ";";
                    s += damage + ";" + ModReferences.ThisPlayerID + ";" + MagicArrowDuration + ";";
                    if (MagicArrowDoubleSlow)
                    {
                        s += "t;";
                    }
                    else
                    {
                        s += "f;";
                    }

                    if (MagicArrowDmgDebuff)
                    {
                        s += "t;";
                    }
                    else
                    {
                        s += "f;";
                    }

                    Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
                }
            }
            else if (GameSetup.IsMpClient)
            {
                MagicArrow.CreateEffect(pos, dir, MagicArrowDmgDebuff, MagicArrowDuration);
                string s = "SC7;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" + Math.Round(dir.x, 5) + ";" + Math.Round(dir.y, 5) + ";" + Math.Round(dir.z, 5) + ";";
                s += damage + ";" + ModReferences.ThisPlayerID + ";" + MagicArrowDuration + ";";
                if (MagicArrowDoubleSlow)
                {
                    s += "t;";
                }
                else
                {
                    s += "f;";
                }

                if (MagicArrowDmgDebuff)
                {
                    s += "t;";
                }
                else
                {
                    s += "f;";
                }

                Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
            }

        }


        public static void ToggleMultishot()
        {
            Multishot.IsOn = !Multishot.IsOn;
            Multishot.localPlayerInstance.SetActive(Multishot.IsOn);
        }


        public static float PurgeRadius = 14;
        public static bool PurgeHeal = false;
        public static void CastPurge()
        {
            Vector3 pos = LocalPlayer.Transform.position;

            Purge.Cast(pos, PurgeRadius,PurgeHeal);

            if (BoltNetwork.isRunning)
            {
                string s = "SC8;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" + PurgeRadius+";" + (PurgeHeal?"1;":"0;");
                Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
            }
        }

        public static float SnapFreezeDist = 22;
        public static float SnapFloatAmount = 0.2f;
        public static float SnapFreezeDuration = 12f;
        public static void CastSnapFreeze()
        {
            Vector3 pos = LocalPlayer.Transform.position;
            float dmg = 23 + ModdedPlayer.instance.SpellDamageBonus;
            dmg *= ModdedPlayer.instance.SpellAMP;
            string s = "SC9;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" + SnapFreezeDist + ";" + SnapFloatAmount + ";" + SnapFreezeDuration + ";" + dmg + ";";
            Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Everyone);
        }

        public static float BL_Damage = 150;
        public static void CastBallLightning()
        {
            float dmg = BL_Damage + (4 * ModdedPlayer.instance.SpellDamageBonus);
            dmg *= ModdedPlayer.instance.SpellAMP;


            Vector3 pos = LocalPlayer.Transform.position + LocalPlayer.Transform.forward;
            Vector3 speed = Camera.main.transform.forward;

            speed.y = 0;
            speed.Normalize();
            speed *= 3;

            string s = "SC10;" +
                Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" +
                Math.Round(speed.x, 5) + ";" + Math.Round(speed.y, 5) + ";" + Math.Round(speed.z, 5) + ";" +
                dmg + ";";
            Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Everyone);

        }

    }
}
