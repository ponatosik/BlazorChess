using System.ComponentModel.DataAnnotations;

namespace BlazorChess.Shared.Models;

public enum GameStatus { NotStarted, InProgress, Finished }

public class OnlineGameInfo
{
	public Guid Id { get; set; }
	public GameStatus Status { get; set; } = GameStatus.NotStarted;
	public DateTime Time { get; set; } = DateTime.Now;
	public string CurrentFenPosition { get; set; } = "";
	public List<string> MovesHistory { get; set; } = new();
}

