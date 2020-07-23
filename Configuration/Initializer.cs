using ChampionsOfForest.Effects;
using ChampionsOfForest.Effects.Sound_Effects;
using ChampionsOfForest.Enemies;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.ExpSources;
using ChampionsOfForest.Items;
using ChampionsOfForest.Player;
using ChampionsOfForest.Res;

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
               ModSettings.Version =  ModAPI.Mods.LoadedMods["ChampionsOfForest"].Version;
            
                if (SteamDSConfig.isDedicated)
                {
                    ModAPI.Log.Write("isDedicated true");
                    ModSettings.IsDedicated = true;
                }

                if (ModSettings.IsDedicated)
                {
                    DedicatedServer.COTFDS.ReadDediServerConfig();
                    new GameObject("NetworkManagerObj").AddComponent<Network.NetworkManager>();
                    GameObject go = new GameObject("Playerobj");
                    go.AddComponent<ModReferences>();
                    ItemDataBase.Initialize();
                    EnemyManager.Initialize();
                    Network.NetworkManager.instance.onGetMessage += Network.CommandReader.OnCommand;
                    ExpEvents.Initialize();
                    return;
                }

                ModSettings.DifficultyChoosen = false;
                if (SceneManager.GetActiveScene().name == "TitleScene")
                {
                    new GameObject("Resource Manager").AddComponent<Res.ResourceLoader>();
                    Res.ResourceLoader.InMainMenu = true;
                    Effects.MainMenuVisual.Create();

                }
                else
                {
                    Res.ResourceLoader.InMainMenu = false;

                    new GameObject("NetworkManagerObj").AddComponent<Network.NetworkManager>();
                    GameObject go = new GameObject("__COTFPlayerobj__");
                    go.AddComponent<ModdedPlayer>();
                    go.AddComponent<Inventory>();
                    go.AddComponent<ModReferences>();
                    go.AddComponent<SpellCaster>();
                    go.AddComponent<ClinetItemPicker>();
                    go.AddComponent<MeteorSpawner>();
                    go.AddComponent<BlackFlame>();
                    go.AddComponent<AsyncHit>();
                    go.AddComponent<GlobalSFX>();
                    go.AddComponent<TheFartCreator>();
                    go.AddComponent<RCoroutines>();


                   // go.AddComponent<Crafting>();
                    CustomCrafting.Init();
                    BuffDB.FillBuffList();
                    ItemDataBase.Initialize();
                    SpellDataBase.Initialize();
                    EnemyManager.Initialize();
                    new GameObject("MainMenuObj").AddComponent<MainMenu>();
                    Network.NetworkManager.instance.onGetMessage += Network.CommandReader.OnCommand;
                    Res.Buildings.InitBuildings();
                    PerkDatabase.FillPerkList();
                    ExpEvents.Initialize();
                    Portal.InitializePortals();
                    CoopCustomWeapons.Init();
                    BallLightning.InitPrefab();
                    ResourceInitializer.SetupMeshesFromOtherAssets();
                    Cataclysm.AssignPrefabs();
                }

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }

        }
        
    }
}
