using Microsoft.UI.Xaml.Controls;

using YourGT.WinClient.ViewModels;

namespace YourGT.WinClient.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}
