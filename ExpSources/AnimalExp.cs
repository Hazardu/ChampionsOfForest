using ChampionsOfForest.Player;

using TheForest.Utils;

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
				xp = UnityEngine.Random.Range(200, 200);
			}
			if (spawnFunctions.deer)
			{
				xp = UnityEngine.Random.Range(75 , 80);
			}
			if (spawnFunctions.squirrel)
			{
				xp = UnityEngine.Random.Range(70, 100);
			}
			if (spawnFunctions.boar)
			{
				xp = UnityEngine.Random.Range(150, 170);
			}
			if (spawnFunctions.crocodile)
			{
				xp = UnityEngine.Random.Range(350, 390);
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

			base.Die();
		}
	}
}