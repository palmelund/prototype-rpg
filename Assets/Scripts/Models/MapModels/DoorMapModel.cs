using UnityEngine;
using World;

namespace Models.MapModels
{
    public class DoorMapModel : IMapModel
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public bool IsOpen { get; set; }
        
        public DoorMapModel()
        {
            
        }

        public DoorMapModel(DoorComponent doorComponent)
        {
            Identifier = doorComponent.Identifier;
            X = doorComponent.transform.position.x;
            Y = doorComponent.transform.position.y;
            IsOpen = doorComponent.IsOpen;
        }
    }
}
