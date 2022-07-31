namespace ChampionsOfForest.Player
{
	public class Perk
	{
		public int id;
		public int[] unlockPath;
		public int[] unlockRequirement;
		public int cost = 1;
		public int levelReq;

		public bool isBought = false;
		public bool isApplied = false;

		public delegate void OnApply();

		public OnApply onApply;

		public delegate string OnPucharseDescriptionUpdate(int level);

		public OnPucharseDescriptionUpdate updateDescription;

		public string name;
		public string originalDescription;

		public string Description
		{
			get; internal set;
		}

		public bool stackable = false;
		public int boughtTimes;

		public int textureVariation = 0;
		public float scale = 1;
		public float posX;
		public float posY;
		public object texture;
		public enum PerkCategory
		{
			MeleeOffense, RangedOffense, MagicOffense, Defense, Support, Utility
		}
		public void ResetDescription()
		{
			Description = originalDescription;
		}
		public PerkCategory category;


		public Perk()
		{
			isApplied = false;

			id = PerkDatabase.perks.Count;
			PerkDatabase.perks.Add(this);
		}
	}
}