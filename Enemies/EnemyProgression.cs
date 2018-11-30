using ChampionsOfForest.Enemies;
using System;
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
        public enum Abilities { SteadFest, BossSteadFest, EliteSteadFest, Molten, FreezingAura, FireAura, Rooting, BlackHole, Mines, Juggernaut, Huge, Tiny, ExtraDamage, ExtraHealth, Illusionist, Blink, Thunder, RainEmpowerement, Shielding, Meteor, RockTosser, DoubleLife, Laser }
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
        private readonly float agroRange = 40;
        private float freezeauraCD;
        private float blackholeCD;

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

        private void Start()
        {
            try
            {
                ModAPI.Console.Write("SETUP: Created EnemyProgression");
            }

            catch (Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());

            }
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
                        ModAPI.Console.Write("NULL1");
                        entity = _Health.entity;

                    }
                    if (entity == null)
                    {
                        ModAPI.Console.Write("NULL2");

                        entity = transform.root.GetComponent<BoltEntity>();

                    }
                    if (entity == null)
                    {
                        ModAPI.Console.Write("NULL3");

                    }
                    else
                    {
                        EnemyManager.AddHostEnemy(this);
                        ModAPI.Console.Write("Adding enemy " + entity.networkId.PackedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                ModAPI.Console.Write(ex.ToString());
            }
            SteadFest = 100;

            abilities = new List<Abilities>();

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

                        }
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
                    Level = Random.Range(1, 6);
                    break;
                case ModSettings.Difficulty.Hard:
                    Level = Random.Range(5, 15);

                    break;
                case ModSettings.Difficulty.Elite:
                    Level = Random.Range(15, 25);

                    break;
                case ModSettings.Difficulty.Master:
                    Level = Random.Range(25, 35);

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Level = Random.Range(35, 50);

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Level = Random.Range(48, 65);

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Level = Random.Range(59, 73);

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Level = Random.Range(66, 87);

                    break;
                case ModSettings.Difficulty.Challenge5:
                    Level = Random.Range(80, 101);

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

            }

            ArmorReduction = 0;

            int hp = _Health.Health * Level;
            _Health.Health = hp;
            MaxHealth = hp;
            _Health.maxHealth = hp;
            Health = hp;
            _Health.maxHealthFloat = hp;

            AnimSpeed = 1 + (float)Level / 50;

            ExpBounty = (int)Random.Range(Health * 0.35f, Health * 1.15f);

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

        private void Update()
        {
            if (!setupComplete)
            {
                Setup();
            }
            Health = _Health.Health;
            bool inRange = false;
            foreach (GameObject g in _AI.allPlayers)
            {
                if (Vector3.Distance(g.transform.position, transform.position) < agroRange)
                {
                    inRange = true;
                    break;
                }
            }
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

                }
            }
            if (inRange)
            {
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

        public int ClampDamage(bool pure, int damage)
        {
            try
            {
                if (pure)
                {
                    if (damage > SteadFestCap)
                    {
                        damage = Mathf.Clamp(damage, 0, SteadFestCap);
                        return damage;
                    }
                }
                float reduction = Mathf.Sqrt((Armor - ArmorReduction) / 10) / 100;
                reduction = Mathf.Min(1, reduction);
                int dmg = Mathf.FloorToInt(damage * (1 - reduction));
                dmg = Mathf.Min(dmg, SteadFestCap);
                return dmg;
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write("ERROR: CLAMPING DMG " + ex.ToString());
            }
            return 1;
        }

        public void OnDie()
        {

            if (setup.waterDetect.drowned)
            {
                ModAPI.Log.Write("Enemy drowned");
                return;
            }
            if (EnemyManager.hostDictionary.ContainsKey(entity.networkId.PackedValue))
            {
                EnemyManager.hostDictionary.Remove(entity.networkId.PackedValue);
            }

            if (GameSetup.IsMultiplayer)
            {
                Network.NetworkManager.SendLine("KX" + Mathf.RoundToInt((float)ExpBounty / ModReferences.PlayerRemoteSetups.Count) + ";", Network.NetworkManager.Target.Everyone);
            }
            else
            {
                ModdedPlayer.instance.AddKillExperience(ExpBounty);
            }
        }
    }
    public class ClinetEnemyProgression
    {
        public static float LifeTime = 15;
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
