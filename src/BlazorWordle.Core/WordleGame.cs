using BlazorWordle.Core.Enums;
using BlazorWordle.Core.ValueObjects;

namespace BlazorWordle.Core;

public sealed class WordleGame
{
    private readonly int _attemptsCount;
    private readonly IReadOnlyList<string> _allWords;
    private readonly List<IReadOnlyList<Letter>> _board;
    private readonly Dictionary<char, LetterState> _lettersInUse;

    public int AttemptsLeft => _attemptsCount - _board.Count;
    public GameStatus Status { get; private set; }
    public IReadOnlyList<IReadOnlyList<Letter>> Board => _board;
    public IReadOnlyDictionary<char, LetterState> LettersInUse => _lettersInUse;
    public string Solution { get; }

    public WordleGame(string solution, IReadOnlyList<string> allWords, int attemptsCount)
    {
        _allWords = allWords;
        _attemptsCount = attemptsCount;
        _board = new List<IReadOnlyList<Letter>>(_attemptsCount);
        _lettersInUse = new Dictionary<char, LetterState>();
        ArgumentNullException.ThrowIfNull(solution);
        Solution = solution.ToLowerInvariant();

        Status = GameStatus.InProgress;
    }

    public bool Submit(string word)
    {
        if (AttemptsLeft == 0)
        {
            throw new Exception("Game over");
        }

        if (word.Length != Solution.Length)
        {
            throw new ArgumentException($"Word length must be {Solution.Length}", nameof(word));
        }

        word = word.ToLowerInvariant();

        if (!_allWords.Contains(word))
        {
            return false;
        }

        var (matchCount, row) = Match(word);

        _board.Add(row);

        if (matchCount == Solution.Length)
        {
            Status = GameStatus.Win;
        }
        else if (AttemptsLeft == 0)
        {
            Status = GameStatus.Lose;
        }

        return true;
    }

    #region Private methods

    private (int matchCount, IReadOnlyList<Letter> row) Match(string word)
    {
        var count = 0;
        var row = new List<Letter>(word.Length);

        for (var i = 0; i < Solution.Length; i++)
        {
            var curChar = word[i];
            var state = LetterState.Absent;

            if (curChar == Solution[i])
            {
                state = LetterState.Correct;
                count++;
            }
            else if (Solution.Contains(curChar))
            {
                state = LetterState.Present;
            }

            UpdateLetterInUse(curChar, state);
            row.Add(new Letter(curChar, state));
        }

        return (count, row);
    }

    private void UpdateLetterInUse(char letter, LetterState state)
    {
        if (!_lettersInUse.ContainsKey(letter))
        {
            _lettersInUse[letter] = state;
            return;
        }

        var prevState = _lettersInUse[letter];

        if (state > prevState)
        {
            _lettersInUse[letter] = state;
        }
    }

    #endregion
}