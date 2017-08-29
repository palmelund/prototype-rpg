using System.Collections.Generic;
using UnityEngine;
using World;

namespace Characters.PathFinding
{
    public class Vertice : IVertice
    {
        public Tile Tile { get; }

        public Vector3 Position { get; }
        public IVertice[] Neighbors => NeighborList.ToArray();

        public List<Vertice> NeighborList = new List<Vertice>();

        public Vertice(Vector3 position, Tile tile)
        {
            Position = position;
            Tile = tile;
        }
    }
}
