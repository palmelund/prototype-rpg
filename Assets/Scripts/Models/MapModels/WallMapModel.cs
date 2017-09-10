using Models.Components;

namespace Models.MapModels
{
    public class WallMapModel : MapModel
    {
        public WallMapModel(WallComponent wallComponent)
        {
            Identifier = wallComponent.Identifier;
            References = wallComponent.References;
            X = wallComponent.transform.position.x;
            Y = wallComponent.transform.position.y;
        }

        public WallMapModel()
        {
            
        }
    }
}
