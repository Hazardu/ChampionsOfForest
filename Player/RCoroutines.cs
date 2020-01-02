//using ChampionsOfForest.Network;
//using System.Collections;
//using System.Collections.Generic;
//using TheForest.Items.Inventory;
//using TheForest.Items.World;
//using TheForest.Utils;
//using UnityEngine;
//namespace ChampionsOfForest.Player
//{
//    internal class PlayerInventoryMod : PlayerInventory
//    {

//        //public static Vector3 Rot;
//        //public static Vector3 Pos;
//        //public static float Sca;
//        public static PlayerInventoryMod instance;

//        public static InventoryItemView originalPlaneAxe;
//        public static Quaternion originalRotation;
//        public static Vector3 OriginalOffset;
//        public static bool SetupComplete;
//        public static GameObject originalPlaneAxeModel = null;
//        public static Transform originalParrent;
//        public static float OriginalTreeDmg;

//        public static Mesh noMesh;
//        public static Mesh originalMesh;



//        public static Dictionary<BaseItem.WeaponModelType, CustomWeapon> customWeapons = new Dictionary<BaseItem.WeaponModelType, CustomWeapon>();
//        public static BaseItem.WeaponModelType ToEquipWeaponType = BaseItem.WeaponModelType.None;
//        public static BaseItem.WeaponModelType EquippedModel = BaseItem.WeaponModelType.None;

//        //protected override void CheckQuickSelect()
//        //{
//        //    if (ModdedPlayer.instance.FastEquip|| this.CurrentView == PlayerInventory.PlayerViews.World && !TheForest.Utils.LocalPlayer.AnimControl.swimming && !TheForest.Utils.LocalPlayer.AnimControl.onRope && !TheForest.Utils.LocalPlayer.AnimControl.onRaft && !TheForest.Utils.LocalPlayer.AnimControl.sitting && !TheForest.Utils.LocalPlayer.AnimControl.upsideDown && !TheForest.Utils.LocalPlayer.AnimControl.skinningAnimal && !TheForest.Utils.LocalPlayer.AnimControl.endGameCutScene && !TheForest.Utils.LocalPlayer.AnimControl.useRootMotion && !TheForest.Utils.LocalPlayer.Create.CreateMode && !TheForest.Utils.LocalPlayer.AnimControl.PlayerIsAttacking())
//        //    {
//        //        bool flag = !TheForest.Utils.Input.IsGamePad || this.QuickSelectGamepadSwitch;
//        //        for (int i = 0; i < this._quickSelectItemIds.Length; i++)
//        //        {
//        //            if (this._quickSelectItemIds[i] > 0 && flag && TheForest.Utils.Input.GetButtonDown(this._quickSelectButtons[i]) && this.Owns(this._quickSelectItemIds[i], false))
//        //            {
//        //                var item = TheForest.Items.ItemDatabase.ItemById(this._quickSelectItemIds[i]);
//        //                if (item.MatchType(TheForest.Items.Item.Types.Equipment))
//        //                {
//        //                    if(ModdedPlayer.instance.FastEquip)
//        //                    {
//        //                        this.Equip(this._quickSelectItemIds[i], false);
//        //                        TheForest.Utils.LocalPlayer.Sfx.PlayWhoosh();
//        //                    }
//        //                    else if (this.Equip(this._quickSelectItemIds[i], false))
//        //                    {
//        //                        this.UnBlock();
//        //                        this.Blocking(true);
//        //                        TheForest.Utils.LocalPlayer.Sfx.PlayWhoosh();
//        //                    }
//        //                }
//        //                else if (item.MatchType(TheForest.Items.Item.Types.Edible))
//        //                {
//        //                    this.InventoryItemViewsCache[this._quickSelectItemIds[i]][0].UseEdible();
//        //                    foreach (TheForest.Items.World.QuickSelectViews quickSelectViews2 in TheForest.Utils.LocalPlayer.QuickSelectViews)
//        //                    {
//        //                        quickSelectViews2.ShowLocalPlayerViews();
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //}



//        [ModAPI.Attributes.Priority(1000)]
//        protected override bool Equip(InventoryItemView itemView, bool pickedUpFromWorld)
//        {
//            if (!ModSettings.IsDedicated)
//            {
//                if (GreatBow.instance != null)
//                    GreatBow.instance.SetActive(false);
//                if (itemView != null)
//                {
//                    EquippedModel = BaseItem.WeaponModelType.None;
//                    if (itemView._heldWeaponInfo.transform.parent.name == "AxePlaneHeld")
//                    {

//                        if (BoltNetwork.isRunning)
//                        {
//                            using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
//                            {
//                                using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
//                                {
//                                    w.Write(28);
//                                    w.Write(ModReferences.ThisPlayerID);
//                                    w.Write((int)PlayerInventoryMod.ToEquipWeaponType);
//                                    w.Close();
//                                }
//                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
//                                answerStream.Close();
//                            }
//                        }


//                        if (ModReferences.rightHandTransform == null)
//                        {
//                            try
//                            {
//                                ModReferences.rightHandTransform = itemView._heldWeaponInfo.transform.parent.gameObject.transform.parent.transform;
//                            }
//                            catch (System.Exception)
//                            {

//                            }
//                        }



//                        if (!SetupComplete)
//                        {
//                            try
//                            {
//                                if (instance == null)
//                                {
//                                    instance = this;
//                                }
//                                SetupComplete = true;
//                                customWeapons.Clear();
//                                originalPlaneAxe = itemView;
//                                originalPlaneAxeModel = itemView._heldWeaponInfo.transform.parent.GetChild(2).gameObject;
//                                originalRotation = originalPlaneAxeModel.transform.localRotation;
//                                OriginalOffset = originalPlaneAxeModel.transform.localPosition;
//                                originalParrent = originalPlaneAxeModel.transform.parent;
//                                OriginalTreeDmg = itemView._heldWeaponInfo.treeDamage;
//                                originalMesh = originalPlaneAxeModel.GetComponent<MeshFilter>().mesh;
//                                noMesh = new Mesh();


//                                //Creating custom weapons---------
//                                CreateCustomWeapons();



