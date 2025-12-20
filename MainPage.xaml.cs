namespace Raymond_Sean_RushHour
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BuildUI();
            LoadStats();
        }

        private void BuildUI()
        {
            Title = "Rush Hour";
            BackgroundColor = Color.FromArgb("#87CEEB");

            var mainStack = new VerticalStackLayout
            {
                Padding = 30,
                Spacing = 30,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            // Title
            mainStack.Add(new Label
            {
                Text = "RUSH HOUR",
                FontSize = 48,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });

            mainStack.Add(new Label
            {
                Text = "Endless Driving Game",
                FontSize = 18,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            });

            // Play Button
            var playBtn = new Button
            {
                Text = "Play Game",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                Padding = new Thickness(20, 15),
                CornerRadius = 10,
                BackgroundColor = Color.FromArgb("#FF6B6B"),
                TextColor = Colors.White
            };
            playBtn.Clicked += OnPlayClicked;
            mainStack.Add(playBtn);

            // Stats Border
            var statsStack = new VerticalStackLayout { Spacing = 10 };
            statsStack.Add(new Label
            {
                Text = "Your Stats",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center
            });

            var highScoreLabel = new Label
            {
                Text = "High Score: 0",
                FontSize = 16,
                TextColor = Colors.White
            };

            var gamesPlayedLabel = new Label
            {
                Text = "Games Played: 0",
                FontSize = 16,
                TextColor = Colors.White
            };

            statsStack.Add(highScoreLabel);
            statsStack.Add(gamesPlayedLabel);

            var statsBorder = new Border
            {
                Stroke = Color.FromArgb("#4ECDC4"),
                StrokeThickness = 2,
                Padding = 20,
                Content = statsStack
            };

            mainStack.Add(statsBorder);

            Content = new ScrollView { Content = mainStack };
        }

        private void LoadStats()
        {
            int highScore = Preferences.Get("HighScore", 0);
            int gamesPlayed = Preferences.Get("GamesPlayed", 0);
        }

        private async void OnPlayClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("GamePage");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadStats();
        }
    }
}
