using Characters.PathFinding;
using Models.MapModels;
using UnityEngine;

namespace World
{
    public class FloorComponent : MonoBehaviour, IWorldComponent
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
        //    FrameSpriteName = "grass";
        //    spriteRenderer.sortingLayerName = "BackgroundTiles";
        //    gameObject.transform.position = position;

        //    gameObject.AddComponent<BoxCollider2D>();
        //}

        public void Configure(string identifier, bool canEnter, string sprite)
        {
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameRegistry.SpriteRegistry[sprite];
            spriteRenderer.sortingLayerName = "BackgroundTiles";
            Identifier = identifier;
            gameObject.AddComponent<BoxCollider2D>();
            CanEnter = true;
        }
    }
}
