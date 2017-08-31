using System.IO;
using System.Linq;
using UnityEngine;
using World;

namespace Models
{
    public class WallModel : Model
    {
        public string SpriteName { get; set; }
        public string SortingLayer { get; set; }

        public WallModel(string identifier, string displayName, string sortingLayer, string spriteName)
        {
            Identifier = identifier;
            DisplayName = displayName;
            SortingLayer = sortingLayer;
            SpriteName = spriteName;
        }

        protected WallModel()
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
            spriteRenderer.sprite = Resources.Load<Sprite>(SpriteName);
            spriteRenderer.sortingLayerName = SortingLayer;

            var wall = go.AddComponent<Wall>();
            wall.Configure(Identifier);

            return go;
        }

        public override GameObject InstantiateEditor(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        public override GameObject InstantiateEditor(Vector3 position, Vector3 rotation)
        {
            throw new System.NotImplementedException();
        }
    }
}