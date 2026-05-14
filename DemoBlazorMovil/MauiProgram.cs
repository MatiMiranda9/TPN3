using DemoBlazorMovil.Services;
using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using DemoBlazorMovil.Services.Auth;

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

            builder.Services.AddTransient<AuthHeaderHandler>();

            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<AuthHeaderHandler>();

                handler.InnerHandler = new HttpClientHandler();

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://cineapi.runasp.net/api/")
                };
            });



            // Servicios
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<MovieService>();
            builder.Services.AddScoped<SalaService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ArticuloService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<VentaService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
