using System;
using System.Linq;

using ChampionsOfForest.Player;
using ChampionsOfForest.Player.Crafting;

using TheForest.Player.Data;

using UnityEngine;

namespace ChampionsOfForest
{
	public partial class MainMenu : MonoBehaviour
	{
		private float InventoryScrollAmount = 0;
		private int SelectedItem;
		public int DraggedItemIndex;

		public bool isDragging;
		private bool consumedsomething;
		private bool drawTotal;

		public Item DraggedItem = null;
		private Vector2 itemPos;
		private Vector2 slotDim;

		#region InventoryMethods

		private void DrawInventory()
		{
			Rect SlotsRect = new Rect(0, 0, Inventory.Width * slotDim.x, Screen.height);
			GUI.Box(SlotsRect, "Inventory", new GUIStyle(GUI.skin.box) { font = secondaryFont, fontSize = Mathf.RoundToInt(65 * screenScale) });
			SelectedItem = -1;

			try
			{
				for (int y = 0; y < Inventory.Height; y++)
				{
					for (int x = 0; x < Inventory.Width; x++)
					{
						int index = y * Inventory.Width + x;

						DrawInvSlot(new Rect(SlotsRect.x + slotDim.x * x, SlotsRect.y + slotDim.y * y + 160 * screenScale + InventoryScrollAmount, slotDim.x, slotDim.y), index);
					}
				}

				//PlayerSlots
				Rect eq = new Rect(SlotsRect.xMax + 30 * screenScale, 0, 420 * screenScale, Screen.height);
				GUI.Box(eq, "Equipment", new GUIStyle(GUI.skin.box) { font = secondaryFont, fontSize = Mathf.RoundToInt(65 * screenScale) });
				Rect head = new Rect(Vector2.zero, slotDim * 2)
				{
					center = eq.center
				};
				head.y -= 3.5f * head.height;

				Rect chest = new Rect(head);
				chest.y += chest.height + 50 * screenScale;

				Rect pants = new Rect(chest);
				pants.y += pants.height + 50 * screenScale;

				Rect boots = new Rect(pants);
				boots.y += boots.height + 50 * screenScale;

				Rect shoulders = new Rect(chest);
				shoulders.position += new Vector2(-chest.width, -chest.height / 2);

				Rect tallisman = new Rect(chest);
				tallisman.position += new Vector2(chest.width, -chest.height / 2);

				Rect gloves = new Rect(shoulders);
				gloves.y += gloves.height + 50 * screenScale;

				Rect bracer = new Rect(tallisman);
				bracer.y += bracer.height + 50 * screenScale;

				Rect ringR = new Rect(bracer);
				ringR.position += new Vector2(chest.width / 2, chest.height + 50 * screenScale);

				Rect ringL = new Rect(gloves);
				ringL.position += new Vector2(-chest.width / 2, chest.height + 50 * screenScale);

				Rect weapon = new Rect(ringL);
				weapon.y += weapon.height * 1.5f + 50 * screenScale;

				Rect offhand = new Rect(ringR);
				offhand.y += offhand.height * 1.5f + 50 * screenScale;

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

				if (ModdedPlayer.Stats.perk_craftingReroll)
				{
					DrawCrafting(eq.xMax + 30 * screenScale);
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
				if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
				{
					var targetSlotID = Inventory.Instance.ItemSlots[SelectedItem].destinationSlotID;
					if (SelectedItem > -1 && targetSlotID != -1 && Inventory.Instance.ItemSlots[targetSlotID] != null)
					{
						DrawItemInfo(itemPos, Inventory.Instance.ItemSlots[SelectedItem], true, Inventory.Instance.ItemSlots[targetSlotID]);
					}
					else
						DrawItemInfo(itemPos, Inventory.Instance.ItemSlots[SelectedItem]);
				}
				else
				{
					DrawItemInfo(itemPos, Inventory.Instance.ItemSlots[SelectedItem]);
				}
			}

			if (isDragging)
			{
				Rect r = new Rect(mousePos, slotDim);
				GUI.color = new Color(1, 1, 1, 0.5f);
				GUI.DrawTexture(r, DraggedItem.icon);
				GUI.color = new Color(1, 1, 1, 1);

				if (UnityEngine.Input.GetMouseButtonUp(0))
				{
					if (DraggedItem.Equipped)
					{
						DraggedItem.OnUnequip();
						Inventory.Instance.ItemSlots[DraggedItemIndex].Equipped = false;
					}
					Inventory.Instance.DropItem(DraggedItemIndex);
					CustomCrafting.ClearIndex(DraggedItemIndex);

					DraggedItem = null;
					DraggedItemIndex = -1;
					isDragging = false;
				}
			}
		}

		private GUIStyle statNameStyle;
		private GUIStyle statValueStyle;
		private GUIStyle craftBtnStyle;
		private GUIStyle craftHeaderStyle;
		private GUIStyle statMinMaxValueStyle;

		private void DrawCrafting(float x)
		{
			Rect craftingrect = new Rect(x, 0, Mathf.Min((Screen.width - x), 400 * screenScale), Screen.height);
			GUI.Box(craftingrect, "");

			float btnW = craftingrect.width / 4;

			int i = 0;
			GUIStyle style = new GUIStyle(GUI.skin.button) { font = mainFont,fontSize=Mathf.RoundToInt(25 * screenScale) };
			if (GUI.Button(new Rect(x + i * btnW, 0, btnW, CustomCrafting.CRAFTINGBAR_HEIGHT * screenScale), "Rerolling", style))
			{
				CustomCrafting.instance.craftMode = CustomCrafting.CraftMode.Rerolling;
				for (int index = 0; index < CustomCrafting.Ingredients.Length; index++)
				{
					CustomCrafting.Ingredients[index].Clear();
				}
			}
			i++;

			if (ModdedPlayer.Stats.perk_craftingReforge)
			{
				if (GUI.Button(new Rect(x + i * btnW, 0, btnW, CustomCrafting.CRAFTINGBAR_HEIGHT * screenScale), "Reforging", style))
				{
					CustomCrafting.instance.craftMode = CustomCrafting.CraftMode.Reforging;
					for (int index = 0; index < CustomCrafting.Ingredients.Length; index++)
					{
						CustomCrafting.Ingredients[index].Clear();
					}
				}
				i++;
			}
			if (ModdedPlayer.Stats.perk_craftingPolishing)
			{
				if (GUI.Button(new Rect(x + i * btnW, 0, btnW, CustomCrafting.CRAFTINGBAR_HEIGHT * screenScale), "Polishing", style))
				{
					CustomCrafting.instance.craftMode = CustomCrafting.CraftMode.Polishing;
					for (int index = 0; index < CustomCrafting.Ingredients.Length; index++)
					{
						CustomCrafting.Ingredients[index].Clear();
					}
					((CustomCrafting.Polishing)CustomCrafting.instance.CurrentCraftingMode).selectedStat = -1;
				}
				i++;
			}
			if (ModdedPlayer.Stats.perk_craftingEmpowering)
			{
				if (GUI.Button(new Rect(x + i * btnW, 0, btnW, CustomCrafting.CRAFTINGBAR_HEIGHT * screenScale), "Empowering", style))
				{
					CustomCrafting.instance.craftMode = CustomCrafting.CraftMode.Empowering;
					for (int index = 0; index < CustomCrafting.Ingredients.Length; index++)
					{
						CustomCrafting.Ingredients[index].Clear();
					}
				}
				i++;
			}
			CustomCrafting.instance.DrawUI(x + 10, 400 * screenScale, screenScale, statNameStyle, statValueStyle, craftBtnStyle, craftHeaderStyle, statMinMaxValueStyle);
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
				if (r.Contains(mousePos))
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
				if (r.Contains(mousePos))
				{
					if (UnityEngine.Input.GetMouseButtonUp(0))
					{
						if (!(CustomCrafting.Ingredients.Any(x => x.i == DraggedItem) || CustomCrafting.instance.changedItem.i == DraggedItem) && DraggedItemIndex > -1)
							ingredient.Assign(DraggedItemIndex, DraggedItem);
						DraggedItem = null;
						DraggedItemIndex = -1;
						isDragging = false;
					}
				}
			}
		}

