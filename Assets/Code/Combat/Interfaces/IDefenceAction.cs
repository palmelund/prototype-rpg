using Code.Combat.Actions;

namespace Code.Combat.Interfaces
{
    public interface IDefenceAction
    {
        CombatPosition Position { get; }

        MeleeType MeleeType { get; }
    }
}