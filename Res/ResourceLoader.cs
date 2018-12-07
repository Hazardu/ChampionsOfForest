using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using BuilderCore;
namespace ChampionsOfForest.Res
{
    public class ResourceLoader : MonoBehaviour
    {
        public static ResourceLoader instance = null;


        public List<Resource> unloadedResources;
        public List<Resource> toDownload;
        public List<Resource> toLoad;
        public Dictionary<int, Mesh> LoadedMeshes;
        public Dictionary<int, Texture2D> LoadedTextures;
        private string LabelText;
        private enum VersionCheckStatus { Unchecked, UpToDate, OutDated, Fail }
        private VersionCheckStatus checkStatus = VersionCheckStatus.Unchecked;
        private string NewestVersion;
        public static bool InMainMenu;
        public bool FinishedLoading = false;
        private WWW download;

        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                if (string.IsNullOrEmpty(Resource.path))
                {
                    Resource.path = Application.dataPath + "/COTF Files/";

                    //ModAPI.Console.Write(Resource.path);
                }

            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            FinishedLoading = false;
            unloadedResources = new List<Resource>();
            FillResources();
            LoadedMeshes = new Dictionary<int, Mesh>();
            LoadedTextures = new Dictionary<int, Texture2D>();
            toLoad = new List<Resource>();
            toDownload = new List<Resource>();
          
                StartCoroutine(FileVerification());
                StartCoroutine(VersionCheck());

        }
    
        IEnumerator VersionCheck()
        {
            WWW ModapiWebsite = new WWW("https://modapi.survivetheforest.net/mod/101/champions-of-the-forest");
            yield return ModapiWebsite;
            if (string.IsNullOrEmpty(ModapiWebsite.error) && !string.IsNullOrEmpty(ModapiWebsite.text))
            {
                Regex regex1 = new Regex(@"Version+\W+([0-9.]+)");
                Match match1 = regex1.Match(ModapiWebsite.text);
                if (match1.Success)
                {
                    Regex regex2 = new Regex(@"([0-9.]+)");
                    Match match2 = regex2.Match(match1.Value);
                    if (match2.Success)
                    {
                        NewestVersion = match2.Value;
                        StopCoroutine("VersionCheck");
                        if (ModSettings.Version == NewestVersion)
                        {
                            checkStatus = VersionCheckStatus.UpToDate;
                        }
                        else
                        {
                            checkStatus = VersionCheckStatus.OutDated;
                        }
                    }
                }
            }
            if (checkStatus == VersionCheckStatus.Unchecked)
            {
                checkStatus = VersionCheckStatus.Fail;
            }

        }
        private IEnumerator FileVerification()
        {
            LabelText = "";

         if(DirExists())
            {
                foreach (Resource resource in unloadedResources)
                {
                    if (File.Exists(Resource.path + resource.fileName))
                    {
                        LabelText = LabelText + " \n CHECKING FILES: " + resource.fileName + " EXISTS";
                        toLoad.Add(resource);
                        yield return new WaitForEndOfFrame();

                    }
                    else
                    {
                        LabelText = LabelText + " \n CHECKING FILES: " + resource.fileName + " MISSING";
                        toDownload.Add(resource);
                        yield return new WaitForEndOfFrame();

                    }
                }
            }
            LabelText = LabelText + " \n DONE CHECKING FILES! \n Please wait...";

            foreach (Resource resource in toDownload)
            {
                LabelText = LabelText + " \n Downloading " + Resource.url + resource.fileName;

                WWW www = new WWW(Resource.url + resource.fileName);
                download = www;
                yield return www;
                if (www.isDone)
                {
                    File.WriteAllBytes(Resource.path + resource.fileName, www.bytes);
                    toLoad.Add(resource);
                }
                else
                {
                    LabelText = LabelText + " \n ERROR | " + www.error;
                }
                download = null;
            }

            foreach (Resource resource in toLoad)
            {

                switch (resource.type)
                {
                    case Resource.ResourceType.Texture:
                        Texture2D t = new Texture2D(1, 1);
                        t.LoadImage(File.ReadAllBytes(Resource.path + resource.fileName));
                        
                        t.Apply();
                        t.Compress(true);

                        LoadedTextures.Add(resource.ID, t);
                        LabelText = LabelText + " \n LOADED IMAGE | " + resource.ID;

                        break;
                    case Resource.ResourceType.Mesh:
                        Mesh mesh = Core.ReadMeshFromOBJ(Resource.path + resource.fileName);
                        LoadedMeshes.Add(resource.ID, mesh);
                        LabelText = LabelText + " \n LOADED MESH | " + resource.ID;

                        break;
                    case Resource.ResourceType.Audio:
                        break;
                    case Resource.ResourceType.Text:
                        break;
                }
                yield return null;
            }
            LabelText = "DONE! \n Files downloaded and loaded";
            toDownload.Clear();
            toLoad.Clear();
            yield return new WaitForSeconds(5);

            FinishedLoading = true;
        }


