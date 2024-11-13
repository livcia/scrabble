using System.Text;

namespace randomWordGenerator.Game;

public partial class GamePage : ContentPage
{
    private readonly GameLogic.GameLogic gameLogic;
    private readonly List<Letter> letters;
    private List<Letter> userLetters;
    private bool czyUsuwanie;
    private int selectedLetterCount;
    private int totalPoints;

    public GamePage()
    {
        InitializeComponent();
        letters = LetterInitializer.InitializeLetters();
        userLetters = LetterInitializer.DrawRandomLetters(letters, 7);
        gameLogic = new GameLogic.GameLogic(letters);
        var totalTiles = GetTotalTiles();
        DisplayUserLetters();
    }

    private int GetTotalTiles()
    {
        return letters.Sum(letter => letter.Quantity);
    }

    private void DisplayUserLetters()
    {
        //LettersStackLayout.Children.Clear();
        foreach (var letter in userLetters)
        {
            var button = CreateLetterButton(letter.Character);
            button.Clicked += OnLetterButtonClicked;
            LettersStackLayout.Children.Add(button);
        }
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

    private async void OnLetterButtonClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (!czyUsuwanie)
            {
                MoveButtonToSelectedLetters(button);
                await AnimateButton(button, 0, -50);
                button.Clicked -= OnLetterButtonClicked;
                button.Clicked += OnSelectedLetterButtonClicked;
            }
            else
            {
                ReplaceLetter(button);
                czyUsuwanie = false;
            }
        }
    }

    private async void OnSelectedLetterButtonClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (!czyUsuwanie)
            {
                MoveButtonToUserLetters(button);
                await AnimateButton(button, 0, 0);
                button.Clicked -= OnSelectedLetterButtonClicked;
                button.Clicked += OnLetterButtonClicked;
            }
            else
            {
                ReplaceLetter(button);
                czyUsuwanie = false;
            }
        }
    }

    private void MoveButtonToSelectedLetters(Button button)
    {
        LettersStackLayout.Children.Remove(button);
        SelectedLettersGrid.Children.Add(button);
        SelectedLettersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        Grid.SetColumn(button, selectedLetterCount);
        selectedLetterCount++;
    }

    private void MoveButtonToUserLetters(Button button)
    {
        var columnIndex = Grid.GetColumn(button);
        SelectedLettersGrid.Children.Remove(button);
        LettersStackLayout.Children.Add(button);
        SelectedLettersGrid.ColumnDefinitions.RemoveAt(columnIndex);

        foreach (var child in SelectedLettersGrid.Children)
        {
            var currentColumn = Grid.GetColumn((BindableObject)child);
            if (currentColumn > columnIndex) Grid.SetColumn((BindableObject)child, currentColumn - 1);
        }

        selectedLetterCount--;
    }

    private void ReplaceLetter(Button button)
    {
        LettersStackLayout.Children.Remove(button);
        var newButton = gameLogic.ExchangeLetter(button.Text[0]);
        LettersStackLayout.Children.Add(newButton);
        newButton.Clicked += OnLetterButtonClicked;
    }

    private async Task AnimateButton(Button button, double x, double y)
    {
        await button.TranslateTo(x, y, 250, Easing.CubicInOut);
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnChangeLetterClicked(object sender, EventArgs e)
    {
        var buttonsToMove = SelectedLettersGrid.Children.ToList();
        foreach (var view in buttonsToMove)
        {
            var button = (Button)view;
            await AnimateButton(button, 0, 0);
            SelectedLettersGrid.Children.Remove(button);
            LettersStackLayout.Children.Add(button);
            button.Clicked -= OnSelectedLetterButtonClicked;
            button.Clicked += OnLetterButtonClicked;
            gameLogic.AddLetterBackToList(button.Text[0]);
        }
        SelectedLettersGrid.ColumnDefinitions.Clear();
        selectedLetterCount = 0;
        czyUsuwanie = true;
    }

    private string GetSelectedLettersString()
    {
        var selectedLetters = new StringBuilder();
        foreach (var child in SelectedLettersGrid.Children)
            if (child is Button button)
                selectedLetters.Append(button.Text);
        return selectedLetters.ToString();
    }

    private async void OnConfirm(object sender, EventArgs e)
    {
        var selectedLettersString = GetSelectedLettersString();
        LoadingImage.IsVisible = true;
        var isWord = await gameLogic.IsWordAsync(selectedLettersString);
        if (isWord)
        {
            testo.Text = $"znalazlo slowo: {selectedLettersString}";
            totalPoints += selectedLettersString.Sum(c => gameLogic.GetPointsForCharacter(c));
            PointsLabel.Text = $"Punkty: {totalPoints}";
            ClearSelectedLetters();
            userLetters = LetterInitializer.DrawRandomLetters(letters, selectedLettersString.Length);
            DisplayUserLetters();
            Left.Text = "Pozostało liter: " + GetTotalTiles();
        }
        else
        {
            testo.Text = $"NIE znalazlo slowa: {selectedLettersString}";
        }
        LoadingImage.IsVisible = false;
    }

    private void ClearSelectedLetters()
    {
        var buttonsToMove = SelectedLettersGrid.Children.ToList();
        foreach (var view in buttonsToMove)
        {
            if (view is Button button)
            {
                SelectedLettersGrid.Children.Remove(button);
            }
        }
        SelectedLettersGrid.ColumnDefinitions.Clear();
        selectedLetterCount = 0;
    }
}