//                            }
//                            catch (System.Exception eee)
//                            {
//                                ModAPI.Log.Write("Error with setting up custom weaponry " + eee.ToString());
//                            }
//                        }
//                        if (ToEquipWeaponType != BaseItem.WeaponModelType.None)
//                        {
//                            EquippedModel = ToEquipWeaponType;
//                            try
//                            {


//                                foreach (CustomWeapon item in customWeapons.Values)
//                                {
//                                    item.obj.SetActive(false);
//                                }
//                                CustomWeapon cw = customWeapons[ToEquipWeaponType];
//                                cw.obj.SetActive(true);
//                                itemView._heldWeaponInfo.weaponSpeed = itemView._heldWeaponInfo.baseWeaponSpeed * cw.swingspeed;
//                                itemView._heldWeaponInfo.tiredSpeed = itemView._heldWeaponInfo.baseTiredSpeed * cw.tiredswingspeed;
//                                itemView._heldWeaponInfo.smashDamage = cw.smashDamage;
//                                itemView._heldWeaponInfo.weaponDamage = cw.damage;
//                                itemView._heldWeaponInfo.treeDamage = cw.treeDamage;
//                                itemView._heldWeaponInfo.weaponRange = cw.ColliderScale * 3;
//                                itemView._heldWeaponInfo.staminaDrain = cw.staminaDrain;
//                                itemView._heldWeaponInfo.noTreeCut = cw.blockTreeCut;
//                                itemView._heldWeaponInfo.transform.localScale = Vector3.one * cw.ColliderScale;
//                                originalPlaneAxeModel.GetComponent<MeshFilter>().mesh = noMesh;
//                            }
//                            catch (System.Exception exc)
//                            {

//                                ModAPI.Log.Write("Error with EQUIPPING custom weaponry " + exc.ToString());
//                            }

//                        }
//                        else
//                        {
//                            itemView._heldWeaponInfo.transform.parent.GetChild(2).gameObject.SetActive(true);
//                            foreach (CustomWeapon item in customWeapons.Values)
//                            {
//                                item.obj.SetActive(false);
//                            }
//                            itemView._heldWeaponInfo.weaponSpeed = itemView._heldWeaponInfo.baseWeaponSpeed;
//                            itemView._heldWeaponInfo.tiredSpeed = itemView._heldWeaponInfo.baseTiredSpeed;
//                            itemView._heldWeaponInfo.smashDamage = itemView._heldWeaponInfo.baseSmashDamage;
//                            itemView._heldWeaponInfo.weaponDamage = itemView._heldWeaponInfo.baseWeaponDamage;
//                            itemView._heldWeaponInfo.treeDamage = OriginalTreeDmg;
//                            itemView._heldWeaponInfo.weaponRange = itemView._heldWeaponInfo.baseWeaponRange;
//                            itemView._heldWeaponInfo.staminaDrain = itemView._heldWeaponInfo.baseStaminaDrain;
//                            itemView._heldWeaponInfo.noTreeCut = false;

//                            itemView._heldWeaponInfo.transform.localScale = Vector3.one * 0.6f;
//                            originalPlaneAxeModel.GetComponent<MeshFilter>().mesh = originalMesh;

//                        }
//                    }
//                    else if (BoltNetwork.isRunning)
//                    {
//                        using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
//                        {
//                            using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
//                            {
//                                w.Write(28);
//                                w.Write(ModReferences.ThisPlayerID);
//                                w.Write(0);
//                                w.Close();
//                            }
//                            ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
//                            answerStream.Close();
//                        }
//                    }
//                }
//            }



//            //if (ModdedPlayer.instance.FastEquip && itemView != null)
//            //{

//            //    _equipPreviousTime = float.MaxValue;
//            //    var equipmentSlot = itemView.ItemCache._equipmentSlot;
//            //    if ((Logs.HasLogs && equipmentSlot != TheForest.Items.Item.EquipmentSlot.LeftHand) || (TheForest.Utils.LocalPlayer.AnimControl.carry && equipmentSlot != TheForest.Items.Item.EquipmentSlot.LeftHand) || (TheForest.Utils.LocalPlayer.Create.CreateMode && itemView.ItemCache.MatchType(TheForest.Items.Item.Types.Projectile | TheForest.Items.Item.Types.RangedWeapon | TheForest.Items.Item.Types.Weapon)) || (!(itemView != _equipmentSlots[(int)equipmentSlot]) && !pickedUpFromWorld) || IsSlotLocked(equipmentSlot) || (itemView.ItemCache.MatchType(TheForest.Items.Item.Types.Special) && !SpecialItemsControlers[itemView._itemId].ToggleSpecial(true)))
//            //    {
//            //        return false;
//            //    }
//            //    if (pickedUpFromWorld || RemoveItem(itemView._itemId, 1, false, false))
//            //    {
//            //        LockEquipmentSlot(equipmentSlot);
//            //        StartCoroutine(CustomEquipSequence((int)equipmentSlot, itemView));
//            //        return true;
//            //    }
//            //    return false;     
//            //}else
//            return base.Equip(itemView, pickedUpFromWorld);
//        }



//        [ModAPI.Attributes.Priority(666)]
//        public override void Attack()
//        {
//            if (ModSettings.IsDedicated)
//            {
//                return;
//            }

//            if (!IsRightHandEmpty() && !_isThrowing && !IsReloading && !blockRangedAttack && !IsSlotLocked(TheForest.Items.Item.EquipmentSlot.RightHand) && !LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._slingShotId))
//            {
//                if (EquippedModel != BaseItem.WeaponModelType.None && customWeapons.ContainsKey(EquippedModel))
//                {
//                    customWeapons[EquippedModel].EnableTrail();
//                }
//                if (ModdedPlayer.instance.DeathPact_Enabled)
//                {
//                    LocalPlayer.Stats.Health -= ModdedPlayer.instance.MaxHealth * 0.07f;
//                    LocalPlayer.Stats.Health = Mathf.Max(1, LocalPlayer.Stats.Health);
//                }
//            }
//            base.Attack();
//        }
//        [ModAPI.Attributes.Priority(666)]
//        public override void AddMaxAmountBonus(int itemId, int amount)
//        {
//            if (ModSettings.IsDedicated)
//            {
//                return;
//            }

