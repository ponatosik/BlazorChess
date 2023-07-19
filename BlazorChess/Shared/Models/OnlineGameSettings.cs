namespace BlazorChess.Shared.Models;

public enum PreferedChessSide { White = ChessSide.White, Black = ChessSide.Black, Random }

public class OnlineGameSettings
{
    public PreferedChessSide Side { get; set; } = PreferedChessSide.Random;

    public static OnlineGameSettings Default => new OnlineGameSettings();
}
