using Microsoft.AspNetCore.Components.WebView;

namespace DemoBlazorMovil
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private void BlazorWebView_Initialized(object sender, BlazorWebViewInitializedEventArgs e)
        {
#if WINDOWS
            
            var webview2 = e.WebView.CoreWebView2;
            if (webview2 != null)
            {
                webview2.OpenDevToolsWindow();
            }
#endif
        }
    }
}
