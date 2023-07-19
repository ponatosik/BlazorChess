using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using BlazorChess.Shared;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

namespace BlazorChess.Client.Chess.Components;

public partial class ChessRenderer
{
	[Parameter, EditorRequired, AllowNull]
	public ChessSpritesheet Spritesheet { get; set; }
	[CascadingParameter, AllowNull]
	public ChessContext Context { get; set; }

	[AllowNull]
	private BECanvasComponent canvasComponent;
	[AllowNull]
	private Canvas2DContext canvas;

	public bool IsInitialized => canvas is not null;
	private TaskCompletionSource<bool> _wasInitialized = new();
	private bool DrawMirrored => Context.PlayerSide == ChessSide.Black;

	protected async override Task OnInitializedAsync()
	{
		Context.OnContextChanged += async () => await RenderChessAsync();
		await base.OnInitializedAsync();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;
		canvas = await canvasComponent.CreateCanvas2DAsync();
		_wasInitialized.SetResult(true);
		await base.OnAfterRenderAsync(firstRender);
	}

	public Task WaitInitialization()
	{
		return _wasInitialized.Task;
	}

	public async Task RenderChessAsync()
	{
		await canvas.ClearRectAsync(0, 0, 800, 800);
		await RenderBoardAsync();
		await RenderSelectedPieceActionsAsync();
		await RenderPiecesAsync();
	}

	private async Task RenderBoardAsync()
	{
		await canvas.SetFillStyleAsync("white");
		await canvas.FillAsync();

		await canvas.SetFillStyleAsync("grey");
		for (int i = 0; i < 8; i++)
			for (int j = 0; j < 8; j++)
				if ((i + j) % 2 == (DrawMirrored ? 0 : 1))
					await canvas.FillRectAsync(i * 100, j * 100, 100, 100);
	}

	private async Task RenderSelectedPieceActionsAsync()
	{
		if (Context.Game is null) return;
		if (Context.SelectedSquare.X < 0 || Context.SelectedSquare.Y < 0) return;

		if (Context.SelectedPiece is not null)
		{
			await canvas.SetFillStyleAsync("blue");
			foreach (var square in Context.Board!.GetPossibleMoves(Context.SelectedPiece))
			{
				var point = square.GetPoint();
				int x = point.X;
				int y = DrawMirrored ? point.Y : 7 - point.Y;

				await canvas.FillRectAsync(x * 100 + 10, y * 100 + 10, 80, 80);
			}
		}

		int selectedX = Context.SelectedSquare.X;
		int selectedY = DrawMirrored ? Context.SelectedSquare.Y : 7 - Context.SelectedSquare.Y;

		await canvas.SetFillStyleAsync("green");
		await canvas.FillRectAsync(selectedX * 100 + 10, selectedY * 100 + 10, 80, 80);
	}

	private async Task RenderPiecesAsync()
	{
		if (Context.Game is null) return;

		await canvas.SetFontAsync("100px serif");
		foreach (var piece in Context.Board!.GetAlivePieces())
		{
			var piecePoint = piece.Square.GetPoint();
			int x = piecePoint.X;
			int y = DrawMirrored ? piecePoint.Y : 7 - piecePoint.Y;

			await canvas.DrawImageAsync(Spritesheet.GetSprite(piece), x * 100 + 10, y * 100 + 10, 80, 80);
		}
	}
}