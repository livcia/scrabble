namespace randomWordGenerator.Game;

public partial class GamePage : ContentPage
{
    private readonly List<Letter> letters;
    private readonly List<Letter> userLetters;
    private int selectedLetterCount = 0;

    public GamePage()
    {
        InitializeComponent();
        letters = LetterInitializer.InitializeLetters();
        userLetters = LetterInitializer.DrawRandomLetters(letters, 7);
        var totalTiles = GetTotalTiles();
        //TotalTilesLabel.Text = $"Total Tiles: {totalTiles}";
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
            LettersStackLayout.Children.Remove(button);
            SelectedLettersGrid.Children.Add(button);

            // Add a new column definition for each selected letter
            SelectedLettersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Set the column for the button
            Grid.SetColumn(button, selectedLetterCount);

            // Animate the button
            await button.TranslateTo(0, -50, 250, Easing.CubicInOut);

            // Change the click event to handle returning the button
            button.Clicked -= OnLetterButtonClicked;
            button.Clicked += OnSelectedLetterButtonClicked;

            selectedLetterCount++;
        }
    }

    private async void OnSelectedLetterButtonClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Get the column index of the button
            int columnIndex = Grid.GetColumn(button);

            SelectedLettersGrid.Children.Remove(button);
            LettersStackLayout.Children.Add(button);

            // Remove the column definition for the returned letter
            SelectedLettersGrid.ColumnDefinitions.RemoveAt(columnIndex);

            // Update the column index for remaining buttons
            foreach (var child in SelectedLettersGrid.Children)
            {
                int currentColumn = Grid.GetColumn((BindableObject)child);
                if (currentColumn > columnIndex)
                {
                    Grid.SetColumn((BindableObject)child, currentColumn - 1);
                }
            }

            // Reset the column count
            selectedLetterCount--;

            // Change the click event back to handle selecting the button
            button.Clicked -= OnSelectedLetterButtonClicked;
            button.Clicked += OnLetterButtonClicked;

            // Animate the button back to its original position
            await button.TranslateTo(0, 0, 250, Easing.CubicInOut);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
