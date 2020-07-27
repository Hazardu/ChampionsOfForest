using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class TrapSphereSpell : MonoBehaviour
	{
		public float Duration;
		public float Radius;
		private float Lifetime;
		private readonly float rotSpeed = 30f;
		private bool coughtPlayer = false;
		private Transform playerTransform;
		public static GameObject Prefab;
		private static Material mat;

		public static void Create(Vector3 pos, float radius, float duration)
		{
			if (Prefab == null)
			{
				try
				{
					Prefab = new GameObject("TrapSphere");
					Prefab.AddComponent<MeshFilter>().mesh = Res.ResourceLoader.instance.LoadedMeshes[68];
					Prefab.transform.localScale = Vector3.one * radius;
					MeshRenderer r = Prefab.AddComponent<MeshRenderer>();
					if (mat == null)
						mat = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData() { MainColor = new Color(1, 0.83f, 0, 0.2f), Metalic = 0f, Smoothness = 0f, renderMode = BuilderCore.BuildingData.RenderMode.Fade });
					r.material = mat;
				}
				catch (System.Exception ex)
				{
					ModAPI.Log.Write(ex.ToString());
				}
			}
			else
			{
			}
			Prefab.SetActive(true);

			GameObject go = GameObject.Instantiate(Prefab, pos + Vector3.up, Quaternion.identity);
			TrapSphereSpell s = go.AddComponent<TrapSphereSpell>();
			s.Radius = radius;
			s.Duration = duration;
			s.Lifetime = 0;
			go.transform.localScale = Vector3.one * radius;

			Prefab.SetActive(false);
		}

		private void Update()
		{
			transform.Rotate((transform.forward + transform.up + Vector3.right) * rotSpeed * Time.deltaTime);
			Lifetime += Time.deltaTime;
			if (coughtPlayer && 0 == ModdedPlayer.Stats.stunImmunity && playerTransform != null)
			{
				var mag = (playerTransform.position - transform.position).sqrMagnitude;
				if (mag > Radius * Radius)
				{
					playerTransform.position = Vector3.MoveTowards(playerTransform.position, transform.position, mag / 2 * Time.deltaTime);
					Player.BuffDB.AddBuff(5, 71, 0.6f, 5);
					Player.BuffDB.AddBuff(10, 72, 0.6f, 5);
				}
			}
			else
			{
				coughtPlayer = false;
			}
			if (Lifetime < Duration)
			{
				if ((LocalPlayer.Transform.root.position - transform.position).sqrMagnitude < Radius * Radius - 2)
				{
					coughtPlayer = true;
					playerTransform = LocalPlayer.Transform.root;
				}
			}
			else
			{
				transform.localScale -= Vector3.one * Time.deltaTime * 2;
				Radius -= Time.deltaTime * 2;
				if (transform.localScale.x < 0.002f)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}