namespace BlazorWordle.Clients;

public sealed class WordsClient
{
    private readonly HttpClient _client;

    public WordsClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<string>> GetWordsAsync()
    {
        return (await _client.GetStringAsync("assets/words.txt"))
            .ToLowerInvariant()
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string GetRandomWord(IReadOnlyList<string> words)
    {
        var word = words[Random.Shared.Next(0, words.Count)];

        if (string.IsNullOrEmpty(word))
        {
            throw new Exception("Word not found.");
        }

        return word;
    }
}