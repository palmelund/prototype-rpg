using System.IO;
using System.Xml.Serialization;
using GameEditor.MapEditor;
using UnityEngine;

namespace Models.DataModels
{
    public abstract class DataModel
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

        // Used for creating new DataModels only!
        public void SerializeToFile(string file)
        {
            var serializer = new XmlSerializer(GetType());
            using (var stream = new FileStream(file, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        public static T DeserializeFromFile<T>(string file)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(file, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}