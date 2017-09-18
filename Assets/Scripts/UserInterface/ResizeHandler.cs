using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 _minSize;
    private Vector2 _maxSize;

    public RectTransform Panel;
    private Vector2 _originalLocalPointerPosition;
    private Vector2 _originalSizeDelta;

    private void Awake()
    {
        Panel = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        _originalSizeDelta = Panel.sizeDelta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Panel, data.position, data.pressEventCamera, out _originalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (Panel == null)
            return;

        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Panel, data.position, data.pressEventCamera, out localPointerPosition);
        Vector3 offsetToOriginal = localPointerPosition - _originalLocalPointerPosition;

        Vector2 sizeDelta = _originalSizeDelta + new Vector2(offsetToOriginal.x, -offsetToOriginal.y);
        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, _minSize.x, _maxSize.x),
            Mathf.Clamp(sizeDelta.y, _minSize.y, _maxSize.y)
        );

        Panel.sizeDelta = sizeDelta;
    }

    public void SetSizeLimits(int minX, int minY, int maxX, int maxY)
    {
        _minSize = new Vector2(minX, minY);
        _maxSize = new Vector2(maxX, maxY);
    }
}