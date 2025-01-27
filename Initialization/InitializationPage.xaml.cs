using TaxDashboard.Models;

namespace TaxDashboard.Initialization;

public partial class InitializationPage : ContentPage
{
    private readonly DatabaseInitializer _initializer;
    private string _loadingDetailsText = "...";

    public InitializationPage(DatabaseInitializer initializer)
    {
        _initializer = initializer;
        initializer.StepExecuting += (s, e) => LoadingDetailsText = e;

        Appearing += async (s, e) => await InitializeApplication();

        InitializeComponent();
    }

    public string LoadingDetailsText
    {
        get => _loadingDetailsText; set
        {
            if (_loadingDetailsText !=
    value) { _loadingDetailsText = value; OnPropertyChanged(nameof(LoadingDetailsText)); }
        }
    }

    public void ChangeDetailsText(string value)
    { LoadingDetailsText = value; }

    private async Task InitializeApplication()
    {
        await _initializer.InitializeDatabaseAsync();
        Application.Current!.Windows[0].Page = new MainPage();
    }
}