global using ChessSide = ChessDotCore.Engine.Interfaces.Color;

using ChessDotCore.Engine.Interfaces;
using System.Drawing;


namespace BlazorChess.Shared;

static public class ChessExtentions
{
    public static Point GetPoint(this ISquare square)
    {
        string squareCode = square.UciCode;
        int x = squareCode[0] - 'A';
        int y = squareCode[1] - '1';
        return new Point(x, y);
    }

    public static IPiece? GetPieceAtPoint(this IBoard board, Point point)
    {
        return board.GetAlivePieces().Where(x => x.Square.GetPoint() == point).FirstOrDefault();
    }
    public static IPiece? GetPieceAtSquare(this IBoard board, ISquare square)
    {
        return board.GetAlivePieces().Where(x => x.Square == square).FirstOrDefault();
    }

    public static IEnumerable<IPiece> GetAlivePieces(this IBoard board)
    {
        return board.Pieces.Where(x => x.Square is not null);
    }

	public static bool IsGameOver(this IBoard board)
	{
		return board.IsDraw || board.IsCheckMate;
	}

	public static IEnumerable<ISquare> GetPossibleMoves(this IPiece piece)
    {
        return piece.ReachableSquares.Concat(piece.AttackedSquares);
    }

	public static IEnumerable<ISquare> GetPossibleMoves(this IBoard board, IPiece piece)
	{
        return board.LegalMoves.Where(x => x.MovingPiece == piece).Select(x => x.ToSquare);
	}

	public static bool IsValidMove(this IBoard board, Point from, Point to)
    {
        return board.LegalMoves.Any(x => x.FromSquare.GetPoint() == from && x.ToSquare.GetPoint() == to);
    }

	public static IMove? GetMoveFromUci(this IBoard board, string uciString)
	{
		return board.LegalMoves.Where(x => x.ToString()!.ToLower() == uciString.ToLower()).FirstOrDefault();
	}

	public static IMove? GetMoveFromCoodrinates(this IBoard board, Point from, Point to)
	{
		return board.LegalMoves.First(x => x.FromSquare.GetPoint() == from && x.ToSquare.GetPoint() == to);
	}

	public static void Move(this IGame game, Point from, Point to)
    {
        IMove move = game.Board.LegalMoves.First(x => x.FromSquare.GetPoint() == from && x.ToSquare.GetPoint() == to);
        game.Move(move);
    }

	//   public static ChessSide OppositeSide(this ChessSide side) 
	//   {
	//       return side switch
	//       {
	//           ChessSide.Black => ChessSide.White,
	//           ChessSide.White => ChessSide.Black,
	//           _ => throw new ArgumentException("There is no opposite sede for " + side.ToString())
	//       };
	//}

	public static ChessSide OppositeSide(this ChessSide side)
	{
		return side switch
		{
			ChessSide.Black => ChessSide.White,
			ChessSide.White => ChessSide.Black,
			_ => throw new ArgumentException("There is no opposite sede for " + side.ToString())
		};
	}
}
