using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest
{
	public class ItemStatTemplate
	{

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
		public readonly Action<float> OnEquip, OnUnequip;


		public ItemStatTemplate(float levelScaling, 
			float valueCap, 
			int id,
			string name, 
			Color color,
			float rangeMin, 
			float rangeMax,
			Action<float> onEquip,
			Action<float> onUnequip,
			float multipier = 1,
			int rounding = 0, 
			bool isPercent = false)
		{
			this.levelScaling = levelScaling;
			this.valueCap = valueCap;
			this.id = id;
			this.name = name;
			this.color = color;
			this.rangeMin = rangeMin;
			this.rangeMax = rangeMax;
			this.multipier = multipier;
			this.isPercent = isPercent;
			this.rounding = rounding;
			OnEquip = onEquip;
			OnUnequip = onUnequip;
		}


	}


	public class ItemStat
	{

		public readonly ItemStatTemplate template;
		public int ID => template.id;

		public int possibleStatsIndex = -1;
		public float amount = 1;
		public float multipier = 1;
		public float Multipier => multipier * template.multipier;


		/// <summary>
		/// Creates new itemStat and adds it to the database
		/// </summary>
		/// <param name="id">ID of the item, used to access it from the DB</param>
		/// <param name="Min">Min amount for the stat at level 1</param>
		/// <param name="Max">Max amount for the stat at level 1</param>
		/// <param name="name">Name of the stat, what should be displayed in the item context menu</param>
		/// <param name="rarity">range 0-7</param>
		/// <param name="LvlPower">How much should it increase per level(min max values will be powered to this amount)</param>
		public ItemStat(int id, float Min, float Max, float LvlPower, string name, ItemStatTemplate.StatCompare comparingMethod, int rarity,Func<string> getValueFunc, Action onEquip = null, Action onUnequip = null)
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
			GetTotalStat = getValueFunc;
			ItemDataBase.AddStat(this);
		}

		public ItemStat(ItemStat other, int level = 1, int possibleStatsIdx= -1)
		{
			this.template = other.template;
				this.multipier = other.multipier;
			amount = RollValue(level);
			possibleStatsIndex = possibleStatsIdx;
		}

		public ItemStat()
		{
		}

		public float RollValue(int level = 1)
		{
			float mult = 1;
			if (template.levelScaling != 0)
			{
				mult = Mathf.Pow(level, template.levelScaling);
			}
			float f = UnityEngine.Random.Range(template.rangeMin, template.rangeMax) * mult;
			if (template.valueCap != 0)
			{
				amount = Mathf.Min(template.valueCap, amount);
			}
			amount *= Multipier;
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