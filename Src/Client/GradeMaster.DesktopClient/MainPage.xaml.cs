using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Web.WebView2.Core;

namespace GradeMaster.DesktopClient;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        #if WINDOWS
            ConfigureWebView2();
        #endif

        #region Not used

        //string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GradeMaster", "WebView2");

        //if (!Directory.Exists(userDataFolder))
        //{
        //    Directory.CreateDirectory(userDataFolder);
        //}

        //blazorWebView.BlazorWebViewInitialized += async (sender, e) =>
        //{
        //    var env = await CoreWebView2Environment.CreateAsync(userDataFolder);
        //    await e.WebView.EnsureCoreWebView2Async(env);
        //};

        /* in the method:
        public void ConfigureWebView2() 
        {
            ...
           
            // not necessary 
            //Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "0");

                ...

                try 
                {
                    ...
                    
                    //e.WebView.CanGoForward = false; not used (maybe add for more native feel)
                } 
                catch 
                {
                    ...
                }
                ...
        }
        */
        #endregion
    }

    private void ConfigureWebView2()
    {
        #if DEBUG
            const string appName = "GradeMasterDev";
        #elif RELEASE
            const string appName = "GradeMaster";
        #endif

        try
        {
            // Define a writable WebView2 User Data Folder
            var userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "WebView2");

            if (!Directory.Exists(userDataFolder))
            {
                Directory.CreateDirectory(userDataFolder);
            }

            // Set the WebView2 user data folder before it initializes
            Environment.SetEnvironmentVariable("WEBVIEW2_USER_DATA_FOLDER", userDataFolder, EnvironmentVariableTarget.Process);

            blazorWebView.BlazorWebViewInitialized += async (sender, e) =>
            {
                try
                {
                    await e.WebView.EnsureCoreWebView2Async(); // No parameters needed

                    // change the BackgroundColor of the WebView
                    const byte zero = 0;
                    e.WebView.DefaultBackgroundColor = Windows.UI.Color.FromArgb(zero, zero, zero, zero); // Change to any color (in this case Transparent)
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            };
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
}
