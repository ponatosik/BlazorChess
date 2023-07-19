using BlazorChess.Shared;
using BlazorChess.Shared.Interfaces;
using ChessDotCore.Engine.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorChess.Client.Chess.Online.Components;

public class SignalReciever : GameInput, IOnlineGameClient
{
	[Parameter, EditorRequired, AllowNull]
	public IOnlineGame OnlineGame { get; set; }

	private TaskCompletionSource<bool>? _didPlayerMakeTurn;
	private IDisposable _onlineGameEventsSubscription;

	protected async override Task OnInitializedAsync()
	{
		_onlineGameEventsSubscription = OnlineGame.RegisterClient(this);
		await base.OnInitializedAsync();
	}

	public async Task MakeMove(string turnUciString)
	{
		if (!IsMyTurn) return;

		IMove? move = Context.Board!.GetMoveFromUci(turnUciString);
		if (move is null) return;

		Context.Game!.Move(move);

		_didPlayerMakeTurn?.SetResult(true);
		Context.MoveMade(InputForSide, turnUciString);
	}

	public override Task MakeTurn()
	{
		_didPlayerMakeTurn = new TaskCompletionSource<bool>();
		return _didPlayerMakeTurn.Task;
	}
}
