using System;
using System.Linq;
using Characters.Player;
using Global;
using Items;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum InventorySlotType
{
    Inventory = -1,
    Head = 0,
    Body = 1,
    Legs = 2,
    RightHand = 3,
    LeftHand = 4
}

public class InventorySlotComponent : MonoBehaviour
{
    public int InventorySlotNumber;
    public InventorySlotType InventorySlotType;
    // public Item CurrentItem { get; set; }
    public ItemComponent ItemComponent { get; set; }

    private void Start()
    {
        // TODO: Fix ME!

        //var invController = GameObject.FindObjectOfType<InventoryController>();
        //if (InventorySlotType == InventorySlotType.Inventory)
        //{
        //    invController.InventorySlots[InventorySlotNumber] = this;
        //}
        //else
        //{
        //    invController.EquipmentSlots[(int) InventorySlotType] = this;
        //}

        //// TODO: temp only
        //if (InventorySlotType == InventorySlotType.Inventory && Random.Range(0, 2) == 0)
        //{
        //    var colors = new string[] {"red", "yellow", "green", "blue"};

        //    var go = new GameObject();
        //    go.transform.SetParent(GameObject.Find("Inventory").transform);
        //    go.transform.position = transform.position;
        //    var rt = go.AddComponent<RectTransform>();
        //    rt.sizeDelta = new Vector2(32, 32);
        //    go.AddComponent<CanvasRenderer>();
        //    var image = go.AddComponent<Image>();
        //    image.sprite = Resources.Load<Sprite>(colors[Random.Range(0, 4)]);
        //    var itemComponent = go.AddComponent<ItemComponent>();
        //    var item = GameRegistry.ItemRegistry.Values.ToArray()[Random.Range(0, GameRegistry.ItemRegistry.Count)];
        //    go.transform.name = item.Identifier;
        //    ItemComponent = itemComponent;
        //    ItemComponent.Identifier = item.Identifier;
        //}
    }
}

public class PlayerInventory
{
    public EquippableItem[] EquippedItems = new EquippableItem[5];
    public Item[] InventoryItems = new Item[18];
}