namespace Dialogue
{
    public class DialogueInteractionText : IDialogueInteraction
    {
        public string Identifier { get; set; }
        public string Text { get; set; }
        public string SpeakerIdentifier { get; set; }   // TODO: Handle random characters, and picking a random character from a group!
        public string NextDialogueInteractionIdentifier { get; set; }
    }
}
