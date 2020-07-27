using UnityEngine;

namespace ChampionsOfForest.Res
{
	public class ResourceInitializer
	{
		public static void SetupMeshesFromOtherAssets()
		{
			if (!Res.ResourceLoader.instance.LoadedMeshes.ContainsKey(2001))
			{
				var meshfilter = Res.ResourceLoader.GetAssetBundle(2001).LoadAsset<GameObject>("AxePrefab.prefab").GetComponent<MeshFilter>();
				Res.ResourceLoader.instance.LoadedMeshes.Add(2001, meshfilter.sharedMesh);
			}
		}
	}
}