namespace Wordle_App
{
    public class AuthService
    {
        public async Task<bool> IsAuthenticatedAsync()
        {
            await Task.Delay(4000);
            return false;
        }
    }
}