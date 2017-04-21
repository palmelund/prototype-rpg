using Code.Characters;

namespace Code.Combat.Interfaces
{
    public interface IRangedAction
    {
        Character Attacker { get; }
        Character Defender { get; }
        int Range { get; }
    }
}