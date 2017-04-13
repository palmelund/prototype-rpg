using System;
using Assets.Code.Combat.Interfaces;

namespace Assets.Code.Combat.Actions.AttackActions
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
