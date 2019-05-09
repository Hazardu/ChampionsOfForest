using ChampionsOfForest.Fun;
using System;
using System.Collections.Generic;
using System.Linq;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class SpellCaster : MonoBehaviour
    {
        #region Instance
        public static SpellCaster instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion



        public SpellInfo[] infos;
        public bool[] Ready;
        public static int SpellCount = 6;


        public void SetSpell(int i, Spell spell = null)
        {
            if (infos[i].spell != null)
            {
                if (infos[i].spell.passive != null)
                {
                    infos[i].spell.passive(false);
                }

                infos[i].spell.EquippedSlot = -1;
                infos[i].spell = null;
            }

            else if (spell != null)
            {
                infos[i].spell = spell;
                infos[i].spell.EquippedSlot = i;
            }

            if (infos[i].spell != null)
            {
                if (infos[i].spell.passive != null)
                {
                    infos[i].spell.passive(true);
                }
            }
            SetMaxCooldowns();
        }

        private void Start()
        {
            try
            {
                infos = new SpellInfo[SpellCount];
                Ready = new bool[SpellCount];
                for (int i = 0; i < SpellCount; i++)
                {
                    infos[i] = new SpellInfo()
                    {
                        spell = null
                    };
                }
                SetMaxCooldowns();
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());

            }
        }




        private void Update()
        {
            try
            {
                for (int i = 0; i < SpellCount; i++)
                {
                    if (!Ready[i])
                    {

                        if (infos[i].spell != null)
                        {
                            infos[i].Cooldown -= Time.deltaTime/ ModdedPlayer.instance.CoolDownMultipier;
                            if (infos[i].Cooldown <= 0 || CotfCheats.Cheat_noCooldowns)
                            {
                                infos[i].Cooldown = 0;
                                Ready[i] = true;
                                infos[i].spell.CanCast = true;
                            }
                        }
                        else
                        {
                            Ready[i] = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write("Error1 \t"+ex.ToString());
            }
            try { 
            bool[] castedSpells = new bool[SpellCount];
                for (int i = 0; i < SpellCount; i++)
                {
                    if (infos[i].spell != null)
                    {
                        
                        if (infos[i].spell.usePassiveOnUpdate)
                        {
                            if (infos[i].spell.passive != null)
                                infos[i].spell.passive(true);
                        }



                        string btnname = "spell" + (i + 1).ToString();
                        if ((infos[i].spell.active != null)&& ModAPI.Input.GetButton(btnname))
                        {
                            if (!infos[i].spell.Channeled)
                            {
                                if (Ready[i] && !ModdedPlayer.instance.Silenced && !ModdedPlayer.instance.Stunned && LocalPlayer.Stats.Energy >= infos[i].spell.EnergyCost * (1 - ModdedPlayer.instance.SpellCostToStamina) * ModdedPlayer.instance.SpellCostRatio && LocalPlayer.Stats.Stamina >= infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina * ModdedPlayer.instance.SpellCostRatio && infos[i].spell.CanCast)
                                {
                                    LocalPlayer.Stats.Energy -= infos[i].spell.EnergyCost * (1 - ModdedPlayer.instance.SpellCostToStamina) *  ModdedPlayer.instance.SpellCostRatio;
                                    if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
                                        LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
                                    LocalPlayer.Stats.Stamina -= infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina *ModdedPlayer.instance.SpellCostRatio;

                                        InfinityCooldownReduction();
                                    Ready[i] = false;
                                    MaxCooldown(i);
                                    infos[i].spell.active();
                                    castedSpells[i] = true;

                                }
                            }
                            else 
                            {
                                if (Ready[i] && !ModdedPlayer.instance.Silenced && !ModdedPlayer.instance.Stunned)
                                {
                                    if (LocalPlayer.Stats.Energy >= 10 && LocalPlayer.Stats.Stamina >= 10 && infos[i].spell.CanCast)
                                    {
                                        LocalPlayer.Stats.Energy -= Time.deltaTime * infos[i].spell.EnergyCost * (1 - ModdedPlayer.instance.SpellCostToStamina) * ModdedPlayer.instance.SpellCostRatio;
                                        if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
                                            LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
                                        LocalPlayer.Stats.Stamina -= Time.deltaTime * infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina * ModdedPlayer.instance.SpellCostRatio;

                                        infos[i].spell.active();
                                        infos[i].spell.ChanneledTime += Time.deltaTime;
                                        castedSpells[i] = true;
                                        infos[i].spell.active();

                                    }
                                    else
                                    {
                                        MaxCooldown(i);
                                    }
                                }
                            }

                        }
                    }
                    if (!castedSpells[i]&& infos[i].spell!= null)
                    {
                        infos[i].spell.ChanneledTime = 0;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                ModAPI.Log.Write(ex.ToString());
            }
        }


        public void SetMaxCooldowns()
        {
            for (int i = 0; i < 6; i++)
            {
                MaxCooldown(i);
            }
        }
        public void MaxCooldown(int i)
        {
            if (infos[i].spell != null)
            {
                infos[i].Cooldown = infos[i].spell.BaseCooldown;
                Ready[i] = false;

            }
        }
        public class SpellInfo
        {
            public Spell spell;
            public float Cooldown;
        }

        #region InfinityPerk
        public static bool InfinityEnabled = false;
        public void InfinityCooldownReduction()
        {
            if (!InfinityEnabled) return;
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i].Cooldown *= 0.9f;
            }
        }
        #endregion
        public static bool InfinityLoopEnabled = false;
        public static void InfinityLoopEffect()
        {
            if (InfinityLoopEnabled)
            {
                var keys = new List<int>();
                for (int i = 0; i < SpellCount; i++)
                {
                    if (instance.infos[i].Cooldown > 0) keys.Add(i);
                }
                if (keys.Count == 0) return;
                int randomI = UnityEngine.Random.Range(0, keys.Count);
                instance.infos[keys[randomI]].Cooldown--;
            }
            
        }
                 
        public static bool RemoveStamina(float cost)
        {
            float realcostE = cost * (1 - ModdedPlayer.instance.SpellCostToStamina) * ModdedPlayer.instance.SpellCostRatio;
            float realcostS = cost * ModdedPlayer.instance.SpellCostToStamina * ModdedPlayer.instance.SpellCostRatio;
            if (LocalPlayer.Stats.Energy < realcostE && LocalPlayer.Stats.Stamina < realcostS) return false;
            LocalPlayer.Stats.Energy -= realcostE;
            if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
                LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
            LocalPlayer.Stats.Stamina -= realcostS;
            return true;
        }
    }
}
