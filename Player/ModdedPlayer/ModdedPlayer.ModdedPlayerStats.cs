namespace ChampionsOfForest.Player
{
	public partial class ModdedPlayer
	{
		public class ModdedPlayerStats
		{
			public const float baseHealth = 50f;
			public const float baseEnergy = 50f;
			public const float baseStaminaRecovery = 4f;

			public readonly AdditivePlayerStat<int> strength = new AdditivePlayerStat<int>(1, addint, substractint);
			public readonly AdditivePlayerStat<int> intelligence = new AdditivePlayerStat<int>(1, addint, substractint);
			public readonly AdditivePlayerStat<int> agility = new AdditivePlayerStat<int>(1, addint, substractint);
			public readonly AdditivePlayerStat<int> vitality = new AdditivePlayerStat<int>(1, addint, substractint);
			public readonly AdditivePlayerStat<float> maxEnergyMult = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<int> maxEnergy = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<float> maxHealthMult = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<int> maxHealth = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<float> meleeDmgFromStr = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> spellDmgFromInt = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> rangedDmgFromAgi = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> energyRecoveryFromInt = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> maxEnergyFromAgi = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> maxHealthFromVit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> fireDamage = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> healthPerSecRate = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> staminaPerSecRate = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly MultiplicativePlayerStat<float> cooldown = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> allDamage = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> attackSpeed = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> movementSpeed = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> critChance = new AdditivePlayerStat<float>(0.05f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> critDamage = new AdditivePlayerStat<float>(0.5f, addfloat, substractfloat, "P");

			public readonly AdditivePlayerStat<float> rangedFlatDmg = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> meleeFlatDmg = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat );
			public readonly AdditivePlayerStat<float> spellFlatDmg = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly MultiOperationPlayerStat<float> rangedIncreasedDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> meleeIncreasedDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> spellIncreasedDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<int> meleeArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<int> rangedArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<int> thornsArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<int> allArmorPiercing = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<float> chanceToSlow = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> chanceToBleed = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> chanceToWeaken = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> areaDamageChance = new AdditivePlayerStat<float>(0.10f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> areaDamage = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> areaDamageRadius = new AdditivePlayerStat<float>(4.0f, addfloat, substractfloat, "P");
			public readonly MultiOperationPlayerStat<float> projectileSpeed = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> projectileSize = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");
			public readonly MultiOperationPlayerStat<float> heavyAttackDmg = new MultiOperationPlayerStat<float>(1, 1, addfloat, substractfloat, multfloat, dividefloat, "P");

			public readonly MultiplicativePlayerStat<float> weaponRange = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> headShotDamage = new MultiplicativePlayerStat<float>(6, multfloat, dividefloat, "P");

			public readonly MultiplicativePlayerStat<float> allRecoveryMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> healthOnHit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> staminaOnHit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> energyOnHit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> healthRecoveryPerSecond = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> staminaRecoveryperSecond = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> energyRecoveryperSecond = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);

			public readonly MultiplicativePlayerStat<float> allDamageTaken = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> magicDamageTaken = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> fireDamageTaken = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> getHitChance = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<int> armor = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly MultiplicativePlayerStat<float> thornsDmgMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> thorns = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> thornsPerStrenght = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> thornsPerVit = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<int> stunImmunity = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<int> rootImmunity = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<int> debuffImmunity = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly AdditivePlayerStat<int> debuffResistance = new AdditivePlayerStat<int>(0, addint, substractint);


			public readonly AdditivePlayerStat<float> jumpPower = new AdditivePlayerStat<float>(1.0f, addfloat, substractfloat,"P");
			public readonly MultiplicativePlayerStat<float> spellCostEnergyCost = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> spellCost = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> attackStaminaCost = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> block = new AdditivePlayerStat<float>(0.5f, addfloat, substractfloat, "P");
			public readonly MultiplicativePlayerStat<float> expGain = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> maxMassacreTime = new AdditivePlayerStat<float>(15.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> timeBonusPerKill = new AdditivePlayerStat<float>(7.5f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<int> MaxLogs = new AdditivePlayerStat<int>(2, addint, substractint);


			public readonly BooleanPlayerStat silenced = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat rooted = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat stunned = new BooleanPlayerStat(false);

			//spells

			//blink
			public readonly AdditivePlayerStat<float> spell_blinkRange = new AdditivePlayerStat<float>(15.0f, addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_blinkDamage = new AdditivePlayerStat<float>( 0,addfloat, substractfloat);
			//parry
			public readonly BooleanPlayerStat spell_parry;
			public readonly AdditivePlayerStat<float> spell_parryDamage = new AdditivePlayerStat<float>( 40,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_parryRadius = new AdditivePlayerStat<float>( 3.5f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_parryBuffDuration = new AdditivePlayerStat<float>( 10,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_parryHeal = new AdditivePlayerStat<float>( 5,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_parryEnergy = new AdditivePlayerStat<float>( 10,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_chanceToParryOnHit = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_parryIgnites = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<float> spell_parryDmgBonus = new AdditivePlayerStat<float>( 0,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_parryBuffDamage = new AdditivePlayerStat<float>( 0,addfloat, substractfloat);
			//healing dome
			public readonly BooleanPlayerStat spell_healingDomeGivesImmunity = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_healingDomeRegEnergy = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<float> spell_healingDomeDuration = new AdditivePlayerStat<float>( 10,addfloat, substractfloat);
			//flare 	
			public readonly AdditivePlayerStat<float> spell_flareDamage = new AdditivePlayerStat<float>( 40,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_flareSlow = new AdditivePlayerStat<float>( 0.4f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_flareBoost = new AdditivePlayerStat<float>( 1.35f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_flareHeal = new AdditivePlayerStat<float>( 11,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_flareRadius = new AdditivePlayerStat<float>( 5.5f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_flareDuration = new AdditivePlayerStat<float>( 20,addfloat, substractfloat);
			//black hole 
			public readonly AdditivePlayerStat<float> spell_blackhole_damage = new AdditivePlayerStat<float>( 40,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_blackhole_duration = new AdditivePlayerStat<float>( 9,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_blackhole_radius = new AdditivePlayerStat<float>( 15,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_blackhole_pullforce = new AdditivePlayerStat<float>( 25,addfloat, substractfloat);
			//sustain shield
			public readonly AdditivePlayerStat<float> spell_shieldPerSecond = new AdditivePlayerStat<float>( 4,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_shieldMax = new AdditivePlayerStat<float>( 40,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_shieldPersistanceLifetime = new AdditivePlayerStat<float>( 20,addfloat, substractfloat);
			//portal
			public readonly AdditivePlayerStat<float> spell_portalDuration = new AdditivePlayerStat<float>( 30,addfloat, substractfloat);
			//warcry
			public readonly AdditivePlayerStat<float> spell_warCryRadius = new AdditivePlayerStat<float>( 50,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_warCryAtkSpeed = new AdditivePlayerStat<float>( 1.2f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_warCryDamage = new AdditivePlayerStat<float>( 1.2f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_warCryGiveDamage = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_warCryGiveArmor = new BooleanPlayerStat(false);
			//magic arrow
			public readonly BooleanPlayerStat spell_magicArrowDmgDebuff = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_magicArrowCrit = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_magicArrowDoubleSlow = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<float> spell_magicArrowDuration = new AdditivePlayerStat<float>( 10f,addfloat, substractfloat);
			//purge
			public readonly AdditivePlayerStat<float> spell_purgeRadius = new AdditivePlayerStat<float>( 30,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_purgeHeal = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_purgeDamageBonus = new BooleanPlayerStat(false);
			//snap freeze
			public readonly AdditivePlayerStat<float> spell_snapFreezeDist = new AdditivePlayerStat<float>( 20,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_snapFloatAmount = new AdditivePlayerStat<float>( 0.2f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_snapFreezeDuration = new AdditivePlayerStat<float>( 7f,addfloat, substractfloat);
			//ball lightning
			public readonly AdditivePlayerStat<float> spell_ballLightning_Damage = new AdditivePlayerStat<float>( 620f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_ballLightning_Crit = new BooleanPlayerStat(false);
			//bash
			public readonly AdditivePlayerStat<float> spell_bashExtraDamage = new AdditivePlayerStat<float>( 1.30f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bashDamageBuff = new AdditivePlayerStat<float>( 0f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bashSlowAmount = new AdditivePlayerStat<float>( 0.4f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bashLifesteal = new AdditivePlayerStat<float>( 0.0f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_bashEnabled = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<float> spell_bashBleedChance = new AdditivePlayerStat<float>( 0,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bashBleedDmg = new AdditivePlayerStat<float>( 0.3f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bashDuration = new AdditivePlayerStat<float>( 2f,addfloat, substractfloat);
			//frenzy
			public readonly AdditivePlayerStat<int> spell_frenzyMaxStacks = new AdditivePlayerStat<int>( 5,addint, substractint);
			public readonly AdditivePlayerStat<int> spell_frenzyStacks = new AdditivePlayerStat<int>( 0, addint, substractint);
			public readonly AdditivePlayerStat<float> spell_frenzyAtkSpeed = new AdditivePlayerStat<float>( 0.02f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_frenzyDmg = new AdditivePlayerStat<float>( 0.075f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_frenzy;
			public readonly BooleanPlayerStat spell_frenzyMS = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_furySwipes = new BooleanPlayerStat(false);
			//focus
			public readonly AdditivePlayerStat<float> spell_focusBonusDmg;
			public readonly AdditivePlayerStat<float> spell_focusOnHS = new AdditivePlayerStat<float>( 1f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_focusOnBS = new AdditivePlayerStat<float>( 0.2f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_focusOnAtkSpeed = new AdditivePlayerStat<float>( 1.3f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_focusOnAtkSpeedDuration = new AdditivePlayerStat<float>( 4f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_focusSlowAmount = new AdditivePlayerStat<float>( 0.8f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_focusSlowDuration = new AdditivePlayerStat<float>( 4f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_focus;
			//seeking arrow
			public readonly AdditivePlayerStat<float> spell_seekingArrow_HeadDamage = new AdditivePlayerStat<float>( 2f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_seekingArrow_SlowDuration = new AdditivePlayerStat<float>( 4f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_seekingArrow_SlowAmount = new AdditivePlayerStat<float>( 0.8f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_seekingArrow_DamagePerDistance = new AdditivePlayerStat<float>( 0.01f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_seekingArrowDuration = new AdditivePlayerStat<float>( 30f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_seekingArrow;
			//cataclysm			  
			public readonly AdditivePlayerStat<float> spell_cataclysmDamage = new AdditivePlayerStat<float>( 24f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_cataclysmDuration = new AdditivePlayerStat<float>( 12f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_cataclysmRadius = new AdditivePlayerStat<float>( 5f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_cataclysmArcane = new BooleanPlayerStat(false);
			//blood infused arrow
			public readonly AdditivePlayerStat<float> spell_bia_SpellDmMult = new AdditivePlayerStat<float>( 1.25f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bia_HealthDmMult = new AdditivePlayerStat<float>( 3f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_bia = new AdditivePlayerStat<float>( 0.65f,addfloat, substractfloat);
			public readonly BooleanPlayerStat spell_bia_TripleDmg = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat spell_bia_Weaken = new BooleanPlayerStat(false);
			//roaring cheeks
			public readonly AdditivePlayerStat<float> spell_fartRadius = new AdditivePlayerStat<float>( 30f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_fartKnockback = new AdditivePlayerStat<float>( 2f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_fartSlow = new AdditivePlayerStat<float>( 0.8f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_fartDebuffDuration = new AdditivePlayerStat<float>( 30f,addfloat, substractfloat);
			public readonly AdditivePlayerStat<float> spell_fartBaseDmg = new AdditivePlayerStat<float>( 20f,addfloat, substractfloat);


			//perks
			public readonly BooleanPlayerStat perk_fireDmgIncreaseOnHit = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<float> perk_parryCounterStrikeDamage = new AdditivePlayerStat<float>(0f, addfloat, substractfloat);
			public readonly AdditiveNetworkSyncedPlayerStat< int> perk_turboRaftOwners =new AdditiveNetworkSyncedPlayerStat<int>(0, addint, substractint);
			public readonly AdditiveNetworkSyncedPlayerStat<float> perk_RaftSpeedMultipier = new AdditiveNetworkSyncedPlayerStat<float>(1, addfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> perk_thirstRate = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> perk_hungerRate = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> perk_projectileNoConsumeChance = new AdditivePlayerStat<float>(0f, addfloat, substractfloat,"P");
			public readonly MultiplicativePlayerStat<float> perk_thrownSpearDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<float> perk_thrownSpearCritChance = new AdditivePlayerStat<float>(0.05f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> perk_thrownSpearhellChance = new AdditivePlayerStat<float>(0.0f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> perk_bulletCritChance = new AdditivePlayerStat<float>(0.1f, addfloat, substractfloat, "P");
			public readonly MultiplicativePlayerStat<float> perk_bulletDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> perk_crossbowDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> perk_bowDamageMult = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly AdditivePlayerStat<int> perk_multishotProjectileCount = new AdditivePlayerStat<int>(1, addint, substractint);
			public readonly BooleanPlayerStat perk_nearDeathExperienceTriggered = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_nearDeathExperienceUnlocked = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_craftingReroll = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_craftingReforge = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_craftingPolishing = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_craftingEmpowering = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_isShieldAutocast = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_parryAnything = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_blackholePullImmune = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_blizzardSlowReduced = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_trueAim = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_trueAimUpgrade = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_goldenResolve = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_projectileDamageIncreasedBySize = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_projectileDamageIncreasedBySpeed = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_infinityMagic = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<float> perk_flashlightIntensity = new AdditivePlayerStat<float>(1f, addfloat, substractfloat, "P");
			public readonly AdditivePlayerStat<float> perk_flashlightBatteryDrain = new AdditivePlayerStat<float>(1f, addfloat, substractfloat, "P");
			public readonly BooleanPlayerStat perk_bunnyHop = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_bunnyHopUpgrade = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_danceOfFiregod = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat perk_danceOfFiregodAtkCap = new BooleanPlayerStat(false);

			//items
			public readonly BooleanPlayerStat i_greatBowIgnites = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_SmokeyCrossbowQuiver = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_CrossfireQuiver = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_HazardCrown = new BooleanPlayerStat(false);
			public readonly AdditivePlayerStat<int> i_HazardCrownBonus = new AdditivePlayerStat<int>(0, addint, substractint);
			public readonly BooleanPlayerStat i_HammerStun = new BooleanPlayerStat(false);
			public readonly MultiplicativePlayerStat<float> i_HammerStunDuration = new MultiplicativePlayerStat<float>(0.4f, multfloat, dividefloat);
			public readonly MultiplicativePlayerStat<float> i_HammerStunAmount = new MultiplicativePlayerStat<float>(0.25f, multfloat, dividefloat, "P");
			public readonly MultiplicativePlayerStat<float> i_HammerSmashDamageAmp = new MultiplicativePlayerStat<float>(1, multfloat, dividefloat, "P");
			public readonly BooleanPlayerStat i_HexedPantsOfMrM_Enabled = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_DeathPact_Enabled = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_EruptionBow = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_ArchangelBow = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_isGreed = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_KingQruiesSword = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_SoraBracers = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_isWindArmor = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_sparkOfLightAfterDark = new BooleanPlayerStat(false);
			public readonly BooleanPlayerStat i_infinityLoop = new BooleanPlayerStat(false);





			//functions
			public float TotalMaxHealth
			{
				get
				{
					float x = baseHealth + (vitality.Value * maxHealthFromVit.Value) + maxHealth.Value;
					x *= 1 + maxHealthMult;
					return x;
				}
			}
			public float TotalMaxEnergy
			{
				get
				{
					float x = baseEnergy + (agility.Value * maxEnergyFromAgi.Value) + maxEnergy.Value;
					x *= 1 + maxEnergyMult;
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

		}
	}
}