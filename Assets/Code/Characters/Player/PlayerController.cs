using System;
using System.Collections.Generic;
using Code.Characters.PathFinding;
using Code.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Characters.Player
{
    class PlayerController : MonoBehaviour
    {
        public static PlayerController ControllerInstance;
        public static Player PlayerInstance;

        public Vector3 Position;
        public Transform Transform;

        public GameObject PlayerGameObject;
        public SpriteRenderer PlayerSpriteRenderer;

        public Stack<PathMember> Path = new Stack<PathMember>();
        private PathMember _nextTile;

        public Npc.Npc Target;

        public GameObject TargetMarker; // Set in editor
        public GameObject FocusMarker;  // Set in editor

        public Text PlayerHitPointText; // Set in editor

        public void Start()
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

            PlayerGameObject.AddComponent<BoxCollider2D>();

            PlayerGameObject.AddComponent<CustomComponentType>().Type = ComponentType.Player;
            PlayerGameObject.AddComponent<PlayerComponent>().Player = PlayerInstance;

            ControllerInstance = this;

            PlayerInstance = new Player();
        }

        public void Update()
        {
            UpdateMove();
            UpdateUi();
        }

        public void UpdateUi()
        {
            if (PlayerInstance == null) return;
            PlayerHitPointText.text = string.Format("HP: {0} / {1}", PlayerInstance.HitPointCurrent, PlayerInstance.HitPointMax);
        }

        public void UpdateMove()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                MoveOnClick();
            }

            if (Transform.position == Position && Path.Count > 0)
            {
                SetNextPathTile();
            }

            PlayerGameObject.transform.position = Vector3.MoveTowards(PlayerGameObject.transform.position, Position, Time.deltaTime * PlayerInstance.Speed);
        }

        public void Move(Vector3 mousePosition)
        {
            var clickPos = Camera.main.ScreenToWorldPoint(mousePosition);
            var x = Mathf.RoundToInt(clickPos.x);
            var y = Mathf.RoundToInt(clickPos.y);
            var target = Map.Instance.GetTileAt(x, y);
            if (target == null || target.CanEnter == false) return;

            if (PlayerGameObject.transform.position == Position)
            {
                var start = Map.Instance.GetTileAt(PlayerGameObject.transform.position);
                Path = PathFinder.AStar(Map.Instance.Graph[start.XCoord, start.YCoord],
                    Map.Instance.Graph[target.XCoord, target.YCoord]);
            }
            else
            {
                var start = _nextTile.Destination;
                Path = PathFinder.AStar(Map.Instance.Graph[start.XCoord, start.YCoord],
                    Map.Instance.Graph[target.XCoord, target.YCoord]);
            }
        }

        public void StopMovement()
        {
            Path = new Stack<PathMember>();
        }

        public void SetTarget(Npc.Npc targetNpc)
        {
            TargetMarker.SetActive(true);
            TargetMarker.transform.SetParent(targetNpc.NpcGameObject.transform);
            TargetMarker.transform.position = new Vector3(targetNpc.NpcGameObject.transform.position.x, targetNpc.NpcGameObject.transform.position.y, 10);
            Target = targetNpc;
        }

        public void SetFocus(Npc.Npc targetNpc)
        {
            FocusMarker.SetActive(true);
            FocusMarker.transform.SetParent(targetNpc.NpcGameObject.transform);
            FocusMarker.transform.position = new Vector3(targetNpc.NpcGameObject.transform.position.x, targetNpc.NpcGameObject.transform.position.y, 10);
            Target = targetNpc;
        }

        private void MoveOnClick()
        {
            Move(Input.mousePosition);
            if (Target != null)
            {
                Target = null;
                TargetMarker.SetActive(false);

                // TODO: Temp
                FocusMarker.SetActive(false);
            }
        }

        private void SetNextPathTile()
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
    }
}
