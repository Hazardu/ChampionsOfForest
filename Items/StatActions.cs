using ChampionsOfForest.Player;
using UnityEngine;

namespace ChampionsOfForest.Items
{
	internal class StatActions
	{
		public static void AddVitality(float f)
		{
			ModdedPlayer.Stats.vitality.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveVitality(float f)
		{
			ModdedPlayer.Stats.vitality.Add(-Mathf.RoundToInt(f));
		}

		public static void AddStrength(float f)
		{
			ModdedPlayer.Stats.strength.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveStrength(float f)
		{
			ModdedPlayer.Stats.strength.Add(-Mathf.RoundToInt(f));
		}

		public static void AddAgility(float f)
		{
			ModdedPlayer.Stats.agility.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveAgility(float f)
		{
			ModdedPlayer.Stats.agility.Substract(Mathf.RoundToInt(f));
		}

		public static void AddIntelligence(float f)
		{
			ModdedPlayer.Stats.intelligence.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveIntelligence(float f)
		{
			ModdedPlayer.Stats.intelligence.Substract(Mathf.RoundToInt(f));
		}

		public static void AddHealth(float f)
		{
			ModdedPlayer.Stats.maxLife.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveHealth(float f)
		{
			ModdedPlayer.Stats.maxLife.Substract(Mathf.RoundToInt(f));
		}

		public static void AddEnergy(float f)
		{
			ModdedPlayer.Stats.maxEnergy.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveEnergy(float f)
		{
			ModdedPlayer.Stats.maxEnergy.Substract(Mathf.RoundToInt(f));
		}

		public static void AddHPRegen(float f)
		{
			ModdedPlayer.Stats.lifeRegenBase.Add(f);
		}

		public static void RemoveHPRegen(float f)
		{
			ModdedPlayer.Stats.lifeRegenBase.Sub(f);
		}

		public static void AddStaminaRegen(float f)
		{
			ModdedPlayer.Stats.energyRegenMult.Add(f);
		}

		public static void RemoveStaminaRegen(float f)
		{
			ModdedPlayer.Stats.energyRegenMult.Sub(f);
		}

		public static void AddEnergyRegen(float f)
		{
			ModdedPlayer.Stats.energyRecoveryBase.Add(f);
		}

		public static void RemoveEnergyRegen(float f)
		{
			ModdedPlayer.Stats.energyRecoveryBase.Sub(f);
		}

		public static void AddStaminaRegenPercent(float f)
		{
			ModdedPlayer.Stats.staminaRegenBase.Add(f);
		}

		public static void RemoveStaminaRegenPercent(float f)
		{
			ModdedPlayer.Stats.staminaRegenBase.Sub(f);
		}

		public static void AddHealthRegenPercent(float f)
		{
			ModdedPlayer.Stats.lifeRegenMult.Add(f);
		}

		public static void RemoveHealthRegenPercent(float f)
		{
			ModdedPlayer.Stats.lifeRegenMult.Sub(f);
		}

		public static void AddDamageReduction(float f)
		{
			ModdedPlayer.Stats.allDamageTaken.Multiply(1 - f);
		}

		public static void RemoveDamageReduction(float f)
		{
			ModdedPlayer.Stats.allDamageTaken.Divide(1 - f);
		}

		public static void AddCritChance(float f)
		{
			ModdedPlayer.Stats.critChance.Add(f);
		}

		public static void RemoveCritChance(float f)
		{
			ModdedPlayer.Stats.critChance.Sub(f);
		}

		public static void AddCritDamage(float f)
		{
			ModdedPlayer.Stats.critDamage.Add(f);
		}

		public static void RemoveCritDamage(float f)
		{
			ModdedPlayer.Stats.critDamage.Sub(f);
		}

		public static void AddLifeOnHit(float f)
		{
			ModdedPlayer.Stats.lifeOnHit.Add(f);
		}

		public static void RemoveLifeOnHit(float f)
		{
			ModdedPlayer.Stats.lifeOnHit.Sub(f);
		}

		public static void AddDodgeChance(float f)
		{
			ModdedPlayer.Stats.getHitChance.valueMultiplicative *= 1 - f;
		}

		public static void RemoveDodgeChance(float f)
		{
			ModdedPlayer.Stats.getHitChance.valueMultiplicative /= 1 - f;
		}

		public static void AddArmor(float f)
		{
			ModdedPlayer.Stats.armor.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveArmor(float f)
		{
			ModdedPlayer.Stats.armor.Substract(Mathf.RoundToInt(f));
		}

		public static void AddEliteDamageReduction(float f)
		{
			ModdedPlayer.Stats.damageFromElites.valueMultiplicative *= 1 - f;
		}

		public static void RemoveEliteDamageReduction(float f)
		{
			ModdedPlayer.Stats.damageFromElites.valueMultiplicative /= 1 - f;
		}

		public static void AddAttackSpeed(float f)
		{
			ModdedPlayer.Stats.attackSpeed.Add(f);
		}

		public static void RemoveAttackSpeed(float f)
		{
			ModdedPlayer.Stats.attackSpeed.Sub(f);
		}

		public static void AddExpFactor(float f)
		{
			ModdedPlayer.Stats.expGain.Add(f);
		}

		public static void RemoveExpFactor(float f)
		{
			ModdedPlayer.Stats.expGain.Sub(f);
		}

		public static void AddMaxMassacreTime(float f)
		{
			ModdedPlayer.Stats.maxMassacreTime.Add(f);
		}

		public static void RemoveMaxMassacreTime(float f)
		{
			ModdedPlayer.Stats.maxMassacreTime.Sub(f);
		}

		public static void AddSpellDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.spellDamageMult.Add(f);
		}

		public static void RemoveSpellDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.spellDamageMult.Sub(f);
		}

		public static void AddMeleeDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.meleeIncreasedDmg.Add(f);
		}

		public static void RemoveMeleeDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.meleeIncreasedDmg.Sub(f);
		}

		public static void AddRangedDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.rangedIncreasedDmg.Add(f);
		}

		public static void RemoveRangedDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.rangedIncreasedDmg.Sub(f);
		}

		public static void AddspellFlatDmg(float f)
		{
			ModdedPlayer.Stats.baseSpellDamage.Add(f);
		}

		public static void RemovespellFlatDmg(float f)
		{
			ModdedPlayer.Stats.baseSpellDamage.Sub(f);
		}

		public static void AddMeleeDamageBonus(float f)
		{
			ModdedPlayer.Stats.baseMeleeDamage.Add(f);
		}

		public static void RemoveMeleeDamageBonus(float f)
		{
			ModdedPlayer.Stats.baseMeleeDamage.Sub(f);
		}

		public static void AddRangedDamageBonus(float f)
		{
			ModdedPlayer.Stats.baseRangedDamage.Add(f);
		}

		public static void RemoveRangedDamageBonus(float f)
		{
			ModdedPlayer.Stats.baseRangedDamage.Sub(f);
		}

		public static void AddmaxEnergyFromAgi(float f)
		{
			ModdedPlayer.Stats.maxEnergyFromAgi.Add(f);
		}

		public static void RemovemaxEnergyFromAgi(float f)
		{
			ModdedPlayer.Stats.maxEnergyFromAgi.Sub(f);
		}

		public static void AddmaxHealthFromVit(float f)
		{
			ModdedPlayer.Stats.maxHealthFromVit.Add(f);
		}

		public static void RemovemaxHealthFromVit(float f)
		{
			ModdedPlayer.Stats.maxHealthFromVit.Sub(f);
		}

		public static void AddspellDmgFromInt(float f)
		{
			ModdedPlayer.Stats.spellDmgFromInt.Add(f);
		}

		public static void RemovespellDmgFromInt(float f)
		{
			ModdedPlayer.Stats.spellDmgFromInt.Sub(f);
		}

		public static void AddmeleeDmgFromStr(float f)
		{
			ModdedPlayer.Stats.meleeDmgFromStr.Add(f);
		}

		public static void RemovemeleeDmgFromStr(float f)
		{
			ModdedPlayer.Stats.meleeDmgFromStr.Sub(f);
		}

		public static void AddHealingMultipier(float f)
		{
			ModdedPlayer.Stats.allRecoveryMult.Add(f);
		}

		public static void RemoveHealingMultipier(float f)
		{
			ModdedPlayer.Stats.allRecoveryMult.Sub(f);
		}

		public static void AddMoveSpeed(float f)
		{
			ModdedPlayer.Stats.mOVEMENT_SPEED.Add(f);
		}

		public static void RemoveMoveSpeed(float f)
		{
			ModdedPlayer.Stats.mOVEMENT_SPEED.Sub(f);
		}

		public static void AddJump(float f)
		{
			ModdedPlayer.Stats.jumpPower.Add(f);
		}

		public static void RemoveJump(float f)
		{
			ModdedPlayer.Stats.jumpPower.Sub(f);
		}

		//   public static void Add( float f)
		//{
		//    ModdedPlayer.Stats..Add( f;
		//}
		//public static void Remove( float f)
		//{
		//    ModdedPlayer.Stats. .Substract( f;
		//}

	

		public static void PERMANENT_expIncrease(float f)
		{
			ModdedPlayer.instance.AddFinalExperience((long)f);
		}

		public static void AddMagicFind(float f)
		{
			ModdedPlayer.Stats.magicFind_quantity.Add(f);
		}

		public static void RemoveMagicFind(float f)
		{
			ModdedPlayer.Stats.magicFind_quantity.Sub(f);

		}

		public static void AddAllStats(float f)
		{
			ModdedPlayer.Stats.strength.Add(Mathf.RoundToInt(f));
			ModdedPlayer.Stats.vitality.Add(Mathf.RoundToInt(f));
			ModdedPlayer.Stats.agility.Add(Mathf.RoundToInt(f));
			ModdedPlayer.Stats.intelligence.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveAllStats(float f)
		{
			ModdedPlayer.Stats.strength.Add(-Mathf.RoundToInt(f));
			ModdedPlayer.Stats.vitality.Add(-Mathf.RoundToInt(f));
			ModdedPlayer.Stats.agility.Add(-Mathf.RoundToInt(f));
			ModdedPlayer.Stats.intelligence.Add(-Mathf.RoundToInt(f));
		}

		public static int GetMaxSocketAmountOnItem(in ItemDefinition.ItemType type)
		{
			switch (type)
			{
				case ItemDefinition.ItemType.Other:
				case ItemDefinition.ItemType.Material:
					return 0;

				case ItemDefinition.ItemType.ShoulderArmor:
				case ItemDefinition.ItemType.Weapon:
				case ItemDefinition.ItemType.Helmet:
				case ItemDefinition.ItemType.Glove:
				case ItemDefinition.ItemType.Boot:
				case ItemDefinition.ItemType.Amulet:
				case ItemDefinition.ItemType.Ring:
					return 1;

				case ItemDefinition.ItemType.Bracer:
				case ItemDefinition.ItemType.Pants:
				case ItemDefinition.ItemType.SpellScroll:
				case ItemDefinition.ItemType.Shield:
				case ItemDefinition.ItemType.Quiver:
					return 2;

				case ItemDefinition.ItemType.ChestArmor:
					return 3;

				default:
					return 0;
			}
		}
		public static ItemStat GetSocketedStat(in int rarity, ItemDefinition.ItemType type, int subtypeOffset)
		{
			float value = 1;
			int statid = 0;
			switch (type)
			{
				case ItemDefinition.ItemType.Shield:
				case ItemDefinition.ItemType.Quiver:
				case ItemDefinition.ItemType.Pants:
				case ItemDefinition.ItemType.ChestArmor:
				case ItemDefinition.ItemType.ShoulderArmor:
				case ItemDefinition.ItemType.Glove:
				case ItemDefinition.ItemType.Bracer:
				case ItemDefinition.ItemType.SpellScroll:
					value = GetSocketStatAmount_Other(in rarity);
					statid = 1;
					break;

				case ItemDefinition.ItemType.Weapon:
					value = GetSocketStatAmount_Weapon(in rarity) - 1;
					statid = 3;
					break;

				case ItemDefinition.ItemType.Helmet:
					value = GetSocketStatAmount_Helmet(in rarity) - 1;
					statid = 0;
					break;

				case ItemDefinition.ItemType.Boot:
					value = GetSocketStatAmount_Boots(in rarity) - 1;
					statid = 2;
					break;

				case ItemDefinition.ItemType.Ring:
				case ItemDefinition.ItemType.Amulet:
					value = GetSocketStatAmount_Amulet(in rarity);
					statid = 4;
					break;
			}
			statid = 5 * (subtypeOffset - 1) + 3001 + statid;
			ItemStat stat = new ItemStat(ItemDatabase.StatByID(statid));
			stat.amount = value * stat.multipier;
			return stat;
		}
		public static float GetSocketedStatAmount(in int rarity, ItemDefinition.ItemType type, int subtypeOffset)
		{
			float value = 1;
			int statid = 0;
			switch (type)
			{
				case ItemDefinition.ItemType.Shield:
				case ItemDefinition.ItemType.Quiver:
				case ItemDefinition.ItemType.Pants:
				case ItemDefinition.ItemType.ChestArmor:
				case ItemDefinition.ItemType.ShoulderArmor:
				case ItemDefinition.ItemType.Glove:
				case ItemDefinition.ItemType.Bracer:
				case ItemDefinition.ItemType.SpellScroll:
					value = GetSocketStatAmount_Other(in rarity);
					statid = 1;
					break;

				case ItemDefinition.ItemType.Weapon:
					value = GetSocketStatAmount_Weapon(in rarity) - 1;
					statid = 3;
					break;

				case ItemDefinition.ItemType.Helmet:
					value = GetSocketStatAmount_Helmet(in rarity) - 1;
					statid = 0;
					break;

				case ItemDefinition.ItemType.Boot:
					value = GetSocketStatAmount_Boots(in rarity) - 1;
					statid = 2;
					break;

				case ItemDefinition.ItemType.Ring:
				case ItemDefinition.ItemType.Amulet:
					value = GetSocketStatAmount_Amulet(in rarity);
					statid = 4;
					break;
			}
			int stat = 5 * (subtypeOffset - 1) + 3001 + statid;
			return value * ItemDatabase.StatByID(stat).multipier;
		}
		public static float GetSocketStatAmount_Amulet(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 50f;

				case 4:
					return 100f ;

				case 5:
					return 200f ;

				case 6:
					return 500f ;

				case 7:
					return 3000f ;

				default:
					return 20f ;
			}
		}

		public static float GetSocketStatAmount_Weapon(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 1.1f;

				case 4:
					return 1.2f;

				case 5:
					return 1.35f;

				case 6:
					return 1.70f;

				case 7:
					return 3.5f;

				default:
					return 1.05f;
			}
		}

		public static float GetSocketStatAmount_Boots(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 1.065f;

				case 4:
					return 1.125f;

				case 5:
					return 1.18f;

				case 6:
					return 1.33f;

				case 7:
					return 1.50f;

				default:
					return 1.04f;
			}
		}

		public static float GetSocketStatAmount_Helmet(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 1.075f;

				case 4:
					return 1.125f;

				case 5:
					return 1.2f;

				case 6:
					return 1.33f;

				case 7:
					return 1.50f;

				default:
					return 1.04f;
			}
		}

		public static float GetSocketStatAmount_Other(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 25f;

				case 4:
					return 60f;

				case 5:
					return 200f;

				case 6:
					return 500f;

				case 7:
					return 1500f;

				default:
					return 10f;
			}
		}
	}
}