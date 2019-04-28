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
            Item item = new Item(ItemDataBase.ItemBases[id]);
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
                "cotfnocooldowns",
                "cotfspawnitem",
                "cotfaddlevel",
                "cotfsetlevel",
                "cotfaddpoints",
                "cotfresetpoints"
            };

            _availableConsoleMethods.Add("cotfnocooldowns",null);
            _availableConsoleMethods.Add("cotfspawnitem",null);
            _availableConsoleMethods.Add("cotfaddlevel",null);
            _availableConsoleMethods.Add("cotfsetlevel",null);
            _availableConsoleMethods.Add("cotfaddpoints",null);
            _availableConsoleMethods.Add("cotfresetpoints",null);
        }
        public override void HandleConsoleInput(string consoleInput)
        {
            if (this._historyEnd == -1)
            {
                this._historyEnd = 0;
                this._history[0] = consoleInput;
            }
            else if (consoleInput != this._history[this._historyEnd])
            {
                this._historyEnd = (this._historyEnd + 1) % this._history.Length;
                this._history[this._historyEnd] = consoleInput;
            }
            this._historyCurrent = -1;
            List<string> list = consoleInput.Split(new char[]
            {
                ' '
            }).ToList<string>();
            string text = list[0].ToLower();
            if (this._availableConsoleMethods.ContainsKey(text))
            {
                if (_availableConsoleMethods[text] == null) {
                    CotfCheat(list);
                    Debug.Log("Executing COTF cheat command");
                    return;
                }


                list.RemoveAt(0);
                int num = 1;
                if (list.Any((string a) => a.StartsWith("--")))
                {
                    string text2 = list.First((string a) => a.StartsWith("--"));
                    list.Remove(text2);
                    num = int.Parse(text2.Substring(2));
                }
                string text3 = (list.Count <= 0) ? null : string.Join(" ", list.ToArray(), 0, list.Count);
                if (num > 1)
                {
                    Debug.Log(string.Concat(new object[]
                    {
                        "$> Being repeat command '",
                        text,
                        " ",
                        text3,
                        "'",
                        num,
                        " times"
                    }));
                }
                while (num-- > 0)
                {
                    try
                    {
                        this._availableConsoleMethods[text].Invoke((!this._availableConsoleMethods[text].IsStatic) ? this : null, new object[]
                        {
                            text3
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(string.Concat(new object[]
                        {
                            "$> '",
                            text,
                            " ",
                            text3,
                            "' #",
                            num,
                            " exception:\n",
                            ex
                        }));
                    }
                }
            }
            else if (!string.IsNullOrEmpty(list[0]))
            {
                Debug.Log("$> Unknown console command '" + list[0] + "'");
            }
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
