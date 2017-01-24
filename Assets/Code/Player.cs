using System;
using System.Collections.Generic;
using Assets.Code.PathFinding;
using Assets.Code.World;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        public float Speed = 1f;
        public Vector3 Position;
        public Transform Transform;

        public GameObject GameObject;
        public SpriteRenderer SpriteRenderer;

        public Stack<PathMember> Path = new Stack<PathMember>();
        private PathMember _nextTile;

        private AllPathFinder _allPathFinder;
        
        void Start()
        {
            GameObject = gameObject; // GameObject("player");
            GameObject.name = "player_go";
            SpriteRenderer = GameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.sprite = Resources.Load<Sprite>("player");
            SpriteRenderer.sortingLayerName = "Characters";
            GameObject.transform.position = new Vector3(0, 0);

            Position = GameObject.transform.position;
            Transform = GameObject.transform;

            _nextTile = new PathMember(Map.Instance.GetTileAt(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)), PathFinderDirection.Stay);
            _allPathFinder = new AllPathFinder(Map.Instance);

            GameObject.AddComponent<BoxCollider2D>();

            Instance = this;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
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

            GameObject.transform.position = Vector3.MoveTowards(GameObject.transform.position, Position, Time.deltaTime * Speed);
        }

        public void PathFinder(Vector3 mousePosition)
        {
            var clickPos = Camera.main.ScreenToWorldPoint(mousePosition);
            var x = Mathf.RoundToInt(clickPos.x);
            var y = Mathf.RoundToInt(clickPos.y);
            var target = Map.Instance.GetTileAt(x, y);
            if (target != null)
            {
                if (GameObject.transform.position == Position)
                {
                    var start = Map.Instance.GetTileAt(GameObject.transform.position);
                    //Path = PathFinder.AllCase(start.Node, dist.Node);
                    Path = _allPathFinder.PathFinder(start, target);
                }
                else
                {
                    //Path = PathFinder.AllCase(_nextTile.Destination.Node, dist.Node);
                    Path = _allPathFinder.PathFinder(_nextTile.Destination, target);
                }
            }
        }
    }
}
