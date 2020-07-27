using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class CustomWeapon
	{
		public static Material trailMaterial;

		public float damage;
		public float swingspeed;
		public float tiredswingspeed;
		public float smashDamage;
		public float treeDamage;
		public float staminaDrain;
		public bool blockTreeCut;
		public bool spearType;
		public Mesh mesh;
		public Vector3 offset;
		public Vector3 rotation;
		public Vector3 tipPosition;
		public float ColliderScale;
		public float Scale;
		public Material material;
		public GameObject obj;
		public GameObject objectToHide;
		public TrailRenderer trail;
		public Renderer renderer;

		public float trailWidth = 0.14f;

		public CustomWeapon(BaseItem.WeaponModelType model, int mesh, Material material, Vector3 offset, Vector3 rotation, Vector3 tip, float colliderScale = 1, float scale = 1, float damage = 5, float smashDamage = 15, float swingspeed = 1, float triedswingspeed = 1, float staminaDrain = 6, bool noTreeCut = false, float treeDamage = 1)
		{
			this.damage = damage;
			this.swingspeed = swingspeed;
			this.tiredswingspeed = triedswingspeed;
			this.smashDamage = smashDamage;
			this.treeDamage = treeDamage;
			this.staminaDrain = staminaDrain;
			this.blockTreeCut = noTreeCut;
			this.mesh = Res.ResourceLoader.instance.LoadedMeshes[mesh];
			this.offset = offset;
			this.rotation = rotation;
			this.material = material;
			this.tipPosition = tip;
			ColliderScale = colliderScale;
			Scale = scale;
			CreateGameObject();
			InitializeCustomWeapon();
			PlayerInventoryMod.customWeapons.Add(model, this);
		}

		public CustomWeapon(BaseItem.WeaponModelType model, int mesh, Material material, Vector3 offset, Vector3 rotation, Vector3 tip, float scale = 1)
		{
			this.damage = 6;
			this.swingspeed = 1;
			this.tiredswingspeed = 1;
			this.smashDamage = 15;
			this.treeDamage = 0;
			this.staminaDrain = 8;
			this.blockTreeCut = false;
			this.mesh = Res.ResourceLoader.instance.LoadedMeshes[mesh];
			this.offset = offset;
			this.rotation = rotation;
			this.material = material;
			this.tipPosition = tip;
			ColliderScale = 1;
			Scale = scale;
			CreateGameObject();
			InitializeCustomWeapon();
			PlayerInventoryMod.customWeapons.Add(model, this);
		}

		public CustomWeapon(BaseItem.WeaponModelType model, GameObject obj, TrailRenderer trail, Vector3 offset, Vector3 rotation, float scale = 1)
		{
			this.damage = 6;
			this.swingspeed = 1;
			this.tiredswingspeed = 1;
			this.smashDamage = 15;
			this.treeDamage = 0;
			this.staminaDrain = 8;
			this.blockTreeCut = false;
			this.offset = offset;
			this.rotation = rotation;
			ColliderScale = 1;
			Scale = scale;
			this.obj = obj;
			this.trail = trail;
			InitializeCustomWeapon();
			PlayerInventoryMod.customWeapons.Add(model, this);
		}

		public CustomWeapon(BaseItem.WeaponModelType model, GameObject obj, Vector3 offset, Vector3 rotation, float scale = 1)
		{
			this.damage = 6;
			this.swingspeed = 1;
			this.tiredswingspeed = 1;
			this.smashDamage = 15;
			this.treeDamage = 0;
			this.staminaDrain = 8;
			this.blockTreeCut = false;
			this.offset = offset;
			this.rotation = rotation;
			ColliderScale = 1;
			Scale = scale;
			this.obj = obj;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.Rotate(rotation, Space.Self);
			obj.transform.localPosition = offset;
			obj.transform.localScale = Vector3.one * Scale;
			PlayerInventoryMod.customWeapons.Add(model, this);
		}

		public void EnableTrail()
		{
			if (trail != null)
			{
				trail.enabled = true;
				trail.Clear();
			}
		}

		public void DisableTrail()
		{
			if (trail != null)
			{
				trail.Clear();
				trail.enabled = false;
			}
		}

		private void InitializeCustomWeapon()
		{
			PlayerInventoryMod.instance.AttackEnded.AddListener(DisableTrail);
		}

		public void CreateGameObject()
		{
			try
			{
				obj = GameObject.Instantiate(PlayerInventoryMod.originalPlaneAxeModel, PlayerInventoryMod.originalParrent);

				GameObject trailObject = new GameObject();
				trailObject.transform.SetParent(PlayerInventoryMod.originalParrent, false);
				trailObject.transform.position = obj.transform.position;
				obj.transform.localRotation = PlayerInventoryMod.originalRotation;
				obj.transform.localPosition = PlayerInventoryMod.OriginalOffset;
				obj.transform.Rotate(rotation, Space.Self);
				obj.transform.localPosition += offset;
				obj.transform.localScale = Vector3.one * Scale;
				renderer = obj.GetComponent<Renderer>();
				renderer.material = material;
				obj.GetComponent<MeshFilter>().mesh = mesh;

				trailObject.transform.rotation = obj.transform.rotation;
				trailObject.transform.localPosition = tipPosition + PlayerInventoryMod.OriginalOffset + offset;

				trail = trailObject.AddComponent<TrailRenderer>();

				if (trailMaterial == null)
				{
					trailMaterial = new Material(Shader.Find("Unlit/Transparent"))
					{
						color = new Color(1, 0.5f, 0.25f, 0.4f),
						mainTexture = Texture2D.whiteTexture
					};
				}

				trail.material = trailMaterial;
				trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
				trail.time = 0.15f;
				trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				trail.widthMultiplier = trailWidth;
				Gradient g = new Gradient()
				{
					colorKeys = new GradientColorKey[]
					{
						new GradientColorKey(new Color(0.735849f, 0.1654735f, 0.0798327f),0),
						new GradientColorKey(new Color(1, 0.0654735f, 0.1798327f),1),
					},
					mode = GradientMode.Blend
				};
				trail.colorGradient = g;
				trail.alignment = LineAlignment.Local;
				trail.gameObject.SetActive(false);
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		public void CreateGameObjectNoMeshes()
		{
			try
			{
				GameObject trailObject = new GameObject();
				trailObject.transform.SetParent(PlayerInventoryMod.originalParrent, false);
				trailObject.transform.position = obj.transform.position;
				obj.transform.localRotation = PlayerInventoryMod.originalRotation;
				obj.transform.localPosition = PlayerInventoryMod.OriginalOffset;
				obj.transform.Rotate(rotation, Space.Self);
				obj.transform.localPosition += offset;
				obj.transform.localScale = Vector3.one * Scale;
				Object.Destroy(obj.GetComponent<Renderer>());
				Object.Destroy(obj.GetComponent<MeshFilter>());

				trailObject.transform.rotation = obj.transform.rotation;
				trailObject.transform.localPosition = tipPosition + PlayerInventoryMod.OriginalOffset + offset;

				trail = trailObject.AddComponent<TrailRenderer>();

				if (trailMaterial == null)
				{
					trailMaterial = new Material(Shader.Find("Unlit/Transparent"))
					{
						color = new Color(1, 0.5f, 0.25f, 0.4f),
						mainTexture = Texture2D.whiteTexture
					};
				}

				trail.material = trailMaterial;
				trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
				trail.time = 0.15f;
				trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				trail.widthMultiplier = trailWidth;
				Gradient g = new Gradient()
				{
					colorKeys = new GradientColorKey[]
					{
						new GradientColorKey(new Color(0.735849f, 0.1654735f, 0.0798327f),0),
						new GradientColorKey(new Color(1, 0.0654735f, 0.1798327f),1),
					},
					mode = GradientMode.Blend
				};
				trail.colorGradient = g;
				trail.alignment = LineAlignment.Local;
				trail.gameObject.SetActive(false);
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		public GameObject CreateClientGameObject(Transform clientHand)
		{
			if (obj == null)
			{
				Debug.LogWarning("NO ORIGINAL OBJ TO DUPLICATE");
				return null;
			}
			var otherObject = (GameObject)Object.Instantiate(obj);  //Instantiate - create copy of an object
			otherObject.transform.localRotation = PlayerInventoryMod.originalRotation;
			otherObject.transform.localPosition = PlayerInventoryMod.OriginalOffset;
			otherObject.transform.Rotate(rotation, Space.Self);
			otherObject.transform.localPosition += offset;
			otherObject.transform.localScale = Vector3.one * Scale;
			return otherObject;
		}
	}
}