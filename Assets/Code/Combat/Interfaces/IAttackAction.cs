using Code.Characters;
using Code.Combat.Actions;

namespace Code.Combat.Interfaces
{
    public interface IAttackAction
    {
        Character Attacker { get; }
        Character Defender { get; }
        CombatPosition Position { get; }

        MeleeType MeleeType { get; }
    }
}