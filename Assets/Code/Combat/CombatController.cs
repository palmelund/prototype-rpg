using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Code.Characters;
using Code.Characters.Npc;
using Code.Characters.Player;
using Code.Combat.Actions.GeneralActions;
using Code.Combat.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Combat
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

        public Dictionary<Character, GameObject> CombatParticipantCards = new Dictionary< Character, GameObject>();        // Dictionary with character as key instead?

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

            //CombatParticipantsContent.sizeDelta = new Vector2(CombatParticipantsContent.sizeDelta.x, 30 * CombatParticipants.Count + 5);

            //int y = 115;    // Ensure elements are added to the top.

            //foreach (Character character in CombatParticipants)
            //{
            //    var combatParticipant = Instantiate(Resources.Load<GameObject>("Prefabs/CombatParticipant"));
            //    combatParticipant.transform.SetParent(CombatParticipantsContent, false);
            //    combatParticipant.transform.position = CombatParticipantsContent.transform.position;
            //    var test = combatParticipant.GetComponentsInChildren<Text>();

            //    var textName = test[0];
            //    var textInitiative = test[1];
            //    var textStatus = test[2];

            //    textName.text = character.Name;
            //    textInitiative.text = character.Initiative.ToString();
            //    textStatus.text = "R";
            //    combatParticipant.transform.position = new Vector3(combatParticipant.transform.position.x, y);
            //    y -= 30;

            //    CombatParticipantCards.Add(combatParticipant);
            //    //CombatParticipantsContent.sizeDelta = new Vector2(CombatParticipantsContent.sizeDelta.x, CombatParticipantsContent.sizeDelta.y + 35);
            //}

            //var v = CombatParticipantCards[_currentTurn].GetComponentInChildren<Image>();
            //if (v.name.Equals("CardPanel"))
            //{
            //    v.color = Color.green;
            //}

            //CommitActionButton.onClick.AddListener(() =>
            //{
            //    var w = CombatParticipantCards[_currentTurn].GetComponentInChildren<Image>();
            //    if (w.name.Equals("CardPanel"))
            //    {
            //        w.color = _defaultColor;
            //    }
            //    _currentTurn++;
            //    w = CombatParticipantCards[_currentTurn].GetComponentInChildren<Image>();
            //    if (w.name.Equals("CardPanel"))
            //    {
            //        w.color = Color.green;
            //    }
            //});
        }

        public void StartCombat()
        {
            GameState.GameActionState = GameActionState.Combat;
            PlayerController.ControllerInstance.Path.Clear();
            foreach (var npc in NpcController.NpcList)
            {
                npc.Path.Clear();
            }

            CombatParticipants.Clear();

            foreach (var card in CombatParticipantCards)
            {
                Destroy(card.Value);
            }

            CombatParticipantCards.Clear();
            
            CombatParticipants.Add(PlayerController.PlayerInstance);

            foreach (var npc in NpcController.NpcList)
            {
                CombatParticipants.Add(npc);
            }

            CombatParticipants = CombatParticipants.OrderByDescending(participant => participant.Initiative).ToList();

            CombatParticipantsContent.sizeDelta = new Vector2(CombatParticipantsContent.sizeDelta.x, 50 * CombatParticipants.Count + 5);
            var y = 405;    // Ensure elements are added to the top.
            foreach (var character in CombatParticipants)
            {
                var combatParticipant = Instantiate(Resources.Load<GameObject>("Prefabs/CombatThinkingParticipant"));
                combatParticipant.transform.SetParent(CombatParticipantsContent, false);
                //combatParticipant.transform.position = CombatParticipantsContent.transform.position;
                var test = combatParticipant.GetComponentsInChildren<Text>();

                var textName = test[0];
                var textInitiative = test[1];
                var textStatus = test[2];

                textName.text = character.Name;
                textInitiative.text = character.Initiative.ToString();
                textStatus.text = "Thinking...";
                combatParticipant.transform.position = new Vector3(combatParticipant.transform.position.x, y);
                y -= 50;

                CombatParticipantCards.Add(character, combatParticipant);
            }
        }

        public void EndCombat()
        {
            GameState.GameActionState = GameActionState.Normal;
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

        public void ActionPlayerFocus(Character committingCharacter, Character focusedCharacter)
        {
            FocusOnCharacter focus = new FocusOnCharacter("Focus of Character", focusedCharacter);
            Actions.Add(committingCharacter, focus);

            CombatParticipantCards[committingCharacter].GetComponentsInChildren<Text>()[2].text =
                "Focus on " + focusedCharacter.Name;
        }

        public void ActionNpcFocus()
        {
            
        }
    }
}
