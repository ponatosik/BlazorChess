using System.Net.Http.Json;
using BlazorChess.Shared.Models;

namespace BlazorChess.Client.Chess.Online.Pages
{
    public partial class CreateGamePage
    {
        private OnlineGameSettings settings = new();
        public async Task CreateGame()
        {
            var response = await Http.PutAsJsonAsync("api/game", settings);
            var gameUri = response.Headers.Location;
            Navigation.NavigateTo(gameUri.ToString());
        }
    }
}