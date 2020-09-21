using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class SpellAimLine
	{
		private LineRenderer lineRenderer;
		private GameObject gameObject;
		private static Material material;
		public bool IsValid => gameObject != null && lineRenderer != null;
		public SpellAimLine()
		{
			gameObject = new GameObject("Spell Aim Gizmo-Line");
			lineRenderer = gameObject.AddComponent<LineRenderer>();
			gameObject.SetActive(false);
			AssignLineRendererProperties();
		}

		private void AssignLineRendererProperties()
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

	public class SpellAimSphere
	{
		private GameObject gameObject;
		private Transform transform;
		private static Shader shader;
		public bool IsValid => gameObject != null;

		public SpellAimSphere(Color color, float radius)
		{
			if (shader == null)
			{
				shader = Res.ResourceLoader.GetAssetBundle(2005).LoadAsset<Shader>("Assets/Intersection.shader");
				//The shader is not rendered if the camera is inside the sphere
				/*Shader Code -------------------------
					Shader "Unlit/IntersectionShader"
				{
					Properties
					{
						_Color("Color", Color) = (1,1,1,.2)
					}
					SubShader
					{
							Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
							Pass
							{
								Stencil
								{
									Ref 172
									Comp Always
									Pass Replace
									ZFail Zero
								}
								Blend Zero One
								Cull Front
								ZTest  GEqual
								ZWrite Off
						}
						Pass
						{
							Blend SrcAlpha OneMinusSrcAlpha
							Stencil
							{
								Ref 172
								Comp Equal
							}
						Cull Off
							CGPROGRAM
							#pragma vertex vert
							#pragma fragment frag

							#include "UnityCG.cginc"
							struct appdata
							{
								float4 vertex : POSITION;
							};

							struct v2f
							{
								float4 vertex : POSITION;
							};

							sampler2D _MainTex;
							float4 _MainTex_ST;
							float4 _Color;

							v2f vert (appdata v)
							{
								v2f o;
								o.vertex = UnityObjectToClipPos(v.vertex);
								return o;
							}

							fixed4 frag (v2f i) : SV_Target
							{
								return _Color;
							}
							ENDCG
						}
					}
				}

				*/
			}

			gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Component.Destroy(gameObject.GetComponent<SphereCollider>());
			transform = gameObject.transform;
			gameObject.SetActive(false);
			transform.localScale = Vector3.one * radius * 2;
			AssignMaterialProperties(color);
		}

		private void AssignMaterialProperties(Color color)
		{
			var mat = new Material(shader);

			mat.SetColor("_Color", color);
			gameObject.GetComponent<Renderer>().material = mat;
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

		public void UpdatePosition(Vector3 position)
		{
			Enable();
			transform.position = position;
		}

		public void SetRadius(float radius)
		{
			transform.localScale = Vector3.one * radius * 2;
		}
	}
}