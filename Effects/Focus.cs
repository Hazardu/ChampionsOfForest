using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class SeekingArrow : MonoBehaviour
	{
		public static GameObject prefab;

		private void Start()
		{
			if (prefab == null)
			{
				prefab = Res.ResourceLoader.GetAssetBundle(2002).LoadAsset<GameObject>("DeathMark.prefab");
			}
			Instantiate(prefab, transform.position, transform.rotation, transform);
		}

		private void Update()
		{
			transform.LookAt(Camera.main.transform);
			if (!SpellActions.SeekingArrow)
				transform.gameObject.SetActive(false);
		}
	}
}