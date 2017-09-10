using System.Collections.Generic;

namespace Models.MapModels
{
    public abstract class MapModel
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public List<string> References { get; set; } = new List<string>();
    }
}