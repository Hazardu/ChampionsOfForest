using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using BuilderCore;

using ChampionsOfForest.Localization;

using UnityEngine;

namespace ChampionsOfForest.Res
{
	public class ResourceLoader : MonoBehaviour
	{
		public static ResourceLoader instance = null;

		public static Texture2D GetTexture(int i)
		{
			if (ModSettings.IsDedicated)
			{
				return null;
			}

			if (instance.LoadedTextures.ContainsKey(i))
			{
				return instance.LoadedTextures[i];
			}
			else if (instance.unloadedResources.ContainsKey(i))
			{
				LoadTexture(instance.unloadedResources[i]);
				return instance.LoadedTextures[i];
			}
			return null;
		}

		public static AssetBundle GetAssetBundle(int id)
		{
			if (instance.ExistingAssetBundles.ContainsKey(id))
			{
				if (instance.LoadedAssetBundles.ContainsKey(id))
					return instance.LoadedAssetBundles[id];
				AssetBundle bundle = AssetBundle.LoadFromFile(instance.ExistingAssetBundles[id]);
				instance.LoadedAssetBundles.Add(id, bundle);
				return bundle;
			}
			return null;
		}

		public static void UnloadTexture(int i)
		{
			Destroy(instance.LoadedTextures[i]);
			instance.LoadedTextures.Remove(i);
		}

		public static void LoadTexture(Resource r)
		{
			Texture2D t = new Texture2D(1, 1);
			t.LoadImage(File.ReadAllBytes(Resource.path + r.fileName));
			t.Apply();
			t.Compress(true);
			instance.LoadedTextures.Add(r.ID, t);
		}

		public Dictionary<int, Resource> unloadedResources;
		public List<Resource> FailedLoadResources;
		public List<Resource> toDownload;
		public Dictionary<int, Mesh> LoadedMeshes;
		public Dictionary<int, Texture2D> LoadedTextures;
		public Dictionary<int, AudioClip> LoadedAudio;
		public Dictionary<int, string> ExistingAssetBundles;
		public Dictionary<int, AssetBundle> LoadedAssetBundles;
		private string LabelText;

		private enum VersionCheckStatus
		{
			Unchecked, UpToDate, OutDated, Fail, NewerThanOnline
		}

		private enum LoadingState
		{
			CheckingFiles, Downloading, Loading, Done
		}

		private LoadingState loadingState;
		private VersionCheckStatus checkStatus = VersionCheckStatus.Unchecked;
		private string OnlineVersion;
		private string Likes;
		private string Downloads;
		private string MOTD;
		private Vector2 MOTDoffset;
		public static bool InMainMenu;
		public bool FinishedLoading = false;
		private WWW download;
		private bool IgnoreErrors;
		private bool ShowMOTD = true;
		private bool MissingMods = false;
		private DateTime buildDate;
		private void Start()
		{
			if (!ModAPI.Mods.LoadedMods.ContainsKey("BuilderCore"))
			{
				MissingMods = true;
				return;
			}

			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
				if (string.IsNullOrEmpty(Resource.path))
				{
					Resource.path = Application.dataPath + "/COTF Files/";
				}
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
			IgnoreErrors = false;
			FinishedLoading = false;
			unloadedResources = new Dictionary<int, Resource>();
			// unloadedResources = new List<Resource>();
			FillResources();
			LoadedMeshes = new Dictionary<int, Mesh>();
			LoadedTextures = new Dictionary<int, Texture2D>();
			LoadedAudio = new Dictionary<int, AudioClip>();
			ExistingAssetBundles = new Dictionary<int, string>();
			toDownload = new List<Resource>();
			FailedLoadResources = new List<Resource>();
			LoadedAssetBundles = new Dictionary<int, AssetBundle>();
			StartCoroutine(FileVerification());
			StartCoroutine(VersionCheck());
			StartCoroutine(DownloadMotd());
			StartCoroutine(Translations.Load(PlayerPreferences.Language));
			buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
		}

		private int DownloadCount;

