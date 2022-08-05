using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player.Buffs;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Player
{
	public partial class SpellActions
	{
		SpellAimLine blinkAim;
		public void DoBlinkAim()
		{
			Transform t = Camera.main.transform;
			var hits1 = Physics.RaycastAll(t.position, t.forward, ModdedPlayer.Stats.spell_blinkRange + 1f);
			foreach (var hit in hits1)
			{
				if (!hit.transform.CompareTag("enemyCollide") && hit.transform.root != LocalPlayer.Transform.root)
				{
					instance.blinkAim.UpdatePosition(t.position + Vector3.down * 2, hit.point - t.forward + Vector3.up * 0.25f);
					return;
				}
			}

			blinkAim.UpdatePosition(t.position + Vector3.down * 2, LocalPlayer.Transform.position + t.forward * ModdedPlayer.Stats.spell_blinkRange);
		}

		public void DoBlink()
		{
			blinkAim?.Disable();

			Transform t = Camera.main.transform;
			Vector3 blinkPoint = Vector3.zero;
			var hits1 = Physics.RaycastAll(t.position, t.forward, ModdedPlayer.Stats.spell_blinkRange + 1f);
			foreach (var hit in hits1)
			{
				if (!hit.transform.CompareTag("enemyCollide") && hit.transform.root != LocalPlayer.Transform.root)
				{
					blinkPoint = hit.point - t.forward + Vector3.up * 0.25f;
					break;
				}
			}
			if (blinkPoint == Vector3.zero)
			{
				blinkPoint = LocalPlayer.Transform.position + t.forward * ModdedPlayer.Stats.spell_blinkRange;
			}

			RaycastHit[] hits = Physics.BoxCastAll(t.position, Vector3.one * 1.2f, blinkPoint - t.position, t.rotation, Vector3.Distance(blinkPoint, t.position) + 1);
			foreach (RaycastHit hit in hits)
			{
				if (hit.transform.CompareTag("enemyCollide"))
				{
					float dmg = ModdedPlayer.Stats.spell_blinkDamage + ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_blinkDamageScaling;
					dmg *= ModdedPlayer.Stats.SpellDamageMult;
					if (GameSetup.IsMpClient)
					{
						BoltEntity enemyEntity = hit.transform.GetComponentInParent<BoltEntity>();
						if (enemyEntity == null)
							enemyEntity = hit.transform.gameObject.GetComponent<BoltEntity>();

						if (enemyEntity != null)
						{
							PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(enemyEntity);
							playerHitEnemy.hitFallDown = true;
							playerHitEnemy.getAttackerType = NetworkUtils.CONVERTEDFLOATattackerType;
							playerHitEnemy.Hit = NetworkUtils.FloatToInt(dmg);
							playerHitEnemy.Send();
						}
					}
					else
					{
						if (EnemyManager.enemyByTransform.ContainsKey(hit.transform.root))
						{
							EnemyManager.enemyByTransform[hit.transform.root].HitMagic(dmg);
						}
						else
						{
							hit.transform.SendMessageUpwards("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}

			if (ModdedPlayer.Stats.spell_blinkDoExplosion)
			{
				Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.Boom);
				var raycastHitExplosion = Physics.OverlapSphere(blinkPoint, (blinkPoint - t.position).magnitude / 4f);
				float dmg = ModdedPlayer.Stats.spell_blinkDamage + LocalPlayer.Rigidbody.velocity.magnitude * ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_blinkDamageScaling / 7f;
				dmg *= ModdedPlayer.Stats.SpellDamageMult * ModdedPlayer.Stats.RandomCritDamage;
				foreach (var hitCollider in raycastHitExplosion)
				{
					if (hitCollider.transform.CompareTag("enemyCollide"))
					{
						if (GameSetup.IsMpClient)
						{
							BoltEntity enemyEntity = hitCollider.transform.GetComponentInParent<BoltEntity>();
							if (enemyEntity == null)
								enemyEntity = hitCollider.transform.gameObject.GetComponent<BoltEntity>();

							if (enemyEntity != null)
							{
								PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(enemyEntity);
								playerHitEnemy.hitFallDown = true;
								playerHitEnemy.getAttackerType = NetworkUtils.CONVERTEDFLOATattackerType;
								playerHitEnemy.Hit = NetworkUtils.FloatToInt(dmg);
								playerHitEnemy.Send();
							}
						}
						else
						{
							if (EnemyManager.enemyByTransform.ContainsKey(hitCollider.transform.root))
							{
								EnemyManager.enemyByTransform[hitCollider.transform.root].HitMagic(dmg);
							}
							else
							{
								hitCollider.transform.SendMessageUpwards("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
							}
						}
					}
				}
			}
			BlinkTowards(blinkPoint);
		}

		private void BlinkTowards(Vector3 point)
		{
			Vector3 vel = LocalPlayer.Rigidbody.velocity;
			LocalPlayer.Transform.position = point + Vector3.up;
			vel.y /= 6;
			LocalPlayer.Rigidbody.velocity = vel * 1.5f;
			Effects.Sound_Effects.GlobalSFX.Play(Effects.Sound_Effects.GlobalSFX.SFX.Warp);
			BuffManager.GiveBuff(4, 97, 1, 0.1f);
		}


	}
}
