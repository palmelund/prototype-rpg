using System;
using System.Collections.Generic;
using Characters.PathFinding;
using GameEditor.MapEditor;
using GameEditor.MapEditor.MapModelEditors;
using Global;
using UnityEngine;

namespace Models.Components
{
    public class FloorComponent : MonoBehaviour, IWorldComponent
    {
        public int X => Mathf.RoundToInt(transform.position.x);
        public int Y => Mathf.RoundToInt(transform.position.y);

        public Vertice Vertice;

        public bool CanEnter = true;

        public FloorModel FloorModel;

        public void OpenEditorWindow()
        {
            if (FindObjectOfType<MapBuilder>().OpenWindowMap.ContainsKey(this))
            {
                Debug.Log("Window already open");
                return;
            }
            var go = FloorMapModelEditor.CreateFromData(this);
            FindObjectOfType<MapBuilder>().OpenWindowMap.Add(this, go);
        }

        public void Configure(FloorModel floorModel)
        {
            FloorModel = floorModel;
            CanEnter = floorModel.CanEnter;
        }
    }
}
