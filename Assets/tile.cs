using UnityEngine;

namespace Assets
{
    public class tile
    {
        public int XCoord;
        public int YCoord;
        public GameObject GameObject;
        public SpriteRenderer SpriteRenderer;

        public float Distance;
        public bool CanEnter = true;

        public tile(int x, int y)
        {
            XCoord = x;
            YCoord = y;
            GameObject = new GameObject(string.Format("tile_x_{0}_y_{1}", x, y));
            SpriteRenderer = GameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.sprite = Resources.Load<Sprite>("grass");
            SpriteRenderer.sortingLayerName = "BackgroundTiles";
            GameObject.transform.position = new Vector3(x, y);
        }
    }
}
