using UnityEngine;

namespace ChampionsOfForest.Player
{
	public partial class ModdedPlayer
	{
		public class ModdedPlayerStats
		{
			public const float baseHealth = 50f;
			public const float baseEnergy = 50f;
			public const float baseStaminaRecovery = 4f;

			public readonly AdditivePlayerStat<int> strength;
			public readonly AdditivePlayerStat<int> intelligence;
			public readonly AdditivePlayerStat<int> agility;
			public readonly AdditivePlayerStat<int> vitality;
			public readonly MultiOperationPlayerStat<float> maxEnergyMult;
			public readonly AdditivePlayerStat<int> maxEnergy;
			public readonly MultiOperationPlayerStat<float> maxHealthMult;
			public readonly AdditivePlayerStat<int> maxHealth;
			public readonly AdditivePlayerStat<float> meleeDmgFromStr;
			public readonly AdditivePlayerStat<float> spellDmgFromInt;
			public readonly AdditivePlayerStat<float> rangedDmgFromAgi;
			public readonly AdditivePlayerStat<float> energyRecoveryFromInt;
			public readonly AdditivePlayerStat<float> maxEnergyFromAgi;
			public readonly AdditivePlayerStat<float> maxHealthFromVit;
			public readonly AdditivePlayerStat<float> fireDamage;
			public readonly MultiOperationPlayerStat<float> healthPerSecRate;
			public readonly MultiOperationPlayerStat<float> staminaPerSecRate;
			public readonly MultiplicativePlayerStat<float> cooldown;
			public readonly MultiOperationPlayerStat<float> allDamage;
			public readonly MultiOperationPlayerStat<float> attackSpeed;
			public readonly MultiOperationPlayerStat<float> movementSpeed;
			public readonly AdditivePlayerStat<float> critChance;
			public readonly AdditivePlayerStat<float> critDamage;

			public readonly AdditivePlayerStat<float> rangedFlatDmg;
			public readonly AdditivePlayerStat<float> meleeFlatDmg;
			public readonly AdditivePlayerStat<float> spellFlatDmg;
			public readonly MultiOperationPlayerStat<float> rangedIncreasedDmg;
			public readonly MultiOperationPlayerStat<float> meleeIncreasedDmg;
			public readonly MultiOperationPlayerStat<float> spellIncreasedDmg;
			public readonly AdditivePlayerStat<int> meleeArmorPiercing;
			public readonly AdditivePlayerStat<int> rangedArmorPiercing;
			public readonly AdditivePlayerStat<float> thornsArmorPiercing;
			public readonly AdditivePlayerStat<int> allArmorPiercing;
			public readonly AdditivePlayerStat<float> chanceToSlow;
			public readonly AdditivePlayerStat<float> chanceToBleed;
			public readonly AdditivePlayerStat<float> chanceToWeaken;
			public readonly AdditivePlayerStat<float> areaDamageChance;
			public readonly AdditivePlayerStat<float> areaDamage;
			public readonly AdditivePlayerStat<float> areaDamageRadius;
			public readonly MultiOperationPlayerStat<float> projectileSpeed;
			public readonly MultiOperationPlayerStat<float> projectileSize;
			public readonly MultiOperationPlayerStat<float> projectilePierceChance;
			public readonly MultiOperationPlayerStat<float> heavyAttackDmg;

			public readonly MultiplicativePlayerStat<float> weaponRange;
			public readonly MultiplicativePlayerStat<float> headShotDamage;

			public readonly MultiOperationPlayerStat<float> allRecoveryMult;
			public readonly AdditivePlayerStat<float> healthOnHit;
			public readonly AdditivePlayerStat<float> staminaOnHit;
			public readonly AdditivePlayerStat<float> energyOnHit;
			public readonly AdditivePlayerStat<float> healthRecoveryPerSecond;
			public readonly AdditivePlayerStat<float> staminaRecoveryperSecond;
			public readonly AdditivePlayerStat<float> energyRecoveryperSecond;

			public readonly MultiplicativePlayerStat<float> allDamageTaken;
			public readonly MultiplicativePlayerStat<float> magicDamageTaken;
			public readonly MultiplicativePlayerStat<float> fireDamageTaken;
			public readonly MultiplicativePlayerStat<float> getHitChance;
			public readonly AdditivePlayerStat<int> armor;
			public readonly MultiplicativePlayerStat<float> thornsDmgMult;
			public readonly AdditivePlayerStat<float> thorns;
			public readonly AdditivePlayerStat<float> thornsPerStrenght;
			public readonly AdditivePlayerStat<float> thornsPerVit;
			public readonly AdditivePlayerStat<int> stunImmunity;
			public readonly AdditivePlayerStat<int> rootImmunity;
			public readonly AdditivePlayerStat<int> debuffImmunity;
			public readonly AdditivePlayerStat<int> debuffResistance;


			public readonly AdditivePlayerStat<float> jumpPower;
			public readonly MultiplicativePlayerStat<float> spellCostEnergyCost;
			public readonly MultiplicativePlayerStat<float> spellCost;
			public readonly MultiplicativePlayerStat<float> attackStaminaCost;
			public readonly AdditivePlayerStat<float> block;
			public readonly AdditivePlayerStat<float> expGain;
			public readonly AdditivePlayerStat<float> maxMassacreTime;
			public readonly AdditivePlayerStat<float> timeBonusPerKill;
			public readonly AdditivePlayerStat<int> MaxLogs;


