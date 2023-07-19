using ChessDotCore.Engine.Interfaces;
using System.Drawing;

namespace BlazorChess.Client.Chess;

public class ChessContext
{
    public event Action? OnContextChanged;
    public event Action<ChessSide, string>? OnMoveMade;

    public IGame? Game { get; set; }
    public Point SelectedSquare { get; set; }
    public IPiece? SelectedPiece { get; set; }
    public ChessSide? PlayerSide { get; set; }

    public bool IsGameStarted => Game is not null;
    public IBoard? Board => Game?.Board;

    public void ContextChanged()
    {
        OnContextChanged?.Invoke();
    }
    public void MoveMade(ChessSide sideMadeTurn, string moveUci)
    {
        ContextChanged();
        OnMoveMade?.Invoke(sideMadeTurn, moveUci);
    }
}
