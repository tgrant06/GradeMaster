using Microsoft.UI.Xaml.Controls;

using YourGT.WinClient.ViewModels;

namespace YourGT.WinClient.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
