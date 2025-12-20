using Raymond_Sean_RushHour.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Raymond_Sean_RushHour
{
    public partial class SettingsPage : ContentPage, INotifyPropertyChanged
    {
        private AppSettings _appSettings;
        private Switch _darkModeSwitch;
        private Slider _fontSizeSlider;
        private Slider _timeLimitSlider;
        private Picker _difficultyPicker;
        private Label _fontSizeLabel;
        private Label _timeLimitLabel;
        private ScrollView _scrollView;

        public SettingsPage()
        {
            _appSettings = new AppSettings();
            _appSettings.Load();

            BuildUI();
        }

        private void BuildUI()
        {
            Title = "Settings";
            BackgroundColor = _appSettings.DarkMode ? Colors.Black : Colors.White;

            var mainStack = new VerticalStackLayout { Padding = 20, Spacing = 20 };

            // Theme Section
            mainStack.Add(new Label
            {
                Text = "Theme",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            });

            _darkModeSwitch = new Switch { IsToggled = _appSettings.DarkMode };
            _darkModeSwitch.Toggled += OnDarkModeToggled;

            mainStack.Add(new HorizontalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(0, 10),
                Children =
                {
                    new Label
                    {
                        Text = "Dark Mode:",
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = _appSettings.FontSize,
                        FontFamily = "BBHBogle",
                        TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
                    },
                    _darkModeSwitch
                }
            });

            mainStack.Add(new BoxView { Color = Color.FromArgb("#CCCCCC"), HeightRequest = 1 });

            // Font Size Section
            mainStack.Add(new Label
            {
                Text = "Font Size",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            });

            _fontSizeLabel = new Label
            {
                Text = $"Font Size: {_appSettings.FontSize}px",
                FontSize = _appSettings.FontSize,
                FontFamily = "BBHBogle",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            };
            mainStack.Add(_fontSizeLabel);

            _fontSizeSlider = new Slider { Minimum = 10, Maximum = 24, Value = _appSettings.FontSize };
            _fontSizeSlider.ValueChanged += OnFontSizeChanged;
            mainStack.Add(_fontSizeSlider);

            mainStack.Add(new BoxView { Color = Color.FromArgb("#CCCCCC"), HeightRequest = 1 });

            // Difficulty Section
            mainStack.Add(new Label
            {
                Text = "Difficulty Mode",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            });

            _difficultyPicker = new Picker
            {
                Title = "Select Difficulty",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black,
                BackgroundColor = _appSettings.DarkMode ? Colors.Black : Colors.White
            };
            _difficultyPicker.ItemsSource = new List<string> { "Easy", "Medium", "Hard" };
            _difficultyPicker.SelectedItem = _appSettings.DifficultyMode;
            _difficultyPicker.SelectedIndexChanged += OnDifficultyChanged;
            mainStack.Add(_difficultyPicker);

            mainStack.Add(new BoxView { Color = Color.FromArgb("#CCCCCC"), HeightRequest = 1 });

            // Time Limit Section
            mainStack.Add(new Label
            {
                Text = "Time Limit (seconds)",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "BBHBogle",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            });

            _timeLimitLabel = new Label
            {
                Text = _appSettings.TimeLimit == 0 ? "Time Limit: Unlimited" : $"Time Limit: {_appSettings.TimeLimit}s",
                FontSize = _appSettings.FontSize,
                FontFamily = "BBHBogle",
                TextColor = _appSettings.DarkMode ? Colors.White : Colors.Black
            };
            mainStack.Add(_timeLimitLabel);

            _timeLimitSlider = new Slider { Minimum = 0, Maximum = 300, Value = _appSettings.TimeLimit };
            _timeLimitSlider.ValueChanged += OnTimeLimitChanged;
            mainStack.Add(_timeLimitSlider);

            mainStack.Add(new BoxView { Color = Color.FromArgb("#CCCCCC"), HeightRequest = 1 });

            // Save Button
            var saveBtn = new Button
            {
                Text = "Save Settings",
                BackgroundColor = Color.FromArgb("#4ECDC4"),
                TextColor = Colors.White,
                CornerRadius = 5,
                Padding = new Thickness(10, 15),
                FontFamily = "BBHBogle",
                FontAttributes = FontAttributes.Bold,
                FontSize = 16
            };
            saveBtn.Clicked += OnSaveClicked;
            mainStack.Add(saveBtn);

            _scrollView = new ScrollView { Content = mainStack };
            Content = _scrollView;
        }

        private void OnDarkModeToggled(object sender, ToggledEventArgs e)
        {
            _appSettings.DarkMode = e.Value;
            BackgroundColor = _appSettings.DarkMode ? Colors.Black : Colors.White;
            // Rebuild UI to apply dark mode changes
            BuildUI();
        }

        private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
        {
            _appSettings.FontSize = (int)e.NewValue;
            _fontSizeLabel.Text = $"Font Size: {(int)e.NewValue}px";
            _fontSizeLabel.FontSize = _appSettings.FontSize;
        }

        private void OnTimeLimitChanged(object sender, ValueChangedEventArgs e)
        {
            int timeLimit = (int)e.NewValue;
            _timeLimitLabel.Text = timeLimit == 0 ? "Time Limit: Unlimited" : $"Time Limit: {timeLimit}s";
            _appSettings.TimeLimit = timeLimit;
        }

        private void OnDifficultyChanged(object sender, EventArgs e)
        {
            if (_difficultyPicker.SelectedItem is string difficulty)
            {
                _appSettings.DifficultyMode = difficulty;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _appSettings.Save();
            await DisplayAlert("Success", "Settings saved successfully!", "OK");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _appSettings.Load();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
