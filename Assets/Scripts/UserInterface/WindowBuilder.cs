using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

public enum WindowBuilderCanvas
{
    Screen,
    World
}

public static class WindowBuilder
{
    public static readonly Dictionary<string, GameObject> OpenWindowDictionary = new Dictionary<string, GameObject>();

    public static GameObject CreateWindow(string windowIdentifier, string headerText, GameObject content, WindowBuilderCanvas canvasType, bool canDrag = true, bool canResize = false, int minX = 200, int minY = 325, int maxX = 200, int maxY = 325)
    {
        if (OpenWindowDictionary.ContainsKey(windowIdentifier))
        {
            Debug.Log("Window already open!");
            return null;
        }

        var window = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Windows/WindowBase"));
        window.transform.SetParent(GameObject.Find("GlobalCanvas").transform, false);

        var baseWindow = window.GetComponent<BasicWindow>();

        if (!canDrag)
        {
            baseWindow.DragZone.SetActive(false);
        }

        if (canResize)
        {
            baseWindow.ResizeZone.GetComponent<ResizeHandler>().SetSizeLimits(minX, minY, maxX, maxY);
        }
        else
        {
            baseWindow.ResizeZone.SetActive(false);
        }

        content.transform.SetParent(baseWindow.Content.transform);

        var width = content.transform.localScale.x > 200 ? content.transform.localScale.x : 200;

        baseWindow.Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, content.transform.localScale.y + 40);

        OpenWindowDictionary.Add(windowIdentifier, window);

        return window;
    }

    private static Transform GetCanvasTransform(WindowBuilderCanvas canvasType)
    {
        switch (canvasType)
        {
            case WindowBuilderCanvas.Screen:
                return GameObject.FindGameObjectWithTag("ScreenCanvas").transform;
            case WindowBuilderCanvas.World:
                return GameObject.FindGameObjectWithTag("WorldCanvas").transform;
            default:
                throw new ArgumentOutOfRangeException(nameof(canvasType), canvasType, null);
        }
    }
}
