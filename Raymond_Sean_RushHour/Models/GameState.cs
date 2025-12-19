namespace Raymond_Sean_RushHour.Models
{
    // Game state tracking
    public class GameState
    {
        public int Score { get; set; }
        public int Distance { get; set; }
        public int CoinsCollected { get; set; }
        public int HighScore { get; set; }
        public bool IsGameOver { get; set; }
        public int Lives { get; set; }
        public double ElapsedTime { get; set; }

        public GameState()
        {
            Score = 0;
            Distance = 0;
            CoinsCollected = 0;
            IsGameOver = false;
            Lives = 3;
            ElapsedTime = 0;
        }
    }
}
