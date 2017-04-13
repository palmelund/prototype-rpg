using UnityEngine;

namespace Assets.Code.Characters
{
    public class Character
    {
        public string Name { get; protected set; }
        public int HitPointCurrent { get; protected set; }
        public int HitPointMax { get; protected set; }
        public float Speed { get; protected set; }

        public int CombatSpeed { get { return Mathf.FloorToInt(Speed); } }

        public int Initiative { get; set; }

        public static Character Instance { get; private set; }

        protected Character()
        {
            Instance = this;
            Speed = 1;
            HitPointMax = 10;
            HitPointCurrent = 10;
        }
    }
}
