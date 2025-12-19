using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Network;
using ChampionsOfForest.Network.Commands;
using ChampionsOfForest.Player;

using ModAPI;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public partial class ModReferences : MonoBehaviour
	{
		
		private static ModReferences instance;
		public static List<GameObject> Players => Scene.SceneTracker.allPlayers;

		public static string ThisPlayerID
		{
			get;
			private set;
		}

		public static Transform rightHandTransform = null;

		private void Start()
		{
			instance = this;
			if (BoltNetwork.isRunning)
			{
				StartCoroutine(InitPlayerID());
			}
		}

		public static void SendRandomItemDrops(int count, EnemyProgression.Enemy type, long value, ModSettings.GameDifficulty difficulty, Vector3 position)
		{
			instance.StartCoroutine(Player.RCoroutines.i.AsyncSendRandomItemDrops(count, type, value, difficulty, position));
		}

		private IEnumerator InitPlayerID()
		{
			if (GameSetup.IsSinglePlayer)
			{
				ThisPlayerID = "LocalPlayer";
			}
			if (ModSettings.IsDedicated)
			{
				yield break;
			}

			while (LocalPlayer.Entity == null)
			{
				yield return null;
			}
			ThisPlayerID = LocalPlayer.Entity.GetState<IPlayerState>().name;
		}

		public static ServerPlayerState PlayerStates = new ServerPlayerState();

		private void UpdateLevelData()
		{
			COTFCommand<GetPlayerStateParams>.Send(NetworkManager.Target.Everyone, new GetPlayerStateParams()
			{
				playerID = ThisPlayerID,
				level = ModdedPlayer.instance.level,
				health = LocalPlayer.Stats.Health,
				maxHealth = ModdedPlayer.Stats.TotalMaxHealth,
				entityNetworkID = LocalPlayer.Entity.networkId.PackedValue
			});
		}



		public static float DamageReduction(int armor)
		{
			armor = Mathf.Max(armor, 0);

			float a = armor;
			float b = (armor + 500f);

			return Mathf.Pow(Mathf.Clamp01(a / b), 2f);
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


		private static Material _bloodMaterial;
		public static Material BloodMaterial
		{
			get
			{
				if (_bloodMaterial == null)
				{
					_bloodMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
					{
						EmissionColor = new Color(0.3f, 0.1f, 0),
						renderMode = BuilderCore.BuildingData.RenderMode.Fade,
						MainColor = Color.red,
						Metalic = 0.8f,
						Smoothness = 0.8f,
					});
				}
				return _bloodMaterial;
			}
		}
	}
}