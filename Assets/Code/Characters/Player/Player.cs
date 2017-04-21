namespace Code.Characters.Player
{
    public class Player : Character
    {
        public Player()
        {
            Speed = 1;
            HitPointCurrent = 10;
            HitPointMax = 10;
            Name = "Player";
            Initiative = 10 + GameState.Rand.Next(0, 10);
        }
    }
}
