namespace GradeMaster.DesktopClient;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "GradeMaster", MinimumHeight = 600, MinimumWidth = 475 };
    }
}
