using System;
using System.IO;
using System.Linq;
using UnityEngine;
using World;

namespace Models
{
    public class DoorModel : Model
    {
        public string DoorSpriteName { get; set; }
        public string DoorSortingLayer { get; set; }
        public string FrameFrameSpriteName { get; set; }
        public string FrameFrameSortingLayer { get; set; }
        public float TurnPoint { get; set; }

        public DoorModel(string identifier, string displayName, string frameSpriteName, string doorSpriteName,
            string frameSortingLayer, string doorSortingLayer, float turnPoint)
        {
            Identifier = identifier;
            DisplayName = displayName;
            TurnPoint = turnPoint;

            FrameFrameSpriteName = frameSpriteName;
            FrameFrameSortingLayer = frameSortingLayer;

            DoorSpriteName = doorSpriteName;
            DoorSortingLayer = doorSortingLayer;
        }

        protected DoorModel()
        {
        }

        public override GameObject InstantiateGame(Vector3 position)
        {
            return InstantiateGame(position, Vector3.zero);
        }

        public override GameObject InstantiateGame(Vector3 position, Vector3 rotation)
        {
            var go = new GameObject(Identifier);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(rotation);

            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(FrameFrameSpriteName);
            spriteRenderer.sortingLayerName = FrameFrameSortingLayer;

            var wall = go.AddComponent<Wall>();
            wall.Configure(Identifier);

            var doorGo = new GameObject();
            doorGo.transform.SetParent(go.transform);
            doorGo.transform.position = position;
            doorGo.transform.rotation = Quaternion.Euler(rotation);

            doorGo.AddComponent<BoxCollider2D>();

            var doorSpriteRenderer = doorGo.AddComponent<SpriteRenderer>();
            doorSpriteRenderer.sprite = Resources.Load<Sprite>(DoorSpriteName);
            doorSpriteRenderer.sortingLayerName = DoorSortingLayer;
            
            var door = doorGo.AddComponent<Door>();
            
            var remaining = position.x - (int) position.x;

            if (remaining == 0f)
            {
                door.Configure(Identifier, new Vector3(TurnPoint, 0) + position);
            }
            else if (remaining == 0.5f)
            {
                door.Configure(Identifier, new Vector3(0, TurnPoint) + position);
            }
            else
            {
                Debug.LogError("Rounding error!");
            }

            return go;
        }

        public override GameObject InstantiateEditor(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public override GameObject InstantiateEditor(Vector3 position, Vector3 rotation)
        {
            throw new NotImplementedException();
        }

        private Tuple<Tile, Tile> GetTouchingTiles(Vector3 position)
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
    }
}