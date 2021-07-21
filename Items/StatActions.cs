using ChampionsOfForest.Player;

using TheForest.Utils;

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
			ModdedPlayer.Stats.maxHealth.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveHealth(float f)
		{
			ModdedPlayer.Stats.maxHealth.Substract(Mathf.RoundToInt(f));
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
			ModdedPlayer.Stats.healthRecoveryPerSecond.Add(f);
		}

		public static void RemoveHPRegen(float f)
		{
			ModdedPlayer.Stats.healthRecoveryPerSecond.Substract(f);
		}

		public static void AddStaminaRegen(float f)
		{
			ModdedPlayer.Stats.staminaRecoveryperSecond.Add(f);
		}

		public static void RemoveStaminaRegen(float f)
		{
			ModdedPlayer.Stats.staminaRecoveryperSecond.Substract(f);
		}

		public static void AddEnergyRegen(float f)
		{
			ModdedPlayer.Stats.energyRecoveryperSecond.Add(f);
		}

		public static void RemoveEnergyRegen(float f)
		{
			ModdedPlayer.Stats.energyRecoveryperSecond.Substract(f);
		}

		public static void AddStaminaRegenPercent(float f)
		{
			ModdedPlayer.Stats.staminaPerSecRate.Add(f);
		}

		public static void RemoveStaminaRegenPercent(float f)
		{
			ModdedPlayer.Stats.staminaPerSecRate.Substract(f);
		}

		public static void AddHealthRegenPercent(float f)
		{
			ModdedPlayer.Stats.healthPerSecRate.Add(f);
		}

		public static void RemoveHealthRegenPercent(float f)
		{
			ModdedPlayer.Stats.healthPerSecRate.Substract(f);
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
			ModdedPlayer.Stats.critChance.Substract(f);
		}

		public static void AddCritDamage(float f)
		{
			ModdedPlayer.Stats.critDamage.Add(f);
		}

		public static void RemoveCritDamage(float f)
		{
			ModdedPlayer.Stats.critDamage.Substract(f);
		}

		public static void AddLifeOnHit(float f)
		{
			ModdedPlayer.Stats.healthOnHit.Add(f);
		}

		public static void RemoveLifeOnHit(float f)
		{
			ModdedPlayer.Stats.healthOnHit.Substract(f);
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

		public static void AddMagicResistance(float f)
		{
			ModdedPlayer.Stats.magicDamageTaken.valueMultiplicative *= 1 - f;
		}

		public static void RemoveMagicResistance(float f)
		{
			ModdedPlayer.Stats.magicDamageTaken.valueMultiplicative /= 1 - f;
		}

		public static void AddAttackSpeed(float f)
		{
			ModdedPlayer.Stats.attackSpeed.Add(f);
		}

		public static void RemoveAttackSpeed(float f)
		{
			ModdedPlayer.Stats.attackSpeed.Substract(f);
		}

		public static void AddExpFactor(float f)
		{
			ModdedPlayer.Stats.expGain.Add(f);
		}

		public static void RemoveExpFactor(float f)
		{
			ModdedPlayer.Stats.expGain.Substract(f);
		}

		public static void AddMaxMassacreTime(float f)
		{
			ModdedPlayer.Stats.maxMassacreTime.Add(f);
		}

		public static void RemoveMaxMassacreTime(float f)
		{
			ModdedPlayer.Stats.maxMassacreTime.Substract(f);
		}

		public static void AddSpellDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.spellIncreasedDmg.Add(f);
		}

		public static void RemoveSpellDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.spellIncreasedDmg.Substract(f);
		}

		public static void AddMeleeDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.meleeIncreasedDmg.Add(f);
		}

		public static void RemoveMeleeDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.meleeIncreasedDmg.Substract(f);
		}

		public static void AddRangedDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.rangedIncreasedDmg.Add(f);
		}

		public static void RemoveRangedDamageAmplifier(float f)
		{
			ModdedPlayer.Stats.rangedIncreasedDmg.Substract(f);
		}

		public static void AddspellFlatDmg(float f)
		{
			ModdedPlayer.Stats.spellFlatDmg.Add(f);
		}

		public static void RemovespellFlatDmg(float f)
		{
			ModdedPlayer.Stats.spellFlatDmg.Substract(f);
		}

		public static void AddMeleeDamageBonus(float f)
		{
			ModdedPlayer.Stats.meleeFlatDmg.Add(f);
		}

		public static void RemoveMeleeDamageBonus(float f)
		{
			ModdedPlayer.Stats.meleeFlatDmg.Substract(f);
		}

		public static void AddRangedDamageBonus(float f)
		{
			ModdedPlayer.Stats.rangedFlatDmg.Add(f);
		}

		public static void RemoveRangedDamageBonus(float f)
		{
			ModdedPlayer.Stats.rangedFlatDmg.Substract(f);
		}

		public static void AddmaxEnergyFromAgi(float f)
		{
			ModdedPlayer.Stats.maxEnergyFromAgi.Add(f);
		}

		public static void RemovemaxEnergyFromAgi(float f)
		{
			ModdedPlayer.Stats.maxEnergyFromAgi.Substract(f);
		}

		public static void AddmaxHealthFromVit(float f)
		{
			ModdedPlayer.Stats.maxHealthFromVit.Add(f);
		}

		public static void RemovemaxHealthFromVit(float f)
		{
			ModdedPlayer.Stats.maxHealthFromVit.Substract(f);
		}

		public static void AddspellDmgFromInt(float f)
		{
			ModdedPlayer.Stats.spellDmgFromInt.Add(f);
		}

		public static void RemovespellDmgFromInt(float f)
		{
			ModdedPlayer.Stats.spellDmgFromInt.Substract(f);
		}

		public static void AddmeleeDmgFromStr(float f)
		{
			ModdedPlayer.Stats.meleeDmgFromStr.Add(f);
		}

		public static void RemovemeleeDmgFromStr(float f)
		{
			ModdedPlayer.Stats.meleeDmgFromStr.Substract(f);
		}

		public static void AddHealingMultipier(float f)
		{
			ModdedPlayer.Stats.allRecoveryMult.Add(f);
		}

		public static void RemoveHealingMultipier(float f)
		{
			ModdedPlayer.Stats.allRecoveryMult.Substract(f);
		}

		public static void AddMoveSpeed(float f)
		{
			ModdedPlayer.Stats.movementSpeed.Add(f);
		}

		public static void RemoveMoveSpeed(float f)
		{
			ModdedPlayer.Stats.movementSpeed.Substract(f);
		}

		public static void AddJump(float f)
		{
			ModdedPlayer.Stats.jumpPower.Add(f);
		}

		public static void RemoveJump(float f)
		{
			ModdedPlayer.Stats.jumpPower.Substract(f);
		}

		//   public static void Add( float f)
		//{
		//    ModdedPlayer.Stats..Add( f;
		//}
		//public static void Remove( float f)
		//{
		//    ModdedPlayer.Stats. .Substract( f;
		//}

		public static void PERMANENT_perkPointIncrease(float f)
		{
			ModdedPlayer.instance.PermanentBonusPerkPoints += Mathf.RoundToInt(f);
			ModdedPlayer.instance.MutationPoints += Mathf.RoundToInt(f);
		}

		public static void PERMANENT_expIncrease(float f)
		{
			ModdedPlayer.instance.AddFinalExperience((long)f);
		}

		public static void AddMagicFind(float f)
		{
			ModdedPlayer.Stats.magicFind.Add(f);
		}

		public static void RemoveMagicFind(float f)
		{
			ModdedPlayer.Stats.magicFind.Substract(f);

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

		public static int GetMaxSocketAmountOnItem(in BaseItem.ItemType type)
		{
			switch (type)
			{
				case BaseItem.ItemType.Other:
				case BaseItem.ItemType.Material:
					return 0;

				case BaseItem.ItemType.ShoulderArmor:
				case BaseItem.ItemType.Weapon:
				case BaseItem.ItemType.Helmet:
				case BaseItem.ItemType.Glove:
				case BaseItem.ItemType.Boot:
				case BaseItem.ItemType.Amulet:
				case BaseItem.ItemType.Ring:
					return 1;

				case BaseItem.ItemType.Bracer:
				case BaseItem.ItemType.Pants:
				case BaseItem.ItemType.SpellScroll:
				case BaseItem.ItemType.Shield:
				case BaseItem.ItemType.Quiver:
					return 2;

				case BaseItem.ItemType.ChestArmor:
					return 3;

				default:
					return 0;
			}
		}
		public static ItemStat GetSocketedStat(in int rarity, BaseItem.ItemType type, int subtypeOffset)
		{
			float value = 1;
			int statid = 0;
			switch (type)
			{
				case BaseItem.ItemType.Shield:
				case BaseItem.ItemType.Quiver:
				case BaseItem.ItemType.Pants:
				case BaseItem.ItemType.ChestArmor:
				case BaseItem.ItemType.ShoulderArmor:
				case BaseItem.ItemType.Glove:
				case BaseItem.ItemType.Bracer:
				case BaseItem.ItemType.SpellScroll:
					value = GetSocketStatAmount_Other(in rarity);
					statid = 1;
					break;

				case BaseItem.ItemType.Weapon:
					value = GetSocketStatAmount_Weapon(in rarity) - 1;
					statid = 3;
					break;

				case BaseItem.ItemType.Helmet:
					value = GetSocketStatAmount_Helmet(in rarity) - 1;
					statid = 0;
					break;

				case BaseItem.ItemType.Boot:
					value = GetSocketStatAmount_Boots(in rarity) - 1;
					statid = 2;
					break;

				case BaseItem.ItemType.Ring:
				case BaseItem.ItemType.Amulet:
					value = GetSocketStatAmount_Amulet(in rarity);
					statid = 4;
					break;
			}
			statid = 5 * (subtypeOffset - 1) + 3001 + statid;
			ItemStat stat = new ItemStat(ItemDataBase.StatByID(statid));
			stat.Amount = value * stat.Multipier;
			return stat;
		}
		public static float GetSocketedStatAmount(in int rarity, BaseItem.ItemType type, int subtypeOffset)
		{
			float value = 1;
			int statid = 0;
			switch (type)
			{
				case BaseItem.ItemType.Shield:
				case BaseItem.ItemType.Quiver:
				case BaseItem.ItemType.Pants:
				case BaseItem.ItemType.ChestArmor:
				case BaseItem.ItemType.ShoulderArmor:
				case BaseItem.ItemType.Glove:
				case BaseItem.ItemType.Bracer:
				case BaseItem.ItemType.SpellScroll:
					value = GetSocketStatAmount_Other(in rarity);
					statid = 1;
					break;

				case BaseItem.ItemType.Weapon:
					value = GetSocketStatAmount_Weapon(in rarity) - 1;
					statid = 3;
					break;

				case BaseItem.ItemType.Helmet:
					value = GetSocketStatAmount_Helmet(in rarity) - 1;
					statid = 0;
					break;

				case BaseItem.ItemType.Boot:
					value = GetSocketStatAmount_Boots(in rarity) - 1;
					statid = 2;
					break;

				case BaseItem.ItemType.Ring:
				case BaseItem.ItemType.Amulet:
					value = GetSocketStatAmount_Amulet(in rarity);
					statid = 4;
					break;
			}
			int stat = 5 * (subtypeOffset - 1) + 3001 + statid;
			return value * ItemDataBase.StatByID(stat).Multipier;
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
					return 1000f ;

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
					return 2.5f;

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
					return 45f;

				case 5:
					return 90f;

				case 6:
					return 300f;

				case 7:
					return 700f;

				default:
					return 10f;
			}
		}
	}
}