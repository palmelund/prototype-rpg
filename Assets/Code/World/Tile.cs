﻿using Assets.Code.Characters;
using Assets.Code.Characters.Npc;
using Assets.Code.Characters.Player;
using UnityEngine;

namespace Assets.Code.World
{
    public class Tile
    {
        public int XCoord;
        public int YCoord;
        public GameObject GameObject;
        public SpriteRenderer SpriteRenderer;

        public bool CanEnter = true;

        private const bool DrawRandomWalls = true;

        public Tile(int x, int y)
        {
            XCoord = x;
            YCoord = y;
            GameObject = new GameObject(string.Format("tile_x_{0}_y_{1}", x, y));
            SpriteRenderer = GameObject.AddComponent<SpriteRenderer>();
            if (DrawRandomWalls && GameState.Rand.Next(10) == 0)  // TODO: Always false
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("wall");
                CanEnter = false;
            }
            else
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("grass");
            }
            SpriteRenderer.sortingLayerName = "BackgroundTiles";
            GameObject.transform.position = new Vector3(x, y);

            GameObject.AddComponent<BoxCollider2D>();   // Used for right-clicking currently

            GameObject.AddComponent<CustomComponentType>().Type = ComponentType.Tile;
            GameObject.AddComponent<TileComponent>().Tile = this;
        }
    }

    public enum ComponentType
    {
        Tile,
        Player,
        Npc,
    }

    public class CustomComponentType : MonoBehaviour
    {
        public ComponentType Type;
    }

    public class TileComponent : MonoBehaviour
    {
        public Tile Tile;
    }

    public class PlayerComponent : MonoBehaviour
    {
        public Player Player;
    }

    public class NpcComponent : MonoBehaviour
    {
        public Npc Npc;
    }
}
