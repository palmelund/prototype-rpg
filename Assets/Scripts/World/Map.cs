using System;
using System.Collections.Generic;
using System.IO;
using Characters.PathFinding;
using Characters.Player;
using Models;
using UnityEngine;

/* TODO:
 * Change the code so that based on tiles deemed reachable by the map maker, 
 * walkable tiles that cant be reached from those wont get a map node to avoid 
 * pathfinding for nodes not reachable.   
 *     => Apply this to the tiles as well if they say they can be entered!
 */

namespace World
{
    public class Map : MonoBehaviour
    {
        // public Tile[,] TileMap;
        public Dictionary<Vector3, Tile> TileMap = new Dictionary<Vector3, Tile>();
        public Dictionary<Vector3, GameObject> WorldModelMap = new Dictionary<Vector3, GameObject>();

        public Dictionary<string, BaseModel> ModelCatalogue = new Dictionary<string, BaseModel>();

        // Todo: better way that does not rely on static global variable?
        public static Map Instance;

        private int _width = 10;
        private int _height = 10;
        
        void Start ()
        {
            Instance = this;
            LoadModels();
            CreateTestMap();
        }
	
        void Update () {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ToggleVisibleGraph();
            }
        }

        private void LoadModels()
        {
            var floorModel = new FloorModel("floor_grass_1.model");
            ModelCatalogue.Add(floorModel.Identifier, floorModel);
            
            var wallModel = new WallModel("wall_stone_1.model");
            ModelCatalogue.Add(wallModel.Identifier, wallModel);

            var doorModel = new DoorModel("door_stone_1.model");
            ModelCatalogue.Add(doorModel.Identifier, doorModel);
        }

