using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Code.Characters.PathFinding;
using Code.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Xml;

namespace Code.Characters.Player
{
    class PlayerController : MonoBehaviour
    {
        public static PlayerController ControllerInstance;
        public static PlayableCharacter PlayableCharacterInstance;

        public GameObject SelectedPlayerCharacter;
        public SpriteRenderer PlayerSpriteRenderer => SelectedPlayerCharacter.GetComponent<SpriteRenderer>();

        public LinkedList<Vertice> Path = new LinkedList<Vertice>();
        private Vertice _nextVertice;

        private Vector3 _sideRotation = Vector3.zero;

        private List<GameObject> _paths = new List<GameObject>();
        private List<Tuple<Vector3, Vector3>> _coveredVertices = new List<Tuple<Vector3, Vector3>>();
        private readonly Dictionary<Vector3, GameObject> _sideObjects = new Dictionary<Vector3, GameObject>();

        private MoveMode _moveMode;

        public void Start()
        {
            SelectedPlayerCharacter = gameObject;
            SelectedPlayerCharacter.name = "player_go";
            SelectedPlayerCharacter.AddComponent<SpriteRenderer>();
            PlayerSpriteRenderer.sprite = Resources.Load<Sprite>("player");
            PlayerSpriteRenderer.sortingLayerName = "Characters";
            SelectedPlayerCharacter.transform.position = new Vector3(0, 0);

            _nextVertice = Map.Instance.TileMap[Mathf.RoundToInt(SelectedPlayerCharacter.transform.position.x),
                Mathf.RoundToInt(SelectedPlayerCharacter.transform.position.y)].Vertice;

            SelectedPlayerCharacter.AddComponent<BoxCollider2D>();

            ControllerInstance = this;

            PlayableCharacterInstance = new PlayableCharacter();
        }

        public void Update()
        {
            UpdateMove();
            UpdateUi();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_paths.Count > 0)
                {
                    foreach (var path in _paths)
                    {
                        Destroy(path);
                    }

                    _paths.Clear();
                    _coveredVertices.Clear();
                }
                else
                {
                    foreach (var tile in Map.Instance.TileMap)
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

                            _paths.Add(go);

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
            }
        }

        public void UpdateUi()
        {
            if (PlayableCharacterInstance == null) return;
        }

