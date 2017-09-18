using System.Collections.Generic;

namespace Dialogue
{
    public class DialogueInteractionAnswers : IDialogueInteraction
    {
        public string Identifier { get; set; }
        public string SpeakerIdentifier { get; set; }
        public string NextDialogueInteractionIdentifier { get; set; }

        public List<string> AnswerIdentifiers = new List<string>();
    }
}