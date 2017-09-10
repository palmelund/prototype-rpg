using Models.Components;

namespace Models.MapModels
{
    public class DoorMapModel : MapModel
    {
        public bool IsOpen { get; set; }
        public bool LoadOtherLevelOnUse { get; set; }
        public string MapReference { get; set; }
        public string SpawnReference { get; set; }
        
        public DoorMapModel()
        {
            
        }

        public DoorMapModel(DoorComponent doorComponent)
        {
            Identifier = doorComponent.Identifier;
            References = doorComponent.References;
            X = doorComponent.transform.position.x;
            Y = doorComponent.transform.position.y;
            IsOpen = doorComponent.IsOpen;
            LoadOtherLevelOnUse = doorComponent.LoadOtherLevelOnUse;
            MapReference = doorComponent.MapReference;
            SpawnReference = doorComponent.SpawnPointReference;
        }
    }
}
