

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
    }

    private void ConfigureWebView2()
    {
        #if DEBUG
            var appName = "GradeMasterDev";
        #elif RELEASE
            var appName = "GradeMaster";
        #endif

        try
        {
            // Define a writable WebView2 User Data Folder
            string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "WebView2");

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