//            base.AddMaxAmountBonus(itemId, amount);
//        }
//        [ModAPI.Attributes.Priority(666)]
//        public override void SetMaxAmountBonus(int itemId, int amount)
//        {
//            base.SetMaxAmountBonus(itemId, amount);
//            if (ModdedPlayer.instance.ExtraCarryingCapactity.ContainsKey(itemId))
//            {
//                ModdedPlayer.instance.ExtraCarryingCapactity[itemId].NewApply();
//            }
//        }
//        [ModAPI.Attributes.Priority(666)]
//        public override void StashEquipedWeapon(bool equipPrevious)
//        {
//            if (GreatBow.instance != null)
//                GreatBow.instance.SetActive(false);
//            base.StashEquipedWeapon(equipPrevious);
//        }

//        [ModAPI.Attributes.Priority(666)]
//        public override void HideRightHand(bool hideOnly)
//        {
//            if (GreatBow.instance != null)
//                GreatBow.instance.SetActive(false);
//            base.HideRightHand(hideOnly);
//        }

//        //[ModAPI.Attributes.Priority(666)]
//        //protected override void ThrowProjectile()
//        //{
//        //    this._isThrowing = false;
//        //    InventoryItemView inventoryItemView = this._equipmentSlots[0];
//        //    if (inventoryItemView != null)
//        //    {
//        //        TheForest.Items.Item itemCache = inventoryItemView.ItemCache;
//        //        bool flag = itemCache._maxAmount < 0;
//        //        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((!this.UseAltWorldPrefab) ? inventoryItemView._worldPrefab : inventoryItemView._altWorldPrefab, inventoryItemView._held.transform.position, inventoryItemView._held.transform.rotation);
//        //        gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;
//        //        Rigidbody component = gameObject.GetComponent<Rigidbody>();
//        //        Collider component2 = gameObject.GetComponent<Collider>();
//        //        if (BoltNetwork.isRunning)
//        //        {
//        //            BoltEntity component3 = gameObject.GetComponent<BoltEntity>();
//        //            if (component3 != null)
//        //            {
//        //                BoltNetwork.Attach(gameObject);
//        //            }
//        //        }
//        //        Vector3 force = ((float)itemCache._projectileThrowForceRange) * ModdedPlayer.instance.ProjectileSpeedRatio * (0.016666f / Time.fixedDeltaTime) * TheForest.Utils.LocalPlayer.MainCamTr.forward;
//        //        if (SpellActions.BIA_bonusDamage > 0)
//        //        {
//        //            gameObject.transform.localScale *= 1.5f;
//        //            force *= 2f;
//        //            component.useGravity = false;
//        //            if (ModReferences.bloodInfusedMaterial == null)
//        //            {
//        //                ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
//        //                {

//        //                    EmissionColor = new Color(0.4f, 0, 0),
//        //                    renderMode = BuilderCore.BuildingData.RenderMode.Fade,
//        //                    MainColor = Color.red,
//        //                    Metalic = 1f,
//        //                    Smoothness = 0.8f,
//        //                });
//        //            }
//        //            //gameObject.GetComponent<Renderer>().material = bloodInfusedMaterial;
//        //            var trail = gameObject.AddComponent<TrailRenderer>();
//        //            trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
//        //            trail.material = ModReferences.bloodInfusedMaterial;
//        //            trail.widthMultiplier = 0.85f;
//        //            trail.time = 2f;
//        //            trail.autodestruct = false;
//        //        }
//        //        if (inventoryItemView.ActiveBonus == TheForest.Items.Craft.WeaponStatUpgrade.Types.StickyProjectile)
//        //        {
//        //            if (component2 != null)
//        //            {
//        //                gameObject.AddComponent<global::StickyBomb>();
//        //                SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
//        //                sphereCollider.isTrigger = true;
//        //                sphereCollider.radius = 0.8f;
//        //            }
//        //            else
//        //            {
//        //                Collider componentInChildren = gameObject.GetComponentInChildren<Collider>();
//        //                if (componentInChildren != null)
//        //                {
//        //                    componentInChildren.gameObject.AddComponent<global::StickyBomb>();
//        //                }
//        //            }
//        //        }

//        //        component.AddForce(force);

//        //        inventoryItemView._held.SendMessage("OnProjectileThrown", gameObject, SendMessageOptions.DontRequireReceiver);
//        //        inventoryItemView.ActiveBonus = (TheForest.Items.Craft.WeaponStatUpgrade.Types)(-1);
//        //        if (!global::ForestVR.Enabled)
//        //        {
//        //            if (itemCache._rangedStyle == TheForest.Items.Item.RangedStyle.Bell)
//        //            {
//        //                component.AddTorque((float)itemCache._projectileThrowTorqueRange * base.transform.forward);
//        //            }
//        //            else if (itemCache._rangedStyle == TheForest.Items.Item.RangedStyle.Forward)
//        //            {
//        //                component.AddTorque((float)itemCache._projectileThrowTorqueRange * TheForest.Utils.LocalPlayer.MainCamTr.forward);
//        //            }
//        //        }
//        //        if (base.transform.GetComponent<Collider>().enabled && component2 && component2.enabled)
//        //        {
//        //            Physics.IgnoreCollision(base.transform.GetComponent<Collider>(), component2);
//        //        }
//        //        if (!flag)
//        //        {
//        //            this.MemorizeOverrideItem(TheForest.Items.Item.EquipmentSlot.RightHand);
//        //        }
//        //        bool equipPrevious = true;
//        //        if (TheForest.Utils.LocalPlayer.FpCharacter.Sitting || TheForest.Utils.LocalPlayer.AnimControl.onRope || TheForest.Utils.LocalPlayer.FpCharacter.SailingRaft)
//        //        {
//        //            equipPrevious = false;
//        //        }
//        //        this.UnequipItemAtSlot(itemCache._equipmentSlot, false, false, equipPrevious);
//        //        TheForest.Utils.LocalPlayer.Sfx.PlayThrow();
//        //    }
//        //}

