using BlazorChess.Shared.Interfaces;

namespace BlazorChess.Client.Chess.Online;

public interface IOnlineGame
{
    public event Action<Exception?>? OnGameClosed;

    public IOnlineGameServer Invoke { get; }
    public IDisposable RegisterClient(IOnlineGameClient client);

	public Guid GameId { get; }
}
