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
            if (spell == null)
            {
                infos[i].spell.EquippedSlot = -1;
                infos[i].spell = null;
            }
            else
            {
                spell.EquippedSlot = i;
                infos[i].spell = spell;
            }
            SetMaxCooldowns();
        }

        private void Start()
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
                Spell bhole = new Spell()
                {
                    Bought = true,
                    CanCast = true,
                    BaseCooldown = 10,
                    Description = "creates a black hole",
                    EnergyCost = 5,
                    icon = Texture2D.whiteTexture,
                    ID = 1,
                    level = 1,
                    Levelrequirement = 1,
                    Name = "Black Hole",
                };
                bhole.active = new Spell.Active(CreatePlayerBlackHole);
                SetSpell(0, bhole);
                MaxCooldown(0);
                  }

        public void CreatePlayerBlackHole()
        {
            try
            {
                float damage = 30 * ModdedPlayer.instance.SpellAMP;
                float duration = 12;
                float radius = 50;
                float pullforce = 25;
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 100);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.root != LocalPlayer.Transform.root)
                    {
                        if (Vector3.Distance(hits[i].point, LocalPlayer.Transform.position) > 3)
                        {
                            Network.NetworkManager.SendLine("SC1;" + Math.Round(hits[i].point.x, 5) + ";" + Math.Round(hits[i].point.y, 5) + ";" + Math.Round(hits[i].point.z, 5) + ";" +
                                "f;" + damage + ";" + duration + ";" + radius + ";" + pullforce + ";", Network.NetworkManager.Target.Everyone);
                            return;

                        }
                    }
                }
                Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 50;
                Network.NetworkManager.SendLine("SC1;" + Math.Round(pos.x, 5) + ";" + Math.Round(pos.y, 5) + ";" + Math.Round(pos.z, 5) + ";" +
                           "f;" + damage + ";" + duration + ";" + radius + ";" + pullforce + ";", Network.NetworkManager.Target.Everyone);
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
                            infos[i].Cooldown -= Time.deltaTime;
                            if (infos[i].Cooldown <= 0)
                            {
                                infos[i].Cooldown = 0;
                                Ready[i] = true;
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
                            if (Ready[i] && !ModdedPlayer.instance.Silenced && !ModdedPlayer.instance.Stunned && LocalPlayer.Stats.Stamina >= infos[i].spell.EnergyCost && infos[i].spell.CanCast)
                            {
                                Ready[i] = false;
                                MaxCooldown(i);
                                infos[i].spell.active();
                                LocalPlayer.Stats.Stamina -= infos[i].spell.EnergyCost;

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
                    infos[i].Cooldown = infos[i].spell.BaseCooldown;
                }
            }
        }
        public void MaxCooldown(int i)
        {
            if (infos[i].spell != null)
            {
                infos[i].Cooldown = infos[i].spell.BaseCooldown;
            }
        }
        public class SpellInfo
        {
            public Spell spell;
            public float Cooldown;
        }
    }
}
