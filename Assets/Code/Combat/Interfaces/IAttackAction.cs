using Assets.Code.Characters;

namespace Assets.Code.Combat.Interfaces
{
    public interface IAttackAction
    {
        Character Attacker { get; }
        Character Defender { get; }
        CombatPosition Position { get; }
    }
}