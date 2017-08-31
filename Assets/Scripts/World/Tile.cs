using Characters.PathFinding;
using UnityEngine;

namespace World
{
    public class Tile : MonoBehaviour, IWorldModel
    {
        public int X => Mathf.RoundToInt(transform.position.x);
        public int Y => Mathf.RoundToInt(transform.position.y);

        public Vertice Vertice;

        public bool CanEnter = true;
        
        public string Identifier { get; protected set; }

        //public void Configure(Vector3 position)
        //{
        //    //XCoord = Mathf.RoundToInt(position.x);
        //    //YCoord = Mathf.RoundToInt(position.y);
        //    //name = $"tile_x_{XCoord}_y_{YCoord}";

        //    Assert.AreEqual(position.x, Mathf.Round(position.x));
        //    Assert.AreEqual(position.y, Mathf.Round(position.y));
        //    Assert.AreEqual(position.z, Mathf.Round(position.z));

        //    var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //    spriteRenderer.sprite = Resources.Load<Sprite>("grass");
        //    FrameFrameSpriteName = "grass";
        //    spriteRenderer.sortingLayerName = "BackgroundTiles";
        //    gameObject.transform.position = position;

        //    gameObject.AddComponent<BoxCollider2D>();
        //}

        public void Configure(string identifier, bool canEnter, string sprite)
        {
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(sprite);
            spriteRenderer.sortingLayerName = "BackgroundTiles";
            Identifier = sprite;
            gameObject.AddComponent<BoxCollider2D>();
            CanEnter = true;
        }

        public string SaveData => $"{transform.position.x},{transform.position.y},{CanEnter}";

        public void ConfigureFromSaveData(string s)
        {
            var content = s.Split(',');
            float x;
            float y;
            bool canEnter;

            if (!float.TryParse(content[0], out x) || !float.TryParse(content[1], out y) || !bool.TryParse(content[2], out canEnter))
            {
                Debug.LogError($"Failed to pass Tile: {s}");
            }
            else
            {
                // Configure(x, y, canEnter, content[3]);
            }
        }
    }
}
