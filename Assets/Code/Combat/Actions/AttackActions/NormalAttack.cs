using Assets.Code.Characters;
using Assets.Code.Combat.Interfaces;

namespace Assets.Code.Combat.Actions.AttackActions
{
    public class NormalAttack : ICombatAction, IAttackAction
    {
        public string Name { get; private set; }

        public Character Attacker { get; protected set; }
        public Character Defender { get; protected set; }
        public CombatPosition Position { get; protected set; }

        public NormalAttack(Character attacker, Character defender, CombatPosition position)
        {
            Name = "Normal Attack";
            Attacker = attacker;
            Defender = defender;
            Position = position;
        }

        public void ExecuteAction()
        {
            IDefenceAction defenderAction = CombatController.Instance.Actions[Defender] as IDefenceAction;
            if (defenderAction != null)
            {
                if (Position == defenderAction.Position)
                {
                    // Defender defended, so do nothing
                }
                else
                {
                    // Damage defender
                }
            }
        }
    }
}
