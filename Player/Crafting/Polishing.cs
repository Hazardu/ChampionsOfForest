using System;

using ChampionsOfForest.Localization;

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
					if (CraftingHandler.changedItem.i == null || CraftingHandler.changedItem.i.stats.Count <= selectedStat || selectedStat < 0)
						return false;
					if (CraftingHandler.changedItem.i.stats[selectedStat].id == 3000)
						return false;
					int itemCount = 0;
					int rarity = CraftingHandler.changedItem.i.rarity;
					for (int i = 0; i < CraftingHandler.ingredients.Length; i++)
					{
						if (CraftingHandler.ingredients[i].i != null)
						{
							if (CraftingHandler.ingredients[i].i.rarity >= rarity)
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
						var stat = CraftingHandler.changedItem.i.stats[selectedStat];
						if (stat.id > 3000)
						{
							CraftingHandler.changedItem.i.stats[selectedStat] = new ItemStat(ItemDatabase.Stats[3000]); //set to empty socket
						}
						else
						{
							stat.amount = stat.RollValue(CraftingHandler.changedItem.i.level) * CustomCrafting.instance.changedItem.i.GetRarityMultiplier();
							if (stat.valueCap != 0)
								stat.amount = Mathf.Min(stat.amount, stat.valueCap);
							stat.amount *= stat.multipier;
						}
						Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.Purge);
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
				GUI.Label(new Rect(x, (CustomCrafting.CRAFTINGBAR_HEIGHT + 5) * screenScale, w, 26 * screenScale), Translations.Polishing_1/*Item to enhance a stat*/, styles[3]); //tr
				MainMenu.Instance.CraftingIngredientBox(new Rect(x + w / 2 - 75 * screenScale, (CustomCrafting.CRAFTINGBAR_HEIGHT + 40) * screenScale, 150 * screenScale, 150 * screenScale), CustomCrafting.instance.changedItem);
				float ypos = (CustomCrafting.CRAFTINGBAR_HEIGHT + 190) * screenScale;
				if (CustomCrafting.instance.changedItem.i != null)
				{
					try
					{
						float mult = CustomCrafting.instance.changedItem.i.GetRarityMultiplier();

						int ind = 0;
						foreach (ItemStat stat in CustomCrafting.instance.changedItem.i.stats)
						{
							Rect statRect = new Rect(x + 10 * screenScale, ypos, w - 20 * screenScale, 26 * screenScale);
							Rect valueMinMaxRect = new Rect(statRect.xMax + 15 * screenScale, ypos, statRect.width, statRect.height);
							ypos += 26 * screenScale;
							string maxAmount = stat.GetMaxValue(CraftingHandler.changedItem.i.level, mult);
							string minAmount = stat.GetMinValue(CraftingHandler.changedItem.i.level, mult);
							string amount = stat.amount.ToString((stat.displayAsPercent ? "P" : "N") + stat.roundingCount);

							GUI.color = MainMenu.RarityColors[stat.rarity];
							if (selectedStat == ind)
							{
								GUI.Label(statRect, "• " + stat.name + " •", new GUIStyle(styles[0]) { fontStyle = FontStyle.Bold, fontSize = Mathf.RoundToInt(19 * screenScale) });
							}
							else
							{
								if (GUI.Button(statRect, stat.name, styles[0]))
								{
									selectedStat = ind;
								}
							}
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

							if (GUI.Button(new Rect(x, ypos, w, 40 * screenScale), CraftingHandler.changedItem.i.stats[selectedStat].id > 3000 ? Translations.IndividualRerolling_3/*Empty socket*/ : Translations.Polishing_2/*Reroll stat value*/, styles[2])) //tr
							{
								Craft();
							}
							ypos += 50 * screenScale;
						}
						else
						{
							GUI.color = Color.gray;
							GUI.Label(new Rect(x, ypos, w, 40 * screenScale), Translations.IndividualRerolling_4/*Select a Stat*/, styles[2]); //tr
							GUI.color = Color.white;
							ypos += 50 * screenScale;
						}
					}
					catch (Exception e)
					{
						Debug.LogWarning("reroll stats button ex " + e);
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