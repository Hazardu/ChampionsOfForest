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
		public float timestamp;
		public string player;
		protected const float MAXRANGE_SQUARED = 1000 * 1000;   //1k feet squared
		protected const float DURATION = 180;     // 3 min
		public enum PingType
		{
			Enemy, Location, Item
		}

		public PingType pingType;
	}
}