using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Items;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // TODO: Make the dual check against the target slot to see if the move is legal => Currently we can equip a dual item places they don't belong!!!
    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public PlayerInventory PlayerInventory = new PlayerInventory();

    public void UpdatePlayerInventory()
    {
        // This should be called after each operation to ensure that the inventory model matches the shown model.
        foreach (var inventorySlotComponent in GameObject.FindObjectsOfType<InventorySlotComponent>())
        {
            if (inventorySlotComponent.InventorySlotType == InventorySlotType.Inventory)
            {
                PlayerInventory.InventoryItems[inventorySlotComponent.InventorySlotNumber] = (inventorySlotComponent.ItemComponent != null) ? inventorySlotComponent.ItemComponent.Item : null;
            }
            else
            {
                PlayerInventory.EquippedItems[(int)inventorySlotComponent.InventorySlotType] = (inventorySlotComponent.ItemComponent != null) ? inventorySlotComponent.ItemComponent .Item as EquippableItem : null;
            }
        }
    }

    public bool MoveItem(InventorySlotComponent from, InventorySlotComponent to)
    {
        Debug.Log("BEGIN MOVE!");
        if (to.ItemComponent == null) // Move
        {
            Debug.Log("Move");
            if (from.InventorySlotType == InventorySlotType.Inventory)
            {
                Debug.Log("From Inventory");
                if (to.InventorySlotType == InventorySlotType.Inventory)
                {
                    Debug.Log("Moving item in inventory!");
                    // Move Items in inventory
                    return MoveItemInInventory(from, to);
                }
                else
                {
                    Debug.Log("Equip item!");
                    // Equip item
                    if (from.ItemComponent.Item is HandItem &&
                        from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.OneHand &&
                        (to.InventorySlotType == InventorySlotType.RightHand ||
                         to.InventorySlotType == InventorySlotType.LeftHand)) // Equip one-handed item
                    {
                        Debug.Log("Equip One Hand!");
                        return EquipOneHand(from, to);
                    }
                    else if (from.ItemComponent.Item is HandItem &&
                             from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand &&
                             (to.InventorySlotType == InventorySlotType.RightHand ||
                              to.InventorySlotType == InventorySlotType.LeftHand)) // Equip two-handed item
                    {
                        Debug.Log("Equip Dual Hand!");

                        if (GetRightHand.ItemComponent != null || GetLeftHand.ItemComponent != null)
                        {
                            Debug.Log("Unequipping Other Hand And Equipping Dual Hand");
                            return EquipDualUnequipOne(from, to);
                        }
                        else
                        {
                            Debug.Log("Equipping Dual Hand!");
                            return EquipDualHand(from, to);
                        }
                    }
                    else if (from.ItemComponent.Item is EquippableItem && !(from.ItemComponent.Item is HandItem)
                    ) // Equip other item
                    {
                        Debug.Log("Equip Item");
                        return EquipEquipment(from, to);
                    }
                    else // Do nothing
                    {
                        Debug.Log("Cannot equip item!");
                        return false;
                    }
                }
            }
            else
            {
                Debug.Log("From Equipped");
                if (to.InventorySlotType == InventorySlotType.Inventory)
                {
                    Debug.Log("Unequipping item!");
                    // Unequip item
                    if (from.ItemComponent.Item is HandItem &&
                        from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.OneHand &&
                        (from.InventorySlotType == InventorySlotType.RightHand ||
                         from.InventorySlotType == InventorySlotType.LeftHand)) // Unequip hand
                    {
                        Debug.Log("Unequipping One Hand");
                        return UnequipOneHand(from, to);
                    }
                    else if (from.ItemComponent.Item is HandItem &&
                             from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand &&
                             (from.InventorySlotType == InventorySlotType.RightHand ||
                              from.InventorySlotType == InventorySlotType.LeftHand)) // Unequip borg hands
                    {
                        Debug.Log("Unequiping Two Hands");
                        return UnequipDualHand(from, to);
                    }
                    else if (from.ItemComponent.Item is EquippableItem && !(from.ItemComponent.Item is HandItem)
                    ) // Unequip item
                    {
                        Debug.Log("Unequipping Item");
                        return UnequipEquipment(from, to);
                    }
                    else
                    {
                        Debug.Log("Cannot unequip item");
                        return false;
                    }
                }
                else
                {
                    Debug.Log("Swithing Hand");
                    // Switch hand
                    if (from.InventorySlotType == InventorySlotType.RightHand &&
                        to.InventorySlotType == InventorySlotType.LeftHand ||
                        to.InventorySlotType == InventorySlotType.RightHand &&
                        from.InventorySlotType == InventorySlotType.LeftHand)
                    {
                        Debug.Log("Swap Hand");
                        return MoveItemInHands(from, to);
                    }
                    else
                    {
                        Debug.Log("Cannot swap items!");
                    }
                }
            }
        }
        else // Swap
        {
            Debug.Log("Swap");
            if (from.InventorySlotType == InventorySlotType.Inventory)
            {
                Debug.Log("From Inventory");
                if (to.InventorySlotType == InventorySlotType.Inventory)
                {
                    Debug.Log("Swap Items In Inventory");
                    // Swap items in inventory
                    return MoveItemInInventory(from, to);

                }
                else
                {
                    Debug.Log("Swapping Inventory With Equipped Item");
                    // Swap with equipped item
                    if (from.ItemComponent.Item is HandItem &&
                        from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.OneHand &&
                        (to.InventorySlotType == InventorySlotType.RightHand ||
                         to.InventorySlotType == InventorySlotType.LeftHand)) // Equip one-handed item
                    {
                        Debug.Log("Swapping Inventory One Hand");
                        if (to.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand)
                        {
                            Debug.Log("Swapping With Two Hand");
                            return EquipOneUnequipDual(from, to);
                        }
                        else
                        {
                            Debug.Log("Swapping With One Hand");
                            return EquipOneUnequipOne(from, to);
                        }

                    }
                    else if (from.ItemComponent.Item is HandItem &&
                             from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand &&
                             (to.InventorySlotType == InventorySlotType.RightHand ||
                              to.InventorySlotType == InventorySlotType.LeftHand)) // Equip two-handed item
                    {
                        Debug.Log("Swapping Inventory Two Hand");
                        if (to.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand)
                        {
                            Debug.Log("Swapping With Two Hand");
                            return EquipDualUnequipDual(from, to);
                        }
                        else
                        {
                            Debug.Log("Swapping With One Hand");
                            return EquipDualUnequipOne(from, to);
                        }
                    }
                    else if (from.ItemComponent.Item is EquippableItem && !(from.ItemComponent.Item is HandItem)
                    ) // Equip other item
                    {
                        Debug.Log("Swapping Equipment With Equipment");
                        return EquipItemUnequipItem(from, to);
                    }
                    else // Do nothing
                    {
                        Debug.Log("Cannot equip item!");
                        return false;
                    }
                }
            }
            else
            {
                Debug.Log("From Equipment");
                if (to.InventorySlotType == InventorySlotType.Inventory)
                {
                    Debug.Log("To Inventory");
                    // Swap with item in inventory
                    // Unequip item
                    if (from.ItemComponent.Item is HandItem &&
                        from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.OneHand &&
                        (from.InventorySlotType == InventorySlotType.RightHand ||
                         from.InventorySlotType == InventorySlotType.LeftHand)) // Unequip hand
                    {
                        Debug.Log("Unequip One Hand");
                        if (to.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand)
                        {
                            Debug.Log("Equip Two Hand");
                            return EquipDualUnequipOne(to, from);
                        }
                        else
                        {
                            Debug.Log("Equip One Hand");
                            return EquipOneUnequipOne(to, from);
                        }
                    }
                    else if (from.ItemComponent.Item is HandItem &&
                             from.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand &&
                             (from.InventorySlotType == InventorySlotType.RightHand ||
                              from.InventorySlotType == InventorySlotType.LeftHand)) // Unequip borg hands
                    {
                        Debug.Log("Unequip Dual Hand");
                        if (to.ItemComponent.Item.ItemEquipmentType == ItemEquipmentType.TwoHand)
                        {
                            Debug.Log("Equip Dual Hand");
                            return EquipDualUnequipDual(to, from);
                        }
                        else
                        {
                            Debug.Log("Equip One Hand");
                            return EquipOneUnequipDual(to, from);
                        }
                    }
                    else if (from.ItemComponent.Item is EquippableItem && !(from.ItemComponent.Item is HandItem)
                    ) // Unequip item
                    {
                        Debug.Log("Unequip Equipment, Equip Other");
                        return EquipItemUnequipItem(to, from);
                    }
                    else
                    {
                        Debug.Log("Cannot unequip item");
                        return false;
                    }
                }
                else
                {
                    // Swap items in hand
                    Debug.Log("Swap items in hand!");
                    return MoveItemInHands(from, to);
                }
            }
        }

        Debug.Log("Some other non-permitted movement happened!");
        return false;
    }

    private InventorySlotComponent GetRightHand => GameObject.FindObjectsOfType<InventorySlotComponent>().First(component => component.InventorySlotType == InventorySlotType.RightHand);
    private InventorySlotComponent GetLeftHand => GameObject.FindObjectsOfType<InventorySlotComponent>().First(component => component.InventorySlotType == InventorySlotType.LeftHand);

    #region Move Item Methods

    private bool UnequipDualHand(InventorySlotComponent from, InventorySlotComponent to)
    {
        to.ItemComponent = from.ItemComponent;
        to.ItemComponent.transform.position = to.transform.position;
        GetLeftHand.ItemComponent = null;
        GetRightHand.ItemComponent = null;
        return true;
    }

    private bool EquipDualHand(InventorySlotComponent from, InventorySlotComponent to)
    {
        GetRightHand.ItemComponent = from.ItemComponent;
        GetLeftHand.ItemComponent = from.ItemComponent;
        from.ItemComponent = null;
        GetRightHand.ItemComponent.transform.position = GetRightHand.transform.position;
        return true;
    }

    private bool UnequipOneHand(InventorySlotComponent from, InventorySlotComponent to)
    {
        to.ItemComponent = from.ItemComponent;
        from.ItemComponent = null;
        to.ItemComponent.transform.position = to.transform.position;
        return true;
    }

    private bool EquipOneHand(InventorySlotComponent from, InventorySlotComponent to)
    {
        to.ItemComponent = from.ItemComponent;
        from.ItemComponent = null;
        to.ItemComponent.transform.position = to.transform.position;
        return true;
    }

    private bool EquipEquipment(InventorySlotComponent from, InventorySlotComponent to)
    {
        switch (from.ItemComponent.Item.ItemEquipmentType)
        {
            case ItemEquipmentType.Head:
                if (to.InventorySlotType != InventorySlotType.Head)
                {
                    return false;
                }
                break;
            case ItemEquipmentType.Body:
                if (to.InventorySlotType != InventorySlotType.Body)
                {
                    return false;
                }
                break;
            case ItemEquipmentType.Legs:
                if (to.InventorySlotType != InventorySlotType.Legs)
                {
                    return false;
                }
                break;
            default:
                Debug.Log("Cannot equip item!");
                return false;
        }


        to.ItemComponent = from.ItemComponent;
        from.ItemComponent = null;
        to.ItemComponent.transform.position = to.transform.position;
        return true;
    }

    private bool UnequipEquipment(InventorySlotComponent from, InventorySlotComponent to)
    {
        to.ItemComponent = from.ItemComponent;
        from.ItemComponent = null;
        to.ItemComponent.transform.position = to.transform.position;
        return true;
    }

    #endregion

    #region Swap Item Methods

    private bool EquipOneUnequipOne(InventorySlotComponent from, InventorySlotComponent to)
    {
        var tmp = from.ItemComponent;
        from.ItemComponent = to.ItemComponent;
        to.ItemComponent = tmp;

        from.ItemComponent.transform.position = from.transform.position;
        to.ItemComponent.transform.position = to.transform.position;
        return true;
    }

    private bool EquipOneUnequipDual(InventorySlotComponent from, InventorySlotComponent to)
    {
        var tmp = from.ItemComponent;

        from.ItemComponent = GetRightHand.ItemComponent;
        GetRightHand.ItemComponent = null;
        GetLeftHand.ItemComponent = null;
        from.ItemComponent.transform.position = from.transform.position;

        to.ItemComponent = tmp;
        to.ItemComponent.transform.position = to.transform.position;
        return true;
    }

    private bool EquipDualUnequipOne(InventorySlotComponent from, InventorySlotComponent to)
    {
        var tmp = from.ItemComponent;
        from.ItemComponent = null;
        var availableInventorySpaces = new Queue<InventorySlotComponent>(GameObject.FindObjectsOfType<InventorySlotComponent>().Where(component => component.InventorySlotType == InventorySlotType.Inventory).Reverse().ToList());

        if (GetRightHand.ItemComponent != null && GetLeftHand.ItemComponent != null && availableInventorySpaces.Count < 2)
        {
            Debug.Log("Not enought inventory spaces!");
            from.ItemComponent = tmp;
            return false;
        }

        if (GetRightHand.ItemComponent != null)
        {
            var rto = availableInventorySpaces.Dequeue();
            rto.ItemComponent = GetRightHand.ItemComponent;
            rto.ItemComponent.transform.position = rto.transform.position;
            GetRightHand.ItemComponent = null;
        }
        if (GetLeftHand.ItemComponent != null)
        {
            var lto = availableInventorySpaces.Dequeue();
            lto.ItemComponent = GetLeftHand.ItemComponent;
            lto.ItemComponent.transform.position = lto.transform.position;
            GetLeftHand.ItemComponent = null;
        }

        GetRightHand.ItemComponent = tmp;
        GetLeftHand.ItemComponent = tmp;
        tmp.transform.position = GetRightHand.transform.position;
        return true;
    }

    private bool EquipDualUnequipDual(InventorySlotComponent from, InventorySlotComponent to)
    {
        var tmp = from.ItemComponent;
        from.ItemComponent = to.ItemComponent;

        GetRightHand.ItemComponent = tmp;
        GetLeftHand.ItemComponent = tmp;

        tmp.transform.position = GetRightHand.transform.position;
        return true;
    }

    private bool EquipItemUnequipItem(InventorySlotComponent from, InventorySlotComponent to)
    {
        if (from.ItemComponent.Item.ItemEquipmentType != to.ItemComponent.Item.ItemEquipmentType)
        {
            Debug.Log("Trying to mix types!");
            return false;
        }

        var tmp = from.ItemComponent;
        from.ItemComponent = to.ItemComponent;
        to.ItemComponent = tmp;

        from.ItemComponent.transform.position = from.transform.position;
        to.ItemComponent.transform.position = to.transform.position;

        return true;
    }

