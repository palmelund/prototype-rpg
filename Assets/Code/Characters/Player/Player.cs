using System;
using System.Collections.Generic;
using Assets.Code.Characters.PathFinding;
using Assets.Code.World;

namespace Assets.Code.Characters.Player
{
    public class Player : Character
    {
        public Player()
        {
            Speed = 1;
            HitPointCurrent = 10;
            HitPointMax = 10;
        }
    }
}
