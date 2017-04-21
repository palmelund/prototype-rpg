using Code.Characters;
using Code.Combat.Interfaces;

namespace Code.Combat.Actions.GeneralActions
{
    public class FocusOnCharacter : ICombatAction
    {
        public string Name { get; private set; }
        public int InitiativeCost { get; private set; }

        public Character FocusedCharacter;

        public FocusOnCharacter(string name, Character character)
        {
            Name = name;
            InitiativeCost = 0;
            FocusedCharacter = character;
        }

        public void ExecuteAction()
        {
            // TODO: Implement this
        }
    }
}