		private IEnumerator VersionCheck()
		{
			WWW ModapiWebsite = new WWW("https://modapi.survivetheforest.net/api/mods/");
			yield return ModapiWebsite;
			if (string.IsNullOrEmpty(ModapiWebsite.error) && !string.IsNullOrEmpty(ModapiWebsite.text))
			{
				Regex regex1 = new Regex(@"id.:101([^}])+");
				Match match1 = regex1.Match(ModapiWebsite.text);
				if (match1.Success)
				{
					Regex versionRegex = new Regex("(?<=version\"..)([0-9.]+)");
					Regex likesRegex = new Regex("(?<=likes\".)(\\d+)");
					Regex downloadsRegex = new Regex("(?<=downloads\".)(\\d+)");

					Match versionMatch = versionRegex.Match(match1.Value);
					Match likesMatch = likesRegex.Match(match1.Value);
					Match downloadsMatch = downloadsRegex.Match(match1.Value);

					if (versionMatch.Success)
					{
						OnlineVersion = versionMatch.Value;
						if (ModSettings.Version == OnlineVersion)
						{
							checkStatus = VersionCheckStatus.UpToDate;
						}
						else if (CompareVersion(OnlineVersion) == Status.Outdated)
						{
							checkStatus = VersionCheckStatus.OutDated;
						}
						else
						{
							checkStatus = VersionCheckStatus.NewerThanOnline;
						}
						yield break;
					}
					if (likesMatch.Success)
						Likes = likesMatch.Value;
					if (downloadsMatch.Success)
						Downloads = downloadsMatch.Value;
				}
			}
			if (checkStatus == VersionCheckStatus.Unchecked)
			{
				checkStatus = VersionCheckStatus.Fail;
			}
		}

		private IEnumerator DownloadMotd()
		{
			WWW motdWebsite = new WWW("https://docs.google.com/document/export?format=txt&id=1tq7scNmg0_CAzg0TfOhfq737ugaoCw3Idr-0esbKlhE&token=AC4w5Vgy9AG6mRMGIoA_NgkcxmFpPmmVUA%3A1548265532959&ouid=105695979354176851391&includes_info_params=true");
			yield return motdWebsite;
			if (string.IsNullOrEmpty(motdWebsite.error))
				MOTD = motdWebsite.text;
			else
				MOTD = "Couldn't get update notes ;(";
		}

		public enum Status
		{
			TheSame, Outdated, Newer
		}

		public static Status CompareVersion(string s1)
		{
			int i = 0;
			int a = 0;
			string val = "";
			int[] values1 = new int[4];
			int[] values2 = new int[4];

			//filling values1
			while (i < s1.Length)
			{
				if (s1[i] != '.')
				{
					val += s1[i];
				}
				else
				{
					values1[a] = int.Parse(val);
					val = "";
					a++;
				}
				i++;
			}
			if (val != "")
			{
				values1[a] = int.Parse(val);
			}
			val = "";
			a = 0;
			i = 0;

			while (i < ModSettings.Version.Length)
			{
				if (ModSettings.Version[i] != '.')
				{
					val += ModSettings.Version[i];
				}
				else
				{
					values2[a] = int.Parse(val);
					val = "";
					a++;
				}
				i++;
			}
			if (val != "")
			{
				values2[a] = int.Parse(val);
			}
			for (i = 0; i < 4; i++)
			{
				if (values1[i] > values2[i])
				{
					return Status.Outdated;
				}
				else if (values1[i] < values2[i])
				{
					return Status.Newer;
				}
			}
			return Status.TheSame;
		}

		public static Status CompareVersion(string s1, string s2)
		{
			int i = 0;
			int a = 0;
			string val = "";
			int[] values1 = new int[4];
			int[] values2 = new int[4];

			//filling values1
			while (i < s1.Length)
			{
				if (s1[i] != '.')
				{
					val += s1[i];
				}
				else
				{
					values1[a] = int.Parse(val);
					val = "";
					a++;
				}
				i++;
			}
			if (val != "")
			{
				values1[a] = int.Parse(val);
			}
			val = "";
			a = 0;
			i = 0;

			while (i < s2.Length)
			{
				if (s2[i] != '.')
				{
					val += s2[i];
				}
				else
				{
					values2[a] = int.Parse(val);
					val = "";
					a++;
				}
				i++;
			}
			if (val != "")
			{
				values2[a] = int.Parse(val);
			}
			for (i = 0; i < 4; i++)
			{
				if (values1[i] > values2[i])
				{
					return Status.Outdated;
				}
				else if (values1[i] < values2[i])
				{
					return Status.Newer;
				}
			}
			return Status.TheSame;
		}

		private int CheckedFileNumber;
		private int DownloadedFileNumber;
		private int LoadedFileNumber;

