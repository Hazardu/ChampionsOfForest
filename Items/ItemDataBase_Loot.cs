using System;
using System.Linq;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
	public static partial class ItemDataBase
	{
		public static Item GetRandomItem(float Worth)
		{
			int averageLevel = 1;
			int highestLevel = 1;
			if (GameSetup.IsMultiplayer)
			{
				ModReferences.RequestAllPlayerLevels();
				float levelSum = ModdedPlayer.instance.level;
				highestLevel = ModdedPlayer.instance.level;
				foreach (var l in ModReferences.PlayerLevels.Values)
				{
					levelSum += l;
					if (l > highestLevel)
						highestLevel = l;
				}
				averageLevel = Convert.ToInt32(levelSum / (1 + ModReferences.PlayerLevels.Count));
			}
			else
			{
				averageLevel = ModdedPlayer.instance.level;
				highestLevel = averageLevel;
			}
			averageLevel = Mathf.Max(1, averageLevel);
			float w = Worth;
			w *= ModdedPlayer.Stats.magicFind.Value;

			int rarity = GetRarity(w,ModSettings.difficulty);

			int[] itemIdPool = null;
			while (itemIdPool == null)
			{
				itemIdPool = ItemRarityGroups[rarity].Where(i => (AllowItemDrop(i, highestLevel, EnemyProgression.Enemy.All))).ToArray();
				if (itemIdPool.Length == 0)
				{
					rarity--;
					itemIdPool = null;
				}
				if (rarity == -1)
					return null;
			}

			int randomID = Random.Range(0, itemIdPool.Length);
			Item item = new Item(ItemBases[itemIdPool[randomID]], 1, 0);

			item.level = Mathf.Max(item.minLevel, Random.Range(averageLevel - 4, averageLevel + 2));
			if (item.ID == 42 || item.ID == 103 || item.type == BaseItem.ItemType.Material)
				item.level = 1;
			item.RollStats();
			return item;
		}

		public static bool AllowItemDrop(int i, in int level, EnemyProgression.Enemy e)
		{
			if (!ItemBases.ContainsKey(i))
			{
				return true;
			}
			if ((int)ItemBases[i].lootTable != 0)
				return (ItemBases[i].lootTable & e) != 0 && ItemBases[i].minLevel <= level;
			return ItemBases[i].minLevel <= level;
		}

		public static Item GetRandomItem(float Worth, EnemyProgression.Enemy killedEnemyType,ModSettings.Difficulty difficulty)
		{
			int averageLevel = 1;
			int highestLevel = 1;
			if (GameSetup.IsMultiplayer)
			{
				ModReferences.RequestAllPlayerLevels();
				float level = ModdedPlayer.instance.level;
				highestLevel = ModdedPlayer.instance.level;
				foreach (var l in ModReferences.PlayerLevels.Values)
				{
					level += l;
					if (l > highestLevel)
						highestLevel = l;
				}
				averageLevel = Convert.ToInt32(level / (1 + ModReferences.PlayerLevels.Count));
			}
			else
			{
				averageLevel = ModdedPlayer.instance.level;
				highestLevel = averageLevel;
			}
			averageLevel = Mathf.Max(1, averageLevel);
			float w = Worth / (averageLevel);
			w *= ModdedPlayer.Stats.magicFind.Value;

			int rarity = GetRarity(w,difficulty);

			int[] itemIdPool = null;
			while (itemIdPool == null)
			{
				itemIdPool = ItemRarityGroups[rarity].Where(i => (AllowItemDrop(i, highestLevel, EnemyProgression.Enemy.All))).ToArray();
				if (itemIdPool.Length == 0)
				{
					rarity--;
					itemIdPool = null;
				}
				if (rarity == -1)
					return null;
			}

			int randomID = Random.Range(0, itemIdPool.Length);
			Item item = new Item(ItemBases[itemIdPool[randomID]], 1, 0);

			item.level = Mathf.Max(item.level, Random.Range(averageLevel - 2, averageLevel + 2));
			if (item.ID == 42 || item.ID == 103 || item.type == BaseItem.ItemType.Material)
				item.level = 1;
			item.RollStats();
			return item;
		}

		public static int GetRarity(float w, ModSettings.Difficulty difficulty)
		{
			int dif = (int)difficulty;
			int rarity = 0;
			float mf = ModdedPlayer.Stats.magicFind.Value - 1;
			mf /= 2f;
			if ((w > 20 && Random.value < 0.70f + 0.45 * mf + dif * 0.075) || (dif> 5 && w > 2000))
			{
				rarity = 1;

				if (w > 80 && (Random.value < 0.50f + 0.4 * mf + dif * 0.07 || w > 2200 && dif> 6))
				{
					rarity = 2;
					if (w > 180 && (Random.value < 0.50f + 0.35 * mf + 0.05f * dif) || w > 4000 && dif> 7)
					{
						if (dif > 0)
						{
							rarity = 3;
							if (w > 360 && (Random.value < 0.5f + 0.22 * mf + 0.034f * dif) || dif> 8)
							{
								if (dif> 1)
								{
									rarity = 4;
									if (w > 720 && (Random.value < 0.26f + 0.085 * mf +0.02f * dif))
									{
										if (dif> 2)
										{
											rarity = 5;
											if (w > 1440 && (Random.value < 0.18f + 0.033 * mf + (0.003f * dif)))
											{
												if (dif> 3)
												{
													rarity = 6;
													if (w > 5000 && (Random.value < 0.04f + 0.025 * mf))
													{
														if (dif> 4)
														{
															rarity = 7;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}

			}
			return rarity;
		}
	}
}