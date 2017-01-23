using UnityEngine;

namespace Assets.Code.World
{
    public class Map : MonoBehaviour
    {

        public Tile[,] TileMap;

        // Todo: better way that does not rely on static global variable?
        public static Map Instance;
        public int Width;
        public int Height;
        
        void Start ()
        {
            Instance = this;
            
            Width = 10;
            Height = 10;

            TileMap = new Tile[10,10];
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    var tile = new Tile(x, y);
                    tile.GameObject.transform.parent = gameObject.transform;
                    TileMap[x, y] = tile;

                }
            }
        }
	
        void Update () {
		
        }

        /*
        public List<Tile> GetNeighbors(Tile t)
        {
            return new List<Tile>(8)
            {
                GetTileAt(t.XCoord + 1, t.YCoord + 1),
                GetTileAt(t.XCoord + 1, t.YCoord),
                GetTileAt(t.XCoord + 1, t.YCoord - 1),
                GetTileAt(t.XCoord, t.YCoord - 1),
                GetTileAt(t.XCoord - 1, t.YCoord - 1),
                GetTileAt(t.XCoord - 1, t.YCoord),
                GetTileAt(t.XCoord - 1, t.YCoord + 1),
                GetTileAt(t.XCoord, t.YCoord + 1)
            };
        }
        */

        public Tile GetTileAt(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return TileMap[x, y];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileAt(Vector3 position)
        {
            return GetTileAt(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

    }
}
