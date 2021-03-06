﻿using System.Collections.Generic;
using GameEditor.MapEditor;
using GameEditor.MapEditor.MapModelEditors;
using UnityEngine;

namespace Models.Components
{
    public class WallComponent : MonoBehaviour, IWorldComponent, IWallBehavior
    {
        public WallModel WallModel;

        public void OpenEditorWindow()
        {
            if (FindObjectOfType<MapBuilder>().OpenWindowMap.ContainsKey(this))
            {
                Debug.Log("Window already open");
                return;
            }
            var go = WallMapModelEditor.CreateFromData(this);
            FindObjectOfType<MapBuilder>().OpenWindowMap.Add(this, go);
        }

        public void Configure(WallModel wallModel)
        {
            WallModel = wallModel;
        }
    }
}