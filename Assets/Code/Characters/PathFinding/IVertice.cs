using UnityEngine;

namespace Code.Characters.PathFinding
{
    public interface IVertice
    {
        Vector3 Position { get; }
        IVertice[] Neighbors { get; }
    }
}