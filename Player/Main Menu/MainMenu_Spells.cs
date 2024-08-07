﻿using System;

using ChampionsOfForest.Localization;
using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest
{
	public partial class MainMenu : MonoBehaviour
	{
		#region SpellsMethods;

		private Spell displayedSpellInfo;
		private Texture2D semiBlack;
		private float semiblackValue;
		private float spellOffset;

		private void DrawSpellMenu()
		{
			GUIStyle style = new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 41), alignment = TextAnchor.MiddleLeft };
			float y = spellOffset;
			for (int i = 0; i < SpellDataBase.SortedSpellIDs.Length; i++)
			{
				Spell spell = SpellDataBase.spellDictionary[SpellDataBase.SortedSpellIDs[i]];
				DrawSpell(ref y, spell, new GUIStyle(style));
			}
			if (displayedSpellInfo == null)
			{
				//scrolling the list
				spellOffset += Input.GetAxis("Mouse ScrollWheel") * 160;
				spellOffset = Mathf.Clamp(spellOffset, -(100 * screenScale * (SpellDataBase.spellDictionary.Count + 4)), 0);
			}
			else
			{
				//background effect

				semiblackValue += Time.unscaledDeltaTime / 5;
				semiBlack.SetPixel(0, 0, new Color(0.6f, 0.16f, 0, 0.6f + Mathf.Sin(semiblackValue * Mathf.PI) * 0.2f));
				semiBlack.Apply();
				GUI.DrawTexture(wholeScreenRect, semiBlack);
				Rect bg = new Rect(Screen.width / 2 - 325 * screenScale, 0, 650 * screenScale, Screen.height);
				GUI.DrawTexture(bg, Res.ResourceLoader.instance.LoadedTextures[27]);

				//go back btn
				if (Input.GetMouseButtonDown(1))
				{
					displayedSpellInfo = null;
					Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickUp);

					return;
				}

				//drawing pretty info
				Rect SpellIcon = new Rect(Screen.width / 2 - 100 * screenScale, 25 * screenScale, 200 * screenScale, 200 * screenScale);
				GUI.DrawTexture(SpellIcon, displayedSpellInfo.icon);
				if (GUI.Button(new Rect(Screen.width - 170 * screenScale, 25 * screenScale, 150 * screenScale, 50 * screenScale), Translations.MainMenu_Spells_1/*Close\n(Right Mouse Button)*/, new GUIStyle(GUI.skin.button) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 15) })) //tr
				{
					displayedSpellInfo = null;
					Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickUp);
					return;
				}

				GUI.Label(new Rect(Screen.width / 2 - 300 * screenScale, 225 * screenScale, 600 * screenScale, 70 * screenScale),
					displayedSpellInfo.Name,
					new GUIStyle(GUI.skin.label)
					{
						font = mainFont,
						fontSize = Mathf.RoundToInt(screenScale * 50),
						fontStyle = FontStyle.Normal,
						alignment = TextAnchor.MiddleCenter,

					});
				GUI.DrawTexture(new Rect(Screen.width / 2 - 150 * screenScale, 325 * screenScale, 300 * screenScale, 35 * screenScale), Res.ResourceLoader.instance.LoadedTextures[30]);

				GUI.Label(new Rect(bg.x + 25 * screenScale, 370 * screenScale, bg.width - 50 * screenScale, 500 * screenScale),
					displayedSpellInfo.GetDescription() + "\n<size=28><color=lightblue>" + (displayedSpellInfo.EnergyCost > 0 ? "\n" +
					Translations.MainMenu_Spells_2/*Energy:*/ //tr
					+ "  <b>" + displayedSpellInfo.EnergyCost + "</b>" : "") + (displayedSpellInfo.BaseCooldown > 0 ?
					(displayedSpellInfo.Cooldown == displayedSpellInfo.BaseCooldown ? "\n" +
					Translations.MainMenu_Spells_3/*Cooldown*/ +  //tr
					":  <b>" + displayedSpellInfo.Cooldown + " s</b>" : "\n" +
					Translations.MainMenu_Spells_4/*Base Cooldown*/ + //tr
					":  <b>" + displayedSpellInfo.BaseCooldown + " s</b>\n" +
					Translations.MainMenu_Spells_5/*Reduced Cooldown*/ //tr
					+ ":  <b>" + displayedSpellInfo.Cooldown + " s</b>")
					: "") +
					"\nRequired level:  <b>" + displayedSpellInfo.LevelRequirement + "</b></color></size>",
					new GUIStyle(GUI.skin.label)
					{
						font = mainFont,
						fontSize = Mathf.RoundToInt(screenScale * 23),
						fontStyle = FontStyle.Normal,
						alignment = TextAnchor.MiddleCenter,
						richText = true
					});

				if (displayedSpellInfo.Bought)
				{
					//select equip slot
					for (int i = 0; i < SpellCaster.instance.infos.Length; i++)
					{
						try
						{
							Rect btn = new Rect(bg.x + 25f * screenScale + i * 100 * screenScale, 900 * screenScale, 100 * screenScale, 100 * screenScale);

							GUI.DrawTexture(btn, Res.ResourceLoader.instance.LoadedTextures[5]);
							if (Input.GetKey(KeyCode.LeftShift) || changingSpellKey)
							{
								//enters key change instead of assigning spell to a slot
								if (GUI.Button(btn, changingSpellKey ? "Press\nany key" : "Change\nkey", new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 29), alignment = TextAnchor.MiddleCenter }))
								{
									ToggleChangeKey(i);
									Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickDown);
								}
							}
							else
							{
								if (displayedSpellInfo.EquippedSlot == i)
								{
									GUI.DrawTexture(btn, displayedSpellInfo.icon);
									if (GUI.Button(btn, "•", new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 17), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter }))
									{

										//Clears the spot
										Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickUp);

										SpellCaster.instance.SetSpell(i);
									}
								}
								else
								{
									if (SpellCaster.instance.infos[i].spell != null)
									{
										GUI.DrawTexture(btn, SpellCaster.instance.infos[i].spell.icon);
										if (GUI.Button(btn, SpellCaster.instance.infos[i].spell.Name, new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 17), alignment = TextAnchor.MiddleCenter }))
										{
											//Replaces spell
											if (displayedSpellInfo.IsEquipped)
												SpellCaster.instance.SetSpell(displayedSpellInfo.EquippedSlot);

											SpellCaster.instance.SetSpell(i, displayedSpellInfo);
											Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickDown);
										}
									}
									else if (GUI.Button(btn, "", new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 34), alignment = TextAnchor.MiddleCenter }))
									{
										//Assigns spell onto empty slot
										if (displayedSpellInfo.IsEquipped)
											SpellCaster.instance.SetSpell(displayedSpellInfo.EquippedSlot);
										Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickDown);
										SpellCaster.instance.SetSpell(i, displayedSpellInfo);

									}
								}
								GUI.color = new Color(0.7f, 0.7f, 0.7f);
								GUI.Label(btn, SpellCaster.instance.infos[i].key.ToString(), new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 45), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
								GUI.color = Color.white;
							}
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
					Rect UnlockRect = new Rect(bg.x + 150 * screenScale, 870 * screenScale, bg.width - 300 * screenScale, 100 * screenScale);
					if (displayedSpellInfo.LevelRequirement <= ModdedPlayer.instance.level)
					{
						if (ModdedPlayer.instance.MutationPoints >= 2)
						{
							GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { font = mainFont, fontSize = Mathf.RoundToInt(41 * screenScale), fontStyle = FontStyle.Bold };
							btnStyle.onActive.textColor = Color.blue;
							btnStyle.onNormal.textColor = Color.gray;
							if (GUI.Button(UnlockRect, Translations.MainMenu_Spells_6/*Unlock*/, btnStyle))  //tr
							{
								displayedSpellInfo.Bought = true;
								ModdedPlayer.instance.MutationPoints -= 2;
								Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.SpellUnlock);
							}
						}
						else
						{
							GUIStyle morePointsStyle = new GUIStyle(GUI.skin.label) { font = mainFont, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(31 * screenScale), fontStyle = FontStyle.Bold };
							GUI.color = Color.gray;
							GUI.Label(UnlockRect, Translations.MainMenu_Spells_7(2)/*Costs 2 mutation points*/, morePointsStyle);  //tr
							GUI.color = Color.white;
						}
					}
					else
					{
						GUIStyle moreLevelsStyle = new GUIStyle(GUI.skin.label) { font = mainFont, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(41 * screenScale), fontStyle = FontStyle.Bold };
						moreLevelsStyle.onNormal.textColor = Color.gray;
						moreLevelsStyle.onActive.textColor = Color.white;
						GUI.Label(UnlockRect, Translations.MainMenu_Spells_8/*Level too low*/, moreLevelsStyle);    //tr
					}
				}
				GUI.color = Color.white;
			}
		}

		private bool changingSpellKey;
		private int changingSpellIdx;
		private void ToggleChangeKey(int i)
		{
			changingSpellKey = !changingSpellKey;
			changingSpellIdx = i;
		}
		private void ChangeKeyUpdate()
		{
			if (!changingSpellKey)
				return;
			var e = Event.current;
			if (e.isKey && e.keyCode != KeyCode.None )
			{
				SpellCaster.instance.infos[changingSpellIdx].key= e.keyCode;
				changingSpellKey = false;
				e.Use();
				Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickDown);

			}
		}


		private void DrawSpell(ref float y, Spell s, GUIStyle style)
		{
			Rect bg = new Rect(0, y, Screen.width * 3.3f / 5, 100 * screenScale);
			bg.x = (Screen.width - bg.width) / 2;
			GUI.DrawTexture(bg, Res.ResourceLoader.instance.LoadedTextures[28]);
			Rect nameRect = new Rect(30 * screenScale + bg.x, y, bg.width / 2, 100 * screenScale);
			bool locked = false;
			if (s.LevelRequirement > ModdedPlayer.instance.level)
			{
				locked = true;
				GUI.color = Color.black;
			}
			else
			{
				if (!s.Bought)
					GUI.color = Color.gray;
				if (bg.Contains(mousePos))
				{
					if (displayedSpellInfo == null)
					{
						style.fontStyle = FontStyle.Bold;
						if (Input.GetMouseButtonDown(0))
						{
							Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.ClickUp);

							displayedSpellInfo = SpellDataBase.spellDictionary[s.ID];
						}
					}
				}
			}
			if (locked)
				GUI.Label(nameRect, Translations.MainMenu_Spells_9/*Unlocked at level */ + s.LevelRequirement, style);  //tr
			else
				GUI.Label(nameRect, s.Name, style);
			Rect iconRect = new Rect(bg.xMax - 140 * screenScale, y + 15 * screenScale, 70 * screenScale, 70 * screenScale);
			GUI.DrawTexture(iconRect, s.icon);

			GUI.color = Color.white;
			y += 100 * screenScale;
		}

		#endregion SpellsMethods;
	}
}