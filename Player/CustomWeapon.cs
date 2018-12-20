using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class CustomWeapon
    {
        public float damage;
        public float swingspeed;
        public float triedswingspeed;
        public float smashDamage;
        public float treeDamage;
        public float staminaDrain;
        public bool canChopTrees;
        public Mesh mesh;
        public Vector3 offset;
        public Vector3 rotation;
        public float ColliderScale;
        public float Scale;
        public Material material;
        public GameObject obj;

        public CustomWeapon(BaseItem.WeaponModelType model, int mesh, Material material, Vector3 offset, Vector3 rotation, float colliderScale = 1, float scale = 1, float damage = 5, float smashDamage = 15, float swingspeed = 1, float triedswingspeed = 1, float staminaDrain = 6, bool canChopTrees = false, float treeDamage = 1)
        {
            this.damage = damage;
            this.swingspeed = swingspeed;
            this.triedswingspeed = triedswingspeed;
            this.smashDamage = smashDamage;
            this.treeDamage = treeDamage;
            this.staminaDrain = staminaDrain;
            this.canChopTrees = canChopTrees;
            this.mesh = Res.ResourceLoader.instance.LoadedMeshes[mesh];
            this.offset = offset;
            this.rotation = rotation;
            this.material = material;
            ColliderScale = colliderScale;
            Scale = scale;
            CreateGameObject();
            PlayerInventoryMod.customWeapons.Add(model, this);
        }

        public void CreateGameObject()
        {
            obj = GameObject.Instantiate(PlayerInventoryMod.originalPlaneAxeModel, PlayerInventoryMod.originalParrent);
            obj.transform.localRotation = PlayerInventoryMod.originalRotation;

            obj.transform.localPosition = PlayerInventoryMod.OriginalOffset;
            obj.transform.Rotate(rotation, Space.Self);

            obj.transform.localPosition += offset;

            obj.transform.localScale = Vector3.one * Scale;

            obj.GetComponent<Renderer>().material = material;
            obj.GetComponent<MeshFilter>().mesh = mesh;
        }






    }
}
