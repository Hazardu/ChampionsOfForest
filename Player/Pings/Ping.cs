using UnityEngine;

namespace ChampionsOfForest
{
	public class MarkObject
	{
		public static Transform Cam
		{
			get
			{
				if (!_camera)
				{
					_camera = Camera.main.transform;
				}
				return _camera;
			}
		}

		private static Transform _camera;
		public float Timestamp;
		public string player;

		public enum PingType
		{
			Enemy, Location, Item
		}

		public PingType pingType;
	}
}