//        [ModAPI.Attributes.Priority(666)]
//        protected override void FireRangedWeapon()
//        {
//            Debug.LogWarning("Fire ranged");
//            var cache = _equipmentSlots[0].ItemCache;
//            bool noconsume = false;
//            if (ModdedPlayer.instance.ReusabilityChance >= 0 && Random.value < ModdedPlayer.instance.ReusabilityChance)
//            {
//                noconsume = true;
//            }
//            noconsume = noconsume || cache._maxAmount < 0 || RemoveItem(cache._ammoItemId, 1, false, true);

//            this.StartCoroutine(RCoroutines.i.AsyncRangedFire(this, _weaponChargeStartTime, _equipmentSlots[0], _inventoryItemViewsCache[cache._ammoItemId][0], noconsume));

//            _weaponChargeStartTime = 0f;
//            float duration = (!noconsume) ? cache._reloadDuration : cache._dryFireReloadDuration;
//            if (GreatBow.isEnabled) duration *= 10;
//            SetReloadDelay(duration);
//            _isThrowing = false;
//        }
//        //public IEnumerator CustomEquipSequence(int someslot, InventoryItemView renamedItemView)
//        //{
//        //    bool specialItemCheck = true;
//        //    var slot = (TheForest.Items.Item.EquipmentSlot)someslot;
//        //    if (_equipmentSlots[(int)slot] != null && _equipmentSlots[(int)slot] != _noEquipedItem)
//        //    {
//        //        _pendingEquip = true;
//        //        _equipmentSlotsNext[(int)slot] = renamedItemView;
//        //        bool canStash = _equipmentSlots[(int)slot].ItemCache._maxAmount >= 0;
//        //        if (canStash)
//        //        {
//        //            MemorizeItem(slot);
//        //        }
//        //        _itemAnimHash.ApplyAnimVars(_equipmentSlots[(int)slot].ItemCache, false);
//        //        int currentItemId = _equipmentSlots[(int)slot]._itemId;

//        //        if (!HasInSlot(slot, currentItemId) || !HasInNextSlot(slot, renamedItemView._itemId))
//        //        {
//        //            if (canStash)
//        //            {
//        //                AddItem(renamedItemView._itemId, 1, true, true, null);
//        //            }
//        //            else
//        //            {
//        //                FakeDrop(renamedItemView._itemId, null);
//        //            }
//        //            if (_equipmentSlotsNext[(int)slot] == renamedItemView)
//        //            {
//        //                _equipmentSlotsNext[(int)slot] = _noEquipedItem;
//        //            }
//        //            _pendingEquip = false;
//        //            yield break;
//        //        }
//        //        UnlockEquipmentSlot(slot);
//        //        if (renamedItemView.ItemCache.MatchType(TheForest.Items.Item.Types.Special))
//        //        {
//        //            specialItemCheck = SpecialItemsControlers[renamedItemView._itemId].ToggleSpecial(true);
//        //        }
//        //        if (specialItemCheck)
//        //        {
//        //            UnequipItemAtSlot(slot, !canStash, canStash, false);
//        //        }
//        //        _equipmentSlotsNext[(int)slot] = _noEquipedItem;
//        //    }
//        //    else if (renamedItemView.ItemCache.MatchType(TheForest.Items.Item.Types.Special))
//        //    {
//        //        specialItemCheck = SpecialItemsControlers[renamedItemView._itemId].ToggleSpecial(true);
//        //    }
//        //    if (specialItemCheck)
//        //    {
//        //        if (renamedItemView._held)
//        //        {
//        //            _equipmentSlots[(int)slot] = renamedItemView;
//        //            renamedItemView.OnItemEquipped();
//        //            renamedItemView._held.SetActive(true);
//        //            HeldItemIdentifier heldItem = renamedItemView._held.GetComponent<HeldItemIdentifier>();
//        //            if (heldItem != null)
//        //            {
//        //                heldItem.Properties.Copy(renamedItemView.Properties);
//        //            }
//        //            renamedItemView.ApplyEquipmentEffect(true);
//        //            _itemAnimHash.ApplyAnimVars(renamedItemView.ItemCache, true);
//        //            if (renamedItemView.ItemCache._equipedSFX != TheForest.Items.Item.SFXCommands.None)
//        //            {
//        //                TheForest.Utils.LocalPlayer.Sfx.SendMessage(renamedItemView.ItemCache._equipedSFX.ToString(), SendMessageOptions.DontRequireReceiver);
//        //            }
//        //            if (renamedItemView.ItemCache._maxAmount >= 0)
//        //            {
//        //                ToggleAmmo(renamedItemView, true);
//        //                ToggleInventoryItemView(renamedItemView._itemId, false, null);
//        //            }
//        //                          }

//        //        UnlockEquipmentSlot(slot);
//        //    }
//        //    _pendingEquip = false;
//        //    yield break;
//        //}
//        public void CreateCustomWeapons()
//        {
//            //long sword
//            new CustomWeapon(BaseItem.WeaponModelType.LongSword,
//                    51,
//                    BuilderCore.Core.CreateMaterial(
//                        new BuilderCore.BuildingData()
//                        {
//                            MainTexture = Res.ResourceLoader.instance.LoadedTextures[60],
//                            Metalic = 0.86f,
//                            Smoothness = 0.66f
//                        }
//                        ),
//                    new Vector3(0.2f - 0.04347827f, -1.5f + 0.173913f, 0.3f - 0.05797101f),
//                    new Vector3(0, -90, 0),
//                    new Vector3(-0.2f, -2.3f, 0),
//                    1.3f, 0.9f, 40, 80, 0.4f, 0.2f, 50, true, 5);


//            //great sword
//            new CustomWeapon(BaseItem.WeaponModelType.GreatSword,
//                52,
//                BuilderCore.Core.CreateMaterial(
//                    new BuilderCore.BuildingData()
//                    {
//                        OcclusionStrength = 0.75f,
//                        Smoothness = 0.6f,
//                        Metalic = 0.6f,
//                        MainTexture = Res.ResourceLoader.instance.LoadedTextures[61],
//                        EmissionMap = Res.ResourceLoader.instance.LoadedTextures[62],
//                        BumpMap = Res.ResourceLoader.instance.LoadedTextures[64],
//                        HeightMap = Res.ResourceLoader.instance.LoadedTextures[65],
//                        Occlusion = Res.ResourceLoader.instance.LoadedTextures[66]
//                    }
//                    ),
//                new Vector3(0.15f - 0.03623189f, -2.13f - 0.0572464f, 0.19f - 0.1014493f),
//                new Vector3(180, 180, 90),
//                new Vector3(0, 0, -3.5f),
//                1.8f, 1f, 60, 90, 0.01f, 0.001f, 85, false, 5);

