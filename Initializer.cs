using ChampionsOfForest.Player;
using ModAPI.Attributes;
using System;
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

                ModSettings.DifficultyChoosen = false;
                if (SceneManager.GetActiveScene().name == "TitleScene")
                {
                    new GameObject("Resource Manager").AddComponent<Res.ResourceLoader>();
                    Res.ResourceLoader.InMainMenu = true;
                }
                else
                {
                    Res.ResourceLoader.InMainMenu = false;
                    SpellDataBase.Initialize();
                    new GameObject("NetworkManagerObj").AddComponent<Network.NetworkManager>();
                    ItemDataBase.Initialize();
                    GameObject go = new GameObject("Playerobj");
                    go.AddComponent<ModdedPlayer>();
                    go.AddComponent<Inventory>();
                    go.AddComponent<ModReferences>();
                    go.AddComponent<SpellCaster>();
                    go.AddComponent<ClinetItemPicker>();
                    BuffDB.FillBuffList();

                    EnemyManager.Initialize();
                    new GameObject("MainMenuObj").AddComponent<MainMenu>();
                    Network.NetworkManager.instance.onGetMessage += Network.CommandReader.OnCommand;
                    Buildings.InitBuildings();
                    Perk.FillPerkList();

                }

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }

        }
    }
}
