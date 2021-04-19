using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	internal class AnimalExp : animalHealth
	{
		protected override void Die()
		{
			long xp = 0;
			if (spawnFunctions.lizard)
			{
				xp = UnityEngine.Random.Range(40, 65);
			}
			if (spawnFunctions.turtle)
			{
				xp = UnityEngine.Random.Range(40, 60);
			}
			if (spawnFunctions.rabbit)
			{
				xp = UnityEngine.Random.Range(45, 60);
			}
			if (spawnFunctions.fish)
			{
				xp = UnityEngine.Random.Range(30, 45);
			}
			if (spawnFunctions.tortoise)
			{
				xp = UnityEngine.Random.Range(100, 110);
			}
			if (spawnFunctions.raccoon)
			{
				xp = UnityEngine.Random.Range(200, 250);
			}
			if (spawnFunctions.deer)
			{
				xp = UnityEngine.Random.Range(75 , 85);
			}
			if (spawnFunctions.squirrel)
			{
				xp = UnityEngine.Random.Range(70, 75);
			}
			if (spawnFunctions.boar)
			{
				xp = UnityEngine.Random.Range(150, 200);
			}
			if (spawnFunctions.crocodile)
			{
				xp = UnityEngine.Random.Range(350, 450);
			}

			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(10);
						w.Write(xp);
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
				}
			}
			ModdedPlayer.instance.AddKillExperience(xp);
			if (xp > 82)
			{
				if(Random.value < 0.5f)
				Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(xp), transform.position + Vector3.up * 4f);

			}
			base.Die();
		}
	}
}