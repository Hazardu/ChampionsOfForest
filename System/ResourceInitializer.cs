using UnityEngine;

namespace ChampionsOfForest.System
{
	public class ResourceInitializer
	{
		public static void SetupMeshesFromOtherAssets()
		{
			if (!ResourceLoader.instance.LoadedMeshes.ContainsKey(2001))
			{
				var meshfilter = ResourceLoader.GetAssetBundle(2001).LoadAsset<GameObject>("AxePrefab.prefab").GetComponent<MeshFilter>();
				ResourceLoader.instance.LoadedMeshes.Add(2001, meshfilter.sharedMesh);
			}
		}
	}
}