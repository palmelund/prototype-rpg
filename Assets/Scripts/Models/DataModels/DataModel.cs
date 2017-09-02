using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Models.DataModels
{
    public abstract class DataModel
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        
        public abstract GameObject InstantiateGame(Vector3 position);
        public abstract GameObject InstantiateGame(Vector3 position, Vector3 rotation);

        public abstract GameObject InstantiateEditor(Vector3 position);
        public abstract GameObject InstantiateEditor(Vector3 position, Vector3 rotation);
        
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