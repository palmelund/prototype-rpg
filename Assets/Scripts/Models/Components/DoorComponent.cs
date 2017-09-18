using System.Collections.Generic;
using GameEditor.MapEditor;
using GameEditor.MapEditor.MapModelEditors;
using Global;
using Models.MapModels;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace Models.Components
{
    public class DoorComponent : MonoBehaviour, IWorldComponent, IWallBehavior
    {
        // TODO: Better way to do this!

        public Vector3 TurnPoint { get; set; }

        public bool IsOpen { get; set; }

        private FloorComponent _sideA;
        private FloorComponent _sideB;
        public string Identifier { get; set; }
        public List<string> References { get; set; }
        public void OpenEditorWindow()
        {
            if (FindObjectOfType<MapBuilder>().OpenWindowMap.ContainsKey(this))
            {
                Debug.Log("Window already open");
                return;
            }
            var go = DoorMapModelEditor.CreateFromData(this);
            FindObjectOfType<MapBuilder>().OpenWindowMap.Add(this, go);
        }

        public bool CanUseDoor { get; set; } = true;

        private GameObject MovingPart { get; set; }

        public bool LoadOtherLevelOnUse { get; set; }

        public string MapReference { get; set; }
        public string SpawnPointReference { get; set; }
        public string KeyGroup { get; set; }

        public void Configure(string identifier, List<string> references, string keyGroup, Vector3 turnPoint, GameObject movingPart)
        {
            Identifier = identifier;
            TurnPoint = turnPoint;
            MovingPart = movingPart;
            References = references;
            KeyGroup = keyGroup;
        }

        public void SetSides(FloorComponent sideA, FloorComponent sideB)
        {
            if (sideA != null && sideB != null)
            {
                CanUseDoor = true;
                _sideA = sideA;
                _sideB = sideB;
            }
            else
            {
                CanUseDoor = false;
            }
        }
        
        public void ToggleDoor()
        {
            if (LoadOtherLevelOnUse)
            {
                var map = GameRegistry.MapRegistry[MapReference];

                var mapModel = Serializer.DeserializeFromFile<MapModelConverter>(map);
                FindObjectOfType<MapComponent>().LoadMapTransition(mapModel, MapReference, SpawnPointReference);

                //FindObjectOfType<MapComponent>().UnloadMap();

                //var mapModel = MapModelConverter.DeserializeFromFile(map);
                //mapModel.CreateMapFromModel();

                return;
            }

            if (!CanUseDoor) return;

            if (IsOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        public void OpenDoor()
        {
            IsOpen = true;
            MovingPart.transform.RotateAround(TurnPoint, Vector3.forward, 90f);
            GetComponentInChildren<BoxCollider2D>().offset = new Vector2(MovingPart.transform.localPosition.x, MovingPart.transform.localPosition.y);
            ConnectTiles();
        }

        public void CloseDoor()
        {
            IsOpen = false;
            MovingPart.transform.RotateAround(TurnPoint, Vector3.forward, -90f);
            GetComponentInChildren<BoxCollider2D>().offset = Vector2.zero;
            DisconnectTiles();
        }

        // TODO: Block other tiles when door is open?

        private void ConnectTiles()
        {
            _sideA.Vertice.NeighborList.Add(_sideB.Vertice);
            _sideB.Vertice.NeighborList.Add(_sideA.Vertice);
        }

        private void DisconnectTiles()
        {
            _sideA.Vertice.NeighborList.Remove(_sideB.Vertice);
            _sideB.Vertice.NeighborList.Remove(_sideA.Vertice);
        }
    }
}