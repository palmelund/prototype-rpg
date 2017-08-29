using System.IO;
using System.Linq;
using UnityEngine;
using World;

namespace Models
{
    public class FloorModel : BaseModel
    {
        public bool CanEnter { get; protected set; }
        
        public FloorModel(string fileName)
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
            var tile = go.AddComponent<Tile>();
            tile.Configure(Identifier, CanEnter, SpriteName);

            return go;
        }

        protected sealed override void LoadFromFile(string fileName)
        {
            var modelString = File.ReadAllLines(fileName);
            modelString = modelString.Where(s => !string.IsNullOrWhiteSpace(s)).Where(s => !s.TrimStart().StartsWith("#")).ToArray();
            if (!modelString[0].Equals("<FloorModel>"))
            {
                Debug.LogError(modelString);
            }

            var identifier = string.Empty;
            var spriteName = string.Empty;
            var sortingLayer = string.Empty;
            var canEnter = false;

            for (var index = 1; index < modelString.Length; index++)
            {
                if (modelString[index].StartsWith("identifier:"))
                {
                    if (identifier.Equals(string.Empty))
                    {
                        identifier = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                    }
                    else
                    {
                        Debug.Log("identifier already set!");
                    }
                }
                else if (modelString[index].StartsWith("spriteName:"))
                {
                    if (spriteName.Equals(string.Empty))
                    {
                        spriteName = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                    }
                    else
                    {
                        Debug.Log("spriteName already set!");
                    }
                }
                else if (modelString[index].StartsWith("sortingLayer:"))
                {
                    if (sortingLayer.Equals(string.Empty))
                    {
                        sortingLayer = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                    }
                    else
                    {
                        Debug.Log("sortingLayer already set!");
                    }
                }
                else if (modelString[index].StartsWith("canEnter:"))
                {
                    var tmp = modelString[index].Split(new[] { ':' }, 2)[1].Trim();
                    if (!bool.TryParse(tmp, out canEnter))
                    {
                        Debug.Log("canEnter not a boolean value!");
                    }
                }
                else
                {
                    Debug.LogError("Cannot parse line: " + modelString[index]);
                }
            }

            Identifier = identifier;
            SpriteName = spriteName;
            SortingLayer = sortingLayer;
            CanEnter = canEnter;
        }
    }
}