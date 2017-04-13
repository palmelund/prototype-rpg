using UnityEngine;
using Random = System.Random;

namespace Assets.Code
{
    public static class GameState
    {
        public static readonly Random Rand = new Random();

        public static GameActionState GameActionState = GameActionState.Normal;

        public static float Euclidean(Vector3 v1, Vector3 v2)
        {
            return Mathf.Sqrt(Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2));
        }
    }
}
