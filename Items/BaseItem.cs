using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest
{
    public class BaseItem
    {
        public enum ItemType{Shield,Offhand, Weapon, Other, Material,Helmet,Boot,Pants,ChestArmor,ShoulderArmor,Glove,Bracer,Amulet,Ring}
        public delegate void OnItemEquip();
        public delegate void OnItemUnequip();
        public delegate void OnItemConsume();


        public List<List<ItemStat>> PossibleStats;
        public int Rarity;                  //rarity to display in different color
        public int ID;                      //the id of an item
        public int StackSize;               //how many items can be placed in one item slot?
        public bool CanConsume;             //can you eat this bad boy?
        public ItemType _itemType;          //determines on which inv slot can this item be placed in
        
        public OnItemEquip onEquip;
        public OnItemUnequip onUnequip;
        public OnItemConsume onConsume;     
        public string name;                 //name of item, 
        public string description;          //what is this item basically
        public string lore;                 //some cool story to make this item have any sense, or a place for a joke
        public string tooltip;              //what should be displayed in the tooltip of this item
        public int level;                   //level of this item
        public Texture2D icon;              //texture of this item
        public bool PickUpAll;              //should the item be picked one by one, or grab all at once


        public BaseItem()
        {

        }
        public BaseItem(List<List<ItemStat>> possibleStats, int rarity, int iD, int StackSize , ItemType itemType, string name, string description, string lore, string tooltip, int level, Texture2D texture,bool pickupAll = false)
        { 
            PossibleStats = possibleStats;
            Rarity = rarity;
            ID = iD;
            this.StackSize = StackSize;
            _itemType = itemType;
            PickUpAll = pickupAll;
            this.name = name;
            this.description = description;
            this.lore = lore;
            this.tooltip = tooltip;
            this.level = level;
            ItemDataBase.Instance._Item_Bases.Add(this);
            icon = texture;
        }
    }
}
