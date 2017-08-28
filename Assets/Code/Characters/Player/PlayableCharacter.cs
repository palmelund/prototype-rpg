namespace Code.Characters.Player
{
    public class PlayableCharacter : Character
    {
        public PlayableCharacter()
        {
            Speed = 1;
            HitPointCurrent = 10;
            HitPointMax = 10;
            Name = "PlayableCharacter";
            Initiative = 10 + GameState.Rand.Next(0, 10);
        }
    }
}
