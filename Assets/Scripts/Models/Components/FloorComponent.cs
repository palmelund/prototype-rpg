using System.Collections.Generic;
using Characters.PathFinding;
using GameEditor.MapEditor;
using GameEditor.MapEditor.MapModelEditors;
using UnityEngine;

namespace Models.Components
{
    public class FloorComponent : MonoBehaviour, IWorldComponent
    {
        public int X => Mathf.RoundToInt(transform.position.x);
        public int Y => Mathf.RoundToInt(transform.position.y);

        public Vertice Vertice;

        public bool CanEnter = true;
        
        public string Identifier { get; set; }
        public List<string> References { get; set; }
        public void OpenEditorWindow()
        {
            if (FindObjectOfType<MapBuilder>().OpenWindowMap.ContainsKey(this))
            {
                Debug.Log("Window already open");
                return;
            }
            var go = FloorMapModelEditor.CreateFromData(this);
            FindObjectOfType<MapBuilder>().OpenWindowMap.Add(this, go);
        }

        //public void Configure(Vector3 position)
        //{
        //    //XCoord = Mathf.RoundToInt(position.x);
        //    //YCoord = Mathf.RoundToInt(position.y);
        //    //name = $"tile_x_{XCoord}_y_{YCoord}";

        //    Assert.AreEqual(position.x, Mathf.Round(position.x));
        //    Assert.AreEqual(position.y, Mathf.Round(position.y));
        //    Assert.AreEqual(position.z, Mathf.Round(position.z));

        //    var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //    spriteRenderer.sprite = Resources.Load<Sprite>("grass");
        //    FrameSpriteName = "grass";
        //    spriteRenderer.sortingLayerName = "BackgroundTiles";
        //    gameObject.transform.position = position;

        //    gameObject.AddComponent<BoxCollider2D>();
        //}

        public void Configure(string identifier, List<string> references, bool canEnter, string sprite)
        {
            References = references;
            gameObject.GetComponent<SpriteRenderer>().sprite = GameRegistry.SpriteRegistry[sprite];
            Identifier = identifier;
            CanEnter = canEnter;
        }
    }
}
