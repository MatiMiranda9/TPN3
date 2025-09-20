using Android.App;
using Android.Content.PM;
using Android.OS;

namespace DemoBlazorMovil
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // 🔧 Esto fuerza a usar el motor Chromium actualizado
            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
        }
    }
}