		private IEnumerator FileVerification()
		{
			LabelText = "";
			loadingState = LoadingState.CheckingFiles;
			CheckedFileNumber = 0;
			DownloadedFileNumber = 0;
			LoadedFileNumber = 0;

			if (DirExists())
			{
				bool DeleteCurrentFiles = false;

				if (ModSettings.RequiresNewFiles)
				{
					if (File.Exists(Resource.path + "VERSION.txt"))
					{
						string versiontext = File.ReadAllText(Resource.path + "VERSION.txt");
						if (CompareVersion(versiontext) == Status.Outdated)
						{
							DeleteCurrentFiles = true;
						}
					}
					else
					{
						DeleteCurrentFiles = true;
					}
				}

				File.WriteAllText(Resource.path + "VERSION.txt", ModSettings.Version);
				foreach (Resource resource in unloadedResources.Values)
				{
					if (File.Exists(Resource.path + resource.fileName))
					{
						if (DeleteCurrentFiles && (ModSettings.outdatedFiles.Contains(resource.ID) || ModSettings.ALLNewFiles))
						{
							LabelText += Translations.ResourceLoader_22/*og:File */ + resource.fileName + Translations.ResourceLoader_21/*og: is marked as outdated, deleting and redownloading.\n*/;//tr
							File.Delete(Resource.path + resource.fileName);
							toDownload.Add(resource);
						}
						LabelText += Translations.ResourceLoader_22/*og:File */ + resource.fileName + Translations.ResourceLoader_23/*og: is ok\n*/;//tr

					}
					else
					{
						LabelText += Translations.ResourceLoader_22/*og:File */ + resource.fileName + Translations.ResourceLoader_24/*og: is missing, downloading.\n*/;//tr
						toDownload.Add(resource);
					}
					CheckedFileNumber++;
					yield return null;
				}
			}

			loadingState = LoadingState.Downloading;

			DownloadCount = toDownload.Count;

			foreach (Resource resource in toDownload)
			{
				LabelText += Translations.ResourceLoader_25/*og:Downloading */ + resource.fileName + "\n"; //tr

				WWW www = new WWW(Resource.url + resource.fileName);
				download = www;

				yield return www;
				if (string.IsNullOrEmpty(www.error) && www.isDone)
				{
					File.WriteAllBytes(Resource.path + resource.fileName, www.bytes);
				}
				else
				{
					ModAPI.Log.Write(resource.fileName + Translations.ResourceLoader_26/*og: - Error with downloading a file */ + www.error);//tr
				}
				download.Dispose();
				DownloadedFileNumber++;
				yield return null;
			}
			loadingState = LoadingState.Loading;
			yield return null;
			yield return null;
			foreach (Resource resource in unloadedResources.Values)
			{
				LabelText += Translations.ResourceLoader_27/*og:Loading */ + resource.fileName + "\n";//tr

				switch (resource.type)
				{
					case Resource.ResourceType.Texture:

						byte[] data = File.ReadAllBytes(Resource.path + resource.fileName);

						if ((data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71 && data[4] == 13 && data[5] == 10 && data[6] == 26 && data[7] == 10) ||
							(data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF))
						{
							Texture2D t = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
							t.LoadImage(data);
							t.Apply();
							if (Resource.CompressTexture)
							{
								t.Compress(true);
							}
							LoadedTextures.Add(resource.ID, t);
						}
						else
						{
							ModAPI.Log.Write("Missing texture " + resource.fileName);
							FailedLoadResources.Add(resource);
						}
						break;

					case Resource.ResourceType.Mesh:
						Mesh mesh = Core.ReadMeshFromOBJ(Resource.path + resource.fileName);
						if (mesh == null)
						{
							ModAPI.Log.Write("Missing mesh " + resource.fileName);
							FailedLoadResources.Add(resource);
						}
						else
						{
							LoadedMeshes.Add(resource.ID, mesh);
						}
						break;

					case Resource.ResourceType.Audio:
						WWW audioWWW = new WWW("file://" + Resource.path + resource.fileName);
						yield return audioWWW;
						AudioClip clip = null;
						if (resource.fileName.EndsWith(".mp3"))
						{
							clip = audioWWW.GetAudioClip(true, true, AudioType.MPEG);
						}
						else
						{
							clip = audioWWW.GetAudioClip(true, true);
						}

						if (clip != null)
						{
							LoadedAudio.Add(resource.ID, clip);
						}
						else
						{
							ModAPI.Log.Write("Missing audio " + resource.fileName);
							FailedLoadResources.Add(resource);
						}
						break;

					case Resource.ResourceType.AssetBundle:
						ExistingAssetBundles.Add(resource.ID, Resource.path + resource.fileName);
						break;
				}
				LoadedFileNumber++;

				yield return null;
			}
			loadingState = LoadingState.Done;
			Effects.MainMenuVisual.Create();
			yield return new WaitForSeconds(1f);
			FinishedLoading = true;
			FailedLoadResources.Clear();
			toDownload.Clear();
			LabelText = null;
	}

		private void AttemptRedownload()
		{
			IgnoreErrors = false;
			FinishedLoading = false;
			toDownload.Clear();
			LoadedMeshes.Clear();
			LoadedTextures.Clear();

			foreach (Resource resource in FailedLoadResources)
			{
				if (File.Exists(Resource.path + resource.fileName))
				{
					File.Delete(Resource.path + resource.fileName);
					//toDownload.Add(resource);
				}
			}
			FailedLoadResources.Clear();
			StartCoroutine(FileVerification());
		}

