using System.Linq;

using ChampionsOfForest.Items;
using ChampionsOfForest.Localization;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Items.ItemDatabase.Stat;

namespace ChampionsOfForest.Items
{
	public static partial class ItemDatabase
	{
		public enum Stat
		{
			NONE = 0,
			STRENGTH = 1,
			AGILITY,
			VITALITY,
			INTELLIGENCE,
			MAX_LIFE,
			MAX_ENERGY,
			LIFE_REGEN_BASE,
			STAMINA_REGEN_BASE,
			STAMINA_AND_ENERGY_REGEN_MULT,
			LIFE_REGEN_MULT,
			DAMAGE_REDUCTION,
			CRIT_CHANCE,
			CRIT_DAMAGE,
			LIFE_ON_HIT,
			DODGE_CHANCE,
			ARMOR,
			ELITE_DAMAGE_REDUCTION,
			ATTACKSPEED,
			EXP_GAIN_MULT,
			MASSACRE_DURATION,
			SPELL_DAMAGE_MULTIPLIER,
			MELEE_DAMAGE_INCREASE,
			RANGED_DAMAGE_INCREASE,
			BASE_SPELL_DAMAGE,
			BASE_MELEE_DAMAGE,
			BASE_RANGED_DAMAGE,
			MAX_ENERGY_FROM_AGILITY,
			MAX_HEALTH_FROM_VITALITY,
			SPELL_DMG_FROM_INT,
			MELEE_DMG_FROM_STR,
			ALL_RECOVERY,
			MOVEMENT_SPEED,
			MELEE_WEAPON_RANGE,
			ATTACK_COST_REDUCTION,
			SPELL_COST_REDUCTION,
			SPELL_COST_TO_STAMINA,
			ENERGY_REGEN_BASE,
			MAX_LIFE_MULT,
			MAX_ENERGY_MULT,
			COOLDOWN_REDUCTION,
			RANGED_DMG_FROM_AGI,
			ENERGY_ON_HIT,
			BLOCK,
			PROJECTILE_SPEED,
			PROJECTILE_SIZE,
			MELEE_ARMOR_PIERCING,
			RANGED_ARMOR_PIERCING,
			ARMOR_PIERCING,
			LOOT_QUANTITY,
			LOOT_QUALITY,
			ALL_ATTRIBUTES,
			REFUNDPOINTS,
			JUMP_POWER,
			HEADSHOT_DAMAGE_MULT,
			FIRE_DAMAGE,
			CHANCE_ON_HIT_TO_SLOW,
			CHANCE_ON_HIT_TO_BLEED,
			CHANCE_ON_HIT_TO_WEAKEN,
			THORNS,
			THORNS_MULT,
			PROJECTILE_PIERCE_CHANCE,
			EXPLOSION_DAMAGE,
			THROWN_SPEAR_DAMAGE,

			EXTRA_CARRIED_STICKS,
			EXTRA_CARRIED_ROCKS,
			EXTRA_CARRIED_ROPES,
			EXTRA_CARRIED_DRINKS,
			EXTRA_CARRIED_FOOD,

			BLACKHOLE_SIZE,
			BLACK_HOLE_DURATION,
			BLACK_HOLE_GRAVITY_FORCE,
			BLACK_HOLE_DAMAGE,
			HAMMER_STUN_ON_HIT,
			SNAP_FREEZE_DURATION,
			RAFT_SPEED,

		}

