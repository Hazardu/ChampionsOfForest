using ChampionsOfForest.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
    public class EnemyProgression : MonoBehaviour
    {
        public enum EnemyType { Normal, Elite, Miniboss, Boss }
        public EnemyType type;

        public EnemyHealth _Health;
        public mutantAI _AI;
        public BoltEntity entity;
        public mutantScriptSetup setup;


        public string EnemyName;
        public int Level;
        public float Health;
        public float MaxHealth;
        public int ExpBounty;
        public int Armor;
        public int ArmorReduction;
        private bool setupComplete = false;
        public float SteadFest;
        private int SteadFestCap;
        private float DamageMult;
        public enum Abilities { SteadFest, BossSteadFest, EliteSteadFest, Molten, FreezingAura, FireAura, Rooting, BlackHole, Trapper, Juggernaut, Huge, Tiny, ExtraDamage, ExtraHealth, Illusionist, Blink, Thunder, RainEmpowerement, Shielding, Meteor, RockTosser, DoubleLife, Laser,Poisonous }
        public List<Abilities> abilities;

        public static string[] Mnames = new string[]
        {
            "Farket",
            "Hazard",
            "Moritz",
            "Souldrinker",
            "Broadbent",
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
            "PlsunbanmeonBBdiscord",
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
            "NÜT",
            "AssPirate",
            "Azpect",
            "LumaR",
            "TeigRolle",
            "Foreck",
            "Samuel","Sebastian","David","Carter","Wyatt","Jayden","John","Owen","Dylan",
            "Luke","Gabriel","Anthony","Isaac","Grayson","Jack","Julian","Levi",
            "Christopher","Joshua","Andrew","Lincoln","Mateo","Ryan","Jaxon",

        };
        private float prerainSpeed;
        public float AnimSpeed = 1;
        private Vector3 preRainScale;
        private float prerainDmg;
        private int prerainArmor;
        private readonly float agroRange = 1200;
        private float freezeauraCD;
        private float blackholeCD;
        private float blinkCD;
        private float shieldingCD;
        private float shieldingON;
        public bool CCimmune = false;


        public float DamageAmp
        {
            get
            {
                return DamageMult;
            }
        }


        private void RollName()
        {
            List<string> names = new List<string>();
            if (_AI.female || _AI.creepy || _AI.femaleSkinny)    //is female
            {
                names.AddRange(new string[] { "Lizz Plays", "Wolfskull", "Wiktoria", "Olivier Broadbent, who put '!' in the title of every single one of his videos.", "Emma", "Olivia", "Ava", "Isabella", "Sophia", "Mia", "CharlotteAmelia", "Evelyn", "Abigail", "Harper", "Emily", "Elizabeth", "Avery", "Sofia", "Ella", "Madison", "Scarlett", "Victoria", "Aria", "GraceChloe", "Camila", "Penelope", "Riley", "Layla", "Lillian", "Nora", "Zoey", "Mila", "Aubrey", "Hannah", "Lily", "Addison", "Eleanor", "Natalie", "Luna", "Savannah", "Brooklyn", "Leah", "Zoe", "Stella", "Hazel", "Ellie", "Paisley", "Audrey", "Skylar", "Violet", "Claire", "Bella", "Aurora", "Lucy", "Anna", "Samantha", "Caroline", "Genesis", "Aaliyah", "Kennedy", "Kinsley", "Allison", "Maya", "Sarah", "Madelyn", "Adeline", "Alexa", "Ariana", "Elena", "Gabriella", "Naomi", "Alice", "Sadie", "Hailey", "Eva", "Emilia", "Autumn", "Quinn", "Nevaeh", "Piper", "Ruby", "Serenity", "Willow", "Everly", "Cora", "Kaylee", "Lydia", "Aubree", "Arianna", "Eliana", "Peyton", "Melanie", "Gianna", "Isabelle", "Julia", "Valentina", "Nova", "Clara", "Vivian", "Reagan", "Mackenzie", "Madeline", "Brielle", "Delilah", "Isla", "Rylee", "Katherine", "Sophie", "Josephine", "Ivy", "Liliana", "Jade", "Maria", "Taylor", "Hadley", "Kylie", "Emery", "Adalynn", "Natalia", "Annabelle", "Faith", "Alexandra", "Ximena", "Ashley", "Brianna", "Raelynn", "Bailey", "Mary", "Athena", "Andrea", "Leilani", "Jasmine", "Lyla", "Margaret", "Alyssa", "Adalyn", "Arya", "Norah", "Khloe", "Kayla", "Eden", "Eliza", "Rose", "Ariel", "Melody", "Alexis" });

            }
            else                                                 // is male
            {
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
            EnemyName = names[Random.Range(0, names.Count)];

        }

        private void Setup()
        {
            try
            {
                if (BoltNetwork.isRunning)
                {
                    entity = transform.root.GetComponentInChildren<BoltEntity>();
                    if (entity == null)
                    {
                        entity = _Health.entity;

                    }
                    if (entity == null)
                    {

                        entity = transform.root.GetComponent<BoltEntity>();

                    }
                    if (entity == null)
                    {

                    }
                    else
                    {
                        EnemyManager.AddHostEnemy(this);
                    }
                }
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());
            }
            SteadFest = 100;

            abilities = new List<Abilities>() { Abilities.Laser, Abilities.Meteor};

            if (UnityEngine.Random.value < 0.1)
            {
                int count = UnityEngine.Random.Range(2, 7);


                int i = 0;
                Array arr = Enum.GetValues(typeof(Abilities));


                if (count > 5) { type = EnemyType.Boss; abilities.Add(Abilities.BossSteadFest); }
                else if (count > 4) { type = EnemyType.Miniboss; }
                else { type = EnemyType.Elite; }

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
                    else if (ab == Abilities.DoubleLife &&!(_AI.creepy|| _AI.creepy_boss|| _AI.creepy_fat|| _AI.creepy_male))
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
                    Level = Random.Range(10, 24);

                    break;
                case ModSettings.Difficulty.Elite:
                    Level = Random.Range(30, 37);

                    break;
                case ModSettings.Difficulty.Master:
                    Level = Random.Range(39, 44);

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Level = Random.Range(48, 55);

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Level = Random.Range(60, 70);

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Level = Random.Range(75, 85);

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Level = Random.Range(90, 110);

                    break;
                case ModSettings.Difficulty.Challenge5:
                    Level = Random.Range(120, 150);

                    break;
            }
            Level = Mathf.RoundToInt(Level * lvlMult);
            DamageMult = (float)Level / 6 + 0.45f;
            Armor = Mathf.RoundToInt(Random.Range(Level * 0.1f, Level * Level * 3));
            if (abilities.Contains(Abilities.Huge))
            {
                Armor *= 2;
                gameObject.transform.localScale *= 2;
            }
            else if (abilities.Contains(Abilities.Tiny))
            {
                gameObject.transform.localScale *= 0.35f;

                    BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);

                
            }

            ArmorReduction = 0;

            int hp = _Health.Health * Level;
            _Health.Health = hp;
            MaxHealth = hp;
            _Health.maxHealth = hp;
            Health = hp;
            _Health.maxHealthFloat = hp;

            AnimSpeed = 1 + (float)Level / 100;

            ExpBounty = (int)Random.Range(Health * 0.35f, Health * 0.7f);
            ExpBounty += (int)Random.Range(Armor * 0.10f, Armor * 0.45f);
            ExpBounty *= Mathf.RoundToInt(abilities.Count * 1.5f + 1);
            if (abilities.Contains(Abilities.SteadFest))
            {
                SteadFest = 10;
            }
            if (abilities.Contains(Abilities.EliteSteadFest))
            {
                SteadFest = 2.5f;
            }
            if (abilities.Contains(Abilities.BossSteadFest))
            {
                SteadFest = 1;
            }
            if (abilities.Contains(Abilities.ExtraHealth))
            {
                _Health.Health *= 3;
                Health *= 3;
                MaxHealth *= 3;
                _Health.maxHealthFloat *= 3;
                _Health.maxHealth *= 3;
            }
            if (abilities.Contains(Abilities.ExtraDamage))
            {
                DamageMult *= 2.5f;
            }


            SteadFestCap = Mathf.RoundToInt(SteadFest * 0.01f * MaxHealth);
            if (SteadFestCap < 1)
            {
                SteadFestCap = 1;
            }

            if (abilities.Contains(Abilities.RainEmpowerement))
            {
                preRainScale = transform.localScale;
                prerainDmg = DamageMult;
                prerainArmor = Armor;
                prerainSpeed = setup.animator.speed;
            }
            if (abilities.Contains(Abilities.Juggernaut))
            {
                CCimmune = true;
            }
            if (abilities.Contains(Abilities.FireAura))
            {
                StartCoroutine(FireAuraLoop());
            }
            DualLifeSpend = false;
            RollName();
            setupComplete = true;
            Invoke("SetHealthToMax", 10);
        }
        private void SetHealthToMax()
        {
            _Health.Health = (int)MaxHealth;
            setup.animator.speed *= AnimSpeed;
            _AI.animSpeed *= AnimSpeed;
            _AI.rotationSpeed *= AnimSpeed;
            prerainSpeed = setup.animator.speed;

        }
        IEnumerator FireAuraLoop()
        {
            
            float dmg = 40f *DamageAmp;
            if (BoltNetwork.isRunning)
            {
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);
                    foreach (var item in ModReferences.AllPlayerEntities)
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
        private GameObject closestPlayer;
        private float closestPlayerMagnitude;
        private float StunCD;
        private float TrapCD;
        private float LaserCD;
        private float MeteorCD;
        private void Update()
        {
            if (!setupComplete)
            {
                Setup();
            }
           if(TrapCD > 0) TrapCD -= Time.deltaTime;
           if(StunCD > 0) StunCD -= Time.deltaTime;
           if(LaserCD > 0) LaserCD -= Time.deltaTime;
           if(MeteorCD > 0) MeteorCD -= Time.deltaTime;
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
                if (Scene.WeatherSystem.Raining)
                {
                    Armor = prerainArmor * 5;
                    DamageMult = prerainDmg * 5;
                    transform.localScale = preRainScale * 1.5f;

                    setup.animator.speed = prerainSpeed * 2;

                }
                else
                {
                    Armor = prerainArmor;
                    DamageMult = prerainDmg;
                    transform.localScale = preRainScale;
                    setup.animator.speed = prerainSpeed;

                }
            }
            else
            {
                if (abilities.Contains(Abilities.Huge))
                {
                    gameObject.transform.localScale = Vector3.one * 2;
                }
                else if (abilities.Contains(Abilities.Tiny))
                {
                    gameObject.transform.localScale = Vector3.one * 0.35f;
                    BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);

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
                        _Health.MySkin.material.color =normalColor;
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
               
            }


            if (inRange)
            {
                if (abilities.Contains(Abilities.Meteor)&& MeteorCD<= 0)
                {
                    Vector3 dir = closestPlayer.transform.position;
                    Network.NetworkManager.SendLine("MT" + dir.x + ";" + dir.y + ";" + dir.z + ";"+(int)Random.Range(-100000,100000)+";", Network.NetworkManager.Target.Everyone);
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
                if (abilities.Contains(Abilities.Laser) && LaserCD <=0)
                {
                    Vector3 dir = closestPlayer.transform.position;
                   
                    LaserCD = 100;
                    Network.NetworkManager.SendLine("LA" + transform.position.x + ";" + transform.position.y + ";" + transform.position.z + ";" + dir.x + ";" + dir.y+2 + ";" + dir.z + ";",Network.NetworkManager.Target.Everyone);


                }
                if (abilities.Contains(Abilities.Rooting)&& StunCD <= 0)
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
                            duration = 5f;
                            break;
                        default:
                            break;
                    }
                    Network.NetworkManager.SendLine("RO" + transform.position.x + ";" + transform.position.y + ";" + transform.position.z + ";" + duration + ";", Network.NetworkManager.Target.Everyone);
                    StunCD = Random.Range(15,30);
                }

                if (abilities.Contains(Abilities.Trapper)&&TrapCD <=0)
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

        }
        public void HitMagic(int damage)
        {
            damage = ClampDamage(true, damage);
            _Health.HitReal(damage);

        }

        Color normalColor;
        public int ClampDamage(bool pure, int damage)
        {

            if (abilities.Contains(Abilities.Shielding))
            {
                ModAPI.Console.Write("Shielding is on");
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


            if (pure)
            {
                if (damage > SteadFestCap)
                {
                    damage = Mathf.Clamp(damage, 0, SteadFestCap);
                    return damage;
                }
            }
            float reduction = Mathf.Sqrt((Armor - ArmorReduction) / 100);
            reduction = Mathf.Clamp01(reduction);
            int dmg = Mathf.FloorToInt(damage * (1 - reduction));
            dmg = Mathf.Min(dmg, SteadFestCap);
            return dmg;

        }

        private bool DualLifeSpend = false;
        public bool OnDie()
        {
            try
            {
                if (abilities.Contains(Abilities.DoubleLife))
                {
                    if (!DualLifeSpend)
                    {
                        DualLifeSpend = true;
                        _Health.Health = _Health.maxHealth / 2;
                        _Health.MySkin.material.color = Color.green;
                        setup.animator.speed *= 1.2f;
                        _AI.animSpeed *= 1.2f;
                        _AI.rotationSpeed *= 1.2f;
                        prerainSpeed = setup.animator.speed;
                        DamageMult *= 2;
                        _Health.releaseFromTrap();
                        return false;
                    }
                }

                if (abilities.Contains(Abilities.Molten))
                {
                    BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position, Quaternion.identity);
                    BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.right * 2, Quaternion.identity);
                    BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.left * 2, Quaternion.identity);
                    BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.forward * 2, Quaternion.identity);
                    BoltNetwork.Instantiate(BoltPrefabs.instantDynamite, transform.position + Vector3.back * 2, Quaternion.identity);

                }

                EnemyManager.RemoveEnemy(this);

                if (setup.waterDetect.drowned)
                {
                    return true;
                }
                if (Random.value < 0.2f || _AI.creepy_boss)
                {

                    Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(ExpBounty), transform.position+ Vector3.up *2);

                }
                if (BoltNetwork.isRunning)
                {
                    Network.NetworkManager.SendLine("KX" + Mathf.RoundToInt((float)ExpBounty / ModReferences.Players.Count) + ";", Network.NetworkManager.Target.Everyone);
                }
                else
                {
                    ModdedPlayer.instance.AddKillExperience(ExpBounty);
                }
            }
            catch (Exception ex)
            {

                ModAPI.Log.Write("DIEING ENEMY EXCEPTION  "+ex.ToString());
            }

            return true;

        }
    }
    public class ClinetEnemyProgression
    {
        public static float LifeTime = 10;
        public BoltEntity Entity;
        public ulong Packed;
        public string EnemyName;
        public int Level;
        public float Health;
        public float MaxHealth;
        public int ExpBounty;
        public int Armor;
        public int ArmorReduction;
        public float SteadFest;
        public int[] Affixes;
        public float creationTime;


        public ClinetEnemyProgression(Transform tr)
        {
            creationTime = Time.time;
            EnemyProgression p = tr.root.GetComponent<EnemyProgression>();
            if (p == null)
            {
                p = tr.root.GetComponentInChildren<EnemyProgression>();
            }
            EnemyName = p.EnemyName;
            Level = p.Level;
            Health = p.Health;
            MaxHealth = p.MaxHealth;
            ExpBounty = p.ExpBounty;
            Armor = p.Armor;
            ArmorReduction = p.ArmorReduction;
            SteadFest = p.SteadFest;
            Affixes = new int[p.abilities.Count];
            for (int i = 0; i < p.abilities.Count; i++)
            {
                Affixes[i] = (int)p.abilities[i];
            }
        }
        public ClinetEnemyProgression()
        {

        }
        public ClinetEnemyProgression(BoltEntity e)
        {

            creationTime = Time.time;
            Entity = e;
            Packed = e.networkId.PackedValue;
            if (GameSetup.IsMpClient)
            {
                Network.NetworkManager.SendLine("EE" + Packed.ToString() + ";", Network.NetworkManager.Target.OnlyServer);
            }
            else
            {
                EnemyProgression p = EnemyManager.hostDictionary[Packed];
                EnemyName = p.EnemyName;
                Level = p.Level;
                Health = p.Health;
                MaxHealth = p.MaxHealth;
                ExpBounty = p.ExpBounty;
                Armor = p.Armor;
                ArmorReduction = p.ArmorReduction;
                SteadFest = p.SteadFest;
                Affixes = new int[p.abilities.Count];
                for (int i = 0; i < p.abilities.Count; i++)
                {
                    Affixes[i] = (int)p.abilities[i];
                }
            }
            if (!EnemyManager.clinetProgressions.ContainsKey(e))
            {
                EnemyManager.clinetProgressions.Add(e, this);
            }

        }

        public ClinetEnemyProgression(BoltEntity entity, string enemyName, int level, float health, float maxHealth, int expBounty, int armor, int armorReduction, float steadFest, int[] affixes) : this(entity)
        {
            Entity = entity;
            EnemyName = enemyName;
            Packed = entity.networkId.PackedValue;
            Level = level;
            Health = health;
            MaxHealth = maxHealth;
            ExpBounty = expBounty;
            Armor = armor;
            ArmorReduction = armorReduction;
            SteadFest = steadFest;
            Affixes = affixes;
            creationTime = Time.time;
            if (!EnemyManager.clinetProgressions.ContainsKey(entity))
            {
                EnemyManager.clinetProgressions.Add(entity, this);
            }
        }

    }
}
