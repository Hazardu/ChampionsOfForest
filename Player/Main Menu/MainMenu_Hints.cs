using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Localization;
using ChampionsOfForest.Player;
using ChampionsOfForest.Player.Crafting;

using TheForest.Utils;

using UnityEngine;
using UnityEngine.SceneManagement;

using Input = UnityEngine.Input;
using Random = UnityEngine.Random;
using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
	public partial class MainMenu
	{

		private static readonly STuple<Func<bool>, string>[] hints =
		{
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 4 , "Gain experience by hunting"),
			new STuple<Func<bool>, string>(()=> ModReferences.Players.Count>1, "Experience is shared between players"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 8 , "Gain experience by killing enemies"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 8 && ModdedPlayer.instance.MutationPoints>0 , "Spend points on abilities and perks. Access upgrades by pressing "+ ModAPI.Input.GetKeyBindingAsString("MenuToggle")),
			new STuple<Func<bool>, string>(()=> Inventory.Instance.IsEmpty, "Obtain your first piece of equipment by breaking effigies"),
			new STuple<Func<bool>, string>(()=> Inventory.Instance.IsNaked, "Equip armor, weapons and trinklets to become stronger. Open inventory by pressing "+ ModAPI.Input.GetKeyBindingAsString("MenuToggle")),
			new STuple<Func<bool>, string>(()=>  ModdedPlayer.instance.level >= 10 && !SpellCaster.instance.infos.Any(x=>x.spell!=null), "Unlock powerful abilities and upgrade them through perks"),
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=null), "You need energy to cast spells. Eat, drink or sit to regain energy"),
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=SpellDataBase.spellDictionary[26]), "Firebolt ability equips an invisible weapon (placeholder effect bruh). Left click after casting to quickly send out balls of fire at enemies. Each attack consumes energy"),
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=SpellDataBase.spellDictionary[26]), "If you sit down with firebolt spell active, you can become a turret"),
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=SpellDataBase.spellDictionary[27]), "Snowstorm ability equips an invisible weapon (placeholder effect bruh). Left click after casting to channel a snowstorm around you"),
			new STuple<Func<bool>, string>(()=>  ModdedPlayer.instance.level >= 30 && SpellCaster.instance.infos.Any(x=>x.spell==null), "Obtain 6 different abilities as soon as possible"),
			new STuple<Func<bool>, string>(()=>  ModdedPlayer.instance.level >= 10 && PerkDatabase.perks.Any(x=>!x.isBought && x.cost==0 && x.levelReq<=ModdedPlayer.instance.level), "There are perks you can unlock for free right now!"),
			new STuple<Func<bool>, string>(()=>  Inventory.Instance.ItemSlots[-12]!=null && Inventory.Instance.ItemSlots[-12].Equipped, "Draw the weapon from your main hand equipment slot by pressing " + ModAPI.Input.GetKeyBindingAsString("EquipWeapon")),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 15 , "Increase the difficulty whenever you think enemies become pushovers"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 25 && ModdedPlayer.Stats.TotalMeleeArmorPiercing + ModdedPlayer.Stats.TotalRangedArmorPiercing < 15 , "Armored enemies are the bane of warriors and archers. Increase your Armor Piercing stat or use strong fire to melt armor"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 15 , "Jack of all trades loses to a master of one. Focus your build on a single playstyle to maximize effectiveness"),
			new STuple<Func<bool>, string>(()=> true , "You can mark locations, items and enemies to better communicate with other players. Hold "+ ModAPI.Input.GetKeyBindingAsString("ping") + " to place a marker" ),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 5 , "You can reset all upgrades by consuming a Heart of Purity"),
			new STuple<Func<bool>, string>(()=> true , "Do not forget about defense. Invest in resistance to stuns, abilities to get away from danger, or sufficient toughness to soak incoming damage"),
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() > PlayerUtils.GetPlayerMeleeDamageRating() , "Maximize ranged damage by aiming for the head"),
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() > PlayerUtils.GetPlayerMeleeDamageRating() , "Bullets fare better against mutants than arrows"),
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() > PlayerUtils.GetPlayerMeleeDamageRating() , "You can combine archery with magic for massive burst damage"),
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() < PlayerUtils.GetPlayerMeleeDamageRating() , "Maximize melee damage by parrying attackers"),
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() < PlayerUtils.GetPlayerMeleeDamageRating() , "You can combine melee with magic for short bursts of great damage"),
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() < PlayerUtils.GetPlayerMeleeDamageRating() , "Parrying does not count as getting hit. You can parry basher and poisonous enemies"),
			new STuple<Func<bool>, string>(()=> true , "Setting enemies ablaze will make them stop attacking"),
			new STuple<Func<bool>, string>(()=> true , "You can shift click during spell selection menu to change a key binding"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 35 , "Defensive perks can negate elite abilities - Black holes cannot suck you in"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 35 , "Defensive perks can negate elite abilities - Blizzard no longer slows you"),
			new STuple<Func<bool>, string>(()=> ModSettings.difficulty >= ModSettings.Difficulty.Elite , "There are items that change how certain spells work"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.Stats.explosionDamage.GetAmount() > 2 , "Explosive damage stat of one player affects explosions caused by all players"),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 60 && !PerkDatabase.perks[89].isBought, "There are powerful, hidden upgrades to be discovered"),

		};
		int currentHint;
		void GetNextHint()
		{
			for (int i = currentHint + 1; i < hints.Length; i++)
			{
				if (hints[i].item0.Invoke())
				{
					currentHint = i;
					return;
				}
			}
			for (int i = 0; i < hints.Length; i++)
			{
				if (hints[i].item0.Invoke())
				{
					currentHint = i;
					return;
				}
			}
		}
		void DrawHints()
		{
			if (GUI.Button(new Rect(Screen.width - screenScale * 600f, 700f * screenScale, screenScale * 600f, 200f * screenScale), "Next hint", hintStyle))
			{
				GetNextHint();
			}
			if (currentHint == -1)
				return;
			GUI.Label(new Rect(Screen.width - screenScale * 600f, 300f * screenScale, screenScale * 600f, 400f * screenScale), hints[currentHint].item1, hintStyle);
		}
	}
}