			public readonly BooleanPlayerStat silenced;
			public readonly BooleanPlayerStat rooted;
			public readonly BooleanPlayerStat stunned;
			public readonly AdditiveNetworkSyncedPlayerStat<float> magicFind;
			public readonly AdditiveNetworkSyncedPlayerStat<float> explosionDamage;
			public readonly AdditiveNetworkSyncedPlayerStat<float> fireTickRate;
			public readonly AdditiveNetworkSyncedPlayerStat<float> fireDuration;

			//spells

			//blink
			public readonly AdditivePlayerStat<float> spell_fireboltDamageScaling;
			public readonly AdditivePlayerStat<float> spell_fireboltEnergyCost;
			public readonly AdditivePlayerStat<float> spell_blinkRange;
			public readonly AdditivePlayerStat<float> spell_blinkDamageScaling;
			public readonly AdditivePlayerStat<float> spell_blinkDamage;
			public readonly BooleanPlayerStat spell_blinkDoExplosion;
			//parry
			public readonly BooleanPlayerStat spell_parry;
			public readonly AdditivePlayerStat<float> spell_parryDamage;
			public readonly AdditivePlayerStat<float> spell_parryAttackSpeed;
			public readonly AdditivePlayerStat<float> spell_parryRadius;
			public readonly AdditivePlayerStat<float> spell_parryBuffDuration;
			public readonly AdditivePlayerStat<float> spell_parryHeal;
			public readonly AdditivePlayerStat<float> spell_parryEnergy;
			public readonly BooleanPlayerStat spell_chanceToParryOnHit;
			public readonly BooleanPlayerStat spell_parryIgnites;
			public readonly AdditivePlayerStat<float> spell_parryDmgBonus;
			public readonly AdditivePlayerStat<float> spell_parryBuffDamage;
			//healing dome
			public readonly BooleanPlayerStat spell_healingDomeGivesImmunity;
			public readonly BooleanPlayerStat spell_healingDomeRegEnergy;
			public readonly AdditivePlayerStat<float> spell_healingDomeDuration;
			//flare 	
			public readonly AdditivePlayerStat<float> spell_flareDamage;
			public readonly AdditivePlayerStat<float> spell_flareDamageScaling;
			public readonly AdditivePlayerStat<float> spell_flareSlow;
			public readonly AdditivePlayerStat<float> spell_flareBoost;
			public readonly AdditivePlayerStat<float> spell_flareHeal;
			public readonly AdditivePlayerStat<float> spell_flareRadius;
			public readonly AdditivePlayerStat<float> spell_flareDuration;
			//black hole 
			public readonly AdditivePlayerStat<float> spell_blackhole_damage;
			public readonly AdditivePlayerStat<float> spell_blackhole_damageScaling;
			public readonly AdditivePlayerStat<float> spell_blackhole_duration;
			public readonly AdditivePlayerStat<float> spell_blackhole_radius;
			public readonly AdditivePlayerStat<float> spell_blackhole_pullforce;
			//sustain shield
			public readonly AdditivePlayerStat<float> spell_shieldPerSecond;
			public readonly AdditivePlayerStat<float> spell_shieldMax;
			public readonly AdditivePlayerStat<float> spell_shieldPersistanceLifetime;
			//portal
			public readonly AdditivePlayerStat<float> spell_portalDuration;
			//warcry
			public readonly AdditivePlayerStat<float> spell_warCryRadius;
			public readonly AdditivePlayerStat<float> spell_warCryAtkSpeed;
			public readonly AdditivePlayerStat<float> spell_warCryDamage;
			public readonly BooleanPlayerStat spell_warCryGiveDamage;
			public readonly BooleanPlayerStat spell_warCryGiveArmor;
			//magic arrow
			public readonly BooleanPlayerStat spell_magicArrowDmgDebuff;
			public readonly BooleanPlayerStat spell_magicArrowCrit;
			public readonly BooleanPlayerStat spell_magicArrowDoubleSlow;
			public readonly AdditivePlayerStat<int> spell_magicArrowVolleyCount;
			public readonly AdditivePlayerStat<float> spell_magicArrowDuration;
			public readonly AdditivePlayerStat<float> spell_magicArrowDamageScaling;
			//purge
			public readonly AdditivePlayerStat<float> spell_blackFlameDamageScaling;

