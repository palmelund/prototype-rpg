namespace Assets.Code.Combat.Interfaces
{
    public interface ICombatAction
    {
        string Name { get; }

        void ExecuteAction();
    }
}