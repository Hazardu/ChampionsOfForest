using System;
using System.Collections.Generic;

using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest
{
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

		public float LevelPow = 1;
		public float ValueCap = 0;
		public int StatID = 0;
		public string Name = "";
		public int Rarity = 0;
		public float MinAmount = 0;
		public float MaxAmount = 0;
		public float Amount = 1;
		public float Multipier = 1;
		public bool DisplayAsPercent = false;
		public int RoundingCount;
		public int possibleStatsIndex = -1;
		private StatCompare comparing;
		public Func<string> GetTotalStat; 
		public delegate void OnEquipDelegate(float f);

		public OnEquipDelegate OnEquip;

		public delegate void OnUnequipDelegate(float f);

		public OnUnequipDelegate OnUnequip;

		public delegate void OnConsumeDelegate(float f);

		public OnConsumeDelegate OnConsume;

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
			LevelPow = LvlPower;
			StatID = id;
			MaxAmount = Max;
			MinAmount = Min;
			Name = name;
			Rarity = rarity;
			OnEquip = onEquip;
			OnUnequip = onUnequip;
			OnConsume = onConsume;
			GetTotalStat = getValueFunc;
			ItemDataBase.AddStat(this);
		}

		public ItemStat(ItemStat s, int level = 1, int possibleStatsIdx= -1)
		{
			Name = s.Name;
			LevelPow = s.LevelPow;
			StatID = s.StatID;
			Rarity = s.Rarity;
			MinAmount = s.MinAmount;
			MaxAmount = s.MaxAmount;
			OnEquip = s.OnEquip;
			OnUnequip = s.OnUnequip;
			OnConsume = s.OnConsume;
			RoundingCount = s.RoundingCount;
			DisplayAsPercent = s.DisplayAsPercent;
			GetTotalStat = s.GetTotalStat;
			this.ValueCap = s.ValueCap;
				this.Multipier = s.Multipier;
			Amount = RollValue(level);
			if (ValueCap != 0)
			{
				Amount = Mathf.Min(ValueCap, Amount);
			}
			Amount *= Multipier;
			possibleStatsIndex = possibleStatsIdx;
		}

		public ItemStat()
		{
		}

		public float RollValue(int level = 1)
		{
			float mult = 1;
			if (LevelPow != 0)
			{
				mult = Mathf.Pow(level, LevelPow);
			}
			float f = UnityEngine.Random.Range(MinAmount, MaxAmount) * mult;
			return f;
		}

		public string GetMaxValue(int level,float rarityMult)
		{
			string formatting = (DisplayAsPercent ? "P" : "N" )+ RoundingCount;
			float mult = 1;
			if (LevelPow != 0)
			{
				mult = Mathf.Pow(level, LevelPow);
			}
			float f =  MaxAmount * mult*rarityMult;
			if (ValueCap != 0)
				return (Mathf.Min(f, ValueCap)*Multipier).ToString(formatting);
			else
				return (f* Multipier).ToString(formatting);
		}

		public string GetMinValue(int level, float rarityMult)
		{
			string formatting =( DisplayAsPercent ? "P" : "N" )+ RoundingCount;

			float mult = 1;
			if (LevelPow != 0)
			{
				mult = Mathf.Pow(level, LevelPow);
			}
			float f = MinAmount * mult * rarityMult;
			if (ValueCap != 0)
				return (Mathf.Min(f, ValueCap) * Multipier).ToString(formatting);
			else
				return (f * Multipier).ToString(formatting);
		}
	}
}