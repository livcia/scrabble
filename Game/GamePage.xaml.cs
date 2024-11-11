using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace randomWordGenerator.Game;

public partial class GamePage : ContentPage
{
    List<Letter> letters;
    List<Letter> userLetters;

    public GamePage()
    {
        InitializeComponent();
        letters = LetterInitializer.InitializeLetters();
        userLetters = LetterInitializer.DrawRandomLetters(letters, 7);
        int totalTiles = GetTotalTiles();
        TotalTilesLabel.Text = $"Total Tiles: {totalTiles}";
        DisplayUserLetters();
    }

    private int GetTotalTiles()
    {
        return letters.Sum(letter => letter.Quantity);
    }

    private void DisplayUserLetters()
    {
        YourLetters.Text = string.Join(", ", userLetters.Select(letter => letter.Character));
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
