using ApexCharts;

using Blazored.LocalStorage;

using Microsoft.Extensions.Logging;

namespace TaxDashboard
{
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.RegisterAppServices();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddApexCharts(opt =>
            {
                opt.GlobalOptions = new ApexChartBaseOptions()
                {
#if DEBUG
                    Debug = true,
#endif
                    Theme = new() { Palette = PaletteType.Palette1, Mode = Mode.Dark },
                    Chart = new() { Background = "transparent", },
                    PlotOptions = new() { RadialBar = new() { StartAngle = -135, EndAngle = 135, OffsetY = -35, } },
                };
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}