using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TheForest.UI;

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
		private static readonly Dictionary<string, string> localizationLang = new Dictionary<string, string>()
		{
			{ "English","English" },
			{ "Polskie","Polish" },
			{ "Français","French" },
			{ "Deutsch","German" },
			{ "русский","Russian" },
			{ "Español","Spanish" },
			{ "Türk","Turkish" },
			{ "suomalainen","Finnish" },
			{ "中国","Chinese" },
			{ "简体中文","ChineseSimplified" },
			{ "Italiano","Italian" },
			{ "日本語","Japanese" },
		};

		private const string REPO = "https://raw.githubusercontent.com/Hazardu/ChampionsOfForest/master/Locales/";
		private const string PATH = "Mods/Champions of the Forest/Localization/";
		private string language = "English";
		private static string getPath(in string localizationID) => PATH + localizationID + ".txt";

		public static void LoadNoDl(string localizationID)
		{
			Debug.Log("localizationID: '" + localizationID);
			var langName = localizationID;
			if (localizationLang.ContainsKey(localizationID))
				langName = localizationLang[localizationID];
			string path = getPath(langName);
			if (Parse(path))
				instance.language = localizationID;
		}
		public static IEnumerator Load(string localizationID)
		{

			Debug.Log("localizationID: '" + localizationID);
			var langName = localizationID;
			if (localizationLang.ContainsKey(localizationID))
				langName = localizationLang[localizationID];
			string path = getPath(langName);

			if (!Directory.Exists(PATH))
				Directory.CreateDirectory(PATH);

			{	//download part
				Debug.Log("Downloading: '" + REPO + langName + ".txt");
				WWW dl = new WWW(REPO + langName + ".txt");
				yield return dl;
				if (dl.bytesDownloaded > 0)
					File.WriteAllText(path, dl.text);
				dl.Dispose();
			}
			yield return null;

			if (Parse(path))
				instance.language = localizationID;
			processingLanguage = false;

		}
		public static bool processingLanguage = false;
		//public static void GetJson()
		//{
		//	ModAPI.Log.Write(JsonUtility.ToJson(instance));
		//}
		private static bool Parse(in string path)
		{
			if (!File.Exists(path))
				return false;
			Debug.Log("Parsing: '" + path);

			var v = File.ReadAllLines(path);
			if (v != null || v.Length <2)
			{
				int errors = 0;
				foreach (var line in v)
				{
					var split = line.Split(new string[] { ":: " }, StringSplitOptions.RemoveEmptyEntries);
					Type t = typeof(Translations);
					if (split.Length == 2)
					{
						try
						{
							string entry = split[1].Trim().Trim('\"').Replace("\\n", "\n").Replace("\\t", "\t");
							t.GetField("_" + split[0]).SetValue(instance, entry);
							Debug.Log("loaded translation variable value");
						}
						catch (Exception e)
						{
							Debug.LogError("failure loading translation value: "+ split[0]);
							errors++;
							ModAPI.Console.Write(e.ToString());
						}
					}
				}
				ModAPI.Console.Write("loaded " + path + " with error count: "+ errors);
				return true;
			}
			instance = new Translations();
			ModAPI.Console.Write("Could not load file");

			return false;
		}

	}

	//class to alter language
	public class MenuOptionsMod : MenuOptions
	{
		protected override void OnLanguageChange()
		{

			base.OnLanguageChange();
			StartCoroutine(Translations.Load(this.Language.value));

		}
	}
}
