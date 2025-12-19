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
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 4 , Translations.MainMenu_Hints_1), //tr
			new STuple<Func<bool>, string>(()=> ModReferences.Players.Count>1, Translations.MainMenu_Hints_2), //tr
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 8 , Translations.MainMenu_Hints_3), //tr
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 8 && ModdedPlayer.instance.MutationPoints>0 , Translations.MainMenu_Hints_4+ //tr
ModAPI.Input.GetKeyBindingAsString("MenuToggle")), 
			new STuple<Func<bool>, string>(()=> Inventory.Instance.IsEmpty, Translations.MainMenu_Hints_5), //tr
			new STuple<Func<bool>, string>(()=> Inventory.Instance.IsNaked, Translations.MainMenu_Hints_6+//tr
				ModAPI.Input.GetKeyBindingAsString("MenuToggle")), 
			new STuple<Func<bool>, string>(()=>  ModdedPlayer.instance.level >= 10 && !SpellCaster.instance.infos.Any(x=>x.spell!=null), Translations.MainMenu_Hints_7), //tr
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=null), Translations.MainMenu_Hints_8), //tr
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=SpellDataBase.spellDictionary[26]), Translations.MainMenu_Hints_9), //tr
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=SpellDataBase.spellDictionary[26]), Translations.MainMenu_Hints_10), //tr
			new STuple<Func<bool>, string>(()=>  SpellCaster.instance.infos.Any(x=>x.spell!=SpellDataBase.spellDictionary[27]), Translations.MainMenu_Hints_11), //tr
			new STuple<Func<bool>, string>(()=>  ModdedPlayer.instance.level >= 30 && SpellCaster.instance.infos.Any(x=>x.spell==null), Translations.MainMenu_Hints_12), //tr
			new STuple<Func<bool>, string>(()=>  ModdedPlayer.instance.level >= 10 && PerkDatabase.perks.Any(x=>!x.isBought && x.cost==0 && x.levelReq<=ModdedPlayer.instance.level), Translations.MainMenu_Hints_13), //tr
			new STuple<Func<bool>, string>(()=>  Inventory.Instance.ItemSlots[-12]!=null && Inventory.Instance.ItemSlots[-12].isEquipped, Translations.MainMenu_Hints_14 + //tr
ModAPI.Input.GetKeyBindingAsString("EquipWeapon")), 
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 15 , Translations.MainMenu_Hints_15), //tr
		new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level < 25 && ModdedPlayer.Stats.TotalMeleeArmorPiercing + ModdedPlayer.Stats.TotalRangedArmorPiercing < 15 , Translations.MainMenu_Hints_16), //tr
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 15 , Translations.MainMenu_Hints_17), //tr
			new STuple<Func<bool>, string>(()=> true ,Translations.MainMenu_Hints_18(//tr
				ModAPI.Input.GetKeyBindingAsString("ping")) ),
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 5 , Translations.MainMenu_Hints_19), //tr
			new STuple<Func<bool>, string>(()=> true , Translations.MainMenu_Hints_20), //tr
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() > PlayerUtils.GetPlayerMeleeDamageRating() , Translations.MainMenu_Hints_21), //tr
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() > PlayerUtils.GetPlayerMeleeDamageRating() , Translations.MainMenu_Hints_22), //tr
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() > PlayerUtils.GetPlayerMeleeDamageRating() , Translations.MainMenu_Hints_23), //tr
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() < PlayerUtils.GetPlayerMeleeDamageRating() , Translations.MainMenu_Hints_24), //tr
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() < PlayerUtils.GetPlayerMeleeDamageRating() , Translations.MainMenu_Hints_25), //tr
			new STuple<Func<bool>, string>(()=> PlayerUtils.GetPlayerRangedDamageRating() < PlayerUtils.GetPlayerMeleeDamageRating() , Translations.MainMenu_Hints_26), //tr
			new STuple<Func<bool>, string>(()=> true , Translations.MainMenu_Hints_27), //tr
			new STuple<Func<bool>, string>(()=> true , Translations.MainMenu_Hints_28), //tr
		new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 35 , Translations.MainMenu_Hints_29), //tr
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 35 , Translations.MainMenu_Hints_30), //tr
			new STuple<Func<bool>, string>(()=> ModSettings.difficulty >= ModSettings.Difficulty.Elite , Translations.MainMenu_Hints_31), //tr
			new STuple<Func<bool>, string>(()=> ModdedPlayer.Stats.explosionDamage.GetAmount() > 2 , Translations.MainMenu_Hints_32), //tr
			new STuple<Func<bool>, string>(()=> ModdedPlayer.instance.level > 60 && !PerkDatabase.perks[89].isBought, Translations.MainMenu_Hints_33), //tr

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