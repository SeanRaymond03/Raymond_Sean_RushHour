using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Raymond_Sean_RushHour.Services
{
    // Game logic and state management
    public class GameService : INotifyPropertyChanged
    {
        private int _score;
        private int _distance;
        private int _coinsCollected;
        private int _lives;
        private double _elapsedTime;
        private bool _isGameActive;

        public int Score
        {
            get => _score;
            set { _score = value; OnPropertyChanged(); }
        }

        public int Distance
        {
            get => _distance;
            set { _distance = value; OnPropertyChanged(); }
        }

        public int CoinsCollected
        {
            get => _coinsCollected;
            set { _coinsCollected = value; OnPropertyChanged(); }
        }

        public int Lives
        {
            get => _lives;
            set { _lives = value; OnPropertyChanged(); }
        }

        public double ElapsedTime
        {
            get => _elapsedTime;
            set { _elapsedTime = value; OnPropertyChanged(); }
        }

        public bool IsGameActive
        {
            get => _isGameActive;
            set { _isGameActive = value; OnPropertyChanged(); }
        }

        // Get difficulty multiplier
        public double GetDifficultyMultiplier(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 0.7,
                "Hard" => 1.5,
                _ => 1.0 // Medium
            };
        }

        // Increase score with difficulty factor
        public void AddScore(int points, string difficulty)
        {
            Score += (int)(points * GetDifficultyMultiplier(difficulty));
        }

        // Reset game state
        public void ResetGame()
        {
            Score = 0;
            Distance = 0;
            CoinsCollected = 0;
            Lives = 3;
            ElapsedTime = 0;
            IsGameActive = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
