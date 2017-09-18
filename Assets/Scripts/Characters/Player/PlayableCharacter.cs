namespace Characters.Player
{
    public class PlayableCharacter : Character
    {
        public PlayerInventory Inventory { get; set; }

        public PlayableCharacter()
        {
            Speed = 1;
            Name = "PlayableCharacter";
        }
    }
}
