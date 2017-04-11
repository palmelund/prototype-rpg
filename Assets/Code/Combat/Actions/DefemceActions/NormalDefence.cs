using Assets.Code.Combat.Interfaces;

namespace Assets.Code.Combat.Actions.DefemceActions
{
    public class NormalDefence : ICombatAction, IDefenceAction
    {
        public string Name { get; private set; }
        public CombatPosition Position { get; private set; }

        public NormalDefence(CombatPosition position)
        {
            Position = position;
        }

        public void ExecuteAction()
        {
            // Does nothing, as it is a response to attack only.
        }
    }
}
