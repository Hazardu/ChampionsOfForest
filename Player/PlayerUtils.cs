namespace ChampionsOfForest.Player
{
	internal class PlayerUtils
	{

		//since those functions use customWeapons field, they cannot be put in a lot of classes
		//modapi needs to first inject code for player stats, custom weaponry, inventory extension
		internal static float GetPlayerMeleeDamageRating()
		{
			var equippedWeapon = Inventory.GetEquippedItemAtSlot(Inventory.EquippableSlots.MainHand);
			float dmg = 10;
			float atkSpeed = 1;
			if (equippedWeapon != null)
			{
				if (PlayerInventoryMod.customWeapons.ContainsKey(equippedWeapon.subtype))
				{
					var cw = PlayerInventoryMod.customWeapons[equippedWeapon.subtype];
					dmg = cw.damage;
				}
			}
			float dps = dmg + ModdedPlayer.Stats.baseMeleeDamage;
			dps *= ModdedPlayer.Stats.MeleeDamageMult;
			dps *= atkSpeed * ModdedPlayer.Stats.attackSpeed;
			dps *= 1 + (ModdedPlayer.Stats.critChance * ModdedPlayer.Stats.critDamage);

			return dps;
		}
		internal static float GetPlayerRangedDamageRating()
		{
			var equippedWeapon = Inventory.GetEquippedItemAtSlot(Inventory.EquippableSlots.MainHand);
			bool greatbow = false;
			float dmg = 10;
			float atkSpeed = 1;
			if (equippedWeapon != null)
			{
				greatbow = equippedWeapon.subtype == ItemDefinition.ItemSubtype.Greatbow;
				if (greatbow)
				{
					dmg += 140;
					atkSpeed = 0.2f;
				}
			}
			float dps = dmg + ModdedPlayer.Stats.baseRangedDamage;
			dps *= ModdedPlayer.Stats.RangedDamageMult;
			if (greatbow)
				dps *= 1.75f;
			dps *= atkSpeed * ModdedPlayer.Stats.attackSpeed;
			dps *= 1 + (ModdedPlayer.Stats.critChance * ModdedPlayer.Stats.critDamage + (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed.value ? (ModdedPlayer.Stats.projectileSpeed - 1) * 1f : 0));

			return dps;
		}
		internal static float GetPlayerSpellDamageRating()
		{
			float dps = ModdedPlayer.Stats.baseSpellDamage +1;
			dps *= ModdedPlayer.Stats.SpellDamageMult;

			return dps;
		}
		internal static float GetPlayerToughnessRating()
		{
			float toughness = ModdedPlayer.Stats.TotalMaxHealth;
			toughness /= ModdedPlayer.Stats.allDamageTaken;
			toughness /= 1f - ModReferences.DamageReduction(ModdedPlayer.Stats.armor);
			return toughness;
		}
	}
}
