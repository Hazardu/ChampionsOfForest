using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Network;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{

	public class ModReferences : MonoBehaviour
	{
		
		private float LevelRequestCooldown = 10;
		private float MFindRequestCooldown = 300;
		public static Material bloodInfusedMaterial;
		private static ModReferences instance;

		public static Transform rightHandTransform = null;
		private void Start()
		{
			instance = this;
			if (BoltNetwork.isRunning)
			{
				Players = new List<GameObject>();
				StartCoroutine(UpdateSetups());
				if (GameSetup.IsMpServer && BoltNetwork.isRunning)
				{
					InvokeRepeating("UpdateLevelData", 1, 1);
				}
			}
			else
			{
				Players = new List<GameObject>() { LocalPlayer.GameObject };
			}
			BoltConnection c;
			
		}

		public static void SendRandomItemDrops(int count, EnemyProgression.Enemy type,long bounty, ModSettings.Difficulty difficulty, Vector3 position)
		{
			RCoroutines.i.StartCoroutine(RCoroutines.i.AsyncSendRandomItemDrops(count, type, bounty,difficulty, position));
		}

		float lastLevelCheckTimestamp;
		private int lastPlayerLevelCount;
		public static void UpdatePlayerInfo(ulong playerID, int level, long exp, float hp, float maxhp, float energy, float maxenergy)
		{
			var player = NetworkManager.GetModdedClient(playerID);
			for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
			{
				var packed = Scene.SceneTracker.allPlayerEntities[i].networkId.PackedValue;
				if (packed == playerID)
				{
					Scene.SceneTracker.allPlayerEntities[i].GetComponentInChildren<TheForest.UI.Multiplayer.PlayerName>().SendMessage("SetNameWithLevel",level);
					break;
				}
				}
			}
			
		}
		public static void RemoveUnusedPlayerLevels()
		{
			List<string> stringsToRemove = new List<string>();
			foreach (var item in PlayerLevels)
			{
				if (!PlayerStates.Any(x => x.name == item.Key))
				{
					stringsToRemove.Add(item.Key);
				}
			}
			foreach (var item in stringsToRemove)
			{
				Debug.Log("Removing player: " + item);
				PlayerLevels.Remove(item);
			}
		}

		public static void RequestAllPlayerLevels()
		{
			if (Time.time - instance.lastLevelCheckTimestamp > 120|| instance.lastPlayerLevelCount != Players.Count)
			{
				RemoveUnusedPlayerLevels();
				instance.LevelRequestCooldown = 120;
				instance.lastLevelCheckTimestamp = Time.time;
				instance.lastPlayerLevelCount = Players.Count;
				Host_RequestLevels();

			}
		}
		private void UpdateLevelData()
		{
			if (Players.Count > 1)
			{
				LevelRequestCooldown -= 1;
				if (LevelRequestCooldown < 0 || lastPlayerLevelCount != Players.Count)
				{
					LevelRequestCooldown = 120;
					lastLevelCheckTimestamp = Time.time;
					lastPlayerLevelCount = Players.Count;
					RemoveUnusedPlayerLevels();
					Host_RequestLevels();
				}
			
				MFindRequestCooldown--;
				if (MFindRequestCooldown <= 0)
				{
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(23);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
						answerStream.Close();
					}
					MFindRequestCooldown = 300;
				}

				if (PlayerHands.ContainsValue(null))
				{
					PlayerHands.Clear();
				}
			}
			else
			{
				//PlayerLevels.Clear();
			}
		}

		public static void Host_RequestLevels()
		{
			if (GameSetup.IsMpServer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(18);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
					answerStream.Close();
				}
			}
		}


		//invalid il code error happens here. i have no clue why, so im randomly changing it so maybe it fixes itself
		//im trying splitting to more classes
		public static void FindHands()
		{
			PlayerHands.Clear();
			for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
			{
				if (Scene.SceneTracker.allPlayers[i].transform.root != LocalPlayer.Transform)
				{
					//BoltEntity entity = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
					//if (entity == null) continue;
					//IPlayerState state = entity.GetState<IPlayerState>();
					//if (state == null) continue;

					string playerName = getName(Scene.SceneTracker.allPlayers[i]);
					if (playerName != "")
					{
						Transform hand = FindDeepChild(Scene.SceneTracker.allPlayers[i].transform.root, "rightHandHeld");
						if (hand != null)
						{
							PlayerHands.Add(playerName, hand.parent);
						}
					}
				}
			}
		}

		private static string getName(GameObject gameObject)
		{
			BoltEntity entity = gameObject.GetComponent<BoltEntity>();
			if (entity == null)
				return "";
			else
			{
				IPlayerState state = entity.GetState<IPlayerState>();
				if (state == null)
					return "";
				else
					return state.name;
			}
		}

		private IEnumerator UpdateSetups()
		{
			while (true)
			{
				yield return null;
				if (Players.Any(x => x == null))
				{
					Players.Clear();
					AllPlayerEntities.Clear();
					PlayerStates.Clear();
					Players.AddRange(Scene.SceneTracker.allPlayers);
					for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
					{
						BoltEntity b = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
						if (b != null)
						{
							AllPlayerEntities.Add(b);
							PlayerStates.Add(b.GetState<IPlayerState>());
						}
					}
				}

				yield return new WaitForSeconds(10);
			}
		}

		public static Transform FindDeepChild(Transform aParent, string aName)
		{
			Transform result = aParent.Find(aName);
			if (result != null)
			{
				return result;
			}
			else
			{
				foreach (Transform child in aParent)
				{
					Transform result2 = FindDeepChild(child, aName);
					if (result2 != null)
					{
						return result2;
					}
				}
				return null;
			}
		}
		public static string RecursiveTransformList(Transform tr, string s = "", int indents = 0)
		{
			s += new System.String('\t', indents) + tr.name + "\n";
			foreach (Transform item in tr)
			{
				s = RecursiveTransformList(item, s, indents + 1);
			}
			return s;
		}

		public static void RecursiveComponentList(GameObject go)
		{
			RecursiveComponentList(go.transform, "");
		}

		private static void RecursiveComponentList(Transform tr, string start)
		{
			ModAPI.Log.Write(start + tr.name + '-' + tr.GetComponents<Component>().Select(x => x.GetType().Name).Join(", "));
			start += '\t';
			foreach (Transform item in tr)
			{
				RecursiveComponentList(item, start);
			}
		}
	}
}