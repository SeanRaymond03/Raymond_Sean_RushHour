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
        public List<RoadBlockade> RoadBlockades { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (canvas == null) return;

            // Background - Grey road
            canvas.FillColor = Color.FromArgb("#808080");
            canvas.FillRectangle(dirtyRect);

            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float laneWidth = width / 5;
            float playerY = height - 120;

            // Road lane markings - White dashed lines
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.StrokeDashPattern = new float[] { 10, 10 };

            for (int i = 1; i < 5; i++)
            {
                float xPos = laneWidth * i;
                canvas.DrawLine(xPos, 0, xPos, height);
            }
            canvas.StrokeDashPattern = null;

            // Draw road blockades (full-lane blockage with stone walls and cones)
            foreach (var blockade in RoadBlockades)
            {
                float blockadeX = blockade.Lane * laneWidth;
                float blockadeY = (float)blockade.YPosition;

                // Draw stone walls on left and right
                // Left stone wall
                canvas.FillColor = Color.FromArgb("#696969");
                canvas.FillRectangle(blockadeX - laneWidth * 0.2f, blockadeY - 10, laneWidth * 0.2f, 40);
                canvas.StrokeColor = Colors.Black;
                canvas.StrokeSize = 1;
                canvas.DrawRectangle(blockadeX - laneWidth * 0.2f, blockadeY - 10, laneWidth * 0.2f, 40);

                // Right stone wall
                canvas.FillColor = Color.FromArgb("#696969");
                canvas.FillRectangle(blockadeX + laneWidth, blockadeY - 10, laneWidth * 0.2f, 40);
                canvas.StrokeColor = Colors.Black;
                canvas.StrokeSize = 1;
                canvas.DrawRectangle(blockadeX + laneWidth, blockadeY - 10, laneWidth * 0.2f, 40);

                // Draw traffic cones filling the lane
                float coneSpacing = laneWidth / 4;
                for (float coneX = blockadeX + coneSpacing / 2; coneX < blockadeX + laneWidth; coneX += coneSpacing)
                {
                    // Cone base (orange/red)
                    canvas.FillColor = Color.FromArgb("#FF6B35");
                    canvas.FillRoundedRectangle(coneX - 8, blockadeY, 16, 25, 3);

                    // Cone top (white stripe)
                    canvas.FillColor = Colors.White;
                    canvas.FillRectangle(coneX - 6, blockadeY + 5, 12, 5);
                }
            }

            // Draw obstacles (top-down view, facing opposite direction)
            foreach (var obstacle in Obstacles)
            {
                float obsX = obstacle.Lane * laneWidth + laneWidth / 2 - 28;  // Narrower car
                float obsY = (float)obstacle.YPosition;

                // Check if this is a police car
                bool isPolice = obstacle.Type == "police";
                bool isFiretruck = obstacle.Type == "firetruck";
                bool isTruck = obstacle.Type == "truck";

                // Calculate vehicle length
                float vehicleLength = 70; // Default car length
                if (isFiretruck) vehicleLength = 210; // 3 car lengths
                if (isTruck) vehicleLength = 280; // 4 car lengths

                // Front bumper (silver/rounded) - longer nose
                canvas.FillColor = Color.FromArgb("#C0C0C0");
                canvas.FillRoundedRectangle(obsX, obsY - 5, 56, 8, 4);
                
                // Main car body (larger)
                canvas.FillColor = Color.FromArgb(obstacle.Color);
                canvas.FillRectangle(obsX, obsY + 3, 56, vehicleLength);
                
                // Hood line - front detail
                canvas.StrokeColor = Color.FromArgb("#000000");
                canvas.StrokeSize = 1;
                canvas.DrawLine(obsX, obsY + 15, obsX + 56, obsY + 15);
                
                // Boot line - back detail
                canvas.DrawLine(obsX, obsY + vehicleLength - 12, obsX + 56, obsY + vehicleLength - 12);

                if (isTruck)
                {
                    // 16-wheeler: Draw trailer first, then cab at bottom
                    // Trailer section (main cargo area)
                    canvas.FillColor = Color.FromArgb(obstacle.Color);
                    canvas.FillRectangle(obsX, obsY + 3, 56, vehicleLength - 50);
                    
                    // Draw multiple axle lines for trailer
                    for (int i = 1; i < 3; i++)
                    {
                        float lineY = obsY + 3 + ((vehicleLength - 50) / 2) * i;
                        canvas.DrawLine(obsX, lineY, obsX + 56, lineY);
                    }
                    
                    // Draw cab (back section at bottom)
                    canvas.FillColor = Color.FromArgb("#1a1a1a"); // Dark cab color
                    canvas.FillRectangle(obsX, obsY + vehicleLength - 47, 56, 50);
                    
                    // Cab windshield (at bottom)
                    canvas.FillColor = Color.FromArgb("#4A90B8");
                    canvas.FillRectangle(obsX + 8, obsY + vehicleLength - 35, 40, 25);
                    
                    // Cab window outline
                    canvas.StrokeColor = Color.FromArgb("#000000");
                    canvas.StrokeSize = 1;
                    canvas.DrawRectangle(obsX + 8, obsY + vehicleLength - 35, 40, 25);
                    
                    // Draw multiple sets of wheels (16 wheels total)
                    canvas.FillColor = Color.FromArgb("#222222");
                    
                    // Front wheels (on trailer front)
                    canvas.FillRectangle(obsX - 4, obsY + 12, 5, 10);
                    canvas.FillRectangle(obsX + 55, obsY + 12, 5, 10);
                    
                    // Back wheel sets (multiple axles on trailer and cab)
                    for (int i = 1; i <= 3; i++)
                    {
                        float wheelY = obsY + 3 + ((vehicleLength - 50) / 3) * i;
                        canvas.FillRectangle(obsX - 4, wheelY - 5, 5, 10);
                        canvas.FillRectangle(obsX + 55, wheelY - 5, 5, 10);
                    }
                }
                else if (isFiretruck)
                {
                    // Firetruck: Main body first, then cab at bottom
                    // Main firetruck body (cargo area)
                    canvas.FillColor = Color.FromArgb(obstacle.Color);
                    canvas.FillRectangle(obsX, obsY + 3, 56, vehicleLength - 50);
                    
                    // Windshield at BOTTOM (cab area)
                    canvas.FillColor = Color.FromArgb("#4A90B8");
                    canvas.FillRectangle(obsX + 8, obsY + vehicleLength - 35, 40, 25);
                    
                    // Window outline
                    canvas.StrokeColor = Color.FromArgb("#000000");
                    canvas.StrokeSize = 1;
                    canvas.DrawRectangle(obsX + 8, obsY + vehicleLength - 35, 40, 25);

                    // Back bumper (silver) - at very bottom
                    canvas.FillColor = Color.FromArgb("#C0C0C0");
                    canvas.FillRectangle(obsX, obsY + vehicleLength - 8, 56, 8);

                    // Wheels for firetruck
                    canvas.FillColor = Color.FromArgb("#222222");
                    // Front wheels (top)
                    canvas.FillRectangle(obsX - 4, obsY + 12, 5, 10);
                    canvas.FillRectangle(obsX + 55, obsY + 12, 5, 10);
                    // Back wheels (bottom near cab)
                    canvas.FillRectangle(obsX - 4, obsY + vehicleLength - 20, 5, 10);
                    canvas.FillRectangle(obsX + 55, obsY + vehicleLength - 20, 5, 10);

                    // Firetruck lights at BOTTOM (by the windshield/cab)
                    canvas.FillColor = Color.FromArgb("#FF0000");
                    canvas.FillCircle(obsX + 20, obsY + vehicleLength - 55, 8);

                    canvas.FillColor = Color.FromArgb("#0000FF");
                    canvas.FillCircle(obsX + 36, obsY + vehicleLength - 55, 8);

                    // Red light beam (left)
                    canvas.FillColor = Color.FromArgb("80FF0000");
                    canvas.StrokeColor = Color.FromArgb("80FF0000");
                    canvas.StrokeSize = 16;
                    canvas.DrawLine(obsX + 20, obsY + vehicleLength - 55, obsX - 20, obsY + vehicleLength - 55);
                    canvas.DrawLine(obsX + 20, obsY + vehicleLength - 55, obsX - 15, obsY + vehicleLength - 75);
                    canvas.DrawLine(obsX + 20, obsY + vehicleLength - 55, obsX - 15, obsY + vehicleLength - 35);
                    canvas.StrokeSize = 2;
                    canvas.DrawLine(obsX - 15, obsY + vehicleLength - 75, obsX - 15, obsY + vehicleLength - 35);

                    // Blue light beam (right)
                    canvas.FillColor = Color.FromArgb("800000FF");
                    canvas.StrokeColor = Color.FromArgb("800000FF");
                    canvas.StrokeSize = 16;
                    canvas.DrawLine(obsX + 36, obsY + vehicleLength - 55, obsX + 70, obsY + vehicleLength - 55);
                    canvas.DrawLine(obsX + 36, obsY + vehicleLength - 55, obsX + 65, obsY + vehicleLength - 75);
                    canvas.DrawLine(obsX + 36, obsY + vehicleLength - 55, obsX + 65, obsY + vehicleLength - 35);
                    canvas.StrokeSize = 2;
                    canvas.DrawLine(obsX + 65, obsY + vehicleLength - 75, obsX + 65, obsY + vehicleLength - 35);
                }
                else if (!isTruck)
                {
                    // Regular car windshield and lights
                    if (isPolice)
                    {
                        // Windshield (at bottom - facing opposite direction)
                        canvas.FillColor = Color.FromArgb("#4A90B8");
                        canvas.FillRectangle(obsX + 8, obsY + 45, 40, 13);
                        
                        // Window outline
                        canvas.StrokeColor = Color.FromArgb("#000000");
                        canvas.StrokeSize = 1;
                        canvas.DrawRectangle(obsX + 8, obsY + 45, 40, 13);

                        // Back bumper (silver) - bottom of car
                        canvas.FillColor = Color.FromArgb("#C0C0C0");
                        canvas.FillRectangle(obsX, obsY + 68, 56, 8);

                        // Wheels - 4 wheels (closer to car body)
                        canvas.FillColor = Color.FromArgb("#222222");
                        // Left front wheel
                        canvas.FillRectangle(obsX - 4, obsY + 12, 5, 10);
                        // Right front wheel
                        canvas.FillRectangle(obsX + 55, obsY + 12, 5, 10);
                        // Left back wheel
                        canvas.FillRectangle(obsX - 4, obsY + 52, 5, 10);
                        // Right back wheel
                        canvas.FillRectangle(obsX + 55, obsY + 52, 5, 10);

                        // Police car lights at top
                        // Red light on left
                        canvas.FillColor = Color.FromArgb("#FF0000");
                        canvas.FillCircle(obsX + 28, obsY + 35, 8);

                        // Blue light on right
                        canvas.FillColor = Color.FromArgb("#0000FF");
                        canvas.FillCircle(obsX + 28, obsY + 35, 8);

                        // Red light beam (left) - projects to the left
                        canvas.FillColor = Color.FromArgb("80FF0000");
                        canvas.StrokeColor = Color.FromArgb("80FF0000");
                        canvas.StrokeSize = 16;
                        canvas.DrawLine(obsX + 28, obsY + 35, obsX - 20, obsY + 35);
                        canvas.DrawLine(obsX + 28, obsY + 35, obsX - 15, obsY + 15);
                        canvas.DrawLine(obsX + 28, obsY + 35, obsX - 15, obsY + 55);
                        canvas.StrokeSize = 2;
                        canvas.DrawLine(obsX - 15, obsY + 15, obsX - 15, obsY + 55);

                        // Blue light beam (right) - projects to the right
                        canvas.FillColor = Color.FromArgb("800000FF");
                        canvas.StrokeColor = Color.FromArgb("800000FF");
                        canvas.StrokeSize = 16;
                        canvas.DrawLine(obsX + 28, obsY + 35, obsX + 70, obsY + 35);
                        canvas.DrawLine(obsX + 28, obsY + 35, obsX + 65, obsY + 15);
                        canvas.DrawLine(obsX + 28, obsY + 35, obsX + 65, obsY + 55);
                        canvas.StrokeSize = 2;
                        canvas.DrawLine(obsX + 65, obsY + 15, obsX + 65, obsY + 55);
                    }
                    else
                    {
                        // Regular car
                        // Windshield (at bottom - facing opposite direction)
                        canvas.FillColor = Color.FromArgb("#4A90B8");
                        canvas.FillRectangle(obsX + 8, obsY + 45, 40, 13);
                        
                        // Window outline
                        canvas.StrokeColor = Color.FromArgb("#000000");
                        canvas.StrokeSize = 1;
                        canvas.DrawRectangle(obsX + 8, obsY + 45, 40, 13);

                        // Back bumper (silver) - bottom of car
                        canvas.FillColor = Color.FromArgb("#C0C0C0");
                        canvas.FillRectangle(obsX, obsY + 68, 56, 8);

                        // Wheels - 4 wheels (closer to car body)
                        canvas.FillColor = Color.FromArgb("#222222");
                        // Left front wheel
                        canvas.FillRectangle(obsX - 4, obsY + 12, 5, 10);
                        // Right front wheel
                        canvas.FillRectangle(obsX + 55, obsY + 12, 5, 10);
                        // Left back wheel
                        canvas.FillRectangle(obsX - 4, obsY + 52, 5, 10);
                        canvas.FillRectangle(obsX + 55, obsY + 52, 5, 10);

                        // Headlights at bottom for regular cars
                        canvas.FillColor = Colors.Yellow;
                        canvas.FillCircle(obsX + 14, obsY + 70, 3);
                        canvas.FillCircle(obsX + 42, obsY + 70, 3);

                        // Draw left headlight beam as filled triangle using lines
                        canvas.FillColor = Color.FromArgb("80FFFF00");
                        canvas.FillCircle(obsX + 14, obsY + 70, 1);
                        canvas.StrokeColor = Color.FromArgb("80FFFF00");
                        canvas.StrokeSize = 14;
                        canvas.DrawLine(obsX + 14, obsY + 70, obsX + 6, obsY + 100);
                        canvas.DrawLine(obsX + 14, obsY + 70, obsX + 22, obsY + 100);
                        canvas.StrokeSize = 2;
                        canvas.DrawLine(obsX + 6, obsY + 100, obsX + 22, obsY + 100);

                        // Draw right headlight beam as filled triangle using lines
                        canvas.FillColor = Color.FromArgb("80FFFF00");
                        canvas.FillCircle(obsX + 42, obsY + 70, 1);
                        canvas.StrokeColor = Color.FromArgb("80FFFF00");
                        canvas.StrokeSize = 14;
                        canvas.DrawLine(obsX + 42, obsY + 70, obsX + 34, obsY + 100);
                        canvas.DrawLine(obsX + 42, obsY + 70, obsX + 50, obsY + 100);
                        canvas.StrokeSize = 2;
                        canvas.DrawLine(obsX + 34, obsY + 100, obsX + 50, obsY + 100);
                    }
                }
            }

            // Draw pickups (coins and boosts)
            foreach (var pickup in Pickups)
            {
                float pickX = pickup.Lane * laneWidth + laneWidth / 2;
                float pickY = (float)pickup.YPosition;
                
                if (pickup.Type == "boost")
                {
                    // Draw boost as green + sign
                    canvas.FillColor = Color.FromArgb("#00FF00");
                    // Vertical bar of +
                    canvas.FillRectangle(pickX - 3, pickY - 12, 6, 24);
                    // Horizontal bar of +
                    canvas.FillRectangle(pickX - 12, pickY - 3, 24, 6);
                    
                    // Green outline
                    canvas.StrokeColor = Color.FromArgb("#00CC00");
                    canvas.StrokeSize = 2;
                    canvas.DrawRectangle(pickX - 3, pickY - 12, 6, 24);
                    canvas.DrawRectangle(pickX - 12, pickY - 3, 24, 6);
                }
                else
                {
                    // Draw coin as gold circle with $ sign
                    canvas.FillColor = Color.FromArgb("#FFD700");
                    canvas.FillCircle(pickX, pickY, 12);
                    
                    // Outer ring for depth
                    canvas.StrokeColor = Color.FromArgb("#DAA520");
                    canvas.StrokeSize = 2;
                    canvas.DrawCircle(pickX, pickY, 12);
                    
                    // Dollar sign
                    canvas.FillColor = Color.FromArgb("#654321");
                    canvas.FontSize = 14;
                    canvas.Font = Microsoft.Maui.Graphics.Font.Default;
                    canvas.DrawString("$", pickX - 3, pickY - 7, 6, 14, HorizontalAlignment.Center, VerticalAlignment.Center);
                }
            }

            // Draw player (top-down view, facing forward)
            float playerX = Player.Lane * laneWidth + laneWidth / 2 - 28;  // Narrower car
            canvas.FillColor = Color.FromArgb(Player.VehicleColor);
            
            // Front bumper (silver/rounded) - longer nose
            canvas.FillColor = Color.FromArgb("#C0C0C0");
            canvas.FillRoundedRectangle(playerX, playerY - 5, 56, 8, 4);
            
            // Main car body (larger)
            canvas.FillColor = Color.FromArgb(Player.VehicleColor);
            canvas.FillRectangle(playerX, playerY + 3, 56, 70);
            
            // Hood line - front detail
            canvas.StrokeColor = Color.FromArgb("#000000");
            canvas.StrokeSize = 1;
            canvas.DrawLine(playerX, playerY + 15, playerX + 56, playerY + 15);
            
            // Boot line - back detail
            canvas.DrawLine(playerX, playerY + 58, playerX + 56, playerY + 58);
            
            // Windshield (at top)
            canvas.FillColor = Color.FromArgb("#4A90B8");
            canvas.FillRectangle(playerX + 8, playerY + 10, 40, 13);
            
            // Window outline
            canvas.StrokeColor = Color.FromArgb("#000000");
            canvas.StrokeSize = 1;
            canvas.DrawRectangle(playerX + 8, playerY + 10, 40, 13);

            // Back bumper (silver) - bottom of car
            canvas.FillColor = Color.FromArgb("#C0C0C0");
            canvas.FillRectangle(playerX, playerY + 68, 56, 8);

            // Wheels - 4 wheels (closer to car body)
            canvas.FillColor = Color.FromArgb("#222222");
            // Left front wheel
            canvas.FillRectangle(playerX - 4, playerY + 12, 5, 10);
            // Right front wheel
            canvas.FillRectangle(playerX + 55, playerY + 12, 5, 10);
            // Left back wheel
            canvas.FillRectangle(playerX - 4, playerY + 52, 5, 10);
            // Right back wheel
            canvas.FillRectangle(playerX + 55, playerY + 52, 5, 10);

            // Headlights at top
            canvas.FillColor = Colors.Yellow;
            canvas.FillCircle(playerX + 14, playerY + 2, 3);
            canvas.FillCircle(playerX + 42, playerY + 2, 3);

            // Draw left player headlight beam
            canvas.FillColor = Color.FromArgb("80FFFF00");
            canvas.FillCircle(playerX + 14, playerY + 2, 1);
            canvas.StrokeColor = Color.FromArgb("80FFFF00");
            canvas.StrokeSize = 14;
            canvas.DrawLine(playerX + 14, playerY + 2, playerX + 6, playerY - 30);
            canvas.DrawLine(playerX + 14, playerY + 2, playerX + 22, playerY - 30);
            canvas.StrokeSize = 2;
            canvas.DrawLine(playerX + 6, playerY - 30, playerX + 22, playerY - 30);

            // Draw right player headlight beam
            canvas.FillColor = Color.FromArgb("80FFFF00");
            canvas.FillCircle(playerX + 42, playerY + 2, 1);
            canvas.StrokeColor = Color.FromArgb("80FFFF00");
            canvas.StrokeSize = 14;
            canvas.DrawLine(playerX + 42, playerY + 2, playerX + 34, playerY - 30);
            canvas.DrawLine(playerX + 42, playerY + 2, playerX + 50, playerY - 30);
            canvas.StrokeSize = 2;
            canvas.DrawLine(playerX + 34, playerY - 30, playerX + 50, playerY - 30);
        }
    }

    public class RoadBlockade
    {
        public int Lane { get; set; }
        public double YPosition { get; set; }

        public RoadBlockade(int lane, double yPosition)
        {
            Lane = lane;
            YPosition = yPosition;
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
        private double _gameSpeed = 5.5;  // 10% faster (was 5.0)
        private int _spawnCounter = 0;
        private int _blockadeSpawnCounter = 0;
        private bool _gameRunning = false;
        private GraphicsView _gameCanvas;
        private Label _scoreLabel;
        private Label _livesLabel;
        private Button _leftBtn, _pauseBtn, _rightBtn;
        private Button _startRestartBtn;
        private Border _gameOverBorder;
        private Border _mainMenuBorder;
        private bool _gamePaused = false;

        private string _scoreText;
        private string _livesText;

        public string ScoreText
        {
            get => _scoreText;
            set { _scoreText = value; OnPropertyChanged(); }
        }

        public string LivesText
        {
            get => _livesText;
            set { _livesText = value; OnPropertyChanged(); }
        }

        public GamePage()
        {
            _appSettings = new AppSettings();
            _appSettings.Load();

            _gameService = new GameService();
            _player = new Player();
            _player.VehicleColor = _appSettings.BoxColor;
            _gameDrawable = new GameDrawable { Player = _player };

            BuildUI();
            InitializeGameTimer();
            UpdateUI();
            
            // Show main menu overlay at start
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _mainMenuBorder.IsVisible = true;
            });
        }

        private void BuildUI()
        {
            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.Padding = 0;
            mainGrid.ColumnSpacing = 0;

            // Game area
            _gameCanvas = new GraphicsView 
            { 
                Drawable = _gameDrawable,
                BackgroundColor = Color.FromArgb("#808080")
            };
            mainGrid.Add(_gameCanvas, 0, 0);

            // Game Over overlay
            _gameOverBorder = new Border
            {
                BackgroundColor = Color.FromArgb("A0000000"),
                Padding = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false,
                WidthRequest = 300,
                StrokeThickness = 0
            };

            var gameOverStack = new VerticalStackLayout
            {
                Spacing = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var gameOverLabel = new Label
            {
                Text = "GAME OVER",
                FontSize = 48,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var finalScoreLabel = new Label
            {
                Text = "Score: 0",
                FontSize = 32,
                FontFamily = "BBHBogle",
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var restartBtn = new Button
            {
                Text = "Restart",
                BackgroundColor = Color.FromArgb("#FF6B6B"),
                TextColor = Colors.White,
                Padding = new Thickness(40, 15),
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle"
            };
            restartBtn.Clicked += (s, e) => RestartGame();

            var menuBtn = new Button
            {
                Text = "Menu",
                BackgroundColor = Color.FromArgb("#B19CD9"),
                TextColor = Colors.Black,
                Padding = new Thickness(40, 15),
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle"
            };
            menuBtn.Clicked += (s, e) => ShowMainMenu();

            gameOverStack.Add(gameOverLabel);
            gameOverStack.Add(finalScoreLabel);
            gameOverStack.Add(restartBtn);
            gameOverStack.Add(menuBtn);

            _gameOverBorder.Content = gameOverStack;
            mainGrid.Add(_gameOverBorder, 0, 0);

            // Main Menu overlay (shows when game not running)
            _mainMenuBorder = new Border
            {
                BackgroundColor = Color.FromArgb("D0000000"),
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                IsVisible = false,
                StrokeThickness = 0
            };

            var menuContainer = new Grid();
            menuContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            menuContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            menuContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            menuContainer.Padding = 30;
            menuContainer.HorizontalOptions = LayoutOptions.Center;
            menuContainer.VerticalOptions = LayoutOptions.Center;
            menuContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Left side - Color picker
            var colorStack = new VerticalStackLayout
            {
                Spacing = 5,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 30, 0)
            };

            var colors = new (string name, string hex)[]
            {
                ("Red", "#FF0000"),
                ("Blue", "#0000FF"),
                ("Yellow", "#FFFF00"),
                ("Green", "#00FF00"),
                ("Cyan", "#00FFFF"),
                ("Magenta", "#FF00FF")
            };

            foreach (var (name, hex) in colors)
            {
                var colorBtn = new Button
                {
                    WidthRequest = 50,
                    HeightRequest = 50,
                    BackgroundColor = Color.FromArgb(hex),
                    BorderWidth = 2,
                    BorderColor = _appSettings.BoxColor == hex ? Colors.White : Colors.Transparent,
                    CornerRadius = 5,
                    Padding = 0
                };
                colorBtn.Clicked += (s, e) => 
                {
                    _appSettings.BoxColor = hex;
                    _appSettings.Save();
                    _player.VehicleColor = hex;
                    _gameDrawable.Player = _player;
                    _gameCanvas?.Invalidate();
                };
                colorStack.Add(colorBtn);
            }

            menuContainer.Add(colorStack, 0, 0);

            // Middle side - Rush Hour title
            var titleLabel = new Label
            {
                Text = "RUSH\nHOUR",
                FontSize = 72,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = Color.FromArgb("#B19CD9"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };
            menuContainer.Add(titleLabel, 1, 0);

            // Right side - Menu options
            var menuStack = new VerticalStackLayout
            {
                Spacing = 15,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(20, 0, 0, 0)
            };

            var playBtn = new Button
            {
                Text = "PLAY",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = Colors.Black,
                BackgroundColor = Color.FromArgb("#B19CD9"),
                Padding = new Thickness(15, 10)
            };
            playBtn.Clicked += (s, e) => OnStartRestartClicked(s, e);
            menuStack.Add(playBtn);

            var settingsBtn = new Button
            {
                Text = "SETTINGS",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = Colors.Black,
                BackgroundColor = Color.FromArgb("#B19CD9"),
                Padding = new Thickness(15, 8)
            };
            settingsBtn.Clicked += (s, e) => GoToSettings();
            menuStack.Add(settingsBtn);

            var statsBtn = new Button
            {
                Text = "STATS",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = Colors.Black,
                BackgroundColor = Color.FromArgb("#B19CD9"),
                Padding = new Thickness(15, 8)
            };
            statsBtn.Clicked += (s, e) => GoToStats();
            menuStack.Add(statsBtn);

            // Difficulty Picker (moved to bottom)
            var difficultyLabel = new Label
            {
                Text = "Difficulty",
                FontSize = 14,
                FontFamily = "BBHBogle",
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 10, 0, 5)
            };
            menuStack.Add(difficultyLabel);

            var difficultyPicker = new Picker
            {
                Title = "Select Difficulty",
                TextColor = Colors.Black,
                BackgroundColor = Color.FromArgb("#B19CD9"),
                FontFamily = "BBHBogle"
            };
            difficultyPicker.ItemsSource = new List<string> { "Easy", "Medium", "Hard" };
            difficultyPicker.SelectedItem = _appSettings.DifficultyMode;
            difficultyPicker.SelectedIndexChanged += (s, e) =>
            {
                if (difficultyPicker.SelectedItem is string difficulty)
                {
                    _appSettings.DifficultyMode = difficulty;
                    _appSettings.Save();
                }
            };
            menuStack.Add(difficultyPicker);

            menuContainer.Add(menuStack, 2, 0);
            _mainMenuBorder.Content = menuContainer;
            mainGrid.Add(_mainMenuBorder, 0, 0);

            // Level and stats section combined
            var levelStack = new VerticalStackLayout
            {
                Padding = 10,
                Spacing = 0,
                BackgroundColor = Color.FromArgb("#000000")
            };

            // Stats and level row (all on same level)
            var statsGrid = new Grid();
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.Padding = 5;

            _scoreLabel = new Label 
            { 
                Text = "Score: 0",
                TextColor = Colors.White, 
                FontAttributes = FontAttributes.Bold, 
                FontSize = 12
            };

            var levelLabel = new Label
            {
                Text = "LEVEL 1",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            _livesLabel = new Label 
            { 
                Text = "Lives: 3",
                TextColor = Colors.White, 
                FontAttributes = FontAttributes.Bold, 
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.End
            };

            statsGrid.Add(_scoreLabel, 0, 0);
            statsGrid.Add(levelLabel, 1, 0);
            statsGrid.Add(_livesLabel, 2, 0);

            levelStack.Add(statsGrid);
            mainGrid.Add(levelStack, 0, 1);

            // Control buttons with full black background
            var buttonContainer = new Grid();
            buttonContainer.BackgroundColor = Color.FromArgb("#000000");
            buttonContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            buttonContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            buttonContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            buttonContainer.Padding = 30;

            // Center - Movement controls (LEFT, PAUSE/RESUME, RIGHT)
            var centerControlStack = new HorizontalStackLayout
            {
                Padding = 0,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill
            };

            // Left move button - Lilac Purple
            _leftBtn = new Button 
            { 
                Text = "LEFT", 
                BackgroundColor = Color.FromArgb("#B19CD9"),
                TextColor = Colors.Black,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 80
            };
            _leftBtn.Clicked += OnLeftClicked;
            _leftBtn.Pressed += (s, e) => _leftBtn.BackgroundColor = Color.FromArgb("#9370DB");
            _leftBtn.Released += (s, e) => _leftBtn.BackgroundColor = Color.FromArgb("#B19CD9");
            centerControlStack.Add(_leftBtn);

            // Pause/Resume button - Lilac Purple
            _pauseBtn = new Button 
            { 
                Text = "PAUSE", 
                BackgroundColor = Color.FromArgb("#B19CD9"),
                TextColor = Colors.Black,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 100
            };
            _pauseBtn.Clicked += OnPauseClicked;
            _pauseBtn.Pressed += (s, e) => _pauseBtn.BackgroundColor = Color.FromArgb("#9370DB");
            _pauseBtn.Released += (s, e) => _pauseBtn.BackgroundColor = Color.FromArgb("#B19CD9");
            centerControlStack.Add(_pauseBtn);

            // Right move button - Lilac Purple
            _rightBtn = new Button 
            { 
                Text = "RIGHT", 
                BackgroundColor = Color.FromArgb("#B19CD9"),
                TextColor = Colors.Black,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 80
            };
            _rightBtn.Clicked += OnRightClicked;
            _rightBtn.Pressed += (s, e) => _rightBtn.BackgroundColor = Color.FromArgb("#9370DB");
            _rightBtn.Released += (s, e) => _rightBtn.BackgroundColor = Color.FromArgb("#B19CD9");
            centerControlStack.Add(_rightBtn);

            buttonContainer.Add(centerControlStack, 1, 0);

            // Right side - Start/Restart button aligned with Level text
            var rightButtonStack = new VerticalStackLayout
            {
                Padding = 0,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };

            var startRestartBtn = new Button 
            { 
                Text = "START", 
                BackgroundColor = Color.FromArgb("#B19CD9"),
                TextColor = Colors.Black,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 80,
                Margin = new Thickness(30, 0, 10, 0)
            };
            _startRestartBtn = startRestartBtn;
            startRestartBtn.Clicked += OnStartRestartClicked;
            startRestartBtn.Pressed += (s, e) => startRestartBtn.BackgroundColor = Color.FromArgb("#9370DB");
            startRestartBtn.Released += (s, e) => startRestartBtn.BackgroundColor = Color.FromArgb("#B19CD9");
            rightButtonStack.Add(startRestartBtn);

            buttonContainer.Add(rightButtonStack, 2, 0);
            mainGrid.Add(buttonContainer, 0, 2);

            Content = mainGrid;
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

            _gameService.Distance += 1;
            _gameService.ElapsedTime += 0.03;

            double difficultyFactor = 1 + (_gameService.Distance / 1000);
            _gameSpeed = 5 * difficultyFactor * _gameService.GetDifficultyMultiplier(_appSettings.DifficultyMode);

            if (_appSettings.TimeLimit > 0 && _gameService.ElapsedTime >= _appSettings.TimeLimit)
            {
                EndGame();
            }

            SpawnObstacles();
            SpawnRoadBlockades();
            SpawnPickups();
            MoveGameObjects();
            CheckCollisions();
            _gameCanvas?.Invalidate();
            UpdateUI();
        }

        private void SpawnObstacles()
        {
            _spawnCounter++;
            int spawnRate = (int)(30 / _gameService.GetDifficultyMultiplier(_appSettings.DifficultyMode));

            if (_spawnCounter > spawnRate)
            {
                int lane = _random.Next(0, 5);
                string[] colors = { "#FF0000", "#0000FF", "#FFFF00", "#00FF00", "#FF00FF" };
                
                // Vehicle type spawn logic
                int vehicleRandom = _random.Next(100);
                string type = "car";
                string carColor = colors[_random.Next(colors.Length)];

                if (vehicleRandom < 10) // 10% chance for police car
                {
                    type = "police";
                    carColor = "#FFFFFF";
                }
                else if (vehicleRandom < 20) // 10% chance for firetruck
                {
                    type = "firetruck";
                    carColor = "#FF0000";
                }
                else if (vehicleRandom < 25) // 5% chance for 16-wheeler
                {
                    type = "truck";
                    carColor = "#8B4513";
                }
                
                var obstacle = new Obstacle(lane, carColor, type);
                _gameDrawable.Obstacles.Add(obstacle);
                _spawnCounter = 0;
            }
        }

        private void SpawnRoadBlockades()
        {
            _blockadeSpawnCounter++;
            int blockadeSpawnRate = 250;

            if (_blockadeSpawnCounter > blockadeSpawnRate)
            {
                int lane = _random.Next(0, 5);
                var blockade = new RoadBlockade(lane, -50);
                _gameDrawable.RoadBlockades.Add(blockade);
                _blockadeSpawnCounter = 0;
            }
        }

        private void SpawnPickups()
        {
            if (_random.Next(100) == 0)
            {
                int lane = _random.Next(0, 5);
                // 10% chance for boost, 90% for coin
                string pickupType = _random.Next(10) == 0 ? "boost" : "coin";
                _gameDrawable.Pickups.Add(new Pickup(lane, pickupType));
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

            for (int i = _gameDrawable.RoadBlockades.Count - 1; i >= 0; i--)
            {
                _gameDrawable.RoadBlockades[i].YPosition += _gameSpeed;
                if (_gameDrawable.RoadBlockades[i].YPosition > 800)
                {
                    _gameDrawable.RoadBlockades.RemoveAt(i);
                }
            }
        }

        private void CheckCollisions()
        {
            double playerY = _gameCanvas.Height - 120;
            const float playerHeight = 60;

            // Check obstacles
            foreach (var obstacle in _gameDrawable.Obstacles.ToList())
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

            // Check road blockades
            foreach (var blockade in _gameDrawable.RoadBlockades.ToList())
            {
                if (blockade.Lane == _player.Lane && 
                    blockade.YPosition >= playerY - 100 && 
                    blockade.YPosition <= playerY + playerHeight)
                {
                    _gameService.Lives--;
                    _gameDrawable.RoadBlockades.Remove(blockade);
                    if (_gameService.Lives <= 0)
                    {
                        EndGame();
                    }
                    break;
                }
            }

            // Check pickups
            foreach (var pickup in _gameDrawable.Pickups.ToList())
            {
                if (pickup.Lane == _player.Lane && 
                    pickup.YPosition >= playerY - 100 && 
                    pickup.YPosition <= playerY + playerHeight)
                {
                    _gameDrawable.Pickups.Remove(pickup);
                    if (pickup.Type == "boost")
                    {
                        // Activate speed boost for 200 units of distance
                        _gameSpeed += 200;
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await Task.Delay(200); // Boost lasts briefly
                                _gameSpeed -= 200;
                            });
                        });
                    }
                    else
                    {
                        // Regular coin
                        _gameService.CoinsCollected++;
                        _gameService.AddScore(50, _appSettings.DifficultyMode);
                    }
                    break;
                }
            }
        }

        private void UpdateUI()
        {
            ScoreText = $"Score: {_gameService.Score}";
            LivesText = $"Lives: {_gameService.Lives}";

            _scoreLabel.Text = ScoreText;
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
            if (_player.Lane < 4)
            {
                _player.Lane++;
                _gameCanvas?.Invalidate();
            }
        }

        private void OnPauseClicked(object sender, EventArgs e)
        {
            if (!_gameRunning)
                return; // Do nothing if game isn't running

            if (_gamePaused)
            {
                // Resume the game
                _gameTimer.Start();
                _gamePaused = false;
                _pauseBtn.Text = "PAUSE";
            }
            else
            {
                // Pause the game
                _gameTimer.Stop();
                _gamePaused = true;
                _pauseBtn.Text = "RESUME";
            }
        }

        private void OnStartRestartClicked(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            _gameDrawable.Obstacles.Clear();
            _gameDrawable.Pickups.Clear();
            _gameDrawable.RoadBlockades.Clear();
            _gameService.ResetGame();
            _player = new Player();
            _player.VehicleColor = _appSettings.BoxColor;
            _gameDrawable.Player = _player;

            _gameOverBorder.IsVisible = false;
            _mainMenuBorder.IsVisible = false;
            _gameTimer.Start();
            _gameRunning = true;
            _gamePaused = false;
            _pauseBtn.Text = "PAUSE";
            _startRestartBtn.Text = "RESTART";
        }

        private void EndGame()
        {
            _gameTimer.Stop();
            _gameRunning = false;
            _gamePaused = false;

            if (_gameService.Score > _appSettings.HighScore)
            {
                _appSettings.HighScore = _gameService.Score;
                _appSettings.Save();
            }

            Preferences.Set("GamesPlayed", Preferences.Get("GamesPlayed", 0) + 1);
            Preferences.Set("TotalDistance", Preferences.Get("TotalDistance", 0) + _gameService.Distance);
            Preferences.Set("TotalCoins", Preferences.Get("TotalCoins", 0) + _gameService.CoinsCollected);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_gameOverBorder.Content is VerticalStackLayout stack && stack.Children.Count > 1)
                {
                    if (stack.Children[1] is Label scoreLabel)
                    {
                        scoreLabel.Text = $"Score: {_gameService.Score}";
                    }
                }
                _gameOverBorder.IsVisible = true;
                _mainMenuBorder.IsVisible = false;
                _pauseBtn.Text = "PAUSE";
                _startRestartBtn.Text = "START";
            });
        }

        private void RestartGame()
        {
            StartGame();
        }

        private void ShowMainMenu()
        {
            _mainMenuBorder.IsVisible = true;
            _gameOverBorder.IsVisible = false;
        }

        private void OpenSettings()
        {
            // Navigate to settings page or open settings overlay
            // This function can be implemented based on the specific navigation or modal display logic of the app
        }

        private async void GoToSettings()
        {
            try
            {
                _mainMenuBorder.IsVisible = false;
                await Shell.Current.GoToAsync("///SettingsPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        private async void GoToStats()
        {
            try
            {
                _mainMenuBorder.IsVisible = false;
                await Shell.Current.GoToAsync("///ScorePage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
