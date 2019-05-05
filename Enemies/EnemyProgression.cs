using Bolt;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Network;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
using Math = System.Math;
using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
    public class EnemyProgression : MonoBehaviour
    {
        //Animation slows
        public Dictionary<int, EnemyDebuff> slows;
        public Dictionary<int, EnemyDebuff> dmgTakenDebuffs;
        public Dictionary<int, EnemyDebuff> dmgDealtDebuffs;
        public Dictionary<int, EnemyDebuff> FireDamageDebuffs;
        public List<DoT> DamageOverTimeList;
        public struct DoT
        {
            /// <summary>
            /// Amount of damage dealt second
            /// </summary>
            public int Amount;

            /// <summary>
            /// Timestamp when stoptime needs to be deleted;
            /// </summary>
            public float StopTime;

            public DoT(int Damage, float duration)
            {
                Amount = Damage;
                StopTime = Time.time + duration;
            }
        }
    
        public float DebuffDmgMult;
        public float dmgTakenIncrease;


        //Type of enemy
        public enum EnemyRarity { Normal, Elite, Miniboss, Boss }
        public EnemyRarity _rarity;

        //References
        public EnemyHealth _Health;
        public mutantAI _AI;
        public BoltEntity entity;
        public mutantScriptSetup setup;


        //Variables
        #region Variables
        public string EnemyName;

        public int Level;
        public int Armor;
        public int ArmorReduction;

        public int Health { get { return (int)_hp; } set { _Health.Health = value; _hp = value; } }
        public float _hp;
        public float MaxHealth;
        private int BaseHealth = 0;

        public float DamageMult;
        public float BaseDamageMult;
        public float DamageAmp => DamageMult;
        public float FireDmgAmp = 1;
        public float FireDmgBonus;

        public long Bounty;

        public float Steadfast = 100;
        private int SteadfastCap = 100000;
        public bool setupComplete = false;
        public bool CCimmune = false;
        private bool DualLifeSpend = false;
        public bool OnDieCalled = false;
        public List<Abilities> abilities;
        public Enemy enemyType;
        public float CreationTime;
        public float AnimSpeed = 1;
        private float BaseAnimSpeed = 1;
        private float prerainDmg;
        private int prerainArmor;
        private readonly float agroRange = 1200;

        //cooldowns
        private float freezeauraCD;
        private float blackholeCD;
        private float blinkCD;
        private float shieldingCD;
        private float shieldingON;
        private float StunCD;
        private float TrapCD;
        private float LaserCD;
        private float MeteorCD;
        private float BeamCD;

        //Closest player, for detecting if in range to cast abilities
        private GameObject closestPlayer;
        private float closestPlayerMagnitude;

        private float timeOfDeath;

        private Color normalColor;

        public enum Abilities { Steadfast, BossSteadfast, EliteSteadfast, FreezingAura, FireAura, Rooting, BlackHole, Trapper, Juggernaut, Huge, Tiny, ExtraDamage, ExtraHealth, Basher, Blink, RainEmpowerement, Shielding, Meteor, Flare, DoubleLife, Laser, Poisonous, Avenger, Sacrifice }
        public enum Enemy { RegularArmsy, PaleArmsy, RegularVags, PaleVags, Cowman, Baby, Girl, Worm, Megan, NormalMale, NormalLeaderMale, NormalFemale, NormalSkinnyMale, NormalSkinnyFemale, PaleMale, PaleSkinnyMale, PaleSkinnedMale, PaleSkinnedSkinnyMale, PaintedMale, PaintedLeaderMale, PaintedFemale, Fireman };
        #endregion

        #region Name
        public static string[] Mnames = new string[]
              {
            "Farket",
            "Hazard",
            "Moritz",
            "Souldrinker",
            "Olivier Broadbent",
            "Subscribe to Pewds",
            "Kutie",
            "Axt",
            "Fionera",
            "Cleetus",
            "Kaspito",
            "SiXxKiLuR",
            "Hellsing",
            "Metamoth",
            "Teledorktronics",
            "SmokeyThePerson",
            "NightOwl",
            "PuffedRice",
            "PhoenixDeath",
            "Lyon the weeb",
            "Danny Parker",
            "Kaspito",
            "Lukaaa",
            "Chefen",
            "Lauren",
            "DrowsyCob",
            "Ali",
            "ArcKaino",
            "Calean",
            "LordSidonus",
            "DTfang",
            "Malкae",
            "R3iGN",
            "Torsin",
            "θฯ12",
            "Иio",
            "Komissar bAv",
            "The Strange Man",
            "Micha",
            "MiikaHD",
            "NÜT",
            "AssPirate",
            "Azpect",
            "LumaR",
            "TeigRolle",
            "Foreck",
            "Gaullin",
            "Alichipmunk",
            "Chad",
            "Blight",
            "Cheddar",
            "CHUNGUS",
            "Do you want anything from MC Donalds?\n🅱orgar",
            "MaVladd",
            "Wren",
            "Ross Draws",
            "Sam Gorski",
            "Mike Diva",
            "Niko Pueringer",
            "Freddy Wong",
            "PewDiePie",
            "Salad Ass",
            "Morgan Page",
            "Hex Cougar",
            "Unlike Pluto",
            "Samuel","Sebastian","David","Carter","Wyatt","Jayden","John","Owen","Dylan",
            "Luke","Gabriel","Anthony","Isaac","Grayson","Jack","Julian","Levi",
            "Christopher","Joshua","Andrew","Lincoln","Mateo","Ryan","Jaxon",

              };
        /// <summary>
        /// Picks a random name for the enemy
        /// </summary>
        private void RollName()
        {
            List<string> names = new List<string>();
            string prefix = "";
            if (_AI.female || _AI.creepy || _AI.femaleSkinny)    //is female
            {
                prefix = "♀ ";
                names.AddRange(new string[] { "Lizz Plays", "Wolfskull", "Wiktoria",
                    "Olivier Broadbent, who put '!' in the title of every single one of his videos.",
                    "Emma", "Olivia", "Isabella", "Sophia", "Mia", "Evelyn","Emily", "Elizabeth", "Sofia",
                    "Victoria",  "Chloe", "Camila", "Layla", "Lillian","Dora the explorer", "Zoey", "Hannah", "Lily",
                    "Natalie", "Luna", "Savannah", "Leah", "Zoe", "Stella", "Ellie", "Claire", "Bella", "Aurora",
                    "Lucy", "Anna", "Samantha", "Caroline", "Genesis", "Aaliyah", "Kennedy", "Allison",
                    "Maya", "Sarah", "Madelyn", "Adeline", "Alexa", "Ariana", "Elena", "Gabriella", "Naomi", "Alice",
                    "Hailey", "Eva", "Emilia",  "Quinn", "Piper", "Ruby", "Serenity", "Willow", "Everly",  "Kaylee",
                    "Lydia", "Aubree", "Arianna", "Eliana", "Peyton", "Melanie", "Gianna", "Isabelle", "Julia", "Valentina",
                    "Nova", "Clara", "Vivian", "Reagan", "Mackenzie", "Madeline", "Brielle", "Delilah", "Isla", "Rylee",
                    "Katherine", "Sophie",  "Liliana", "Jade", "Maria", "Taylor", "Hadley", "Kylie", "Emery", "Adalynn", "Natalia",
                    "Annabelle", "Faith", "Alexandra", "Athena", "Andrea", "Leilani", "Jasmine", "Lyla", "Margaret", "Alyssa",
                    "Eliza", "Rose", "Ariel", "Alexis","xKito","Sophie Francis","Albedo" });

            }
            else                                                 // is male
            {
                prefix = "♂ ";
                names.AddRange(Mnames);
            }
            if (_AI.creepy_male)
            {
                names.Add("Alex Armsy");
            }
            if (_AI.maleSkinny)
            {
                names.Add("Zebulon");
            }
            EnemyName = prefix + names[Random.Range(0, names.Count)];

        }
        #endregion

        private void Setup()
        {
            try
            {
                if (BoltNetwork.isRunning)
                {
                    if (entity == null) { entity = transform.root.GetComponentInChildren<BoltEntity>(); }
                    if (entity == null) { entity = _Health.entity; }
                    if (entity == null) { entity = transform.root.GetComponent<BoltEntity>(); }
                    EnemyManager.AddHostEnemy(this);
                }
                else if (GameSetup.IsSinglePlayer)
                {
                    EnemyManager.singlePlayerList.Add(this);
                }
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());
            }

            //Assiging base health, if new enemy
            if (BaseHealth == 0)
            {
                BaseHealth = _Health.Health;
            }
            else    //reverting health to base health if reused enemy (in case enemies are pooled)
            {
                _Health.Health = BaseHealth;
            }

            StopAllCoroutines();
            RollName();

            Steadfast = 101;
            slows = new Dictionary<int, EnemyDebuff>();
            dmgTakenDebuffs = new Dictionary<int, EnemyDebuff>();
            dmgDealtDebuffs = new Dictionary<int, EnemyDebuff>();
            FireDamageDebuffs = new Dictionary<int, EnemyDebuff>();
            DamageOverTimeList = new List<DoT>();
            abilities = new List<Abilities>();

            //picking abilities
            if (UnityEngine.Random.value < 0.1 ||( _AI.creepy_boss&&!_AI.girlFullyTransformed))
            {
                int abilityAmount = UnityEngine.Random.Range(3, 7);
                if (_AI.creepy_boss) { abilityAmount = 10; }   //Megan boss always has abilities, a lot of them.

                int i = 0;
                Array abilityArray = Enum.GetValues(typeof(Abilities));

                //Determining if enemy is Elite, Miniboss or Boss type of enemy
                if (abilityAmount > 6 && Random.value < 0.3f) { _rarity = EnemyRarity.Boss; abilities.Add(Abilities.BossSteadfast); }
                else if (abilityAmount > 4 && Random.value < 0.3f) { abilities.Add(Abilities.EliteSteadfast); _rarity = EnemyRarity.Miniboss; }
                else { _rarity = EnemyRarity.Elite; }

                if (_AI.creepy_boss && !_AI.girlFullyTransformed)    //Force adding BossSteadfast to Megan
                {
                    abilities.Clear();
                    _rarity = EnemyRarity.Boss;
                    abilities.Add(Abilities.BossSteadfast);
                }

                //Trial and error method of picking abilities
                while (i < abilityAmount)
                {
                    bool canAdd = true;
                    Abilities ab = (Abilities)abilityArray.GetValue(UnityEngine.Random.Range(0, abilityArray.Length));
                    if (ab == Abilities.Steadfast || ab == Abilities.EliteSteadfast || ab == Abilities.BossSteadfast)
                    {
                        if (abilities.Contains(Abilities.Steadfast) || abilities.Contains(Abilities.EliteSteadfast) || abilities.Contains(Abilities.BossSteadfast))
                        {
                            canAdd = false;
                        }
                    }
                    else if (ab == Abilities.Tiny || ab == Abilities.Huge)
                    {
                        if (_AI.creepy_boss && ab == Abilities.Tiny)
                        {
                            canAdd = false;
                        }
                        else if (abilities.Contains(Abilities.Huge) || abilities.Contains(Abilities.Tiny))
                        {
                            canAdd = false;
                        }
                    }
                    else if (ab == Abilities.DoubleLife && !(_AI.creepy || _AI.creepy_fat))
                    {
                        canAdd = false;

                    }
                    if (abilities.Contains(ab))
                    {
                        canAdd = false;
                    }
                    if (canAdd)
                    {
                        i++; abilities.Add(ab);
                    }

                }

            }
            else
            {
                _rarity = EnemyRarity.Normal;
            }


            SetType();

            //Assigning level
            SetLevel();

            //Assigning rest of stats
            DamageMult = Mathf.Pow(Level, 2.5f) / 100f + 0.7f;


            Armor = Mathf.FloorToInt(Random.Range(Level * Level *1.6f + 20, Level * Level * 2f  + 50));
            Armor *= (int)ModSettings.difficulty+1;
            ArmorReduction = 0;
            _hp = Mathf.RoundToInt((_Health.Health * Mathf.Pow(Level, 2.3f) / 6));
            AnimSpeed = 0.81f + (float)Level / 130;


            StartCoroutine(UpdateDoT());

            //Extra health for boss type enemies
            switch (_rarity)
            {
                case EnemyRarity.Elite:
                    _hp *= 2;

                    break;
                case EnemyRarity.Miniboss:
                    _hp *= 5;

                    break;
                case EnemyRarity.Boss:
                    _hp *= 10;
                    break;
            }

            //Applying some abilities
            if (abilities.Contains(Abilities.Huge))
            {
                Armor *= 2;
                gameObject.transform.localScale *= 2;
                _hp *= 2;
                DamageMult *= 2;
            }
            else if (abilities.Contains(Abilities.Tiny))
            {
                gameObject.transform.localScale *= 0.35f;
                _hp *= 2;
                DamageMult *= 1.5f;
                BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);
            }
            if (abilities.Contains(Abilities.Steadfast))
            {
                Steadfast = 8;
            }
            if (abilities.Contains(Abilities.EliteSteadfast))
            {
                Steadfast = 2f;
            }
            if (abilities.Contains(Abilities.BossSteadfast))
            {
                Steadfast = 1;
            }
            if (abilities.Contains(Abilities.ExtraHealth))
            {
                _hp *= 3;
            }
            if (abilities.Contains(Abilities.ExtraDamage))
            {
                DamageMult *= 2.5f;
            }
            if (abilities.Contains(Abilities.RainEmpowerement))
            {
                prerainDmg = DamageMult;
                prerainArmor = Armor;
            }
            if (abilities.Contains(Abilities.Juggernaut))
            {
                CCimmune = true;
            }
            if (abilities.Contains(Abilities.Avenger))
            {
                gameObject.AddComponent<Avenger>().progression = this;
            }
            if (abilities.Contains(Abilities.FireAura))
            {
                float aurDmg =(3 * Level + 10f) * DamageAmp/8;
                FireAura.Cast(gameObject, aurDmg);
                if (BoltNetwork.isRunning)
                Network.NetworkManager.SendLine("ES2"+entity.networkId.PackedValue + ";"+aurDmg,NetworkManager.Target.Clients);
                InvokeRepeating("SendFireAura",20,30);
            }
            //Clamping Health
            _hp = Mathf.Min(_hp, int.MaxValue - 5);


            //Setting other health variables
            _Health.maxHealthFloat = _hp;
            MaxHealth = _hp;
            _Health.maxHealth = Mathf.RoundToInt(_hp);
            _Health.Health = Mathf.RoundToInt(_hp);
            DebuffDmgMult = DamageMult;
            DualLifeSpend = false;
            setupComplete = true;
            OnDieCalled = false;
            BaseDamageMult = DamageMult;
            BaseAnimSpeed = AnimSpeed;

            AssignBounty();

            SteadfastCap = Mathf.RoundToInt(Steadfast * 0.01f * MaxHealth);
            if (SteadfastCap < 1) // Making sure the enemy can be killed
            {
                SteadfastCap = 1;
            }

            CreationTime = Time.time;

            if (BoltNetwork.isRunning)
            {
                ulong id = entity.networkId.PackedValue;
                    
                string aa = "AI" + id + ";" + BaseDamageMult + ";";
                foreach (var ability in abilities)
                {
                    aa += (int)ability + ";";
                }
                NetworkManager.SendLine(aa, NetworkManager.Target.Clients);
            }
        }
        /// <summary>
        /// Slows this enemy
        /// </summary>
        /// <param name="source">ID of the source, used for refreshing the duration of applied effects</param>
        public void Slow(int source, float amount, float time)
        {
            //source - 20 is snap freeze
            //source - 40 is hammer attack
            //source - 41 is magic arrow hit
            //source - 43-60 are bashes
            //source - 61-75 are player hits
            //source - 90 - focus on headshot
            //source - 91 - seeking arrow on body shot;
            if (slows.ContainsKey(source))
            {
                slows[source].duration = Mathf.Max(slows[source].duration, time);
                slows[source].amount = Mathf.Min(amount, slows[source].amount);
            }
            else
            {
                slows.Add(source, new EnemyDebuff()
                {
                    Source = source,
                    amount = amount,
                    duration = time,
                });
            }
        }
        public void DmgDealtDebuff(int source, float amount, float time)
        {
            if (dmgDealtDebuffs.ContainsKey(source))
            {
                dmgDealtDebuffs[source].duration = Mathf.Max(dmgDealtDebuffs[source].duration, time);
                dmgDealtDebuffs[source].amount = amount;
            }
            else
            {
                dmgDealtDebuffs.Add(source, new EnemyDebuff()
                {
                    Source = source,
                    amount = amount,
                    duration = time,
                });
            }
        }
        public void DmgTakenDebuff(int source, float amount, float time)
        {
            if (dmgTakenDebuffs.ContainsKey(source))
            {
                dmgTakenDebuffs[source].duration = Mathf.Max(dmgTakenDebuffs[source].duration, time);
                dmgTakenDebuffs[source].amount = amount;
            }
            else
            {
                dmgTakenDebuffs.Add(source, new EnemyDebuff()
                {
                    Source = source,
                    amount = amount,
                    duration = time,
                });
            }
        }
        public void FireDebuff(int source, float amount, float time)
        {
            Debug.LogWarning("Fire debuff, " + source + ", " + amount + ", " + time);
            if (FireDamageDebuffs.ContainsKey(source))
            {
                FireDamageDebuffs[source].duration = Mathf.Max(FireDamageDebuffs[source].duration, time);
                FireDamageDebuffs[source].amount = amount;
            }
            else
            {
                FireDamageDebuffs.Add(source, new EnemyDebuff()
                {
                    Source = source,
                    amount = amount,
                    duration = time,
                });
            }
        }

        public void ReduceArmor(int amount)
        {
            ArmorReduction += amount;
        }
        public static void ReduceArmor(BoltEntity target, int amount)
        {
            if (GameSetup.IsMultiplayer)
            {
                PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
                playerHitEnemy.Target = target;
                playerHitEnemy.Hit = -amount;
                playerHitEnemy.Send();
            }
            else
            {
                EnemyManager.hostDictionary[target.networkId.PackedValue].ArmorReduction += amount;
            }
        }
        public static void ReduceArmor(EnemyProgression target, int amount)
        {
            target.ArmorReduction += amount;
        }

        public void HitMagic(int damage)
        {
            damage = ClampDamage(false, damage, true);
            _Health.HitReal(damage);

        }
        public int ClampDamage(bool pure, int damage, bool magic = false)
        {
            if (abilities.Contains(Abilities.Shielding))
            {
                if (shieldingON > 0)
                {
                    return 0;
                }
                else if (shieldingCD <= 0)
                {
                    switch (ModSettings.difficulty)
                    {
                        case ModSettings.Difficulty.Normal:
                            shieldingCD = 60;
                            break;
                        case ModSettings.Difficulty.Hard:
                            shieldingCD = 55;

                            break;
                        case ModSettings.Difficulty.Elite:
                            shieldingCD = 50;

                            break;
                        case ModSettings.Difficulty.Master:
                            shieldingCD = 45;

                            break;
                        case ModSettings.Difficulty.Challenge1:
                            shieldingCD = 40;

                            break;
                        case ModSettings.Difficulty.Challenge2:
                            shieldingCD = 35;

                            break;
                        case ModSettings.Difficulty.Challenge3:
                            shieldingCD = 30;

                            break;
                        case ModSettings.Difficulty.Challenge4:
                            shieldingCD = 25;
                            break;
                        case ModSettings.Difficulty.Challenge5:
                            shieldingCD = 20;
                            break;
                        default:
                            shieldingCD = 15;
                            break;
                    }
                    normalColor = _Health.MySkin.material.color;
                    _Health.MySkin.material.color = Color.black;
                    shieldingON = 4;
                    return 0;
                }
            }
            damage = Mathf.CeilToInt(damage * dmgTakenIncrease);
            if (pure)
            {
                if (damage > SteadfastCap)
                {
                    if (Steadfast == 100)
                    {
                        return damage;
                    }

                    int dmgpure = Mathf.Min(damage, SteadfastCap);
                    return dmgpure;
                }
                return damage;
            }

            float reduction = ModReferences.DamageReduction(Mathf.Clamp(Armor - ArmorReduction, 0, int.MaxValue));
            if (magic)
            {
                reduction /= 1.75f;
            }

            int dmg = Mathf.CeilToInt(damage * (1 - reduction));
            if (Steadfast == 100)
            {
                dmg = Mathf.Min(dmg, SteadfastCap);
            }

            return dmg;

        }
        public bool OnDie()
        {
            try
            {
                if (GameSetup.IsMpClient)
                {
                    return true;
                }
                if (setup.waterDetect.drowned)
                {
                    return true;
                }
                if (abilities.Contains(Abilities.DoubleLife))
                {
                    if (!DualLifeSpend)
                    {
                        DualLifeSpend = true;
                        _Health.Health = _Health.maxHealth / 2;
                        _Health.MySkin.material.color = Color.magenta;
                        prerainDmg *= 2;

                        _Health.releaseFromTrap();
                        return false;
                    }
                }

                if (OnDieCalled)
                {
                    return true;
                }
                EnemyManager.RemoveEnemy(this);
                if (BoltNetwork.isRunning)
                {
                    foreach (var item in EnemyManager.hostDictionary)
                    {

                        item.Value.gameObject.SendMessage("ThisEnemyDied", this, SendMessageOptions.DontRequireReceiver);
                    }
                    if (abilities.Contains(Abilities.Sacrifice))
                    {
                        foreach (var item in EnemyManager.hostDictionary)
                        {
                            if ((item.Value.transform.position - transform.position).sqrMagnitude > 100) continue;
                            item.Value.ArmorReduction /= 2;
                            item.Value.AnimSpeed *= 1.2f;
                            item.Value.DamageMult *= 1.2f;
                            item.Value.Health = (int)item.Value.MaxHealth;
                        }
                    }
                }
                else
                {
                     foreach (var item in EnemyManager.singlePlayerList)
                    {
                        item.gameObject.SendMessage("ThisEnemyDied", this, SendMessageOptions.DontRequireReceiver);


                    }
                    if (abilities.Contains(Abilities.Sacrifice))
                    {
                        foreach (var item in EnemyManager.singlePlayerList)
                        {
                            if ((item.transform.position - transform.position).sqrMagnitude > 100) continue;
                            item.ArmorReduction /= 2;
                            item.AnimSpeed *= 1.2f;
                            item.DamageMult *= 1.2f;
                            item.Health = (int)item.MaxHealth;
                        }
                    }
                }
                if (Random.value <= 0.1f || _AI.creepy_boss || abilities.Count > 0)
                {
                    int itemCount = Random.Range(1, 6);
                    if (_AI.creepy_boss)
                    {
                        itemCount += 14;
                    }
                    else if (abilities.Count >= 3)
                    {
                        itemCount += Random.Range(3, 6);
                    }
                    if (_rarity == EnemyRarity.Boss)
                    {
                        itemCount += 4;
                    }
                    if (_rarity == EnemyRarity.Miniboss)
                    {
                        itemCount += 2;
                    }

                    itemCount = Mathf.RoundToInt(itemCount* ItemDataBase.MagicFind);



                    for (int i = 0; i < itemCount; i++)
                    {
                        Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(Bounty, enemyType), transform.position + Vector3.up * (2.5f + i / 4));
                    }
                    if (enemyType == Enemy.Megan && (int)ModSettings.difficulty > 4)
                    {
                        //Drop megan only amulet
                        Network.NetworkManager.SendItemDrop(new Item(ItemDataBase.ItemBases[80], 1, 2), transform.position + Vector3.up * 3);

                    }
                }
                if (GameSetup.IsMpServer)
                {
                    Network.NetworkManager.SendLine("KX" + Convert.ToInt64(Bounty / (Mathf.Max(1,0.8f+ ModReferences.Players.Count * 0.2f))) + ";", Network.NetworkManager.Target.Everyone);
                }
                else if (GameSetup.IsSinglePlayer)
                {
                    ModdedPlayer.instance.AddKillExperience(Bounty);
                }
                OnDieCalled = true;
                timeOfDeath = 60;

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write("DIEING ENEMY EXCEPTION  " + ex.ToString());
            }

            return true;

        }

        private void OnEnable()
        {
            OnDieCalled = false;
        }
        private void Update()
        {
            if (!setupComplete)
            {
                if (_Health.Health > 0 && ModSettings.DifficultyChoosen)
                {
                    Setup();
                }

                return;
            }
            if (Time.time - CreationTime < 10)
            {
                _Health.Health = Mathf.RoundToInt(MaxHealth);

            }
            if (OnDieCalled && _hp > 0)
            {
                timeOfDeath -= Time.deltaTime;
                if (timeOfDeath < 0)
                {
                    OnDieCalled = false;
                }
            }
            FireDmgBonus = 0;
           foreach (EnemyDebuff item in FireDamageDebuffs.Values)
                {
                    FireDmgBonus += item.amount;
                }
        
                int[] FDBKeys = new List<int>(FireDamageDebuffs.Keys).ToArray();
                for (int i = 0; i < FDBKeys.Length; i++)
                {
                    int key = FDBKeys[i];

                    FireDamageDebuffs[key].duration -= Time.deltaTime;

                    if (FireDamageDebuffs[key].duration < 0)
                    {
                        FireDamageDebuffs.Remove(key);
                    }
                }
          

            int[] DTDKeys = new List<int>(dmgTakenDebuffs.Keys).ToArray();
            int[] DDDKeys = new List<int>(dmgDealtDebuffs.Keys).ToArray();
            DebuffDmgMult = 1;
            dmgTakenIncrease = 1;
          
                for (int i = 0; i < DTDKeys.Length; i++)
                {

                    int key = DTDKeys[i];
                    dmgTakenIncrease *= dmgTakenDebuffs[key].amount;
                    dmgTakenDebuffs[key].duration -= Time.deltaTime;

                    if (dmgTakenDebuffs[key].duration < 0)
                    {
                        dmgTakenDebuffs.Remove(key);
                    }
                }
        
          
                for (int i = 0; i < DDDKeys.Length; i++)
                {
                    int key = DDDKeys[i];
                    DebuffDmgMult *= dmgDealtDebuffs[key].amount;
                    dmgDealtDebuffs[key].duration -= Time.deltaTime;

                    if (dmgDealtDebuffs[key].duration < 0)
                    {
                        dmgDealtDebuffs.Remove(key);
                    }
                }
         


            if (TrapCD > 0)
            {
                TrapCD -= Time.deltaTime;
            }

            if (StunCD > 0)
            {
                StunCD -= Time.deltaTime;
            }

            if (LaserCD > 0)
            {
                LaserCD -= Time.deltaTime;
            }

            if (MeteorCD > 0)
            {
                MeteorCD -= Time.deltaTime;
            }
            if (BeamCD > 0)
            {
                BeamCD -= Time.deltaTime;
            }
            AnimSpeed = BaseAnimSpeed;
            int[] Keys = new List<int>(slows.Keys).ToArray();
            for (int i = 0; i < Keys.Length; i++)
            {
                int key = Keys[i];
                if (!(slows[key].amount < 1 && CCimmune))
                {
                    AnimSpeed *= slows[key].amount;
                    slows[key].duration -= Time.deltaTime;

                }
                else
                {
                    slows[key].duration = -1;
                }
                if (slows[key].duration < 0)
                {
                    slows.Remove(key);
                }
            }
            _hp = _Health.Health;
            bool inRange = false;
            closestPlayerMagnitude = agroRange;
            foreach (GameObject g in _AI.allPlayers)
            {
                float f = (g.transform.position - transform.position).sqrMagnitude;
                if (f < agroRange)
                {
                    if (f < closestPlayerMagnitude)
                    {
                        closestPlayer = g;
                        closestPlayerMagnitude = f;
                    }
                    inRange = true;

                }
            }

            transform.localScale = Vector3.one;
            if (abilities.Contains(Abilities.RainEmpowerement))
            {
                if (TheForest.Utils.Scene.WeatherSystem.Raining)
                {
                    Armor = prerainArmor * 5;
                    DamageMult = prerainDmg * 5;
                    transform.localScale *= 1.5f;

                    AnimSpeed *= 2;

                }
                else
                {
                    Armor = prerainArmor;
                    DamageMult = prerainDmg;

                }
            }
            ArmorReduction = Mathf.Min(ArmorReduction, Armor);
            if (abilities.Contains(Abilities.Huge))
            {
                gameObject.transform.localScale *= 2f;
            }
            else if (abilities.Contains(Abilities.Tiny))
            {
                gameObject.transform.localScale *= 0.35f;
                BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);

            }
            if (abilities.Contains(Abilities.DoubleLife))
            {
                if (DualLifeSpend)
                {
                    _Health.MySkin.material.color = Color.green;
                    AnimSpeed *= 1.1f;
                    gameObject.transform.localScale *= 1.4f;

                }
            }

            if (abilities.Contains(Abilities.Shielding))
            {

                if (shieldingON > 0)
                {
                    shieldingON -= Time.deltaTime;
                    _Health.MySkin.material.color = Color.black;

                    if (shieldingON <= 0)
                    {
                        _Health.MySkin.material.color = normalColor;
                    }
                }
                if (shieldingCD > 0)
                {
                    shieldingCD -= Time.deltaTime;
                }
            }


            if (inRange)
            {
             
                if (abilities.Contains(Abilities.Meteor) && MeteorCD <= 0)
                {
                    Vector3 dir = closestPlayer.transform.position;
                    Network.NetworkManager.SendLine("MT" + dir.x + ";" + dir.y + ";" + dir.z + ";" + Random.Range(-100000, 100000) + ";", Network.NetworkManager.Target.Everyone);
                    MeteorCD = 50f;
                }
                if (abilities.Contains(Abilities.Flare) && BeamCD <= 0)
                {
                    Vector3 dir = transform.position;
                    float dmg = 60;
                    float slow = 0.25f;
                    float boost = 1.5f;
                    float duration = 20;
                    float radius = 3;

                    switch (ModSettings.difficulty)
                    {

                        case ModSettings.Difficulty.Hard:
                            dmg = 100;
                            radius = 8f;
                            break;
                        case ModSettings.Difficulty.Elite:
                            dmg = 250;
                            radius = 8.4f;
                            break;
                        case ModSettings.Difficulty.Master:
                            dmg = 600;
                            radius = 8.7f;
                            break;
                        case ModSettings.Difficulty.Challenge1:
                            dmg = 3000;
                            radius = 9f;
                            break;
                        case ModSettings.Difficulty.Challenge2:
                            dmg = 7000;
                            radius = 9.2f;
                            break;
                        case ModSettings.Difficulty.Challenge3:
                            dmg = 15000;
                            radius = 9.4f;
                            break;
                        case ModSettings.Difficulty.Challenge4:
                            dmg = 30000;
                            radius = 9.7f;
                            break;
                        case ModSettings.Difficulty.Challenge5:
                            dmg = 60000;
                            radius = 10;
                            break;
                    }

                    float Healing = dmg / 10;


                    Network.NetworkManager.SendLine("SC3;" + dir.x + ";" + dir.y + ";" + dir.z + ";" + "t;" + dmg + ";" + Healing + ";" + slow + ";" + boost + ";" + duration + ";" + radius + ";", Network.NetworkManager.Target.Everyone);
                    BeamCD = 120f;
                }

                if (abilities.Contains(Abilities.Blink))
                {
                    if (blinkCD <= 0)
                    {
                        transform.root.position = closestPlayer.transform.position + transform.forward * -2;
                        blinkCD = Random.Range(15, 25);
                    }
                    else
                    {
                        blinkCD -= Time.deltaTime;
                    }

                }
                if (abilities.Contains(Abilities.Laser) && LaserCD <= 0)
                {
                    Vector3 dir = closestPlayer.transform.position;

                    LaserCD = 100;
                    Network.NetworkManager.SendLine("LA" + transform.position.x + ";" + transform.position.y + ";" + transform.position.z + ";" + dir.x + ";" + dir.y + 2 + ";" + dir.z + ";", Network.NetworkManager.Target.Everyone);


                }
                if (abilities.Contains(Abilities.Rooting) && StunCD <= 0)
                {
                    float duration = 3;
                    switch (ModSettings.difficulty)
                    {
                        case ModSettings.Difficulty.Hard:
                            duration = 3.4f;
                            break;
                        case ModSettings.Difficulty.Elite:
                            duration = 3.8f;
                            break;
                        case ModSettings.Difficulty.Master:
                            duration = 4;
                            break;
                        case ModSettings.Difficulty.Challenge1:
                            duration = 4.3f;
                            break;
                        case ModSettings.Difficulty.Challenge2:
                            duration = 4.6f;
                            break;
                        case ModSettings.Difficulty.Challenge3:
                            duration = 5;
                            break;
                        case ModSettings.Difficulty.Challenge4:
                            duration = 5.5f;
                            break;
                        case ModSettings.Difficulty.Challenge5:
                            duration = 7f;
                            break;
                        default:
                            break;
                    }
                    Network.NetworkManager.SendLine("RO" + transform.position.x + ";" + transform.position.y + ";" + transform.position.z + ";" + duration + ";", Network.NetworkManager.Target.Everyone);
                    StunCD = Random.Range(15, 30);
                }

                if (abilities.Contains(Abilities.Trapper) && TrapCD <= 0)
                {
                    if (closestPlayerMagnitude < agroRange / 2)
                    {
                        float radius = 10f;

                        Network.NetworkManager.SendLine("TR" + transform.position.x + ";" + transform.position.y + ";" + transform.position.z + ";14;" + radius + ";", Network.NetworkManager.Target.Everyone);
                        TrapCD = 50;
                    }
                }

                if (abilities.Contains(Abilities.FreezingAura))
                {
                    if (freezeauraCD > 0) { freezeauraCD -= Time.deltaTime; }
                    else
                    {
                        if (BoltNetwork.isRunning)
                        {
                            Network.NetworkManager.SendLine("ES1;" + entity.networkId.PackedValue + ";", Network.NetworkManager.Target.Everyone);
                        }
                        else
                        {
                            SnowAura sa = new GameObject("Snow").AddComponent<SnowAura>();
                            sa.followTarget = transform.root;
                        }
                        freezeauraCD = 100;
                    }
                }
                if (abilities.Contains(Abilities.BlackHole))
                {
                    if (blackholeCD > 0) { blackholeCD -= Time.deltaTime; }
                    else
                    {
                        float damage = 5 * Level * Level;
                        float duration = 5;
                        float radius = 15;
                        float pullforce = 12;
                        if (BoltNetwork.isRunning)
                        {
                            Network.NetworkManager.SendLine("SC1;" + Math.Round(transform.position.x, 5) + ";" + Math.Round(transform.position.y, 5) + ";" + Math.Round(transform.position.z, 5) + ";" +
                                                       "t;" + damage + ";" + duration + ";" + radius + ";" + pullforce + ";", Network.NetworkManager.Target.Everyone);
                        }
                        else
                        {
                            BlackHole.CreateBlackHole(transform.position, true, damage, duration, radius, pullforce);
                        }
                        blackholeCD = 45;
                    }
                }
            }
            _AI.animSpeed = AnimSpeed;
            setup.animator.speed = AnimSpeed;


        }
        private void SetType()
        {
            if (_AI.creepy && _AI.pale) { enemyType = Enemy.PaleVags; }
            else if (_AI.creepy && !_AI.pale) { enemyType = Enemy.RegularVags; }
            else if (_AI.creepy_male && _AI.pale) { enemyType = Enemy.PaleArmsy; }
            else if (_AI.creepy_male && !_AI.pale) { enemyType = Enemy.RegularArmsy; }
            else if (_AI.creepy_fat) { enemyType = Enemy.Cowman; }
            else if (_AI.creepy_baby) { enemyType = Enemy.Baby; }
            else if (_AI.creepy_boss) { enemyType = Enemy.Megan; }
            else if (_AI.female && !_AI.pale && !_AI.painted) { enemyType = Enemy.NormalFemale; }
            else if (_AI.femaleSkinny && !_AI.pale && !_AI.painted) { enemyType = Enemy.NormalSkinnyFemale; }
            else if (_AI.female && !_AI.pale && _AI.painted) { enemyType = Enemy.PaintedFemale; }
            else if (_AI.fireman) { enemyType = Enemy.Fireman; }
            else if (_AI.male && !_AI.pale && !_AI.painted && !_AI.leader && !_AI.skinned) { enemyType = Enemy.NormalMale; }
            else if (_AI.maleSkinny && !_AI.pale && !_AI.painted && !_AI.leader && !_AI.skinned) { enemyType = Enemy.NormalSkinnyMale; }
            else if (_AI.male && !_AI.pale && !_AI.painted && _AI.leader && !_AI.skinned) { enemyType = Enemy.NormalLeaderMale; }
            else if (_AI.male && !_AI.pale && _AI.painted && _AI.leader && !_AI.skinned) { enemyType = Enemy.PaintedLeaderMale; }
            else if (_AI.male && !_AI.pale && _AI.painted && !_AI.leader && !_AI.skinned) { enemyType = Enemy.PaintedMale; }
            else if (_AI.maleSkinny && _AI.pale && !_AI.painted && !_AI.leader && !_AI.skinned) { enemyType = Enemy.PaleSkinnyMale; }
            else if (_AI.maleSkinny && _AI.pale && !_AI.painted && !_AI.leader && _AI.skinned) { enemyType = Enemy.PaleSkinnedSkinnyMale; }
            else if (_AI.male && _AI.pale && !_AI.painted && !_AI.leader && !_AI.skinned) { enemyType = Enemy.PaleMale; }
            else if (_AI.maleSkinny && _AI.pale && !_AI.painted && !_AI.leader && _AI.skinned) { enemyType = Enemy.PaleSkinnedMale; }
        }
        private void SetLevel()
        {
            float extraLevels = 1;
            if (_AI.creepy || _AI.creepy_fat || _AI.creepy_male)
            {
                extraLevels = 5;

            }
            else if (_AI.creepy_baby)
            {
                extraLevels = 0;

            }
            else if (_AI.creepy_boss)
            {
                extraLevels = 30;

            }
            else if (_AI.leader)
            {
                extraLevels = 5f;
            }
            else if (_AI.painted)
            {
                extraLevels += 5f;
            }
            else if (_AI.pale)
            {
                extraLevels += 8f;
                if (_AI.skinned)
                {
                    extraLevels += 9f;
                }
            }

            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Normal:
                    Level = Random.Range(1, 3);
                    break;
                case ModSettings.Difficulty.Hard:
                    Level = Random.Range(7, 11);

                    break;
                case ModSettings.Difficulty.Elite:
                    Level = Random.Range(20, 25);

                    break;
                case ModSettings.Difficulty.Master:
                    Level = Random.Range(35, 45);

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Level = Random.Range(70, 76);

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Level = Random.Range(80, 90);

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Level = Random.Range(95, 105);

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Level = Random.Range(120, 140);

                    break;
                case ModSettings.Difficulty.Challenge5:
                    Level = Random.Range(200, 250);

                    break;
            }
            Level = Mathf.CeilToInt(Level + extraLevels);
        }
        private void AssignBounty()
        {
            double b =Random.Range(_hp * 0.9f, _hp * 1.1f) * Mathf.Sqrt(Level) + Armor* 0.2;
            if (abilities.Count > 1)
            {
                b = b * abilities.Count * 0.9f;
            }
            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Hard:
                    b = b * 1.25;
                    break;
                case ModSettings.Difficulty.Elite:
                    b = b * 1.7;

                    break;
                case ModSettings.Difficulty.Master:
                    b = b * 2.4;

                    break;
                case ModSettings.Difficulty.Challenge1:
                    b = b * 4;

                    break;
                case ModSettings.Difficulty.Challenge2:
                    b = b * 6;

                    break;
                case ModSettings.Difficulty.Challenge3:
                    b = b * 9f;

                    break;
                case ModSettings.Difficulty.Challenge4:
                    b = b * 14;

                    break;
                case ModSettings.Difficulty.Challenge5:
                    b = b * 22;

                    break;
                default:
                    break;
            }
            Bounty = (long)b;
        }

        void SendFireAura()
        {
            if (abilities.Contains(Abilities.FireAura))
            {
                float aurDmg = (8 * Level + 10f);
                FireAura.Cast(gameObject, aurDmg);
                if (BoltNetwork.isRunning)
                    Network.NetworkManager.SendLine("ES2" + entity.networkId.PackedValue + ";" + aurDmg, NetworkManager.Target.Clients);
            }
        }

        IEnumerator UpdateDoT()
        {
            while (Health > 0)
            {

                if (!(DamageOverTimeList == null || DamageOverTimeList.Count == 0))
                {
                    int d = DamageOverTimeList.Sum(x => x.Amount);
                    HitMagic(d);
                }
                yield return null;
                float t = Time.time;
                for (int i = 0; i < DamageOverTimeList.Count; i++)
                {
                    if (DamageOverTimeList[i].StopTime < t) DamageOverTimeList.RemoveAt(i);
                }
                yield return new WaitForSeconds(1);
            }
            DamageOverTimeList.Clear();

        }

        public void DoDoT(int dmg, float duration)
        {
            DamageOverTimeList.Add(new DoT(dmg, duration));
        }
    }
}
