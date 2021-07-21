using System.Collections;
using System.Collections.Generic;

using Bolt;

using ManagedSteam.CallbackStructures;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class Portal : MonoBehaviour
	{
		private static readonly int PortalID = 0;
		public static Portal[] portals;

		public static void InitializePortals()
		{
			GameObject p1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			p1.name = "__PORTAL__";
			p1.GetComponent<SphereCollider>().isTrigger = true;
			Portal pcomponent = p1.AddComponent<Portal>();
			if (!ModSettings.IsDedicated)
			{
				Camera cam1 = new GameObject("__CAM__").AddComponent<Camera>();
				cam1.transform.parent = p1.transform;
				cam1.fieldOfView = 45;
				Light light = p1.AddComponent<Light>();
				light.type = LightType.Point;
				light.range = 30;
				Material mat = new Material(Shader.Find("Unlit/Texture"));
				mat.color = Color.blue;
				p1.GetComponent<MeshFilter>().mesh = Res.ResourceLoader.instance.LoadedMeshes[112];

				MeshRenderer MR = p1.GetComponent<MeshRenderer>();
				MR.material = mat;

				pcomponent.cam = cam1;
				pcomponent.rend = MR;
			}
			GameObject p2 = Instantiate(p1);
			Portal portal2 = p2.GetComponent<Portal>();
			pcomponent.otherPortal = portal2;
			portal2.otherPortal = pcomponent;

			portals = new Portal[]
			{
			   pcomponent,portal2
			};

			p1.SetActive(false);
			p2.SetActive(false);
		}
		public static bool BothPortalsActive => portals[0].gameObject.activeSelf && portals[1].gameObject.activeSelf;
		public static void CreatePortal(Vector3 pos, float Duration, int portalID, bool leadsToCaves, bool leadsToEndgame)
		{
			portals[portalID].gameObject.SetActive(true);
			portals[portalID].transform.position = pos;
			if (portals[(portalID + 1) % 2].gameObject.activeSelf)
			{
				portals[portalID].Duration = Mathf.Max(Duration,portals[(portalID + 1) % 2].Duration);
				portals[(portalID + 1) % 2].Duration = portals[portalID].Duration;
			}
			else
			{
				portals[portalID].Duration = Duration;
			}
			portals[portalID].Cave = leadsToCaves;
			portals[portalID].Endgame = leadsToEndgame;
			portals[portalID].Enable();
			if (portalID == 1)
			{
				portalID = 0;
			}
			else
			{
				portalID = 1;
			}
		}
		public static void SyncBothPortals()
		{
			for (int i = 0; i < 2; i++)
			{
				using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(stream))
					{
						var pos = portals[i].gameObject.transform.position;
						w.Write(3);
						w.Write(6);
						w.Write(pos.x);
						w.Write(pos.y);
						w.Write(pos.z);
						w.Write(portals[i].Duration);
						w.Write(i);
						w.Write(portals[i].Cave);
						w.Write(portals[i].Endgame);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(stream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
				}
			}
		}
		public static void SyncTransform(Vector3 pos, float Duration, int portalID, bool inCave, bool inEndgame)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(3);
					w.Write(6);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(Duration);
					w.Write(portalID);
					w.Write(inCave);
					w.Write(inEndgame);
					w.Close();
				}
				ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
				answerStream.Close();
			}
		}

		public static int GetPortalID()
		{
			if (!portals[0].gameObject.activeSelf)
			{
				return 0;
			}

			if (!portals[1].gameObject.activeSelf)
			{
				return 1;
			}

			return PortalID;
		}

		public Portal otherPortal;

		public Camera cam;

		public MeshRenderer rend;

		public RenderTexture renderTexture;

		public List<Transform> Excludedtransforms;

		public float Duration;
		public bool DoCooldown;
		public bool Cave;
		public bool Endgame;

		// Start is called before the first frame update
		private void Start()
		{
			renderTexture = new RenderTexture(1024, 1024, 0);
			cam.forceIntoRenderTexture = true;
			cam.targetTexture = renderTexture;
			Excludedtransforms = new List<Transform>();
			TimeOfPass = 0;
		}

		private void Enable()
		{
			transform.localScale = Vector3.zero;
			DoCooldown = false;
			StopAllCoroutines();
			StartCoroutine(ScaleIn());
			StartCoroutine(DurationCoroutine());
			Excludedtransforms.Clear();
			TimeOfPass = 0;
		}

		// Update is called once per frame
		private void Update()
		{
			if (ModSettings.IsDedicated)
			{
				return;
			}

			if (!otherPortal.gameObject.activeSelf)
			{
				rend.material.mainTexture = Texture2D.blackTexture;
				return;
			}

			Vector3 lookAtDir = transform.position - Camera.main.transform.position;
			lookAtDir.Normalize();

			otherPortal.cam.transform.position = otherPortal.transform.position + lookAtDir;
			otherPortal.cam.transform.rotation = Quaternion.LookRotation(lookAtDir, Vector3.up);

			rend.material.mainTexture = otherPortal.cam.activeTexture;
		}

		private IEnumerator ScaleIn()
		{
			Effects.Sound_Effects.GlobalSFX.Play(1016, 0, 2);
			yield return null;
			while (transform.localScale.x < 2.2)
			{
				transform.Rotate(Vector3.up * Time.deltaTime *300);
				transform.localScale += Vector3.one * Time.deltaTime*0.5f;
				yield return null;
			}
		}

		private IEnumerator DurationCoroutine()
		{
			while (!DoCooldown)
			{
				if (otherPortal.gameObject.activeSelf)
				{
					DoCooldown = true;
				}
				
				yield return null;
			}
			while (Duration > 0)
			{
				Duration--;
				if (Duration <= 1)
					transform.localScale *= 0.5f;
				yield return new WaitForSeconds(1);
			}
			float t = 1;
			while (t > 0)
			{
				transform.Rotate(Vector3.up * Time.deltaTime / t);
				t -= Time.deltaTime*2;
				transform.localScale =Vector3.one* (2.2f * t);
			}

			gameObject.SetActive(false);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!otherPortal.gameObject.activeSelf)
			{
				return;
			}

			if ( other.transform.root == LocalPlayer.Transform)
			{
				if (TimeOfPass + 0.2f < Time.time)
				{
					if ((other.transform.root.position - transform.position).sqrMagnitude < 5)
					{
						TimeOfPass = Time.time;

						LocalPlayer.Transform.position = otherPortal.transform.position;
						Vector3 dir = LocalPlayer.Transform.position - transform.position;
						dir.Normalize();
						LocalPlayer.Rigidbody.AddForce(dir * 15, ForceMode.VelocityChange);

						if ((otherPortal.Endgame && !LocalPlayer.IsInEndgame) || (!otherPortal.Endgame && LocalPlayer.IsInEndgame))
						{
							LocalPlayer.GameObject.GetComponent<LocalPlayer>().SetInEndGame(otherPortal.Endgame);
							GameObject endgameLoader = GameObject.FindWithTag("EndgameLoader");
							if (endgameLoader)
							{
								TheForest.World.SceneLoadTrigger component = endgameLoader.GetComponent<TheForest.World.SceneLoadTrigger>();
								if (TheForest.Utils.LocalPlayer.ActiveAreaInfo.HasActiveEndgameArea)
								{
									component.SetCanLoad(true);
									component.ForceLoad();
								}
							}
						}
						if ((otherPortal.Cave && !LocalPlayer.IsInCaves))
						{
							LocalPlayer.Stats.InACave();
						}
						else if ((!otherPortal.Cave && LocalPlayer.IsInCaves))
						{
							LocalPlayer.Stats.NotInACave();
						}
					}
				}
			}
			else if (other.attachedRigidbody != null)
			{
				ModAPI.Console.Write("Collided with rigidbody");
				if (Excludedtransforms.Contains(other.attachedRigidbody.transform))
				{
					return;
				}
				otherPortal.Excludedtransforms.Add(other.attachedRigidbody.transform);
				
				other.attachedRigidbody.position = otherPortal.transform.position;
				Vector3 dir = other.attachedRigidbody.position - transform.position;
				dir.Normalize();
				other.attachedRigidbody.AddForce(dir * 15, ForceMode.VelocityChange);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!otherPortal.gameObject.activeSelf)
			{
				return;
			}

			if (!ModSettings.IsDedicated && other.transform.root == LocalPlayer.Transform)
			{
				return;
			}
			else if (other.attachedRigidbody != null)
			{
				Excludedtransforms.Remove(other.attachedRigidbody.transform);
			}
		}

		private static float TimeOfPass = 0;
	}
}