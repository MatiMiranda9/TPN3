using DemoBlazorMovil.Services;
using Microsoft.Extensions.Logging;

namespace DemoBlazorMovil
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<MovieService>();
            builder.Services.AddSingleton<AuthService>();

            return builder.Build();
        }
    }
}
