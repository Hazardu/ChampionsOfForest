using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using BuilderCore;

using ChampionsOfForest.Localization;
using ChampionsOfForest.Re;

using UnityEngine;

namespace ChampionsOfForest.System
{
	public class ResourceLoader : MonoBehaviour
	{
		public static ResourceLoader instance = null;

		//get or load texture with id
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

		//get or load asset bundle
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


		public static void LoadTexture(ModResource r)
		{
			Texture2D t = new Texture2D(1, 1);
			t.LoadImage(File.ReadAllBytes(ModResource.path + r.fileName));
			t.Apply();
			t.Compress(true);
			instance.LoadedTextures.Add(r.ID, t);
		}


		
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
				if (string.IsNullOrEmpty(ModResource.path))
				{
					ModResource.path = Application.dataPath + "/COTF Files/";
				}
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
			IgnoreErrors = false;
			FinishedLoading = false;
			unloadedResources = new Dictionary<int, ModResource>();
			// unloadedResources = new List<Resource>();
			FillResources();
			LoadedMeshes = new Dictionary<int, Mesh>();
			LoadedTextures = new Dictionary<int, Texture2D>();
			LoadedAudio = new Dictionary<int, AudioClip>();
			ExistingAssetBundles = new Dictionary<int, string>();
			toDownload = new List<ModResource>();
			FailedLoadResources = new List<ModResource>();
			LoadedAssetBundles = new Dictionary<int, AssetBundle>();
			StartCoroutine(FileVerification());
			StartCoroutine(VersionCheck());
			StartCoroutine(DownloadMotd());
			StartCoroutine(Translations.Load(PlayerPreferences.Language));
			buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
		}

		
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
				MOTD = "Offline"; //tr
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

		
		private IEnumerator FileVerification()
		{
			debugLabelText = "";
			loadingState = LoadingState.CheckingFiles;
			CheckedFileNumber = 0;
			DownloadedFileNumber = 0;
			LoadedFileNumber = 0;

			if (DirExists())
			{
				bool DeleteCurrentFiles = false;

				if (ModSettings.RequiresNewFiles)
				{
					if (File.Exists(ModResource.path + "VERSION.txt"))
					{
						string versiontext = File.ReadAllText(ModResource.path + "VERSION.txt");
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

				File.WriteAllText(ModResource.path + "VERSION.txt", ModSettings.Version);
				foreach (ModResource resource in unloadedResources.Values)
				{
					if (File.Exists(ModResource.path + resource.fileName))
					{
						if (DeleteCurrentFiles && (ModSettings.outdatedFiles.Contains(resource.ID) || ModSettings.ALLNewFiles))
						{
							debugLabelText += Translations.ResourceLoader_22/*og:File */ + resource.fileName + Translations.ResourceLoader_21/*og: is marked as outdated, deleting and redownloading.\n*/;//tr
							File.Delete(ModResource.path + resource.fileName);
							toDownload.Add(resource);
						}
						debugLabelText += Translations.ResourceLoader_22/*og:File */ + resource.fileName + Translations.ResourceLoader_23/*og: is ok\n*/;//tr

					}
					else
					{
						debugLabelText += Translations.ResourceLoader_22/*og:File */ + resource.fileName + Translations.ResourceLoader_24/*og: is missing, downloading.\n*/;//tr
						toDownload.Add(resource);
					}
					CheckedFileNumber++;
					yield return null;
				}
			}

			loadingState = LoadingState.Downloading;

			DownloadCount = toDownload.Count;

			foreach (ModResource resource in toDownload)
			{
				debugLabelText += Translations.ResourceLoader_25/*og:Downloading */ + resource.fileName + "\n"; //tr

				WWW www = new WWW(ModResource.url + resource.fileName);
				download = www;

				yield return www;
				if (string.IsNullOrEmpty(www.error) && www.isDone)
				{
					File.WriteAllBytes(ModResource.path + resource.fileName, www.bytes);
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
			foreach (ModResource resource in unloadedResources.Values)
			{
				debugLabelText += Translations.ResourceLoader_27/*og:Loading */ + resource.fileName + "\n";//tr

				switch (resource.type)
				{
					case ModResource.ResourceType.Texture:

						byte[] data = File.ReadAllBytes(ModResource.path + resource.fileName);

						if ((data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71 && data[4] == 13 && data[5] == 10 && data[6] == 26 && data[7] == 10) ||
							(data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF))
						{
							Texture2D t = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
							t.LoadImage(data);
							t.Apply();
							if (ModResource.CompressTexture)
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

					case ModResource.ResourceType.Mesh:
						Mesh mesh = Core.ReadMeshFromOBJ(ModResource.path + resource.fileName);
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

					case ModResource.ResourceType.Audio:
						WWW audioWWW = new WWW("file://" + ModResource.path + resource.fileName);
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

					case ModResource.ResourceType.AssetBundle:
						ExistingAssetBundles.Add(resource.ID, ModResource.path + resource.fileName);
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
			debugLabelText = null;
	}

		private void AttemptRedownload()
		{
			IgnoreErrors = false;
			FinishedLoading = false;
			toDownload.Clear();
			LoadedMeshes.Clear();
			LoadedTextures.Clear();

			foreach (ModResource resource in FailedLoadResources)
			{
				if (File.Exists(ModResource.path + resource.fileName))
				{
					File.Delete(ModResource.path + resource.fileName);
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
				GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), debugLabelText, style);
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
					foreach (ModResource item in FailedLoadResources)
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
			if (!Directory.Exists(ModResource.path))
			{
				debugLabelText += debugLabelText + " \n NO DIRECTORY FOUND, DOWNLOADING \n Please wait... ";
				Directory.CreateDirectory(ModResource.path);
				foreach (ModResource resource in unloadedResources.Values)
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


		private int CheckedFileNumber;
		private int DownloadedFileNumber;
		private int LoadedFileNumber;
		private int DownloadCount;
		public Dictionary<int, ModResource> unloadedResources;
		public List<ModResource> FailedLoadResources;
		public List<ModResource> toDownload;
		public Dictionary<int, Mesh> LoadedMeshes;
		public Dictionary<int, Texture2D> LoadedTextures;
		public Dictionary<int, AudioClip> LoadedAudio;
		public Dictionary<int, string> ExistingAssetBundles;
		public Dictionary<int, AssetBundle> LoadedAssetBundles;

#if DEBUG
		private string debugLabelText;
#endif
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









		private static ResourceLoader()
		{
			ResourceList.FillResources()
		}
	}
}