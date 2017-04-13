using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Code.Characters;
using Assets.Code.Characters.Player;
using Assets.Code.Combat.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Combat
{
    public class CombatController : MonoBehaviour
    {
        public static CombatController Instance { get; private set; }

        // Action chosen by all in the current turn.
        public Dictionary<Character, ICombatAction> Actions = new Dictionary<Character, ICombatAction>();
        public List<Character> CombatParticipants = new List<Character>();

        public RectTransform CombatInitiativeRectTransform; // Set in editor
        public RectTransform CombatParticipantsContent; // Set in editor
        public Button CommitActionButton;   // Set in editor

        public List<GameObject> CombatParticipantCards = new List<GameObject>();

        public void Start()
        {
            Instance = this;
            
            CombatParticipants.Add(new Player());
            CombatParticipants.Add(new Player());
            CombatParticipants.Add(new Player());
            CombatParticipants.Add(new Player());
            CreateInitiativeContent();
            
        }

        private readonly Color _defaultColor = new Color(1.000f, 1.000f, 1.000f, 0.392f);

        public void Update()
        {
            
        }

        private int _currentTurn = 0;

        public void CreateInitiativeContent()
        {
            foreach (Transform child in CombatParticipantsContent.transform)
            {
                Destroy(child);
            }

            CombatParticipantsContent.sizeDelta = new Vector2(CombatParticipantsContent.sizeDelta.x, 30 * CombatParticipants.Count + 5);

            int y = 115;    // Ensure elements are added to the top.

            foreach (Character character in CombatParticipants)
            {
                var combatParticipant = Instantiate(Resources.Load<GameObject>("Prefabs/CombatParticipant"));
                combatParticipant.transform.SetParent(CombatParticipantsContent, false);
                combatParticipant.transform.position = CombatParticipantsContent.transform.position;
                var test = combatParticipant.GetComponentsInChildren<Text>();

                var textName = test[0];
                var textInitiative = test[1];
                var textStatus = test[2];

                textName.text = character.Name;
                textInitiative.text = character.Initiative.ToString();
                textStatus.text = "R";
                combatParticipant.transform.position = new Vector3(combatParticipant.transform.position.x, y);
                y -= 30;

                CombatParticipantCards.Add(combatParticipant);
                //CombatParticipantsContent.sizeDelta = new Vector2(CombatParticipantsContent.sizeDelta.x, CombatParticipantsContent.sizeDelta.y + 35);
            }

            var v = CombatParticipantCards[_currentTurn].GetComponentInChildren<Image>();
            if (v.name.Equals("CardPanel"))
            {
                v.color = Color.green;
            }

            CommitActionButton.onClick.AddListener(() =>
            {
                var w = CombatParticipantCards[_currentTurn].GetComponentInChildren<Image>();
                if (w.name.Equals("CardPanel"))
                {
                    w.color = _defaultColor;
                }
                _currentTurn++;
                w = CombatParticipantCards[_currentTurn].GetComponentInChildren<Image>();
                if (w.name.Equals("CardPanel"))
                {
                    w.color = Color.green;
                }
            });
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