        public void UpdateMove()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && _moveMode == MoveMode.Move)
            {
                MoveOnClick();
            }
            else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() &&
                     _moveMode == MoveMode.BuildWall)
            {
                BuildWall();
            }
            else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() &&
                     _moveMode == MoveMode.BuildDoor)
            {
                BuildDoor();
            }

            if (transform.position == _nextVertice.Tile.gameObject.transform.position && Path.Count > 0)
            {
                var currentVertice = _nextVertice;
                _nextVertice = Path.First.Value;
                Path.RemoveFirst();

                if (currentVertice != _nextVertice && !currentVertice.NeighborList.Contains(_nextVertice))
                {
                    _nextVertice = currentVertice;
                    _paths.Clear();
                }
            }

            SelectedPlayerCharacter.transform.position = Vector3.MoveTowards(SelectedPlayerCharacter.transform.position,
                _nextVertice.Tile.gameObject.transform.position, Time.deltaTime * PlayableCharacterInstance.Speed);

            if (transform.position != _nextVertice.Tile.gameObject.transform.position)
            {
                var vectorToTarget = _nextVertice.Tile.gameObject.transform.position - SelectedPlayerCharacter.transform.position;
                var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                var quaterion = Quaternion.AngleAxis(angle, Vector3.forward);

                SelectedPlayerCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, quaterion, Time.deltaTime * 10);
            }
        }

        private void BuildDoor()
        {
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildMode.Side);

            if (_sideObjects.ContainsKey(pos))
            {
                return;
            }

            var go = new GameObject("DoorComponent Frame");

            go.transform.position = pos;
            go.transform.rotation = go.transform.rotation = Quaternion.Euler(_sideRotation);

            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("door_frame");
            spriteRenderer.sortingLayerName = "BackgroundConstruction";

            _sideObjects.Add(go.transform.position, go);

            var doorGo = new GameObject("DoorComponent");
            doorGo.transform.position = pos;
            doorGo.transform.rotation = go.transform.rotation = Quaternion.Euler(_sideRotation);

            doorGo.transform.SetParent(go.transform);

            var doorSpriteRenderer = doorGo.AddComponent<SpriteRenderer>();
            doorSpriteRenderer.sprite = Resources.Load<Sprite>("door");
            doorSpriteRenderer.sortingLayerName = "DoorComponent";

            var door = doorGo.AddComponent<DoorComponent>();

            doorGo.AddComponent<BoxCollider2D>();

            var touchingSides = GetTouchingTiles(pos);
            door.SideA = touchingSides.Item1;
            door.SideB = touchingSides.Item2;

            if (door.SideA.XCoord == door.SideB.XCoord) // Vertical
            {
                door.TurnPoint = new Vector3(-0.45f, 0f) + pos;
            }
            else    // Horizontal
            {
                door.TurnPoint = new Vector3(0f, -0.45f) + pos;
            }

            door.DisconnectTiles();

            // TODO: Proper blocking when not connected to a wall
            // TODO: And think about how they should handle other actors blocking the way: Wait, Abort, or Recalc?
            //          This should really be a setting, so that the user can control how their actor should behave.
        }

        private void BuildWall()
        {
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildMode.Side);

            if (_sideObjects.ContainsKey(pos))
            {
                Debug.Log("Side already occupied!");
                return;
            }

            var go = new GameObject("Wall");

            go.transform.position = pos;
            go.transform.rotation = Quaternion.Euler(_sideRotation);

            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("flat_wall");
            spriteRenderer.sortingLayerName = "BackgroundConstruction";

            _sideObjects.Add(go.transform.position, go);

            var touchingSides = GetTouchingTiles(pos);

            if (touchingSides.Item1 != null && touchingSides.Item2 != null && touchingSides.Item1.CanEnter == true &&
                touchingSides.Item2.CanEnter == true)
            {
                touchingSides.Item1.Vertice.NeighborList.Remove(touchingSides.Item2.Vertice);
                touchingSides.Item2.Vertice.NeighborList.Remove(touchingSides.Item1.Vertice);

                // Remove potential diagonals

                if (touchingSides.Item1.XCoord == touchingSides.Item2.XCoord)   // Vertical
                {
                    var right1Tile = Map.Instance.GetTileAt(touchingSides.Item1.XCoord + 1, touchingSides.Item1.YCoord);
                    var left1Tile = Map.Instance.GetTileAt(touchingSides.Item1.XCoord - 1, touchingSides.Item1.YCoord);

                    var right2Tile = Map.Instance.GetTileAt(touchingSides.Item2.XCoord + 1, touchingSides.Item2.YCoord);
                    var left2Tile = Map.Instance.GetTileAt(touchingSides.Item2.XCoord - 1, touchingSides.Item2.YCoord);

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
                else    // Horizontal
                {
                    var up1Tile = Map.Instance.GetTileAt(touchingSides.Item1.XCoord, touchingSides.Item1.YCoord + 1);
                    var down1Tile = Map.Instance.GetTileAt(touchingSides.Item1.XCoord, touchingSides.Item1.YCoord - 1);

                    var up2Tile = Map.Instance.GetTileAt(touchingSides.Item2.XCoord, touchingSides.Item2.YCoord + 1);
                    var down2Tile = Map.Instance.GetTileAt(touchingSides.Item2.XCoord, touchingSides.Item2.YCoord - 1);

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

        public void Move(Vector3 mousePosition)
        {
            var clickPos = Camera.main.ScreenToWorldPoint(mousePosition);
            var x = Mathf.RoundToInt(clickPos.x);
            var y = Mathf.RoundToInt(clickPos.y);
            var target = Map.Instance.GetTileAt(x, y);
            if (target == null || target.CanEnter == false) return;

            if (SelectedPlayerCharacter.transform.position == _nextVertice.Tile.gameObject.transform.position)
            {
                var start = Map.Instance.GetTileAt(SelectedPlayerCharacter.transform.position);
                Path = PathFinder.AStar(Map.Instance.TileMap[start.XCoord, start.YCoord].Vertice,
                    Map.Instance.TileMap[target.XCoord, target.YCoord].Vertice);
            }
            else
            {
                var start = _nextVertice.Tile;
                Path = PathFinder.AStar(Map.Instance.TileMap[start.XCoord, start.YCoord].Vertice,
                    Map.Instance.TileMap[target.XCoord, target.YCoord].Vertice);
            }
        }

        public void StopMovement()
        {
            Path = new LinkedList<Vertice>();
        }

        private void MoveOnClick()
        {
            Move(Input.mousePosition);
        }

        public void SetModeMove()
        {
            _moveMode = MoveMode.Move;
        }

        public void SetModeWall()
        {
            _moveMode = MoveMode.BuildWall;
        }

        public void SetModelDoor()
        {
            _moveMode = MoveMode.BuildDoor;
        }

        public Tuple<Tile, Tile> GetTouchingTiles(Vector3 position)
        {
            var xDecimal = Mathf.Abs(position.x - (int)position.x);
            var yDecimal = Mathf.Abs(position.y - (int)position.y);

            if (xDecimal == 0.5f)
            {
                var left = new Vector3(position.x - 0.5f, position.y);
                var right = new Vector3(position.x + 0.5f, position.y);
                return new Tuple<Tile, Tile>(Map.Instance.GetTileAt(Mathf.RoundToInt(left.x), Mathf.RoundToInt(left.y)), Map.Instance.GetTileAt(Mathf.RoundToInt(right.x), Mathf.RoundToInt(right.y)));
            }
            else if (yDecimal == 0.5f)
            {
                var down = new Vector3(position.x, position.y - 0.5f);
                var up = new Vector3(position.x, position.y + 0.5f);
                return new Tuple<Tile, Tile>(Map.Instance.GetTileAt(Mathf.RoundToInt(down.x), Mathf.RoundToInt(down.y)), Map.Instance.GetTileAt(Mathf.RoundToInt(up.x), Mathf.RoundToInt(up.y)));
            }
            else
            {
                Debug.LogError($"X: {xDecimal} Y: {yDecimal}");
                return new Tuple<Tile, Tile>(null, null);
            }
        }

        public Vector3 RoundToBuildModePosition(Vector3 position, BuildMode buildMode)
        {
            switch (buildMode)
            {
                case BuildMode.Center:
                    return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
                case BuildMode.Corner:
                    return new Vector3(Mathf.Round(position.x + 0.5f) - 0.5f, Mathf.Round(position.y + 0.5f) - 0.5f);
                case BuildMode.Side:
                    var xDecimal = position.x - (int)position.x;
                    var yDecimal = position.y - (int)position.y;

                    if (position.x < 0)
                    {
                        xDecimal = 1 - Mathf.Abs(xDecimal);
                    }

                    if (position.y < 0)
                    {
                        yDecimal = 1 - Mathf.Abs(yDecimal);
                    }

                    if (xDecimal >= 0.5f && yDecimal >= 0.5f)
                    {
                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) - 0.5f);  // Down
                        }
                        else
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) - 0.5f, Mathf.RoundToInt(position.y));  // Left
                        }
                    }
                    else if (yDecimal >= 0.5f)
                    {
                        yDecimal -= 0.5f;
                        xDecimal = 0.5f - xDecimal;

                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) - 0.5f);  // Down
                        }
                        else
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y));  // Right
                        }
                    }
                    else if (xDecimal >= 0.5f)
                    {
                        xDecimal -= 0.5f;
                        yDecimal = 0.5f - yDecimal;

                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + 0.5f);  // Up
                        }
                        else
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) - 0.5f, Mathf.RoundToInt(position.y));  // Left
                        }
                    }
                    else
                    {
                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y));  // Right
                        }
                        else
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + 0.5f);  // Up
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildMode), buildMode, null);
            }
        }
    }

    public enum MoveMode
    {
        Move,
        BuildWall,
        BuildDoor
    }

    public enum BuildMode
    {
        Center,
        Corner,
        Side
    }

    public class DoorComponent : MonoBehaviour
    {
        public Vector3 TurnPoint;

        public bool IsOpen { get; private set; } = false;

        public Tile SideA;
        public Tile SideB;

        private void Update()
        {
            // TODO: Animation?
        }

        public void ToggleDoor()
        {
            if (IsOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        public void OpenDoor()
        {
            IsOpen = true;
            transform.RotateAround(TurnPoint, Vector3.forward, 90f);
            ConnectTiles();
        }

        public void CloseDoor()
        {
            IsOpen = false;
            transform.RotateAround(TurnPoint, Vector3.forward, -90f);
            DisconnectTiles();
        }

        public void ConnectTiles()
        {
            SideA.Vertice.NeighborList.Add(SideB.Vertice);
            SideB.Vertice.NeighborList.Add(SideA.Vertice);
        }

        public void DisconnectTiles()
        {
            SideA.Vertice.NeighborList.Remove(SideB.Vertice);
            SideB.Vertice.NeighborList.Remove(SideA.Vertice);
        }
    }

    // Model Testing Grounds, these are for building, save state is seperate

    [Serializable]
    public class BaseModel
    {
        public string Identifier { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public string SortingLayer { get; set; } = "Default";
        public bool Independent { get; set; } = true;   // Sets if the model can be build directly. Some models used in LayerModel may not work directly.
    }

    [Serializable]
    public class GroundModel : BaseModel
    {
        public string MaterialName { get; set; }
        public bool CanEnter { get; set; }
    }

    [Serializable]
    public class WallModel : BaseModel
    {
        public string MaterialName { get; set; }
    }

    [Serializable]
    public class LayerModel : BaseModel
    {
        public List<string> SubModels { get; set; } = new List<string>();   // Uses identifier
    }

    [Serializable]
    public class DoorModel : BaseModel
    {
        public Vector3 TurnPoint { get; set; }
    }
}