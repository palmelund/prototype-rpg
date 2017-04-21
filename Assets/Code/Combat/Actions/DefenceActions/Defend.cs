using Code.Combat.Interfaces;

namespace Code.Combat.Actions.DefenceActions
{
    public class Defend : ICombatAction, IDefenceAction
    {
        public string Name { get; private set; }
        public int InitiativeCost { get; private set; }
        public CombatPosition Position { get; private set; }
        public MeleeType MeleeType { get; private set; }

        public Defend(CombatPosition position, MeleeType meleeType, int initiativeCost)
        {
            Position = position;
            MeleeType = meleeType;
            InitiativeCost = initiativeCost;
        }

        public void ExecuteAction()
        {
            // Does nothing, as it is a response to attack only.
        }
    }
}
