namespace ChampionsOfForest.System
{
	public class ModResource
	{
		public const bool CompressTexture = true;
		public const string url = "https://modapi.survivetheforest.net/uploads/objects/7/";
		public readonly string fileName;
		public static string path;
		public bool loaded;

		public enum ResourceType
		{
			Texture, Mesh, Audio, Text, AssetBundle
		}

		public readonly ResourceType type;
		public int ID;


		public ModResource(int id, string FileName)
		{
			ID = id;
			fileName = FileName;
			loaded = false;
			if (fileName.EndsWith(".png", true, System.Globalization.CultureInfo.CurrentCulture) || fileName.EndsWith(".jpeg", true, System.Globalization.CultureInfo.CurrentCulture) || fileName.EndsWith(".jpg", true, System.Globalization.CultureInfo.CurrentCulture))
			{
				type = ResourceType.Texture;
			}
			else if (fileName.EndsWith(".txt", true, System.Globalization.CultureInfo.CurrentCulture))
			{
				type = ResourceType.Text;
			}
			else if (fileName.EndsWith(".obj", true, System.Globalization.CultureInfo.CurrentCulture) || fileName.EndsWith(".mesh", true, System.Globalization.CultureInfo.CurrentCulture))
			{
				type = ResourceType.Mesh;
			}
			else if (fileName.EndsWith(".ogg", true, System.Globalization.CultureInfo.CurrentCulture) || fileName.EndsWith(".mp3", true, System.Globalization.CultureInfo.CurrentCulture) || fileName.EndsWith(".wav", true, System.Globalization.CultureInfo.CurrentCulture))
			{
				type = ResourceType.Audio;
			}

			if (!ResourceLoader.instance.unloadedResources.ContainsKey(id))
				ResourceLoader.instance.unloadedResources.Add(id, this);

		}

		public ModResource(int id, string FileName, ResourceType t)
		{
			ID = id;
			fileName = FileName;
			loaded = false;
			type = t;
			// ResourceLoader.instance.unloadedResources.Add(this);
			ResourceLoader.instance.unloadedResources.Add(id, this);
		}
	}
}