using System;
using System.Collections.Generic;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Localization;

using TheForest.Utils;

namespace ChampionsOfForest.Player
{
	public static class SpellDataBase
	{
		public static Dictionary<int, Spell> spellDictionary = new Dictionary<int, Spell>();
		public static int[] SortedSpellIDs;

		public static void Reset()
		{
			foreach (var spell in spellDictionary.Values)
			{
				spell.Cooldown = spell.BaseCooldown;
			}
		}

		public static void Initialize()
		{
			try
			{
				spellDictionary = new Dictionary<int, Spell>();
				FillSpells();
				List<int> SortedSpellIDsTemp = new List<int>(spellDictionary.Keys);
				SortedSpellIDsTemp.Sort((x, y) => spellDictionary[x].LevelRequirement.CompareTo(spellDictionary[y].LevelRequirement));
				SortedSpellIDs = SortedSpellIDsTemp.ToArray();
				// ModAPI.Log.Write("SETUP: SPELL DB");
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}

		public static void FillSpells()
		{
			Spell bh = new Spell(iD: 1, TextureID: 119, levelRequirement: 20, energyCost: 100, baseCooldown: 120, name: Translations.MainMenu_HUD_11/*og:Black Hole*/, () => string.Format(Translations.SpellDataBase_1( ModdedPlayer.Stats.spell_blackhole_duration, ModdedPlayer.Stats.spell_blackhole_damageScaling)))//tr
			{
				active = SpellActions.CreatePlayerBlackHole,
				CastOnRelease = true,
				aim = SpellActions.BlackHoleAim,
				aimEnd = SpellActions.BlackHoleAimEnd
			};
			Spell healingDome = new Spell(iD: 2, TextureID: 122, levelRequirement: 6, energyCost: 80, baseCooldown: 70, name: Translations.SpellDataBase_3/*og:Healing Dome*/, () => Translations.SpellDataBase_2/*og:Creates a sphere of vaporized aloe that heals all allies inside.*/)//tr
			{
				active = SpellActions.CreateHealingDome,
				CastOnRelease = true,
				aim = SpellActions.HealingDomeAim,
				aimEnd = SpellActions.HealingDomeAimEnd
			};
			new Spell(iD: 3, TextureID: 121, levelRequirement: 3, energyCost: 25, baseCooldown: 14, name: Translations.SpellDataBase_5/*og:Blink*/, () => Translations.SpellDataBase_4( ModdedPlayer.Stats.spell_blinkDamageScaling.ToString()))//tr
			{
				active = SpellActions.DoBlink,
				CastOnRelease = true,
				aim = SpellActions.DoBlinkAim,
			};
			new Spell(iD: 4, TextureID: 120, levelRequirement: 10, energyCost: 100, baseCooldown: 70, name: Translations.SpellDataBase_7/*og:Sun Flare*/, () => Translations.SpellDataBase_6(ModdedPlayer.Stats.spell_flareDamageScaling.ToString(), ModdedPlayer.Stats.spell_flareSlow))//tr
			{
				active = SpellActions.CastFlare,
			};
			new Spell(iD: 5, TextureID: 118, levelRequirement: 8, energyCost: 50, name: Translations.SpellDataBase_9/*og:Sustain Shield*/, () => Translations.SpellDataBase_8(ModdedPlayer.Stats.spell_shieldPersistanceLifetime))//tr
			{
				active = SpellActions.CastSustainShieldActive,
				passive = SpellActions.CastSustainShieldPassive,
				usePassiveOnUpdate = true,
			};
			new Spell(iD: 6, TextureID: 117, levelRequirement: 2, energyCost: 10, baseCooldown: 0.4f, name: Translations.SpellDataBase_11/*og:Wide Reach*/, () => Translations.SpellDataBase_10/*og:Picks up all items, including equipment, in a small radius around you.*/)//tr
			{
				active = AutoPickupItems.DoPickup,
				CastOnRelease = false,
			};
			new Spell(iD: 7, TextureID: 115, levelRequirement: 21, energyCost: 25, baseCooldown: 2.5f, name: Translations.SpellDataBase_13/*og:Black Flame*/, () =>Translations.SpellDataBase_12( ModdedPlayer.Stats.spell_blackFlameDamageScaling.ToString()))//tr
			{
				active = BlackFlame.Toggle,
			};
			new Spell(iD: 8, TextureID: 123, levelRequirement: 14, energyCost: 85, baseCooldown: 180, name: Translations.SpellDataBase_15/*og:War Cry*/, () => Translations.SpellDataBase_14(2))//tr
			{
				active = SpellActions.CastWarCry,
			};
			new Spell(iD: 9, TextureID: 114, levelRequirement: 12, energyCost: 90, baseCooldown: 60, name: Translations.SpellDataBase_17/*og:Portal*/, () => Translations.SpellDataBase_16/*og:Creates a wormhole, that links 2 locations. Allows the player and items to pass through.*/)//tr
			{
				CastOnRelease= true,
				aim= SpellActions.DoPortalAim,
				active = SpellActions.CastPortal,
			};
			new Spell(iD: 10, TextureID: 125, levelRequirement: 27, energyCost: 100, baseCooldown: 20, name: Translations.SpellDataBase_19/*og:Magic Arrow*/, () =>Translations.SpellDataBase_18( ModdedPlayer.Stats.spell_magicArrowDamageScaling.ToString()))//tr
			{
				active = SpellActions.CastMagicArrow,
				CastOnRelease = true,
				aim = SpellActions.MagicArrowAim,
				aimEnd = SpellActions.MagicArrowAimEnd
			};
			new Spell(iD: 11, TextureID: 127, levelRequirement: 40, energyCost: 30, baseCooldown: 12, name: Translations.SpellDataBase_21/*og:Multishot Enchantment*/, () => Translations.SpellDataBase_20/*og:Enchants your ranged weapons with a magic formation of copying. Every shot causes multiple projectiles to be created with a downside of heavy energy toll. Energy is consumed upon firing and depends on the amount of projectiles fired*/)//tr
			{
				active = SpellActions.ToggleMultishot,
			};
			new Spell(iD: 12, TextureID: 133, levelRequirement: 35, energyCost: 65, baseCooldown: 160, name: Translations.SpellDataBase_23/*og:Golden Skin*/, () => Translations.SpellDataBase_22(GoldenSkin.Duration))//tr
			{
				active = GoldenSkin.Cast,
			};
			new Spell(iD: 13, TextureID: 132, levelRequirement: 7, energyCost: 40, baseCooldown: 16, name: Translations.SpellDataBase_25/*og:Purge*/, () => Translations.SpellDataBase_24/*og:Everyone in your surroundings gets cleansed of their negative debuffs. Negates poison.*/)//tr
			{
				active = SpellActions.CastPurge,
			};
			new Spell(iD: 14, TextureID: 128, levelRequirement: 20, energyCost: 220, baseCooldown: 40, name: Translations.SpellDataBase_27/*og:Snap Freeze*/, () =>Translations.SpellDataBase_26( ModdedPlayer.Stats.spell_snapDamageScaling.ToString(), ""))//tr
			{
				active = SpellActions.CastSnapFreeze,
			};
			new Spell(iD: 15, TextureID: 131, levelRequirement: 25, energyCost: 10, baseCooldown: 200, name: Translations.SpellDataBase_29/*og:Berserk*/, () => Translations.SpellDataBase_28/*og:For short amount of time, gain increased damage dealt, attack speed and movement speed and have unlimited stamina, however, you take increased damage.*/)//tr
			{
				active = Berserker.Cast,
			};
			new Spell(iD: 16, TextureID: 130, levelRequirement: 44, energyCost: 350, baseCooldown: 100, name: Translations.SpellDataBase_31/*og:Ball Lightning*/, () => Translations.SpellDataBase_30( ModdedPlayer.Stats.spell_ballLightning_DamageScaling.ToString()))//tr
			{
				active = SpellActions.CastBallLightning,
			};
			new Spell(iD: 17, TextureID: 134, levelRequirement: 17, energyCost: 25, baseCooldown: 25, name: "Bash",
				() => Translations.SpellDataBase_32(ModdedPlayer.Stats.spell_bashDuration, ModdedPlayer.Stats.spell_bashDamageDebuffAmount.ToString(), ModdedPlayer.Stats.spell_bashDuration, (ModdedPlayer.Stats.spell_bashDamageDebuffAmount-1).ToString("P")))//tr
			{
				passive = SpellActions.BashPassiveEnabled,
				active = SpellActions.BashActive
			};
			new Spell(18, 136, 1, 0, 60, Translations.SpellDataBase_34/*og:Frenzy*/, () =>Translations.SpellDataBase_33( ModdedPlayer.Stats.spell_frenzyDmg.ToString(), ModdedPlayer.Stats.spell_frenzyMaxStacks))//tr
			{
				passive = x => ModdedPlayer.Stats.spell_frenzy.value = x,
				active = () =>
				{
					float f = ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive;
					LocalPlayer.Stats.Energy += ModdedPlayer.Stats.TotalMaxEnergy * f * 0.05f;
					ModdedPlayer.Stats.attackSpeed.valueMultiplicative /= 1 + f * ModdedPlayer.Stats.spell_frenzyAtkSpeed;
					ModdedPlayer.Stats.allDamage.valueMultiplicative /= 1 + f * ModdedPlayer.Stats.spell_frenzyDmg;
					if (ModdedPlayer.Stats.spell_frenzyMS)
						ModdedPlayer.Stats.movementSpeed.valueMultiplicative /= 1 + f * 0.05f;

					ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive = 0;
				}
			};
			new Spell(19, 135, 27, 40, 10, Translations.SpellDataBase_36/*og:Seeking Arrow*/, () => Translations.SpellDataBase_35("10%",ModdedPlayer.Stats.spell_seekingArrow_HeadDamage.ToString(),ModdedPlayer.Stats.spell_seekingArrow_SlowAmount.ToString(), ModdedPlayer.Stats.spell_seekingArrow_SlowDuration.ToString())/*og:Casting this spell enchants your next ranged attack, causing all of your ranged attacks to fly towards the first enemy hit by the enchanted projectile. While active, damage is decreased by 10%, projectiles deal 50% less headshot damage and bodyshots slow enemies by 10% for 4 seconds.*/)//tr
			{
				active = SpellActions.SeekingArrow_Active,
			};
			new Spell(20, 137, 4, 50, 30, Translations.SpellDataBase_38/*og:Focus*/, () => Translations.SpellDataBase_37(ModdedPlayer.Stats.spell_focusOnHS.ToString(), ModdedPlayer.Stats.spell_focusSlowAmount, ModdedPlayer.Stats.spell_focusOnBS, "25%", 5)/*og:Passive: When landing a headshot, next projectile will deal 50% more damage and slow the enemy by 20%. When landing a body shot, next projectile will deal only 15% more damage, but attack speed is increased.\nActive: Gain 15% critical hit chance for 5 seconds*/)//tr
			{
				passive = x => ModdedPlayer.Stats.spell_focus.value = x,
				active = () => BuffDB.AddBuff(28, 102, 1.25f, 5f)
			};
			new Spell(21, 140, 8, 35, 60, Translations.SpellDataBase_40/*og:Parry*/, () => Translations.SpellDataBase_39(ModdedPlayer.Stats.spell_parryDamage, ModdedPlayer.Stats.spell_parryBuffDuration,"50%", 10)/*og:Passive: When parrying an enemy, deal magic damage to enemies around the target. Additionally, gain energy, heal yourself for a small amount and get stun immunity for 10 seconds after parrying.\nActive: Gain 50% damage reduction for 10 seconds*/)//tr
			{
				passive = x => ModdedPlayer.Stats.spell_parry.value = x,
				active = () => BuffDB.AddBuff(29, 103, 0.5f, 10f)

			};
			new Spell(22, 141, 50, 500, 300, Translations.SpellDataBase_42/*og:Cataclysm*/, () => Translations.SpellDataBase_41( ModdedPlayer.Stats.spell_cataclysmDamageScaling.ToString()))//tr
			{
				active = () => SpellActions.CastCataclysm(),
				CastOnRelease = true,
				aim = SpellActions.CataclysmAim,
				aimEnd = SpellActions.CataclysmAimEnd
			};
			new Spell(23, 165, 11, 75, 8, Translations.SpellDataBase_44/*og:Blood Infused Arrow*/, () => Translations.SpellDataBase_43/*og:Sacrifice your own vitals to empower your next arrow. Drains your health and adds lost health as damage to the next projectile.*/)//tr
			{
				active = () => SpellActions.CastBloodInfArr(),
			};
			new Spell(24, 182, 55, 400, 360, Translations.SpellDataBase_46/*og:Roaring Cheeks*/, () => Translations.SpellDataBase_45("70%")/*og:Release a powerful gust of toxic gas that poisons and knocks enemies back, or cast it mid air to perform a secondary jump, the damage and area of effect will be lower but 2/3 of the cooldown will be refunded*/)//tr
			{
				active = () => SpellActions.RipAFatOne(),
			};
			new Spell(25, 189, 9, 40, 45, Translations.SpellDataBase_48/*og:Taunt*/, () => Translations.SpellDataBase_47("75%")/*og:Makes enemies hyper aggressive and forces them to attack you. Taunted enemies move twice attack twice as fast and take 50% increased damage*/)//tr
			{
				active = Taunt.OnSpellUsed,
				CastOnRelease = true,
				aim = Taunt.Aim,
				aimEnd = Taunt.AimEnd
			};
			new Spell(26, 201, 6, 45, 20, Translations.SpellDataBase_50/*og:Firebolt*/, () => Translations.SpellDataBase_49 (ModdedPlayer.Stats.spell_fireboltDamageScaling.GetFormattedAmount()))//tr
			{
				active = () => Spells.ActiveSpellManager.Instance.PrepareForFiring(Spells.FireboltSpell.Instance),
			};
			new Spell(27, 200, 9, 120, 20, Translations.SpellDataBase_52/*og:Snow Storm*/, () => Translations.SpellDataBase_51(10))//tr
			{
				active = () => Spells.ActiveSpellManager.Instance.PrepareForFiring(Spells.BlizzardSpell.Instance),
			};
			//new Spell(24, 165, 1, 1, 2, "Corpse Explosion", "")
			//{
			//    active = () => SpellActions.CastCorpseExplosion(),
			//};
			//new Spell(25, 165, 1, 1, "Devour", "...")
			//{
			//    active = () =>
			//};
		}
	}
}