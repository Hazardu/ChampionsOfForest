using System.Collections;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class AsyncHit : MonoBehaviour
	{
		public static AsyncHit instance = null;

		private void Start()
		{
			instance = this;
		}


		public static void SendCommandDelayed(int frames, byte[] s, Network.NetworkManager.Target target)
		{
			instance.StartCoroutine(instance.SendCommandDelayedCoroutine(frames, s, target));
		}

		public IEnumerator SendCommandDelayedCoroutine(int frames, byte[] s, Network.NetworkManager.Target target)
		{
			for (int i = 0; i < frames; i++)
			{
				yield return null;
			}
			Network.NetworkManager.SendLine(s, target);
		}
	}
}