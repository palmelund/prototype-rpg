using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using World;

namespace Models
{
    public class FloorModel : Model
    {
        public string SpriteName { get; set; }
        public string SortingLayer { get; set; }

        // TODO: Should not have non-empty publich constructor
        public bool CanEnter { get; set; }

        public FloorModel(string identifier, string displayName, string sortingLayer, string spriteName, bool canEnter)
        {
            Identifier = identifier;
            DisplayName = displayName;

            SpriteName = spriteName;
            SortingLayer = sortingLayer;

            CanEnter = canEnter;
        }

        protected FloorModel()
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
            var tile = go.AddComponent<Tile>();
            tile.Configure(Identifier, CanEnter, SpriteName);

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