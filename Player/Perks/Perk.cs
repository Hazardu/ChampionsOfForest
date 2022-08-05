using System.Linq;

using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest.Perks
{
	public class Perk
	{
		public enum PerkCategory
		{
			MeleeOffense, RangedOffense, MagicOffense, Defense, Support, Utility
		}
		public delegate void OnApply();
		public delegate string OnPucharseDescriptionUpdate(int level);

		internal int id,	cost, levelReq, purchaseCount, purchaseMax;
		internal int[] unlockPath, unlockRequirement;
		public bool isBought, isApplied, stackable;
		public OnApply onApply;
		public OnPucharseDescriptionUpdate updateDescription;
		internal string name;
		internal string originalDescription;
		internal int textureVariation;
		internal PerkCategory category;
		internal float scale = 1;
		internal float posX;
		internal float posY;
		

		
		public Perk()
		{
			isApplied = false;
			isBought = false;
			id = PerkDatabase.perks.Count;
			PerkDatabase.perks.Add(this);
		}


		public void UpdateDescription()
		{
			if (updateDescription != null)
			{
				if (stackable)
					Description = originalDescription + ' ' + updateDescription(purchaseCount);
				else
					Description = originalDescription + ' ' + updateDescription(1);
			}
			else
			{
				Description = originalDescription;
			}
		}
		public void ResetDescription()
		{
			Description = originalDescription;
		}
		public string Description
		{
			get; internal set;
		}


		public bool AllRequirementsBought => unlockRequirement == null || !unlockRequirement.Any(pid => PerkDatabase.perks[pid].isBought);
		public bool AnyUnlockPathBought => unlockPath == null || unlockPath.Any(pid => PerkDatabase.perks[pid].isBought);

		public bool BuyRequirementsMet => ModdedPlayer.instance.MutationPoints >= cost &&
			AllRequirementsBought &&
			AnyUnlockPathBought &&
			levelReq <= ModdedPlayer.instance.level &&
			(!stackable || purchaseCount<purchaseMax);


	}
}