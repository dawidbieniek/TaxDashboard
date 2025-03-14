using ApexCharts;

using BlazorTable;

using CommunityToolkit.Maui;

using LifeManagers.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MudBlazor.Services;

using TaxDashboard.Data;

namespace TaxDashboard
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Configuration
                .AddJsonFile("appsettings.json", optional:true)
                .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazorTable();
            builder.Services.AddMudServices();
            builder.Services.AddAppServices();
            builder.Services.AddDataServices<AppDbContext>(opt =>
            {
                opt.WithDataDirectoryPath(FileSystem.Current.AppDataDirectory)
                    .WithSeeder<AppDbContext, AppDbSeeder>()
                    .EnablePeriodicBackups(TimeSpan.FromHours(18), lastBackupFileName: "lastBackup");
#if DEBUG
                opt.UsingDebugMode();
#endif // DEBUG
            });

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