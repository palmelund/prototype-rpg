using System.Collections.Generic;
using Global;
using UnityEngine;

namespace Models.MapModels
{
    public abstract class MapModel : IGameSerializable
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Vector3 Position => new Vector3(X, Y);

        public List<string> References { get; set; } = new List<string>();
    }
}