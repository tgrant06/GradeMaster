using Microsoft.UI.Windowing;
using Windows.Graphics;

namespace GradeMaster.DesktopClient;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    #region Not used

    // was like this before
    //protected override Window CreateWindow(IActivationState? activationState)
    //{
    //    return new Window(new MainPage()) { Title = "GradeMaster", MinimumHeight = 640, MinimumWidth = 500 };
    //}

    #endregion

    // new implementation
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage())
        {
            Title = "GradeMaster",
            MinimumHeight = 680,
            MinimumWidth = 500
        };

        #if WINDOWS
        window.Created += (s, e) =>
        {
            var mauiWindow = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;

            if (mauiWindow != null)
            {
                // Get the native window handle
                var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(mauiWindow);

                // Get the WindowId for the AppWindow
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);

                // Get the AppWindow
                var appWindow = AppWindow.GetFromWindowId(windowId);

                if (appWindow != null)
                {
                    // Get the display area where the window is located
                    var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);

                    if (displayArea != null)
                    {
                        // Get the working area (screen dimensions)
                        var workArea = displayArea.WorkArea;

                        // Define the window's width and height
                        int windowWidth = workArea.Width / 4 * 3; // Replace with your preferred width
                        int windowHeight = workArea.Height / 4 * 3; // Replace with your preferred height
                        // Initially it would also be like minus 100 to be the same dimensions as without setting position. Maybe change this later

                        // Calculate the centered position
                        int x = (workArea.Width - windowWidth) / 2 + workArea.X;
                        int y = (workArea.Height - windowHeight) / 2 + workArea.Y;

                        // Resize and move the window to the center
                        appWindow.Resize(new SizeInt32(windowWidth, windowHeight));
                        appWindow.Move(new PointInt32(x, y));
                    }
                }
            }
        };
        #endif

        return window;
    }
}
