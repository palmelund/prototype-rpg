using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Characters.PathFinding;
using Assets.Code.Items;
using Assets.Code.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code.Characters
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        
        public Vector3 Position;
        public Transform Transform;

        public GameObject PlayerGameObject;
        public SpriteRenderer PlayerSpriteRenderer;

        public Stack<PathMember> Path = new Stack<PathMember>();
        private PathMember _nextTile;

        public Npc.Npc Target;

        public GameObject TargetMarker;

        public Text PlayerHitPointText;


        public int PlayerHitPointCurrent { get; private set; }
        public int PlayerHitPointMax { get; private set; }
        public float PlayerSpeed { get; private set; }

        public TestMeleeWeapon Weapon = new TestMeleeWeapon();

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

            PlayerGameObject.AddComponent<BoxCollider2D>();

            PlayerGameObject.AddComponent<CustomComponentType>().Type = ComponentType.Player;
            PlayerGameObject.AddComponent<PlayerComponent>().Player = this;

            Instance = this;

            PlayerSpeed = 1;
            PlayerHitPointCurrent = 10;
            PlayerHitPointMax = 10;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                MoveOnClick();
            }
            /*
            if (Target != null && GameState.Euclidean(PlayerGameObject.transform.position, Target.NpcGameObject.transform.position) <= 1.4142135f) // Attack range, to be fixed
            {
                // Todo: Attack speed, range, damage, etc...
                // Todo: Move towards target, cancel target
                Debug.Log("Attack!");
            }
            */

            if (Target != null && GameState.Euclidean(PlayerGameObject.transform.position, Target.NpcGameObject.transform.position) <=
    Weapon.Range)
            {
                if (Weapon.CoolDown <= Time.time)
                {
                    // Attack
                    Debug.Log("Attack: " + Time.time);
                    Target.Attack(Weapon.Damage);
                    Weapon.CoolDown = Time.time + Weapon.AttackSpeed;
                }
            }

            if (Transform.position == Position && Path.Count > 0)
            {
                SetNextPathTile();
            }

            PlayerGameObject.transform.position = Vector3.MoveTowards(PlayerGameObject.transform.position, Position, Time.deltaTime * PlayerSpeed);

            PlayerHitPointText.text = string.Format("HP: {0} / {1}", PlayerHitPointCurrent, PlayerHitPointMax);
        }

        public void Move(Vector3 mousePosition)
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

        private void MoveOnClick()
        {
            Move(Input.mousePosition);
            if (Target != null)
            {
                Target = null;
                TargetMarker.SetActive(false);
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
