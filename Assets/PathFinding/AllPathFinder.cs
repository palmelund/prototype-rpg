using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.PathFinding
{
    public class AllPathFinder
    {
        private const float Sqrt2 = 1.4142135f;
        
        private readonly PathFinderNode[,] _graph;

        public AllPathFinder(Map m)
        {
            _graph = new PathFinderNode[m.Width,m.Height];
            GenerateGraph(m);
        }

        public Stack<PathMember> PathFinder(Tile startTile, Tile targetTile)
        {
            var path = new Stack<PathMember>();

            if (startTile == null || targetTile == null) return path;

            var startX = startTile.XCoord;
            var startY = startTile.YCoord;
            var targetX = targetTile.XCoord;
            var targetY = targetTile.YCoord;

            if (startX < 0 || startY < 0 || targetX > _graph.GetLength(0) - 1 || targetY > _graph.GetLength(1) - 1) return path;
            if (startX == targetX && startY == targetY) return path;
            if (_graph[startX, startY] == null || _graph[targetX, targetY] == null) return path;

            CalculateDistance(startTile);
            if (float.IsPositiveInfinity(_graph[targetX, targetY].Distance)) return path;

            var endNode = _graph[targetX, targetY];
            var startNode = _graph[startX, startY];
            var next = endNode.Neighbors.Aggregate((a, b) => a.Distance < b.Distance ? a : b);
            var dir = GetPathFinderDirection(next, endNode);
            path.Push(new PathMember(Map.Instance.GetTileAt(endNode.X, endNode.Y), dir));

            while (!next.Equals(startNode))
            {
                var prev = next;
                next = next.Neighbors.Aggregate((a, b) => a.Distance < b.Distance ? a : b);
                dir = GetPathFinderDirection(next, prev);
                path.Push(new PathMember(Map.Instance.GetTileAt(prev.X, prev.Y), dir));
            }
            return path;
        }

        private void GenerateGraph(Map m)
        {
            for (var y = 0; y < m.TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < m.TileMap.GetLength(0); x++)
                {
                    if (m.TileMap[x, y] == null || m.TileMap[x, y].CanEnter == false) continue;
                    _graph[x,y] = new PathFinderNode(x,y);
                }
            }

            for (var y = 0; y < m.TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < m.TileMap.GetLength(0); x++)
                {
                    if (m.TileMap[x,y] == null || m.TileMap[x,y].CanEnter == false) continue;

                    // Straights

                    if (x > 0)
                    {
                        _graph[x, y].StraightNeighbors.Add(_graph[x - 1, y]);
                    }

                    if (y > 0)
                    {
                        _graph[x, y].StraightNeighbors.Add(_graph[x, y - 1]);
                    }

                    if (x < m.TileMap.GetLength(0) - 1)
                    {
                        _graph[x, y].StraightNeighbors.Add(_graph[x + 1, y]);
                    }

                    if (y < m.TileMap.GetLength(1) - 1)
                    {
                        _graph[x, y].StraightNeighbors.Add(_graph[x, y + 1]);
                    }

                    // Diagonals

                    if (x > 0 && y > 0 && _graph[x - 1, y] != null && _graph[x, y - 1] != null)
                    {
                        _graph[x, y].DiagonalNeighbors.Add(_graph[x - 1, y - 1]);
                    }

                    if (x > 0 && y < m.TileMap.GetLength(1) - 1 && _graph[x - 1, y] != null &&
                        _graph[x, y + 1] != null)
                    {
                        _graph[x, y].DiagonalNeighbors.Add(_graph[x - 1, y + 1]);
                    }

                    if (x < m.TileMap.GetLength(0) - 1 && y < m.TileMap.GetLength(1) - 1 &&
                        _graph[x + 1, y] != null && _graph[x, y + 1] != null)
                    {
                        _graph[x, y].DiagonalNeighbors.Add(_graph[x + 1, y + 1]);
                    }

                    if (x < m.TileMap.GetLength(0) - 1 && y > 0 && _graph[x + 1, y] != null &&
                        _graph[x, y - 1] != null)
                    {
                        _graph[x, y].DiagonalNeighbors.Add(_graph[x + 1, y - 1]);
                    }
                }
            }

            foreach (var node in _graph)
            {
                if (node == null) continue;
                node.StraightNeighbors.RemoveAll(n => n == null);
                node.DiagonalNeighbors.RemoveAll(n => n == null);
            }
        }

        private void ResetDistance()
        {
            foreach (var node in _graph)
            {
                if (node == null) continue;
                node.Distance = float.PositiveInfinity;
            }
        }

        private void CalculateDistance(Tile start)
        {
            ResetDistance();
            VisitNeighbors(_graph[start.XCoord, start.YCoord], 0);
        }

        private void VisitNeighbors(PathFinderNode node, float distance)
        {
            node.Distance = distance;
            foreach (var neighbor in node.StraightNeighbors)
            {
                if (neighbor.Distance > distance + 1)
                {
                    VisitNeighbors(neighbor, distance + 1);
                }
            }

            foreach (var neighbor in node.DiagonalNeighbors)
            {
                if (neighbor.Distance > distance + Sqrt2)
                {
                    VisitNeighbors(neighbor, distance + Sqrt2);
                }
            }
        }

        private PathFinderDirection GetPathFinderDirection(PathFinderNode from, PathFinderNode to)
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
