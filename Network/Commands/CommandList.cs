using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Enemies;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Player;
using TheForest.Utils;
using UnityEngine;


using static ChampionsOfForest.Network.Commands.CommandType;
using ChampionsOfForest.Network.CommandParams;

namespace ChampionsOfForest.Network
{
	public partial class Commands
	{
		internal Commands()
		{
			registeredCommands = new RegisteredCommand[]
			{
				new RegisteredCommand("NETWORK_STAT_UPDATE",
				(r)=> {
					NetworkPlayerStats.syncedStats[r.ReadInt32()].ReceivedOtherPlayerChange(r);
				}),

				new RegisteredCommand("DIFFICULTY_INFO_REQUEST",
				(r)=> {
					if (ModSettings.DifficultyChosen)
						ModSettings.BroadCastSettingsToClients();

				}),

				new RegisteredCommand("DIFFICULTY_INFO_ANSWER",
				(r)=> {
					var str = r.ReadStruct<params_DIFFICULTY_INFO_ANSWER>();
					ModSettings.DifficultyChosen=true;
					ModSettings.difficulty = (ModSettings.GlobalDifficulty)str.Difficulty;
					ModSettings.dropsOnDeath = (ModSettings.PlayerDropsOnDeath)str.DropsOnDeath;
					ModSettings.ExpMultiplier = str.ExpMultiplier;
					ModSettings.EnemyDamageMultiplier = str.EnemyDamageMultiplier;
					ModSettings.FriendlyFireDamage = str.FriendlyFireDamage;
					ModSettings.FriendlyFire = str.FriendlyFire;
					ModSettings.KillOnDowned = str.KillOnDowned;
					ModSettings.FriendlyFireMagic = str.FriendlyFire;
				}),

				new RegisteredCommand("SPELL_BLACK_HOLE",
				(r)=> {
					BlackHole.Create(r.ReadVector3(), r.ReadStruct<params_SPELL_BLACK_HOLE>());
				}),

				new RegisteredCommand("SPELL_SANCTUARY",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								Sanctuary.Create(pos,
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadBoolean(),
									r.ReadBoolean(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadUInt64());
				}),

				new RegisteredCommand("SPELL_SUNFLARE",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
								SunFlare.Create(pos,
									r.ReadBoolean(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadSingle(),
									r.ReadUInt64());
				}),

				new RegisteredCommand("SPELL_BLACKFLAME",
				(r)=> {
					bool isOn = r.ReadBoolean();
					ulong playerID = r.ReadUInt64();
					BlackFlame.ToggleOtherPlayer(playerID, isOn);
				}),

				new RegisteredCommand("SPELL_WARCRY",
				(r)=> {
					WarCry.Cast(new Vector3( r.ReadSingle(), r.ReadSingle(), r.ReadSingle()),
						r.ReadSingle(),
						r.ReadSingle(),
						r.ReadSingle(),
						r.ReadInt32(),
						r.ReadSingle(),
						r.ReadUInt64());

				}),

				new RegisteredCommand("SPELL_PORTAL",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					float duration = r.ReadSingle();
					int id = r.ReadInt32();
					Portal.CreatePortal(pos, duration, id, r.ReadBoolean(), r.ReadBoolean());
				}),

				new RegisteredCommand("SPELL_MAGIC_ARROW",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					Vector3 dir = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());

					float dmg = r.ReadSingle();
					ulong playerID = r.ReadUInt64();
					float duration = r.ReadSingle();
					bool slow = r.ReadBoolean();
					bool dmgdebuff = r.ReadBoolean();
					if (GameSetup.IsMpServer)
					{
						MagicArrow.Create(pos, dir, dmg, playerID, duration, slow, dmgdebuff);
					}
					else
					{
						MagicArrow.CreateEffect(pos, dir, dmgdebuff, duration);
					}
				}),

