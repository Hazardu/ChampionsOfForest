using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace ChampionsOfForest.Effects.Sound_Effects
{
	public class GlobalSFX : MonoBehaviour
	{
		public enum SFX
		{
			Invanvl,
			Invaxe,
			Invblst,
			Invbody,
			Invbook,
			Invbow,
			Invcap,
			Invgrab,
			Invharm,
			Invlarm,
			Invpot,
			Invring,
			Invrock,
			Invscrol,
			Invshiel,
			Invsign,
			Invstaf,
			Invsword,
			ClickUp,
			ClickDown,
			Purge,
			Unlock,
			BloodInfusedArrow,
			Warp,
			SpellUnlock,
			Pickup,
			Sacrifice,
			BlackholeDisappearing,
			Boom,
		}
		public static GlobalSFX instance;
		private AudioSource[] audioSources;
		private void Start()
		{
			int[] ids = Enumerable.Range(1070, 18).Concat(
				new int[] { 1002, 1003, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1016, 1005 }).ToArray();
			
			audioSources = new AudioSource[ids.Length];
			for (int i = 0; i < ids.Length; i++)
			{
				var source = new GameObject().AddComponent<AudioSource>();
				source.clip = Res.ResourceLoader.instance.LoadedAudio[ids[i]];
				source.spatialBlend = 1f;
				source.rolloffMode = AudioRolloffMode.Linear;
				source.maxDistance = 1999999;
				audioSources[i] = source;
				source.gameObject.SetActive(false);
			}
			instance = this;
		}

		public static void Play(SFX sfx, ulong delay = 0, float pitch = 1 )
		{
			int id = (int)sfx;
			instance.audioSources[id].gameObject.SetActive(true);
			instance.audioSources[id].pitch=pitch;
			instance.audioSources[id].Play(delay);
		}
	}
}