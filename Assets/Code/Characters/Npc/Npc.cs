using System.Collections.Generic;
using Code.Characters.PathFinding;
using Code.World;
using UnityEngine;

namespace Code.Characters.Npc
{
    public class Npc : Character
    {
        public string NpcId;
        public GameObject NpcGameObject;
        public SpriteRenderer NpcSpriteRenderer;
        
        public Vector3 Position;
        public Transform Transform;

        public Stack<PathMember> Path = new Stack<PathMember>();
        internal PathMember NextTile;

        public Npc(string npcId, string npcName, int hpMax)
        {
            Name = npcName;
            NpcId = npcId;

            NpcGameObject = new GameObject { name = npcId };
            NpcSpriteRenderer = NpcGameObject.AddComponent<SpriteRenderer>();
            NpcSpriteRenderer.sprite = Resources.Load<Sprite>("player");
            NpcSpriteRenderer.sortingLayerName = "Characters";
            Initiative = 5 + GameState.Rand.Next(0, 10);

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

            HitPointCurrent = hpMax;
            HitPointMax = hpMax;

            NextTile = new PathMember(Map.Instance.GetTileAt(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)), PathFinderDirection.Stay);
        }

        public void Move(int x, int y)
        {
            var target = Map.Instance.GetTileAt(x, y);

            if (target.CanEnter == false) return;

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
    }
}
