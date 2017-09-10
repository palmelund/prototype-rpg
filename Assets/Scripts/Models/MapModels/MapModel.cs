using System.Collections.Generic;
using UnityEngine;

namespace Models.MapModels
{
    public abstract class MapModel
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Vector3 Position => new Vector3(X, Y);

        public List<string> References { get; set; } = new List<string>();
    }
}