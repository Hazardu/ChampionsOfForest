using Bolt;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public Vector3 knockbackDir;
        public float knockbackSpeed;



        public struct DoT
        {
            /// <summary>
            /// Amount of damage dealt second
            /// </summary>
            public float Amount;

            /// <summary>
            /// Timestamp when stoptime needs to be deleted;
            /// </summary>
            private int Ticks;
            public bool Tick()
            {
                Ticks--;
                return Ticks <= 0;
            }
            public DoT(int Damage, float duration)
            {
                Amount = Damage;
                Ticks=Mathf.CeilToInt(duration);
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

        public int Health { get => (int)Mathf.Min(_hp, int.MaxValue - 5); set { _Health.Health = value; _hp = value; } }
        public float FinalHealth;

        public float _hp;
        public float MaxHealth;
        private int BaseHealth = 0;

        public float DamageMult;
        public float BaseDamageMult;
        public float DamageAmp => DamageMult;
        public float FireDmgAmp = 1;
        public float FireDmgBonus;

        public long bounty;

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
        public float BaseAnimSpeed = 1;
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
        private float ArcaneCataclysmCD;
        private float FireCataclysmCD;

        //Closest player, for detecting if in range to cast abilities
        private GameObject closestPlayer;
        private float closestPlayerMagnitude;
        private Avenger avengerability;

        private float timeOfDeath;

        private Color normalColor;

        public enum Abilities { Steadfast, BossSteadfast, EliteSteadfast, FreezingAura, FireAura, Rooting, BlackHole, Trapper, Juggernaut, Huge, Tiny, ExtraDamage, ExtraHealth, Basher, Blink, RainEmpowerement, Shielding, Meteor, Flare, DoubleLife, Laser, Poisonous, Sacrifice, Avenger, FireCataclysm, ArcaneCataclysm }
        public enum Enemy { RegularArmsy, PaleArmsy, RegularVags, PaleVags, Cowman, Baby, Girl, Worm, Megan, NormalMale, NormalLeaderMale, NormalFemale, NormalSkinnyMale, NormalSkinnyFemale, PaleMale, PaleSkinnyMale, PaleSkinnedMale, PaleSkinnedSkinnyMale, PaintedMale, PaintedLeaderMale, PaintedFemale, Fireman };
        #endregion
        public static string[] fNames = new string[] { "Lizz Plays", "Wolfskull", "Wiktoria",
                    "Emma", "Olivia", "Isabella", "Sophia", "Mia", "Evelyn","Emily", "Elizabeth", "Sofia",
                    "Victoria",  "Chloe", "Camila", "Layla", "Lillian", "Hannah", "Lily",
                    "Natalie", "Luna", "Savannah", "Leah", "Zoe", "Stella", "Ellie", "Claire", "Bella", "Aurora",
                    "Lucy", "Anna", "Samantha", "Caroline", "Genesis", "Aaliyah", "Kennedy", "Allison",
                    "Maya", "Sarah", "Madelyn", "Adeline", "Alexa", "Ariana", "Elena", "Gabriella", "Naomi", "Alice",
                    "Hailey", "Eva", "Emilia",  "Quinn", "Piper", "Serenity", "Willow", "Everly",  "Kaylee",
                    "Lydia", "Aubree", "Arianna", "Eliana", "Peyton", "Melanie", "Gianna", "Isabelle", "Julia", "Valentina",
                    "Nova", "Clara", "Vivian", "Reagan", "Mackenzie", "Madeline", "Delilah", "Isla", "Rylee",
                    "Katherine", "Sophie",  "Liliana", "Jade", "Maria", "Taylor Swift", "Hadley", "Kylie", "Emery", "Adalynn", "Natalia",
                    "Annabelle", "Faith", "Alexandra", "Athena", "Andrea", "Leilani", "Jasmine", "Lyla", "Margaret", "Alyssa",
                    "Eliza", "Ariel", "Alexis","xKito","Sophie Francis","Albedo","Hazardina","Kaspita" };
    #region Name
    public static string[] mNames = new string[]
              {
                  "Farket","Hazard","Moritz","Souldrinker","Olivier Broadbent","Subscribe to Pewds","Kutie","Axt","Fionera","Cleetus","Hellsing","Metamoth","Teledorktronics","SmokeyThePerson","NightOwl","PuffedRice","PhoenixDeath","Lyon the weeb","Danny Parker","Kaspito","Lukaaa","Chefen","Lauren","DrowsyCob","Ali","ArcKaino","Calean","LordSidonus","DTfang","Malкae","R3iGN","Torsin","θฯ12","Иio","Komissar bAv","The Strange Man","Micha","MiikaHD","NÜT","AssPirate","Azpect","LumaR","TeigRolle","Foreck","Gaullin","Alichipmunk","Chad","Blight","Cheddar","MaddVladd","Wren","Ross Draws","Sam Gorski","Mike Diva","Niko Pueringer","Freddy Wong","PewDiePie","Salad Ass","Morgan Page","Hex Cougar","Unlike Pluto","Sora","Film Crafterz","Fon","Sigmar","Mohammed","Cyde","MaximumAsp79","Diavolo","Doppio Vinegar","Dio Brando","Giorno Giovanna","Fellow Komrade","Samuel","Sebastian","David","Carter","Wyatt","Jayden","John","Owen","Dylan","Luke","Gabriel","Anthony","Isaac","Grayson","Jack","Julian","Levi","Christopher","Joshua","Andrew","Lincoln","Mateo","Ryan","Jaxon","Xet","Adolf","Geoxor","Eraized", "Xelthaz", "Commanderroot", "Plqauttro","Commula","Tom from Myspace","Maurycy"
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
                //prefix = "♀ ";
                names.AddRange(fNames);

            }
            else                                                 // is male
            {
                //prefix = "♂ ";
                names.AddRange(mNames);
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

            Steadfast = 100;
            slows = new Dictionary<int, EnemyDebuff>();
            dmgTakenDebuffs = new Dictionary<int, EnemyDebuff>();
            dmgDealtDebuffs = new Dictionary<int, EnemyDebuff>();
            FireDamageDebuffs = new Dictionary<int, EnemyDebuff>();
            DamageOverTimeList = new List<DoT>();
            abilities = new List<Abilities>();

            //picking abilities
            if (UnityEngine.Random.value < 0.1 || (_AI.creepy_boss && !_AI.girlFullyTransformed) || ModSettings.difficulty == ModSettings.Difficulty.Hell)
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
            int dif = (int)ModSettings.difficulty;
            DamageMult = Mathf.Pow(Level, 4) / 100f + 0.5f;
            DamageMult *= dif*2 + 1;

            Armor = Mathf.FloorToInt(Random.Range(Mathf.Pow(Level, 2.4f) *0.36f+ 1, Mathf.Pow(Level, 2.45f)+ 20));
            Armor *= dif/2 + 1;
            ArmorReduction = 0;
            _hp = (_Health.Health * Mathf.Pow((float)Level, 2.215f + (dif * 0.19f)) /16);
            _hp *= dif/2 + 1;
            AnimSpeed =0.9f + (float)Level / 205;
       
            if(_rarity != EnemyRarity.Normal)
            {
                Armor *= 3;
            }


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
                    if (!abilities.Contains(Abilities.Tiny))
                        gameObject.transform.localScale *= 1.1f;

                    break;
            }
            _hp *= (float)(dif * 0.35f + 0.55f);
            if (dif > 3)
                _hp *= 2.35f;
                if (dif > 7)
                _hp *= 3f;
            //Applying some abilities
            if (abilities.Contains(Abilities.Huge))
            {
                Armor *= 2;
                gameObject.transform.localScale *= 2;
                _hp *= 2;
                DamageMult *= 2;
                AnimSpeed /= 2;
            }
            else if (abilities.Contains(Abilities.Tiny))
            {
                gameObject.transform.localScale *= 0.35f;
                _hp *= 0.9f;
                DamageMult *= 1.2f;
                BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);
            }
            if (abilities.Contains(Abilities.Steadfast))
            {
                Steadfast = 8;
            }
            if (abilities.Contains(Abilities.EliteSteadfast))
            {
                Steadfast = 2.5f;
            }
            if (abilities.Contains(Abilities.BossSteadfast))
            {
                Steadfast = 1.5f;
            }
            if (abilities.Contains(Abilities.ExtraHealth))
            {
                _hp *= 3;
            }
            if (abilities.Contains(Abilities.ExtraDamage))
            {
                DamageMult *= 4f;
            }
            if (abilities.Contains(Abilities.RainEmpowerement))
            {
                prerainDmg = DamageMult;
                prerainArmor = Armor;
            }
            if (abilities.Contains(Abilities.Juggernaut))
            {
                CCimmune = true;
                AnimSpeed /= 1.4f;
                DamageMult *= 2;

            }
            if (abilities.Contains(Abilities.Avenger))
            {
                if(avengerability == null)
                avengerability = gameObject.AddComponent<Avenger>();
                avengerability.progression = this;
                avengerability.Stacks = 0;

            }
            if (abilities.Contains(Abilities.FireAura))
            {
                float aurDmg = (5 * Level + 1) * ((int)ModSettings.difficulty + 1.3f);
                FireAura.Cast(gameObject, aurDmg);
                if (BoltNetwork.isRunning)
                {
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(8);
                            w.Write(2);
                            w.Write(entity.networkId.PackedValue);
                            w.Write(aurDmg);
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
                        answerStream.Close();
                    }
                }

                InvokeRepeating("SendFireAura", 20, 30);
            }
            else
            {
                var aura = gameObject.GetComponent<FireAura>();
                    if(aura != null)
                {
                    Destroy(aura);
                }
            }
            //Clamping Health
            try
            {
            MaxHealth = _hp;
            _Health.maxHealthFloat = _hp;
            Armor = Mathf.Min(Armor, int.MaxValue - 5);
            if (Armor < 0) Armor = int.MaxValue;
            //Setting other health variables
            _Health.maxHealth = Mathf.RoundToInt(Mathf.Min(_hp, int.MaxValue - 5));
            _Health.Health = Mathf.RoundToInt(Mathf.Min(_hp, int.MaxValue - 5));
            _hp -= _Health.Health;
            DebuffDmgMult = DamageMult;
            DualLifeSpend = false;
            setupComplete = true;
            OnDieCalled = false;
            BaseDamageMult = DamageMult;
            BaseAnimSpeed = AnimSpeed;
            }
            catch (Exception e)
            {
                ModAPI.Log.Write(e.ToString());
                CotfUtils.Log(e.Message);
            }
           
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
                using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                {
                    using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                    {
                        w.Write(30);
                        w.Write(id);
                        w.Write(BaseDamageMult);
                        foreach (Abilities ability in abilities)
                        {
                            w.Write((int)ability);
                        }
                    w.Close();
                    }
                    ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
                    answerStream.Close();
                }
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
            //source - 120-135 -afterburn;
            //source - 140 is cataclysm fire
            //source - 141 is cataclysm arcane
            //source - 142 is the fart

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
            Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, damage, new Color(0f, 0, 1f, 0.8f));

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
                    shieldingON = 3;
                    return 0;
                }
            }
            if (!abilities.Contains(Abilities.Juggernaut))
                
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
                reduction /= 1.5f;
            }

            int dmg = Mathf.CeilToInt(damage * (1 - reduction));
            if (Steadfast != 100)
            {
                dmg = Mathf.Min(dmg, SteadfastCap);
            }

            return dmg;

        }
        public bool OnDie()
        {
            try
            {
                DamageOverTimeList.Clear();
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
                        _hp= MaxHealth / 2;
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
                    foreach (EnemyProgression item in EnemyManager.hostDictionary.Values)
                    {
                        if (item != null && item.gameObject != null && item.gameObject.activeSelf)
                        {
                            item.gameObject.SendMessage("ThisEnemyDied", this, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    if (abilities.Contains(Abilities.Sacrifice))
                    {
                        Effects.Sound_Effects.GlobalSFX.Play(1013, 2000);
                        foreach (EnemyProgression item in EnemyManager.hostDictionary.Values)
                        {
                            if (!(item != null && item != this && item.gameObject != null && item.gameObject.activeSelf))
                            {
                                continue;
                            }

                            if ((item.transform.position - transform.position).sqrMagnitude > 100)
                            {
                                continue;
                            }

                            item.ArmorReduction =0;
                            item.BaseAnimSpeed *= 1.25f;
                            item.BaseDamageMult *= 2f;
                            
                            item._hp = item.MaxHealth;
                            
                        }
                    }
                }
                else
                {
                    foreach (EnemyProgression item in EnemyManager.singlePlayerList)
                    {
                        if (item != null && item.gameObject != null && item.gameObject.activeSelf)
                        {
                            item.gameObject.SendMessage("ThisEnemyDied", this, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    if (abilities.Contains(Abilities.Sacrifice))
                    {
                        foreach (EnemyProgression item in EnemyManager.singlePlayerList)
                        {
                            if (!(item != null && item.gameObject != null && item.gameObject.activeSelf))
                            {
                                continue;
                            }

                            if ((item.transform.position - transform.position).sqrMagnitude > 100)
                            {
                                continue;
                            }
                            item.ArmorReduction = 0;
                            item.BaseAnimSpeed *= 1.25f;
                            item.BaseDamageMult *= 2f;
                            item._hp = item.MaxHealth;
                        }
                    }
                }

                if (Random.value <= 0.1f * ItemDataBase.MagicFind || _AI.creepy_boss || abilities.Count > 0)
                {
                    int itemCount = Random.Range(1, 3 + ModReferences.Players.Count);
                    if (_AI.creepy_boss)
                    {
                        itemCount += 15;
                    }
                    else if (abilities.Count >= 3)
                    {
                        itemCount += Random.Range(3, 7);
                    }
                    if (_rarity == EnemyRarity.Boss)
                    {
                        itemCount += 6;
                    }
                    if (_rarity == EnemyRarity.Miniboss)
                    {
                        itemCount += 3;
                    }
                    itemCount += Mathf.RoundToInt((int)ModSettings.difficulty);
                    itemCount = Mathf.RoundToInt(itemCount * ItemDataBase.MagicFind);

                    ModReferences.SendRandomItemDrops(itemCount, enemyType, bounty, transform.position);

                    if (enemyType == Enemy.Megan && (int)ModSettings.difficulty > 4)
                    {
                        //Drop megan only amulet
                        Network.NetworkManager.SendItemDrop(new Item(ItemDataBase.ItemBases[80], 1, 2), transform.position + Vector3.up * 3);

                    }
                }
                if (GameSetup.IsMpServer)
                {
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(10);
                            w.Write(Convert.ToInt64(bounty / (Mathf.Max(1, 0.7f + ModReferences.Players.Count * 0.3f))));
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                        answerStream.Close();
                    }
                }
                else if (GameSetup.IsSinglePlayer)
                {
                    ModdedPlayer.instance.AddKillExperience(bounty);
                }
                OnDieCalled = true;
                timeOfDeath = 120;
                _Health.Health = 0;
                _hp = 0;
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
            if (setup.ai.creepy_boss && !setup.ai.girlFullyTransformed)
            {
                return;
            }
            if (Time.time - CreationTime < 4)
            {
                if (_hp > MaxHealth) MaxHealth = _hp;

            }
            if (OnDieCalled && _hp+ _Health.Health > 0)
            {
                timeOfDeath -= Time.deltaTime;
                if (timeOfDeath < 0)
                {
                    OnDieCalled = false;
                }
            }

            if (knockbackSpeed > 0)
            {
                knockbackSpeed -= Time.deltaTime * (knockbackSpeed + 3.5f);
                transform.root.Translate(knockbackDir * knockbackSpeed);
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

            UpdateDoT();

            if (ArcaneCataclysmCD > 0)
            {
                ArcaneCataclysmCD -= Time.deltaTime;
            }
            if (FireCataclysmCD > 0)
            {
                FireCataclysmCD -= Time.deltaTime;
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
            if(_hp >0)
            {
                if (_Health.Health < int.MaxValue/20)
                {
                     float f = int.MaxValue/2 - _Health.Health;
                    f = Mathf.Min(f, _hp);
                    _Health.Health += (int)f;
                    _hp -= f;
                }
            }
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
            if (abilities.Contains(Abilities.Avenger))
            {
                transform.localScale +=0.1f * avengerability.Stacks * Vector3.one;
            }
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
                   
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(17);
                            w.Write(dir.x);
                            w.Write(dir.y);
                            w.Write(dir.z);
                            w.Write(Random.Range(-100000, 100000));
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                        answerStream.Close();
                    }
                    MeteorCD = 55f;
                }
                if (abilities.Contains(Abilities.Flare) && BeamCD <= 0)
                {
                    Vector3 dir = transform.position;
                    float dmg = 60;
                    float slow = 0.2f;
                    float boost = 1.4f;
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
                            dmg = 1400;
                            radius = 9f;
                            break;
                        case ModSettings.Difficulty.Challenge2:
                            dmg = 3000;
                            radius = 9.2f;
                            break;
                        case ModSettings.Difficulty.Challenge3:
                            dmg = 6000;
                            radius = 9.4f;
                            break;
                        case ModSettings.Difficulty.Challenge4:
                            dmg = 12000;
                            radius = 9.7f;
                            break;
                        case ModSettings.Difficulty.Challenge5:
                            dmg = 19000;
                            radius = 10;
                            break;
                        case ModSettings.Difficulty.Challenge6:
                            dmg = 40000;
                            radius = 14;
                            break;
                        case ModSettings.Difficulty.Hell:
                            dmg = 50000;
                            radius = 17;
                            break;
                    }

                    float Healing = dmg / 5;
                    dmg *= 2;
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(3);
                            w.Write(3);
                            w.Write(dir.x);
                            w.Write(dir.y);
                            w.Write(dir.z);
                            w.Write(true);
                            w.Write(dmg);
                            w.Write(Healing);
                            w.Write(slow);
                            w.Write(boost);
                            w.Write(duration);
                            w.Write(radius);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                        answerStream.Close();
                    }
                 
                    BeamCD = 120f;
                }

                if (abilities.Contains(Abilities.Blink))
                {
                    if (blinkCD <= 0)
                    {
                        transform.root.position = closestPlayer.transform.position + transform.forward * -2.5f;
                        blinkCD = Mathf.Max(Random.Range(14, 33) - (int) ModSettings.difficulty,6);
                        Effects.Sound_Effects.GlobalSFX.Play(5);

                    }
                    else
                    {
                        blinkCD -= Time.deltaTime;
                    }

                }
                if (abilities.Contains(Abilities.Laser) && LaserCD <= 0)
                {
                    Vector3 dir = closestPlayer.transform.position;
                    
                    LaserCD = 110;
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(16);
                            w.Write(transform.position.x);
                            w.Write(transform.position.y);
                            w.Write(transform.position.z);
                            w.Write(dir.x);
                            w.Write(dir.y);
                            w.Write(dir.z);
                            w.Close();
                            
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                        answerStream.Close();
                    }
                }
                if (abilities.Contains(Abilities.Rooting) && StunCD <= 0)
                {
                    float duration = 3;
                    switch (ModSettings.difficulty)
                    {
                        case ModSettings.Difficulty.Normal:
                            duration = 3;
                            break;
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
                            duration = 8f;
                            break;
                    }
              
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(12); 
                            w.Write(transform.position.x);
                            w.Write(transform.position.y);
                            w.Write(transform.position.z);
                            w.Write(duration);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                        answerStream.Close();
                    }
                    StunCD = Random.Range(20, 35);
                }

                if (abilities.Contains(Abilities.Trapper) && TrapCD <= 0)
                {
                    if (closestPlayerMagnitude < 80)
                    {
                        float radius = 8f;
                        using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                        {
                            using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                            {
                                w.Write(15);
                                w.Write(transform.position.x);
                                w.Write(transform.position.y);
                                w.Write(transform.position.z);
                                w.Write(20f);
                                w.Write(radius);
                            w.Close();
                            }
                            ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                            answerStream.Close();
                        }
                        TrapCD = 40;
                    }
                }
                if (abilities.Contains(Abilities.ArcaneCataclysm) && ArcaneCataclysmCD <= 0)
                {
                    if (closestPlayerMagnitude < agroRange / 2)
                    {
                        float dmg = 60;
                        float radius = 10;
                        switch (ModSettings.difficulty)
                        {
                            case ModSettings.Difficulty.Hard:
                                dmg = 100;
                                radius = 11;
                                break;
                            case ModSettings.Difficulty.Elite:
                                dmg = 250;
                                radius = 12;
                                break;
                            case ModSettings.Difficulty.Master:
                                dmg = 600;
                                radius = 13;
                                break;
                            case ModSettings.Difficulty.Challenge1:
                                dmg = 1400;
                                radius = 14;
                                break;
                            case ModSettings.Difficulty.Challenge2:
                                dmg = 3000;
                                radius = 15;
                                break;
                            case ModSettings.Difficulty.Challenge3:
                                dmg = 7000;
                                radius = 16;
                                break;
                            case ModSettings.Difficulty.Challenge4:
                                dmg = 13000;
                                radius = 17;
                                break;
                            case ModSettings.Difficulty.Challenge5:
                                dmg = 20000;
                                radius = 18;
                                break;
                            case ModSettings.Difficulty.Challenge6:
                            case ModSettings.Difficulty.Hell:
                                dmg = 35000;
                                radius = 19;
                                break;

                        }
                        radius -= 1;
                        if (BoltNetwork.isRunning)
                        {
                            using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                            {
                                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                                {
                                    w.Write(3);
                                    w.Write(11);
                                    w.Write(transform.position.x);
                                    w.Write(transform.position.y);
                                    w.Write(transform.position.z);
                                    w.Write(radius);
                                    w.Write(dmg);
                                    w.Write(15f);
                                    w.Write(true);
                                    w.Write(true);
                                w.Close();
                                }
                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
                                answerStream.Close();
                            }
                        }
                        Effects.Cataclysm.Create(transform.position, radius, dmg, 15, Effects.Cataclysm.TornadoType.Arcane, true);
                        ArcaneCataclysmCD = 150;
                    }
                }
                if (abilities.Contains(Abilities.FireCataclysm) && FireCataclysmCD <= 0)
                {
                    if (closestPlayerMagnitude < agroRange / 2)
                    {
                        float dmg = 60;
                        float radius = 10;
                        switch (ModSettings.difficulty)
                        {
                            case ModSettings.Difficulty.Hard:
                                dmg = 100;
                                radius = 11;
                                break;
                            case ModSettings.Difficulty.Elite:
                                dmg = 250;
                                radius = 12;
                                break;
                            case ModSettings.Difficulty.Master:
                                dmg = 600;
                                radius = 13;
                                break;
                            case ModSettings.Difficulty.Challenge1:
                                dmg = 1400;
                                radius = 14;
                                break;
                            case ModSettings.Difficulty.Challenge2:
                                dmg = 3000;
                                radius = 15;
                                break;
                            case ModSettings.Difficulty.Challenge3:
                                dmg = 7000;
                                radius = 16;
                                break;
                            case ModSettings.Difficulty.Challenge4:
                                dmg = 13000;
                                radius = 17;
                                break;
                            case ModSettings.Difficulty.Challenge5:
                                dmg = 20000;
                                radius = 18;
                                break;
                            case ModSettings.Difficulty.Challenge6:
                            case ModSettings.Difficulty.Hell:
                                dmg = 35000;
                                radius = 19;
                                break;

                        }
                        radius -= 1;
                        dmg /= 1.5f;
                        if (BoltNetwork.isRunning)
                        {
                            using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                            {
                                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                                {
                                    w.Write(3);
                                    w.Write(11);
                                    w.Write(transform.position.x);
                                    w.Write(transform.position.y);
                                    w.Write(transform.position.z);
                                    w.Write(radius);
                                    w.Write(dmg);
                                    w.Write(18f);
                                    w.Write(false);
                                    w.Write(true);
                                w.Close();
                                }
                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
                                answerStream.Close();
                            }
                        }
                        Effects.Cataclysm.Create(transform.position, radius, dmg, 15, Effects.Cataclysm.TornadoType.Fire, true);
                        FireCataclysmCD = 170;
                    }
                }

                if (abilities.Contains(Abilities.FreezingAura))
                {
                    if (freezeauraCD > 0) { freezeauraCD -= Time.deltaTime; }
                    else
                    {
                        if (BoltNetwork.isRunning)
                        {
                            using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                            {
                                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                                {
                                    w.Write(8);
                                    w.Write(1);
                                    w.Write(entity.networkId.PackedValue);
                                w.Close();
                                }
                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                                answerStream.Close();
                            }
                        }
                        else
                        {
                            SnowAura sa = new GameObject("Snow").AddComponent<SnowAura>();
                            sa.followTarget = transform.root;
                        }
                        freezeauraCD = 80;
                    }
                }
                if (abilities.Contains(Abilities.BlackHole))
                {
                    if (blackholeCD > 0) { blackholeCD -= Time.deltaTime; }
                    else
                    {
                        float damage = Mathf.Pow(Level,1.7f);
                        float duration = 7.5f;
                        float radius = 21 + 3* (int)ModSettings.difficulty;
                        float pullforce = 35;
                        if (BoltNetwork.isRunning)
                        {
                            using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                            {
                                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                                {
                                    w.Write(3);
                                    w.Write(1);
                                    w.Write(transform.position.x);
                                    w.Write(transform.position.y);
                                    w.Write(transform.position.z);
                                    w.Write(true);
                                    w.Write(damage);
                                    w.Write(duration);
                                    w.Write(radius);
                                    w.Write(pullforce);
                                    w.Write("");

                                    w.Close();
                                }
                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                                answerStream.Close();
                            }
                        }
                        else
                        {
                            BlackHole.CreateBlackHole(transform.position, true, damage, duration, radius, pullforce);
                        }
                        blackholeCD = 55;
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
                extraLevels = 2;

            }
            else if (_AI.creepy_baby)
            {
                extraLevels = 0;

            }
            else if (_AI.creepy_boss)
            {
                extraLevels = 10;

            }
            else if (_AI.leader)
            {
                extraLevels = 2f;
            }
            else if (_AI.painted)
            {
                extraLevels += 2f;
            }
            else if (_AI.pale)
            {
                extraLevels += 1f;
                if (_AI.skinned)
                {
                    extraLevels += 2f;
                }
            }

            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Normal:
                    Level = Random.Range(1, 4);
                    break;
                case ModSettings.Difficulty.Hard:
                    Level = Random.Range(5, 10);

                    break;
                case ModSettings.Difficulty.Elite:
                    Level = Random.Range(14, 20);

                    break;
                case ModSettings.Difficulty.Master:
                    Level = Random.Range(30, 40);

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Level = Random.Range(70, 76);

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Level = Random.Range(86, 96);

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Level = Random.Range(100, 110);

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Level = Random.Range(120, 140);

                    break;
                case ModSettings.Difficulty.Challenge5:
                    Level = Random.Range(200, 250);

                    break;
                case ModSettings.Difficulty.Challenge6:
                    Level = Random.Range(340, 350);

                    break;
                case ModSettings.Difficulty.Hell:
                    Level = Random.Range(380, 390);

                    break;
            }
            Level = Mathf.CeilToInt(Level + extraLevels);
        }
        private void AssignBounty()
        {
            double b = Random.Range((MaxHealth) * 0.2f, MaxHealth*0.5f) * Mathf.Pow(Level,0.5f)*0.5f + Armor;
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
                case ModSettings.Difficulty.Challenge6:
                    b = b * 40;

                    break;
                case ModSettings.Difficulty.Hell:
                    b = b * 40;

                    break;
            }
            bounty = Convert.ToInt64(b);
        }

        private void SendFireAura()
        {
            if (abilities.Contains(Abilities.FireAura))
            {
                float aurDmg = (5* Level + 1)* ((int)ModSettings.difficulty+1.3f);
                FireAura.Cast(gameObject, aurDmg);
                if (BoltNetwork.isRunning)
                {
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(8);
                            w.Write(2);
                            w.Write(entity.networkId.PackedValue);
                            w.Write(aurDmg);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
                        answerStream.Close();
                    }
                }
            }
        }
        float DoTTimestamp;
        private void UpdateDoT()
        {
            if(DoTTimestamp < Time.time)
            {
                if (DamageOverTimeList != null && DamageOverTimeList.Count >0)
                {
                        DamageMath.DamageClamp(DamageOverTimeList.Sum(x => x.Amount),out int d,out int rep);
                    Debug.Log("DOT dmg" + d);
                    for (int i = 0; i < rep; i++)
                    _Health.Hit(d);
                }
                for (int i = 0; i < DamageOverTimeList.Count; i++)
                {
                    if (DamageOverTimeList[i].Tick())
                    {
                        DamageOverTimeList.RemoveAt(i);
                    }
                }
                DoTTimestamp = Time.time + 1;            }

        }

        public void DoDoT(int dmg, float duration)
        {
           if(abilities == null || ! abilities.Contains(Abilities.Juggernaut))
            DamageOverTimeList.Add(new DoT(dmg, duration));
        }



        public void AddKnockback(Vector3 dir, float amount)
        {
            knockbackDir += dir;
            knockbackDir.Normalize();
            if (amount > knockbackSpeed)
                knockbackSpeed = amount;
        
        }


    }
}
