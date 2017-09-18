using System.Collections.Generic;
using Global;
using Models.Components;
using UnityEngine;

namespace Models.DataModels
{
    public class WallDataModel : DataModel
    {
        public string SpriteName { get; set; }
        public string SortingLayer { get; set; }
        
        protected WallDataModel()
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
            spriteRenderer.sprite = GameRegistry.SpriteRegistry[SpriteName];
            spriteRenderer.sortingLayerName = SortingLayer;

            go.AddComponent<BoxCollider2D>();

            var wall = go.AddComponent<WallComponent>();
            wall.Configure(Identifier, new List<string>());

            return go;
        }
    }
}