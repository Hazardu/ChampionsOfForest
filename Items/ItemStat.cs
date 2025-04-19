using System;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Player;

using UnityEngine;

using static ChampionsOfForest.Items.ItemDatabase;
using static ChampionsOfForest.Items.ItemStatBuilder;

namespace ChampionsOfForest.Items
{

	public class ItemStatBuilder : ItemStat
	{
		private float CompareAdd(List<float> x) => x.Sum();
		private float CompareMult(List<float> x)
		{
			float f = 1;
			for (int j = 0; j < x.Count; j++)
				f *= x[j];
			return f - 1;
		}
		private float CompareMultPlus1(List<float> x)
		{
			float f = 1;
			for (int j = 0; j < x.Count; j++)
				f *= 1 + x[j];
			return f;
		}
		private float Compare1MinusMult(List<float> x)
		{
			float f = 1;
			for (int k = 0; k < x.Count; k++)
				f *= 1 - x[k];
			return 1 - f;
		}

		public ItemStatBuilder(Stat id, string name, float min, float max)
		{
			this.id = (int)id;
			this.name = name;
			this.rangeMin = min;
			this.rangeMax = max;
			ItemDatabase.AddStat(this);
		}
		public ItemStatBuilder Additive(IAdditiveStat<bool> stat)
		{
			this.comparingFunc = CompareAdd;
			this.OnEquip = f => stat.Add(true);
			this.OnUnequip = f => stat.Sub(false);
			return this;
		}
		public ItemStatBuilder Additive(IAdditiveStat<float> stat)
		{
			this.comparingFunc = CompareAdd;
			this.OnEquip = f => stat.Add(f);
			this.OnUnequip = f => stat.Sub(f);
			return this;
		}
		public ItemStatBuilder Additive(IAdditiveStat<int> stat)
		{
			this.comparingFunc = CompareAdd;
			this.OnEquip = f => stat.Add((int)f);
			this.OnUnequip = f => stat.Sub((int)f);
			return this;
		}

		public ItemStatBuilder Multiplicative(IMultiplicativeStat<float> stat)
		{
			this.comparingFunc = CompareMult;
			this.OnEquip = f => stat.Multiply(f);
			this.OnEquip = f => stat.Divide(f);
			return this;
		}

		public ItemStatBuilder MultiplyPlusOne(IMultiplicativeStat<float> stat)
		{
			this.comparingFunc = CompareMultPlus1;
			this.OnEquip = f => stat.Multiply(f+1f);
			this.OnEquip = f => stat.Divide(f+1f);
			return this;
		}

		public ItemStatBuilder OneMinusMultiplier(IMultiplicativeStat<float> stat)
		{
			this.comparingFunc = Compare1MinusMult;
			this.OnEquip = f => stat.Multiply(1f-f);
			this.OnEquip = f => stat.Divide(1f-f);
			return this;
		}

		// this stat will scale linearly with item level
		public ItemStatBuilder LinearLevelScaling()
		{
			this.levelExponent = 1;
			return this;
		}

		// this stat will scale linearly with item level
		public ItemStatBuilder NoLevelScaling()
		{
			this.levelExponent = 0;
			return this;
		}

		public ItemStatBuilder LevelScaling(float scaling)
		{
			this.levelExponent = scaling;
			return this;
		}

		public ItemStatBuilder RarityScaling(float scaling)
		{
			this.rarityCoefficient = scaling;
			return this;
		}

		public ItemStatBuilder NoRarityScaling()
		{
			this.rarityCoefficient = 0;
			return this;
		}

		public ItemStatBuilder AffectsStat<T>(NumericPlayerStatBase<T> stat) where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
		{
			if (stat is null)
				throw new ArgumentNullException(nameof(stat));
			this.GetTotalStat = stat.GetFormattedAmount;
			return this;
		}

		public ItemStatBuilder RoundTo(int digitsAfterZero)
		{
			this.roundingCount = digitsAfterZero;
			return this;
		}

		public ItemStatBuilder PercentFormatting()
		{
			this.displayAsPercent = true;
			return this;
		}

		public ItemStatBuilder Cap(float maximumPossibleValue)
		{
			this.valueCap = maximumPossibleValue;
			return this;
		}

