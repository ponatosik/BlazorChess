using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;
using BlazorChess.Shared;
using System.Drawing;

namespace BlazorChess.Client.Chess.Components;

public partial class MouseInput
{
	[Parameter, AllowNull]
	public RenderFragment ChildContent { get; set; }

	private Point _selectedSqure;
	private Point _discardSquare = new Point(-1, -1);

	private bool MirrorInput => Context.PlayerSide == ChessSide.Black;
	private TaskCompletionSource<bool>? _didPlayerMakeTurn;

	private void SelectPiece()
	{
		if (!IsMyTurn) return;

		var targetPiece = Context.Game!.Board.GetPieceAtPoint(Context.SelectedSquare);
		if (Context.Game.Board.Turn == targetPiece?.Color)
			Context.SelectedPiece = targetPiece;


		Context.ContextChanged();
	}

	private void TryMakeMove()
	{
		if (!IsMyTurn) return;
		if (Context.SelectedPiece is null) return;
		if (!Context.Game!.Board.IsValidMove(Context.SelectedPiece.Square.GetPoint(), Context.SelectedSquare)) return;

		var move = Context.Board!.GetMoveFromCoodrinates(Context.SelectedPiece.Square.GetPoint(), Context.SelectedSquare);

		Context.SelectedSquare = _discardSquare;
		Context.SelectedPiece = null;

		Context.Game.Move(move);
		_didPlayerMakeTurn?.SetResult(true);
		Context.MoveMade(InputForSide, move.ToString()!);
	}

	private void SelectSquare(MouseEventArgs eventArgs)
	{
		if (!IsMyTurn) return;
		_selectedSqure.X = (int)eventArgs.OffsetX / 100;
		_selectedSqure.Y = MirrorInput ? (int)(eventArgs.OffsetY / 100) : 7 - (int)(eventArgs.OffsetY / 100);

		if (Context.SelectedSquare == _selectedSqure) return;
		Context.SelectedSquare = _selectedSqure;

		Context.ContextChanged();
	}


	public override Task MakeTurn()
	{
		// Returns task to await untill user makes move with mouse
		_didPlayerMakeTurn = new TaskCompletionSource<bool>();
		return _didPlayerMakeTurn.Task;
	}
}