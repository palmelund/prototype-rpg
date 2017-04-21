using System;
using Code.Combat.Interfaces;

namespace Code.Combat.Actions.AttackActions
{
    class RangedAttack : ICombatAction
    {
        public string Name { get; private set; }
        public int InitiativeCost { get; private set; }
        public void ExecuteAction()
        {
            throw new NotImplementedException();
        }
    }
}
