using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using UnityEngine;

namespace Characters.PathFinding
{
    public static class PathFinder
    {
        public static LinkedList<T> AStar<T>(T fromVertice, T toVertice) where T : IVertice
        {
            if (fromVertice == null || toVertice == null)
            {
                Debug.LogError("fromVertice: " + (fromVertice == null ? "null" : "ref") + " toVertice: " +
                               (toVertice == null ? "null" : "ref"));
                return new LinkedList<T>();
            }

            var closedSet = new List<T>();
            var openSet = new SimplePriorityQueue<T, float>(); // new List<T>();
            openSet.Enqueue(fromVertice, 0f);
            var cameFrom = new Dictionary<T, T>();

            var gScore = new Dictionary<T, float> { [fromVertice] = 0f };

            var fScore = new Dictionary<T, float> { [fromVertice] = HeuristicCostEstimate(fromVertice, toVertice) };

            while (openSet.Count > 0)
            {
                var currentVertice = openSet.First();
                if (currentVertice.Equals(toVertice))
                {
                    return ReconstructPath(cameFrom, currentVertice);
                }

                openSet.Remove(currentVertice);
                closedSet.Add(currentVertice);

                foreach (T neighborVertice in currentVertice.Neighbors)
                {
                    if (closedSet.Contains(neighborVertice))
                    {
                        continue;
                    }

                    var tentativeGScore = gScore[currentVertice] + Vector3.Distance(currentVertice.Position, neighborVertice.Position);

                    if (openSet.Contains(neighborVertice) && tentativeGScore >= gScore[neighborVertice])
                    {
                        continue;
                    }

                    cameFrom[neighborVertice] = currentVertice;
                    gScore[neighborVertice] = tentativeGScore;
                    fScore[neighborVertice] = gScore[neighborVertice] +
                                              HeuristicCostEstimate(neighborVertice, toVertice);

                    if (openSet.Contains(neighborVertice))
                    {
                        openSet.Remove(neighborVertice);
                    }

                    openSet.Enqueue(neighborVertice, fScore[neighborVertice]);

                }
            }

            return new LinkedList<T>();
        }

        private static LinkedList<T> ReconstructPath<T>(Dictionary<T, T> cameFrom, T currentVertice)
            where T : IVertice
        {
            // TODO: If path is wrong, change whether it adds first or last
            var path = new LinkedList<T>();
            path.AddFirst(currentVertice);

            while (cameFrom.ContainsKey(currentVertice))
            {
                currentVertice = cameFrom[currentVertice];
                path.AddFirst(currentVertice);
            }

            return path;
        }

        private static float HeuristicCostEstimate<T>(T fromVertice, T toVertice) where T : IVertice
        {
            return Mathf.Sqrt(Vector3.Distance(fromVertice.Position, toVertice.Position));
        }
    }

    //public static class PathFinder
    //{
    //    private const float Sqrt2 = 1.4142135f;

    //    public static Stack<PathMember> AStar(Node start, Node target)
    //    {
    //        if (target == null)
    //        {
    //            Debug.Log("Fuck!");
    //        }

    //        var closedSet = new List<Node>();
    //        var openSet = new List<Node> {start};
    //        var cameFrom = new Dictionary<Node, Node>();
    //        var gScore = new Dictionary<Node, float>();
    //        gScore[start] = 0;

    //        var fScore = new Dictionary<Node, float>();
    //        fScore[start] = HeuristicCostEstimate(start, target);

    //        while (openSet.Count > 0)
    //        {
    //            var current = openSet.Aggregate((a, b) => fScore[a] < fScore[b] ? a : b);

    //            if (current.Equals(target))
    //            {
    //                return ReconstructPath(cameFrom, current, start);
    //            }

    //            openSet.Remove(current);
    //            closedSet.Add(current);

    //            foreach (var neighbor in current.StraightNeighbors)
    //            {
    //                if (closedSet.Contains(neighbor)) continue;
    //                var tentativeGScore = gScore[current] + 1;
    //                if (!openSet.Contains(neighbor))
    //                {
    //                    openSet.Add(neighbor);
    //                }
    //                else if (tentativeGScore >= gScore[neighbor])
    //                {
    //                    continue;
    //                }

    //                cameFrom[neighbor] = current;
    //                gScore[neighbor] = tentativeGScore;
    //                fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, target);
    //            }

    //            foreach (var neighbor in current.DiagonalNeighbors)
    //            {
    //                if (closedSet.Contains(neighbor)) continue;
    //                var tentativeGScore = gScore[current] + Sqrt2;
    //                if (!openSet.Contains(neighbor))
    //                {
    //                    openSet.Add(neighbor);
    //                }
    //                else if (tentativeGScore >= gScore[neighbor])
    //                {
    //                    continue;
    //                }

    //                cameFrom[neighbor] = current;
    //                gScore[neighbor] = tentativeGScore;
    //                fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, target);
    //            }
    //        }
    //        return new Stack<PathMember>();
    //    }

    //    private static Stack<PathMember> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current, Node start)
    //    {
    //        var totalPath = new Queue<Node>();
    //        totalPath.Enqueue(current);

    //        while (cameFrom.ContainsKey(current))
    //        {
    //            current = cameFrom[current];
    //            totalPath.Enqueue(current);
    //        }

    //        var path = new Stack<PathMember>();

    //        var next = totalPath.Dequeue();

    //        if (totalPath.Count == 0)
    //        {
    //            return path;
    //        }

    //        var prev = totalPath.Dequeue();
    //        var dir = GetPathFinderDirection(prev, next);

    //        path.Push(new PathMember(Map.Instance.GetTileAt(next.X, next.Y), dir));
    //        while (!prev.Equals(start))
    //        {
    //            next = prev;
    //            prev = totalPath.Dequeue();
    //            dir = GetPathFinderDirection(prev, next);
    //            path.Push(new PathMember(Map.Instance.GetTileAt(next.X, next.Y), dir));
    //        }

    //        return path;
    //    }

    //    private static float HeuristicCostEstimate(Node start, Node target)
    //    {
    //        try
    //        {
    //            var dx = Mathf.Abs(start.X - target.X);
    //            var dy = Mathf.Abs(start.Y - target.Y);
    //            return (dx + dy) + (Sqrt2 - 2) * Mathf.Min(dx, dy);
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.Log(e);
    //            return float.PositiveInfinity;
    //        }
    //    }

    //    private static PathFinderDirection GetPathFinderDirection(Node from, Node to)
    //    {
    //        if (from.Equals(to)) throw new Exception("How?");

    //        var rx = to.X - from.X;
    //        var ry = to.Y - from.Y;

    //        if (rx == 1 && ry == 0) return PathFinderDirection.Right;
    //        if (rx == -1 && ry == 0) return PathFinderDirection.Left;

    //        if (rx == 0 && ry == 1) return PathFinderDirection.Up;
    //        if (rx == 0 && ry == -1) return PathFinderDirection.Down;

    //        if (rx == 1 && ry == 1) return PathFinderDirection.UpRight;
    //        if (rx == 1 && ry == -1) return PathFinderDirection.DownRight;

    //        if (rx == -1 && ry == 1) return PathFinderDirection.UpLeft;
    //        if (rx == -1 && ry == -1) return PathFinderDirection.DownLeft;

    //        throw new Exception("The PathFinding Algorithm skipped a tile...");
    //    }
    //}
}