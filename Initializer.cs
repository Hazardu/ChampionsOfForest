using ChampionsOfForest.Enemies;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Player;
using ModAPI.Attributes;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChampionsOfForest
{
    public class Initializer
    {


        [ExecuteOnGameStart]
        public static void Initialize()
        {
            try
            {
                if (SteamDSConfig.isDedicated)
                {
                    ModAPI.Log.Write("isDedicated true");
                    ModSettings.IsDedicated = true;
                }

                if (ModSettings.IsDedicated)
                {

                    ReadDediServerConfig();




                    new GameObject("NetworkManagerObj").AddComponent<Network.NetworkManager>();
                    GameObject go = new GameObject("Playerobj");
                    //go.AddComponent<ModdedPlayer>();
                    //go.AddComponent<Inventory>();
                    go.AddComponent<ModReferences>();
                    //go.AddComponent<SpellCaster>();
                    //go.AddComponent<ClinetItemPicker>();
                    //go.AddComponent<MeteorSpawner>();
                    //BuffDB.FillBuffList();
                    ItemDataBase.Initialize();
                    //SpellDataBase.Initialize();
                    EnemyManager.Initialize();
                    //new GameObject("MainMenuObj").AddComponent<MainMenu>();
                    Network.NetworkManager.instance.onGetMessage += Network.CommandReader.OnCommand;
                    //Res.Buildings.InitBuildings();

                    return;
                }

                ModSettings.DifficultyChoosen = false;
                if (SceneManager.GetActiveScene().name == "TitleScene")
                {
                    new GameObject("Resource Manager").AddComponent<Res.ResourceLoader>();
                    Res.ResourceLoader.InMainMenu = true;
                }
                else
                {
                    Res.ResourceLoader.InMainMenu = false;

                    new GameObject("NetworkManagerObj").AddComponent<Network.NetworkManager>();
                    GameObject go = new GameObject("Playerobj");
                    go.AddComponent<ModdedPlayer>();
                    go.AddComponent<Inventory>();
                    go.AddComponent<ModReferences>();
                    go.AddComponent<SpellCaster>();
                    go.AddComponent<ClinetItemPicker>();
                    go.AddComponent<MeteorSpawner>();
                    BuffDB.FillBuffList();
                    ItemDataBase.Initialize();
                    SpellDataBase.Initialize();
                    EnemyManager.Initialize();
                    new GameObject("MainMenuObj").AddComponent<MainMenu>();
                    Network.NetworkManager.instance.onGetMessage += Network.CommandReader.OnCommand;
                    Res.Buildings.InitBuildings();
                    Perk.FillPerkList();

                }

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }

        }

        private static void ReadDediServerConfig()
        {
            string path = Application.dataPath + "/CotFConfig.txt";
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                var difficultyRegex = new Regex(@"/(?<=Difficulty=)\d+/");
                var friendlyFireRegex = new Regex(@"/(?<=FriendlyFire=)\d+/");

                ModSettings.difficulty = (ModSettings.Difficulty)(int.Parse(difficultyRegex.Match(content).Value));
                ModSettings.FriendlyFire = int.Parse(difficultyRegex.Match(content).Value)==1;
               ModSettings.DifficultyChoosen = true;

            }
            else
            {
                string[] DefaultConfig = new string[]
                {
                    "This is a config file for Champions of the Forest for Dedicated Server.",
                    "To set it up properly, please do the following:",
                    "-Only modify text to the right of '='",
                    "-For difficulty, please write a number from 0 to 8. (0-Normal, 1-Hard, 2-Elite, 3-Master, 4-Challenge1, 5-Challenge2, 6-Challenge3, 7-Challenge4, 8-Challenge5)",
                    "-For friendly fire, write 0 if disabled, 1 if enabled",
                    "--------------------------------------------------------------------",
                    "Difficulty=0",
                    "FriendlyFire=1",
                };
                Debug.Log("Champions Of The Forest for Dedicated Server created a config file at " + path);
                File.WriteAllLines(path, DefaultConfig);
                ModSettings.DifficultyChoosen = true;
            }
        }
    }
}
