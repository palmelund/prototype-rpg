using Code.Characters.PathFinding;
using UnityEngine;

namespace Code.World
{
    public class Tile : MonoBehaviour
    {
        public int XCoord;
        public int YCoord;

        public Vertice Vertice;

        public bool CanEnter = true;

        public void Configure(Vector3 position)
        {
            XCoord = Mathf.RoundToInt(position.x);
            YCoord = Mathf.RoundToInt(position.y);
            name = $"tile_x_{XCoord}_y_{YCoord}";

            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("grass");
            spriteRenderer.sortingLayerName = "BackgroundTiles";
            gameObject.transform.position = position;

            gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
