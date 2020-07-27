using System;

using UnityEngine;

namespace ChampionsOfForest.Player.Crafting
{
	public partial class CustomCrafting
	{
		public class Polishing : ICraftingMode
		{
			public int selectedStat = -1;

			public bool validRecipe
			{
				get
				{
					if (CraftingHandler.changedItem.i == null || CraftingHandler.changedItem.i.Stats.Count <= selectedStat || selectedStat < 0)
						return false;
					int itemCount = 0;
					int rarity = CraftingHandler.changedItem.i.Rarity;
					for (int i = 0; i < CraftingHandler.ingredients.Length; i++)
					{
						if (CraftingHandler.ingredients[i].i != null)
						{
							if (CraftingHandler.ingredients[i].i.Rarity >= rarity)
								itemCount++;
							else
								return false;
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
						var stat = CraftingHandler.changedItem.i.Stats[selectedStat];

						stat.Amount =Mathf.Max(stat.Amount, stat.RollValue(CraftingHandler.changedItem.i.level) * CustomCrafting.instance.changedItem.i.GetRarityMultiplier());
						if (stat.ValueCap>0)
						stat.Amount = Mathf.Min(stat.Amount, stat.ValueCap);

						Effects.Sound_Effects.GlobalSFX.Play(3);
						for (int i = 0; i < CraftingHandler.ingredients.Length; i++)
						{
							CraftingHandler.ingredients[i].RemoveItem();
						}
					}
				}
			}

			public int IngredientCount => 1;

			public CustomCrafting CraftingHandler => CustomCrafting.instance;

			public void DrawUI(in float x, in float w, in float screenScale, in GUIStyle[] styles)
			{
				GUI.Label(new Rect(x, (CustomCrafting.CRAFTINGBAR_HEIGHT + 5) * screenScale, w, 26 * screenScale), "Item to enchance a stat", styles[3]);
				MainMenu.Instance.CraftingIngredientBox(new Rect(x + w / 2 - 75 * screenScale, (CustomCrafting.CRAFTINGBAR_HEIGHT + 40) * screenScale, 150 * screenScale, 150 * screenScale), CustomCrafting.instance.changedItem);
				float ypos = (CustomCrafting.CRAFTINGBAR_HEIGHT + 190) * screenScale;
				if (CustomCrafting.instance.changedItem.i != null)
				{
					try
					{
						float mult = CustomCrafting.instance.changedItem.i.GetRarityMultiplier();

						int ind = 0;
						foreach (ItemStat stat in CustomCrafting.instance.changedItem.i.Stats)
						{
							Rect statRect = new Rect(x + 10 * screenScale, ypos, w - 20 * screenScale, 26 * screenScale);
							Rect valueMinMaxRect = new Rect(statRect.xMax + 15 * screenScale, ypos, statRect.width, statRect.height);
							ypos += 26 * screenScale;
							float maxAmount = stat.GetMaxValue(CraftingHandler.changedItem.i.level) * mult;
							float minAmount = stat.GetMinValue(CraftingHandler.changedItem.i.level) * mult;
							float amount = stat.Amount;
							if (stat.DisplayAsPercent)
							{
								amount *= 100;
								maxAmount *= 100;
								minAmount *= 100;
							}
							amount = (float)Math.Round(amount, stat.RoundingCount);
							maxAmount = (float)Math.Round(maxAmount, stat.RoundingCount);
							minAmount = (float)Math.Round(minAmount, stat.RoundingCount);
							GUI.color = MainMenu.RarityColors[stat.Rarity];
							if (selectedStat == ind)
							{
								GUI.Label(statRect, "• " + stat.Name, new GUIStyle(styles[0]) { fontStyle = FontStyle.Bold, fontSize = Mathf.RoundToInt(19 * screenScale) });
							}
							else
							{
								if (GUI.Button(statRect, stat.Name, styles[0]))
								{
									selectedStat = ind;
								}
							}
							GUI.color = Color.white;
							ind++;
							if (stat.DisplayAsPercent)
							{
								GUI.Label(statRect, amount.ToString("N" + stat.RoundingCount) + "%", styles[1]);
								GUI.Label(valueMinMaxRect, "[" + minAmount.ToString("N" + stat.RoundingCount) + "%" + " - " + maxAmount.ToString("N" + stat.RoundingCount) + "%]", styles[4]);
							}
							else
							{
								GUI.Label(statRect, amount.ToString("N" + stat.RoundingCount), styles[1]);
								GUI.Label(valueMinMaxRect, "[" + minAmount.ToString("N" + stat.RoundingCount) + " - " + maxAmount.ToString("N" + stat.RoundingCount) + "]", styles[4]);
							}
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
							if (GUI.Button(new Rect(x, ypos, w, 40 * screenScale), "Reroll stat value", styles[2]))
							{
								Craft();
							}
							ypos += 50 * screenScale;
						}
						else
						{
							GUI.color = Color.gray;
							GUI.Label(new Rect(x, ypos, w, 40 * screenScale), "Select a Stat", styles[2]);
							GUI.color = Color.white;
							ypos += 50 * screenScale;
						}
					}
					catch (Exception e)
					{
						Debug.LogWarning("reroll stats button ex " + e.ToString());
					}
				}
				float baseX = x + ((w - 200 * screenScale) / 2);
				float baseY = ypos + 30 * screenScale;
				if (CustomCrafting.instance.changedItem.i != null)
					for (int j = 0; j < 1; j++)
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