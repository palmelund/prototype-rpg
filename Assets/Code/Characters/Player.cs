using System;
using System.Collections.Generic;
using Assets.Code.PathFinding;
using Assets.Code.World;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Characters
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        public float Speed = 1f;
        public Vector3 Position;
        public Transform Transform;

        public GameObject PlayerGameObject;
        public SpriteRenderer PlayerSpriteRenderer;

        public Stack<PathMember> Path = new Stack<PathMember>();
        private PathMember _nextTile;
        
        private PathFinder _pathFinder;
        
        void Start()
        {
            PlayerGameObject = gameObject; // PlayerGameObject("player");
            PlayerGameObject.name = "player_go";
            PlayerSpriteRenderer = PlayerGameObject.AddComponent<SpriteRenderer>();
            PlayerSpriteRenderer.sprite = Resources.Load<Sprite>("player");
            PlayerSpriteRenderer.sortingLayerName = "Characters";
            PlayerGameObject.transform.position = new Vector3(0, 0);

            Position = PlayerGameObject.transform.position;
            Transform = PlayerGameObject.transform;

            _nextTile = new PathMember(Map.Instance.GetTileAt(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)), PathFinderDirection.Stay);
            _pathFinder = new PathFinder();

            PlayerGameObject.AddComponent<BoxCollider2D>();

            Instance = this;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //PathFinder(Input.mousePosition);
                PathFinder(Input.mousePosition);
            }

            if (Transform.position == Position && Path.Count > 0)
            {
                _nextTile = Path.Pop();
                switch (_nextTile.Direction)
                {
                    case PathFinderDirection.Up:
                        Position += Vector3.up;
                        break;
                    case PathFinderDirection.UpRight:
                        Position += Vector3.up;
                        Position += Vector3.right;
                        break;
                    case PathFinderDirection.Right:
                        Position += Vector3.right;
                        break;
                    case PathFinderDirection.DownRight:
                        Position += Vector3.right;
                        Position += Vector3.down;
                        break;
                    case PathFinderDirection.Down:
                        Position += Vector3.down;
                        break;
                    case PathFinderDirection.DownLeft:
                        Position += Vector3.down;
                        Position += Vector3.left;
                        break;
                    case PathFinderDirection.Left:
                        Position += Vector3.left;
                        break;
                    case PathFinderDirection.UpLeft:
                        Position += Vector3.left;
                        Position += Vector3.up;
                        break;
                    case PathFinderDirection.Stay:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            PlayerGameObject.transform.position = Vector3.MoveTowards(PlayerGameObject.transform.position, Position, Time.deltaTime * Speed);
        }

        public void PathFinder(Vector3 mousePosition)
        {
            var clickPos = Camera.main.ScreenToWorldPoint(mousePosition);
            var x = Mathf.RoundToInt(clickPos.x);
            var y = Mathf.RoundToInt(clickPos.y);
            var target = Map.Instance.GetTileAt(x, y);
            if (target != null)
            {
                if (PlayerGameObject.transform.position == Position)
                {
                    var start = Map.Instance.GetTileAt(PlayerGameObject.transform.position);
                    Path = _pathFinder.AStar(Map.Instance.Graph[start.XCoord, start.YCoord],
                        Map.Instance.Graph[target.XCoord, target.YCoord]);
                }
                else
                {
                    var start = _nextTile.Destination;
                    Path = _pathFinder.AStar(Map.Instance.Graph[start.XCoord, start.YCoord],
                        Map.Instance.Graph[target.XCoord, target.YCoord]);
                }
            }
        }
    }
}
