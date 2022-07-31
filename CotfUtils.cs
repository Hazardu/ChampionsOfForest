using System;
using System.Text.RegularExpressions;

using UnityEngine;

namespace ChampionsOfForest
{
	public static class CotfUtils
	{
		//removes any non ascii characters from a name of the player
		public static string TrimNonAscii(string value)
		{
			string pattern = "[^ -~]+";
			Regex reg_exp = new Regex(pattern);
			return reg_exp.Replace(value, "a");
		}

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
	}

}