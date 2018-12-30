using BuilderCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
namespace ChampionsOfForest.Res
{
    public class ResourceLoader : MonoBehaviour
    {
        public static ResourceLoader instance = null;
        public static Texture2D GetTexture(int i)
        {
            if (instance.LoadedTextures.ContainsKey(i))
            {
                return instance.LoadedTextures[i];
            }
            else if (instance.unloadedResources.ContainsKey(i))
            {
                LoadTexture(instance.unloadedResources[i]);
                return instance.LoadedTextures[i];
            }
            return null;

        }
        public static void UnloadTexture(int i)
        {
            Destroy(instance.LoadedTextures[i]);
            instance.LoadedTextures.Remove(i);
        }
        public static void LoadTexture(Resource r)
        {
            Texture2D t = new Texture2D(1, 1);
            t.LoadImage(File.ReadAllBytes(Resource.path + r.fileName));
            t.Apply();
            t.Compress(true);
            instance.LoadedTextures.Add(r.ID, t);

        }


        public Dictionary<int, Resource> unloadedResources;
        //public List<Resource> unloadedResources;
        public List<Resource> toDownload;
        public List<Resource> toLoad;
        public Dictionary<int, Mesh> LoadedMeshes;
        public Dictionary<int, Texture2D> LoadedTextures;
        private string LabelText;
        private enum VersionCheckStatus { Unchecked, UpToDate, OutDated, Fail,NewerThanOnline }
        private VersionCheckStatus checkStatus = VersionCheckStatus.Unchecked;
        private string OnlineVersion;
        public static bool InMainMenu;
        public bool FinishedLoading = false;
        private WWW download;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                if (string.IsNullOrEmpty(Resource.path))
                {
                    Resource.path = Application.dataPath + "/COTF Files/";
                }

            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            FinishedLoading = false;
            unloadedResources = new Dictionary<int, Resource>();
           // unloadedResources = new List<Resource>();
            FillResources();
            LoadedMeshes = new Dictionary<int, Mesh>();
            LoadedTextures = new Dictionary<int, Texture2D>();
            toLoad = new List<Resource>();
            toDownload = new List<Resource>();

            StartCoroutine(FileVerification());
            StartCoroutine(VersionCheck());

        }

