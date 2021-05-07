using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using ChampionsOfForest.Player;

using TheForest.Save;
using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class Serializer : MonoBehaviour
	{
		private static bool Saving = false;

		private static Serializer Instance
		{
			get;
			set;
		}

		private static void CreateInstance()
		{
			if (Instance == null)
			{
				Instance = new GameObject().AddComponent<Serializer>();
			}
		}

		private void OnApplicationQuit()
		{
			EmergencySave();
		}

		private void DoLoad(string path, out float HealthPercentage, out Dictionary<int, int> ExtraCarriedItems)
		{
			ExtraCarriedItems = new Dictionary<int, int>();
			HealthPercentage = 1;
			try
			{
				byte[] bytes = File.ReadAllBytes(path);
				BinaryReader buf = new BinaryReader(new MemoryStream(bytes));

				//reading in the same order to saving
				string version = buf.ReadString();
				if (ModSettings.RequiresNewSave)
				{
					var ver = Res.ResourceLoader.CompareVersion(version, ModSettings.RequiresNewSaveVersion);
					if (ver == Res.ResourceLoader.Status.Newer)
					{
						CotfUtils.Log("last time cotf was played on this save was on version: " + version + "  \ndue to issues with this and following updates, new save will be used. Sorry for inconvienience");
						return;
					}
				}
				ModdedPlayer.instance.ExpCurrent = buf.ReadInt64();                 //buf.Write(ModdedPlayer.instance.ExpCurrent);
				HealthPercentage = buf.ReadSingle();                          //buf.Write(LocalPlayer.Stats.Health / ModdedPlayer.Stats.TotalMaxHealth);
				ModdedPlayer.instance.MutationPoints = buf.ReadInt32();             //buf.Write(ModdedPlayer.instance.MutationPoints);
				ModdedPlayer.instance.level = buf.ReadInt32();                      //buf.Write(ModdedPlayer.instance.Level);
				ModdedPlayer.instance.AssignLevelAttributes();
				ModdedPlayer.instance.PermanentBonusPerkPoints = buf.ReadInt32();   //buf.Write(ModdedPlayer.instance.PermanentBonusPerkPoints);
				ModdedPlayer.instance.LastDayOfGeneration = buf.ReadInt32();        //buf.Write(ModdedPlayer.instance.LastDayOfGeneration);
				ModdedPlayer.instance.ExpGoal = ModdedPlayer.instance.GetGoalExp();
				//extra carried item amounts
				//key - ID of an item
				//value - amount at the moment of saving
				int ExtraItemCount = buf.ReadInt32();                               //buf.Write(ModdedPlayer.instance.ExtraCarryingCapactity.Count);
				for (int i = 0; i < ExtraItemCount; i++)
				{
					int ID = buf.ReadInt32();
					int AMOUNT = buf.ReadInt32();
					ExtraCarriedItems.Add(ID, AMOUNT);
				}
				//loading inventory
				int ItemSlotCount = buf.ReadInt32();
				for (int i = 0; i < ItemSlotCount; i++)
				{
					int Slot = buf.ReadInt32();
					int ID = buf.ReadInt32();

					if (ID != -1)
					{
						int LVL = buf.ReadInt32();
						int AMO = buf.ReadInt32();
						int StatCount = buf.ReadInt32();

						Item LoadedItem = new Item(ItemDataBase.ItemBases[ID], AMO, 0, false)
						{
							level = LVL
						};

						for (int a = 0; a < StatCount; a++)
						{
							int statID = buf.ReadInt32();
							int statgroupID = buf.ReadInt32();
							float statAMO = buf.ReadSingle();

							ItemStat stat = new ItemStat(ItemDataBase.Stats[statID],1,statgroupID)
							{
								Amount = statAMO
							};

							LoadedItem.Stats.Add(stat);
							LoadedItem.SortStats();
						}
						Player.Inventory.Instance.ItemSlots[Slot] = LoadedItem;
					}
					else
					{
						Player.Inventory.Instance.ItemSlots[Slot] = null;
					}
				}

				//loading perks
				int perkCount = buf.ReadInt32();
				for (int i = 0; i < perkCount; i++)
				{
					int ID = buf.ReadInt32();
					bool isBought = buf.ReadBoolean();
					int ApplyCount = buf.ReadInt32();
					PerkDatabase.perks[ID].isBought = isBought;
					if (isBought)
					{
						PerkDatabase.perks[ID].boughtTimes = ApplyCount;
						if (ApplyCount == 0)
						{
							PerkDatabase.perks[ID].isApplied = true;
							PerkDatabase.perks[ID].apply();
						}
						else
						{
							for (int a = 0; a < ApplyCount; a++)
							{
								PerkDatabase.perks[ID].apply();
							}
							PerkDatabase.perks[ID].isApplied = true;
						}
						PerkDatabase.perks[ID].OnBuy();
					}
				}

				//loading bought spells
				int spellCount = buf.ReadInt32();
				for (int i = 0; i < spellCount; i++)
				{
					int ID = buf.ReadInt32();
					bool isBought = buf.ReadBoolean();
					SpellDataBase.spellDictionary[ID].Bought = isBought;
				}

				//loading spell loadout
				int spellLoadoutCount = buf.ReadInt32();
				for (int i = 0; i < spellLoadoutCount; i++)
				{
					int ID = buf.ReadInt32();
					if (ID == -1)
					{
						SpellCaster.instance.infos[i].spell = null;
					}
					else
					{
						SpellCaster.instance.infos[i].spell = SpellDataBase.spellDictionary[ID];
						SpellCaster.instance.infos[i].spell.EquippedSlot = i;
					}
				}

				ModdedPlayer.ReapplyAllSpell();
			}
			catch (System.Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}

		private static void DoSave()
		{
			Saving = true;
			while (Path() == string.Empty)
			{
				Thread.Sleep(20);
			}
			//Thread.Sleep(1000);

			MemoryStream stream = new MemoryStream();
			BinaryWriter buf = new BinaryWriter(stream);

			//saving modded player variables
			buf.Write(ModSettings.Version);
			buf.Write(ModdedPlayer.instance.ExpCurrent);
			buf.Write(LocalPlayer.Stats.Health / ModdedPlayer.Stats.TotalMaxHealth);
			buf.Write(ModdedPlayer.instance.MutationPoints);
			buf.Write(ModdedPlayer.instance.level);
			buf.Write(ModdedPlayer.instance.PermanentBonusPerkPoints);
			buf.Write(ModdedPlayer.instance.LastDayOfGeneration);

			//saving extra item amounts to not loose any extra
			//im doing that because the game trims the excess on load
			buf.Write(ModdedPlayer.instance.ExtraCarryingCapactity.Count);
			foreach (KeyValuePair<int, ModdedPlayer.ExtraItemCapacity> item in ModdedPlayer.instance.ExtraCarryingCapactity)
			{
				buf.Write(item.Value.ID);
				buf.Write(LocalPlayer.Inventory.AmountOf(item.Value.ID));
			}

			//saving inventory
			buf.Write(Player.Inventory.Instance.ItemSlots.Count);

			foreach (KeyValuePair<int, Item> item in Player.Inventory.Instance.ItemSlots)
			{
				buf.Write(item.Key);

				if (item.Value != null)
				{
					//save the slot id

					//save individual item
					buf.Write(item.Value.ID);
					buf.Write(item.Value.level);
					buf.Write(item.Value.Amount);
					buf.Write(item.Value.Stats.Count);
					//save every stat
					for (int i = 0; i < item.Value.Stats.Count; i++)
					{
						buf.Write(item.Value.Stats[i].StatID);
						buf.Write(item.Value.Stats[i].possibleStatsIndex);
						buf.Write(item.Value.Stats[i].Amount);
					}
				}
				else
				{
					buf.Write(-1);
				}
			}

			//saving perks
			buf.Write(PerkDatabase.perks.Count);
			foreach (Perk item in PerkDatabase.perks)
			{
				buf.Write(item.id);
				buf.Write(item.isBought);
				buf.Write(item.boughtTimes);
			}

			//saving bought spells
			buf.Write(SpellDataBase.spellDictionary.Count);
			foreach (KeyValuePair<int, Spell> pair in SpellDataBase.spellDictionary)
			{
				buf.Write(pair.Value.ID);
				buf.Write(pair.Value.Bought);
			}

			//saving spell loadout
			buf.Write(SpellCaster.instance.infos.Length);
			foreach (SpellCaster.SpellInfo info in SpellCaster.instance.infos)
			{
				if (info.spell == null)
				{
					buf.Write(-1);
				}
				else
				{
					buf.Write(info.spell.ID);
				}
			}

			//finally
			//saving to a file

			//checking if dir exists
			if (!Directory.Exists(Path()))
			{
				Directory.CreateDirectory(Path());
			}

			string path = Path() + "/COTF.save";
			File.WriteAllBytes(path, stream.ToArray());
			Saving = false;
		}

		private IEnumerator DoLoadCoroutine()
		{
			while (LocalPlayer.Stats == null || LocalPlayer.Inventory == null)
			{
				yield return null;
			}

			yield return null;
			yield return null;
			yield return null;
			yield return null;
			yield return null;

			//waits some frames

			string path = Path();
			if (path == string.Empty)
			{
				yield break;
			}
			//checking and cancelling loading routine if there is no directory of the save
			if (!Directory.Exists(path))
			{
				yield break;
			}

			path += "/COTF.save";
			if (!File.Exists(path))
			{
				yield break;
			}

			yield return new WaitForSeconds(1f);

			DoLoad(path, out float HealthPercentage, out Dictionary<int, int> ExtraCarriedItems);

			//waiting for buffs and perks to apply
			yield return new WaitForSeconds(5f);

			
			ModdedPlayer.ResetAllStats();
			//bringing health back to correct amount
			yield return null;
			LocalPlayer.Stats.Health = ModdedPlayer.Stats.TotalMaxHealth * HealthPercentage;
			//waiting for buffs and perks to apply
			yield return new WaitForSeconds(6f);
			//fixing missing items
			foreach (KeyValuePair<int, int> item in ExtraCarriedItems)
			{
				int amount = LocalPlayer.Inventory.AmountOf(item.Key);
				if (amount < item.Value)
				{
					int toAdd = item.Value - amount;
					LocalPlayer.Inventory.AddItem(item.Key, toAdd);
				}
			}
		}

		public static void EmergencySave()
		{
			if (GameSetup.IsMpClient)
			{
				ModAPI.Console.Write("Saving");
				CreateInstance();
				if (!Saving)
				{
					DoSave();
				}
			}
		}

		public static void Save()
		{
			if (ModSettings.IsDedicated)
				return;
			CreateInstance();
			if (!Saving)
			{
				Thread saveThread = new Thread(DoSave);
				saveThread.Start();
			}
		}

		public static void Load()
		{
			if (ModSettings.IsDedicated)
				return;
			CreateInstance();
			Instance.StartCoroutine(Instance.DoLoadCoroutine());
		}

		public static string Path()
		{
			if (GameSetup.IsMultiplayer)
			{
				if (GameSetup.IsMpClient)
				{
					return "Mods/Champions of the Forest/Multiplayer/" + PlayerSpawn.GetClientSaveFileName();
				}
				else if (GameSetup.IsMpServer)
				{
					if (GameSetup.Slot.ToString() == "0")
					{
						return string.Empty;
					}

					return "Mods/Champions of the Forest/Multiplayer/" + GameSetup.Slot.ToString();
				}
			}
			if (GameSetup.Slot.ToString() == "0")
			{
				return string.Empty;
			}

			return "Mods/Champions of the Forest/Singleplayer/" + GameSetup.Slot.ToString();
		}
	}

	public class SaveOverride : PlayerStats
	{
		public override void OnSaveSlotSelected()
		{
			if (!this.Dead)
			{
				Serializer.Save();
			}
			base.OnSaveSlotSelected();
		}

		public override void JustSave()
		{
			if (!this.Dead && GameSetup.IsMpClient)
			{
				Serializer.Save();
			}
			base.JustSave();
		}
	}
}