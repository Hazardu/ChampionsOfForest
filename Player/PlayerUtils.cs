namespace ChampionsOfForest.Player
{
	internal static class PlayerUtils
	{

		public static float MeleeDamageMult() => 
			MeleeDamageMult(ModdedPlayer.Stats.allDamage, ModdedPlayer.Stats.meleeIncreasedDmg, ModdedPlayer.Stats.strength, ModdedPlayer.Stats.meleeDmgFromStr);
		public static float MeleeDamageMult(float allDamage, float meleeIncreasedDmg, float strength, float meleeDmgFromStr) =>
			allDamage * meleeIncreasedDmg * (1 + (strength * meleeDmgFromStr));


		public static float RangedDamageMult(float allDamage, float rangedIncreasedDmg, float agility, float rangedDmgFromAgi, bool perk_projectileDamageIncreasedBySize, float projectileSize) =>
			allDamage * rangedIncreasedDmg * (1 + (agility * rangedDmgFromAgi)) * (perk_projectileDamageIncreasedBySize ? (1 + (projectileSize - 1) * 2) : 1f);
		public static float RangedDamageMult() =>
			RangedDamageMult(ModdedPlayer.Stats.allDamage, ModdedPlayer.Stats.rangedIncreasedDmg, ModdedPlayer.Stats.agility, ModdedPlayer.Stats.rangedDmgFromAgi, ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize, ModdedPlayer.Stats.projectileSize);


		public static float SpellDamageMult() =>
			SpellDamageMult(ModdedPlayer.Stats.allDamage, ModdedPlayer.Stats.spellIncreasedDmg, ModdedPlayer.Stats.intelligence, ModdedPlayer.Stats.spellDmgFromInt);
		public static float SpellDamageMult(float allDamage, float spellIncreasedDmg, float intelligence, float spellDmgFromInt) => 
			allDamage * spellIncreasedDmg * (1 + intelligence * spellDmgFromInt);




		//since those functions use customWeapons field, they cannot be put in a lot of classes
		//modapi needs to first inject code for player stats, custom weaponry, inventory extension
		internal static float GetPlayerMeleeDamageRating()
		{
			return GetPlayerMeleeDamageRating(ModdedPlayer.Stats.meleeFlatDmg, ModdedPlayer.Stats.attackSpeed, ModdedPlayer.Stats.allDamage, ModdedPlayer.Stats.meleeIncreasedDmg, ModdedPlayer.Stats.strength, ModdedPlayer.Stats.meleeDmgFromStr, ModdedPlayer.Stats.critChance, ModdedPlayer.Stats.critDamage);
		}

		public static float GetPlayerMeleeDamageRating(float meleeFlatDmg, float attackSpeed, float allDamage, float meleeIncreasedDmg, float strength, float meleeDmgFromStr, float critChance, float critDamage)
		{
			float dmg = 10;
			float dps = dmg + meleeFlatDmg;
			dps *= MeleeDamageMult(allDamage, meleeIncreasedDmg, strength, meleeDmgFromStr);
			dps *= attackSpeed;
			dps *= 1 + (critChance * critDamage);

			return dps;
		}

		internal static float GetPlayerRangedDamageRating()
		{
			return GetPlayerRangedDamageRating(
				ModdedPlayer.Stats.rangedFlatDmg,
				ModdedPlayer.Stats.attackSpeed,
				ModdedPlayer.Stats.allDamage,
				ModdedPlayer.Stats.rangedIncreasedDmg,
				ModdedPlayer.Stats.agility,
				ModdedPlayer.Stats.rangedDmgFromAgi,
				ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize,
				ModdedPlayer.Stats.projectileSize,
				ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed,
				ModdedPlayer.Stats.projectileSpeed,
				ModdedPlayer.Stats.critChance,
				ModdedPlayer.Stats.critDamage);
		}
		public static float GetPlayerRangedDamageRating(float rangedFlatDmg, float attackSpeed, float allDamage, float rangedIncreasedDmg, float agility, float rangedDmgFromAgi, bool perk_projectileDamageIncreasedBySize, float projectileSize, bool perk_projectileDamageIncreasedBySpeed, float projectileSpeed, float critChance, float critDamage)
		{
			float dmg = 10;
			float dps = dmg + rangedFlatDmg;
			dps *= RangedDamageMult(allDamage, rangedIncreasedDmg, agility, rangedDmgFromAgi, perk_projectileDamageIncreasedBySize, projectileSize);
			dps *= attackSpeed;
			dps *= 1 + (critChance * (critDamage + (perk_projectileDamageIncreasedBySpeed ? (projectileSpeed - 1) * 2.5f : 0)));

			return dps;
		}
		internal static float GetPlayerSpellDamageRating()
		{
			return GetPlayerSpellDamageRating(ModdedPlayer.Stats.spellFlatDmg, ModdedPlayer.Stats.allDamage, ModdedPlayer.Stats.spellIncreasedDmg, ModdedPlayer.Stats.intelligence, ModdedPlayer.Stats.spellDmgFromInt, ModdedPlayer.Stats.critChance, ModdedPlayer.Stats.critDamage);
		}
		public static float GetPlayerSpellDamageRating(float spellFlatDmg, float allDamage, float spellIncreasedDmg, float intelligence, float spellDmgFromInt, float critChance, float critDamage)
		{
			float dps = spellFlatDmg + 1;
			dps *= SpellDamageMult(allDamage, spellIncreasedDmg, intelligence, spellDmgFromInt);
			dps *= 1 + (critChance * critDamage);
			return dps;
		}



		internal static float GetPlayerToughnessRating()
		{
			return GetPlayerToughnessRating(ModdedPlayer.Stats.TotalMaxHealth,
				ModdedPlayer.Stats.allDamageTaken,
				ModdedPlayer.Stats.armor,
				ModdedPlayer.Stats.getHitChance,
				ModdedPlayer.Stats.magicDamageTaken,
				ModdedPlayer.Stats.block);
		}
		public static float GetPlayerToughnessRating(float maxHP, float dmgTaken, int armor, float getHitChance, float magicDamageTaken, float block)
		{
			float toughness = maxHP + block;
			toughness /= dmgTaken;
			toughness /= 1f - CotfUtils.GetArmorEffectiveness(armor);
			toughness /= getHitChance;
			toughness /= magicDamageTaken / 2f + 0.5f;
			return toughness;
		}
	}
}
