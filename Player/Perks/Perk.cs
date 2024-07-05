using UnityEngine;

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
		public delegate bool CanBuy();

		public OnApply onApply;
		public CanBuy canBuy;

		public delegate string OnPucharseDescriptionUpdate(int level);

		public OnPucharseDescriptionUpdate updateDescription;

		public string name;
		public string originalDescription;

		public string Description
		{
			get; internal set;
		}

		public Texture2D texture;

		public bool stackable = false;
		public int boughtTimes;

		public int textureVariation = 0;
		public float scale = 1;
		public float posX;
		public float posY;

		public enum PerkCategory
		{
			MeleeOffense, RangedOffense, MagicOffense, Defense, Support, Utility
		}
		public void ResetDescription()
		{
			Description = originalDescription;
		}
		public PerkCategory category;

		public Perk(string name, string description, int[] inheritIDs, float x, float y, PerkCategory category, float size, int levelRequirement, OnApply applyMethods)
		{
			this.name = name;
			this.originalDescription = description;
			unlockPath = inheritIDs;
			this.category = category;
			scale = size;
			stackable = false;
			posX = x;
			posY = y;
			levelReq = levelRequirement;
			onApply = applyMethods;
			id = PerkDatabase.perks.Count;
			isApplied = false;
			PerkDatabase.perks.Add(this);
			canBuy = null;
		}

		public Perk()
		{
			isApplied = false;

			id = PerkDatabase.perks.Count;
			PerkDatabase.perks.Add(this);
			canBuy = null;

		}

		public Perk(string name, string description, int inheritIDs, float x, float y, PerkCategory category, float size, int levelRequirement, OnApply applyMethods)
		{
			this.name = name;
			this.originalDescription = description;
			unlockPath = new int[] { inheritIDs };
			posX = x;
			posY = y;
			this.category = category;
			scale = size;
			stackable = false;
			levelReq = levelRequirement;
			onApply = applyMethods;
			id = PerkDatabase.perks.Count;
			isApplied = false;
			PerkDatabase.perks.Add(this);
			canBuy = null;

		}

		public void OnBuy()
		{
			try
			{
				if (updateDescription != null)
				{
					if (stackable)
						Description = originalDescription + ' ' + updateDescription(boughtTimes);
					else
						Description = originalDescription + ' ' + updateDescription(1);
				}
				else
				{
					Description = originalDescription;
				}
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		public bool Locked
		{
			get
			{
				for (int i = 0; i < unlockPath.Length; i++)
				{
					if (unlockPath[i] == -1 || (PerkDatabase.perks[unlockPath[i]].isBought))
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}