				new RegisteredCommand("SPELL_PURGE",
				(r)=> {
					Purge.Cast(new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle()), r.ReadSingle(), r.ReadBoolean(), r.ReadBoolean());

				}),

				new RegisteredCommand("SPELL_CATACLYSM",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					float radius = r.ReadSingle();
					float dmg = r.ReadSingle();
					float duration = r.ReadSingle();
					bool isArcane = r.ReadBoolean();
					bool fromEnemy = r.ReadBoolean();
					Cataclysm.Create(pos, radius, dmg, duration, isArcane ? Cataclysm.TornadoType.Arcane : Cataclysm.TornadoType.Fire, fromEnemy);
				}),

				new RegisteredCommand("SPELL_BALLLIGHTNING",
				(r)=> {
					Vector3 pos = r.ReadVector3();
					Vector3 speed = r.ReadVector3();
					var param = r.ReadStruct<params_SPELL_BALLLIGHTNING>();
					if (BallLightning.lastID < param.ID)
						BallLightning.lastID = param.ID+1;
					BallLightning.Create(pos, speed, param);
				}),

				new RegisteredCommand("SPELL_BALLLIGHTNING_REQUEST",
				(r)=> {
					var cmd = new CommandStream(SPELL_BALLLIGHTNING);
					Vector3 pos = r.ReadVector3(), speed = r.ReadVector3();
					var param = r.ReadStruct<params_SPELL_BALLLIGHTNING>();
					if (BallLightning.lastID <= param.ID)
						BallLightning.lastID = param.ID;
					else
						param.ID  =++BallLightning.lastID;
					cmd.Write(pos);
					cmd.Write(speed);
					cmd.Write(param);
					cmd.Send(NetworkManager.Target.Everyone);
				}),

				new RegisteredCommand("SPELL_BALL_LIGHTNING_TRIGGER",
				(r)=> {
					uint id = r.ReadUInt32();
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					if (BallLightning.list.ContainsKey(id))
						BallLightning.list[id].CoopTrigger(pos);
				}),

				new RegisteredCommand("SPELL_PARRY",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					ParrySound.Play(pos);
					if(GameSetup.IsMpClient)
						return;

					float radius = r.ReadSingle();
					bool ignite = r.ReadBoolean();
					float dmg = r.ReadSingle();
					var hits = Physics.SphereCastAll(pos, radius, Vector3.one);
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].transform.CompareTag("enemyCollide"))
						{
							var prog = hits[i].transform.root.GetComponent<EnemyProgression>();
							if (prog == null)
								hits[i].transform.root.SendMessageUpwards("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
							else
								prog.HitPhysical(dmg);
							if (ignite)
								hits[i].transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

						}
					}
				}),

				new RegisteredCommand("SPELL_FART",
				(r)=> {
					bool grounded = r.ReadBoolean();
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					Vector3 dir = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					ulong playerID = r.ReadUInt64();

					FartSpell.CreateEffect(pos, dir, !grounded);
					if (GameSetup.IsMpServer)
					{
						FartSpell.DealDamageAsHost(pos, dir, r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), playerID);
					}
				}),

				new RegisteredCommand("SPELL_TAUNT",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					float radius = r.ReadSingle();
					if (GameSetup.IsMpServer)
					{
						float duration = r.ReadSingle();
						float slow = r.ReadSingle();
						bool pullIn = r.ReadBoolean();
						ulong playerID = r.ReadUInt64();
						var player = NetworkManager.GetModdedClient(playerID);
						if ()
						{
							Taunt.Cast(pos, radius, player.gameObject, duration, slow, pullIn);
						}
					}
					Taunt.CastEffect(pos, radius);
				}),

				new RegisteredCommand("SPELL_BASH_HIT_ENEMY",
				(r)=> {
					ulong enemy = r.ReadUInt64();
					if (EnemyManager.hostDictionary.ContainsKey(enemy))
					{
						EnemyProgression p = EnemyManager.hostDictionary[enemy];
						float duration = r.ReadSingle();
						var source = r.ReadInt32();
						float slowAmount = r.ReadSingle();
						float dmgDebuff = r.ReadSingle();
						var bleedDmg = r.ReadInt32();
						float bleedChance = r.ReadSingle();
						p.Slow(source, slowAmount, duration);
						p.DmgTakenDebuff(source, dmgDebuff, duration);
						if (UnityEngine.Random.value < bleedChance)
						{
							p.DoDoT(bleedDmg, duration);
						}
					}
				}),

				new RegisteredCommand("ITEM_REMOVE_PICKUP",
				(r)=> {
					PickUpManager.RemovePickup(r.ReadUInt64());
				}),

				new RegisteredCommand("ITEM_SPAWN_PICKUP",
				(r)=> {
					ItemTemplate baseItem = ItemDataBase.itemTemplatesById[r.ReadInt32()];
					Item item = new Item(baseItem, 1, 0, false);   //reading first value, id
					ulong id = r.ReadUInt64();
					int itemLvl = r.ReadInt32();
					item.level = itemLvl;
					int amount = r.ReadInt32();
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					int dropSource = r.ReadInt32();
					while (r.BaseStream.Position != r.BaseStream.Length)
					{
						ItemStat stat = new ItemStat(ItemDataBase.statsById[r.ReadInt32()], itemLvl, r.ReadInt32())
						{
							amount = r.ReadSingle()
						};
						item.Stats.Add(stat);
					}
					PickUpManager.SpawnPickUp(item, pos, amount, id, (ItemPickUp.DropSource)dropSource);
				}),

				new RegisteredCommand("ENEMY_STATS_INFO_BROADCAST",
				(r)=> {
					ulong packed = r.ReadUInt64();
					if (EnemyManager.hostDictionary.ContainsKey(packed))
					{
						EnemyProgression ep = EnemyManager.hostDictionary[packed];
						CommandStream cmd = new CommandStream(ENEMY_STATS_INFO_RECEIVE);
					cmd.Writer.Write(packed);
						cmd.Writer.Write(ep.enemyName);
						cmd.Writer.Write(ep.level);
						cmd.Writer.Write((float)ep.HP);
						cmd.Writer.Write(ep.DamageTotal);
						cmd.Writer.Write((float)ep.maxHealth);
						cmd.Writer.Write(ep.bounty);
						cmd.Writer.Write(ep.armor);
						cmd.Writer.Write(ep.armorReduction);
						cmd.Writer.Write(ep.Steadfast);
						cmd.Writer.Write(ep.abilities.Count);
						foreach (EnemyProgression.Abilities item in ep.abilities)
						{
							cmd.Writer.Write((int)item);
						}
						cmd.Send(NetworkManager.Target.Clients);
					}
					else
					{
						throw new Exception("No enemy with id: "+ packed);
					}
				}),

				new RegisteredCommand("ENEMY_STATS_INFO_RECEIVE",
				(r)=> {
					ulong packed = r.ReadUInt64();
					BoltEntity entity = BoltNetwork.FindEntity(new Bolt.NetworkId(packed));
					string name = r.ReadString();
					int level = r.ReadInt32();
					double health = r.ReadSingle();
					float damage = r.ReadSingle();
					double maxhealth = r.ReadSingle();
					long bounty = r.ReadInt64();
					int armor = r.ReadInt32();
					int armorReduction = r.ReadInt32();
					float steadfast = r.ReadSingle();
					int length = r.ReadInt32();
					int[] affixes = new int[length];
					for (int i = 0; i < length; i++)
					{
						affixes[i] = r.ReadInt32();
					}
					if (EnemyManager.clinetProgressions.ContainsKey(entity))
					{
						ClientEnemyProgression cp = EnemyManager.clinetProgressions[entity];
						cp.Update(entity, name, level, (float)health, damage, (float)maxhealth, bounty, armor, armorReduction, steadfast, affixes);
					}
					else
					{
						new ClientEnemyProgression(entity).Update(entity, name, level, (float)health, damage, (float)maxhealth, bounty, armor, armorReduction, steadfast, affixes);
					}
				}),

				new RegisteredCommand("SPELL_ENEMY_BLIZZARD",
				(r)=> {
					ulong enemyId = r.ReadUInt64();
					Blizzard sa = new GameObject("Blizzard").AddComponent<Blizzard>();
					sa.followTarget = BoltNetwork.FindEntity(new Bolt.NetworkId(enemyId)).transform;
				}),

				new RegisteredCommand("SPELL_ENEMY_RADIANCE",
				(r)=> {
					ulong packed = r.ReadUInt64();
					float dmg = r.ReadSingle();
					GameObject go = BoltNetwork.FindEntity(new Bolt.NetworkId(packed)).gameObject;
					FireAura.Cast(go, dmg);
				}),

				new RegisteredCommand("SPELL_ENEMY_CHAINS",
				(r)=> {
					if (ModdedPlayer.Stats.rootImmunity == 0 && ModdedPlayer.Stats.stunImmunity == 0)
					{
						Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
						if ((LocalPlayer.Transform.position - pos).sqrMagnitude < 1250)
						{
							float duration = r.ReadSingle();
							ModdedPlayer.instance.Root(duration);
							CommandStream cmd = new CommandStream(SPELL_ENEMY_CHAINS_VISUAL);
							cmd.Writer.Write(LocalPlayer.Transform.position.x);
							cmd.Writer.Write(LocalPlayer.Transform.position.y);
							cmd.Writer.Write(LocalPlayer.Transform.position.z);
							cmd.Writer.Write(duration);
							cmd.Send(NetworkManager.Target.Everyone);
						}
					}
				}),

				new RegisteredCommand("SPELL_ENEMY_CHAINS_VISUAL",
				(r)=> {
					if (ModSettings.IsDedicated)
						return;

					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					float duration = r.ReadSingle();
					RootSpell.Create(pos, duration);
				}),

				new RegisteredCommand("SPELL_ENEMY_TRAP_SPHERE",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					float duration = r.ReadSingle();
					float radius = r.ReadSingle();
					TrapSphereSpell.Create(pos, radius, duration);
				}),

				new RegisteredCommand("SPELL_ENEMY_LASAER",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					Vector3 dir = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());

					EnemyLaser.CreateLaser(pos, dir);
				}),

				new RegisteredCommand("SPELL_ENEMY_METEOR",
				(r)=> {
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					Meteor.CreateEnemy(pos, r.ReadInt32());
				}),

				new RegisteredCommand("BUFF_GIVE_PLAYER",
				(r)=> {
					ulong playerID = r.ReadUInt64();
					if (playerID == ulong.MaxValue || ModdedPlayer.PlayerID == playerID)
					{
						int source = r.ReadInt32();
						float amount = r.ReadSingle();
						float duration = r.ReadSingle();
						BuffManager.GiveBuff(3, source, amount, duration);
					}
				}),

				new RegisteredCommand("STUN_PLAYER",
				(r)=> {
					if (ModdedPlayer.Stats.stunImmunity == 0)
					{
						ulong playerID = r.ReadUInt64();
						if (ModdedPlayer.PlayerID == playerID)
						{
							float duration = r.ReadSingle();
							ModdedPlayer.instance.Stun(duration);
						}
					}
				}),

				new RegisteredCommand("EXPERIENCE_KILL",
				(r)=> {
					ModdedPlayer.instance.AddKillExperience(r.ReadInt64());
				}),

				new RegisteredCommand("EXPERIENCE_FINAL",
				(r)=> {
					ModdedPlayer.instance.AddFinalExperience(r.ReadInt64());
				}),

				new RegisteredCommand("PLAYER_STATS_INFO_REQUEST",
				(r)=> {
					ModdedPlayer.instance.SendModdedClientUpdate();
				}),

				new RegisteredCommand("PLAYER_STATS_INFO_RECEIVE",
				(r)=> {
					ModReferences.UpdatePlayerInfo( r.ReadUInt64(), r.ReadInt32(), r.ReadInt64(), r.ReadSingle(),r.ReadSingle(),r.ReadSingle(),r.ReadSingle());
				}),

				new RegisteredCommand("HITMARKER_COLOR",
				(r)=> {
					if (ModSettings.IsDedicated)
						return;

					float amount = r.ReadSingle();
					Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					Color color = new Color(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					MainMenu.CreateHitMarker(amount, pos, color);
				}),

				new RegisteredCommand("ENEMY_DEBUFF_SLOW",
				(r)=> {
					if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
					{
						ulong id = r.ReadUInt64();
						if (EnemyManager.hostDictionary.ContainsKey(id))
						{
							float amount = r.ReadSingle();
							float time = r.ReadSingle();
							int src = r.ReadInt32();
							EnemyManager.hostDictionary[id].Slow(src, amount, time);
						}
					}
				}),

				new RegisteredCommand("ENEMY_DEBUFF_FIRE",
				(r)=> {
					if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
					{
						ulong enemyId = r.ReadUInt64();
						if (EnemyManager.hostDictionary.ContainsKey(enemyId))
						{
							float amount = r.ReadSingle();
							float time = r.ReadSingle();
							int src = r.ReadInt32();
							EnemyManager.hostDictionary[enemyId].FireDebuff(src, amount, time);
						}
					}
				}),

				new RegisteredCommand("ENEMY_DEBUFF_DOT",
				(r)=> {
					ulong id = r.ReadUInt64();
					if (EnemyManager.hostDictionary.ContainsKey(id))
					{
						EnemyProgression p = EnemyManager.hostDictionary[id];
						p.DoDoT(r.ReadSingle(), r.ReadSingle());
					}
				}),

				new RegisteredCommand("ENEMY_DEBUFF_DAMAGE_TAKEN",
				(r)=> {
					if (GameSetup.IsMpServer)
					{
						ulong enemyId = r.ReadUInt64();
						if (EnemyManager.hostDictionary.ContainsKey(enemyId))
						{
							EnemyProgression p = EnemyManager.hostDictionary[enemyId];
							var source = r.ReadInt32();
							float amount = r.ReadSingle();
							float duration = r.ReadSingle();
							p.DmgTakenDebuff(source, amount, duration);
						}
					}
				}),

				new RegisteredCommand("ENEMY_KNOCKBACK",
				(r)=> {
					if (GameSetup.IsMpServer)
					{
						ulong enemyId = r.ReadUInt64();
						if (EnemyManager.hostDictionary.ContainsKey(enemyId))
						{
							var enemy = EnemyManager.hostDictionary[enemyId];
							enemy.AddKnockbackByDistance(new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle()), r.ReadSingle());
						}
					}
				}),

				new RegisteredCommand("ENEMY_STATS_UPDATE_VOLATILE_REQUEST",
				(r)=> {
					var entity = BoltNetwork.FindEntity(new Bolt.NetworkId(r.ReadUInt64()));
					if (EnemyManager.clinetProgressions.ContainsKey(entity))
					{
						ClientEnemyProgression cp = EnemyManager.clinetProgressions[entity];
						cp.UpdateDynamic(r.ReadSingle(), r.ReadInt32(), r.ReadInt32(), r.ReadSingle());
						Debug.Log("Received update dynamic client enemy cmd");
					}
					else
					{
						throw new Exception("Could not update dynamic progression, entity is "+ entity);
					}
				}),

				new RegisteredCommand("ENEMY_STATS_UPDATE_VOLATILE_ANSWER",
				(r)=> {
					ulong packed = r.ReadUInt64();
					if (EnemyManager.hostDictionary.TryGetValue(packed, out var enemy))
					{
						CommandStream cmd = new CommandStream(ENEMY_STATS_UPDATE_VOLATILE_REQUEST);
						cmd.Writer.Write(packed);
						cmd.Writer.Write((float)enemy.HP);
						cmd.Writer.Write(enemy.armor);
						cmd.Writer.Write(enemy.armorReduction);
						cmd.Writer.Write(enemy.DamageAmp);
						cmd.Send(NetworkManager.Target.Clients);
						UnityEngine.Debug.Log("ENEMY_STATS_UPDATE_VOLATILE request answered");
					}
					else
					{
						throw new Exception("Host doesnt have enemy with that packed");

					}
				}),

				new RegisteredCommand("ITEM_PICKUP_PLAYER_COLLECTED",
				(r)=> {
					if (GameSetup.IsMpServer)
					{
						ulong itemID = r.ReadUInt64();
						if (PickUpManager.PickUps.ContainsKey(itemID))
						{
							int itemAmount = r.ReadInt32();
							string playerID = r.ReadString();

							if (PickUpManager.PickUps[itemID].amount > 0)
							{
								int givenAmount = itemAmount;
								if (itemAmount > PickUpManager.PickUps[itemID].amount)
								{
									givenAmount = Mathf.Min(PickUpManager.PickUps[itemID].amount, itemAmount);
								}

								NetworkUtils.SendItemToPlayer(PickUpManager.PickUps[itemID].item, playerID, givenAmount);

								PickUpManager.PickUps[itemID].amount -= givenAmount;

								if (PickUpManager.PickUps[itemID].amount > 0)
								{
									return;
								}
							}
						}
						CommandStream cmd = new CommandStream(ITEM_REMOVE_PICKUP);
						cmd.Writer.Write(itemID);
						cmd.Send(NetworkManager.Target.Clients);
					}
				}),

				new RegisteredCommand("ITEM_ADD_TO_INVENTORY",
				(r)=> {
					if (ModdedPlayer.PlayerID == r.ReadUInt64())
					{
						//creating the item.
						Item item = new Item(ItemDataBase.itemTemplatesById[r.ReadInt32()], r.ReadInt32(), 0, false)
						{
							level = r.ReadInt32()
						};

						//adding stats to the item
						while (r.BaseStream.Position != r.BaseStream.Length)
						{
							int statId = r.ReadInt32();
							int statPoolIdx = r.ReadInt32();
							ItemStat stat = new ItemStat(ItemDataBase.statsById[statId], 1, statPoolIdx)
							{
								amount = r.ReadSingle()
							};
							item.Stats.Add(stat);
						}

						Inventory.Instance.AddItem(item, item.Amount);
					}
				}),

				new RegisteredCommand("EQUIP_CUSTOM_WEAPON",
				(r)=> {
					ulong playerID = r.ReadUInt64();
					int weaponID = r.ReadInt32();
					var client = NetworkManager.GetModdedClient(playerID);
					CoopCustomWeapons.SetWeaponOn(client.rightHand.transform, weaponID);
				}),

				new RegisteredCommand("ENEMY_SYNC_APPEARANCE_REQUEST",
				(r)=> {
					if (GameSetup.IsMpServer)
					{
						ulong id = r.ReadUInt64();
						if (EnemyManager.hostDictionary.ContainsKey(id))
						{
							EnemyProgression p = EnemyManager.hostDictionary[id];
							p.SyncAppearance(id);
						}
					}
				}),

				new RegisteredCommand("ENEMY_SYNC_APPEARANCE",
				(r)=> {
					ulong enemyId = r.ReadUInt64();
					float dmg = r.ReadSingle();
					float scale = r.ReadSingle();
					Color color = new Color(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					List<EnemyProgression.Abilities> abilities = new List<EnemyProgression.Abilities>();
					while (r.BaseStream.Position != r.BaseStream.Length)
						abilities.Add((EnemyProgression.Abilities)r.ReadInt32());
					new ClientEnemy(enemyId, dmg, scale, color, abilities);
				}),

				new RegisteredCommand("PING_CLEAR",
				(r)=> {
					ulong playerID = r.ReadUInt64();
					if (MainMenu.Instance.otherPlayerPings.ContainsKey(playerID))
						MainMenu.Instance.otherPlayerPings.Remove(playerID);
				}),

				new RegisteredCommand("PING_PLACE",
				(r)=> {                 
					ulong playerID = r.ReadUInt64();
					MarkObject.PingType pingType = (MarkObject.PingType)r.ReadInt32();
					switch (pingType)
					{
						case MarkObject.PingType.Enemy:
							ulong enemyId = r.ReadUInt64();
							bool isElite = r.ReadBoolean();
							string name = r.ReadString();
							BoltEntity entity = BoltNetwork.FindEntity(new Bolt.NetworkId(enemyId));
							Transform tr = entity.transform;
							if (playerID == ModdedPlayer.PlayerID)
								MainMenu.Instance.localPlayerPing = new MarkEnemy(tr, name, isElite, entity);
							else
							{
								if (MainMenu.Instance.otherPlayerPings.ContainsKey(playerID))
									MainMenu.Instance.otherPlayerPings[playerID] = new MarkEnemy(tr, name, isElite, entity);
								else
									MainMenu.Instance.otherPlayerPings.Add(playerID, new MarkEnemy(tr, name, isElite, entity));

							}
							break;

						case MarkObject.PingType.Location:
							float x = r.ReadSingle(), y = r.ReadSingle(), z = r.ReadSingle();
							if (playerID == ModdedPlayer.PlayerID)
								MainMenu.Instance.localPlayerPing = new MarkPostion(new Vector3(x, y, z));
							else
							{
								if (MainMenu.Instance.otherPlayerPings.ContainsKey(playerID))
									MainMenu.Instance.otherPlayerPings[playerID] = new MarkPostion(new Vector3(x, y, z));
								else
									MainMenu.Instance.otherPlayerPings.Add(playerID, new MarkPostion(new Vector3(x, y, z)));
							}

							break;

						case MarkObject.PingType.Item:
							ulong PickupID = r.ReadUInt64();
							if (PickUpManager.PickUps.ContainsKey(PickupID))
							{
								var pu = PickUpManager.PickUps[PickupID];
								if (playerID == ModdedPlayer.PlayerID)
									MainMenu.Instance.localPlayerPing = new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity);
								else
								{
									if (MainMenu.Instance.otherPlayerPings.ContainsKey(playerID))
										MainMenu.Instance.otherPlayerPings[playerID] = new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity);
									else
										MainMenu.Instance.otherPlayerPings.Add(playerID, new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity));
								}
							}
							break;
					}

				}),

				new RegisteredCommand("PING_ENEMY_INFO",
				(r)=> {
					if (GameSetup.IsMpServer)
					{
						ulong playerId = r.ReadUInt64();
						ulong enemyId = r.ReadUInt64();
						if (EnemyManager.hostDictionary.ContainsKey(enemyId))
						{
							var enemy = EnemyManager.hostDictionary[enemyId];
							CommandStream cmd = new CommandStream(PING_PLACE);
							cmd.Writer.Write(playerId);
							cmd.Writer.Write(0);
							cmd.Writer.Write(enemyId);
							cmd.Writer.Write(enemy._rarity != EnemyProgression.EnemyRarity.Normal);
							cmd.Writer.Write(enemy.enemyName);
							cmd.Send(NetworkManager.Target.Everyone);
							
						}
					}
				}),

				new RegisteredCommand("SPELL_BALLLIGHTNING_CLIENT_VISUAL",
				(r)=> {
					ulong playerId = r.ReadUInt64();
					if (ModdedPlayer.PlayerID == playerId)
					{
						Vector3 pos = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
						SpellActions.CastBallLightningVisual(pos, Vector3.down);
					}
				}),

				new RegisteredCommand("ITEM_ARCHANGEL_FF",
				(r)=> {
					ulong playerId = r.ReadUInt64();
					if (ModdedPlayer.PlayerID == playerId)
					{
						BuffManager.GiveBuff(25, 91, r.ReadSingle(), 10);
						BuffManager.GiveBuff(9, 92, 1.35f, 30);
						LocalPlayer.Stats.Energy += ModdedPlayer.Stats.TotalMaxEnergy / 10f;
						ModdedPlayer.instance.damageAbsorbAmounts[2] = r.ReadSingle();
					}
				}),

				new RegisteredCommand("BUFF_ADD",
				(r)=> {
					ulong playerId = r.ReadUInt64();
					if (ModdedPlayer.PlayerID == playerId)
						BuffManager.GiveBuff(r.ReadInt32(), r.ReadInt32(), r.ReadSingle(), r.ReadSingle());
				}),

				new RegisteredCommand("BUFF_ADD_AOE",
				(r)=> {
					var vector = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
					var dist = r.ReadSingle();
					if ((vector - LocalPlayer.Transform.position).sqrMagnitude <= dist * dist)
						BuffManager.GiveBuff(r.ReadInt32(), r.ReadInt32(), r.ReadSingle(), r.ReadSingle());
				}),

				new RegisteredCommand("BUFF_ADD_GLOBAL",
				(r)=> {
					BuffManager.GiveBuff(r.ReadInt32(), r.ReadInt32(), r.ReadSingle(), r.ReadSingle());
				}),

			};
		}
	}
}
