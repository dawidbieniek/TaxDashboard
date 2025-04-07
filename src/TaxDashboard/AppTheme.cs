using MudBlazor;

namespace TaxDashboard;
internal static class AppTheme
{
    private const string White = "rgba(255,255,255,0.9)";
    private const string Dark = "#151843";
    private const string DarkDarken = "#151843";

    public static MudTheme Theme = new()
    {
        PaletteDark = new()
        {
            DrawerBackground = Dark,
            DrawerText = White,
            DrawerIcon = White,
            AppbarBackground = Dark,
            AppbarText = White,
            Primary = "#52e2bf",
            Surface = "#1d2a54",
            Dark = Dark,
            DarkDarken = DarkDarken,
            LinesDefault = DarkDarken,
            Background = "#205f8a",
            TextPrimary = White,
            TextSecondary = "rgba(255,255,255,0.8)",
            
        },
        LayoutProperties = new()
        { 
            AppbarHeight = "48px",
            DrawerWidthLeft = "240px",
        }


    };
}
