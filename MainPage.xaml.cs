namespace Wordle_App
{
    public partial class MainPage : ContentPage
    {
        //variables
        private int points = 0;
        private int currentRowIndex = 0;
        private int remainingAttempts = 6;

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
            InitializeNewRow();
            GetWords();
        }

        public async void GetWords()
        {
            //Get words from internet
            var response = await httpClient.GetStringAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
           //Splits into an array of individual words
            string[] individualWords = response.Split(new[] { '\n' });
            //adds individual words to list
            words.AddRange(individualWords);
            //random word
            chosenWord = PickWord();
            //shows word for testing purposes
            //RandomWordLabel.Text = chosenWord;
        }

        public string PickWord()
        {
            //checks if there are words in the list
            if (words.Count > 0)
            {
                int randomNumber = random.Next(0, words.Count);
                return words[randomNumber];
            }
            else
            {
                //if list is empty
                return null;
            }
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                //gets the text from the clicked button
                string buttonText = button.Text;

                if (buttonText.Equals("Back"))
                {
                    // Back button
                    HandleBackButton();
                }
                else if (buttonText.Equals("Submit"))
                {
                    //Submit button
                    HandleSubmitButton();
                }
                else
                {
                    // other buttons
                    HandleOtherButton(buttonText);
                }
            }
        }
        private void HandleOtherButton(string buttonText)
        {
            //adds button text to current row inputList
            userInputLists[currentRowIndex].Add(buttonText);

            // Create a frame for the new input
            var label = new Label
            {
                Text = buttonText,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromRgba(0, 0, 0, 0),
                TextColor = Color.FromRgb(0, 0, 0)
            };

            var frame = new Frame
            {
                BorderColor = Color.FromRgba(0, 0, 0, 0),
                BackgroundColor = Color.FromRgba(0, 0, 0, 0),
                Padding = 10,
                Margin = 2,
                Content = label
            };

            //sets the position of the frame
            Grid.SetRow(frame, currentRowIndex);
            Grid.SetColumn(frame, userInputLists[currentRowIndex].Count - 1);

            // Adds frame to lists and grid
            frameLists[currentRowIndex].Add(frame);
            Grid5.Children.Add(frame);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Handle Back button
            HandleBackButton();
        }

        private void HandleBackButton()
        {
            //checks if row index is within range
            if (currentRowIndex >= 0 && currentRowIndex < userInputLists.Count && userInputLists[currentRowIndex].Count > 0)
            {
                //removes last input from current row
                userInputLists[currentRowIndex].RemoveAt(userInputLists[currentRowIndex].Count - 1);

                if (frameLists[currentRowIndex].Count > 0)
                {
                    var lastFrame = frameLists[currentRowIndex].Last();
                    //remove last frame from grid
                    frameLists[currentRowIndex].Remove(lastFrame);
                    Grid5.Children.Remove(lastFrame);
                }
            }
        }
        
        private void submitButton(object sender, EventArgs e)
        {
            // Handle the Submit button click
            HandleSubmitButton();

            // Update the grid display
            UpdateGridDisplay();
        }

        private async void HandleSubmitButton()
        {
            // Check if user input matches chosen word
            string userGuess = string.Join("", userInputLists[currentRowIndex]);

            // Determine correct colors after user submits a guess
            UpdateColorsAfterSubmission(userGuess);

            if (userGuess.Equals(chosenWord, StringComparison.OrdinalIgnoreCase))
            {
                ConfirmationLbl.Text = "Correct!";
                await Task.Delay(1500);
                ConfirmationLbl.Text = string.Empty; // Clears message

                // Congratulatory Message
                DisplayAlert("Congratulations", "You guessed the word correctly!", "OK");

                //update points
                points++;
                PointsLabel.Text = $"Points: {points}";

                // Clear the grid, reset lists, and start a new game
                ClearGridAndReset();
            }
            else
            {
                // incorrect guess
                ConfirmationLbl.Text = "Incorrect, try again!";
                await Task.Delay(1500);
                ConfirmationLbl.Text = string.Empty; // Clears message
                remainingAttempts--;

                if (remainingAttempts == 0)
                {
                    // if user runs out of attempts
                    DisplayAlert("Game Over", $"You ran out of attempts. The correct word was {chosenWord}.", "OK");

                    ClearGridAndReset();
                }
                else
                {
                    // Increment the row index for the next word only if it's less than 6
                    if (currentRowIndex < 6)
                    {
                        currentRowIndex++;

                        // Add new lists for the next row
                        userInputLists.Add(new List<string>());
                        frameLists.Add(new List<Frame>());
                    }
                }
            }
        }

        private void InitializeNewRow()
        {
            // Add new lists for the first row
            userInputLists.Add(new List<string>());
            frameLists.Add(new List<Frame>());
        }

        private void UpdateGridDisplay()
        {
            // Clear existing frames
            foreach (var frameList in frameLists)
            {
                foreach (var frame in frameList)
                {
                    Grid5.Children.Remove(frame);
                }
            }

            // Display user input in the frames
            for (int i = 0; i < frameLists.Count; i++)
            {
                var currentFrameList = frameLists[i];
                for (int j = 0; j < currentFrameList.Count; j++)
                {
                    var frame = currentFrameList[j];
                    Grid.SetRow(frame, i);
                    Grid.SetColumn(frame, j);
                    Grid5.Children.Add(frame);
                }
            }
        }
        
        private void UpdateColorsAfterSubmission(string userGuess)
        {
            for (int i = 0; i < userGuess.Length; i++)
            {
                bool isInCorrectPosition = i < chosenWord.Length && userGuess[i].ToString().Equals(chosenWord[i].ToString(), StringComparison.OrdinalIgnoreCase);

                // Check if the letter is in the correct word at any position
                bool isInCorrectWord = chosenWord.IndexOf(userGuess[i], StringComparison.OrdinalIgnoreCase) != -1;

                Frame frame = frameLists[currentRowIndex][i];

                // Determine the background color based on the conditions
                if (isInCorrectPosition)
                {
                    frame.BackgroundColor = Color.FromRgb(66, 245, 123); // Correct letter in the correct position (green)
                }
                else if (isInCorrectWord)
                {
                    frame.BackgroundColor = Color.FromRgb(245, 227, 66); // Correct letter - incorrect position (yellow)
                }
                else
                {
                    frame.BackgroundColor = Color.FromRgb(115, 113, 98); //Not in word - (grey)
                }
            }
        }

        private void ClearGridAndReset()
        {
            // Clear the grid by removing all frames
            foreach (var frameList in frameLists)
            {
                foreach (var frame in frameList)
                {
                    Grid5.Children.Remove(frame);
                }
            }

            // Reset lists
            userInputLists.Clear();
            frameLists.Clear();

            // Add new lists for the first row
            userInputLists.Add(new List<string>());
            frameLists.Add(new List<Frame>());

            // Generate a new random word
            chosenWord = PickWord();
            //Display random word for testing purposes
            //RandomWordLabel.Text = chosenWord;

            // Update the grid display
            UpdateGridDisplay();

            // Reset the currentRowIndex to 0
            currentRowIndex = 0;

            remainingAttempts = 6;

            // Clear any confirmation messages
            ConfirmationLbl.Text = string.Empty;
        }
    }
}