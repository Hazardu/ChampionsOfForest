using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
				if (PlayerInventoryMod.customWeapons.ContainsKey(equippedWeapon.weaponModel))
				{
					var cw = PlayerInventoryMod.customWeapons[equippedWeapon.weaponModel];
					dmg = cw.damage;
				}
			}
			float dps = dmg + ModdedPlayer.Stats.meleeFlatDmg;
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
				greatbow = equippedWeapon.weaponModel == BaseItem.WeaponModelType.Greatbow;
				if (greatbow)
				{
					dmg += 140;
					atkSpeed = 0.2f;
				}
			}
			float dps = dmg + ModdedPlayer.Stats.rangedFlatDmg;
			dps *= ModdedPlayer.Stats.RangedDamageMult;
			if (greatbow)
				dps *= 1.75f;
			dps *= atkSpeed * ModdedPlayer.Stats.attackSpeed;
			dps *= 1 + (ModdedPlayer.Stats.critChance * ModdedPlayer.Stats.critDamage + (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed.value ? (ModdedPlayer.Stats.projectileSize - 1) * 2.5f : 0));

			return dps;
		}
		internal static float GetPlayerSpellDamageRating()
		{
			float dps = ModdedPlayer.Stats.spellFlatDmg +1;
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
