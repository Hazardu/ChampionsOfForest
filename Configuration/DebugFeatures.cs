using ChampionsOfForest.Player;
using TheForest.Utils;
using UnityEngine;
using Input = UnityEngine.Input;

namespace ChampionsOfForest
{
	//implement additional UI elements
	public partial class MainMenu : MonoBehaviour
	{

#if Debugging_Enabled
		Vector3? GetEyePosition()
		{

			Transform t = Camera.main.transform;
			Physics.Raycast(t.position, t.forward, out var raycastHit);
			if (raycastHit.transform!=null)
			{
					return raycastHit.transform.position;
			}

			return null;
		}
		void CastMeteor(Vector3 position)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(17);
					w.Write(position.x);
					w.Write(position.y);
					w.Write(position.z);
					w.Write(Random.Range(-100000, 100000));
					w.Close();
				}
				Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}
		public static Vector3 _DEBUG_WEAPON_POSITION_OFFSET, _DEBUG_WEAPON_ROTATION_OFFSET;
		string posX= "0.0", posY= "0.0", posZ= "0.0";
		string rotX= "0.0", rotY= "0.0", rotZ= "0.0";
		private Vector3 rulerStart = Vector3.zero;
		private Vector3 rulerEnd = Vector3.zero;
		private int rulerHue = 1;
		private GameObject rulerObj;

		public static LayerMask GetCollisionMaskOf(GameObject go)
		{
			int myLayer = go.layer;
			int layerMask = 0;
			for (int i = 0; i < 32; i++)
			{
				if (!Physics.GetIgnoreLayerCollision(myLayer, i))
				{
					layerMask = layerMask | 1 << i;
				}
			}
			return layerMask;
		}
		partial void DrawDebug()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				Vector3? eyepos = GetEyePosition();
				if (eyepos.HasValue)
				{
					CastMeteor(new Vector3(eyepos.Value.x, eyepos.Value.y + 100, eyepos.Value.z));
				}
			}
			GUI.Label(new Rect(10, 10, 500, 30), $"Player position X = {LocalPlayer.Transform.position.x} Y = {LocalPlayer.Transform.position.y} Z = {LocalPlayer.Transform.position.z}");
			if (Physics.Raycast(Camera.main.transform.position + (Vector3.forward * 2), Camera.main.transform.forward, out var raycastInfo, 1000, ~0))
			{
				LayerMask layermasks = GetCollisionMaskOf(raycastInfo.transform.gameObject);
				GUI.Label(new Rect(10, 30, 500, 30), $"Tag = {raycastInfo.transform.gameObject.tag} Name = {raycastInfo.transform.gameObject.name} LayerMask = {layermasks.value.ToString("X")} Layer = {raycastInfo.transform.gameObject.layer}" );
			}

			//Ruler

			if (rulerObj == null)
			{
				rulerObj = new GameObject();
				rulerObj.AddComponent<LineRenderer>();
			}
			
			if (Input.GetKeyDown(KeyCode.F7))
			{
				rulerStart = LocalPlayer.Transform.position;
			}
			if (Input.GetKeyDown(KeyCode.F8))
			{
				rulerEnd = LocalPlayer.Transform.position;
			}
			if (Input.GetKeyDown(KeyCode.F9))
			{
				rulerStart = Vector3.zero;
				rulerEnd = Vector3.zero;
			}
		
			if (rulerStart != Vector3.zero && rulerEnd != Vector3.zero)
			{
				rulerHue++;
				Color rulerColor = Color.HSVToRGB((rulerHue % 360) / 360f, 1f, 1f);
				rulerObj.transform.position = rulerStart;
				
				LineRenderer lr = rulerObj.GetComponent<LineRenderer>();
				lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
				lr.SetColors(rulerColor, rulerColor);
				lr.SetWidth(0.1f, 0.1f);
				lr.SetPosition(0, rulerStart);
				lr.SetPosition(1, rulerEnd);
				Vector3 firstCameraPoint = Camera.main.WorldToScreenPoint(rulerStart);
				Vector3 lastCameraPoint = Camera.main.WorldToScreenPoint(rulerEnd);
				if (firstCameraPoint.z > 0f)
				{
					firstCameraPoint.y = Screen.height - firstCameraPoint.y;
					float size = Mathf.Clamp(600 / Vector3.Distance(rulerStart, LocalPlayer.Transform.position), 10, 50);
					size *= screenScale;
					Rect r = new Rect(0, 0, 400, size)
					{
						center = firstCameraPoint
					};
					GUI.Label(r, $"Client distance = {Vector3.Distance(rulerStart, LocalPlayer.Transform.position)}");
				}
				if (lastCameraPoint.z > 0f)
				{
					lastCameraPoint.y = Screen.height - lastCameraPoint.y;
					float size = Mathf.Clamp(600 / Vector3.Distance(rulerEnd, LocalPlayer.Transform.position), 10, 50);
					size *= screenScale;
					Rect r = new Rect(0, 0, 400, size)
					{
						center = lastCameraPoint
					};
					GUI.Label(r, $"Client distance = {Vector3.Distance(rulerEnd, LocalPlayer.Transform.position)}\nLineLen = {Vector3.Distance(rulerStart, rulerEnd)}");
				}
			}
			if (PlayerInventoryMod.EquippedModel != BaseItem.ItemSubtype.None)
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