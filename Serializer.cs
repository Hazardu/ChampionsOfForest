using ChampionsOfForest.Player;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TheForest.Save;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public class Serializer : MonoBehaviour
    {
        private static bool Saving = false;
        private static Serializer Instance
        {
            get;
            set;
        }
        private static void CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new GameObject().AddComponent<Serializer>();
            }
        }

        private void DoLoad(string path, out float HealthPercentage, out Dictionary<int, int> ExtraCarriedItems)
        {
            ExtraCarriedItems = new Dictionary<int, int>();
            HealthPercentage = 1;
            try
            {


                byte[] bytes = File.ReadAllBytes(path);
                BinaryReader buf = new BinaryReader(new MemoryStream(bytes));

                //reading in the same order to saving
                string version = buf.ReadString();
                if (ModSettings.RequiresNewSave)
                {
                    if (Res.ResourceLoader.CompareVersion(version) == Res.ResourceLoader.Status.Outdated)
                    {

                        return;
                    }
                }
                ModdedPlayer.instance.ExpCurrent = buf.ReadInt64();                 //buf.Write(ModdedPlayer.instance.ExpCurrent);
                HealthPercentage = buf.ReadSingle();                          //buf.Write(LocalPlayer.Stats.Health / ModdedPlayer.instance.MaxHealth);
                ModdedPlayer.instance.MutationPoints = buf.ReadInt32();             //buf.Write(ModdedPlayer.instance.MutationPoints);
                ModdedPlayer.instance.Level = buf.ReadInt32();                      //buf.Write(ModdedPlayer.instance.Level);
                ModdedPlayer.instance.PermanentBonusPerkPoints = buf.ReadInt32();   //buf.Write(ModdedPlayer.instance.PermanentBonusPerkPoints);
                ModdedPlayer.instance.LastDayOfGeneration = buf.ReadInt32();        //buf.Write(ModdedPlayer.instance.LastDayOfGeneration);
                ModdedPlayer.instance.ExpGoal = ModdedPlayer.instance.GetGoalExp();
                //extra carried item amounts
                //key - ID of an item
                //value - amount at the moment of saving
                int ExtraItemCount = buf.ReadInt32();                               //buf.Write(ModdedPlayer.instance.ExtraCarryingCapactity.Count);
                for (int i = 0; i < ExtraItemCount; i++)
                {
                    int ID = buf.ReadInt32();
                    int AMOUNT = buf.ReadInt32();
                    ExtraCarriedItems.Add(ID, AMOUNT);
                }
                //loading inventory
                int ItemSlotCount = buf.ReadInt32();
                for (int i = 0; i < ItemSlotCount; i++)
                {
                    int Slot = buf.ReadInt32();
                    int ID = buf.ReadInt32();

                    if (ID != -1)
                    {
                        int LVL = buf.ReadInt32();
                        int AMO = buf.ReadInt32();
                        int StatCount = buf.ReadInt32();

                        Item LoadedItem = new Item(ItemDataBase.ItemBases[ID], AMO, 0, false)
                        {
                            level = LVL
                        };



                        for (int a = 0; a < StatCount; a++)
                        {
                            int statID = buf.ReadInt32();
                            float statAMO = buf.ReadSingle();

                            ItemStat stat = new ItemStat(ItemDataBase.Stats[statID])
                            {
                                Amount = statAMO
                            };

                            LoadedItem.Stats.Add(stat);
                        }
                        Player.Inventory.Instance.ItemList[Slot] = LoadedItem;
                    }
                    else
                    {
                        Player.Inventory.Instance.ItemList[Slot] = null;
                    }
                }

                //loading perks
                int perkCount = buf.ReadInt32();
                for (int i = 0; i < perkCount; i++)
                {
                    int ID = buf.ReadInt32();
                    bool isBought = buf.ReadBoolean();
                    int ApplyCount = buf.ReadInt32();
                    Perk.AllPerks[ID].IsBought = isBought;
                    if (isBought)
                    {
                        Perk.AllPerks[ID].ApplyAmount = ApplyCount;
                        if (ApplyCount == 0)
                        {
                            Perk.AllPerks[ID].Applied = true;
                            Perk.AllPerks[ID].ApplyMethods();
                        }
                        else
                        {
                            for (int a = 0; a < ApplyCount; a++)
                            {
                                Perk.AllPerks[ID].ApplyMethods();

                            }
                            Perk.AllPerks[ID].Applied = true;

                        }
                    }
                }

                //loading bought spells
                int spellCount = buf.ReadInt32();
                for (int i = 0; i < spellCount; i++)
                {
                    int ID = buf.ReadInt32();
                    bool isBought = buf.ReadBoolean();
                    SpellDataBase.spellDictionary[ID].Bought = isBought;
                }

                //loading spell loadout
                int spellLoadoutCount = buf.ReadInt32();
                for (int i = 0; i < spellLoadoutCount; i++)
                {
                    int ID = buf.ReadInt32();
                    if (ID == -1)
                    {
                        SpellCaster.instance.infos[i].spell = null;
                    }
                    else
                    {
                        SpellCaster.instance.infos[i].spell = SpellDataBase.spellDictionary[ID];
                    }
                }
            }
            catch (System.Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }



        private IEnumerator DoSaveCoroutine()
        {
            Saving = true;
            while (Path() == string.Empty)
            {
                yield return null;

            }
            yield return new WaitForSeconds(1);



            MemoryStream stream = new MemoryStream();
            BinaryWriter buf = new BinaryWriter(stream);

            //saving modded player variables
            buf.Write(ModSettings.Version);
            buf.Write(ModdedPlayer.instance.ExpCurrent);
            buf.Write(LocalPlayer.Stats.Health / ModdedPlayer.instance.MaxHealth);
            buf.Write(ModdedPlayer.instance.MutationPoints);
            buf.Write(ModdedPlayer.instance.Level);
            buf.Write(ModdedPlayer.instance.PermanentBonusPerkPoints);
            buf.Write(ModdedPlayer.instance.LastDayOfGeneration);

            //saving extra item amounts to not loose any extra
            //im doing that because the game trims the excess on load
            buf.Write(ModdedPlayer.instance.ExtraCarryingCapactity.Count);
            foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> item in ModdedPlayer.instance.ExtraCarryingCapactity)
            {
                buf.Write(item.Value.ID);
                buf.Write(LocalPlayer.Inventory.AmountOf(item.Value.ID));
            }

            //saving inventory
            buf.Write(Player.Inventory.Instance.ItemList.Count);

            foreach (KeyValuePair<int, Item> item in Player.Inventory.Instance.ItemList)
            {
                buf.Write(item.Key);

                if (item.Value != null)
                {
                    //save the slot id

                    //save individual item
                    buf.Write(item.Value.ID);
                    buf.Write(item.Value.level);
                    buf.Write(item.Value.Amount);
                    buf.Write(item.Value.Stats.Count);
                    //save every stat
                    for (int i = 0; i < item.Value.Stats.Count; i++)
                    {
                        buf.Write(item.Value.Stats[i].StatID);
                        buf.Write(item.Value.Stats[i].Amount);
                    }
                }
                else
                {
                    buf.Write(-1);

                }
            }

            //saving perks
            buf.Write(Perk.AllPerks.Count);
            foreach (Perk item in Perk.AllPerks)
            {
                buf.Write(item.ID);
                buf.Write(item.IsBought);
                buf.Write(item.ApplyAmount);
            }

            //saving bought spells
            buf.Write(SpellDataBase.spellDictionary.Count);
            foreach (KeyValuePair<int, Spell> pair in SpellDataBase.spellDictionary)
            {
                buf.Write(pair.Value.ID);
                buf.Write(pair.Value.Bought);

            }

            //saving spell loadout
            buf.Write(SpellCaster.instance.infos.Length);
            foreach (SpellCaster.SpellInfo info in SpellCaster.instance.infos)
            {
                if (info.spell == null)
                {
                    buf.Write(-1);
                }
                else
                {
                    buf.Write(info.spell.ID);

                }
            }

            //finally
            //saving to a file

            //checking if dir exists
            if (!Directory.Exists(Path()))
            {
                Directory.CreateDirectory(Path());
            }


            string path = Path() + "/COTF.save";
            File.WriteAllBytes(path, stream.ToArray());
            Saving = false;
        }

        private IEnumerator DoLoadCoroutine()
        {
            while (LocalPlayer.Stats == null || LocalPlayer.Inventory == null)
            {
                yield return null;
            }

            yield return null;
            yield return null;
            yield return null;
            yield return null;

            //waits some frames

            string path = Path();
            if (path == string.Empty)
            {
                yield break;
            }
            //checking and cancelling loading routine if there is no directory of the save
            if (!Directory.Exists(path))
            {
                yield break;
            }


            path += "/COTF.save";
            if (!File.Exists(path))
            {
                yield break;
            }


            DoLoad(path, out float HealthPercentage, out Dictionary<int, int> ExtraCarriedItems);



            //waiting for buffs and perks to apply 
            yield return new WaitForSeconds(0.5f);

            //bringing health back to correct amount
            LocalPlayer.Stats.Health = ModdedPlayer.instance.MaxHealth * HealthPercentage;


            //fixing missing items 
            foreach (KeyValuePair<int, int> item in ExtraCarriedItems)
            {
                int amount = LocalPlayer.Inventory.AmountOf(item.Key);
                if (amount < item.Value)
                {
                    int toAdd = item.Value - amount;
                    LocalPlayer.Inventory.AddItem(item.Key, toAdd);
                }
            }
            SpellCaster.instance.SetMaxCooldowns();
        }






        public static void Save()
        {
            CreateInstance();
            if (!Saving)
            {
                ModAPI.Log.Write("SAVING");
                Instance.StartCoroutine(Instance.DoSaveCoroutine());
            }
        }

        public static void Load()
        {
            CreateInstance();
            ModAPI.Log.Write("LOADING");
            Instance.StartCoroutine(Instance.DoLoadCoroutine());

        }

        public static string Path()
        {

            if (GameSetup.IsMultiplayer)
            {
                if (GameSetup.IsMpClient)
                {
                    return "Mods/Champions of the Forest/Multiplayer/" + PlayerSpawn.GetClientSaveFileName();
                }
                else if (GameSetup.IsMpServer)
                {
                    if (GameSetup.Slot.ToString() == "0")
                    {
                        return string.Empty;
                    }

                    return "Mods/Champions of the Forest/Multiplayer/" + GameSetup.Slot.ToString();
                }
            }
            if (GameSetup.Slot.ToString() == "0")
            {
                return string.Empty;
            }

            return "Mods/Champions of the Forest/Singleplayer/" + GameSetup.Slot.ToString();
        }
    }
    public class SaveOverride : PlayerStats
    {
        public override void OnSaveSlotSelected()
        {
            if (!this.Dead)
            {
                Serializer.Save();
            }
            base.OnSaveSlotSelected();
        }
        public override void JustSave()
        {
            if (!this.Dead && GameSetup.IsMpClient)
            {
                Serializer.Save();
            }
            base.JustSave();
        }
    }
}
