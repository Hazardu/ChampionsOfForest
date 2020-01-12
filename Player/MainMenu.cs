using ChampionsOfForest.Network;
using ChampionsOfForest.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheForest.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ChampionsOfForest.Player.BuffDB;
using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
    public class MainMenu : MonoBehaviour
    {
        //Instance of this object
        public static MainMenu Instance { get; private set; }

        //Difficulty Settings
        public string[] DiffSel_Names = new string[] { "Easy", "Veteran", "Elite", "Master", "Challenge I", "Challenge II", "Challenge III", "Challenge IV", "Challenge V", "Challenge VI", "Hell", };
        public string[] DiffSel_Descriptions = new string[]
        {
            "Easiest difficulty, recommended for new games.",
            "Much harder than normal difficulty, tougher enemies. \nUnlocks higher tier loot. Recommended level 15+",
            "Tougher enemies and more experience. For those who can kill elites at previous difficulty.",
            "Unlocks higher tier of items. For strong players only. Enemies are much tougher ",
            "Challenge I unlocks 6th tier of items. \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge II unlocks 7th tier of items - legendary gear. \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge III\nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge IV\nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge V  \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge VI  \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Hell  \nHarder than Challenge VI, every enemy is an elite. Made to bully Alex Armsy.",

        };
        private int DiffSelPage = 0;

        //variables
        public float rr;                                // current resolution to fullHD ratio
        private Vector2 mousepos;                       //mouse postion in GUI space
        private Rect wholeScreen;                       //rect that contains the entire screen
        private bool Crouching;                         //used in hud display, when crouching scans enemies
        private bool MenuInteractable;                  //determines if menu elements can be clicked or not
        private GUIStyle MenuBtnStyle;                  //style of font for main menu button
        private readonly float DarkeningSpeed = 2;      //speed of transion between menus
        public Font MainFont;                           //main font for the mod
        private float requestResendTime = 0;            //float that measures time between difficulty request times


        //main menu vars
        //alphas for button highlight image
        private float Opacity1 = 0;
        private float Opacity2 = 0;
        private float Opacity3 = 0;
        private float Opacity4 = 0;
        private readonly float OpacityGainSpeed = 3;    //speed at which the highlights gain alpha - curently gains full opacity after 0.33 seconds

        //HUD variables
        private float ScanTime = 0;
        private float ScanRotation = 0;
        private Transform scannedTransform;
        private BoltEntity scannedEntity;
        private bool HideHud = false;
        private float ProgressBarAmount = 0;
        private Rect HUDenergyLabelRect;
        private Rect HUDHealthLabelRect;
        private Rect HUDShieldLabelRect;

        private GUIStyle HUDStatStyle;

        //Inventory variables
        private float InventoryScrollAmount = 0;
        private int SelectedItem;
        public int DraggedItemIndex;

        public bool isDragging;
        private bool consumedsomething;

        public Item DraggedItem = null;
        private Vector2 itemPos;
        private Vector2 slotDim;


        //Perks variables
        private readonly float PerkHexagonSide = 60;
        private float PerkHeight;
        private float PerkWidth;
        private float difficultyCooldown;

        private readonly float GuideWidthDecrease = 150;
        private readonly float GuideMargin = 30;
        private Transform _mainCam;
        private Transform Cam
        {
            get
            {
                if (_mainCam == null)
                {
                    _mainCam = Camera.main.transform;
                }
                return _mainCam;
            }
        }

        //Textures
        private Texture2D _black;
        private Texture2D _blackTexture;
        private Texture2D _combatDurationTex;
        private Texture2D _expBarFillTex;
        private Texture2D _expBarBackgroundTex;
        private Texture2D _NoSpellIcon;
        private Texture2D _SpellBG;
        private Texture2D _SpellInactive;
        private Texture2D _SpellFrame;
        private Texture2D _SpellCoolDownFill;
        private Texture2D _expBarFrameTex;
        private const float BuffSize = 50;

        //a static variable for colors of items with different rarities
        //affects item border in inventory, text color in pickup, particle effect color 
        public static Color[] RarityColors = new Color[]
        {
            new Color(0.4f,0.4f,0.4f),
            new Color(0.6f,0.6f,0.6f),
            new Color(0.1f,0.1f,0.75f),
            new Color(0.1f,0.5f,1f),
            new Color(1,0.95f,0.1f),
            new Color(1,0.5f,0f),
            new Color(0.1f,1,0.3f),
            new Color(0.74f,0.05f,0.05f),
        };

        public enum OpenedMenuMode
        {
            Main,
            Inventory,
            Hud,
            Spells,
            Stats,
            Perks,
        }
        public OpenedMenuMode _openedMenu;


        public List<HitMarker> hitMarkers = new List<HitMarker>();
        public class HitMarker
        {
            public string txt;
            public Vector3 worldPosition;
            public float lifetime;
            public bool Player;
            public void Delete(int i)
            {
                Instance.hitMarkers.RemoveAt(i);
            }
            public HitMarker(int t, Vector3 p)
            {
                txt = t.ToString("N0");
                worldPosition = p;
                lifetime = 4f;
                Instance.hitMarkers.Add(this);
            }
            public HitMarker(int t, Vector3 p, bool Player)
            {
                txt = t.ToString("N0");
                worldPosition = p;
                lifetime = 6f;
                this.Player = Player;
                Instance.hitMarkers.Add(this);
            }
        }








        //setting the instance
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private Matrix4x4 matrixBackup;
        private void Start()
        {

            try
            {
                var sceneName = SceneManager.GetActiveScene().name;
                if (sceneName == "TitleScene" || sceneName == "TitleSceneLoader")
                {
                    Destroy(gameObject);
                }


                rr = Screen.height / 1080f;
                wholeScreen = new Rect(0, 0, Screen.width, Screen.height);

                slotDim = new Vector2(100 * rr, 100 * rr);

                _black = new Texture2D(1, 1);
                _black.SetPixel(0, 0, Color.black);
                _black.Apply();
                _openedMenu = OpenedMenuMode.Hud;
                _blackTexture = new Texture2D(1, 1);
                _blackTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));
                _blackTexture.Apply();
                matrixBackup = GUI.matrix;
                MenuInteractable = true;

                //Perks
                PerkHeight = PerkHexagonSide * 2 * rr;
                PerkWidth = PerkHexagonSide * 1.732050f * rr; //times sqrt(3)

                //HUD
                HideHud = false;
                HUDenergyLabelRect = new Rect(Screen.width - 500 * rr, Screen.height - 100 * rr, 500 * rr, 100 * rr);
                HUDHealthLabelRect = new Rect(Screen.width - 500 * rr, Screen.height - 140 * rr, 500 * rr, 100 * rr);
                HUDShieldLabelRect = new Rect(Screen.width - 500 * rr, Screen.height - 180 * rr, 500 * rr, 100 * rr);

                //The main font as of now is Gabriola
                MainFont = Font.CreateDynamicFontFromOSFont("Bahnschrift", Mathf.RoundToInt(24 * rr));
                if (MainFont == null)
                {
                    MainFont = Font.CreateDynamicFontFromOSFont("Arial", Mathf.RoundToInt(24 * rr));
                }

                //Getting textures using ResourceLoader
                _combatDurationTex = ResourceLoader.instance.LoadedTextures[18];
                _expBarFillTex = ResourceLoader.instance.LoadedTextures[16];
                _expBarBackgroundTex = ResourceLoader.instance.LoadedTextures[15];
                _expBarFrameTex = ResourceLoader.instance.LoadedTextures[17];
                _NoSpellIcon = ResourceLoader.instance.LoadedTextures[5];
                _SpellBG = ResourceLoader.instance.LoadedTextures[5];
                _SpellInactive = ResourceLoader.instance.LoadedTextures[5];
                _SpellFrame = ResourceLoader.instance.LoadedTextures[6];
                _SpellCoolDownFill = ResourceLoader.instance.LoadedTextures[8];
                //Inventory

                DraggedItemIndex = -1;

                DraggedItem = null;
                semiBlack = new Texture2D(1, 1);

                //host difficulty raise/lower cooldown
                if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer) difficultyCooldown = 5 * 60; //once every 5 minutes 

                if (GameSetup.IsMultiplayer) otherPlayerPings = new Dictionary<string, MarkObject>();

                StartCoroutine(ProgressionRefresh());
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write("ERROR: Failure in start of Main Menu: " + ex.ToString());
            }
        }
        public int LevelsToGain = 0;





        private IEnumerator ProgressionRefresh()
        {
            while (true)
            {
                if (GameSetup.IsMultiplayer)
                {
                    foreach (KeyValuePair<BoltEntity, ClinetEnemyProgression> item in EnemyManager.clinetProgressions)
                    {
                        if (Time.time > item.Value.creationTime + ClinetEnemyProgression.LifeTime)
                        {
                            EnemyManager.clinetProgressions.Remove(item.Key);
                            break;
                        }
                    }
                }
                yield return null;
                if (GameSetup.IsSinglePlayer)
                {
                    foreach (KeyValuePair<Transform, ClinetEnemyProgression> item in EnemyManager.spProgression)
                    {
                        if (Time.time > item.Value.creationTime + ClinetEnemyProgression.LifeTime)
                        {
                            EnemyManager.spProgression.Remove(item.Key);
                            break;
                        }
                    }
                }
                yield return null;
                foreach (KeyValuePair<ulong, EnemyProgression> item in EnemyManager.hostDictionary)
                {
                    if (item.Value._hp + item.Value._Health.Health < 1)
                    {
                        EnemyManager.hostDictionary.Remove(item.Key);
                        break;
                    }
                }
                yield return null;
            }
        }



        private void Update()
        {

            if (difficultyCooldown > 0) difficultyCooldown -= Time.deltaTime;
            LevelUpDuration -= Time.deltaTime;

            try
            {
                PingUpdate();

            }
            catch (Exception exc)
            {
                ModAPI.Console.Write("ERROR with ping update\n" + exc.ToString());
            }



            if (_openedMenu != OpenedMenuMode.Hud)
            {
                if (_openedMenu == OpenedMenuMode.Inventory)
                {
                    if (UnityEngine.Input.GetMouseButtonUp(1))
                    {
                        if (consumedsomething) consumedsomething = false;
                    }

                    if (SelectedItem != -1 && Inventory.Instance.ItemList[SelectedItem] != null)
                    {
                        var item = Inventory.Instance.ItemList[SelectedItem];
                        if (UnityEngine.Input.GetMouseButtonDown(0))
                        {
                            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                            {
                                //unequip if equipped
                                if (SelectedItem < -1)
                                {
                                    for (int i = 0; i < Inventory.Instance.SlotCount; i++)
                                    {
                                        if (Inventory.Instance.ItemList[i] == null)
                                        {
                                            DraggedItem = null;
                                            isDragging = false;
                                            CustomCrafting.ClearIndex(SelectedItem);
                                            CustomCrafting.ClearIndex(i);
                                            DraggedItemIndex = -1;
                                            Inventory.Instance.ItemList[i] = item;
                                            Inventory.Instance.ItemList[SelectedItem] = null;
                                            SelectedItem = -1;
                                            break;
                                        }
                                    }
                                }
                                else//move to its correct slot or swap if slot is not empty
                                {
                                    int targetSlot = -1;

                                    switch (item._itemType)
                                    {
                                        case BaseItem.ItemType.Helmet:
                                            targetSlot = -2;
                                            break;
                                        case BaseItem.ItemType.ChestArmor:
                                            targetSlot = -3;
                                            break;
                                        case BaseItem.ItemType.Pants:
                                            targetSlot = -4;
                                            break;
                                        case BaseItem.ItemType.Boot:
                                            targetSlot = -5;
                                            break;
                                        case BaseItem.ItemType.ShoulderArmor:
                                            targetSlot = -6;
                                            break;
                                        case BaseItem.ItemType.Glove:
                                            targetSlot = -7;
                                            break;
                                        case BaseItem.ItemType.Amulet:
                                            targetSlot = -8;
                                            break;
                                        case BaseItem.ItemType.Bracer:
                                            targetSlot = -9;
                                            break;
                                        case BaseItem.ItemType.Ring:
                                            if (Inventory.Instance.ItemList[-10] == null)
                                                targetSlot = -10;
                                            else if (Inventory.Instance.ItemList[-11] == null)
                                                targetSlot = -11;
                                            else targetSlot = -10;
                                            break;
                                        case BaseItem.ItemType.Weapon:
                                            targetSlot = -12;
                                            break;
                                        case BaseItem.ItemType.Quiver:
                                        case BaseItem.ItemType.SpellScroll:
                                        case BaseItem.ItemType.Shield:
                                            targetSlot = -13;
                                            break;
                                    }
                                    if (targetSlot != -1)
                                    {
                                        if (Inventory.Instance.ItemList[targetSlot] == null)
                                        {
                                            DraggedItem = null;
                                            isDragging = false;
                                            DraggedItemIndex = -1;
                                            CustomCrafting.ClearIndex(SelectedItem);
                                            Inventory.Instance.ItemList[targetSlot] = item;
                                            Inventory.Instance.ItemList[SelectedItem] = null;
                                            SelectedItem = -1;

                                        }
                                        else
                                        {
                                            DraggedItem = null;
                                            isDragging = false;
                                            DraggedItemIndex = -1;
                                            CustomCrafting.ClearIndex(SelectedItem);
                                            Inventory.Instance.ItemList[SelectedItem] = Inventory.Instance.ItemList[targetSlot];
                                            Inventory.Instance.ItemList[targetSlot] = item;
                                            SelectedItem = -1;
                                        }
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
                            {
                                if (SelectedItem > -1)
                                {
                                    int max = 2;
                                    switch (CustomCrafting.instance.craftMode)
                                    {
                                        case CustomCrafting.CraftMode.Rerolling:

                                            max = CustomCrafting.instance.rerolling.IngredientCount;
                                            break;
                                        case CustomCrafting.CraftMode.Reforging:
                                            max = CustomCrafting.instance.reforging.IngredientCount;
                                            break;
                                        case CustomCrafting.CraftMode.Repurposing:
                                            break;
                                        case CustomCrafting.CraftMode.Upgrading:
                                            break;
                                        default:
                                            break;
                                    }
                                    for (int j = 0; j < max; j++)
                                    {
                                        if (CustomCrafting.instance.ingredients[j].i == null)
                                        {
                                            if (!(CustomCrafting.instance.ingredients.Any(x => x.i == item) || CustomCrafting.instance.changedItem.i == item))
                                            {
                                                CustomCrafting.instance.ingredients[j].Assign(SelectedItem, item);
                                                DraggedItem = null;
                                                DraggedItemIndex = -1;
                                                isDragging = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (consumedsomething)
                {
                    consumedsomething = false;
                }
                //LocalPlayer.FpCharacter.LockView(false);
                LevelsToGain = 0;
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                    StartCoroutine(FadeMenuSwitch(OpenedMenuMode.Hud));
                }
            }
            else
            {
                if (LevelsToGain > 0)
                {
                    if (ProgressBarAmount < 1)
                    {
                        ProgressBarAmount = Mathf.MoveTowards(ProgressBarAmount, 1, Time.unscaledDeltaTime * 2);

                    }
                    else
                    {
                        LevelsToGain--;
                        ProgressBarAmount = 0;
                    }
                }
                else
                {
                    if (ProgressBarAmount < (float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal)
                    {
                        ProgressBarAmount = Mathf.MoveTowards(ProgressBarAmount, Convert.ToSingle((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal), Time.unscaledDeltaTime / 2);
                    }
                    else
                    {
                        ProgressBarAmount = Convert.ToSingle((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal);
                    }
                }
                ProgressBarAmount = Mathf.Clamp01(ProgressBarAmount);
            }
            Crouching = LocalPlayer.FpCharacter.crouching;
            if (ModAPI.Input.GetButtonDown("MenuToggle"))
            {
                MenuKeyPressAction();
            }



        }

        //Draws everything
        private void OnGUI()
        {
            try
            {


                //DETAIL-----------------------------------------------------------------------------
                //GUI.skin.horizontalSlider.fixedWidth = 150;
                //GUI.skin.horizontalSlider.fontSize = 30;
                //PlayerInventoryMod.Pos = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.x, -0.5f, 0.5f), GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.y, -0.5f, 0.5f), GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.z, -0.5f, 0.5f));
                //GUILayout.Label(PlayerInventoryMod.Pos.x.ToString());
                //GUILayout.Label(PlayerInventoryMod.Pos.y.ToString());
                //GUILayout.Label(PlayerInventoryMod.Pos.z.ToString());
                //PlayerInventoryMod.Rot = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.x, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.y, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.z, -180, 180));
                //GUILayout.Label(PlayerInventoryMod.Rot.ToString());
                //foreach (var item in PlayerInventoryMod.customWeapons.Values)
                //{
                //    item.obj.transform.localPosition = PlayerInventoryMod.OriginalOffset + PlayerInventoryMod.Pos + item.offset;
                //    item.obj.transform.localRotation = PlayerInventoryMod.originalRotation;
                //    item.obj.transform.Rotate(PlayerInventoryMod.Rot + item.rotation, UnityEngine.Space.Self);
                //}

                // //BIG OFFSET-----------------------------------------------------------------------------
                //GUILayout.Label("-----------------------------------------------------------------------------------------------------------------------------------------");
                //PlayerInventoryMod.Pos = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.x, -1, 1), GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.y, -1, 1), GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.z, -1, 1));
                //GUILayout.Label(PlayerInventoryMod.Pos.ToString("N4"));
                //PlayerInventoryMod.Rot = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.x, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.y, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.z, -180, 180));
                //GUILayout.Label(PlayerInventoryMod.Rot.ToString("N4"));
                //// PlayerInventoryMod.Sca = GUILayout.HorizontalSlider(PlayerInventoryMod.Sca, 0.01f, 3f);
                //// GUILayout.Label(PlayerInventoryMod.Sca.ToString("N4"));

                //foreach (var item in PlayerInventoryMod.customWeapons.Values)
                //{
                //    item.obj.transform.localPosition = PlayerInventoryMod.OriginalOffset + PlayerInventoryMod.Pos + item.offset;
                //    item.obj.transform.localRotation = PlayerInventoryMod.originalRotation;
                //    item.obj.transform.Rotate(PlayerInventoryMod.Rot + item.rotation, UnityEngine.Space.Self);
                //    //item.obj.transform.localScale = Vector3.one * item.Scale * PlayerInventoryMod.Sca;
                //}


                GUI.skin.label.normal.textColor = Color.white;
                GUI.skin.label.onNormal.textColor = Color.white;
                GUI.contentColor = Color.white;
                GUI.backgroundColor = Color.white;
                GUI.color = Color.white;
                GUI.skin.label.clipping = TextClipping.Overflow;
                GUI.skin.label.richText = true;


                mousepos = new Vector2(UnityEngine.Input.mousePosition.x, Screen.height - UnityEngine.Input.mousePosition.y);

                if (!ModSettings.DifficultyChoosen)
                {
                    if (LocalPlayer.Stats != null)
                    {


                        if (GameSetup.IsMpClient)
                        {
                            DifficultySelectionClinet();
                        }
                        else
                        {
                            DifficultySelectionHost();
                        }
                    }
                }
                else
                {
                    if (HUDStatStyle == null)
                    {
                        HUDStatStyle = new GUIStyle(GUI.skin.label)
                        {
                            font = MainFont,
                            fontSize = Mathf.RoundToInt(33 * rr),
                            alignment = TextAnchor.LowerRight,
                            wordWrap = false,
                            clipping = TextClipping.Overflow,
                        };
                    }

                    if (MenuBtnStyle == null)
                    {
                        MenuBtnStyle = new GUIStyle(GUI.skin.label)
                        {
                            font = MainFont,
                            alignment = TextAnchor.MiddleCenter,
                            fontSize = Mathf.RoundToInt(40 * rr),
                            wordWrap = false,
                            fontStyle = FontStyle.BoldAndItalic,
                            onHover = new GUIStyleState()
                            {
                                textColor = new Color(1, 0.5f, 0.35f)
                            }
                        };
                    }

                    try
                    {



                        if (_openedMenu == OpenedMenuMode.Main)
                        {
                            GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));

                            //GUI.Label(new Rect(wholeScreen.center, Vector2.one * 500), "Main");

                            DrawMain();
                        }
                        else if (_openedMenu == OpenedMenuMode.Inventory)
                        {
                            GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));
                            DrawInventory();

                            //GUI.Label(new Rect(wholeScreen.center, Vector2.one * 500), "Inventory");
                        }
                        else if (_openedMenu == OpenedMenuMode.Hud)
                        {
                            InventoryScrollAmount = 0;
                            DrawHUD();
                        }
                        else if (_openedMenu == OpenedMenuMode.Spells)
                        {
                            GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));
                            DrawSpellMenu();

                        }
                        else if (_openedMenu == OpenedMenuMode.Stats)
                        {
                            GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));
                            DrawGuide();

                        }
                        else if (_openedMenu == OpenedMenuMode.Perks)
                        {
                            DrawPerks();


                        }
                        GUI.DrawTexture(wholeScreen, _blackTexture);
                    }
                    catch (Exception ex)
                    {

                        ModAPI.Log.Write(ex.ToString());
                    }
                }
            }
            catch (Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }
        }

        #region MainMenuMethods
        //Draws main menu with buttons
        private void DrawMain()
        {
            try
            {
                if (MenuBtnStyle == null)
                {
                    MenuBtnStyle = new GUIStyle(GUI.skin.label)
                    {
                        fontSize = 30,
                        alignment = TextAnchor.MiddleCenter
                    };
                }
                if (isDragging)
                {
                    if (DraggedItem.Equipped)
                    {
                        DraggedItem.OnUnequip();
                        Inventory.Instance.ItemList[DraggedItemIndex].Equipped = false;
                    }
                    Inventory.Instance.DropItem(DraggedItemIndex);
                    DraggedItem = null;
                    DraggedItemIndex = -1;
                    isDragging = false;

                }
                targetPerkOffset = wholeScreen.center;
                currentPerkOffset = targetPerkOffset;
                Vector2 center = wholeScreen.center;
                Rect MiddleR = new Rect(Vector2.zero, new Vector2(300 * rr, 300 * rr))
                {
                    center = center
                };

                GUI.Label(new Rect(10 * rr, 10 * rr, 300, 100), "Difficulty: " + DiffSel_Names[(int)ModSettings.difficulty], new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(20f * rr), alignment = TextAnchor.MiddleLeft });
                //drawing difficulty raise lower buttons
                if (difficultyCooldown <= 0 && !GameSetup.IsMpClient)
                {
                    if ((int)ModSettings.difficulty < (int)ModSettings.Difficulty.Hell && GUI.Button(new Rect(10 * rr, 90 * rr, 200 * rr, 40 * rr), "Raise Difficulty", new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(20f * rr), alignment = TextAnchor.MiddleLeft, font = MainFont, hover = new GUIStyleState() { textColor = new Color(0.6f, 0, 0) } }))
                    {
                        //raise difficulty
                        difficultyCooldown = 10 * 60;
                        ModSettings.difficulty++;
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
                    if (ModSettings.difficulty > (int)ModSettings.Difficulty.Normal && GUI.Button(new Rect(10 * rr, 130 * rr, 200 * rr, 40 * rr), "Lower Difficulty", new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(20f * rr), alignment = TextAnchor.MiddleLeft, font = MainFont, hover = new GUIStyleState() { textColor = new Color(0f, 0.6f, 0) } }))
                    {
                        //lower difficulty
                        difficultyCooldown = 10 * 60;
                        ModSettings.difficulty--;
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


                Rect r1 = new Rect(wholeScreen.center, new Vector2(Screen.height / 2, Screen.height / 2));
                Rect r2 = new Rect(r1);
                Rect r3 = new Rect(r1);
                Rect r4 = new Rect(r1);
                float Mindist = 500f / 1500f;
                Mindist *= r1.width;
                MenuButton(Mindist, r1, OpenedMenuMode.Inventory, "Inventory", new Vector2(1, -1), ref Opacity1,-50*rr,-50*rr);
                r2.position = center - r1.size;
                MenuButton(Mindist, r2, OpenedMenuMode.Spells, "Abilities", new Vector2(-1, 1), ref Opacity2, 50 * rr, 50 * rr);
                r3.position = center - new Vector2(0, r1.width);
                MenuButton(Mindist, r3, OpenedMenuMode.Stats, "Guide & Stats", Vector2.one, ref Opacity3, -50 * rr, 50 * rr);
                r4.position = center - new Vector2(r1.width, 0);
                MenuButton(Mindist, r4, OpenedMenuMode.Perks, "Mutations", -Vector2.one, ref Opacity4, 50 * rr, -50 * rr);
                GUI.Label(MiddleR, "Level \n" + ModdedPlayer.instance.Level.ToString(), MenuBtnStyle);

                string HudHideStatus = "[ HUD ]";
                if (HideHud)
                {
                    HudHideStatus = "[ NO HUD ]";
                }

                if (GUI.Button(new Rect(Screen.width - 120 * rr, 40 * rr, 120 * rr, 40 * rr), HudHideStatus, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(19 * rr), alignment = TextAnchor.MiddleCenter }))
                {
                    HideHud = !HideHud;
                }
                DisplayedPerkIDs = null;

                semiblackValue = 0;
            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        //Draws a single main menu button
        private void MenuButton(float mindist, Rect rect, OpenedMenuMode mode, string text, Vector2 Scale, ref float Opacity, float offset_X,float offset_Y)
        {

            Matrix4x4 backupMatrix = GUI.matrix;
            GUIUtility.ScaleAroundPivot(Scale, rect.center);
            float dist = Vector2.Distance(mousepos, wholeScreen.center);
            GUI.DrawTexture(rect, Res.ResourceLoader.instance.LoadedTextures[1]);

            if (dist > mindist && dist < rect.height && rect.Contains(mousepos) && MenuInteractable)
            {
                Opacity = Mathf.Clamp01(Opacity + Time.unscaledDeltaTime * OpacityGainSpeed);

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(FadeMenuSwitch(mode));
                }
            }
            else
            {
                Opacity = Mathf.Clamp01(Opacity - Time.unscaledDeltaTime * OpacityGainSpeed);
            }
            if (Opacity > 0)
            {

                GUI.color = new Color(1,1,1, Opacity);
                GUI.DrawTexture(rect, Res.ResourceLoader.instance.LoadedTextures[2]);
                GUI.color = Color.white;

            }
            GUI.matrix = backupMatrix;
            rect.x += offset_X;
            rect.y += offset_Y;
            GUI.Label(rect, text, MenuBtnStyle);

        }

        private void MenuKeyPressAction()
        {
            if (!MenuInteractable)
            {
                return;
            }

            if (_openedMenu == OpenedMenuMode.Main)
            {
                StartCoroutine(FadeMenuSwitch(OpenedMenuMode.Hud));
                LocalPlayer.FpCharacter.UnLockView();
                LocalPlayer.Inventory.EquipPreviousWeapon();


            }
            else if (_openedMenu == OpenedMenuMode.Hud)
            {
                StartCoroutine(FadeMenuSwitch(OpenedMenuMode.Main));
                LocalPlayer.FpCharacter.LockView(true);
                LocalPlayer.Inventory.StashEquipedWeapon(false);
                LocalPlayer.Inventory.StashLeftHand();
            }
            else
            {
                StartCoroutine(FadeMenuSwitch(OpenedMenuMode.Main));
                LocalPlayer.FpCharacter.LockView(true);
                LocalPlayer.Inventory.StashEquipedWeapon(false);
                LocalPlayer.Inventory.StashLeftHand();
            }
        }

        #endregion

        #region DifficultySelectionMethods
        private void DifficultySelectionHost()
        {
            if ((bool)LocalPlayer.FpCharacter)
            {
                LocalPlayer.FpCharacter.LockView(true);
                LocalPlayer.FpCharacter.MovementLocked = true;
            }
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _black);
            GUIStyle DifficultySelectionStyle = new GUIStyle(GUI.skin.button);
            GUIStyle DifSelNameStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.FloorToInt(30 * rr),
                font = MainFont,
            };
            GUIStyle DifSelDescStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.FloorToInt(16 * rr),
                font = MainFont,

            };

            if (ModSettings.FriendlyFire)
            {
                GUI.color = Color.red;

                if (GUI.Button(new Rect(Screen.width / 2 - 200 * rr, Screen.height - 120 * rr, 400 * rr, 50 * rr), "Friendly Fire enabled", new GUIStyle(GUI.skin.button)
                {
                    font = MainFont,
                    fontSize = Mathf.FloorToInt(30 * rr)
                }))
                { ModSettings.FriendlyFire = !ModSettings.FriendlyFire; }
            }
            else
            {
                GUI.color = Color.gray;
                if (GUI.Button(new Rect(Screen.width / 2 - 200 * rr, Screen.height - 120 * rr, 400 * rr, 50 * rr), "Friendly Fire disabled", new GUIStyle(GUI.skin.button)
                {
                    font = MainFont,
                    fontSize = Mathf.FloorToInt(30 * rr)
                }))
                { ModSettings.FriendlyFire = !ModSettings.FriendlyFire; }
            }

            switch (ModSettings.dropsOnDeath)
            {
                case ModSettings.DropsOnDeathMode.All:
                    GUI.color = Color.red;
                    break;
                case ModSettings.DropsOnDeathMode.Equipped:
                    GUI.color = Color.yellow;
                    break;
                case ModSettings.DropsOnDeathMode.Disabled:
                    GUI.color = Color.gray;
                    break;
                default:
                    break;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 200 * rr, Screen.height - 70 * rr, 400 * rr, 50 * rr), "Item drops on death: " + ModSettings.dropsOnDeath, new GUIStyle(GUI.skin.button)
            {
                font = MainFont,
                fontSize = Mathf.FloorToInt(30 * rr)
            }))
            {
                int i = (int)ModSettings.dropsOnDeath + 1;
                i = i % 3;
                ModSettings.dropsOnDeath = (ModSettings.DropsOnDeathMode)i;
            }

            GUI.color = Color.white;
            for (int i = 0; i < 4; i++)
            {
                int ii = i + DiffSelPage * 4;
                if (DiffSel_Names.Length > ii)
                {


                    Rect r = new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 8, Screen.height / 2);
                    r.x += i * r.width;
                    if (GUI.Button(r, "", DifficultySelectionStyle))
                    {
                        ModSettings.DifficultyChoosen = true;
                        Array values = Enum.GetValues(typeof(ModSettings.Difficulty));
                        ModSettings.difficulty = (ModSettings.Difficulty)values.GetValue(ii);
                        LocalPlayer.FpCharacter.UnLockView();
                        LocalPlayer.FpCharacter.MovementLocked = false;
                        return;
                    }
                    Rect name = new Rect(r);
                    name.height /= 5;
                    Rect desc = new Rect(r);
                    desc.height *= 0.8f;
                    desc.y += name.height;

                    GUI.Label(name, DiffSel_Names[ii], DifSelNameStyle);
                    GUI.Label(desc, DiffSel_Descriptions[ii], DifSelDescStyle);
                }
            }

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 4), "Select difficulty", DifSelNameStyle);
            if (GUI.Button(new Rect(0, Screen.height / 2 - 100 * rr, 200 * rr, 200 * rr), "Prev\nPage", DifSelNameStyle))
            {
                DiffSelPage = Mathf.Clamp(DiffSelPage - 1, 0, 2);
            }
            if (GUI.Button(new Rect(Screen.width - 200 * rr, Screen.height / 2 - 100 * rr, 200 * rr, 200 * rr), "Next\nPage", DifSelNameStyle))
            {
                DiffSelPage = Mathf.Clamp(DiffSelPage + 1, 0, 2);
            }

        }

        private void DifficultySelectionClinet()
        {
            if (LocalPlayer.FpCharacter != null)
            {
                LocalPlayer.FpCharacter.LockView(true);
                LocalPlayer.FpCharacter.MovementLocked = true;
            }
            Rect r = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(r, _black);
            GUI.Label(r, "Please wait for the host to choose a difficulty", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, font = MainFont, fontSize = Mathf.RoundToInt(50 * rr) });
            if (requestResendTime <= 0)
            {
                using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                {
                    using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                    {
                        w.Write(1);
                        w.Close();
                    }
                    Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
                    answerStream.Close();
                }
                requestResendTime = 2;
            }
            else
            {
                requestResendTime -= Time.deltaTime;
            }
        }
        #endregion

        #region InventoryMethods
        private void DrawInventory()
        {
            Rect SlotsRect = new Rect(0, 0, Inventory.Width * slotDim.x, Screen.height);
            GUI.Box(SlotsRect, "Inventory", new GUIStyle(GUI.skin.box) { font = MainFont, fontSize = Mathf.RoundToInt(65 * rr) });
            SelectedItem = -1;

            try
            {
                for (int y = 0; y < Inventory.Height; y++)
                {
                    for (int x = 0; x < Inventory.Width; x++)
                    {
                        int index = y * Inventory.Width + x;

                        DrawInvSlot(new Rect(SlotsRect.x + slotDim.x * x, SlotsRect.y + slotDim.y * y + 160 * rr + InventoryScrollAmount, slotDim.x, slotDim.y), index);
                    }
                }


                //PlayerSlots
                Rect eq = new Rect(SlotsRect.xMax + 30 * rr, 0, 420 * rr, Screen.height);
                GUI.Box(eq, "Equipment", new GUIStyle(GUI.skin.box) { font = MainFont, fontSize = Mathf.RoundToInt(65 * rr) });
                Rect head = new Rect(Vector2.zero, slotDim)
                {
                    center = eq.center
                };
                head.y -= 3.5f * head.height;

                Rect chest = new Rect(head);
                chest.y += chest.height + 50 * rr;

                Rect pants = new Rect(chest);
                pants.y += pants.height + 50 * rr;

                Rect boots = new Rect(pants);
                boots.y += boots.height + 50 * rr;

                Rect shoulders = new Rect(chest);
                shoulders.position += new Vector2(-chest.width, -chest.height / 2);

                Rect tallisman = new Rect(chest);
                tallisman.position += new Vector2(chest.width, -chest.height / 2);

                Rect gloves = new Rect(shoulders);
                gloves.y += gloves.height + 50 * rr;

                Rect bracer = new Rect(tallisman);
                bracer.y += bracer.height + 50 * rr;

                Rect ringR = new Rect(bracer);
                ringR.position += new Vector2(chest.width / 2, chest.height + 50 * rr);

                Rect ringL = new Rect(gloves);
                ringL.position += new Vector2(-chest.width / 2, chest.height + 50 * rr);

                Rect weapon = new Rect(ringL);
                weapon.y += weapon.height * 1.5f + 50 * rr;

                Rect offhand = new Rect(ringR);
                offhand.y += offhand.height * 1.5f + 50 * rr;

                DrawInvSlot(head, -2, "Head");
                DrawInvSlot(chest, -3, "Torso");
                DrawInvSlot(pants, -4, "Legs");
                DrawInvSlot(boots, -5, "Feet");
                DrawInvSlot(shoulders, -6, "Shoulders");
                DrawInvSlot(gloves, -7, "Hands");
                DrawInvSlot(tallisman, -8, "Neck");
                DrawInvSlot(bracer, -9, "Wrists");
                DrawInvSlot(ringR, -10, "Finger");
                DrawInvSlot(ringL, -11, "Finger");
                DrawInvSlot(weapon, -12, "Main hand");
                DrawInvSlot(offhand, -13, "Offhand");


                if (ModdedPlayer.instance.CraftingReroll)
                {
                        DrawCrafting(eq.xMax + 30 * rr);
                }
            }
            catch (Exception exception)
            {
                ModAPI.Log.Write(exception.ToString());
                DraggedItem = null;
                DraggedItemIndex = -1;
                isDragging = false;

            }



            if (SelectedItem != -1)
            {

                DrawItemInfo(itemPos, Inventory.Instance.ItemList[SelectedItem]);
            }

            if (isDragging)
            {
                Rect r = new Rect(mousepos, slotDim);
                GUI.color = new Color(1, 1, 1, 0.5f);
                GUI.DrawTexture(r, DraggedItem.icon);
                GUI.color = new Color(1, 1, 1, 1);

                if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    if (DraggedItem.Equipped)
                    {
                        DraggedItem.OnUnequip();
                        Inventory.Instance.ItemList[DraggedItemIndex].Equipped = false;
                    }
                    Inventory.Instance.DropItem(DraggedItemIndex);
                    CustomCrafting.ClearIndex(DraggedItemIndex);

                    DraggedItem = null;
                    DraggedItemIndex = -1;
                    isDragging = false;
                }
            }
        }

        private const float craftingbarheight = 50;
        private void DrawCrafting(float x)
        {
            Rect craftingrect = new Rect(x, 0, Mathf.Min((Screen.width - x), 400 * rr), Screen.height);
            GUI.Box(craftingrect, "");

            float btnW = craftingrect.width / 4;

            int i = 0;

            if (GUI.Button(new Rect(x + i * btnW, 0, btnW, craftingbarheight * rr), ((CustomCrafting.CraftMode)i).ToString()))
            {
                CustomCrafting.instance.craftMode = CustomCrafting.CraftMode.Rerolling;
                for (int kk = 0; kk < CustomCrafting.instance.ingredients.Length; kk++)
                {
                    CustomCrafting.instance.ingredients[kk].Clear();
                }
            }
            i++;
            if (ModdedPlayer.instance.CraftingReforge)
            {
                if (GUI.Button(new Rect(x + i * btnW, 0, btnW, craftingbarheight * rr), "Reforging"))
                {
                    CustomCrafting.instance.craftMode = CustomCrafting.CraftMode.Reforging;
                    for (int kk = 0; kk < CustomCrafting.instance.ingredients.Length; kk++)
                    {
                        CustomCrafting.instance.ingredients[kk].Clear();
                    }
                }
                i++;
            }


            switch (CustomCrafting.instance.craftMode)
            {
                case CustomCrafting.CraftMode.Rerolling:
                    DrawRerolling(x, craftingrect.width);
                    break;
                case CustomCrafting.CraftMode.Reforging:
                    if (ModdedPlayer.instance.CraftingReforge)
                        DrawReforging(x, craftingrect.width);
                    break;
                case CustomCrafting.CraftMode.Repurposing:
                    break;
                case CustomCrafting.CraftMode.Upgrading:
                    break;
            }

        }

        private void DrawReforging(float x, float w)
        {
            GUI.Label(new Rect(x, (craftingbarheight + 5) * rr, w, 26 * rr), "Item to reforge", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter, fontSize = Mathf.RoundToInt(22 * rr), font = MainFont });
            CraftingIngredientBox(new Rect(x + w / 2 - 75 * rr, (craftingbarheight + 40) * rr, 150 * rr, 150 * rr), CustomCrafting.instance.changedItem);
            float ypos = (craftingbarheight + 190) * rr;
            if (CustomCrafting.instance.changedItem.i != null)
            {
                GUIStyle StatNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(16 * rr), font = MainFont };
                GUIStyle StatValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = Mathf.RoundToInt(16 * rr), fontStyle = FontStyle.Bold, font = MainFont };

                int ind = 1;
                try
                {

                    foreach (ItemStat stat in CustomCrafting.instance.changedItem.i.Stats)
                    {
                        Rect statRect = new Rect(x + 10 * rr, ypos, w - 20 * rr, 26 * rr);
                        ypos += 26 * rr;


                        double amount = stat.Amount;
                        if (stat.DisplayAsPercent)
                        {
                            amount *= 100;
                        }
                        amount = Math.Round(amount, stat.RoundingCount);
                        GUI.color = RarityColors[stat.Rarity];
                        GUI.Label(statRect, ind + ".  " + stat.Name, StatNameStyle);
                        GUI.color = Color.white;
                        ind++;

                        if (stat.DisplayAsPercent)
                        {
                            GUI.Label(statRect, amount.ToString("N" + stat.RoundingCount) + "%", StatValueStyle);
                        }
                        else
                        {
                            GUI.Label(statRect, amount.ToString("N" + stat.RoundingCount), StatValueStyle);
                        }
                    }
                }
                catch (Exception e)
                {

                    Debug.LogException(e);
                }
                try
                {
                    if (CustomCrafting.instance.reforging.validRecipe)
                    {
                        if (GUI.Button(new Rect(x, ypos, w, 40 * rr), "Reforge item", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(30 * rr), fontStyle = FontStyle.Normal, font = MainFont }))
                        {
                            CustomCrafting.instance.reforging.PerformReforge();


                        }
                        ypos += 50 * rr;
                    }

                }
                catch (Exception e)
                {
                    Debug.LogWarning("reroll stats button ex " + e.ToString());

                }
            }
            float baseX = x + ((w - 240 * rr) / 2);
            float baseY = ypos + 30 * rr;
            if (CustomCrafting.instance.changedItem.i != null)
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        int index = 3 * k + j;
                        CraftingIngredientBox(new Rect(baseX + j * 80 * rr, baseY + k * 80 * rr, 80 * rr, 80 * rr), CustomCrafting.instance.ingredients[index]);
                    }
                }
        }
        private void DrawRerolling(float x, float w)
        {
            GUI.Label(new Rect(x, (craftingbarheight + 5) * rr, w, 26 * rr), "Item to reroll stats", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter, fontSize = Mathf.RoundToInt(22 * rr), font = MainFont });
            CraftingIngredientBox(new Rect(x + w / 2 - 75 * rr, (craftingbarheight + 40) * rr, 150 * rr, 150 * rr), CustomCrafting.instance.changedItem);
            float ypos = (craftingbarheight + 190) * rr;
            if (CustomCrafting.instance.changedItem.i != null)
            {
                GUIStyle StatNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(16 * rr), font = MainFont };
                GUIStyle StatValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = Mathf.RoundToInt(16 * rr), fontStyle = FontStyle.Bold, font = MainFont };

                int ind = 1;
                try
                {
                    Rect nameRect = new Rect(x + 10 * rr, ypos, w - 20 * rr, 30 * rr);
                    ypos += 30 * rr;
                    GUI.color = RarityColors[CustomCrafting.instance.changedItem.i.Rarity];

                    GUI.Label(nameRect, CustomCrafting.instance.changedItem.i.name, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(20 * rr), fontStyle = FontStyle.Bold, font = MainFont });

                    foreach (ItemStat stat in CustomCrafting.instance.changedItem.i.Stats)
                    {
                        Rect statRect = new Rect(x + 10 * rr, ypos, w - 20 * rr, 26 * rr);
                        ypos += 26 * rr;


                        double amount = stat.Amount;
                        if (stat.DisplayAsPercent)
                        {
                            amount *= 100;
                        }
                        amount = Math.Round(amount, stat.RoundingCount);
                        GUI.color = RarityColors[stat.Rarity];
                        GUI.Label(statRect, ind + ".  " + stat.Name, StatNameStyle);
                        GUI.color = Color.white;
                        ind++;

                        if (stat.DisplayAsPercent)
                        {
                            GUI.Label(statRect, amount.ToString("N" + stat.RoundingCount) + "%", StatValueStyle);
                        }
                        else
                        {
                            GUI.Label(statRect, amount.ToString("N" + stat.RoundingCount), StatValueStyle);
                        }
                    }
                }
                catch (Exception e)
                {

                    Debug.LogException(e);
                }
                try
                {
                    if (CustomCrafting.instance.rerolling.validRecipe)
                    {
                        if (GUI.Button(new Rect(x, ypos, w, 40 * rr), "Reroll stats", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(30 * rr), fontStyle = FontStyle.Normal, font = MainFont }))
                        {
                            CustomCrafting.instance.rerolling.PerformReroll();

                        }
                        ypos += 50 * rr;
                    }

                }
                catch (Exception e)
                {
                    Debug.LogWarning("reroll stats button ex " + e.ToString());

                }
            }
            float baseX = x + ((w - 160 * rr) / 2);
            float baseY = ypos + 30 * rr;
            if (CustomCrafting.instance.changedItem.i != null)
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        int index = 3 * k + j;
                        CraftingIngredientBox(new Rect(baseX + j * 80 * rr, baseY + k * 80 * rr, 80 * rr, 80 * rr), CustomCrafting.instance.ingredients[index]);
                    }
                }
        }

        public void CraftingIngredientBox(Rect r, CustomCrafting.CraftingIngredient ingredient)
        {
            if (ingredient.i != null)
                GUI.color = RarityColors[ingredient.i.Rarity];
            else
                GUI.color = Color.white;
            GUI.DrawTexture(r, Res.ResourceLoader.instance.LoadedTextures[12]);


            GUI.color = new Color(1, 1, 1, 1);

            if (ingredient.i != null)
            {
                Rect itemRect = new Rect(r);
                float f = r.width * 0.15f;
                itemRect.width -= f;
                itemRect.height -= f;
                itemRect.x += f / 2;
                itemRect.y += f / 2;
                GUI.DrawTexture(itemRect, ingredient.i.icon);
                if (r.Contains(mousepos))
                {
                    if (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetMouseButtonDown(1))
                    {
                        ingredient.Clear();
                        if (ingredient == CustomCrafting.instance.changedItem)
                            CustomCrafting.instance.ClearedItem();
                    }
                }
            }
            if (isDragging)
            {
                if (r.Contains(mousepos))
                {
                    if (UnityEngine.Input.GetMouseButtonUp(0))
                    {
                        if (!(CustomCrafting.instance.ingredients.Any(x => x.i == DraggedItem) || CustomCrafting.instance.changedItem.i == DraggedItem) && DraggedItemIndex > -1)
                            ingredient.Assign(DraggedItemIndex, DraggedItem);
                        DraggedItem = null;
                        DraggedItemIndex = -1;
                        isDragging = false;
                    }
                }
            }
        }
        private void DrawItemInfo(Vector2 pos, Item item)
        {
            if (item == null)
            {
                return;
            }

            float width = 390 * rr;
            Vector2 originalPos = pos;
            pos.x += 5 * rr;

            if (pos.x + width > Screen.width)
            {
                pos.x -= width + slotDim.x;
                pos.x -= 5 * rr;
            }


            Rect descriptionBox = new Rect(originalPos, new Vector2(width + 10 * rr, 500 * rr));
            Rect ItemNameRect = new Rect(pos.x, pos.y, width, 50 * rr);
            GUIStyle ItemNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, fontSize = Mathf.RoundToInt(35 * rr), fontStyle = FontStyle.Bold, font = MainFont };
            float y = 70 + pos.y;
            Rect[] StatRects = new Rect[item.Stats.Count];
            GUIStyle StatNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(20 * rr), font = MainFont };
            GUIStyle StatValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = Mathf.RoundToInt(20 * rr), fontStyle = FontStyle.Bold, font = MainFont };

            for (int i = 0; i < StatRects.Length; i++)
            {
                StatRects[i] = new Rect(pos.x, y, width, 22 * rr);
                y += 22 * rr;
            }
            y += 30 * rr;

            Rect LevelAndTypeRect = new Rect(pos.x, y, width, 35 * rr);
            GUIStyle TypeStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(14 * rr), font = MainFont };
            GUIStyle LevelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerRight, fontSize = Mathf.RoundToInt(28 * rr), font = MainFont, fontStyle = FontStyle.Italic };
            y += 30 * rr;

            GUIStyle DescriptionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(16 * rr), font = MainFont };
            GUIStyle LoreStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(13 * rr), font = MainFont, fontStyle = FontStyle.Italic };
            GUIStyle TooltipStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(15 * rr), font = MainFont, fontStyle = FontStyle.Bold };

            Rect DescrRect = new Rect(pos.x, y, width, DescriptionStyle.CalcHeight(new GUIContent(item.description), width)); y += DescrRect.height;
            y += 30 * rr;

            Rect LoreRect = new Rect(pos.x, y, width, LoreStyle.CalcHeight(new GUIContent(item.lore), width)); y += LoreRect.height;
            y += 30 * rr;

            Rect toolTipTitleRect = new Rect(pos.x, y, width, TooltipStyle.fontSize + 2); y += toolTipTitleRect.height;
            Rect toolTipRect = new Rect(pos.x, y, width, TooltipStyle.CalcHeight(new GUIContent(item.tooltip), width)); y += toolTipRect.height;
            descriptionBox.height = toolTipRect.yMax - descriptionBox.y + 5;
            if (descriptionBox.yMax > Screen.height)
            {
                float f = Screen.height - descriptionBox.yMax;
                descriptionBox.y += f;
                ItemNameRect.y += f;
                for (int i = 0; i < StatRects.Length; i++)
                {
                    StatRects[i].y += f;
                }
                LevelAndTypeRect.y += f;
                DescrRect.y += f;
                LoreRect.y += f;
                toolTipTitleRect.y += f;
                toolTipRect.y += f;
            }
            GUI.color = new Color(1, 1, 1, 0.8f);
            GUI.DrawTexture(descriptionBox, _black);
            GUI.color = RarityColors[item.Rarity];
            GUI.Label(ItemNameRect, item.name, ItemNameStyle);
            for (int i = 0; i < StatRects.Length; i++)
            {
                GUI.color = RarityColors[item.Stats[i].Rarity];
                GUI.Label(StatRects[i], item.Stats[i].Name, StatNameStyle);
                double amount = item.Stats[i].Amount;
                if (item.Stats[i].DisplayAsPercent)
                {
                    amount *= 100;
                }
                amount = Math.Round(amount, item.Stats[i].RoundingCount);

                if (item.Stats[i].DisplayAsPercent)
                {
                    GUI.Label(StatRects[i], amount.ToString("N" + item.Stats[i].RoundingCount) + "%", StatValueStyle);
                }
                else
                {
                    GUI.Label(StatRects[i], amount.ToString("N" + item.Stats[i].RoundingCount), StatValueStyle);
                }
            }
            GUI.color = Color.white;
            switch (item._itemType)
            {
                case BaseItem.ItemType.Shield:
                    GUI.Label(LevelAndTypeRect, "Shield", TypeStyle);
                    break;
                case BaseItem.ItemType.Quiver:
                    GUI.Label(LevelAndTypeRect, "Quiver", TypeStyle);
                    break;
                case BaseItem.ItemType.Weapon:
                    GUI.Label(LevelAndTypeRect, "Weapon", TypeStyle);
                    break;
                case BaseItem.ItemType.Other:
                    GUI.Label(LevelAndTypeRect, "Other", TypeStyle);
                    break;
                case BaseItem.ItemType.Material:
                    GUI.Label(LevelAndTypeRect, "Material", TypeStyle);
                    break;
                case BaseItem.ItemType.Helmet:
                    GUI.Label(LevelAndTypeRect, "Helmet", TypeStyle);
                    break;
                case BaseItem.ItemType.Boot:
                    GUI.Label(LevelAndTypeRect, "Boots", TypeStyle);
                    break;
                case BaseItem.ItemType.Pants:
                    GUI.Label(LevelAndTypeRect, "Pants", TypeStyle);
                    break;
                case BaseItem.ItemType.ChestArmor:
                    GUI.Label(LevelAndTypeRect, "Chest armor", TypeStyle);
                    break;
                case BaseItem.ItemType.ShoulderArmor:
                    GUI.Label(LevelAndTypeRect, "Shoulder armor", TypeStyle);
                    break;
                case BaseItem.ItemType.Glove:
                    GUI.Label(LevelAndTypeRect, "Gloves", TypeStyle);
                    break;
                case BaseItem.ItemType.Bracer:
                    GUI.Label(LevelAndTypeRect, "Bracers", TypeStyle);
                    break;
                case BaseItem.ItemType.Amulet:
                    GUI.Label(LevelAndTypeRect, "Amulet", TypeStyle);
                    break;
                case BaseItem.ItemType.Ring:
                    GUI.Label(LevelAndTypeRect, "Ring", TypeStyle);
                    break;
                case BaseItem.ItemType.SpellScroll:
                    GUI.Label(LevelAndTypeRect, "Scroll", TypeStyle);
                    break;
            }
            if (item.level <= ModdedPlayer.instance.Level)
            {
                //GUI.color = Color.white;
                GUI.Label(LevelAndTypeRect, "Level " + item.level, LevelStyle);
            }
            else
            {
                GUI.color = Color.red;
                GUI.Label(LevelAndTypeRect, "Level " + item.level, LevelStyle);
            }
            GUI.color = Color.white;
            GUI.Label(DescrRect, item.description, DescriptionStyle);
            GUI.Label(LoreRect, item.lore, LoreStyle);
            if (!string.IsNullOrEmpty(item.tooltip))
            {
                GUI.Label(toolTipTitleRect, "Tooltip:", TooltipStyle);
                GUI.Label(toolTipRect, item.tooltip, TooltipStyle);
            }

            //move item

        }


        private float hoveredOverID = -1;
        private float DraggedItemAlpha = 0;

        private void DrawInvSlot(Rect r, int index)
        {
            Color frameColor = Color.black;
            GUI.DrawTexture(r, Res.ResourceLoader.instance.LoadedTextures[12]);

            if (Inventory.Instance.ItemList[index] != null)
            {
                frameColor = RarityColors[Inventory.Instance.ItemList[index].Rarity];
                if (Inventory.Instance.ItemList[index].icon != null)
                {

                    Rect itemRect = new Rect(r);
                    float f = r.width * 0.15f;
                    itemRect.width -= f;
                    itemRect.height -= f;
                    itemRect.x += f / 2;
                    itemRect.y += f / 2;

                    if (DraggedItemIndex != -1)
                    {
                        if (DraggedItemIndex == index)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);

                        }
                    }
                    if (ModdedPlayer.instance.Level < Inventory.Instance.ItemList[index].level && index < -1)
                    {
                        frameColor = Color.black;
                        GUI.color = Color.black;
                    }
                    if (CustomCrafting.isIngredient(index))
                        GUI.color = new Color(0, 0, 0, 0.4f);
                    else
                        GUI.color = new Color(1, 1, 1, 1);

                    GUI.DrawTexture(itemRect, Inventory.Instance.ItemList[index].icon);
                    if (Inventory.Instance.ItemList[index].StackSize > 1)
                    {
                        GUI.color = Color.white;

                        GUI.Label(r, Inventory.Instance.ItemList[index].Amount.ToString("N0"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerLeft, fontSize = Mathf.RoundToInt(15 * rr), font = MainFont, fontStyle = FontStyle.Bold });
                    }


                    if (isDragging)
                    {
                        if (r.Contains(mousepos))
                        {

                            bool canPlace = false;
                            switch (index)
                            {
                                case -2:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Helmet)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -3:
                                    if (DraggedItem._itemType == BaseItem.ItemType.ChestArmor)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -4:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Pants)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -5:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Boot)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -6:
                                    if (DraggedItem._itemType == BaseItem.ItemType.ShoulderArmor)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -7:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Glove)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -8:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Amulet)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -9:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Bracer)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -10:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Ring)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -11:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Ring)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -12:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Weapon)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -13:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Quiver || DraggedItem._itemType == BaseItem.ItemType.SpellScroll || DraggedItem._itemType == BaseItem.ItemType.Shield)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                            }
                            if (canPlace || index > -1)
                            {
                                if (hoveredOverID == index)
                                {
                                    DraggedItemAlpha = Mathf.Clamp(DraggedItemAlpha + Time.unscaledDeltaTime / 2.5f, 0, 0.3f);
                                    GUI.color = new Color(1, 1, 1, DraggedItemAlpha);
                                    GUI.DrawTexture(itemRect, DraggedItem.icon);
                                    GUI.color = new Color(1, 1, 1, 1);

                                }
                                else
                                {
                                    DraggedItemAlpha = 0;
                                    hoveredOverID = index;
                                }
                            }
                            if (UnityEngine.Input.GetMouseButtonUp(0))
                            {
                                SelectedItem = index;
                                Effects.Sound_Effects.GlobalSFX.Play(1);

                                if (index < -1)
                                {

                                    if (canPlace)
                                    {
                                        Item backup = Inventory.Instance.ItemList[index];
                                        Inventory.Instance.ItemList[index] = DraggedItem;
                                        Inventory.Instance.ItemList[DraggedItemIndex] = backup;
                                        CustomCrafting.UpdateIndex(index, DraggedItemIndex);
                                        CustomCrafting.UpdateIndex(DraggedItemIndex, index);


                                        DraggedItem = null;
                                        DraggedItemIndex = -1;
                                        isDragging = false;
                                    }
                                    else
                                    {
                                        Inventory.Instance.ItemList[DraggedItemIndex] = DraggedItem;
                                        DraggedItem = null;
                                        DraggedItemIndex = -1;
                                    }
                                }
                                else
                                {
                                    if (DraggedItem.ID != Inventory.Instance.ItemList[index].ID
                                     || DraggedItem.Amount == DraggedItem.StackSize
                                     || Inventory.Instance.ItemList[index].Amount == Inventory.Instance.ItemList[index].StackSize
                                     || (Inventory.Instance.ItemList[index].StackSize <= 1 && DraggedItem.StackSize <= 1))
                                    {
                                        //replace items
                                        Item backup = Inventory.Instance.ItemList[index];
                                        Inventory.Instance.ItemList[index] = DraggedItem;
                                        Inventory.Instance.ItemList[DraggedItemIndex] = backup;
                                        CustomCrafting.UpdateIndex(index, DraggedItemIndex);
                                        CustomCrafting.UpdateIndex(DraggedItemIndex, index);
                                        DraggedItem = null;
                                        DraggedItemIndex = -1;
                                        isDragging = false;
                                    }
                                    else if (DraggedItemIndex != index)
                                    {
                                        //stack items
                                        int i = DraggedItem.Amount + Inventory.Instance.ItemList[index].Amount - DraggedItem.StackSize;
                                        if (i > 0)  //too much to stack completely
                                        {
                                            Inventory.Instance.ItemList[index].Amount = Inventory.Instance.ItemList[index].StackSize;
                                            Inventory.Instance.ItemList[DraggedItemIndex].Amount = i;
                                            DraggedItem = null;
                                            DraggedItemIndex = -1;
                                            isDragging = false;
                                        }
                                        else //enough to stack completely
                                        {
                                            Inventory.Instance.ItemList[index].Amount += DraggedItem.Amount;
                                            Inventory.Instance.ItemList[DraggedItemIndex] = null;

                                            CustomCrafting.ClearIndex(DraggedItemIndex);
                                            DraggedItem = null;
                                            DraggedItemIndex = -1;
                                            isDragging = false;
                                        }
                                    }
                                    else
                                    {
                                        DraggedItem = null;
                                        DraggedItemIndex = -1;
                                        isDragging = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (r.Contains(mousepos))
                        {
                            if (UnityEngine.Input.GetMouseButtonDown(0) && !UnityEngine.Input.GetKey(KeyCode.LeftShift) && !UnityEngine.Input.GetKey(KeyCode.LeftControl))
                            {
                                Effects.Sound_Effects.GlobalSFX.Play(0);

                                isDragging = true;
                                DraggedItem = Inventory.Instance.ItemList[index];
                                DraggedItemIndex = index;

                            }
                            else if (UnityEngine.Input.GetMouseButtonDown(1) && index > -1 && !UnityEngine.Input.GetKey(KeyCode.LeftShift) && !UnityEngine.Input.GetKey(KeyCode.LeftControl))
                            {
                                Effects.Sound_Effects.GlobalSFX.Play(0);

                                if (!consumedsomething)
                                {
                                    consumedsomething = true;
                                    if (Inventory.Instance.ItemList[index].OnConsume())
                                    {
                                        Inventory.Instance.ItemList[index].Amount--;
                                        if (Inventory.Instance.ItemList[index].Amount <= 0)
                                        {
                                            Inventory.Instance.ItemList[index] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (isDragging)
                {

                    if (r.Contains(mousepos))
                    {
                        if (hoveredOverID == index)
                        {
                            Rect itemRect = new Rect(r);
                            float f = r.width * 0.15f;
                            itemRect.width -= f;
                            itemRect.height -= f;
                            itemRect.x += f / 2;
                            itemRect.y += f / 2;

                            DraggedItemAlpha = Mathf.Clamp(DraggedItemAlpha + Time.unscaledDeltaTime / 2.5f, 0, 0.3f);
                            GUI.color = new Color(1, 1, 1, DraggedItemAlpha);
                            GUI.DrawTexture(itemRect, DraggedItem.icon);
                        }
                        else
                        {
                            DraggedItemAlpha = 0;
                            hoveredOverID = index;
                        }
                    }
                    GUI.color = new Color(1, 1, 1, 1);
                    if (r.Contains(mousepos) && UnityEngine.Input.GetMouseButtonUp(0))
                    {
                        isDragging = false;
                        if (index < -1)
                        {
                            bool canPlace = false;
                            switch (index)
                            {
                                case -2:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Helmet)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -3:
                                    if (DraggedItem._itemType == BaseItem.ItemType.ChestArmor)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -4:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Pants)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -5:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Boot)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -6:
                                    if (DraggedItem._itemType == BaseItem.ItemType.ShoulderArmor)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -7:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Glove)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -8:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Amulet)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -9:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Bracer)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -10:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Ring)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -11:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Ring)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -12:
                                    if (DraggedItem._itemType == BaseItem.ItemType.Weapon)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                                case -13:
                                    if (DraggedItem._itemType == BaseItem.ItemType.SpellScroll || DraggedItem._itemType == BaseItem.ItemType.Quiver || DraggedItem._itemType == BaseItem.ItemType.Shield)
                                    {
                                        canPlace = true;
                                    }

                                    break;
                            }
                            if (canPlace)
                            {
                                Inventory.Instance.ItemList[index] = DraggedItem;
                                Inventory.Instance.ItemList[DraggedItemIndex] = null;
                                CustomCrafting.UpdateIndex(DraggedItemIndex, index);
                                DraggedItem = null;
                                DraggedItemIndex = -1;
                                Effects.Sound_Effects.GlobalSFX.Play(1);

                            }
                            else
                            {
                                Inventory.Instance.ItemList[index] = null;
                                Inventory.Instance.ItemList[DraggedItemIndex] = DraggedItem;
                                DraggedItem = null;
                                DraggedItemIndex = -1;
                            }
                        }

                        else if (DraggedItemIndex != index)
                        {
                            Inventory.Instance.ItemList[index] = DraggedItem;
                            Inventory.Instance.ItemList[DraggedItemIndex] = null;
                            CustomCrafting.UpdateIndex(DraggedItemIndex, index);
                            DraggedItem = null;
                            DraggedItemIndex = -1;
                            Effects.Sound_Effects.GlobalSFX.Play(1);

                        }
                    }
                }
            }
            GUI.color = frameColor;
            GUI.DrawTexture(r, Res.ResourceLoader.instance.LoadedTextures[13]);
            GUI.color = Color.white;
            if (r.Contains(mousepos))
            {
                if (Inventory.Instance.ItemList[index] != null)
                {
                    itemPos = r.position + r.width * Vector2.right;
                    SelectedItem = index;
                }
                else
                {
                    SelectedItem = -1;
                }
            }
        }

        private void DrawInvSlot(Rect r, int index, string title)
        {


            Rect TitleR = new Rect(r.x, r.y - 35 * rr, r.width, 35 * rr);
            GUI.Label(TitleR, title, new GUIStyle(GUI.skin.box) { font = MainFont, fontSize = Mathf.RoundToInt(20 * rr), wordWrap = false, alignment = TextAnchor.MiddleCenter, clipping = TextClipping.Overflow });
            DrawInvSlot(r, index);

        }
        #endregion

        #region HUDMethods
        private void DrawHUD()
        {



            if (HideHud)
            {
                return;
            }
            GUI.color = new Color(1, 0.5f, 0.7f, 0.5f);
            GUIStyle HitmarkerStyle = new GUIStyle(GUI.skin.label) { font = MainFont, clipping = TextClipping.Overflow, wordWrap = true, alignment = TextAnchor.MiddleCenter };
            for (int i = 0; i < hitMarkers.Count; i++)
            {
                hitMarkers[i].lifetime -= Time.unscaledDeltaTime * 2;
                if (hitMarkers[i].lifetime < 0 || i - hitMarkers.Count + 10 < 0)
                {
                    hitMarkers[i].Delete(i);
                }
                else
                {
                    if (hitMarkers[i].Player)
                    {
                        GUI.color = new Color(0.25f, 1, 0.25f, 0.5f);

                    }
                    else
                    {
                        GUI.color = new Color(1, 0.25f, 0.55f, 0.5f);

                    }




                    float distance = Vector3.Distance(Camera.main.transform.position, hitMarkers[i].worldPosition);
                    Vector3 pos = Camera.main.WorldToScreenPoint(hitMarkers[i].worldPosition);
                    pos.y = Screen.height - pos.y;
                    float size = Mathf.Clamp(800 / distance, 10, 80);
                    size *= rr;
                    Rect r = new Rect(0, 0, 400, size)
                    {
                        center = pos
                    };

                    GUI.Label(r, hitMarkers[i].txt, new GUIStyle(HitmarkerStyle) { fontSize = Mathf.RoundToInt(size) });
                }

            }
            GUI.color = Color.white;


            float BuffOffsetX = 0;
            float BuffOffsetY = 1080 - BuffSize;
            const float MaxX = 540;

            if (ModdedPlayer.instance.Rooted)
            {
                TimeSpan span = TimeSpan.FromSeconds(ModdedPlayer.instance.RootDuration);
                DrawBuff(BuffOffsetX, BuffOffsetY, ResourceLoader.GetTexture(162), "", (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()), false, ModdedPlayer.instance.RootDuration);
                BuffOffsetX += BuffSize;
                if (BuffOffsetX > MaxX)
                {
                    BuffOffsetX = 0;
                    BuffOffsetY -= BuffSize;
                }
            }
            if (ModdedPlayer.instance.Stunned)
            {
                TimeSpan span = TimeSpan.FromSeconds(ModdedPlayer.instance.StunDuration);
                DrawBuff(BuffOffsetX, BuffOffsetY, ResourceLoader.GetTexture(163), "", (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()), false, ModdedPlayer.instance.StunDuration);
                BuffOffsetX += BuffSize;
                if (BuffOffsetX > MaxX)
                {
                    BuffOffsetX = 0;
                    BuffOffsetY -= BuffSize;
                }
            }
            foreach (KeyValuePair<int, Buff> buff in BuffDB.activeBuffs)
            {
                TimeSpan span = TimeSpan.FromSeconds(buff.Value.duration);
                string valueText = "";
                if (buff.Value.DisplayAmount)
                {
                    if (buff.Value.DisplayAsPercent)
                    {
                        valueText += buff.Value.amount > 0 ? ((buff.Value.amount - 1) * 100).ToString("F2") + "%" : "-" + ((buff.Value.amount - 1) * 100).ToString("F2") + "%";
                    }
                    else
                    {
                        valueText += "" + buff.Value.amount.ToString("F2");
                    }
                }
                if (valueText.EndsWith(".00") || valueText.EndsWith(",00") || valueText.EndsWith(".00%") || valueText.EndsWith(",00%"))
                {
                    valueText = valueText.TrimEnd('%').TrimEnd('0').TrimEnd(',');
                }
                DrawBuff(BuffOffsetX, BuffOffsetY, ResourceLoader.GetTexture(BuffDB.BuffsByID[buff.Value._ID].IconID), valueText, (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()), !buff.Value.isNegative, buff.Value.duration);
                BuffOffsetX += BuffSize;
                if (BuffOffsetX > MaxX)
                {
                    BuffOffsetX = 0;
                    BuffOffsetY -= BuffSize;
                }
            }

            GUI.color = Color.blue;
            GUI.Label(HUDenergyLabelRect, Mathf.Floor(LocalPlayer.Stats.Stamina).ToString("N0") + "/" + Mathf.Floor(ModdedPlayer.instance.MaxEnergy).ToString("N0"), HUDStatStyle);
            GUI.color = new Color(0.8f, 0.0f, 0.0f);

            GUI.Label(HUDHealthLabelRect, Mathf.Floor(LocalPlayer.Stats.Health).ToString("N0") + "/" + Mathf.Floor(ModdedPlayer.instance.MaxHealth).ToString("N0"), HUDStatStyle);
            if (ModdedPlayer.instance.DamageAbsorbAmount > 0)
            {

                GUI.color = new Color(1f, 0.15f, 0.8f);
                GUI.Label(HUDShieldLabelRect, Mathf.Floor(ModdedPlayer.instance.DamageAbsorbAmount).ToString("N0"), HUDStatStyle);

            }
            GUI.color = Color.white;


            float SquareSize = 45 * rr;
            for (int i = 0; i < SpellCaster.SpellCount; i++)
            {
                Rect r = new Rect(
                    Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f) + i * SquareSize,
                    Screen.height - SquareSize,
                    SquareSize,
                    SquareSize
                    );


                GUI.DrawTexture(r, _SpellBG);
                if (SpellCaster.instance.infos[i].spell == null || SpellCaster.instance.infos[i].spell.icon == null)
                {


                }
                else
                {
                    GUI.color = new Color(1, 1, 1, 0.4f);
                    GUI.color = new Color(1, 1, 1, 1f);
                    if (ModdedPlayer.instance.Silenced)
                    {
                        GUI.color = Color.black;
                    }
                    else if (!(SpellCaster.instance.Ready[i] && !ModdedPlayer.instance.Silenced && LocalPlayer.Stats.Energy >= SpellCaster.instance.infos[i].spell.EnergyCost * (1 - ModdedPlayer.instance.SpellCostToStamina) * ModdedPlayer.instance.SpellCostRatio && LocalPlayer.Stats.Stamina >= SpellCaster.instance.infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina * ModdedPlayer.instance.SpellCostRatio && SpellCaster.instance.infos[i].spell.CanCast))
                    {
                        GUI.color = Color.blue;

                    }
                    GUI.DrawTexture(r, SpellCaster.instance.infos[i].spell.icon);

                    GUI.Label(r, ModAPI.Input.GetKeyBindingAsString("spell" + (i + 1).ToString()), new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 15), fontStyle = FontStyle.Normal, alignment = TextAnchor.MiddleCenter });
                    GUI.color = Color.white;
                    if (!SpellCaster.instance.infos[i].spell.Bought)
                    {
                        SpellCaster.instance.SetSpell(i);
                    }
                    else if (!SpellCaster.instance.Ready[i])
                    {
                        Rect fillr = new Rect(r);
                        float f = SpellCaster.instance.infos[i].Cooldown / SpellCaster.instance.infos[i].spell.BaseCooldown;
                        fillr.height *= f;
                        fillr.y += SquareSize * (1 - f);
                        GUI.DrawTexture(fillr, _SpellCoolDownFill, ScaleMode.ScaleAndCrop);

                        TimeSpan span = TimeSpan.FromSeconds(SpellCaster.instance.infos[i].Cooldown);
                        string formattedTime = span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString();
                        GUI.Label(r, formattedTime, new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(rr * 20), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
                    }
                }

                GUI.DrawTexture(r, _SpellFrame);

            }
            float width = SpellCaster.SpellCount * SquareSize;
            float height = width * 0.1f;
            float combatHeight = width * 33 / 1500;
            //Defining rectangles to later use to draw HUD elements
            Rect XPbar = new Rect(Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f), Screen.height - height - SquareSize, width, height);
            Rect XPbarFill = new Rect(XPbar);
            XPbarFill.width *= ProgressBarAmount;
            Rect CombatBar = new Rect(XPbar.x, 0, SpellCaster.SpellCount * SquareSize * (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime), combatHeight);
            Rect CombatBarCount = new Rect(XPbar.x, 30 * rr, SpellCaster.SpellCount * SquareSize, combatHeight);

            float cornerDimension = Screen.height - XPbar.y;
            Rect LeftCorner = new Rect(XPbar.x - cornerDimension, XPbar.y, cornerDimension, cornerDimension);
            Rect RightCorner = new Rect(XPbar.xMax, XPbar.y, cornerDimension, cornerDimension);
            Rect CombatBarText = new Rect(CombatBarCount)
            {
                y = CombatBar.yMax + 40 * rr,
                height = 100 * rr
            };
            Rect CombatBarTimer = new Rect(CombatBar.xMax, CombatBar.y, 300, combatHeight);
            GUIStyle CombatCountStyle = new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.FloorToInt(19 * rr), alignment = TextAnchor.MiddleCenter };
            GUI.DrawTexture(XPbar, _expBarBackgroundTex, ScaleMode.ScaleToFit, true, 1500 / 150);
            GUI.DrawTextureWithTexCoords(XPbarFill, _expBarFillTex, new Rect(0, 0, ProgressBarAmount, 1));
            GUI.DrawTexture(XPbar, _expBarFrameTex, ScaleMode.ScaleToFit, true, 1500 / 150);
            GUI.DrawTexture(LeftCorner, Res.ResourceLoader.GetTexture(106));
            matrixBackup = GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(-1, 1), RightCorner.center);
            GUI.DrawTexture(RightCorner, Res.ResourceLoader.GetTexture(106));
            GUI.matrix = matrixBackup;

            if (ModdedPlayer.instance.TimeUntillMassacreReset > 0)
            {
                GUI.DrawTextureWithTexCoords(CombatBar, _combatDurationTex, new Rect(0, 0, (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime), 1));
                GUI.color = new Color(0.7f, 0.4f, 0.4f, 1f);
                if (ModdedPlayer.instance.MassacreText != "")
                {
                    GUI.DrawTexture(CombatBarText, Res.ResourceLoader.GetTexture(142));
                }

                GUI.Label(CombatBarCount, "+" + ModdedPlayer.instance.NewlyGainedExp.ToString("N0") + " EXP\tx" + ModdedPlayer.instance.MassacreMultipier, CombatCountStyle);
                GUI.color = new Color(1, 0f, 0f, (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime) + 0.2f);
                string content = ModdedPlayer.instance.MassacreText;
                if (ModdedPlayer.instance.MassacreKills > 5)
                {
                    content += "\t" + ModdedPlayer.instance.MassacreKills + " kills";
                }


                GUI.Label(CombatBarText, content, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.FloorToInt(45 * rr), alignment = TextAnchor.UpperCenter, clipping = TextClipping.Overflow, richText = true, wordWrap = false });
                GUI.color = new Color(1, 1, 1, 1f);
            }

            if (LocalPlayer.FpCharacter.crouching)
            {
                bool scanning = false;
                RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, Vector3.one * 2.5f, Camera.main.transform.forward, Camera.main.transform.rotation, 500);
                int enemyHit = -1;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("enemyCollide"))
                    {
                        enemyHit = i;
                        break;
                    }
                }
                if (enemyHit != -1)
                {

                    ScanTime += Time.unscaledDeltaTime * 1.75f;
                    if (hits[enemyHit].transform.root == scannedTransform)
                    {
                        if (BoltNetwork.isRunning && scannedEntity != null)
                        {
                            cp = EnemyManager.GetCP(scannedEntity);

                        }
                        else
                        {
                            cp = EnemyManager.GetCP(hits[enemyHit].transform.root);

                        }
                        if (cp != null && cp.Level > 0)
                        {

                            scanning = true;

                            GUIStyle infoStyle = new GUIStyle(GUI.skin.label)
                            {
                                font = MainFont,
                                fontSize = Mathf.RoundToInt(28 * rr),
                                alignment = TextAnchor.MiddleRight,
                                wordWrap = false,
                                clipping = TextClipping.Overflow,
                                richText = true,

                            };

                            Vector2 origin = wholeScreen.center;
                            origin.y -= 400 * rr;
                            origin.x += 370 * rr;
                            float y = 0;
                            DrawScannedEnemyLabel(cp.EnemyName, new Rect(origin.x, origin.y + y, 250 * rr, 66 * rr), infoStyle);
                            y += rr * 60;
                            DrawScannedEnemyLabel("Level: " + cp.Level, new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                            y += rr * 60;
                            if (ScanTime > 1.5f)
                            {
                                DrawScannedEnemyLabel(cp.Health.ToString("N0") + "/" + cp.MaxHealth.ToString("N0") + "♥", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                y += rr * 60;
                            }
                            if (ScanTime > 3f)
                            {
                                DrawScannedEnemyLabel("Armor: " + cp.Armor.ToString("N0"), new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                y += rr * 40;
                                if (cp.ArmorReduction > 0)
                                {
                                    DrawScannedEnemyLabel("Armor reduction: -" + cp.ArmorReduction.ToString("N0"), new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                    y += rr * 40;
                                }
                                y += rr * 20;
                            }
                            if (ScanTime > 4.5f)
                            {
                                DrawScannedEnemyLabel("Bounty: " + cp.ExpBounty.ToString("N0"), new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                y += rr * 85;
                            }
                            if (ScanTime > 6f)
                            {
                                if (cp.Affixes.Length > 0)
                                {

                                    DrawScannedEnemyLabel("☠️ ELITE ☠️", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(33 * rr), alignment = TextAnchor.MiddleRight });
                                    y += rr * 50;

                                    Array arr = Enum.GetValues(typeof(EnemyProgression.Abilities));
                                    foreach (int i in cp.Affixes)
                                    {
                                        EnemyProgression.Abilities ability = (EnemyProgression.Abilities)arr.GetValue(i);
                                        switch (ability)
                                        {
                                            case EnemyProgression.Abilities.Poisonous:
                                                DrawScannedEnemyLabel("Poisonous", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Steadfast:
                                                DrawScannedEnemyLabel("Steadfast", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.BossSteadfast:
                                                DrawScannedEnemyLabel("Boss Steadfast", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.EliteSteadfast:
                                                DrawScannedEnemyLabel("Elite Steadfast", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            //case EnemyProgression.Abilities.Molten:
                                            //    DrawScannedEnemyLabel("Nothing yet", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                            //    break;
                                            case EnemyProgression.Abilities.FreezingAura:
                                                DrawScannedEnemyLabel("Blizzard", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.FireAura:
                                                DrawScannedEnemyLabel("Radiance", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Rooting:
                                                DrawScannedEnemyLabel("Chains", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.BlackHole:
                                                DrawScannedEnemyLabel("Gravity manipulation", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Trapper:
                                                DrawScannedEnemyLabel("Trapper", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Juggernaut:
                                                DrawScannedEnemyLabel("Juggernaut", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Huge:
                                                DrawScannedEnemyLabel("Gargantuan", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Tiny:
                                                DrawScannedEnemyLabel("Tiny", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.ExtraDamage:
                                                DrawScannedEnemyLabel("Extra deadly", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.ExtraHealth:
                                                DrawScannedEnemyLabel("Extra tough", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Basher:
                                                DrawScannedEnemyLabel("Basher", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Blink:
                                                DrawScannedEnemyLabel("Warping", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            //case EnemyProgression.Abilities.Thunder:
                                            //    DrawScannedEnemyLabel("Nothing yet", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                            //    break;
                                            case EnemyProgression.Abilities.RainEmpowerement:
                                                DrawScannedEnemyLabel("Rain empowerment", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Shielding:
                                                DrawScannedEnemyLabel("Shielding", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Meteor:
                                                DrawScannedEnemyLabel("Meteor Rain", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Flare:
                                                DrawScannedEnemyLabel("Flare", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.DoubleLife:
                                                DrawScannedEnemyLabel("Undead", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            case EnemyProgression.Abilities.Laser:
                                                DrawScannedEnemyLabel("Plasma cannon", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                            //case EnemyProgression.Abilities.Avenger:
                                            //    DrawScannedEnemyLabel("Avenger", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                            //    break;
                                            //case EnemyProgression.Abilities.Sacrifice:
                                            //    DrawScannedEnemyLabel("Sacrifice", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                            //    break;
                                            default:
                                                DrawScannedEnemyLabel(ability.ToString(), new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                break;
                                        }
                                        y += rr * 50;

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        cp = null;

                        scannedTransform = hits[enemyHit].transform.root;
                        scannedEntity = scannedTransform.GetComponentInChildren<BoltEntity>();
                        if (scannedEntity == null)
                        {
                            scannedEntity = scannedTransform.GetComponent<BoltEntity>();
                        }
                        ScanTime = 0;

                    }
                }

                if (scanning)
                {
                    Rect scanRect = new Rect(0, 0, 60 * rr, 60 * rr)
                    {
                        center = wholeScreen.center
                    };
                    ScanRotation += Time.deltaTime * 45;
                    Matrix4x4 matrix4X4 = GUI.matrix;
                    GUIUtility.RotateAroundPivot(ScanRotation, scanRect.center);
                    GUI.DrawTexture(scanRect, ResourceLoader.instance.LoadedTextures[24]);

                    GUI.matrix = matrix4X4;
                }
                else
                {
                    Rect scanRect = new Rect(0, 0, 20 * rr, 20 * rr)
                    {
                        center = wholeScreen.center
                    };
                    //ScanRotation += Time.deltaTime * 45;
                    GUI.DrawTexture(scanRect, ResourceLoader.instance.LoadedTextures[24]);

                }
            }
            else
            {
                ScanTime = 0;

            }



            PingGUIDraw();


            if (LevelUpDuration > 0)
            {
                float y = Mathf.Cos(Mathf.PI * (LevelUpDuration / (2 * lvlUpDuration))) * 50 * rr;
                //Level up icon
                Rect r = new Rect(710 * rr, y, 500 * rr, 300 * rr);

                float opacity = 1;
                if (LevelUpDuration < lvlUpFadeDuration)
                {
                    opacity = Mathf.Sin(LevelUpDuration * Mathf.PI / (2 * lvlUpFadeDuration));
                }
                else if (LevelUpDuration > lvlUpDuration - lvlUpFadeDuration)
                {
                    opacity = Mathf.Sin((lvlUpDuration - LevelUpDuration) * Mathf.PI / (2 * lvlUpFadeDuration));
                }
                GUI.color = new Color(1, 1, 1, opacity);
                GUI.DrawTexture(r, ResourceLoader.GetTexture(164));
                GUI.Label(r, LevelUpText, new GUIStyle(GUI.skin.label) { font = MainFont, clipping = TextClipping.Overflow, wordWrap = true, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(46 * rr), });
                GUI.color = new Color(1, 1, 1, 1);

            }
        }
        ClinetEnemyProgression cp = null;

        bool pingBlocked = false;
        void UnlockPing()
        {
            pingBlocked = false;
        }
        public void LockPing()
        {
            pingBlocked = true;
            Invoke("UnlockPing", 1f);

        }

        //called at update, shows or hides ping menu, and casts ping commands
        private void PingUpdate()
        {



            try
            {
                if (otherPlayerPings != null)
                {
                    string toRem = null;
                    foreach (var item in otherPlayerPings)
                    {
                        if (item.Value.pingType == MarkObject.PingType.Enemy)
                        {
                            if (((MarkEnemy)item.Value).Outdated())
                            {
                                //remove ping
                                toRem = item.Key;
                                break;
                            }

                        }
                        else if (item.Value.pingType == MarkObject.PingType.Location)
                        {
                            if (((MarkPostion)item.Value).Outdated())
                            {
                                //remove ping
                                toRem = item.Key;
                                break;
                            }
                        }
                        else if (item.Value.pingType == MarkObject.PingType.Item)
                        {
                            if (((MarkPickup)item.Value).Outdated())
                            {
                                //remove ping
                                toRem = item.Key;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(toRem))
                        otherPlayerPings.Remove(toRem);
                }
                    if (localPlayerPing != null)
                    {
                        if (localPlayerPing.pingType == MarkObject.PingType.Enemy)
                        {
                            if (((MarkEnemy)localPlayerPing).Outdated())
                            {
                                localPlayerPing = null;

                            }

                        }
                        else if (localPlayerPing.pingType == MarkObject.PingType.Location)
                        {
                            if (((MarkPostion)localPlayerPing).Outdated())
                            {
                                localPlayerPing = null;

                            }
                        }
                        else if (localPlayerPing.pingType == MarkObject.PingType.Item)
                        {
                            if (((MarkPickup)localPlayerPing).Outdated())
                            {
                                localPlayerPing = null;

                            }
                        }
                    }
                

            }
            catch (Exception exc)
            {
                ModAPI.Log.Write(exc.ToString());
            }
          
            drawPingPreview = false;
            if (!pingBlocked && (ModAPI.Input.GetButton("ping")|| UnityEngine.Input.GetMouseButton(2)))
            {
                if (localPlayerPing != null)
                {
                    SendClearMyPing();
                    localPlayerPing = null;
                    LockPing();
                    return;
                }

                //is holding middle mouse btn
                if (Physics.Raycast(Cam.position  +Cam.forward, Cam.forward, out RaycastHit hit, 200))
                {

                    drawPingPreview = true;
                    previewPingPos = hit.point;
                    previewPingDist = hit.distance;
                    if (ScanTime > 0)
                    {
                        previewPingType = MarkObject.PingType.Enemy;
                        previewPingPos = scannedTransform.position+Vector3.up;

                        if (UnityEngine.Input.GetMouseButtonDown(1))
                        {
                            LockPing();
                            if (GameSetup.IsMultiplayer)
                            {
                                if (cp != null && cp.Level > 0)
                                {
                                    if (GameSetup.IsMpClient)
                                    {

                                        using (MemoryStream answerStream = new MemoryStream())
                                        {
                                            using (BinaryWriter w = new BinaryWriter(answerStream))
                                            {
                                                w.Write(37);
                                                w.Write(ModReferences.ThisPlayerID);
                                                w.Write(scannedEntity.networkId.PackedValue);
                                                w.Close();
                                            }
                                            NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
                                            answerStream.Close();
                                        }
                                    }
                                    else
                                    {
                                        if (EnemyManager.hostDictionary.ContainsKey(scannedEntity.networkId.PackedValue))
                                        {
                                            using (MemoryStream answerStream = new MemoryStream())
                                            {
                                                using (BinaryWriter w = new BinaryWriter(answerStream))
                                                {
                                                    w.Write(36);
                                                    w.Write(ModReferences.ThisPlayerID);
                                                    w.Write(0);
                                                    w.Write(scannedEntity.networkId.PackedValue);
                                                    w.Write(cp.Affixes.Length > 0);
                                                    w.Write(cp.EnemyName);
                                                    w.Close();
                                                }
                                                NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
                                                answerStream.Close();
                                            }
                                            localPlayerPing = new MarkEnemy(scannedTransform, cp.EnemyName, cp.Affixes.Length > 0);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                   var enemy = hit.transform.root.GetComponent<EnemyProgression>();
                                if(enemy == null)
                                {
                                enemy = hit.transform.root.GetComponentInChildren<EnemyProgression>();
                                }
                                if (enemy != null)
                                    localPlayerPing = new MarkEnemy(enemy.transform, enemy.EnemyName, enemy._rarity != EnemyProgression.EnemyRarity.Normal);
                                else
                                {
                                    localPlayerPing = new MarkEnemy(enemy.transform, "Enemy", false);
                                }
                            }
                        }
                    }


                    else if (hit.transform.CompareTag("enemyCollide"))
                    {
                        //indicate pinging enemyCollide
                        previewPingType = MarkObject.PingType.Enemy;
                        if (UnityEngine.Input.GetMouseButtonDown(1))
                        {
                            LockPing();
                            if (GameSetup.IsMultiplayer)
                            {
                                var entity = hit.transform.GetComponentInParent<BoltEntity>();
                                if (entity != null)
                                {
                                    if (GameSetup.IsMpClient)
                                    {

                                        using (MemoryStream answerStream = new MemoryStream())
                                        {
                                            using (BinaryWriter w = new BinaryWriter(answerStream))
                                            {
                                                w.Write(37);
                                                w.Write(ModReferences.ThisPlayerID);
                                                w.Write(entity.networkId.PackedValue);
                                                w.Close();
                                            }
                                            NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
                                            answerStream.Close();
                                        }
                                    }
                                    else
                                    {
                                        if (EnemyManager.hostDictionary.ContainsKey(entity.networkId.PackedValue))
                                        {
                                            var enemy = EnemyManager.hostDictionary[entity.networkId.PackedValue];
                                            using (MemoryStream answerStream = new MemoryStream())
                                            {
                                                using (BinaryWriter w = new BinaryWriter(answerStream))
                                                {
                                                    w.Write(36);
                                                    w.Write(ModReferences.ThisPlayerID);
                                                    w.Write(0);
                                                    w.Write(entity.networkId.PackedValue);
                                                    w.Write(enemy._rarity != EnemyProgression.EnemyRarity.Normal);
                                                    w.Write(enemy.EnemyName);
                                                    w.Close();
                                                }
                                                NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
                                                answerStream.Close();
                                            }
                                            localPlayerPing = new MarkEnemy(enemy.transform, enemy.EnemyName, enemy._rarity != EnemyProgression.EnemyRarity.Normal);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cp != null)
                                {
                                    localPlayerPing = new MarkEnemy(scannedTransform, cp.EnemyName, cp.Affixes.Length > 0);
                                }
                                else
                                {
                                    ModAPI.Console.Write("Cp is null");
                                }
                            }
                        }


                    }
                    else
                    {
                        ModAPI.Console.Write(hit.transform.tag);
                        var pu = hit.transform.GetComponent<ItemPickUp>();
                        if (pu != null)
                        {
                            //pickup marker
                            previewPingType = MarkObject.PingType.Item;
                            if (UnityEngine.Input.GetMouseButtonDown(1))
                            {
                                LockPing();
                                if (GameSetup.IsMultiplayer)
                                {

                                    using (MemoryStream answerStream = new MemoryStream())
                                    {
                                        using (BinaryWriter w = new BinaryWriter(answerStream))
                                        {
                                            w.Write(36);
                                            w.Write(ModReferences.ThisPlayerID);
                                            w.Write(2);
                                            w.Write(pu.ID);
                                            w.Close();
                                        }
                                        NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
                                        answerStream.Close();
                                    }
                                    localPlayerPing = new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity);

                                }
                                else
                                {
                                    localPlayerPing = new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity);

                                }
                            }

                        }
                        else
                        {
                            //location marker
                            previewPingType = MarkObject.PingType.Location;
                            if (UnityEngine.Input.GetMouseButtonDown(1))
                            {
                                LockPing();
                                if (GameSetup.IsMultiplayer)
                                {

                                    using (MemoryStream answerStream = new MemoryStream())
                                    {
                                        using (BinaryWriter w = new BinaryWriter(answerStream))
                                        {
                                            w.Write(36);
                                            w.Write(ModReferences.ThisPlayerID);
                                            w.Write(1);
                                            w.Write(hit.point.x);
                                            w.Write(hit.point.y + 1);
                                            w.Write(hit.point.z);
                                            w.Close();
                                        }
                                        NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
                                        answerStream.Close();
                                    }
                                    localPlayerPing = new MarkPostion(hit.point + Vector3.up);

                                }
                                else
                                {
                                    localPlayerPing = new MarkPostion(hit.point + Vector3.up);

                                }
                            }
                        }
                    }
                }

            }
           
        }


        void SendClearMyPing()
        {
            if (GameSetup.IsMultiplayer)
            {
                using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                {
                    using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                    {
                        w.Write(19);
                        w.Write(ModReferences.ThisPlayerID);
                        w.Write(ModdedPlayer.instance.Level);
                        w.Close();
                    }
                    ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                    answerStream.Close();
                }
            }
        }

        bool drawPingPreview;
        Vector3 previewPingPos;
        MarkObject.PingType previewPingType;
        float previewPingDist;
        void PingGUIDraw()
        {
            if (otherPlayerPings != null)
            {
                foreach (var item in otherPlayerPings.Values)
                {
                    if (item.pingType == MarkObject.PingType.Enemy)
                    {
                        ((MarkEnemy)item).Draw();

                    }
                    else if (item.pingType == MarkObject.PingType.Location)
                    {
                        ((MarkPostion)item).Draw();

                    }
                    else if (item.pingType == MarkObject.PingType.Item)
                    {
                        ((MarkPickup)item).Draw();

                    }
                }
            }
            if (localPlayerPing != null)
            {
                if (localPlayerPing != null)
                {
                    if (localPlayerPing.pingType == MarkObject.PingType.Enemy)
                    {
                        ((MarkEnemy)localPlayerPing).Draw();

                    }
                    else if (localPlayerPing.pingType == MarkObject.PingType.Location)
                    {
                        ((MarkPostion)localPlayerPing).Draw();

                    }
                    else if (localPlayerPing.pingType == MarkObject.PingType.Item)
                    {
                        ((MarkPickup)localPlayerPing).Draw();

                    }
                }
            }
            else if (drawPingPreview)
            {
                try
                {


                    Vector3 pos = Camera.main.WorldToScreenPoint(previewPingPos);
                    pos.y = Screen.height - pos.y;
                    float size = Mathf.Clamp(700 / previewPingDist, 10, 40);
                    size *= rr;
                    Rect r = previewPingType != MarkObject.PingType.Item ?
                        new Rect(0, 0, size * 3.34f, size)
                        {
                            center = pos
                        } :
                        new Rect(0, 0, size * 1.3f, size * 2.4f)
                        {
                            center = pos
                        };
                    
                    GUI.color = new Color(1, 1, 1, 0.35f);
                    switch (previewPingType)
                    {
                        case MarkObject.PingType.Enemy:
                            GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(172));
                            break;
                        case MarkObject.PingType.Location:
                            GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(173));
                            break;
                        case MarkObject.PingType.Item:
                            GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(174));
                            break;
                        default:
                            break;
                    }

                    GUI.color = Color.white;
                    GUI.Label(new Rect(Screen.width/2 - 300*rr,  10*rr, 1000*rr, 100), "Right click to place marker. When placed, press middle mouse or ping key to clear", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, clipping = TextClipping.Overflow, fontStyle = FontStyle.Italic, fontSize = (int)(15f * rr), font = MainFont });
                }
                catch (Exception e)
                {

                    ModAPI.Console.Write(e.ToString());
                }
            }

        }

        public Dictionary<string, MarkObject> otherPlayerPings;
        public MarkObject localPlayerPing;


        private const float lvlUpDuration = 3;
        private const float lvlUpFadeDuration = 1;
        public float LevelUpDuration;
        public string LevelUpText;
        public AudioSource lvlUpAudio;
        public void LevelUpAction()
        {
            LevelUpDuration = lvlUpDuration;
            LevelUpText = "\n" + ModdedPlayer.instance.Level.ToString();

            ///TODO play some audio upon leveling up
            ///
            if (lvlUpAudio == null)
            {
                lvlUpAudio = new GameObject("LvlupAudio").AddComponent<AudioSource>();
                lvlUpAudio.clip = Res.ResourceLoader.instance.LoadedAudio[1001];
                lvlUpAudio.rolloffMode = AudioRolloffMode.Linear;
                lvlUpAudio.maxDistance = 1000;
                lvlUpAudio.transform.position = LocalPlayer.Transform.position;
                lvlUpAudio.Play();
            }
            else
            {
                lvlUpAudio.transform.position = LocalPlayer.Transform.position;
                lvlUpAudio.Play();
            }


        }




        private void DrawScannedEnemyLabel(string content, Rect r, GUIStyle style)
        {
            GUI.DrawTexture(r, Res.ResourceLoader.instance.LoadedTextures[25]);
            Rect rOffset = new Rect(r);
            rOffset.x -= 30;
            GUI.Label(rOffset, content, style);

        }
        #endregion

        //Does fade out fade in transision between menus
        public IEnumerator FadeMenuSwitch(OpenedMenuMode mode)
        {
            MenuInteractable = false;
            float alpha = 0;
            while (alpha < 1)
            {
                alpha = Mathf.Clamp(alpha + Time.unscaledDeltaTime * DarkeningSpeed, 0, 1);
                _blackTexture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
                _blackTexture.Apply();
                yield return null;
            }

            _openedMenu = mode;
            if (mode == OpenedMenuMode.Hud)
            {
                LocalPlayer.FpCharacter.UnLockView();
                //LocalPlayer.Inventory.EquipPreviousWeapon();
            }
            yield return null;
            while (alpha > 0)
            {
                alpha = Mathf.Clamp(alpha - Time.unscaledDeltaTime * DarkeningSpeed, 0, 1);
                _blackTexture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
                _blackTexture.Apply();
                yield return null;
            }
            MenuInteractable = true;
        }

        #region SpellsMethods;
        private Spell displayedSpellInfo;
        private Texture2D semiBlack;
        private float semiblackValue;
        private float spellOffset;

        private void DrawSpellMenu()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 41), alignment = TextAnchor.MiddleLeft };
            float y = spellOffset;
            for (int i = 0; i < SpellDataBase.SortedSpellIDs.Length; i++)
            {
                Spell spell = SpellDataBase.spellDictionary[SpellDataBase.SortedSpellIDs[i]];
                DrawSpell(ref y, spell, new GUIStyle(style));
            }
            if (displayedSpellInfo == null)
            {
                //scrolling the list
                spellOffset += UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 160;
                spellOffset = Mathf.Clamp(spellOffset, -(100 * rr * (SpellDataBase.spellDictionary.Count + 4)), 0);
            }
            else
            {
                //background effect

                semiblackValue += Time.unscaledDeltaTime / 5;
                semiBlack.SetPixel(0, 0, new Color(0.6f, 0.16f, 0, 0.6f + Mathf.Sin(semiblackValue * Mathf.PI) * 0.2f));
                semiBlack.Apply();
                GUI.DrawTexture(wholeScreen, semiBlack);
                Rect bg = new Rect(Screen.width / 2 - 325 * rr, 0, 650 * rr, Screen.height);
                GUI.DrawTexture(bg, Res.ResourceLoader.instance.LoadedTextures[27]);

                //go back btn
                if (UnityEngine.Input.GetMouseButtonDown(1))
                {
                    displayedSpellInfo = null;
                    Effects.Sound_Effects.GlobalSFX.Play(1);

                    return;
                }

                //drawing pretty info
                Rect SpellIcon = new Rect(Screen.width / 2 - 100 * rr, 25 * rr, 200 * rr, 200 * rr);
                GUI.DrawTexture(SpellIcon, displayedSpellInfo.icon);
                if (GUI.Button(new Rect(Screen.width - 170 * rr, 25 * rr, 150 * rr, 50 * rr), "GO BACK TO SPELLS\n(Right Mouse Button)", new GUIStyle(GUI.skin.button) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 15) }))
                {
                    displayedSpellInfo = null;
                    Effects.Sound_Effects.GlobalSFX.Play(1);
                    return;

                }

                GUI.Label(new Rect(Screen.width / 2 - 300 * rr, 225 * rr, 600 * rr, 70 * rr),
                    displayedSpellInfo.Name,
                    new GUIStyle(GUI.skin.label)
                    {
                        font = MainFont,
                        fontSize = Mathf.RoundToInt(rr * 50),
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleCenter
                    });
                GUI.DrawTexture(new Rect(Screen.width / 2 - 150 * rr, 325 * rr, 300 * rr, 35 * rr), Res.ResourceLoader.instance.LoadedTextures[30]);

                GUI.Label(new Rect(Screen.width / 2 - 300 * rr, 370 * rr, 600 * rr, 400 * rr),
                    displayedSpellInfo.Description + (displayedSpellInfo.EnergyCost > 0 ? "\nEnergy cost:  " + displayedSpellInfo.EnergyCost : "") +
                    "\nRequired level:  " + displayedSpellInfo.Levelrequirement,
                    new GUIStyle(GUI.skin.label)
                    {
                        font = MainFont,
                        fontSize = Mathf.RoundToInt(rr * 29),
                        fontStyle = FontStyle.Normal,
                        alignment = TextAnchor.MiddleCenter
                    });

                if (displayedSpellInfo.Bought)
                {
                    //select equip slot
                    for (int i = 0; i < SpellCaster.instance.infos.Length; i++)
                    {
                        try
                        {
                            Rect btn = new Rect(bg.x + 25f * rr + i * 100 * rr, 800 * rr, 100 * rr, 100 * rr);

                            GUI.DrawTexture(btn, Res.ResourceLoader.instance.LoadedTextures[5]);

                            if (displayedSpellInfo.EquippedSlot == i)
                            {
                                GUI.DrawTexture(btn, displayedSpellInfo.icon);
                                if (GUI.Button(btn, "•", new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 17), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter }))
                                {
                                    //Clears the spot
                                    Effects.Sound_Effects.GlobalSFX.Play(1);

                                    SpellCaster.instance.SetSpell(i);
                                }
                            }
                            else
                            {
                                if (SpellCaster.instance.infos[i].spell != null)
                                {
                                    GUI.DrawTexture(btn, SpellCaster.instance.infos[i].spell.icon);
                                    if (GUI.Button(btn, SpellCaster.instance.infos[i].spell.Name, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 17), alignment = TextAnchor.MiddleCenter }))
                                    {
                                        //Replaces spell
                                        if (displayedSpellInfo.IsEquipped)
                                        {
                                            SpellCaster.instance.SetSpell(displayedSpellInfo.EquippedSlot);
                                        }

                                        SpellCaster.instance.SetSpell(i, displayedSpellInfo);
                                        Effects.Sound_Effects.GlobalSFX.Play(0);

                                    }
                                }
                                else
                                {
                                    if (GUI.Button(btn, "", new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 34), alignment = TextAnchor.MiddleCenter }))
                                    {
                                        //Assigns spell onto empty slot
                                        if (displayedSpellInfo.IsEquipped)
                                        {
                                            SpellCaster.instance.SetSpell(displayedSpellInfo.EquippedSlot);
                                        }
                                        Effects.Sound_Effects.GlobalSFX.Play(0);
                                        SpellCaster.instance.SetSpell(i, displayedSpellInfo);
                                    }
                                }
                            }
                            GUI.color = new Color(0.7f, 0.7f, 0.7f);
                            GUI.Label(btn, ModAPI.Input.GetKeyBindingAsString("spell" + (i + 1).ToString()), new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 45), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
                            GUI.color = Color.white;
                            GUI.DrawTexture(btn, Res.ResourceLoader.instance.LoadedTextures[6]);
                        }
                        catch (Exception ex)
                        {
                            ModAPI.Log.Write(ex.ToString());
                        }
                    }
                }
                else
                {
                    //buy button
                    Rect UnlockRect = new Rect(bg.x + 150 * rr, 800 * rr, bg.width - 300 * rr, 100 * rr);
                    if (displayedSpellInfo.Levelrequirement <= ModdedPlayer.instance.Level)
                    {
                        if (ModdedPlayer.instance.MutationPoints >= 2)
                        {
                            GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { font = MainFont, fontSize = Mathf.RoundToInt(41 * rr), fontStyle = FontStyle.Bold };
                            btnStyle.onActive.textColor = Color.blue;
                            btnStyle.onNormal.textColor = Color.gray;
                            if (GUI.Button(UnlockRect, "Unlock", btnStyle))
                            {
                                displayedSpellInfo.Bought = true;
                                ModdedPlayer.instance.MutationPoints -= 2;
                                Effects.Sound_Effects.GlobalSFX.Play(6);

                            }
                        }
                        else
                        {
                            GUIStyle morePointsStyle = new GUIStyle(GUI.skin.label) { font = MainFont, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(41 * rr), fontStyle = FontStyle.Bold };
                            morePointsStyle.onNormal.textColor = Color.gray;
                            morePointsStyle.onActive.textColor = Color.white;
                            GUI.Label(UnlockRect, "You need 2 points to unlock an ability", morePointsStyle);
                        }
                    }
                    else
                    {
                        GUIStyle moreLevelsStyle = new GUIStyle(GUI.skin.label) { font = MainFont, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(41 * rr), fontStyle = FontStyle.Bold };
                        moreLevelsStyle.onNormal.textColor = Color.gray;
                        moreLevelsStyle.onActive.textColor = Color.white;
                        GUI.Label(UnlockRect, "YOUR LEVEL IS TOO LOW TO UNLOCK", moreLevelsStyle);
                    }
                }
                GUI.color = Color.white;

            }

        }

        private void DrawSpell(ref float y, Spell s, GUIStyle style)
        {
            Rect bg = new Rect(0, y, Screen.width * 3.3f / 5, 100 * rr);
            bg.x = (Screen.width - bg.width) / 2;
            GUI.DrawTexture(bg, Res.ResourceLoader.instance.LoadedTextures[28]);
            Rect nameRect = new Rect(30 * rr + bg.x, y, bg.width / 2, 100 * rr);
            bool locked = false;
            if (s.Levelrequirement > ModdedPlayer.instance.Level)
            {
                locked = true;
                GUI.color = Color.black;
            }
            else
            {
                if(!s.Bought)
                GUI.color = Color.gray;
                if (bg.Contains(mousepos))
                {
                    if (displayedSpellInfo == null)
                    {
                        style.fontStyle = FontStyle.Bold;
                        if (UnityEngine.Input.GetMouseButtonDown(0))
                        {
                            Effects.Sound_Effects.GlobalSFX.Play(0);

                            displayedSpellInfo = SpellDataBase.spellDictionary[s.ID];
                        }
                    }
                }
            }
            if(locked)
            GUI.Label(nameRect,"Unlocked at level "+ s.Levelrequirement, style);

                else
            GUI.Label(nameRect, s.Name, style);
            Rect iconRect = new Rect(bg.xMax - 140 * rr, y + 15 * rr, 70 * rr, 70 * rr);
            GUI.DrawTexture(iconRect, s.icon);

            GUI.color = Color.white;
            y += 100 * rr;

        }


        #endregion


        #region StatsMenu

        private float BookPositionY;
        private float BookScrollAmount;
        private GUIStyle headerstyle;
        private GUIStyle statStyle;
        private GUIStyle statStyleAmount;
        private GUIStyle statStyleTooltip;
        private GUIStyle TextLabel;
        public struct Bookmark
        {
            public float position;
            public string name;
        }
        public List<Bookmark> Bookmarks = new List<Bookmark>();

        private void Header(string s)
        {
            if (BookPositionY < Screen.height && BookPositionY > -140 * rr)
            {
                Rect labelRect = new Rect(GuideWidthDecrease * rr + GuideMargin * rr, BookPositionY, Screen.width - 2 * rr * (GuideMargin + GuideWidthDecrease), 70 * rr);
                GUI.Label(labelRect, s, headerstyle);
                BookPositionY += 70 * rr;
                Rect imageRect = new Rect(400 * rr, BookPositionY, Screen.width - 800 * rr, 60 * rr);
                GUI.DrawTexture(imageRect, ResourceLoader.GetTexture(30));
                BookPositionY += 70 * rr;
            }
            else
            {
                BookPositionY += 140 * rr;
            }
        }

        private void Space(float pixelsUnscaled)
        {
            BookPositionY += pixelsUnscaled * rr;
        }
        private void Stat(string statName, string amount, string tooltip = "")
        {
            if (BookPositionY < Screen.height && BookPositionY > -70 * rr)
            {
                Rect labelRect = new Rect(100 * rr + GuideWidthDecrease * rr + GuideMargin * rr, BookPositionY, Screen.width - 2 * rr * (GuideMargin + GuideWidthDecrease) - 200 * rr, statStyle.fontSize);
                GUI.Label(labelRect, statName, statStyle);
                GUI.Label(labelRect, amount, statStyleAmount);
                BookPositionY += statStyle.fontSize;
                if (labelRect.Contains(mousepos) && tooltip != "")
                {
                    float h = statStyleTooltip.CalcHeight(new GUIContent(tooltip), Screen.width - 500 * rr);
                    Rect tooltipRect = new Rect(GuideWidthDecrease * rr + 150 * rr, BookPositionY, Screen.width - 500 * rr, h);
                    GUI.Label(tooltipRect, tooltip, statStyleTooltip);
                    BookPositionY += h;

                }
                BookPositionY += 5 * rr;
            }
            else
            {
                float h = statStyleTooltip.CalcHeight(new GUIContent(tooltip), Screen.width - 500 * rr);
                BookPositionY += 5 * rr + h;
                BookPositionY += statStyle.fontSize;
            }
        }
        private void Label(string s)
        {
            float h = TextLabel.CalcHeight(new GUIContent(s), Screen.width - 500 * rr);

            if (BookPositionY < Screen.height && BookPositionY > -h * rr)
            {
                Rect rect = new Rect(GuideWidthDecrease * rr + GuideMargin * rr, BookPositionY, Screen.width - 2 * rr * (GuideMargin + GuideWidthDecrease), h);
                GUI.Label(rect, s, TextLabel);
                BookPositionY += h;
            }
            else
            {
                BookPositionY += h;
            }
        }
        private void Image(int iconID, float height, float centerPosition = 0.5f)
        {
            height *= rr;
            if (BookPositionY < Screen.height && BookPositionY > -height * rr)
            {

                Texture2D tex = Res.ResourceLoader.GetTexture(iconID);
                Rect rect = new Rect(0, 0, height * tex.width / tex.height, height)
                {
                    center = new Vector2(Screen.width * centerPosition, height / 2 + BookPositionY)
                };
                BookPositionY += rect.height;
                GUI.DrawTexture(rect, tex);
            }
            else
            {
                BookPositionY += height;
            }
        }
        private void MarkBookmark(string s)
        {
            Bookmarks.Add(new Bookmark() { name = s, position = BookPositionY });
        }

        private void SetGuiStylesForGuide()
        {
            headerstyle = new GUIStyle(GUI.skin.label)
            {
                font = MainFont,
                fontSize = Mathf.RoundToInt(70 * rr),
                hover = new GUIStyleState()
                {
                    textColor = new Color(1, 1, 0.8f)
                },
                alignment = TextAnchor.UpperCenter,
                richText = true,
            };
            statStyle = new GUIStyle(GUI.skin.label)
            {
                font = MainFont,
                fontSize = Mathf.RoundToInt(24 * rr),
                hover = new GUIStyleState()
                {
                    textColor = new Color(1, 1, 0.8f)
                },
                alignment = TextAnchor.MiddleLeft,
                richText = true,
            };
            statStyleAmount = new GUIStyle(GUI.skin.label)
            {
                font = MainFont,
                fontSize = Mathf.RoundToInt(25 * rr),
                normal = new GUIStyleState()
                {
                    textColor = new Color(0.3f, 1, 0.3f)
                },
                hover = new GUIStyleState()
                {
                    textColor = new Color(0, 1, 0.1f)
                },
                alignment = TextAnchor.MiddleRight,
                richText = true,
            };
            int margin = Mathf.RoundToInt(25 * rr);
            statStyleTooltip = new GUIStyle(GUI.skin.label)
            {
                font = MainFont,
                fontSize = Mathf.RoundToInt(28 * rr),
                fontStyle = FontStyle.Italic,
                margin = new RectOffset(margin, margin, margin, margin),
                stretchWidth = true,
                alignment = TextAnchor.UpperLeft,
                richText = true,
            };
            TextLabel = new GUIStyle(GUI.skin.label)
            {
                font = MainFont,
                fontSize = Mathf.RoundToInt(35 * rr),
                stretchWidth = true,
                alignment = TextAnchor.UpperLeft,
                richText = true,
                hover = new GUIStyleState()
                {
                    textColor = new Color(1, 1, 0.8f)
                },
            };
        }

        private void DrawGuide()
        {
            Bookmarks.Clear();
            BookScrollAmount = Mathf.Clamp(BookScrollAmount + 200 * rr * UnityEngine.Input.GetAxis("Mouse ScrollWheel"), -Screen.height * 20 * rr, 0);
            BookPositionY = BookScrollAmount;
            SetGuiStylesForGuide();

            Header("Basic Information");
            MarkBookmark("Home");
            Label("\tExperience");
            Stat("Current level", ModdedPlayer.instance.Level.ToString("N0"));
            Stat("Current experience", ModdedPlayer.instance.ExpCurrent.ToString("N0"));
            Stat("Experience goal", ModdedPlayer.instance.ExpGoal.ToString("N0"), "Next level: " + (ModdedPlayer.instance.Level + 1) + " you will need to get this amount of experience:\t " + ModdedPlayer.instance.GetGoalExp(ModdedPlayer.instance.Level + 1).ToString("N0"));
            Stat("Progress amount: ", (((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal) * 100).ToString() + "%");
            Label("\tLevel is the estimation of my power. I must become stronger to survive." +
                "\nHigher level allow me to equip better equipement. " +
                "\nLeveling up gives me the ability to develop usefull abilities. (Currently you have " + ModdedPlayer.instance.MutationPoints + " mutation points), which you can spend on unlocking spells or perks. ");
            Space(50);
            Label("\nSources of experience" +
                "\n-Mutants - Enemies give the most experience, it's possible to chain kills to get more exp, and the reward is exp reward is based on bounty." +
                "\n-Animals - Experience gained does not increase with difficulty. It's only viable for levels <10" +
                "\n-Tall bushes - Give minium amount of experience. Not viable after level 6" +
                "\n-Trees - Gives little experience for every tree chopped down." +
                "\n-Effigies - It's possible to gain experience and low rarity items by breaking effigies scattered across the map." +
                "\n-Rare consumable - Gives a large amount of experience, it's rarity is orange\n");

            Space(100);
            
            Header("Information - Items");
            Label("\tEquipement can be obtained by killing enemies and breaking effigies. Normal enemies can drop a few items on death, if the odds are in your favor. The chance to get any items from a normal enemy is 10%. The amount of items obtained from normal enemies is at least 1 and maximum amount increases with players in a lobby.\n" +
             "Elite enemies always drop items in large amounts.\n" +
             "\tItems can be equipped by dragging and dropping them onto a right equipement slot or shift+left click. The item will grant it's stats only if you meets item's level requirement. The best tier of items is only obtainable on high difficulties.");
            Label("By unlocking a perk in the survival category, it's possible to change the stats on your existing items, and reforge unused items into something useful. Reforged item will have the same level as item put into the main crafting slot.");
            
            Space(100);
            
            Header("Information - Mutations and Abilities");
            Label("\tUpon leveling up, the player will recieve a upgrade point. Then it's up to the player to use it to unlock a mutation, that will serve as a permanent perk, or to spend two upgrade points to unlock a ability.\n" +
                "Abilities are in majority of the cases more powerful than perks, as they cost more and the number of active abilities is limitied to 6.\n" +
                "Some perks can be bought multiple times.\n" +
                "\n" +
                "Refunding - it is possible to refund all points, to do so, heart of purity needs to be consumed. This item is of yellow rarity, and thus unobtainable on easy and veteran difficulties." +
                "More points - to gain a point without leveling, a rare item of green rarity needs to be consumed. It permamently adds a upgrade point, and it persists even after refunding.");
            Space(100);

            Header("Information - Enemies");
            Label("\tEnemies in the forest have adapted to your skill. As they level with you, they become faster and stronger. But speed and strength alone shouldn't be your main concern. There are a lot more dangerous beings out there." +
                "\n\n" +
                "Common enemies changed slightly. Their health increases with level.\n" +
                "A new statistic to enemies is 'Armor'. This property reduces damage taken by the enemies from physical attacks, and partly reduces damage from magical attacks. Armor can be reduced in a number of ways.\n" +
                "The easiest way to reduce armor is to use fire. Fire works as a way to crowd control enemies, it renders a few enemies unable to run and attack as they shake off the flames.\n" +
                "Other way to reduce armor is to equip items, which reduce armor on hit.\n" +
                "If you dont have any way to reduce enemy's armor, damaging them with spells would decrease the reduction from armor by 2/3, allowing you to deal some damage.");
            Space(30);
            Label("Elite enemies\n" +
                "An elite is a uncommon type of a mutant with increased stats and access to special abilities, that make encounters with them challenging." +
                "\nEnemy abilities:");
            Label("- Steadfast - This defensive ability causes enemy to reduce all damage exceeding a percent of their maximum health. To deal with this kind of ability, damage over time and fast attacks are recommended. This ability counters nuke instances of damage.");
            Label("- Blizzard - A temporary aura around an enemy, that slows anyone in it's area of effect. Affects movement speed and attack speed. Best way to deal with this is to avoid getting within it's range. Crowd controll from ranged attacks and running seems like the best option.");
            Label("- Radiance - A permanent aura around an enemy. It deals damage anyone around. The only way of dealing with this is to never get close to the enemy.");
            Label("- Chains - Roots anyone in a big radius around the elite. The duration this root increases with difficulty. Several abilities that provide resistance to crowd controll clear the effects of this ability.");
            Label("- Black hole - A very strong ability. The spell has a fixed cooldown, and the enemy will attempt to cast it as soon as a player gets within his range effective.");
            Label("- Trap sphere - Long lasting sphere that forces you to stay inside it untill it's effects wears off");
            Label("- Juggernaut - The enemy is completely immune to crowd controll and bleeding.\n");
            Label("- Gargantuan - Describes an enemy that is bigger, faster, stronger and has more health.");
            Label("- Tiny - An enemy has decreased size. It's harder to hit it with ranged attacks and most of the melee weapons can only attack the enemy with slow smashes.");
            Label("- Extra tough - enemy has a lot more healt");
            Label("- Extra deadly - enemy has a lot more damage");
            Label("- Basher - the enemy stuns on hit. Best way to fight it is to not get hit or parry it's attacks.");
            Label("- Warping - An ability allowing to teleport. Strong against glass cannon builds, running away and ranged attacks. Weak agnist melee strikes and a lot of durability.");
            Label("- Rain Empowerment - If it rains, the enemy gains in strenght, speed, armor and size.");
            Label("- Meteors - Periodically spawns a rain of powerful meteors. They are rather easy to spot and they move at a slow medium speed.");
            Label("- Flare - Slows and damages me if you stand inside. Heals and makes enemies faster.");
            Label("- Undead - An enemy upon dieing restores portion of it's health, gets stronger and bigger.");
            Label("- Plasma cannon - Creates a turret that fires a laser beam that damages players and buildings.");
            Label("- Poisonous - Enemies gain a attack modifier, that applies a stacking debuff, which deals damage over time. Once hit, it is adviced to retreat and wait for the poison stop damaging you.");
            Label("- Cataclysm - Enemy uses the cataclysm spell to slow you down and damage you.");


            Header("Changes");
            Label("Champions of The Forest provides variety of changes to in-game mechanics." +
                "\nArmor no longer absorbs all damage. Instead it reduces the damage by 70%." +
                "\nPlayer is slowed down if out of stamina (the inner blue bar)" +
                "\nTraps no longer instantly kill cannibals. Instead they deal damage." +
                "\nDynamite no longer instantly kills enemies." +
                "\nEnemies have armor and increased health." +
                "\nPlayers take increased damage from explosives. This affects how much damage the worm does" +
                "\nPlayer deal increased damage to other players if friendly fire is enabled.");


            Space(300);

            Header("Statistics");
            Stat("Strength", ModdedPlayer.instance.strength.ToString("N0") + " str", "Increases melee damage by " + ModdedPlayer.instance.DamagePerStrength * 100 + "% for every 1 point of strength. Current bonus melee damage from strength [" + ModdedPlayer.instance.strength * 100 * ModdedPlayer.instance.DamagePerStrength + "]");
            Stat("Agility", ModdedPlayer.instance.agility.ToString("N0") + " agi", "Increases ranged damage by " + ModdedPlayer.instance.RangedDamageperAgi * 100 + "% for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.instance.agility * 100 * ModdedPlayer.instance.RangedDamageperAgi + "]\n" +
                "Increases maximum energy by " + ModdedPlayer.instance.EnergyPerAgility + " for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.instance.agility * ModdedPlayer.instance.EnergyPerAgility + "]");
            Stat("Vitality", ModdedPlayer.instance.vitality.ToString("N0") + " vit", "Increases health by " + ModdedPlayer.instance.HealthPerVitality + "for every 1 point of vitality. Current bonus health from vitality [" + ModdedPlayer.instance.vitality * ModdedPlayer.instance.HealthPerVitality + "]");
            Stat("Intelligence", ModdedPlayer.instance.intelligence.ToString("N0") + " int", "Increases spell damage by " + ModdedPlayer.instance.SpellDamageperInt * 100 + "% for every 1 point of intelligence. Current bonus spell damage from intelligence [" + ModdedPlayer.instance.intelligence * 100 * ModdedPlayer.instance.SpellDamageperInt + "]\n" +
                "Increases stamina regen by " + ModdedPlayer.instance.EnergyRegenPerInt * 100 + "% for every 1 point of intelligence. Current bonus stamina regen from intelligence [" + ModdedPlayer.instance.intelligence * 100 * ModdedPlayer.instance.EnergyRegenPerInt + "]");


            Space(60);
            Image(105, 70);
            Header("Health & Energy");
            Space(10);

            Stat("Max health", ModdedPlayer.instance.MaxHealth.ToString("N0") + "", "Total health pool.\n" +
                "Base health: " + ModdedPlayer.instance.baseHealth +
                "\nBonus health: " + ModdedPlayer.instance.HealthBonus +
                "\nHealth from vitality: " + ModdedPlayer.instance.HealthPerVitality * ModdedPlayer.instance.vitality +
                "\nHealth multipier: " + ModdedPlayer.instance.MaxHealthPercent * 100 + "%");
            Stat("Max energy", ModdedPlayer.instance.MaxEnergy.ToString("N0") + "", "Total energy pool.\n" +
                "Base energy: " + ModdedPlayer.instance.baseEnergy +
                "\nBonus energy: " + ModdedPlayer.instance.EnergyBonus +
                "\nEnergy from agility: " + ModdedPlayer.instance.EnergyPerAgility * ModdedPlayer.instance.agility +
                "\nEnergy multipier: " + ModdedPlayer.instance.MaxEnergyPercent * 100 + "%");


            Space(60);
            Image(99, 70);
            Header("Defense");
            Space(10);
            Stat("Armor", ModdedPlayer.instance.Armor.ToString("N0"), "Armor provides physical damage reduction\nYour current amount of armor provides " + ModdedPlayer.instance.ArmorDmgRed * 100 + "% dmg reduction.");
            Stat("Magic resistance", ModdedPlayer.instance.MagicResistance * 100 + "%", "Magic damage reduction. Decreases damage from enemy abilities.");
            Stat("Dodge Chance", ModdedPlayer.instance.DodgeChance * 100 + "%", "A chance to avoid entire instance of damage. Works only for physical damage sources.");
            Stat("Damage taken reduction", Math.Round((ModdedPlayer.instance.DamageReductionTotal - 1) * 100, 1) + "%");
            Stat("Block", ModdedPlayer.instance.BlockFactor * 100 + "%");
            Stat("Absorb amount", ModdedPlayer.instance.DamageAbsorbAmount * 100 + "%");
            Stat("Fire resistance", Math.Round((1 - ModdedPlayer.instance.FireDamageTakenMult) * 100) + "%");


            Space(60);
            Header("Recovery");
            Space(10);

            Stat("Total Stamina recovery per second", ModdedPlayer.instance.StaminaRecover + "", "Stamina regen is temporairly paused after sprinting");
            Stat("Stamina per second", ModdedPlayer.instance.StaminaRegen * (1 + ModdedPlayer.instance.StaminaRegenPercent) + "", "Stamina per second: " + ModdedPlayer.instance.StaminaRegen + "\nStamina regen bonus: " + ModdedPlayer.instance.StaminaRegenPercent * 100 + "%");

            Stat("Energy per second", ModdedPlayer.instance.EnergyPerSecond * ModdedPlayer.instance.StaminaAndEnergyRegenAmp + "", "Energy per second: " + ModdedPlayer.instance.EnergyPerSecond + "\nStamina and energy regen multipier: " + ModdedPlayer.instance.StaminaAndEnergyRegenAmp * 100 + "%");
            Stat("Energy on hit", ModdedPlayer.instance.EnergyOnHit * ModdedPlayer.instance.StaminaAndEnergyRegenAmp + "", "Energy on hit: " + ModdedPlayer.instance.EnergyOnHit + "\nStamina and energy regen multipier: " + ModdedPlayer.instance.StaminaAndEnergyRegenAmp * 100 + "%");
            Stat("Health per second", ModdedPlayer.instance.LifeRegen * (ModdedPlayer.instance.HealthRegenPercent + 1) * ModdedPlayer.instance.HealingMultipier + "", "Health per second: " + ModdedPlayer.instance.LifeRegen + "\nStamina regen bonus: " + ModdedPlayer.instance.HealthRegenPercent * 100 + "%\nAll Healing Amplification: " + (ModdedPlayer.instance.HealingMultipier - 1) * 100 + "%");
            Stat("Health on hit", ModdedPlayer.instance.LifeOnHit * (ModdedPlayer.instance.HealthRegenPercent + 1) * ModdedPlayer.instance.HealingMultipier + "", "Health on hit: " + ModdedPlayer.instance.LifeOnHit + "\nStamina regen bonus: " + Math.Round(ModdedPlayer.instance.HealthRegenPercent * 100, 2) + "%\nAll Healing Amplification: " + (ModdedPlayer.instance.HealingMultipier - 1) * 100 + "%");

            Space(60);
            Header("Attack");
            Space(10);
            Stat("All damage amplification", Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
            Stat("Critical hit damage", Math.Round(ModdedPlayer.instance.CritDamage, 2) + "%");
            Stat("Critical hit chance", Math.Round(ModdedPlayer.instance.CritChance * 100, 2) + "%");
            Stat("Attack speed", Math.Round(ModdedPlayer.instance.AttackSpeed * 100, 2) + "%", "Increases the speed of player actions - weapon swinging, reloading guns and drawing bows");
            Stat("Additional fire damage", Math.Round(ModdedPlayer.instance.FireAmp * 100, 2) + "%", "Increases fire damage");
            Stat("Bleed chance", ModdedPlayer.instance.ChanceToBleedOnHit.ToString("P"), "Bleeding enemies take 5% of damage dealt per second for 10 seconds");
            Stat("Weaken chance", ModdedPlayer.instance.ChanceToWeakenOnHit.ToString("P"), "Weakened enemies take 20% increased damage from all players.");
            Stat("Slow chance", ModdedPlayer.instance.ChanceToSlowOnHit.ToString("P"), "Slowed enemies move and attack 50% slower");


            Space(20);
            Image(89, 70);
            Header("Melee");
            Space(10);

            Stat("Melee damage", Math.Round(ModdedPlayer.instance.MeleeAMP * 100, 2) + "%", "Melee damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
                "Bonus from strength: " + Math.Round(ModdedPlayer.instance.strength * ModdedPlayer.instance.DamagePerStrength * 100, 2) + "%\n" +
                "Melee damage amplification: " + Math.Round((ModdedPlayer.instance.MeleeDamageAmplifier - 1) * 100, 2) + "%\n" +
                "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
            Stat("Additional melee weapon damage", Math.Round(ModdedPlayer.instance.MeleeDamageBonus) + "", "Melee damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to weapon damage and multiplied by the stat above");
            Stat("Melee range", Math.Round(ModdedPlayer.instance.MeleeRange * 100) + "%");
            Stat("Heavy attack damage", ModdedPlayer.instance.HeavyAttackMult.ToString("P"));

            Space(20);
            Image(98, 70);
            Header("Ranged");
            Space(10);

            Stat("Ranged damage", Math.Round(ModdedPlayer.instance.RangedAMP * 100, 2) + "%", "Ranged damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
             "Bonus from agility: " + Math.Round(ModdedPlayer.instance.agility * ModdedPlayer.instance.RangedDamageperAgi * 100, 2) + "%\n" +
             "Ranged damage amplification: " + Math.Round((ModdedPlayer.instance.RangedDamageAmplifier - 1) * 100, 2) + "%\n" +
             "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
            Stat("Additional ranged weapon damage", Math.Round(ModdedPlayer.instance.RangedDamageBonus) + "", "Ranged damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to projectile damage and multiplied by the stat above");
            Stat("Projectile speed", Math.Round(ModdedPlayer.instance.ProjectileSpeedRatio * 100) + "%", "Faster projectiles fly further and fall slower");
            Stat("Projectile size", Math.Round(ModdedPlayer.instance.ProjectileSizeRatio * 100) + "%", "Bigger projectiles allow to land headshots easier. Most projectiles still can hit only 1 target.");
            Stat("Headshot damage", Math.Round(ModdedPlayer.instance.HeadShotDamage * 100) + "%", "Damage multipier on headshot");
            Stat("No consume chance", ModdedPlayer.instance.ReusabilityChance.ToString("P"));
            Stat("Spear headshot chance", ModdedPlayer.instance.SpearCritChance.ToString("P"));
            if (ModdedPlayer.instance.SpearhellChance > 0) Stat("Double spear chance", ModdedPlayer.instance.SpearhellChance.ToString("P"));
            if (ModdedPlayer.instance.SpearDamageMult != 1) Stat("Spear damage", ModdedPlayer.instance.SpearhellChance.ToString("P"));
            if (ModdedPlayer.instance.SpearArmorRedBonus) Stat("Spears reduce additional armor", "");
            Stat("Bullet headshot chance", ModdedPlayer.instance.BulletCritChance.ToString("P"));
            if (ModdedPlayer.instance.BulletDamageMult != 1) Stat("Bullet damage", ModdedPlayer.instance.SpearhellChance.ToString("P"));
            if (ModdedPlayer.instance.CrossbowDamageMult != 1) Stat("Crossbow damage", ModdedPlayer.instance.CrossbowDamageMult.ToString("P"));
            if (ModdedPlayer.instance.BowDamageMult != 1) Stat("Bow damage", ModdedPlayer.instance.BowDamageMult.ToString("P"));
            if (ModdedPlayer.instance.IsCrossfire) Stat("Shooting an enemy creates magic arrows", "");

            Stat("Multishot Projectiles", ModdedPlayer.instance.SoraSpecial ? (4 + ModdedPlayer.instance.MultishotCount).ToString("N") : ModdedPlayer.instance.MultishotCount.ToString("N"));
            Stat("Multishot Cost", (ModdedPlayer.instance.SoraSpecial ? 1f * Mathf.Pow(ModdedPlayer.instance.MultishotCount,1.75f) : 10 * Mathf.Pow(ModdedPlayer.instance.MultishotCount, 1.75f)).ToString("N"), "Formula for multishot cost in energy is (Multishot Projectiles ^ 1.75) * 10");



            Space(20);
            Image(110, 70);
            Header("Magic");
            Space(10);

            Stat("Spell damage", Math.Round(ModdedPlayer.instance.SpellAMP * 100, 2) + "%", "Spell damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
             "Bonus from intelligence: " + Math.Round(ModdedPlayer.instance.intelligence * ModdedPlayer.instance.SpellDamageperInt * 100, 2) + "%\n" +
             "Spell damage amplification: " + Math.Round((ModdedPlayer.instance.SpellDamageAmplifier - 1) * 100, 2) + "%\n" +
             "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
            Stat("Additional spell damage", Math.Round(ModdedPlayer.instance.SpellDamageBonus) + "", "Spell damage bonus can be increased by perks and inventory items. This is added to spell damage and multiplied by the stat above. Often spells take a fraction of this stat and add it to spell's damage.");
            Stat("Spell cost reduction", Math.Round((1 - ModdedPlayer.instance.SpellCostRatio) * 100) * -1 + "%", "");
            Stat("Spell cost to stamina", Math.Round((ModdedPlayer.instance.SpellCostToStamina) * 100) + "%", "");
            Stat("Cooldown reduction", Math.Round((1 - ModdedPlayer.instance.CoolDownMultipier) * 100) + "%", "");


            Space(20);
            GUI.color = Color.red;
            Image(96, 70);
            GUI.color = Color.white;
            Header("Armor reduction");
            Space(10);
            Stat("Melee", ModdedPlayer.instance.ARreduction_melee + "", "Total melee armor reduction: " + ModdedPlayer.instance.MeleeArmorReduction);
            Stat("Ranged", ModdedPlayer.instance.ARreduction_ranged + "", "Total ranged armor reduction: " + ModdedPlayer.instance.RangedArmorReduction);
            Stat("Any source", ModdedPlayer.instance.ARreduction_all + "", "Decreases armor of enemies hit by either of the sources");


            Space(60);

            Header("Survivor stats");
            Space(10);

            Stat("Movement speed", Math.Round(ModdedPlayer.instance.MoveSpeed *ModdedPlayer.instance.MoveSpeedMult * 100 ) + "% ms", "Multipier of base movement speed. Base walking speed is equal to " + FPCharacterMod.basewalkSpeed + " units per second, with bonuses it's " + FPCharacterMod.basewalkSpeed * ModdedPlayer.instance.MoveSpeed *ModdedPlayer.instance.MoveSpeedMult + " units per second");
            Stat("Jump power", Math.Round(ModdedPlayer.instance.JumpPower * 100) + "%", "Multipier of base jump power. Increases height of your jumps");
            Stat("Hunger rate", (1 / ModdedPlayer.instance.HungerRate) * 100 + "%", "How much slower is the rate of consuming food compared to normal.");
            Stat("Thirst rate", (1 / ModdedPlayer.instance.ThirstRate) * 100 + "%", "How much slower is the rate of consuming water compared to normal.");
            Stat("Experience gain", ModdedPlayer.instance.ExpFactor * 100 + "%", "Multipier of any experience gained");
            Stat("Massacre duration", ModdedPlayer.instance.MaxMassacreTime + " s", "How long massacres can last");
            Stat("Time on kill", ModdedPlayer.instance.TimeBonusPerKill + " s", "Amount of time that is added to massacre for every kill");
            if (ModdedPlayer.instance.TurboRaft)
                Stat("Turbo raft speed", ModdedPlayer.instance.RaftSpeedMultipier + "%", "Speed multiplier of rafts");


            Space(40);
            Image(90, 70);
            Header("Inventory Stats");
            Space(10);
            foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> pair in ModdedPlayer.instance.ExtraCarryingCapactity)
            {
                string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Value.ID, (pair.Value.Amount > 1), false);
                Stat(item_name, "+" + pair.Value.Amount, "How many extra '" + item_name + "' you can carry. Item ID is " + pair.Value.ID);
            }
            Space(10);
            if (ModdedPlayer.instance.GeneratedResources.Count > 0)
                Header("Generated resources");
            foreach (var pair in ModdedPlayer.instance.GeneratedResources)
            {
                string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Key, (pair.Value > 1), false);
                Stat(item_name, pair.Value.ToString(), "How many '" + item_name + "' you generate daily. Item ID is " + pair.Key);
            }

            if (BookPositionY < Screen.height && BookPositionY > -140 * rr)
            {
                Rect labelRect = new Rect(GuideWidthDecrease * rr + GuideMargin * rr, BookPositionY, Screen.width - 2 * rr * (GuideMargin + GuideWidthDecrease), 85 * rr);
                if (GUI.Button(labelRect, "Bugged stats? Click to reset", new GUIStyle(GUI.skin.button)
                {
                    font = MainFont,
                    fontSize = Mathf.RoundToInt(70 * rr),
                    alignment = TextAnchor.UpperCenter,
                    richText = true,
                }))
                {
                    ModdedPlayer.ResetAllStats();
                }
                BookPositionY += 85 * rr;
                Rect imageRect = new Rect(400 * rr, BookPositionY, Screen.width - 800 * rr, 60 * rr);
                GUI.DrawTexture(imageRect, ResourceLoader.GetTexture(30));
                BookPositionY += 70 * rr;
            }
            else
            {
                BookPositionY += 155 * rr;
            }



            //Space(200);
            //Header("Dairy");
            //Stat("Day 0", "");
            //Label("I barely survived the plane crash. Shortly after hitting the ground I lost consciousness. I remember my son Timmy being taken by a red human, and nothing else. I need to find my boy...\n");
            //if (LocalPlayer.Stats.DaySurvived < 1)
            //{
            //    return;
            //}

            //Stat("Day 1", "");
            //Label("There is something weird about this island. I swear I have seen some people. They did not look friendly. I'd better stay on guard.\n");
            //if (LocalPlayer.Stats.DaySurvived < 2)
            //{
            //    return;
            //}

            //Stat("Day 2", "");
            //Label("They are clearly hostile towards me. They are horrifying. They are cannibals. I need to find a way to defend myself.\n");
            //if (LocalPlayer.Stats.DaySurvived < 10)
            //{
            //    return;
            //}

            //Stat("Day 10", "");
            //Label("Something is seriousely wrong about this place. Those creatures... I started seeing them on the surface. They appear to have human elements, but they are definitely monsters. They are extremely hostile. \n I need to get stronger or else i'll get slaughtered here.");
        }
        #endregion



        private void DrawLine(Vector2 pointA, Vector2 pointB, float length, Color color)
        {

            Color c = GUI.color;

            float width = 20.0f * rr;
            GUI.color = color;
            float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;

            GUIUtility.RotateAroundPivot(angle, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, length, width), Res.ResourceLoader.GetTexture(71));
            GUI.matrix = matrixBackup;
            GUI.color = c;

        }

        private Perk.PerkCategory _perkpage = Perk.PerkCategory.MeleeOffense;
        private readonly float[] _perkCategorySizes = new float[6];
        private int[] DisplayedPerkIDs = new int[0];
        private Vector2 currentPerkOffset;
        private Vector2 targetPerkOffset;
        private float _perkDetailAlpha;
        private float _timeToBuyPerk;

        private bool PerkEnabled(Perk perk)
        {
            if (perk.InheritIDs.Length == 0)
            {
                return true;
            }

            for (int i = 0; i < perk.InheritIDs.Length; i++)
            {
                if (perk.InheritIDs[i] == -1) { return true; }
                else if (Perk.AllPerks[perk.InheritIDs[i]].IsBought)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Hovered;
        private bool Buying;
        private void DrawPerks()
        {
            //offset for background
            float x = Mathf.Clamp(-(currentPerkOffset.x - wholeScreen.center.x) / (Screen.width * 5) + 0.25f, 0, 0.5f);
            float y = Mathf.Clamp((currentPerkOffset.y - wholeScreen.center.y) / (Screen.height * 5) + 0.25f, 0, 0.5f);
            Rect bgRectCords = new Rect(x, y, 0.5f, 0.5f);


            //Drawing background images
            switch (_perkpage)
            {
                case Perk.PerkCategory.MeleeOffense:
                    GUI.DrawTextureWithTexCoords(wholeScreen, ResourceLoader.GetTexture(72), bgRectCords);
                    break;
                case Perk.PerkCategory.RangedOffense:
                    GUI.DrawTextureWithTexCoords(wholeScreen, ResourceLoader.GetTexture(74), bgRectCords);
                    break;
                case Perk.PerkCategory.MagicOffense:
                    GUI.DrawTextureWithTexCoords(wholeScreen, ResourceLoader.GetTexture(73), bgRectCords);
                    break;
                case Perk.PerkCategory.Defense:
                    GUI.DrawTextureWithTexCoords(wholeScreen, ResourceLoader.GetTexture(75), bgRectCords);
                    break;
                case Perk.PerkCategory.Support:
                    GUI.DrawTextureWithTexCoords(wholeScreen, ResourceLoader.GetTexture(77), bgRectCords);
                    break;
                case Perk.PerkCategory.Utility:
                    GUI.DrawTextureWithTexCoords(wholeScreen, ResourceLoader.GetTexture(76), bgRectCords);
                    break;
                default:
                    break;
            }







            //move left right
            if (mousepos.y > Screen.height - 30 * rr)
            {
                targetPerkOffset += Vector2.down * Time.unscaledDeltaTime * 300;
            }
            else if (mousepos.y < 30 * rr)
            {
                targetPerkOffset += Vector2.down * Time.unscaledDeltaTime * -300;

            }
            if (mousepos.x > Screen.width - 30 * rr)
            {
                targetPerkOffset += Vector2.right * Time.unscaledDeltaTime * -300;
            }
            else if (mousepos.x < 30 * rr)
            {
                targetPerkOffset += Vector2.right * Time.unscaledDeltaTime * 300;
            }
            currentPerkOffset = Vector3.Slerp(currentPerkOffset, targetPerkOffset, Time.unscaledDeltaTime * 15f);

            //filling DisplayedPerkIDs with perk ids where category is the same as the selected one
            if (DisplayedPerkIDs == null)
            {
                DisplayedPerkIDs = Perk.AllPerks.Where(p => p.Category == _perkpage).Select(p => p.ID).ToArray();
            }

            //Drawing Perks
            Rect rect = new Rect(currentPerkOffset, new Vector2(PerkWidth, PerkHeight) * 2)
            {
                center = currentPerkOffset
            };
            GUI.DrawTexture(rect, ResourceLoader.GetTexture(84));
            GUI.color = Color.black;
            GUI.Label(rect, ModdedPlayer.instance.MutationPoints.ToString(), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(80 * rr), fontStyle = FontStyle.Bold, font = MainFont });
            GUI.color = Color.white;



            Hovered = false;
            Buying = false;
            SelectedPerk = null;
            for (int i = 0; i < DisplayedPerkIDs.Length; i++)
            {
                DrawPerk(DisplayedPerkIDs[i]);
            }
            if (SelectedPerk != null)
            {
                Rect r = new Rect(selectedPerk_Rect);
                Perk p = SelectedPerk;
                Hovered = true;
                r.center = SelectedPerk_center;
                if (_perkDetailAlpha == 0)
                    Effects.Sound_Effects.GlobalSFX.Play(0);

                _perkDetailAlpha += Time.unscaledDeltaTime;
                if (_perkDetailAlpha >= 0.7f)
                {
                    GUI.color = new Color(_perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f);

                    Rect Name = new Rect(r.x - 200 * rr, r.y - 130 * rr, 400 * rr + r.width, 90 * rr);

                    GUI.Label(Name, p.Name, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(40 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });

                    Rect Desc = new Rect(r.x - 200 * rr, r.yMax + 30 * rr, 400 * rr + r.width, 1000 * rr);

                    string desctext = p._description;

                    if (!p.IsBought || p.Endless)
                    {
                        desctext = "Press to buy\n" + p._description;
                        Rect LevelReq = new Rect(r.x - 440 * rr, r.y, 400 * rr, r.height);
                        Rect Cost = new Rect(r.xMax + 40 * rr, r.y, 400 * rr, r.height);
                        if (p.LevelRequirement > ModdedPlayer.instance.Level)
                        {
                            GUI.color = Color.red;
                        }

                        GUI.Label(LevelReq, "Level " + p.LevelRequirement, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(33 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });
                        if (ModdedPlayer.instance.MutationPoints < p.PointsToBuy)
                        {
                            GUI.color = Color.red;
                        }
                        else
                        {
                            GUI.color = Color.white;
                        }

                        GUI.Label(Cost, "Cost in mutation points: " + p.PointsToBuy, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(33 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });
                        GUI.color = Color.white;
                        if (UnityEngine.Input.GetMouseButton(0) && ModdedPlayer.instance.MutationPoints >= p.PointsToBuy && PerkEnabled(Perk.AllPerks[SelectedPerk_ID]) && Perk.AllPerks[SelectedPerk_ID].LevelRequirement <= ModdedPlayer.instance.Level)
                        {
                            _timeToBuyPerk += Time.unscaledDeltaTime;
                            Buying = true;
                            Rect buyRect = new Rect(0, 1 - _timeToBuyPerk / 2, 1, _timeToBuyPerk / 2);
                            Rect buyRect2 = new Rect(r);
                            r.height *= _timeToBuyPerk / 2;

                            GUI.color = SelectedPerk_Color;
                            if (p.Endless && p.IsBought)
                            {
                                Color c = GUI.color;
                                c.a = 0.5f;
                                c.r /= 2;
                                c.g /= 2;
                                c.b /= 2;
                                GUI.color = c;
                            }
                            GUI.DrawTextureWithTexCoords(r, ResourceLoader.GetTexture(p.TextureVariation * 2 + 81 + 1), buyRect);
                            GUI.color = Color.white;
                            if (_timeToBuyPerk >= 2)
                            {
                                if (Perk.AllPerks[SelectedPerk_ID].Endless)
                                {
                                    Perk.AllPerks[SelectedPerk_ID].ApplyAmount++;
                                }
                                Perk.AllPerks[SelectedPerk_ID].IsBought = true;
                                Perk.AllPerks[SelectedPerk_ID].OnBuy();
                                
                                ModdedPlayer.instance.MutationPoints -= p.PointsToBuy;
                                Perk.AllPerks[SelectedPerk_ID].ApplyMethods();
                                Perk.AllPerks[SelectedPerk_ID].Applied = true;
                                Buying = false;
                                Effects.Sound_Effects.GlobalSFX.Play(3);

                            }
                        }
                    }
                    GUIStyle descStyle = new GUIStyle(GUI.skin.box) { margin = new RectOffset(5, 5, Mathf.RoundToInt(10 * rr), 10), alignment = TextAnchor.UpperCenter, fontSize = Mathf.RoundToInt(28 * rr), font = MainFont, fontStyle = FontStyle.Normal, richText = true, wordWrap = true };
                    Desc.height = descStyle.CalcHeight(new GUIContent(desctext), Desc.width) + 10 * rr;
                    GUI.Label(Desc, desctext, descStyle);

                }


                GUI.color = Color.white;
            }
            if (!Buying)
            {
                _timeToBuyPerk = 0;
            }
            if (!Hovered)
            {
                _perkDetailAlpha = 0;
            }

            Array menus = Enum.GetValues(typeof(Perk.PerkCategory));
            float btnSize = 250 * rr;
            float bigBtnSize = 40 * rr;
            float offset = Screen.width / 2 - (menus.Length * btnSize + bigBtnSize) / 2;

            for (int i = 0; i < menus.Length; i++)
            {
                Rect topButton = new Rect(offset, 35 * rr, btnSize, 60 * rr);
                if ((Perk.PerkCategory)menus.GetValue(i) == _perkpage)
                {
                    _perkCategorySizes[i] = Mathf.Clamp(_perkCategorySizes[i] + Time.unscaledDeltaTime * 40, 0, bigBtnSize);
                    topButton.width += bigBtnSize;
                    topButton.height += bigBtnSize / 2;
                }
                else
                {
                    _perkCategorySizes[i] = Mathf.Clamp(_perkCategorySizes[i] - Time.unscaledDeltaTime * 30, 0, bigBtnSize);
                }
                topButton.width += _perkCategorySizes[i];
                topButton.height += _perkCategorySizes[i] / 2;
                offset += topButton.width;
                string content = string.Empty;
                switch ((Perk.PerkCategory)menus.GetValue(i))
                {
                    case Perk.PerkCategory.MeleeOffense:
                        content = "Melee";
                        break;
                    case Perk.PerkCategory.RangedOffense:
                        content = "Ranged";
                        break;
                    case Perk.PerkCategory.MagicOffense:
                        content = "Magic";
                        break;
                    case Perk.PerkCategory.Defense:
                        content = "Defensive";
                        break;
                    case Perk.PerkCategory.Support:
                        content = "Support";
                        break;
                    case Perk.PerkCategory.Utility:
                        content = "Survival";
                        break;
                    default:
                        content = ((Perk.PerkCategory)menus.GetValue(i)).ToString();
                        break;
                }
                if (GUI.Button(topButton, content, new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(40 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow }))
                {
                    _perkpage = (Perk.PerkCategory)menus.GetValue(i);
                    targetPerkOffset = wholeScreen.center;
                    currentPerkOffset = targetPerkOffset;
                    DisplayedPerkIDs = Perk.AllPerks.Where(p => p.Category == _perkpage).Select(p => p.ID).ToArray();
                }


            }
        }

        private Perk SelectedPerk = null;
        private Rect selectedPerk_Rect;
        private Vector2 SelectedPerk_center;
        private int SelectedPerk_ID;
        private Color SelectedPerk_Color;

        private void DrawPerk(int a)
        {
            Perk p = Perk.AllPerks[a];

            bool show = false;
            for (int i = 0; i < p.InheritIDs.Length; i++)
            {
                if (p.InheritIDs[i] == -1 || Perk.AllPerks[p.InheritIDs[i]].IsBought)
                {
                    show = true;
                    break;
                }
            }
            if (!show)
            {
                return;
            }

            Vector2 center = new Vector2(PerkWidth * p.PosOffsetX, PerkHeight * p.PosOffsetY);
            center += currentPerkOffset;
            Vector2 size = new Vector2(PerkWidth, PerkHeight);
            size *= p.Size;
            Rect r = new Rect(Vector2.zero, size)
            {
                center = center
            };

            Color color = Color.white;
            switch (_perkpage)
            {
                case Perk.PerkCategory.MeleeOffense:
                    color = Color.red;
                    break;
                case Perk.PerkCategory.RangedOffense:
                    color = Color.green;
                    break;
                case Perk.PerkCategory.MagicOffense:
                    color = Color.blue;
                    break;
                case Perk.PerkCategory.Defense:
                    color = Color.magenta;
                    break;
                case Perk.PerkCategory.Support:
                    color = Color.yellow;
                    break;
                case Perk.PerkCategory.Utility:
                    color = Color.white;
                    break;
                default:
                    break;
            }
            if (p.IsBought)
            {
                GUI.color = color;
                GUI.DrawTexture(r, ResourceLoader.GetTexture(p.TextureVariation * 2 + 81 + 1));
                if (p.Endless)
                {
                    GUI.color = Color.black;
                    GUI.Label(r, p.ApplyAmount.ToString(), new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(40 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow, alignment = TextAnchor.MiddleCenter });
                }
                GUI.color = Color.white;

            }
            else
            {
                GUI.color = Color.gray;
                GUI.DrawTexture(r, ResourceLoader.GetTexture(p.TextureVariation * 2 + 81));
                GUI.color = Color.white;

            }
            if (p.Icon != null)
            {
                GUI.DrawTexture(r, p.Icon);

            }
            float distsquared = (mousepos - r.center).sqrMagnitude;
            if (r.Contains(mousepos) && distsquared < PerkHexagonSide * PerkHexagonSide * 0.81f)
            {
                SelectedPerk = p;
                selectedPerk_Rect = r;
                SelectedPerk_center = center;
                SelectedPerk_ID = a;
                SelectedPerk_Color = color;
            }


        }


        private void DrawBuff(float x, float y, Texture2D tex, string amount, string time, bool isPositive, float durationInSeconds)
        {
            Rect r = new Rect(x * rr, y * rr, BuffSize * rr, BuffSize * rr);

            GUI.DrawTexture(r, ResourceLoader.GetTexture(143));
            if (isPositive)
            {
                GUI.color = new Color(1, 1, 1, 0.25f + 0.5f * Mathf.Sin(durationInSeconds * 3));
                GUI.DrawTexture(r, ResourceLoader.GetTexture(145));
                GUI.color = new Color(1, 1, 1, 1);
            }
            else
            {
                GUI.color = new Color(1, 0, 0, 0.25f + 0.5f * Mathf.Sin(durationInSeconds * 3));
                GUI.DrawTexture(r, ResourceLoader.GetTexture(145));
                GUI.color = new Color(1, 1, 1, 1);
            }


            GUI.DrawTexture(r, tex);

            GUI.DrawTexture(r, ResourceLoader.GetTexture(144));
            GUI.Label(r, amount, new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(16 * rr), font = MainFont, fontStyle = FontStyle.Italic, normal = new GUIStyleState() { textColor = Color.red }, richText = true, clipping = TextClipping.Overflow, alignment = TextAnchor.UpperLeft });
            GUI.Label(r, time, new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(15 * rr), font = MainFont, fontStyle = FontStyle.Normal, richText = true, clipping = TextClipping.Overflow, alignment = TextAnchor.LowerRight });
        }

    }
}
