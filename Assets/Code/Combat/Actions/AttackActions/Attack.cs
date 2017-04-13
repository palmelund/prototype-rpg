using System;
using Assets.Code.Characters;
using Assets.Code.Combat.Interfaces;

namespace Assets.Code.Combat.Actions.AttackActions
{
    public class Attack : ICombatAction, IAttackAction
    {
        public string Name { get; private set; }
        public int InitiativeCost { get; private set; }

        public Character Attacker { get; protected set; }
        public Character Defender { get; protected set; }
        public CombatPosition Position { get; protected set; }
        public MeleeType MeleeType { get; private set; }

        public Attack(Character attacker, Character defender, CombatPosition position, MeleeType meleeType, int initiativeCost)
        {
            Name = "Normal Attack";
            Attacker = attacker;
            Defender = defender;
            Position = position;
            MeleeType = meleeType;
            InitiativeCost = initiativeCost;
        }

        public void ExecuteAction()
        {
            var defenderAction = CombatController.Instance.Actions[Defender] as IDefenceAction;
            if (defenderAction != null)
            {
                switch (MeleeType)
                {
                    case MeleeType.NormalMelee:
                        if (Position == defenderAction.Position)
                        {
                            // Defender takes no damage
                        }
                        else
                        {
                            if (defenderAction.MeleeType == MeleeType.FocusedMelee)
                            {
                                // Defender takes reduced damage
                            }
                            else
                            {
                                // Defender takes damage       
                            }
                        }
                        break;
                    case MeleeType.FocusedMelee:
                        if (defenderAction.MeleeType == MeleeType.FocusedMelee && defenderAction.Position == Position)
                        {
                            // Successfully defended
                        }
                        else
                        {
                            // Defender takes damage
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                // Attack with full damage
            }
        }
    }
}
