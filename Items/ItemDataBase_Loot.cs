
using System;
using System.Collections.Generic;
using System.Linq;

using LibNoise.Unity.Operator;

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
				averageLevel = Convert.ToInt32(ModReferences.PlayerLevels.Values.Average());
				highestLevel = ModReferences.PlayerLevels.Values.Max();
			}
			else
			{
				averageLevel = ModdedPlayer.instance.Level;
				highestLevel = averageLevel;
			}
			averageLevel = Mathf.Max(1, averageLevel);
			float w = Worth / (averageLevel);
			w *= MagicFind;

			int rarity = GetRarity(w);

			int[] itemIdPool = null;
			while (itemIdPool == null)
			{
				itemIdPool = ItemRarityGroups[rarity].Where(i => (AllowItemDrop(i, highestLevel,EnemyProgression.Enemy.All))).ToArray();
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

			item.level = Mathf.Max(item.minLevel, Random.Range(averageLevel-4, averageLevel + 2));
			if (item.ID == 42 || item.ID == 103)
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
				averageLevel = Convert.ToInt32(ModReferences.PlayerLevels.Values.Average());
				highestLevel = ModReferences.PlayerLevels.Values.Max();
			}
			else
			{
				averageLevel = ModdedPlayer.instance.Level;
				highestLevel = averageLevel;
			}
			averageLevel = Mathf.Max(1, averageLevel);
			float w = Worth / (averageLevel);
			w *= MagicFind;

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

			if ((w > 20 && Random.value < 0.80f) || ((int)ModSettings.difficulty > 5 && w > 2000) || Random.value < ((int)ModSettings.difficulty * 0.075))
			{
				rarity = 1;

				if (w > 120 && (Random.value < 0.50f || w > 2200 && (int)ModSettings.difficulty > 6))
				{
					if (ModSettings.difficulty > 0)
					{
						rarity = 2;
						if (w > 180 && (Random.value < 0.50f || Random.value < (0.065f * (int)ModSettings.difficulty)) || w > 4000 && (int)ModSettings.difficulty > 7)
						{
							if (ModSettings.difficulty > 0)
							{
								rarity = 3;
								if (w > 230 && (Random.value < 0.5f || Random.value < (0.04f * (int)ModSettings.difficulty)))
								{
									if ((int)ModSettings.difficulty > 1)
									{
										rarity = 4;
										if (w > 300 && (Random.value < 0.45f || Random.value < (0.03f * (int)ModSettings.difficulty)))
										{
											if ((int)ModSettings.difficulty > 2)
											{
												rarity = 5;
												if (w > 400 && (Random.value < 0.25f || Random.value < (0.001f * (int)ModSettings.difficulty)))
												{
													if ((int)ModSettings.difficulty > 3)
													{
														rarity = 6;
														if (w > 500 && (Random.value < 0.07f || Random.value < (0.00009f * (int)ModSettings.difficulty)))
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
			}
			return rarity;
		}
		public static void RequestMagicFind()
		{
			MagicFind = ModdedPlayer.instance.MagicFindMultipier;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(23);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
					answerStream.Close();
				}
			}
		}


	}
}
