﻿using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	internal class CutEffigyMod : CutEffigy
	{
		protected override void CutDown()
		{
			if (!breakEventPlayed)
			{
				long expAmount = 35;
				ModdedPlayer.instance.AddFinalExperience(expAmount);
				if (!GameSetup.IsMpClient && Random.value * ModdedPlayer.Stats.magicFind < 0.5f)
				{
					Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(170*ModdedPlayer.Stats.magicFind.Value, EnemyProgression.Enemy.NormalSkinnyMale,ModSettings.difficulty, transform.position), transform.position + Vector3.up * (1.75f), ItemPickUp.DropSource.Effigy);
				}
				if (ModdedPlayer.Stats.perk_doubleStickHarvesting)
				{
					Object.Instantiate(Loot, Loot.transform.position, Loot.transform.rotation);
				}

			}
			base.CutDown();
		}
	}
}