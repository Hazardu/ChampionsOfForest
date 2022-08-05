using System;
using System.Text.RegularExpressions;

using UnityEngine;

namespace ChampionsOfForest
{
	public static class CotfUtils
	{

		public static void Log(string s, bool logFile = false)
		{
			if (logFile)
				ModAPI.Log.Write(s);
			ModAPI.Console.Write(s);
			Debug.Log(s);
		}

		public static string ListAllChildren(Transform tr, string prefix)
		{
			string s = prefix + "•" + tr.name + " ";
			var comps = tr.gameObject.GetComponents<Component>();
			s += "( ";
			foreach (var item in comps)
			{
				s += item.GetType().ToString() + ", ";
			}
			s += ")\n";
			foreach (Transform child in tr)
			{
				s += ListAllChildren(child, prefix + "  ");
			}
			return s;
		}

		public static float DamageReduction(int armor)
		{
			armor = Mathf.Max(armor, 0);

			float a = armor;
			float b = (armor + 500f);

			return Mathf.Pow(Mathf.Clamp01(a / b), 2f);
		}
	}

}