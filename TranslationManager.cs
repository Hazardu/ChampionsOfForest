using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ChampionsOfForest.Localization
{
	[Serializable]
	public partial class Translations
	{
		static Translations instance;
		public Translations()
		{
			instance = this;
		}

		private const string PATH = "Mods/Champions of the Forest/Localization/";
		private string language = "EN";
		private static string getPath(in string localizationID) => PATH + localizationID + ".txt";
		public static void Load(in string localizationID)
		{
			string path = getPath(localizationID);
			if (File.Exists(path))
				Parse(path);


		}
		public static void GetJson()
		{
			ModAPI.Log.Write(JsonUtility.ToJson(instance));
		}
		private static void Parse(in string path)
		{
			var v = JsonUtility.FromJson(File.ReadAllText(path), typeof(Translations));
			if (v != null)
				instance = v as Translations;
		}
	}
}
