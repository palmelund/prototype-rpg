using World;

namespace Models.MapModels
{
    public class FloorMapModel : IMapModel
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool CanEnter { get; set; }

        public FloorMapModel()
        {
            
        }

        public FloorMapModel(FloorComponent floorComponent)
        {
            Identifier = floorComponent.Identifier;
            X = floorComponent.X;
            Y = floorComponent.Y;
            CanEnter = floorComponent.CanEnter;
        }
    }
}
