using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class Spell
	{
		public int ID;
		public Texture2D icon;
		public int Levelrequirement;
		public float EnergyCost;
		public float Cooldown;
		public float BaseCooldown;
		public float ChanneledTime;
		public bool Channeled;
		public bool CastOnRelease;

		public bool IsEquipped
		{
			get
			{
				if (EquippedSlot != -1)
				{
					return true;
				}
				return false;
			}
		}

		public delegate void Active();

		public delegate void Aim();

		public Active active;
		public Aim aim;
		public Aim aimEnd;

		public delegate void Passive(bool on);

		public Passive passive;
		public bool usePassiveOnUpdate;

		public bool Bought;
		public bool CanCast = false;

		//Display
		public string Name;

		public delegate string DescriptionDelegate();
		public DescriptionDelegate GetDescription ;
		public int EquippedSlot = -1;

		public Spell()
		{
		}

		public Spell(int iD, int TextureID, int levelrequirement, float energyCost, float baseCooldown, string name, DescriptionDelegate description)
		{
			ID = iD;
			Levelrequirement = levelrequirement;
			EnergyCost = energyCost;
			Cooldown = baseCooldown;
			BaseCooldown = baseCooldown;
			Name = name;
			GetDescription = description;
			CanCast = true;
			Bought = false;

			icon = Res.ResourceLoader.instance.LoadedTextures[TextureID];
			SpellDataBase.spellDictionary.Add(iD, this);
		}

		/// <summary>
		/// Creates a channeled type of spell
		/// </summary>
		public Spell(int iD, int TextureID, int levelrequirement, float energyCost, string name, DescriptionDelegate description)
		{
			ID = iD;
			Levelrequirement = levelrequirement;
			EnergyCost = energyCost;
			Channeled = true;
			Cooldown = 1;
			Name = name;
			GetDescription = description;
			CanCast = true;
			Bought = false;
			icon = Res.ResourceLoader.instance.LoadedTextures[TextureID];
			SpellDataBase.spellDictionary.Add(iD, this);
		}
	}
}