        private IEnumerator VersionCheck()
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
                        OnlineVersion = match2.Value;
                        if (ModSettings.Version == OnlineVersion)
                        {
                            checkStatus = VersionCheckStatus.UpToDate;
                        }
                        else if(CompareVersion(OnlineVersion) == Status.Outdated)
                        {
                            checkStatus = VersionCheckStatus.OutDated;
                        }
                        else
                        {
                            checkStatus = VersionCheckStatus.NewerThanOnline;
                        }
                        yield break;
                    }
                }
            }
            if (checkStatus == VersionCheckStatus.Unchecked)
            {
                checkStatus = VersionCheckStatus.Fail;
            }

        }

        public enum Status { TheSame,Outdated,Newer}

        private Status CompareVersion(string s1)
        {
            int i = 0;
            int a = 0;
            string val = "";
            int[] values1 = new int[4];
            int[] values2 = new int[4];

            //filling values1
            while (i < s1.Length)
            {
              if(s1[i] != '.')
                {
                    val += s1[i];
                }
                else
                {
                    values1[a] = int.Parse(val);
                    val = "";
                    a++;

                }
                i++;
            }
            if (val != "")
            {
                values1[a] = int.Parse(val);
            }
            val = "";
            a = 0;
            i = 0;


            while (i < ModSettings.Version.Length)
            {
                if (ModSettings.Version[i] != '.')
                {
                    val += ModSettings.Version[i];
                }
                else
                {
                    values2[a] = int.Parse(val);
                    val = "";
                    a++;

                }
                i++;
            }
            if (val != "")
            {
                values2[a] = int.Parse(val);
            }
            ModAPI.Log.Write(values1[0]+", "+ values1[1] + ", " + values1[2] + ", " + values1[3]+ "\n" + values2[0] + ", " + values2[1] + ", " + values2[2] + ", " + values2[3]);
            for (i = 0; i < 4; i++)
            {
                if(values1[i]>values2[i] )
                {
                    return Status.Outdated;
                }
                else if (  values1[i] < values2[i])
                {
                    return Status.Newer;
                }
            }
            return Status.TheSame;
        }


        private IEnumerator FileVerification()
        {
            LabelText = "";
            
            if (DirExists())
            {
                bool DeleteCurrentFiles = false;
                if (ModSettings.RequiresNewFiles)
                {
                    if (File.Exists(Resource.path + "VERSION.txt"))
                        {
                        string versiontext = File.ReadAllText(Resource.path + "VERSION.txt");
                            if (CompareVersion(versiontext) == Status.Outdated)
                        {
                            DeleteCurrentFiles = true;
                        }
                    }
                }
                File.WriteAllText(Resource.path + "VERSION.txt", ModSettings.Version);
                foreach (Resource resource in unloadedResources.Values)
                {
                    if (File.Exists(Resource.path + resource.fileName))
                    {
                        if (DeleteCurrentFiles &&ModSettings.outdatedFiles.Contains(resource.ID))
                        {
                            LabelText = LabelText + " \n CHECKING FILES: " + resource.fileName + " OUTDATED,DELETING";
                            File.Delete(Resource.path + resource.fileName);
                            toDownload.Add(resource);
                            yield return new WaitForEndOfFrame();

                        }
                        else
                        {
                            LabelText = LabelText + " \n CHECKING FILES: " + resource.fileName + " EXISTS and is up to date";
                            toLoad.Add(resource);
                            yield return new WaitForEndOfFrame();
                        }
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
                        LabelText = LabelText + " \n LOADING IMAGE... | " + resource.ID;

                        Texture2D t = new Texture2D(1, 1, TextureFormat.RGBA32,true, true);
                        //Texture2D t = new Texture2D(1, 1);
                        t.LoadImage(File.ReadAllBytes(Resource.path + resource.fileName));
                        t.Apply();
                        if(resource.CompressTexture)
                        t.Compress(true);

                        LoadedTextures.Add(resource.ID, t);
                        LabelText = LabelText + " ...DONE";
                        break;
                    case Resource.ResourceType.Mesh:
                        Mesh mesh = Core.ReadMeshFromOBJ(Resource.path + resource.fileName);
                        LoadedMeshes.Add(resource.ID, mesh);
                        LabelText = LabelText + " \n LOADED MESH | " + resource.ID;

                        break;
                    case Resource.ResourceType.Audio:
                        //hit or miss
                        break;
                    case Resource.ResourceType.Text:
                        //i guess they never miss
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

        private float p;

        private void OnGUI()
        {
            if (!InMainMenu)
            {
                return;
            }

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
                    GUILayout.Label("Champions of The Forest is outdated! \n Installed " + ModSettings.Version + ";  Newest " + OnlineVersion, versionStyle);

                    break;
                case VersionCheckStatus.Fail:
                    GUI.color = Color.gray;
                    GUILayout.Label("Failed to get update info", versionStyle);
                    break;
                case VersionCheckStatus.NewerThanOnline:
                    GUI.color = Color.yellow;
                    GUILayout.Label("You're using a version newer than uploaded to ModAPI", versionStyle);
                    break;
            }
            GUI.color = Color.white;

            GUILayout.EndArea();
        }

        private bool DirExists()
        {
            if (!Directory.Exists(Resource.path))
            {
                LabelText = LabelText + " \n NO DIRECTORY FOUND, DOWNLOADING \n Please wait... ";
                Directory.CreateDirectory(Resource.path);
                foreach (Resource resource in unloadedResources.Values)
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

        private void FillResources()
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

            //new Resource(59, "SwordMetalic.png");
            new Resource(60, "SwordTexture.png");
            new Resource(61, "SwordColor.png");
            new Resource(62, "SwordEmissive.png");
            new Resource(64, "SwordNormal.png");
            new Resource(65, "SwordRoughness.png");
            new Resource(66, "SwordAmbientOcculusion.png");
            new Resource(67, "ManyParticles.png");
            new Resource(68, "InverseSphere.obj");
            new Resource(69, "ChainPart.obj");
            new Resource(70, "Spike.obj");
            new Resource(71, "particle.png");

            new Resource(72, "Melee.jpg");
            new Resource(73, "Magic.jpg");
            new Resource(74, "Ranged.jpg");
            new Resource(75, "Defensive.jpg");
            new Resource(76, "Utility.jpg");
            new Resource(77, "Support.jpg");
            new Resource(78, "Background1.jpg");

            new Resource(82, "PerkNode1.png");
            new Resource(81, "PerkNode2.png");
            new Resource(83, "PerkNode3.png");
            new Resource(84, "PerkNode4.png");

            new Resource(85, "ItemBoots.png");
            new Resource(86, "ItemGloves.png");
            new Resource(87, "ItemPants.png");
            new Resource(88, "ItemGreatSword.png");
            new Resource(89, "ItemLongSword.png");
            new Resource(90, "ItemRing.png");
            new Resource(91, "ItemHelmet.png");
            new Resource(92, "ItemBoots1.png");
            new Resource(93, "ItemBracer.png");
            new Resource(94, "ItemBracer2.png");
            new Resource(95, "ItemShoulder.png");
            new Resource(96, "ItemChest.png");
            new Resource(97, "ItemPants.png");
            new Resource(98, "ItemQuiver.png");
            new Resource(99, "ItemShield.png");
            new Resource(100, "ItemScarf.png");
            new Resource(101, "ItemAmulet.png");
            new Resource(102, "Heart.obj");
            new Resource(103, "HeartTexture.png");
            new Resource(104, "HeartNormal.png");
            new Resource(105, "ItemHeart.png");




        }


    }

}
