using System.Collections.Generic;
using TheForest.Items.Inventory;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    internal class PlayerInventoryMod : PlayerInventory
    {

        //public static Vector3 Rot;
        //public static Vector3 Pos;
        public static PlayerInventoryMod instance;

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


        public static BaseItem.WeaponModelType ToEquipWeaponType = BaseItem.WeaponModelType.None;
        public static BaseItem.WeaponModelType EquippedModel = BaseItem.WeaponModelType.None;

        protected override bool Equip(InventoryItemView itemView, bool pickedUpFromWorld)
        {
            if (!ModSettings.IsDedicated)
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
                                if (instance == null)
                                {
                                    instance = this;
                                }
                               
                             
                                //ModAPI.Log.Write("SETUP: Custom weapons");
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

                                //Creating custom weapons---------
                                CreateCustomWeapons();

                                try
                                {
                                    ModReferences.rightHandTransform = itemView._heldWeaponInfo.transform.parent.gameObject.transform.parent.transform;
                                    ModAPI.Console.Write(ModReferences.rightHandTransform.name);

                               

                                }
                                catch (System.Exception e)
                                {

                                    ModAPI.Console.Write(e.ToString());
                                }

                            }
                            catch (System.Exception eee)
                            {
                                ModAPI.Log.Write("Error with setting up custom weaponry " + eee.ToString());
                            }
                        }
                        if (ToEquipWeaponType != BaseItem.WeaponModelType.None)
                        {
                            EquippedModel = ToEquipWeaponType;
                            try
                            {


                                foreach (CustomWeapon item in customWeapons.Values)
                                {
                                    item.obj.SetActive(false);
                                }
                                CustomWeapon cw = customWeapons[ToEquipWeaponType];
                                cw.obj.SetActive(true);
                                itemView._heldWeaponInfo.weaponSpeed = itemView._heldWeaponInfo.baseWeaponSpeed * cw.swingspeed;
                                itemView._heldWeaponInfo.tiredSpeed = itemView._heldWeaponInfo.baseTiredSpeed * cw.tiredswingspeed;
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
            }
            return base.Equip(itemView, pickedUpFromWorld);
        }


        //RANGED MOD CHANGES---------------------------------------------------


        protected override void FireRangedWeapon()
        {
            if (ModSettings.IsDedicated) return;
                InventoryItemView inventoryItemView = _equipmentSlots[0];
            TheForest.Items.Item itemCache = inventoryItemView.ItemCache;
            bool flag = itemCache._maxAmount < 0;
            bool flag2 = false;
            if (flag || RemoveItem(itemCache._ammoItemId, 1, false, true))
            {
                ModdedPlayer.instance.lastShotProjectile = itemCache;
                InventoryItemView inventoryItemView2 = _inventoryItemViewsCache[itemCache._ammoItemId][0];
                TheForest.Items.Item itemCache2 = inventoryItemView2.ItemCache;
                FakeParent component = inventoryItemView2._held.GetComponent<FakeParent>();
                if (UseAltWorldPrefab)
                {
                }
                GameObject gameObject = (!(bool)component || component.gameObject.activeSelf) ? Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, inventoryItemView2._held.transform.position, inventoryItemView2._held.transform.rotation) : Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, component.RealPosition, component.RealRotation);
                gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;
                if ((bool)gameObject.GetComponent<Rigidbody>())
                {
                    if (itemCache.MatchRangedStyle(TheForest.Items.Item.RangedStyle.Shoot))
                    {
                        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformDirection(Vector3.forward * (0.016666f / Time.fixedDeltaTime) * ModdedPlayer.instance.ProjectileSpeedRatio * itemCache._projectileThrowForceRange), ForceMode.VelocityChange);
                    }
                    else
                    {
                        float num = Time.time - _weaponChargeStartTime;
                        if (ForestVR.Enabled)
                        {
                            gameObject.GetComponent<Rigidbody>().AddForce(inventoryItemView2._held.transform.up * ModdedPlayer.instance.ProjectileSpeedRatio * itemCache._projectileThrowForceRange);
                        }
                        else
                        {
                            gameObject.GetComponent<Rigidbody>().AddForce(inventoryItemView2._held.transform.up * ModdedPlayer.instance.ProjectileSpeedRatio * Mathf.Clamp01(num / itemCache._projectileMaxChargeDuration) * (0.016666f / Time.fixedDeltaTime) * itemCache._projectileThrowForceRange);
                        }
                        if (LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._bowId))
                        {
                            gameObject.SendMessage("setCraftedBowDamage", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    inventoryItemView._held.SendMessage("OnAmmoFired", gameObject, SendMessageOptions.DontRequireReceiver);
                }
                if (itemCache._attackReleaseSFX != 0)
                {
                    LocalPlayer.Sfx.SendMessage(itemCache._attackReleaseSFX.ToString(), SendMessageOptions.DontRequireReceiver);
                }
                Mood.HitRumble();
            }
            else
            {
                flag2 = true;
                if (itemCache._dryFireSFX != 0)
                {
                    LocalPlayer.Sfx.SendMessage(itemCache._dryFireSFX.ToString(), SendMessageOptions.DontRequireReceiver);
                }
            }
            if (flag)
            {
                UnequipItemAtSlot(itemCache._equipmentSlot, false, false, flag);
            }
            else
            {
                ToggleAmmo(inventoryItemView, true);
            }
            _weaponChargeStartTime = 0f;
            SetReloadDelay((!flag2) ? itemCache._reloadDuration : itemCache._dryFireReloadDuration);
            _isThrowing = false;
        }





        protected override void ThrowProjectile()
        {
            base.ThrowProjectile();
        }



        public override void Attack()
        {
            if (ModSettings.IsDedicated) return;

            if (!IsRightHandEmpty() && !_isThrowing && !IsReloading && !blockRangedAttack && !IsSlotLocked(TheForest.Items.Item.EquipmentSlot.RightHand) && !LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._slingShotId))
            {
                if (EquippedModel != BaseItem.WeaponModelType.None && customWeapons.ContainsKey(EquippedModel))
                {
                    customWeapons[EquippedModel].EnableTrail();
                }
                if (ModdedPlayer.instance.DeathPact_Enabled)
                {
                    LocalPlayer.Stats.Health -= ModdedPlayer.instance.MaxHealth * 0.07f;
                    LocalPlayer.Stats.Health = Mathf.Max(1, LocalPlayer.Stats.Health);
                }
            }
            base.Attack();
        }

        public override void AddMaxAmountBonus(int itemId, int amount)
        {
            if (ModSettings.IsDedicated) return;
            base.AddMaxAmountBonus(itemId, amount);
        }

        public override void SetMaxAmountBonus(int itemId, int amount)
        {
            base.SetMaxAmountBonus(itemId, amount);
            if (ModdedPlayer.instance.ExtraCarryingCapactity.ContainsKey(itemId))
            {
                ModdedPlayer.instance.ExtraCarryingCapactity[itemId].NewApply();
            }
        }


        public void CreateCustomWeapons()
        {
            //long sword
            new CustomWeapon(BaseItem.WeaponModelType.LongSword,
                    51,
                    BuilderCore.Core.CreateMaterial(
                        new BuilderCore.BuildingData()
                        {
                            MainTexture = Res.ResourceLoader.instance.LoadedTextures[60],
                            Metalic = 0.86f,
                            Smoothness = 0.66f
                        }
                        ),
                    new Vector3(0.2f - 0.04347827f, -1.5f + 0.173913f, 0.3f - 0.05797101f),
                    new Vector3(0, -90, 0),
                    new Vector3(-0.2f, -2.3f, 0),
                    1.3f, 0.9f, 40, 80, 0.4f, 0.2f, 50, true, 5);


            //great sword
            new CustomWeapon(BaseItem.WeaponModelType.GreatSword,
                52,
                BuilderCore.Core.CreateMaterial(
                    new BuilderCore.BuildingData()
                    {
                        OcclusionStrenght = 0.75f,
                        Smoothness = 0.6f,
                        Metalic = 0.6f,
                        MainTexture = Res.ResourceLoader.instance.LoadedTextures[61],
                        EmissionMap = Res.ResourceLoader.instance.LoadedTextures[62],
                        BumpMap = Res.ResourceLoader.instance.LoadedTextures[64],
                        HeightMap = Res.ResourceLoader.instance.LoadedTextures[65],
                        Occlusion = Res.ResourceLoader.instance.LoadedTextures[66]
                    }
                    ),
                new Vector3(0.15f - 0.03623189f, -2.13f - 0.0572464f, 0.19f - 0.1014493f),
                new Vector3(180, 180, 90),
                new Vector3(0, 0, -3.5f),
                1.8f, 1f, 60, 90, 0.01f, 0.001f, 85, true, 5);

            //black axe
            new CustomWeapon(BaseItem.WeaponModelType.Hammer,
                   108,
                   BuilderCore.Core.CreateMaterial(
                       new BuilderCore.BuildingData()
                       {
                           Metalic = 0.86f,
                           Smoothness = 0.66f,
                           MainColor = new Color(0.2f,0.2f,0.2f),
                       }
                       ),
                   new Vector3(0,0,0),
                   new Vector3(0,0,90),
                   new Vector3(0,0,-2f),
                   1.6f, 1f, 25, 250, 0f, 0f, 500, true, 6);


        }
    }
}