		private void OnGUI()
		{
			if (MissingMods)
			{
				float rr = (float)Screen.height / 1080;
				GUI.color = Color.black;
				Rect BGR = new Rect(0, 0, Screen.width, Screen.height);
				GUI.DrawTexture(BGR, Texture2D.whiteTexture);
				GUI.color = Color.red;

				string s = Translations.ResourceLoader_1/*og:Champions of the forest will NOT work without BuilderCore mod. Please install it to use Champions of The Forest*/; //tr

				GUI.Label(new Rect(0, 30 * rr, Screen.width, 200 * rr), s, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = (int)(50 * rr), alignment = TextAnchor.UpperCenter });
				GUI.color = Color.white;

				if (!ModAPI.Mods.LoadedMods.ContainsKey("BuilderCore"))
				{
					if (GUI.Button(new Rect(760 * rr, 200 * rr, 700 * rr, 100 * rr), "BuilderCore Page", new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold, fontSize = (int)(50 * rr), alignment = TextAnchor.MiddleCenter }))
					{
						Application.OpenURL("https://modapi.survivetheforest.net/mod/82/buildercore");
					}
				}
				if (GUI.Button(new Rect(760 * rr, 800 * rr, 700 * rr, 100 * rr), Translations.ResourceLoader_2/*og:Quit Game*/, new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold, fontSize = (int)(50 * rr), alignment = TextAnchor.MiddleCenter })) //tr
				{
					Application.Quit();
				}
				return;
			}

			if (!InMainMenu)
			{
				return;
			}

			if (!FinishedLoading)
			{
				float rr = (float)Screen.height / 1080;
				GUI.color = Color.black;
				Rect BGR = new Rect(0, 0, Screen.width, Screen.height);
				GUI.DrawTexture(BGR, Texture2D.whiteTexture);
				GUI.color = Color.white;

				GUI.Label(new Rect(0, 30 * rr, Screen.width, 60 * rr), Translations.ResourceLoader_3/*og:Please wait while Champions of the Forest is loading.*/, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic, fontSize = (int)(30 * rr), alignment = TextAnchor.UpperCenter }); //tr
				GUIStyle skin = new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = (int)(30 * rr),
					alignment = TextAnchor.MiddleCenter
				};
				switch (loadingState)
				{
					case LoadingState.CheckingFiles:
						GUI.Label(new Rect(0, 100 * rr, Screen.width, 300 * rr), Translations.ResourceLoader_4/*og:Step (1 of 3)\nChecking for existing files.*/, new GUIStyle(GUI.skin.label) { fontSize = (int)(55 * rr), alignment = TextAnchor.UpperCenter }); //tr
						Rect pgBar = new Rect(Screen.width / 2 - 300 * rr, 600 * rr, 600 * rr, 50 * rr);
						Rect prog = new Rect(pgBar);
						pgBar.width *= (float)CheckedFileNumber / unloadedResources.Count;
						GUI.color = Color.gray;
						GUI.DrawTexture(prog, Texture2D.whiteTexture);
						GUI.color = Color.red;
						GUI.DrawTexture(pgBar, Texture2D.whiteTexture);
						GUI.color = Color.black;
						GUI.Label(prog, CheckedFileNumber + "/" + unloadedResources.Count, skin);
						break;

					case LoadingState.Downloading:
						GUI.Label(new Rect(0, 100 * rr, Screen.width, 300 * rr), Translations.ResourceLoader_5/*og:Step (2 of 3)\nDownloading missing files.*/, new GUIStyle(GUI.skin.label) { fontSize = (int)(55 * rr), alignment = TextAnchor.UpperCenter }); //tr
						Rect pgBar1 = new Rect(Screen.width / 2 - 300 * rr, 600 * rr, 600 * rr, 50 * rr);
						Rect prog1 = new Rect(pgBar1);
						pgBar1.width *= (float)DownloadedFileNumber / DownloadCount;
						GUI.color = Color.gray;
						GUI.DrawTexture(prog1, Texture2D.whiteTexture);
						GUI.color = Color.blue;
						GUI.DrawTexture(pgBar1, Texture2D.whiteTexture);
						GUI.color = Color.black;
						GUI.Label(prog1, DownloadedFileNumber + "/" + DownloadCount, skin);
						if (download != null)
						{

							Rect downloadRectBG = new Rect(prog1);
							downloadRectBG.y += 100 * rr;
							Rect downloadRect = new Rect(downloadRectBG);
							downloadRect.width *= download.progress;
							GUI.color = Color.gray;
							GUI.DrawTexture(downloadRectBG, Texture2D.whiteTexture);
							GUI.color = Color.cyan;
							GUI.DrawTexture(downloadRect, Texture2D.whiteTexture);
							GUI.color = Color.black;
							GUI.Label(prog1, download.progress * 100 + "%", skin);
						}
						GUI.color = Color.white;
						break;

					case LoadingState.Loading:
						GUI.Label(new Rect(0, 100 * rr, Screen.width, 300 * rr), Translations.ResourceLoader_6/*og:Step (3 of 3)\nLoading assets.*/, new GUIStyle(GUI.skin.label) { fontSize = (int)(55 * rr), alignment = TextAnchor.UpperCenter }); //tr
						Rect pgBar2 = new Rect(Screen.width / 2 - 300 * rr, 600 * rr, 600 * rr, 50 * rr);
						Rect prog2 = new Rect(pgBar2);
						pgBar2.width *= (float)LoadedFileNumber / unloadedResources.Count;
						GUI.color = Color.gray;
						GUI.DrawTexture(prog2, Texture2D.whiteTexture);
						GUI.color = Color.green;
						GUI.DrawTexture(pgBar2, Texture2D.whiteTexture);
						GUI.color = Color.black;
						GUI.Label(prog2, LoadedFileNumber + "/" + unloadedResources.Count, skin);
						break;

					case LoadingState.Done:
						GUI.Label(new Rect(0, 100 * rr, Screen.width, 300 * rr), Translations.ResourceLoader_7/*og:Done!\n Enjoy*/, new GUIStyle(GUI.skin.label) { fontSize = (int)(55 * rr), alignment = TextAnchor.UpperCenter }); //tr
						break;
				}
				GUI.color = Color.white;
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = (int)(12 * rr),
					alignment = TextAnchor.LowerRight,
				};
				GUI.color = Color.gray;
				GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), LabelText, style);
				GUI.color = Color.white;

			}
			else
			{
				if (FailedLoadResources.Count > 0 && !IgnoreErrors)
				{
					float rr = (float)Screen.height / 1080;
					GUI.color = Color.black;
					Rect BGR = new Rect(0, 0, Screen.width, Screen.height);
					GUI.DrawTexture(BGR, Texture2D.whiteTexture);
					GUI.color = Color.white;
					string text = Translations.ResourceLoader_8/*og:OH NO!\nThere were errors with loading resources for COTF!\n\nUnable to load those assets:\n*/; //tr
					foreach (Resource item in FailedLoadResources)
					{
						text += item.fileName + ",\t";
					}
					text += Translations.ResourceLoader_9/*og:\n\nWhat would you like to do now?*/; //tr
					GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = (int)(30 * rr), alignment = TextAnchor.UpperCenter, wordWrap = true };
					Rect labelRect = new Rect(0, 100 * rr, Screen.width, style.CalcHeight(new GUIContent(text), Screen.width));
					GUI.Label(labelRect, text, style);
					float y = labelRect.yMax;
					y = Mathf.Clamp(y, 0, Screen.height - 100 * rr);
					Rect bt1 = new Rect(Screen.width - 300 * rr, y, 300 * rr, 100 * rr);
					Rect bt2 = new Rect(Screen.width - 600 * rr, y, 300 * rr, 100 * rr);
					GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { fontSize = (int)(30 * rr), wordWrap = true, fontStyle = FontStyle.BoldAndItalic };
					if (GUI.Button(bt1, Translations.ResourceLoader_10/*og:IGNORE ERRORS*/, btnStyle)) //tr
					{
						IgnoreErrors = true;
						FailedLoadResources = null;
					}
					if (GUI.Button(bt2, Translations.ResourceLoader_11/*og:ATTEMPT REDOWNLOAD*/, btnStyle)) //tr
					{
						AttemptRedownload();
					}
				}

				GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

				GUIStyle versionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperRight, fontSize = 34, richText = true };
				switch (checkStatus)
				{
					case VersionCheckStatus.Unchecked:
						GUI.color = Color.gray;
						GUILayout.Label(Translations.ResourceLoader_12/*og:Checking for updated version...*/, versionStyle); //tr
						break;

					case VersionCheckStatus.UpToDate:
						GUI.color = Color.green;
						GUILayout.Label(Translations.ResourceLoader_14/*og:COTF up to date. */ + ((!string.IsNullOrEmpty(Likes) && !string.IsNullOrEmpty(Downloads)) ? Translations.ResourceLoader_13/*og:Thanks for {0} likes and {1} downloads!*/( Likes, Downloads) : ""), versionStyle); //tr

						break;

					case VersionCheckStatus.OutDated:
						GUI.color = Color.red;
						versionStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.UpperRight, fontSize = 34, richText = true };

						if (GUILayout.Button(Translations.ResourceLoader_15/*og:<b>Champions of The Forest is outdated!</b> \n Installed {0};  Newest {1}*/(ModSettings.Version, OnlineVersion), versionStyle)) //tr
						{
							Application.OpenURL(@"https://modapi.survivetheforest.net/mod/101/champions-of-the-forest/");
						}

						break;

					case VersionCheckStatus.Fail:
						GUI.color = Color.gray;
						GUILayout.Label(Translations.ResourceLoader_16/*og:Failed to get update info*/, versionStyle); //tr
						break;

					case VersionCheckStatus.NewerThanOnline:
						GUI.color = Color.yellow;
						GUILayout.Label(Translations.ResourceLoader_17(/*og:Preview version ({0}) | ({1})*/ ModSettings.Version , buildDate), versionStyle); //tr
						break;
				}
				GUI.color = Color.white;
				GUILayout.EndArea();

				if (MOTD != "")
				{
					GUIStyle title = new GUIStyle(GUI.skin.button) { fontSize = 34, fontStyle = FontStyle.Bold, richText = true };
					GUIStyle motdstyle = new GUIStyle(GUI.skin.box) { fontSize = 22, fontStyle = FontStyle.Normal, alignment = TextAnchor.UpperCenter, wordWrap = true, richText = true };

					Rect r1 = new Rect(new Rect(Screen.width * 2 / 3, 120, Screen.width / 3, 50));
					if (GUI.Button(r1, "◄" + Translations.ResourceLoader_18/*og:NEWS*/ + "►", title)) //tr
					{
						ShowMOTD = !ShowMOTD;
					}

					if (ShowMOTD)
					{
						float height = motdstyle.CalcHeight(new GUIContent(MOTD), r1.width);
						Rect r2 = new Rect(r1.x, r1.y + 40, r1.width, Screen.height - 200);
						GUILayout.BeginArea(r2);
						MOTDoffset = GUILayout.BeginScrollView(MOTDoffset);

						GUILayout.Label(MOTD, motdstyle);

						GUILayout.EndScrollView();
						GUILayout.EndArea();
					}
				}


				//language selection
				if (Translations.processingLanguage)
				{
					GUI.Label(new Rect(0, 0, 500, 100), Translations.ResourceLoader_19/*og:Changing COTF language...*/); //tr

				}
				else
				{
					if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 50, 200, 50), Translations.ResourceLoader_20/*og:Update COTF Language*/)) //tr
					{
						StartCoroutine(Translations.Load(PlayerPreferences.Language));
						Translations.LoadNoDl(PlayerPreferences.Language);
					}
				}


			}
		}

		private bool DirExists()
		{
			if (!Directory.Exists(Resource.path))
			{
				LabelText += LabelText + " \n NO DIRECTORY FOUND, DOWNLOADING \n Please wait... ";
				Directory.CreateDirectory(Resource.path);
				foreach (Resource resource in unloadedResources.Values)
				{
					toDownload.Add(resource);
				}
				return false;
			}
			else
			{
				return true;
			}
		}

		private void FillResources()
		{
			new Resource(1, "wheel.png");
			new Resource(2, "wheelOn.png");
			new Resource(5, "SpellBG.png");
			new Resource(6, "SpellFrame.png");
			new Resource(8, "CoolDownFill.png");
			new Resource(12, "Item1SlotBg.png");
			new Resource(13, "Item1Frame.png");
			new Resource(15, "ProgressBack.png");
			new Resource(16, "ProgressFill.png");
			new Resource(17, "ProgressFront.png");
			new Resource(18, "CombatTimer.png");
			new Resource(20, "BlackHoleTex.png");
			new Resource(21, "BlackHole.obj");
			new Resource(22, "Leaf.png");
			new Resource(24, "SmallCircle.png");
			new Resource(25, "Row.png");
			new Resource(26, "snowflake.png");
			new Resource(27, "Background.png");
			new Resource(28, "HorizontalListItem.png");
			new Resource(30, "Space.png");
			new Resource(40, "amulet.obj");
			new Resource(41, "Glove.obj");
			new Resource(42, "ChestArmor.obj");
			new Resource(43, "ring.obj");
			new Resource(44, "Boots2.obj");
			new Resource(45, "Bracer.obj");
			//new Resource(46, "Boots1.obj");
			//new Resource(47, "Boots2.obj");
			new Resource(48, "helmet_armet_2.obj");
			new Resource(49, "Shield.obj");
			new Resource(50, "Pants2.obj");
			new Resource(51, "Sword.obj");
			new Resource(52, "HeavySword.obj");
			new Resource(53, "Shoulder.obj");
			//new Resource(59, "SwordMetalic.png");
			new Resource(60, "SwordTexture.png");
			new Resource(61, "SwordColor.png");
			new Resource(62, "SwordEmissive.png");
			new Resource(64, "SwordNormal.png");
			new Resource(65, "SwordRoughness.png");
			new Resource(66, "SwordAmbientOcculusion.png");
			new Resource(67, "ManyParticles.png");
			new Resource(68, "InverseSphere.obj");
			new Resource(69, "ChainPart.obj");
			new Resource(70, "Spike.obj");
			new Resource(71, "particle.png");
			new Resource(72, "Melee.jpg");
			new Resource(73, "Magic.jpg");
			new Resource(74, "Ranged.jpg");
			new Resource(75, "Defensive.jpg");
			new Resource(76, "Utility.jpg");
			new Resource(77, "Support.jpg");
			new Resource(78, "Background1.jpg");
			new Resource(82, "PerkNode1.png");
			new Resource(81, "PerkNode2.png");
			new Resource(83, "PerkNode3.png");
			new Resource(84, "PerkNode4.png");
			new Resource(85, "ItemBoots.png");
			new Resource(86, "ItemGloves.png");
			new Resource(87, "ItemPants.png");
			new Resource(88, "ItemGreatSword.png");
			new Resource(89, "ItemLongSword.png");
			new Resource(90, "ItemRing.png");
			new Resource(91, "ItemHelmet.png");
			new Resource(92, "ItemBoots1.png");
			new Resource(93, "ItemBracer.png");
			new Resource(94, "ItemBracer2.png");
			new Resource(95, "ItemShoulder.png");
			new Resource(96, "ItemChest.png");
			new Resource(98, "ItemQuiver.png");
			new Resource(99, "ItemShield.png");
			new Resource(100, "ItemScarf.png");
			new Resource(101, "ItemAmulet.png");
			new Resource(102, "Heart.obj");
			new Resource(103, "HeartTexture.png");
			new Resource(104, "HeartNormal.png");
			new Resource(105, "ItemHeart.png");
			new Resource(106, "Corner.png");
			new Resource(107, "BeamParticleHorizontal.png");
			new Resource(108, "Hammer.obj");
			new Resource(109, "ItemHammer.png");
			new Resource(110, "ItemScroll.png");
			new Resource(111, "FlameParticle.png");
			new Resource(112, "Portal.obj");
			new Resource(113, "FantasyArrow.obj");
			new Resource(114, "SpellPortal.png");
			new Resource(115, "SpellDarkFlame.png");
			new Resource(116, "Quiver.obj");
			new Resource(117, "SpellReach.png");
			new Resource(118, "SpellShield.png");
			new Resource(119, "BlackHoleSpell.png");
			new Resource(120, "FlareSpell.png");
			new Resource(121, "BlinkSpell.png");
			new Resource(122, "SpellHealDome.png");
			new Resource(123, "WarCrySpell.png");
			new Resource(124, "Scroll.obj");
			new Resource(125, "SpellMagicArrow.png");
			new Resource(126, "SpellEffect.png");
			new Resource(127, "MultishotSpell.png");
			new Resource(128, "Iceparticle.png");
			new Resource(129, "Shockwave.png");
			new Resource(130, "ballLightningSpell.png");
			new Resource(131, "SpellBerserk.png");
			new Resource(132, "SpellPurge.png");
			new Resource(133, "SpellGold.png");
			new Resource(134, "Bash.png");
			new Resource(135, "SeekingArrow.png");
			new Resource(136, "Frenzy.png");
			new Resource(137, "Focus.png");
			new Resource(138, "ItemAxe.png");
			new Resource(139, "Parry.wav");
			new Resource(140, "Parry.png");
			new Resource(141, "Cataclysm.png");
			new Resource(142, "MassacreBG.png");
			new Resource(143, "BuffBG.png");
			new Resource(144, "BuffBorder.png");
			new Resource(145, "BuffActive.png");
			new Resource(146, "ArmorBuff.png");
			new Resource(147, "Armordebuff.png");
			new Resource(148, "AtkSlow.png");
			new Resource(149, "AtkSpeedBuff.png");
			new Resource(150, "BuffMS.png");
			new Resource(151, "DamageBuff.png");
			new Resource(152, "Damagedebuff.png");
			new Resource(153, "Deathpact.png");
			new Resource(154, "DebuffImmune.png");
			new Resource(155, "DebuffResistant.png");
			new Resource(156, "DebuffSlowMs.png");
			new Resource(157, "poisoned.png");
			new Resource(158, "RootImmune.png");
			new Resource(159, "SecondChanceCD.png");
			new Resource(160, "StaminaRegen.png");
			new Resource(161, "StaminaRegenDebuff.png");
			new Resource(162, "rooted.png");
			new Resource(163, "Stun.png");
			new Resource(164, "LevelUp.png");
			new Resource(165, "BloodInfusedArrow.png");
			new Resource(167, "Bow.obj");
			new Resource(168, "BowNormals.png");
			new Resource(169, "FireBowTex.png");
			new Resource(170, "ItemFireBow.png");
			new Resource(171, "EnemyPingElite.png");
			new Resource(172, "EnemyPingNormal.png");
			new Resource(173, "LocationPing.png");
			new Resource(174, "PickupPing.png");
			new Resource(175, "Polearm.obj");
			new Resource(181, "itemPolearm.png");
			new Resource(182, "FartSpell.png");
			new Resource(184, "ItemOre.png");
			new Resource(185, "ItemFeather.png");
			new Resource(186, "ItemSharkTooth.png");
			new Resource(187, "ItemSapphire.png");
			new Resource(188, "ItemMoonstone.png");
			new Resource(189, "SpellTaunt.png");
			new Resource(190, "ItemSocketDrill.png");
			new Resource(191, "BottleBlue.png");
			new Resource(192, "BottleGreen.png");
			new Resource(193, "BottleOrange.png");
			new Resource(194, "BottleOrange2.png");
			new Resource(195, "BottleYellow.png");
			new Resource(196, "BottleCrimson.png");
			new Resource(197, "BottleGray.png");
			new Resource(198, "BottleAcid.png");
			new Resource(199, "BottlePurple.png");
			new Resource(200, "SpellSnowstorm.png");
			new Resource(201, "SpellFirebolt.png");
			//Sound files
			new Resource(1000, "thundersound.wav");
			new Resource(1001, "Levelup.wav");
			new Resource(1002, "ClickUp.wav");
			new Resource(1003, "ClickDown.wav");
			new Resource(1004, "Drop.wav");
			new Resource(1005, "MeteorImpact.wav");
			new Resource(1006, "MeteorSFX.wav");
			new Resource(1007, "Purge.wav");
			new Resource(1008, "Unlock.wav");
			new Resource(1009, "BloodInfusedArrow.wav");
			new Resource(1010, "Warp.wav");
			new Resource(1011, "SpellUnlock.wav");
			new Resource(1012, "Pickup.wav");
			new Resource(1013, "sacrificeSfx.wav");
			new Resource(1014, "SnapFreeze.wav");
			new Resource(1015, "Blizzard.wav");
			new Resource(1016, "BlackholeDisappearing.wav");
			new Resource(1017, "Roaring Cheeks spell sfx.wav");

			new Resource(1050, "flip1.wav");
			new Resource(1051, "flip.wav");
			new Resource(1052, "flipanvl.wav");
			new Resource(1053, "flipaxe.wav");
			new Resource(1054, "flipblst.wav");
			new Resource(1055, "flipbody.wav");
			new Resource(1056, "flipbook.wav");
			new Resource(1057, "flipbow.wav");
			new Resource(1058, "flipcap.wav");
			new Resource(1059, "flipharm.wav");
			new Resource(1060, "fliplarm.wav");
			new Resource(1061, "flipmag.wav");
			new Resource(1062, "flipmag1.wav");
			new Resource(1063, "flippot.wav");
			new Resource(1064, "flipring.wav");
			new Resource(1065, "fliprock.wav");
			new Resource(1066, "flipsarc.wav");
			new Resource(1067, "flipscrl.wav");
			new Resource(1068, "flipshld.wav");
			new Resource(1069, "flipsign.wav");
			new Resource(1070, "invanvl.wav");
			new Resource(1071, "invaxe.wav");
			new Resource(1072, "invblst.wav");
			new Resource(1073, "invbody.wav");
			new Resource(1074, "invbook.wav");
			new Resource(1075, "invbow.wav");
			new Resource(1076, "invcap.wav");
			new Resource(1077, "invgrab.wav");
			new Resource(1078, "invharm.wav");
			new Resource(1079, "invlarm.wav");
			new Resource(1080, "invpot.wav");
			new Resource(1081, "invring.wav");
			new Resource(1082, "invrock.wav");
			new Resource(1083, "invscrol.wav");
			new Resource(1084, "invshiel.wav");
			new Resource(1085, "invsign.wav");
			new Resource(1086, "invstaf.wav");
			new Resource(1087, "invsword.wav");

			//Asset Bundles
			new Resource(2000, "balllightning", Resource.ResourceType.AssetBundle);
			new Resource(2001, "axe", Resource.ResourceType.AssetBundle);
			new Resource(2002, "deathmark", Resource.ResourceType.AssetBundle);
			new Resource(2003, "fire", Resource.ResourceType.AssetBundle);
			new Resource(2004, "blackholeeffect.bruh", Resource.ResourceType.AssetBundle);
			new Resource(2005, "update157", Resource.ResourceType.AssetBundle);
		}
	}
}