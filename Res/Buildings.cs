using BuilderCore;

using UnityEngine;

namespace ChampionsOfForest.Res
{
	//creates prefabs using Builder core
	public class Buildings
	{
		private void Test()
		{
			GameObject gameObject = null;

			Material mat = Core.CreateMaterial(
				new BuildingData()
				{
					Smoothness = 1,
					MainColor = Color.red,
					Metalic = 1,
					EmissionColor = new Color(0.3f, 0f, 0f)
				});

			gameObject.GetComponent<Renderer>().material = mat;
		}

		public static void InitBuildings()
		{
			Building blackHole = new Building()
			{
				save = false,
				data = new BuildingData[] {
					new BuildingData()
					{
						mesh = ResourceLoader.instance.LoadedMeshes[21],
						MainTexture = ResourceLoader.instance.LoadedTextures[20],
						renderMode = BuildingData.RenderMode.Fade,
						EmissionMap = ResourceLoader.instance.LoadedTextures[20],
						AddCollider = false,
					}
				}
			};
			Core.AddBuilding(blackHole, 401);
			Core.prefabs[401].SetActive(true);

			Renderer r = Core.prefabs[401].GetComponent<Renderer>();
			r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			r.receiveShadows = false;

			Light l = Core.prefabs[401].AddComponent<Light>();
			l.intensity = 1.5f;
			l.range = 60;
			l.color = new Color(1, 1, 1f);

			l.shadows = LightShadows.None;
			l.type = LightType.Point;
			Core.prefabs[401].SetActive(false);
		}
	}
}