using Microsoft.Extensions.Logging;

namespace Raymond_Sean_RushHour
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterPages();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<GamePage>();
            builder.Services.AddSingleton<ScorePage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<AppShell>();
            
            return builder;
        }
    }
}
