using ChampionsOfForest.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheForest.Utils;
using UnityEngine;
using static ChampionsOfForest.Player.BuffDB;
using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
    public class MainMenu : MonoBehaviour
    {
        //Instance of this object
        public static MainMenu Instance { get; private set; }

        //Difficulty Settings
        public string[] DiffSel_Names = new string[] { "Normal", "Hard", "Elite", "Master", "Challenge I", "Challenge II", "Challenge III", "Challenge IV", "Challenge V", };
        public string[] DiffSel_Descriptions = new string[]
        {
            "Easiest difficulty, recommended for new games.\nItem drops of tier 0 & 1",
            "Much harder than normal difficulty, tougher enemies. \nUnlocks 2nd tier loot. Recommended level 15+",
            "Unlocks 3rd tier loot, tougher enemies.",
            "Unlocks 4th tier of items. For strong players only. Enemies are much tougher ",
            "Challenge I unlocks 5th tier of items. \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge II unlocks 6th tier of items. \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge III unlocks 7th tier of items. \nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge IV\nWith every challenge difficulty enemies are stronger, and their bounties are higher",
            "Challenge V  \nWith every challenge difficulty enemies are stronger, and their bounties are higher",

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

        public Item DraggedItem = null;
        private Vector2 itemPos;
        private Vector2 slotDim;


        //Perks variables
        private readonly float PerkHexagonSide = 60;
        private float PerkHeight;
        private float PerkWidth;


        private readonly float GuideWidthDecrease = 150;
        private readonly float GuideMargin = 30;

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
                    if (item.Value._hp < 1)
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



            if (_openedMenu != OpenedMenuMode.Hud)
            {
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
                        ProgressBarAmount = Mathf.MoveTowards(ProgressBarAmount, 1, Time.unscaledDeltaTime * 5);

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
                        ProgressBarAmount = Mathf.MoveTowards(ProgressBarAmount, Convert.ToSingle((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal), Time.unscaledDeltaTime * 2);
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
                    if (GameSetup.IsMpClient)
                    {
                        DifficultySelectionClinet();
                    }
                    else
                    {
                        DifficultySelectionHost();
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


                        switch (_openedMenu)
                        {
                            case OpenedMenuMode.Main:
                                GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));

                                //GUI.Label(new Rect(wholeScreen.center, Vector2.one * 500), "Main");

                                DrawMain();
                                break;
                            case OpenedMenuMode.Inventory:
                                GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));
                                DrawInventory();

                                //GUI.Label(new Rect(wholeScreen.center, Vector2.one * 500), "Inventory");
                                break;
                            case OpenedMenuMode.Hud:
                                InventoryScrollAmount = 0;
                                DrawHUD();
                                break;
                            case OpenedMenuMode.Spells:
                                GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));
                                DrawSpellMenu();

                                break;
                            case OpenedMenuMode.Stats:
                                GUI.DrawTexture(wholeScreen, Res.ResourceLoader.GetTexture(78));
                                DrawGuide();

                                break;
                            case OpenedMenuMode.Perks:
                                DrawPerks();

                                break;
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

                Rect r1 = new Rect(wholeScreen.center, new Vector2(Screen.height / 2, Screen.height / 2));
                Rect r2 = new Rect(r1);
                Rect r3 = new Rect(r1);
                Rect r4 = new Rect(r1);
                float Mindist = 500f / 1500f;
                Mindist *= r1.width;
                MenuButton(Mindist, r1, OpenedMenuMode.Inventory, "Backpack", new Vector2(1, -1), ref Opacity1);
                r2.position = center - r1.size;
                MenuButton(Mindist, r2, OpenedMenuMode.Spells, "Abilities", new Vector2(-1, 1), ref Opacity2);
                r3.position = center - new Vector2(0, r1.width);
                MenuButton(Mindist, r3, OpenedMenuMode.Stats, "Guide", Vector2.one, ref Opacity3);
                r4.position = center - new Vector2(r1.width, 0);
                MenuButton(Mindist, r4, OpenedMenuMode.Perks, "Genetics", -Vector2.one, ref Opacity4);
                GUI.Label(MiddleR, "Level \n" + ModdedPlayer.instance.Level.ToString(), MenuBtnStyle);

                string HudHideStatus = "[ HUD ]";
                if (HideHud)
                {
                    HudHideStatus = "[ NO HUD ]";
                }

                if (GUI.Button(new Rect(Screen.width - 120 * rr, 40 * rr, 120 * rr, 40 * rr), HudHideStatus, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(23 * rr), alignment = TextAnchor.MiddleCenter }))
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
        private void MenuButton(float mindist, Rect rect, OpenedMenuMode mode, string text, Vector2 Scale, ref float Opacity, float r = 1, float g = 1, float b = 1)
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

                GUI.color = new Color(r, g, b, Opacity);
                GUI.DrawTexture(rect, Res.ResourceLoader.instance.LoadedTextures[2]);
                GUI.color = Color.white;

            }
            GUI.matrix = backupMatrix;
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
            if (GUI.Button(new Rect(Screen.width / 2 - 200 * rr, Screen.height - 70 * rr, 400 * rr, 50 * rr), "Item drops on death: "+ ModSettings.dropsOnDeath, new GUIStyle(GUI.skin.button)
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
            LocalPlayer.FpCharacter.LockView(true);
            LocalPlayer.FpCharacter.MovementLocked = true;
            Rect r = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(r, _black);
            GUI.Label(r, "Please wait for the host to choose a difficulty", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, font = MainFont, fontSize = Mathf.RoundToInt(50 * rr) });
            if (requestResendTime <= 0)
            {
                Network.NetworkManager.SendLine("AB", Network.NetworkManager.Target.OnlyServer);
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

            for (int y = 0; y < Inventory.Height; y++)
            {
                for (int x = 0; x < Inventory.Width; x++)
                {
                    try
                    {
                        int index = y * Inventory.Width + x;

                        DrawInvSlot(new Rect(SlotsRect.x + slotDim.x * x, SlotsRect.y + slotDim.y * y + 160 * rr + InventoryScrollAmount, slotDim.x, slotDim.y), index);
                    }
                    catch (Exception ex)
                    {

                        ModAPI.Log.Write(ex.ToString());
                    }

                }
            }
            //PlayerSlots
            Rect eq = new Rect(SlotsRect.xMax + 290 * rr, 0, 420 * rr, Screen.height);
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
                    DraggedItem = null;
                    DraggedItemIndex = -1;
                    isDragging = false;
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
                    GUI.Label(StatRects[i], amount.ToString("N0") + "%", StatValueStyle);
                }
                else
                {
                    GUI.Label(StatRects[i], amount.ToString("N0"), StatValueStyle);
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
            GUI.Label(toolTipTitleRect, "Tooltip:", TooltipStyle);
            GUI.Label(toolTipRect, item.tooltip, TooltipStyle);

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
                    GUI.color = new Color(1, 1, 1, 1);

                    GUI.DrawTexture(itemRect, Inventory.Instance.ItemList[index].icon);
                    if (Inventory.Instance.ItemList[index].Amount > 1)
                    {
                        GUI.Label(r, Inventory.Instance.ItemList[index].Amount.ToString("N0"), new GUIStyle { alignment = TextAnchor.LowerLeft, margin = new RectOffset(Mathf.RoundToInt(10 * rr), 0, 0, Mathf.RoundToInt(10 * rr)), fontSize = Mathf.RoundToInt(12 * rr), font = MainFont, fontStyle = FontStyle.Bold });
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
                                if (index < -1)
                                {

                                    if (canPlace)
                                    {
                                        Item backup = Inventory.Instance.ItemList[index];
                                        Inventory.Instance.ItemList[index] = DraggedItem;
                                        Inventory.Instance.ItemList[DraggedItemIndex] = backup;
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
                                        DraggedItem = null;
                                        DraggedItemIndex = -1;
                                        isDragging = false;
                                    }
                                    else
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
                                            DraggedItem = null;
                                            DraggedItemIndex = -1;
                                            isDragging = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (r.Contains(mousepos))
                        {
                            if (UnityEngine.Input.GetMouseButtonDown(0))
                            {

                                isDragging = true;
                                DraggedItem = Inventory.Instance.ItemList[index];
                                DraggedItemIndex = index;

                            }
                            else if (UnityEngine.Input.GetMouseButtonDown(1) && index > -1)
                            {
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
                                DraggedItem = null;
                                DraggedItemIndex = -1;
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
                            DraggedItem = null;
                            DraggedItemIndex = -1;

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
            GUI.Label(TitleR, title, new GUIStyle(GUI.skin.box) { font = MainFont, fontSize = Mathf.RoundToInt(20 * rr), wordWrap = true, alignment = TextAnchor.MiddleCenter });
            DrawInvSlot(r, index);

        }
        #endregion

        #region HUDMethods
        private void DrawHUD()
        {
            try
            {


                if (HideHud)
                {
                    return;
                }
                GUI.color = new Color(1, 0.5f, 0.7f, 0.5f);
                GUIStyle HitmarkerStyle = new GUIStyle(GUI.skin.label) { font = MainFont, clipping = TextClipping.Overflow, wordWrap = true, alignment = TextAnchor.MiddleCenter };
                for (int i = 0; i < hitMarkers.Count; i++)
                {
                    hitMarkers[i].lifetime -= Time.unscaledDeltaTime;
                    if (hitMarkers[i].lifetime < 0)
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


                float BuffOffset = 0;
                if (ModdedPlayer.instance.Rooted)
                {
                    Rect r = new Rect(0, Screen.height - 30 * rr - BuffOffset, 300 * rr, 30 * rr);
                    string s = string.Format("ROOTED {0} s", Math.Round(ModdedPlayer.instance.RootDuration, 1));
                    GUI.Label(r, s, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, wordWrap = false, font = MainFont, fontSize = Mathf.RoundToInt(rr * 14) });
                    BuffOffset += 15 * rr;
                }
                if (ModdedPlayer.instance.Stunned)
                {
                    Rect r = new Rect(0, Screen.height - 30 * rr - BuffOffset, 300 * rr, 30 * rr);
                    string s = string.Format("STUNNED {0} s", Math.Round(ModdedPlayer.instance.RootDuration, 1));
                    GUI.Label(r, s, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, wordWrap = false, font = MainFont, fontSize = Mathf.RoundToInt(rr * 14) });
                    BuffOffset += 15 * rr;
                }
                foreach (KeyValuePair<int, Buff> buff in BuffDB.activeBuffs)
                {
                    Rect r = new Rect(0, Screen.height - 16 * rr - BuffOffset, 300 * rr, 16 * rr);
                    TimeSpan span = TimeSpan.FromSeconds(buff.Value.duration);

                    string s = buff.Value.BuffName + "   (" + (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()) + ")";

                    if (buff.Value.DisplayAmount)
                    {
                        if (buff.Value.DisplayAsPercent)
                        {
                            s += buff.Value.amount > 0 ? "( +" + (buff.Value.amount - 1) * 100 + "% )" : "( -" + (buff.Value.amount - 1) * 100 + "% )";
                        }
                        else
                        {
                            s += "( " + buff.Value.amount + " )";
                        }
                    }

                    GUI.Label(r, s, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, wordWrap = false, font = MainFont, fontSize = Mathf.RoundToInt(rr * 14) });
                    BuffOffset += 16 * rr;

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
                        GUI.Label(r, ModAPI.Input.GetKeyBindingAsString("spell" + (i + 1).ToString()), new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 18), fontStyle = FontStyle.Normal, alignment = TextAnchor.MiddleCenter });
                        GUI.color = new Color(1, 1, 1, 1f);
                        if (ModdedPlayer.instance.Silenced || ModdedPlayer.instance.Stunned)
                        {
                            GUI.color = Color.black;
                        }
                        else if ((LocalPlayer.Stats.Energy < SpellCaster.instance.infos[i].spell.EnergyCost * (1 - ModdedPlayer.instance.SpellCostToStamina) * (1 - ModdedPlayer.instance.SpellCostRatio) || LocalPlayer.Stats.Stamina < SpellCaster.instance.infos[i].spell.EnergyCost * ModdedPlayer.instance.SpellCostToStamina * (1 - ModdedPlayer.instance.SpellCostRatio)))
                        {
                            GUI.color = Color.blue;

                        }
                        GUI.DrawTexture(r, SpellCaster.instance.infos[i].spell.icon);
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
                float combatHeight = width * 63 / 1500;
                //Defining rectangles to later use to draw HUD elements
                Rect XPbar = new Rect(Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f), Screen.height - height - SquareSize, width, height);
                //Rect XPbar = new Rect(Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f), Screen.height - SquareSize - height, width, height);
                Rect XPbarFill = new Rect(XPbar);
                XPbarFill.width *= ProgressBarAmount;
                Rect CombatBar = new Rect(XPbar.x, 20 * rr, SpellCaster.SpellCount * SquareSize * (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime), combatHeight);
                Rect CombatBarCount = new Rect(XPbar.x, 0, SpellCaster.SpellCount * SquareSize, combatHeight);

                float cornerDimension = Screen.height - XPbar.y;
                Rect LeftCorner = new Rect(XPbar.x - cornerDimension, XPbar.y, cornerDimension, cornerDimension);
                Rect RightCorner = new Rect(XPbar.xMax, XPbar.y, cornerDimension, cornerDimension);
                Rect CombatBarText = new Rect(CombatBarCount)
                {
                    y = CombatBar.yMax,
                    height = 100f * rr
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
                    GUI.Label(CombatBarCount, "+" + ModdedPlayer.instance.NewlyGainedExp + " EXP\tx" + ModdedPlayer.instance.MassacreMultipier, CombatCountStyle);
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
                    RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, Vector3.one, Camera.main.transform.forward, Camera.main.transform.rotation, 10000);
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
                            ClinetEnemyProgression cp = null;
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

                                Rect scanRect = new Rect(0, 0, 60 * rr, 60 * rr)
                                {
                                    center = wholeScreen.center
                                };
                                ScanRotation += Time.deltaTime * 45;
                                Matrix4x4 matrix4X4 = GUI.matrix;
                                GUIUtility.RotateAroundPivot(ScanRotation, scanRect.center);
                                GUI.DrawTexture(scanRect, ResourceLoader.instance.LoadedTextures[24]);

                                GUI.matrix = matrix4X4;

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
                            scannedTransform = hits[enemyHit].transform.root;
                            scannedEntity = scannedTransform.GetComponentInChildren<BoltEntity>();
                            if (scannedEntity == null)
                            {
                                scannedEntity = scannedTransform.GetComponent<BoltEntity>();
                            }
                            ScanTime = 0;

                        }
                    }
                }
                else
                {
                    ScanTime = 0;
                }


            }
            catch
            {

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
            GUIStyle style = new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 50), alignment = TextAnchor.MiddleLeft };
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
                    return;
                }

                //drawing pretty info
                Rect SpellIcon = new Rect(Screen.width / 2 - 100 * rr, 25 * rr, 200 * rr, 200 * rr);
                GUI.DrawTexture(SpellIcon, displayedSpellInfo.icon);
                if (GUI.Button(new Rect(Screen.width - 170 * rr, 25 * rr, 150 * rr, 50 * rr), "GO BACK TO SPELLS\n(Right Mouse Button)", new GUIStyle(GUI.skin.button) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 15) }))
                {
                    displayedSpellInfo = null;
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
                            if (GUI.Button(UnlockRect, "UNLOCK ABILITY", btnStyle))
                            {
                                displayedSpellInfo.Bought = true;
                                ModdedPlayer.instance.MutationPoints -= 2;
                            }
                        }
                        else
                        {
                            GUIStyle morePointsStyle = new GUIStyle(GUI.skin.label) { font = MainFont, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(41 * rr), fontStyle = FontStyle.Bold };
                            morePointsStyle.onNormal.textColor = Color.gray;
                            morePointsStyle.onActive.textColor = Color.white;
                            GUI.Label(UnlockRect, "YOU NEED 2 POINTS TO UNLOCK AN ABILITY", morePointsStyle);
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
            Rect bg = new Rect(0, y, Screen.width * 3 / 5, 100 * rr);
            bg.x = (Screen.width - bg.width) / 2;
            GUI.DrawTexture(bg, Res.ResourceLoader.instance.LoadedTextures[28]);
            Rect nameRect = new Rect(30 * rr + bg.x, y, bg.width / 2, 100 * rr);

            if (s.Levelrequirement > ModdedPlayer.instance.Level)
            {
                GUI.color = Color.gray;
            }
            else
            {
                if (bg.Contains(mousepos))
                {
                    if (displayedSpellInfo == null)
                    {
                        style.fontStyle = FontStyle.Bold;
                        if (UnityEngine.Input.GetMouseButtonDown(0))
                        {

                            displayedSpellInfo = SpellDataBase.spellDictionary[s.ID];
                        }
                    }
                }
            }
            GUI.Label(nameRect, s.Name + "  (LEVEL: " + s.Levelrequirement + ")", style);
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
                Rect labelRect = new Rect(50 * rr + GuideWidthDecrease * rr + GuideMargin * rr, BookPositionY, Screen.width - 2 * rr * (GuideMargin + GuideWidthDecrease) - 100 * rr, statStyle.fontSize);
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
                fontSize = Mathf.RoundToInt(30 * rr),
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
                fontSize = Mathf.RoundToInt(33 * rr),
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

            Header("Me");
            MarkBookmark("Home");
            Label("\tExperience");
            Stat("Current level", ModdedPlayer.instance.Level.ToString("N0"));
            Stat("Current experience", ModdedPlayer.instance.ExpCurrent.ToString("N0"));
            Stat("Experience goal", ModdedPlayer.instance.ExpGoal.ToString("N0"), "Next level: " + (ModdedPlayer.instance.Level + 1) + " you will need to get this amount of experience:\t " + ModdedPlayer.instance.GetGoalExp(ModdedPlayer.instance.Level + 1).ToString("N0"));
            Stat("Progress amount: ", (((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal) * 100).ToString() + "%");
            Label("\tLevel is the estimation of my power. I must become stronger to survive." +
                "\nHigher level allow me to equip better equipement. " +
                "\nLeveling up gives me the ability to develop usefull abilities. (Currently you have " + ModdedPlayer.instance.MutationPoints + " mutation points), which you can spend on unlocking spells or perks. " +
                "\nStronger bodies require more energy to live. Resource usage increases. With every level, your food and water depletion rate increases by 4% (increases by 100% every 25 levels)" +
                "This property multiplies with your thirst rate and hunger rate stat.");
            Space(50);
            Label("\nWhat to do in order to get stronger:" +
                "\n-Kill enemies - combat is the most reliable way of progression." +
                "\n-Hunt animals - experience gained does not increase with difficulty." +
                "\n-Cut down bushes and breaking effigies" +
                "\n-Chop trees" +
                "\n-Eat rare consumabes\n" +
                "\nExperience from kills can be increased by quickly killing multipe enemies." +
                "\nThe gray bar that appears on screen after killing an enemy sygnalizes how much time you have left to sustain your killing spree. You can make the massacre last longer by equiping items with correct stats. Killing a enemy while the there is still time left will result in adding some time to the duration (it will increase the time left by " + ModdedPlayer.instance.TimeBonusPerKill + " seconds). " +
                "\nWhen the time runs out, all the exp you gained will be multipied by the some multipier that is dependant on the size of your streak. Then the experience is added to your exp pool and the kill streak is reset");
            Stat("Massacre duration", ModdedPlayer.instance.MaxMassacreTime.ToString() + " s");
            Stat("Time on kill", ModdedPlayer.instance.TimeBonusPerKill.ToString() + " s");
            Space(300);

            Header("Statistics");
            Stat("Strenght", ModdedPlayer.instance.strenght.ToString("N0") + " str", "Increases melee damage by " + ModdedPlayer.instance.DamagePerStrenght * 100 + "% for every 1 point of strenght. Current bonus melee damage from strenght [" + ModdedPlayer.instance.strenght * 100 * ModdedPlayer.instance.DamagePerStrenght + "]");
            Stat("Agility", ModdedPlayer.instance.agility.ToString("N0") + " agi", "Increases ranged damage by " + ModdedPlayer.instance.RangedDamageperAgi * 100 + "% for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.instance.agility * 100 * ModdedPlayer.instance.RangedDamageperAgi + "]\n" +
                "Increases maximum energy by " + ModdedPlayer.instance.EnergyPerAgility + " for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.instance.agility * ModdedPlayer.instance.EnergyPerAgility + "]");
            Stat("Vitality", ModdedPlayer.instance.vitality.ToString("N0") + " vit", "Increases health by " + ModdedPlayer.instance.HealthPerVitality + "% for every 1 point of vitality. Current bonus health from vitality [" + ModdedPlayer.instance.vitality * ModdedPlayer.instance.HealthPerVitality + "]");
            Stat("Intelligence", ModdedPlayer.instance.intelligence.ToString("N0") + " int", "Increases spell damage by " + ModdedPlayer.instance.SpellDamageperInt * 100 + "% for every 1 point of intelligence. Current bonus spell damage from intelligence [" + ModdedPlayer.instance.intelligence * 100 * ModdedPlayer.instance.SpellDamageperInt + "]\n" +
                "Increases stamina regen by " + ModdedPlayer.instance.EnergyRegenPerInt * 100 + "% for every 1 point of intelligence. Current bonus stamina regen from intelligence [" + ModdedPlayer.instance.intelligence * 100 * ModdedPlayer.instance.EnergyRegenPerInt + "]");


            Space(60);
            Image(105, 70);
            Label("Health & Energy");
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


            Space(20);
            Image(89, 70);
            Header("Melee");
            Space(10);

            Stat("Melee damage", Math.Round(ModdedPlayer.instance.MeleeAMP * 100, 2) + "%", "Melee damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
                "Bonus from strenght: " + Math.Round(ModdedPlayer.instance.strenght * ModdedPlayer.instance.DamagePerStrenght * 100, 2) + "%\n" +
                "Melee damage amplification: " + Math.Round((ModdedPlayer.instance.MeleeDamageAmplifier - 1) * 100, 2) + "%\n" +
                "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
            Stat("Additional melee weapon damage", Math.Round(ModdedPlayer.instance.MeleeDamageBonus) + "", "Melee damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to weapon damage and multiplied by the stat above");
            Stat("Melee range", Math.Round(ModdedPlayer.instance.MeleeRange * 100) + "%");

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
            Stat("Projectile size", Math.Round(ModdedPlayer.instance.ProjectileSpeedRatio * 100) + "%", "Bigger projectiles allow to land headshots easier. Most projectiles still can hit only 1 target.");


            Space(20);
            Image(110, 70);
            Header("Magic");
            Space(10);

            Stat("Spell damage", Math.Round(ModdedPlayer.instance.SpellAMP * 100, 2) + "%", "Spell damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
             "Bonus from intelligence: " + Math.Round(ModdedPlayer.instance.intelligence * ModdedPlayer.instance.SpellDamageperInt * 100, 2) + "%\n" +
             "Spell damage amplification: " + Math.Round((ModdedPlayer.instance.SpellDamageAmplifier - 1) * 100, 2) + "%\n" +
             "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
            Stat("Additional spell damage", Math.Round(ModdedPlayer.instance.SpellDamageBonus) + "", "Spell damage bonus can be increased by perks and inventory items. This is added to spell damage and multiplied by the stat above. Often spells take a fraction of this stat and add it to spell's damage.");
            Stat("Spell cost reduction", Math.Round((1 - ModdedPlayer.instance.SpellCostRatio) * 100) + "%", "");
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

            Stat("Movement speed", Math.Round(ModdedPlayer.instance.MoveSpeed * 100) + "% ms", "Multipier of base movement speed. Base walking speed is equal to " + FPCharacterMod.basewalkSpeed + " units per second, with bonuses it's " + FPCharacterMod.basewalkSpeed * ModdedPlayer.instance.MoveSpeed + " units per second");
            Stat("Jump power", Math.Round(ModdedPlayer.instance.MoveSpeed * 100) + "%", "Multipier of base jump power. Increases height of your jumps");
            Stat("Hunger rate", (1 / ModdedPlayer.instance.HungerRate) * 100 + "%", "How much slower is the rate of consuming food compared to normal. Every level you gain increases hunger rate by " + ModdedPlayer.HungerPerLevelRateMult * 100 + "%, but several levels it's not changed. Currently, hunger rate from level is equal to " + (Mathf.Max(1, ModdedPlayer.instance.Level * ModdedPlayer.HungerPerLevelRateMult)) * 100 + "%.\nFinal hunger rate is " + (Mathf.Max(1, ModdedPlayer.instance.Level * ModdedPlayer.HungerPerLevelRateMult)) * ModdedPlayer.instance.HungerRate * 100 + "% faster than normal");
            Stat("Thirst rate", (1 / ModdedPlayer.instance.ThirstRate) * 100 + "%", "How much slower is the rate of consuming water compared to normal. Every level you gain increases thirst rate by " + ModdedPlayer.ThirstPerLevelRateMult * 100 + "%, but several levels it's not changed. Currently, thirst rate from level is equal to " + (Mathf.Max(1, ModdedPlayer.instance.Level * ModdedPlayer.ThirstPerLevelRateMult)) * 100 + "%.\nFinal thirst rate is " + (Mathf.Max(1, ModdedPlayer.instance.Level * ModdedPlayer.ThirstPerLevelRateMult)) * ModdedPlayer.instance.ThirstRate * 100 + "% faster than normal");
            Stat("Experience gain", ModdedPlayer.instance.ExpFactor * 100 + "%", "Multipier of any experience gained");
            Stat("Massacre duration", ModdedPlayer.instance.MaxMassacreTime + " s", "How long massacres can last");
            Stat("Time on kill", ModdedPlayer.instance.TimeBonusPerKill + " s", "Amount of time that is added to massacre for every kill");


            Space(40);
            Image(90, 70);
            Header("Inventory Stats");
            Space(10);
            foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> pair in ModdedPlayer.instance.ExtraCarryingCapactity)
            {
                string item_name = Scene.HudGui.GetItemName(pair.Value.ID, (pair.Value.Amount > 1), false);
                Stat(item_name, "+" + pair.Value.Amount, "How many extra '" + item_name + "' you can carry. Item ID is " + pair.Value.ID);
            }


            Space(200);
            Header("Differences");
            Label("Champions of The Forest provides variety of changes to in-game mechanics." +
                "\nArmor no longer absorbs all damage. Instead it reduces the damage by 70%." +
                "\nTraps no longer instantly kill cannibals. Instead they deal damage." +
                "\nDynamite no longer instantly kills enemies." +
                "\nEnemies have armor and increased health." +
                "\nPlayers take increased damage from explosives." +
                "\nPlayer deal increased damage to other players.");



            Space(100);
            Header("Enemies");
            Label("\tEnemies in the forest have evolved. They become faster and stronger, the more experience they get. But speed and strenght alone shouldn't be my main concern. There are a lot more dangerous beings out there." +
                "\n\n" +
                "Normal enemies changed slightly. Their health has drastically increased. I was not able to find the cause of such change. I must keep looking for the answer.\n" +
                "Apart from increased health, enemies have armor. It noticeably reduces damage when dealing with stronger enemies." +
                "\nThe easiest way to reduce one's armor is to ignite them. Fire additionally works as crowd controll, rendering some burning enemies incapable to fighting back.\n" +
                "Other way to reduce armor is to equip items, which reduce armor on hit.\n" +
                "If you dont have any way to reduce enemy's armor, damaging them with spells would ignore it on the whole.");
            Space(30);
            Label("Elite enemies\n" +
                "An elite is a rare enemy with increased stats and special abilities, that force me to pick a strategy of dealing with them." +
                "Enemy abilities:");
            Label("- Steadfast - This defensive ability causes enemy to reduce all damage exceeding a percent of their maximum health. To deal with this kind of ability, damage over time and fast attacks are recommended. This ability counters slow but powerful sources of damage.");
            Label("- Blizzard - A temporary aura around an enemy that slows anyone in its area of effect. Affects movement speed and attack speed. Best way to deal with this is to avoid getting in range. Crowd controll from ranged abilities and running seems like the best option.");
            Label("- Radiance - A permanent aura around an enemy. It deals damage anyone around. The only way of dealing with this is to never get close to the enemy.");
            Label("- Chains - Roots anyone in a big radius from the enemy. The duration this root increases with difficulty. Several abilities that provide resistance to crowd controll clear the effects of this ability.");
            Label("- Black hole - A very strong ability. Only engage in battle with enemy if I am certain of my strenght. It provides crowd controll and deals a lot of damage. The crowd controll can be avoided by running away from the area of effect of the spell or teleport outside of it. The only weak side of this spell is the high cooldown.");
            Label("- Trap sphere - Long lasting sphere that forces me to stay inside it untill its effects wears off");
            Label("- Juggernaut - The enemy is completely immune to crowd controll.\n");
            Label("- Gargantuan - Describes an enemy that is bigger, faster, stronger and has more health.");
            Label("- Tiny - An enemy has decreased size. It's harder to hit it with ranged attacks and most of the melee weapons can only attack the enemy with slow smashes. Tiny enemies are slower.");
            Label("- Extra tough - enemy has a lot more healt");
            Label("- Extra deadly - enemy has a lot more damage");
            Label("- Basher - the enemy stuns on hit. Best way to fight it is to not get hit.");
            Label("- Warping - An ability allowing to teleport. Strong agnist glass cannon builds, running away and ranged attacks. Weak agnist melee strikes and a lot of durability.");
            Label("- Rain Empowerment - During the rain, the enemy gains in strengt, speed, armor and size.");
            Label("- Meteors - Periodically spawns a rain of damaging meteors. They are rather easy to spot and move slowly. If I start running early enough, I should not have any trouble with avoiding them");
            Label("- Flare - The same ability that I can use. Slows and damages me if I stand inside. Heals and makes enemies faster.");
            Label("- Undead - An enemy upon dieing restores portion of it's health, gets stronger and bigger.");
            Label("- Plasma cannon - Creates a turret that fires a laser beam that damages me and buildings.");
            Label("- Poisonous - Enemies gain attack modifier that on every hit applies a stacking debuff that deals damage over time. Once I get hit with this, I need to retreat and wait for the poison to end.");



            Space(100);
            Header("Items");
            Label("\tEquipement can be obtained by killing enemies. Normal enemies can drop a few items on death, if the odds are correct. The chance to get any items from a normal enemy is 10%. The amount of items obtained from normal enemies is at least 1 and maximum of 5\n" +
                "Elite enemies have a 100% chance to drop an item and they usually drop more.\n" +
                "\tItems can be equipped by dragging and dropping them onto a right equipement slot. The item will grant it's stats only if I met the level requirements. The best rarity of items are only obtainable on high difficulties. Through there is a really small chance to get a legendary item on normal.");

            Space(200);
            Header("Dairy");
            Stat("Day 0", "");
            Label("I barely survived the plane crash. Shortly after hitting the ground I lost consciousness. I remember my son Timmy being taken by a red human, and nothing else. I need to find my boy...\n");
            if (LocalPlayer.Stats.DaySurvived < 1)
            {
                return;
            }

            Stat("Day 1", "");
            Label("There is something weird about this island. I swear I have seen some people. They did not look friendly. I'd better stay on guard.\n");
            if (LocalPlayer.Stats.DaySurvived < 2)
            {
                return;
            }

            Stat("Day 2", "");
            Label("They are clearly hostile towards me. They are horrifying. They are cannibals. I need to find a way to defend myself.\n");
            if (LocalPlayer.Stats.DaySurvived < 10)
            {
                return;
            }

            Stat("Day 10", "");
            Label("Something is seriousely wrong about this place. Those creatures... I started seeing them on the surface. They appear to have human elements, but they are definitely monsters. They are extremely hostile. \n I need to get stronger or else i'll get slaughtered here.");
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
                _perkDetailAlpha += Time.unscaledDeltaTime;
                if (_perkDetailAlpha >= 0.7f)
                {
                    GUI.color = new Color(_perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f);

                    Rect Name = new Rect(r.x - 200 * rr, r.y - 130 * rr, 400 * rr + r.width, 90 * rr);

                    GUI.Label(Name, p.Name, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(40 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });

                    Rect Desc = new Rect(r.x - 200 * rr, r.yMax + 30 * rr, 400 * rr + r.width, 1000 * rr);

                    string desctext = p.Description;

                    if (!p.IsBought || p.Endless)
                    {
                        desctext = "Press to buy\n" + p.Description;
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

                                ModdedPlayer.instance.MutationPoints -= p.PointsToBuy;
                                Perk.AllPerks[SelectedPerk_ID].ApplyMethods();
                                Perk.AllPerks[SelectedPerk_ID].Applied = true;
                                Buying = false;
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

    }
}
