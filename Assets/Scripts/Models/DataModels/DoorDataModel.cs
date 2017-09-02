using System;
using UnityEngine;
using World;

namespace Models.DataModels
{
    public class DoorDataModel : DataModel
    {
        public string DoorSpriteName { get; set; }
        public string DoorSortingLayer { get; set; }
        public string FrameSpriteName { get; set; }
        public string FrameSortingLayer { get; set; }
        public float TurnPoint { get; set; }

        public DoorDataModel(string identifier, string displayName, string frameSpriteName, string doorSpriteName,
            string frameSortingLayer, string doorSortingLayer, float turnPoint)
        {
            Identifier = identifier;
            DisplayName = displayName;
            TurnPoint = turnPoint;

            FrameSpriteName = frameSpriteName;
            FrameSortingLayer = frameSortingLayer;

            DoorSpriteName = doorSpriteName;
            DoorSortingLayer = doorSortingLayer;
        }

        protected DoorDataModel()
        {
        }

        public override GameObject InstantiateGame(Vector3 position)
        {
            return InstantiateGame(position, Vector3.zero);
        }

        public override GameObject InstantiateGame(Vector3 position, Vector3 rotation)
        {
            var go = new GameObject(Identifier);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(rotation);
            
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameRegistry.SpriteRegistry[FrameSpriteName];
            spriteRenderer.sortingLayerName = FrameSortingLayer;
            

            var movingPart = new GameObject();
            movingPart.transform.SetParent(go.transform);
            movingPart.transform.position = position;
            movingPart.transform.rotation = Quaternion.Euler(rotation);

            go.AddComponent<BoxCollider2D>();

            var doorSpriteRenderer = movingPart.AddComponent<SpriteRenderer>();
            doorSpriteRenderer.sprite = GameRegistry.SpriteRegistry[DoorSpriteName];
            doorSpriteRenderer.sortingLayerName = DoorSortingLayer;

            var door = go.AddComponent<DoorComponent>();

            var remaining = position.x - (int) position.x;

            if (remaining == 0f)
            {
                door.Configure(Identifier, new Vector3(TurnPoint, 0) + position, movingPart);
            }
            else if (remaining == 0.5f)
            {
                door.Configure(Identifier, new Vector3(0, TurnPoint) + position, movingPart);
            }
            else
            {
                Debug.LogError("Rounding error!");
            }

            return go;
        }

        public override GameObject InstantiateEditor(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public override GameObject InstantiateEditor(Vector3 position, Vector3 rotation)
        {
            throw new NotImplementedException();
        }
    }
}