namespace BlazorChess.Shared.Interfaces;

public interface IOnlineGameServer
{
	public Task MakeMove(string moveUciCode);
	public Task<string[]> GetMoveHistory();
}
