using Bolt;
using ChampionsOfForest.Enemies;
using ChampionsOfForest.Enemies.EnemyAbilities;
using System;
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
        public class SlowDebuff
        {
            public int Source;
            public float duration;
            public float amount;
        }
        public Dictionary<int, SlowDebuff> slows;

        //Type of enemy
        public enum EnemyType { Normal, Elite, Miniboss, Boss }
        public EnemyType type;

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

        public float Health;
        public float MaxHealth;
        private int BaseHealth = 0;

        private float DamageMult;
        public float DamageAmp => DamageMult;
        public float FireDmgAmp;

        public long Bounty;

        public float SteadFest = 100;
        private int SteadFestCap = 100000;
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

        //Closest player, for detecting if in range to cast abilities
        private GameObject closestPlayer;
        private float closestPlayerMagnitude;

        private Color normalColor;

        public enum Abilities { SteadFest, BossSteadFest, EliteSteadFest, Molten, FreezingAura, FireAura, Rooting, BlackHole, Trapper, Juggernaut, Huge, Tiny, ExtraDamage, ExtraHealth, Illusionist, Blink, Thunder, RainEmpowerement, Shielding, Meteor, RockTosser, DoubleLife, Laser, Poisonous }
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
            SteadFest = 1000;
            slows = new Dictionary<int, SlowDebuff>();
            abilities = new List<Abilities>();

            //picking abilities
            if (UnityEngine.Random.value < 0.1 || _AI.creepy_boss)
            {
                int count = UnityEngine.Random.Range(2, 7);
                if (_AI.creepy_boss) { count = 10; }   //Megan boss always has abilities, a lot of them.


                int i = 0;
                Array arr = Enum.GetValues(typeof(Abilities));

                //Determining if enemy is Elite, Miniboss or Boss type of enemy
                if (count > 6 && Random.value < 0.3f) { type = EnemyType.Boss; abilities.Add(Abilities.BossSteadFest); }
                else if (count > 4 && Random.value < 0.3f) { abilities.Add(Abilities.EliteSteadFest); type = EnemyType.Miniboss; }
                else { type = EnemyType.Elite; }

                if (_AI.creepy_boss)    //Force adding BossSteadFest to Megan
                {
                    abilities.Clear();
                    type = EnemyType.Boss;
                    abilities.Add(Abilities.BossSteadFest);
                }

                //Trial and error method of picking abilities
                while (i < count)
                {
                    bool success = true;
                    Abilities ab = (Abilities)arr.GetValue(UnityEngine.Random.Range(0, arr.Length));
                    if (ab == Abilities.SteadFest || ab == Abilities.EliteSteadFest || ab == Abilities.BossSteadFest)
                    {
                        if (abilities.Contains(Abilities.SteadFest) || abilities.Contains(Abilities.EliteSteadFest) || abilities.Contains(Abilities.BossSteadFest))
                        {
                            success = false;
                        }
                    }
                    else if (ab == Abilities.Tiny || ab == Abilities.Huge)
                    {
                        if (abilities.Contains(Abilities.Huge) || abilities.Contains(Abilities.Tiny))
                        {
                            success = false;
                        }
                    }
                    else if (ab == Abilities.DoubleLife && !(_AI.creepy || _AI.creepy_fat))
                    {
                        success = false;

                    }
                    if (abilities.Contains(ab))
                    {
                        success = false;
                    }
                    if (success)
                    {
                        i++; abilities.Add(ab);
                    }

                }

            }
            else
            {
                type = EnemyType.Normal;
            }


            AssignEnemyType();

            //Assigning level
            AssignLevel();

            //Assigning rest of stats
            DamageMult = Level / 4.5f + 0.55f;
            Armor = Mathf.FloorToInt(Random.Range(Level * Level * 0.5f, Level * Level * 1.1f));
            ArmorReduction = 0;
            Health = Mathf.RoundToInt((_Health.Health * Mathf.Pow(Level, 1.3f)));
            AnimSpeed = 1f + (float)Level / 125;

            //Extra health for boss type enemies
            switch (type)
            {
                case EnemyType.Elite:
                    Health *= 2;

                    break;
                case EnemyType.Miniboss:
                    Health *= 5;

                    break;
                case EnemyType.Boss:
                    Health *= 10;
                    break;
            }

            //Applying some abilities
            if (abilities.Contains(Abilities.Huge))
            {
                Armor *= 2;
                gameObject.transform.localScale *= 2;
                Health *= 2;
                DamageMult *= 2;
            }
            else if (abilities.Contains(Abilities.Tiny))
            {
                gameObject.transform.localScale *= 0.35f;
                Health *= 2;
                DamageMult *= 1.5f;
                BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);
            }
            if (abilities.Contains(Abilities.SteadFest))
            {
                SteadFest = 8;
            }
            if (abilities.Contains(Abilities.EliteSteadFest))
            {
                SteadFest = 2f;
            }
            if (abilities.Contains(Abilities.BossSteadFest))
            {
                SteadFest = 1;
            }
            if (abilities.Contains(Abilities.ExtraHealth))
            {
                Health *= 3;
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
            if (abilities.Contains(Abilities.FireAura))
            {
                StartCoroutine(FireAuraLoop());
            }
            //Clamping Health
            Health = Mathf.Min(Health, int.MaxValue - 5);


            //Setting other health variables
            _Health.maxHealthFloat = Health;
            MaxHealth = Health;
            _Health.maxHealth = Mathf.RoundToInt(Health);
            _Health.Health = Mathf.RoundToInt(Health);

            RollName();

            DualLifeSpend = false;
            setupComplete = true;
            OnDieCalled = false;

            BaseAnimSpeed = AnimSpeed;

            AssignBounty();

            SteadFestCap = Mathf.RoundToInt(SteadFest * 0.01f * MaxHealth);
            if (SteadFestCap < 1) // Making sure the enemy can be killed
            {
                SteadFestCap = 1;
            }

            CreationTime = Time.time;
        }
        /// <summary>
        /// Slows this enemy
        /// </summary>
        /// <param name="source">ID of the source, used for refreshing the duration of applied effects</param>
        public void Slow(int source, float amount, float time)
        {
            if (slows.ContainsKey(source))
            {
                slows[source].duration = Mathf.Max(slows[source].duration, time);
                slows[source].amount = amount;
            }
            else
            {
                slows.Add(source, new SlowDebuff()
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
                playerHitEnemy.Hit = -amount - 50000;
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
            damage = ClampDamage(true, damage);
            _Health.HitReal(damage);

        }
        public int ClampDamage(bool pure, int damage)
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

            if (pure)
            {
                if (damage > SteadFestCap)
                {
                    int dmgpure = Mathf.Min(damage, SteadFestCap);
                    return dmgpure;
                }
                return damage;
            }

            float reduction = ModReferences.DamageReduction(Mathf.Clamp(Armor - ArmorReduction, 0, int.MaxValue));

            //reduction = Mathf.Clamp01(reduction);
            int dmg = Mathf.CeilToInt(damage * (1 - reduction));
            dmg = Mathf.Min(dmg, SteadFestCap);

            //ModAPI.Console.Write("reducted damage " + Armor + " - " + ArmorReduction + " -------->" + damage + " * " + reduction + " --> " + dmg);

            return dmg;

        }
        public bool OnDie()
        {
            try
            {
                //ModAPI.Console.Write("Enemy Dies, \nhealth left "+ _Health.Health+ "\n OnDieCalled "+ OnDieCalled.ToString()+"\nDrowned "+ setup.waterDetect.drowned);
                if (_Health.Health > 0)
                {
                    return false;
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

                if (abilities.Contains(Abilities.Molten))
                {
                    //not working, find a fix or replacement
                    //BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position, Quaternion.identity);
                    //BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.right * 2, Quaternion.identity);
                    //BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.left * 2, Quaternion.identity);
                    //BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.forward * 2, Quaternion.identity);
                    //BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.back * 2, Quaternion.identity);

                }
                if (OnDieCalled)
                {
                    return true;
                }
                OnDieCalled = true;
                Invoke("ReanimateMe", 30);
                EnemyManager.RemoveEnemy(this);

                if (setup.waterDetect.drowned)
                {
                    return true;
                }
                if (Random.value < 0.2f || _AI.creepy_boss || abilities.Count >= 2)
                {
                    int itemCount = Random.Range(1, 4);
                    if (_AI.creepy_boss)
                    {
                        itemCount += 12;
                    }
                    else if (abilities.Count >= 2)
                    {
                        itemCount += Random.Range(2, 5);
                    }
                    for (int i = 0; i < itemCount; i++)
                    {
                        Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(Bounty, enemyType), transform.position + Vector3.up * 3);
                    }
                    if (enemyType == Enemy.Megan && (int)ModSettings.difficulty > 6)
                    {
                        //Drop megan only amulet
                        //Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(Bounty, enemyType), transform.position + Vector3.up * 3);

                    }
                }
                if (BoltNetwork.isRunning)
                {
                    Network.NetworkManager.SendLine("KX" + Mathf.RoundToInt(Bounty / ModReferences.Players.Count) + ";", Network.NetworkManager.Target.Everyone);
                }
                else
                {
                    ModdedPlayer.instance.AddKillExperience(Bounty);
                }
            }
            catch (Exception ex)
            {

                ModAPI.Log.Write("DIEING ENEMY EXCEPTION  " + ex.ToString());
            }

            return true;

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
            AnimSpeed = BaseAnimSpeed;
            int[] Keys = new List<int>(slows.Keys).ToArray();
            for (int i = 0; i < Keys.Length; i++)
            {
                int key = Keys[i];
                AnimSpeed *= slows[key].amount;
                slows[key].duration -= Time.deltaTime;
                if (slows[key].duration < 0)
                {
                    slows.Remove(key);
                }
            }
            Health = _Health.Health;
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
                    _Health.MySkin.material.color = Color.magenta;
                    AnimSpeed *= 1.2f;
                    gameObject.transform.localScale *= 1.3f;

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
            if (DualLifeSpend)
            {
                transform.localScale *= 1.2f;
                _Health.MySkin.material.color = Color.green;
                AnimSpeed *= 1.2f;
            }


            if (inRange)
            {
                if (abilities.Contains(Abilities.Meteor) && MeteorCD <= 0)
                {
                    Vector3 dir = closestPlayer.transform.position;
                    Network.NetworkManager.SendLine("MT" + dir.x + ";" + dir.y + ";" + dir.z + ";" + Random.Range(-100000, 100000) + ";", Network.NetworkManager.Target.Everyone);
                    MeteorCD = 50f;
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
                    float duration = 2;
                    switch (ModSettings.difficulty)
                    {
                        case ModSettings.Difficulty.Hard:
                            duration = 2.4f;
                            break;
                        case ModSettings.Difficulty.Elite:
                            duration = 2.8f;
                            break;
                        case ModSettings.Difficulty.Master:
                            duration = 3;
                            break;
                        case ModSettings.Difficulty.Challenge1:
                            duration = 3.3f;
                            break;
                        case ModSettings.Difficulty.Challenge2:
                            duration = 3.6f;
                            break;
                        case ModSettings.Difficulty.Challenge3:
                            duration = 4;
                            break;
                        case ModSettings.Difficulty.Challenge4:
                            duration = 4.5f;
                            break;
                        case ModSettings.Difficulty.Challenge5:
                            duration = 6f;
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
                        float damage = 40 * Level;
                        float duration = 6;
                        float radius = 17;
                        float pullforce = 16;
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
        private void AssignEnemyType()
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
        private void ReanimateMe()
        {
            OnDieCalled = false;
        }
        private void AssignLevel()
        {
            float lvlMult = 1;
            if (_AI.creepy || _AI.creepy_fat || _AI.creepy_male)
            {
                lvlMult = 2.6f;

            }
            else if (_AI.creepy_baby)
            {
                lvlMult = 0.5f;
            }
            else if (_AI.creepy_boss)
            {
                lvlMult = 20;
            }
            else if (_AI.leader)
            {
                lvlMult = 1.5f;
            }
            else if (_AI.painted)
            {
                lvlMult *= 2.2f;
            }
            else if (_AI.pale)
            {
                lvlMult *= 1.8f;
                if (_AI.skinned)
                {
                    lvlMult *= 1.8f;
                }
            }

            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Normal:
                    Level = Random.Range(1, 3);
                    break;
                case ModSettings.Difficulty.Hard:
                    Level = Random.Range(13, 28);

                    break;
                case ModSettings.Difficulty.Elite:
                    Level = Random.Range(35, 43);

                    break;
                case ModSettings.Difficulty.Master:
                    Level = Random.Range(50, 56);

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Level = Random.Range(60, 66);

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Level = Random.Range(70, 80);

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Level = Random.Range(85, 95);

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Level = Random.Range(105, 120);

                    break;
                case ModSettings.Difficulty.Challenge5:
                    Level = Random.Range(160, 170);

                    break;
            }
            Level = Mathf.CeilToInt(Level * lvlMult);
        }
        private void AssignBounty()
        {
            Bounty = (int)(Random.Range(Health * 0.3f, Health * 0.4f) + Random.Range(Armor * 0.2f, Armor * 0.25f) / 3);
            if (abilities.Count > 0)
            {
                Bounty = Mathf.RoundToInt(Bounty * abilities.Count * 0.9f);
            }
            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Hard:
                    Bounty = Mathf.RoundToInt(Bounty * 1.3f);
                    break;
                case ModSettings.Difficulty.Elite:
                    Bounty = Mathf.RoundToInt(Bounty * 2);

                    break;
                case ModSettings.Difficulty.Master:
                    Bounty = Mathf.RoundToInt(Bounty * 4.6f);

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Bounty = Mathf.RoundToInt(Bounty * 10f);

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Bounty = Mathf.RoundToInt(Bounty * 22);

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Bounty = Mathf.RoundToInt(Bounty * 48f);

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Bounty = Mathf.RoundToInt(Bounty * 100f);

                    break;
                case ModSettings.Difficulty.Challenge5:
                    Bounty = Mathf.RoundToInt(Bounty * 250);

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///Deals damage to nearby players
        /// </summary>
        private IEnumerator FireAuraLoop()
        {

            float dmg = 40f * DamageAmp;
            if (BoltNetwork.isRunning)
            {
                while (true)
                {
                    yield return new WaitForSeconds(1f);
                    foreach (BoltEntity item in ModReferences.AllPlayerEntities)
                    {
                        if ((item.transform.position - transform.position).sqrMagnitude < 80)
                        {
                            PlayerHitByEnemey playerHitByEnemey = PlayerHitByEnemey.Create(item);
                            playerHitByEnemey.Damage = (int)dmg;
                            playerHitByEnemey.Send();
                        }
                    }
                }
            }
            else
            {
                while (true)
                {
                    if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < 80)
                    {
                        LocalPlayer.Stats.Health -= Time.deltaTime * dmg;
                    }
                    yield return null;
                }
            }
        }

    }
}
