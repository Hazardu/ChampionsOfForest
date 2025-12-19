using System;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest.Items
{
	public static partial class ItemDatabase
	{
		static int randomItemPoolLevel = -1; // at which level was the random item pool created
		static RandomItemPoolEntry[] randomItemPoolEntries = new RandomItemPoolEntry[(int)ItemDefinition.Rarity.Max];
		static int[] odds = new int[]
		{
			// odds of upgrading from one rarity to another
			// the odds are 1 in x
			3, // Common
			10, // Magic
			25, // Rare
			200, // Legendary
		};

		static System.Random rng = new System.Random();


		private static int GetLevel(Vector3 pos)
		{
			int level;
			{
				if (GameSetup.IsMultiplayer)
				{
					var states = ModReferences.PlayerStates.All;
					switch (ModSettings.LootLevelRule)
					{
						case ModSettings.LootLevelRules.HighestPlayerLevel:
							level = states.Max(x => x.level);
							break;
						case ModSettings.LootLevelRules.AverageLevel:
							level = (int)states.Average(x => (double)x.level);
							break;
						case ModSettings.LootLevelRules.LowestLevel:
							level = states.Min(x => x.level);
							break;
						case ModSettings.LootLevelRules.ClosestPlayer:
							{
								level = ModdedPlayer.instance.level;
								float dist = (LocalPlayer.Transform.position - pos).sqrMagnitude;
								foreach (var state in states)
								{
									float d = (state.gameObject.transform.position - pos).sqrMagnitude;
									if (d < dist)
									{
										dist = d;
										level = state.level;
									}
								}
							}
							break;
						default:
							level = ModdedPlayer.instance.level;
							break;
					}
				}
				else
				{
					level = ModdedPlayer.instance.level;
				}
			}
			return level;
		}


		class RandomItemPoolEntry
		{
			public List<ItemDefinition> items;
			public RandomItemPoolEntry(int level, int rarity)
			{
				items = new List<ItemDefinition>();
				foreach (var item in ItemRarityGroups[rarity])
				{
					if (item.minLevel <= level)
					{
						items.Add(item);
					}
				}
			}

		};

		static ItemDefinition GetItemFromPool(List<ItemDefinition> pool, int randomWeight, int totalWeight)
		{
			randomWeight = Mathf.Max(randomWeight, totalWeight);
			int i = 0;
			while (randomWeight > totalWeight && i < pool.Count - 1)
			{
				if (randomWeight < pool[i].lootWeight)
					return pool[i];
				else
				{
					randomWeight -= pool[i].lootWeight;
					i++;
				}
			}
			return pool[i];
		}

		static List<ItemDefinition> GetPool(int level, int rarity, EnemyProgression.Enemy enemyType)
		{
			if (randomItemPoolLevel != level)
			{
				randomItemPoolLevel = level;
				for (int i = 0; i < randomItemPoolEntries.Length; i++)
					randomItemPoolEntries[i] = new RandomItemPoolEntry(level, i);

			}
			if (rarity < randomItemPoolEntries.Length)
				return randomItemPoolEntries[rarity].items.Where(item => (item.lootTable & enemyType) > 0).ToList();
			return null;
		}

		// returns a list of random items that meet the criteria of the enemy type and level of players
		public static Item[] GetRandomItems(EnemyProgression.Enemy killedEnemyType, ModSettings.GameDifficulty difficulty, Vector3 pos, int count)
		{
			int level = GetLevel(pos);
			ItemDefinition.Rarity rarity = GetRandomRarity(level, ModdedPlayer.Stats.magicFind_quality.Value, difficulty);
			List<ItemDefinition> pool = GetPool(level, (int)rarity, killedEnemyType);
			if (pool.Count == 0)
			{
				Debug.LogError($"No items for pool level:{level} rarity:{rarity} enemy:{killedEnemyType}");
				return null;
			}

			int totalWeight = pool.Sum(x => x.lootWeight);
			Item[] items = new Item[count];
			for (int i = 0; i < count; i++)
			{
				items[i] = new Item(GetItemFromPool(pool, Random.Range(0, totalWeight), totalWeight), level);
			}
			return items;

		}

		public static ItemDefinition.Rarity GetRandomRarity(int level, float qualitymult, ModSettings.GameDifficulty difficulty)
		{
			qualitymult += (int)difficulty * ModSettings.MagicFindPerDifficultyLevel;

			int rarityNum = 0;
			while (rarityNum < (int)ItemDefinition.Rarity.Max - 1)
			{
				double chance = (odds[rarityNum] / qualitymult);
				double rand = rng.NextDouble() * chance;
				if (rand < 1.0)
					rarityNum++;
				else
					break;
			}
			return (ItemDefinition.Rarity)rarityNum;
		}
	}
}