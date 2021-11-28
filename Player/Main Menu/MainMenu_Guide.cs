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
			GUI.Label(new Rect(0, 0, 300, 100), Translations.MainMenu_Guide_2/*og:Page*/ + Translations.MainMenu_Guide_1/*og:: */ + guidePage); //tr
			int a = 0;

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
				//Label(string.Format("\tEquipement can be obtained by killing enemies and breaking effigies. Normal enemies can drop a few items on death, if the odds are in your favor. The chance to get any items from a normal enemy is {0}. The amount of items obtained from normal enemies increases with more players in a lobby.\n", 0.1f * ModdedPlayer.Stats.magicFind.Value * ModSettings.DropChanceMultiplier:P) +
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


				Header(Translations.MainMenu_Guide_3/*og:Attributes*/); //tr

				Stat(Translations.MainMenu_Guide_4/*og:Strength*/, //tr
					ModdedPlayer.Stats.strength.GetFormattedAmount(),
					string.Format(Translations.MainMenu_Guide_5/*og:Increases melee damage by {0} for every 1 point of strength for a total of {1:P1}*/, //tr
						ModdedPlayer.Stats.meleeDmgFromStr.GetFormattedAmount(),
						ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount()));

				Stat(Translations.MainMenu_Guide_6/*og:Agility*/, //tr
					ModdedPlayer.Stats.agility.GetFormattedAmount(),
					string.Format(Translations.MainMenu_Guide_7/*og:Increases ranged damage by {0} for every point of agility for a total of {1:P1}. \nIncreases maximum energy by {2} for every point of agility*/, //tr
						ModdedPlayer.Stats.rangedDmgFromAgi.GetFormattedAmount(), ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount(), ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount()));

				Stat(Translations.MainMenu_Guide_8/*og:Vitality*/, //tr
					ModdedPlayer.Stats.vitality.GetFormattedAmount(),
					string.Format(Translations.MainMenu_Guide_9/*og:Increases health by {0} for every  point of vitality for a total of {1}*/, //tr
					ModdedPlayer.Stats.maxHealthFromVit.GetFormattedAmount(), ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.maxHealthFromVit.GetAmount()));

				Stat(Translations.MainMenu_Guide_10/*og:Intelligence*/,    //tr
					ModdedPlayer.Stats.intelligence.GetFormattedAmount(),
					string.Format(Translations.MainMenu_Guide_11/*og:Increases spell damage by {0} for every point of intelligence for a total of {1}. \n Increases stamina regen by {2} for every point of intelligence for a total of {3}.*/, //tr
						ModdedPlayer.Stats.spellDmgFromInt.GetFormattedAmount(), ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount(), ModdedPlayer.Stats.energyRecoveryFromInt.GetFormattedAmount(), (ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.energyRecoveryFromInt.GetAmount()).ToString("P1")));


				Space(60);
				Image(99, 70);
				Header(Translations.MainMenu_Guide_12/*og:Defense*/); //tr
				Space(10);
				Stat(Translations.MainMenu_Guide_13/*og:Health*/, //tr
					ModdedPlayer.Stats.TotalMaxHealth.ToString(),
					string.Format(Translations.MainMenu_Guide_14/*og:Total health pool.\nAll players start with base health pool of {0}.\nHealth points from vitality: {1} \nAdditional health points: {2}\nHealth increase bonuses: {3}*/,//tr
					ModdedPlayer.ModdedPlayerStats.baseHealth, ModdedPlayer.Stats.maxHealthFromVit.GetAmount() * ModdedPlayer.Stats.vitality.GetAmount(), ModdedPlayer.Stats.maxHealth.GetFormattedAmount(), ModdedPlayer.Stats.maxHealthMult.GetFormattedAmount()));

				Stat(Translations.MainMenu_Guide_15/*og:Energy*/, //tr
					ModdedPlayer.Stats.TotalMaxEnergy.ToString(),
					string.Format(Translations.MainMenu_Guide_16/*og:Total energy pool.\nAll players start with base energy pool of {0}\n Energy from agility: {1}\nEnergy multiplier: {2}*/, //tr
					ModdedPlayer.ModdedPlayerStats.baseEnergy, ModdedPlayer.Stats.maxEnergy.GetFormattedAmount(), ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount() * ModdedPlayer.Stats.agility.GetAmount(), ModdedPlayer.Stats.maxEnergyMult.GetFormattedAmount()));

				Stat(Translations.MainMenu_Guide_17/*og:Armor*/, //tr
					ModdedPlayer.Stats.armor.GetFormattedAmount(),
					string.Format(Translations.MainMenu_Guide_18/*og:Armor provides physical damage reduction.\nPhysical damage reduction from armor is equal to {0}*/, //tr
					ModReferences.DamageReduction(ModdedPlayer.Stats.armor.Value).ToString("P2")));
				Stat(Translations.MainMenu_Guide_19/*og:Magic Resistance*/, //tr
					(1 - ModdedPlayer.Stats.magicDamageTaken.GetAmount()).ToString("P"),
					Translations.MainMenu_Guide_20/*og:Magic damage reduction. Decreases damage from enemy abilities.*/);  //tr
				Stat(Translations.MainMenu_Guide_21/*og:Dodge Chance*/, //tr
					(1 - ModdedPlayer.Stats.getHitChance.GetAmount()).ToString("P"),
					Translations.MainMenu_Guide_22/*og:A chance to avoid entire instance of physical damage. Dodge is ineffective against fire, poison, cold and majority of spells.\nMeteor rain ability deals physical damage and can be dodged, but the ignition from it cannot.*/);    //tr
				Stat(Translations.MainMenu_Guide_23/*og:Damage Reduction*/, //tr
					(1f - ModdedPlayer.Stats.allDamageTaken.GetAmount()).ToString("P"),
					Translations.MainMenu_Guide_24/*og:Reduces damage taken from all sources*/); //tr
				Stat(Translations.MainMenu_Guide_26/*og:Damage Block*/, ModdedPlayer.Stats.block.GetFormattedAmount(), Translations.MainMenu_Guide_25/*og:Every instance of physical damage is reduced by the block amount. Block is applied after damage reduction modifiers.*/); //tr
				Stat(Translations.Item_1/*og:Shield*/, ModdedPlayer.instance.DamageAbsorbAmount.ToString(), Translations.MainMenu_Guide_27/*og:Additional health that will disappear after a time period. This temporary health can be obtained with sustain shield ability and some items*/); //tr
				Stat(Translations.MainMenu_Guide_28/*og:Fire Resistance*/, //tr
					(1 - ModdedPlayer.Stats.fireDamageTaken.GetAmount()).ToString("P"), Translations.MainMenu_Guide_29/*og:Reduced damage from ignition, radiance and a number of other fire-attribute spells.*/); //tr
				Stat(Translations.MainMenu_Guide_30/*og:Thorns Damage*/, //tr
					ModdedPlayer.Stats.TotalThornsDamage.ToString(),
				string.Format(Translations.MainMenu_Guide_31/*og:Thorns inflict damage to attacking enemies. Thorns from gear and mutations {0}. Thorns from attributes {1}.\nThorns damage is applied to attackers even when you are blocking*/, //tr
					ModdedPlayer.Stats.thorns.GetFormattedAmount(), ModdedPlayer.Stats.thornsPerStrenght.GetAmount() * ModdedPlayer.Stats.strength.GetAmount() + ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.thornsPerVit.GetAmount()));

				Space(60);
				Header(Translations.MainMenu_Guide_32/*og:Recovery*/); //tr
				Space(10);


				Stat(Translations.MainMenu_Guide_34/*og:Total Stamina recovery per second*/, ModdedPlayer.Stats.TotalStaminaRecoveryAmount.ToString(), Translations.MainMenu_Guide_34/*og:Stamina regeneration is temporarily paused after sprinting*/); //tr
				Stat("Stamina per second", (ModdedPlayer.Stats.staminaRecoveryperSecond.GetAmount() * ModdedPlayer.Stats.staminaPerSecRate.GetAmount()).ToString("N2"),
					string.Format(Translations.MainMenu_Guide_35/*og:Flat stamina regeneration bonus: {0} per second. Increase to stamina regeneration: {1}*/, //tr
					ModdedPlayer.Stats.staminaRecoveryperSecond.GetAmount(), ModdedPlayer.Stats.staminaPerSecRate.GetFormattedAmount()));

				Stat(Translations.MainMenu_Guide_37/*og:Energy per second*/, ModdedPlayer.Stats.energyRecoveryperSecond.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", string.Format(Translations.MainMenu_Guide_37/*og:Energy per second: {0}\nStamina and energy regen multipier: {1}*/, ModdedPlayer.Stats.energyRecoveryperSecond.GetAmount(), ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier)); //tr
				Stat(Translations.MainMenu_Guide_39/*og:Energy on hit*/, ModdedPlayer.Stats.energyOnHit.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", string.Format(Translations.MainMenu_Guide_39/*og:Energy on hit from items and perks: {0}*/, ModdedPlayer.Stats.energyOnHit.GetAmount()));//tr
				Stat(Translations.MainMenu_Guide_41/*og:Health per second*/, ModdedPlayer.Stats.healthRecoveryPerSecond.GetAmount() * (ModdedPlayer.Stats.healthPerSecRate.GetAmount()) * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", string.Format(Translations.MainMenu_Guide_41/*og:Health per second: {0}\nStamina regen bonus: {1}\nAll Recovery Amplification: {2}*/, ModdedPlayer.Stats.healthRecoveryPerSecond.GetAmount(), ModdedPlayer.Stats.healthPerSecRate.GetFormattedAmount(), ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));//tr
				Stat(Translations.MainMenu_Guide_43/*og:Health on hit*/, ModdedPlayer.Stats.healthOnHit.GetAmount() * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", string.Format(Translations.MainMenu_Guide_43/*og:Health on hit: {0}\nHealth regen bonus: {1}\nAll Healing Amplification: {2}*/, ModdedPlayer.Stats.healthOnHit.GetAmount(), ModdedPlayer.Stats.allRecoveryMult.GetFormattedAmount(), ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));//tr

				Space(60);
				Header(Translations.MainMenu_Guide_44/*og:Attack*/);//tr
				Space(10);
				Stat(Translations.MainMenu_Guide_45/*og:All damage*/, ModdedPlayer.Stats.allDamage.GetFormattedAmount());//tr
				Stat(Translations.MainMenu_Guide_46/*og:Critical hit damage*/, ModdedPlayer.Stats.critDamage.GetFormattedAmount());//tr
				Stat(Translations.MainMenu_Guide_47/*og:Critical hit chance*/, ModdedPlayer.Stats.critChance.GetFormattedAmount());//tr
				Stat(Translations.MainMenu_Guide_49/*og:Attack speed*/, ModdedPlayer.Stats.attackSpeed.GetFormattedAmount(), Translations.MainMenu_Guide_49/*og:Increases the speed of player actions - weapon swinging, reloading guns and drawing bows*/);//tr
				Stat(Translations.MainMenu_Guide_51/*og:Fire damage*/, ModdedPlayer.Stats.fireDamage.GetFormattedAmount(), Translations.MainMenu_Guide_51/*og:Increases fire damage*/);//tr
				Stat(Translations.MainMenu_Guide_53/*og:Bleed chance*/, ModdedPlayer.Stats.chanceToBleed.GetFormattedAmount(), Translations.MainMenu_Guide_53/*og:Bleeding enemies take 5% of damage dealt per second for 10 seconds*/);//tr
				Stat(Translations.MainMenu_Guide_55/*og:Weaken chance*/, ModdedPlayer.Stats.chanceToWeaken.GetFormattedAmount(), Translations.MainMenu_Guide_55/*og:Weakened enemies take 20% increased damage from all players.*/);//tr
				Stat(Translations.MainMenu_Guide_57/*og:Slow chance*/, ModdedPlayer.Stats.chanceToSlow.GetFormattedAmount(), Translations.MainMenu_Guide_57/*og:Slowed enemies move and attack 50% slower*/);//tr

				Space(20);
				Image(89, 70);
				Header(Translations.MainMenu_Guide_58/*og:Melee*/);//tr
				Space(10);

				Stat(Translations.MainMenu_Guide_60/*og:Melee damage*/, ModdedPlayer.Stats.MeleeDamageMult.ToString("P"), string.Format(Translations.MainMenu_Guide_60/*og:Melee damage multiplier can be increased by perks, inventory items, spells, passive abilities, and attributes.\nBonus from strength: {0}%\nIncrease to melee damage: {1}\nIncrease to all damage: {2}*/, ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount() * 100, (ModdedPlayer.Stats.meleeIncreasedDmg - 1).ToString("P"), (ModdedPlayer.Stats.allDamage - 1).ToString("P")));//tr
				Stat(Translations.MainMenu_Guide_62/*og:Additional melee weapon damage*/, ModdedPlayer.Stats.meleeFlatDmg.GetFormattedAmount(), Translations.MainMenu_Guide_62/*og:Melee damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to weapon damage and multiplied by the stat above*/);//tr
				Stat(Translations.MainMenu_Guide_63/*og:Melee range*/, ModdedPlayer.Stats.weaponRange.GetFormattedAmount());//tr
				Stat(Translations.MainMenu_Guide_64/*og:Heavy attack damage*/, ModdedPlayer.Stats.heavyAttackDmg.GetFormattedAmount());//tr

				Space(20);
				Image(98, 70);
				Header(Translations.MainMenu_Guide_65/*og:Ranged*/);//tr
				Space(10);

				Stat(Translations.MainMenu_Guide_67/*og:Ranged damage*/, ModdedPlayer.Stats.RangedDamageMult.ToString("P"), string.Format(Translations.MainMenu_Guide_67/*og:Ranged damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\nBonus from agility: {0}\nIncrease to ranged damage: {1}\nFrom size matters perk: {2}\nIncrease to all damage: {3}*/, (ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount()).ToString("P"), (ModdedPlayer.Stats.rangedIncreasedDmg.GetAmount() - 1).ToString("P"), (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize.GetAmount() ? (ModdedPlayer.Stats.projectileSize.GetAmount() - 1) * 2 : 0f).ToString("P"), (ModdedPlayer.Stats.allDamage - 1).ToString("P")));//tr
				Stat(Translations.MainMenu_Guide_69/*og:Additional ranged weapon damage*/, ModdedPlayer.Stats.rangedFlatDmg.GetFormattedAmount(), Translations.MainMenu_Guide_69/*og:Ranged damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to projectile damage and multiplied by the stat above*/);//tr
				Stat(Translations.MainMenu_Guide_71/*og:Projectile speed*/, ModdedPlayer.Stats.projectileSpeed.GetFormattedAmount(), Translations.MainMenu_Guide_71/*og:Faster projectiles fly further and fall slower*/);//tr
				Stat(Translations.MainMenu_Guide_73/*og:Projectile size*/, ModdedPlayer.Stats.projectileSize.GetFormattedAmount(), Translations.MainMenu_Guide_73/*og:Bigger projectiles allow to land headshots easier. Most projectiles still can hit only 1 target.*/);//tr
				Stat(Translations.MainMenu_Guide_75/*og:Headshot damage*/, ModdedPlayer.Stats.headShotDamage.GetFormattedAmount(), Translations.MainMenu_Guide_75/*og:Damage multiplier on headshot*/);//tr
				Stat(Translations.MainMenu_Guide_77/*og:Projectile pierce chance*/, ModdedPlayer.Stats.projectilePierceChance.GetFormattedAmount(), Translations.MainMenu_Guide_77/*og:Chance for a projectile to pierce a bone of an enemy and fly right through to hit objects behind the enemy. Increasing this value beyond 100% will make your projectiles always pierce on first enemy contact, and any further hits will also have a chance to pierce.*/);//tr
				Stat(Translations.MainMenu_Guide_78/*og:No consume chance*/, ModdedPlayer.Stats.perk_projectileNoConsumeChance.GetFormattedAmount());//tr
				Stat(Translations.MainMenu_Guide_79/*og:Spear headshot chance*/, ModdedPlayer.Stats.perk_thrownSpearCritChance.GetFormattedAmount());//tr
				if (ModdedPlayer.Stats.perk_thrownSpearhellChance.GetAmount() > 0)
					Stat(Translations.MainMenu_Guide_80/*og:Double spear chance*/, ModdedPlayer.Stats.perk_thrownSpearhellChance.GetFormattedAmount());//tr
				if (ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_81/*og:Spear damage*/, ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetFormattedAmount());//tr
				Stat(Translations.MainMenu_Guide_82/*og:Bullet headshot chance*/, ModdedPlayer.Stats.perk_bulletCritChance.GetFormattedAmount());//tr
				if (ModdedPlayer.Stats.perk_bulletDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_83/*og:Bullet damage*/, ModdedPlayer.Stats.perk_bulletDamageMult.GetFormattedAmount());//tr
				if (ModdedPlayer.Stats.perk_crossbowDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_84/*og:Crossbow damage*/, ModdedPlayer.Stats.perk_crossbowDamageMult.GetFormattedAmount());//tr
				if (ModdedPlayer.Stats.perk_bowDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_85/*og:Bow damage*/, ModdedPlayer.Stats.perk_bowDamageMult.GetFormattedAmount());//tr
				if (ModdedPlayer.Stats.i_CrossfireQuiver.GetAmount())
					Stat(Translations.MainMenu_Guide_86/*og:Shooting an enemy creates magic arrows pointed at them. There is a short cooldown on this ability*/, "");//tr

				Stat(Translations.MainMenu_Guide_87/*og:Multishot Projectiles*/, (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? (4 + ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()) : ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()).ToString("N"));//tr
				Stat(Translations.MainMenu_Guide_89/*og:Multishot Cost*/, (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? 1f * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f) : 10 * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f)).ToString(), Translations.MainMenu_Guide_89/*og:Formula for multishot cost in energy is (Multishot Projectiles ^ 1.75) * 10*/);//tr

				Space(20);
				Image(110, 70);
				Header(Translations.MainMenu_Guide_90/*og:Magic*/);//tr
				Space(10);

				Stat(Translations.MainMenu_Guide_92/*og:Spell Damage*/, ModdedPlayer.Stats.TotalMagicDamageMultiplier.ToString("P"), string.Format(Translations.MainMenu_Guide_92/*og:Spell damage multiplier can be increased by perks, inventory items, spells, passive abilities, and attributes.\nBonus from intelligence: {0}\nIncrease to spell damage: {1}\nIncrease to all damage: {2}*/, (ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount()).ToString("P"), (ModdedPlayer.Stats.spellIncreasedDmg - 1).ToString("P"), (ModdedPlayer.Stats.allDamage - 1).ToString("P")));//tr
				Stat(Translations.MainMenu_Guide_94/*og:Additional Spell Damage*/, ModdedPlayer.Stats.spellFlatDmg.GetFormattedAmount(), Translations.MainMenu_Guide_94/*og:Spell damage bonus can be increased by perks and inventory items. This is added to spell damage and multiplied by the stat above. Often spells take a fraction of this stat and add it to spell's damage.*/);//tr
				Stat(Translations.MainMenu_Guide_96/*og:Spell Cost Reduction*/, (1 - ModdedPlayer.Stats.spellCost.GetAmount()).ToString("P"), Translations.MainMenu_Guide_96/*og:Reduced amount of energy needed to cast spells. Energy is the dark blue bar on the hud. Energy is drained over time and requires food or rest to regain in.*/);//tr
				Stat(Translations.MainMenu_Guide_98/*og:Spell Cost redirected to stamina*/, ModdedPlayer.Stats.SpellCostToStamina.ToString("P"), Translations.MainMenu_Guide_98/*og:Stamina is the resource which quickly regenerates*/);//tr
				Stat(Translations.MainMenu_Guide_99/*og:Cooldown Reduction*/, (1 - ModdedPlayer.Stats.cooldown.GetAmount()).ToString("P"));//tr

				Space(20);
				GUI.color = Color.red;
				Image(96, 70);
				GUI.color = Color.white;
				Header(Translations.MainMenu_Guide_100/*og:Armor reduction*/);//tr
				Space(10);
				Stat(Translations.MainMenu_Guide_58/*og:Melee*/, ModdedPlayer.Stats.meleeArmorPiercing.GetAmount() + "", string.Format(Translations.MainMenu_Guide_102/*og:Total melee armor reduction: {0}*/, ModdedPlayer.Stats.TotalMeleeArmorPiercing.ToString()));//tr
				Stat(Translations.MainMenu_Guide_65/*og:Ranged*/, ModdedPlayer.Stats.rangedArmorPiercing.GetAmount() + "", string.Format(Translations.MainMenu_Guide_103/*og:Total ranged armor reduction: {0}*/, ModdedPlayer.Stats.TotalRangedArmorPiercing.ToString()));//tr
				Stat(Translations.MainMenu_Guide_104/*og:Thorns*/, ModdedPlayer.Stats.thornsArmorPiercing.GetAmount() + "", string.Format(Translations.MainMenu_Guide_104/*og:Total thorns armor reduction: {0}*/, ModdedPlayer.Stats.TotalThornsArmorPiercing.ToString()));//tr
				Stat(Translations.MainMenu_Guide_106/*og:Any source*/, ModdedPlayer.Stats.allArmorPiercing.GetAmount() + "", Translations.MainMenu_Guide_106/*og:Decreases armor of enemies hit by either of the sources*/);//tr
			}
			else if (guidePage == a++)
			{
				Header(Translations.MainMenu_Guide_107/*og:Survivor stats*/);//tr
				Space(10);

				Stat(Translations.MainMenu_Guide_109/*og:Movement Speed*/, ModdedPlayer.Stats.movementSpeed.GetAmount().ToString(), string.Format(Translations.MainMenu_Guide_109/*og:Multiplier of base movement speed. Base walking speed is equal to {0} feet per second, with bonuses it's {1} feet/second*/, FPCharacterMod.basewalkSpeed, FPCharacterMod.basewalkSpeed * ModdedPlayer.Stats.movementSpeed.GetAmount()));//tr
				Stat(Translations.MainMenu_Guide_111/*og:Jump Power*/, ModdedPlayer.Stats.jumpPower.GetAmount().ToString(), Translations.MainMenu_Guide_111/*og:Multiplier of base jump power. Increases height of your jumps*/);//tr
				Stat(Translations.MainMenu_Guide_113/*og:Hunger Rate*/, (1 / ModdedPlayer.Stats.perk_hungerRate).ToString("P"), Translations.MainMenu_Guide_113/*og:How much slower is the rate of consuming food compared to normal.*/);//tr
				Stat(Translations.MainMenu_Guide_115/*og:Thirst Rate*/, (1 / ModdedPlayer.Stats.perk_thirstRate).ToString("P"), Translations.MainMenu_Guide_115/*og:How much slower is the rate of consuming water compared to normal.*/);//tr
				Stat(Translations.MainMenu_Guide_117/*og:Experience Rate*/, ModdedPlayer.Stats.expGain.GetFormattedAmount(), Translations.MainMenu_Guide_117/*og:Multiplier of any experience gained*/);//tr
				Stat(Translations.MainMenu_Guide_120/*og:Massacre Duration*/, ModdedPlayer.Stats.maxMassacreTime.GetAmount() + Translations.MainMenu_Guide_120/*og: s*/, Translations.MainMenu_Guide_118/*og:How long massacres can last*/);//tr
				Stat(Translations.MainMenu_Guide_122/*og:Massacre Time On Kill*/, ModdedPlayer.Stats.timeBonusPerKill.GetAmount() + Translations.MainMenu_Guide_120/*og: s*/, Translations.MainMenu_Guide_121/*og:Amount of time that is added to massacre for every kill*/);//tr
				if (ModdedPlayer.Stats.perk_turboRaftOwners.GetAmount() > 0)
					Stat(Translations.MainMenu_Guide_124/*og:Turbo Raft Speed*/, ModdedPlayer.Stats.perk_RaftSpeedMultipier.GetFormattedAmount(), Translations.MainMenu_Guide_124/*og:Speed multiplier of rafts. Other player's items and perks also affect this value*/);//tr
				Stat(Translations.MainMenu_Guide_126/*og:Magic Find*/, ModdedPlayer.Stats.magicFind.Value.ToString("P"), Translations.MainMenu_Guide_126/*og:Affects rarity of items looted from monsters, as well as the chance to get items from non-elite enemies. Increases globally, and this value is affected by every player. */);//tr
				foreach (var mfStat in ModdedPlayer.Stats.magicFind.OtherPlayerValues)
				{
					Stat(mfStat.Key + Translations.MainMenu_Guide_128/*og:'s Magic Find*/, mfStat.Value.ToString("P"), Translations.MainMenu_Guide_128/*og:Magic of another player. Other players' magic find contribute to the quality of everyone's loot*/);//tr
				}

				Space(40);
				Image(90, 70);
				Header(Translations.MainMenu_Guide_129/*og:Inventory Stats*/);//tr
				Space(10);
				foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> pair in ModdedPlayer.instance.ExtraCarryingCapactity)
				{
					string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Value.ID, (pair.Value.Amount > 1), false);
					Stat(item_name, "+" + pair.Value.Amount, string.Format(Translations.MainMenu_Guide_130/*og:Amount of additional items of type '{0}' you are allowed to carry.*/, item_name));//tr
				}
				Space(10);
				if (ModdedPlayer.instance.GeneratedResources.Count > 0)
					Header("Generated resources");
				foreach (var pair in ModdedPlayer.instance.GeneratedResources)
				{
					string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Key, (pair.Value > 1), false);
					Stat(item_name, pair.ToString(), string.Format(Translations.MainMenu_Guide_131/*og:Amount of additional items of type'{0}' you generate daily.*/, item_name));//tr
				}
			}
			else if (guidePage == a++)
			{
				if (BookPositionY < Screen.height && BookPositionY > -140 * screenScale)
				{
					Rect labelRect = new Rect(GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease), 85 * screenScale);
					if (GUI.Button(labelRect, Translations.MainMenu_Guide_132/*og:Recalculate Stats*/, new GUIStyle(GUI.skin.button)//tr
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
			guidePage = Mathf.Clamp(pageNumber, 0, 3);
			BookScrollAmount = 0;
			BookScrollAmountGoal = 0;
		}
	}
}