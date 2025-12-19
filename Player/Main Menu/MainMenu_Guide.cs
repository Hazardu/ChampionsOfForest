using System.Collections.Generic;

using ChampionsOfForest.Localization;
using ChampionsOfForest.Player;

using UnityEngine;

using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
	public partial class MainMenu : MonoBehaviour
	{
		private readonly float GuideWidthDecrease = 150;
		private readonly float GuideMargin = 30;


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
				Rect labelRect = new Rect(250 * screenScale + GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, (Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease) - 500 * screenScale)/2f, statStyle.fontSize);
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
				Texture2D tex = ResourceLoader.GetTexture(iconID);
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
			GUI.Label(new Rect(0, 0, 300, 100), Translations.MainMenu_Guide_2 + Translations.MainMenu_Guide_1 + guidePage); 
			int a = -1;

			Bookmarks.Clear();

			BookPositionY = BookScrollAmount;
			SetGuiStylesForGuide();

			if (GUI.Button(new Rect(5f * screenScale, 5f * screenScale, 100 * screenScale, 50 * screenScale), "<", headerstyle))
			{
				ChangePage(guidePage - 1);
			}
			if (GUI.Button(new Rect(Screen.width - 105f * screenScale, 5f * screenScale, 100 * screenScale, 50 * screenScale), ">", headerstyle))
			{
				ChangePage(guidePage + 1);
			}





			if (guidePage == a++)
			{

				Header("Guide will be back after being improved");
				//Header("Basic Information");
				//MarkBookmark("Home");
				//Label("\tExperience");
				//Stat("Current level", ModdedPlayer.instance.level.ToString());
				//Stat("Current experience", ModdedPlayer.instance.ExpCurrent.ToString());
				//Stat("Experience goal", ModdedPlayer.instance.ExpGoal.ToString(), "Next level: " + (ModdedPlayer.instance.level + 1) + " you will need to get this amount of experience:\t " + ModdedPlayer.instance.GetGoalExp(ModdedPlayer.instance.level + 1));
				//Stat("Progress amount: ", (((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal).ToString("P")));
				//Label("\tLevel is the estimation of my power. I must become stronger to survive." +
				//	"\nHigher level allow me to equip better equipment. " +
				//	"\nLeveling up gives me the ability to develop usefull abilities. (Currently you have " + ModdedPlayer.instance.MutationPoints + " mutation points), which you can spend on unlocking spells or perks. ");
				//Space(50);
				//Label("\nSources of experience" +
				//	"\n-Mutants - Enemies give the most experience, it's possible to chain kills to get more exp, and the reward is exp reward is based on bounty." +
				//	"\n-Animals - Experience gained does not increase with difficulty. Good way of gaining experience. Rarer animals like crocodiles and racoons give more experience compared to common like rabbits and lizards" +
				//	"\n-Tall bushes - Give minimum amount of experience." +
				//	"\n-Trees - A bit more than bushes." +
				//	"\n-Effigies - It's possible to gain experience and low rarity items by breaking effigies scattered across the map." +
				//	"\n-Rare consumable - Gives a large amount of experience, it's rarity is orange\n");

				//Space(100);

				//Header("Information - Items");
				//Label("\tEquipement can (e obtained by killing enemies and breaking effigies. Normal enemies can drop a few items on death, if the odds are in your favor. The chance to get any items from a normal enemy is {0}. The amount of items obtained from normal enemies increases with more players in a lobby.\n", 0.1f * ModdedPlayer.Stats.magicFind.Value * ModSettings.DropChanceMultiplier:P) +
				// "Elite enemies always drop items in large amounts.\n" +
				// "\tItems can be equipped by dragging and dropping them onto a right equipment slot or shift+left click. The item will grant it's stats only if you meets item's level requirement. The best tier of items is only obtainable on high difficulties.");
				//Label("By unlocking a perk in the survival category, it's possible to change the stats on your existing items, and reforge unused items into something useful. Reforged item will have the same level as item put into the main crafting slot.");
				//Label("In the inventory, you can compare an item with your equipped item by holding down left shift.");
				//Label("By holding down left shift and clicking on an item, it will be equipped. This server the same purpose as dragging and dropping an item.");
				//Label("By holding down left control and clicking on an item, be used as an ingredient for crafting.");
				//Label("By pressing left alt you toggle a window to show total amount of a stat when you hover over it.");

				//Space(100);
				//Header("Information - Statistics");
				//Label("\tAttributes");
				//Label("Strength - This stat increases melee damage and thorns. It multiplies with melee damage increase.");
				//Label("Agility - This stat increases ranged damage and maximum energy.");
				//Label("Intelligence - This stat increases magic damage and energy regeneration rate.");
				//Label("Vitality - This stat increases maximum health");

				//Space(100);

				//Header("Information - Mutations and Abilities");
				//Label("\tUpon leveling up, the player will receive a upgrade point. Then it's up to the player to use it to unlock a mutation, that will serve as a permanent perk, or to spend two upgrade points to unlock a ability.\n" +
				//	"Abilities are in majority of the cases more powerful than perks, as they cost more and the number of active abilities is limited to 6.\n" +
				//	"Some perks can be bought multiple times for increased effects.\n" +
				//	"\n" +
				//	"Refunding - it is possible to refund all points, to do so, heart of purity needs to be consumed. This item is of yellow rarity, and thus unobtainable on easy and veteran difficulties. Heart of purity will be granted to a player upon reaching level 15, 30 and 40" +
				//	"More points - to gain a point without leveling, a rare item of green rarity needs to be consumed. It permanently adds a upgrade point, and it persists even after refunding. Items to grant additional upgrade points are granted to player to upon reaching level 50, 65 and 75. To obtain them in different means, you need to play on challenge difficulties or higher");
				//Space(100);

				//Header("Information - Enemies");
				//Label("\tEnemies in the forest have adapted to your skill. As they level with you, they become faster and stronger. But speed and strength alone shouldn't be your main concern. There are a lot more dangerous beings out there." +
				//	"\n\n" +
				//	"Common enemies changed slightly. Their health increases with level.\n" +
				//	"A new statistic to enemies is 'Armor'. This property reduces damage taken by the enemies from physical attacks, and partly reduces damage from magical attacks. Armor can be reduced in a number of ways.\n" +
				//	"The easiest way to reduce armor is to use fire. Fire works as a way to crowd control enemies, it renders a few enemies unable to run and attack as they shake off the flames.\n" +
				//	"Other way to reduce armor is to equip items, which reduce armor on hit.\n" +
				//	"If you dont have any way to reduce enemy's armor, damaging them with spells would decrease the reduction from armor by 2/3, allowing you to deal some damage.");
				//Space(30);
				//Label("Elite enemies\n" +
				//	"An elite is a uncommon type of a mutant with increased stats and access to special abilities, that make encounters with them challenging." +
				//	"\nEnemy abilities:");
				//Label("- Steadfast - This defensive ability causes enemy to reduce all damage exceeding a percent of their maximum health. To deal with this kind of ability, damage over time and fast attacks are recommended. This ability counters nuke instances of damage.");
				//Label("- Blizzard - A temporary aura around an enemy, that slows anyone in it's area of effect. Affects movement speed and attack speed. Best way to deal with this is to avoid getting within it's range. Crowd controll from ranged attacks and running seems like the best option.");
				//Label("- Radiance - A permanent aura around an enemy. It deals damage anyone around. The only way of dealing with this is to never get close to the enemy.");
				//Label("- Chains - Roots anyone in a big radius around the elite. The duration this root increases with difficulty. Several abilities that provide resistance to crowd control clear the effects of this ability.");
				//Label("- Black hole - A very strong ability. The spell has a fixed cooldown, and the enemy will attempt to cast it as soon as a player gets within his range effective.");
				//Label("- Trap sphere - Long lasting sphere that forces you to stay inside it until it's effects wears off");
				//Label("- Juggernaut - The enemy is completely immune to crowd control and bleeding.\n");
				//Label("- Gargantuan - Describes an enemy that is bigger, faster, stronger and has more health.");
				//Label("- Tiny - An enemy has decreased size. It's harder to hit it with ranged attacks and most of the melee weapons can only attack the enemy with slow smashes.");
				//Label("- Extra tough - enemy has a lot more health");
				//Label("- Extra deadly - enemy has a lot more damage");
				//Label("- Basher - the enemy stuns on hit. Best way to fight it is to not get hit or parry it's attacks.");
				//Label("- Warping - An ability allowing to teleport. Strong against glass cannon builds, running away and ranged attacks. Weak against melee strikes and a lot of durability.");
				//Label("- Rain Empowerment - If it rains, the enemy gains in strength, speed, armor and size.");
				//Label("- Meteors - Periodically spawns a rain of powerful meteors. They are rather easy to spot and they move at a slow medium speed.");
				//Label("- Flare - Slows and damages me if you stand inside. Heals and makes enemies faster.");
				//Label("- Undead - An enemy upon dieing restores portion of it's health, gets stronger and bigger.");
				//Label("- Plasma cannon - Creates a turret that fires a laser beam that damages players and buildings.");
				//Label("- Poisonous - Enemies gain a attack modifier, that applies a stacking debuff, which deals damage over time. Once hit, it is advised to retreat and wait for the poison stop damaging you.");
				//Label("- Cataclysm - Enemy uses the cataclysm spell to slow you down and damage you.");

				//Header("Changes");
				//Label("Champions of The Forest provides variety of changes to in-game mechanics." +
				//	"\nArmor no longer absorbs all damage. Instead it reduces the damage by 70%." +
				//	"\nPlayer is slowed down if out of stamina (the inner blue bar)" +
				//	"\nTraps no longer instantly kill cannibals. Instead they deal damage." +
				//	"\nDynamite no longer instantly kills enemies. Instead it deals up to 700 damage" +
				//	"\nEnemies have armor and increased health." +
				//	"\nPlayers now take increased damage from fire, frost, drowning, falling, food poisoning and polluted water based on their maximum health" +
				//	"\nPlayers take increased damage from explosives. This affects how much damage the worm does" +
				//	"\nPlayer deal increased damage to other players if friendly fire is enabled.");
			}
			else if (guidePage == a++)
			{


				Header(Translations.MainMenu_Guide_3); 

				Stat(Translations.MainMenu_Guide_4, 
					ModdedPlayer.Stats.strength.GetFormattedAmount(),
					Translations.MainMenu_Guide_5( 
						ModdedPlayer.Stats.meleeDmgFromStr.GetFormattedAmount(),
						ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount()));

				Stat(Translations.MainMenu_Guide_6, 
					ModdedPlayer.Stats.agility.GetFormattedAmount(),
					Translations.MainMenu_Guide_7( 
						ModdedPlayer.Stats.rangedDmgFromAgi.GetFormattedAmount(), ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount(), ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount()));

				Stat(Translations.MainMenu_Guide_8, 
					ModdedPlayer.Stats.vitality.GetFormattedAmount(),
					Translations.MainMenu_Guide_9( 
					ModdedPlayer.Stats.maxHealthFromVit.GetFormattedAmount(), ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.maxHealthFromVit.GetAmount()));

				Stat(Translations.MainMenu_Guide_10,    
					ModdedPlayer.Stats.intelligence.GetFormattedAmount(),
					Translations.MainMenu_Guide_11( 
						ModdedPlayer.Stats.spellDmgFromInt.GetFormattedAmount(), ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount(), ModdedPlayer.Stats.energyRecoveryFromInt.GetFormattedAmount(), (ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.energyRecoveryFromInt.GetAmount()).ToString("P1")));


				Space(60);
				Image(99, 70);
				Header(Translations.MainMenu_Guide_12); 
				Space(10);
				Stat(Translations.MainMenu_Guide_13, 
					ModdedPlayer.Stats.TotalMaxHealth.ToString(),
					Translations.MainMenu_Guide_14(
					ModdedPlayer.ModdedPlayerStats.baseHealth, ModdedPlayer.Stats.maxHealthFromVit.GetAmount() * ModdedPlayer.Stats.vitality.GetAmount(), ModdedPlayer.Stats.maxLife.GetFormattedAmount(), ModdedPlayer.Stats.maxLifeMult.GetFormattedAmount()));

				Stat(Translations.MainMenu_Guide_15, 
					ModdedPlayer.Stats.TotalMaxEnergy.ToString(),
					Translations.MainMenu_Guide_16( 
					ModdedPlayer.ModdedPlayerStats.baseEnergy, ModdedPlayer.Stats.maxEnergy.GetFormattedAmount(), ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount() * ModdedPlayer.Stats.agility.GetAmount(), ModdedPlayer.Stats.maxEnergyMult.GetFormattedAmount()));

				Stat(Translations.MainMenu_Guide_17, 
					ModdedPlayer.Stats.armor.GetFormattedAmount(),
					Translations.MainMenu_Guide_18( 
					ModReferences.DamageReduction(ModdedPlayer.Stats.armor.Value).ToString("P2")));
				Stat("Reduced damage from Elites", 
					(1 - ModdedPlayer.Stats.damageFromElites.GetAmount()).ToString("P"),
					"Decreases the damage taken from elite monsters and their abilities."); 
				Stat(Translations.MainMenu_Guide_21, 
					(1 - ModdedPlayer.Stats.getHitChance.GetAmount()).ToString("P"),
					Translations.MainMenu_Guide_22);    
				Stat(Translations.MainMenu_Guide_23, 
					(1f - ModdedPlayer.Stats.allDamageTaken.GetAmount()).ToString("P"),
					Translations.MainMenu_Guide_24); 
				Stat(Translations.MainMenu_Guide_26, ModdedPlayer.Stats.block.GetFormattedAmount(), Translations.MainMenu_Guide_25); 
				Stat(Translations.Item_1, ModdedPlayer.instance.DamageAbsorbAmount.ToString(), Translations.MainMenu_Guide_27); 
				Stat(Translations.MainMenu_Guide_28, 
					(1 - ModdedPlayer.Stats.fireDamageTaken.GetAmount()).ToString("P"), Translations.MainMenu_Guide_29); 
				Stat(Translations.MainMenu_Guide_30, 
					ModdedPlayer.Stats.TotalThornsDamage.ToString(),
				Translations.MainMenu_Guide_31( 
					ModdedPlayer.Stats.thorns.GetFormattedAmount(), ModdedPlayer.Stats.thornsPerStrenght.GetAmount() * ModdedPlayer.Stats.strength.GetAmount() + ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.thornsPerVit.GetAmount()));

				Space(60);
				Header(Translations.MainMenu_Guide_32); 
				Space(10);

				Stat(Translations.MainMenu_Guide_133, (ModdedPlayer.Stats.TotalStaminaRecoveryAmount).ToString("N2"), //tr
					Translations.MainMenu_Guide_35( 
					ModdedPlayer.Stats.energyRegenMult.GetAmount(), ModdedPlayer.Stats.staminaRegenBase.GetFormattedAmount()));

				Stat(Translations.MainMenu_Guide_37, ModdedPlayer.Stats.energyRecoveryBase.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", Translations.MainMenu_Guide_36( ModdedPlayer.Stats.energyRecoveryBase.GetAmount(), ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier)); 
				Stat(Translations.MainMenu_Guide_39, ModdedPlayer.Stats.energyOnHit.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", Translations.MainMenu_Guide_38( ModdedPlayer.Stats.energyOnHit.GetAmount()));
				Stat(Translations.MainMenu_Guide_41, ModdedPlayer.Stats.lifeRegenBase.GetAmount() * (ModdedPlayer.Stats.lifeRegenMult.GetAmount()) * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", Translations.MainMenu_Guide_40( ModdedPlayer.Stats.lifeRegenBase.GetAmount(), ModdedPlayer.Stats.lifeRegenMult.GetFormattedAmount(), ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));
				Stat(Translations.MainMenu_Guide_43, ModdedPlayer.Stats.lifeOnHit.GetAmount() * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", Translations.MainMenu_Guide_42( ModdedPlayer.Stats.lifeOnHit.GetAmount(), ModdedPlayer.Stats.allRecoveryMult.GetFormattedAmount(), ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));

				Space(60);
				Header(Translations.MainMenu_Guide_44);
				Space(10);
				Stat(Translations.MainMenu_Guide_45, ModdedPlayer.Stats.allDamage.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_46, ModdedPlayer.Stats.critDamage.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_47, ModdedPlayer.Stats.critChance.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_49, ModdedPlayer.Stats.attackSpeed.GetFormattedAmount(), Translations.MainMenu_Guide_49);
				Stat(Translations.MainMenu_Guide_51, ModdedPlayer.Stats.fireDamage.GetFormattedAmount(), Translations.MainMenu_Guide_51);
				Stat(Translations.MainMenu_Guide_53, ModdedPlayer.Stats.chanceToBleed.GetFormattedAmount(), Translations.MainMenu_Guide_53);
				Stat(Translations.MainMenu_Guide_55, ModdedPlayer.Stats.chanceToWeaken.GetFormattedAmount(), Translations.MainMenu_Guide_55);
				Stat(Translations.MainMenu_Guide_57, ModdedPlayer.Stats.chanceToSlow.GetFormattedAmount(), Translations.MainMenu_Guide_57);

				Space(20);
				Image(89, 70);
				Header(Translations.MainMenu_Guide_58);
				Space(10);

				Stat(Translations.MainMenu_Guide_60, ModdedPlayer.Stats.MeleeDamageMult.ToString("P"), Translations.MainMenu_Guide_59( ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount() * 100, (ModdedPlayer.Stats.meleeIncreasedDmg - 1).ToString("P"), (ModdedPlayer.Stats.allDamage - 1).ToString("P")));
				Stat(Translations.MainMenu_Guide_62, ModdedPlayer.Stats.baseMeleeDamage.GetFormattedAmount(), Translations.MainMenu_Guide_62);
				Stat(Translations.MainMenu_Guide_63, ModdedPlayer.Stats.weaponRange.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_64, ModdedPlayer.Stats.heavyAttackDmg.GetFormattedAmount());

				Space(20);
				Image(98, 70);
				Header(Translations.MainMenu_Guide_65);
				Space(10);

				Stat(Translations.MainMenu_Guide_67, ModdedPlayer.Stats.RangedDamageMult.ToString("P"), Translations.MainMenu_Guide_66( (ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount()).ToString("P"), (ModdedPlayer.Stats.rangedIncreasedDmg.GetAmount() - 1).ToString("P"), (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize.GetAmount() ? (ModdedPlayer.Stats.projectileSize.GetAmount() - 1) : 0f).ToString("P"), (ModdedPlayer.Stats.allDamage - 1).ToString("P")));
				Stat(Translations.MainMenu_Guide_69, ModdedPlayer.Stats.baseRangedDamage.GetFormattedAmount(), Translations.MainMenu_Guide_69);
				Stat(Translations.MainMenu_Guide_71, ModdedPlayer.Stats.projectileSpeed.GetFormattedAmount(), Translations.MainMenu_Guide_71);
				Stat(Translations.MainMenu_Guide_73, ModdedPlayer.Stats.projectileSize.GetFormattedAmount(), Translations.MainMenu_Guide_73);
				Stat(Translations.MainMenu_Guide_75, ModdedPlayer.Stats.headShotDamage.GetFormattedAmount(), Translations.MainMenu_Guide_75);
				Stat(Translations.MainMenu_Guide_77, ModdedPlayer.Stats.projectilePierceChance.GetFormattedAmount(), Translations.MainMenu_Guide_77);
				Stat(Translations.MainMenu_Guide_78, ModdedPlayer.Stats.perk_projectileNoConsumeChance.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_79, ModdedPlayer.Stats.perk_thrownSpearCritChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_thrownSpearhellChance.GetAmount() > 0)
					Stat(Translations.MainMenu_Guide_80, ModdedPlayer.Stats.perk_thrownSpearhellChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_81, ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_82, ModdedPlayer.Stats.perk_bulletCritChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_bulletDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_83, ModdedPlayer.Stats.perk_bulletDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_crossbowDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_84, ModdedPlayer.Stats.perk_crossbowDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_bowDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_85, ModdedPlayer.Stats.perk_bowDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.i_CrossfireQuiver.GetAmount())
					Stat(Translations.MainMenu_Guide_86, "");

				Stat(Translations.MainMenu_Guide_87, (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? (4 + ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()) : ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()).ToString("N"));
				Stat(Translations.MainMenu_Guide_89, (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? 1f * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f) : 10 * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f)).ToString(), Translations.MainMenu_Guide_89);

				Space(20);
				Image(110, 70);
				Header(Translations.MainMenu_Guide_90);
				Space(10);

				Stat(Translations.MainMenu_Guide_92, ModdedPlayer.Stats.TotalMagicDamageMultiplier.ToString("P"), Translations.MainMenu_Guide_91( (ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount()).ToString("P"), (ModdedPlayer.Stats.spellDamageMult - 1).ToString("P"), (ModdedPlayer.Stats.allDamage - 1).ToString("P")));
				Stat(Translations.MainMenu_Guide_94, ModdedPlayer.Stats.baseSpellDamage.GetFormattedAmount(), Translations.MainMenu_Guide_94);
				Stat(Translations.MainMenu_Guide_96, (1 - ModdedPlayer.Stats.spellCost.GetAmount()).ToString("P"), Translations.MainMenu_Guide_96);
				Stat(Translations.MainMenu_Guide_98, ModdedPlayer.Stats.SpellCostToStamina.ToString("P"), Translations.MainMenu_Guide_98);
				Stat(Translations.MainMenu_Guide_99, (1 - ModdedPlayer.Stats.cooldown.GetAmount()).ToString("P"));

				Space(20);
				GUI.color = Color.red;
				Image(96, 70);
				GUI.color = Color.white;
				Header(Translations.MainMenu_Guide_100);
				Space(10);
				Stat(Translations.MainMenu_Guide_58, ModdedPlayer.Stats.meleeArmorPiercing.ToString(), Translations.MainMenu_Guide_102( ModdedPlayer.Stats.TotalMeleeArmorPiercing.ToString()));
				Stat(Translations.MainMenu_Guide_65, ModdedPlayer.Stats.rangedArmorPiercing.ToString(), Translations.MainMenu_Guide_103( ModdedPlayer.Stats.TotalRangedArmorPiercing.ToString()));
				Stat(Translations.MainMenu_Guide_104, ModdedPlayer.Stats.thornsArmorPiercing.ToString(), Translations.MainMenu_Guide_103( ModdedPlayer.Stats.TotalThornsArmorPiercing.ToString()));
				Stat(Translations.MainMenu_Guide_106, ModdedPlayer.Stats.allArmorPiercing.ToString(), Translations.MainMenu_Guide_106);
			}
			else if (guidePage == a++)
			{
				Header(Translations.MainMenu_Guide_107);
				Space(10);

				Stat(Translations.MainMenu_Guide_109, ModdedPlayer.Stats.mOVEMENT_SPEED.ToString(), Translations.MainMenu_Guide_108(FPCharacterMod.basewalkSpeed, FPCharacterMod.basewalkSpeed * ModdedPlayer.Stats.mOVEMENT_SPEED));
				Stat(Translations.MainMenu_Guide_111, ModdedPlayer.Stats.jumpPower.ToString(), Translations.MainMenu_Guide_111);
				Stat(Translations.MainMenu_Guide_113, (1 / ModdedPlayer.Stats.perk_hungerRate).ToString("P"), Translations.MainMenu_Guide_113);
				Stat(Translations.MainMenu_Guide_115, (1 / ModdedPlayer.Stats.perk_thirstRate).ToString("P"), Translations.MainMenu_Guide_115);
				Stat(Translations.MainMenu_Guide_117, ModdedPlayer.Stats.expGain.ToString(), Translations.MainMenu_Guide_117);
				Stat(Translations.MainMenu_Guide_120, ModdedPlayer.Stats.maxMassacreTime.ToString() + "s", Translations.MainMenu_Guide_118);
				Stat(Translations.MainMenu_Guide_122, ModdedPlayer.Stats.timeBonusPerKill.ToString() +"s", Translations.MainMenu_Guide_121);
				if (ModdedPlayer.Stats.perk_turboRaftOwners.GetAmount() > 0)
					Stat(Translations.MainMenu_Guide_124, ModdedPlayer.Stats.perk_RaftSpeedMultipier.ToString(), Translations.MainMenu_Guide_124);
				Stat(Translations.MainMenu_Guide_126, ModdedPlayer.Stats.magicFind_quantity.Value.ToString("P"), Translations.MainMenu_Guide_126);
				foreach (var mfStat in ModdedPlayer.Stats.magicFind_quantity.OtherPlayerValues)
				{
					Stat(mfStat.Key + Translations.MainMenu_Guide_128, mfStat.Value.ToString("P"), Translations.MainMenu_Guide_128);
				}

				Space(40);
				Image(90, 70);
				Header(Translations.MainMenu_Guide_129);
				Space(10);
				foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> pair in ModdedPlayer.instance.ExtraCarryingCapactity)
				{
					string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Value.ID, (pair.Value.Amount > 1), false);
					Stat(item_name, "+" + pair.Value.Amount, Translations.MainMenu_Guide_130( item_name));
				}
				Space(10);
				if (ModdedPlayer.instance.GeneratedResources.Count > 0)
					Header(Translations.MainMenu_Guide_134); //tr
				foreach (var pair in ModdedPlayer.instance.GeneratedResources)
				{
					string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Key, (pair.Value > 1), false);
					Stat(item_name, pair.ToString(), Translations.MainMenu_Guide_131( item_name));
				}
			}
			else if (guidePage == a++)
			{
				if (BookPositionY < Screen.height && BookPositionY > -140 * screenScale)
				{
					Rect labelRect = new Rect(GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease), 85 * screenScale);
					if (GUI.Button(labelRect, Translations.MainMenu_Guide_132, new GUIStyle(GUI.skin.button)
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

					Header("Player Levels");
					Space(10);

					foreach (var pair in ModReferences.PlayerLevels)
					{
						Stat("Level of " + pair.Key, pair.Value.ToString("N"));

					}
				}
				else
				{
					BookPositionY += 155 * screenScale;
				}
			}

		}

		void ChangePage(int pageNumber)
		{
			guidePage = Mathf.Clamp(pageNumber, 0, 3);
			BookScrollAmount = 0;
			BookScrollAmountGoal = 0;
		}
	}
}