			public readonly AdditivePlayerStat<float> spell_purgeRadius;
			public readonly BooleanPlayerStat spell_purgeHeal;
			public readonly BooleanPlayerStat spell_purgeDamageBonus;
			//snap freeze
			public readonly AdditivePlayerStat<float> spell_snapFreezeDist;
			public readonly AdditivePlayerStat<float> spell_snapFloatAmount;
			public readonly AdditivePlayerStat<float> spell_snapDamageScaling;
			public readonly AdditivePlayerStat<float> spell_snapFreezeDuration;
			//ball lightning
			public readonly AdditivePlayerStat<float> spell_ballLightning_Damage;
			public readonly AdditivePlayerStat<float> spell_ballLightning_DamageScaling;
			public readonly BooleanPlayerStat spell_ballLightning_Crit;
			//bash
			public readonly AdditivePlayerStat<float> spell_bashDamageDebuffAmount;
			public readonly AdditivePlayerStat<float> spell_bashDamageBuff;
			public readonly AdditivePlayerStat<float> spell_bashSlowAmount;
			public readonly AdditivePlayerStat<float> spell_bashLifesteal;
			public readonly BooleanPlayerStat spell_bashEnabled;
			public readonly AdditivePlayerStat<float> spell_bashBleedChance;
			public readonly AdditivePlayerStat<float> spell_bashBleedDmg;
			public readonly AdditivePlayerStat<float> spell_bashDuration;
			//frenzy
			public readonly AdditivePlayerStat<int> spell_frenzyMaxStacks;
			public readonly AdditivePlayerStat<int> spell_frenzyStacks;
			public readonly AdditivePlayerStat<float> spell_frenzyAtkSpeed;
			public readonly AdditivePlayerStat<float> spell_frenzyDmg;
			public readonly BooleanPlayerStat spell_frenzy;
			public readonly BooleanPlayerStat spell_frenzyMS;
			public readonly BooleanPlayerStat spell_furySwipes;
			//focus
			public readonly AdditivePlayerStat<float> spell_focusBonusDmg;
			public readonly AdditivePlayerStat<float> spell_focusOnHS;
			public readonly AdditivePlayerStat<float> spell_focusOnBS;
			public readonly AdditivePlayerStat<float> spell_focusOnAtkSpeed;
			public readonly AdditivePlayerStat<float> spell_focusOnAtkSpeedDuration;
			public readonly AdditivePlayerStat<float> spell_focusSlowAmount;
			public readonly AdditivePlayerStat<float> spell_focusSlowDuration;
			public readonly BooleanPlayerStat spell_focus;
			//seeking arrow
			public readonly AdditivePlayerStat<float> spell_seekingArrow_HeadDamage;
			public readonly AdditivePlayerStat<float> spell_seekingArrow_DamageBonus;
			public readonly AdditivePlayerStat<float> spell_seekingArrow_SlowDuration;
			public readonly AdditivePlayerStat<float> spell_seekingArrow_SlowAmount;
			public readonly AdditivePlayerStat<float> projectile_DamagePerDistance;
			public readonly AdditivePlayerStat<float> spell_seekingArrowDuration;
			public readonly BooleanPlayerStat spell_seekingArrow;
			//cataclysm			  
			public readonly AdditivePlayerStat<float> spell_cataclysmDamage;
			public readonly AdditivePlayerStat<float> spell_cataclysmDamageScaling;
			public readonly AdditivePlayerStat<float> spell_cataclysmDuration;
			public readonly AdditivePlayerStat<float> spell_cataclysmRadius;
			public readonly BooleanPlayerStat spell_cataclysmArcane;
			//blood infused arrow
			public readonly AdditivePlayerStat<float> spell_bia_SpellDmMult;
			public readonly AdditivePlayerStat<float> spell_bia_HealthDmMult;
			public readonly AdditivePlayerStat<float> spell_bia_HealthTaken;
			public readonly AdditivePlayerStat<float> spell_bia_AccumulatedDamage;
			public readonly BooleanPlayerStat spell_bia_TripleDmg;
			public readonly BooleanPlayerStat spell_bia_Weaken;
			//roaring cheeks
			public readonly AdditivePlayerStat<float> spell_fartRadius;
			public readonly AdditivePlayerStat<float> spell_fartKnockback;
			public readonly AdditivePlayerStat<float> spell_fartSlow;
			public readonly AdditivePlayerStat<float> spell_fartDebuffDuration;
			public readonly AdditivePlayerStat<float> spell_fartBaseDmg;
			//taunt
			public readonly MultiplicativePlayerStat<float> spell_taunt_speedChange;
			public readonly BooleanPlayerStat spell_taunt_pullEnemiesIn;
			//snow storm
			public readonly AdditivePlayerStat<float> spell_snowstormMaxCharge;
			public readonly AdditivePlayerStat<float> spell_snowstormDamageMult;
			public readonly MultiplicativePlayerStat<float> spell_snowstormHitDelay;
			public readonly BooleanPlayerStat spell_snowstormPullEnemiesIn;

			public readonly AdditivePlayerStat<float> spell_berserkDuration;


			//perks
			public readonly BooleanPlayerStat perk_fireDmgIncreaseOnHit;
			public readonly AdditivePlayerStat<float> perk_parryCounterStrikeDamage;
			public readonly AdditiveNetworkSyncedPlayerStat<int> perk_turboRaftOwners;
			public readonly AdditiveNetworkSyncedPlayerStat<float> perk_RaftSpeedMultipier;
			public readonly MultiplicativePlayerStat<float> perk_thirstRate;
			public readonly MultiplicativePlayerStat<float> perk_hungerRate;
			public readonly AdditivePlayerStat<float> perk_projectileNoConsumeChance;
			public readonly MultiplicativePlayerStat<float> perk_thrownSpearDamageMult;
			public readonly BooleanPlayerStat perk_thrownSpearExtraArmorReduction;

