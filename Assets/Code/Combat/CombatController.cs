using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Code.Characters;
using Assets.Code.Combat.Interfaces;
using UnityEngine;

namespace Assets.Code.Combat
{
    public class CombatController : MonoBehaviour
    {
        public static CombatController Instance { get; private set; }

        // Action chosen by all in the current turn.
        public Dictionary<Character, ICombatAction> Actions = new Dictionary<Character, ICombatAction>();

        public void Start()
        {
            Instance = this;
        }

        public void Update()
        {
            
        }

        public TimeSpan GetCombatTimer { get { return _stopwatch.Elapsed; } }

        readonly Stopwatch _stopwatch = new Stopwatch();

        public void StartCombatTimer()
        {
            _stopwatch.Start();
        }

        public void StopCombatTimer()
        {
            _stopwatch.Stop();
        }

        public void ResetCombatTimer()
        {
            _stopwatch.Reset();
        }

        public void ResetAndRestartCombatTimer()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public int GetRemainingTime(TimeSpan timespan)
        {
            var remaining = (timespan - GetCombatTimer).Seconds;
            return remaining > 0 ? remaining : 0;
        }

        /*
         * ATTACK ACTIONS
         */

        /*
         * DEFENCE ACTIONS
         */

        /*
         * GENERAL ACTIONS
         */
    }
}
