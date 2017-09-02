using World;

namespace Models.MapModels
{
    public class WallMapModel : IMapModel
    {
        public string Identifier { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public WallMapModel(WallComponent wallComponent)
        {
            Identifier = wallComponent.Identifier;
            X = wallComponent.transform.position.x;
            Y = wallComponent.transform.position.y;
        }

        public WallMapModel()
        {
            
        }
    }
}
