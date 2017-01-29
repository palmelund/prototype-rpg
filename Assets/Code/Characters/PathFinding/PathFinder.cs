using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.Characters.PathFinding
{
    public static class PathFinder
    {
        private const float Sqrt2 = 1.4142135f;

        public static Stack<PathMember> AStar(Node start, Node target)
        {
            var closedSet = new List<Node>();
            var openSet = new List<Node> { start };
            var cameFrom = new Dictionary<Node, Node>();
            var gScore = new Dictionary<Node, float>();
            gScore[start] = 0;

            var fScore = new Dictionary<Node, float>();
            fScore[start] = HeuristicCostEstimate(start, target);

            while (openSet.Count > 0)
            {
                var current = openSet.Aggregate((a, b) => fScore[a] < fScore[b] ? a : b);

                if (current.Equals(target))
                {
                    return ReconstructPath(cameFrom, current, start);
                }

                openSet.Remove(current);
                closedSet.Add(current);
                
                foreach (var neighbor in current.StraightNeighbors)
                {
                    if (closedSet.Contains(neighbor)) continue;
                    var tentativeGScore = gScore[current] + 1;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGScore >= gScore[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, target);
                }

                foreach (var neighbor in current.DiagonalNeighbors)
                {
                    if (closedSet.Contains(neighbor)) continue;
                    var tentativeGScore = gScore[current] + Sqrt2;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGScore >= gScore[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, target);
                }
            }
            return new Stack<PathMember>();
        }

        private static Stack<PathMember> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current, Node start)
        {
            var totalPath = new Queue<Node>();
            totalPath.Enqueue(current);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Enqueue(current);
            }

            var path = new Stack<PathMember>();
            
            var next = totalPath.Dequeue();

            if (totalPath.Count == 0)
            {
                return path;
            }

            var prev = totalPath.Dequeue();
            var dir = GetPathFinderDirection(prev, next);

            path.Push(new PathMember(Map.Instance.GetTileAt(next.X, next.Y), dir));
            while (!prev.Equals(start))
            {
                next = prev;
                prev = totalPath.Dequeue();
                dir = GetPathFinderDirection(prev, next);
                path.Push(new PathMember(Map.Instance.GetTileAt(next.X, next.Y), dir));
            }

            return path;
        }

        private static float HeuristicCostEstimate(Node start, Node target)
        {
            var dx = Mathf.Abs(start.X - target.X);
            var dy = Mathf.Abs(start.Y - target.Y);
            return (dx + dy) + (Sqrt2 - 2) * Mathf.Min(dx, dy);
        }

        private static PathFinderDirection GetPathFinderDirection(Node from, Node to)
        {
            if (from.Equals(to)) throw new Exception("How?");

            var rx = to.X - from.X;
            var ry = to.Y - from.Y;

            if (rx == 1 && ry == 0) return PathFinderDirection.Right;
            if (rx == -1 && ry == 0) return PathFinderDirection.Left;

            if (rx == 0 && ry == 1) return PathFinderDirection.Up;
            if (rx == 0 && ry == -1) return PathFinderDirection.Down;

            if (rx == 1 && ry == 1) return PathFinderDirection.UpRight;
            if (rx == 1 && ry == -1) return PathFinderDirection.DownRight;

            if (rx == -1 && ry == 1) return PathFinderDirection.UpLeft;
            if (rx == -1 && ry == -1) return PathFinderDirection.DownLeft;

            throw new Exception("The PathFinding Algorithm skipped a tile...");
        }
    }
}
