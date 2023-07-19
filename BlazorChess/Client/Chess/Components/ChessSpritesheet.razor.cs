using Microsoft.AspNetCore.Components;
using ChessDotCore.Engine.Interfaces;

namespace BlazorChess.Client.Chess.Components;

public partial class ChessSpritesheet
{
	public bool IsLoaded => imagesLoaded >= chessImagesByPieceChar.Count;

	protected Dictionary<char, ElementReference> chessImagesByPieceChar = new();
	protected int imagesLoaded = 0;
	private TaskCompletionSource<bool> _didLoad = new TaskCompletionSource<bool>();

	public Task WaitLoading()
	{
		return _didLoad.Task;
	}

	public ElementReference GetSprite(IPiece piece)
	{
		var sprite = chessImagesByPieceChar[piece.PieceChar];
		return chessImagesByPieceChar[piece.PieceChar];
	}

	protected void ImageLoaded()
	{
		imagesLoaded++;
		if (IsLoaded) _didLoad.SetResult(true);
	}
}