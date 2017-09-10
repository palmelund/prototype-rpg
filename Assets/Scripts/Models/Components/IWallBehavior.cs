namespace Models.Components
{
    /// <summary>
    /// This interface adds no functionality to a class, but makes the system see it as a wall when generating a graph for pathfinding.
    /// It should be used for walls as well as other blocking components, such as doors.
    /// </summary>
    public interface IWallBehavior
    {
        
    }
}