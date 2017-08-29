using UnityEngine;

namespace Characters.PathFinding
{
    public interface IVertice
    {
        Vector3 Position { get; }
        IVertice[] Neighbors { get; }
    }
}