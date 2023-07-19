using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.WebUtilities;
using BlazorChess.Shared.Interfaces;
using TypedSignalR.Client;

namespace BlazorChess.Client.Chess.Online;

public class OnlineGame : IOnlineGame
{
    public event Action<string>? OnMoveRecieved;
    public event Action<Exception?>? OnGameClosed;

    public Guid GameId => _gameId;

    public IOnlineGameServer Invoke => _hubProxy;

	private HubConnection _hubConnection;
    private IOnlineGameServer _hubProxy;
    private readonly Guid _gameId;

    public IDisposable RegisterClient(IOnlineGameClient client) 
    {
        return _hubConnection.Register(client);
    }


    public OnlineGame(NavigationManager navigation, Guid gameId)
    {
        _gameId = gameId;

        var queryParams = new Dictionary<string, string>()
        {
            ["gameid"] = gameId.ToString()
        };

        var hubUrl = QueryHelpers.AddQueryString(navigation.ToAbsoluteUri("/chesshub").ToString(), queryParams);

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        _hubConnection.On<string>("MakeMove", (moveStr) => OnMoveRecieved?.Invoke(moveStr));
        _hubConnection.Closed += (exception) =>
        {
            OnGameClosed?.Invoke(exception);
            return Task.CompletedTask;
        };

        _hubConnection.StartAsync();
        _hubProxy = _hubConnection.CreateHubProxy<IOnlineGameServer>();
	}
}
