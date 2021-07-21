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

		public static void Log(string s)
		{
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

	public static class DamageMath
	{
		public const int SILENTattackerType = 2000000;
		public const int SILENTattackerTypeMagic = 2000001;
		public const int CONVERTEDFLOATattackerType = 1000000;


		/// <summary>
		/// Takes a float input damage and returns a integer that is smaller than int.maxvalue and amount of times it has to be sent.
		/// </summary>
		
		public static void ReduceDamageToSendOverNet(float damage, out int outdamage, out int repetitions)
		{
			if (damage < int.MaxValue / 5)
			{
				outdamage = Mathf.FloorToInt(damage);
				repetitions = 1;
				return;
			}
			repetitions = Mathf.FloorToInt(damage / ((float)int.MaxValue / 5f)) + 1;
			outdamage = Mathf.RoundToInt(damage / repetitions);
		}

		public static int GetSendableDamage(float damage)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(damage), 0);

		}
	}
}