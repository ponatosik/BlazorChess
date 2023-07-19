using Microsoft.AspNetCore.SignalR;
using BlazorChess.Shared.Interfaces;
using BlazorChess.Server.Data;

namespace BlazorChess.Server.Hubs;

public class ChessHub : Hub<IOnlineGameClient>, IOnlineGameServer
{
	private readonly ChessDbContext _dbContext;
	public ChessHub(ChessDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task MakeMove(string moveUciCode)
    {
		if(ConnectedGameId is null)
			throw new Exception("Game id is not specified");

		_dbContext.Games.FirstOrDefault(x => x.Id == ConnectedGameId)
			?.MovesHistory.Add(moveUciCode);

		await Clients.Groups(ConnectedGameId!.ToString()!).MakeMove(moveUciCode);
		await _dbContext.SaveChangesAsync();
	}

    public override async Task OnConnectedAsync() 
    {
		var httpContext = Context.GetHttpContext();
		if (httpContext == null || !httpContext.Request.Query.Keys.Contains("gameid"))
		{
			await base.OnConnectedAsync();
			throw new Exception("Game id is not specified");
		}

		string gameIdString = httpContext.Request.Query["gameid"]!;
		if (!Guid.TryParse(gameIdString, out Guid gameId))
		{
			await base.OnConnectedAsync();
			throw new Exception("Game id is not valid");
		}

		await ConnectToGame(gameId);
		await base.OnConnectedAsync();
	}

	public async Task ConnectToGame(Guid gameId) 
	{
		ConnectedGameId = gameId;
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
	}

	public async Task<string[]> GetMoveHistory()
	{
		if (ConnectedGameId is null)
			throw new Exception("Game id is not specified");

		var moveHistory = _dbContext.Games.FirstOrDefault(x => x.Id == ConnectedGameId)?.MovesHistory;
		return moveHistory?.ToArray() ?? Array.Empty<string>();
	}

	protected Guid? ConnectedGameId
	{
		get
		{
			Guid? Id;
			Context.Items.TryGetValue("GameId", out object gameId);
			Id = gameId as Guid? ?? null;
			return Id;
		}
		set
		{
			Context.Items["GameId"] = value;
		}
	}
}