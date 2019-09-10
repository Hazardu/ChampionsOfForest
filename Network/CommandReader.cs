using ChampionsOfForest.Effects;
using ChampionsOfForest.Enemies;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Player;
using System;
using System.Collections.Generic;
using System.IO;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Network
{
    public class CommandReader
    { 
        public static void OnCommand(byte[] bytes)
        {

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    int cmdIndex = r.ReadInt32();

                    if (cmdIndex == 1)  //previousely AB
                    {
                        if (GameSetup.IsMpServer && ModSettings.DifficultyChoosen)
                        {
                            using (MemoryStream answerStream = new MemoryStream())
                            {
                                using (BinaryWriter w = new BinaryWriter(answerStream))
                                {
                                    w.Write(2);
                                    w.Write((int)ModSettings.difficulty);
                                    w.Write(ModSettings.FriendlyFire);
                                    w.Write((int)ModSettings.dropsOnDeath);
                                w.Close();
                                }
                                Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
                                answerStream.Close();
                            }
                        }
                    }
                    else if (cmdIndex == 2) //request for the what is the difficulty
                    {
                        if (!GameSetup.IsMpClient || ModSettings.IsDedicated) return;

                        int index = r.ReadInt32();
                        ModSettings.FriendlyFire = r.ReadBoolean();
                        ModSettings.dropsOnDeath = (ModSettings.DropsOnDeathMode)r.ReadInt32();
                        Array values = Enum.GetValues(typeof(ModSettings.Difficulty));
                        ModSettings.difficulty = (ModSettings.Difficulty)values.GetValue(index);
                        if (!ModSettings.DifficultyChoosen)
                        {
                            LocalPlayer.FpCharacter.UnLockView();
                            LocalPlayer.FpCharacter.MovementLocked = false;
                        }
                        ModSettings.DifficultyChoosen = true;
                    }
                    else if (cmdIndex == 3) //spell casted
                    {
                        int spellid = r.ReadInt32();
                        if (spellid == 1)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            BlackHole.CreateBlackHole(pos, r.ReadBoolean(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        }
                        else if (spellid == 2)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            HealingDome.CreateHealingDome(pos, r.ReadSingle(), r.ReadSingle(), r.ReadBoolean(),r.ReadBoolean(), r.ReadSingle());
                        }
                        else if (spellid == 3)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            DarkBeam.Create(pos, r.ReadBoolean(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        }
                        else if (spellid == 4)
                        {
                            bool isOn = r.ReadBoolean();
                            string packed = r.ReadString();
                            BlackFlame.ToggleOtherPlayer(packed, isOn);

                        }
                        else if (spellid == 5)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            float radius = r.ReadSingle();
                            float speed = r.ReadSingle();
                            float dmg = r.ReadSingle();
                            bool GiveDmg = r.ReadBoolean();
                            bool GiveAr = r.ReadBoolean();
                            int ar = 0;
                            if (GiveAr)
                            {
                                ar = r.ReadInt32();
                            }

                            WarCry.Cast(pos, radius,speed,dmg, GiveDmg, GiveAr, ar);

                        }
                        else if (spellid == 6)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            float duration = r.ReadSingle();
                            int id = r.ReadInt32();

                            Portal.CreatePortal(pos, duration, id, r.ReadBoolean(), r.ReadBoolean());
                        }
                        else if (spellid == 7)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            Vector3 dir = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());

                            float dmg = r.ReadSingle();
                            string caster = r.ReadString();
                            float duration = r.ReadSingle();
                            bool slow = r.ReadBoolean();
                            bool dmgdebuff = r.ReadBoolean();
                            if (GameSetup.IsMpServer)
                            {
                                MagicArrow.Create(pos, dir, dmg, caster, duration, slow, dmgdebuff);
                            }
                            else
                            {
                                MagicArrow.CreateEffect(pos, dir, dmgdebuff, duration);
                            }

                        }
                        else if (spellid == 8)
                        {
                            Purge.Cast(new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle()), r.ReadSingle(), r.ReadBoolean());

                        }
                        else if (spellid == 9)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            float dist = r.ReadSingle();

                            SnapFreeze.CreateEffect(pos, dist);
                            if (!GameSetup.IsMpClient)
                            {
                                SnapFreeze.HostAction(pos, dist, r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            }

                        }
                        else if (spellid == 10)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            Vector3 speed = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            float dmg = r.ReadSingle();
                            uint id = r.ReadUInt32();

                            if (BallLightning.lastID < id)
                            {
                                BallLightning.lastID = id;
                            }

                            BallLightning.Create(pos, speed, dmg, id);

                        }
                        else if (spellid == 11)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            float radius = r.ReadSingle();
                            float dmg = r.ReadSingle();
                            float duration = r.ReadSingle();
                            bool isArcane = r.ReadBoolean();
                            bool fromEnemy = r.ReadBoolean();
                            Cataclysm.Create(pos, radius, dmg, duration, isArcane ? Cataclysm.TornadoType.Arcane : Cataclysm.TornadoType.Fire, fromEnemy);
                        }
                        else if (spellid == 12)
                        {
                            //a request from a client to a host to spawn a ball lightning. The host assigns the id of 
                            //a ball lightning to not create overlapping ids
                            using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                            {
                                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                                {
                                    w.Write(3);
                                    w.Write(10);
                                    w.Write(r.ReadSingle());
                                    w.Write(r.ReadSingle());
                                    w.Write(r.ReadSingle());
                                    w.Write(r.ReadSingle());
                                    w.Write(r.ReadSingle());
                                    w.Write(r.ReadSingle());
                                    w.Write(r.ReadSingle());
                                    w.Write((uint)(BallLightning.lastID + 1));
                                    w.Close();
                                    BallLightning.lastID++;
                                }
                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                                answerStream.Close();
                            }
                        }
                        else if (spellid == 13) //parry was casted by a client
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            float radius = r.ReadSingle();
                            bool ignite = r.ReadBoolean();
                            float dmg = r.ReadSingle();

                            DamageMath.DamageClamp(dmg, out int d, out int rep);
                            var hits = Physics.SphereCastAll(pos, radius, Vector3.one);

                            for (int i = 0; i < hits.Length; i++)
                            {
                                if (hits[i].transform.CompareTag("enemyCollide"))
                                {
                                    for (int a = 0; a < rep; a++)
                                    {
                                        hits[i].transform.SendMessageUpwards("Hit", d, SendMessageOptions.DontRequireReceiver);
                                        if (ignite)
                                            hits[i].transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
                                    }
                                }
                            }
                        }
                    
                    }
                    else if (cmdIndex == 4) //remove item 
                    {
                        PickUpManager.RemovePickup(r.ReadUInt64());

                    }
                    else if (cmdIndex == 5) //create item
                    {
                        Item item = new Item(ItemDataBase.ItemBases[r.ReadInt32()], 1, 0, false);   //reading first value, id
                        ulong id = r.ReadUInt64();
                        item.level = r.ReadInt32();
                        int amount = r.ReadInt32();
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        while (r.BaseStream.Position != r.BaseStream.Length)
                        {
                            ItemStat stat = new ItemStat(ItemDataBase.Stats[r.ReadInt32()])
                            {
                                Amount = r.ReadSingle()
                            };
                            item.Stats.Add(stat);
                        }
                        PickUpManager.SpawnPickUp(item, pos, amount, id);
                    }
                    else if (cmdIndex == 6) //host has been asked to share info on enemy
                    {
                        if (!GameSetup.IsMpClient)
                        {
                            ulong packed = r.ReadUInt64();
                            if (EnemyManager.hostDictionary.ContainsKey(packed))
                            {
                                EnemyProgression ep = EnemyManager.hostDictionary[packed];
                                using (MemoryStream answerStream = new MemoryStream())
                                {
                                    using (BinaryWriter w = new BinaryWriter(answerStream))
                                    {
                                        w.Write(7);
                                        w.Write(packed);
                                        w.Write(ep.EnemyName);
                                        w.Write(ep.Level);
                                        w.Write(ep._hp + ep._Health.Health);
                                        w.Write(ep.MaxHealth);
                                        w.Write(ep.Bounty);
                                        w.Write(ep.Armor);
                                        w.Write(ep.ArmorReduction);
                                        w.Write(ep.Steadfast);
                                        w.Write(ep.abilities.Count);
                                        foreach (EnemyProgression.Abilities item in ep.abilities)
                                        {
                                            w.Write((int)item);
                                        }

                                    w.Close();
                                    }
                                    Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
                                    answerStream.Close();
                                }
                            }
                            else
                            {
                                CotfUtils.Log("no enemy in host's dictionary");
                            }
                        }
                    }
                    else if (cmdIndex == 7) //host answered info about a enemy and the info is processed
                    {
                        if (ModSettings.IsDedicated)
                        {
                            return;
                        }

                        if (GameSetup.IsMpClient)
                        {
                            ulong packed = r.ReadUInt64();
                            if (!EnemyManager.allboltEntities.ContainsKey(packed))
                            {
                                EnemyManager.GetAllEntities();
                            }
                            if (EnemyManager.allboltEntities.ContainsKey(packed))
                            {
                                BoltEntity entity = EnemyManager.allboltEntities[packed];
                                string name = r.ReadString();
                                int level = r.ReadInt32();
                                float health = r.ReadSingle();
                                float maxhealth = r.ReadSingle();
                                long bounty = r.ReadInt64();
                                int armor = r.ReadInt32();
                                int armorReduction = r.ReadInt32();
                                float steadfast = r.ReadSingle();
                                int length = r.ReadInt32();
                                int[] affixes = new int[length];
                                for (int i = 0; i < length; i++)
                                {
                                    affixes[i] = r.ReadInt32();
                                }
                                if (EnemyManager.clinetProgressions.ContainsKey(entity))
                                {
                                    ClinetEnemyProgression cp = EnemyManager.clinetProgressions[entity];
                                    cp.creationTime = Time.time;
                                    cp.Entity = entity;
                                    cp.Level = level;
                                    cp.Health = health;
                                    cp.MaxHealth = maxhealth;
                                    cp.Armor = armor;
                                    cp.ArmorReduction = armorReduction;
                                    cp.EnemyName = name;
                                    cp.ExpBounty = bounty;
                                    cp.Steadfast = steadfast;
                                    cp.Affixes = affixes;
                                }
                                else
                                {
                                    new ClinetEnemyProgression(entity, name, level, health, maxhealth, bounty, armor, armorReduction, steadfast, affixes);
                                }
                            }
                        }
                    }
                    else if (cmdIndex == 8) //enemy spell casted
                    {
                        int id = r.ReadInt32();
                        if (id == 1) //snow aura
                        {
                            ulong packed = r.ReadUInt64();
                            SnowAura sa = new GameObject("Snow").AddComponent<SnowAura>();
                            if (!EnemyManager.allboltEntities.ContainsKey(packed))
                            {
                                EnemyManager.GetAllEntities();
                            }
                            sa.followTarget = EnemyManager.allboltEntities[packed].transform;
                        }
                        else if (id == 2) //fire aura
                        {
                            ulong packed = r.ReadUInt64();
                            float dmg = r.ReadSingle();
                            GameObject go = EnemyManager.allboltEntities[packed].gameObject;
                            FireAura.Cast(go, dmg);
                        }
                    }
                    else if (cmdIndex == 9)  //poison Player
                    {
                        string playerID = r.ReadString();
                        if (ModReferences.ThisPlayerID == playerID)
                        {
                            int source = r.ReadInt32();
                            float amount = r.ReadSingle();
                            float duration = r.ReadSingle();

                            BuffDB.AddBuff(3, source, amount, duration);
                        }
                    }
                    else if (cmdIndex == 10) //kill experience
                    {
                        ModdedPlayer.instance.AddKillExperience(r.ReadInt64());
                    }
                    else if (cmdIndex == 11) //add experience without massacre
                    {
                        ModdedPlayer.instance.AddFinalExperience(r.ReadInt64());
                    }
                    else if (cmdIndex == 12)  //root the player
                    {
                        if (ModdedPlayer.instance.RootImmune == 0 && ModdedPlayer.instance.StunImmune == 0)
                        {
                            Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                            if ((LocalPlayer.Transform.position - pos).sqrMagnitude < 1250)
                            {
                                float duration = r.ReadSingle();
                                ModdedPlayer.instance.Root(duration);
                                using (MemoryStream answerStream = new MemoryStream())
                                {
                                    using (BinaryWriter w = new BinaryWriter(answerStream))
                                    {
                                        w.Write(14);
                                        w.Write(LocalPlayer.Transform.position.x);
                                        w.Write(LocalPlayer.Transform.position.y);
                                        w.Write(LocalPlayer.Transform.position.z);
                                        w.Write(duration);
                                w.Close();
                                    }
                                    NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
                                answerStream.Close();
                                }
                            }
                        }
                    }
                    else if (cmdIndex == 13)  //stun the player
                    {
                        if (ModSettings.IsDedicated) return;
                        if (ModdedPlayer.instance.StunImmune == 0)
                        {
                            string playerID = r.ReadString();
                            if (ModReferences.ThisPlayerID == playerID)
                            {
                                float duration = r.ReadSingle();
                                ModdedPlayer.instance.Stun(duration);
                            }
                        }
                    }
                    else if (cmdIndex == 14) //player has been chained, now spawn effect
                    {
                        if (ModSettings.IsDedicated) return;

                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        float duration = r.ReadSingle();
                        RootSpell.Create(pos, duration);
                    }
                    else if (cmdIndex == 15)    //create trap sphere
                    {
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        float duration = r.ReadSingle();
                        float radius = r.ReadSingle();
                        TrapSphereSpell.Create(pos, radius, duration);
                    }
                    else if (cmdIndex == 16)    //create enemy laser, aka plasma cannon
                    {
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        Vector3 dir = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());

                        EnemyLaser.CreateLaser(pos, dir);
                    }
                    else if (cmdIndex == 17) //create enemy meteor rain
                    {
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        Meteor.CreateEnemy(pos, r.ReadInt32());
                    }
                    else if (cmdIndex == 18) //player's level info, or command to wipe level data
                    {
                        if (r.BaseStream.Position == r.BaseStream.Length) ModReferences.PlayerLevels.Clear();
                        using (MemoryStream answerStream = new MemoryStream())
                        {
                            using (BinaryWriter w = new BinaryWriter(answerStream))
                            {
                                w.Write(19);
                                w.Write(ModReferences.ThisPlayerID);
                                w.Write(ModdedPlayer.instance.Level);
                            w.Close();
                            }
                            Network.NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
                            answerStream.Close();
                        }
                    }
                    else if (cmdIndex == 19)//add or update some players level to list
                    {
                        string packed = r.ReadString();
                        int level = r.ReadInt32();
                        if (ModReferences.PlayerLevels.ContainsKey(packed))
                        {
                            ModReferences.PlayerLevels[packed] = level;
                        }
                        else
                        {
                            ModReferences.PlayerLevels.Add(packed, level);
                        }
                    }
                    else if (cmdIndex == 20) //enemy hitmarker
                    {
                        if (ModSettings.IsDedicated) return;

                        int amount = r.ReadInt32();
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        new MainMenu.HitMarker(amount, pos);
                    }
                    else if (cmdIndex == 21)  //player hitmarker
                    {
                        if (ModSettings.IsDedicated) return;

                        int amount = r.ReadInt32();
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        new MainMenu.HitMarker(amount, pos, true);
                    }
                    else if (cmdIndex == 22)   //slow Enemy
                    {
                        if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
                        {
                            ulong id = r.ReadUInt64();
                            if (EnemyManager.hostDictionary.ContainsKey(id))
                            {
                                float amount = r.ReadSingle();
                                float time = r.ReadSingle();
                                int src = r.ReadInt32();
                                EnemyManager.hostDictionary[id].Slow(src, amount, time);
                            }
                        }
                    }
                    else if (cmdIndex == 23)  //sync magic find
                    {
                        if (GameSetup.IsMpServer)
                        {
                            if (ModSettings.IsDedicated)
                            {
                                ItemDataBase.MagicFind = 1;
                            }
                            else
                            {
                                ItemDataBase.MagicFind = ModdedPlayer.instance.MagicFindMultipier;
                            }
                        }
                        else
                        {
                            using (MemoryStream answerStream = new MemoryStream())
                            {
                                using (BinaryWriter w = new BinaryWriter(answerStream))
                                {
                                    w.Write(24);
                                    w.Write(ModdedPlayer.instance.MagicFindMultipier);
                            w.Close();
                                }
                                Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
                            answerStream.Close();
                            }
                        }
                    }
                    else if (cmdIndex == 24) //update magic find for host
                    {
                        if (GameSetup.IsMpServer)
                            ItemDataBase.MagicFind *= r.ReadSingle();
                    }
                    else if (cmdIndex == 25) //ask for item
                    {
                        if (GameSetup.IsMpServer)
                        {
                            ulong itemID = r.ReadUInt64();
                            if (PickUpManager.PickUps.ContainsKey(itemID))
                            {
                                int itemAmount = r.ReadInt32();
                                string playerID = r.ReadString();

                                if (PickUpManager.PickUps[itemID].amount > 0)
                                {
                                    int givenAmount = itemAmount;
                                    if (itemAmount > PickUpManager.PickUps[itemID].amount)
                                    {
                                        givenAmount = Mathf.Min(PickUpManager.PickUps[itemID].amount, itemAmount);
                                    }

                                    NetworkManager.SendItemToPlayer(PickUpManager.PickUps[itemID].item, playerID, givenAmount);

                                    PickUpManager.PickUps[itemID].amount -= givenAmount;

                                    if (PickUpManager.PickUps[itemID].amount > 0)
                                    {
                                        return;
                                    }
                                }
                            }
                            using (MemoryStream answerStream = new MemoryStream())
                            {
                                using (BinaryWriter w = new BinaryWriter(answerStream))
                                {
                                    w.Write(4);
                                    w.Write(itemID);
                                w.Close();
                                }
                                Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
                                answerStream.Close();
                            }
                        }
                    }
                    else if (cmdIndex == 26) //give item to player
                    {
                        string playerID = r.ReadString();
                        if (ModReferences.ThisPlayerID == playerID)
                        {
                            //creating the item.
                            Item item = new Item(ItemDataBase.ItemBases[r.ReadInt32()], r.ReadInt32(), 0, false)
                            {
                                level = r.ReadInt32()
                            };

                            //adding stats to the item
                            while (r.BaseStream.Position != r.BaseStream.Length)
                            {
                                ItemStat stat = new ItemStat(ItemDataBase.Stats[r.ReadInt32()])
                                {
                                    Amount = r.ReadSingle()
                                };
                                item.Stats.Add(stat);
                            }

                            Player.Inventory.Instance.AddItem(item, item.Amount);

                        }
                    }
                    else if (cmdIndex == 27) // bonus fire damage
                    {
                        if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
                        {

                            ulong id = r.ReadUInt64();
                            if (EnemyManager.hostDictionary.ContainsKey(id))
                            {
                                float amount = r.ReadSingle();
                                float time = r.ReadSingle();
                                int src = r.ReadInt32();
                                EnemyManager.hostDictionary[id].FireDebuff(src, amount, time);
                            }
                        }
                    }
                    else if (cmdIndex == 28) //custom weapon in mp
                    {
                        string id = r.ReadString();
                        int weaponID = r.ReadInt32();
                        if (!ModReferences.PlayerHands.ContainsKey(id))
                        {

                            ModReferences.FindHands();
                        }

                        if (ModReferences.PlayerHands.ContainsKey(id))
                        {
                            CoopCustomWeapons.SetWeaponOn(ModReferences.PlayerHands[id], weaponID);
                        }
                        else
                        {
                            Debug.LogWarning("NO HAND IN COMMAND READER");
                        }
                    }
                    else if (cmdIndex == 29) //request for enemy damage information 
                    {
                        if (GameSetup.IsMpServer)
                        {
                            ulong id = r.ReadUInt64();
                            if (EnemyManager.hostDictionary.ContainsKey(id))
                            {
                                EnemyProgression p = EnemyManager.hostDictionary[id];
                                using (MemoryStream answerStream = new MemoryStream())
                                {
                                    using (BinaryWriter w = new BinaryWriter(answerStream))
                                    {
                                        w.Write(30);
                                        w.Write(id);
                                        w.Write(p.BaseDamageMult);
                                        foreach (EnemyProgression.Abilities ability in p.abilities)
                                        {
                                            w.Write((int)ability);
                                        }
                                    w.Close();
                                    }
                                    NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Clients);
                                    answerStream.Close();
                                }
                            }
                        }
                    }
                    else if (cmdIndex == 30) //answer to client damage
                    {
                        ulong id = r.ReadUInt64();
                        float dmg = r.ReadSingle();
                        List<EnemyProgression.Abilities> abilities = new List<EnemyProgression.Abilities>();
                        while (r.BaseStream.Position != r.BaseStream.Length)
                        {
                            abilities.Add((EnemyProgression.Abilities)r.ReadInt32());
                        }
                        new ClientEnemy(id, dmg, abilities);
                    }
                    else if (cmdIndex == 31) //detonate ball lightning
                    {
                        uint id = r.ReadUInt32();
                        Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
                        if (BallLightning.list.ContainsKey(id))
                        {
                            BallLightning.list[id].CoopTrigger(pos);
                        }
                    }
                    else if (cmdIndex == 32) //apply DoT to an enemy
                    {
                        ulong id = r.ReadUInt64();
                        if (EnemyManager.hostDictionary.ContainsKey(id))
                        {
                            EnemyProgression p = EnemyManager.hostDictionary[id];
                            p.DoDoT(r.ReadInt32(), r.ReadSingle());
                        }
                    }
                    else if (cmdIndex == 33) //enemy got bashed
                    {
                        ulong enemy = r.ReadUInt64();
                        if (EnemyManager.hostDictionary.ContainsKey(enemy))
                        {
                            EnemyProgression p = EnemyManager.hostDictionary[enemy];
                            float duration = r.ReadSingle();
                            var source = r.ReadInt32();
                            float slowAmount = r.ReadSingle();
                            float dmgDebuff = r.ReadSingle();
                            var bleedDmg = r.ReadInt32();
                            float bleedChance = r.ReadSingle();
                            p.Slow(source, slowAmount, duration);
                            p.DmgTakenDebuff(source, dmgDebuff, duration);
                            if (UnityEngine.Random.value < bleedChance)
                            {
                                p.DoDoT(bleedDmg, duration);
                            }
                        }
                    }
                    else if (cmdIndex == 34)
                    {
                        if (GameSetup.IsMpServer)
                        {
                            ulong enemy = r.ReadUInt64();
                            if (EnemyManager.hostDictionary.ContainsKey(enemy))
                            {
                                EnemyProgression p = EnemyManager.hostDictionary[enemy];
                                var source = r.ReadInt32();
                                float amount = r.ReadSingle();
                                float duration = r.ReadSingle();
                                p.DmgTakenDebuff(source, amount, duration);
                            }
                        }
                    }
                r.Close();
                }
                stream.Close();
            }
        }
    }
}
    
