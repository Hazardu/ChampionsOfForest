using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class Taunt
	{
		public static float spellradius = 30f;
		public static float spellduration = 10f;
		private static SpellAimSphere aimSphere;

		public static void AimEnd()
		{
			aimSphere?.Disable();
		}

		public static void Aim()
		{
			if (aimSphere == null)
			{
				aimSphere = new SpellAimSphere(new Color(1f, 0.1f, 0.15f, 0.5f), spellradius);
			}
			Transform t = Camera.main.transform;

			Vector3 point = Vector3.zero;
			var hits1 = Physics.RaycastAll(t.position, t.forward, 300f);
			foreach (var hit in hits1)
			{
				if (hit.transform.root != LocalPlayer.Transform.root)
				{
					point = hit.point + Vector3.up * 2f;
					break;
				}
			}
			if (point == Vector3.zero)
			{
				point = LocalPlayer.Transform.position + t.forward * 300;
			}
			aimSphere.SetRadius(spellradius);
			aimSphere.UpdatePosition(point);
		}

		public static void OnSpellUsed()
		{
			Transform t = Camera.main.transform;

			Vector3 point = Vector3.zero;
			var hits1 = Physics.RaycastAll(t.position, t.forward, 300f);
			foreach (var hit in hits1)
			{
				if (hit.transform.root != LocalPlayer.Transform.root)
				{
					point = hit.point;
					break;
				}
			}
			if (point == Vector3.zero)
			{
				point = LocalPlayer.Transform.position;
			}
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(3);
						w.Write(15);
						w.Write(point.x);
						w.Write(point.y);
						w.Write(point.z);
						w.Write(spellradius);
						if (GameSetup.IsMpClient)
						{
							w.Write(spellduration);
							w.Write(ModdedPlayer.Stats.spell_taunt_speedChange);
							w.Write(ModdedPlayer.Stats.spell_taunt_pullEnemiesIn);
							w.Write(ModReferences.ThisPlayerID);
						}
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
					answerStream.Close();
				}
				if (GameSetup.IsMpServer)
				{
					Cast(point, spellradius, LocalPlayer.GameObject, spellduration, ModdedPlayer.Stats.spell_taunt_speedChange,ModdedPlayer.Stats.spell_taunt_pullEnemiesIn.GetAmount());
				}
				CastEffect(point, spellradius);
			}
			else
			{
				Cast(point, spellradius, LocalPlayer.GameObject, spellduration, ModdedPlayer.Stats.spell_taunt_speedChange, ModdedPlayer.Stats.spell_taunt_pullEnemiesIn.GetAmount());
				CastEffect(point, spellradius);

			}
		}

		public static void Cast(in Vector3 pos, in float radius, GameObject player, in float duration, in float slow, bool pullIn)
		{
			float sqrRad = radius * radius;
			if (BoltNetwork.isRunning)
			{
				Debug.Log("Multiplayer Cast");
				foreach (var enemy in EnemyManager.hostDictionary.Values)
				{
					if (enemy == null)
						continue;
					if ((enemy.transform.position - pos).sqrMagnitude <= sqrRad)
					{
						if (pullIn)
						{
							enemy.AddKnockbackByDistance(pos - enemy.transform.position, Vector3.Distance(pos, enemy.transform.position) * 2);
							enemy.Taunt(player, duration, slow/2f);

						}
						else
						{
							enemy.Taunt(player, duration, slow);
						
						}
						Debug.Log("Taunted " + enemy.enemyName);
					}
				}
			}
			else
			{
				Debug.Log("Singleplayer Cast");

				foreach (var enemy in EnemyManager.enemyByTransform)
				{
					try
					{
					if ((enemy.Key.position - pos).sqrMagnitude <= sqrRad)
					{
						if (pullIn)
						{
								enemy.Value.AddKnockbackByDistance(pos - enemy.Key.position, Vector3.Distance(pos, enemy.Key.transform.position) / 1.4f);
							}
							enemy.Value.Taunt(player, duration,slow);
						Debug.Log("Taunted " + enemy.Value.enemyName);

					}
					}
					catch (System.Exception e)
					{

						Debug.LogWarning("Error:\n\n" + enemy.ToString() + "\n" + e.ToString());
					}
					
				}
			}
		}

		private static Material particleMaterial;

		public static void CastEffect(Vector3 pos, float radius)
		{
			GameObject o = new GameObject("__SHOUTPARTICLES__");

			o.transform.position = pos;
			o.transform.rotation = Quaternion.Euler(90, 0, 0);
			o.transform.localScale = Vector3.one * radius / 50;

			GameObject.Destroy(o, 1);
			ParticleSystem ps = o.AddComponent<ParticleSystem>();
			ParticleSystem.MainModule main = ps.main;
			ParticleSystem.EmissionModule emission = ps.emission;
			ParticleSystem.ShapeModule shape = ps.shape;
			ParticleSystem.LimitVelocityOverLifetimeModule limit = ps.limitVelocityOverLifetime;
			ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();

			main.loop = false;
			main.duration = 0.5f;
			main.startLifetime = 0.5f;
			main.startSpeed = 150;
			main.startSize = 3;
			main.startColor = new Color(1, 0, 0, 0.69f);
			main.gravityModifier = -1;

			emission.rateOverTime = 2000;

			shape.shapeType = ParticleSystemShapeType.Circle;

			limit.dampen = 0.2f;
			limit.limit = 1;

			if (particleMaterial == null)
			{
				particleMaterial = new Material(Shader.Find("Particles/Additive"))
				{
					mainTexture = Res.ResourceLoader.GetTexture(111),
					mainTextureScale = new Vector2(10, 1)
				};

				particleMaterial.SetColor("_TintColor", new Color(0.7f, 0.5f, 0f, 0.6f));
			}
			rend.material = particleMaterial;
			rend.renderMode = ParticleSystemRenderMode.Stretch;
			rend.lengthScale = 2f;
		}
	}
}