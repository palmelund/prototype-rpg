using System.Collections.Generic;
using Models.Components;
using UnityEngine;

namespace Characters.PathFinding
{
    public class Vertice : IVertice
    {
        public FloorComponent FloorComponent { get; }

        public Vector3 Position { get; }
        public IVertice[] Neighbors => NeighborList.ToArray();

        public List<Vertice> NeighborList = new List<Vertice>();

        public Vertice(FloorComponent floorComponent)
        {
            Position = floorComponent.transform.position;
            FloorComponent = floorComponent;
        }
    }
}
