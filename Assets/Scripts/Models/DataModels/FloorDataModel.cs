using System.Collections.Generic;
using Models.Components;
using UnityEngine;

namespace Models.DataModels
{
    public class FloorDataModel : DataModel
    {
        public string SpriteName { get; set; }
        public string SortingLayer { get; set; }
        
        public bool CanEnter { get; set; }
        
        protected FloorDataModel()
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
            spriteRenderer.sortingLayerName = "BackgroundTiles";

            var floorComponent = go.AddComponent<FloorComponent>();
            floorComponent.Configure(Identifier, new List<string>(), CanEnter, SpriteName);
            go.AddComponent<BoxCollider2D>();
            return go;
        }
    }
}