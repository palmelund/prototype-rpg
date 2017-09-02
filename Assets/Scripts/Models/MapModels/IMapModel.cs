namespace Models.MapModels
{
    public interface IMapModel
    {
        string Identifier { get; set; }
        float X { get; set; }
        float Y { get; set; }
    }
}