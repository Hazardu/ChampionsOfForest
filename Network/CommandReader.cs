using ChampionsOfForest.Enemies;
using ChampionsOfForest.Player;
using System;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Network
{
    public class CommandReader
    {
        private static char[] ch;
        private static string parseval;
        private static int i;



        public static void OnCommand(string s)
        {
            if (s.StartsWith("AB"))     //ask the host to send the command to set the difficulty for clinet
            {
                if (GameSetup.IsMpServer && ModSettings.DifficultyChoosen)
                {
                    string answer = "AA" + (int)ModSettings.difficulty + ";";
                    if (ModSettings.FriendlyFire)
                    {
                        answer += "t;";
                    }
                    else
                    {
                        answer += "f;";
                    }

                    Network.NetworkManager.SendLine(answer, Network.NetworkManager.Target.Clinets);
                }
            }
            else if (s.StartsWith("AA"))    //answer for the what is the difficulty query
            {
                if (ModSettings.DifficultyChoosen || !GameSetup.IsMpClient)
                {
                    return;
                }
                i = 2;
                ch = s.ToCharArray();
                int index = int.Parse(Read());
                Array values = Enum.GetValues(typeof(ModSettings.Difficulty));
                ModSettings.difficulty = (ModSettings.Difficulty)values.GetValue(index);
                ModSettings.DifficultyChoosen = true;
                LocalPlayer.FpCharacter.UnLockView();
                LocalPlayer.FpCharacter.MovementLocked = false;
            }
            else if (s.StartsWith("SC"))    //spell cast
            {
                i = 2;
                ch = s.ToCharArray();
                int spellid = int.Parse(Read());
                if (spellid == 1)
                {
                    Vector3 pos = new Vector3(float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                    BlackHole.CreateBlackHole(pos, ReadBool(), float.Parse(Read()), float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                }
                else if (spellid == 2)
                {
                    Vector3 pos = new Vector3(float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                    HealingDome.CreateHealingDome(pos, float.Parse(Read()), float.Parse(Read()), ReadBool(), float.Parse(Read()));
                }

            }
            else if (s.StartsWith("RI"))    //remove item
            {
                i = 2;
                ch = s.ToCharArray();
                int id = int.Parse(Read());
                PickUpManager.RemovePickup(id);

            }
            else if (s.StartsWith("CI"))    //create item
            {
                i = 2;
                ch = s.ToCharArray();
                Item item = new Item(ItemDataBase.Instance.ItemBases[int.Parse(Read())], 1, 0, false);   //reading first value, id
                int id = int.Parse(Read());
                item.level = int.Parse(Read());
                int amount = int.Parse(Read());
                Vector3 pos = new Vector3(float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                while (i < ch.Length)
                {
                    ItemStat stat = new ItemStat(ItemDataBase.Instance.Stats[int.Parse(Read())])
                    {
                        Amount = float.Parse(Read())
                    };
                    item.Stats.Add(stat);
                }
                PickUpManager.SpawnPickUp(item, pos, amount, id);
            }
            else if (s.StartsWith("EE"))       //host has been asked to share info on enemy
            {
                if (!GameSetup.IsMpClient)
                {
                    i = 2;
                    ch = s.ToCharArray();
                    ulong packed = ulong.Parse(Read());
                    if (EnemyManager.hostDictionary.ContainsKey(packed))
                    {
                        EnemyProgression ep = EnemyManager.hostDictionary[packed];
                        parseval = "EA" + packed + ";" + ep.EnemyName + ";" + ep.Level + ";" + ep.Health + ";" + ep.MaxHealth + ";" + ep.ExpBounty + ";" + ep.Armor + ";" + ep.ArmorReduction + ";" + ep.SteadFest + ";" + ep.abilities.Count;
                        foreach (EnemyProgression.Abilities item in ep.abilities)
                        {
                            parseval += (int)item + ";";
                        }
                        Network.NetworkManager.SendLine(parseval, Network.NetworkManager.Target.Everyone);
                    }


                }
            }
            else if (s.StartsWith("ES"))       //enemy spell
            {
                i = 2;
                ch = s.ToCharArray();
                int id = int.Parse(Read());
                switch (id)
                {
                    case 1: //snow aura
                        ulong packed = ulong.Parse(Read());
                        SnowAura sa = new GameObject("Snow").AddComponent<SnowAura>();
                        if (!EnemyManager.allboltEntities.ContainsKey(packed))
                        {
                            EnemyManager.GetAllEntities();
                        }

                        sa.followTarget = EnemyManager.allboltEntities[packed].transform;
                        break;
                    default:
                        break;
                }
            }
            else if (s.StartsWith("PO"))    //poison Player
            {
                i = 2;
                ch = s.ToCharArray();
                int playerID = int.Parse(Read());
                if (ModReferences.ThisPlayerID == playerID)
                {
                    int source = int.Parse(Read());
                    float amount = float.Parse(Read());
                    float duration = float.Parse(Read());

                    BuffDB.AddBuff(3, source, amount, duration);
                }


            }
            else if (s.StartsWith("KX"))
            {
                i = 2;
                ch = s.ToCharArray();
                ModdedPlayer.instance.AddKillExperience(long.Parse(Read()));
            }
            else if (s.StartsWith("EA"))       //host answered info about a enemy
            {
                if (GameSetup.IsMpClient)
                {
                    i = 2;
                    ch = s.ToCharArray();
                    ulong packed = ulong.Parse(Read());
                    if (!EnemyManager.allboltEntities.ContainsKey(packed))
                    {
                        EnemyManager.GetAllEntities();
                    }
                    if (EnemyManager.allboltEntities.ContainsKey(packed))
                    {
                        BoltEntity entity = EnemyManager.allboltEntities[packed];
                        string name = Read();
                        int v1 = int.Parse(Read());
                        float v2 = float.Parse(Read());
                        float v3 = float.Parse(Read());
                        int v4 = int.Parse(Read());
                        int v5 = int.Parse(Read());
                        int v6 = int.Parse(Read());
                        float v7 = float.Parse(Read());
                        int lenght = int.Parse(Read());
                        int[] affixes = new int[lenght];
                        int id = 0;
                        while (i < ch.Length)
                        {
                            affixes[id] = int.Parse(Read());
                            id++;
                        }
                        if (EnemyManager.clinetProgressions.ContainsKey(entity))
                        {
                            ClinetEnemyProgression cp = EnemyManager.clinetProgressions[entity];
                            cp.creationTime = Time.time;
                            cp.Entity = entity;
                            cp.Level = v1;
                            cp.Health = v2;
                            cp.MaxHealth = v3;
                            cp.Armor = v5;
                            cp.ArmorReduction = v6;
                            cp.EnemyName = name;
                            cp.ExpBounty = v4;
                            cp.SteadFest = v7;
                            cp.Affixes = affixes;
                        }
                        else
                        {
                            new ClinetEnemyProgression(entity, name, v1, v2, v3, v4, v5, v6, v7, affixes);
                        }
                    }
                }
            }
            else if (s.StartsWith("RO"))    //answer for the what is the difficulty query
            {
                if (!ModdedPlayer.instance.StunImmune)
                {
                    i = 2;
                    ch = s.ToCharArray();
                    Vector3 pos = new Vector3(float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                    if ((LocalPlayer.Transform.position - pos).sqrMagnitude < 1200)
                    {
                        float duration = float.Parse(Read());
                        NetworkManager.SendLine("RE" + LocalPlayer.Transform.position.x + ";" + LocalPlayer.Transform.position.y + ";" + LocalPlayer.Transform.position.z + ";" + duration + ";", NetworkManager.Target.Everyone);
                        ModdedPlayer.instance.Stun(duration);
                    }
                }
            }
            else if (s.StartsWith("RE"))    //answer for the what is the difficulty query
            {
                i = 2;
                ch = s.ToCharArray();
                Vector3 pos = new Vector3(float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                float duration = float.Parse(Read());
                RootSpell.Create(pos, duration);
            }
            else if (s.StartsWith("TR"))    //answer for the what is the difficulty query
            {
                i = 2;
                ch = s.ToCharArray();
                Vector3 pos = new Vector3(float.Parse(Read()), float.Parse(Read()), float.Parse(Read()));
                float duration = float.Parse(Read());
                float radius = float.Parse(Read());
                TrapSphereSpell.Create(pos, radius, duration);

            }
        }
        private static string Read()
        {
            parseval = string.Empty;
            while (ch[i] != ';' && i < ch.Length)
            {
                parseval += ch[i];
                i++;
            }
            i++;
            return parseval;
        }
        private static bool ReadBool()
        {
            parseval = string.Empty;
            while (ch[i] != ';' && i < ch.Length)
            {
                parseval += ch[i];
                i++;
            }
            i++;
            if (parseval[0] == '1' || parseval[0] == 't')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
