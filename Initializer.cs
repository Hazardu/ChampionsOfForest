using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModAPI;
using ModAPI.Attributes;
using TheForest.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChampionsOfForest.Network;
using ChampionsOfForest;
using ChampionsOfForest.Player;

namespace ChampionsOfForest
{
    public class Initializer
    {


        [ExecuteOnGameStart]
        public static void Initialize()
        {
            try
            {

                ModSettings.DifficultyChoosen = false;
                if (SceneManager.GetActiveScene().name == "TitleScene")
                {
                    ModAPI.Log.Write("Title Scene");
                    new GameObject("Resource Manager").AddComponent<Res.ResourceLoader>();
                    Res.ResourceLoader.InMainMenu = true;
                }
                else
                {
                    ModAPI.Log.Write("Other Scene");
                    Res.ResourceLoader.InMainMenu = false;
                    new GameObject("NetworkManagerObj").AddComponent<Network.NetworkManager>();
                    ItemDataBase.Initialize();
                    GameObject go = new GameObject("Playerobj");
                    go.AddComponent<ModdedPlayer>();
                    go.AddComponent<Inventory>();
                    go.AddComponent<ModReferences>();
                    go.AddComponent<SpellCaster>();
                    go.AddComponent<ClinetItemPicker>();
                    EnemyManager.Initialize();  
                        new GameObject("MainMenuObj").AddComponent<MainMenu>();
                    BuffDataBase.Init();
                    Network.NetworkManager.instance.onGetMessage += Network.CommandReader.OnCommand;
                    Buildings.InitBuildings();

                }

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }

        }
    }
}
