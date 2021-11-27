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
			else
			{
				//falling back to eng
				new Translations();
			}
			processingLanguage = false;

		}
		public static bool processingLanguage = false;
		public static void GetJson()
		{
			ModAPI.Log.Write(JsonUtility.ToJson(instance));
		}
		private static bool Parse(in string path)
		{
			if (!File.Exists(path))
				return false;
			Debug.Log("Parsing: '" + path);

			var v = File.ReadAllLines(path);
			if (v != null)
			{
				foreach (var line in v)
				{
					var split = line.Split(new string[] { ":: " }, StringSplitOptions.RemoveEmptyEntries);
					Type t = typeof(Translations);
					if (split.Length == 2)
					{
						try
						{
							string entry = split[1].Trim().Trim('\"').Replace("\\n", "\n").Replace("\\t", "\t");

							var f = t.GetField("_" + split[0]);
							f.SetValue(instance, entry);
							Debug.Log("entry " + split[0] + ":   " + f.GetValue(instance));
						}
						catch (Exception e)
						{

							ModAPI.Log.Write(e.ToString());
						}
					}
				}
				ModAPI.Console.Write(path);
				return true;
			}

			return false;
		}

	}

	public class MenuOptionsMod : MenuOptions
	{
		protected override void OnLanguageChange()
		{

			base.OnLanguageChange();
			StartCoroutine(Translations.Load(this.Language.value));

		}
	}
}
