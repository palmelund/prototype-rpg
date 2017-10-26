using System.Collections.Generic;
using Global;
using UnityEngine;

namespace Items
{
    public enum ItemEquipmentType
    {
        NotEquippable,
        Head,
        Body,
        Legs,
        OneHand,
        TwoHand
    }
    
    public abstract class EquippableItem : Item
    {
        public abstract void OnEquip();

        public abstract void OnUnequip();
    }

    public class HandItem : EquippableItem
    {
        public override void OnEquip()
        {
        }

        public override void OnUnequip()
        {
        }
    }

    public class HeadArmor : EquippableItem
    {
        public override void OnEquip()
        {
        }

        public override void OnUnequip()
        {
        }
    }

    public class BodyArmor : EquippableItem
    {
        public override void OnEquip()
        {
        }

        public override void OnUnequip()
        {
        }
    }

    public class LegArmor : EquippableItem
    {
        public override void OnEquip()
        {
        }

        public override void OnUnequip()
        {
        }
    }

    public class Junk : Item
    {
        
    }

    public abstract class Item
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ItemEquipmentType ItemEquipmentType { get; set; }

        protected Item()
        {

        }

        protected Item(string identifier, string name, string description)
        {

        }
    }

    public class Key : Item
    {
        public List<string> DoorReferences { get; set; }

        public Key()
        {

        }

        public Key(string identifier, string name, string description, List<string> doorReferences) : base(identifier, name, description)
        {
            DoorReferences = doorReferences;
        }
    }
}
