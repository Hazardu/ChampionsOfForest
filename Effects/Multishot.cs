﻿using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class Multishot : MonoBehaviour
	{
		public Transform root;
		public Transform child1;
		public Transform child2;
		public Transform child3;
		public static float opacity;

		public static GameObject localPlayerInstance;
		public static bool IsOn;
		public static Material mat;

		public static GameObject Create(Transform root, Transform hand)
		{
			GameObject go = new GameObject("__SPELL EFFECT 1__");

			GameObject child = GameObject.CreatePrimitive(PrimitiveType.Quad);
			if (mat == null)
				mat = new Material(Shader.Find("Particles/Additive"));
			mat.SetColor("_TintColor", new Color(0.0f, 0.3f, 0.2f, 0.05f));
			mat.mainTexture = Res.ResourceLoader.GetTexture(126);
			child.GetComponent<Renderer>().material = mat;

			var light = go.AddComponent<Light>();
			light.shadowStrength = 1;
			light.shadows = LightShadows.Hard;
			light.type = LightType.Point;
			light.range = 14;
			light.color = new Color(0, 1f, 0.66f);
			light.intensity = 0.6f;

			go.transform.parent = hand;
			var c = go.AddComponent<Multishot>();
			c.root = root;
			go.transform.position = hand.position;
			go.transform.localScale = Vector3.one * 0.6f;
			child.transform.parent = go.transform;
			child.transform.localPosition = Vector3.forward;
			c.child1 = child.transform;
			c.child2 = Instantiate(child, go.transform.position + Vector3.forward * 1.5f, Quaternion.identity, go.transform).transform;
			c.child3 = Instantiate(child, go.transform.position + Vector3.forward * 2, Quaternion.identity, go.transform).transform;
			c.child2.localScale *= 2;
			c.child3.localScale *= 4;

			return go;
		}

		public static void Fired()
		{
			opacity = 0;
		}

		private const float maxOpacity = 1f, regainOpacityRate = 0.3f;

		private void Update()
		{
			transform.rotation = root.transform.rotation;
			child1.Rotate(Vector3.forward * 90 * Time.deltaTime);
			child2.Rotate(-Vector3.forward * 45 * Time.deltaTime);
			child3.Rotate(Vector3.forward * 20 * Time.deltaTime);

			if (opacity < maxOpacity)
			{
				opacity += Time.deltaTime * regainOpacityRate;
				mat.SetColor("_TintColor", new Color(0.0f, 0.3f * opacity, 0.2f * opacity, 0.05f * opacity));
			}
		}
	}
}