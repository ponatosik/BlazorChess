using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorChess.Client.Chess.Online;


public class OnlineGameService
{
    private NavigationManager _navigationManager;
    public OnlineGameService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public OnlineGame JoinOnlineGame(Guid gameId)
    {
        return new OnlineGame(_navigationManager, gameId);
    }
}
