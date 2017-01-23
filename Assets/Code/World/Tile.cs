using UnityEngine;

namespace Assets.Code.World
{
    public class Tile
    {
        public int XCoord;
        public int YCoord;
        public GameObject GameObject;
        public SpriteRenderer SpriteRenderer;
        
        public bool CanEnter = true;
        
        public Tile(int x, int y)
        {
            XCoord = x;
            YCoord = y;
            GameObject = new GameObject(string.Format("tile_x_{0}_y_{1}", x, y));
            SpriteRenderer = GameObject.AddComponent<SpriteRenderer>();
            if (GameState.Rand.Next(5) == 0)
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("wall");
                CanEnter = false;
            }
            else
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("grass");
            }
            SpriteRenderer.sortingLayerName = "BackgroundTiles";
            GameObject.transform.position = new Vector3(x, y);
        }
    }
}
