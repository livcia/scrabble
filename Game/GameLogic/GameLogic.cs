using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace randomWordGenerator.Game.GameLogic;

public class GameLogic(List<Letter> letters)
{
    public Button ExchangeLetter(char btnletter)
    {
        var found = false;
        foreach (var letter in letters)
            if (letter.Character == btnletter)
            {
                found = true;
                letter.Quantity++;
                break;
            }

        if (!found) letters.Add(new Letter(btnletter, 1, GetPointsForCharacter(btnletter)));

        var letterToAdd = LetterInitializer.DrawRandomLetter(letters);

        var button = new Button
        {
            Text = letterToAdd.Character.ToString(),
            FontSize = 24,
            WidthRequest = 50,
            HeightRequest = 50,
            Margin = new Thickness(5)
        };
        return button;
    }

    public void AddLetterBackToList(char character)
    {
        var existingLetter = letters.FirstOrDefault(l => l.Character == character);
        if (existingLetter != null)
        {
            existingLetter.Quantity++;
        }
        else
        {
            var points = GetPointsForCharacter(character);
            letters.Add(new Letter(character, 1, points));
        }
    }

    public int GetPointsForCharacter(char character)
    {
        return character switch
        {
            'A' => 1,
            'Ą' => 5,
            'B' => 3,
            'C' => 2,
            'Ć' => 6,
            'D' => 2,
            'E' => 1,
            'Ę' => 5,
            'F' => 5,
            'G' => 3,
            'H' => 3,
            'I' => 1,
            'J' => 3,
            'K' => 2,
            'L' => 2,
            'Ł' => 3,
            'M' => 2,
            'N' => 1,
            'Ń' => 7,
            'O' => 1,
            'Ó' => 5,
            'P' => 2,
            'R' => 1,
            'S' => 1,
            'Ś' => 5,
            'T' => 2,
            'U' => 3,
            'W' => 1,
            'Y' => 2,
            'Z' => 1,
            'Ź' => 9,
            'Ż' => 5,
            '_' => 0,
            _ => 0
        };
    }

    public async Task<bool> IsWordAsync(string word)
    {
        var firstLetter = word[0];
        var length = word.Length;

        var totalWords = await FetchTotalWordsAsync($"https://scrabblemania.pl/słowa-na-literę-{firstLetter}");
        var totalPages = (int)Math.Ceiling(totalWords / 500.0);

        return await FetchWordsFromLinksAsync(firstLetter, length, totalPages, word.ToUpper());
    }

    private async Task<int> FetchTotalWordsAsync(string url)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);

        var wordsHelper = htmlDoc.DocumentNode.SelectNodes("//p")
            ?.Select(p => p.InnerText.Trim())
            .Where(text => !string.IsNullOrEmpty(text) && text[0] == '7')
            .ToList();

        var regex = new Regex(@"\(.*?\)");
        foreach (var number in wordsHelper)
        {
            var match = regex.Match(number);
            if (match.Success)
            {
                var extractedNumber = match.Value.Trim('(', ')');
                if (int.TryParse(extractedNumber, out var totalWords))
                {
                    return totalWords;
                }
            }
        }

        return 0;
    }

    private async Task<bool> FetchWordsFromLinksAsync(char firstLetter, int length, int totalPages, string wordToFind)
    {
        using var httpClient = new HttpClient();

        for (int page = 1; page <= totalPages; page++)
        {
            var url = page == 1
                ? $"https://scrabblemania.pl/słowa-na-literę-{firstLetter}/{length}-literowe"
                : $"https://scrabblemania.pl/słowa-na-literę-{firstLetter}/{length}-literowe/strona-{page}";

            var response = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var pageWords = htmlDoc.DocumentNode.SelectNodes("//a")
                ?.Select(a => a.InnerText.Trim().ToUpper())
                .Where(text => !string.IsNullOrEmpty(text) && text.Length == length && text[0] == firstLetter);

            if (pageWords != null)
            {
                foreach (var word in pageWords)
                {
                    if (word == wordToFind)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}