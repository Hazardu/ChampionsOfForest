using BuilderCore;
using ChampionsOfForest.Res;
using UnityEngine;

namespace ChampionsOfForest
{
    //adds prefabs using Builder core
    public class Buildings
    {
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
                        renderMode = BuildingData.RenderMode.Transparent,
                        EmissionColor = new Color(0.2f,0.2f,0.2f),
                        Smoothness =0,
                        EmissionMap = ResourceLoader.instance.LoadedTextures[20],
                        AddCollider = false,
                    }
                }
            };
            Core.AddBuilding(blackHole, 401);
            Core.prefabs[401].SetActive(true);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * 2;
            sphere.GetComponent<Renderer>().material = Core.CreateMaterial(new BuildingData() { MainColor = Color.black, Smoothness = 0, Metalic = 1 });
            sphere.transform.SetParent(Core.prefabs[401].transform);
            sphere.transform.localPosition = Vector3.zero;

            Light l = sphere.AddComponent<Light>();
            l.intensity = 2.3f;
            l.range = 60;
            l.color = Color.blue;
            l.cookie = Res.ResourceLoader.instance.LoadedTextures[23];
            l.cookieSize = 4;
            l.type = LightType.Point;
            Core.prefabs[401].SetActive(false);
            ModAPI.Log.Write("Successfully added black hole [building with id 401]");

        }
    }
}
