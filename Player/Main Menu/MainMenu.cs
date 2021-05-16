using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ChampionsOfForest.Player;
using ChampionsOfForest.Player.Crafting;

using TheForest.Utils;

using UnityEngine;
using UnityEngine.SceneManagement;

using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
	public partial class MainMenu : MonoBehaviour
	{
		//Instance of this object
		public static MainMenu Instance
		{
			get; private set;
		}

		//Difficulty Settings
		public static readonly string[] DiffSel_Names = new string[] { "Easy", "Veteran", "Elite", "Master", "Challenge I", "Challenge II", "Challenge III", "Challenge IV", "Challenge V", "Challenge VI", "Hell", };

		public static readonly string[] DiffSel_Descriptions = new string[]
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

		//main menu variables
		public float screenScale;                                // current resolution to fullHD ratio

		private Vector2 mousePos;                       //mouse postion in GUI space
		private Rect wholeScreenRect;                       //rect that contains the entire screen
		private bool isCrouching;                         //used in hud display, when crouching scans enemies
		private bool isMenuInteractable;                  //determines if menu elements can be clicked or not
		private GUIStyle menuBtnStyle;                  //style of font for main menu button
		private GUIStyle chgDiffLabelStyle;
		private GUIStyle chgDiffBtnStyle;
		private const float screenTransisionSpeed = 2;      //speed of transion between menus
		public Font mainFont;                           //main font for the mod
		public Font secondaryFont;                      //secondary font

		//alpha values for button highlight image
		private readonly float[] buttonHighlightOpacity = { 0, 0, 0, 0 };

		private const float opacityGainSpeed = 3;    //speed at which the highlights gain alpha - curently gains full opacity after 0.33 seconds

		private float difficultyCooldown;

		private Transform MainCamera
		{
			get
			{
				if (_mainCamera == null)
				{
					_mainCamera = Camera.main.transform;
				}
				return _mainCamera;
			}
		}

		private Transform _mainCamera;

		//Textures
		private Texture2D blackSquareTex;

		private Texture2D screenTransTex;

		//a static variable for colors of items with different rarities
		//affects item border in inventory, text color in pickup, particle effect color
		public readonly static Color[] RarityColors = new Color[]
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
		private Matrix4x4 guiMatrixBackup;

		//setting the instance
		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			try
			{
				var sceneName = SceneManager.GetActiveScene().name;
				if (sceneName == "TitleScene" || sceneName == "TitleSceneLoader")
				{
					Destroy(gameObject);
					return;
				}

				screenScale = Screen.height / 1080f;
				wholeScreenRect = new Rect(0, 0, Screen.width, Screen.height);

				slotDim = new Vector2(50 * screenScale, 50 * screenScale);

				blackSquareTex = new Texture2D(1, 1);
				blackSquareTex.SetPixel(0, 0, Color.black);
				blackSquareTex.Apply();
				_openedMenu = OpenedMenuMode.Hud;
				screenTransTex = new Texture2D(1, 1);
				screenTransTex.SetPixel(0, 0, new Color(0, 0, 0, 0));
				screenTransTex.Apply();
				guiMatrixBackup = GUI.matrix;
				isMenuInteractable = true;

				//Perks
				PerkHeight = PerkHexagonSide * 2 * screenScale;
				PerkWidth = PerkHexagonSide * 1.732050f * screenScale; //times sqrt(3)

				//HUD
				HideHud = false;
				HUDenergyLabelRect = new Rect(Screen.width - 500 * screenScale, Screen.height - 100 * screenScale, 500 * screenScale, 100 * screenScale);
				HUDHealthLabelRect = new Rect(Screen.width - 500 * screenScale, Screen.height - 140 * screenScale, 500 * screenScale, 100 * screenScale);
				HUDShieldLabelRect = new Rect(Screen.width - 500 * screenScale, Screen.height - 180 * screenScale, 500 * screenScale, 100 * screenScale);

				//The main font as of now is Gabriola
				mainFont = Font.CreateDynamicFontFromOSFont("Bahnschrift", Mathf.RoundToInt(24 * screenScale));
				if (mainFont == null)
				{
					mainFont = Font.CreateDynamicFontFromOSFont("Arial", Mathf.RoundToInt(24 * screenScale));
				}
				secondaryFont = Font.CreateDynamicFontFromOSFont("Old English Text MT", 35);
				if (!secondaryFont)
					secondaryFont = mainFont;
				//Getting textures using ResourceLoader
				_combatDurationTex = ResourceLoader.instance.LoadedTextures[18];
				_expBarFillTex = ResourceLoader.instance.LoadedTextures[16];
				_expBarBackgroundTex = ResourceLoader.instance.LoadedTextures[15];
				_expBarFrameTex = ResourceLoader.instance.LoadedTextures[17];
				_SpellBG = ResourceLoader.instance.LoadedTextures[5];
				_SpellFrame = ResourceLoader.instance.LoadedTextures[6];
				_SpellCoolDownFill = ResourceLoader.instance.LoadedTextures[8];
				//Inventory

				DraggedItemIndex = -1;

				DraggedItem = null;
				semiBlack = new Texture2D(1, 1);

				//host difficulty raise/lower cooldown
				if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
					difficultyCooldown = 5 * 60; //once every 5 minutes

				if (GameSetup.IsMultiplayer)
					otherPlayerPings = new Dictionary<string, MarkObject>();

				StartCoroutine(ProgressionRefresh());
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write("ERROR: Failure in start of Main Menu: " + ex.ToString());
			}
		}
		float BookScrollAmountGoal = 0;
		private void Update()
		{
			if (difficultyCooldown > 0)
				difficultyCooldown -= Time.deltaTime;
			LevelUpDuration -= Time.deltaTime;

			if (_openedMenu == OpenedMenuMode.Stats)
			{
				BookScrollAmountGoal = Mathf.Clamp(BookScrollAmountGoal + 500 * screenScale * UnityEngine.Input.GetAxis("Mouse ScrollWheel"), -Screen.height * 10 * screenScale, 0);
				if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
				{
					ChangePage(guidePage + 1);
				}
				if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
				{
					ChangePage(guidePage - 1);

				}
				if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
				{
					BookScrollAmountGoal = Mathf.Clamp(BookScrollAmountGoal - 1000f * screenScale, -Screen.height * 10 * screenScale, 0);
				}
				if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
				{
					BookScrollAmountGoal = Mathf.Clamp(BookScrollAmountGoal + 1000f * screenScale, -Screen.height * 10 * screenScale, 0);
				}
				BookScrollAmount = Vector3.Slerp(new Vector3(BookScrollAmount, 0, 0), new Vector3(BookScrollAmountGoal, 0, 0), Time.deltaTime * 30f * screenScale).x;
			}
			else if (_openedMenu == OpenedMenuMode.Inventory)
			{
				if (UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt))
					drawTotal =! drawTotal;
			}

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
						if (consumedsomething)
							consumedsomething = false;
					}

					if (SelectedItem != -1 && Inventory.Instance.ItemSlots[SelectedItem] != null)
					{
						var item = Inventory.Instance.ItemSlots[SelectedItem];
						if (UnityEngine.Input.GetMouseButtonDown(0))
						{
							if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
							{
								//unequip if equipped
								if (SelectedItem < -1)
								{
									for (int i = 0; i < Inventory.SlotCount; i++)
									{
										if (Inventory.Instance.ItemSlots[i] == null)
										{
											DraggedItem = null;
											isDragging = false;
											CustomCrafting.ClearIndex(SelectedItem);
											CustomCrafting.ClearIndex(i);
											DraggedItemIndex = -1;
											Inventory.Instance.ItemSlots[i] = item;
											Inventory.Instance.ItemSlots[SelectedItem] = null;
											SelectedItem = -1;
											break;
										}
									}
								}
								else//move to its correct slot or swap if slot is not empty
								{
									int targetSlot = -1;

									switch (item.type)
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
											if (Inventory.Instance.ItemSlots[-10] == null)
												targetSlot = -10;
											else if (Inventory.Instance.ItemSlots[-11] == null)
												targetSlot = -11;
											else
												targetSlot = -10;
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
										if (Inventory.Instance.ItemSlots[targetSlot] == null)
										{
											DraggedItem = null;
											isDragging = false;
											DraggedItemIndex = -1;
											CustomCrafting.ClearIndex(SelectedItem);
											Inventory.Instance.ItemSlots[targetSlot] = item;
											Inventory.Instance.ItemSlots[SelectedItem] = null;
											SelectedItem = -1;
										}
										else
										{
											DraggedItem = null;
											isDragging = false;
											DraggedItemIndex = -1;
											CustomCrafting.ClearIndex(SelectedItem);
											Inventory.Instance.ItemSlots[SelectedItem] = Inventory.Instance.ItemSlots[targetSlot];
											Inventory.Instance.ItemSlots[targetSlot] = item;
											SelectedItem = -1;
										}
									}
								}
							}
							else if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
							{
								if (SelectedItem > -1 && CustomCrafting.instance.craftMode != CustomCrafting.CraftMode.None)
								{
									if (CustomCrafting.instance.changedItem.i == null)
									{
										CustomCrafting.instance.changedItem.Assign(SelectedItem, Inventory.Instance.ItemSlots[SelectedItem]);
									}
									else
									{
										int max = CustomCrafting.instance.CurrentCraftingMode.IngredientCount;
										for (int j = 0; j < max; j++)
										{
											if (CustomCrafting.Ingredients[j].i == null)
											{
												if (!(CustomCrafting.Ingredients.Any(x => x.i == item) || CustomCrafting.instance.changedItem.i == item))
												{
													CustomCrafting.Ingredients[j].Assign(SelectedItem, item);
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
			isCrouching = LocalPlayer.FpCharacter.crouching;
			if (ModAPI.Input.GetButtonDown("MenuToggle"))
			{
				MenuKeyPressAction();
			}
		}

		//its a bad idea to put this i main menu
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
					if (item.Value.extraHealth + item.Value.HealthScript.Health < 1)
					{
						EnemyManager.hostDictionary.Remove(item.Key);
						break;
					}
				}
				yield return null;
			}
		}

		private bool stylesInitialized = false;

		private void InitializeStyles()
		{
			stylesInitialized = true;
			statNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(16 * screenScale), font = mainFont };
			statValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = Mathf.RoundToInt(16 * screenScale), fontStyle = FontStyle.Bold, font = mainFont };
			statMinMaxValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(14 * screenScale), fontStyle = FontStyle.Normal, font = mainFont };
			HUDStatStyle = new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(33 * screenScale), alignment = TextAnchor.LowerRight, wordWrap = false, clipping = TextClipping.Overflow, };
			menuBtnStyle = new GUIStyle(GUI.skin.label) { font = mainFont, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(40 * screenScale), wordWrap = false, fontStyle = FontStyle.BoldAndItalic, onHover = new GUIStyleState() { textColor = new Color(1, 0.5f, 0.35f) } };
			chgDiffLabelStyle = new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(20f * screenScale), alignment = TextAnchor.MiddleLeft };
			chgDiffBtnStyle = new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(18f * screenScale), alignment = TextAnchor.MiddleCenter, font = mainFont, hover = new GUIStyleState() { textColor = new Color(0.6f, 0, 0) } };
			craftBtnStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(30 * screenScale), fontStyle = FontStyle.Normal, font = mainFont };
			craftHeaderStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter, fontSize = Mathf.RoundToInt(24 * screenScale), font = mainFont };
		}

		partial void DrawDebug();

		//Draws everything
		private void OnGUI()
		{
			try
			{
				DrawDebug();

				GUI.skin.label.normal.textColor = Color.white;
				GUI.skin.label.onNormal.textColor = Color.white;
				GUI.contentColor = Color.white;
				GUI.backgroundColor = Color.white;
				GUI.color = Color.white;
				GUI.skin.label.clipping = TextClipping.Overflow;
				GUI.skin.label.richText = true;

				mousePos = new Vector2(UnityEngine.Input.mousePosition.x, Screen.height - UnityEngine.Input.mousePosition.y);

				if (!ModSettings.DifficultyChoosen)
				{
					DifficultySelectionScreen();
				}
				else
				{
					if (!stylesInitialized)
						InitializeStyles();
					try
					{
						if (_openedMenu == OpenedMenuMode.Main)
						{
							GUI.DrawTexture(wholeScreenRect, Res.ResourceLoader.GetTexture(78));

							//GUI.Label(new Rect(wholeScreen.center, Vector2.one * 500), "Main");

							DrawMain();
						}
						else if (_openedMenu == OpenedMenuMode.Inventory)
						{
							GUI.DrawTexture(wholeScreenRect, Res.ResourceLoader.GetTexture(78));
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
							GUI.DrawTexture(wholeScreenRect, Res.ResourceLoader.GetTexture(78));
							DrawSpellMenu();
						}
						else if (_openedMenu == OpenedMenuMode.Stats)
						{
							GUI.DrawTexture(wholeScreenRect, Res.ResourceLoader.GetTexture(78));
							DrawGuide();
						}
						else if (_openedMenu == OpenedMenuMode.Perks)
						{
							DrawPerks();
						}
						GUI.DrawTexture(wholeScreenRect, screenTransTex);
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

		private Rect[] mainmenubtns;

		private void DrawMain()
		{
			try
			{
				if (isDragging)
				{
					if (DraggedItem.Equipped)
					{
						DraggedItem.OnUnequip();
						Inventory.Instance.ItemSlots[DraggedItemIndex].Equipped = false;
					}
					Inventory.Instance.DropItem(DraggedItemIndex);
					DraggedItem = null;
					DraggedItemIndex = -1;
					isDragging = false;
				}
				targetPerkOffset = wholeScreenRect.center;
				currentPerkOffset = targetPerkOffset;
				Vector2 center = wholeScreenRect.center;
				Rect MiddleR = new Rect(Vector2.zero, new Vector2(300 * screenScale, 300 * screenScale))
				{
					center = center
				};

				GUI.Label(new Rect(10 * screenScale, 10 * screenScale, 300, 100), "Difficulty: " + DiffSel_Names[(int)ModSettings.difficulty], chgDiffLabelStyle);
				//drawing difficulty raise lower buttons
				if (difficultyCooldown <= 0 && !GameSetup.IsMpClient)
				{
					if ((int)ModSettings.difficulty < (int)ModSettings.Difficulty.Hell && GUI.Button(new Rect(10 * screenScale, 90 * screenScale, 200 * screenScale, 40 * screenScale), "Raise Difficulty", chgDiffBtnStyle))
					{
						//raise difficulty
						difficultyCooldown = 10 * 60;
						ModSettings.difficulty++;
						ModSettings.BroadCastSettingsToClients();
					}
					if (ModSettings.difficulty > (int)ModSettings.Difficulty.Easy && GUI.Button(new Rect(10 * screenScale, 130 * screenScale, 200 * screenScale, 40 * screenScale), "Lower Difficulty", chgDiffBtnStyle))
					{
						//lower difficulty
						difficultyCooldown = 10 * 60;
						ModSettings.difficulty--;
						ModSettings.BroadCastSettingsToClients();
					}
					if (GUI.Button(new Rect(10 * screenScale, 170 * screenScale, 200 * screenScale, 40 * screenScale), "Change Options", chgDiffBtnStyle))
						{
						ModSettings.DifficultyChoosen = false;
						difficultyCooldown = 10 * 60;
					}
				}

				Rect r1 = new Rect(wholeScreenRect.center, new Vector2(Screen.height / 2, Screen.height / 2));
				Rect r2 = new Rect(r1);
				Rect r3 = new Rect(r1);
				Rect r4 = new Rect(r1);
				float minDist = 500f / 1500f;
				minDist *= r1.width;
				MenuButton(minDist, ref r1, OpenedMenuMode.Inventory, "Inventory", new Vector2(1, -1), ref buttonHighlightOpacity[0], -50 * screenScale, -50 * screenScale);
				r2.position = center - r1.size;
				MenuButton(minDist, ref r2, OpenedMenuMode.Spells, "Abilities", new Vector2(-1, 1), ref buttonHighlightOpacity[1], 50 * screenScale, 50 * screenScale);
				r3.position = center - new Vector2(0, r1.width);
				MenuButton(minDist, ref r3, OpenedMenuMode.Stats, "Guide & Stats", Vector2.one, ref buttonHighlightOpacity[2], -50 * screenScale, 50 * screenScale);
				r4.position = center - new Vector2(r1.width, 0);
				MenuButton(minDist, ref r4, OpenedMenuMode.Perks, "Mutations", -Vector2.one, ref buttonHighlightOpacity[3], 50 * screenScale, -50 * screenScale);
				GUI.Label(MiddleR, "Level \n" + ModdedPlayer.instance.level.ToString(), menuBtnStyle);

				if (GUI.Button(new Rect(Screen.width - 120 * screenScale, 40 * screenScale, 120 * screenScale, 40 * screenScale), HideHud ? "[ NO HUD ]" : "[ HUD ]", chgDiffLabelStyle))
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
		private void MenuButton(in float mindist, ref Rect rect, OpenedMenuMode mode, in string text, in Vector2 Scale, ref float Opacity, in float offset_X, in float offset_Y)
		{
			Matrix4x4 backupMatrix = GUI.matrix;
			GUIUtility.ScaleAroundPivot(Scale, rect.center);
			float dist = Vector2.Distance(mousePos, wholeScreenRect.center);
			GUI.DrawTexture(rect, Res.ResourceLoader.instance.LoadedTextures[1]);

			if (dist > mindist && dist < rect.height && rect.Contains(mousePos) && isMenuInteractable)
			{
				Opacity = Mathf.Clamp01(Opacity + Time.unscaledDeltaTime * opacityGainSpeed);

				if (UnityEngine.Input.GetMouseButtonDown(0))
				{
					StartCoroutine(FadeMenuSwitch(mode));
				}
			}
			else
			{
				Opacity = Mathf.Clamp01(Opacity - Time.unscaledDeltaTime * opacityGainSpeed);
			}
			if (Opacity > 0)
			{
				GUI.color = new Color(1, 1, 1, Opacity);
				GUI.DrawTexture(rect, Res.ResourceLoader.instance.LoadedTextures[2]);
				GUI.color = Color.white;
			}
			GUI.matrix = backupMatrix;
			rect.x += offset_X;
			rect.y += offset_Y;
			GUI.Label(rect, text, menuBtnStyle);
		}

		private void MenuKeyPressAction()
		{
			if (!isMenuInteractable)
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

		#endregion MainMenuMethods

		//Does fade out fade in transision between menus
		public IEnumerator FadeMenuSwitch(OpenedMenuMode mode)
		{
			isMenuInteractable = false;
			float alpha = 0;
			while (alpha < 1)
			{
				alpha = Mathf.Clamp(alpha + Time.unscaledDeltaTime * screenTransisionSpeed, 0, 1);
				screenTransTex.SetPixel(0, 0, new Color(0, 0, 0, alpha));
				screenTransTex.Apply();
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
				alpha = Mathf.Clamp(alpha - Time.unscaledDeltaTime * screenTransisionSpeed, 0, 1);
				screenTransTex.SetPixel(0, 0, new Color(0, 0, 0, alpha));
				screenTransTex.Apply();
				yield return null;
			}
			isMenuInteractable = true;
		}
	}
}