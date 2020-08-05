using System.Collections;
using System.Collections.Generic;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class BlackFlame : MonoBehaviour
	{
		public static BlackFlame instance;
		public static float DmgAmp = 1;
		public static float FireDamageBonus => (30 + ModdedPlayer.Stats.spellFlatDmg /ModdedPlayer.Stats.spell_blackFlameDamageScaling) * ModdedPlayer.Stats.TotalMagicDamageMultiplier * DmgAmp / 3;

		private static Material mat1;
		private static Material mat2;
		public static GameObject instanceLocalPlayer;

		public static bool GiveDamageBuff;
		public static bool GiveAfterburn;

		public static GameObject Create()
		{
			AnimationCurve sizecurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 5.129462f, 5.129462f), new Keyframe(0.2449522f, 1, 0, 0), new Keyframe(1, 0, -1.242162f, -1.242162f), });
			if (mat1 == null)
			{
				mat1 = new Material(Shader.Find("Particles/Multiply"));
				mat2 = new Material(Shader.Find("Particles/Additive"));

				mat1.color = Color.white;
				mat2.color = Color.white;

				mat1.mainTexture = Res.ResourceLoader.GetTexture(111);
				mat2.mainTexture = Res.ResourceLoader.GetTexture(111);

				mat1.renderQueue = 3050;
				mat2.renderQueue = 3000;
			}

			GameObject go = new GameObject("__BlackFlames__");

			ParticleSystem ps = go.AddComponent<ParticleSystem>();
			ParticleSystem.MainModule main = ps.main;
			ParticleSystem.EmissionModule emission = ps.emission;
			ParticleSystem.ShapeModule shape = ps.shape;
			ParticleSystem.RotationOverLifetimeModule rot = ps.rotationOverLifetime;
			ParticleSystem.SizeOverLifetimeModule size = ps.sizeOverLifetime;
			ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();

			main.startSize = 0.4f;
			main.startSpeed = -6f;
			main.startLifetime = 0.75f;
			main.startColor = new Color(0, 0, 1, 0.623f);
			main.startRotation = new ParticleSystem.MinMaxCurve(0, 90);

			emission.rateOverTime = 100;

			shape.shapeType = ParticleSystemShapeType.Cone;
			shape.angle = 0;
			shape.radius = 0.25f;

			rot.enabled = true;
			rot.z = 180;

			size.enabled = true;
			size.size = new ParticleSystem.MinMaxCurve(1, sizecurve);

			rend.renderMode = ParticleSystemRenderMode.Stretch;
			rend.lengthScale = 2;
			rend.normalDirection = 1;
			rend.material = mat1;

			GameObject go2 = GameObject.Instantiate(go, go.transform);
			ParticleSystem ps2 = go.GetComponent<ParticleSystem>();
			ParticleSystemRenderer rend2 = go.GetComponent<ParticleSystemRenderer>();
			ParticleSystem.MainModule main2 = ps2.main;
			main2.startColor = new Color(0, 0.15f, 1, 0.623f);
			main2.startLifetime = 1;
			main2.startSpeed = -4;

			rend2.material = mat2;

			var light = go2.AddComponent<Light>();
			light.shadowStrength = 1;
			light.shadows = LightShadows.Hard;
			light.type = LightType.Point;
			light.range = 20;
			light.color = new Color(0.6f, 0.3f, 1f);
			light.intensity = 0.6f;

			return go;
		}

		public static bool IsOn = false;
		public static float Cost = 20;

		public void Start()
		{
			StartCoroutine(StartCoroutine());
			if (instance == null)
				instance = this;
		}

		public IEnumerator StartCoroutine()
		{
			yield return null;
			yield return null;
			while (ModReferences.rightHandTransform == null)
			{
				yield return null;
				LocalPlayer.Inventory?.SendMessage("GetRightHand");
			}
			yield return null;
			if (instanceLocalPlayer == null)
			{
				instanceLocalPlayer = Create();
				instanceLocalPlayer.transform.position = ModReferences.rightHandTransform.position;
				instanceLocalPlayer.transform.rotation = ModReferences.rightHandTransform.rotation;
				instanceLocalPlayer.transform.parent = ModReferences.rightHandTransform;
				instanceLocalPlayer.SetActive(false);
			}
		}

		private void Update()
		{
			if (IsOn)
			{
				if (ModdedPlayer.Stats.perk_danceOfFiregod)
					SpellCaster.RemoveStamina(Cost * 10 * Time.deltaTime);
				else
					SpellCaster.RemoveStamina(Cost * Time.deltaTime);
				if (LocalPlayer.Stats.Stamina < 5)
				{
					Toggle();
				}
				if (GiveDamageBuff)
				{
					BuffDB.AddBuff(13, 44, 1.6f, 1f);
				}
			}
		}

		public static void Toggle()
		{
			IsOn = !IsOn;
			instanceLocalPlayer.SetActive(IsOn);
			if (BoltNetwork.isRunning)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(4);
						w.Write(IsOn);
						w.Write(ModReferences.ThisPlayerID);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
					answerStream.Close();
				}
			}
		}

		private static Dictionary<Transform, GameObject> blackFlamesClients = new Dictionary<Transform, GameObject>();

		public static void ToggleOtherPlayer(string playerName, bool ison)
		{
			//ModAPI.Console.Write("Toggling black flames for client " + playerName + ison);
			if (!ModReferences.PlayerHands.ContainsKey(playerName))
			{
				ModReferences.FindHands();
			}
			if (ModReferences.PlayerHands.ContainsKey(playerName))
			{
				Transform t = ModReferences.PlayerHands[playerName];
				if (blackFlamesClients.ContainsKey(t))
				{
					blackFlamesClients[t].SetActive(ison);
				}
				else
				{
					GameObject go = Create();
					go.transform.parent = t;

					go.transform.position = t.position;
					go.transform.rotation = t.rotation;
					blackFlamesClients.Add(t, go);

					go.SetActive(ison);
				}
			}
		}
	}
}