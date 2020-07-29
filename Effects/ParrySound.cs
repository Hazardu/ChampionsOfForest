using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class ParrySound : MonoBehaviour
	{
		private static ParrySound instance;
		private AudioSource source;

		private void Initialize()
		{
			source = gameObject.AddComponent<AudioSource>();
			source.clip = Res.ResourceLoader.instance.LoadedAudio[139];
			source.loop = false;
			source.playOnAwake = false;
			source.pitch = 0.75f;
		}

		public static void Play(Vector3 pos)
		{
			if (instance == null)
			{
				instance = new GameObject().AddComponent<ParrySound>();
				instance.Initialize();
			}
			instance.transform.position = pos;
			instance.source.pitch = Random.Range(0.6f, 1f);
			instance.source.Play();
		}
	}
}