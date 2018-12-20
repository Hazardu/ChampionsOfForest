using System;
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
                    infos[i].spell.passive(false);
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

                //Testing 

                //Spell bhole = new Spell()
                //{
                //    Bought = true,
                //    CanCast = true,
                //    BaseCooldown = 10,
                //    Description = "creates a black hole",
                //    EnergyCost = 5,
                //    icon = Texture2D.whiteTexture,
                //    ID = 1,
                //    level = 1,
                //    Levelrequirement = 1,
                //    Name = "Black Hole",
                //};
                //bhole.active = new Spell.Active(CreatePlayerBlackHole);

                SetSpell(0, SpellDataBase.spellDictionary[1]);
                SetMaxCooldowns();

            }
            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());

            }

            //MaxCooldown(0);
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
                            infos[i].Cooldown -= Time.deltaTime;
                            if (infos[i].Cooldown <= 0)
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

                for (int i = 0; i < SpellCount; i++)
                {
                    if (infos[i].spell != null)
                    {
                        string btnname = "spell" + (i + 1).ToString();
                        if (ModAPI.Input.GetButton(btnname))
                        {
                            if (Ready[i] && !ModdedPlayer.instance.Silenced && !ModdedPlayer.instance.Stunned && LocalPlayer.Stats.Energy >= infos[i].spell.EnergyCost * (1-ModdedPlayer.instance.SpellCostToStamina) && LocalPlayer.Stats.Stamina >= infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina && infos[i].spell.CanCast)
                            {
                                LocalPlayer.Stats.Stamina -= infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina;
                                LocalPlayer.Stats.Energy -= infos[i].spell.EnergyCost * (1-ModdedPlayer.instance.SpellCostToStamina);
                                Ready[i] = false;
                                MaxCooldown(i);
                                infos[i].spell.active();
                             

                            }
                         
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());
            }
        }


        public void SetMaxCooldowns()
        {
            for (int i = 0; i < 6; i++)
            {
                if (infos[i].spell != null)
                {
                    infos[i].Cooldown = infos[i].spell.BaseCooldown * ModdedPlayer.instance.CoolDownMultipier;
                }
            }
        }
        public void MaxCooldown(int i)
        {
            if (infos[i].spell != null)
            {
                infos[i].Cooldown = infos[i].spell.BaseCooldown * ModdedPlayer.instance.CoolDownMultipier;
            }
        }
        public class SpellInfo
        {
            public Spell spell;
            public float Cooldown;
        }
    }
}
