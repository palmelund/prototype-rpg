using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace Global
{
    public static class Serializer
    {
        private static Type[] _gameSerializableObjectList;

        public static void BuildGameSerializableObjectList()
        {
            _gameSerializableObjectList = TypesImplementingInterface<IGameSerializable>();
        }

        private static Type[] GameSerializableObjects
        {
            get
            {
                if (_gameSerializableObjectList == null)
                {
                    BuildGameSerializableObjectList();
                }
                return _gameSerializableObjectList;
            }
        }

        public static void SerializeToFile<T>(T obj, string fileName) where T : IGameSerializable
        {
            var serializer = new XmlSerializer(typeof(T), GameSerializableObjects);
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public static T DeserializeFromFile<T>(string fileName) where T : IGameSerializable
        {
            var serializer = new XmlSerializer(typeof(T), GameSerializableObjects);
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        private static Type[] TypesImplementingInterface<T>()
        {
            var @interface = typeof(T);
            return @interface.IsInterface
                ? AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type =>
                    !type.IsInterface && !type.IsAbstract && type.GetInterfaces().Contains(@interface)).ToArray()
                : new Type[] { };
        }
    }
}
