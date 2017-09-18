namespace Dialogue
{
    /*
     * TODO
     * In the future, certain dialogue actions should depend on quests and world state, as well as the current position of the player and party.
     */

    public interface IDialogueInteraction
    {
        string Identifier { get; set; }
        string SpeakerIdentifier { get; set; }
        string NextDialogueInteractionIdentifier { get; set; }
    }
}