//            //hammer
//            new CustomWeapon(BaseItem.WeaponModelType.Hammer,
//                   108,
//                   BuilderCore.Core.CreateMaterial(
//                       new BuilderCore.BuildingData()
//                       {
//                           Metalic = 0.86f,
//                           Smoothness = 0.66f,
//                           MainColor = new Color(0.2f, 0.2f, 0.2f),
//                       }
//                       ),
//                   new Vector3(0, 0, 0),
//                   new Vector3(0, 0, 90),
//                   new Vector3(0, 0, -2f),
//                   1.6f, 1f, 25, 250, 0f, 0f, 500, true, 6);

//            var Axe_PlaneAxe = GameObject.Instantiate(PlayerInventoryMod.originalPlaneAxeModel, PlayerInventoryMod.originalParrent);
//            var Axe_Renderer = Axe_PlaneAxe.GetComponent<Renderer>();
//            if (Axe_Renderer != null) Destroy(Axe_Renderer);
//            var Axe_Filter = Axe_PlaneAxe.GetComponent<MeshFilter>();
//            if (Axe_Filter != null) Destroy(Axe_Filter);


//            Vector3 AxeOffset = new Vector3(0.15f - 0.03623189f, -2.13f - 0.0572464f, 0.19f - 0.1014493f);// new Vector3(0.179f,-0.31f,0.026f);
//            Axe_PlaneAxe.transform.localScale = Vector3.one;
//            Vector3 AxeRotation = new Vector3(180, 180, 90);
//            Axe_PlaneAxe.transform.position += AxeOffset;
//            GameObject axeObject = Instantiate(Res.ResourceLoader.GetAssetBundle(2001).LoadAsset<GameObject>("AxePrefab.prefab"), Axe_PlaneAxe.transform);
//            Axe_PlaneAxe.transform.localPosition = PlayerInventoryMod.OriginalOffset;
//            Axe_PlaneAxe.transform.localRotation = PlayerInventoryMod.originalRotation;
//            Axe_PlaneAxe.transform.Rotate(AxeRotation, Space.Self);
//            axeObject.transform.localScale = Vector3.one;
//            var AxeTrail = axeObject.transform.GetChild(0).GetComponent<TrailRenderer>();
//            AxeTrail.transform.localPosition = new Vector3(0, -0.3f, 0);
//            new CustomWeapon(BaseItem.WeaponModelType.Axe, Axe_PlaneAxe, AxeTrail, AxeOffset, AxeRotation, 1)
//            {
//                blockTreeCut = false,
//                damage = 8,
//                smashDamage = 10,
//                staminaDrain = 4,
//                swingspeed = 50,
//                treeDamage = 10,
//                tiredswingspeed = 50,
//                ColliderScale = 0.4f

//            };
//            AxeTrail.gameObject.SetActive(false);

//        }
//    }
//    public class RCoroutines
//    {
//        public static RCoroutines i;
//        public RCoroutines()
//        {
//            i = this;
//        }


//        public IEnumerator AsyncCrossbowFire(int _ammoId, GameObject _ammoSpawnPosGo, GameObject _boltProjectile, crossbowController cc)
//        {
//            int repeats = ModdedPlayer.RangedRepetitions();
//            //Collider[] spawns = new Collider[repeats];
//            Vector3 updir = _ammoSpawnPosGo.transform.up;
//            Vector3 right = _ammoSpawnPosGo.transform.right;
//            Vector3 positionOriginal = _ammoSpawnPosGo.transform.position;
//            Quaternion rotation = _ammoSpawnPosGo.transform.rotation;
//            Vector3 forceUp = Vector3.zero;
//            for (int i = 0; i < repeats; i++)
//            {
//                bool noconsume = false;
//                if (ModdedPlayer.instance.ReusabilityChance >= 0 && Random.value < ModdedPlayer.instance.ReusabilityChance)
//                {
//                    noconsume = true;
//                }
//                if (noconsume || LocalPlayer.Inventory.RemoveItem(_ammoId, 1, false, true))
//                {
//                    Vector3 position = positionOriginal;
//                    if (i > 0)
//                    {
//                        position += 0.5f * updir * Mathf.Sin((i / 2f) * Mathf.PI) / 3;
//                        position += 0.4f * right * (((i) % 3) - 2);


//                    }

//                    GameObject gameObject = Object.Instantiate(_boltProjectile, position, rotation);
//                    gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;
//                    //var col = gameObject.GetComponent<Collider>();
//                    //if (col != null)
//                    //{
//                    //    for (int j = Mathf.Max(0, i - 25); j < i; j++)
//                    //    {
//                    //        if (spawns[j] != null)
//                    //            Physics.IgnoreCollision(spawns[j], col);
//                    //        spawns[i] = col;
//                    //    }
//                    //}
//                    //gameObject.AddComponent<ProjectileIgnoreCollision>();
//                    gameObject.layer = 19;
//                    Physics.IgnoreLayerCollision(19, 19, true);

