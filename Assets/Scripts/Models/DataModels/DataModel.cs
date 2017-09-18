using System.IO;
using System.Xml.Serialization;
using GameEditor.MapEditor;
using Global;
using UnityEngine;

namespace Models.DataModels
{
    public abstract class DataModel : IGameSerializable
    {
        public string Identifier { get; set; }
        public abstract GameObject Instantiate(Vector3 position);
        public abstract GameObject Instantiate(Vector3 position, Vector3 rotation);
        // TODO: Instantiate from mapdata and savedata

        public void OnEditorBuilderSelect()
        {
            Debug.Log($"Setting {Identifier} as current buildable");
            Object.FindObjectOfType<MapBuilder>().SelectedDataModel = this;
        }
    }
}