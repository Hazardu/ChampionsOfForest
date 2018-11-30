namespace ChampionsOfForest
{
    public class EnemyHealthMod : EnemyHealth
    {
        public EnemyProgression progression;

        //creates progression
        protected override void Start()
        {
            base.Start();
            progression = gameObject.AddComponent<EnemyProgression>();
            progression._Health = this;
            progression._AI = ai;
            progression.entity = entity;
            progression.setup = setup;
        }

        //changes how damage is calculated to include armor and abilities
        public override void Hit(int damage)
        {
            HitPhysical(damage);
        }

        public void HitPhysical(int damage)
        {
            damage = progression.ClampDamage(false, damage);
            HitReal(damage);
        }

        public override void Die()
        {
            progression.OnDie();
            //here will go code for progressions dual life ability

            base.Die();
        }
    }

}
