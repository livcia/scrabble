using Microsoft.Maui.Controls;

namespace randomWordGenerator.Game;

public static class LetterInitializer
{
    public static List<Letter> InitializeLetters()
    {
        return new List<Letter>
        {
            new('A', 9, 1),
            new('Ą', 1, 5),
            new('B', 2, 3),
            new('C', 3, 2),
            new('Ć', 1, 6),
            new('D', 3, 2),
            new('E', 7, 1),
            new('Ę', 1, 5),
            new('F', 1, 5),
            new('G', 2, 3),
            new('H', 2, 3),
            new('I', 8, 1),
            new('J', 2, 3),
            new('K', 3, 2),
            new('L', 3, 2),
            new('Ł', 2, 3),
            new('M', 3, 2),
            new('N', 5, 1),
            new('Ń', 1, 7),
            new('O', 6, 1),
            new('Ó', 1, 5),
            new('P', 3, 2),
            new('R', 4, 1),
            new('S', 4, 1),
            new('Ś', 1, 5),
            new('T', 3, 2),
            new('U', 2, 3),
            new('W', 4, 1),
            new('Y', 4, 2),
            new('Z', 5, 1),
            new('Ź', 1, 9),
            new('Ż', 1, 5),
            new('_', 2, 0)
        };
    }

    public static List<Letter> DrawRandomLetters(List<Letter> letters, int count)
    {
        var random = new Random();
        var drawnLetters = new List<Letter>();

        for (var i = 0; i < count; i++)
        {
            if (letters.Count == 0)
                break;

            var index = random.Next(letters.Count);
            var letter = letters[index];

            drawnLetters.Add(new Letter(letter.Character, 1, letter.Points));
            letter.Quantity--;

            if (letter.Quantity == 0) letters.RemoveAt(index);
        }

        return drawnLetters;
    }
    public static Letter DrawRandomLetter(List<Letter> letters){
        var random = new Random();
        var index = random.Next(letters.Count);
        var letter = letters[index];
        letter.Quantity--;
        if (letter.Quantity == 0) letters.RemoveAt(index);
        return letter;
    }
}