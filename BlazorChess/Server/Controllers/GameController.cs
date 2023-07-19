using BlazorChess.Server.Data;
using BlazorChess.Shared.Models;
using Microsoft.AspNetCore.Mvc;

using ChessSide = ChessDotCore.Engine.Interfaces.Color;


namespace BlazorChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
	private readonly ChessDbContext _dbContext;
	public GameController(ChessDbContext dbContext) 
	{
		_dbContext = dbContext;
	}


	[HttpPut]
	public ActionResult<Guid> Put(OnlineGameSettings settings)
	{
		OnlineGameInfo gameInfo= new OnlineGameInfo();
		_dbContext.Games.Add(gameInfo);
		_dbContext.SaveChanges();

		Guid gameId = gameInfo.Id;

		ChessSide side = settings.Side switch
		{
			PreferedChessSide.White => ChessSide.White,
			PreferedChessSide.Black => ChessSide.Black,
			PreferedChessSide.Random => new Random().Next(2) == 1 ? ChessSide.White : ChessSide.Black,
			_ => throw new ArgumentException($"Unknow value \"{settings.Side}\"")
		};

		return Created($"chess/online/{side}/{gameId.ToString("N")}", gameId);
	}

	[HttpGet]
	public ActionResult<IEnumerable<OnlineGameInfo>> Get()
	{
		return Ok(_dbContext.Games);
	}
}
