using BlazorChess.Shared;
using BlazorChess.Client.Chess.Components;
using ChessDotCore.Engine.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BlazorChess.Client.Chess;

public abstract class ChessGame : ComponentBase
{
	[Parameter]
	public string? PlayerSideStr { get; set; }
	[AllowNull]
	public ChessContext Context { get; } = new ();

	[AllowNull]
	public GameInput WhiteInput { get; set; }
	[AllowNull]
	public GameInput BlackInput { get; set; }

	[AllowNull]
	public ChessSide PlayerSide { get; set; }
	[AllowNull]
	public ChessSide OpponentSide { get; set; }


	[AllowNull]
	public GameInput PlayerInput { get; set; }
	[AllowNull]
	public GameInput OpponentInput { get; set; }

	[AllowNull]
	public ChessSpritesheet Spritesheet { get; set; }
	[AllowNull]
	public ChessRenderer Renderer { get; set; }
	[AllowNull]
	public GameStatus GameStatus { get; set; }

	[Inject, AllowNull]
	protected IChess ChessEngine { get; set; }

	protected async override Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			return;
		}

		WhiteInput = PlayerSide == ChessSide.White ? PlayerInput : OpponentInput;
		BlackInput = PlayerSide == ChessSide.Black ? PlayerInput : OpponentInput;

		Context.OnMoveMade += async(previousSide, _) => await NextTurn(previousSide);

		await Spritesheet.WaitLoading();
		await Renderer.WaitInitialization();
		await RestartGame();
		await base.OnAfterRenderAsync(firstRender);
	}

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		Context.PlayerSide = PlayerSideStr?.ToLower() switch
		{
			"black" => ChessSide.Black,
			"white" => ChessSide.White,
			_ => null
		};


		PlayerSide = Context.PlayerSide ?? ChessSide.White;
		OpponentSide = Context.PlayerSide?.OppositeSide() ?? ChessSide.Black;
	}

	public async Task NextTurn(ChessSide previousSide) 
	{
		if (previousSide == ChessSide.Black) await WhiteInput.MakeTurn();
        if (previousSide == ChessSide.White) await BlackInput.MakeTurn();
    }

	virtual public Task RestartGame()
	{
		Context.Game = ChessEngine.CreateGame("newGame");
		StateHasChanged();
		Context.ContextChanged();
		WhiteInput.MakeTurn();
		return Task.CompletedTask;
	}

	public async Task UpdateGameAsync()
	{
		if (Renderer.IsInitialized)
			await Renderer.RenderChessAsync();
		StateHasChanged();
		GameStatus.Update();
	}
}
