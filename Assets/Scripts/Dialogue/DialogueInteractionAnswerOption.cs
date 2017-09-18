namespace Dialogue
{
    public class DialogueInteractionAnswerOption : IDialogueInteraction
    {
        public string Identifier { get; set; }
        public string SpeakerIdentifier { get; set; }
        public string NextDialogueInteractionIdentifier { get; set; }
        public string Text { get; set; }
    }
}