		private void DrawItemInfo(Vector2 pos, Item item, bool drawCompare = false, Item comparedItem = null)
		{
			if (item == null)
			{
				return;
			}

			float width = 390 * screenScale;
			Vector2 originalPos = pos;
			pos.x += 5 * screenScale;

			if (pos.x + width > Screen.width)
			{
				pos.x -= width + slotDim.x;
				pos.x -= 5 * screenScale;
			}

			Rect descriptionBox = new Rect(originalPos, new Vector2(width + 10 * screenScale, 500 * screenScale));
			Rect ItemNameRect = new Rect(pos.x, pos.y, width, 50 * screenScale);
			GUIStyle ItemNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, fontSize = Mathf.RoundToInt(35 * screenScale), fontStyle = FontStyle.Bold, font = mainFont };
			float y = 70 + pos.y;
			Rect[] StatRects = new Rect[item.Stats.Count];
			GUIStyle StatNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = Mathf.RoundToInt(20 * screenScale), font = mainFont };
			GUIStyle StatValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight, fontSize = Mathf.RoundToInt(20 * screenScale), fontStyle = FontStyle.Bold, font = mainFont };

			for (int i = 0; i < StatRects.Length; i++)
			{
				StatRects[i] = new Rect(pos.x, y, width, 22 * screenScale);
				y += 22 * screenScale;
			}
			y += 30 * screenScale;

			Rect LevelAndTypeRect = new Rect(pos.x, y, width, 35 * screenScale);
			GUIStyle TypeStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(14 * screenScale), font = mainFont };
			GUIStyle LevelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerRight, fontSize = Mathf.RoundToInt(28 * screenScale), font = mainFont, fontStyle = FontStyle.Italic };
			y += 30 * screenScale;

			GUIStyle DescriptionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(16 * screenScale), font = mainFont };
			GUIStyle LoreStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(13 * screenScale), font = mainFont, fontStyle = FontStyle.Italic };
			GUIStyle TooltipStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, fontSize = Mathf.RoundToInt(15 * screenScale), font = mainFont, fontStyle = FontStyle.Bold };

			Rect DescrRect = new Rect(pos.x, y, width, DescriptionStyle.CalcHeight(new GUIContent(item.description), width));
			y += DescrRect.height;
			y += 30 * screenScale;

			Rect LoreRect = new Rect(pos.x, y, width, LoreStyle.CalcHeight(new GUIContent(item.lore), width));
			y += LoreRect.height;
			y += 30 * screenScale;

			Rect toolTipTitleRect = new Rect(pos.x, y, width, TooltipStyle.fontSize + 2);
			y += toolTipTitleRect.height;
			Rect toolTipRect = new Rect(pos.x, y, width, TooltipStyle.CalcHeight(new GUIContent(item.tooltip), width));
			y += toolTipRect.height;
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
			GUI.DrawTexture(descriptionBox, blackSquareTex);
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
			if (drawTotal)
			{
				
					int count = item.Stats.Count;
				if (count > 0)
				{
					GUIStyle totalStatStyle = new GUIStyle(StatValueStyle)
					{
						fontSize = (int)(18 * screenScale),
						alignment = TextAnchor.MiddleLeft,
					};
					Rect totalBG = new Rect(StatRects[0].xMax+5 * screenScale, StatRects[0].y - 45 * screenScale, 105 * screenScale,0);
					totalBG.yMax = StatRects[StatRects.Length - 1].yMax + 5f;
					GUI.color = new Color(1, 1, 1, 0.8f);
					GUI.DrawTexture(totalBG, blackSquareTex);
					GUI.color = Color.gray;
					GUI.Label(new Rect(totalBG.x, totalBG.y, totalBG.width, 30 * screenScale), "Total", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, font = mainFont });
					for (int i = 0; i < count; i++)
					{
						if (item.Stats[i].GetTotalStat != null)
						{
							Rect rect = new Rect(StatRects[i].xMax + 3f, StatRects[i].y, 100 * screenScale, StatRects[i].height);
							GUI.Label(rect, item.Stats[i].GetTotalStat(), totalStatStyle);
						}
					}
					GUI.color = Color.white;


				}
			}

			if (drawCompare)
			{
				var grouped = item.GetGroupedStats();
				var otherGrouped = comparedItem.GetGroupedStats();
				GUIStyle statCompareStyle = new GUIStyle(StatValueStyle)
				{
					fontStyle = FontStyle.Italic,
					fontSize = (int)(18 * screenScale),
					alignment = TextAnchor.MiddleLeft,
				};
				if (grouped != null)
				{
					Rect compareBG = new Rect(drawTotal?StatRects[0].xMax + 110 * screenScale: StatRects[0].xMax + 5 * screenScale, StatRects[0].y - 45 * screenScale, 105 * screenScale,0);
					compareBG.yMax = StatRects[StatRects.Length - 1].yMax + 5f;

					GUI.color = new Color(1, 1, 1, 0.8f);
					GUI.DrawTexture(compareBG, blackSquareTex);
					GUI.color = Color.gray;
					GUI.Label(new Rect(compareBG.x, compareBG.y, compareBG.width, 30 * screenScale), "Compare", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, font = mainFont });
					int count = item.Stats.Count;
					for (int i = 0; i < count; i++)
					{
						Rect compareRect = new Rect(compareBG.x + 5 * screenScale, StatRects[i].y, 100 * screenScale, StatRects[i].height);
						//object baseVarValue = item.Stats[i].GetVariable();
						//dynamic castedValue = Convert.ChangeType(baseVarValue, item.Stats[i].variableType);

						float statIncr = otherGrouped != null && otherGrouped.ContainsKey(item.Stats[i].StatID) ? -otherGrouped[item.Stats[i].StatID] : 0;
						statIncr += grouped[item.Stats[i].StatID];
						string text = "↑   +";
						if (statIncr > 0)
						{
							GUI.color = Color.green;
						}
						else if (statIncr < 0)
						{
							GUI.color = Color.red;
							text = "↓   ";
						}
						if (item.Stats[i].DisplayAsPercent)
							statIncr *= 100;
						statIncr = (float)Math.Round(statIncr, item.Stats[i].RoundingCount);
						text += statIncr.ToString("N");
						if (item.Stats[i].DisplayAsPercent)
							text += "%";

						GUI.Label(compareRect, text, statCompareStyle);
					}
				}
			}
			GUI.color = Color.white;
			switch (item.type)
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
			if (item.level <= ModdedPlayer.instance.level)
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
		}

		private float hoveredOverID = -1;
		private float DraggedItemAlpha = 0;

		private void DrawInvSlot(Rect r, int index)
		{
			Color frameColor = Color.black;
			GUI.DrawTexture(r, Res.ResourceLoader.instance.LoadedTextures[12]);

			if (Inventory.Instance.ItemSlots[index] != null)
			{
				frameColor = RarityColors[Inventory.Instance.ItemSlots[index].Rarity];
				if (Inventory.Instance.ItemSlots[index].icon != null)
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
					if (ModdedPlayer.instance.level < Inventory.Instance.ItemSlots[index].level && index < -1)
					{
						frameColor = Color.black;
						GUI.color = Color.black;
					}
					if (CustomCrafting.isIngredient(index))
						GUI.color = new Color(0, 0, 0, 0.4f);
					else
						GUI.color = new Color(1, 1, 1, 1);

					GUI.DrawTexture(itemRect, Inventory.Instance.ItemSlots[index].icon);
					if (Inventory.Instance.ItemSlots[index].StackSize > 1)
					{
						GUI.color = Color.white;

						GUI.Label(r, Inventory.Instance.ItemSlots[index].Amount.ToString("N0"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerLeft, fontSize = Mathf.RoundToInt(15 * screenScale), font = mainFont, fontStyle = FontStyle.Bold });
					}

					if (isDragging)
					{
						if (r.Contains(mousePos))
						{
							bool canPlace = DraggedItem.CanPlaceInSlot(in index);
							if (index > -1)
							{
								canPlace = true;
							}
							if (DraggedItemIndex < 0)
							{
								if (DraggedItem.type != Inventory.Instance.ItemSlots[index].type)
								{
									if (DraggedItemIndex == -13)
									{
										if (Inventory.Instance.ItemSlots[index].type != BaseItem.ItemType.Shield || Inventory.Instance.ItemSlots[index].type != BaseItem.ItemType.Quiver || Inventory.Instance.ItemSlots[index].type != BaseItem.ItemType.SpellScroll)
										{
											canPlace = false;
										}
									}
									else
									{
										canPlace = false;
									}
								}
							}
							
							// Prevent Inventory Item Dupe
                            if (CustomCrafting.isIngredient(index)) canPlace = false;

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
								if (Inventory.Instance.ItemSlots[index].CombineItems(DraggedItem))
								{
									Inventory.Instance.ItemSlots[DraggedItemIndex].Amount--;
									if (Inventory.Instance.ItemSlots[DraggedItemIndex].Amount <= 0)
									{
										Inventory.Instance.ItemSlots[DraggedItemIndex] = null;
									}
									DraggedItem = null;
									DraggedItemIndex = -1;
									return;
								}
								if (index < -1)
								{
									if (canPlace)
									{
										Inventory.SwapItems(index, DraggedItemIndex);

										DraggedItem = null;
										DraggedItemIndex = -1;
										isDragging = false;
									}
									else
									{
										Inventory.Instance.ItemSlots[DraggedItemIndex] = DraggedItem;
										DraggedItem = null;
										DraggedItemIndex = -1;
									}
								}
								else
								{
									if (DraggedItem.ID != Inventory.Instance.ItemSlots[index].ID
									 || DraggedItem.Amount == DraggedItem.StackSize
									 || Inventory.Instance.ItemSlots[index].Amount == Inventory.Instance.ItemSlots[index].StackSize
									 || (Inventory.Instance.ItemSlots[index].StackSize <= 1 && DraggedItem.StackSize <= 1))
									{
										if (canPlace)
										{
											//replace items
											Inventory.SwapItems(index, DraggedItemIndex);
											DraggedItem = null;
											DraggedItemIndex = -1;
											isDragging = false;
										}
									}
									else if (DraggedItemIndex != index)
									{
										//stack items
										int i = DraggedItem.Amount + Inventory.Instance.ItemSlots[index].Amount - DraggedItem.StackSize;
										if (i > 0)  //too much to stack completely
										{
											Inventory.Instance.ItemSlots[index].Amount = Inventory.Instance.ItemSlots[index].StackSize;
											Inventory.Instance.ItemSlots[DraggedItemIndex].Amount = i;
											DraggedItem = null;
											DraggedItemIndex = -1;
											isDragging = false;
										}
										else //enough to stack completely
										{
											Inventory.Instance.ItemSlots[index].Amount += DraggedItem.Amount;
											Inventory.Instance.ItemSlots[DraggedItemIndex] = null;

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
						if (r.Contains(mousePos))
						{
							if (UnityEngine.Input.GetMouseButtonDown(0) && !UnityEngine.Input.GetKey(KeyCode.LeftShift) && !UnityEngine.Input.GetKey(KeyCode.LeftControl))
							{
								Effects.Sound_Effects.GlobalSFX.Play(0);

								isDragging = true;
								DraggedItem = Inventory.Instance.ItemSlots[index];
								DraggedItemIndex = index;
							}
							else if (UnityEngine.Input.GetMouseButtonDown(1) && index > -1 && !UnityEngine.Input.GetKey(KeyCode.LeftShift) && !UnityEngine.Input.GetKey(KeyCode.LeftControl))
							{
								Effects.Sound_Effects.GlobalSFX.Play(0);

								if (!consumedsomething)
								{
									consumedsomething = true;
									if (Inventory.Instance.ItemSlots[index].OnConsume())
									{
										Inventory.Instance.ItemSlots[index].Amount--;
										if (Inventory.Instance.ItemSlots[index].Amount <= 0)
										{
											Inventory.Instance.ItemSlots[index] = null;
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
					if (r.Contains(mousePos))
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
					if (r.Contains(mousePos) && UnityEngine.Input.GetMouseButtonUp(0))
					{
						isDragging = false;
						if (index < -1)
						{
							bool canPlace = DraggedItem.CanPlaceInSlot(in index);
							if (canPlace)
							{
								Inventory.Instance.ItemSlots[index] = DraggedItem;
								Inventory.Instance.ItemSlots[DraggedItemIndex] = null;
								CustomCrafting.UpdateIndex(DraggedItemIndex, index);
								DraggedItem = null;
								DraggedItemIndex = -1;
								Effects.Sound_Effects.GlobalSFX.Play(1);
							}
							else
							{
								Inventory.Instance.ItemSlots[index] = null;
								Inventory.Instance.ItemSlots[DraggedItemIndex] = DraggedItem;
								DraggedItem = null;
								DraggedItemIndex = -1;
							}
						}
						else if (DraggedItemIndex != index)
						{
							Inventory.Instance.ItemSlots[index] = DraggedItem;
							Inventory.Instance.ItemSlots[DraggedItemIndex] = null;
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
			if (r.Contains(mousePos))
			{
				if (Inventory.Instance.ItemSlots[index] != null)
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
			Rect TitleR = new Rect(r.x, r.y - 35 * screenScale, r.width, 35 * screenScale);
			GUI.Label(TitleR, title, new GUIStyle(GUI.skin.box) { font = mainFont, fontSize = Mathf.RoundToInt(20 * screenScale), wordWrap = false, alignment = TextAnchor.MiddleCenter, clipping = TextClipping.Overflow });
			DrawInvSlot(r, index);
		}

		#endregion InventoryMethods
	}
}
