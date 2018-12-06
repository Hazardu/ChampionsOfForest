using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Items.Inventory;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Player
{
    class PlayerInventoryMod : PlayerInventory
    {
        

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
}
