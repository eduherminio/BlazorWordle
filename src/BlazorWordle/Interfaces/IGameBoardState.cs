using BlazorWordle.Core.Enums;
using BlazorWordle.Core.ValueObjects;

namespace BlazorWordle.Interfaces;

public interface IGameBoardState
{
    #region Events

    event Action? OnStateChanged;

    #endregion

    #region State

    ICurrentWordState CurrentWord { get; }
    int AttemptsLeft { get; }
    IReadOnlyList<IReadOnlyList<Letter>> Board { get; }
    IReadOnlyDictionary<char, LetterState> LettersInUse { get; }
    string Solution { get; }
    GameStatus Status { get; }

    #endregion
}