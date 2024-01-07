
namespace Wordle_App;

public partial class LoadingPage : ContentPage
{
   
    public LoadingPage()
	{
		InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(2000);

        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
    }
}