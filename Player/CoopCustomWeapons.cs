using System.Collections;
using System.Collections.Generic;

using BuilderCore;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class CoopCustomWeapons : MonoBehaviour
	{
		//Light l;
		//Light l2;
		//Vector3 xx;
		//Vector3 rr;
		//public void OnGUI()
		//{
		//    if (l == null)
		//    {
		//        l =
		//        new GameObject().AddComponent<Light>();
		//        l.range = 60;
		//        l.type = LightType.Point;
		//        l2 = Instantiate(l);
		//    }
		//     GUILayout.Label("---------------------------------------------------------------------------------------------------");
		//     xx = new Vector3(
		//         GUILayout.HorizontalSlider(xx.x, -60, 60),
		//         GUILayout.HorizontalSlider(xx.y, -60, 60),
		//         GUILayout.HorizontalSlider(xx.z, -60, 60));
		//    rr = new Vector3(
		//         GUILayout.HorizontalSlider(rr.x, -180, 180),
		//         GUILayout.HorizontalSlider(rr.y, -180, 180),
		//         GUILayout.HorizontalSlider(rr.z, -180, 180));
		//    var vals = new Dictionary<int, CustomWeaponPrefabData>(prefabDatas);
		//        GUILayout.BeginArea(new Rect(1500, 400, 420, Screen.height),GUI.skin.box);
		//    foreach (var pair in vals)
		//    {
		//     GUILayout.Label("---------------------------------------------------------------------------------------------------");
		//            GUILayout.Label(((BaseItem.WeaponModelType)pair.Key).ToString());
		//           var val = pair.Value;
		//            Vector3 of = prefabDatas[pair.Key].offset;
		//            GUILayout.Label("offset: x:" + of.x + "  y:" + of.y + "  z:" + of.z);
		//            of = new Vector3(
		//                GUILayout.HorizontalSlider(of.x, -1.5f, 1.5f),
		//                GUILayout.HorizontalSlider(of.y, -1.5f, 1.5f),
		//                GUILayout.HorizontalSlider(of.z, -1.5f, 1.5f));
		//            val.offset = of;
		//            Vector3 rot = prefabDatas[pair.Key].rotation;
		//            GUILayout.Label("rotation: x:" + rot.x + "  y:" + rot.y + "  z:" + rot.z);
		//            rot = new Vector3(
		//                GUILayout.HorizontalSlider(rot.x, -180, 180),
		//                GUILayout.HorizontalSlider(rot.y, -180, 180),
		//                GUILayout.HorizontalSlider(rot.z, -180, 180));
		//            val.rotation = rot;
		//            prefabDatas[pair.Key] = val;
		//    }
		//    GUILayout.EndArea();
		//    foreach (var pair in prefabDatas)
		//    {
		//    CorrectTransform(pair.Value.obj, pair.Key);
		//    }
		//    l.transform.position = prefabDatas[2].obj.transform.position + Vector3.up * 4;
		//    l2.transform.position = prefabDatas[2].obj.transform.position + Vector3.down * 4  +Vector3.forward * 5;
		//}

		public static Dictionary<Transform, Dictionary<int, GameObject>> customWeapons = new Dictionary<Transform, Dictionary<int, GameObject>>();
		public static CoopCustomWeapons instance;
		public static Dictionary<int, GameObject> prefabs = new Dictionary<int, GameObject>();
		public static Dictionary<int, CustomWeaponPrefabData> prefabDatas = new Dictionary<int, CustomWeaponPrefabData>();

		public struct CustomWeaponPrefabData
		{
			public BaseItem.WeaponModelType model;
			public Vector3 offset;
			public Vector3 rotation;
			public Vector3 tip;
			public float Scale;
			public GameObject obj;

			public CustomWeaponPrefabData(BaseItem.WeaponModelType model, Vector3 offset, Vector3 rotation, Vector3 tip, float scale, GameObject g)
			{
				this.model = model;
				this.offset = offset;
				this.rotation = rotation;
				this.tip = tip;
				Scale = scale;
				this.obj = g;
			}
		}

		public static void Init()
		{
			customWeapons = new Dictionary<Transform, Dictionary<int, GameObject>>();
			prefabs = new Dictionary<int, GameObject>();
			prefabDatas = new Dictionary<int, CustomWeaponPrefabData>();
			//longsword
			AddWeapon(BaseItem.WeaponModelType.LongSword,
				51,
				BuilderCore.Core.CreateMaterial(
					new BuilderCore.BuildingData()
					{
						MainTexture = Res.ResourceLoader.instance.LoadedTextures[60],
						Metalic = 0.86f,
						Smoothness = 0.66f
					}
					),
				new Vector3(-0.2289183f, -0.034201f, -1.15787f),
				new Vector3(90, -90, 0),
				new Vector3(-0.2f, -2.3f, 0),
				0.9f);

			//great sword
			AddWeapon(BaseItem.WeaponModelType.GreatSword,
				52,
				BuilderCore.Core.CreateMaterial(
					new BuilderCore.BuildingData()
					{
						OcclusionStrenght = 0.75f,
						Smoothness = 0.6f,
						Metalic = 0.6f,
						MainTexture = Res.ResourceLoader.instance.LoadedTextures[61],
						EmissionMap = Res.ResourceLoader.instance.LoadedTextures[62],
						BumpMap = Res.ResourceLoader.instance.LoadedTextures[64],
						HeightMap = Res.ResourceLoader.instance.LoadedTextures[65],
						Occlusion = Res.ResourceLoader.instance.LoadedTextures[66]
					}
					),
				new Vector3(-0.05901787f, -0.075443f, -1.6f),
				new Vector3(0, 180, 0),
				new Vector3(0, 0, 0),
				 1f);

			//hammer
			AddWeapon(BaseItem.WeaponModelType.Hammer,
					 108,
					 BuilderCore.Core.CreateMaterial(
						 new BuilderCore.BuildingData()
						 {
							 Metalic = 0.86f,
							 Smoothness = 0.66f,
							 MainColor = new Color(0.2f, 0.2f, 0.2f),
						 }
						 ),
					 new Vector3(-0.055f, -0.07f, 1.65f),
					 new Vector3(180, 0, 0),
					 new Vector3(0, 0, -2f),
					 1f);

			AddWeapon(BaseItem.WeaponModelType.Axe, Instantiate(Res.ResourceLoader.GetAssetBundle(2001).LoadAsset<GameObject>("AxePrefab.prefab")),
				new Vector3(-0.05901787f, -0.075443f, -1.6f),
				new Vector3(0, 180, 0),
				new Vector3(0, 0, 0),
				1);
			AddWeapon(BaseItem.WeaponModelType.Greatbow, 167,
			Core.CreateMaterial(new BuildingData()
			{
				BumpMap = Res.ResourceLoader.GetTexture(168),
				BumpScale = 1.5f,
				Metalic = 0.2f,
				Smoothness = 0.0f,
				MainTexture = Res.ResourceLoader.GetTexture(169),
				EmissionMap = Res.ResourceLoader.GetTexture(169),
			}), new Vector3(0, 0, 1.1f), new Vector3(0, 0, 0), Vector3.zero, 1.1f);

			instance = new GameObject().AddComponent<CoopCustomWeapons>();
		}

		public static void AddWeapon(BaseItem.WeaponModelType model, int mesh, Material material, Vector3 offset, Vector3 rotation, Vector3 tip, float Scale)
		{
			try
			{
				GameObject go = new GameObject();
				prefabs.Add((int)model, go);
				Renderer renderer = go.AddComponent<MeshRenderer>();
				renderer.material = material;
				go.AddComponent<MeshFilter>().mesh = Res.ResourceLoader.instance.LoadedMeshes[mesh];
				go.transform.localRotation = new Quaternion(0.4f, -0.6f, -0.6f, 0.4f);
				go.transform.localPosition = new Vector3(0.0f, 0.0f, -0.8f);//+ new Vector3(-0.26f, 1.4f, -1.255f);
				go.transform.Rotate(rotation, Space.Self);
				go.transform.localPosition += offset;
				go.transform.localScale = Vector3.one * Scale;
				if (CustomWeapon.trailMaterial == null)
				{
					CustomWeapon.trailMaterial = new Material(Shader.Find("Unlit/Transparent"))
					{
						color = new Color(1, 0.5f, 0.25f, 0.4f),
						mainTexture = Texture2D.whiteTexture
					};
				}

				prefabDatas.Add((int)model, new CustomWeaponPrefabData(model, offset, rotation, tip, Scale, go));
				go.SetActive(false);
			}
			catch (System.Exception e)
			{
				ModAPI.Console.Write("adding weapon error\n" + e.ToString());
			}
		}

		public static void AddWeapon(BaseItem.WeaponModelType model, GameObject go, Vector3 offset, Vector3 rotation, Vector3 tip, float Scale)
		{
			try
			{
				prefabs.Add((int)model, go);
				go.transform.localRotation = new Quaternion(0.4f, -0.6f, -0.6f, 0.4f);
				go.transform.localPosition = new Vector3(0.0f, 0.0f, -0.8f);
				go.transform.Rotate(rotation, Space.Self);
				go.transform.localPosition += offset;
				go.transform.localScale = Vector3.one * Scale;
				if (CustomWeapon.trailMaterial == null)
				{
					CustomWeapon.trailMaterial = new Material(Shader.Find("Unlit/Transparent"))
					{
						color = new Color(1, 0.5f, 0.25f, 0.4f),
						mainTexture = Texture2D.whiteTexture
					};
				}

				prefabDatas.Add((int)model, new CustomWeaponPrefabData(model, offset, rotation, tip, Scale, go));
				go.SetActive(false);
			}
			catch (System.Exception e)
			{
				ModAPI.Console.Write("adding weapon error\n" + e.ToString());
			}
		}

		public static Dictionary<int, GameObject> CloneWeapons(Transform handTransform)
		{
			Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();
			foreach (KeyValuePair<int, GameObject> pair in prefabs)
			{
				int i = pair.Key;
				pair.Value.SetActive(true);
				GameObject clone = GameObject.Instantiate(pair.Value, handTransform);
				dict.Add(pair.Key, clone);
				clone.SetActive(false);
				pair.Value.SetActive(false);
			}
			return dict;
		}

		public static void SetWeaponOn(Transform tr, int i)
		{
			instance.StartCoroutine(instance.EquipDelayed(tr, i));
		}

		private List<GameObject> objectsToDisable = new List<GameObject>();

		private IEnumerator EquipDelayed(Transform handTransform, int i)
		{
			objectsToDisable.Clear();

			yield return null;
			if (i != (int)BaseItem.WeaponModelType.None)
			{
				foreach (Transform child in handTransform)
				{
					objectsToDisable.Add(child.gameObject);
					child.gameObject.SetActive(false);
				}
			}
			else
			{
				foreach (KeyValuePair<int, GameObject> item in customWeapons[handTransform])
				{
					item.Value.SetActive(false);
				}
				yield break;
			}
			yield return null;

			if (!customWeapons.ContainsKey(handTransform))
			{
				Dictionary<int, GameObject> dic = CloneWeapons(handTransform);
				customWeapons.Add(handTransform, dic);
			}
			yield return null;
			Transform toIgnore = null;
			if (customWeapons.ContainsKey(handTransform))
			{
				foreach (KeyValuePair<int, GameObject> item in customWeapons[handTransform])
				{
					if (item.Key != i)
					{
						item.Value.SetActive(false);
					}
					else
					{
						toIgnore = item.Value.transform;
						item.Value.SetActive(true);
						CorrectTransform(customWeapons[handTransform][item.Key], i);
					}
				}
			}

			yield return null;
			yield return new WaitForSeconds(2.5f);
			for (int j = 0; j < objectsToDisable.Count; j++)
			{
				if (objectsToDisable[j].transform != toIgnore)
				{
					objectsToDisable[j].SetActive(false);
				}
			}
			objectsToDisable.Clear();
		}

		public void CorrectTransform(GameObject go, int i)
		{
			go.transform.localRotation = new Quaternion(0.4f, -0.6f, -0.6f, 0.4f);
			go.transform.localPosition = new Vector3(0.0f, 0.0f, -0.8f);
			go.transform.Rotate(prefabDatas[i].rotation, Space.Self);
			go.transform.localPosition += prefabDatas[i].offset;
			go.transform.localScale = Vector3.one * prefabDatas[i].Scale;
		}
	}
}