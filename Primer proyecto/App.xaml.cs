namespace Primer_proyecto;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new Views.PageListPersonas());
    }
}