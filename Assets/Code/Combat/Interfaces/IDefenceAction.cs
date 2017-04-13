using Assets.Code.Combat.Actions;

namespace Assets.Code.Combat.Interfaces
{
    public interface IDefenceAction
    {
        CombatPosition Position { get; }

        MeleeType MeleeType { get; }
    }
}