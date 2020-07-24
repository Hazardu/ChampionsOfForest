using TheForest.Utils;

namespace ChampionsOfForest.ExpSources
{
	internal class AnimalExp : animalHealth
	{
		protected override void Die()
		{
			int xp = 0;
			if (spawnFunctions.lizard)
			{
				xp = UnityEngine.Random.Range(20, 25);
			}
			if (spawnFunctions.turtle)
			{
				xp = UnityEngine.Random.Range(10, 20);
			}
			if (spawnFunctions.rabbit)
			{
				xp = UnityEngine.Random.Range(25, 30);
			}
			if (spawnFunctions.fish)
			{
				xp = UnityEngine.Random.Range(10, 15);
			}
			if (spawnFunctions.tortoise)
			{
				xp = UnityEngine.Random.Range(30, 35);
			}
			if (spawnFunctions.raccoon)
			{
				xp = UnityEngine.Random.Range(40, 45);
			}
			if (spawnFunctions.deer)
			{
				xp = UnityEngine.Random.Range(50, 60);
			}
			if (spawnFunctions.squirrel)
			{
				xp = UnityEngine.Random.Range(10, 20);
			}
			if (spawnFunctions.boar)
			{
				xp = UnityEngine.Random.Range(60, 70);
			}
			if (spawnFunctions.crocodile)
			{
				xp = UnityEngine.Random.Range(180, 200);
			}

			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(11);
						w.Write(xp);
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				}
			}
			else
			{
				ModdedPlayer.instance.AddFinalExperience(xp);
			}

			base.Die();
		}
	}
}