using ChampionsOfForest.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TheForest;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Fun
{
    public static class CotfCheats
    {
        public static void SetLevel(int lvl)
        {
            ModdedPlayer.instance.Level = lvl;
            ModdedPlayer.Respec();
        }
        public static void AddLevel(int lvl)
        {
            for (int i = 0; i < lvl; i++)
            {
                ModdedPlayer.instance.LevelUp();
            }
        }
        public static void AddPoints(int points)
        {
            ModdedPlayer.instance.MutationPoints += points;
        }
        public static void Respec()
        {
            ModdedPlayer.Respec();
        }
        public static void CotfItem(int id,int level)
        {
            Item item = new Item(ItemDataBase.ItemBases[id],1,0,false);
            item.level = level;
            item.RollStats();
            ChampionsOfForest.Player.Inventory.Instance.AddItem(item);
        }

        public static bool Cheat_noCooldowns;
        public static void NoCooldowns(string state)
        {
            if (state == "on")
                Cheat_noCooldowns = true;
            else if (state == "off")
                Cheat_noCooldowns = false;
            else
                Cheat_noCooldowns = !Cheat_noCooldowns;
        }

    }
    public class DebugConsoleMod : DebugConsole
    {
        List<string> CotfCommandsList;
        protected override void Awake()
        {
            base.Awake();
            CotfCommandsList = new List<string>()
            {
                "cotfhelp",
                "cotfnocooldowns",
                "cotfspawnitem",
                "cotfaddlevel",
                "cotfsetlevel",
                "cotfaddpoints",
                "cotfresetpoints"
            };

            _availableConsoleMethods.Add("cotfhelp",null);
            _availableConsoleMethods.Add("cotfnocooldowns",null);
            _availableConsoleMethods.Add("cotfspawnitem",null);
            _availableConsoleMethods.Add("cotfaddlevel",null);
            _availableConsoleMethods.Add("cotfsetlevel",null);
            _availableConsoleMethods.Add("cotfaddpoints",null);
            _availableConsoleMethods.Add("cotfresetpoints",null);
        }
        public override void HandleConsoleInput(string consoleInput)
        {
            List<string> commandParam = consoleInput.Split(new char[]
            {
                ' '
            }).ToList<string>();
            string commandID = commandParam[0].ToLower();
            if (base._availableConsoleMethods.ContainsKey(commandID))
            {
                if (_availableConsoleMethods[commandID] == null)
                {

                    if (commandParam.Any((string a) => a.StartsWith("--")))
                    {
                        int repetitions = 1;
                        string repetitionString = commandParam.First((string a) => a.StartsWith("--"));
                        commandParam.Remove(repetitionString);
                        repetitions = int.Parse(repetitionString.Substring(2));
                        for (int a = 0; a < repetitions; a++)
                        {
                            CotfCheat(commandParam);
                        }
                        return;
                    }
                    else
                    {
                        CotfCheat(commandParam);
                        return;
                    }
                }
            }
                base.HandleConsoleInput(consoleInput);
        }

        private void CotfCheat(List<string> list)
        {
            string text = list[0].ToLower();

            if (!CotfCommandsList.Contains(text)) return;
            try
            {
                switch (text)
                {
                    case "cotfnocooldowns":
                        CotfCheats.NoCooldowns(list[1]);
                        break;
                    case "cotfspawnitem":
                        CotfCheats.CotfItem(int.Parse(list[1]), int.Parse(list[2]));
                        break;
                    case "cotfaddlevel":
                        CotfCheats.AddLevel(int.Parse(list[1]));
                        break;
                    case "cotfsetlevel":
                        CotfCheats.SetLevel(int.Parse(list[1]));
                        break;
                    case "cotfaddpoints":
                        CotfCheats.AddPoints(int.Parse(list[1]));
                        break;
                    case "cotfresetpoints":
                        CotfCheats.Respec();
                        break;
                    case "cotfhelp":
                            Debug.LogWarning("Avaible champions of the forest commands:");
                        Debug.Log("cotfnocooldowns [on, off, no parameter to toggle]");
                        Debug.Log("cotfspawnitem [item id] [level]");
                        Debug.Log("cotfaddlevel [amount]");
                        Debug.Log("cotfsetlevel [amount]");
                        Debug.Log("cotfaddpoints [amount]");
                        Debug.Log("cotfresetpoints");
                        break;
                }
            }
            catch (Exception)
            {
                Debug.LogWarning("Incorrect COTF command usage");
                throw;
            }
        }
    }
}
