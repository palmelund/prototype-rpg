using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.PathFinding
{
    public class PathFinderNode
    {
        public readonly int X;
        public readonly int Y;

        public float Distance;

        public List<PathFinderNode> StraightNeighbors = new List<PathFinderNode>();
        public List<PathFinderNode> DiagonalNeighbors = new List<PathFinderNode>();

        // TODO: Better way?
        public List<PathFinderNode> Neighbors
        {
            get { return new List<PathFinderNode>().Concat(StraightNeighbors).Concat(DiagonalNeighbors).ToList(); }
        }

        public PathFinderNode(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