//                    Rigidbody component = gameObject.GetComponent<Rigidbody>();
//                    if (BoltNetwork.isRunning)
//                    {
//                        BoltEntity component2 = gameObject.GetComponent<BoltEntity>();
//                        if ((bool)component2)
//                        {
//                            BoltNetwork.Attach(gameObject);
//                        }
//                    }
//                    PickUp componentInChildren = gameObject.GetComponentInChildren<PickUp>(true);
//                    if ((bool)componentInChildren)
//                    {
//                        SheenBillboard[] componentsInChildren = gameObject.GetComponentsInChildren<SheenBillboard>();
//                        SheenBillboard[] array = componentsInChildren;
//                        foreach (SheenBillboard sheenBillboard in array)
//                        {
//                            sheenBillboard.gameObject.SetActive(false);
//                        }
//                        componentInChildren.gameObject.SetActive(false);
//                        if (gameObject.activeInHierarchy)
//                        {
//                            cc.SendMessage("PublicEnablePickupTrigger", componentInChildren.gameObject);
//                        }
//                    }
//                    if (i == 0) forceUp = gameObject.transform.up;
//                    Vector3 force = 22000f * ModdedPlayer.instance.ProjectileSpeedRatio * (0.016666f / Time.fixedDeltaTime) * forceUp;
//                    if (SpellActions.BIA_bonusDamage > 0)
//                    {
//                        gameObject.transform.localScale *= 2f;
//                        force *= 2f;
//                        component.useGravity = false;
//                        if (ModReferences.bloodInfusedMaterial == null)
//                        {
//                            ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
//                            {

//                                EmissionColor = new Color(0.4f, 0, 0),
//                                renderMode = BuilderCore.BuildingData.RenderMode.Fade,
//                                MainColor = Color.red,
//                                Metalic = 1f,
//                                Smoothness = 0.8f,
//                            });
//                        }
//                        //gameObject.GetComponent<Renderer>().material = bloodInfusedMaterial;
//                        var trail = gameObject.AddComponent<TrailRenderer>();
//                        trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
//                        trail.material = ModReferences.bloodInfusedMaterial;
//                        trail.widthMultiplier = 0.75f;
//                        trail.time = 2f;
//                        trail.autodestruct = false;
//                    }
//                    component.AddForce(force);
//                }
//                if (i % 2 == 1)
//                    yield return null;
//            }
//        }

//        public IEnumerator AsyncRangedFire(PlayerInventory inv, float _weaponChargeStartTime, InventoryItemView inventoryItemView, InventoryItemView inventoryItemView2, bool noconsume)
//        {
//            TheForest.Items.Item itemCache = inventoryItemView.ItemCache;
//            bool flag = itemCache._maxAmount < 0;
//            int repeats = ModdedPlayer.RangedRepetitions();
//            CotfUtils.Log("repeats = " + repeats);
//            //Collider[] spawns = new Collider[repeats];
//            Vector3 forceUp = inventoryItemView2._held.transform.up;
//            Vector3 right = inventoryItemView2._held.transform.right;
//            Vector3 up = inventoryItemView2._held.transform.up;
//            Vector3 originalPos = inventoryItemView2._held.transform.position;
//            FakeParent component = inventoryItemView2._held.GetComponent<FakeParent>();
//            Quaternion rotation = inventoryItemView2._held.transform.rotation;
//            TheForest.Items.Item itemCache2 = inventoryItemView2.ItemCache;
//            for (int i = 0; i < repeats; i++)
//            {


//                if (noconsume)
//                {
//                    Vector3 pos = originalPos;
//                    if (i > 0)
//                    {
//                        // pos += 0.5f * up * (i + 1) / 3;
//                        pos += 0.5f * right * (((i - 1) % 3) - 1);
//                    }
//                    //if (i > 0 && i < 9)
//                    //{
//                    //    pos += right * Mathf.Cos(45 * (i - 1) * 3.14f / 360) * 0.5f + up * Mathf.Sin(45 * (i + 1) * 3.14f / 360) * 5f;
//                    //}
//                    //else if (i >= 9 && i < 22)
//                    //{
//                    //    pos +=right * Mathf.Cos(30 * (i + 5) * 3.14f / 360) * 1f +up * Mathf.Sin(30 * (i + 5) * 3.14f / 360) * 1f;
//                    //}
//                    //else if (i > 21)
//                    //{
//                    //    pos += right * Mathf.Cos(25 * (i) * 3.14f / 360) * 1.5f + up * Mathf.Sin(25 * (i) * 3.14f / 360) * 1.5f;
//                    //}

//                    GameObject gameObject = (!(bool)component || component.gameObject.activeSelf) ? Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, pos, rotation) : Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, pos, rotation);
//                    //var col = gameObject.GetComponent<Collider>();
//                    //if (col != null)
//                    //{
//                    //    for (int j = Mathf.Max(0, i - 18); j < i; j++)
//                    //    {
//                    //        if (spawns[j] != null)
//                    //            Physics.IgnoreCollision(spawns[j], col);
//                    //        spawns[i] = col;
//                    //    }
//                    //}
//                    gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;

//                    try
//                    {
//                        gameObject.transform.GetChild(0).gameObject.layer = 19;
//                        //gameObject.transform.GetChild(6).gameObject.layer = 19;

//                    }
//                    catch (System.Exception)
//                    {

//                        throw;
//                    }
//                    gameObject.layer = 19;
//                    Physics.IgnoreLayerCollision(19, 19, true);
//                    if (noconsume) GameObject.Destroy(gameObject, 3.5f);
//                    else
//                    {
//                        if (i >= 4 && i < 20) GameObject.Destroy(gameObject, 7);         //if spamming arrows, delete 4th and further after really show timespan
//                        else if (i >= 20) GameObject.Destroy(gameObject, 3.5f);
//                    }
//                    if ((bool)gameObject.GetComponent<Rigidbody>())
//                    {
//                        if (itemCache.MatchRangedStyle(TheForest.Items.Item.RangedStyle.Shoot))
//                        {
//                            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformDirection(Vector3.forward * (0.016666f / Time.fixedDeltaTime) * ModdedPlayer.instance.ProjectileSpeedRatio * itemCache._projectileThrowForceRange), ForceMode.VelocityChange);
//                        }
//                        else
//                        {
//                            float num = Time.time - _weaponChargeStartTime;
//                            if (ForestVR.Enabled)
//                            {
//                                gameObject.GetComponent<Rigidbody>().AddForce(inventoryItemView2._held.transform.up * ModdedPlayer.instance.ProjectileSpeedRatio * itemCache._projectileThrowForceRange);
//                            }
//                            else
//                            {

//                                Vector3 proj_force = forceUp * ModdedPlayer.instance.ProjectileSpeedRatio * Mathf.Clamp01(num / itemCache._projectileMaxChargeDuration) * (0.016666f / Time.fixedDeltaTime) * itemCache._projectileThrowForceRange;
//                                var proj_rb = gameObject.GetComponent<Rigidbody>();
//                                if (GreatBow.isEnabled)
//                                {
//                                    proj_force *= 1.1f;
//                                    proj_rb.useGravity = false;

