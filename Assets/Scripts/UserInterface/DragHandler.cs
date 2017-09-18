using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 _originalLocalPointerPosition;
    private Vector3 _originalPanelLocalPosition;
    public RectTransform WindowPanel;
    public RectTransform WindowBase;

    private void Awake()
    {
        //WindowPanel = transform.parent as RectTransform;
        //WindowBase = WindowPanel.parent as RectTransform;
    }

    public void OnPointerDown(PointerEventData data)
    {
        _originalPanelLocalPosition = WindowPanel.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(WindowBase, data.position, data.pressEventCamera, out _originalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (WindowPanel == null || WindowBase == null)
            return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(WindowBase, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - _originalLocalPointerPosition;
            WindowPanel.localPosition = _originalPanelLocalPosition + offsetToOriginal;
        }

        ClampToWindow();
    }

    // Clamp panel to area of parent
    private void ClampToWindow()
    {
        Vector3 pos = WindowPanel.localPosition;

        Vector3 minPosition = WindowBase.rect.min - WindowPanel.rect.min;
        Vector3 maxPosition = WindowBase.rect.max - WindowPanel.rect.max;

        pos.x = Mathf.Clamp(WindowPanel.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(WindowPanel.localPosition.y, minPosition.y, maxPosition.y);

        WindowPanel.localPosition = pos;
    }
}