namespace Raymond_Sean_RushHour.Models
{
    // Application settings with persistence
    public class AppSettings
    {
        public bool DarkMode { get; set; }
        public int FontSize { get; set; }
        public string DifficultyMode { get; set; } // "Easy", "Medium", "Hard"
        public string BoxColor { get; set; }
        public int TimeLimit { get; set; } // in seconds, 0 = unlimited
        public int HighScore { get; set; }

        public AppSettings()
        {
            DarkMode = false;
            FontSize = 14;
            DifficultyMode = "Medium";
            BoxColor = "#512BD4";
            TimeLimit = 0;
            HighScore = 0;
        }

        // Save settings to Preferences
        public void Save()
        {
            Preferences.Set("DarkMode", DarkMode);
            Preferences.Set("FontSize", FontSize);
            Preferences.Set("DifficultyMode", DifficultyMode);
            Preferences.Set("BoxColor", BoxColor);
            Preferences.Set("TimeLimit", TimeLimit);
            Preferences.Set("HighScore", HighScore);
        }

        // Load settings from Preferences
        public void Load()
        {
            DarkMode = Preferences.Get("DarkMode", false);
            FontSize = Preferences.Get("FontSize", 14);
            DifficultyMode = Preferences.Get("DifficultyMode", "Medium");
            BoxColor = Preferences.Get("BoxColor", "#512BD4");
            TimeLimit = Preferences.Get("TimeLimit", 0);
            HighScore = Preferences.Get("HighScore", 0);
        }
    }
}
