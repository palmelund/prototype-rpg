namespace Code.Combat.Interfaces
{
    public interface ICombatAction
    {
        string Name { get; }

        int InitiativeCost { get; }

        void ExecuteAction();
    }
}