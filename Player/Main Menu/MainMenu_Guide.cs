using System;
using System.Collections.Generic;

using ChampionsOfForest.Player;

using UnityEngine;

using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
	public partial class MainMenu : MonoBehaviour
	{
		private readonly float GuideWidthDecrease = 150;
		private readonly float GuideMargin = 30;

		#region StatsMenu

		private float BookPositionY;
		private float BookScrollAmount;
		private GUIStyle headerstyle;
		private GUIStyle statStyle;
		private GUIStyle statStyleAmount;
		private GUIStyle statStyleTooltip;
		private GUIStyle TextLabel;

		public struct Bookmark
		{
			public float position;
			public string name;
		}

		public List<Bookmark> Bookmarks = new List<Bookmark>();

		private void Header(string s)
		{
			if (BookPositionY < Screen.height && BookPositionY > -140 * screenScale)
			{
				Rect labelRect = new Rect(GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease), 70 * screenScale);
				GUI.Label(labelRect, s, headerstyle);
				BookPositionY += 70 * screenScale;
				Rect imageRect = new Rect(400 * screenScale, BookPositionY, Screen.width - 800 * screenScale, 60 * screenScale);
				GUI.DrawTexture(imageRect, ResourceLoader.GetTexture(30));
				BookPositionY += 70 * screenScale;
			}
			else
			{
				BookPositionY += 140 * screenScale;
			}
		}

		private void Space(float pixelsUnscaled)
		{
			BookPositionY += pixelsUnscaled * screenScale;
		}

		private void Stat(string statName, string amount, string tooltip = "")
		{
			if (BookPositionY < Screen.height && BookPositionY > -70 * screenScale)
			{
				Rect labelRect = new Rect(100 * screenScale + GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease) - 200 * screenScale, statStyle.fontSize);
				GUI.Label(labelRect, statName, statStyle);
				GUI.Label(labelRect, amount, statStyleAmount);
				BookPositionY += statStyle.fontSize;
				if (labelRect.Contains(mousePos) && tooltip != "")
				{
					float h = statStyleTooltip.CalcHeight(new GUIContent(tooltip), Screen.width - 500 * screenScale);
					Rect tooltipRect = new Rect(GuideWidthDecrease * screenScale + 150 * screenScale, BookPositionY, Screen.width - 500 * screenScale, h);
					GUI.Label(tooltipRect, tooltip, statStyleTooltip);
					BookPositionY += h;
				}
				BookPositionY += 5 * screenScale;
			}
			else
			{
				float h = statStyleTooltip.CalcHeight(new GUIContent(tooltip), Screen.width - 500 * screenScale);
				BookPositionY += 5 * screenScale + h;
				BookPositionY += statStyle.fontSize;
			}
		}

		private void Label(string s)
		{
			float h = TextLabel.CalcHeight(new GUIContent(s), Screen.width - 500 * screenScale);

			if (BookPositionY < Screen.height && BookPositionY > -h * screenScale)
			{
				Rect rect = new Rect(GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease), h);
				GUI.Label(rect, s, TextLabel);
				BookPositionY += h;
			}
			else
			{
				BookPositionY += h;
			}
		}

		private void Image(int iconID, float height, float centerPosition = 0.5f)
		{
			height *= screenScale;
			if (BookPositionY < Screen.height && BookPositionY > -height * screenScale)
			{
				Texture2D tex = Res.ResourceLoader.GetTexture(iconID);
				Rect rect = new Rect(0, 0, height * tex.width / tex.height, height)
				{
					center = new Vector2(Screen.width * centerPosition, height / 2 + BookPositionY)
				};
				BookPositionY += rect.height;
				GUI.DrawTexture(rect, tex);
			}
			else
			{
				BookPositionY += height;
			}
		}

		private void MarkBookmark(string s)
		{
			Bookmarks.Add(new Bookmark() { name = s, position = BookPositionY });
		}

		private void SetGuiStylesForGuide()
		{
			headerstyle = new GUIStyle(GUI.skin.label)
			{
				font = secondaryFont,
				fontSize = Mathf.RoundToInt(70 * screenScale),
				hover = new GUIStyleState()
				{
					textColor = new Color(1, 1, 0.8f)
				},
				alignment = TextAnchor.UpperCenter,
				richText = true,
			};
			statStyle = new GUIStyle(GUI.skin.label)
			{
				font = mainFont,
				fontSize = Mathf.RoundToInt(24 * screenScale),
				hover = new GUIStyleState()
				{
					textColor = new Color(1, 1, 0.8f)
				},
				alignment = TextAnchor.MiddleLeft,
				richText = true,
			};
			statStyleAmount = new GUIStyle(GUI.skin.label)
			{
				font = mainFont,
				fontSize = Mathf.RoundToInt(25 * screenScale),
				normal = new GUIStyleState()
				{
					textColor = new Color(0.3f, 1, 0.3f)
				},
				hover = new GUIStyleState()
				{
					textColor = new Color(0, 1, 0.1f)
				},
				alignment = TextAnchor.MiddleRight,
				richText = true,
			};
			int margin = Mathf.RoundToInt(25 * screenScale);
			statStyleTooltip = new GUIStyle(GUI.skin.label)
			{
				font = mainFont,
				fontSize = Mathf.RoundToInt(28 * screenScale),
				fontStyle = FontStyle.Italic,
				margin = new RectOffset(margin, margin, margin, margin),
				stretchWidth = true,
				alignment = TextAnchor.UpperLeft,
				richText = true,
			};
			TextLabel = new GUIStyle(GUI.skin.label)
			{
				font = mainFont,
				fontSize = Mathf.RoundToInt(35 * screenScale),
				stretchWidth = true,
				alignment = TextAnchor.UpperLeft,
				richText = true,
				hover = new GUIStyleState()
				{
					textColor = new Color(1, 1, 0.8f)
				},
			};
		}

		private void DrawGuide()
		{
			Bookmarks.Clear();
			BookScrollAmount = Mathf.Clamp(BookScrollAmount + 200 * screenScale * UnityEngine.Input.GetAxis("Mouse ScrollWheel"), -Screen.height * 20 * screenScale, 0);
			BookPositionY = BookScrollAmount;
			SetGuiStylesForGuide();

			Header("Basic Information");
			MarkBookmark("Home");
			Label("\tExperience");
			Stat("Current level", ModdedPlayer.instance.level.ToString("N0"));
			Stat("Current experience", ModdedPlayer.instance.ExpCurrent.ToString("N0"));
			Stat("Experience goal", ModdedPlayer.instance.ExpGoal.ToString("N0"), "Next level: " + (ModdedPlayer.instance.level + 1) + " you will need to get this amount of experience:\t " + ModdedPlayer.instance.GetGoalExp(ModdedPlayer.instance.level + 1).ToString("N0"));
			Stat("Progress amount: ", (((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal) * 100).ToString() + "%");
			Label("\tLevel is the estimation of my power. I must become stronger to survive." +
				"\nHigher level allow me to equip better equipement. " +
				"\nLeveling up gives me the ability to develop usefull abilities. (Currently you have " + ModdedPlayer.instance.MutationPoints + " mutation points), which you can spend on unlocking spells or perks. ");
			Space(50);
			Label("\nSources of experience" +
				"\n-Mutants - Enemies give the most experience, it's possible to chain kills to get more exp, and the reward is exp reward is based on bounty." +
				"\n-Animals - Experience gained does not increase with difficulty. It's only viable for levels <10" +
				"\n-Tall bushes - Give minium amount of experience. Not viable after level 6" +
				"\n-Trees - Gives little experience for every tree chopped down." +
				"\n-Effigies - It's possible to gain experience and low rarity items by breaking effigies scattered across the map." +
				"\n-Rare consumable - Gives a large amount of experience, it's rarity is orange\n");

			Space(100);
			Header("Information - Statistics");
			Label("\tAttributes");
			Label("Strength - This stat increases melee damage multipier. It multiplies with melee damage increase.");
			Label("Agility - This stat increases ranged damage multipier. Additionally it increases maximum amount of energy");
			Label("Agility - This stat increases maximum health points, this value is further multiplied by maximum health %");

			Space(100);

			Header("Information - Items");
			Label("\tEquipement can be obtained by killing enemies and breaking effigies. Normal enemies can drop a few items on death, if the odds are in your favor. The chance to get any items from a normal enemy is 10%. The amount of items obtained from normal enemies is at least 1 and maximum amount increases with players in a lobby.\n" +
			 "Elite enemies always drop items in large amounts.\n" +
			 "\tItems can be equipped by dragging and dropping them onto a right equipement slot or shift+left click. The item will grant it's stats only if you meets item's level requirement. The best tier of items is only obtainable on high difficulties.");
			Label("By unlocking a perk in the survival category, it's possible to change the stats on your existing items, and reforge unused items into something useful. Reforged item will have the same level as item put into the main crafting slot.");

			Space(100);

			Header("Information - Mutations and Abilities");
			Label("\tUpon leveling up, the player will recieve a upgrade point. Then it's up to the player to use it to unlock a mutation, that will serve as a permanent perk, or to spend two upgrade points to unlock a ability.\n" +
				"Abilities are in majority of the cases more powerful than perks, as they cost more and the number of active abilities is limitied to 6.\n" +
				"Some perks can be bought multiple times.\n" +
				"\n" +
				"Refunding - it is possible to refund all points, to do so, heart of purity needs to be consumed. This item is of yellow rarity, and thus unobtainable on easy and veteran difficulties." +
				"More points - to gain a point without leveling, a rare item of green rarity needs to be consumed. It permamently adds a upgrade point, and it persists even after refunding.");
			Space(100);

			Header("Information - Enemies");
			Label("\tEnemies in the forest have adapted to your skill. As they level with you, they become faster and stronger. But speed and strength alone shouldn't be your main concern. There are a lot more dangerous beings out there." +
				"\n\n" +
				"Common enemies changed slightly. Their health increases with level.\n" +
				"A new statistic to enemies is 'Armor'. This property reduces damage taken by the enemies from physical attacks, and partly reduces damage from magical attacks. Armor can be reduced in a number of ways.\n" +
				"The easiest way to reduce armor is to use fire. Fire works as a way to crowd control enemies, it renders a few enemies unable to run and attack as they shake off the flames.\n" +
				"Other way to reduce armor is to equip items, which reduce armor on hit.\n" +
				"If you dont have any way to reduce enemy's armor, damaging them with spells would decrease the reduction from armor by 2/3, allowing you to deal some damage.");
			Space(30);
			Label("Elite enemies\n" +
				"An elite is a uncommon type of a mutant with increased stats and access to special abilities, that make encounters with them challenging." +
				"\nEnemy abilities:");
			Label("- Steadfast - This defensive ability causes enemy to reduce all damage exceeding a percent of their maximum health. To deal with this kind of ability, damage over time and fast attacks are recommended. This ability counters nuke instances of damage.");
			Label("- Blizzard - A temporary aura around an enemy, that slows anyone in it's area of effect. Affects movement speed and attack speed. Best way to deal with this is to avoid getting within it's range. Crowd controll from ranged attacks and running seems like the best option.");
			Label("- Radiance - A permanent aura around an enemy. It deals damage anyone around. The only way of dealing with this is to never get close to the enemy.");
			Label("- Chains - Roots anyone in a big radius around the elite. The duration this root increases with difficulty. Several abilities that provide resistance to crowd controll clear the effects of this ability.");
			Label("- Black hole - A very strong ability. The spell has a fixed cooldown, and the enemy will attempt to cast it as soon as a player gets within his range effective.");
			Label("- Trap sphere - Long lasting sphere that forces you to stay inside it untill it's effects wears off");
			Label("- Juggernaut - The enemy is completely immune to crowd controll and bleeding.\n");
			Label("- Gargantuan - Describes an enemy that is bigger, faster, stronger and has more health.");
			Label("- Tiny - An enemy has decreased size. It's harder to hit it with ranged attacks and most of the melee weapons can only attack the enemy with slow smashes.");
			Label("- Extra tough - enemy has a lot more healt");
			Label("- Extra deadly - enemy has a lot more damage");
			Label("- Basher - the enemy stuns on hit. Best way to fight it is to not get hit or parry it's attacks.");
			Label("- Warping - An ability allowing to teleport. Strong against glass cannon builds, running away and ranged attacks. Weak agnist melee strikes and a lot of durability.");
			Label("- Rain Empowerment - If it rains, the enemy gains in strenght, speed, armor and size.");
			Label("- Meteors - Periodically spawns a rain of powerful meteors. They are rather easy to spot and they move at a slow medium speed.");
			Label("- Flare - Slows and damages me if you stand inside. Heals and makes enemies faster.");
			Label("- Undead - An enemy upon dieing restores portion of it's health, gets stronger and bigger.");
			Label("- Plasma cannon - Creates a turret that fires a laser beam that damages players and buildings.");
			Label("- Poisonous - Enemies gain a attack modifier, that applies a stacking debuff, which deals damage over time. Once hit, it is adviced to retreat and wait for the poison stop damaging you.");
			Label("- Cataclysm - Enemy uses the cataclysm spell to slow you down and damage you.");

			Header("Changes");
			Label("Champions of The Forest provides variety of changes to in-game mechanics." +
				"\nArmor no longer absorbs all damage. Instead it reduces the damage by 70%." +
				"\nPlayer is slowed down if out of stamina (the inner blue bar)" +
				"\nTraps no longer instantly kill cannibals. Instead they deal damage." +
				"\nDynamite no longer instantly kills enemies." +
				"\nEnemies have armor and increased health." +
				"\nPlayers take increased damage from explosives. This affects how much damage the worm does" +
				"\nPlayer deal increased damage to other players if friendly fire is enabled.");

			Space(300);

			Header("Statistics");
			Stat("Strength", ModdedPlayer.instance.strength.GetFormattedAmount()+ " str", "Increases melee damage by " + ModdedPlayer.instance.DamagePerStrength * 100 + "% for every 1 point of strength. Current bonus melee damage from strength [" + ModdedPlayer.instance.strength.Value * 100 * ModdedPlayer.instance.DamagePerStrength + "]");
			Stat("Agility", ModdedPlayer.instance.agility.GetFormattedAmount() + " agi", "Increases ranged damage by " + ModdedPlayer.instance.RangedDamageperAgi * 100 + "% for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.instance.agility.Value * 100 * ModdedPlayer.instance.RangedDamageperAgi + "]\n" +
				"Increases maximum energy by " + ModdedPlayer.instance.EnergyPerAgility + " for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.instance.agility.Value * ModdedPlayer.instance.EnergyPerAgility + "]");
			Stat("Vitality", ModdedPlayer.instance.vitality.GetFormattedAmount() + " vit", "Increases health by " + ModdedPlayer.instance.HealthPerVitality + "for every 1 point of vitality. Current bonus health from vitality [" + ModdedPlayer.instance.vitality.Value * ModdedPlayer.instance.HealthPerVitality + "]");
			Stat("Intelligence", ModdedPlayer.instance.intelligence.GetFormattedAmount() + " int", "Increases spell damage by " + ModdedPlayer.instance.SpellDamageperInt * 100 + "% for every 1 point of intelligence. Current bonus spell damage from intelligence [" + ModdedPlayer.instance.intelligence.Value * 100 * ModdedPlayer.instance.SpellDamageperInt + "]\n" +
				"Increases stamina regen by " + ModdedPlayer.instance.EnergyRegenPerInt * 100 + "% for every 1 point of intelligence. Current bonus stamina regen from intelligence [" + ModdedPlayer.instance.intelligence.Value * 100 * ModdedPlayer.instance.EnergyRegenPerInt + "]");

			Space(60);
			Image(105, 70);
			Header("Health & Energy");
			Space(10);

			Stat("Max health", ModdedPlayer.instance.MaxHealth.ToString("N0") + "", "Total health pool.\n" +
				"Base health: " + ModdedPlayer.instance.baseHealth +
				"\nBonus health: " + ModdedPlayer.instance.HealthBonus +
				"\nHealth from vitality: " + ModdedPlayer.instance.HealthPerVitality * ModdedPlayer.instance.vitality.Value +
				"\nHealth multipier: " + ModdedPlayer.instance.MaxHealthPercent * 100 + "%");
			Stat("Max energy", ModdedPlayer.instance.MaxEnergy.ToString("N0") + "", "Total energy pool.\n" +
				"Base energy: " + ModdedPlayer.instance.baseEnergy +
				"\nBonus energy: " + ModdedPlayer.instance.EnergyBonus +
				"\nEnergy from agility: " + ModdedPlayer.instance.EnergyPerAgility * ModdedPlayer.instance.agility.Value +
				"\nEnergy multipier: " + ModdedPlayer.instance.MaxEnergyPercent * 100 + "%");

			Space(60);
			Image(99, 70);
			Header("Defense");
			Space(10);
			Stat("Armor", ModdedPlayer.instance.Armor.ToString("N0"), "Armor provides physical damage reduction\nYour current amount of armor provides " + ModdedPlayer.instance.ArmorDmgRed * 100 + "% dmg reduction.");
			Stat("Magic resistance", ModdedPlayer.instance.MagicResistance * 100 + "%", "Magic damage reduction. Decreases damage from enemy abilities.");
			Stat("Dodge Chance", ModdedPlayer.instance.DodgeChance * 100 + "%", "A chance to avoid entire instance of damage. Works only for physical damage sources.");
			Stat("Damage taken reduction", Math.Round((ModdedPlayer.instance.DamageReductionTotal - 1) * 100, 1) + "%");
			Stat("Block", ModdedPlayer.instance.BlockFactor * 100 + "%");
			Stat("Absorb amount", ModdedPlayer.instance.DamageAbsorbAmount * 100 + "%");
			Stat("Fire resistance", Math.Round((1 - ModdedPlayer.instance.FireDamageTakenMult) * 100) + "%");
			Stat("Thorns", ModdedPlayer.instance.thornsDamage.ToString("N0"), $"Thorns inflict damage to attacking enemies. Thorns from gear and mutations {ModdedPlayer.instance.thorns.ToString("N0")}. Thorns from attributes {(ModdedPlayer.instance.thornsPerStrenght * ModdedPlayer.instance.strength.Value+ ModdedPlayer.instance.vitality.Value * ModdedPlayer.instance.thornsPerVit).ToString("N0")}.");

			Space(60);
			Header("Recovery");
			Space(10);

			Stat("Total Stamina recovery per second", ModdedPlayer.instance.StaminaRecover + "", "Stamina regen is temporairly paused after sprinting");
			Stat("Stamina per second", ModdedPlayer.instance.StaminaRegen * (1 + ModdedPlayer.instance.StaminaRegenPercent) + "", "Stamina per second: " + ModdedPlayer.instance.StaminaRegen + "\nStamina regen bonus: " + ModdedPlayer.instance.StaminaRegenPercent * 100 + "%");

			Stat("Energy per second", ModdedPlayer.instance.EnergyPerSecond * ModdedPlayer.instance.StaminaAndEnergyRegenAmp + "", "Energy per second: " + ModdedPlayer.instance.EnergyPerSecond + "\nStamina and energy regen multipier: " + ModdedPlayer.instance.StaminaAndEnergyRegenAmp * 100 + "%");
			Stat("Energy on hit", ModdedPlayer.instance.EnergyOnHit * ModdedPlayer.instance.StaminaAndEnergyRegenAmp + "", "Energy on hit: " + ModdedPlayer.instance.EnergyOnHit + "\nStamina and energy regen multipier: " + ModdedPlayer.instance.StaminaAndEnergyRegenAmp * 100 + "%");
			Stat("Health per second", ModdedPlayer.Stats.healthRecoveryPerSecond * (ModdedPlayer.Stats.healthPerSecRate + 1) * ModdedPlayer.Stats.allRecoveryMult + "", "Health per second: " + ModdedPlayer.Stats.healthRecoveryPerSecond + "\nStamina regen bonus: " + ModdedPlayer.Stats.healthPerSecRate * 100 + "%\nAll Healing Amplification: " + (ModdedPlayer.Stats.allRecoveryMult - 1) * 100 + "%");
			Stat("Health on hit", ModdedPlayer.instance.LifeOnHit * (ModdedPlayer.Stats.healthPerSecRate + 1) * ModdedPlayer.Stats.allRecoveryMult + "", "Health on hit: " + ModdedPlayer.instance.LifeOnHit + "\nStamina regen bonus: " + Math.Round(ModdedPlayer.Stats.healthPerSecRate * 100, 2) + "%\nAll Healing Amplification: " + (ModdedPlayer.Stats.allRecoveryMult - 1) * 100 + "%");

			Space(60);
			Header("Attack");
			Space(10);
			Stat("All damage amplification", Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
			Stat("Critical hit damage", Math.Round(ModdedPlayer.instance.CritDamage, 2) + "%");
			Stat("Critical hit chance", Math.Round(ModdedPlayer.instance.CritChance * 100, 2) + "%");
			Stat("Attack speed", Math.Round(ModdedPlayer.instance.AttackSpeed * 100, 2) + "%", "Increases the speed of player actions - weapon swinging, reloading guns and drawing bows");
			Stat("Additional fire damage", Math.Round(ModdedPlayer.instance.FireAmp * 100, 2) + "%", "Increases fire damage");
			Stat("Bleed chance", ModdedPlayer.instance.ChanceToBleedOnHit.ToString("P"), "Bleeding enemies take 5% of damage dealt per second for 10 seconds");
			Stat("Weaken chance", ModdedPlayer.instance.ChanceToWeakenOnHit.ToString("P"), "Weakened enemies take 20% increased damage from all players.");
			Stat("Slow chance", ModdedPlayer.instance.ChanceToSlowOnHit.ToString("P"), "Slowed enemies move and attack 50% slower");

			Space(20);
			Image(89, 70);
			Header("Melee");
			Space(10);

			Stat("Melee damage", Math.Round(ModdedPlayer.instance.MeleeAMP * 100, 2) + "%", "Melee damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
				"Bonus from strength: " + Math.Round(ModdedPlayer.instance.strength.Value * ModdedPlayer.instance.DamagePerStrength * 100, 2) + "%\n" +
				"Melee damage amplification: " + Math.Round((ModdedPlayer.instance.MeleeDamageAmplifier - 1) * 100, 2) + "%\n" +
				"Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
			Stat("Additional melee weapon damage", Math.Round(ModdedPlayer.instance.MeleeDamageBonus) + "", "Melee damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to weapon damage and multiplied by the stat above");
			Stat("Melee range", Math.Round(ModdedPlayer.instance.MeleeRange * 100) + "%");
			Stat("Heavy attack damage", ModdedPlayer.instance.HeavyAttackMult.ToString("P"));

			Space(20);
			Image(98, 70);
			Header("Ranged");
			Space(10);

			Stat("Ranged damage", Math.Round(ModdedPlayer.instance.RangedAMP * 100, 2) + "%", "Ranged damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
			 "Bonus from agility: " + Math.Round(ModdedPlayer.instance.agility.Value * ModdedPlayer.instance.RangedDamageperAgi * 100, 2) + "%\n" +
			 "Ranged damage amplification: " + Math.Round((ModdedPlayer.instance.RangedDamageAmplifier - 1) * 100, 2) + "%\n" +
			 "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
			Stat("Additional ranged weapon damage", Math.Round(ModdedPlayer.instance.RangedDamageBonus) + "", "Ranged damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to projectile damage and multiplied by the stat above");
			Stat("Projectile speed", Math.Round(ModdedPlayer.instance.ProjectileSpeedRatio * 100) + "%", "Faster projectiles fly further and fall slower");
			Stat("Projectile size", Math.Round(ModdedPlayer.instance.ProjectileSizeRatio * 100) + "%", "Bigger projectiles allow to land headshots easier. Most projectiles still can hit only 1 target.");
			Stat("Headshot damage", Math.Round(ModdedPlayer.instance.HeadShotDamage * 100) + "%", "Damage multipier on headshot");
			Stat("No consume chance", ModdedPlayer.instance.ReusabilityChance.ToString("P"));
			Stat("Spear headshot chance", ModdedPlayer.instance.SpearCritChance.ToString("P"));
			if (ModdedPlayer.instance.SpearhellChance > 0)
				Stat("Double spear chance", ModdedPlayer.instance.SpearhellChance.ToString("P"));
			if (ModdedPlayer.instance.SpearDamageMult != 1)
				Stat("Spear damage", ModdedPlayer.instance.SpearhellChance.ToString("P"));
			if (ModdedPlayer.instance.SpearArmorRedBonus)
				Stat("Spears reduce additional armor", "");
			Stat("Bullet headshot chance", ModdedPlayer.instance.BulletCritChance.ToString("P"));
			if (ModdedPlayer.instance.BulletDamageMult != 1)
				Stat("Bullet damage", ModdedPlayer.instance.SpearhellChance.ToString("P"));
			if (ModdedPlayer.instance.CrossbowDamageMult != 1)
				Stat("Crossbow damage", ModdedPlayer.instance.CrossbowDamageMult.ToString("P"));
			if (ModdedPlayer.instance.BowDamageMult != 1)
				Stat("Bow damage", ModdedPlayer.instance.BowDamageMult.ToString("P"));
			if (ModdedPlayer.instance.IsCrossfire)
				Stat("Shooting an enemy creates magic arrows", "");

			Stat("Multishot Projectiles", ModdedPlayer.instance.SoraSpecial ? (4 + ModdedPlayer.instance.MultishotCount).ToString("N") : ModdedPlayer.instance.MultishotCount.ToString("N"));
			Stat("Multishot Cost", (ModdedPlayer.instance.SoraSpecial ? 1f * Mathf.Pow(ModdedPlayer.instance.MultishotCount, 1.75f) : 10 * Mathf.Pow(ModdedPlayer.instance.MultishotCount, 1.75f)).ToString("N"), "Formula for multishot cost in energy is (Multishot Projectiles ^ 1.75) * 10");

			Space(20);
			Image(110, 70);
			Header("Magic");
			Space(10);

			Stat("Spell damage", Math.Round(ModdedPlayer.instance.TotalSpellAmplification * 100, 2) + "%", "Spell damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
			 "Bonus from intelligence: " + Math.Round(ModdedPlayer.instance.intelligence.Value * ModdedPlayer.instance.SpellDamageperInt * 100, 2) + "%\n" +
			 "Spell damage amplification: " + Math.Round((ModdedPlayer.instance.SpellDamageAmplifier - 1) * 100, 2) + "%\n" +
			 "Damage output amplification" + Math.Round((ModdedPlayer.instance.DamageOutputMultTotal - 1) * 100, 2) + "%");
			Stat("Additional spell damage", Math.Round(ModdedPlayer.instance.SpellDamageBonus) + "", "Spell damage bonus can be increased by perks and inventory items. This is added to spell damage and multiplied by the stat above. Often spells take a fraction of this stat and add it to spell's damage.");
			Stat("Spell cost reduction", Math.Round((1 - ModdedPlayer.instance.SpellCostRatio) * 100) * -1 + "%", "");
			Stat("Spell cost to stamina", Math.Round((ModdedPlayer.instance.SpellCostToStamina) * 100) + "%", "");
			Stat("Cooldown reduction", Math.Round((1 - ModdedPlayer.instance.CoolDownMultipier) * 100) + "%", "");

			Space(20);
			GUI.color = Color.red;
			Image(96, 70);
			GUI.color = Color.white;
			Header("Armor reduction");
			Space(10);
			Stat("Melee", ModdedPlayer.instance.ARreduction_melee + "", "Total melee armor reduction: " + ModdedPlayer.instance.MeleeArmorReduction);
			Stat("Ranged", ModdedPlayer.instance.ARreduction_ranged + "", "Total ranged armor reduction: " + ModdedPlayer.instance.RangedArmorReduction);
			Stat("Any source", ModdedPlayer.instance.ARreduction_all + "", "Decreases armor of enemies hit by either of the sources");

			Space(60);

			Header("Survivor stats");
			Space(10);

			Stat("Movement speed", Math.Round(ModdedPlayer.instance.MoveSpeed * ModdedPlayer.instance.MoveSpeedMult * 100) + "% ms", "Multipier of base movement speed. Base walking speed is equal to " + FPCharacterMod.basewalkSpeed + " units per second, with bonuses it's " + FPCharacterMod.basewalkSpeed * ModdedPlayer.instance.MoveSpeed * ModdedPlayer.instance.MoveSpeedMult + " units per second");
			Stat("Jump power", Math.Round(ModdedPlayer.instance.JumpPower * 100) + "%", "Multipier of base jump power. Increases height of your jumps");
			Stat("Hunger rate", (1 / ModdedPlayer.instance.HungerRate) * 100 + "%", "How much slower is the rate of consuming food compared to normal.");
			Stat("Thirst rate", (1 / ModdedPlayer.instance.ThirstRate) * 100 + "%", "How much slower is the rate of consuming water compared to normal.");
			Stat("Experience gain", ModdedPlayer.instance.ExpFactor * 100 + "%", "Multipier of any experience gained");
			Stat("Magic find", ItemDataBase.MagicFind * 100 + "%", "Affects quantity of items looted from monsters, as well as the chance to get items from non-elite enemies");
			Stat("Massacre duration", ModdedPlayer.instance.MaxMassacreTime + " s", "How long massacres can last");
			Stat("Time on kill", ModdedPlayer.instance.TimeBonusPerKill + " s", "Amount of time that is added to massacre for every kill");
			if (ModdedPlayer.instance.TurboRaft)
				Stat("Turbo raft speed", ModdedPlayer.instance.RaftSpeedMultipier + "%", "Speed multiplier of rafts");

			Space(40);
			Image(90, 70);
			Header("Inventory Stats");
			Space(10);
			foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> pair in ModdedPlayer.instance.ExtraCarryingCapactity)
			{
				string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Value.ID, (pair.Value.Amount > 1), false);
				Stat(item_name, "+" + pair.Value.Amount, "How many extra '" + item_name + "' you can carry. Item ID is " + pair.Value.ID);
			}
			Space(10);
			if (ModdedPlayer.instance.GeneratedResources.Count > 0)
				Header("Generated resources");
			foreach (var pair in ModdedPlayer.instance.GeneratedResources)
			{
				string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Key, (pair.Value > 1), false);
				Stat(item_name, pair.Value.ToString(), "How many '" + item_name + "' you generate daily. Item ID is " + pair.Key);
			}

			if (BookPositionY < Screen.height && BookPositionY > -140 * screenScale)
			{
				Rect labelRect = new Rect(GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease), 85 * screenScale);
				if (GUI.Button(labelRect, "Bugged stats? Click to reset", new GUIStyle(GUI.skin.button)
				{
					font = mainFont,
					fontSize = Mathf.RoundToInt(70 * screenScale),
					alignment = TextAnchor.UpperCenter,
					richText = true,
				}))
				{
					ModdedPlayer.ResetAllStats();
				}
				BookPositionY += 85 * screenScale;
				Rect imageRect = new Rect(400 * screenScale, BookPositionY, Screen.width - 800 * screenScale, 60 * screenScale);
				GUI.DrawTexture(imageRect, ResourceLoader.GetTexture(30));
				BookPositionY += 70 * screenScale;
			}
			else
			{
				BookPositionY += 155 * screenScale;
			}
		}

		#endregion StatsMenu
	}
}