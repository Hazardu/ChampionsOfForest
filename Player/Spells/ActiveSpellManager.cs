
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bolt;

using ChampionsOfForest.Network;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player.Spells
{
	public class ActiveSpellData
	{
		public float attackDelay;
		public Func<bool> casterFunc, vfxOnlyFunc;
		public delegate void SyncDelegate();
		public SyncDelegate syncFunc;

	}

	public class BlizzardSpell : ActiveSpellData
	{
		static BlizzardSpell instance;
		public static BlizzardSpell Instance
		{
			get
			{
				if (instance == null || instance.fx == null)
					instance = new BlizzardSpell();
				return instance;
			}
		}

		private BlizzardSpell()
		{
			attackDelay = 0f;
			casterFunc = Cast;
			syncFunc = Sync;
			var o = new GameObject();
			fx = o.AddComponent<BlizzardSpellEffect>();
			fx.gameObject.SetActive(false);
		}

		BlizzardSpellEffect fx;
		bool Cast()
		{
			float cost = (85f) * (fx.radius / 10) * ModdedPlayer.Stats.spellCost * Time.deltaTime;
			float costS = cost * ModdedPlayer.Stats.SpellCostToStamina;
			float costE = (cost - costS);
			if (fx.gameObject.activeSelf)
			{
				if (LocalPlayer.Stats.Energy - 5 > costE && LocalPlayer.Stats.Stamina > costS)
				{
					LocalPlayer.Stats.Energy -= costE;
					LocalPlayer.Stats.Stamina -= costS;

					fx.Active();
					return true;
				}
				else
				{
					fx.gameObject.SetActive(false);
				}
			}
			else
			{
				fx.gameObject.SetActive(true);
				return true;
			}


			return false;
		}

		void Sync()
		{

		}
	}
	public class BlizzardSpellEffect : MonoBehaviour
	{
		public float radius;
		ParticleSystem p;
		ParticleSystem.ShapeModule s;
		ParticleSystem.EmissionModule e;
		AudioSource src;
		void Start()
		{
			p = gameObject.AddComponent<ParticleSystem>();
			//p.transform.Rotate(Vector3.right * 120);
			Renderer r = p.GetComponent<Renderer>();
			r.material = ChampionsOfForest.Enemies.EnemyAbilities.SnowAura.ParticleMaterial;

			s = p.shape;
			s.shapeType = ParticleSystemShapeType.Sphere;

			s.radius = radius;

			e = p.emission;
			e.rateOverTime = 350;
			var main = p.main;
			main.startSize = 0.15f;
			main.startSpeed = 0;
			main.gravityModifier = -0.7f;
			main.prewarm = false;
			main.startLifetime = 2;
			main.startColor = Color.gray;
			var vel = p.velocityOverLifetime;
			vel.enabled = true;
			vel.space = ParticleSystemSimulationSpace.World;
			vel.y = new ParticleSystem.MinMaxCurve(3, 0);
			var siz = p.sizeOverLifetime;
			siz.size = new ParticleSystem.MinMaxCurve(2, 0);

			src = gameObject.AddComponent<AudioSource>();
			src.clip = Res.ResourceLoader.instance.LoadedAudio[1015];
			src.loop = true;
			src.volume = 0.0f;
			src.pitch = 1f;
			src.Play();
		}

		public void OnEnable()
		{
			radius = 1f;
			s.radius = radius;
			e.rateOverTime = 30 * radius;
			src.Play();
			StartCoroutine(DealContinousDamage());

		}
		void Update()
		{
			if (!(ActiveSpellManager.Instance.readyToCastSpell && UnityEngine.Input.GetMouseButton(0)))
			{
				gameObject.SetActive(false);
			}
		}

		public void Active()
		{
			radius = Mathf.Clamp(radius += Time.deltaTime, 1, ModdedPlayer.Stats.spell_snowstormMaxCharge);
			s.radius = radius;
			e.rateOverTime = 30 * radius;
			src.pitch = 1 + radius / 12;
			src.volume = 0.1f + radius / 12;
			transform.position = LocalPlayer.Transform.position;
			transform.Rotate(Vector3.up * radius * 50);
		}
		public void OnDisable()
		{
			p.Clear();
			StopAllCoroutines();
		}
		IEnumerator DealContinousDamage()
		{
			while (true)
			{
				yield return new WaitForSeconds(ModdedPlayer.Stats.spell_snowstormHitDelay.Value);
				StartCoroutine(DealDamage());
			}
		}
		IEnumerator DealDamage()
		{
			float dmg = 5 + ModdedPlayer.Stats.spellFlatDmg / 3f;
			dmg *= ModdedPlayer.Stats.SpellDamageMult;
			float crit = ModdedPlayer.Stats.RandomCritDamage;
			dmg *= crit;
			dmg *= radius / 3.33333f;
			dmg *= ModdedPlayer.Stats.spell_snowstormDamageMult;
			var hits = Physics.SphereCastAll(LocalPlayer.Transform.position, radius, Vector3.one, radius, -9);
			int onHitEffectProcs = 0;
			if (GameSetup.IsMpClient)
			{
				for (int i = 0; i < hits.Length; i++)
				{
					if (hits[i].transform.CompareTag("enemyCollide"))
					{
						var entity = hits[i].transform.GetComponentInParent<BoltEntity>();
						if (entity != null)
						{

							var phe = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
							phe.Target = entity;
							phe.getAttackerType = DamageMath.SILENTattackerTypeMagic;
							phe.Hit = DamageMath.GetSendableDamage(dmg);
							phe.Send();
							if (onHitEffectProcs < 6)
							{
								{
									var hitContext = new COTFEvents.HitOtherParams(dmg, crit != 1, entity, this);
									ChampionsOfForest.COTFEvents.Instance.OnHitSpell.Invoke(hitContext);
									ChampionsOfForest.COTFEvents.Instance.OnHitEnemy.Invoke(hitContext);
								}
								ModdedPlayer.instance.OnHit();
								onHitEffectProcs++;
							}
							yield return null;
							EnemyProgression.ReduceArmor(entity, Mathf.CeilToInt(dmg / 100f));
							EnemyProgression.Slow(entity, 144, 0.2f, 0.95f);
							yield return null;
							if (ModdedPlayer.Stats.spell_snowstormPullEnemiesIn)
							{
								if ((hits[i].point - LocalPlayer.Transform.position).sqrMagnitude > 4)
								{
									EnemyProgression.AddKnockbackByDistance(entity.networkId.PackedValue, (LocalPlayer.Transform.position - hits[i].transform.position).normalized, 1);
								}
							}
						}
					}
				}
			}
			else
			{

				for (int i = 0; i < hits.Length; i++)
				{

					if (EnemyManager.enemyByTransform.ContainsKey(hits[i].transform.root))
					{
						EnemyProgression prog = EnemyManager.enemyByTransform[hits[i].transform.root];

						if (prog == null)
							continue;

						prog.HitMagic(dmg);
						prog.Slow(144, 0.2f, 0.85f);
						prog.ReduceArmor(Mathf.CeilToInt(dmg / 100f));
						if (onHitEffectProcs < 6)
						{
							ModdedPlayer.instance.OnHit();
							onHitEffectProcs++;

						}
						{
							var hitContext = new COTFEvents.HitOtherParams(dmg, crit != 1, prog, this);
							ChampionsOfForest.COTFEvents.Instance.OnHitSpell.Invoke(hitContext);
							ChampionsOfForest.COTFEvents.Instance.OnHitEnemy.Invoke(hitContext);
						}
						if (ModdedPlayer.Stats.spell_snowstormPullEnemiesIn)
						{
							if ((hits[i].point - LocalPlayer.Transform.position).sqrMagnitude > 4)
							{
								prog.AddKnockbackByDistance((LocalPlayer.Transform.position - hits[i].transform.position).normalized, 1);
							}
						}
						yield return null;
					}
				}
			}

		}
	}



	public class FireboltSpell : ActiveSpellData
	{
		class FireboltSpellAsyncCaster : MonoBehaviour
		{
			public void Cast(int count)
			{
				StartCoroutine(CastAsync(count));
			}
			public IEnumerator CastAsync(int count)
			{
				Vector3 pos = instance.cam.position + instance.offset;
				Vector3 right = instance.cam.transform.right;
				for (int i = 0; i < count; i++)
				{

					var posOffset = pos;
					if (i > 0)
						posOffset += 0.5f * right * (((i - 1) % 3) - 1);
					float damage = ModdedPlayer.Stats.SpellDamageMult * (ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.spell_fireboltDamageScaling + 20 + ModdedPlayer.instance.FurySwipesDmg);
					if (i > 0)
					{
						damage *= Mathf.Pow(ModdedPlayer.Stats.perk_multishotDamagePennalty, i);
					}
					var o = instance.pool[instance.poolPos];
					o.dmg = damage;
					o.speedf = 70 * ModdedPlayer.Stats.projectileSpeed;
					o.speed = o.speedf * instance.cam.forward;
					o.transform.localScale = Vector3.one * 0.1f * ModdedPlayer.Stats.projectileSize;
					o.transform.position = posOffset;
					o.transform.rotation = Quaternion.LookRotation(instance.cam.forward);
					o.gameObject.SetActive(true);
					instance.poolPos++;
					if (instance.poolPos >= poolSize)
						instance.poolPos = 0;
					yield return null;

				}
			}
		}
		static FireboltSpell instance;
		public static FireboltSpell Instance
		{
			get
			{
				if (instance == null || instance.pool[0] == null)
					instance = new FireboltSpell();
				else
				{
					instance.ResetCam();
					instance.poolPos = 0;
				}
				return instance;
			}
		}

		internal Transform cam;
		internal Vector3 offset;
		internal FireboltProjectile[] pool;
		const int poolSize = 100;
		int poolPos;
		static Material mat;
		FireboltSpellAsyncCaster asyncCaster;
		void ResetCam()
		{
			cam = Camera.main.transform;
		}
		public FireboltSpell()
		{
			attackDelay = 0.38f;
			casterFunc = Cast;
			syncFunc = Sync;
			poolPos = 0;
			offset = Vector3.down * 0.4f;
			ResetCam();
			if (mat == null)
			{
				mat = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
				{
					EmissionColor = new Color(1, 0.6f, 0.2f),
					Metalic = 0,
					Smoothness = 0.8f
				});
			}
			var o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			o.AddComponent<Rigidbody>().useGravity = false;
			var c = o.GetComponent<SphereCollider>();
			c.isTrigger = true;
			c.radius = 4;
			var r = o.GetComponent<MeshRenderer>();
			r.material = mat;
			r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			var l = o.AddComponent<Light>();
			l.color = new Color(1, 0.6f, 0.2f);
			l.type = LightType.Point;
			l.intensity = 1;
			l.range = 15f;
			o.transform.localScale = Vector3.one * 0.1f;
			var f = o.AddComponent<FireboltProjectile>();
			o.SetActive(false);
			pool = new FireboltProjectile[poolSize];
			for (int i = 0; i < poolSize; i++)
			{
				pool[i] = UnityEngine.Object.Instantiate(f);
				pool[i].gameObject.SetActive(false);
			}

			asyncCaster = new GameObject().AddComponent<FireboltSpellAsyncCaster>();



		}
		public bool Cast()
		{
			int repeats = ModdedPlayer.RangedRepetitions();
			bool b = false;
			Vector3 pos = cam.position + offset;
			Vector3 right = cam.transform.right;
			float costTotal = 0;
			for (int i = 0; i < repeats; i++)
			{
				float cost = costTotal + ModdedPlayer.Stats.spell_fireboltEnergyCost * ModdedPlayer.Stats.spellCost * (i > 0 ? Mathf.Pow(ModdedPlayer.Stats.perk_multishotDamagePennalty, i) : 1);
				float costS = cost * ModdedPlayer.Stats.SpellCostToStamina;
				if (LocalPlayer.Stats.Stamina > costS)
				{
					float costE = cost - costS;
					if (LocalPlayer.Stats.Energy > costE)                    //good to go
						costTotal = cost;
					else
					{
						repeats = i;
						break;
					}
				}
				else
				{
					repeats = i;
					break;
				}
			}
			if (repeats >= 1)
			{

				float costS = costTotal * ModdedPlayer.Stats.SpellCostToStamina;
				float costE = costTotal - costS;

				LocalPlayer.Stats.Energy -= costE;
				LocalPlayer.Stats.Stamina -= costS;

				asyncCaster.Cast(repeats);
				return true;
			}
			return false;
		}
		public void Sync()
		{

		}
	}
	public class FireboltProjectile : MonoBehaviour
	{
		public Vector3 speed;
		public float speedf;
		public float dmg;
		public float dieTimestamp;
		public int pierceCount;
		void OnEnable()
		{
			dieTimestamp = Time.time + 15;
			pierceCount = 0;
		}

		void Update()
		{
			if (Time.time > dieTimestamp)
				gameObject.SetActive(false);
			if (ModdedPlayer.Stats.spell_seekingArrow)
			{
				if (Time.time - ModdedPlayer.Stats.spell_seekingArrowDuration > SpellActions.SeekingArrow_TimeStamp)
				{
					ModdedPlayer.Stats.spell_seekingArrow.value = false;
				}
			}
			transform.position += Time.deltaTime * speed;

			if (ModdedPlayer.Stats.spell_seekingArrow)
			{
				if ((transform.position - SpellActions.SeekingArrow_Target.position).sqrMagnitude > 3)
				{
					Vector3 targetvel = SpellActions.SeekingArrow_Target.position - transform.position;
					targetvel.Normalize();
					targetvel *= speedf;
					speed = Vector3.RotateTowards(speed, targetvel, Time.fixedDeltaTime * 2f * ModdedPlayer.Stats.projectileSpeed, 0.2f);
				}
			}
		}

		void OnTriggerEnter(Collider other)
		{
			try
			{


				if (other.CompareTag("enemyCollide") || other.CompareTag("enemyRoot"))
				{
					if (SpellActions.SeekingArrow_ChangeTargetOnHit)
					{
						SpellActions.SetSeekingArrowTarget(other.transform);
					}

					var crit = ModdedPlayer.Stats.RandomCritDamage;
					var dmgOutput = dmg * crit;

					ModdedPlayer.instance.OnHit();
					if (GameSetup.IsMpClient)
					{
						var entity = other.GetComponentInParent<BoltEntity>();
						if (entity != null)
						{
							{
								var hitContext = new COTFEvents.HitOtherParams(dmgOutput, crit != 1, entity, this);
								ChampionsOfForest.COTFEvents.Instance.OnHitSpell.Invoke(hitContext);
								ChampionsOfForest.COTFEvents.Instance.OnHitEnemy.Invoke(hitContext);
							}
							var phe = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
							phe.Target = entity;
							phe.getAttackerType = DamageMath.SILENTattackerTypeMagic;
							phe.Hit = DamageMath.GetSendableDamage(dmgOutput);
							if (crit > 1)
							{
								int myID = 3000 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
								float fireDmg = 1 + ModdedPlayer.Stats.spellFlatDmg / 3;
								fireDmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
								fireDmg *= ModdedPlayer.Stats.fireDamage + 1;
								using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
								{
									using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
									{
										w.Write(27);
										w.Write(entity.networkId.PackedValue);
										w.Write(fireDmg);
										w.Write(15);
										w.Write(myID);
										w.Close();
									}
									AsyncHit.SendCommandDelayed(2, answerStream.ToArray(), NetworkManager.Target.OnlyServer);
									answerStream.Close();
								}
								phe.Burn = true;
							}
							phe.Send();
						}
					}
					else        //is singleplayer or host
					{
						if (EnemyManager.enemyByTransform.ContainsKey(other.transform.root))
						{
							var progression = EnemyManager.enemyByTransform[other.transform.root];
							{
								var hitContext = new COTFEvents.HitOtherParams(dmgOutput, crit != 1, progression, this);
								ChampionsOfForest.COTFEvents.Instance.OnHitSpell.Invoke(hitContext);
								ChampionsOfForest.COTFEvents.Instance.OnHitEnemy.Invoke(hitContext);
							}
							progression.HitMagic(dmgOutput);
							if (crit > 1)
							{
								if (ModdedPlayer.Stats.perk_fireDmgIncreaseOnHit)
								{
									//int myID = 3000 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
									float fireDmg = 1 + ModdedPlayer.Stats.spellFlatDmg / 3;
									fireDmg *= ModdedPlayer.Stats.TotalMagicDamageMultiplier;
									fireDmg *= ModdedPlayer.Stats.fireDamage + 1;
									progression.FireDebuff(3000, fireDmg, 14);

								}
								progression.HealthScript.Burn();
							}
						}
						else
						{
							Debug.LogWarning("Enemy not found");
						}
					}
					if (UnityEngine.Random.value < ModdedPlayer.Stats.projectilePierceChance - pierceCount)
					{
						pierceCount++;
					}
					else
					{
						dieTimestamp = 0;
					}
				}
			}
			catch (Exception exc)
			{

				Debug.LogWarning(exc.ToString());
			}
		}
	}

	public class ActiveSpellManager : MonoBehaviour
	{

		public static ActiveSpellManager Instance;
		void Start()
		{
			Instance = this;

		}

		ActiveSpellData currentSpell;
		public bool readyToCastSpell;
		float cooldown = 0;
		public void OnWeaponEquipped()
		{
			readyToCastSpell = false;
		}

		public void PrepareForFiring(ActiveSpellData spell)
		{
			LocalPlayer.Inventory.UnequipItemAtSlot(TheForest.Items.Item.EquipmentSlot.RightHand, false, true, true);
			currentSpell = spell;
			readyToCastSpell = true;
		}

		void Update()
		{
			if (cooldown > 0)
				cooldown -= Time.deltaTime * ModdedPlayer.Stats.attackSpeed;
			if (readyToCastSpell && !LocalPlayer.FpCharacter.MovementLocked && cooldown <= 0)
			{
				//check for mouse input
				if (UnityEngine.Input.GetMouseButton(0))
				{
					if (currentSpell.casterFunc())
					{
						cooldown = currentSpell.attackDelay;


						if (GameSetup.IsMultiplayer)
						{
							currentSpell.syncFunc();
						}
					}
				}
			}
		}

	}
}
