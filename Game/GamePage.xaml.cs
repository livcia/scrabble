using System.Text;

namespace randomWordGenerator.Game;

public partial class GamePage : ContentPage
{
    private readonly GameLogic.GameLogic gameLogic;
    private readonly List<Letter> letters;
    private readonly List<Letter> userLetters;
    private bool czyUsuwanie;
    private int selectedLetterCount;


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
        LettersStackLayout.Children.Clear();
        foreach (var letter in userLetters)
        {
            var button = new Button
            {
                Text = letter.Character.ToString(),
                FontSize = 24,
                WidthRequest = 50,
                HeightRequest = 50,
                Margin = new Thickness(5)
            };
            button.Clicked += OnLetterButtonClicked;
            LettersStackLayout.Children.Add(button);
        }
    }

    private async void OnLetterButtonClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (!czyUsuwanie)
            {
                LettersStackLayout.Children.Remove(button);
                SelectedLettersGrid.Children.Add(button);
                SelectedLettersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                Grid.SetColumn(button, selectedLetterCount);

                await button.TranslateTo(0, -50, 250, Easing.CubicInOut);

                button.Clicked -= OnLetterButtonClicked;
                button.Clicked += OnSelectedLetterButtonClicked;

                selectedLetterCount++;
            }
            else
            {
                LettersStackLayout.Children.Remove(button);
                var btn = gameLogic.ExchangeLetter(button.Text[0]);
                LettersStackLayout.Children.Add(btn);
                btn.Clicked += OnLetterButtonClicked;
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

                button.Clicked -= OnSelectedLetterButtonClicked;
                button.Clicked += OnLetterButtonClicked;

                await button.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                SelectedLettersGrid.Children.Remove(button);
                var btn = gameLogic.ExchangeLetter(button.Text[0]);
                LettersStackLayout.Children.Add(btn);
                btn.Clicked += OnLetterButtonClicked;
                czyUsuwanie = false;
            }
        }
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
            await button.TranslateTo(0, 0, 250, Easing.CubicInOut);

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
        var isWord = await gameLogic.IsWordAsync(selectedLettersString);
        if (isWord)
        {
            testo.Text = "znalazlo slowo";
        }
        else
        {
            testo.Text = "NIE znalazlo slowa";
        }
    }
}