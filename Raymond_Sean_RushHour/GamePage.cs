using Raymond_Sean_RushHour.Models;
using Raymond_Sean_RushHour.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Raymond_Sean_RushHour
{
    public class GameDrawable : IDrawable
    {
        public List<Obstacle> Obstacles { get; set; } = new();
        public List<Pickup> Pickups { get; set; } = new();
        public Player Player { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (canvas == null) return;

            // Background
            canvas.FillColor = Color.FromArgb("#87CEEB");
            canvas.FillRectangle(dirtyRect);

            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float laneWidth = width / 3;
            float playerY = height - 120;

            // Road markings
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 3;
            canvas.DrawLine(laneWidth, 0, laneWidth, height);
            canvas.DrawLine(laneWidth * 2, 0, laneWidth * 2, height);

            // Center dashes
            canvas.StrokeSize = 2;
            canvas.StrokeDashPattern = new float[] { 5, 5 };
            for (float y = 0; y < height; y += 20)
            {
                canvas.DrawLine(laneWidth, y, laneWidth, y + 10);
                canvas.DrawLine(laneWidth * 2, y, laneWidth * 2, y + 10);
            }
            canvas.StrokeDashPattern = null;

            // Draw obstacles
            foreach (var obstacle in Obstacles)
            {
                float obsX = obstacle.Lane * laneWidth + laneWidth / 2 - 30;
                canvas.FillColor = Color.FromArgb(obstacle.Color);
                canvas.FillRoundedRectangle(obsX, (float)obstacle.YPosition, 60, 80, 5);

                canvas.FillColor = Colors.LightBlue;
                canvas.FillRoundedRectangle(obsX + 8, (float)obstacle.YPosition + 10, 44, 20, 3);
            }

            // Draw pickups
            foreach (var pickup in Pickups)
            {
                float pickX = pickup.Lane * laneWidth + laneWidth / 2 - 15;
                canvas.FillColor = pickup.Type == "coin" ? Colors.Gold : Colors.LimeGreen;
                canvas.FillCircle(pickX, (float)pickup.YPosition, 15);
            }

            // Draw player
            float playerX = Player.Lane * laneWidth + laneWidth / 2 - 30;
            canvas.FillColor = Color.FromArgb(Player.VehicleColor);
            canvas.FillRoundedRectangle(playerX, playerY, 60, 80, 5);

            canvas.FillColor = Colors.LightBlue;
            canvas.FillRoundedRectangle(playerX + 8, playerY + 10, 44, 20, 3);

            // Headlights
            canvas.FillColor = Colors.Yellow;
            canvas.FillCircle(playerX + 15, playerY + 60, 8);
            canvas.FillCircle(playerX + 45, playerY + 60, 8);
        }
    }

    public partial class GamePage : ContentPage, INotifyPropertyChanged
    {
        private GameService _gameService;
        private AppSettings _appSettings;
        private GameDrawable _gameDrawable;
        private Random _random = new();
        private IDispatcherTimer _gameTimer;
        private Player _player;
        private double _gameSpeed = 5;
        private int _spawnCounter = 0;
        private bool _gameRunning = false;
        private GraphicsView _gameCanvas;
        private Label _scoreLabel;
        private Label _coinsLabel;
        private Label _livesLabel;
        private Button _leftBtn, _stopBtn, _rightBtn;

        private string _scoreText;
        private string _coinsText;
        private string _livesText;

        public string ScoreText
        {
            get => _scoreText;
            set { _scoreText = value; OnPropertyChanged(); }
        }

        public string CoinsText
        {
            get => _coinsText;
            set { _coinsText = value; OnPropertyChanged(); }
        }

        public string LivesText
        {
            get => _livesText;
            set { _livesText = value; OnPropertyChanged(); }
        }

        public GameDrawable GameDrawable
        {
            get => _gameDrawable;
            set { _gameDrawable = value; OnPropertyChanged(); }
        }

        public GamePage()
        {
            BuildUI();

            _gameService = new GameService();
            _appSettings = new AppSettings();
            _appSettings.Load();

            _player = new Player();
            _player.VehicleColor = _appSettings.BoxColor;
            _gameDrawable = new GameDrawable { Player = _player };

            InitializeGameTimer();
            UpdateUI();
        }

        private void BuildUI()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Padding = 0;
            grid.ColumnSpacing = 0;

            // Header
            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.Padding = 15;
            headerGrid.BackgroundColor = Color.FromArgb("#FF6B6B");

            _scoreLabel = new Label { TextColor = Colors.White, FontAttributes = FontAttributes.Bold, FontSize = 16 };
            _coinsLabel = new Label { TextColor = Colors.White, FontAttributes = FontAttributes.Bold, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center };
            _livesLabel = new Label { TextColor = Colors.White, FontAttributes = FontAttributes.Bold, FontSize = 16, HorizontalTextAlignment = TextAlignment.End };

            headerGrid.Add(_scoreLabel, 0, 0);
            headerGrid.Add(_coinsLabel, 1, 0);
            headerGrid.Add(_livesLabel, 2, 0);

            grid.Add(headerGrid, 0, 0);

            // Game Canvas
            _gameCanvas = new GraphicsView { Drawable = _gameDrawable, BackgroundColor = Color.FromArgb("#87CEEB") };
            grid.Add(_gameCanvas, 0, 1);

            // Controls
            var controlGrid = new Grid();
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            controlGrid.Padding = 10;
            controlGrid.BackgroundColor = Color.FromArgb("#333333");

            _leftBtn = new Button { Text = " <- Left", BackgroundColor = Color.FromArgb("#4ECDC4"), TextColor = Colors.White };
            _stopBtn = new Button { Text = "Start Game", BackgroundColor = Color.FromArgb("#FF6B6B"), TextColor = Colors.White };
            _rightBtn = new Button { Text = "Right -> ", BackgroundColor = Color.FromArgb("#4ECDC4"), TextColor = Colors.White };

            _leftBtn.Clicked += OnLeftClicked;
            _stopBtn.Clicked += OnStopClicked;
            _rightBtn.Clicked += OnRightClicked;

            controlGrid.Add(_leftBtn, 0, 0);
            controlGrid.Add(_stopBtn, 1, 0);
            controlGrid.Add(_rightBtn, 2, 0);

            grid.Add(controlGrid, 0, 2);

            Content = grid;
        }

        private void InitializeGameTimer()
        {
            _gameTimer = Dispatcher.CreateTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(30);
            _gameTimer.Tick += OnGameTick;
        }

        private void OnGameTick(object sender, EventArgs e)
        {
            if (!_gameRunning) return;

            UpdateGameState();
            SpawnObstacles();
            SpawnPickups();
            MoveGameObjects();
            CheckCollisions();
            _gameCanvas?.Invalidate();
            UpdateUI();
        }

        private void UpdateGameState()
        {
            _gameService.Distance += 1;
            _gameService.ElapsedTime += 0.03;

            double difficultyFactor = 1 + (_gameService.Distance / 1000);
            _gameSpeed = 5 * difficultyFactor * _gameService.GetDifficultyMultiplier(_appSettings.DifficultyMode);

            if (_appSettings.TimeLimit > 0 && _gameService.ElapsedTime >= _appSettings.TimeLimit)
            {
                EndGame();
            }
        }

        private void SpawnObstacles()
        {
            _spawnCounter++;
            int spawnRate = (int)(50 / _gameService.GetDifficultyMultiplier(_appSettings.DifficultyMode));

            if (_spawnCounter > spawnRate)
            {
                int lane = _random.Next(0, 3);
                string[] colors = { "#4ECDC4", "#FF6B6B", "#FFE66D", "#95E1D3" };
                var obstacle = new Obstacle(lane, colors[_random.Next(colors.Length)]);
                _gameDrawable.Obstacles.Add(obstacle);
                _spawnCounter = 0;
            }
        }

        private void SpawnPickups()
        {
            if (_random.Next(200) == 0)
            {
                int lane = _random.Next(0, 3);
                string type = _random.Next(2) == 0 ? "coin" : "boost";
                _gameDrawable.Pickups.Add(new Pickup(lane, type));
            }
        }

        private void MoveGameObjects()
        {
            for (int i = _gameDrawable.Obstacles.Count - 1; i >= 0; i--)
            {
                _gameDrawable.Obstacles[i].YPosition += _gameSpeed;
                if (_gameDrawable.Obstacles[i].YPosition > 800)
                {
                    _gameDrawable.Obstacles.RemoveAt(i);
                    _gameService.AddScore(10, _appSettings.DifficultyMode);
                }
            }

            for (int i = _gameDrawable.Pickups.Count - 1; i >= 0; i--)
            {
                _gameDrawable.Pickups[i].YPosition += _gameSpeed;
                if (_gameDrawable.Pickups[i].YPosition > 800)
                {
                    _gameDrawable.Pickups.RemoveAt(i);
                }
            }
        }

        private void CheckCollisions()
        {
            double playerY = _gameCanvas.Height - 120;
            const float playerHeight = 80;

            // Check obstacles
            foreach (var obstacle in _gameDrawable.Obstacles)
            {
                if (obstacle.Lane == _player.Lane && 
                    obstacle.YPosition >= playerY - 100 && 
                    obstacle.YPosition <= playerY + playerHeight)
                {
                    _gameService.Lives--;
                    _gameDrawable.Obstacles.Remove(obstacle);
                    if (_gameService.Lives <= 0)
                    {
                        EndGame();
                    }
                    break;
                }
            }

            // Check pickups
            foreach (var pickup in _gameDrawable.Pickups)
            {
                if (pickup.Lane == _player.Lane && 
                    pickup.YPosition >= playerY - 100 && 
                    pickup.YPosition <= playerY + playerHeight)
                {
                    _gameDrawable.Pickups.Remove(pickup);
                    if (pickup.Type == "coin")
                    {
                        _gameService.CoinsCollected++;
                        _gameService.AddScore(50, _appSettings.DifficultyMode);
                    }
                    else
                    {
                        _gameService.AddScore(100, _appSettings.DifficultyMode);
                    }
                    break;
                }
            }
        }

        private void UpdateUI()
        {
            ScoreText = $"Score: {_gameService.Score}";
            CoinsText = $"Distance: {_gameService.Distance}";
            LivesText = $"Lives: {_gameService.Lives}";

            _scoreLabel.Text = ScoreText;
            _coinsLabel.Text = CoinsText;
            _livesLabel.Text = LivesText;
        }

        private void OnLeftClicked(object sender, EventArgs e)
        {
            if (_player.Lane > 0)
            {
                _player.Lane--;
                _gameCanvas?.Invalidate();
            }
        }

        private void OnRightClicked(object sender, EventArgs e)
        {
            if (_player.Lane < 2)
            {
                _player.Lane++;
                _gameCanvas?.Invalidate();
            }
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            if (_gameRunning)
            {
                _gameTimer.Stop();
                _gameRunning = false;
                _stopBtn.Text = "Resume";
            }
            else
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            _gameService.ResetGame();
            _player = new Player();
            _player.VehicleColor = _appSettings.BoxColor;
            _gameDrawable = new GameDrawable { Player = _player };
            _gameCanvas.Drawable = _gameDrawable;
            _gameTimer.Start();
            _gameRunning = true;
            _stopBtn.Text = "Pause";
        }

        private void EndGame()
        {
            _gameTimer.Stop();
            _gameRunning = false;
            _stopBtn.Text = "Start Game";

            if (_gameService.Score > _appSettings.HighScore)
            {
                _appSettings.HighScore = _gameService.Score;
                _appSettings.Save();
            }

            Preferences.Set("GamesPlayed", Preferences.Get("GamesPlayed", 0) + 1);
            Preferences.Set("TotalDistance", Preferences.Get("TotalDistance", 0) + _gameService.Distance);
            Preferences.Set("TotalCoins", Preferences.Get("TotalCoins", 0) + _gameService.CoinsCollected);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Game Over", $"Final Score: {_gameService.Score}\nDistance: {_gameService.Distance}\nCoins: {_gameService.CoinsCollected}", "OK");
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
