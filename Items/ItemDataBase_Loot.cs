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
		private static int GetLevel(Vector3 pos)
		{
			int level;
			{
				if (GameSetup.IsMultiplayer)
				{
					switch (ModSettings.lootLevelPolicy)
					{
						case ModSettings.LootLevelPolicy.HighestPlayerLevel:
							{
								ModReferences.RequestAllPlayerLevels();
								int highestLevel = ModdedPlayer.instance.level;
								foreach (var l in ModReferences.PlayerLevels.Values)
								{
									if (l > highestLevel)
										highestLevel = l;
								}
								level = highestLevel;
							}

							break;
						case ModSettings.LootLevelPolicy.AverageLevel:
							{
								ModReferences.RequestAllPlayerLevels();
								float levelSum = ModdedPlayer.instance.level;
								foreach (var l in ModReferences.PlayerLevels.Values)
								{
									levelSum += l;
								}
								level = Convert.ToInt32(levelSum / (1 + ModReferences.PlayerLevels.Count));
							}

							break;
						case ModSettings.LootLevelPolicy.LowestLevel:
							{
								ModReferences.RequestAllPlayerLevels();
								int lowestLevel
									= ModdedPlayer.instance.level;
								foreach (var l in ModReferences.PlayerLevels.Values)
								{
									if (l < lowestLevel)
										lowestLevel = l;
								}
								level = lowestLevel;
							}
							break;
						case ModSettings.LootLevelPolicy.ClosestPlayer:
							{
								level = ModdedPlayer.instance.level;
								float dist = Vector3.Distance(LocalPlayer.Transform.position, pos);
								IPlayerState state = null;
								foreach (var playerstate in ModReferences.PlayerStates)
								{
									float d = Vector3.Distance(playerstate.Transform.Position, pos);
									if (d < dist)
									{
										dist = d;
										state = playerstate;
									}
								}
								if(state != null)
									level = ModReferences.PlayerLevels[state.name];
							}
							break;
						case ModSettings.LootLevelPolicy.HostLevel:
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
		public static Item GetRandomItem(float Worth, Vector3 pos)
		{
			int level = GetLevel(pos);
			float w = Worth;
			w *= ModdedPlayer.Stats.magicFind.Value;

			int rarity = GetRarity(w, ModSettings.difficulty);

			int[] itemIdPool = null;
			while (itemIdPool == null)
			{
				itemIdPool = itemsByRarities[rarity].Where(i => (AllowItemDrop(i, level, EnemyProgression.Enemy.All))).ToArray();
				if (itemIdPool.Length == 0)
				{
					rarity--;
					itemIdPool = null;
				}
				if (rarity == -1)
					return null;
			}

			int randomID = Random.Range(0, itemIdPool.Length);
			Item item = new Item(itemTemplatesById[itemIdPool[randomID]]);

			item.level = level;
			if (item.ID == 42 || item.ID == 103 || item.type == ItemTemplate.ItemType.Material)
				item.level = 1;
			item.RollStats();
			return item;
		}

		public static bool AllowItemDrop(int i, in int level, EnemyProgression.Enemy e)
		{
			if (!itemTemplatesById.ContainsKey(i))
			{
				return true;
			}
			if ((int)itemTemplatesById[i].lootTable != 0)
				return (itemTemplatesById[i].lootTable & e) != 0 && itemTemplatesById[i].minLevel <= level;
			return itemTemplatesById[i].minLevel <= level;
		}

		public static Item GetRandomItem(float Worth, EnemyProgression.Enemy killedEnemyType, ModSettings.Difficulty difficulty, Vector3 pos)
		{
			int level = GetLevel(pos);
			float w = Worth / (level);
			w *= ModdedPlayer.Stats.magicFind.Value;

			int rarity = GetRarity(w, difficulty);

			int[] itemIdPool = null;
			while (itemIdPool == null)
			{
				itemIdPool = itemsByRarities[rarity].Where(i => (AllowItemDrop(i, level, EnemyProgression.Enemy.All))).ToArray();
				if (itemIdPool.Length == 0)
				{
					rarity--;
					itemIdPool = null;
				}
				if (rarity == -1)
					return null;
			}

			int randomID = Random.Range(0, itemIdPool.Length);
			Item item = new Item(itemTemplatesById[itemIdPool[randomID]]);

			item.level = level;
			if (item.ID == 42 || item.ID == 103 || item.type == ItemTemplate.ItemType.Material)
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
			if ((w > 20 && Random.value < 0.70f + 0.45 * mf + dif * 0.075) || (dif > 5 && w > 2000))
			{
				rarity = 1;

				if (w > 80 && (Random.value < 0.50f + 0.4 * mf + dif * 0.07 || w > 2200 && dif > 6))
				{
					rarity = 2;
					if (w > 180 && (Random.value < 0.50f + 0.35 * mf + 0.05f * dif) || w > 5000 && dif > 7 && Random.value < 0.90f)
					{
						if (dif > 0 || Random.value < 0.05f)
						{
							rarity = 3;
							if (w > 360 && (Random.value < 0.5f + 0.22 * mf + 0.034f * dif) || dif > 8 && Random.value < 0.70f)
							{
								if (dif > 1 || Random.value < 0.02f)
								{
									rarity = 4;
									if (w > 720 && (Random.value < 0.26f + 0.085 * mf + 0.02f * dif))
									{
										if (dif > 2 || Random.value < 0.01f)
										{
											rarity = 5;
											if (w > 1440 && (Random.value < 0.18f + 0.033 * mf + (0.003f * dif)))
											{
												if (dif > 3 || Random.value < 0.05f)
												{
													rarity = 6;
													if (w > 5000 && (Random.value < 0.04f + 0.025 * mf))
													{
														if (dif > 4 || Random.value < 0.001f)
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