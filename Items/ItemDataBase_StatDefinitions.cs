using System.Linq;

using ChampionsOfForest.Items;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public static partial class ItemDataBase
	{
		public enum Stat
		{
			ALL = -1,
			NONE = 0,
			STRENGTH = 1,
			AGILITY,
			VITALITY,
			INTELLIGENCE,
			MAXIMUMLIFE,
			MAXIMUMENERGY,
			LIFEPERSECOND,
			STAMINAPERSECOND,
			STAMINAREGENERATION,
			LIFEREGENERATION,
			DAMAGEREDUCTION,
			CRITICALHITCHANCE,
			CRITICALHITDAMAGE,
			LIFEONHIT,
			DODGECHANCE,
			ARMOR,
			RESISTANCETOMAGIC,
			ATTACKSPEED,
			EXPGAIN,
			MASSACREDURATION,
			SPELLDAMAGEINCREASE,
			MELEEDAMAGEINCREASE,
			RANGEDDAMAGEINCREASE,
			BASESPELLDAMAGE,
			BASEMELEEDAMAGE,
			BASERANGEDDAMAGE,
			MAXENERGYFROMAGI,
			MAXHEALTHFROMVIT,
			SPELLDMGFROMINT,
			MELEEDMGFROMSTR,
			ALLHEALINGPERCENT,
			PERMANENTPERKPOINTS,
			EXPERIENCE,
			MOVEMENTSPEED,
			MELEEWEAPONRANGE,
			ATTACKCOSTREDUCTION,
			SPELLCOSTREDUCTION,
			SPELLCOSTTOSTAMINA,
			LESSERSTRENGTH,
			LESSERAGILITY,
			LESSERVITALITY,
			LESSERINTELLIGENCE,
			LESSERARMOR,
			ENERGYPERSECOND,
			PERCENTMAXIMUMLIFE,
			PERCENTMAXIMUMENERGY,
			COOLDOWNREDUCTION,
			RANGEDDMGFROMAGI,
			ENERGYONHIT,
			BLOCK,
			PROJECTILESPEED,
			PROJECTILESIZE,
			MELEEARMORPIERCING,
			RANGEDARMORPIERCING,
			ARMORPIERCING,
			MAGICFIND,
			ALLATTRIBUTES,
			REFUNDPOINTS,
			JUMPPOWER,
			HEADSHOTDAMAGE,
			FIREDAMAGE,
			CHANCEONHITTOSLOW,
			CHANCEONHITTOBLEED,
			CHANCEONHITTOWEAKEN,
			THORNS,

			EXTRACARRIEDSTICKS = 1000,
			EXTRACARRIEDROCKS,
			EXTRACARRIEDROPES,

			BLACKHOLESIZE = 2000,
			BLACKHOLELIFETIME,
			BLACKHOLEGRAVITATIONALFORCE,
			BLACKHOLEDAMAGE,
			STUNONHIT,
			SNAPFREEZEDURATION,
			RAFTSPEED,
		}

		public static void PopulateStats()
		{
			var scAdd = new ItemStat.StatCompare(x => { return x.Sum(); });
			var scMult = new ItemStat.StatCompare(x =>
			{
				float f = 1;
				for (int j = 0; j < x.Count; j++)
					f *= x[j];
				return f - 1;
			});
			var scMultPlusOne = new ItemStat.StatCompare(x =>
			{
				float f = 1;
				for (int j = 0; j < x.Count; j++)
					f *= 1+ x[j];
				return f;
			});
			var scOneMinusMult = new ItemStat.StatCompare(x =>
			{
				float f = 1;
				for (int k = 0; k < x.Count; k++)
					f *= 1 - x[k];
				return 1 - f;
			});

			int i = 1;
			new ItemStat(i, 1, 2.3f, 0.82f, "Strength", scAdd, 4,
				()=>ModdedPlayer.Stats.strength.GetFormattedAmount(), StatActions.AddStrength, StatActions.RemoveStrength, StatActions.AddStrength);
			i++;
			new ItemStat(i, 1, 2.3f, 0.83f, "Agility", scAdd, 4, () => ModdedPlayer.Stats.agility.GetFormattedAmount(), StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility);
			i++;
			new ItemStat(i, 1, 2.3f, 0.83f, "Vitality", scAdd, 4, () => ModdedPlayer.Stats.vitality.GetFormattedAmount(), StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality);
			i++;
			new ItemStat(i, 1, 2.3f, 0.83f, "Intelligence", scAdd, 4, () => ModdedPlayer.Stats.intelligence.GetFormattedAmount(), StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence);
			i++;
			new ItemStat(i, 1, 4f, 1.2f, "Maximum Life", scAdd, 3, () => ModdedPlayer.Stats.maxHealth.GetFormattedAmount(), StatActions.AddHealth, StatActions.RemoveHealth, StatActions.AddHealth);
			i++;
			new ItemStat(i, 0.25f, 1.25f, 1.2f, "Maximum Energy", scAdd, 3, () => ModdedPlayer.Stats.maxEnergy.GetFormattedAmount(), StatActions.AddEnergy, StatActions.RemoveEnergy, StatActions.AddEnergy);
			i++;
			new ItemStat(i, 0.005f, 0.025f, 1.2f, "Life Per Second", scAdd, 3, () => ModdedPlayer.Stats.healthRecoveryPerSecond.GetFormattedAmount(), StatActions.AddHPRegen, StatActions.RemoveHPRegen, StatActions.AddHPRegen) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.031f, 1.2f, "Stamina Per Second", scAdd, 2, () => ModdedPlayer.Stats.staminaRecoveryperSecond.GetFormattedAmount(), StatActions.AddStaminaRegen, StatActions.RemoveStaminaRegen, StatActions.AddStaminaRegen) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.025f, 0.5f, "Stamina Regeneration", scAdd,5, () => ModdedPlayer.Stats.staminaPerSecRate.GetFormattedAmount(),  StatActions.AddStaminaRegenPercent, StatActions.RemoveStaminaRegenPercent, StatActions.AddStaminaRegenPercent) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 1f };
			i++;
			new ItemStat(i, 0.01f, 0.025f, 0.5f, "Life Regeneration", scAdd, 5, () => ModdedPlayer.Stats.healthPerSecRate.GetFormattedAmount(), StatActions.AddHealthRegenPercent, StatActions.RemoveHealthRegenPercent, StatActions.AddHealthRegenPercent) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 1.5f };
			i++;
			new ItemStat(i, 0.001f, 0.007f, 0.5f, "Damage Reduction", scOneMinusMult, 4, () => (1-ModdedPlayer.Stats.allDamageTaken).ToString("P"), StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, StatActions.AddDamageReduction) { ValueCap = 0.3f, DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.003f, 0.013f, 0.25f, "Critical Hit Chance", scAdd, 6, () => ModdedPlayer.Stats.critChance.GetFormattedAmount(), StatActions.AddCritChance, StatActions.RemoveCritChance, StatActions.AddCritChance) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.15f };
			i++;
			new ItemStat(i, 0.001f, 0.00215f, 1.25f, "Critical Hit Damage", scAdd, 6, () => ModdedPlayer.Stats.critDamage.GetFormattedAmount(), StatActions.AddCritDamage, StatActions.RemoveCritDamage, StatActions.AddCritDamage) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 1f };
			i++;
			new ItemStat(i, 0.1f, 0.28f, 1f, "Life on hit", scAdd, 4, () => ModdedPlayer.Stats.healthOnHit.GetFormattedAmount(), StatActions.AddLifeOnHit, StatActions.RemoveLifeOnHit, StatActions.AddLifeOnHit);
			i++;
			new ItemStat(i, 0.002f, 0.004f, 0.5f, "Dodge chance", scOneMinusMult, 4, () => (1-ModdedPlayer.Stats.getHitChance).ToString("P"), StatActions.AddDodgeChance, StatActions.RemoveDodgeChance, StatActions.AddDodgeChance) { ValueCap = 0.35f, DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 1f, 5f, 1.1f, "Armor", scAdd, 3, () => ModdedPlayer.Stats.armor.GetFormattedAmount(), StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor);
			i++;
			new ItemStat(i, 0.003f, 0.006f, 0.5f, "Resistance to magic", scOneMinusMult, 5, () => (1 - ModdedPlayer.Stats.magicDamageTaken).ToString("P"), StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, StatActions.AddMagicResistance) { ValueCap = 0.35f, DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.004f, 0.008f, 0.4f, "Attack speed", scAdd, 6, () => ModdedPlayer.Stats.attackSpeed.GetFormattedAmount(), StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, StatActions.AddAttackSpeed) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.005f, 0.02f, 0.25f, "Experience gain", scMult, 7, () => ModdedPlayer.Stats.expGain.GetFormattedAmount(), StatActions.AddExpFactor, StatActions.RemoveExpFactor, StatActions.AddExpFactor) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.5f };
			i++;
			new ItemStat(i, 1.5f, 1.8f, 0f, "Massacre Duration", scAdd, 7, () => ModdedPlayer.Stats.maxMassacreTime.GetFormattedAmount(), StatActions.AddMaxMassacreTime, StatActions.RemoveMaxMassacreTime, StatActions.AddMaxMassacreTime)
			{
				RoundingCount = 1
			};
			i++;
			new ItemStat(i, 0.011f, 0.016f, 0.45f, "Spell Damage Increase", scAdd, 5, () => ModdedPlayer.Stats.spellIncreasedDmg.GetFormattedAmount(), StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, StatActions.AddSpellDamageAmplifier) 
			{ 
				DisplayAsPercent = true,
				RoundingCount = 1,
				ValueCap = 0.75f 
			};
			i++;
			new ItemStat(i, 0.012f, 0.017f, 0.45f, "Melee Damage Increase", scAdd, 5, () => ModdedPlayer.Stats.meleeIncreasedDmg.GetFormattedAmount(), StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, StatActions.AddMeleeDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.75f };
			i++;
			new ItemStat(i, 0.01f, 0.015f, 0.45f, "Ranged Damage Increase", scAdd, 5, () => ModdedPlayer.Stats.rangedIncreasedDmg.GetFormattedAmount(), StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, StatActions.AddRangedDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.75f };
			i++;
			new ItemStat(i, 0.65f, 1.2f, 1.2f, "Base Spell Damage", scAdd, 4, () => ModdedPlayer.Stats.spellFlatDmg.GetFormattedAmount(), StatActions.AddspellFlatDmg, StatActions.RemovespellFlatDmg, StatActions.AddspellFlatDmg);
			i++;
			new ItemStat(i, 0.65f, 0.9f, 1.2f, "Base Melee Damage", scAdd, 4, () => ModdedPlayer.Stats.meleeFlatDmg.GetFormattedAmount(), StatActions.AddMeleeDamageBonus, StatActions.RemoveMeleeDamageBonus, StatActions.AddMeleeDamageBonus);
			i++;
			new ItemStat(i, 0.85f, 1f, 1.2f, "Base Ranged Damage", scAdd, 4,() => ModdedPlayer.Stats.rangedFlatDmg.GetFormattedAmount(), StatActions.AddRangedDamageBonus, StatActions.RemoveRangedDamageBonus, StatActions.AddRangedDamageBonus);
			i++;
			new ItemStat(i, 0.0025f, 0.015f, 0f, "Energy Per Agility", scAdd, 7, () => ModdedPlayer.Stats.maxEnergyFromAgi.GetFormattedAmount(), StatActions.AddmaxEnergyFromAgi, StatActions.RemovemaxEnergyFromAgi, StatActions.AddmaxEnergyFromAgi) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.05f, 0.1f, 0f, "Health Per Vitality", scAdd, 7, () => ModdedPlayer.Stats.maxHealthFromVit.GetFormattedAmount(), StatActions.AddmaxHealthFromVit, StatActions.RemovemaxHealthFromVit, StatActions.AddmaxHealthFromVit) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.0013f, 0.0023f, 0f, "Spell Damage Per Int", scAdd, 7, () => ModdedPlayer.Stats.spellDmgFromInt.GetFormattedAmount(), StatActions.AddspellDmgFromInt, StatActions.RemovespellDmgFromInt, StatActions.AddspellDmgFromInt) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.0013f, 0.0025f, 0f, "Melee Damage Per Strength", scAdd,7, () => ModdedPlayer.Stats.meleeDmgFromStr.GetFormattedAmount(), StatActions.AddmeleeDmgFromStr, StatActions.RemovemeleeDmgFromStr, StatActions.AddmeleeDmgFromStr) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.0005f, 0.003f, 0.6f, "All Recovery", scMultPlusOne, 6, () => ModdedPlayer.Stats.allRecoveryMult.GetFormattedAmount(), StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, StatActions.AddHealingMultipier) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 1f / 4f, 1f / 4f, 0f, "PERMANENT PERK POINTS", scAdd, 6,()=> ModdedPlayer.instance.MutationPoints.ToString(), null, null, StatActions.PERMANENT_perkPointIncrease);
			i++;
			new ItemStat(i, 100f, 100f, 3.45f, "EXPERIENCE", scAdd, 5,()=> ModdedPlayer.instance.ExpCurrent.ToString("N")+ " / " + ModdedPlayer.instance.ExpGoal.ToString("N"), null, null, StatActions.PERMANENT_expIncrease);
			i++;
			new ItemStat(i, 0.004f, 0.02f, 0.4f, "Movement Speed", scAdd, 6, () => ModdedPlayer.Stats.movementSpeed.GetFormattedAmount(), StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, StatActions.AddMoveSpeed) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.3f };
			i++;
			new ItemStat(i, 0.008f, 0.01f, 0.5f, "Weapon Size", scMultPlusOne, 4, () => ModdedPlayer.Stats.weaponRange.GetFormattedAmount(), f => ModdedPlayer.Stats.weaponRange.Multiply(1 + f), f => ModdedPlayer.Stats.weaponRange.Divide(1 + f), null) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.4f };
			i++;
			new ItemStat(i, 0.01f, 0.015f, 0.4f, "Attack Cost Reduction", scOneMinusMult, 3, () => (ModdedPlayer.Stats.attackStaminaCost-1).ToString("P"), f => ModdedPlayer.Stats.attackStaminaCost.Multiply(1 - f), f => ModdedPlayer.Stats.attackStaminaCost.Divide(1 - f)) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.5f };
			i++;
			new ItemStat(i, 0.001f, 0.006f, 0.4f, "Spell Cost Reduction", scOneMinusMult, 6, () => (1 - ModdedPlayer.Stats.spellCost).ToString("P"), f => ModdedPlayer.Stats.spellCost.valueMultiplicative *= 1 - f, f => ModdedPlayer.Stats.spellCost.valueMultiplicative /= 1 - f, f => ModdedPlayer.Stats.spellCost.valueMultiplicative*= 1 - f) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.3f };
			i++;
			new ItemStat(i, 0.005f, 0.009f, 0.4f, "Spell Cost to Stamina", scOneMinusMult, 5, () => (ModdedPlayer.Stats.SpellCostToStamina).ToString("P"), f => ModdedPlayer.Stats.spellCostEnergyCost.Multiply(1-f), f => ModdedPlayer.Stats.spellCostEnergyCost.Divide(1 - f)) { ValueCap = 0.95f, DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Strength", scAdd, 2, () => ModdedPlayer.Stats.strength.GetFormattedAmount(), StatActions.AddStrength, StatActions.RemoveStrength, StatActions.AddStrength);
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Agility", scAdd, 2, () => ModdedPlayer.Stats.agility.GetFormattedAmount(), StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility);
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Vitality", scAdd, 2, () => ModdedPlayer.Stats.vitality.GetFormattedAmount(), StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality);
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Intelligence", scAdd, 2, () => ModdedPlayer.Stats.intelligence.GetFormattedAmount(), StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence);
			i++;
			new ItemStat(i, 0.3f, 1f, 1.1f, "Armor", scAdd, 2, () => ModdedPlayer.Stats.armor.GetFormattedAmount(), StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor);
			i++;
			new ItemStat(i, 0.001f, 0.006f, 0.8f, "Energy Per Second", scAdd, 5, () => ModdedPlayer.Stats.energyRecoveryperSecond.GetFormattedAmount(), StatActions.AddEnergyRegen, StatActions.RemoveEnergyRegen, StatActions.AddEnergyRegen) { RoundingCount = 3 };
			i++;
			new ItemStat(i, 0.003f, 0.006f, 0.5f, "Maximum Life", scAdd, 6, () => ModdedPlayer.Stats.maxHealthMult.GetFormattedAmount(), f => ModdedPlayer.Stats.maxHealthMult.valueAdditive += f, f => ModdedPlayer.Stats.maxHealthMult.valueAdditive -= f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.003f, 0.006f, 0.5f, "Maximum Energy", scAdd, 6, () => ModdedPlayer.Stats.maxEnergyMult.GetFormattedAmount(), f => ModdedPlayer.Stats.maxEnergyMult.valueAdditive+= f, f => ModdedPlayer.Stats.maxEnergyMult.valueAdditive -= f, f => ModdedPlayer.Stats.maxEnergyMult.valueAdditive += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.002f, 0.005f, 0.5f, "Cooldown Reduction", scOneMinusMult, 6, () => (1-ModdedPlayer.Stats.cooldown).ToString("P"), f => ModdedPlayer.Stats.cooldown.valueMultiplicative *= (1 - f), f => ModdedPlayer.Stats.cooldown.valueMultiplicative /= (1 - f), f => ModdedPlayer.Stats.cooldown.valueMultiplicative *= (1 - f)) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.5f };
			i++;
			new ItemStat(i, 0.0005f, 0.0023f, 0f, "Ranged Damage Per Agi", scAdd, 7, () => ModdedPlayer.Stats.rangedDmgFromAgi.GetFormattedAmount(), f => ModdedPlayer.Stats.rangedDmgFromAgi.valueAdditive += f, f => ModdedPlayer.Stats.rangedDmgFromAgi.valueAdditive += -f, f => ModdedPlayer.Stats.rangedDmgFromAgi.valueAdditive += f) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.045f, 0.8f, "Energy on hit", scAdd, 4, () => ModdedPlayer.Stats.energyOnHit.GetFormattedAmount(), f => ModdedPlayer.Stats.energyOnHit.valueAdditive += f, f => ModdedPlayer.Stats.energyOnHit.valueAdditive += -f, f => ModdedPlayer.Stats.energyOnHit.valueAdditive += f) { RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.03f, 0.5f, "Block", scAdd, 1, () => ModdedPlayer.Stats.block.GetFormattedAmount(), f => ModdedPlayer.Stats.block.valueAdditive += f, f => ModdedPlayer.Stats.block.valueAdditive += -f, f => ModdedPlayer.Stats.block.valueAdditive += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.005f, 0.015f, 0.4f, "Projectile speed", scAdd, 4, () => ModdedPlayer.Stats.projectileSpeed.GetFormattedAmount(), f => ModdedPlayer.Stats.projectileSpeed.valueAdditive += f, f => ModdedPlayer.Stats.projectileSpeed.valueAdditive += -f, f => ModdedPlayer.Stats.projectileSpeed.valueAdditive += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.01f, 0.03f, 0.4f, "Projectile size", scAdd, 4, () => ModdedPlayer.Stats.projectileSize.GetFormattedAmount(), f => ModdedPlayer.Stats.projectileSize.valueAdditive += f, f => ModdedPlayer.Stats.projectileSize.valueAdditive += -f, f => ModdedPlayer.Stats.projectileSize.valueAdditive += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.75f, 1.25f, 1.21f, "Melee armor piercing", scAdd, 5, () => ModdedPlayer.Stats.TotalMeleeArmorPiercing.ToString(), f => ModdedPlayer.Stats.meleeArmorPiercing.valueAdditive += Mathf.RoundToInt(f), f => ModdedPlayer.Stats.meleeArmorPiercing.valueAdditive += -Mathf.RoundToInt(f), f => ModdedPlayer.Stats.meleeArmorPiercing.valueAdditive += Mathf.RoundToInt(f));
			i++;
			new ItemStat(i, 0.35f, 0.9f, 1.2f, "Ranged armor piercing", scAdd, 5, () => ModdedPlayer.Stats.TotalRangedArmorPiercing.ToString(), f => ModdedPlayer.Stats.rangedArmorPiercing.valueAdditive += Mathf.RoundToInt(f), f => ModdedPlayer.Stats.rangedArmorPiercing.valueAdditive += -Mathf.RoundToInt(f), f => ModdedPlayer.Stats.rangedArmorPiercing.valueAdditive += Mathf.RoundToInt(f));
			i++;
			new ItemStat(i, 0.35f, 0.8f, 1.2f, "Armor piercing", scAdd, 5, () => $"M{ModdedPlayer.Stats.TotalMeleeArmorPiercing} R{ModdedPlayer.Stats.TotalRangedArmorPiercing}", f => ModdedPlayer.Stats.allArmorPiercing.valueAdditive += Mathf.RoundToInt(f), f => ModdedPlayer.Stats.allArmorPiercing.valueAdditive += -Mathf.RoundToInt(f), f => ModdedPlayer.Stats.allArmorPiercing.valueAdditive += Mathf.RoundToInt(f));
			i++;
			new ItemStat(i, 0.002f, 0.014f, 0.4f, "Magic Find", scAdd, 7, () => ItemDataBase.MagicFind.ToString("P"), StatActions.AddMagicFind, StatActions.RemoveMagicFind, StatActions.AddMagicFind) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.25f };
			i++;
			new ItemStat(i, 0.4f, 0.75f, 0.8f, "All attributes", scAdd, 4, () => $"S{ModdedPlayer.Stats.strength} A{ModdedPlayer.Stats.agility} I{ModdedPlayer.Stats.intelligence} V{ModdedPlayer.Stats.vitality}", StatActions.AddAllStats, StatActions.RemoveAllStats, StatActions.AddAllStats);
			i++;
			new ItemStat(i, 0, 0, 0, "Refund points", scAdd, 7,()=>"", f => ModdedPlayer.Respec(), f => ModdedPlayer.Respec(), f => ModdedPlayer.Respec());
			i++;
			new ItemStat(i, 0.003f, 0.008f, 0.4f, "Jump Power", scAdd, 4,()=> ModdedPlayer.Stats.jumpPower.GetFormattedAmount() ,StatActions.AddJump, StatActions.RemoveJump, StatActions.AddJump) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.022f, 0.068f, 0.45f, "Headshot Damage", scMultPlusOne, 4, () => ModdedPlayer.Stats.headShotDamage.GetFormattedAmount(), f => ModdedPlayer.Stats.headShotDamage.valueMultiplicative *= 1+f, f => ModdedPlayer.Stats.headShotDamage.valueMultiplicative/= 1+f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.003f, 0.011f, 0.4f, "Fire Damage", scAdd, 4, () => ModdedPlayer.Stats.fireDamage.GetFormattedAmount(), f => ModdedPlayer.Stats.fireDamage.valueAdditive += f, f => ModdedPlayer.Stats.fireDamage.valueAdditive -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.003f, 0.008f, 0.5f, "Chance on hit to slow", scAdd, 3, () => ModdedPlayer.Stats.chanceToSlow.GetFormattedAmount(), f => ModdedPlayer.Stats.chanceToSlow.valueAdditive += f, f => ModdedPlayer.Stats.chanceToSlow.valueAdditive -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.001f, 0.006f, 0.5f, "Chance on hit to bleed", scAdd, 3, () => ModdedPlayer.Stats.chanceToBleed.GetFormattedAmount(), f => ModdedPlayer.Stats.chanceToBleed.valueAdditive += f, f => ModdedPlayer.Stats.chanceToBleed.valueAdditive -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.003f, 0.008f, 0.5f, "Chance on hit to weaken", scAdd, 3, () => ModdedPlayer.Stats.chanceToWeaken.GetFormattedAmount(), f => ModdedPlayer.Stats.chanceToWeaken.valueAdditive += f, f => ModdedPlayer.Stats.chanceToWeaken.valueAdditive -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.83f, 1.3f, 1.2f, "Thorns", scAdd, 4, () => ModdedPlayer.Stats.thorns.GetFormattedAmount(), f => ModdedPlayer.Stats.thorns.valueAdditive += f, f => ModdedPlayer.Stats.thorns.valueAdditive -= f, f => ModdedPlayer.Stats.thorns.valueAdditive += f);
			;
			i++;

			//Extra carry items
			i = 1000;
			new ItemStat(i, 6f, 15f, 0f, "Extra Carried Sticks", scAdd, 4, () =>LocalPlayer.Inventory.GetMaxAmountOf(57).ToString(), f => ModdedPlayer.instance.AddExtraItemCapacity(57, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(57, -Mathf.RoundToInt(f)), null);
			i++;
			new ItemStat(i, 3f, 10f, 0f, "Extra Carried Rocks", scAdd, 4, () => LocalPlayer.Inventory.GetMaxAmountOf(53).ToString(), f => ModdedPlayer.instance.AddExtraItemCapacity(53, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(53, -Mathf.RoundToInt(f)), null);
			i++;
			new ItemStat(i, 2f, 8f, 0f, "Extra Carried Ropes", scAdd, 4, () => LocalPlayer.Inventory.GetMaxAmountOf(43).ToString(), f => ModdedPlayer.instance.AddExtraItemCapacity(54, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(43, -Mathf.RoundToInt(f)), null);
			i++;

			i = 2000;
			new ItemStat(i, 3f, 20f, 0f, "Black Hole Size", scAdd, 6, () => ModdedPlayer.Stats.spell_blackhole_radius.GetFormattedAmount(), f => ModdedPlayer.Stats.spell_blackhole_radius.valueAdditive += f, f => ModdedPlayer.Stats.spell_blackhole_radius.valueAdditive += -f, f => ModdedPlayer.Stats.spell_blackhole_radius.valueAdditive += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 1f, 4f, 0f, "Black Hole Lifetime", scAdd, 6, () => ModdedPlayer.Stats.spell_blackhole_duration.GetFormattedAmount(), f => ModdedPlayer.Stats.spell_blackhole_duration.valueAdditive += f, f => ModdedPlayer.Stats.spell_blackhole_duration.valueAdditive += -f, f => ModdedPlayer.Stats.spell_blackhole_duration.valueAdditive += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 4f, 7f, 0f, "Black Hole Gravitational Force", scAdd, 6, () => ModdedPlayer.Stats.spell_blackhole_pullforce.GetFormattedAmount(), f => ModdedPlayer.Stats.spell_blackhole_pullforce.valueAdditive += f, f => ModdedPlayer.Stats.spell_blackhole_pullforce.valueAdditive += -f, f => ModdedPlayer.Stats.spell_blackhole_pullforce.valueAdditive += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.3125f, 0.465f, 1f, "Black Hole damage", scAdd, 6, () => ModdedPlayer.Stats.spell_blackhole_damage.GetFormattedAmount(), f => ModdedPlayer.Stats.spell_blackhole_damage.valueAdditive += f, f => ModdedPlayer.Stats.spell_blackhole_damage.valueAdditive += -f, f => ModdedPlayer.Stats.spell_blackhole_damage.valueAdditive += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 1, 1, 0, "Stun on hit", scAdd, 1, () =>"", f => ModdedPlayer.Stats.i_HammerStun.value = true, f => ModdedPlayer.Stats.i_HammerStun.value = false, null);
			i++;
			new ItemStat(i, 1, 3.5f, 0, "Snap Freeze Duration", scAdd, 3, () => ModdedPlayer.Stats.spell_snapFreezeDuration.GetFormattedAmount(), f => ModdedPlayer.Stats.spell_snapFreezeDuration.valueAdditive += f, f => ModdedPlayer.Stats.spell_snapFreezeDuration.valueAdditive -= f, null);
			i++;
			new ItemStat(i, 0.5f, 1.25f, 0, "Raft Speed", scAdd, 4, () => ModdedPlayer.Stats.perk_RaftSpeedMultipier.GetFormattedAmount(), f => ModdedPlayer.Stats.perk_RaftSpeedMultipier.Add(f), f => ModdedPlayer.Stats.perk_RaftSpeedMultipier.Substract(f), null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			i = 3000;
			new ItemStat(i++, 1, 3.5f, 0, "Empty Socket", scAdd, 0, null, null, null) { Multipier = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Crit Chance", scAdd, 0,null, StatActions.AddCritChance, StatActions.RemoveCritChance, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Agility", scAdd, 0,null, StatActions.AddAgility, StatActions.RemoveAgility, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Movement Speed", scAdd, 0,null, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Ranged Damage", scMult, 0,null, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Ranged Armor Piercing", scAdd, 0,null, f => ModdedPlayer.Stats.rangedArmorPiercing.valueAdditive += Mathf.RoundToInt(f), f => ModdedPlayer.Stats.rangedArmorPiercing.valueAdditive += -Mathf.RoundToInt(f), null) { RoundingCount = 0 };

			//3006
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Attack Speed", scAdd, 0,null, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Strength", scAdd, 0,null, StatActions.AddStrength, StatActions.RemoveStrength, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Damage Reduction", scOneMinusMult, 0,null, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Melee Damage", scMult, 0,null, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Melee Armor Piercing", scAdd, 0,null, f => ModdedPlayer.Stats.meleeArmorPiercing.valueAdditive += Mathf.RoundToInt(f), f => ModdedPlayer.Stats.meleeArmorPiercing.valueAdditive += -Mathf.RoundToInt(f), null) { RoundingCount = 0 };

			//30011
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Cooldown Reduction", scOneMinusMult, 0,null, f => ModdedPlayer.Stats.cooldown.valueMultiplicative *= (1 - f), f => ModdedPlayer.Stats.cooldown.valueMultiplicative /= (1 - f), null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Intelligence", scAdd, 0,null, StatActions.AddIntelligence, StatActions.RemoveIntelligence, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Spell Cost Reduction", scOneMinusMult, 0,null, f => ModdedPlayer.Stats.spellCost.valueMultiplicative *= 1 - f, f => ModdedPlayer.Stats.spellCost.valueMultiplicative /= 1 - f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Spell Damage", scMult, 0,null, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Energy On Hit", scAdd, 0,null, f => ModdedPlayer.Stats.energyOnHit.valueAdditive += f, f => ModdedPlayer.Stats.energyOnHit.valueAdditive += -f, null) { RoundingCount = 2, Multipier = 0.02f };

			//3016
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Experience Gain", scMult, 0,null, StatActions.AddExpFactor, StatActions.RemoveExpFactor, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Vitality ", scAdd, 0,null, StatActions.AddVitality, StatActions.RemoveVitality, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Magic Find", scMult, 0,null, f => ModdedPlayer.Stats.spellCost.valueMultiplicative *= 1 - f, f => ModdedPlayer.Stats.spellCost.valueMultiplicative /= 1 - f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: All Healing", scMult, 0,null, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Life Per Second", scAdd, 0,null, StatActions.AddHPRegen, StatActions.RemoveHPRegen, null) { RoundingCount = 1, Multipier = 0.04f };

			//3021
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Maximum Health ", scMult, 0,null, f => ModdedPlayer.Stats.maxHealthMult.valueAdditive *= 1 + f, f => ModdedPlayer.Stats.maxHealthMult.valueAdditive /= 1 + f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Armor ", scAdd, 0,null, StatActions.AddArmor, StatActions.RemoveArmor, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Resistance To Magic", scOneMinusMult, 0,null, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Weapon Range", scMult, 0,null, f => ModdedPlayer.Stats.weaponRange.Multiply(1+f), f => ModdedPlayer.Stats.weaponRange.Divide(1+f), null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Thorns", scAdd, 0,null, f => ModdedPlayer.Stats.thorns.valueAdditive += f, f => ModdedPlayer.Stats.thorns.valueAdditive -= f, null) { RoundingCount = 0, Multipier = 3f };
		}
	}
}