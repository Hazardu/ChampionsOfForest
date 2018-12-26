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
        public static bool SetupComplete;
        public static GameObject originalPlaneAxeModel = null;
        public static Transform originalParrent;
        public static float OriginalTreeDmg;

        public static Mesh noMesh;
        public static Mesh originalMesh;

        public static Dictionary<BaseItem.WeaponModelType, CustomWeapon> customWeapons = new Dictionary<BaseItem.WeaponModelType, CustomWeapon>();


        public static BaseItem.WeaponModelType CustomEquipModel = BaseItem.WeaponModelType.None;
        public static BaseItem.WeaponModelType EquippedModel = BaseItem.WeaponModelType.None;

        public static bool ChangeMaxAmount= false;

        protected override bool Equip(InventoryItemView itemView, bool pickedUpFromWorld)
        {
            if (itemView != null)
            {
                EquippedModel = BaseItem.WeaponModelType.None;
                if (itemView._heldWeaponInfo.transform.parent.name == "AxePlaneHeld")
                {
                    if (!SetupComplete)
                    {
                        try
                        {

                            ModAPI.Log.Write("SETUP: Custom weapons");
                            //ModAPI.Log.Write("small axe: " + itemView._heldWeaponInfo.smallAxe);
                            //ModAPI.Log.Write("allowBodyCut: " + itemView._heldWeaponInfo.allowBodyCut);
                            //ModAPI.Log.Write("animSpeed: " + itemView._heldWeaponInfo.animSpeed);
                            //ModAPI.Log.Write("animTiredSpeed: " + itemView._heldWeaponInfo.animTiredSpeed);
                            //ModAPI.Log.Write("blockDamagePercent: " + itemView._heldWeaponInfo.blockDamagePercent);
                            //ModAPI.Log.Write("blockStaminaDrain: " + itemView._heldWeaponInfo.blockStaminaDrain);
                            //ModAPI.Log.Write("chainSaw: " + itemView._heldWeaponInfo.chainSaw);
                            //ModAPI.Log.Write("canDoGroundAxeChop: " + itemView._heldWeaponInfo.canDoGroundAxeChop);
                            //ModAPI.Log.Write("doSingleArmBlock: " + itemView._heldWeaponInfo.doSingleArmBlock);
                            //ModAPI.Log.Write("fireStick: " + itemView._heldWeaponInfo.fireStick);
                            //ModAPI.Log.Write("machete: " + itemView._heldWeaponInfo.machete);
                            //ModAPI.Log.Write("noBodyCut: " + itemView._heldWeaponInfo.noBodyCut);
                            //ModAPI.Log.Write("noTreeCut: " + itemView._heldWeaponInfo.noTreeCut);
                            //ModAPI.Log.Write("pushForce: " + itemView._heldWeaponInfo.pushForce);
                            //ModAPI.Log.Write("repairTool: " + itemView._heldWeaponInfo.repairTool);
                            //ModAPI.Log.Write("rock: " + itemView._heldWeaponInfo.rock);
                            //ModAPI.Log.Write("shell: " + itemView._heldWeaponInfo.shell);
                            //ModAPI.Log.Write("soundDetectRange: " + itemView._heldWeaponInfo.soundDetectRange);
                            //ModAPI.Log.Write("spear: " + itemView._heldWeaponInfo.spear);
                            //ModAPI.Log.Write("staminaDrain: " + itemView._heldWeaponInfo.staminaDrain);
                            //ModAPI.Log.Write("stick: " + itemView._heldWeaponInfo.stick);
                            //ModAPI.Log.Write("tiredSpeed: " + itemView._heldWeaponInfo.tiredSpeed);
                            //ModAPI.Log.Write("treeDamage: " + itemView._heldWeaponInfo.treeDamage);
                            //ModAPI.Log.Write("weaponDamage: " + itemView._heldWeaponInfo.weaponDamage);
                            //ModAPI.Log.Write("weaponRange: " + itemView._heldWeaponInfo.weaponRange);
                            //ModAPI.Log.Write("weaponSpeed: " + itemView._heldWeaponInfo.weaponSpeed);
                            //ModAPI.Log.Write("weaponAudio name: " + itemView._heldWeaponInfo.weaponAudio.name);

                            
                            //item id is 80 for plane axe
                            //collider dimensions:
                            //(3.1, 1.6, 0.5) size
                            //(1.1, 0.4, 0.0) center

                            SetupComplete = true;
                            customWeapons.Clear();
                            originalPlaneAxe = itemView;
                            originalPlaneAxeModel = itemView._heldWeaponInfo.transform.parent.GetChild(2).gameObject;
                            originalRotation = originalPlaneAxeModel.transform.localRotation;
                            OriginalOffset = originalPlaneAxeModel.transform.localPosition;
                            originalParrent = originalPlaneAxeModel.transform.parent;
                            OriginalTreeDmg = itemView._heldWeaponInfo.treeDamage;
                            originalMesh = originalPlaneAxeModel.GetComponent<MeshFilter>().mesh;
                            noMesh = new Mesh();

                            new CustomWeapon(BaseItem.WeaponModelType.LongSword, 51, BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData() { MainTexture = Res.ResourceLoader.instance.LoadedTextures[60], Metalic = 0.76f, Smoothness = 0.66f}), new Vector3(0.2f - 0.04347827f, -1.5f + 0.173913f, 0.3f - 0.05797101f), new Vector3(0, -90, 0), 1.4f, 0.9f, 40, 80, 0.4f, 0.2f, 50, true, 5);
                           // new CustomWeapon(BaseItem.WeaponModelType.GreatSword, 52, BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData() { OcclusionStrenght = 0.3f, MetalicTexture = Res.ResourceLoader.instance.LoadedTextures[59], Smoothness = 0.4f, Metalic = 0.0f, MainTexture = Res.ResourceLoader.instance.LoadedTextures[61], EmissionMap = Res.ResourceLoader.instance.LoadedTextures[62], BumpMap = Res.ResourceLoader.instance.LoadedTextures[64], HeightMap = Res.ResourceLoader.instance.LoadedTextures[65], Occlusion = Res.ResourceLoader.instance.LoadedTextures[66] }), new Vector3(0.15f - 0.03623189f, -2.13f - 0.0572464f, 0.19f - 0.1014493f), new Vector3(180, 180, 90), 2.5f, 1f, 100, 300, 0.2f, 0.15f, 100, false, 1000);
                            new CustomWeapon(BaseItem.WeaponModelType.GreatSword, 52, BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData() { OcclusionStrenght = 0.45f, Smoothness = 0.5f, Metalic = 0.4f, MainTexture = Res.ResourceLoader.instance.LoadedTextures[61], EmissionMap = Res.ResourceLoader.instance.LoadedTextures[62], BumpMap = Res.ResourceLoader.instance.LoadedTextures[64], HeightMap = Res.ResourceLoader.instance.LoadedTextures[65], Occlusion = Res.ResourceLoader.instance.LoadedTextures[66] }), new Vector3(0.15f - 0.03623189f, -2.13f - 0.0572464f, 0.19f - 0.1014493f), new Vector3(180, 180, 90), 2.5f, 1f, 100, 300, 0.05f, 0.005f, 85, false, 1000);
                        }
                        catch (System.Exception eee)
                        {
                            ModAPI.Log.Write("Error with setting up custom weaponry " + eee.ToString());
                        }
                    }
                    if (CustomEquipModel != BaseItem.WeaponModelType.None)
                    {
                        ModAPI.Console.Write("Equipping custom weapon " + CustomEquipModel.ToString());
                        EquippedModel = CustomEquipModel;
                        try
                        {


                            foreach (CustomWeapon item in customWeapons.Values)
                            {
                                item.obj.SetActive(false);
                            }
                            CustomWeapon cw = customWeapons[CustomEquipModel];
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
                        catch (System.Exception exc)
                        {

                            ModAPI.Log.Write("Error with EQUIPPING custom weaponry " + exc.ToString());
                        }

                    }
                    else
                    {
                        ModAPI.Console.Write("EQUIPPING normal plane axe");

                        itemView._heldWeaponInfo.transform.parent.GetChild(2).gameObject.SetActive(true);
                        foreach (CustomWeapon item in customWeapons.Values)
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
        protected override void FireRangedWeapon()
        {
            base.FireRangedWeapon();
        }

        protected override void ThrowProjectile()
        {
            base.ThrowProjectile();
        }

        public override void Attack()
        {
            if (!IsRightHandEmpty() && !_isThrowing && !IsReloading && !blockRangedAttack && !IsSlotLocked(TheForest.Items.Item.EquipmentSlot.RightHand) && !LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._slingShotId))
            {
                if (Player.SpellActions.IsCleaveEquipped)
                {
                    //blah blah blax special attack modifier goes here
                }
            }
            base.Attack();
        }
    }
}
