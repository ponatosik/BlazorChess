namespace BlazorChess.Shared.Interfaces;

public interface IOnlineGameClient
{
    public Task MakeMove(string moveUciCode);
}