			public readonly AdditivePlayerStat<float> perk_thrownSpearCritChance;
			public readonly AdditivePlayerStat<float> perk_thrownSpearhellChance;
			public readonly AdditivePlayerStat<float> perk_bulletCritChance;
			public readonly MultiplicativePlayerStat<float> perk_bulletDamageMult;
			public readonly MultiplicativePlayerStat<float> perk_crossbowDamageMult;
			public readonly MultiplicativePlayerStat<float> perk_bowDamageMult;
			public readonly AdditivePlayerStat<int> perk_multishotProjectileCount;
			public readonly MultiplicativePlayerStat<float> perk_multishotDamagePennalty;
			public readonly BooleanPlayerStat perk_nearDeathExperienceTriggered;
			public readonly BooleanPlayerStat perk_nearDeathExperienceUnlocked;
			public readonly BooleanPlayerStat perk_craftingReroll;
			public readonly BooleanPlayerStat perk_craftingReforge;
			public readonly BooleanPlayerStat perk_craftingPolishing;
			public readonly BooleanPlayerStat perk_craftingEmpowering;
			public readonly BooleanPlayerStat perk_craftingRerollingSingleStat;
			public readonly BooleanPlayerStat perk_isShieldAutocast;
			public readonly BooleanPlayerStat perk_parryAnything;
			public readonly BooleanPlayerStat perk_blackholePullImmune;
			public readonly BooleanPlayerStat perk_blizzardSlowReduced;
			public readonly BooleanPlayerStat perk_trueAim;
			public readonly BooleanPlayerStat perk_trueAimUpgrade;
			public readonly BooleanPlayerStat perk_goldenResolve;
			public readonly BooleanPlayerStat perk_projectileDamageIncreasedBySize;
			public readonly BooleanPlayerStat perk_projectileDamageIncreasedBySpeed;
			public readonly BooleanPlayerStat perk_infinityMagic;
			public readonly AdditivePlayerStat<float> perk_flashlightIntensity;
			public readonly AdditivePlayerStat<float> perk_flashlightBatteryDrain;
			public readonly BooleanPlayerStat perk_bunnyHop;
			public readonly BooleanPlayerStat perk_bunnyHopUpgrade;
			public readonly BooleanPlayerStat perk_danceOfFiregod;
			public readonly BooleanPlayerStat perk_danceOfFiregodAtkCap;
			public readonly BooleanPlayerStat perk_doubleStickHarvesting;
			public readonly BooleanPlayerStat perk_chargedAtkKnockback;

			//items
			public readonly BooleanPlayerStat i_greatBowIgnites;
			public readonly BooleanPlayerStat i_SmokeyCrossbowQuiver;
			public readonly BooleanPlayerStat i_CrossfireQuiver;
			public readonly BooleanPlayerStat i_HazardCrown;
			public readonly AdditivePlayerStat<int> i_HazardCrownBonus;
			public readonly BooleanPlayerStat i_HammerStun;
			public readonly MultiplicativePlayerStat<float> i_HammerStunDuration;
			public readonly MultiplicativePlayerStat<float> i_HammerStunAmount;
			public readonly MultiplicativePlayerStat<float> smashDamage;
			public readonly BooleanPlayerStat i_HexedPantsOfMrM_Enabled;
			public readonly BooleanPlayerStat i_DeathPact_Enabled;
			public readonly BooleanPlayerStat i_EruptionBow;
			public readonly BooleanPlayerStat i_ArchangelBow;
			public readonly BooleanPlayerStat i_isGreed;
			public readonly BooleanPlayerStat i_KingQruiesSword;
			public readonly BooleanPlayerStat i_SoraBracers;
			public readonly BooleanPlayerStat i_isWindArmor;
			public readonly BooleanPlayerStat i_sparkOfLightAfterDark;
			public readonly BooleanPlayerStat i_infinityLoop;
			public readonly AdditivePlayerStat<int> i_setcount_AkagisSet;
			public readonly AdditivePlayerStat<int> i_setcount_BerserkSet;