        float p;

        void OnGUI()
        {
            if (!InMainMenu) return;
            if (!FinishedLoading)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 20,
                    alignment = TextAnchor.LowerLeft,
                };
                if (download != null)
                {
                    p = download.progress;
                    GUI.color = new Color(1 - p, 1, 1 - p);
                    GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), LabelText + " \n " + p * 100 + "%", style);
                }
                else
                {
                    GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), LabelText, style);

                }
                GUI.color = Color.white;

            }

            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

            GUIStyle versionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperRight, fontSize = 34, fontStyle = FontStyle.Italic };
            switch (checkStatus)
            {
                case VersionCheckStatus.Unchecked:
                    GUI.color = Color.gray;
                    GUILayout.Label("Checking for updated version...", versionStyle);
                    break;
                case VersionCheckStatus.UpToDate:
                    GUI.color = Color.green;
                    GUILayout.Label("Champions of The Forest is up to date.", versionStyle);

                    break;
                case VersionCheckStatus.OutDated:
                    GUI.color = Color.red;
                    GUILayout.Label("Champions of The Forest is outdated! \n Installed " + ModSettings.Version + ";  Newest " + NewestVersion, versionStyle);

                    break;
                case VersionCheckStatus.Fail:
                    GUI.color = Color.gray;
                    GUILayout.Label("Failed to get update info", versionStyle);
                    break;

            }
            GUI.color = Color.white;

            GUILayout.EndArea();
        }
        bool DirExists()
        {
            if (!Directory.Exists(Resource.path))
            {
                LabelText = LabelText + " \n NO DIRECTORY FOUND, DOWNLOADING \n Please wait... ";
                Directory.CreateDirectory(Resource.path);
                foreach (Resource resource in unloadedResources)
                {
                    toDownload.Add(resource);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        void FillResources()
        {
            new Resource(1, "wheel.png");
            new Resource(2, "wheelOn.png");
            new Resource(5, "SpellBG.png");
            new Resource(6, "SpellFrame.png");
            new Resource(8, "CoolDownFill.png");
            new Resource(12, "Item1SlotBg.png");
            new Resource(13, "Item1Frame.png");
            new Resource(15, "ProgressBack.png");
            new Resource(16, "ProgressFill.png");
            new Resource(17, "ProgressFront.png");
            new Resource(18, "CombatTimer.png");
            new Resource(20, "BlackHoleTex.png");
            new Resource(21, "BlackHole.obj");
            new Resource(22, "Leaf.png");
            new Resource(23, "Stars.png");
            new Resource(24, "SmallCircle.png");
            new Resource(25, "Row.png");
            new Resource(26, "snowflake.png");
            new Resource(27, "Background.png");
            new Resource(28, "HorizontalListItem.png");
            new Resource(30, "Space.png");

            new Resource(40, "amulet.obj");
            new Resource(41, "Glove.obj");
            new Resource(42, "jacket.obj");
            new Resource(43, "ring.obj");
            new Resource(44, "shoe.obj");
            new Resource(45, "Bracer.obj");
            //new Resource(46, "Boots1.obj");
            //new Resource(47, "Boots2.obj");
            new Resource(48, "helmet_armet_2.obj");
            new Resource(49, "Shield.obj");
            new Resource(50, "Pants.obj");
            new Resource(51, "Sword.obj");
            new Resource(52, "HeavySword.obj");
            new Resource(53, "Shoulder.obj");


        }


    }

}
