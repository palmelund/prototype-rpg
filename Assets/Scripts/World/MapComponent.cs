using System;
using System.Collections.Generic;
using System.Linq;
using Characters.PathFinding;
using Characters.Player;
using Global;
using Models.Components;
using Models.MapModels;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/* TODO:
 * Change the code so that based on tiles deemed reachable by the map maker, 
 * walkable tiles that cant be reached from those wont get a map node to avoid 
 * pathfinding for nodes not reachable.   
 *     => Apply this to the tiles as well if they say they can be entered!
 */

namespace World
{
    public class MapComponent : MonoBehaviour
    {
        //public Dictionary<Vector3, FloorComponent> FloorModelMap { get; } = new Dictionary<Vector3, FloorComponent>();
        //public Dictionary<Vector3, GameObject> WorldModelMap { get; } = new Dictionary<Vector3, GameObject>();

        //public Dictionary<Vector3, List<PointComponent>> PointMap { get; } = new Dictionary<Vector3, List<PointComponent>>();


        public Dictionary<Vector3, GameObject> ModelsMap { get; } = new Dictionary<Vector3, GameObject>();

        public Dictionary<string, MapModel> ReferenceMap { get; } = new Dictionary<string, MapModel>(); // TODO: Map Model or Map Component?

        // Todo: better way that does not rely on static global variable?

        void Start()
        {
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ToggleVisibleGraph();
            }
        }

        public List<T> GetComponentsOfType<T>() where T : MonoBehaviour
        {
            return (from model in ModelsMap.Values where model.GetComponent<T>() != null select model.GetComponent<T>())
                .ToList();
        }

        public T GetComponentAt<T>(float x, float y) where T : MonoBehaviour
        {
            return GetComponentAt<T>(new Vector3(x, y));
        }

        public T GetComponentAt<T>(int x, int y) where T : MonoBehaviour
        {
            return GetComponentAt<T>(new Vector3(x, y));
        }

        public T GetComponentAt<T>(Vector3 position) where T : MonoBehaviour
        {
            return ModelsMap.ContainsKey(position) ? ModelsMap[position].GetComponent<T>() : null;
        }