		public ItemStatBuilder AffectsCarryCapacity(params MoreCraftingReceipes.VanillaItemIDs[] itemIdsToExpandCarryCapacity)
		{
			this.comparingFunc = CompareAdd;
			this.OnEquip = f =>
			{
				for (int i = 0; i < itemIdsToExpandCarryCapacity.Length; i++)
				{
					ModdedPlayer.instance.AddExtraItemCapacity((int)itemIdsToExpandCarryCapacity[i], Mathf.RoundToInt(f));
				}
			};
			this.OnUnequip = f =>
			{
				for (int i = 0; i < itemIdsToExpandCarryCapacity.Length; i++)
				{
					ModdedPlayer.instance.AddExtraItemCapacity((int)itemIdsToExpandCarryCapacity[i], -Mathf.RoundToInt(f));
				}
			};
			this.GetTotalStat = () => string.Join("," , itemIdsToExpandCarryCapacity.Select(id => LocalPlayer.Inventory.GetMaxAmountOf((int)id).ToString()));
			return this;
		}



	}
	public class ItemStat
	{
		public class StatCompare
		{
			public readonly Func<List<float>, float> CalculateValues;

			public StatCompare(Func<List<float>, float> calculateValues)
			{
				CalculateValues = calculateValues;
			}
		}

		public float levelExponent = 1;
		public float valueCap = 0;
		public int id = 0;
		public string name = "";
		public float rangeMin = 0;
		public float rangeMax = 0;
		public float amount = 1;
		public float multipier = 1;
		public float rarityCoefficient = 1;

		// display
		public bool displayAsPercent = false;
		public int roundingCount;
		public int possibleStatsIndex = -1;


		public Func<List<float>, float> comparingFunc;
		public Func<string> GetTotalStat; 

		public delegate void StatActionDelegate(float f);

		public StatActionDelegate OnEquip;
		public StatActionDelegate OnUnequip;


		public float EvaluateTotalIncrease(List<float> amounts)
		{
			return comparingFunc(amounts);
		}


		public ItemStat()
		{
			
		}

		// Returns a copy of an item stat
		public ItemStat(ItemStat original, int level, int possibleStatsIdx, float rarityMult)
		{
			name = original.name;
			levelExponent = original.levelExponent;
			id = original.id;
			rangeMin = original.rangeMin;
			rangeMax = original.rangeMax;
			OnEquip = original.OnEquip;
			OnUnequip = original.OnUnequip;
			roundingCount = original.roundingCount;
			displayAsPercent = original.displayAsPercent;
			GetTotalStat = original.GetTotalStat;
			valueCap = original.valueCap;
			multipier = original.multipier;
			rarityCoefficient = original.rarityCoefficient;
			comparingFunc = original.comparingFunc;
			amount = RollValue(level, rarityMult);
			possibleStatsIndex = possibleStatsIdx;
		}


		private float GetMultiplier(int level, float rarityMult)
		{
			float levelMult = 1;
			if (levelExponent != 0)
			{
				levelMult = Mathf.Pow(level, levelExponent);
			}
			rarityMult *= rarityCoefficient;
			return multipier * levelMult * rarityMult;
		}

		public float RollValue(int level, float rarityMult)
		{
			float randomValue = UnityEngine.Random.Range(rangeMin, rangeMax);
			float mult = GetMultiplier(level, rarityMult);
			float val = randomValue * mult;
			if (valueCap != 0 && val > valueCap)
				val = valueCap;
			return val;
		}

		public string GetMaxValue(int level,float rarityMult)
		{
			string formatting = (displayAsPercent ? "P" : "N" )+ roundingCount;

			float mult = GetMultiplier(level, rarityMult);
			float val = rangeMax * mult;
			if (valueCap != 0 && val > valueCap)
				return (valueCap).ToString(formatting) + " (capped)";
			else 
				return (val).ToString(formatting);
		}

		public string GetMinValue(int level, float rarityMult)
		{
			string formatting = (displayAsPercent ? "P" : "N") + roundingCount;

			float mult = GetMultiplier(level, rarityMult);
			float val = rangeMin * mult;
			if (valueCap != 0 && val > valueCap)
				return (valueCap).ToString(formatting) + " (capped)";
			else
				return (val).ToString(formatting);
		}
	}
}