using System.Linq;

using UnityEngine;

namespace ChampionsOfForest.Player.Crafting
{
	public partial class CustomCrafting
	{
		internal const float CRAFTINGBAR_HEIGHT = 50;

		public static CustomCrafting instance;
		internal ICraftingMode[] craftingModes;

		public enum CraftMode
		{
			None = -1, Rerolling, Reforging, Polishing, Empowering, IndividualRerolling
		}

		public CraftMode craftMode = CraftMode.None;
		public ICraftingMode CurrentCraftingMode => craftingModes[(int)craftMode];

		public static void Init()
		{
			instance = new CustomCrafting();
		}

		public CustomCrafting()
		{
			craftingModes = new ICraftingMode[] { new Rerolling(), new Reforging(), new Polishing(), new Empowering(),new IndividualRerolling() };
			ingredients = new CraftingIngredient[9];
			for (int i = 0; i < 9; i++)
			{
				ingredients[i] = (new CraftingIngredient());
			}
			changedItem = new CraftingIngredient();
		}

		public CraftingIngredient changedItem;
		private CraftingIngredient[] ingredients;
		public static CraftingIngredient[] Ingredients => instance.ingredients;

		public void DrawUI(in float x, in float w, in float screenScale, params GUIStyle[] styles)
		{
			if (craftMode != CraftMode.None)
			{
				CurrentCraftingMode.DrawUI(x, w, screenScale, styles);
			}
		}

		public void ClearedItem()
		{
			//nothing needs to be done
		}

		public static bool isIngredient(int index)
		{
			return instance.ingredients.Any(x => x.pos == index) || instance.changedItem.pos == index;
		}

		public static bool UpdateIndex(int index, int newIndex)
		{
			for (int i = 0; i < instance.ingredients.Length; i++)
			{
				if (instance.ingredients[i].pos == index)
				{
					if (newIndex < -1)
					{
						instance.ingredients[i].Clear();
						return true;
					}
					instance.ingredients[i].pos = newIndex;
					return true;
				}
			}
			if (instance.changedItem.pos == index)
			{
				if (newIndex < -1)
				{
					instance.changedItem.Clear();
					return true;
				}
				instance.changedItem.pos = newIndex;
				return true;
			}
			return false;
		}

		public static bool ClearIndex(int index)
		{
			for (int i = 0; i < instance.ingredients.Length; i++)
			{
				if (instance.ingredients[i].pos == index)
				{
					instance.ingredients[i].Clear();
					return true;
				}
			}
			if (instance.changedItem.pos == index)
			{
				instance.changedItem.Clear();
				return true;
			}
			return false;
		}
	}
}