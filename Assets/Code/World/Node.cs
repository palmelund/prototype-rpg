using System.Collections.Generic;

namespace Assets.Code.World
{
    public class Node
    {
        public readonly int X;
        public readonly int Y;
        
        public readonly List<Node> StraightNeighbors = new List<Node>();
        public readonly List<Node> DiagonalNeighbors = new List<Node>();

        public Tile Tile;

        public Node(int x, int y, Tile tile)
        {
            X = x;
            Y = y;
            Tile = tile;
        }
    }
}
