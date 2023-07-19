using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using BlazorChess.Shared;
using BlazorChess.Client.Services;
using ChessDotCore.Engine.Interfaces;

namespace BlazorChess.Client.Chess.Online.Pages;

public partial class OnlineGamePage
{
    [AllowNull]
    protected IOnlineGame OnlineGame;
    [Parameter, AllowNull]
    public Guid GameId { get; set; }

	[Inject]
	private OnlineGameService OnlineService { get; set; }
	[Inject]
	private AlertService AlertService { get; set; }
	[Inject]
	private ClipboardService Clipboard { get; set; }
	[Inject]
	private NavigationManager Navigation { get; set; }

	protected async override Task OnInitializedAsync()
	{
		OnlineGame = OnlineService.JoinOnlineGame(GameId);

		Context.OnMoveMade += async (previousTurn, moveUci) =>
		{
			if (previousTurn == Context.PlayerSide)
				await SendMove(moveUci);
		};
		OnlineGame.OnGameClosed += async (exception) =>
		{
			if (exception is not null)
				await NotifyError(exception);
		};
		await base.OnInitializedAsync();
	}

	public async Task SendMove(string moveUci)
	{
		await OnlineGame.Invoke.MakeMove(moveUci);
	}

	public async Task NotifyError(Exception exception)
	{
		await AlertService.ShowAsync($"Online game error: {exception.Message}");
	}

	public async Task CopyInvitationLink()
	{
		var link = Navigation.ToAbsoluteUri($"/Chess/Online/{PlayerSide.OppositeSide()}/{GameId.ToString("N")}");
		await Clipboard.WriteAsync(link.ToString());
	}

	override public async Task RestartGame()
	{
		Context.Game = ChessEngine.CreateGame("newGame");

		var moveHistory = await OnlineGame.Invoke.GetMoveHistory();
		foreach (var moveUci in moveHistory)
		{
			IMove? move = Context.Board!.GetMoveFromUci(moveUci);
			Context.Game.Move(move);
		}

		StateHasChanged();
		Context.ContextChanged();

		if (Context.Game.Board.Turn == Color.White)
			WhiteInput.MakeTurn();
		if (Context.Game.Board.Turn == Color.Black)
			BlackInput.MakeTurn();
	}
}