			public ModdedPlayerStats()
			{


				this.strength = new AdditivePlayerStat<int>(1, addint, substractint);
				this.intelligence = new AdditivePlayerStat<int>(1, addint, substractint);
				this.agility = new AdditivePlayerStat<int>(1, addint, substractint);
				this.vitality = new AdditivePlayerStat<int>(1, addint, substractint);
				this.maxEnergyMult = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.maxEnergy = new AdditivePlayerStat<int>(0, addint, substractint);
				this.maxHealthMult = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.maxHealth = new AdditivePlayerStat<int>(0, addint, substractint);
				this.meleeDmgFromStr = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.spellDmgFromInt = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.rangedDmgFromAgi = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.energyRecoveryFromInt = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.maxEnergyFromAgi = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.maxHealthFromVit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.fireDamage = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.healthPerSecRate = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.staminaPerSecRate = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.cooldown = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.allDamage = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.attackSpeed = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.movementSpeed = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P1");
				this.critChance = new AdditivePlayerStat<float>(0.05f, addfloat, substractfloat, "P");
				this.critDamage = new AdditivePlayerStat<float>(0.5f, addfloat, substractfloat, "P");

				this.rangedFlatDmg = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.meleeFlatDmg = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.spellFlatDmg = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.rangedIncreasedDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.meleeIncreasedDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.spellIncreasedDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.meleeArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
				this.rangedArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
				this.thornsArmorPiercing = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.allArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
				this.chanceToSlow = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.chanceToBleed = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.chanceToWeaken = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.areaDamageChance = new AdditivePlayerStat<float>(0.10f, addfloat, substractfloat, "P");
				this.areaDamage = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.areaDamageRadius = new AdditivePlayerStat<float>(4.0f, addfloat, substractfloat, "P");
				this.projectileSpeed = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.projectileSize = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.projectilePierceChance = new MultiOperationPlayerStat<float>(0, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.heavyAttackDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");

				this.weaponRange = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.headShotDamage = new MultiplicativePlayerStat<float>(6, multfloat, dividefloat, "P");
				this.projectile_DamagePerDistance = new AdditivePlayerStat<float>(0.00f, addfloat, substractfloat);

				this.allRecoveryMult = new MultiOperationPlayerStat<float>(1,1, addfloat, substractfloat, multfloat, dividefloat, "P");
				this.healthOnHit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.staminaOnHit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.energyOnHit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.healthRecoveryPerSecond = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.staminaRecoveryperSecond = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.energyRecoveryperSecond = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);

				this.allDamageTaken = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.magicDamageTaken = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.fireDamageTaken = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.getHitChance = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.armor = new AdditivePlayerStat<int>(0, addint, substractint);
				this.thornsDmgMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.thorns = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.thornsPerStrenght = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.thornsPerVit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.stunImmunity = new AdditivePlayerStat<int>(0, addint, substractint);
				this.rootImmunity = new AdditivePlayerStat<int>(0, addint, substractint);
				this.debuffImmunity = new AdditivePlayerStat<int>(0, addint, substractint);
				this.debuffResistance = new AdditivePlayerStat<int>(0, addint, substractint);


				this.jumpPower = new AdditivePlayerStat<float>(1.0f, addfloat, substractfloat, "P");
				this.spellCostEnergyCost = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.spellCost = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.attackStaminaCost = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.block = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.expGain = new AdditivePlayerStat<float>(1, addfloat, substractfloat, "P");
				this.maxMassacreTime = new AdditivePlayerStat<float>(15.0f, addfloat, substractfloat, "P");
				this.timeBonusPerKill = new AdditivePlayerStat<float>(7.5f, addfloat, substractfloat, "P");
				this.MaxLogs = new AdditivePlayerStat<int>(2, addint, substractint);


				this.silenced = new BooleanPlayerStat(false);
				this.rooted = new BooleanPlayerStat(false);
				this.stunned = new BooleanPlayerStat(false);

				this.magicFind = new AdditiveNetworkSyncedPlayerStat<float>(1.0f, addfloat, substractfloat, "P");
				this.explosionDamage = new AdditiveNetworkSyncedPlayerStat<float>(0.0f, addfloat, substractfloat, "N1");
				this.fireTickRate = new AdditiveNetworkSyncedPlayerStat<float>(0.0f, addfloat, substractfloat);
				this.fireDuration = new AdditiveNetworkSyncedPlayerStat<float>(0.0f, addfloat, substractfloat);
				//spells

				//blink
				this.spell_fireboltEnergyCost = new AdditivePlayerStat<float>(15.0f, addfloat, substractfloat);
				this.spell_fireboltDamageScaling = new AdditivePlayerStat<float>(0.2f, addfloat, substractfloat);
				this.spell_blinkRange = new AdditivePlayerStat<float>(15.0f, addfloat, substractfloat);
				this.spell_blinkDamageScaling = new AdditivePlayerStat<float>(3.0f, addfloat, substractfloat, "P");
				this.spell_blinkDamage = new AdditivePlayerStat<float>(0, addfloat, substractfloat);
				this.spell_blinkDoExplosion = new BooleanPlayerStat(false);
				//parry
				this.spell_parry = new BooleanPlayerStat(false);
				this.spell_parryDamage = new AdditivePlayerStat<float>(40, addfloat, substractfloat);
				this.spell_parryAttackSpeed = new AdditivePlayerStat<float>(1, addfloat, substractfloat);
				this.spell_parryRadius = new AdditivePlayerStat<float>(3.5f, addfloat, substractfloat);
				this.spell_parryBuffDuration = new AdditivePlayerStat<float>(10, addfloat, substractfloat);
				this.spell_parryHeal = new AdditivePlayerStat<float>(10, addfloat, substractfloat);
				this.spell_parryEnergy = new AdditivePlayerStat<float>(10, addfloat, substractfloat);
				this.spell_chanceToParryOnHit = new BooleanPlayerStat(false);
				this.spell_parryIgnites = new BooleanPlayerStat(false);
				this.spell_parryDmgBonus = new AdditivePlayerStat<float>(0, addfloat, substractfloat);
				this.spell_parryBuffDamage = new AdditivePlayerStat<float>(0, addfloat, substractfloat);
				//healing dome
				this.spell_healingDomeGivesImmunity = new BooleanPlayerStat(false);
				this.spell_healingDomeRegEnergy = new BooleanPlayerStat(false);
				this.spell_healingDomeDuration = new AdditivePlayerStat<float>(10, addfloat, substractfloat);
				//flare 	
				this.spell_flareDamage = new AdditivePlayerStat<float>(40, addfloat, substractfloat);
				this.spell_flareDamageScaling = new AdditivePlayerStat<float>(1.65f, addfloat, substractfloat);
				this.spell_flareSlow = new AdditivePlayerStat<float>(0.4f, addfloat, substractfloat);
				this.spell_flareBoost = new AdditivePlayerStat<float>(1.35f, addfloat, substractfloat);
				this.spell_flareHeal = new AdditivePlayerStat<float>(11, addfloat, substractfloat);
				this.spell_flareRadius = new AdditivePlayerStat<float>(8.5f, addfloat, substractfloat);
				this.spell_flareDuration = new AdditivePlayerStat<float>(20, addfloat, substractfloat);
				//black hole 
				this.spell_blackhole_damage = new AdditivePlayerStat<float>(40, addfloat, substractfloat);
				this.spell_blackhole_damageScaling = new AdditivePlayerStat<float>(2.25f, addfloat, substractfloat, "P");
				this.spell_blackhole_duration = new AdditivePlayerStat<float>(9, addfloat, substractfloat);
				this.spell_blackhole_radius = new AdditivePlayerStat<float>(15, addfloat, substractfloat);
				this.spell_blackhole_pullforce = new AdditivePlayerStat<float>(25, addfloat, substractfloat);
				//sustain shield
				this.spell_shieldPerSecond = new AdditivePlayerStat<float>(4, addfloat, substractfloat);
				this.spell_shieldMax = new AdditivePlayerStat<float>(40, addfloat, substractfloat);
				this.spell_shieldPersistanceLifetime = new AdditivePlayerStat<float>(20, addfloat, substractfloat);
				//portal
				this.spell_portalDuration = new AdditivePlayerStat<float>(30, addfloat, substractfloat);
				//warcry
				this.spell_warCryRadius = new AdditivePlayerStat<float>(50, addfloat, substractfloat);
				this.spell_warCryAtkSpeed = new AdditivePlayerStat<float>(1.2f, addfloat, substractfloat);
				this.spell_warCryDamage = new AdditivePlayerStat<float>(1.2f, addfloat, substractfloat);
				this.spell_warCryGiveDamage = new BooleanPlayerStat(false);
				this.spell_warCryGiveArmor = new BooleanPlayerStat(false);
				//magic arrow
				this.spell_magicArrowDmgDebuff = new BooleanPlayerStat(false);
				this.spell_magicArrowCrit = new BooleanPlayerStat(false);
				this.spell_magicArrowDoubleSlow = new BooleanPlayerStat(false);
				this.spell_magicArrowVolleyCount = new AdditivePlayerStat<int>(0, addint, substractint);
				this.spell_magicArrowDuration = new AdditivePlayerStat<float>(10f, addfloat, substractfloat);
				this.spell_magicArrowDamageScaling = new AdditivePlayerStat<float>(5f, addfloat, substractfloat, "P");
				//purge
				this.spell_blackFlameDamageScaling = new AdditivePlayerStat<float>(1 / 2.5f, addfloat, substractfloat, "P");

				this.spell_purgeRadius = new AdditivePlayerStat<float>(30, addfloat, substractfloat);
				this.spell_purgeHeal = new BooleanPlayerStat(false);
				this.spell_purgeDamageBonus = new BooleanPlayerStat(false);
				//snap freeze
				this.spell_snapFreezeDist = new AdditivePlayerStat<float>(20, addfloat, substractfloat);
				this.spell_snapFloatAmount = new AdditivePlayerStat<float>(0.2f, addfloat, substractfloat);
				this.spell_snapDamageScaling = new AdditivePlayerStat<float>(3.5f, addfloat, substractfloat, "P");
				this.spell_snapFreezeDuration = new AdditivePlayerStat<float>(7f, addfloat, substractfloat);
				//ball lightning
				this.spell_ballLightning_Damage = new AdditivePlayerStat<float>(720f, addfloat, substractfloat);
				this.spell_ballLightning_DamageScaling = new AdditivePlayerStat<float>(26.66f, addfloat, substractfloat, "P");
				this.spell_ballLightning_Crit = new BooleanPlayerStat(false);
				//bash
				this.spell_bashDamageDebuffAmount = new AdditivePlayerStat<float>(1.30f, addfloat, substractfloat, "P");
				this.spell_bashDamageBuff = new AdditivePlayerStat<float>(0f, addfloat, substractfloat);
				this.spell_bashSlowAmount = new AdditivePlayerStat<float>(0.4f, addfloat, substractfloat);
				this.spell_bashLifesteal = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
				this.spell_bashEnabled = new BooleanPlayerStat(false);
				this.spell_bashBleedChance = new AdditivePlayerStat<float>(0, addfloat, substractfloat);
				this.spell_bashBleedDmg = new AdditivePlayerStat<float>(0.3f, addfloat, substractfloat);
				this.spell_bashDuration = new AdditivePlayerStat<float>(2f, addfloat, substractfloat);
				//frenzy
				this.spell_frenzyMaxStacks = new AdditivePlayerStat<int>(5, addint, substractint);
				this.spell_frenzyStacks = new AdditivePlayerStat<int>(0, addint, substractint);
				this.spell_frenzyAtkSpeed = new AdditivePlayerStat<float>(0.02f, addfloat, substractfloat);
				this.spell_frenzyDmg = new AdditivePlayerStat<float>(0.075f, addfloat, substractfloat, "P");
				this.spell_frenzy = new BooleanPlayerStat(false);
				this.spell_frenzyMS = new BooleanPlayerStat(false);
				this.spell_furySwipes = new BooleanPlayerStat(false);
				//focus
				this.spell_focusBonusDmg = new AdditivePlayerStat<float>(0, addfloat, substractfloat);
				this.spell_focusOnHS = new AdditivePlayerStat<float>(0.5f, addfloat, substractfloat);
				this.spell_focusOnBS = new AdditivePlayerStat<float>(0.15f, addfloat, substractfloat);
				this.spell_focusOnAtkSpeed = new AdditivePlayerStat<float>(1.3f, addfloat, substractfloat);
				this.spell_focusOnAtkSpeedDuration = new AdditivePlayerStat<float>(4f, addfloat, substractfloat);
				this.spell_focusSlowAmount = new AdditivePlayerStat<float>(0.8f, addfloat, substractfloat);
				this.spell_focusSlowDuration = new AdditivePlayerStat<float>(4f, addfloat, substractfloat);
				this.spell_focus = new BooleanPlayerStat(false);
				//seeking arrow
				this.spell_seekingArrow_HeadDamage = new AdditivePlayerStat<float>(0.5f, addfloat, substractfloat);
				this.spell_seekingArrow_DamageBonus = new AdditivePlayerStat<float>(0f, addfloat, substractfloat);
				this.spell_seekingArrow_SlowDuration = new AdditivePlayerStat<float>(4f, addfloat, substractfloat);
				this.spell_seekingArrow_SlowAmount = new AdditivePlayerStat<float>(0.9f, addfloat, substractfloat);
				this.spell_seekingArrowDuration = new AdditivePlayerStat<float>(10f, addfloat, substractfloat);
				this.spell_seekingArrow = new BooleanPlayerStat(false);
				//cataclysm			  
				this.spell_cataclysmDamage = new AdditivePlayerStat<float>(24f, addfloat, substractfloat);
				this.spell_cataclysmDamageScaling = new AdditivePlayerStat<float>(3f, addfloat, substractfloat, "P");
				this.spell_cataclysmDuration = new AdditivePlayerStat<float>(12f, addfloat, substractfloat);
				this.spell_cataclysmRadius = new AdditivePlayerStat<float>(5f, addfloat, substractfloat);
				this.spell_cataclysmArcane = new BooleanPlayerStat(false);
				//blood infused arrow
				this.spell_bia_SpellDmMult = new AdditivePlayerStat<float>(1.15f, addfloat, substractfloat);
				this.spell_bia_HealthDmMult = new AdditivePlayerStat<float>(3f, addfloat, substractfloat);
				this.spell_bia_HealthTaken = new AdditivePlayerStat<float>(0.65f, addfloat, substractfloat);
				this.spell_bia_AccumulatedDamage = new AdditivePlayerStat<float>(0f, addfloat, substractfloat);
				this.spell_bia_TripleDmg = new BooleanPlayerStat(false);
				this.spell_bia_Weaken = new BooleanPlayerStat(false);
				//roaring cheeks
				this.spell_fartRadius = new AdditivePlayerStat<float>(30f, addfloat, substractfloat);
				this.spell_fartKnockback = new AdditivePlayerStat<float>(2f, addfloat, substractfloat);
				this.spell_fartSlow = new AdditivePlayerStat<float>(0.8f, addfloat, substractfloat);
				this.spell_fartDebuffDuration = new AdditivePlayerStat<float>(30f, addfloat, substractfloat);
				this.spell_fartBaseDmg = new AdditivePlayerStat<float>(20f, addfloat, substractfloat);
				//taunt
				this.spell_taunt_speedChange = new MultiplicativePlayerStat<float>(2, multfloat, dividefloat, "P");
				this.spell_taunt_pullEnemiesIn = new BooleanPlayerStat(false);

				this.spell_snowstormMaxCharge = new AdditivePlayerStat<float>(10, addfloat, substractfloat);
				this.spell_snowstormDamageMult = new AdditivePlayerStat<float>(1, addfloat, substractfloat);
				this.spell_snowstormHitDelay = new MultiplicativePlayerStat<float>(0.5f, multfloat, dividefloat);
				this.spell_snowstormPullEnemiesIn = new BooleanPlayerStat(false);

				this.spell_berserkDuration = new AdditivePlayerStat<float>(30, addfloat, substractfloat);


				//perks
				this.perk_fireDmgIncreaseOnHit = new BooleanPlayerStat(false);
				this.perk_parryCounterStrikeDamage = new AdditivePlayerStat<float>(0f, addfloat, substractfloat);
				this.perk_turboRaftOwners = new AdditiveNetworkSyncedPlayerStat<int>(0, addint, substractint);
				this.perk_RaftSpeedMultipier = new AdditiveNetworkSyncedPlayerStat<float>(1, addfloat, dividefloat, "P");
				this.perk_thirstRate = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.perk_hungerRate = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.perk_projectileNoConsumeChance = new AdditivePlayerStat<float>(0f, addfloat, substractfloat, "P");
				this.perk_thrownSpearDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.perk_thrownSpearExtraArmorReduction = new BooleanPlayerStat(false);

				this.perk_thrownSpearCritChance = new AdditivePlayerStat<float>(0.05f, addfloat, substractfloat, "P");
				this.perk_thrownSpearhellChance = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
				this.perk_bulletCritChance = new AdditivePlayerStat<float>(0.1f, addfloat, substractfloat, "P");
				this.perk_bulletDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.perk_crossbowDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.perk_bowDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.perk_multishotProjectileCount = new AdditivePlayerStat<int>(1, addint, substractint);
				this.perk_multishotDamagePennalty = new MultiplicativePlayerStat<float>(0.75f, addfloat, substractfloat);
				this.perk_nearDeathExperienceTriggered = new BooleanPlayerStat(false);
				this.perk_nearDeathExperienceUnlocked = new BooleanPlayerStat(false);
				this.perk_craftingReroll = new BooleanPlayerStat(false);
				this.perk_craftingReforge = new BooleanPlayerStat(false);
				this.perk_craftingPolishing = new BooleanPlayerStat(false);
				this.perk_craftingEmpowering = new BooleanPlayerStat(false);
				this.perk_craftingRerollingSingleStat = new BooleanPlayerStat(false);
				this.perk_isShieldAutocast = new BooleanPlayerStat(false);
				this.perk_parryAnything = new BooleanPlayerStat(false);
				this.perk_blackholePullImmune = new BooleanPlayerStat(false);
				this.perk_blizzardSlowReduced = new BooleanPlayerStat(false);
				this.perk_trueAim = new BooleanPlayerStat(false);
				this.perk_trueAimUpgrade = new BooleanPlayerStat(false);
				this.perk_goldenResolve = new BooleanPlayerStat(false);
				this.perk_projectileDamageIncreasedBySize = new BooleanPlayerStat(false);
				this.perk_projectileDamageIncreasedBySpeed = new BooleanPlayerStat(false);
				this.perk_infinityMagic = new BooleanPlayerStat(false);
				this.perk_flashlightIntensity = new AdditivePlayerStat<float>(1f, addfloat, substractfloat, "P");
				this.perk_flashlightBatteryDrain = new AdditivePlayerStat<float>(1f, addfloat, substractfloat, "P");
				this.perk_bunnyHop = new BooleanPlayerStat(false);
				this.perk_bunnyHopUpgrade = new BooleanPlayerStat(false);
				this.perk_danceOfFiregod = new BooleanPlayerStat(false);
				this.perk_danceOfFiregodAtkCap = new BooleanPlayerStat(false);
				this.perk_doubleStickHarvesting = new BooleanPlayerStat(false);
				this.perk_chargedAtkKnockback = new BooleanPlayerStat(false);

				//items
				this.i_greatBowIgnites = new BooleanPlayerStat(false);
				this.i_SmokeyCrossbowQuiver = new BooleanPlayerStat(false);
				this.i_CrossfireQuiver = new BooleanPlayerStat(false);
				this.i_HazardCrown = new BooleanPlayerStat(false);
				this.i_HazardCrownBonus = new AdditivePlayerStat<int>(0, addint, substractint);
				this.i_HammerStun = new BooleanPlayerStat(false);
				this.i_HammerStunDuration = new MultiplicativePlayerStat<float>(0.4f, multfloat, dividefloat);
				this.i_HammerStunAmount = new MultiplicativePlayerStat<float>(0.25f, multfloat, dividefloat, "P");
				this.smashDamage = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
				this.i_HexedPantsOfMrM_Enabled = new BooleanPlayerStat(false);
				this.i_DeathPact_Enabled = new BooleanPlayerStat(false);
				this.i_EruptionBow = new BooleanPlayerStat(false);
				this.i_ArchangelBow = new BooleanPlayerStat(false);
				this.i_isGreed = new BooleanPlayerStat(false);
				this.i_KingQruiesSword = new BooleanPlayerStat(false);
				this.i_SoraBracers = new BooleanPlayerStat(false);
				this.i_isWindArmor = new BooleanPlayerStat(false);
				this.i_sparkOfLightAfterDark = new BooleanPlayerStat(false);
				this.i_infinityLoop = new BooleanPlayerStat(false);
				this.i_setcount_AkagisSet = new AdditivePlayerStat<int>(0,addint,substractint);
				this.i_setcount_BerserkSet = new AdditivePlayerStat<int>(0,addint,substractint);

				ModAPI.Log.Write("Initialized player stats");
			}


