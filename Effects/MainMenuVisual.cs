using System.Collections;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class MainMenuVisual : MonoBehaviour
	{
		public static void Create()
		{
			new GameObject().AddComponent<MainMenuVisual>();
		}

		private IEnumerator Start()
		{
			do
			{
				yield return null;
				yield return null;	
				yield return null;
				yield return null;
			}
			while (!Res.ResourceLoader.instance.FinishedLoading);

			Light light1 = new GameObject().AddComponent<Light>();
			light1.color = Color.red;
			light1.shadows = LightShadows.Hard;
			light1.range = 20;
			light1.transform.position = new Vector3(0, 0, -40);
		}
	}
}