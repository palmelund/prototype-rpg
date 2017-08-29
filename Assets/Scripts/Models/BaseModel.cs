using UnityEngine;

namespace Models
{
    public abstract class BaseModel
    {
        public string Identifier { get; protected set; }
        public string SortingLayer { get; protected set; }
        public string SpriteName { get; protected set; }

        public abstract GameObject Instantiate(Vector3 position);
        public abstract GameObject Instantiate(Vector3 position, Vector3 rotation);
        
        protected abstract void LoadFromFile(string fileName);
    }
}