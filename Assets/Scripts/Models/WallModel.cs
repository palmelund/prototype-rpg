using System.Collections.Generic;
using Models.Components;
using UnityEngine;

namespace Models
{
    public class WallModel : BaseModel
    {
        public Sprite Sprite;
        public override GameObject Instantiate(Vector3 position)
        {
            return Instantiate(position, Vector3.zero);
        }

        public override GameObject Instantiate(Vector3 position, Vector3 rotation)
        {
            var go = new GameObject(IdName);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(rotation);

            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite;
            spriteRenderer.sortingLayerName = "BackgroundTiles";

            go.AddComponent<BoxCollider2D>();

            var wall = go.AddComponent<WallComponent>();
            wall.Configure(this);

            return go;
        }
    }
}