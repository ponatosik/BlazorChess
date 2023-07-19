using Microsoft.AspNetCore.Components;
using ChessDotCore.Engine.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace BlazorChess.Client.Chess;

public abstract class GameInput : ComponentBase
{
    [CascadingParameter, AllowNull]
    public ChessContext Context { get; set; }
	[Parameter]
	public ChessSide InputForSide { get; set; }

	public abstract Task MakeTurn();

	protected bool IsMyTurn => Context.IsGameStarted && InputForSide switch
	{
		ChessSide.Black => Context.Board!.Turn == Color.Black,
		ChessSide.White => Context.Board!.Turn == Color.White,
		_ => false
	};
	protected override void OnInitialized()
	{
		Console.WriteLine(InputForSide);
		base.OnInitialized();
	}
}
