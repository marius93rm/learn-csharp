using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace SalutoUtente;

public class App : Application
{
    public App()
    {
        /* Milestone 4: Impostazione della pagina principale dell'app */
        MainPage = new NavigationPage(new MainPage())
        {
            BarBackgroundColor = Colors.SteelBlue,
            BarTextColor = Colors.White
        };
    }
}
