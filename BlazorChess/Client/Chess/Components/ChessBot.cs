using BlazorChess.Shared;
using ChessDotCore.Engine.Interfaces;

namespace BlazorChess.Client.Chess.Components;

public class ChessBot : GameInput
{
	private Random random = new();

	public async Task<bool> TryMakeMove()
	{
		if (!IsMyTurn) return false;
		if (Context.Board!.IsGameOver()) return false;

		IMove[] possibleMoves = Context.Board!.LegalMoves.ToArray();
		IMove randomMove = possibleMoves[random.Next(possibleMoves.Length)];

		Context.SelectedSquare = randomMove.FromSquare.GetPoint();
		Context.ContextChanged();
		await Task.Delay(250);

		Context.SelectedSquare = randomMove.ToSquare.GetPoint();
		Context.Game!.Move(randomMove);
		Context.MoveMade(InputForSide, randomMove.ToString()!);
		return true;
	}

	public override Task MakeTurn()
	{
		return TryMakeMove();
	}
}