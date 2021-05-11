using ChampionsOfForest.Player;

using UnityEngine;

namespace ChampionsOfForest
{
	//implement additional UI elements
	public partial class MainMenu : MonoBehaviour
	{
#if Debugging_Enabled

		public static Vector3 _DEBUG_WEAPON_POSITION_OFFSET, _DEBUG_WEAPON_ROTATION_OFFSET;
		string posX= "0.0", posY= "0.0", posZ= "0.0";
		string rotX= "0.0", rotY= "0.0", rotZ= "0.0";
		partial void DrawDebug()
		{
			if (PlayerInventoryMod.EquippedModel != BaseItem.WeaponModelType.None)
			{
				GUI.Label(new Rect(600, 300, 500, 30), "Offset");
				GUI.Label(new Rect(600, 330, 100, 30), PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].offset.x.ToString());
				GUI.Label(new Rect(700, 330, 100, 30), PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].offset.y.ToString());
				GUI.Label(new Rect(800, 330, 100, 30), PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].offset.z.ToString());

				posX = GUI.TextField(new Rect(600, 360, 100, 30), posX);
				posY = GUI.TextField(new Rect(700, 360, 100, 30), posY);
				posZ = GUI.TextField(new Rect(800, 360, 100, 30), posZ);

				GUI.Label(new Rect(600, 400, 500, 30), "Rotation");
				GUI.Label(new Rect(600, 430, 100, 30), PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].rotation.x.ToString());
				GUI.Label(new Rect(700, 430, 100, 30), PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].rotation.y.ToString());
				GUI.Label(new Rect(800, 430, 100, 30), PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].rotation.z.ToString());

				rotX = GUI.TextField(new Rect(600, 460, 100, 30), rotX);
				rotY = GUI.TextField(new Rect(700, 460, 100, 30), rotY);
				rotZ = GUI.TextField(new Rect(800, 460, 100, 30), rotZ);

				if (GUI.Button(new Rect(600, 500, 300, 40), "Apply"))
				{
					if (float.TryParse(posX, out float x))
					{
						if (float.TryParse(posY, out float y))
						{
							if (float.TryParse(posZ, out float z))
							{
								Vector3 v = new Vector3(x, y, z);
								PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].obj.transform.localPosition = v;
								PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].offset = v;
							}
						}
					}
					if (float.TryParse(rotX, out float x1))
					{
						if (float.TryParse(rotY, out float y))
						{
							if (float.TryParse(rotZ, out float z))
							{
								Vector3 v = new Vector3(x1, y, z);
								PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].obj.transform.localRotation =Quaternion.Euler( v);
								PlayerInventoryMod.customWeapons[PlayerInventoryMod.EquippedModel].rotation = v;
							}
						}
					}
				}
			}
		}
#else

		partial void DrawDebug()
		{
		}

#endif
	}
}