        public void GenerateGraph()
        {
            // TODO: DoorMapModel/wall support

            /* How it works
             * 1) Add a vertice to all walkable tiles
             * 2) Connect all walkable tiles with all their neighbors
             * 3) Add connection changes for doors
             * 4) Remove all connections blocked by walls / closed doors
             * 5) Re-attach actors (Actors should have some temp vertice to use while map is being generated.
             * 
             * In the future, graph should be generated once, before adding characters to the scene, as moving shouldn't be allowed from the editor.
             */

            // Deattach actors (if any)
            var playerController = FindObjectOfType<PlayerController>();
            playerController.SelectedPlayerCharacter?.Path.Clear();

            // Add vertice to all walkable tiles

            foreach (var go in ModelsMap.Values)
            {
                if (go.GetComponent<FloorComponent>())
                {
                    var floor = go.GetComponent<FloorComponent>();
                    floor.Vertice = new Vertice(floor);
                }
            }

            // Add neighbors to all tiles

            foreach (var go in ModelsMap.Values)
            {
                if (go.GetComponent<FloorComponent>())
                {
                    var floor = go.GetComponent<FloorComponent>();

                    if (floor.CanEnter == false || floor.Vertice == null)
                    {
                        continue;
                    }

                    // Straights

                    floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X - 1, floor.Y)?.Vertice);
                    floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X + 1, floor.Y)?.Vertice);
                    floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X, floor.Y - 1)?.Vertice);
                    floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X, floor.Y + 1)?.Vertice);

                    // Diagonals
                    if (GetComponentAt<FloorComponent>(floor.X + 1, floor.Y) != null && GetComponentAt<FloorComponent>(floor.X, floor.Y + 1) != null)
                    {
                        floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X + 1, floor.Y + 1)?.Vertice);
                    }

                    if (GetComponentAt<FloorComponent>(floor.X + 1, floor.Y) != null && GetComponentAt<FloorComponent>(floor.X, floor.Y - 1) != null)
                    {
                        floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X + 1, floor.Y - 1)?.Vertice);
                    }

                    if (GetComponentAt<FloorComponent>(floor.X - 1, floor.Y) != null && GetComponentAt<FloorComponent>(floor.X, floor.Y + 1) != null)
                    {
                        floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X - 1, floor.Y + 1)?.Vertice);
                    }
                    if (GetComponentAt<FloorComponent>(floor.X - 1, floor.Y) != null && GetComponentAt<FloorComponent>(floor.X, floor.Y - 1) != null)
                    {
                        floor.Vertice.NeighborList.Add(GetComponentAt<FloorComponent>(floor.X - 1, floor.Y - 1)?.Vertice);
                    }
                }
            }

            // Remove null neighbors

            foreach (var go in ModelsMap.Values)
            {
                if (go.GetComponent<FloorComponent>() != null)
                {
                    go.GetComponent<FloorComponent>()?.Vertice?.NeighborList.RemoveAll(n => n == null);
                }
            }

            // Add door connections

            foreach (var go in ModelsMap.Values)
            {
                if (go.GetComponent<DoorComponent>() != null)
                {
                    var doorComponent = go.GetComponent<DoorComponent>();
                    var touchingSides = GetTouchingTiles(doorComponent.transform.position);
                    doorComponent.SetSides(touchingSides.Item1, touchingSides.Item2);
                }
            }

            // Apply wall blocking to walls and doors

            foreach (var go in ModelsMap.Values)
            {
                if (go.GetComponent<IWallBehavior>() != null)
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
                            var right1Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item1.X + 1, touchingSides.Item1.Y);
                            var left1Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item1.X - 1, touchingSides.Item1.Y);

                            var right2Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item2.X + 1, touchingSides.Item2.Y);
                            var left2Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item2.X - 1, touchingSides.Item2.Y);

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
                            var up1Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item1.X, touchingSides.Item1.Y + 1);
                            var down1Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item1.X, touchingSides.Item1.Y - 1);

                            var up2Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item2.X, touchingSides.Item2.Y + 1);
                            var down2Tile =
                                GetComponentAt<FloorComponent>(touchingSides.Item2.X, touchingSides.Item2.Y - 1);

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
            if (playerController.SelectedPlayerCharacter?.NextVertice != null)
            {
                playerController.SelectedPlayerCharacter.NextVertice =
                    GetComponentAt<FloorComponent>(playerController.SelectedPlayerCharacter.NextVertice.Position)
                        ?.Vertice;
            }
        }

        public Tuple<FloorComponent, FloorComponent> GetTouchingTiles(Vector3 position)
        {
            var xDecimal = Mathf.Abs(position.x - (int)position.x);
            var yDecimal = Mathf.Abs(position.y - (int)position.y);

            // TODO: Approximate?

            if (xDecimal == 0.5f)
            {
                var left = new Vector3(position.x - 0.5f, position.y);
                var right = new Vector3(position.x + 0.5f, position.y);
                return new Tuple<FloorComponent, FloorComponent>(
                    GetComponentAt<FloorComponent>(Mathf.RoundToInt(left.x), Mathf.RoundToInt(left.y)),
                    GetComponentAt<FloorComponent>(Mathf.RoundToInt(right.x), Mathf.RoundToInt(right.y)));
            }
            else if (yDecimal == 0.5f)
            {
                var down = new Vector3(position.x, position.y - 0.5f);
                var up = new Vector3(position.x, position.y + 0.5f);
                return new Tuple<FloorComponent, FloorComponent>(
                    GetComponentAt<FloorComponent>(Mathf.RoundToInt(down.x), Mathf.RoundToInt(down.y)),
                    GetComponentAt<FloorComponent>(Mathf.RoundToInt(up.x), Mathf.RoundToInt(up.y)));
            }
            else
            {
                Debug.LogError($"X: {xDecimal} Y: {yDecimal}");
                return new Tuple<FloorComponent, FloorComponent>(null, null);
            }
        }

        public void SaveMap()
        {
            // TODO
        }

        public void LoadMapFromSave()
        {
            // TODO
        }

        public void LoadMapTransition(MapModelConverter mapModelConverter, string mapIdentifier, string spawnReference)
        {
            // Save player state
            SavePlayerState(spawnReference);

            // Save map state (Delta)
            // TODO

            // Unload map
            UnloadMap();

            // Load map
            mapModelConverter.CreateMapFromModel();

            // Load map state (Delta)
            // TODO

            // Load players
            LoadPlayerState();
        }

        public void UnloadMap()
        {
            foreach (var model in ModelsMap.Values)
            {
                Destroy(model);
            }
            ModelsMap.Clear();

            ReferenceMap.Clear();
        }


        #region Player Loading

        private class PlayerTransferState
        {
            public readonly string SpawnPoint;

            public PlayerTransferState(string spawnPoint)
            {
                SpawnPoint = spawnPoint;
            }
        }

        private PlayerTransferState _pts;

        public void SavePlayerState(string spawnPoint)
        {
            if (!string.IsNullOrWhiteSpace(spawnPoint))
            {
                _pts = new PlayerTransferState(spawnPoint);
            }

            Destroy(GameObject.Find("player_go"));
            FindObjectOfType<PlayerController>().SelectedPlayerCharacter = null;
        }

        public void LoadPlayerState()
        {
            if (_pts != null)
            {
                if (!ReferenceMap.ContainsKey(_pts.SpawnPoint))
                {
                    Debug.LogError("Cannot find reference!");
                    return;
                }

                var character = new PlayableCharacter();
                FindObjectOfType<PlayerController>().SelectedPlayerCharacter = character;

                character.GameObject = new GameObject("player_go");
                var spriteRenderer = character.GameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GameRegistry.SpriteRegistry["actor_debug"];
                spriteRenderer.sortingLayerName = "Characters";

                character.GameObject.transform.position = ReferenceMap[_pts.SpawnPoint].Position;

                character.NextVertice = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(character.GameObject.transform.position)?.Vertice;

                character.GameObject.AddComponent<BoxCollider2D>();

                _pts = null;
            }
            else
            {
                var character = new PlayableCharacter();
                FindObjectOfType<PlayerController>().SelectedPlayerCharacter = character;

                character.GameObject = new GameObject("player_go");
                var spriteRenderer = character.GameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GameRegistry.SpriteRegistry["actor_debug"];
                spriteRenderer.sortingLayerName = "Characters";

                var components = GetComponentsOfType<FloorComponent>();
                character.GameObject.transform.position = components[Random.Range(0, components.Count)].transform.position;

                character.NextVertice = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(character.GameObject.transform.position)?.Vertice;

                character.GameObject.AddComponent<BoxCollider2D>();
            }
        }

        #endregion

        #region PathGraph

        private readonly List<GameObject> _graphEdges = new List<GameObject>();
        private readonly List<Tuple<Vector3, Vector3>> _coveredVertices = new List<Tuple<Vector3, Vector3>>();

        public void DrawVisibleGraph()
        {
            foreach (var floor in GetComponentsOfType<FloorComponent>())
            {
                if (floor.Vertice == null) continue;

                foreach (var vertex in floor.Vertice.NeighborList)
                {
                    var pos1 = floor.gameObject.transform.position;
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

        #endregion
    }
}