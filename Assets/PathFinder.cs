using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public enum PathFinderDirection
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
        Stay
    }

    public sealed class PathMember
    {
        public readonly tile Destination;
        public readonly PathFinderDirection Direction;

        public PathMember(tile destinationTile, PathFinderDirection direction)
        {
            Destination = destinationTile;
            Direction = direction;
        }
    }

    public static class PathFinder
    {
        private static float Diagonal = 1.414213562373095f;

        public static Stack<PathMember> AllCase(tile startTile, tile endTile)
        {
            if (startTile.Equals(endTile)) return new Stack<PathMember>();
            ResetAllCase();
            VisitAllCase(startTile, 0);
            if (endTile.Distance == float.MaxValue) return new Stack<PathMember>();
            return BuildPath(startTile, endTile);
        }

        private static Stack<PathMember> BuildPath(tile startTile, tile endTile)
        {
            Stack<PathMember> path = new Stack<PathMember>();
            tile next = map.Instance.GetNeighbors(endTile).Where(t => t != null).Aggregate((a, b) => a.Distance < b.Distance ? a : b);
            var dir = map.Instance.GetPathFinderDirection(next, endTile);
            path.Push(new PathMember(endTile, dir));

            while (!next.Equals(startTile))
            {
                //path.Enqueue(next);
                var prev = next;
                next = map.Instance.GetNeighbors(prev).Where(t => t != null).Aggregate((a, b) => a.Distance < b.Distance ? a : b);
                dir = map.Instance.GetPathFinderDirection(next, prev);
                path.Push(new PathMember(prev, dir));
            }
            return path;
        }

        private static void ResetAllCase()
        {
            foreach (tile tile in map.Instance.Map)
            {
                tile.Distance = float.MaxValue;
            }
        }

        private static void VisitAllCase(tile t, float distance)
        {
            t.Distance = distance;
            List<tile> neighbors = map.Instance.GetNeighbors(t);
            for (int i = 0; i < 8; i++)
            {
                if (neighbors[i] == null) continue;
                if (neighbors[i].CanEnter == false) continue;

                if (i % 2 == 0) // diagonal
                {
                    if (neighbors[i].Distance > distance + Diagonal)
                    {
                        VisitAllCase(neighbors[i], distance + Diagonal);
                    }
                }
                else
                {
                    if (neighbors[i].Distance > distance + 1)
                    {
                        VisitAllCase(neighbors[i], distance + 1);
                    }
                }
            }
        }
    }
}
