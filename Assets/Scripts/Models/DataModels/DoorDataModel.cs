using System.Collections.Generic;
using Models.Components;
using UnityEngine;

namespace Models.DataModels
{
    public class DoorDataModel : DataModel
    {
        public string DoorSpriteName { get; set; }
        public string DoorSortingLayer { get; set; }
        public string FrameSpriteName { get; set; }
        public string FrameSortingLayer { get; set; }
        public float TurnPoint { get; set; }
        
        protected DoorDataModel()
        {
        }

        public override GameObject Instantiate(Vector3 position)
        {
            return Instantiate(position, Vector3.zero);
        }

        public override GameObject Instantiate(Vector3 position, Vector3 rotation)
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

            var hover = new GameObject("Hover");
            hover.transform.SetParent(go.transform);
            hover.transform.localPosition = Vector3.zero;

            var hoverSpriteRenderer = hover.AddComponent<SpriteRenderer>();
            hoverSpriteRenderer.sprite = GameRegistry.SpriteRegistry["hover_door"];
            hoverSpriteRenderer.sortingLayerName = "Hover";

            var door = go.AddComponent<DoorComponent>();

            door.LoadOtherLevelOnUse = false;
            door.MapReference = "";
            door.SpawnPointReference = "";

            var remaining = position.x - (int) position.x;

            if (remaining == 0f)
            {
                door.Configure(Identifier, new List<string>(), new Vector3(TurnPoint, 0) + position, movingPart);
            }
            else if (remaining == 0.5f)
            {
                door.Configure(Identifier, new List<string>(), new Vector3(0, TurnPoint) + position, movingPart);
            }
            else
            {
                Debug.LogError("Rounding error!");
            }

            return go;
        }
    }
}