using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class player : MonoBehaviour
    {

        public float Speed = 1f;
        public Vector3 Position;
        public Transform Transform;

        public GameObject GameObject;
        public SpriteRenderer SpriteRenderer;

        public Stack<PathMember> Path = new Stack<PathMember>();
        private PathMember NextTile;
        // Use this for initialization
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

            NextTile = new PathMember(map.Instance.GetTileAt(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)), PathFinderDirection.Stay);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(clickPos.x);
                int y = Mathf.RoundToInt(clickPos.y);
                tile dist = map.Instance.GetTileAt(x,y);
                if (dist != null)
                {
                    PathMarker.ClearPath();
                    int myX = Mathf.RoundToInt(GameObject.transform.position.x);
                    int myY = Mathf.RoundToInt(GameObject.transform.position.y);
                    tile start = map.Instance.GetTileAt(myX, myY);
                    Path = PathFinder.AllCase(start, dist);

                    if (map.Instance.GetTileAt(myX, myY).Equals(NextTile.Destination))
                    {
                        PathMarker.CreatePath(Path);
                    }
                    else
                    {
                        Path.Push(new PathMember(start, PathFinderDirection.Stay));
                        PathMarker.CreatePath(Path);
                    }
                }
            }

            if (Transform.position == Position && Path.Count > 0)
            {
                PathMarker.ClearNext();
                NextTile = Path.Pop();
                switch (NextTile.Direction)
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
    }
}
