﻿using Assets.Code.Characters;
using Assets.Code.Combat.Actions;

namespace Assets.Code.Combat.Interfaces
{
    public interface IAttackAction
    {
        Character Attacker { get; }
        Character Defender { get; }
        CombatPosition Position { get; }

        MeleeType MeleeType { get; }
    }
}