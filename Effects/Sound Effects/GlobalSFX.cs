using UnityEngine;

namespace ChampionsOfForest.Effects.Sound_Effects
{
	public class GlobalSFX : MonoBehaviour
	{
		public static GlobalSFX instance;
		private AudioSource[] audioSources;
		private readonly int[] ids = new int[] { 1002, 1003, 1007, 1008, 1009, 1010, 1011, 1012, 1013,1016 };

		private void Start()
		{
			audioSources = new AudioSource[ids.Length];
			for (int i = 0; i < ids.Length; i++)
			{
				var source = new GameObject().AddComponent<AudioSource>();
				source.clip = Res.ResourceLoader.instance.LoadedAudio[ids[i]];
				source.spatialBlend = 0;
				source.rolloffMode = AudioRolloffMode.Linear;
				source.maxDistance = 1999999;
				audioSources[i] = source;
				source.gameObject.SetActive(false);
			}
			instance = this;
		}

		public static void Play(int id, ulong delay = 0, float pitch = 1)
		{
			if (id > -1 && id < instance.audioSources.Length)
			{
				instance.audioSources[id].gameObject.SetActive(true);
				instance.audioSources[id].pitch=pitch;
				instance.audioSources[id].Play(delay);
			}
		}
	}
}