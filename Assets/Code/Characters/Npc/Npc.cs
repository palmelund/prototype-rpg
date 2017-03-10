using System.Collections.Generic;
using Assets.Code.Characters.PathFinding;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.Characters.Npc
{
    public class Npc
    {

        public string NpcId;
        public string Name;
        public GameObject NpcGameObject;
        public SpriteRenderer NpcSpriteRenderer;

        public float Speed = 1f;
        public Vector3 Position;
        public Transform Transform;

        public Stack<PathMember> Path = new Stack<PathMember>();
        internal PathMember NextTile;

        public int NpcHitPointCurrent { get; private set; }
        public int NpcHitPointMax { get; private set; }

        public Npc(string npcId, string npcName, int hpMax)
        {
            Name = npcName;
            NpcId = npcId;

            NpcGameObject = new GameObject { name = npcId };
            NpcSpriteRenderer = NpcGameObject.AddComponent<SpriteRenderer>();
            NpcSpriteRenderer.sprite = Resources.Load<Sprite>("player");
            NpcSpriteRenderer.sortingLayerName = "Characters";

            var xpos = 0;
            var ypos = 0;
            var validStartPosition = false;
            while (!validStartPosition)
            {
                xpos = GameState.Rand.Next(0, 10);
                ypos = GameState.Rand.Next(0, 10);
                if (Map.Instance.Graph[xpos, ypos] != null) validStartPosition = true;
            }

            NpcGameObject.transform.position = new Vector3(xpos, ypos);
            NpcGameObject.AddComponent<BoxCollider2D>();

            NpcGameObject.AddComponent<CustomComponentType>().Type = ComponentType.Npc;
            NpcGameObject.AddComponent<NpcComponent>().Npc = this;

            Position = NpcGameObject.transform.position;
            Transform = NpcGameObject.transform;

            NpcHitPointCurrent = hpMax;
            NpcHitPointMax = hpMax;

            NextTile = new PathMember(Map.Instance.GetTileAt(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)), PathFinderDirection.Stay);
        }

        public void Move(int x, int y)
        {
            var target = Map.Instance.GetTileAt(x, y);
            var targetNode = Map.Instance.GetTileNode(target);

            if (targetNode != null)
            {
                if (NpcGameObject.transform.position == Position)
                {
                    var start = Map.Instance.GetTileAt(NpcGameObject.transform.position);
                    var startNode = Map.Instance.GetTileNode(start);
                    Path = PathFinder.AStar(startNode, targetNode);
                }
                else
                {
                    var start = NextTile.Destination;
                    var startNode = Map.Instance.GetTileNode(start);
                    Path = PathFinder.AStar(startNode, targetNode);
                }
            }
        }

        public void Attack(int damage)
        {
            NpcHitPointCurrent -= damage;
            if (NpcHitPointCurrent <= 0)
            {
                var child = NpcGameObject.transform.GetChild(0).gameObject;
                if(child != null)
                {
                    child.transform.SetParent(null);
                    child.SetActive(false);
                }
                NpcController.NpcList.Remove(this);
                if (Player.Instance.Target.Equals(this))
                {
                    Player.Instance.Target = null;
                }
                Object.Destroy(NpcGameObject);
                Debug.Log(string.Format("Npc \"{0}\" | \"{1}\" has beem killed!", NpcId, Name));
            }
        }
    }
}
