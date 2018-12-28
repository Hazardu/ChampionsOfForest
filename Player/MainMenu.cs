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
            "Difficulty for beginners \n Enemy stats at 100% of their base value \n Exp rate - 100% \n Loot rarity - 100%",
            "Difficulty for somewhat intermediate players \n Enemy stats at 300% of their base value \n Exp rate - 200% \n Loot rarity - 200%",
            "Difficulty for levels 30-50 \n Enemy stats at 1 200% of their base value \n Exp rate - 900% \n Loot rarity - 900%",
            "Difficulty for levels 50+ with good equipement \n Enemy stats at 15 000% of their base value \n Exp rate - 10000% \n Loot rarity - 10 000%",
            "Challenge I-V unlock best tier item drops \n Enemy stats at 45 000% of their base value \n Exp rate - 40 000% \n Loot rarity - 40 000%",
            "Challenge I-V unlock best tier item drops \n Enemy stats at 100 000% of their base value \n Exp rate - 85 000% \n Loot rarity - 85 000%",
            "Challenge I-V unlock best tier item drops \n Enemy stats at 250 000% of their base value \n Exp rate - 40 000% \n Loot rarity - 40 000%",
            "Challenge I-V unlock best tier item drops \n Enemy stats at 1 000 000% of their base value \n Exp rate - 500 000% \n Loot rarity - 500 000%",
            "Challenge I-V unlock best tier item drops \n Enemy stats at 10 000 000% of their base value \n Exp rate - 1 000 000% \n Loot rarity - 1 000 000%",

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

        private Rect HUDenergyLabelRect;
        private Rect HUDHealthLabelRect;

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
        float PerkHexagonSide = 60;
        float PerkHeight;
        float PerkWidth;

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
            new Color(0.1f,0.1f,0.9f),
            new Color(0.1f,0.1f,0.9f),
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


        public Dictionary<int,HitMarker> hitMarkers =new Dictionary<int, HitMarker>();
        public class HitMarker
        {
           public int txt;
            public Vector3 worldPosition;
            public float lifetime;
            private int ID;
            public void Delete()
            {
                Instance.hitMarkers.Remove(ID);
            }
            public HitMarker(int t,Vector3 p)
            {
                txt = t;
                worldPosition = p;
                lifetime = 5f;
                ID = Instance.hitMarkers.Count;
                Instance.hitMarkers.Add(ID,this);
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
                 PerkHeight = PerkHexagonSide * 2*rr;
                 PerkWidth = PerkHexagonSide * 1.732050f*rr; //times sqrt(3)

                //HUD
                HideHud = false;
                HUDenergyLabelRect = new Rect(Screen.width - 500 * rr, Screen.height - 100 * rr, 500 * rr, 100 * rr);
                HUDHealthLabelRect = new Rect(Screen.width - 500 * rr, Screen.height - 140 * rr, 500 * rr, 100 * rr);

                //The main font as of now is Gabriola
                MainFont = Font.CreateDynamicFontFromOSFont("Bahnschrift", Mathf.RoundToInt(24 * rr));
                if(MainFont ==null) MainFont = Font.CreateDynamicFontFromOSFont("Arial", Mathf.RoundToInt(24 * rr));

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
            }
            catch (Exception ex)
            {
                ModAPI.Log.Write("ERROR: Failure in start of Main Menu: " + ex.ToString());
            }
        }

        private void Update()
        {
            if (_openedMenu != OpenedMenuMode.Hud)
            {
                // LocalPlayer.FpCharacter.LockView(true);
            }
            Crouching = LocalPlayer.FpCharacter.crouching;
            if (ModAPI.Input.GetButtonDown("MenuToggle"))
            {
                MenuKeyPressAction();
            }

            try
            {
     if (UnityEngine.Input.GetKeyDown(KeyCode.F5))
            {
                Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(UnityEngine.Random.Range(300, 2000)), LocalPlayer.Transform.position + Vector3.up + LocalPlayer.Transform.forward);
            }
            }
            catch (Exception e)
            {

                ModAPI.Log.Write(e.ToString());
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
                ////PlayerInventoryMod.Rot = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.x, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.y, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.z, -180, 180));
                ////GUILayout.Label(PlayerInventoryMod.Rot.ToString());
                //foreach (var item in PlayerInventoryMod.customWeapons.Values)
                //{
                //    item.obj.transform.localPosition = PlayerInventoryMod.OriginalOffset + PlayerInventoryMod.Pos + item.offset;
                //    item.obj.transform.localRotation = PlayerInventoryMod.originalRotation;
                //    item.obj.transform.Rotate(PlayerInventoryMod.Rot + item.rotation, Space.Self);
                //}

                //BIG OFFSET-----------------------------------------------------------------------------
                //  PlayerInventoryMod.Pos = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.x, -5, 5), GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.y, -5, 5), GUILayout.HorizontalSlider(PlayerInventoryMod.Pos.z, -5, 5));
                //GUILayout.Label(PlayerInventoryMod.Pos.ToString());
                //PlayerInventoryMod.Rot = new Vector3(GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.x, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.y, -180, 180), GUILayout.HorizontalSlider(PlayerInventoryMod.Rot.z, -180, 180));
                //GUILayout.Label(PlayerInventoryMod.Rot.ToString());
                //foreach (var item in PlayerInventoryMod.customWeapons)
                //{
                //    item.obj.transform.localPosition = PlayerInventoryMod.OriginalOffset + PlayerInventoryMod.Pos + item.offset;
                //    item.obj.transform.localRotation = PlayerInventoryMod.originalRotation;
                //    item.obj.transform.Rotate(PlayerInventoryMod.Rot + item.rotation, Space.Self);
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
                            fontSize = Mathf.RoundToInt(37 * rr),
                            wordWrap = false,
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
                                DrawStats();

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
                MenuButton(Mindist, r3, OpenedMenuMode.Stats, "Diary", Vector2.one, ref Opacity3);
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
                Opacity = Mathf.Clamp01(Opacity + Time.deltaTime * OpacityGainSpeed);

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(FadeMenuSwitch(mode));
                }
            }
            else
            {
                Opacity = Mathf.Clamp01(Opacity - Time.deltaTime * OpacityGainSpeed);
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
                    fontSize = Mathf.FloorToInt(30 * rr) }))
                { ModSettings.FriendlyFire = !ModSettings.FriendlyFire; }
            }
            else
            {
                GUI.color = Color.gray;
                if (GUI.Button(new Rect(Screen.width / 2 - 200 * rr, Screen.height - 120 * rr, 400 * rr, 50 * rr), "Friendly Fire disabled", new GUIStyle(GUI.skin.button)
                {
                    font = MainFont,
                    fontSize = Mathf.FloorToInt(30 * rr) }))
                { ModSettings.FriendlyFire = !ModSettings.FriendlyFire; }
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
            head.y -=3.5f*head.height;

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
                    Inventory.Instance.DropItem(DraggedItemIndex);
                    DraggedItem = null;
                    DraggedItemIndex = -1;
                    isDragging = false;
                }
            }
        }




        private void DrawItemInfo(Vector2 pos, Item item)
        {
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
            GUIStyle ItemNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(45 * rr),fontStyle = FontStyle.Bold, font = MainFont };
            float y = 70 + pos.y;
            Rect[] StatRects = new Rect[item.Stats.Count];
            GUIStyle StatNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(26 * rr), font = MainFont };
            GUIStyle StatValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = Mathf.RoundToInt(26 * rr),fontStyle = FontStyle.Bold, font = MainFont };

            for (int i = 0; i < StatRects.Length; i++)
            {
                StatRects[i] = new Rect(pos.x, y, width, 40 * rr);
                y += 42 * rr;
            }
            y += 35 * rr;

            Rect LevelAndTypeRect = new Rect(pos.x, y, width, 35 * rr);
            GUIStyle TypeStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(25 * rr), font = MainFont };
            GUIStyle LevelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerRight, fontSize = Mathf.RoundToInt(28 * rr), font = MainFont, fontStyle = FontStyle.Italic };
            y += 35 * rr;

            GUIStyle DescriptionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(30 * rr), font = MainFont };
            GUIStyle LoreStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(27 * rr), font = MainFont, fontStyle = FontStyle.Italic };
            GUIStyle TooltipStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(30 * rr), font = MainFont, fontStyle = FontStyle.Bold };

            Rect DescrRect = new Rect(pos.x, y, width, DescriptionStyle.CalcHeight(new GUIContent(item.description), width)); y += DescrRect.height;
            y += 35 * rr;

            Rect LoreRect = new Rect(pos.x, y, width, LoreStyle.CalcHeight(new GUIContent(item.lore), width)); y += LoreRect.height;
            y += 35 * rr;

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
                    GUI.Label(StatRects[i], amount.ToString() + "%", StatValueStyle);
                }
                else
                {
                    GUI.Label(StatRects[i], amount.ToString(), StatValueStyle);
                }
            }
            GUI.color = Color.white;
            switch (item._itemType)
            {
                case BaseItem.ItemType.Shield:
                    GUI.Label(LevelAndTypeRect, "Shield", TypeStyle);
                    break;
                case BaseItem.ItemType.Offhand:
                    GUI.Label(LevelAndTypeRect, "Offhand", TypeStyle);
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
                    }

                    GUI.DrawTexture(itemRect, Inventory.Instance.ItemList[index].icon);
                    GUI.color = new Color(1, 1, 1, 1);
                    if (isDragging)
                    {
                        if (r.Contains(mousepos) && UnityEngine.Input.GetMouseButtonUp(0))
                        {
                            SelectedItem = index;
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
                                        if (DraggedItem._itemType == BaseItem.ItemType.Offhand || DraggedItem._itemType == BaseItem.ItemType.Shield)
                                        {
                                            canPlace = true;
                                        }

                                        break;
                                }
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
                                    if (DraggedItem._itemType == BaseItem.ItemType.Weapon || DraggedItem._itemType == BaseItem.ItemType.Offhand || DraggedItem._itemType == BaseItem.ItemType.Shield)
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
                GUI.color = new Color(1,0f, 0f,0.7f);
                GUIStyle HitmarkerStyle = new GUIStyle(GUI.skin.label) { font = MainFont, clipping = TextClipping.Overflow,wordWrap=true, alignment = TextAnchor.MiddleCenter };
                foreach (KeyValuePair<int,HitMarker> pair in hitMarkers)
                {
                    hitMarkers[pair.Key].lifetime -= Time.deltaTime;
                    if (hitMarkers[pair.Key].lifetime < 0)
                    {
                        hitMarkers[pair.Key].Delete();
                    }
                    else
                    {
                        float distance = Vector3.Distance(Camera.main.transform.position, pair.Value.worldPosition);
                        Vector3 pos = Camera.main.WorldToScreenPoint(pair.Value.worldPosition);
                        pos.y = Screen.height - pos.y;
                        float size = Mathf.Clamp(800 / distance, 10, 80);
                        size *= rr;
                        Rect r = new Rect(0, 0, 400, size);
                        r.center = pos;

                        GUI.Label(r, pair.Value.txt.ToString(),new GUIStyle(HitmarkerStyle) { fontSize = (int)size});
                    }

                }
                GUI.color = Color.white;


                float BuffOffset = 0;
                foreach (KeyValuePair<int, Buff> buff in BuffDB.activeBuffs)
                {
                    Rect r = new Rect(0, Screen.height - 30 * rr - BuffOffset, 300 * rr, 30 * rr);
                    string s = string.Format("BUFF: {0} , {1} seconds, {2}%", buff.Value.BuffName, buff.Value.duration, buff.Value.amount * 100);
                    GUI.Label(r, s, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, wordWrap = false, font = MainFont, fontSize = Mathf.RoundToInt(rr * 20) });
                    BuffOffset += 30 * rr;

                }

                GUI.color = Color.blue;
                GUI.Label(HUDenergyLabelRect, Mathf.Floor(LocalPlayer.Stats.Stamina) + "/" + ModdedPlayer.instance.MaxEnergy, HUDStatStyle);
                GUI.color = new Color(0, 0.8f, 0.1f);

                GUI.Label(HUDHealthLabelRect, Mathf.Floor(LocalPlayer.Stats.Health) + "/" + ModdedPlayer.instance.MaxHealth, HUDStatStyle);
                GUI.color = Color.white;

                float SquareSize = 50 * rr;
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
                        GUI.DrawTexture(r, SpellCaster.instance.infos[i].spell.icon);
                        if (!SpellCaster.instance.infos[i].spell.Bought)
                        {

                        }
                        else
                        {
                            Rect fillr = new Rect(r);
                            float f = SpellCaster.instance.infos[i].Cooldown / SpellCaster.instance.infos[i].spell.BaseCooldown;
                            fillr.height *= f;
                            fillr.y += SquareSize * (1 - f);
                            GUI.DrawTexture(fillr, _SpellCoolDownFill, ScaleMode.ScaleAndCrop);

                        }
                    }
                    GUI.DrawTexture(r, _SpellFrame);

                }
                float width = SpellCaster.SpellCount * SquareSize;
                float height = width * 0.1f;
                float combatHeight = width * 63 / 1500;
                Rect XPbar = new Rect(Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f), Screen.height - SquareSize - height, width, height);
                Rect XPbarFill = new Rect(XPbar);
                XPbarFill.width *= (float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal;
                Rect CombatBar = new Rect(XPbar.x, 20 * rr, SpellCaster.SpellCount * SquareSize * (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime), combatHeight);
                Rect CombatBarCount = new Rect(XPbar.x, 0, SpellCaster.SpellCount * SquareSize, combatHeight);
                
                Rect CombatBarText = new Rect(CombatBarCount)
                {
                    y = CombatBar.yMax,
                    height = 100f*rr
                };
                Rect CombatBarTimer = new Rect(CombatBar.xMax,CombatBar.y,300, combatHeight);
                 GUIStyle CombatCountStyle = new GUIStyle(GUI.skin.label) { font = MainFont,fontSize = Mathf.FloorToInt(19*rr),alignment = TextAnchor.MiddleCenter };
                GUI.DrawTexture(XPbar, _expBarBackgroundTex, ScaleMode.ScaleAndCrop, true, 1500 / 150);
                GUI.DrawTextureWithTexCoords(XPbarFill, _expBarFillTex, new Rect(0, 0, (float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal, 1));
                GUI.DrawTexture(XPbar, _expBarFrameTex, ScaleMode.ScaleAndCrop, true, 1500 / 150);
                // GUIStyle expInfoStyle = new GUIStyle(GUI.skin.label)
                //{
                //    font = MainFont,
                //    fontSize = Mathf.RoundToInt(22 * rr),
                //    alignment = TextAnchor.LowerCenter,
                //    wordWrap = false,
                //    clipping = TextClipping.Overflow,
                //};
                //GUI.Label(XPbar, ModdedPlayer.instance.ExpCurrent + "/" + ModdedPlayer.instance.ExpGoal + "       " + Mathf.Floor(ModdedPlayer.instance.ExpCurrent * 100 / ModdedPlayer.instance.ExpGoal) + "%", expInfoStyle);
                if (ModdedPlayer.instance.TimeUntillMassacreReset > 0)
                {
                    GUI.DrawTextureWithTexCoords(CombatBar, _combatDurationTex, new Rect(0, 0, (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime), 1));
                    GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                    GUI.Label(CombatBarCount, "+" + ModdedPlayer.instance.NewlyGainedExp + " EXP", CombatCountStyle);
                    GUI.color = new Color(1, 1, 1, 1f);
                    GUI.Label(CombatBarTimer, Math.Round(ModdedPlayer.instance.TimeUntillMassacreReset,1).ToString() + " sec", new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.FloorToInt(19 * rr), alignment = TextAnchor.MiddleLeft });
                    GUI.color = new Color(0, 0f, 0f, (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.instance.MaxMassacreTime));
                    string content = ModdedPlayer.instance.MassacreText;
                    if (ModdedPlayer.instance.MassacreKills > 6)
                    {
                        content += "\t" + ModdedPlayer.instance.MassacreKills + " kills";
                    }


                    GUI.Label(CombatBarText, content, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.FloorToInt(45 * rr), alignment = TextAnchor.UpperCenter, clipping = TextClipping.Overflow, richText = true, wordWrap = false });
                    GUI.color = new Color(1, 1, 1, 1f);
                }

                if (LocalPlayer.FpCharacter.crouching)
                {
                    RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 10000);
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
                        ScanTime += Time.deltaTime;
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
                            if (cp != null)
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
                                    DrawScannedEnemyLabel(cp.Health + "/" + cp.MaxHealth + " ♥", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                    y += rr * 60;
                                }
                                if (ScanTime > 3f)
                                {
                                    DrawScannedEnemyLabel("Armor: " + cp.Armor, new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                    y += rr * 60;
                                    if (cp.ArmorReduction != 0)
                                    {
                                        GUI.color = Color.red;
                                        DrawScannedEnemyLabel("Armor debuff: -" + cp.ArmorReduction, new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                        y += rr * 60;
                                        GUI.color = Color.white;
                                    }
                                }
                                if (ScanTime > 4.5f)
                                {
                                    DrawScannedEnemyLabel("Bounty: " + cp.ExpBounty, new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
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
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.SteadFest:
                                                    DrawScannedEnemyLabel("Stead fest", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.BossSteadFest:
                                                    DrawScannedEnemyLabel("Boss ability: Stead fest", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.EliteSteadFest:
                                                    DrawScannedEnemyLabel("Elite ability: Stead fest", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Molten:
                                                    DrawScannedEnemyLabel("Molten", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.FreezingAura:
                                                    DrawScannedEnemyLabel("Absolute zero", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.FireAura:
                                                    DrawScannedEnemyLabel("Radiance", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Rooting:
                                                    DrawScannedEnemyLabel("Chains", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.BlackHole:
                                                    DrawScannedEnemyLabel("Gravity manipulation", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Trapper:
                                                    DrawScannedEnemyLabel("Trapper", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Juggernaut:
                                                    DrawScannedEnemyLabel("Unstoppable force", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Huge:
                                                    DrawScannedEnemyLabel("Gargantuan", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Tiny:
                                                    DrawScannedEnemyLabel("Tiny", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.ExtraDamage:
                                                    DrawScannedEnemyLabel("Extra deadly", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.ExtraHealth:
                                                    DrawScannedEnemyLabel("Extra tough", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Illusionist:
                                                    DrawScannedEnemyLabel("Illusionist", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Blink:
                                                    DrawScannedEnemyLabel("Warping", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Thunder:
                                                    DrawScannedEnemyLabel("Thunder", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.RainEmpowerement:
                                                    DrawScannedEnemyLabel("Rain empowerment", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Shielding:
                                                    DrawScannedEnemyLabel("Refraction", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Meteor:
                                                    DrawScannedEnemyLabel("Chaos meteor", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.RockTosser:
                                                    DrawScannedEnemyLabel("Boulder tosser", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.DoubleLife:
                                                    DrawScannedEnemyLabel("Undead", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                                case EnemyProgression.Abilities.Laser:
                                                    DrawScannedEnemyLabel("Plasma cannon", new Rect(origin.x, origin.y + y, 250 * rr, 65 * rr), infoStyle);
                                                    y += rr * 50;
                                                    break;
                                            }

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
            catch (Exception ex)
            {

                ModAPI.Log.Write("ERROR: FAILURE IN DRAW HUD METHOD \n " + ex.ToString());
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
        private IEnumerator FadeMenuSwitch(OpenedMenuMode mode)
        {
            MenuInteractable = false;
            float alpha = 0;
            while (alpha < 1)
            {
                alpha = Mathf.Clamp(alpha + Time.deltaTime * DarkeningSpeed, 0, 1);
                _blackTexture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
                _blackTexture.Apply();
                yield return null;
            }

            _openedMenu = mode;
            yield return null;
            while (alpha > 0)
            {
                alpha = Mathf.Clamp(alpha - Time.deltaTime * DarkeningSpeed, 0, 1);
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
            foreach (System.Collections.Generic.KeyValuePair<int, Spell> pair in SpellDataBase.spellDictionary)
            {
                DrawSpell(ref y, pair.Value, new GUIStyle( style));
            }
            if (displayedSpellInfo == null)
            {
                //scrolling the list
                spellOffset += UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 140;
                spellOffset = Mathf.Clamp(spellOffset, -100 * rr, 100 * rr * (SpellDataBase.spellDictionary.Count + 4));
            }
            else
            {
                //background effect

                semiblackValue += Time.deltaTime / 5;
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

                GUI.Label(new Rect(Screen.width / 2 - 300 * rr, 225 * rr, 600 * rr, 70 * rr), displayedSpellInfo.Name, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 50), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
                GUI.DrawTexture(new Rect(Screen.width / 2 - 150 * rr, 325 * rr, 300 * rr, 35 * rr), Res.ResourceLoader.instance.LoadedTextures[30]);
                GUI.Label(new Rect(Screen.width / 2 - 300 * rr, 370 * rr, 600 * rr, 400 * rr), displayedSpellInfo.Description + "\nStamina cost:  " + displayedSpellInfo.EnergyCost + "\nRequired level:  " + displayedSpellInfo.Levelrequirement, new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 32), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });

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
                                if (GUI.Button(btn, "ASSIGNED\nHERE", new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 17), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter }))
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
                            GUI.color = new Color(0.7f, 0.7f,0.7f);
                            GUI.Label(btn, ModAPI.Input.GetKeyBindingAsString("spell" + (i + 1).ToString()), new GUIStyle(GUI.skin.label) { font = MainFont, fontSize = Mathf.RoundToInt(rr * 45), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
                            GUI.color = Color.white;
                            GUI.DrawTexture(btn, Res.ResourceLoader.instance.LoadedTextures[6]);
                        }
                        catch (Exception ex)
                        {
                            ModAPI.Log.Write(i + " spell failure   ........ " + ex.ToString());

                        }
                    }
                }
                else
                {
                    //buy button
                    Rect UnlockRect = new Rect(bg.x + 150 * rr, 800 * rr, bg.width - 300 * rr, 100 * rr);
                    if(displayedSpellInfo.Levelrequirement <= ModdedPlayer.instance.Level)
                    {
                        if (ModdedPlayer.instance.MutationPoints >= 2)
                        {
                            GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { font = MainFont, fontSize = (int)(41 * rr), fontStyle = FontStyle.Bold };
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
                            GUIStyle morePointsStyle = new GUIStyle(GUI.skin.label) { font = MainFont, alignment = TextAnchor.MiddleCenter, fontSize = (int)(41 * rr), fontStyle = FontStyle.Bold };
                            morePointsStyle.onNormal.textColor = Color.gray;
                            morePointsStyle.onActive.textColor = Color.white;
                            GUI.Label(UnlockRect, "YOU NEED 2 POINTS TO UNLOCK AN ABILITY", morePointsStyle);
                        }
                    }
                    else
                    {
                        GUIStyle moreLevelsStyle = new GUIStyle(GUI.skin.label) { font = MainFont, alignment = TextAnchor.MiddleCenter, fontSize = (int)(41 * rr), fontStyle = FontStyle.Bold };
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
            Rect nameRect = new Rect(30 * rr+bg.x, y, bg.width / 2, 100 * rr);

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

        //private void BuySpell(Spell s)
        //{
        //    ModdedPlayer.instance.MutationPoints-=2;
        //    s.Bought = true;
        //}
        #endregion

        #region StatsMenu
        private void DrawStats()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width / 3, Screen.height));
            GUIStyle header = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(30f * rr),
                font = MainFont,
                fontStyle = FontStyle.Bold,
                margin = new RectOffset(Mathf.RoundToInt(50 * rr), 0, 0, 0)
            };
            GUIStyle label = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(24f * rr),
                font = MainFont,
            };
            GUILayout.Label("Stats", header);
            GUILayout.Space(75 * rr);

            GUILayout.Label("HEALTH: " + ModdedPlayer.instance.MaxHealth, header);
            GUILayout.Label("Base health: 10", label);
            GUILayout.Label("Bonus from vitality: " + ModdedPlayer.instance.vitality * ModdedPlayer.instance.HealthPerVitality, label);
            GUILayout.Label("Bonus health: " + ModdedPlayer.instance.HealthBonus, label);
            GUILayout.Label("Bonus health percent: " + ModdedPlayer.instance.MaxHealthPercent * 100 + "%", label);
            GUILayout.Space(50 * rr);

            GUILayout.Label("HEALTH REGEN", header);
            GUILayout.Label("Health per second: " + ModdedPlayer.instance.LifeRegen, label);
            GUILayout.Label("Health per second multipier: " + ModdedPlayer.instance.HealthRegenPercent * 100 + "%", label);
            GUILayout.Label("Health per hit: " + ModdedPlayer.instance.LifeOnHit, label);
            GUILayout.Label("Healing multipier: " + ModdedPlayer.instance.HealingMultipier * 100 + "%", label);
            GUILayout.Space(50 * rr);




            GUILayout.Label("ENERGY: " + ModdedPlayer.instance.MaxEnergy, header);
            GUILayout.Label("Base energy: 10", label);
            GUILayout.Label("Bonus from agility: " + ModdedPlayer.instance.agility * ModdedPlayer.instance.EnergyPerAgility, label);
            GUILayout.Label("Bonus energy: " + ModdedPlayer.instance.EnergyBonus, label);
            GUILayout.Label("Bonus energy percent: " + ModdedPlayer.instance.MaxEnergyPercent * 100 + "%", label);
            GUILayout.Space(50 * rr);

            GUILayout.Label("STAMINA REGEN", header);
            GUILayout.Label("Stamina per second: " + ModdedPlayer.instance.StaminaRecover, label);
            GUILayout.Label("Stamina per second bonus: " + ModdedPlayer.instance.StaminaRegen, label);
            GUILayout.Label("Stamina regen multipier: " + ModdedPlayer.instance.StaminaRegenPercent * 100 + "%", label);
            GUILayout.Space(50 * rr);

            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(Screen.width / 3, 0, Screen.width / 3, Screen.height));

            GUILayout.Label("ATTRIBUTES", header);
            GUILayout.Label("Strenght: " + ModdedPlayer.instance.strenght, label);
            GUILayout.Label("Intelligence: " + ModdedPlayer.instance.intelligence, label);
            GUILayout.Label("Agility: " + ModdedPlayer.instance.agility, label);
            GUILayout.Label("Vitality: " + ModdedPlayer.instance.vitality, label);
            GUILayout.Space(50 * rr);

            GUILayout.Label("OFFENSIVE", header);
            GUILayout.Label("Spell damage amplification: " + ModdedPlayer.instance.SpellAMP * 100 + "%", label);

            GUILayout.Space(10 * rr);

            GUILayout.Label("Critical hit chance: " + ModdedPlayer.instance.CritChance, label);
            GUILayout.Label("Critical hit damage: " + ModdedPlayer.instance.CritDamage, label);
            GUILayout.Space(10 * rr);

            GUILayout.Label("Outgoing damage increase: " + ModdedPlayer.instance.DamageOutputMult * 100 + "%", label);
            GUILayout.Label("Strenght melee damage increase: " + ModdedPlayer.instance.strenght * ModdedPlayer.instance.DamagePerStrenght * 100 + "%", label);
            GUILayout.Space(10 * rr);

            GUILayout.Label("Melee damage: " + ModdedPlayer.instance.MeleeDamageBonus, label);
            GUILayout.Label("Melee damage multipier: " + ModdedPlayer.instance.MeleeDamageAmplifier * 100 + "%", label);
            GUILayout.Space(10 * rr);

            GUILayout.Label("Ranged damage: " + ModdedPlayer.instance.RangedDamageBonus, label);
            GUILayout.Label("Ranged damage multipier: " + ModdedPlayer.instance.RangedDamageAmplifier * 100 + "%", label);
            GUILayout.Space(10 * rr);

            GUILayout.Label("Spell damage: " + ModdedPlayer.instance.SpellDamageBonus, label);
            GUILayout.Label("Spell damage multipier: " + ModdedPlayer.instance.SpellDamageAmplifier * 100 + "%", label);
            GUILayout.Space(50 * rr);

            GUILayout.Label("DEFENSIVE", header);
            GUILayout.Label("Damage reduction: " + ModdedPlayer.instance.DamageReduction * 100 + "%", label);
            GUILayout.Label("Dodge chance: " + ModdedPlayer.instance.DodgeChance * 100 + "%", label);
            GUILayout.Label("Armor: " + ModdedPlayer.instance.Armor, label);
            GUILayout.Label("Damage reduction from armor: " + ModdedPlayer.instance.ArmorDmgRed, label);
            GUILayout.Label("Magic resistance: " + ModdedPlayer.instance.MagicResistance, label);
            GUILayout.Space(50 * rr);

            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect((Screen.width / 3) * 2, 0, Screen.width / 3, Screen.height));

            GUILayout.Label("ADVENTURE", header);
            GUILayout.Label("Movement speed: " + ModdedPlayer.instance.MoveSpeed * 100 + "%", label);
            GUILayout.Label("Swing speed: " + ModdedPlayer.instance.AttackSpeed * 100 + "%", label);
            GUILayout.Label("Experience gain: " + ModdedPlayer.instance.ExpFactor * 100 + "%", label);
            GUILayout.Label("Massacre duration: " + ModdedPlayer.instance.MaxMassacreTime + " seconds", label);
            GUILayout.Space(50 * rr);
            GUILayout.EndArea();

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



        bool Hovered;
        bool Buying;
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
                targetPerkOffset += Vector2.down * Time.deltaTime * 300;
            }
            else if (mousepos.y < 30 * rr)
            {
                targetPerkOffset += Vector2.down * Time.deltaTime * -300;

            }
            if (mousepos.x > Screen.width - 30 * rr)
            {
                targetPerkOffset += Vector2.right * Time.deltaTime * -300;
            }
            else if (mousepos.x < 30 * rr)
            {
                targetPerkOffset += Vector2.right * Time.deltaTime * 300;
            }
            currentPerkOffset = Vector3.Slerp(currentPerkOffset, targetPerkOffset, Time.deltaTime * 15f);

            //filling DisplayedPerkIDs with perk ids where category is the same as the selected one
            DisplayedPerkIDs = Perk.AllPerks.Where(p => p.Category == _perkpage).Select(p => p.ID).ToArray();

            //Drawing Perks
            Rect rect = new Rect(currentPerkOffset, new Vector2(PerkWidth, PerkHeight) * 2);
            rect.center = currentPerkOffset;
            GUI.DrawTexture(rect, ResourceLoader.GetTexture(84));
            GUI.Label(rect, ModdedPlayer.instance.MutationPoints.ToString(), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = (int)(80 * rr),fontStyle = FontStyle.Bold,font = MainFont });



            Hovered = false;
            Buying = false;
            for (int i = 0; i < DisplayedPerkIDs.Length; i++)
            {
                DrawPerk(DisplayedPerkIDs[i]);
            }
           
            if (!Buying)
            {
                _timeToBuyPerk = 0;
            }
            if (!Hovered)
            {
                _perkDetailAlpha = 0;
            }






            //Drawing buttons for selecting category
            Array menus = Enum.GetValues(typeof(Perk.PerkCategory));
            float btnSize = 250 * rr;
            float bigBtnSize = 40 * rr;
            float offset = Screen.width / 2 - (menus.Length * btnSize + bigBtnSize) / 2;

            for (int i = 0; i < menus.Length; i++)
            {
                Rect topButton = new Rect(offset, 35 * rr, btnSize, 60 * rr);
                if ((Perk.PerkCategory)menus.GetValue(i) == _perkpage)
                {
                    _perkCategorySizes[i] = Mathf.Clamp(_perkCategorySizes[i] + Time.deltaTime * 40, 0, bigBtnSize);
                    topButton.width += bigBtnSize;
                    topButton.height += bigBtnSize / 2;
                }
                else
                {
                    _perkCategorySizes[i] = Mathf.Clamp(_perkCategorySizes[i] - Time.deltaTime * 30, 0, bigBtnSize);
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
                        content = "Utility";
                        break;
                    default:
                        content = ((Perk.PerkCategory)menus.GetValue(i)).ToString();
                        break;
                }
                if (GUI.Button(topButton, content, new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = (int)(40 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow }))
                {
                    _perkpage = (Perk.PerkCategory)menus.GetValue(i);
                    targetPerkOffset = wholeScreen.center;
                    currentPerkOffset = targetPerkOffset;
                }


            }
        }

        void DrawPerk(int a)
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
            if (!show) return;




            Vector2 center = new Vector2(PerkWidth * p.PosOffsetX, PerkHeight * p.PosOffsetY);
            center += currentPerkOffset;
            Vector2 size = new Vector2(PerkWidth, PerkHeight);
            size *= p.Size;
            Rect r = new Rect(Vector2.zero, size);
            r.center = center;

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
                GUI.color = Color.white;

            }
            else
            {
                GUI.color = Color.gray;
                GUI.DrawTexture(r, ResourceLoader.GetTexture(p.TextureVariation * 2 + 81));
                GUI.color = Color.white;

            }
            if (r.Contains(mousepos))
            {
                Hovered = true;
                r.center = center;
                _perkDetailAlpha += Time.deltaTime;
                if (_perkDetailAlpha >= 0.7f)
                {
                    GUI.color = new Color(_perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f, _perkDetailAlpha - 0.7f);

                    Rect Name = new Rect(r.x - 200 * rr, r.y - 130 * rr, 400 * rr + r.width, 100 * rr);
                    GUI.Label(Name, p.Name, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter, fontSize = (int)(40 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });

                    Rect Desc = new Rect(r.x - 200 * rr, r.yMax + 30 * rr, 400 * rr + r.width, 1000 * rr);

                    string desctext = p.Description;

                    if (!p.IsBought)
                    {
                        desctext = "HOLD LEFT MOUSE BUTTON TO BUY\n \n" + p.Description;
                        Rect LevelReq = new Rect(r.x - 440 * rr, r.y, 400 * rr, r.height);
                        Rect Cost = new Rect(r.xMax + 40 * rr, r.y, 400 * rr, r.height);
                        GUI.Label(LevelReq, "Level " + p.LevelRequirement + " required", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = (int)(33* rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });
                        GUI.Label(Cost, "Cost in mutation points: " + p.PointsToBuy, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = (int)(33 * rr), font = MainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });
                        if (UnityEngine.Input.GetMouseButton(0) && ModdedPlayer.instance.MutationPoints >= p.PointsToBuy && PerkEnabled(Perk.AllPerks[a]) && Perk.AllPerks[a].LevelRequirement <= ModdedPlayer.instance.Level)
                        {
                            _timeToBuyPerk += Time.deltaTime;
                            Buying = true;
                            Rect buyRect = new Rect(0, 1-_timeToBuyPerk / 2, 1,_timeToBuyPerk/2);
                            Rect buyRect2 = new Rect(r);
                            r.height *= _timeToBuyPerk / 2;

                            GUI.color = color;
                            GUI.DrawTextureWithTexCoords(r,ResourceLoader.GetTexture(p.TextureVariation *2+81 +1), buyRect);
                            GUI.color = Color.white;
                            if (_timeToBuyPerk >= 2)
                            {
                                ModdedPlayer.instance.MutationPoints -= p.PointsToBuy;
                                Perk.AllPerks[a].IsBought = true;
                                Perk.AllPerks[a].ApplyMethods();
                                Perk.AllPerks[a].Applied= true;
                                Buying = false;
                            }
                        }
                    }
                    GUI.Label(Desc, desctext, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, fontSize = (int)(28 * rr), font = MainFont, fontStyle = FontStyle.Normal, richText = true, clipping = TextClipping.Overflow });
                }


                GUI.color = Color.white;

            }


        }

    }
}
