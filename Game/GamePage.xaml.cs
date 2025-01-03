using System.Diagnostics;

namespace randomWordGenerator.Game;

public partial class GamePage : ContentPage
{
    private readonly GameLogic.GameLogic gameLogic;
    private readonly List<Letter> letters;
    private List<Letter> userLetters;
    private bool czyUsuwanie;
    private int totalPoints;
    private Button? currentlySelectedButton;
    private ScrabbleBoard scrabbleBoard;

    public GamePage()
    {
        InitializeComponent();

        letters = LetterInitializer.InitializeLetters();
        userLetters = LetterInitializer.DrawRandomLetters(letters, 7);
        gameLogic = new GameLogic.GameLogic(letters);
        DisplayUserLetters();
        scrabbleBoard = new ScrabbleBoard();
        scrabbleBoard.ClearChoosenLetterRequested += OnClearChoosenLetterRequested;
        ScrabbleBoardContainer.Children.Add(scrabbleBoard.BoardGrid);
    }

    private void DisplayUserLetters()
    {
        foreach (var letter in userLetters)
        {
            var button = CreateLetterButton(letter.Character);
            button.Clicked += OnLetterButtonClicked;
            LettersStackLayout.Children.Add(button);
        }
    }

    private void OnLetterButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            scrabbleBoard.LetterToPut = button.Text;
            LettersStackLayout.Children.Remove(button);

            if (ChoosenLetter.Children.Count > 0)
            {
                var previousButton = ChoosenLetter.Children[0] as Button;
                if (previousButton != null)
                {
                    ChoosenLetter.Children.Remove(previousButton);
                    LettersStackLayout.Children.Add(previousButton);
                }
            }

            ChoosenLetter.Children.Add(button);
        }
    }

    private void OnClearChoosenLetterRequested(object sender, EventArgs e)
    {
        ChoosenLetter.Children.Clear();
    }

    private Button CreateLetterButton(char character)
    {
        return new Button
        {
            Text = character.ToString(),
            FontSize = 24,
            WidthRequest = 50,
            HeightRequest = 50,
            Margin = new Thickness(5)
        };
    }
}