using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using BlazorChess.Shared;

namespace BlazorChess.Client.Chess.Components;

public partial class GameStatus
{
    [CascadingParameter, EditorRequired, AllowNull]
    public ChessContext Context { get; set; }

    private string gameStatus = "Game status";
	protected async override Task OnInitializedAsync()
	{
		Context.OnContextChanged += () => Update();
		await base.OnInitializedAsync();
	}

	public void Update()
	{
		if (!Context.IsGameStarted)
		{
			gameStatus = "Game has not started yet";
			StateHasChanged();
			return;
		}

		string status = "";

		if (Context.Board!.IsGameOver())
			status += "Game over \n";

		status += $"Turn: {Context.Board!.TurnNumber}\n";
		status += $"Turn of : {Context.Board!.Turn.ToString()}\n";

		if (Context.Board!.MoveHistory.Any())
			status += $"Last move: {Context.Board!.MoveHistory.Last()}\n";

		gameStatus = status;
		StateHasChanged();
	}
}