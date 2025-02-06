using LifeManagers.Data;

namespace TaxDashboard.Initialization;

public partial class InitializationPage : ContentPage
{
    private readonly IDatabaseInitializer _databaseInitializer;

    private string _loadingDetailsText = "...";

    public InitializationPage(IDatabaseInitializer databaseInitializer) : base()
    {
        Appearing += async (s, e) => await InitializeApplication();

        _databaseInitializer = databaseInitializer;
        databaseInitializer.StepExecuting += (s, e) => LoadingDetailsText = e;
    }

    public string LoadingDetailsText
    {
        get => _loadingDetailsText;
        set
        {
            if (_loadingDetailsText != value)
            {
                _loadingDetailsText = value;
                OnPropertyChanged(nameof(LoadingDetailsText));
            }
        }
    }

    public void ChangeDetailsText(string value)
    {
        LoadingDetailsText = value;
    }

    protected async Task InitializeApplication()
    {
        await _databaseInitializer.InitializeDatabaseAsync();
        NavigateToPage(new MainPage());
    }

    protected void NavigateToPage(Page page)
    {
        if (Application.Current is null)
            throw new InvalidOperationException($"{nameof(InitializationPage)} can only be used in maui application context");

        Application.Current!.Windows[0].Page = page;
    }
}