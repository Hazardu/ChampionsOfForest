using System;
using System.Collections.Generic;

using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Localization;

using UnityEngine;
using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
	public partial class EnemyProgression : MonoBehaviour
	{
		private void Setup()
		{
			try
			{
				if (BoltNetwork.isRunning)
				{
					if (entity == null)
					{
						entity = transform.root.GetComponentInChildren<BoltEntity>();
					}
					if (entity == null)
					{
						entity = HealthScript.entity;
					}
					if (entity == null)
					{
						entity = transform.root.GetComponent<BoltEntity>();
					}
					EnemyManager.AddHostEnemy(this);
				}
				if (!EnemyManager.enemyByTransform.ContainsKey(transform))
					EnemyManager.enemyByTransform.Add(this.transform, this);
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}

			//Assiging base health, if new enemy
			if (baseHealth == 0)
			{
				baseHealth = HealthScript.Health;
			}
			else    //reverting health to base health if reused enemy (in case enemies are pooled)
			{
				HealthScript.Health = baseHealth;
			}

			StopAllCoroutines();

			Steadfast = 100;
			slows = new Dictionary<int, EnemyDebuff>();
			dmgTakenDebuffs = new Dictionary<int, EnemyDebuff>();
			dmgDealtDebuffs = new Dictionary<int, EnemyDebuff>();
			FireDamageDebuffs = new Dictionary<int, EnemyDebuff>();
			DamageOverTimeList = new List<DoT>();
			abilities = new List<Abilities>();

			bool isElite = (Random.value < 0.1 || (AIScript.creepy_boss && !AIScript.girlFullyTransformed) || ModSettings.difficulty == ModSettings.Difficulty.Hell) && ModSettings.AllowElites;
			SetType(ref isElite);


			//picking abilities
			if (isElite)
			{
				int abilityAmount = (int)ModSettings.difficulty > (int)ModSettings.Difficulty.Veteran ? Random.Range(3,   7) : 2;
				if (AIScript.creepy_boss)
				{
					abilityAmount = 10;
				}   //Megan boss always has abilities, a lot of them.

				int i = 0;
				Array abilityArray = Enum.GetValues(typeof(Abilities));

				//Determining if enemy is Elite, Miniboss or Boss type of enemy
				if (abilityAmount > 6 && Random.value < 0.1f)
				{
					_rarity = EnemyRarity.Boss;
					abilities.Add(Abilities.BossSteadfast);
				}
				else if (abilityAmount > 4 && Random.value < 0.25f)
				{
					abilities.Add(Abilities.EliteSteadfast);
					_rarity = EnemyRarity.Miniboss;
				}
				else
				{
					_rarity = EnemyRarity.Elite;
				}

				if (AIScript.creepy_boss && !AIScript.girlFullyTransformed)    //Force adding BossSteadfast to Megan
				{
					abilities.Clear();
					_rarity = EnemyRarity.Boss;
					abilities.Add(Abilities.BossSteadfast);
				}

				//Trial and error method of picking abilities
				while (i < abilityAmount)
				{
					bool canAdd = true;
					Abilities ab = (Abilities)abilityArray.GetValue(Random.Range(0, abilityArray.Length));
					if (ab == Abilities.Steadfast || ab == Abilities.EliteSteadfast || ab == Abilities.BossSteadfast)
					{
						if (abilities.Contains(Abilities.Steadfast) || abilities.Contains(Abilities.EliteSteadfast) || abilities.Contains(Abilities.BossSteadfast))
						{
							canAdd = false;
						}
					}
					else if (ab == Abilities.Tiny || ab == Abilities.Gargantuan)
					{
						if (AIScript.creepy_boss && ab == Abilities.Tiny)
						{
							canAdd = false;
						}
						else if (abilities.Contains(Abilities.Gargantuan) || abilities.Contains(Abilities.Tiny))
						{
							canAdd = false;
						}
					}
					else if (ab == Abilities.Undead && !(AIScript.creepy || AIScript.creepy_fat))
					{
						canAdd = false;
					}
					else if (ab == Abilities.ArcaneCataclysm || ab == Abilities.BlackHole || ab == Abilities.FireCataclysm || ab == Abilities.Meteor)
					{
						if ((int)ModSettings.difficulty < (int)ModSettings.Difficulty.Master)
							canAdd = false;
					}
					if (abilities.Contains(ab))
					{
						canAdd = false;
					}
					if (canAdd)
					{
						i++;
						abilities.Add(ab);
					}
				}
			}
			else
			{
				_rarity = EnemyRarity.Normal;
			}

			SetLevel();
			RollName(isElite);
			setupDifficulty = ModSettings.difficulty;
			//Assigning rest of stats
			int dif = (int)setupDifficulty;
			DamageMult = level < 65 ? 
				(Mathf.Pow(2.7182818284f,level/6) + level) //e^(x/6) + x
				: Mathf.Pow(level-60, 2.60f) * 600;
			armor = Mathf.FloorToInt(Random.Range(Mathf.Pow(level, 2.43f) * 0.3333f + 1, Mathf.Pow(level, 2.43f) * 0.66666f + 20) * ModSettings.EnemyArmorMultiplier);
			armor *= dif / 2 + 1;
			armorReduction = 0;

			//Set HP;
			{
				double hp = Math.Sqrt(HealthScript.Health) + 45.0;
				hp *= Math.Pow(level, 2.38);
				hp *= 0.2;
				hp *= Math.Pow(2.0, ((double)level - 50.0) / 32.0);
				extraHealth = hp;
			}

			AnimSpeed = 0.94f + (float)level / 170;
			HealthScript.Health = int.MaxValue;

			extraHealth *= ModSettings.EnemyHealthMultiplier;
			DamageMult *= ModSettings.EnemyDamageMultiplier;
			AnimSpeed *= ModSettings.EnemySpeedMultiplier;

			if (_rarity != EnemyRarity.Normal)
			{
				armor *= 2;
			}
			//Extra health for boss type enemies
			switch (_rarity)
			{
				case EnemyRarity.Boss:
					extraHealth *= 2;
					if (!abilities.Contains(Abilities.Tiny))
						gameObject.transform.localScale *= 1.2f;
					break;

				case EnemyRarity.Miniboss:
					extraHealth *= 10;
					if (!abilities.Contains(Abilities.Tiny))
						gameObject.transform.localScale *= 1.15f;
					break;

				case EnemyRarity.Elite:
					extraHealth *= 5;
					if (!abilities.Contains(Abilities.Tiny))
						gameObject.transform.localScale *= 1.1f;
					break;
				default:
					break;

			}

			
			//Applying some abilities
			if (abilities.Contains(Abilities.Gargantuan))
			{
				armor *= 3;
				extraHealth *= 2.5f;
				DamageMult *= 2.5f;
				AnimSpeed /= 1.5f;
				BroadcastMessage("SetTriggerScale", 2.5f, SendMessageOptions.DontRequireReceiver);

			}
			else if (abilities.Contains(Abilities.Tiny))
			{
				AnimSpeed *= 1.1f;
				DamageMult *= 1.2f;
				BroadcastMessage("SetTriggerScale", 5f, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				BroadcastMessage("SetTriggerScale", 1.6f, SendMessageOptions.DontRequireReceiver);

			}
			if (abilities.Contains(Abilities.Steadfast))
			{
				Steadfast = 6;
				armor *= 2;
			}
			if (abilities.Contains(Abilities.EliteSteadfast))
			{
				Steadfast = 4f;
				armor *= 2;
			}
			if (abilities.Contains(Abilities.BossSteadfast))
			{
				Steadfast = 1f;
				armor *= 2;
			}
			if (abilities.Contains(Abilities.ExtraHealth))
			{
				extraHealth *= 4;
			}
			if (abilities.Contains(Abilities.ExtraDamage))
			{
				DamageMult *= 5f;
			}
			if (abilities.Contains(Abilities.RainEmpowerment))
			{
				prerainDmg = DamageMult;
				prerainArmor = armor;
			}
			if (abilities.Contains(Abilities.Juggernaut))
			{
				CCimmune = true;
				AnimSpeed /= 1.15f;
			}
			if (abilities.Contains(Abilities.Avenger))
			{
				if (avengerability == null)
					avengerability = gameObject.AddComponent<Avenger>();
				avengerability.progression = this;
				avengerability.Stacks = 0;
			}
			else if (avengerability != null)
			{
				Destroy(avengerability);
			}
			if (abilities.Contains(Abilities.Radiance))
			{
				float aurDmg = DamageMult/20;
				FireAura.Cast(gameObject, aurDmg);
				if (BoltNetwork.isRunning)
				{
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(8);
							w.Write(2);
							w.Write(entity.networkId.PackedValue);
							w.Write(aurDmg);
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
						answerStream.Close();
					}
				}

				InvokeRepeating("SendFireAura", 20, 30);
			}
			else
			{
				var aura = gameObject.GetComponent<FireAura>();
				if (aura != null)
				{
					Destroy(aura);
				}
			}
			//Clamping Health
			try
			{
				maxHealth = extraHealth;
				HealthScript.maxHealthFloat = (float)maxHealth;
				HealthScript.Health =(int) Mathf.Min(HealthScript.maxHealthFloat, int.MaxValue);
				extraHealth = Math.Max(0,maxHealth - HealthScript.Health);
				ClampHealth();
				armor = Mathf.Min(armor, int.MaxValue - 5);
				if (armor < 0)
					armor = int.MaxValue;
				DebuffDmgMult = 1;
				DualLifeSpend = false;
				setupComplete = true;
				OnDieCalled = false;
				BaseDamageMult = Mathf.Abs(DamageMult);
				BaseAnimSpeed = AnimSpeed;
				knockbackSpeed = 0;
			}
			catch (Exception e)
			{
				CotfUtils.Log(e.Message);
			}

			{
				double b = Math.Pow(maxHealth, 0.8) * 0.8* Math.Pow(level, 0.25) + 100.0;
				if (abilities.Count > 1)
				{
					b *= abilities.Count;
				}
				b *= Math.Max(1.0, (double)level / 20.0);
				b *= ModSettings.ExpMultiplier;
				bounty = Convert.ToInt64(b);
			}


			steadfastCap = (float)Math.Max(Steadfast * 0.01 * maxHealth, 1.0);
			if (steadfastCap < 1) // Making sure the enemy can be killed
			{
				steadfastCap = 1;
			}

			CreationTime = Time.time;

			if (BoltNetwork.isRunning)
			{
				ulong id = entity.networkId.PackedValue;
				color = normalColor;
				SyncAppearance(id);
			}
		}



		public static string[] fNames = new string[] { "Lizz", "Wolfskull", "Wiktoria",
					"Emma", "Isabella", "Sophia", "Mia", "Evelyn","Emily", "Elizabeth", "Sofia",
					"Victoria", "Chloe", "Camila", "Layla", "Lillian", "Lily",
					"Natalie","Lucy", "Anna",
					"Maya", "Alice",
					"Hailey", "Eva", "Emilia", "Quinn", "Piper", "Kaylee",
					"Isla", "Katherine", "Jaiden", "Maria", "Taylor Swift", "Natalia",
					"Annabelle", "Alexandra", "Athena", "Andrea", "Jasmine", "Lyla", "Margaret", "Alyssa",
					 "Alexis","Sophie Francis","Albedo","Hazardina","Kaspita", "Ruby",
					//my pet names
					"Lara", "Misty"
		};
		public static string[] mNames = new string[]
				  {
				  "Farket","Hazard","Moritz","Souldrinker","Olivier Broadbent","Subscribe to Pewds","Kutie","Axt","Fionera","Cleetus","Hellsing","Metamoth","Teledorktronics","SmokeyBear","NightOwl","PuffedRice","PhoenixDeath","Danny Parker","Kaspito","Chefen","Lauren","DrowsyCob","Ali chipmunk","Malкae","R3iGN","Torsin","Иio","The Strange Man","MiikaHD","NÜT","Azpect","Chad","Cheddar","MaddVladd","Wren","Ross Draws","Sam Gorski","Niko Pueringer","Freddy Wong","PewDiePie","Salad Ass","Unlike Pluto","Sora","Kuldar","Fon","Sigmar","Mohammed","Cyde","MaximumAsp79","Sebastian","David","John Deere","Isaac","Adolf Hitler","Joseph Stalin","Eraized","Punny", "Aezyn", "Infernal", "PorkyBunBuns","Edgar","Twomad", "Seth Everman", "Pols", "Tristam", "Siewca", "Bowser", "Choppa", "RatsForLife", "Falun", "Orichalcos", "Dirty Dan", "Lunsrea"
				  };

		private void RollName(bool isElite)
		{
			if (AIScript.creepy_boss)
			{
				enemyName = "Megan Cross";
				return;
			}
			if (!isElite)
			{
				switch (enemyType)
				{
					case Enemy.RegularArmsy:
						enemyName = Translations.EnemyProgression_Setup_1; //tr
						return;
					case Enemy.PaleArmsy:
						enemyName = Translations.EnemyProgression_Setup_2; //tr
						return;
					case Enemy.RegularVags:
						enemyName = Translations.EnemyProgression_Setup_3; //tr
						return;
					case Enemy.PaleVags:
						enemyName = Translations.EnemyProgression_Setup_4; //tr
						return;
					case Enemy.Cowman:
						enemyName = Translations.EnemyProgression_Setup_5; //tr
						return;
					case Enemy.Baby:
						enemyName = Translations.EnemyProgression_Setup_6; //tr
						return;
					case Enemy.Worm:
						enemyName = Translations.EnemyProgression_Setup_6; //tr
						return;
					case Enemy.NormalMale:
						enemyName = Translations.EnemyProgression_Setup_7; //tr
						return;
					case Enemy.NormalLeaderMale:
						enemyName = Translations.EnemyProgression_Setup_8; //tr
						return;
					case Enemy.NormalFemale:
						enemyName = Translations.EnemyProgression_Setup_9; //tr
						return;
					case Enemy.NormalSkinnyMale:
						enemyName = Translations.EnemyProgression_Setup_10; //tr
						return;
					case Enemy.NormalSkinnyFemale:
						enemyName = Translations.EnemyProgression_Setup_10; //tr
						return;
					case Enemy.PaleMale:
						enemyName = Translations.EnemyProgression_Setup_11; //tr
						return;
					case Enemy.PaleSkinnyMale:
						enemyName = Translations.EnemyProgression_Setup_12; //tr
						return;
					case Enemy.PaleSkinnedMale:
						enemyName = Translations.EnemyProgression_Setup_13; //tr
						return;
					case Enemy.PaleSkinnedSkinnyMale:
						enemyName = Translations.EnemyProgression_Setup_14; //tr
						return;
					case Enemy.PaintedMale:
						enemyName = Translations.EnemyProgression_Setup_15; //tr
						return;
					case Enemy.PaintedLeaderMale:
						enemyName = Translations.EnemyProgression_Setup_16; //tr
						return;
					case Enemy.PaintedFemale:
						enemyName = Translations.EnemyProgression_Setup_17; //tr
						return;
					case Enemy.Fireman:
						enemyName = Translations.EnemyProgression_Setup_18; //tr
						return;
				}
			}
			List<string> names = new List<string>();
			string prefix = "";
			if (AIScript.female || AIScript.creepy || AIScript.femaleSkinny)    //is female
			{
				names.AddRange(fNames);
			}
			else                                                 // is male
			{
				names.AddRange(mNames);
			}
			enemyName = prefix + names[Random.Range(0, names.Count)];
		}
		private void SetType(ref bool IsElite)
		{
			if (AIScript.creepy && AIScript.pale)
			{
				enemyType = Enemy.PaleVags;
			}
			else if (AIScript.creepy && !AIScript.pale)
			{
				enemyType = Enemy.RegularVags;
			}
			else if (AIScript.creepy_male && AIScript.pale)
			{
				enemyType = Enemy.PaleArmsy;
			}
			else if (AIScript.creepy_male && !AIScript.pale)
			{
				enemyType = Enemy.RegularArmsy;
			}
			else if (AIScript.creepy_fat)
			{
				enemyType = Enemy.Cowman;
			}
			else if (AIScript.creepy_baby)
			{
				enemyType = Enemy.Baby;
				IsElite = false;
			}
			else if (AIScript.creepy_boss)
			{
				enemyType = Enemy.Megan;
			}
			else if (AIScript.female && !AIScript.pale && !AIScript.painted)
			{
				enemyType = Enemy.NormalFemale;
			}
			else if (AIScript.femaleSkinny && !AIScript.pale && !AIScript.painted)
			{
				enemyType = Enemy.NormalSkinnyFemale;
			}
			else if (AIScript.female && !AIScript.pale && AIScript.painted)
			{
				enemyType = Enemy.PaintedFemale;
			}
			else if (AIScript.fireman)
			{
				enemyType = Enemy.Fireman;
			}
			else if (AIScript.male && !AIScript.pale && !AIScript.painted && !AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.NormalMale;
			}
			else if (AIScript.maleSkinny && !AIScript.pale && !AIScript.painted && !AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.NormalSkinnyMale;
			}
			else if (AIScript.male && !AIScript.pale && !AIScript.painted && AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.NormalLeaderMale;
			}
			else if (AIScript.male && !AIScript.pale && AIScript.painted && AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.PaintedLeaderMale;
			}
			else if (AIScript.male && !AIScript.pale && AIScript.painted && !AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.PaintedMale;
			}
			else if (AIScript.maleSkinny && AIScript.pale && !AIScript.painted && !AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.PaleSkinnyMale;
			}
			else if (AIScript.maleSkinny && AIScript.pale && !AIScript.painted && !AIScript.leader && AIScript.skinned)
			{
				enemyType = Enemy.PaleSkinnedSkinnyMale;
			}
			else if (AIScript.male && AIScript.pale && !AIScript.painted && !AIScript.leader && !AIScript.skinned)
			{
				enemyType = Enemy.PaleMale;
			}
			else if (AIScript.male && AIScript.pale && !AIScript.painted && !AIScript.leader && AIScript.skinned)
			{
				enemyType = Enemy.PaleSkinnedMale;
			}
		}
		

		private void SetLevel()
		{
			float extraLevels = 1;
			if (AIScript.creepy || AIScript.creepy_fat || AIScript.creepy_male)
			{
				extraLevels = 2;
			}
			else if (AIScript.creepy_baby)
			{
				extraLevels = 0;
			}
			else if (AIScript.creepy_boss)
			{
				extraLevels = 6;
			}
			else if (AIScript.leader)
			{
				extraLevels = 2f;
			}
			else if (AIScript.painted)
			{
				extraLevels += 2f;
			}
			else if (AIScript.pale)
			{
				extraLevels += 1f;
				if (AIScript.skinned)
				{
					extraLevels += 2f;
				}
			}

			switch (ModSettings.difficulty)
			{
				case ModSettings.Difficulty.Easy:
					level = Random.Range(1, 4);
					break;

				case ModSettings.Difficulty.Veteran:
					level = Random.Range(10, 14);

					break;

				case ModSettings.Difficulty.Elite:
					level = Random.Range(20, 25);

					break;

				case ModSettings.Difficulty.Master:
					level = Random.Range(30, 40);

					break;

				case ModSettings.Difficulty.Challenge1:
					level = Random.Range(50, 60);

					break;

				case ModSettings.Difficulty.Challenge2:
					level = Random.Range(89, 92);

					break;

				case ModSettings.Difficulty.Challenge3:
					level = Random.Range(100, 101);

					break;

				case ModSettings.Difficulty.Challenge4:
					level = Random.Range(130, 133);

					break;

				case ModSettings.Difficulty.Challenge5:
					level = Random.Range(160, 165);

					break;

				case ModSettings.Difficulty.Challenge6:
					level = 200;

					break;

				case ModSettings.Difficulty.Hell:
					level = 300;

					break;
			}
			level = Mathf.CeilToInt(level + extraLevels + ModSettings.EnemyLevelIncrease);
		}

	

	}
}
