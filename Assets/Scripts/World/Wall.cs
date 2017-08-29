using UnityEngine;

namespace World
{
    public class Wall : MonoBehaviour, IWorldModel
    {
        public string Identifier { get; private set; }

        public void Configure(string identifier)
        {
            Identifier = identifier;
        }
    }
}