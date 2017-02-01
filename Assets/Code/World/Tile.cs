using Assets.Code.Characters;
using Assets.Code.Characters.Npc;
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
            if (false && GameState.Rand.Next(10) == 0)  // TODO: Always false
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

            GameObject.AddComponent<BoxCollider2D>();   // Used for right-clicking currently

            var c = GameObject.AddComponent<CustomComponentType>();
            c.Type = ComponentType.Tile;

            var t = GameObject.AddComponent<TileComponent>();
            t.Tile = this;
        }
    }

    // Hacky solution to the way the engine does things

    public class TileComponent : MonoBehaviour
    {
        public Tile Tile;
    }

    public class PlayerComponent : MonoBehaviour
    {
        public Player Player;
    }

    public class NpcComponent : MonoBehaviour
    {
        public Npc Npc;
    }

    public enum ComponentType
    {
        Tile,
        Player,
        Npc,
    }

    public class CustomComponentType : MonoBehaviour
    {
        public ComponentType Type;
    }
}
