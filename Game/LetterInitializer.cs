namespace randomWordGenerator.Game;

public static class LetterInitializer
{
    public static List<Letter> InitializeLetters()
    {
        return new List<Letter>
            {
                new Letter('A', 9, 1),
                new Letter('Ą', 1, 5),
                new Letter('B', 2, 3),
                new Letter('C', 3, 2),
                new Letter('Ć', 1, 6),
                new Letter('D', 3, 2),
                new Letter('E', 7, 1),
                new Letter('Ę', 1, 5),
                new Letter('F', 1, 5),
                new Letter('G', 2, 3),
                new Letter('H', 2, 3),
                new Letter('I', 8, 1),
                new Letter('J', 2, 3),
                new Letter('K', 3, 2),
                new Letter('L', 3, 2),
                new Letter('Ł', 2, 3),
                new Letter('M', 3, 2),
                new Letter('N', 5, 1),
                new Letter('Ń', 1, 7),
                new Letter('O', 6, 1),
                new Letter('Ó', 1, 5),
                new Letter('P', 3, 2),
                new Letter('R', 4, 1),
                new Letter('S', 4, 1),
                new Letter('Ś', 1, 5),
                new Letter('T', 3, 2),
                new Letter('U', 2, 3),
                new Letter('W', 4, 1),
                new Letter('Y', 4, 2),
                new Letter('Z', 5, 1),
                new Letter('Ź', 1, 9),
                new Letter('Ż', 1, 5),
                new Letter('_',2,0)
            };
    }
    public static List<Letter> DrawRandomLetters(List<Letter> letters, int count)
    {
        Random random = new Random();
        List<Letter> drawnLetters = new List<Letter>();

        for (int i = 0; i < count; i++)
        {
            if (letters.Count == 0)
                break;

            int index = random.Next(letters.Count);
            Letter letter = letters[index];

            drawnLetters.Add(new Letter(letter.Character, 1, letter.Points));
            letter.Quantity--;

            if (letter.Quantity == 0)
            {
                letters.RemoveAt(index);
            }
        }

        return drawnLetters;
    }
}