#endregion

    // TODO: Split?

    public bool MoveItemInInventory(InventorySlotComponent from, InventorySlotComponent to)
    {

        if (to.ItemComponent != null)   // Swap two items
        {
            var tmp = from.ItemComponent;
            from.ItemComponent = to.ItemComponent;
            to.ItemComponent = tmp;

            from.ItemComponent.transform.position = from.transform.position;
            to.ItemComponent.transform.position = to.transform.position;
            // TODO: Update models
            return true;
        }
        else    // Move item
        {
            to.ItemComponent = from.ItemComponent;
            from.ItemComponent = null;
            to.ItemComponent.transform.position = to.transform.position;
            return true;
            // TODO: Update models
        }
    }

    public bool MoveItemInHands(InventorySlotComponent from, InventorySlotComponent to)
    {
        if (to.ItemComponent != null)   // We have something in the other hand
        {
            var tmp = to.ItemComponent;
            to.ItemComponent = from.ItemComponent;
            from.ItemComponent = tmp;

            to.ItemComponent.transform.position = to.transform.position;
            from.ItemComponent.transform.position = from.transform.position;
            return true;
            // TODO: Update models
        }
        else    // We have something in one hand only
        {
            to.ItemComponent = from.ItemComponent;
            from.ItemComponent = null;
            to.ItemComponent.transform.position = to.transform.position;
            return true;
            // TODO: Update models
        }
    }

    public readonly InventorySlotComponent[] EquipmentSlots = new InventorySlotComponent[5];
    public readonly InventorySlotComponent[] InventorySlots = new InventorySlotComponent[18];
}