using Microsoft.Maui;
using Microsoft.UI.Xaml;

namespace SalutoUtente.WinUI;

public partial class App : MauiWinUIApplication
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => SalutoUtente.MauiProgram.CreateMauiApp();
}
