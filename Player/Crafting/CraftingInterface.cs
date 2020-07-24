using UnityEngine;

namespace ChampionsOfForest.Player.Crafting
{
	public interface ICraftingMode
	{
		CustomCrafting CraftingHandler
		{
			get;
		}

		int IngredientCount
		{
			get;
		}

		bool validRecipe
		{
			get;
		}

		void Craft();

		void DrawUI(in float x, in float w, in float screenScale, in GUIStyle[] styles);
	}
}