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
				xp = Random.Range(40, 65);
			}
			if (spawnFunctions.turtle)
			{
				xp = Random.Range(40, 60);
			}
			if (spawnFunctions.rabbit)
			{
				xp = Random.Range(45, 60);
			}
			if (spawnFunctions.fish)
			{
				xp = Random.Range(30, 45);
			}
			if (spawnFunctions.tortoise)
			{
				xp = Random.Range(100, 110);
			}
			if (spawnFunctions.raccoon)
			{
				xp = Random.Range(200, 250);
			}
			if (spawnFunctions.deer)
			{
				xp = Random.Range(75 , 85);
			}
			if (spawnFunctions.squirrel)
			{
				xp = Random.Range(70, 75);
			}
			if (spawnFunctions.boar)
			{
				xp = Random.Range(150, 200);
			}
			if (spawnFunctions.crocodile)
			{
				xp = Random.Range(350, 450);
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
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Others);
				}
			}
			ModdedPlayer.instance.AddKillExperience(xp);
			if (xp > 82)
			{
				if(Random.value < 0.5f)
					Network.NetworkManager.SendItemDrop(ItemDatabase.GetRandomItem(xp,transform.position), transform.position + Vector3.up * 4f, ItemPickUp.DropSource.EnemyOnDeath);

			}
			base.Die();
		}
	}
}