using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class SpellAimLine
	{
		LineRenderer lineRenderer;
		GameObject gameObject;
		static Material material;
		public SpellAimLine()
		{
			gameObject = new GameObject("Spell Aim Gizmo-Line");
			lineRenderer=gameObject.AddComponent<LineRenderer>();
				gameObject.SetActive(false);
			AssignLineRendererProperties();	
		}
		void AssignLineRendererProperties()
		{
			lineRenderer.positionCount = 2;
			if (material == null)
			{
				material = new Material(Shader.Find("Particles/Additive"));
				material.SetColor("_TintColor", new Color(0f, 1, 0.95f, 1f));
			}

			lineRenderer.sharedMaterial = material;
			lineRenderer.widthMultiplier = 0.15f;
		}

		public void Enable()
		{
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
		}
		public void Disable()
		{
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}


		public void UpdatePosition(Vector3 start, Vector3 end)
		{
			Enable();
			lineRenderer.SetPosition(0, start);
			lineRenderer.SetPosition(1, end);
		}


	}
}
