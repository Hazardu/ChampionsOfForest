using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest
{
	public class ItemStatTemplate
	{
		


		public class StatCompare
		{
			public readonly Func<List<float>, float> CalculateValues;

			public StatCompare(Func<List<float>, float> calculateValues)
			{
				CalculateValues = calculateValues;
			}
		}

		public readonly float levelScaling = 1;
		public readonly float valueCap = 0;
		public readonly int id = 0;
		public readonly string name = "";
		public readonly Color color;
		public readonly float rangeMin = 0;
		public readonly float rangeMax = 0;
		public readonly float multipier = 1;
		public readonly bool isPercent = false;
		public readonly int rounding;
		public readonly Func<string> GetTotalStat;
		public readonly Action<float> OnEquip, OnUnequip;
		private readonly StatCompare comparing;

	}


	public class ItemStat
	{

		public readonly ItemStatTemplate template;


		public int possibleStatsIndex = -1;
		public float amount = 1;


		//public delegate object VariableReturnDelegate();
		//public VariableReturnDelegate GetVariable;
		//public Type variableType;
		public float EvaluateTotalIncrease(List<float> amounts)
		{
			return comparing.CalculateValues(amounts);
		}

		/// <summary>
		/// Creates new itemStat and adds it to the database
		/// </summary>
		/// <param name="id">ID of the item, used to access it from the DB</param>
		/// <param name="Min">Min amount for the stat at level 1</param>
		/// <param name="Max">Max amount for the stat at level 1</param>
		/// <param name="name">Name of the stat, what should be displayed in the item context menu</param>
		/// <param name="rarity">range 0-7</param>
		/// <param name="LvlPower">How much should it increase per level(min max values will be powered to this amount)</param>
		public ItemStat(int id, float Min, float Max, float LvlPower, string name, StatCompare comparingMethod, int rarity,Func<string> getValueFunc, OnEquipDelegate onEquip = null, OnUnequipDelegate onUnequip = null, OnConsumeDelegate onConsume = null)
		{
			comparing = comparingMethod;
			levelScaling = LvlPower;
			this.id = id;
			MaxAmount = Max;
			rangeMin = Min;
			this.name = name;
			Rarity = rarity;
			OnEquip = onEquip;
			OnUnequip = onUnequip;
			OnConsume = onConsume;
			GetTotalStat = getValueFunc;
			ItemDataBase.AddStat(this);
		}

		public ItemStat(ItemStat s, int level = 1, int possibleStatsIdx= -1)
		{
			name = s.name;
			levelScaling = s.levelScaling;
			id = s.id;
			Rarity = s.Rarity;
			rangeMin = s.rangeMin;
			MaxAmount = s.MaxAmount;
			OnEquip = s.OnEquip;
			OnUnequip = s.OnUnequip;
			OnConsume = s.OnConsume;
			rounding = s.rounding;
			isPercent = s.isPercent;
			GetTotalStat = s.GetTotalStat;
			this.valueCap = s.valueCap;
				this.multipier = s.multipier;
			amount = RollValue(level);
			if (valueCap != 0)
			{
				amount = Mathf.Min(valueCap, amount);
			}
			amount *= multipier;
			possibleStatsIndex = possibleStatsIdx;
		}

		public ItemStat()
		{
		}

		public float RollValue(int level = 1)
		{
			float mult = 1;
			if (levelScaling != 0)
			{
				mult = Mathf.Pow(level, levelScaling);
			}
			float f = UnityEngine.Random.Range(rangeMin, MaxAmount) * mult;
			return f;
		}

		public string GetMaxValue(int level,float rarityMult)
		{
			string formatting = (isPercent ? "P" : "N" )+ rounding;
			float mult = 1;
			if (levelScaling != 0)
			{
				mult = Mathf.Pow(level, levelScaling);
			}
			float f =  MaxAmount * mult*rarityMult;
			if (valueCap != 0)
				return (Mathf.Min(f, valueCap)*multipier).ToString(formatting);
			else
				return (f* multipier).ToString(formatting);
		}

		public string GetMinValue(int level, float rarityMult)
		{
			string formatting =( isPercent ? "P" : "N" )+ rounding;

			float mult = 1;
			if (levelScaling != 0)
			{
				mult = Mathf.Pow(level, levelScaling);
			}
			float f = rangeMin * mult * rarityMult;
			if (valueCap != 0)
				return (Mathf.Min(f, valueCap) * multipier).ToString(formatting);
			else
				return (f * multipier).ToString(formatting);
		}
	}
}