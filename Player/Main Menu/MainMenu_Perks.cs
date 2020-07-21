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
	public partial class MainMenu : MonoBehaviour
	{

		private Perk.PerkCategory _perkpage = Perk.PerkCategory.MeleeOffense;
		private readonly float[] _perkCategorySizes = new float[6];
		private int[] DisplayedPerkIDs = new int[0];
		private Vector2 currentPerkOffset;
		private Vector2 targetPerkOffset;
		private float _perkDetailAlpha;
		private float _timeToBuyPerk;

		const float PerkHexagonSide = 60;
		private float PerkHeight;
		private float PerkWidth;

		private bool PerkRequirementsMet(Perk perk)
		{
			if (perk.RequiredIds == null)
			{
				return true;
			}

			for (int i = 0; i < perk.RequiredIds.Length; i++)
			{
				if (!Perk.AllPerks[perk.RequiredIds[i]].IsBought)
				{
					return false;
				}
			}
			return true;
		}
		private bool PerkEnabled(Perk perk)
		{
			if (perk.InheritIDs.Length == 0)
			{
				return true;
			}

			for (int i = 0; i < perk.InheritIDs.Length; i++)
			{
				if (perk.InheritIDs[i] == -1)
				{
					return true;
				}
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
			float x = Mathf.Clamp(-(currentPerkOffset.x - wholeScreenRect.center.x) / (Screen.width * 5) + 0.25f, 0, 0.5f);
			float y = Mathf.Clamp((currentPerkOffset.y - wholeScreenRect.center.y) / (Screen.height * 5) + 0.25f, 0, 0.5f);
			Rect bgRectCords = new Rect(x, y, 0.5f, 0.5f);


			//Drawing background images
			switch (_perkpage)
			{
				case Perk.PerkCategory.MeleeOffense:
					GUI.DrawTextureWithTexCoords(wholeScreenRect, ResourceLoader.GetTexture(72), bgRectCords);
					break;
				case Perk.PerkCategory.RangedOffense:
					GUI.DrawTextureWithTexCoords(wholeScreenRect, ResourceLoader.GetTexture(74), bgRectCords);
					break;
				case Perk.PerkCategory.MagicOffense:
					GUI.DrawTextureWithTexCoords(wholeScreenRect, ResourceLoader.GetTexture(73), bgRectCords);
					break;
				case Perk.PerkCategory.Defense:
					GUI.DrawTextureWithTexCoords(wholeScreenRect, ResourceLoader.GetTexture(75), bgRectCords);
					break;
				case Perk.PerkCategory.Support:
					GUI.DrawTextureWithTexCoords(wholeScreenRect, ResourceLoader.GetTexture(77), bgRectCords);
					break;
				case Perk.PerkCategory.Utility:
					GUI.DrawTextureWithTexCoords(wholeScreenRect, ResourceLoader.GetTexture(76), bgRectCords);
					break;
				default:
					break;
			}







			//move left right
			if (mousePos.y > Screen.height - 30 * screenScale)
			{
				targetPerkOffset += Vector2.down * Time.unscaledDeltaTime * 300;
			}
			else if (mousePos.y < 30 * screenScale)
			{
				targetPerkOffset += Vector2.down * Time.unscaledDeltaTime * -300;

			}
			if (mousePos.x > Screen.width - 30 * screenScale)
			{
				targetPerkOffset += Vector2.right * Time.unscaledDeltaTime * -300;
			}
			else if (mousePos.x < 30 * screenScale)
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
			GUI.Label(rect, ModdedPlayer.instance.MutationPoints.ToString(), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(80 * screenScale), fontStyle = FontStyle.Bold, font = mainFont });
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

					Rect Name = new Rect(r.x - 200 * screenScale, r.y - 130 * screenScale, 400 * screenScale + r.width, 90 * screenScale);

					GUI.Label(Name, p.Name, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(40 * screenScale), font = mainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });

					Rect Desc = new Rect(r.x - 200 * screenScale, r.yMax + 30 * screenScale, 400 * screenScale + r.width, 1000 * screenScale);

					string desctext = p.Description;

					if (!p.IsBought || p.Endless)
					{
						desctext = "Press to buy\n" + p.Description;
						Rect LevelReq = new Rect(r.x - 440 * screenScale, r.y, 400 * screenScale, r.height);
						Rect Cost = new Rect(r.xMax + 40 * screenScale, r.y, 400 * screenScale, r.height);
						if (p.LevelRequirement > ModdedPlayer.instance.Level)
						{
							GUI.color = Color.red;
						}

						GUI.Label(LevelReq, "Level " + p.LevelRequirement, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(33 * screenScale), font = mainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });
						if (ModdedPlayer.instance.MutationPoints < p.PointsToBuy)
						{
							GUI.color = Color.red;
						}
						else
						{
							GUI.color = Color.white;
						}

						GUI.Label(Cost, "Cost in mutation points: " + p.PointsToBuy, new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(33 * screenScale), font = mainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow });
						GUI.color = Color.white;
						if (UnityEngine.Input.GetMouseButton(0) && ModdedPlayer.instance.MutationPoints >= p.PointsToBuy && PerkRequirementsMet(Perk.AllPerks[SelectedPerk_ID]) && PerkEnabled(Perk.AllPerks[SelectedPerk_ID]) && Perk.AllPerks[SelectedPerk_ID].LevelRequirement <= ModdedPlayer.instance.Level)
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
					GUIStyle descStyle = new GUIStyle(GUI.skin.box) { margin = new RectOffset(5, 5, Mathf.RoundToInt(10 * screenScale), 10), alignment = TextAnchor.UpperCenter, fontSize = Mathf.RoundToInt(28 * screenScale), font = mainFont, fontStyle = FontStyle.Normal, richText = true, wordWrap = true };
					Desc.height = descStyle.CalcHeight(new GUIContent(desctext), Desc.width) + 10 * screenScale;
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
			float btnSize = 250 * screenScale;
			float bigBtnSize = 40 * screenScale;
			float offset = Screen.width / 2 - (menus.Length * btnSize + bigBtnSize) / 2;

			for (int i = 0; i < menus.Length; i++)
			{
				Rect topButton = new Rect(offset, 35 * screenScale, btnSize, 60 * screenScale);
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
				if (GUI.Button(topButton, content, new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(40 * screenScale), font = mainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow }))
				{
					_perkpage = (Perk.PerkCategory)menus.GetValue(i);
					targetPerkOffset = wholeScreenRect.center;
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
			if (p.RequiredIds != null)
			{
				for (int i = 0; i < p.RequiredIds.Length; i++)
				{
					if (!Perk.AllPerks[p.RequiredIds[i]].IsBought)
					{
						return;
					}
				}
			}
			for (int i = 0; i < p.InheritIDs.Length; i++)
			{
				if (p.InheritIDs[i] == -1 || (Perk.AllPerks[p.InheritIDs[i]].IsBought))
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
					GUI.Label(r, p.ApplyAmount.ToString(), new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(40 * screenScale), font = mainFont, fontStyle = FontStyle.Bold, richText = true, clipping = TextClipping.Overflow, alignment = TextAnchor.MiddleCenter });
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
			float distsquared = (mousePos - r.center).sqrMagnitude;
			if (r.Contains(mousePos) && distsquared < PerkHexagonSide * PerkHexagonSide * 0.81f)
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
