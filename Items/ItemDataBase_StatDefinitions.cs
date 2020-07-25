﻿using System.Linq;

using ChampionsOfForest.Items;
using ChampionsOfForest.Player;

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
			ENERGYPERAGILITY,
			HEALTHPERVITALITY,
			SPELLDAMAGEPERINT,
			DAMAGEPERSTRENGTH,
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
			RANGEDDAMAGEPERAGI,
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
			var scOneMinusMult = new ItemStat.StatCompare(x =>
			{
				float f = 1;
				for (int k = 0; k < x.Count; k++)
					f *= 1 - x[k];
				return 1 - f;
			});

			int i = 1;
			new ItemStat(i, 1, 2.25f, 0.82f, "Strength", scAdd, 4, StatActions.AddStrength, StatActions.RemoveStrength, StatActions.AddStrength);
			i++;
			new ItemStat(i, 1, 2.25f, 0.83f, "Agility", scAdd, 4, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility);
			i++;
			new ItemStat(i, 1, 2.25f, 0.83f, "Vitality", scAdd, 4, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality);
			i++;
			new ItemStat(i, 1, 2.25f, 0.83f, "Intelligence", scAdd, 4, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence);
			i++;
			new ItemStat(i, 1, 4f, 1.2f, "Maximum Life", scAdd, 3, StatActions.AddHealth, StatActions.RemoveHealth, StatActions.AddHealth);
			i++;
			new ItemStat(i, 0.25f, 1.25f, 1.2f, "Maximum Energy", scAdd, 3, StatActions.AddEnergy, StatActions.RemoveEnergy, StatActions.AddEnergy);
			i++;
			new ItemStat(i, 0.01f, 0.025f, 1.2f, "Life Per Second", scAdd, 3, StatActions.AddHPRegen, StatActions.RemoveHPRegen, StatActions.AddHPRegen) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.025f, 1.2f, "Stamina Per Second", scAdd, 2, StatActions.AddStaminaRegen, StatActions.RemoveStaminaRegen, StatActions.AddStaminaRegen) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.025f, 0.5f, "Stamina Regeneration", scMult, 5, StatActions.AddStaminaRegenPercent, StatActions.RemoveStaminaRegenPercent, StatActions.AddStaminaRegenPercent) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 1f };
			i++;
			new ItemStat(i, 0.01f, 0.025f, 0.5f, "Life Regeneration", scMult, 5, StatActions.AddHealthRegenPercent, StatActions.RemoveHealthRegenPercent, StatActions.AddHealthRegenPercent) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 1.5f };
			i++;
			new ItemStat(i, 0.001f, 0.007f, 0.5f, "Damage Reduction", scOneMinusMult, 4, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, StatActions.AddDamageReduction) { ValueCap = 0.3f, DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.003f, 0.011f, 0.25f, "Critical Hit Chance", scAdd, 6, StatActions.AddCritChance, StatActions.RemoveCritChance, StatActions.AddCritChance) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.15f };
			i++;
			new ItemStat(i, 0.0008f, 0.00175f, 1.25f, "Critical Hit Damage", scAdd, 6, StatActions.AddCritDamage, StatActions.RemoveCritDamage, StatActions.AddCritDamage) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.75f };
			i++;
			new ItemStat(i, 0.1f, 0.28f, 1f, "Life on hit", scAdd, 4, StatActions.AddLifeOnHit, StatActions.RemoveLifeOnHit, StatActions.AddLifeOnHit);
			i++;
			new ItemStat(i, 0.002f, 0.004f, 0.5f, "Dodge chance", scOneMinusMult, 4, StatActions.AddDodgeChance, StatActions.RemoveDodgeChance, StatActions.AddDodgeChance) { ValueCap = 0.35f, DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 1f, 5f, 1.1f, "Armor", scAdd, 3, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor);
			i++;
			new ItemStat(i, 0.003f, 0.006f, 0.5f, "Resistance to magic", scOneMinusMult, 5, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, StatActions.AddMagicResistance) { ValueCap = 0.35f, DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.004f, 0.008f, 0.4f, "Attack speed", scAdd, 6, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, StatActions.AddAttackSpeed) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.005f, 0.02f, 0.25f, "Exp %", scMult, 7, StatActions.AddExpFactor, StatActions.RemoveExpFactor, StatActions.AddExpFactor) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.5f };
			i++;
			new ItemStat(i, 0.8f, 1.3f, 0f, "Massacre Duration", scAdd, 7, StatActions.AddMaxMassacreTime, StatActions.RemoveMaxMassacreTime, StatActions.AddMaxMassacreTime);
			i++;
			new ItemStat(i, 0.011f, 0.016f, 0.45f, "Spell Damage Increase", scMult, 5, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, StatActions.AddSpellDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.75f };
			i++;
			new ItemStat(i, 0.012f, 0.017f, 0.45f, "Melee Damage Increase", scMult, 5, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, StatActions.AddMeleeDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.75f };
			i++;
			new ItemStat(i, 0.01f, 0.015f, 0.45f, "Ranged Damage Increase", scMult, 5, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, StatActions.AddRangedDamageAmplifier) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.75f };
			i++;
			new ItemStat(i, 0.65f, 1.2f, 1.2f, "Base Spell Damage", scAdd, 4, StatActions.AddSpellDamageBonus, StatActions.RemoveSpellDamageBonus, StatActions.AddSpellDamageBonus);
			i++;
			new ItemStat(i, 0.65f, 0.9f, 1.2f, "Base Melee Damage", scAdd, 4, StatActions.AddMeleeDamageBonus, StatActions.RemoveMeleeDamageBonus, StatActions.AddMeleeDamageBonus);
			i++;
			new ItemStat(i, 0.85f, 1f, 1.2f, "Base Ranged Damage", scAdd, 4, StatActions.AddRangedDamageBonus, StatActions.RemoveRangedDamageBonus, StatActions.AddRangedDamageBonus);
			i++;
			new ItemStat(i, 0.0025f, 0.015f, 0f, "Energy Per Agility", scAdd, 7, StatActions.AddEnergyPerAgility, StatActions.RemoveEnergyPerAgility, StatActions.AddEnergyPerAgility) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.05f, 0.1f, 0f, "Health Per Vitality", scAdd, 7, StatActions.AddHealthPerVitality, StatActions.RemoveHealthPerVitality, StatActions.AddHealthPerVitality) { DisplayAsPercent = false, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.0013f, 0.0023f, 0f, "Spell Damage Per Int", scAdd, 7, StatActions.AddSpellDamageperInt, StatActions.RemoveSpellDamageperInt, StatActions.AddSpellDamageperInt) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.0013f, 0.0025f, 0f, "Damage Per Strength", scAdd, 7, StatActions.AddDamagePerStrength, StatActions.RemoveDamagePerStrength, StatActions.AddDamagePerStrength) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.0005f, 0.003f, 0.6f, "All Healing %", scMult, 6, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, StatActions.AddHealingMultipier) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 1f / 4f, 1f / 4f, 0f, "PERMANENT PERK POINTS", scAdd, 6, null, null, StatActions.PERMANENT_perkPointIncrease);
			i++;
			new ItemStat(i, 100f, 100f, 3.45f, "EXPERIENCE", scAdd, 5, null, null, StatActions.PERMANENT_expIncrease);
			i++;
			new ItemStat(i, 0.004f, 0.01f, 0.4f, "Movement Speed", scAdd, 6, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, StatActions.AddMoveSpeed) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.3f };
			i++;
			new ItemStat(i, 0.008f, 0.01f, 0.5f, "Melee Weapon Range", scAdd, 4, f => ModdedPlayer.instance.MeleeRange += f, f => ModdedPlayer.instance.MeleeRange -= f, f => ModdedPlayer.instance.MeleeRange += f) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.4f };
			i++;
			new ItemStat(i, 0.01f, 0.015f, 0.4f, "Attack Cost Reduction", scOneMinusMult, 3, f => ModdedPlayer.instance.StaminaAttackCost *= 1 - f, f => ModdedPlayer.instance.StaminaAttackCost /= 1 - f, f => ModdedPlayer.instance.StaminaAttackCost *= 1 - f) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.5f };
			i++;
			new ItemStat(i, 0.001f, 0.006f, 0.4f, "Spell Cost Reduction", scOneMinusMult, 6, f => ModdedPlayer.instance.SpellCostRatio *= 1 - f, f => ModdedPlayer.instance.SpellCostRatio /= 1 - f, f => ModdedPlayer.instance.SpellCostRatio *= 1 - f) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.3f };
			i++;
			new ItemStat(i, 0.005f, 0.009f, 0.4f, "Spell Cost to Stamina", scOneMinusMult, 5, f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => RemovePercentage(ref ModdedPlayer.instance.SpellCostToStamina, f), f => AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, f)) { ValueCap = 0.95f, DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Strength", scAdd, 2, StatActions.AddStrength, StatActions.RemoveStrength, StatActions.AddStrength);
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Agility", scAdd, 2, StatActions.AddAgility, StatActions.RemoveAgility, StatActions.AddAgility);
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Vitality", scAdd, 2, StatActions.AddVitality, StatActions.RemoveVitality, StatActions.AddVitality);
			i++;
			new ItemStat(i, 0.4f, 0.8f, 0.8f, "Intelligence", scAdd, 2, StatActions.AddIntelligence, StatActions.RemoveIntelligence, StatActions.AddIntelligence);
			i++;
			new ItemStat(i, 0.3f, 1f, 1.1f, "Armor", scAdd, 2, StatActions.AddArmor, StatActions.RemoveArmor, StatActions.AddArmor);
			i++;
			new ItemStat(i, 0.001f, 0.006f, 0.8f, "Energy Per Second", scAdd, 5, StatActions.AddEnergyRegen, StatActions.RemoveEnergyRegen, StatActions.AddEnergyRegen) { RoundingCount = 3 };
			i++;
			new ItemStat(i, 0.003f, 0.006f, 0.5f, "Maximum Life", scAdd, 6, f => ModdedPlayer.instance.MaxHealthPercent += f, f => ModdedPlayer.instance.MaxHealthPercent -= f, f => ModdedPlayer.instance.MaxHealthPercent += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.003f, 0.006f, 0.5f, "Maximum Energy", scAdd, 6, f => ModdedPlayer.instance.MaxEnergyPercent += f, f => ModdedPlayer.instance.MaxEnergyPercent -= f, f => ModdedPlayer.instance.MaxEnergyPercent += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.002f, 0.005f, 0.5f, "Cooldown Reduction", scOneMinusMult, 6, f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier /= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f)) { DisplayAsPercent = true, RoundingCount = 2, ValueCap = 0.5f };
			i++;
			new ItemStat(i, 0.0005f, 0.0023f, 0f, "Ranged Damage Per Agi", scAdd, 7, f => ModdedPlayer.instance.RangedDamageperAgi += f, f => ModdedPlayer.instance.RangedDamageperAgi += -f, f => ModdedPlayer.instance.RangedDamageperAgi += f) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.01f, 0.045f, 0.9f, "Energy on hit", scAdd, 4, f => ModdedPlayer.instance.EnergyOnHit += f, f => ModdedPlayer.instance.EnergyOnHit += -f, f => ModdedPlayer.instance.EnergyOnHit += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.01f, 0.035f, 0.7f, "Block", scAdd, 1, f => ModdedPlayer.instance.BlockFactor += f, f => ModdedPlayer.instance.BlockFactor += -f, f => ModdedPlayer.instance.BlockFactor += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.005f, 0.015f, 0.4f, "Projectile speed", scAdd, 4, f => ModdedPlayer.instance.ProjectileSpeedRatio += f, f => ModdedPlayer.instance.ProjectileSpeedRatio += -f, f => ModdedPlayer.instance.ProjectileSpeedRatio += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.01f, 0.03f, 0.4f, "Projectile size", scAdd, 4, f => ModdedPlayer.instance.ProjectileSizeRatio += f, f => ModdedPlayer.instance.ProjectileSizeRatio += -f, f => ModdedPlayer.instance.ProjectileSizeRatio += f) { DisplayAsPercent = true, RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.75f, 1.25f, 1.21f, "Melee armor piercing", scAdd, 5, f => ModdedPlayer.instance.ARreduction_melee += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_melee += -Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_melee += Mathf.RoundToInt(f));
			i++;
			new ItemStat(i, 0.35f, 0.9f, 1.21f, "Ranged armor piercing", scAdd, 5, f => ModdedPlayer.instance.ARreduction_ranged += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_ranged += -Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_ranged += Mathf.RoundToInt(f));
			i++;
			new ItemStat(i, 0.35f, 0.75f, 1.21f, "Armor piercing", scAdd, 5, f => ModdedPlayer.instance.ARreduction_all += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_all += -Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_all += Mathf.RoundToInt(f));
			i++;
			new ItemStat(i, 0.001f, 0.013f, 0.4f, "Magic Find", scAdd, 7, StatActions.AddMagicFind, StatActions.RemoveMagicFind, StatActions.AddMagicFind) { DisplayAsPercent = true, RoundingCount = 1, ValueCap = 0.25f };
			i++;
			new ItemStat(i, 0.45f, 0.75f, 0.8f, "All attributes", scAdd, 4, StatActions.AddAllStats, StatActions.RemoveAllStats, StatActions.AddAllStats);
			i++;
			new ItemStat(i, 0, 0, 0, "Refund points", scAdd, 7, f => ModdedPlayer.Respec(), f => ModdedPlayer.Respec(), f => ModdedPlayer.Respec());
			i++;
			new ItemStat(i, 0.003f, 0.008f, 0.4f, "Jump Power", scMult, 4, StatActions.AddJump, StatActions.RemoveJump, StatActions.AddJump) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.026f, 0.066f, 0.45f, "Headshot Damage", scAdd, 4, f => ModdedPlayer.instance.HeadShotDamage += f, f => ModdedPlayer.instance.HeadShotDamage -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.003f, 0.01f, 0.4f, "Fire Damage", scAdd, 4, f => ModdedPlayer.instance.FireAmp += f, f => ModdedPlayer.instance.FireAmp -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.003f, 0.008f, 0.5f, "Chance on hit to slow", scAdd, 3, f => ModdedPlayer.instance.ChanceToSlowOnHit += f, f => ModdedPlayer.instance.ChanceToSlowOnHit -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.001f, 0.005f, 0.4f, "Chance on hit to bleed", scAdd, 3, f => ModdedPlayer.instance.ChanceToBleedOnHit += f, f => ModdedPlayer.instance.ChanceToBleedOnHit -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.003f, 0.008f, 0.5f, "Chance on hit to weaken", scAdd, 3, f => ModdedPlayer.instance.ChanceToWeakenOnHit += f, f => ModdedPlayer.instance.ChanceToWeakenOnHit -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			new ItemStat(i, 0.83f, 1.3f, 1.2f, "Thorns", scAdd, 4, f => ModdedPlayer.instance.thorns += f, f => ModdedPlayer.instance.thorns -= f, f => ModdedPlayer.instance.thorns += f);
			;
			i++;

			//Extra carry items
			i = 1000;
			new ItemStat(i, 6f, 15f, 0f, "Extra Carried Sticks", scAdd, 4, f => ModdedPlayer.instance.AddExtraItemCapacity(57, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(57, -Mathf.RoundToInt(f)), null);
			i++;
			new ItemStat(i, 3f, 10f, 0f, "Extra Carried Rocks", scAdd, 4, f => ModdedPlayer.instance.AddExtraItemCapacity(53, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(53, -Mathf.RoundToInt(f)), null);
			i++;
			new ItemStat(i, 2f, 8f, 0f, "Extra Carried Ropes", scAdd, 4, f => ModdedPlayer.instance.AddExtraItemCapacity(54, Mathf.RoundToInt(f)), f => ModdedPlayer.instance.AddExtraItemCapacity(43, -Mathf.RoundToInt(f)), null);
			i++;

			i = 2000;
			new ItemStat(i, 3f, 20f, 0f, "Black Hole Size", scAdd, 6, f => SpellActions.BLACKHOLE_radius += f, f => SpellActions.BLACKHOLE_radius += -f, f => SpellActions.BLACKHOLE_radius += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.4f, 4f, 0f, "Black Hole Lifetime", scAdd, 6, f => SpellActions.BLACKHOLE_duration += f, f => SpellActions.BLACKHOLE_duration += -f, f => SpellActions.BLACKHOLE_duration += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 1f, 7f, 0f, "Black Hole Gravitational Force", scAdd, 6, f => SpellActions.BLACKHOLE_pullforce += f, f => SpellActions.BLACKHOLE_pullforce += -f, f => SpellActions.BLACKHOLE_pullforce += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 0.3125f, 0.365f, 1f, "Black Hole damage", scAdd, 6, f => SpellActions.BLACKHOLE_damage += f, f => SpellActions.BLACKHOLE_damage += -f, f => SpellActions.BLACKHOLE_damage += f) { RoundingCount = 1 };
			i++;
			new ItemStat(i, 1, 1, 0, "Stun on hit", scAdd, 1, f => ModdedPlayer.instance.IsHammerStun = true, f => ModdedPlayer.instance.IsHammerStun = false, null);
			i++;
			new ItemStat(i, 1, 3.5f, 0, "Snap Freeze Duration", scAdd, 3, f => SpellActions.SnapFreezeDuration += f, f => SpellActions.SnapFreezeDuration -= f, null);
			i++;
			new ItemStat(i, 0.5f, 1.25f, 0, "Raft Speed - HOST ONLY", scAdd, 4, f => ModdedPlayer.instance.RaftSpeedMultipier += f, f => ModdedPlayer.instance.RaftSpeedMultipier -= f, null) { DisplayAsPercent = true, RoundingCount = 2 };
			i++;
			i = 3000;
			new ItemStat(i++, 1, 3.5f, 0, "Empty Socket", scAdd, 0, null, null, null) { Multipier = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Crit Chance", scAdd, 0, StatActions.AddCritChance, StatActions.RemoveCritChance, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Agility", scAdd, 0, StatActions.AddAgility, StatActions.RemoveAgility, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Movement Speed", scAdd, 0, StatActions.AddMoveSpeed, StatActions.RemoveMoveSpeed, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Ranged Damage", scMult, 0, StatActions.AddRangedDamageAmplifier, StatActions.RemoveRangedDamageAmplifier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Feather: Ranged Armor Piercing", scAdd, 0, f => ModdedPlayer.instance.ARreduction_ranged += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_ranged += -Mathf.RoundToInt(f), null) { RoundingCount = 0 };

			//3006
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Attack Speed", scAdd, 0, StatActions.AddAttackSpeed, StatActions.RemoveAttackSpeed, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Strength", scAdd, 0, StatActions.AddStrength, StatActions.RemoveStrength, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Damage Reduction", scOneMinusMult, 0, StatActions.AddDamageReduction, StatActions.RemoveDamageReduction, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Melee Damage", scMult, 0, StatActions.AddMeleeDamageAmplifier, StatActions.RemoveMeleeDamageAmplifier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Shark Tooth: Melee Armor Piercing", scAdd, 0, f => ModdedPlayer.instance.ARreduction_melee += Mathf.RoundToInt(f), f => ModdedPlayer.instance.ARreduction_melee += -Mathf.RoundToInt(f), null) { RoundingCount = 0 };

			//30011
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Cooldown Reduction", scOneMinusMult, 0, f => ModdedPlayer.instance.CoolDownMultipier *= (1 - f), f => ModdedPlayer.instance.CoolDownMultipier /= (1 - f), null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Intelligence", scAdd, 0, StatActions.AddIntelligence, StatActions.RemoveIntelligence, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Spell Cost Reduction", scOneMinusMult, 0, f => ModdedPlayer.instance.SpellCostRatio *= 1 - f, f => ModdedPlayer.instance.SpellCostRatio /= 1 - f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Spell Damage", scMult, 0, StatActions.AddSpellDamageAmplifier, StatActions.RemoveSpellDamageAmplifier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Sapphire: Energy On Hit", scAdd, 0, f => ModdedPlayer.instance.EnergyOnHit += f, f => ModdedPlayer.instance.EnergyOnHit += -f, null) { RoundingCount = 2, Multipier = 0.02f };

			//3016
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Experience Gain", scMult, 0, StatActions.AddExpFactor, StatActions.RemoveExpFactor, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Vitality ", scAdd, 0, StatActions.AddVitality, StatActions.RemoveVitality, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Magic Find", scMult, 0, f => ModdedPlayer.instance.SpellCostRatio *= 1 - f, f => ModdedPlayer.instance.SpellCostRatio /= 1 - f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: All Healing", scMult, 0, StatActions.AddHealingMultipier, StatActions.RemoveHealingMultipier, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Moonstone: Life Per Second", scAdd, 0, StatActions.AddHPRegen, StatActions.RemoveHPRegen, null) { RoundingCount = 1, Multipier = 0.04f };

			//3021
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Maximum Health ", scMult, 0, f => ModdedPlayer.instance.MaxHealthPercent *= 1 + f, f => ModdedPlayer.instance.MaxHealthPercent /= 1 + f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Armor ", scAdd, 0, StatActions.AddArmor, StatActions.RemoveArmor, null) { RoundingCount = 0 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Resistance To Magic", scOneMinusMult, 0, StatActions.AddMagicResistance, StatActions.RemoveMagicResistance, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Weapon Range", scMult, 0, f => ModdedPlayer.instance.MeleeRange += f, f => ModdedPlayer.instance.MeleeRange -= f, null) { DisplayAsPercent = true, RoundingCount = 1 };
			new ItemStat(i++, 1, 3.5f, 7, "Socketed Ore: Thorns", scAdd, 0, f => ModdedPlayer.instance.thorns += f, f => ModdedPlayer.instance.thorns -= f, null) { RoundingCount = 0, Multipier = 3f };
		}
	}
}