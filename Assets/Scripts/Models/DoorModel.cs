using System;
using System.IO;
using System.Linq;
using UnityEngine;
using World;

namespace Models
{
    public class DoorModel : BaseModel
    {
        public float TurnPoint { get; set; }
        public string DoorSpriteName { get; protected set; }
        public string DoorSortingLayer { get; protected set; }

        public DoorModel(string fileName)
        {
            LoadFromFile(fileName);
        }

        public override GameObject Instantiate(Vector3 position)
        {
            return Instantiate(position, Vector3.zero);
        }

        public override GameObject Instantiate(Vector3 position, Vector3 rotation)
        {
            var go = new GameObject(Identifier);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(rotation);

            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(SpriteName);
            spriteRenderer.sortingLayerName = SortingLayer;

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

        protected sealed override void LoadFromFile(string fileName)
        {
            var modelString = File.ReadAllLines(fileName);
            modelString = modelString.Where(s => !string.IsNullOrWhiteSpace(s)).Where(s => !s.TrimStart().StartsWith("#")).ToArray();
            if (!modelString[0].Equals("<DoorModel>"))
            {
                Debug.LogError(modelString);
            }

            var identifier = string.Empty;
            var spriteName = string.Empty;
            var doorSpriteName = string.Empty;
            var sortingLayer = string.Empty;
            var doorSortingLayer = string.Empty;
            float turnPoint = 0;

            for (var index = 1; index < modelString.Length; index++)
            {
                if (modelString[index].StartsWith("identifier:"))
                {
                    identifier = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                }
                else if (modelString[index].StartsWith("spriteName:"))
                {
                    spriteName = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                }
                else if (modelString[index].StartsWith("doorSpriteName:"))
                {
                    doorSpriteName = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                }
                else if (modelString[index].StartsWith("sortingLayer:"))
                {
                    sortingLayer = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                }
                else if (modelString[index].StartsWith("doorSortingLayer:"))
                {
                    doorSortingLayer = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                }
                else if (modelString[index].StartsWith("turnPoint:"))
                {
                    var tmp = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                    if (!float.TryParse(tmp, out turnPoint))
                    {
                        Debug.Log("turnPointX not a float value!");
                    }
                }
                else
                {
                    Debug.LogError("Cannot parse line: " + modelString[index]);
                }
            }

            Identifier = identifier;
            SpriteName = spriteName;
            DoorSpriteName = doorSpriteName;
            SortingLayer = sortingLayer;
            DoorSortingLayer = doorSortingLayer;
            TurnPoint = turnPoint;
        }
    }
}