		private static void PopulateStats()
		{
			// Attributes
			new ItemStatBuilder(Stat.STRENGTH, "Strength", 2f, 3f)
				.AffectsStat(ModdedPlayer.Stats.strength)
				.Additive(ModdedPlayer.Stats.strength)
				.LevelScaling(0.4f);

			new ItemStatBuilder(Stat.AGILITY, "Agility", 2f, 3f)
				.AffectsStat(ModdedPlayer.Stats.agility)
				.Additive(ModdedPlayer.Stats.agility)
				.LevelScaling(0.4f);

			new ItemStatBuilder(Stat.VITALITY, "Vitality", 2f, 3f)
				.AffectsStat(ModdedPlayer.Stats.vitality)
				.Additive(ModdedPlayer.Stats.vitality)
				.LevelScaling(0.4f);

			new ItemStatBuilder(Stat.INTELLIGENCE, "Intelligence", 2f, 3f)
				.AffectsStat(ModdedPlayer.Stats.vitality)
				.Additive(ModdedPlayer.Stats.vitality)
				.LevelScaling(0.4f);

			new ItemStatBuilder(Stat.ALL_ATTRIBUTES, "All attributes", 1f, 1.5f) 
			{ 
				OnEquip = f => {
					ModdedPlayer.Stats.strength.Add((int)f);
					ModdedPlayer.Stats.intelligence.Add((int)f);
					ModdedPlayer.Stats.agility.Add((int)f);
					ModdedPlayer.Stats.vitality.Add((int)f);
				},
				OnUnequip = f => {
					ModdedPlayer.Stats.strength.Sub((int)f);
					ModdedPlayer.Stats.intelligence.Sub((int)f);
					ModdedPlayer.Stats.agility.Sub((int)f);
					ModdedPlayer.Stats.vitality.Sub((int)f);
				},
				GetTotalStat = () => $"{ModdedPlayer.Stats.strength} Str, {ModdedPlayer.Stats.agility} Agi, {ModdedPlayer.Stats.intelligence} Int, {ModdedPlayer.Stats.vitality} Vit"
			}.LevelScaling(0.4f);


			// Life
			new ItemStatBuilder(Stat.MAX_LIFE, "Increased life", 3.75f, 5.65f)
				.AffectsStat(ModdedPlayer.Stats.maxLife)
				.Additive(ModdedPlayer.Stats.maxLife)
				.LevelScaling(1.21f);

			new ItemStatBuilder(Stat.MAX_ENERGY, "Increased energy", 0.7f, 1.5f)
				.AffectsStat(ModdedPlayer.Stats.maxEnergy)
				.Additive(ModdedPlayer.Stats.maxEnergy)
				.LevelScaling(0.5f);

			new ItemStatBuilder(Stat.LIFE_REGEN_BASE, "Life regeneration", 0.013f, 0.023f)
				.AffectsStat(ModdedPlayer.Stats.lifeRegenBase)
				.Additive(ModdedPlayer.Stats.lifeRegenBase)
				.LinearLevelScaling()
				.RoundTo(2);

			new ItemStatBuilder(Stat.STAMINA_REGEN_BASE, "Stamina regeneration", 0.035f, 0.11f)
				.AffectsStat(ModdedPlayer.Stats.staminaRegenBase)
				.Additive(ModdedPlayer.Stats.staminaRegenBase)
				.LevelScaling(0.5f)
				.RoundTo(2);

			new ItemStatBuilder(Stat.STAMINA_AND_ENERGY_REGEN_MULT, "Stamina and energy regeneration", 0.04f, 0.08f)
				.AffectsStat(ModdedPlayer.Stats.energyRegenMult)
				.Additive(ModdedPlayer.Stats.energyRegenMult)
				.LevelScaling(0.1f)
				.RarityScaling(0.2f)
				.PercentFormatting()
				.RoundTo(2);

			new ItemStatBuilder(Stat.LIFE_REGEN_MULT, "Life regeneration", 0.04f, 0.08f)
				.AffectsStat(ModdedPlayer.Stats.lifeRegenMult)
				.Additive(ModdedPlayer.Stats.lifeRegenMult)
				.LevelScaling(0.1f)
				.RarityScaling(0.2f)
				.PercentFormatting();

			new ItemStatBuilder(Stat.DAMAGE_REDUCTION, "Reduced damage taken", 0.03f, 0.07f) // needs tweaking
				.AffectsStat(ModdedPlayer.Stats.allDamageTaken)
				.OneMinusMultiplier(ModdedPlayer.Stats.allDamageTaken)
				.LevelScaling(0.1f)
				.RarityScaling(0.33f)
				.PercentFormatting();

			new ItemStatBuilder(Stat.CRIT_CHANCE, "Critical hit chance", 0.035f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.critChance)
				.Additive(ModdedPlayer.Stats.critChance)    // change to OneMinusMultiplier if you dont want players reaching 100% crit chance
				.LevelScaling(0.1f)
				.RarityScaling(0.1f)
				.PercentFormatting()
				.RoundTo(2);

			new ItemStatBuilder(Stat.CRIT_DAMAGE, "Critical hit damage", 0.30f, 0.50f)
				.AffectsStat(ModdedPlayer.Stats.critDamage)
				.Additive(ModdedPlayer.Stats.critDamage)
				.LevelScaling(0.1f)
				.RarityScaling(0.1f)
				.PercentFormatting();

			new ItemStatBuilder(Stat.LIFE_ON_HIT, "Life on hit", 0.30f, 0.50f)
				.AffectsStat(ModdedPlayer.Stats.lifeOnHit)
				.Additive(ModdedPlayer.Stats.lifeOnHit)
				.LevelScaling(0.5f)
				.RarityScaling(0.6666f);

			new ItemStatBuilder(Stat.DODGE_CHANCE, "Dodge chance", 0.035f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.getHitChance)
				.OneMinusMultiplier(ModdedPlayer.Stats.getHitChance)
				.LevelScaling(0.1f)
				.RarityScaling(0.1f)
				.PercentFormatting()
				.RoundTo(2);

			new ItemStatBuilder(Stat.ARMOR, "Armor", 5f, 10f)
				.AffectsStat(ModdedPlayer.Stats.armor)
				.Additive(ModdedPlayer.Stats.armor)
				.LevelScaling(1.4f)
				.RarityScaling(1.1f);

			new ItemStatBuilder(Stat.ELITE_DAMAGE_REDUCTION, "Reduced damage taken from elites", 0.04f, 0.09f)
				.AffectsStat(ModdedPlayer.Stats.damageFromElites)
				.OneMinusMultiplier(ModdedPlayer.Stats.damageFromElites)
				.LevelScaling(0.05f)
				.RarityScaling(0.2f)
				.RoundTo(2)
				.PercentFormatting()
				.Cap(0.5f);

			new ItemStatBuilder(Stat.MAX_LIFE, "Attack speed", 0.025f, 0.05f)
				.AffectsStat(ModdedPlayer.Stats.attackSpeed)
				.Additive(ModdedPlayer.Stats.attackSpeed)
				.LevelScaling(0.05f)
				.RarityScaling(0.2f)
				.RoundTo(2)
				.PercentFormatting()
				.Cap(0.5f);

			new ItemStatBuilder(Stat.EXP_GAIN_MULT, "Increased experience gained", 0.025f, 0.05f)
				.AffectsStat(ModdedPlayer.Stats.attackSpeed)
				.Additive(ModdedPlayer.Stats.attackSpeed)
				.LevelScaling(0.1f)
				.RarityScaling(0.3f)
				.RoundTo(2)
				.PercentFormatting()
				.Cap(0.5f);

			new ItemStatBuilder(Stat.MASSACRE_DURATION, "Increased massacre duration", 1, 2)
				.AffectsStat(ModdedPlayer.Stats.maxMassacreTime)
				.Additive(ModdedPlayer.Stats.maxMassacreTime)
				.LevelScaling(0.1f)
				.RoundTo(2)
				.Cap(0.5f);

			new ItemStatBuilder(Stat.SPELL_DAMAGE_MULTIPLIER, "Increased spell damage", 0.03f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.spellDamageMult)
				.Additive(ModdedPlayer.Stats.spellDamageMult)
				.LevelScaling(0.1f)
				.PercentFormatting()
				.RoundTo(2)
				.Cap(0.5f);

			new ItemStatBuilder(Stat.MELEE_DAMAGE_INCREASE, "Increased melee damage", 0.03f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.meleeIncreasedDmg)
				.Additive(ModdedPlayer.Stats.meleeIncreasedDmg)
				.LevelScaling(0.1f)
				.PercentFormatting()
				.RoundTo(2)
				.Cap(0.5f);

			new ItemStatBuilder(Stat.RANGED_DAMAGE_INCREASE, "Increased ranged damage", 0.03f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.rangedIncreasedDmg)
				.Additive(ModdedPlayer.Stats.rangedIncreasedDmg)
				.LevelScaling(0.1f)
				.PercentFormatting()
				.RoundTo(2)
				.Cap(0.5f);


			new ItemStatBuilder(Stat.BASE_MELEE_DAMAGE, "Bonus melee damage", 1.5f, 3f)
				.AffectsStat(ModdedPlayer.Stats.baseMeleeDamage)
				.Additive(ModdedPlayer.Stats.baseMeleeDamage)
				.LevelScaling(0.6f);

			new ItemStatBuilder(Stat.BASE_SPELL_DAMAGE, "Bonus spell damage", 1.5f, 3f)
				.AffectsStat(ModdedPlayer.Stats.baseSpellDamage)
				.Additive(ModdedPlayer.Stats.baseSpellDamage)
				.LevelScaling(0.6f);

			new ItemStatBuilder(Stat.BASE_RANGED_DAMAGE, "Bonus ranged damage", 1.5f, 3f)
				.AffectsStat(ModdedPlayer.Stats.baseRangedDamage)
				.Additive(ModdedPlayer.Stats.baseMeleeDamage)
				.LevelScaling(0.6f);

			new ItemStatBuilder(Stat.MAX_ENERGY_FROM_AGILITY, "Each point of agility increases max energy", 0.0125f, 0.025f)
				.AffectsStat(ModdedPlayer.Stats.maxEnergyFromAgi)
				.Additive(ModdedPlayer.Stats.maxEnergyFromAgi)
				.RarityScaling(0.5f)
				.NoLevelScaling();

			new ItemStatBuilder(Stat.MAX_HEALTH_FROM_VITALITY, "Each point of vitality increases max health", 0.075f, 0.25f)
				.AffectsStat(ModdedPlayer.Stats.maxHealthFromVit)
				.Additive(ModdedPlayer.Stats.maxHealthFromVit)
				.RarityScaling(0.5f)
				.NoLevelScaling();

			new ItemStatBuilder(Stat.SPELL_DMG_FROM_INT, "Each point of intelligence increases spell damage", 0.004f, 0.008f)
				.AffectsStat(ModdedPlayer.Stats.spellDmgFromInt)
				.Additive(ModdedPlayer.Stats.spellDmgFromInt)
				.RarityScaling(0.5f)
				.NoLevelScaling();

			new ItemStatBuilder(Stat.MELEE_DMG_FROM_STR, "Each point of strength increases melee damage", 0.001f, 0.0018f)
				.AffectsStat(ModdedPlayer.Stats.meleeDmgFromStr)
				.Additive(ModdedPlayer.Stats.meleeDmgFromStr)
				.RarityScaling(0.5f)
				.NoLevelScaling();

			new ItemStatBuilder(Stat.RANGED_DMG_FROM_AGI, "Each point of agility increases ranged damage", 0.001f, 0.0018f)
				.AffectsStat(ModdedPlayer.Stats.rangedDmgFromAgi)
				.Additive(ModdedPlayer.Stats.rangedDmgFromAgi)
				.RarityScaling(0.5f)
				.NoLevelScaling();

			new ItemStatBuilder(Stat.ALL_RECOVERY, "Energy and health recovery", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.allRecoveryMult)
				.Additive(ModdedPlayer.Stats.allRecoveryMult)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.MOVEMENT_SPEED, "Movement speed", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.mOVEMENT_SPEED)
				.Additive(ModdedPlayer.Stats.mOVEMENT_SPEED)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.JUMP_POWER, "Jump power", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.mOVEMENT_SPEED)
				.Additive(ModdedPlayer.Stats.mOVEMENT_SPEED)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.MELEE_WEAPON_RANGE, "Melee weapon range", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.weaponRange)
				.MultiplyPlusOne(ModdedPlayer.Stats.weaponRange)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.ATTACK_COST_REDUCTION, "Reduced stamina cost of attacks", 0.045f, 0.09f)
				.AffectsStat(ModdedPlayer.Stats.attackStaminaCost)
				.OneMinusMultiplier(ModdedPlayer.Stats.attackStaminaCost)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.SPELL_COST_REDUCTION, "Reduced resource cost of spells", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.spellCost)
				.OneMinusMultiplier(ModdedPlayer.Stats.spellCost)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.SPELL_COST_TO_STAMINA, "Spells partially use stamina instead of energy", 0.6f, 0.13f)
				.AffectsStat(ModdedPlayer.Stats.spellCostEnergyCost)
				.OneMinusMultiplier(ModdedPlayer.Stats.spellCostEnergyCost)
				.PercentFormatting()
				.LevelScaling(0.05f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.ENERGY_REGEN_BASE, "Energy regeneration", 0.06f, 0.13f)
				.AffectsStat(ModdedPlayer.Stats.energyRecoveryBase)
				.Additive(ModdedPlayer.Stats.energyRecoveryBase)
				.RoundTo(2)
				.LevelScaling(0.5f);

			new ItemStatBuilder(Stat.MAX_LIFE_MULT, "Increased life", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.maxLifeMult)
				.Additive(ModdedPlayer.Stats.maxLifeMult)
				.RoundTo(1)
				.LevelScaling(0.05f)
				.PercentFormatting();

			new ItemStatBuilder(Stat.MAX_ENERGY_MULT, "Increased energy", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.maxEnergyMult)
				.Additive(ModdedPlayer.Stats.maxEnergyMult)
				.RoundTo(1)
				.LevelScaling(0.05f)
				.PercentFormatting();

			new ItemStatBuilder(Stat.COOLDOWN_REDUCTION, "Reduced spell cooldown", 0.03f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.cooldown)
				.OneMinusMultiplier(ModdedPlayer.Stats.cooldown)
				.PercentFormatting()
				.LevelScaling(0.1f)
				.RarityScaling(0.2f);

			new ItemStatBuilder(Stat.ENERGY_ON_HIT, "Energy on hit", 0.1f, 0.2f)
				.AffectsStat(ModdedPlayer.Stats.energyOnHit)
				.Additive(ModdedPlayer.Stats.energyOnHit)
				.RoundTo(2)
				.LevelScaling(0.5f);

			new ItemStatBuilder(Stat.BLOCK, "Block", 1, 1.7f)
				.AffectsStat(ModdedPlayer.Stats.block)
				.Additive(ModdedPlayer.Stats.block);

			new ItemStatBuilder(Stat.PROJECTILE_SPEED, "Projectile speed", 0.02f, 0.05f)
				.AffectsStat(ModdedPlayer.Stats.projectileSpeed)
				.Additive(ModdedPlayer.Stats.projectileSpeed)
				.PercentFormatting()
				.LevelScaling(0.25f);

			new ItemStatBuilder(Stat.PROJECTILE_SIZE, "Projectile size", 0.03f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.projectileSize)
				.Additive(ModdedPlayer.Stats.projectileSize)
				.PercentFormatting()
				.LevelScaling(0.25f);

			new ItemStatBuilder(Stat.PROJECTILE_PIERCE_CHANCE, "Chance for projectiles to pierce", 0.16f, 0.36f)
				.AffectsStat(ModdedPlayer.Stats.projectilePierceChance)
				.Additive(ModdedPlayer.Stats.projectilePierceChance)
				.PercentFormatting()
				.LevelScaling(0.075f);

			new ItemStatBuilder(Stat.HEADSHOT_DAMAGE_MULT, "Headshot damage", 0.1f, 0.26f)
				.AffectsStat(ModdedPlayer.Stats.headShotDamage)
				.Additive(ModdedPlayer.Stats.headShotDamage)
				.PercentFormatting()
				.LevelScaling(0.35f);

			new ItemStatBuilder(Stat.MELEE_ARMOR_PIERCING, "Armor damage on melee hit", 1.5f, 3f)
				.AffectsStat(ModdedPlayer.Stats.meleeArmorPiercing)
				.Additive(ModdedPlayer.Stats.block)
				.LevelScaling(1.1f)
				.RarityScaling(1.5f);

			new ItemStatBuilder(Stat.RANGED_ARMOR_PIERCING, "Armor damage on ranged hit", 0.6f, 1.2f)
				.AffectsStat(ModdedPlayer.Stats.rangedArmorPiercing)
				.Additive(ModdedPlayer.Stats.block)
				.LevelScaling(1.1f)
				.RarityScaling(1.5f);

			new ItemStatBuilder(Stat.ARMOR_PIERCING, "Armor damage on hit", 0.5f, 1f)
				.AffectsStat(ModdedPlayer.Stats.allArmorPiercing)
				.Additive(ModdedPlayer.Stats.block)
				.LevelScaling(1.1f)
				.RarityScaling(1.5f);

			new ItemStatBuilder(Stat.PROJECTILE_SPEED, "Increased quantity of loot", 0.01f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.magicFind_quantity)
				.Additive(ModdedPlayer.Stats.magicFind_quantity)
				.PercentFormatting()
				.LevelScaling(0.15f);

			new ItemStatBuilder(Stat.PROJECTILE_SPEED, "Increased quality of loot", 0.01f, 0.06f)
				.AffectsStat(ModdedPlayer.Stats.projectileSpeed)
				.Additive(ModdedPlayer.Stats.projectileSpeed)
				.PercentFormatting()
				.LevelScaling(0.15f);

			new ItemStatBuilder(Stat.FIRE_DAMAGE, "Fire damage", 0.035f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.fireDamage)
				.Additive(ModdedPlayer.Stats.fireDamage)
				.PercentFormatting()
				.LevelScaling(0.25f)
				.RarityScaling(0.5f);


			new ItemStatBuilder(Stat.CHANCE_ON_HIT_TO_BLEED, "Chance to cause bleeding on hit", 0.01f, 0.03f)
				.AffectsStat(ModdedPlayer.Stats.chanceToBleed)
				.Additive(ModdedPlayer.Stats.chanceToBleed)
				.PercentFormatting()
				.LevelScaling(0.2f)
				.RarityScaling(0.5f);

			new ItemStatBuilder(Stat.CHANCE_ON_HIT_TO_SLOW, "Chance to slow enemies on hit", 0.02f, 0.04f)
				.AffectsStat(ModdedPlayer.Stats.chanceToSlow)
				.Additive(ModdedPlayer.Stats.chanceToSlow)
				.PercentFormatting()
				.LevelScaling(0.2f)
				.RarityScaling(0.5f);

			new ItemStatBuilder(Stat.CHANCE_ON_HIT_TO_WEAKEN, "Chance to weaken enemies on hit", 0.02f, 0.04f)
				.AffectsStat(ModdedPlayer.Stats.chanceToWeaken)
				.Additive(ModdedPlayer.Stats.chanceToWeaken)
				.PercentFormatting()
				.LevelScaling(0.2f)
				.RarityScaling(0.5f);

			new ItemStatBuilder(Stat.THORNS, "Thorns", 1.5f, 2f)
				.AffectsStat(ModdedPlayer.Stats.thorns)
				.Additive(ModdedPlayer.Stats.thorns)
				.LevelScaling(1.2f)
				.RarityScaling(1.5f);

			new ItemStatBuilder(Stat.THORNS_MULT, "Thorns Damage", 0.05f, 0.07f)
				.AffectsStat(ModdedPlayer.Stats.thornsDmgMult)
				.MultiplyPlusOne(ModdedPlayer.Stats.thornsDmgMult)
				.PercentFormatting()
				.LevelScaling(0.5f)
				.RarityScaling(1f);

			new ItemStatBuilder(Stat.EXPLOSION_DAMAGE, "Explosion damage", 0.15f, 0.5f)
				.AffectsStat(ModdedPlayer.Stats.explosionDamage)
				.Additive(ModdedPlayer.Stats.explosionDamage)
				.LevelScaling(0.3f)
				.RoundTo(2)
				.PercentFormatting();

			new ItemStatBuilder(Stat.THROWN_SPEAR_DAMAGE, "Increased spear throw damage", 0.25f, 0.5f)
				.AffectsStat(ModdedPlayer.Stats.perk_thrownSpearDamageMult)
				.MultiplyPlusOne(ModdedPlayer.Stats.perk_thrownSpearDamageMult)
				.LevelScaling(0.16f)
				.RoundTo(2)
				.PercentFormatting();



			new ItemStatBuilder(Stat.THROWN_SPEAR_DAMAGE, "Increased spear throw damage", 0.25f, 0.5f)
				.AffectsStat(ModdedPlayer.Stats.perk_thrownSpearDamageMult)
				.MultiplyPlusOne(ModdedPlayer.Stats.perk_thrownSpearDamageMult)
				.NoLevelScaling()
				.RoundTo(0);


			//Extra carry items
			new ItemStatBuilder(Stat.EXTRA_CARRIED_STICKS, "More carried sticks", 5, 15)
				.AffectsCarryCapacity(MoreCraftingReceipes.VanillaItemIDs.STICK)
				.NoLevelScaling()
				.RoundTo(0);

			new ItemStatBuilder(Stat.EXTRA_CARRIED_ROCKS, "More carried rocks", 5, 15)
				.AffectsCarryCapacity(MoreCraftingReceipes.VanillaItemIDs.ROCK)
				.NoLevelScaling()
				.RoundTo(0);

			new ItemStatBuilder(Stat.EXTRA_CARRIED_ROPES, "More carried ropes", 5, 15)
				.AffectsCarryCapacity(MoreCraftingReceipes.VanillaItemIDs.ROPE)
				.NoLevelScaling()
				.RoundTo(0);

			new ItemStatBuilder(Stat.EXTRA_CARRIED_DRINKS, "More carried booze and soda", 15, 30)
				.AffectsCarryCapacity(
					MoreCraftingReceipes.VanillaItemIDs.BOOZE, 
					MoreCraftingReceipes.VanillaItemIDs.SODA)
				.NoLevelScaling()
				.RoundTo(0);

			new ItemStatBuilder(Stat.EXTRA_CARRIED_FOOD, "More carried food", 10, 20)
				.AffectsCarryCapacity(
					MoreCraftingReceipes.VanillaItemIDs.GENERICMEAT,
					MoreCraftingReceipes.VanillaItemIDs.SMALLGENERICMEAT,
					MoreCraftingReceipes.VanillaItemIDs.BLACKBERRY,
					MoreCraftingReceipes.VanillaItemIDs.BLUEBERRY,
					MoreCraftingReceipes.VanillaItemIDs.CHOCOLATEBAR,
					MoreCraftingReceipes.VanillaItemIDs.LIZARD,
					MoreCraftingReceipes.VanillaItemIDs.RABBITDEAD)
				.NoLevelScaling()
				.RoundTo(0);


			// Black hole stats
			new ItemStatBuilder(Stat.BLACK_HOLE_DAMAGE, "Bonus black hole damage", 3f, 12f)
				.AffectsStat(ModdedPlayer.Stats.spell_blackhole_damage)
				.Additive(ModdedPlayer.Stats.spell_blackhole_damage)
				.LevelScaling(0.6f);

			new ItemStatBuilder(Stat.BLACK_HOLE_GRAVITY_FORCE, "Black hole gravitational force", 10f, 30f)
				.AffectsStat(ModdedPlayer.Stats.spell_blackhole_pullforce)
				.Additive(ModdedPlayer.Stats.spell_blackhole_pullforce)
				.NoLevelScaling()
				.NoRarityScaling();

			new ItemStatBuilder(Stat.BLACKHOLE_SIZE, "Black hole radius", 5f, 20f)
				.AffectsStat(ModdedPlayer.Stats.spell_blackhole_radius)
				.Additive(ModdedPlayer.Stats.spell_blackhole_radius)
				.NoLevelScaling()
				.NoRarityScaling();

			new ItemStatBuilder(Stat.BLACK_HOLE_DURATION, "Black hole duration", 5f, 20f)
				.AffectsStat(ModdedPlayer.Stats.spell_blackhole_duration)
				.Additive(ModdedPlayer.Stats.spell_blackhole_duration)
				.NoLevelScaling()
				.NoRarityScaling();



			new ItemStatBuilder(Stat.HAMMER_STUN_ON_HIT, "Stuns enemies on hit", 1, 1)
				.Additive(ModdedPlayer.Stats.i_HammerStun)
				.NoLevelScaling()
				.NoRarityScaling()
				.NoRarityScaling()
				.NoLevelScaling()
				.GetTotalStat = () => "";

			new ItemStatBuilder(Stat.SNAP_FREEZE_DURATION, "Snap freeze duration", 1f, 5f)
				.AffectsStat(ModdedPlayer.Stats.spell_snapFreezeDuration)
				.Additive(ModdedPlayer.Stats.spell_snapFreezeDuration)
				.NoLevelScaling()
				.NoRarityScaling();

			new ItemStatBuilder(Stat.RAFT_SPEED, "Global raft speed", 0.5f, 4f)
				.AffectsStat(ModdedPlayer.Stats.perk_RaftSpeedMultipier)
				.Additive(ModdedPlayer.Stats.perk_RaftSpeedMultipier)
				.PercentFormatting()
				.RoundTo(2)
				.NoLevelScaling()
				.NoRarityScaling();

		}
	}
}