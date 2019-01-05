using UnityEngine;
namespace ChampionsOfForest
{
    public class EnemyHealthMod : EnemyHealth
    {
        public EnemyProgression progression = null;


        protected override void OnEnable()
        {
           
                Invoke("LateProgressionCreate", 3);
            

            base.OnEnable();
        }
        protected override void HitFire()
        {
            if (this.ai.creepy_boss && !this.ai.girlFullyTransformed)
            {
                return;
            }
            if (!this.deadBlock)
            {
                this.setSkinDamage(UnityEngine.Random.Range(0, 3));
                this.targetSwitcher.attackerType = 4;
                int num = this.douseMult - 1;
                int i = 0;
                if (num < 1)
                {
                    num = 1;
                }
                if (this.ai.creepy_boss)
                {
                     i = Mathf.CeilToInt((float)(2 * num) * TheForest.Utils.Settings.GameSettings.Ai.fireDamageCreepyRatio * progression.FireDmgAmp);

                    this.HitFireDamageOnly(i);
                }
                else if (this.ai.creepy || this.ai.creepy_male || this.ai.creepy_fat || this.ai.creepy_baby || this.ai.creepy_boss)
                {
                     i = Mathf.CeilToInt((float)(UnityEngine.Random.Range(3, 6) * num) * TheForest.Utils.Settings.GameSettings.Ai.fireDamageCreepyRatio * progression.FireDmgAmp);

                    this.Hit(i);
                }
                else
                {
                     i = Mathf.CeilToInt((float)(UnityEngine.Random.Range(4, 6) * num) * TheForest.Utils.Settings.GameSettings.Ai.fireDamageRatio * progression.FireDmgAmp);
                    this.Hit(i);
                }
                progression.ArmorReduction += i;
            }
        }
        private void LateProgressionCreate()
        {
            try
            {


                if (progression == null)
                {
                    progression = gameObject.AddComponent<EnemyProgression>();
                    progression._Health = this;
                    progression._AI = ai;
                    progression.entity = entity;
                    progression.setup = setup;

                }
                progression.setupComplete = false;
                progression.OnDieCalled = false;
            }
            catch (System.Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }
        }

        protected override void Update()
        {
            if(progression == null)
            {
                progression = GetComponent<EnemyProgression>();
            }
            base.Update();
        }
        protected override void OnDestroy()
        {
            if (!ai.creepy_fat)
            {
                if (Health <= 0)
                {
                    progression.OnDie();
                }
            }
            base.OnDestroy();
        }
        //changes how damage is calculated to include armor and abilities
        public override void Hit(int damage)
        {
            if (ai.creepy_fat)
            {
                base.Hit(damage);
                return;
            }
            HitPhysical(damage);
        }

        public void HitPhysical(int damage)
        {
            if (damage < -50000)
            {
                //reduce armor
                int armorreduction = -damage - 50000;
                progression.ArmorReduction += armorreduction;
                return;
            }
            int dmg = progression.ClampDamage(false, damage);
            HitReal(dmg);
        }

        public override void HitReal(int damage)
        {
            Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, damage);

            if (!ai.creepy_fat)
            {

                if (Health - damage < 1)
                {
                    if (!progression.OnDie())
                    {
                        Health -= damage;
                        return;
                    }
                }
            }
            base.HitReal(damage);

        }
        protected override void dieExplode()
        {
            if (ai.creepy_fat)
            {
                base.dieExplode();
                return;
            }
            if (progression.OnDie())
            {
                base.dieExplode();
            }
        }
        protected override void DieTrap(int type)
        {
            if (ai.creepy_fat)
            {
                base.DieTrap(type);
                return;
            }
            if (progression.OnDie())
            {
                base.DieTrap(type);
            }
        }
        public override void Die()
        {
            if (ai.creepy_fat)
            {
                base.Die();
                return;
            }
            if (progression.OnDie())
            {
                base.Die();
            }
        }
    }

}
