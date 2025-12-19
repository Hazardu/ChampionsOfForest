using UnityEngine;

namespace ChampionsOfForest.Items
{
	public struct RarityDisplayInfo
	{
		public Color color;
		public string name;
		public Color glowColor;
		public float glowIntensity;
		public RarityDisplayInfo(Item.Rarity rarity, Color color, Color glowColor, float glowIntensity)
		{
			this.name = rarity.ToString();	//TODO change to localized string
			this.color = color;
			this.glowColor = glowColor;
			this.glowIntensity = glowIntensity;
		}
	}
}