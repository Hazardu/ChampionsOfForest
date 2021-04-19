using TheForest.Items;

namespace ChampionsOfForest.Items
{
	public class TheForestItemDBMod : TheForest.Items.ItemDatabase
	{
		public override void OnEnable()
		{
			base.OnEnable();
			//turtle shell is now addable to quick select
			this._itemsCache[141]._type |= TheForest.Items.Item.Types.Weapon;
		}
	}
}