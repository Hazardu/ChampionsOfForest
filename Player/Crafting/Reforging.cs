using System;
using System.Linq;

using UnityEngine;

namespace ChampionsOfForest.Player.Crafting
{
	public partial class CustomCrafting
	{
		public class Reforging : ICraftingMode
		{
			public int IngredientCount => 3;

			public bool validRecipe
			{
				get
				{
					if (CraftingHandler.changedItem.i == null)
						return false;
					int itemCount = 0;
					int rarity = CraftingHandler.changedItem.i.Rarity;
					for (int i = 0; i < CraftingHandler.ingredients.Length; i++)
					{
						if (CraftingHandler.ingredients[i].i != null)
						{
							if (CraftingHandler.ingredients[i].i.Rarity >= rarity)
							{
								itemCount++;
							}
							else
							{
								return false;
							}
						}
					}
					return itemCount == IngredientCount;
				}
			}

			public void Craft()
			{
				if (CraftingHandler.changedItem.i != null)
				{
					if (validRecipe)
					{
						int lvl = CraftingHandler.changedItem.i.level;
						var v = ItemDataBase.ItemBases.Where(x => x.Value.ID != CraftingHandler.changedItem.i.ID && x.Value.Rarity == CraftingHandler.changedItem.i.Rarity).Select(x => x.Value).ToArray();
						;
						var ib = v[UnityEngine.Random.Range(0, v.Length)];

						var newItem = new Item(ib, 1, 0, false)
						{
							level = lvl,
						};
						newItem.RollStats();
						Inventory.Instance.ItemSlots[CraftingHandler.changedItem.pos] = newItem;
						CraftingHandler.changedItem.i = newItem;
						Effects.Sound_Effects.GlobalSFX.Play(3);

						for (int i = 0; i < CraftingHandler.ingredients.Length; i++)
						{
							CraftingHandler.ingredients[i].RemoveItem();
						}
					}
				}
			}

			public CustomCrafting CraftingHandler => CustomCrafting.instance;

			public void DrawUI(in float x, in float w, in float screenScale, in GUIStyle[] styles)
			{
				GUI.Label(new Rect(x, (CustomCrafting.CRAFTINGBAR_HEIGHT + 5) * screenScale, w, 26 * screenScale), "Item to reforge", styles[3]);
				MainMenu.Instance.CraftingIngredientBox(new Rect(x + w / 2 - 75 * screenScale, (CustomCrafting.CRAFTINGBAR_HEIGHT + 40) * screenScale, 150 * screenScale, 150 * screenScale), CustomCrafting.instance.changedItem);
				float ypos = (CustomCrafting.CRAFTINGBAR_HEIGHT + 190) * screenScale;
				if (CustomCrafting.instance.changedItem.i != null)
				{
					int ind = 1;
					try
					{
						float mult = CustomCrafting.instance.changedItem.i.GetRarityMultiplier();

						Rect nameRect = new Rect(x + 10 * screenScale, ypos, w - 20 * screenScale, 30 * screenScale);
						ypos += 30 * screenScale;
						GUI.color = MainMenu.RarityColors[CustomCrafting.instance.changedItem.i.Rarity];
						GUI.Label(nameRect, CustomCrafting.instance.changedItem.i.name, styles[3]);
						foreach (ItemStat stat in CustomCrafting.instance.changedItem.i.Stats)
						{
							Rect statRect = new Rect(x + 10 * screenScale, ypos, w - 20 * screenScale, 26 * screenScale);
							Rect valueMinMaxRect = new Rect(statRect.xMax + 15 * screenScale, ypos, statRect.width, statRect.height);
							ypos += 26 * screenScale;
							string maxAmount = stat.GetMaxValue(CraftingHandler.changedItem.i.level, mult);
							string minAmount = stat.GetMinValue(CraftingHandler.changedItem.i.level, mult);
							string amount = stat.Amount.ToString((stat.DisplayAsPercent ? "P" : "N") + stat.RoundingCount);
							GUI.color = MainMenu.RarityColors[stat.Rarity];
							GUI.Label(statRect, ind + ".  " + stat.Name, styles[0]);
							GUI.color = Color.white;
							ind++;

							GUI.Label(statRect, amount, styles[1]);
							GUI.Label(valueMinMaxRect, "[ " + minAmount + " - " + maxAmount + " ]", styles[4]);

						}
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
					try
					{
						if (validRecipe)
						{
							if (GUI.Button(new Rect(x, ypos, w, 40 * screenScale), "Reforge item", styles[2]))
							{
								Craft();
							}
							ypos += 50 * screenScale;
						}
					}
					catch (Exception e)
					{
						Debug.LogWarning("reroll stats button ex " + e.ToString());
					}
				}
				float baseX = x + ((w - 240 * screenScale) / 2);
				float baseY = ypos + 30 * screenScale;
				if (CustomCrafting.instance.changedItem.i != null)
					for (int j = 0; j < 3; j++)
					{
						for (int k = 0; k < 1; k++)
						{
							int index = 3 * k + j;
							MainMenu.Instance.CraftingIngredientBox(new Rect(baseX + j * 80 * screenScale, baseY + k * 80 * screenScale, 80 * screenScale, 80 * screenScale), CustomCrafting.instance.ingredients[index]);
						}
					}
			}
		}
	}
}