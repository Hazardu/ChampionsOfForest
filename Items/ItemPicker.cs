using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class ClinetItemPicker : MonoBehaviour
	{
		public static Transform cam;
		public static float radius = 6.5f;

		#region Instance

		public static ClinetItemPicker Instance
		{
			get;
			private set;
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
		}

		#endregion Instance

		private void Update()
		{
			if (cam == null)            //waits for main cam to be created. Its not there instantly after loading into the scene
			{
				if (Camera.main.transform != null)
				{
					cam = Camera.main.transform;
				}

				return;
			}
			if (LocalPlayer.Stats.Dead)
				return;

			RaycastHit[] hits = Physics.BoxCastAll(cam.position, Vector3.one * 0.1f, cam.forward, cam.rotation, radius);
			for (int i = 0; i < hits.Length; i++)
			{
				ItemPickUp pu = hits[i].transform.root.GetComponent<ItemPickUp>();
				if (pu != null)
				{
					pu.EnableDisplay();
					if (ModAPI.Input.GetButtonDown("ItemPickUp"))
					{
						pu.PickUp();
					}
					return;
				}
			}
		}
	}
}