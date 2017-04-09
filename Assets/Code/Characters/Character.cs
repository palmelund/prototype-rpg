using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Characters
{
    public class Character
    {
        public int HitPointCurrent { get; protected set; }
        public int HitPointMax { get; protected set; }
        public float Speed { get; protected set; }

        private static Character _this;
        public static Character Instance { get { return _this; } }

        protected Character()
        {
            _this = this;
        }
    }
}
