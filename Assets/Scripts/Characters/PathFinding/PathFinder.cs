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
}