//                                }
//                                if (SpellActions.BIA_bonusDamage > 0)
//                                {
//                                    proj_force *= 1.1f;
//                                    proj_rb.useGravity = false;
//                                    if (ModReferences.bloodInfusedMaterial == null)
//                                    {
//                                        ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
//                                        {

//                                            EmissionColor = new Color(0.6f, 0, 0),
//                                            renderMode = BuilderCore.BuildingData.RenderMode.Opaque,
//                                            MainColor = Color.red,
//                                            Metalic = 1f,
//                                            Smoothness = 0.9f,
//                                        });
//                                    }
//                                    //gameObject.GetComponent<Renderer>().material = bloodInfusedMaterial;
//                                    var trail = gameObject.AddComponent<TrailRenderer>();
//                                    trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
//                                    trail.material = ModReferences.bloodInfusedMaterial;
//                                    trail.widthMultiplier = 0.85f;
//                                    trail.time = 2.5f;
//                                    trail.autodestruct = false;
//                                }
//                                proj_rb.AddForce(proj_force);

//                            }
//                            if (LocalPlayer.Inventory.HasInSlot(TheForest.Items.Item.EquipmentSlot.RightHand, LocalPlayer.AnimControl._bowId))
//                            {
//                                gameObject.SendMessage("setCraftedBowDamage", SendMessageOptions.DontRequireReceiver);
//                            }
//                        }
//                        inventoryItemView._held.SendMessage("OnAmmoFired", gameObject, SendMessageOptions.DontRequireReceiver);
//                    }
//                    if (itemCache._attackReleaseSFX != 0)
//                    {
//                        LocalPlayer.Sfx.SendMessage(itemCache._attackReleaseSFX.ToString(), SendMessageOptions.DontRequireReceiver);
//                    }
//                    Mood.HitRumble();
//                }
//                else
//                {
//                    if (itemCache._dryFireSFX != 0)
//                    {
//                        LocalPlayer.Sfx.SendMessage(itemCache._dryFireSFX.ToString(), SendMessageOptions.DontRequireReceiver);
//                    }
//                }
//                if (i % 2 == 1)
//                    yield return null;
//            }
//            if (flag)
//            {
//                inv.UnequipItemAtSlot(itemCache._equipmentSlot, false, false, flag);
//            }
//            else
//            {
//                inv.ToggleAmmo(inventoryItemView, true);
//            }

//            yield break;
//        }

//    }

