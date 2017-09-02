using UnityEngine;

namespace World
{
    public class WallComponent : MonoBehaviour, IWorldComponent, IWallBehavior
    {
        public string Identifier { get; private set; }

        public void Configure(string identifier)
        {
            Identifier = identifier;
        }
    }
}