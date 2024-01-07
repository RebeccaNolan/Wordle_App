namespace Wordle_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await Task.Delay(2000);

            //Navigate to MainPage
            MainPage = new AppShell();
        }
    }
}