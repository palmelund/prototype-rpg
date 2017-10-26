using Global;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string Identifier { get; set; }
    public Item Item => null; // GameRegistry.ItemRegistry[Identifier];

    public Vector3 StartPosition { get; private set; }
    private RectTransform RectTransform => GetComponent<RectTransform>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
        {
            Debug.Log("No canvas in parents!");
            return;
        }

        StartPosition = transform.position;

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        Vector3 globalMousePosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform, eventData.position,
            eventData.pressEventCamera, out globalMousePosition))
        {
            transform.position = globalMousePosition;
        }
    }

    public static PlayerInventory PlayerInventory = new PlayerInventory();

    public void OnEndDrag(PointerEventData eventData)
    {
        var hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
        var toSlot = hit.transform?.gameObject.GetComponent<InventorySlotComponent>();
        if (toSlot == null)
        {
            Debug.Log("Cannot move item here!");
            transform.position = StartPosition;
            return;
        }

        hit = Physics2D.Raycast(StartPosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
        var fromSlot = hit.transform?.gameObject.GetComponent<InventorySlotComponent>();

        if (FindObjectOfType<InventoryController>().MoveItem(fromSlot, toSlot))
        {
            FindObjectOfType<InventoryController>().UpdatePlayerInventory();
        }
        else
        {
            transform.position = StartPosition;
        }
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
