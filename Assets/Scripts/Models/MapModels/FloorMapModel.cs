using Models.Components;
using UnityEngine;

namespace Models.MapModels
{
    public class FloorMapModel : MapModel
    {
        public bool CanEnter { get; set; }

        public FloorMapModel()
        {
            
        }

        public FloorMapModel(FloorComponent floorComponent)
        {
            Identifier = floorComponent.Identifier;
            References = floorComponent.References;
            X = floorComponent.X;
            Y = floorComponent.Y;
            CanEnter = floorComponent.CanEnter;
        }
    }
}
