using TaxDashboard.Initialization;

namespace TaxDashboard
{
    public partial class App : Application
    {
        private readonly Page? _startingPage;

        public App(InitializationPage startingPage)
        {
            _startingPage = startingPage;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_startingPage ?? new MainPage()) { Title = "TaxDashboard" };
        }
    }
}