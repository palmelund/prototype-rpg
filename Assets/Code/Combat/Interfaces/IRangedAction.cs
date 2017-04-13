using Assets.Code.Characters;

namespace Assets.Code.Combat.Interfaces
{
    public interface IRangedAction
    {
        Character Attacker { get; }
        Character Defender { get; }
        int Range { get; }
    }
}