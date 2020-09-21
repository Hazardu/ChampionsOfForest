using System;
using System.Collections;
using System.Linq;

using ChampionsOfForest.Network;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class TheFartCreator : MonoBehaviour
	{
		private static GameObject prefabStanding;
		private static GameObject prefabJumping;
		private static TheFartCreator instance;

		private void Start()
		{
			try
			{
				instance = this;
				if (prefabStanding == null)
				{
					var bundle = Res.ResourceLoader.GetAssetBundle(2005);
					prefabStanding = (GameObject)bundle.LoadAssetWithSubAssets("assets/roaringfx.prefab")[0];
					prefabJumping = (GameObject)bundle.LoadAssetWithSubAssets("assets/roaring jump.prefab").Where(x => x.name == "roaring jump").First();
					ModAPI.Log.Write("prefabJumping = " + prefabJumping.name);
					var renderers = prefabStanding.GetComponentsInChildren<Renderer>();
					foreach (var item in renderers)
					{
						ModAPI.Console.Write(item.material.shader.name);
					}
				}
			}
			catch (Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		public static void FartWarmup(float radius, float dmg, float knockback, float slowAmount, float duration)
		{
			instance.StartCoroutine(instance.FartWarmupAsync(radius, dmg, knockback, slowAmount, duration));
		}

		public static void CreateEffect(Vector3 pos, Vector3 dir, bool useJumpVariation)
		{
			if (useJumpVariation)
			{
				var go = Instantiate(prefabJumping, pos, Quaternion.LookRotation(dir));
				Destroy(go, 12f);
			}
			else
			{
				var go = Instantiate(prefabStanding, pos, Quaternion.LookRotation(dir));
				ModAPI.Console.Write("Created fart " + go.name);
				Destroy(go, 15f);
			}
		}

		public static void DealDamageAsHost(Vector3 pos, Vector3 dir, float radius, float dmg, float knockback, float slowAmount, float duration)
		{
			instance.StartCoroutine(instance.AsyncHitEnemies(pos, dir, radius, dmg, knockback, slowAmount, duration));
		}

		private IEnumerator FartWarmupAsync(float radius, float dmg, float knockback, float slowAmount, float duration)
		{
			ModAPI.Console.Write("1");
			PlayAudio(LocalPlayer.Transform.position);
			yield return new WaitForSeconds(7.17f);
			var origin = LocalPlayer.Transform.position;
			var back = -LocalPlayer.Transform.forward;
			ModAPI.Console.Write("2");

			var obj = Instantiate(prefabStanding, origin, Quaternion.LookRotation(back));
			LocalPlayer.Rigidbody.AddForce((-back * 2 + Vector3.up) * 5, ForceMode.VelocityChange);
			if (GameSetup.IsMultiplayer)
			{
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				System.IO.BinaryWriter writer = new System.IO.BinaryWriter(memoryStream);
				writer.Write(3);
				writer.Write(14);
				writer.Write(true);
				writer.Write(origin.x);
				writer.Write(origin.y);
				writer.Write(origin.z);
				writer.Write(back.x);
				writer.Write(back.y);
				writer.Write(back.z);
				writer.Write(radius);
				writer.Write(dmg);
				writer.Write(knockback);
				writer.Write(slowAmount);
				writer.Write(duration);
				writer.Close();
				NetworkManager.SendLine(memoryStream.ToArray(), NetworkManager.Target.Others);
				memoryStream.Close();
			}
			yield return null;
			if (!GameSetup.IsMpClient)
			{
				Effects.TheFartCreator.DealDamageAsHost(origin, back, radius, dmg, knockback, slowAmount, duration);
			}
			else
			{
			}
		}

		public static void PlayAudio(Vector3 pos)
		{
			var src = new GameObject().AddComponent<AudioSource>();
			src.clip = Res.ResourceLoader.instance.LoadedAudio[1017];
			src.transform.position = pos;
			src.loop = false;
			src.Play();
		}

		private IEnumerator AsyncHitEnemies(Vector3 pos, Vector3 dir, float radius, float dmg, float knockback, float slowAmount, float duration)
		{
			int scanCount = 0;
			Vector3 center = pos + dir * radius;
			float radsqr = radius * radius;
			
				foreach (var enemy in EnemyManager.enemyByTransform)
				{
					if (!enemy.Key.gameObject.activeSelf)
						continue;
					if ((scanCount++) % 10 == 0)
						yield return null;
					float sqrDist = (center - enemy.Key.position).sqrMagnitude;
					if (sqrDist <= radsqr)
					{
						enemy.Value.HitMagic(dmg * 2);
						enemy.Value.DoDoT(dmg, duration);
						enemy.Value.Slow(142, slowAmount, duration);
						enemy.Value.AddKnockbackByDistance(dir, knockback);
					}
				
			}
		}
	}
}