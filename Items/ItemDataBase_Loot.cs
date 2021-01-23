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

			int rarity = GetRarity(w);

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

		public static Item GetRandomItem(float Worth, EnemyProgression.Enemy killedEnemyType)
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

			int rarity = GetRarity(w);

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
			if (item.ID == 42 || item.ID == 103)
				item.level = 1;
			item.RollStats();
			return item;
		}

		public static int GetRarity(float w)
		{
			int rarity = 0;
			float mf = ModdedPlayer.Stats.magicFind.Value - 1;
			mf /= 2f;
			if ((w > 20 && Random.value < 0.70f + 0.5 * mf) || ((int)ModSettings.difficulty > 5 && w > 2000) || Random.value < ((int)ModSettings.difficulty * 0.075))
			{
				rarity = 1;

				if (w > 80 && (Random.value < 0.50f + 0.4 * mf || w > 2200 && (int)ModSettings.difficulty > 6))
				{
					rarity = 2;
					if (w > 180 && (Random.value < 0.50f + 0.3 * mf || Random.value < (0.065f * (int)ModSettings.difficulty)) || w > 4000 && (int)ModSettings.difficulty > 7)
					{
						if (ModSettings.difficulty > 0)
						{
							rarity = 3;
							if (w > 230 && (Random.value < 0.5f + 0.2 * mf || Random.value < (0.04f * (int)ModSettings.difficulty) || (int)ModSettings.difficulty > 8))
							{
								if ((int)ModSettings.difficulty > 1)
								{
									rarity = 4;
									if (w > 300 && (Random.value < 0.3f + 0.08 * mf || Random.value < (0.02f * (int)ModSettings.difficulty)))
									{
										if ((int)ModSettings.difficulty > 2)
										{
											rarity = 5;
											if (w > 400 && (Random.value < 0.2f + 0.03 * mf || Random.value < (0.002f * (int)ModSettings.difficulty)))
											{
												if ((int)ModSettings.difficulty > 3)
												{
													rarity = 6;
													if (w > 500 && (Random.value < 0.04f + 0.02 * mf || Random.value < (0.0015f * (int)ModSettings.difficulty)))
													{
														if ((int)ModSettings.difficulty > 4)
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