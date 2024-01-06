
namespace Wordle_App;

public partial class LoadingPage : ContentPage
{
    private readonly AuthService _authService;
    public LoadingPage(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (await _authService.IsAuthenticatedAsync())
        {
            //user is logged in 
            //redirect to main page
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
        else
        {
            //user is not logges in
            //redirect to loading page
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}