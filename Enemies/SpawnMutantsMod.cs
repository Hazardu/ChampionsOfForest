using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Enemies
{
	public class SpawnMutantsMod : spawnMutants
	{
		protected override void OnDestroy()
		{
			if ((spawnInCave || sinkholeSpawn) && ModSettings.randomCaveSpawns)
			{
				var copy = Instantiate(this, transform.position,transform.rotation);
				copy.alreadySpawned = false;
				copy.RandomizeAmounts();
				CotfUtils.Log("Destroying spawner", true);
			}

			base.OnDestroy();
		}
		private void RandomizeAmounts()
		{
			this.amount_male_skinny = this.amount_female_skinny = this.amount_skinny_pale = this.amount_male = this.amount_female = this.amount_fireman = this.amount_pale = this.amount_armsy = this.amount_vags = this.amount_baby = this.amount_fat = this.amount_girl = 0;
			var num = UnityEngine.Random.Range(1, 10);
			for (int i = 0; i < num; i++)
			{
				int dist = UnityEngine.Random.Range(0, 8);
				switch (dist)
				{
					case 0:
						amount_skinny_pale++;
						break;
					case 1:
						amount_pale++;
						break;
					case 2:
						amount_fireman++;
						break;
					case 3:
						amount_armsy++;
						break;
					case 4:
						amount_vags++;
						break;
					case 5:
						amount_baby++;
						break;
					case 6:
						amount_fat++;
						break;
					case 7:
						amount_girl++;
						break;
				}
			}
		}
		public override void updateSpawnConditions()
		{
			int num = this.amount_male_skinny + this.amount_female_skinny + this.amount_skinny_pale + this.amount_male + this.amount_female + this.amount_fireman + this.amount_pale + this.amount_armsy + this.amount_vags + this.amount_baby + this.amount_fat + this.amount_girl;
			
			if (num <= 0 && ModSettings.randomCaveSpawns && (spawnInCave || sinkholeSpawn))
			{
				RandomizeAmounts();
			}


			base.updateSpawnConditions();
		}
	}
}
