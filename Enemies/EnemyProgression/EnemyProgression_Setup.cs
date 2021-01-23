using System;
using System.Collections.Generic;
using System.Linq;
using Bolt;
using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Player;
using TheForest.Utils;
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

			SetType();


			//picking abilities
			if ((UnityEngine.Random.value < 0.1 || (AIScript.creepy_boss && !AIScript.girlFullyTransformed) || ModSettings.difficulty == ModSettings.Difficulty.Hell) && enemyType != Enemy.Baby && ModSettings.AllowElites)
			{
				int abilityAmount = (int)ModSettings.difficulty > (int)ModSettings.Difficulty.Veteran ? UnityEngine.Random.Range(3,   7) : 2;
				if (AIScript.creepy_boss)
				{
					abilityAmount = 10;
				}   //Megan boss always has abilities, a lot of them.

				int i = 0;
				Array abilityArray = Enum.GetValues(typeof(Abilities));

				//Determining if enemy is Elite, Miniboss or Boss type of enemy
				if (abilityAmount > 6 && Random.value < 0.3f)
				{
					_rarity = EnemyRarity.Boss;
					abilities.Add(Abilities.BossSteadfast);
				}
				else if (abilityAmount > 4 && Random.value < 0.3f)
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
					Abilities ab = (Abilities)abilityArray.GetValue(UnityEngine.Random.Range(0, abilityArray.Length));
					if (ab == Abilities.Steadfast || ab == Abilities.EliteSteadfast || ab == Abilities.BossSteadfast)
					{
						if (abilities.Contains(Abilities.Steadfast) || abilities.Contains(Abilities.EliteSteadfast) || abilities.Contains(Abilities.BossSteadfast))
						{
							canAdd = false;
						}
					}
					else if (ab == Abilities.Tiny || ab == Abilities.Huge)
					{
						if (AIScript.creepy_boss && ab == Abilities.Tiny)
						{
							canAdd = false;
						}
						else if (abilities.Contains(Abilities.Huge) || abilities.Contains(Abilities.Tiny))
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
			RollName();
			//Assigning rest of stats
			int dif = (int)ModSettings.difficulty;
			DamageMult = Mathf.Pow(Level, 3.5f) / 100f + 0.4f;
			DamageMult *= dif * 2f + 1;

			Armor = Mathf.FloorToInt(Random.Range(Mathf.Pow(Level, 2.4f) * 0.36f + 1, Mathf.Pow(Level, 2.45f) + 20) * ModSettings.EnemyArmorMultiplier);
			Armor *= dif / 2 + 1;
			ArmorReduction = 0;
			extraHealth = (HealthScript.Health * Mathf.Pow((float)Level, 2.215f + (dif * 0.19f)) / 16);
			extraHealth *= dif / 2 + 1;
			AnimSpeed = 0.94f + (float)Level / 190;

			extraHealth *= ModSettings.EnemyHealthMultiplier;
			DamageMult *= ModSettings.EnemyDamageMultiplier;
			AnimSpeed *= ModSettings.EnemySpeedMultiplier;

			if (_rarity != EnemyRarity.Normal)
			{
				Armor *= 2;
			}
			//Extra health for boss type enemies
			switch (_rarity)
			{
				case EnemyRarity.Elite:
					extraHealth *= 2;

					break;

				case EnemyRarity.Miniboss:
					extraHealth *= 5;

					break;

				case EnemyRarity.Boss:
					extraHealth *= 10;
					if (!abilities.Contains(Abilities.Tiny))
						gameObject.transform.localScale *= 1.1f;

					break;
			}
			extraHealth *= (float)(dif * 0.25f + 0.75f);
			if (dif > 3)
				extraHealth *= 2.15f;
			if (dif > 6)
				extraHealth *= 3.4f;
			//Applying some abilities
			if (abilities.Contains(Abilities.Huge))
			{
				Armor *= 2;
				gameObject.transform.localScale *= 2;
				extraHealth *= 2;
				DamageMult *= 2;
				AnimSpeed /= 1.6f;
			}
			else if (abilities.Contains(Abilities.Tiny))
			{
				gameObject.transform.localScale *= 0.35f;
				AnimSpeed *= 1.1f;
				DamageMult *= 1.2f;
				BroadcastMessage("SetTriggerScaleForTiny", SendMessageOptions.DontRequireReceiver);
			}
			if (abilities.Contains(Abilities.Steadfast))
			{
				Steadfast = 5;
			}
			if (abilities.Contains(Abilities.EliteSteadfast))
			{
				Steadfast = 2f;
			}
			if (abilities.Contains(Abilities.BossSteadfast))
			{
				Steadfast = 0.25f;
			}
			if (abilities.Contains(Abilities.ExtraHealth))
			{
				extraHealth *= 3;
			}
			if (abilities.Contains(Abilities.ExtraDamage))
			{
				DamageMult *= 4f;
			}
			if (abilities.Contains(Abilities.RainEmpowerement))
			{
				prerainDmg = DamageMult;
				prerainArmor = Armor;
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
			if (abilities.Contains(Abilities.FireAura))
			{
				float aurDmg = (5 * Level + 1) * ((int)ModSettings.difficulty + 1.3f);
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
						ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
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
				HealthScript.maxHealthFloat = extraHealth;
				Armor = Mathf.Min(Armor, int.MaxValue - 5);
				if (Armor < 0)
					Armor = int.MaxValue;
				//Setting other health variables
				HealthScript.maxHealth = Mathf.RoundToInt(Mathf.Min(extraHealth, int.MaxValue - 5));
				HealthScript.Health = Mathf.RoundToInt(Mathf.Min(extraHealth, int.MaxValue - 5));
				extraHealth -= HealthScript.Health;
				DebuffDmgMult = DamageMult;
				DualLifeSpend = false;
				setupComplete = true;
				OnDieCalled = false;
				BaseDamageMult = DamageMult;
				BaseAnimSpeed = AnimSpeed;
			}
			catch (Exception e)
			{
				ModAPI.Log.Write(e.ToString());
				CotfUtils.Log(e.Message);
			}

			AssignBounty();

			SteadfastCap = Mathf.Max(Steadfast * 0.01f * maxHealth, 1f);
			if (SteadfastCap < 1) // Making sure the enemy can be killed
			{
				SteadfastCap = 1;
			}

			CreationTime = Time.time;

			if (BoltNetwork.isRunning)
			{
				ulong id = entity.networkId.PackedValue;
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(30);
						w.Write(id);
						w.Write(BaseDamageMult);
						foreach (Abilities ability in abilities)
						{
							w.Write((int)ability);
						}
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Clients);
					answerStream.Close();
				}
			}
		}



		public static string[] fNames = new string[] { "Lizz Plays", "Wolfskull", "Wiktoria",
					"Emma", "Olivia", "Isabella", "Sophia", "Mia", "Evelyn","Emily", "Elizabeth", "Sofia",
					"Victoria",  "Chloe", "Camila", "Layla", "Lillian", "Hannah", "Lily",
					"Natalie", "Luna", "Savannah", "Leah", "Zoe", "Stella", "Ellie", "Claire", "Bella", "Aurora",
					"Lucy", "Anna", "Samantha", "Caroline", "Genesis", "Aaliyah", "Kennedy", "Allison",
					"Maya", "Sarah", "Madelyn", "Adeline", "Alexa", "Ariana", "Elena", "Gabriella", "Naomi", "Alice",
					"Hailey", "Eva", "Emilia",  "Quinn", "Piper", "Serenity", "Willow", "Everly",  "Kaylee",
					"Lydia", "Aubree", "Arianna", "Eliana", "Peyton", "Melanie", "Gianna", "Isabelle", "Julia", "Valentina",
					"Nova", "Clara", "Vivian", "Reagan", "Mackenzie", "Madeline", "Delilah", "Isla", "Rylee",
					"Katherine", "Sophie",  "Liliana", "Jade", "Maria", "Taylor Swift", "Hadley", "Kylie", "Emery", "Adalynn", "Natalia",
					"Annabelle", "Faith", "Alexandra", "Athena", "Andrea", "Leilani", "Jasmine", "Lyla", "Margaret", "Alyssa",
					"Eliza", "Ariel", "Alexis","xKito","Sophie Francis","Albedo","Hazardina","Kaspita" };
		public static string[] mNames = new string[]
				  {
				  "Farket","Hazard","Moritz","Souldrinker","Olivier Broadbent","Subscribe to Pewds","Kutie","Axt","Fionera","Cleetus","Hellsing","Metamoth","Teledorktronics","SmokeyThePerson","NightOwl","PuffedRice","PhoenixDeath","Lyon the weeb","Danny Parker","Kaspito","Lukaaa","Chefen","Lauren","DrowsyCob","Ali","ArcKaino","Calean","LordSidonus","DTfang","Malкae","R3iGN","Torsin","θฯ12","Иio","Komissar bAv","The Strange Man","Micha","MiikaHD","NÜT","AssPirate","Azpect","LumaR","TeigRolle","Foreck","Gaullin","Alichipmunk","Chad","Blight","Cheddar","MaddVladd","Wren","Ross Draws","Sam Gorski","Mike Diva","Niko Pueringer","Freddy Wong","PewDiePie","Salad Ass","Morgan Page","Hex Cougar","Unlike Pluto","Sora","Film Crafterz","Fon","Sigmar","Mohammed","Cyde","MaximumAsp79","Diavolo","Doppio Vinegar","Dio Brando","Giorno Giovanna","Fellow Komrade","Samuel","Sebastian","David","Carter","Wyatt","Jayden","John","Owen","Dylan","Luke","Gabriel","Anthony","Isaac","Grayson","Jack","Julian","Levi","Christopher","Joshua","Andrew","Lincoln","Mateo","Ryan","Jaxon","Xet","Adolf","Geoxor","Eraized", "Xelthaz", "Commanderroot", "Plqauttro","Tom from Myspace","Maurycy","Kuldar, stab this guy","Punny", "Aezyn", "Infernal", "PorkyBunBuns"
				  };

		private void RollName()
		{
			if (enemyType == Enemy.Megan)
			{
				enemyName = "Megan Cross";
				return;
			}

			List<string> names = new List<string>();
			string prefix = "";
			if (AIScript.female || AIScript.creepy || AIScript.femaleSkinny)    //is female
			{
				//prefix = "♀ ";
				names.AddRange(fNames);
			}
			else                                                 // is male
			{
				//prefix = "♂ ";
				names.AddRange(mNames);
			}
			if (AIScript.creepy_male)
			{
				names.Add("Alex Armsy");
			}
			if (AIScript.maleSkinny)
			{
				names.Add("Zebulon");
			}
			enemyName = prefix + names[Random.Range(0, names.Count)];
		}
		private void SetType()
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
					Level = Random.Range(1, 6);
					break;

				case ModSettings.Difficulty.Veteran:
					Level = Random.Range(5, 10);

					break;

				case ModSettings.Difficulty.Elite:
					Level = Random.Range(12, 20);

					break;

				case ModSettings.Difficulty.Master:
					Level = Random.Range(25, 40);

					break;

				case ModSettings.Difficulty.Challenge1:
					Level = Random.Range(50, 66);

					break;

				case ModSettings.Difficulty.Challenge2:
					Level = Random.Range(86, 96);

					break;

				case ModSettings.Difficulty.Challenge3:
					Level = Random.Range(100, 110);

					break;

				case ModSettings.Difficulty.Challenge4:
					Level = Random.Range(120, 140);

					break;

				case ModSettings.Difficulty.Challenge5:
					Level = Random.Range(200, 250);

					break;

				case ModSettings.Difficulty.Challenge6:
					Level = Random.Range(340, 350);

					break;

				case ModSettings.Difficulty.Hell:
					Level = Random.Range(380, 390);

					break;
			}
			Level = Mathf.CeilToInt(Level + extraLevels + ModSettings.EnemyLevelIncrease);
		}

		private void AssignBounty()
		{
			double b = Random.Range(maxHealth * 0.35f, maxHealth * 0.45f) * Mathf.Sqrt(Level) + Armor;
			if (abilities.Count > 1)
			{
				b *= abilities.Count;
			}
			switch (ModSettings.difficulty)
			{
				case ModSettings.Difficulty.Easy:
					b = b * 1.3f;
					break;
				case ModSettings.Difficulty.Veteran:
					b = b * 1.5;
					break;

				case ModSettings.Difficulty.Elite:
					b = b * 1.7;

					break;

				case ModSettings.Difficulty.Master:
					b = b * 2.2;

					break;

				case ModSettings.Difficulty.Challenge1:
					b = b * 3;

					break;

				case ModSettings.Difficulty.Challenge2:
					b = b * 4;

					break;

				case ModSettings.Difficulty.Challenge3:
					b = b * 5.25f;

					break;

				case ModSettings.Difficulty.Challenge4:
					b = b * 6.8;

					break;

				case ModSettings.Difficulty.Challenge5:
					b = b * 10;

					break;

				case ModSettings.Difficulty.Challenge6:
					b = b * 15;

					break;

				case ModSettings.Difficulty.Hell:
					b = b * 15;

					break;
			}
			b *= ModSettings.ExpMultiplier;
			bounty = Convert.ToInt64(b);
		}

	}
}
