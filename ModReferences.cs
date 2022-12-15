using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Network;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{

	public class ModReferences : MonoBehaviour
	{
		
		private float LevelRequestCooldown = 10;
		private float MFindRequestCooldown = 300;
		public static Material bloodInfusedMaterial;
		private static ModReferences instance;

		public static Transform rightHandTransform = null;
		private void Start()
		{
			instance = this;
			if (BoltNetwork.isRunning)
			{
				Players = new List<GameObject>();
				StartCoroutine(UpdateSetups());
				if (GameSetup.IsMpServer && BoltNetwork.isRunning)
				{
					InvokeRepeating("UpdateLevelData", 1, 1);
				}
			}
			else
			{
				Players = new List<GameObject>() { LocalPlayer.GameObject };
			}
			BoltConnection c;
			
		}


		//public static Transform FindDeepChild(Transform aParent, string aName)
		//{
		//	Transform result = aParent.Find(aName);
		//	if (result != null)
		//	{
		//		return result;
		//	}
		//	else
		//	{
		//		foreach (Transform child in aParent)
		//		{
		//			Transform result2 = FindDeepChild(child, aName);
		//			if (result2 != null)
		//			{
		//				return result2;
		//			}
		//		}
		//		return null;
		//	}
		//}
		//public static string RecursiveTransformList(Transform tr, string s = "", int indents = 0)
		//{
		//	s += new System.String('\t', indents) + tr.name + "\n";
		//	foreach (Transform item in tr)
		//	{
		//		s = RecursiveTransformList(item, s, indents + 1);
		//	}
		//	return s;
		//}

		//public static void RecursiveComponentList(GameObject go)
		//{
		//	RecursiveComponentList(go.transform, "");
		//}

		//private static void RecursiveComponentList(Transform tr, string start)
		//{
		//	ModAPI.Log.Write(start + tr.name + '-' + tr.GetComponents<Component>().Select(x => x.GetType().Name).Join(", "));
		//	start += '\t';
		//	foreach (Transform item in tr)
		//	{
		//		RecursiveComponentList(item, start);
		//	}
		//}
	}
}