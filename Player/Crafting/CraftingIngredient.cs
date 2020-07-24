namespace ChampionsOfForest.Player.Crafting
{
	public partial class CustomCrafting
	{
		public class CraftingIngredient
		{
			public Item i = null;
			public int pos = -1;

			public void Assign(int index, Item i)
			{
				this.i = i;
				pos = index;
			}

			public void RemoveItem()
			{
				if (Inventory.Instance.ItemSlots.ContainsKey(pos))
				{
					Inventory.Instance.ItemSlots[pos].Amount--;
					if (Inventory.Instance.ItemSlots[pos].Amount < 1)
						Inventory.Instance.ItemSlots[pos] = null;
				}
				i = null;
				pos = -1;
			}

			public void Clear()
			{
				i = null;
				pos = -1;
			}
		}
	}
}