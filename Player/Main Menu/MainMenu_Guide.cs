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
			int margin = Mathf.RoundToInt(20 * screenScale);
			statStyleTooltip = new GUIStyle(GUI.skin.label)
			{
				font = mainFont,
				fontSize = Mathf.RoundToInt(20 * screenScale),
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
		public int guidePage;
		private void DrawGuide()
		{
			GUI.Label(new Rect(0, 0, 300, 100), "Page: " + guidePage);
			int a = 0;

			Bookmarks.Clear();
			
			BookPositionY = BookScrollAmount;
			SetGuiStylesForGuide();

			if (GUI.Button(new Rect(5f * screenScale, 5f * screenScale, 100 * screenScale, 50 * screenScale), "<", headerstyle))
			{
				ChangePage(guidePage - 1);
			}
			if (GUI.Button(new Rect(Screen.width -105f * screenScale, 5f * screenScale, 100 * screenScale, 50 * screenScale), ">", headerstyle))
			{
				ChangePage(guidePage +1);
			}





			if (guidePage == a++)
			{

				Header("Basic Information");
				MarkBookmark("Home");
				Label("\tExperience");
				Stat("Current level", ModdedPlayer.instance.level.ToString());
				Stat("Current experience", ModdedPlayer.instance.ExpCurrent.ToString());
				Stat("Experience goal", ModdedPlayer.instance.ExpGoal.ToString(), "Next level: " + (ModdedPlayer.instance.level + 1) + " you will need to get this amount of experience:\t " + ModdedPlayer.instance.GetGoalExp(ModdedPlayer.instance.level + 1));
				Stat("Progress amount: ", (((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal).ToString("P")));
				Label("\tLevel is the estimation of my power. I must become stronger to survive." +
					"\nHigher level allow me to equip better equipement. " +
					"\nLeveling up gives me the ability to develop usefull abilities. (Currently you have " + ModdedPlayer.instance.MutationPoints + " mutation points), which you can spend on unlocking spells or perks. ");
				Space(50);
				Label("\nSources of experience" +
					"\n-Mutants - Enemies give the most experience, it's possible to chain kills to get more exp, and the reward is exp reward is based on bounty." +
					"\n-Animals - Experience gained does not increase with difficulty. Good way of gaining experience. Rarer animals like crocodiles and racoons give more experience compared to common like rabbits and lizards" +
					"\n-Tall bushes - Give minium amount of experience." +
					"\n-Trees - A bit more than bushes." +
					"\n-Effigies - It's possible to gain experience and low rarity items by breaking effigies scattered across the map." +
					"\n-Rare consumable - Gives a large amount of experience, it's rarity is orange\n");

				Space(100);

				Header("Information - Items");
				Label($"\tEquipement can be obtained by killing enemies and breaking effigies. Normal enemies can drop a few items on death, if the odds are in your favor. The chance to get any items from a normal enemy is {0.1f* ModdedPlayer.Stats.magicFind.Value* ModSettings.DropChanceMultiplier:P}. The amount of items obtained from normal enemies increases with more players in a lobby.\n" +
				 "Elite enemies always drop items in large amounts.\n" +
				 "\tItems can be equipped by dragging and dropping them onto a right equipement slot or shift+left click. The item will grant it's stats only if you meets item's level requirement. The best tier of items is only obtainable on high difficulties.");
				Label("By unlocking a perk in the survival category, it's possible to change the stats on your existing items, and reforge unused items into something useful. Reforged item will have the same level as item put into the main crafting slot.");
				Label("In the inventory, you can compare an item with your equipped item by holding down left shift.");
				Label("By holding down left shift and clicking on an item, it will be equipped. This server the same purpose as dragging and dropping an item.");
				Label("By holding down left control and clicking on an item, be used as an ingredient for crafting.");
				Label("By pressing left alt you toggle a window to show total amount of a stat when you hover over it.");
				
				Space(100);
				Header("Information - Statistics");
				Label("\tAttributes");
				Label("Strength - This stat increases melee damage and thorns. It multiplies with melee damage increase.");
				Label("Agility - This stat increases ranged damage and maximum energy.");
				Label("Intelligence - This stat increases magic damage and energy regeneration rate.");
				Label("Vitality - This stat increases maximum health");

				Space(100);

				Header("Information - Mutations and Abilities");
				Label("\tUpon leveling up, the player will recieve a upgrade point. Then it's up to the player to use it to unlock a mutation, that will serve as a permanent perk, or to spend two upgrade points to unlock a ability.\n" +
					"Abilities are in majority of the cases more powerful than perks, as they cost more and the number of active abilities is limitied to 6.\n" +
					"Some perks can be bought multiple times for increased effects.\n" +
					"\n" +
					"Refunding - it is possible to refund all points, to do so, heart of purity needs to be consumed. This item is of yellow rarity, and thus unobtainable on easy and veteran difficulties. Heart of purity will be granted to a player upon reaching level 15, 30 and 40" +
					"More points - to gain a point without leveling, a rare item of green rarity needs to be consumed. It permamently adds a upgrade point, and it persists even after refunding. Items to grant additional upgrade points are granted to player to upon reaching level 50, 65 and 75. To obtain them in different means, you need to play on challenge difficulties or higher");
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
					"\nDynamite no longer instantly kills enemies. Instead it deals up to 700 damage" +
					"\nEnemies have armor and increased health." +
					"\nPlayers now take increased damage from fire, frost, drowning, falling, food poisoning and polluted water based on their maximum health" +
					"\nPlayers take increased damage from explosives. This affects how much damage the worm does" +
					"\nPlayer deal increased damage to other players if friendly fire is enabled.");
			}
			else if (guidePage == a++)
			{


				Header("Statistics");
				Stat("Strength", ModdedPlayer.Stats.strength.GetFormattedAmount() + " str", "Increases melee damage by " + ModdedPlayer.Stats.meleeDmgFromStr.GetFormattedAmount() + " for every 1 point of strength. Current bonus melee damage from strength [" + ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount() *100+ "%]");
				Stat("Agility", ModdedPlayer.Stats.agility.GetFormattedAmount() + " agi", "Increases ranged damage by " + ModdedPlayer.Stats.rangedDmgFromAgi.GetFormattedAmount() + " for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount() * 100 + "%]\n" +
					"Increases maximum energy by " + ModdedPlayer.Stats.maxEnergyFromAgi.GetFormattedAmount() + " for every 1 point of agility. Current bonus ranged damage from agility [" + ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount()*100 + "%]");
				Stat("Vitality", ModdedPlayer.Stats.vitality.GetFormattedAmount() + " vit", "Increases health by " + ModdedPlayer.Stats.maxHealthFromVit.GetFormattedAmount() + " for every 1 point of vitality. Current bonus health from vitality [" + ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.maxHealthFromVit.GetAmount() + "]");
				Stat("Intelligence", ModdedPlayer.Stats.intelligence.GetFormattedAmount() + " int", "Increases spell damage by " + ModdedPlayer.Stats.spellDmgFromInt.GetFormattedAmount() + " for every 1 point of intelligence. Current bonus spell damage from intelligence [" + ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount() + "]\n" +
					"Increases stamina regen by " + ModdedPlayer.Stats.energyRecoveryFromInt.GetFormattedAmount() + " for every 1 point of intelligence. Current bonus stamina regen from intelligence [" + ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.energyRecoveryFromInt.GetAmount()*100 + "%]");
			

				Space(60);
				Image(99, 70);
				Header("Defense");
				Space(10);
				Stat("Max health", ModdedPlayer.Stats.TotalMaxHealth.ToString(), "Total health pool.\n" +
					"Base health: " + ModdedPlayer.ModdedPlayerStats.baseHealth +
					"\nBonus health: " + ModdedPlayer.Stats.maxHealth.GetFormattedAmount() +
					"\nHealth from vitality: " + ModdedPlayer.Stats.maxHealthFromVit.GetAmount() * ModdedPlayer.Stats.vitality.GetAmount() +
					"\nHealth multipier: " + ModdedPlayer.Stats.maxHealthMult.GetFormattedAmount());
				Stat("Max energy", ModdedPlayer.Stats.TotalMaxEnergy.ToString(), "Total energy pool.\n" +
					"Base energy: " + ModdedPlayer.ModdedPlayerStats.baseEnergy +
					"\nBonus energy: " + ModdedPlayer.Stats.maxEnergy.GetFormattedAmount() +
					"\nEnergy from agility: " + ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount() * ModdedPlayer.Stats.agility.GetAmount() +
					"\nEnergy multipier: " + ModdedPlayer.Stats.maxEnergyMult.GetFormattedAmount());
				Stat("Armor", ModdedPlayer.Stats.armor.GetFormattedAmount(), $"Armor provides physical damage reduction.\nPhysical damage reduction from {ModdedPlayer.Stats.armor.GetFormattedAmount()} armor is equal to {1-ModReferences.DamageReduction(ModdedPlayer.Stats.armor.Value):P}");
				Stat("Magic resistance", (1 - ModdedPlayer.Stats.magicDamageTaken.GetAmount()).ToString("P"), "Magic damage reduction. Decreases damage from enemy abilities.");
				Stat("Dodge Chance", (1 - ModdedPlayer.Stats.getHitChance.GetAmount()).ToString("P"), "A chance to avoid entire instance of damage. Works only for physical damage sources. This means dodge is ineffective against fire, poison, cold, various spells. Meteor rain ability deals physical damage and can be dodged");
				Stat("Damage taken reduction", (1f - ModdedPlayer.Stats.allDamageTaken.GetAmount()).ToString("P"));
				Stat("Block", ModdedPlayer.Stats.block.GetFormattedAmount());
				Stat("Temporary health", ModdedPlayer.instance.DamageAbsorbAmount.ToString(),"One way to obtain temporary health is to use sustain shield ability");
				Stat("Fire resistance", (1 - ModdedPlayer.Stats.fireDamageTaken.GetAmount()).ToString("P"));
				Stat("Thorns", ModdedPlayer.Stats.TotalThornsDamage.ToString(), $"Thorns inflict damage to attacking enemies. Thorns from gear and mutations {ModdedPlayer.Stats.thorns.GetFormattedAmount()}. Thorns from attributes {(ModdedPlayer.Stats.thornsPerStrenght.GetAmount() * ModdedPlayer.Stats.strength.GetAmount() + ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.thornsPerVit.GetAmount())}.\nThorns damage is applied to attackers even when you are blocking");
		
				Space(60);
				Header("Recovery");
				Space(10);


				Stat("Total Stamina recovery per second", ModdedPlayer.Stats.TotalStaminaRecoveryAmount.ToString() + "", "Stamina regen is temporairly paused after sprinting");
				Stat("Stamina per second", ModdedPlayer.Stats.staminaRecoveryperSecond.GetAmount() *  ModdedPlayer.Stats.staminaPerSecRate.GetAmount() + "", "Stamina per second: " + ModdedPlayer.Stats.staminaRecoveryperSecond.GetAmount() + "\nStamina regen bonus: " + ModdedPlayer.Stats.staminaPerSecRate.GetFormattedAmount());

				Stat("Energy per second", ModdedPlayer.Stats.energyRecoveryperSecond.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", "Energy per second: " + ModdedPlayer.Stats.energyRecoveryperSecond.GetAmount() + "\nStamina and energy regen multipier: " + ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier);
				Stat("Energy on hit", ModdedPlayer.Stats.energyOnHit.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", "Energy on hit from items and perks: " + ModdedPlayer.Stats.energyOnHit.GetAmount());
				Stat("Health per second", ModdedPlayer.Stats.healthRecoveryPerSecond.GetAmount() * (ModdedPlayer.Stats.healthPerSecRate.GetAmount()) * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", "Health per second: " + ModdedPlayer.Stats.healthRecoveryPerSecond.GetAmount() + "\nStamina regen bonus: " + ModdedPlayer.Stats.healthPerSecRate.GetFormattedAmount() + "\nAll Recovery Amplification: " + (ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));
				Stat("Health on hit", ModdedPlayer.Stats.healthOnHit.GetAmount() * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", "Health on hit: " + ModdedPlayer.Stats.healthOnHit.GetAmount() + "\nHealth regen bonus: " + ModdedPlayer.Stats.allRecoveryMult.GetFormattedAmount() + "\nAll Healing Amplification: " + (ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));
		
				Space(60);
				Header("Attack");
				Space(10);
				Stat("All damage", ModdedPlayer.Stats.allDamage.GetFormattedAmount());
				Stat("Critical hit damage", ModdedPlayer.Stats.critDamage.GetFormattedAmount());
				Stat("Critical hit chance", ModdedPlayer.Stats.critChance.GetFormattedAmount());
				Stat("Attack speed", ModdedPlayer.Stats.attackSpeed.GetFormattedAmount(), "Increases the speed of player actions - weapon swinging, reloading guns and drawing bows");
				Stat("Fire damage", ModdedPlayer.Stats.fireDamage.GetFormattedAmount(), "Increases fire damage");
				Stat("Bleed chance", ModdedPlayer.Stats.chanceToBleed.GetFormattedAmount(), "Bleeding enemies take 5% of damage dealt per second for 10 seconds");
				Stat("Weaken chance", ModdedPlayer.Stats.chanceToWeaken.GetFormattedAmount(), "Weakened enemies take 20% increased damage from all players.");
				Stat("Slow chance", ModdedPlayer.Stats.chanceToSlow.GetFormattedAmount(), "Slowed enemies move and attack 50% slower");
		
				Space(20);
				Image(89, 70);
				Header("Melee");
				Space(10);

				Stat("Melee damage", ModdedPlayer.Stats.MeleeDamageMult.ToString("P"), "Melee damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
				   "Bonus from strength: " + ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount()*100 + "%\n" +
				   "Increase to melee damage: " + (ModdedPlayer.Stats.meleeIncreasedDmg-1).ToString("P") + "\n" +
				   "Increase to all damage: " + (ModdedPlayer.Stats.allDamage- 1).ToString("P"));
				Stat("Additional melee weapon damage", ModdedPlayer.Stats.meleeFlatDmg.GetFormattedAmount(), "Melee damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to weapon damage and multiplied by the stat above");
				Stat("Melee range", ModdedPlayer.Stats.weaponRange.GetFormattedAmount());
				Stat("Heavy attack damage", ModdedPlayer.Stats.heavyAttackDmg.GetFormattedAmount());
		
				Space(20);
				Image(98, 70);
				Header("Ranged");
				Space(10);

				Stat("Ranged damage", ModdedPlayer.Stats.RangedDamageMult.ToString("P"), "Ranged damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
				"Bonus from agility: " + (ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount()).ToString("P") + "\n" +
				"Increase to ranged damage: " + (ModdedPlayer.Stats.rangedIncreasedDmg.GetAmount()-1).ToString("P") + "\n" +
				"From size matters perk: " + (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize.GetAmount() ? (ModdedPlayer.Stats.projectileSize.GetAmount()-1)*2 : 0f).ToString("P") +
				"\nIncrease to all damage: " + (ModdedPlayer.Stats.allDamage - 1).ToString("P"));
				Stat("Additional ranged weapon damage", ModdedPlayer.Stats.rangedFlatDmg.GetFormattedAmount(), "Ranged damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to projectile damage and multiplied by the stat above");
				Stat("Projectile speed", ModdedPlayer.Stats.projectileSpeed.GetFormattedAmount(), "Faster projectiles fly further and fall slower");
				Stat("Projectile size", ModdedPlayer.Stats.projectileSize.GetFormattedAmount(), "Bigger projectiles allow to land headshots easier. Most projectiles still can hit only 1 target.");
				Stat("Headshot damage", ModdedPlayer.Stats.headShotDamage.GetFormattedAmount(), "Damage multipier on headshot");
				Stat("Projectile pierce chance", ModdedPlayer.Stats.projectilePierceChance.GetFormattedAmount(), "Chance for a projectile to pierce a bone of an enemy and fly right through to hit objects behind the enemy. Increasing this value beyond 100% will make your projectiles always pierce on first enemy contact, and any further hits will also have a chance to pierce.");
				Stat("No consume chance", ModdedPlayer.Stats.perk_projectileNoConsumeChance.GetFormattedAmount());
				Stat("Spear headshot chance", ModdedPlayer.Stats.perk_thrownSpearCritChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_thrownSpearhellChance.GetAmount() > 0)
					Stat("Double spear chance", ModdedPlayer.Stats.perk_thrownSpearhellChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetAmount() != 1)
					Stat("Spear damage", ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetFormattedAmount());
				Stat("Bullet headshot chance", ModdedPlayer.Stats.perk_bulletCritChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_bulletDamageMult.GetAmount() != 1)
					Stat("Bullet damage", ModdedPlayer.Stats.perk_bulletDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_crossbowDamageMult.GetAmount() != 1)
					Stat("Crossbow damage", ModdedPlayer.Stats.perk_crossbowDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_bowDamageMult.GetAmount() != 1)
					Stat("Bow damage", ModdedPlayer.Stats.perk_bowDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.i_CrossfireQuiver.GetAmount())
					Stat("Shooting an enemy creates magic arrows", "");

				Stat("Multishot Projectiles", (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? (4 + ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()) : ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()).ToString("N"));
				Stat("Multishot Cost", (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? 1f * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f) : 10 * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f)).ToString(), "Formula for multishot cost in energy is (Multishot Projectiles ^ 1.75) * 10"); ;
		
				Space(20);
				Image(110, 70);
				Header("Magic");
				Space(10);

				Stat("Spell damage", ModdedPlayer.Stats.TotalMagicDamageMultiplier.ToString("P"), "Spell damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n" +
				"Bonus from intelligence: " + (ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount()).ToString("P") + "\n" +
				"Increase to spell damage: " + (ModdedPlayer.Stats.spellIncreasedDmg-1).ToString("P") + "\n" +
				"Increase to all damage: " + (ModdedPlayer.Stats.allDamage - 1).ToString("P"));
				Stat("Additional spell damage", ModdedPlayer.Stats.spellFlatDmg.GetFormattedAmount(), "Spell damage bonus can be increased by perks and inventory items. This is added to spell damage and multiplied by the stat above. Often spells take a fraction of this stat and add it to spell's damage.");
				Stat("Spell cost reduction", (1 - ModdedPlayer.Stats.spellCost.GetAmount()).ToString("P"));
				Stat("Spell cost redirected to stamina", ModdedPlayer.Stats.SpellCostToStamina.ToString("P"));
				Stat("Cooldown reduction", (1 - ModdedPlayer.Stats.cooldown.GetAmount()).ToString("P"));
	
				Space(20);
				GUI.color = Color.red;
				Image(96, 70);
				GUI.color = Color.white;
				Header("Armor reduction");
				Space(10);
				Stat("Melee", ModdedPlayer.Stats.meleeArmorPiercing.GetAmount() + "", "Total melee armor reduction: " + ModdedPlayer.Stats.TotalMeleeArmorPiercing.ToString());
				Stat("Ranged", ModdedPlayer.Stats.rangedArmorPiercing.GetAmount() + "", "Total ranged armor reduction: " + ModdedPlayer.Stats.TotalRangedArmorPiercing.ToString());
				//Stat("Thorns", ModdedPlayer.Stats.thornsArmorPiercing.GetAmount() + "", "Total thorns armor reduction: " + ModdedPlayer.Stats.TotalThornsArmorPiercing.ToString());
				Stat("Any source", ModdedPlayer.Stats.allArmorPiercing.GetAmount() + "", "Decreases armor of enemies hit by either of the sources");
			}
			else if (guidePage == a++)
			{
				Header("Survivor stats");
				Space(10);

				Stat("Movement speed", ModdedPlayer.Stats.movementSpeed.GetAmount().ToString(), "Multipier of base movement speed. Base walking speed is equal to " + FPCharacterMod.basewalkSpeed + " feet per second, with bonuses it's " + FPCharacterMod.basewalkSpeed * ModdedPlayer.Stats.movementSpeed.GetAmount() + " feet/second");
				Stat("Jump power", ModdedPlayer.Stats.jumpPower.GetAmount().ToString(), "Multipier of base jump power. Increases height of your jumps");
				Stat("Hunger rate", (1 / ModdedPlayer.Stats.perk_hungerRate).ToString("P"), "How much slower is the rate of consuming food compared to normal.");
				Stat("Thirst rate", (1 / ModdedPlayer.Stats.perk_thirstRate).ToString("P"), "How much slower is the rate of consuming water compared to normal.");
				Stat("Experience gain", ModdedPlayer.Stats.expGain.GetFormattedAmount(), "Multipier of any experience gained");
				Stat("Massacre duration", ModdedPlayer.Stats.maxMassacreTime.GetAmount() + " s", "How long massacres can last");
				Stat("Time on kill", ModdedPlayer.Stats.timeBonusPerKill.GetAmount() + " s", "Amount of time that is added to massacre for every kill");
				if (ModdedPlayer.Stats.perk_turboRaftOwners.GetAmount() > 0)
					Stat("Turbo raft speed", ModdedPlayer.Stats.perk_RaftSpeedMultipier.GetFormattedAmount(), "Speed multiplier of rafts. Other player's items and perks also affect this value");
				Stat("Magic find", ModdedPlayer.Stats.magicFind.Value.ToString("P"), "Affects rarity of items looted from monsters, as well as the chance to get items from non-elite enemies. Increases globally, and this value is affected by every player. ");
				foreach (var mfStat in ModdedPlayer.Stats.magicFind.OtherPlayerValues)
				{
					Stat(mfStat.Key+"'s Magic Find", mfStat.Value.ToString("P"), "Magic find from other players");
				}
	
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
					Stat(item_name, pair.ToString(), "How many '" + item_name + "' you generate daily. Item ID is " + pair.Key);
				}
			}
			else if (guidePage == a++)
			{
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

		}

		void ChangePage(int pageNumber)
		{
			guidePage = Mathf.Clamp(pageNumber,0,3);
			BookScrollAmount = 0;
			BookScrollAmountGoal = 0;
		}
		#endregion StatsMenu
	}
}