//}
using System.Collections;
using TheForest.Items.Inventory;
using TheForest.Items.World;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class RCoroutines
    {
        public static RCoroutines i;
        public RCoroutines()
        {
            i = this;
        }


        public IEnumerator AsyncCrossbowFire(int _ammoId, GameObject _ammoSpawnPosGo, GameObject _boltProjectile, crossbowController cc)
        {
            int repeats = ModdedPlayer.RangedRepetitions();
            //Collider[] spawns = new Collider[repeats];
            Vector3 updir = _ammoSpawnPosGo.transform.up;
            Vector3 right = _ammoSpawnPosGo.transform.right;
            Vector3 positionOriginal = _ammoSpawnPosGo.transform.position;
            Quaternion rotation = _ammoSpawnPosGo.transform.rotation;
            Vector3 forceUp = Vector3.zero;
            for (int i = 0; i < repeats; i++)
            {
                bool noconsume = false;
                if (ModdedPlayer.instance.ReusabilityChance >= 0 && Random.value < ModdedPlayer.instance.ReusabilityChance)
                {
                    noconsume = true;
                }
                if (noconsume || LocalPlayer.Inventory.RemoveItem(_ammoId, 1, false, true))
                {
                    Vector3 position = positionOriginal;
                    if (i > 0)
                    {
                        position += 0.5f * updir * Mathf.Sin((i / 2f) * Mathf.PI) / 3;
                        position += 0.4f * right * (((i) % 3) - 2);


                    }

                    GameObject gameObject = Object.Instantiate(_boltProjectile, position, rotation);
                    gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;
                    //var col = gameObject.GetComponent<Collider>();
                    //if (col != null)
                    //{
                    //    for (int j = Mathf.Max(0, i - 25); j < i; j++)
                    //    {
                    //        if (spawns[j] != null)
                    //            Physics.IgnoreCollision(spawns[j], col);
                    //        spawns[i] = col;
                    //    }
                    //}
                    //gameObject.AddComponent<ProjectileIgnoreCollision>();
                    gameObject.layer = 19;
                    Physics.IgnoreLayerCollision(19, 19, true);

                    Rigidbody component = gameObject.GetComponent<Rigidbody>();
                    if (BoltNetwork.isRunning)
                    {
                        BoltEntity component2 = gameObject.GetComponent<BoltEntity>();
                        if ((bool)component2)
                        {
                            BoltNetwork.Attach(gameObject);
                        }
                    }
                    PickUp componentInChildren = gameObject.GetComponentInChildren<PickUp>(true);
                    if ((bool)componentInChildren)
                    {
                        SheenBillboard[] componentsInChildren = gameObject.GetComponentsInChildren<SheenBillboard>();
                        SheenBillboard[] array = componentsInChildren;
                        foreach (SheenBillboard sheenBillboard in array)
                        {
                            sheenBillboard.gameObject.SetActive(false);
                        }
                        componentInChildren.gameObject.SetActive(false);
                        if (gameObject.activeInHierarchy)
                        {
                            cc.SendMessage("PublicEnablePickupTrigger", componentInChildren.gameObject);
                        }
                    }
                    if (i == 0) forceUp = gameObject.transform.up;
                    Vector3 force = 22000f * ModdedPlayer.instance.ProjectileSpeedRatio * (0.016666f / Time.fixedDeltaTime) * forceUp;
                    if (SpellActions.BIA_bonusDamage > 0)
                    {
                        gameObject.transform.localScale *= 2f;
                        force *= 2f;
                        component.useGravity = false;
                        if (ModReferences.bloodInfusedMaterial == null)
                        {
                            ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
                            {

                                EmissionColor = new Color(0.6f, 0, 0),
                                renderMode = BuilderCore.BuildingData.RenderMode.Fade,
                                MainColor = Color.red,
                                Metalic = 1f,
                                Smoothness = 0.9f,
                            });
                        }
                        //gameObject.GetComponent<Renderer>().material = bloodInfusedMaterial;
                        var trail = gameObject.AddComponent<TrailRenderer>();
                        trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
                        trail.material = ModReferences.bloodInfusedMaterial;
                        trail.widthMultiplier = 0.85f;
                        trail.time = 2.5f;
                        trail.autodestruct = false;
                    }
                    component.AddForce(force);
                }
                if (i % 2 == 1)
                    yield return null;
            }
        }

        public IEnumerator AsyncRangedFire(PlayerInventory inv, float _weaponChargeStartTime, InventoryItemView inventoryItemView, InventoryItemView inventoryItemView2, bool noconsume)
        {
            TheForest.Items.Item itemCache = inventoryItemView.ItemCache;
            bool flag = itemCache._maxAmount < 0;
            int repeats = ModdedPlayer.RangedRepetitions();
            CotfUtils.Log("repeats = " + repeats);
            //Collider[] spawns = new Collider[repeats];
            Vector3 forceUp = inventoryItemView2._held.transform.up;
            Vector3 right = inventoryItemView2._held.transform.right;
            Vector3 up = inventoryItemView2._held.transform.up;
            Vector3 originalPos = inventoryItemView2._held.transform.position;
            FakeParent component = inventoryItemView2._held.GetComponent<FakeParent>();
            Quaternion rotation = inventoryItemView2._held.transform.rotation;
            TheForest.Items.Item itemCache2 = inventoryItemView2.ItemCache;
            for (int i = 0; i < repeats; i++)
            {


                if (noconsume)
                {
                    Vector3 pos = originalPos;
                    if (i > 0)
                    {
                        // pos += 0.5f * up * (i + 1) / 3;
                        pos += 0.5f * right * (((i - 1) % 3) - 1);
                    }
                    //if (i > 0 && i < 9)
                    //{
                    //    pos += right * Mathf.Cos(45 * (i - 1) * 3.14f / 360) * 0.5f + up * Mathf.Sin(45 * (i + 1) * 3.14f / 360) * 5f;
                    //}
                    //else if (i >= 9 && i < 22)
                    //{
                    //    pos +=right * Mathf.Cos(30 * (i + 5) * 3.14f / 360) * 1f +up * Mathf.Sin(30 * (i + 5) * 3.14f / 360) * 1f;
                    //}
                    //else if (i > 21)
                    //{
                    //    pos += right * Mathf.Cos(25 * (i) * 3.14f / 360) * 1.5f + up * Mathf.Sin(25 * (i) * 3.14f / 360) * 1.5f;
                    //}

                    GameObject gameObject = (!(bool)component || component.gameObject.activeSelf) ? Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, pos, rotation) : Object.Instantiate(itemCache2._ammoPrefabs.GetPrefabForBonus(inventoryItemView.ActiveBonus, true).gameObject, pos, rotation);
                    //var col = gameObject.GetComponent<Collider>();
                    //if (col != null)
                    //{
                    //    for (int j = Mathf.Max(0, i - 18); j < i; j++)
                    //    {
                    //        if (spawns[j] != null)
                    //            Physics.IgnoreCollision(spawns[j], col);
                    //        spawns[i] = col;
                    //    }
                    //}
                    gameObject.transform.localScale *= ModdedPlayer.instance.ProjectileSizeRatio;

                    try
                    {
                        gameObject.transform.GetChild(0).gameObject.layer = 19;
                        //gameObject.transform.GetChild(6).gameObject.layer = 19;

                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                    gameObject.layer = 19;
                    Physics.IgnoreLayerCollision(19, 19, true);
                    if (noconsume) GameObject.Destroy(gameObject, 5f);
                    else
                    {
                        if (i >= 4 ) GameObject.Destroy(gameObject, 7);         //if spamming arrows, delete 4th and further after really show timespan
                    }
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

                                Vector3 proj_force = forceUp * ModdedPlayer.instance.ProjectileSpeedRatio * Mathf.Clamp01(num / itemCache._projectileMaxChargeDuration) * (0.016666f / Time.fixedDeltaTime) * itemCache._projectileThrowForceRange;
                                var proj_rb = gameObject.GetComponent<Rigidbody>();
                                if (GreatBow.isEnabled)
                                {
                                    proj_force *= 1.1f;
                                    proj_rb.useGravity = false;

                                }
                                if (SpellActions.BIA_bonusDamage > 0)
                                {
                                    proj_force *= 1.1f;
                                    proj_rb.useGravity = false;
                                    if (ModReferences.bloodInfusedMaterial == null)
                                    {
                                        ModReferences.bloodInfusedMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
                                        {

                                            EmissionColor = new Color(0.6f, 0, 0),
                                            renderMode = BuilderCore.BuildingData.RenderMode.Opaque,
                                            MainColor = Color.red,
                                            Metalic = 1f,
                                            Smoothness = 0.9f,
                                        });
                                    }
                                    //gameObject.GetComponent<Renderer>().material = bloodInfusedMaterial;
                                    var trail = gameObject.AddComponent<TrailRenderer>();
                                    trail.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0.006248474f, 0f, 0f), });
                                    trail.material = ModReferences.bloodInfusedMaterial;
                                    trail.widthMultiplier = 0.85f;
                                    trail.time = 2.5f;
                                    trail.autodestruct = false;
                                }
                                proj_rb.AddForce(proj_force);

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
                    if (itemCache._dryFireSFX != 0)
                    {
                        LocalPlayer.Sfx.SendMessage(itemCache._dryFireSFX.ToString(), SendMessageOptions.DontRequireReceiver);
                    }
                }
                if (i % 2 == 1)
                    yield return null;
            }
            if (flag)
            {
                inv.UnequipItemAtSlot(itemCache._equipmentSlot, false, false, flag);
            }
            else
            {
                inv.ToggleAmmo(inventoryItemView, true);
            }

            yield break;
        }

    }

}