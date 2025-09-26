using DemoBlazorMovil.Services;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
                    builder.Services.AddBlazorWebView();
                });

            builder.Services.AddMauiBlazorWebView();



            // 🔹 Configuración del HttpClient dependiendo de la plataforma
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://cineapi.runasp.net/api/")
            });


            // 🔹 Registrar servicios
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddScoped<AuthService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
