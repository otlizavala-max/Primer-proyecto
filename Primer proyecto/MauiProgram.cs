using Microsoft.Extensions.Logging;
using Primer_proyecto.Services;
using Primer_proyecto.ViewModels;

namespace Primer_proyecto;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<DatabaseServices>();

        builder.Services.AddTransient<PageListPersonasViewModel>();
        builder.Services.AddTransient<PageAddPersonasViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}