        public void CreateTestMap()
        {
            TileMap.Clear();
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var go = ModelCatalogue["floor_grass_1"].Instantiate(new Vector3(x, y));
                    TileMap.Add(go.transform.position, go.GetComponent<Tile>());
                }
            }

            GenerateGraph();
        }

        public Tile GetTileAt(int x, int y)
        {
            return GetTileAt(new Vector3(x, y));
        }

        public Tile GetTileAt(Vector3 position)
        {
            return TileMap.ContainsKey(position) ? TileMap[position] : null;
        }
        
        public void GenerateGraph()
        {
            // TODO: Door/wall support

            /* How it works
             * 1) Add a vertice to all walkable tiles
             * 2) Connect all walkable tiles with all their neighbors
             * 3) Add connection changes for doors
             * 4) Remove all connections blocked by walls / closed doors
             * 5) Re-attach actors (Actors should have some temp vertice to use while map is being generated.
             * 
             * In the future, graph should be generated once, before adding characters to the scene.
             */

            // Deattach actors (if any)
            var playerController = FindObjectOfType<PlayerController>();
            playerController.SelectedPlayerCharacter?.Path.Clear();
            
            // Add vertice to all walkable tiles

            foreach (var tile in TileMap.Values)
            {
                if (tile == null || tile.CanEnter == false)
                {
                    continue;
                }

                tile.Vertice = new Vertice(tile.transform.position, tile);
            }

            // Add neighbors to all tiles

            foreach (var tile in TileMap.Values)
            {
                if (tile == null || tile.CanEnter == false || tile.Vertice == null)
                {
                    continue;
                }
                // Straights

                tile.Vertice.NeighborList.Add(GetTileAt(tile.X - 1, tile.Y)?.Vertice);
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X + 1, tile.Y)?.Vertice);
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X, tile.Y - 1)?.Vertice);
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X, tile.Y + 1)?.Vertice);

                // Diagonals
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X + 1, tile.Y + 1)?.Vertice);
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X + 1, tile.Y - 1)?.Vertice);
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X - 1, tile.Y + 1)?.Vertice);
                tile.Vertice.NeighborList.Add(GetTileAt(tile.X - 1, tile.Y - 1)?.Vertice);
            }

            // Remove null neighbors

            foreach (var tile in TileMap.Values)
            {
                tile?.Vertice?.NeighborList.RemoveAll(n => n == null);
            }

            // Add door connections

            foreach (var go in WorldModelMap.Values)
            {
                if (go.GetComponentInChildren<Door>() != null)
                {

                    var door = go.GetComponentInChildren<Door>();
                    var touchingTiles = GetTouchingTiles(go.transform.position);
                    door.SetSides(touchingTiles.Item1, touchingTiles.Item2);
                }
            }

            // Apply wall blocking to walls and doors

            foreach (var go in WorldModelMap.Values)
            {
                if (go.GetComponent<Wall>() != null)
                {
                    var touchingSides = GetTouchingTiles(go.transform.position);

                    if (touchingSides.Item1 != null && touchingSides.Item2 != null &&
                        touchingSides.Item1.CanEnter == true &&
                        touchingSides.Item2.CanEnter == true)
                    {
                        touchingSides.Item1.Vertice.NeighborList.Remove(touchingSides.Item2.Vertice);
                        touchingSides.Item2.Vertice.NeighborList.Remove(touchingSides.Item1.Vertice);

                        // Remove potential diagonals

                        if (touchingSides.Item1.X == touchingSides.Item2.X) // Vertical
                        {
                            var right1Tile = Map.Instance.GetTileAt(touchingSides.Item1.X + 1, touchingSides.Item1.Y);
                            var left1Tile = Map.Instance.GetTileAt(touchingSides.Item1.X - 1, touchingSides.Item1.Y);

                            var right2Tile = Map.Instance.GetTileAt(touchingSides.Item2.X + 1, touchingSides.Item2.Y);
                            var left2Tile = Map.Instance.GetTileAt(touchingSides.Item2.X - 1, touchingSides.Item2.Y);

                            if (right1Tile != null && right1Tile.CanEnter)
                            {
                                right1Tile.Vertice.NeighborList.Remove(touchingSides.Item2.Vertice);
                                touchingSides.Item2.Vertice.NeighborList.Remove(right1Tile.Vertice);
                            }

                            if (left1Tile != null && left1Tile.CanEnter)
                            {
                                left1Tile.Vertice.NeighborList.Remove(touchingSides.Item2.Vertice);
                                touchingSides.Item2.Vertice.NeighborList.Remove(left1Tile.Vertice);
                            }

                            if (right2Tile != null && right2Tile.CanEnter)
                            {
                                right2Tile.Vertice.NeighborList.Remove(touchingSides.Item1.Vertice);
                                touchingSides.Item1.Vertice.NeighborList.Remove(right2Tile.Vertice);
                            }

                            if (left2Tile != null && left2Tile.CanEnter)
                            {
                                left2Tile.Vertice.NeighborList.Remove(touchingSides.Item1.Vertice);
                                touchingSides.Item1.Vertice.NeighborList.Remove(left2Tile.Vertice);
                            }
                        }
                        else // Horizontal
                        {
                            var up1Tile = Map.Instance.GetTileAt(touchingSides.Item1.X, touchingSides.Item1.Y + 1);
                            var down1Tile = Map.Instance.GetTileAt(touchingSides.Item1.X, touchingSides.Item1.Y - 1);

                            var up2Tile = Map.Instance.GetTileAt(touchingSides.Item2.X, touchingSides.Item2.Y + 1);
                            var down2Tile = Map.Instance.GetTileAt(touchingSides.Item2.X, touchingSides.Item2.Y - 1);

                            if (up1Tile != null && up1Tile.CanEnter)
                            {
                                up1Tile.Vertice.NeighborList.Remove(touchingSides.Item2.Vertice);
                                touchingSides.Item2.Vertice.NeighborList.Remove(up1Tile.Vertice);
                            }

                            if (down1Tile != null && down1Tile.CanEnter)
                            {
                                down1Tile.Vertice.NeighborList.Remove(touchingSides.Item2.Vertice);
                                touchingSides.Item2.Vertice.NeighborList.Remove(down1Tile.Vertice);
                            }

                            if (up2Tile != null && up2Tile.CanEnter)
                            {
                                up2Tile.Vertice.NeighborList.Remove(touchingSides.Item1.Vertice);
                                touchingSides.Item1.Vertice.NeighborList.Remove(up2Tile.Vertice);
                            }

                            if (down2Tile != null && down2Tile.CanEnter)
                            {
                                down2Tile.Vertice.NeighborList.Remove(touchingSides.Item1.Vertice);
                                touchingSides.Item1.Vertice.NeighborList.Remove(down2Tile.Vertice);
                            }
                        }
                    }
                }
            }

            // Re-attach actors
            if (playerController.SelectedPlayerCharacter != null)
            {
                playerController.SelectedPlayerCharacter.NextVertice = GetTileAt(playerController.SelectedPlayerCharacter.NextVertice.Position).Vertice;  //GetTileAt(playerController.SelectedPlayerCharacter.transform.position).Vertice;
            }
        }

        public Tuple<Tile, Tile> GetTouchingTiles(Vector3 position)
        {
            var xDecimal = Mathf.Abs(position.x - (int)position.x);
            var yDecimal = Mathf.Abs(position.y - (int)position.y);

            if (xDecimal == 0.5f)
            {
                var left = new Vector3(position.x - 0.5f, position.y);
                var right = new Vector3(position.x + 0.5f, position.y);
                return new Tuple<Tile, Tile>(GetTileAt(Mathf.RoundToInt(left.x), Mathf.RoundToInt(left.y)), GetTileAt(Mathf.RoundToInt(right.x), Mathf.RoundToInt(right.y)));
            }
            else if (yDecimal == 0.5f)
            {
                var down = new Vector3(position.x, position.y - 0.5f);
                var up = new Vector3(position.x, position.y + 0.5f);
                return new Tuple<Tile, Tile>(GetTileAt(Mathf.RoundToInt(down.x), Mathf.RoundToInt(down.y)), GetTileAt(Mathf.RoundToInt(up.x), Mathf.RoundToInt(up.y)));
            }
            else
            {
                Debug.LogError($"X: {xDecimal} Y: {yDecimal}");
                return new Tuple<Tile, Tile>(null, null);
            }
        }

        private readonly List<GameObject> _graphEdges = new List<GameObject>();
        private readonly List<Tuple<Vector3, Vector3>> _coveredVertices = new List<Tuple<Vector3, Vector3>>();

        public void DrawVisibleGraph()
        {
            foreach (var tile in Map.Instance.TileMap.Values)
            {
                if (tile == null || tile.Vertice == null) continue;

                foreach (var vertex in tile.Vertice.NeighborList)
                {
                    var pos1 = tile.gameObject.transform.position;
                    var pos2 = vertex.Position;

                    pos1.z -= 0.1f;
                    pos2.z -= 0.1f;

                    if (_coveredVertices.Contains(new Tuple<Vector3, Vector3>(pos1, pos2)) ||
                        _coveredVertices.Contains(new Tuple<Vector3, Vector3>(pos2, pos1)))
                    {
                        continue;
                    }
                    else
                    {
                        _coveredVertices.Add(new Tuple<Vector3, Vector3>(pos1, pos2));
                    }


                    var go = new GameObject();
                    var lineRenderer = go.AddComponent<LineRenderer>();

                    _graphEdges.Add(go);

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPositions(new[]
                    {
                        pos1,
                        pos2
                    });

                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                }
            }
        }

        public void ClearVisibleGraph()
        {
            foreach (var graphEdge in _graphEdges)
            {
                Destroy(graphEdge);
            }
            _graphEdges.Clear();
            _coveredVertices.Clear();
        }

        public void ToggleVisibleGraph()
        {
            if (_graphEdges.Count > 0)
                ClearVisibleGraph();
            else
                DrawVisibleGraph();
        }
    }
}
