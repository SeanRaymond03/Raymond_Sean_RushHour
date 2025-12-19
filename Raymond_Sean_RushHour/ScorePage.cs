using Raymond_Sean_RushHour.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Raymond_Sean_RushHour
{
    public partial class ScorePage : ContentPage, INotifyPropertyChanged
    {
        private AppSettings _appSettings;
        private VerticalStackLayout _mainStack;
        private Label _highScoreLabel;
        private Label _distanceLabel;
        private Label _coinsLabel;
        private Label _gamesLabel;

        public ScorePage()
        {
            _appSettings = new AppSettings();
            _appSettings.Load();

            BuildUI();
            UpdateUI();
        }

        private void BuildUI()
        {
            Title = "Scores";
            BackgroundColor = _appSettings.DarkMode ? Colors.Black : Colors.White;

            _mainStack = new VerticalStackLayout { Padding = 20, Spacing = 15 };

            // Title
            var titleLabel = new Label
            {
                Text = "Leaderboard",
                FontSize = 28,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            };
            _mainStack.Add(titleLabel);

            // High Score Card
            _highScoreLabel = new Label
            {
                Text = "0",
                FontSize = 48,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb(_appSettings.BoxColor),
                HorizontalTextAlignment = TextAlignment.Center
            };

            var scoreFrame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Color.FromArgb(_appSettings.BoxColor),
                HasShadow = true,
                Content = new VerticalStackLayout
                {
                    Spacing = 5,
                    Padding = 15,
                    Children =
                    {
                        new Label
                        {
                            Text = "Your Best Score",
                            FontSize = 16,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
                        },
                        _highScoreLabel
                    }
                }
            };
            _mainStack.Add(scoreFrame);

            // Stats Grid
            var statsGrid = new Grid();
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            statsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            statsGrid.ColumnSpacing = 10;
            statsGrid.RowSpacing = 10;

            _distanceLabel = new Label { FontSize = 20, TextColor = Color.FromArgb(_appSettings.BoxColor) };
            var distFrame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Color.FromArgb(_appSettings.BoxColor),
                HasShadow = true,
                Content = new VerticalStackLayout
                {
                    Spacing = 5,
                    Padding = 15,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label { Text = "Total Distance", FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black },
                        _distanceLabel
                    }
                }
            };
            statsGrid.Add(distFrame, 0, 0);

            _coinsLabel = new Label { FontSize = 20, TextColor = Color.FromArgb(_appSettings.BoxColor) };
            var coinsFrame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Color.FromArgb(_appSettings.BoxColor),
                HasShadow = true,
                Content = new VerticalStackLayout
                {
                    Spacing = 5,
                    Padding = 15,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label { Text = "Total Coins", FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black },
                        _coinsLabel
                    }
                }
            };
            statsGrid.Add(coinsFrame, 1, 0);

            _gamesLabel = new Label { FontSize = 20, TextColor = Color.FromArgb(_appSettings.BoxColor) };
            var gamesFrame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Color.FromArgb(_appSettings.BoxColor),
                HasShadow = true,
                Content = new VerticalStackLayout
                {
                    Spacing = 5,
                    Padding = 15,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label { Text = "Games Played", FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black },
                        _gamesLabel
                    }
                }
            };
            statsGrid.Add(gamesFrame, 0, 1);
            statsGrid.SetColumnSpan(gamesFrame, 2);

            _mainStack.Add(statsGrid);

            // Reset Button
            var resetBtn = new Button
            {
                Text = "Reset Statistics",
                BackgroundColor = Color.FromArgb("#FF6B6B"),
                TextColor = Colors.White,
                CornerRadius = 5,
                Padding = new Thickness(10, 15)
            };
            resetBtn.Clicked += OnResetClicked;
            _mainStack.Add(resetBtn);

            Content = new ScrollView { Content = _mainStack };
        }

        private void UpdateUI()
        {
            int highScore = Preferences.Get("HighScore", 0);
            int totalDistance = Preferences.Get("TotalDistance", 0);
            int totalCoins = Preferences.Get("TotalCoins", 0);
            int gamesPlayed = Preferences.Get("GamesPlayed", 0);

            _highScoreLabel.Text = highScore.ToString();
            _distanceLabel.Text = totalDistance.ToString();
            _coinsLabel.Text = totalCoins.ToString();
            _gamesLabel.Text = gamesPlayed.ToString();
        }

        private async void OnResetClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Reset?", "Are you sure you want to reset all statistics?", "Yes", "No");
            if (result)
            {
                Preferences.Set("HighScore", 0);
                Preferences.Set("TotalDistance", 0);
                Preferences.Set("TotalCoins", 0);
                Preferences.Set("GamesPlayed", 0);
                UpdateUI();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _appSettings.Load();
            UpdateUI();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
