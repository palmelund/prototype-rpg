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

        public Npc(string npcId, string npcName)
        {
            Name = npcName;
            NpcId = npcId;

            NpcGameObject = new GameObject { name = npcId };
            NpcSpriteRenderer = NpcGameObject.AddComponent<SpriteRenderer>();
            NpcSpriteRenderer.sprite = Resources.Load<Sprite>("player");
            NpcSpriteRenderer.sortingLayerName = "Characters";
            NpcGameObject.transform.position = new Vector3(GameState.Rand.Next(0, 10), GameState.Rand.Next(0, 10));
            NpcGameObject.AddComponent<BoxCollider2D>();

            NpcGameObject.AddComponent<CustomComponentType>().Type = ComponentType.Npc;
            NpcGameObject.AddComponent<NpcComponent>().Npc = this;

            Position = NpcGameObject.transform.position;
            Transform = NpcGameObject.transform;

            NextTile = new PathMember(Map.Instance.GetTileAt(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)), PathFinderDirection.Stay);
        }

        public void Move(int x, int y)
        {
            var target = Map.Instance.GetTileAt(x, y);
            if (target != null)
            {
                if (NpcGameObject.transform.position == Position)
                {
                    var start = Map.Instance.GetTileAt(NpcGameObject.transform.position);
                    Path = PathFinder.AStar(Map.Instance.Graph[start.XCoord, start.YCoord],
                        Map.Instance.Graph[target.XCoord, target.YCoord]);
                }
                else
                {
                    var start = NextTile.Destination;
                    Path = PathFinder.AStar(Map.Instance.Graph[start.XCoord, start.YCoord],
                        Map.Instance.Graph[target.XCoord, target.YCoord]);
                }
            }
        }
    }
}
