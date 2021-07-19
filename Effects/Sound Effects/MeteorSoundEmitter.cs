using UnityEngine;

namespace ChampionsOfForest.Effects.Sound_Effects
{
	public class MeteorSoundEmitter : MonoBehaviour
	{
		public int nMeteors = 0;
		private static AudioClip hitSound, InitSound;
		private AudioSource SoundSource;
		
		void Start()
		{
			hitSound = Res.ResourceLoader.instance.LoadedAudio[1005];
			InitSound = Res.ResourceLoader.instance.LoadedAudio[1006];
			SoundSource = gameObject.AddComponent<AudioSource>();
			SoundSource.spatialBlend = 1f;
			SoundSource.rolloffMode = AudioRolloffMode.Linear;
			SoundSource.maxDistance = 100;
		}

		public void PlaySpawnSound()
		{
			SoundSource.PlayOneShot(InitSound);
		}

		public void PlayExplosionSound()
		{
			SoundSource.PlayOneShot(hitSound);
			--nMeteors;
			if (nMeteors == 0)
			{
				Destroy(this, 4f);
			}
		}

	}
}
