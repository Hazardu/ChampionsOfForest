using System.Collections.Generic;
using TheForest.Items.Inventory;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    internal class PlayerInventoryMod : PlayerInventory
    {

        public static Vector3 Rot;
        public static Vector3 Pos;


        public static InventoryItemView originalPlaneAxe;
        public static Quaternion originalRotation;
        public static Vector3 OriginalOffset;
        public static bool SetupComplete = false;
        public static GameObject originalPlaneAxeModel = null;
        public static Transform originalParrent;
        public static float OriginalTreeDmg;

        public static Mesh noMesh;
        public static Mesh originalMesh;

        public static List<CustomWeapon> customWeapons = new List<CustomWeapon>();

      
        public static int CustomEquipID = -1;

        protected override bool Equip(InventoryItemView itemView, bool pickedUpFromWorld)
        {
            if (itemView != null)
            {
                if (itemView._heldWeaponInfo.transform.parent.name == "AxePlaneHeld")
                {
                    if (CustomEquipID != -1)
                    {
                        if (!SetupComplete)
                        {
                            try
                            {


                                //item id is 80
                                //collider dimensions:
                                //(3.1, 1.6, 0.5) size
                                //(1.1, 0.4, 0.0) center

                                SetupComplete = true;

                                originalPlaneAxe = itemView;
                                originalPlaneAxeModel = itemView._heldWeaponInfo.transform.parent.GetChild(2).gameObject;
                                originalRotation = originalPlaneAxeModel.transform.localRotation;
                                OriginalOffset = originalPlaneAxeModel.transform.localPosition;
                                originalParrent = originalPlaneAxeModel.transform.parent;
                                OriginalTreeDmg = itemView._heldWeaponInfo.treeDamage;
                                originalMesh = originalPlaneAxeModel.GetComponent<MeshFilter>().mesh;
                                noMesh = new Mesh();
                                new CustomWeapon(51, BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData() { MainTexture = Res.ResourceLoader.instance.LoadedTextures[60], Metalic = 0.76f, Smoothness = 0.66f,EmissionColor = new Color(0.03f,0.03f,0.03f) }),new Vector3(0.2f,-1.5f,0.3f),new Vector3(0,-90,0), 2.2f, 0.9f, 30, 50, 0.7f, 0.6f, 10, false, 100);
                                new CustomWeapon(52, BuilderCore.Core.CreateMaterial(
                                    new BuilderCore.BuildingData() {Smoothness =0.7f,Metalic = 0.6f, MainTexture = Res.ResourceLoader.instance.LoadedTextures[61],EmissionMap= Res.ResourceLoader.instance.LoadedTextures[62],BumpMap= Res.ResourceLoader.instance.LoadedTextures[64],HeightMap= Res.ResourceLoader.instance.LoadedTextures[65],Occlusion = Res.ResourceLoader.instance.LoadedTextures[66] }
                                    ), new Vector3(0.15f,-2.13f,0.19f), new Vector3(180,180,90), 2.2f, 0.9f, 30, 50, 0.7f, 0.6f, 10, false, 100);
                            }
                            catch (System.Exception e)
                            {

                                ModAPI.Log.Write(e.ToString());
                            }

                        }
                        foreach (CustomWeapon item in customWeapons)
                        {
                            item.obj.SetActive(false);
                        }
                        CustomWeapon cw = customWeapons[CustomEquipID];
                        cw.obj.SetActive(true);

                        itemView._heldWeaponInfo.weaponSpeed = itemView._heldWeaponInfo.baseWeaponSpeed * cw.swingspeed;
                        itemView._heldWeaponInfo.tiredSpeed = itemView._heldWeaponInfo.baseTiredSpeed * cw.triedswingspeed;
                        itemView._heldWeaponInfo.smashDamage = cw.smashDamage;
                        itemView._heldWeaponInfo.weaponDamage = cw.damage;
                        itemView._heldWeaponInfo.treeDamage = cw.treeDamage;
                        itemView._heldWeaponInfo.weaponRange = cw.ColliderScale * 3;
                        itemView._heldWeaponInfo.staminaDrain = cw.staminaDrain;
                        itemView._heldWeaponInfo.noTreeCut = cw.canChopTrees;
                        itemView._heldWeaponInfo.transform.localScale = Vector3.one * cw.ColliderScale;


                        originalPlaneAxeModel.GetComponent<MeshFilter>().mesh = noMesh;
                    }
                    else
                    {
                        itemView._heldWeaponInfo.transform.parent.GetChild(2).gameObject.SetActive(true);
                        foreach (CustomWeapon item in customWeapons)
                        {
                            item.obj.SetActive(false);
                        }
                        itemView._heldWeaponInfo.weaponSpeed = itemView._heldWeaponInfo.baseWeaponSpeed;
                        itemView._heldWeaponInfo.tiredSpeed = itemView._heldWeaponInfo.baseTiredSpeed;
                        itemView._heldWeaponInfo.smashDamage = itemView._heldWeaponInfo.baseSmashDamage;
                        itemView._heldWeaponInfo.weaponDamage = itemView._heldWeaponInfo.baseWeaponDamage;
                        itemView._heldWeaponInfo.treeDamage = OriginalTreeDmg;
                        itemView._heldWeaponInfo.weaponRange = itemView._heldWeaponInfo.baseWeaponRange;
                        itemView._heldWeaponInfo.staminaDrain = itemView._heldWeaponInfo.baseStaminaDrain;
                        itemView._heldWeaponInfo.noTreeCut = false;
                        itemView._heldWeaponInfo.transform.localScale = Vector3.one * 0.6f;
                        originalPlaneAxeModel.GetComponent<MeshFilter>().mesh = originalMesh;

                    }
                }
            }

            return base.Equip(itemView, pickedUpFromWorld);
        }

        public override void Attack()
        {
            if (!IsRightHandEmpty() && !_isThrowing && !IsReloading && !blockRangedAttack && !IsSlotLocked(TheForest.Items.Item.EquipmentSlot.RightHand) && !LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._slingShotId))
            {
                if (Player.SpellActions.IsCleaveEquipped)
                {

                }
            }
            base.Attack();
        }
    }

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

            public CustomWeapon(int mesh, Material material, Vector3 offset, Vector3 rotation, float colliderScale = 1, float scale = 1 ,float damage = 5, float smashDamage = 15, float swingspeed = 1, float triedswingspeed = 1, float staminaDrain = 6, bool canChopTrees = false, float treeDamage = 1)
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
                PlayerInventoryMod.customWeapons.Add(this);
            }

            public void CreateGameObject()
            {
                obj =GameObject.Instantiate(PlayerInventoryMod.originalPlaneAxeModel, PlayerInventoryMod.originalParrent);
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