			//functions
			public float TotalMaxHealth
			{
				get
				{
					float x = baseHealth + (vitality.Value * maxHealthFromVit.Value) + maxHealth.Value;
					x *= maxHealthMult;
					return x;
				}
			}
			public float TotalMaxEnergy
			{
				get
				{
					float x = baseEnergy + (agility.Value * maxEnergyFromAgi.Value) + maxEnergy.Value;
					x *= maxEnergyMult;
					return x;
				}
			}
			public float TotalMagicDamageMultiplier
			{
				get
				{
					float f = spellDmgFromInt.Value * intelligence.Value;
					return (1 + f) * spellIncreasedDmg.Value * allDamage.Value;
				}
			}
			public float SpellCostToStamina => 1 - spellCostEnergyCost;
			public float TotalThorns => thorns.Value + thornsPerStrenght * strength.Value + thornsPerVit * vitality.Value;
			public float TotalThornsDamage => TotalThorns * thornsDmgMult.Value * meleeIncreasedDmg * allDamage;
			public float TotalArmor => armor.Value - instance.lostArmor;
			public float TotalStaminaRecoveryAmount => (baseStaminaRecovery + staminaRecoveryperSecond) * TotalStaminaRecoveryMultiplier;
			public float TotalStaminaRecoveryMultiplier => 1 + (1 + intelligence * energyRecoveryFromInt) * allRecoveryMult * staminaRecoveryperSecond;
			public float TotalEnergyRecoveryMultiplier => 1 + (1 + intelligence * energyRecoveryFromInt) * allRecoveryMult;


			public float MeleeDamageMult => allDamage.Value * meleeIncreasedDmg.Value * (1 + (strength * meleeDmgFromStr));
			public float RangedDamageMult => allDamage.Value * rangedIncreasedDmg.Value * (1 + (agility * rangedDmgFromAgi)) * (perk_projectileDamageIncreasedBySize ? 1 + (projectileSize.Value - 1) * 2 : 1f);
			public float SpellDamageMult => allDamage.Value * spellIncreasedDmg.Value * (1 + intelligence * spellDmgFromInt);

			public int TotalMeleeArmorPiercing => allArmorPiercing + meleeArmorPiercing;
			public int TotalRangedArmorPiercing => allArmorPiercing + rangedArmorPiercing;
			public int TotalThornsArmorPiercing => Mathf.RoundToInt(allArmorPiercing * thornsArmorPiercing);
			public bool Critted => Random.value < critChance;
			public float RandomCritDamage => Random.value < critChance ? 1f + critDamage : 1f;


		}
	}
}