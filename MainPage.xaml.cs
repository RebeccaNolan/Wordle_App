namespace Wordle_App
{
    public partial class MainPage : ContentPage
    {
        private int points = 0;

        private List<List<string>> userInputLists = new List<List<string>>();
        private List<List<Frame>> frameLists = new List<List<Frame>>();

        //To get words from GitHub
        private List<string> words = new List<string>();
        private HttpClient httpClient = new HttpClient();
        private Random random = new Random();
        public string chosenWord { get; set; }


        public MainPage()
        {
            InitializeComponent();
            GetWords();
        }

        public async void GetWords()
        {
            var response = await httpClient.GetStringAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
            string[] individualWords = response.Split(new[] { '\n' });
            words.AddRange(individualWords);
            chosenWord = PickWord();
            RandomWordLabel.Text = chosenWord;
        }

        public string PickWord()
        {
            if (words.Count > 0)
            {
                int randomNumber = random.Next(0, words.Count);
                return words[randomNumber];
            }
            else
            {
                return null;
            }
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {

        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {

        }

        private void submitButton(object sender, EventArgs e)
        {

        }
    }
}