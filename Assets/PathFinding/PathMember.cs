namespace Assets.PathFinding
{
    public sealed class PathMember
    {
        public readonly Tile Destination;
        public readonly PathFinderDirection Direction;

        public PathMember(Tile destinationTile, PathFinderDirection direction)
        {
            Destination = destinationTile;
            Direction = direction;
        }
    }
}
