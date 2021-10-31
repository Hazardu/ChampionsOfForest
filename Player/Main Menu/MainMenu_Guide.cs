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
			GUI.Label(new Rect(0, 0, 300, 100), Translations.MainMenu_Guide_1/*og:Page: */ + guidePage);
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

				Header(Translations.MainMenu_Guide_2/*og:Basic Information*/);
				MarkBookmark(Translations.MainMenu_Guide_3/*og:Home*/);
				Label(Translations.MainMenu_Guide_4/*og:\tExperience*/);
				Stat(Translations.MainMenu_Guide_5/*og:Current level*/, ModdedPlayer.instance.level.ToString());
				Stat(Translations.MainMenu_Guide_6/*og:Current experience*/, ModdedPlayer.instance.ExpCurrent.ToString());
				Stat(Translations.MainMenu_Guide_7/*og:Experience goal*/, ModdedPlayer.instance.ExpGoal.ToString(), Translations.MainMenu_Guide_8/*og:Next level: */ + (ModdedPlayer.instance.level + 1) + Translations.MainMenu_Guide_9/*og: you will need to get this amount of experience:\t */ + ModdedPlayer.instance.GetGoalExp(ModdedPlayer.instance.level + 1));
				Stat(Translations.MainMenu_Guide_10/*og:Progress amount: */, (((float)ModdedPlayer.instance.ExpCurrent / ModdedPlayer.instance.ExpGoal).ToString("P")));
				Label(Translations.MainMenu_Guide_11/*og:\tLevel is the estimation of my power. I must become stronger to survive.*/ +
					Translations.MainMenu_Guide_12/*og:\nHigher level allow me to equip better equipment. */ +
					Translations.MainMenu_Guide_13/*og:\nLeveling up gives me the ability to develop usefull abilities. (Currently you have */ + ModdedPlayer.instance.MutationPoints + Translations.MainMenu_Guide_14/*og: mutation points), which you can spend on unlocking spells or perks. */);
				Space(50);
				Label(Translations.MainMenu_Guide_15/*og:\nSources of experience*/ +
					Translations.MainMenu_Guide_16/*og:\n-Mutants - Enemies give the most experience, it's possible to chain kills to get more exp, and the reward is exp reward is based on bounty.*/ +
					Translations.MainMenu_Guide_17/*og:\n-Animals - Experience gained does not increase with difficulty. Good way of gaining experience. Rarer animals like crocodiles and racoons give more experience compared to common like rabbits and lizards*/ +
					Translations.MainMenu_Guide_18/*og:\n-Tall bushes - Give minimum amount of experience.*/ +
					Translations.MainMenu_Guide_19/*og:\n-Trees - A bit more than bushes.*/ +
					Translations.MainMenu_Guide_20/*og:\n-Effigies - It's possible to gain experience and low rarity items by breaking effigies scattered across the map.*/ +
					Translations.MainMenu_Guide_21/*og:\n-Rare consumable - Gives a large amount of experience, it's rarity is orange\n*/);

				Space(100);

				Header(Translations.MainMenu_Guide_22/*og:Information - Items*/);
				Label(Translations.MainMenu_Guide_23/*og:\tEquipement can be obtained by killing enemies and breaking effigies. Normal enemies can drop a few items on death, if the odds are in your favor. The chance to get any items from a normal enemy is */ + (0.1f* ModdedPlayer.Stats.magicFind.Value* ModSettings.DropChanceMultiplier).ToString("P") + Translations.MainMenu_Guide_24/*og:. The amount of items obtained from normal enemies increases with more players in a lobby.\n*/ +
				 Translations.MainMenu_Guide_25/*og:Elite enemies always drop items in large amounts.\n*/ +
				 Translations.MainMenu_Guide_26/*og:\tItems can be equipped by dragging and dropping them onto a right equipment slot or shift+left click. The item will grant it's stats only if you meets item's level requirement. The best tier of items is only obtainable on high difficulties.*/);
				Label(Translations.MainMenu_Guide_27/*og:By unlocking a perk in the survival category, it's possible to change the stats on your existing items, and reforge unused items into something useful. Reforged item will have the same level as item put into the main crafting slot.*/);
				Label(Translations.MainMenu_Guide_28/*og:In the inventory, you can compare an item with your equipped item by holding down left shift.*/);
				Label(Translations.MainMenu_Guide_29/*og:By holding down left shift and clicking on an item, it will be equipped. This server the same purpose as dragging and dropping an item.*/);
				Label(Translations.MainMenu_Guide_30/*og:By holding down left control and clicking on an item, be used as an ingredient for crafting.*/);
				Label(Translations.MainMenu_Guide_31/*og:By pressing left alt you toggle a window to show total amount of a stat when you hover over it.*/);
				
				Space(100);
				Header(Translations.MainMenu_Guide_32/*og:Information - Statistics*/);
				Label(Translations.MainMenu_Guide_33/*og:\tAttributes*/);
				Label(Translations.MainMenu_Guide_34/*og:Strength - This stat increases melee damage and thorns. It multiplies with melee damage increase.*/);
				Label(Translations.MainMenu_Guide_35/*og:Agility - This stat increases ranged damage and maximum energy.*/);
				Label(Translations.MainMenu_Guide_36/*og:Intelligence - This stat increases magic damage and energy regeneration rate.*/);
				Label(Translations.MainMenu_Guide_37/*og:Vitality - This stat increases maximum health*/);

				Space(100);

				Header(Translations.MainMenu_Guide_38/*og:Information - Mutations and Abilities*/);
				Label(Translations.MainMenu_Guide_39/*og:\tUpon leveling up, the player will receive a upgrade point. Then it's up to the player to use it to unlock a mutation, that will serve as a permanent perk, or to spend two upgrade points to unlock a ability.\n*/ +
					Translations.MainMenu_Guide_40/*og:Abilities are in majority of the cases more powerful than perks, as they cost more and the number of active abilities is limited to 6.\n*/ +
					Translations.MainMenu_Guide_41/*og:Some perks can be bought multiple times for increased effects.\n*/ +
					"\n" +
					Translations.MainMenu_Guide_42/*og:Refunding - it is possible to refund all points, to do so, heart of purity needs to be consumed. This item is of yellow rarity, and thus unobtainable on easy and veteran difficulties. Heart of purity will be granted to a player upon reaching level 15, 30 and 40*/ +
					Translations.MainMenu_Guide_43/*og:More points - to gain a point without leveling, a rare item of green rarity needs to be consumed. It permanently adds a upgrade point, and it persists even after refunding. Items to grant additional upgrade points are granted to player to upon reaching level 50, 65 and 75. To obtain them in different means, you need to play on challenge difficulties or higher*/);
				Space(100);

				Header(Translations.MainMenu_Guide_44/*og:Information - Enemies*/);
				Label(Translations.MainMenu_Guide_45/*og:\tEnemies in the forest have adapted to your skill. As they level with you, they become faster and stronger. But speed and strength alone shouldn't be your main concern. There are a lot more dangerous beings out there.*/ +
					Translations.MainMenu_Guide_46/*og:\n\n*/ +
					Translations.MainMenu_Guide_47/*og:Common enemies changed slightly. Their health increases with level.\n*/ +
					Translations.MainMenu_Guide_48/*og:A new statistic to enemies is 'Armor'. This property reduces damage taken by the enemies from physical attacks, and partly reduces damage from magical attacks. Armor can be reduced in a number of ways.\n*/ +
					Translations.MainMenu_Guide_49/*og:The easiest way to reduce armor is to use fire. Fire works as a way to crowd control enemies, it renders a few enemies unable to run and attack as they shake off the flames.\n*/ +
					Translations.MainMenu_Guide_50/*og:Other way to reduce armor is to equip items, which reduce armor on hit.\n*/ +
					Translations.MainMenu_Guide_51/*og:If you dont have any way to reduce enemy's armor, damaging them with spells would decrease the reduction from armor by 2/3, allowing you to deal some damage.*/);
				Space(30);
				Label(Translations.MainMenu_Guide_52/*og:Elite enemies\n*/ +
					Translations.MainMenu_Guide_53/*og:An elite is a uncommon type of a mutant with increased stats and access to special abilities, that make encounters with them challenging.*/ +
					Translations.MainMenu_Guide_54/*og:\nEnemy abilities:*/);
				Label(Translations.MainMenu_Guide_55/*og:- Steadfast - This defensive ability causes enemy to reduce all damage exceeding a percent of their maximum health. To deal with this kind of ability, damage over time and fast attacks are recommended. This ability counters nuke instances of damage.*/);
				Label(Translations.MainMenu_Guide_56/*og:- Blizzard - A temporary aura around an enemy, that slows anyone in it's area of effect. Affects movement speed and attack speed. Best way to deal with this is to avoid getting within it's range. Crowd controll from ranged attacks and running seems like the best option.*/);
				Label(Translations.MainMenu_Guide_57/*og:- Radiance - A permanent aura around an enemy. It deals damage anyone around. The only way of dealing with this is to never get close to the enemy.*/);
				Label(Translations.MainMenu_Guide_58/*og:- Chains - Roots anyone in a big radius around the elite. The duration this root increases with difficulty. Several abilities that provide resistance to crowd control clear the effects of this ability.*/);
				Label(Translations.MainMenu_Guide_59/*og:- Black hole - A very strong ability. The spell has a fixed cooldown, and the enemy will attempt to cast it as soon as a player gets within his range effective.*/);
				Label(Translations.MainMenu_Guide_60/*og:- Trap sphere - Long lasting sphere that forces you to stay inside it until it's effects wears off*/);
				Label(Translations.MainMenu_Guide_61/*og:- Juggernaut - The enemy is completely immune to crowd control and bleeding.\n*/);
				Label(Translations.MainMenu_Guide_62/*og:- Gargantuan - Describes an enemy that is bigger, faster, stronger and has more health.*/);
				Label(Translations.MainMenu_Guide_63/*og:- Tiny - An enemy has decreased size. It's harder to hit it with ranged attacks and most of the melee weapons can only attack the enemy with slow smashes.*/);
				Label(Translations.MainMenu_Guide_64/*og:- Extra tough - enemy has a lot more health*/);
				Label(Translations.MainMenu_Guide_65/*og:- Extra deadly - enemy has a lot more damage*/);
				Label(Translations.MainMenu_Guide_66/*og:- Basher - the enemy stuns on hit. Best way to fight it is to not get hit or parry it's attacks.*/);
				Label(Translations.MainMenu_Guide_67/*og:- Warping - An ability allowing to teleport. Strong against glass cannon builds, running away and ranged attacks. Weak against melee strikes and a lot of durability.*/);
				Label(Translations.MainMenu_Guide_68/*og:- Rain Empowerment - If it rains, the enemy gains in strength, speed, armor and size.*/);
				Label(Translations.MainMenu_Guide_69/*og:- Meteors - Periodically spawns a rain of powerful meteors. They are rather easy to spot and they move at a slow medium speed.*/);
				Label(Translations.MainMenu_Guide_70/*og:- Flare - Slows and damages me if you stand inside. Heals and makes enemies faster.*/);
				Label(Translations.MainMenu_Guide_71/*og:- Undead - An enemy upon dieing restores portion of it's health, gets stronger and bigger.*/);
				Label(Translations.MainMenu_Guide_72/*og:- Plasma cannon - Creates a turret that fires a laser beam that damages players and buildings.*/);
				Label(Translations.MainMenu_Guide_73/*og:- Poisonous - Enemies gain a attack modifier, that applies a stacking debuff, which deals damage over time. Once hit, it is advised to retreat and wait for the poison stop damaging you.*/);
				Label(Translations.MainMenu_Guide_74/*og:- Cataclysm - Enemy uses the cataclysm spell to slow you down and damage you.*/);

				Header(Translations.MainMenu_Guide_75/*og:Changes*/);
				Label(Translations.MainMenu_Guide_76/*og:Champions of The Forest provides variety of changes to in-game mechanics.*/ +
					Translations.MainMenu_Guide_77/*og:\nArmor no longer absorbs all damage. Instead it reduces the damage by 70%.*/ +
					Translations.MainMenu_Guide_78/*og:\nPlayer is slowed down if out of stamina (the inner blue bar)*/ +
					Translations.MainMenu_Guide_79/*og:\nTraps no longer instantly kill cannibals. Instead they deal damage.*/ +
					Translations.MainMenu_Guide_80/*og:\nDynamite no longer instantly kills enemies. Instead it deals up to 700 damage*/ +
					Translations.MainMenu_Guide_81/*og:\nEnemies have armor and increased health.*/ +
					Translations.MainMenu_Guide_82/*og:\nPlayers now take increased damage from fire, frost, drowning, falling, food poisoning and polluted water based on their maximum health*/ +
					Translations.MainMenu_Guide_83/*og:\nPlayers take increased damage from explosives. This affects how much damage the worm does*/ +
					Translations.MainMenu_Guide_84/*og:\nPlayer deal increased damage to other players if friendly fire is enabled.*/);
			}
			else if (guidePage == a++)
			{


				Header(Translations.MainMenu_Guide_85/*og:Statistics*/);
				Stat(Translations.MainMenu_Guide_86/*og:Strength*/, ModdedPlayer.Stats.strength.GetFormattedAmount() + Translations.MainMenu_Guide_87/*og: str*/, Translations.MainMenu_Guide_88/*og:Increases melee damage by */ + ModdedPlayer.Stats.meleeDmgFromStr.GetFormattedAmount() + Translations.MainMenu_Guide_89/*og: for every 1 point of strength. Current bonus melee damage from strength [*/ + ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount() *100+ Translations.MainMenu_Guide_90/*og:%]*/);
				Stat(Translations.MainMenu_Guide_91/*og:Agility*/, ModdedPlayer.Stats.agility.GetFormattedAmount() + Translations.MainMenu_Guide_92/*og: agi*/, Translations.MainMenu_Guide_93/*og:Increases ranged damage by */ + ModdedPlayer.Stats.rangedDmgFromAgi.GetFormattedAmount() + Translations.MainMenu_Guide_94/*og: for every 1 point of agility. Current bonus ranged damage from agility [*/ + ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount() * 100 + Translations.MainMenu_Guide_95/*og:%]\n*/ +
					Translations.MainMenu_Guide_96/*og:Increases maximum energy by */ + ModdedPlayer.Stats.maxEnergyFromAgi.GetFormattedAmount() + Translations.MainMenu_Guide_94/*og: for every 1 point of agility. Current bonus ranged damage from agility [*/ + ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount()*100 + Translations.MainMenu_Guide_90/*og:%]*/);
				Stat(Translations.MainMenu_Guide_97/*og:Vitality*/, ModdedPlayer.Stats.vitality.GetFormattedAmount() + Translations.MainMenu_Guide_98/*og: vit*/, Translations.MainMenu_Guide_99/*og:Increases health by */ + ModdedPlayer.Stats.maxHealthFromVit.GetFormattedAmount() + Translations.MainMenu_Guide_100/*og: for every 1 point of vitality. Current bonus health from vitality [*/ + ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.maxHealthFromVit.GetAmount() + "]");
				Stat(Translations.MainMenu_Guide_101/*og:Intelligence*/, ModdedPlayer.Stats.intelligence.GetFormattedAmount() + Translations.MainMenu_Guide_102/*og: int*/, Translations.MainMenu_Guide_103/*og:Increases spell damage by */ + ModdedPlayer.Stats.spellDmgFromInt.GetFormattedAmount() + Translations.MainMenu_Guide_104/*og: for every 1 point of intelligence. Current bonus spell damage from intelligence [*/ + ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount() + Translations.MainMenu_Guide_105/*og:]\n*/ +
					Translations.MainMenu_Guide_106/*og:Increases stamina regen by */ + ModdedPlayer.Stats.energyRecoveryFromInt.GetFormattedAmount() + Translations.MainMenu_Guide_107/*og: for every 1 point of intelligence. Current bonus stamina regen from intelligence [*/ + ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.energyRecoveryFromInt.GetAmount()*100 + Translations.MainMenu_Guide_90/*og:%]*/);
			

				Space(60);
				Image(99, 70);
				Header(Translations.MainMenu_Guide_108/*og:Defense*/);
				Space(10);
				Stat(Translations.MainMenu_Guide_109/*og:Max health*/, ModdedPlayer.Stats.TotalMaxHealth.ToString(), Translations.MainMenu_Guide_110/*og:Total health pool.\n*/ +
					Translations.MainMenu_Guide_111/*og:Base health: */ + ModdedPlayer.ModdedPlayerStats.baseHealth +
					Translations.MainMenu_Guide_112/*og:\nBonus health: */ + ModdedPlayer.Stats.maxHealth.GetFormattedAmount() +
					Translations.MainMenu_Guide_113/*og:\nHealth from vitality: */ + ModdedPlayer.Stats.maxHealthFromVit.GetAmount() * ModdedPlayer.Stats.vitality.GetAmount() +
					Translations.MainMenu_Guide_114/*og:\nHealth multiplier: */ + ModdedPlayer.Stats.maxHealthMult.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_115/*og:Max energy*/, ModdedPlayer.Stats.TotalMaxEnergy.ToString(), Translations.MainMenu_Guide_116/*og:Total energy pool.\n*/ +
					Translations.MainMenu_Guide_117/*og:Base energy: */ + ModdedPlayer.ModdedPlayerStats.baseEnergy +
					Translations.MainMenu_Guide_118/*og:\nBonus energy: */ + ModdedPlayer.Stats.maxEnergy.GetFormattedAmount() +
					Translations.MainMenu_Guide_119/*og:\nEnergy from agility: */ + ModdedPlayer.Stats.maxEnergyFromAgi.GetAmount() * ModdedPlayer.Stats.agility.GetAmount() +
					Translations.MainMenu_Guide_120/*og:\nEnergy multiplier: */ + ModdedPlayer.Stats.maxEnergyMult.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_121/*og:Armor*/, ModdedPlayer.Stats.armor.GetFormattedAmount(), Translations.MainMenu_Guide_122/*og:Armor provides physical damage reduction.\nPhysical damage reduction from */ + (ModdedPlayer.Stats.armor.GetFormattedAmount()) + Translations.MainMenu_Guide_123/*og: armor is equal to */ + (ModReferences.DamageReduction(ModdedPlayer.Stats.armor.Value)).ToString("P") + "");
				Stat(Translations.MainMenu_Guide_124/*og:Magic resistance*/, (1 - ModdedPlayer.Stats.magicDamageTaken.GetAmount()).ToString("P"), Translations.MainMenu_Guide_125/*og:Magic damage reduction. Decreases damage from enemy abilities.*/);
				Stat(Translations.MainMenu_Guide_126/*og:Dodge Chance*/, (1 - ModdedPlayer.Stats.getHitChance.GetAmount()).ToString("P"), Translations.MainMenu_Guide_127/*og:A chance to avoid entire instance of damage. Works only for physical damage sources. This means dodge is ineffective against fire, poison, cold, various spells. Meteor rain ability deals physical damage and can be dodged*/);
				Stat(Translations.MainMenu_Guide_128/*og:Damage taken reduction*/, (1f - ModdedPlayer.Stats.allDamageTaken.GetAmount()).ToString("P"));
				Stat(Translations.MainMenu_Guide_129/*og:Block*/, ModdedPlayer.Stats.block.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_130/*og:Temporary health*/, ModdedPlayer.instance.DamageAbsorbAmount.ToString(),Translations.MainMenu_Guide_131/*og:One way to obtain temporary health is to use sustain shield ability*/);
				Stat(Translations.MainMenu_Guide_132/*og:Fire resistance*/, (1 - ModdedPlayer.Stats.fireDamageTaken.GetAmount()).ToString("P"));
				Stat(Translations.MainMenu_Guide_133/*og:Thorns*/, ModdedPlayer.Stats.TotalThornsDamage.ToString(), Translations.MainMenu_Guide_134/*og:Thorns inflict damage to attacking enemies. Thorns from gear and mutations */ + (ModdedPlayer.Stats.thorns.GetFormattedAmount()) + Translations.MainMenu_Guide_135/*og:. Thorns from attributes */ + ((ModdedPlayer.Stats.thornsPerStrenght.GetAmount() * ModdedPlayer.Stats.strength.GetAmount() + ModdedPlayer.Stats.vitality.GetAmount() * ModdedPlayer.Stats.thornsPerVit.GetAmount())) + Translations.MainMenu_Guide_136/*og:.\nThorns damage is applied to attackers even when you are blocking*/);
		
				Space(60);
				Header(Translations.MainMenu_Guide_137/*og:Recovery*/);
				Space(10);


				Stat(Translations.MainMenu_Guide_138/*og:Total Stamina recovery per second*/, ModdedPlayer.Stats.TotalStaminaRecoveryAmount.ToString() + "", Translations.MainMenu_Guide_139/*og:Stamina regeneration is temporarily paused after sprinting*/);
				Stat(Translations.MainMenu_Guide_140/*og:Stamina per second*/, ModdedPlayer.Stats.staminaRecoveryperSecond.GetAmount() *  ModdedPlayer.Stats.staminaPerSecRate.GetAmount() + "", Translations.MainMenu_Guide_141/*og:Stamina per second: */ + ModdedPlayer.Stats.staminaRecoveryperSecond.GetAmount() + Translations.MainMenu_Guide_142/*og:\nStamina regen bonus: */ + ModdedPlayer.Stats.staminaPerSecRate.GetFormattedAmount());

				Stat(Translations.MainMenu_Guide_143/*og:Energy per second*/, ModdedPlayer.Stats.energyRecoveryperSecond.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", Translations.MainMenu_Guide_144/*og:Energy per second: */ + ModdedPlayer.Stats.energyRecoveryperSecond.GetAmount() + Translations.MainMenu_Guide_145/*og:\nStamina and energy regen multipier: */ + ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier);
				Stat(Translations.MainMenu_Guide_146/*og:Energy on hit*/, ModdedPlayer.Stats.energyOnHit.GetAmount() * ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier + "", Translations.MainMenu_Guide_147/*og:Energy on hit from items and perks: */ + ModdedPlayer.Stats.energyOnHit.GetAmount());
				Stat(Translations.MainMenu_Guide_148/*og:Health per second*/, ModdedPlayer.Stats.healthRecoveryPerSecond.GetAmount() * (ModdedPlayer.Stats.healthPerSecRate.GetAmount()) * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", Translations.MainMenu_Guide_149/*og:Health per second: */ + ModdedPlayer.Stats.healthRecoveryPerSecond.GetAmount() + Translations.MainMenu_Guide_142/*og:\nStamina regen bonus: */ + ModdedPlayer.Stats.healthPerSecRate.GetFormattedAmount() + Translations.MainMenu_Guide_150/*og:\nAll Recovery Amplification: */ + (ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));
				Stat(Translations.MainMenu_Guide_151/*og:Health on hit*/, ModdedPlayer.Stats.healthOnHit.GetAmount() * ModdedPlayer.Stats.allRecoveryMult.GetAmount() + "", Translations.MainMenu_Guide_152/*og:Health on hit: */ + ModdedPlayer.Stats.healthOnHit.GetAmount() + Translations.MainMenu_Guide_153/*og:\nHealth regen bonus: */ + ModdedPlayer.Stats.allRecoveryMult.GetFormattedAmount() + Translations.MainMenu_Guide_154/*og:\nAll Healing Amplification: */ + (ModdedPlayer.Stats.allRecoveryMult.GetAmount() - 1));
		
				Space(60);
				Header(Translations.MainMenu_Guide_155/*og:Attack*/);
				Space(10);
				Stat(Translations.MainMenu_Guide_156/*og:All damage*/, ModdedPlayer.Stats.allDamage.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_157/*og:Critical hit damage*/, ModdedPlayer.Stats.critDamage.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_158/*og:Critical hit chance*/, ModdedPlayer.Stats.critChance.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_159/*og:Attack speed*/, ModdedPlayer.Stats.attackSpeed.GetFormattedAmount(), Translations.MainMenu_Guide_160/*og:Increases the speed of player actions - weapon swinging, reloading guns and drawing bows*/);
				Stat(Translations.MainMenu_Guide_161/*og:Fire damage*/, ModdedPlayer.Stats.fireDamage.GetFormattedAmount(), Translations.MainMenu_Guide_162/*og:Increases fire damage*/);
				Stat(Translations.MainMenu_Guide_163/*og:Bleed chance*/, ModdedPlayer.Stats.chanceToBleed.GetFormattedAmount(), Translations.MainMenu_Guide_164/*og:Bleeding enemies take 5% of damage dealt per second for 10 seconds*/);
				Stat(Translations.MainMenu_Guide_165/*og:Weaken chance*/, ModdedPlayer.Stats.chanceToWeaken.GetFormattedAmount(), Translations.MainMenu_Guide_166/*og:Weakened enemies take 20% increased damage from all players.*/);
				Stat(Translations.MainMenu_Guide_167/*og:Slow chance*/, ModdedPlayer.Stats.chanceToSlow.GetFormattedAmount(), Translations.MainMenu_Guide_168/*og:Slowed enemies move and attack 50% slower*/);
		
				Space(20);
				Image(89, 70);
				Header(Translations.MainMenu_Guide_169/*og:Melee*/);
				Space(10);

				Stat(Translations.MainMenu_Guide_170/*og:Melee damage*/, ModdedPlayer.Stats.MeleeDamageMult.ToString("P"), Translations.MainMenu_Guide_171/*og:Melee damage multiplier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n*/ +
				   Translations.MainMenu_Guide_172/*og:Bonus from strength: */ + ModdedPlayer.Stats.strength.GetAmount() * ModdedPlayer.Stats.meleeDmgFromStr.GetAmount()*100 + Translations.MainMenu_Guide_173/*og:%\n*/ +
				   Translations.MainMenu_Guide_174/*og:Increase to melee damage: */ + (ModdedPlayer.Stats.meleeIncreasedDmg-1).ToString("P") + "\n" +
				   Translations.MainMenu_Guide_175/*og:Increase to all damage: */ + (ModdedPlayer.Stats.allDamage- 1).ToString("P"));
				Stat(Translations.MainMenu_Guide_176/*og:Additional melee weapon damage*/, ModdedPlayer.Stats.meleeFlatDmg.GetFormattedAmount(), Translations.MainMenu_Guide_177/*og:Melee damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to weapon damage and multiplied by the stat above*/);
				Stat(Translations.MainMenu_Guide_178/*og:Melee range*/, ModdedPlayer.Stats.weaponRange.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_179/*og:Heavy attack damage*/, ModdedPlayer.Stats.heavyAttackDmg.GetFormattedAmount());
		
				Space(20);
				Image(98, 70);
				Header(Translations.MainMenu_Guide_180/*og:Ranged*/);
				Space(10);

				Stat(Translations.MainMenu_Guide_181/*og:Ranged damage*/, ModdedPlayer.Stats.RangedDamageMult.ToString("P"), Translations.MainMenu_Guide_182/*og:Ranged damage multipier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n*/ +
				Translations.MainMenu_Guide_183/*og:Bonus from agility: */ + (ModdedPlayer.Stats.agility.GetAmount() * ModdedPlayer.Stats.rangedDmgFromAgi.GetAmount()).ToString("P") + "\n" +
				Translations.MainMenu_Guide_184/*og:Increase to ranged damage: */ + (ModdedPlayer.Stats.rangedIncreasedDmg.GetAmount()-1).ToString("P") + "\n" +
				Translations.MainMenu_Guide_185/*og:From size matters perk: */ + (ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize.GetAmount() ? (ModdedPlayer.Stats.projectileSize.GetAmount()-1)*2 : 0f).ToString("P") +
				Translations.MainMenu_Guide_186/*og:\nIncrease to all damage: */ + (ModdedPlayer.Stats.allDamage - 1).ToString("P"));
				Stat(Translations.MainMenu_Guide_187/*og:Additional ranged weapon damage*/, ModdedPlayer.Stats.rangedFlatDmg.GetFormattedAmount(), Translations.MainMenu_Guide_188/*og:Ranged damage bonus can be increased by perks and inventory items (mainly this stat occurs on weapons). This is added to projectile damage and multiplied by the stat above*/);
				Stat(Translations.MainMenu_Guide_189/*og:Projectile speed*/, ModdedPlayer.Stats.projectileSpeed.GetFormattedAmount(), Translations.MainMenu_Guide_190/*og:Faster projectiles fly further and fall slower*/);
				Stat(Translations.MainMenu_Guide_191/*og:Projectile size*/, ModdedPlayer.Stats.projectileSize.GetFormattedAmount(), Translations.MainMenu_Guide_192/*og:Bigger projectiles allow to land headshots easier. Most projectiles still can hit only 1 target.*/);
				Stat(Translations.MainMenu_Guide_193/*og:Headshot damage*/, ModdedPlayer.Stats.headShotDamage.GetFormattedAmount(), Translations.MainMenu_Guide_194/*og:Damage multiplier on headshot*/);
				Stat(Translations.MainMenu_Guide_195/*og:Projectile pierce chance*/, ModdedPlayer.Stats.projectilePierceChance.GetFormattedAmount(), Translations.MainMenu_Guide_196/*og:Chance for a projectile to pierce a bone of an enemy and fly right through to hit objects behind the enemy. Increasing this value beyond 100% will make your projectiles always pierce on first enemy contact, and any further hits will also have a chance to pierce.*/);
				Stat(Translations.MainMenu_Guide_197/*og:No consume chance*/, ModdedPlayer.Stats.perk_projectileNoConsumeChance.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_198/*og:Spear headshot chance*/, ModdedPlayer.Stats.perk_thrownSpearCritChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_thrownSpearhellChance.GetAmount() > 0)
					Stat(Translations.MainMenu_Guide_199/*og:Double spear chance*/, ModdedPlayer.Stats.perk_thrownSpearhellChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_200/*og:Spear damage*/, ModdedPlayer.Stats.perk_thrownSpearDamageMult.GetFormattedAmount());
				Stat(Translations.MainMenu_Guide_201/*og:Bullet headshot chance*/, ModdedPlayer.Stats.perk_bulletCritChance.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_bulletDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_202/*og:Bullet damage*/, ModdedPlayer.Stats.perk_bulletDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_crossbowDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_203/*og:Crossbow damage*/, ModdedPlayer.Stats.perk_crossbowDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.perk_bowDamageMult.GetAmount() != 1)
					Stat(Translations.MainMenu_Guide_204/*og:Bow damage*/, ModdedPlayer.Stats.perk_bowDamageMult.GetFormattedAmount());
				if (ModdedPlayer.Stats.i_CrossfireQuiver.GetAmount())
					Stat(Translations.MainMenu_Guide_205/*og:Shooting an enemy creates magic arrows*/, "");

				Stat(Translations.MainMenu_Guide_206/*og:Multishot Projectiles*/, (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? (4 + ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()) : ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount()).ToString("N"));
				Stat(Translations.MainMenu_Guide_207/*og:Multishot Cost*/, (ModdedPlayer.Stats.i_SoraBracers.GetAmount() ? 1f * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f) : 10 * Mathf.Pow(ModdedPlayer.Stats.perk_multishotProjectileCount.GetAmount(), 1.75f)).ToString(), Translations.MainMenu_Guide_208/*og:Formula for multishot cost in energy is (Multishot Projectiles ^ 1.75) * 10*/);
		
				Space(20);
				Image(110, 70);
				Header(Translations.MainMenu_Guide_209/*og:Magic*/);
				Space(10);

				Stat(Translations.MainMenu_Guide_210/*og:Spell damage*/, ModdedPlayer.Stats.TotalMagicDamageMultiplier.ToString("P"), Translations.MainMenu_Guide_211/*og:Spell damage multiplier can be increased by perks, inventory items, spells, passive abilities, and attributes.\n*/ +
				Translations.MainMenu_Guide_212/*og:Bonus from intelligence: */ + (ModdedPlayer.Stats.intelligence.GetAmount() * ModdedPlayer.Stats.spellDmgFromInt.GetAmount()).ToString("P") + "\n" +
				Translations.MainMenu_Guide_213/*og:Increase to spell damage: */ + (ModdedPlayer.Stats.spellIncreasedDmg-1).ToString("P") + "\n" +
				Translations.MainMenu_Guide_175/*og:Increase to all damage: */ + (ModdedPlayer.Stats.allDamage - 1).ToString("P"));
				Stat(Translations.MainMenu_Guide_214/*og:Additional spell damage*/, ModdedPlayer.Stats.spellFlatDmg.GetFormattedAmount(), Translations.MainMenu_Guide_215/*og:Spell damage bonus can be increased by perks and inventory items. This is added to spell damage and multiplied by the stat above. Often spells take a fraction of this stat and add it to spell's damage.*/);
				Stat(Translations.MainMenu_Guide_216/*og:Spell cost reduction*/, (1 - ModdedPlayer.Stats.spellCost.GetAmount()).ToString("P"));
				Stat(Translations.MainMenu_Guide_217/*og:Spell cost redirected to stamina*/, ModdedPlayer.Stats.SpellCostToStamina.ToString("P"));
				Stat(Translations.MainMenu_Guide_218/*og:Cooldown reduction*/, (1 - ModdedPlayer.Stats.cooldown.GetAmount()).ToString("P"));
	
				Space(20);
				GUI.color = Color.red;
				Image(96, 70);
				GUI.color = Color.white;
				Header(Translations.MainMenu_Guide_219/*og:Armor reduction*/);
				Space(10);
				Stat(Translations.MainMenu_Guide_169/*og:Melee*/, ModdedPlayer.Stats.meleeArmorPiercing.GetAmount() + "", Translations.MainMenu_Guide_220/*og:Total melee armor reduction: */ + ModdedPlayer.Stats.TotalMeleeArmorPiercing.ToString());
				Stat(Translations.MainMenu_Guide_180/*og:Ranged*/, ModdedPlayer.Stats.rangedArmorPiercing.GetAmount() + "", Translations.MainMenu_Guide_221/*og:Total ranged armor reduction: */ + ModdedPlayer.Stats.TotalRangedArmorPiercing.ToString());
				//Stat(Translations.MainMenu_Guide_133/*og:Thorns*/, ModdedPlayer.Stats.thornsArmorPiercing.GetAmount() + "", Translations.MainMenu_Guide_222/*og:Total thorns armor reduction: */ + ModdedPlayer.Stats.TotalThornsArmorPiercing.ToString());
				Stat(Translations.MainMenu_Guide_223/*og:Any source*/, ModdedPlayer.Stats.allArmorPiercing.GetAmount() + "", Translations.MainMenu_Guide_224/*og:Decreases armor of enemies hit by either of the sources*/);
			}
			else if (guidePage == a++)
			{
				Header(Translations.MainMenu_Guide_225/*og:Survivor stats*/);
				Space(10);

				Stat(Translations.MainMenu_Guide_226/*og:Movement speed*/, ModdedPlayer.Stats.movementSpeed.GetAmount().ToString(), Translations.MainMenu_Guide_227/*og:Multiplier of base movement speed. Base walking speed is equal to */ + FPCharacterMod.basewalkSpeed + Translations.MainMenu_Guide_228/*og: feet per second, with bonuses it's */ + FPCharacterMod.basewalkSpeed * ModdedPlayer.Stats.movementSpeed.GetAmount() + Translations.MainMenu_Guide_229/*og: feet/second*/);
				Stat(Translations.MainMenu_Guide_230/*og:Jump power*/, ModdedPlayer.Stats.jumpPower.GetAmount().ToString(), Translations.MainMenu_Guide_231/*og:Multiplier of base jump power. Increases height of your jumps*/);
				Stat(Translations.MainMenu_Guide_232/*og:Hunger rate*/, (1 / ModdedPlayer.Stats.perk_hungerRate).ToString("P"), Translations.MainMenu_Guide_233/*og:How much slower is the rate of consuming food compared to normal.*/);
				Stat(Translations.MainMenu_Guide_234/*og:Thirst rate*/, (1 / ModdedPlayer.Stats.perk_thirstRate).ToString("P"), Translations.MainMenu_Guide_235/*og:How much slower is the rate of consuming water compared to normal.*/);
				Stat(Translations.MainMenu_Guide_236/*og:Experience gain*/, ModdedPlayer.Stats.expGain.GetFormattedAmount(), Translations.MainMenu_Guide_237/*og:Multiplier of any experience gained*/);
				Stat(Translations.MainMenu_Guide_238/*og:Massacre duration*/, ModdedPlayer.Stats.maxMassacreTime.GetAmount() + Translations.MainMenu_Guide_239/*og: s*/, Translations.MainMenu_Guide_240/*og:How long massacres can last*/);
				Stat(Translations.MainMenu_Guide_241/*og:Time on kill*/, ModdedPlayer.Stats.timeBonusPerKill.GetAmount() + Translations.MainMenu_Guide_239/*og: s*/, Translations.MainMenu_Guide_242/*og:Amount of time that is added to massacre for every kill*/);
				if (ModdedPlayer.Stats.perk_turboRaftOwners.GetAmount() > 0)
					Stat(Translations.MainMenu_Guide_243/*og:Turbo raft speed*/, ModdedPlayer.Stats.perk_RaftSpeedMultipier.GetFormattedAmount(), Translations.MainMenu_Guide_244/*og:Speed multiplier of rafts. Other player's items and perks also affect this value*/);
				Stat(Translations.MainMenu_Guide_245/*og:Magic find*/, ModdedPlayer.Stats.magicFind.Value.ToString("P"), Translations.MainMenu_Guide_246/*og:Affects rarity of items looted from monsters, as well as the chance to get items from non-elite enemies. Increases globally, and this value is affected by every player. */);
				foreach (var mfStat in ModdedPlayer.Stats.magicFind.OtherPlayerValues)
				{
					Stat(mfStat.Key+Translations.MainMenu_Guide_247/*og:'s Magic Find*/, mfStat.Value.ToString("P"), Translations.MainMenu_Guide_248/*og:Magic find from other players*/);
				}
	
				Space(40);
				Image(90, 70);
				Header(Translations.MainMenu_Guide_249/*og:Inventory Stats*/);
				Space(10);
				foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> pair in ModdedPlayer.instance.ExtraCarryingCapactity)
				{
					string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Value.ID, (pair.Value.Amount > 1), false);
					Stat(item_name, "+" + pair.Value.Amount, Translations.MainMenu_Guide_250/*og:How many extra '*/ + item_name + Translations.MainMenu_Guide_251/*og:' you can carry. Item ID is */ + pair.Value.ID);
				}
				Space(10);
				if (ModdedPlayer.instance.GeneratedResources.Count > 0)
					Header(Translations.MainMenu_Guide_252/*og:Generated resources*/);
				foreach (var pair in ModdedPlayer.instance.GeneratedResources)
				{
					string item_name = TheForest.Utils.Scene.HudGui.GetItemName(pair.Key, (pair.Value > 1), false);
					Stat(item_name, pair.ToString(), Translations.MainMenu_Guide_253/*og:How many '*/ + item_name + Translations.MainMenu_Guide_254/*og:' you generate daily. Item ID is */ + pair.Key);
				}
			}
			else if (guidePage == a++)
			{
				if (BookPositionY < Screen.height && BookPositionY > -140 * screenScale)
				{
					Rect labelRect = new Rect(GuideWidthDecrease * screenScale + GuideMargin * screenScale, BookPositionY, Screen.width - 2 * screenScale * (GuideMargin + GuideWidthDecrease), 85 * screenScale);
					if (GUI.Button(labelRect, Translations.MainMenu_Guide_255/*og:Bugged stats? Click to reset*/, new GUIStyle(GUI.skin.button)
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