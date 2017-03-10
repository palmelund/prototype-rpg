using UnityEngine;

/* TODO:
 * Change the code so that based on tiles deemed reachable by the map maker, 
 * walkable tiles that cant be reached from those wont get a map node to avoid 
 * pathfinding for nodes not reachable.   
 *     => Apply this to the tiles as well if they say they can be entered!
 */

namespace Assets.Code.World
{
    public class Map : MonoBehaviour
    {

        public Tile[,] TileMap;
        public Node[,] Graph;

        // Todo: better way that does not rely on static global variable?
        public static Map Instance;
        public int Width;
        public int Height;
        
        void Start ()
        {
            CreateTestMap();
        }
	
        void Update () {
		
        }

        public void CreateTestMap()
        {
            Instance = this;

            Width = 10;
            Height = 10;

            TileMap = new Tile[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var tile = new Tile(x, y);
                    tile.GameObject.transform.parent = gameObject.transform;
                    TileMap[x, y] = tile;
                }
            }
            Graph = new Node[Width,Height];
            GenerateGraph();
        }

        public Tile GetTileAt(int x, int y)
        {
            return  (x >= 0 && x < Width && y >= 0 && y < Height) ? TileMap[x, y] : null;
        }

        public Tile GetTileAt(Vector3 position)
        {
            return GetTileAt(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public Node GetTileNode(Tile tile)
        {
            return Graph[tile.XCoord, tile.YCoord];
        }

        private void GenerateGraph()
        {
            for (var y = 0; y < TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < TileMap.GetLength(0); x++)
                {
                    if (TileMap[x, y] == null || TileMap[x, y].CanEnter == false) continue;
                    Graph[x, y] = new Node(x, y, TileMap[x,y]);
                }
            }

            for (var y = 0; y < TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < TileMap.GetLength(0); x++)
                {
                    if (TileMap[x, y] == null || TileMap[x, y].CanEnter == false) continue;

                    // Straights

                    if (x > 0)
                    {
                        Graph[x, y].StraightNeighbors.Add(Graph[x - 1, y]);
                    }

                    if (y > 0)
                    {
                        Graph[x, y].StraightNeighbors.Add(Graph[x, y - 1]);
                    }

                    if (x < TileMap.GetLength(0) - 1)
                    {
                        Graph[x, y].StraightNeighbors.Add(Graph[x + 1, y]);
                    }

                    if (y < TileMap.GetLength(1) - 1)
                    {
                        Graph[x, y].StraightNeighbors.Add(Graph[x, y + 1]);
                    }

                    // Diagonals

                    if (x > 0 && y > 0 && Graph[x - 1, y] != null && Graph[x, y - 1] != null)
                    {
                        Graph[x, y].DiagonalNeighbors.Add(Graph[x - 1, y - 1]);
                    }

                    if (x > 0 && y < TileMap.GetLength(1) - 1 && Graph[x - 1, y] != null &&
                        Graph[x, y + 1] != null)
                    {
                        Graph[x, y].DiagonalNeighbors.Add(Graph[x - 1, y + 1]);
                    }

                    if (x < TileMap.GetLength(0) - 1 && y < TileMap.GetLength(1) - 1 &&
                        Graph[x + 1, y] != null && Graph[x, y + 1] != null)
                    {
                        Graph[x, y].DiagonalNeighbors.Add(Graph[x + 1, y + 1]);
                    }

                    if (x < TileMap.GetLength(0) - 1 && y > 0 && Graph[x + 1, y] != null &&
                        Graph[x, y - 1] != null)
                    {
                        Graph[x, y].DiagonalNeighbors.Add(Graph[x + 1, y - 1]);
                    }
                }
            }

            foreach (var node in Graph)
            {
                if (node == null) continue;
                node.StraightNeighbors.RemoveAll(n => n == null);
                node.DiagonalNeighbors.RemoveAll(n => n == null);
            }
        }
    }
}
