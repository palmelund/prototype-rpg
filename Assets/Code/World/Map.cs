using Code.Characters.PathFinding;
using Code.Characters.Player;
using UnityEngine;

/* TODO:
 * Change the code so that based on tiles deemed reachable by the map maker, 
 * walkable tiles that cant be reached from those wont get a map node to avoid 
 * pathfinding for nodes not reachable.   
 *     => Apply this to the tiles as well if they say they can be entered!
 */

namespace Code.World
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

                    var go = new GameObject();
                    var tile = go.AddComponent<Tile>();
                    tile.Configure(new Vector3(x, y));
                    TileMap[x, y] = tile;
                }
            }

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


        
        public void GenerateGraph()
        {
            for (var y = 0; y < TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < TileMap.GetLength(0); x++)
                {
                    if (TileMap[x, y] == null || TileMap[x, y].CanEnter == false) continue;
                    TileMap[x, y].Vertice = new Vertice(new Vector3(x, y), GetTileAt(x, y));
                }
            }

            for (var y = 0; y < TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < TileMap.GetLength(0); x++)
                {
                    if (TileMap[x, y] == null || TileMap[x, y].CanEnter == false || TileMap[x, y].Vertice == null) continue;

                    // Straights

                    if (x > 0)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x - 1, y].Vertice);
                    }

                    if (y > 0)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x, y - 1].Vertice);
                    }

                    if (x < TileMap.GetLength(0) - 1)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x + 1, y].Vertice);
                    }

                    if (y < TileMap.GetLength(1) - 1)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x, y + 1].Vertice);
                    }

                    // Diagonals

                    if (x > 0 && y > 0 && TileMap[x - 1, y].Vertice != null && TileMap[x, y - 1].Vertice != null)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x - 1, y - 1].Vertice);
                    }

                    if (x > 0 && y < TileMap.GetLength(1) - 1 && TileMap[x - 1, y].Vertice != null &&
                        TileMap[x, y + 1].Vertice != null)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x - 1, y + 1].Vertice);
                    }

                    if (x < TileMap.GetLength(0) - 1 && y < TileMap.GetLength(1) - 1 &&
                        TileMap[x + 1, y].Vertice != null && TileMap[x, y + 1].Vertice != null)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x + 1, y + 1].Vertice);
                    }

                    if (x < TileMap.GetLength(0) - 1 && y > 0 && TileMap[x + 1, y].Vertice != null &&
                        TileMap[x, y - 1].Vertice != null)
                    {
                        TileMap[x, y].Vertice.NeighborList.Add(TileMap[x + 1, y - 1].Vertice);
                    }
                }
            }

            foreach (var tile in TileMap)
            {
                tile?.Vertice?.NeighborList.RemoveAll(n => n == null);
            }
        }
    }
}
