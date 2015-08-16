using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers {
    static class Library {
	  /// <summary>
	  /// The directions that a piece can move in.
	  /// </summary>
	  private enum Directions {
		UpRight,
		UpLeft,
		DownRight,
		DownLeft
	  }
	  /// <summary>
	  /// Returns a dictionary with the key the squares that the specified piece can move to and the value a list of the pieces that would be jumped if that piece moves to the key.
	  /// </summary>
	  /// <param name="board">A 2-dimensional array of squares representing the board.</param>
	  /// <param name="currentPiece">The piece that is being moved.</param>
	  /// <returns>A dictionary with the key the squares that the piece on the specified square can move to and the value a list of the pieces that would be jumped if that piece moves to the key.</returns>
	  public static Dictionary<Square, List<Piece>> PossibleMoves (Square[,] board, Piece pieceMoved) {
		return RecursivePossibleMoves(board, pieceMoved, pieceMoved.Location, new Dictionary<Square, List<Piece>>( ));
	  }
	  /// <summary>
	  /// Returns a dictionary with the key the squares that the specified piece can move to from the specified square and the value a list of the pieces that would be jumped if that piece moves to the key.  It is recursive.
	  /// </summary>
	  /// <param name="source">The square to determine the possible moves from.  It does not necessarily contain a piece.</param>
	  /// <param name="PieceMoved">The piece that is being moved, even if it is moved from a different square.</param>
	  /// <param name="movesFound">A dictionary of the results found so far.  If it is not called recursively then it should be an empty list.</param>
	  /// <returns>A dictionary with the key the squares that the piece on the specified square can move to and the value a list of the pieces that would be jumped if that piece moves to the key.</returns>
	  private static Dictionary<Square, List<Piece>> RecursivePossibleMoves (Square[,] board, Piece pieceMoved, Square source, Dictionary<Square, List<Piece>> movesFound) {
		Dictionary<Square, List<Piece>> retVal = new Dictionary<Square, List<Piece>>(movesFound);
		List<Directions> directions = PossibleDirections(pieceMoved);		  //A list of all directions that source.Piece can move in if the surrounding squares are empty and it is not in that edge of the board.
		foreach (Directions direction in directions) {		//For each direction in which source.Piece might be able to move.
		    Square adjSquare = AdjacentSquare(board, source, direction);		//The square on direction's side from source.
		    if (adjSquare == null)	    //If source is on the edge of the board.
			  continue;
		    //If  this function was called for a single move (and wasn't called for the second move of a multi-jump) and adjSquare is empty the source.Piece can move there, so add adjSquare to movesFound.
		    if (pieceMoved.Location == source && adjSquare.Piece == null) {
			  retVal.Add(adjSquare, new List<Piece>( ));		//The list of pieces jumped should be empty.
			  continue;
		    }
		    //If adjSquare belongs to the opponent of source.Piece.Player and the next square (if it exists) in direction is empty then source.Piece can jump adjSquare.Piece
		    if (adjSquare.Piece != null && adjSquare.Piece.Player != pieceMoved.Player) {
			  Square nextSquare = AdjacentSquare(board, adjSquare, direction);
			  //If adjSquare is at the edge of the board then nextSquare would be null.  If nextSquare is not null and it is empty and nextSquare was not found before and it is not the orignal square then add nextSquare to movesFound.
			  if (nextSquare != null && nextSquare.Piece == null && !retVal.ContainsKey(nextSquare) && nextSquare != pieceMoved.Location) {
				retVal.Add(nextSquare, new List<Piece> { adjSquare.Piece });	    //Add the nextSquare as the key because source.Piece can move there and adjSquare.Piece as the value because it would be jumped.
				//Implement double-jumps by calling PossibleMoves on nextSquare.  Add all items that jump in the return value to retVal.
				foreach (KeyValuePair<Square, List<Piece>> item in RecursivePossibleMoves(board, pieceMoved, nextSquare, retVal)) {
				    if (item.Value != null && !retVal.ContainsKey(item.Key)) {
					  item.Value.Add(adjSquare.Piece);		//This move will also jump adjSquare.
					  retVal.Add(item.Key, item.Value);
				    }
				}
			  }
		    }
		}
		return retVal;
	  }
	  /// <summary>
	  /// Returns a list of all possible directions that a specified piece can move in (does not take neighboring pieces or the edge of the board into account).
	  /// </summary>
	  /// <param name="piece">The piece to determine where it can move to.</param>
	  /// <returns></returns>
	  private static List<Directions> PossibleDirections (Piece piece) {
		List<Directions> retVal = new List<Directions>(4);
		//If the player is black, which starts out on top, or the piece is a king  then add  DownRight and DownLeft to movesFound.
		if (piece.Player == Players.Black || piece.King) {
		    //If there is a square right of source and it is empty then add DownRight to movesFound.
		    retVal.Add(Directions.DownRight);
		    //DownLeft
		    retVal.Add(Directions.DownLeft);
		}
		//If the player is red, which starts out at the bottom of the board, or the piece is a king  then add UpRight and UpLeft to RetVal.
		if (piece.Player == Players.Red || piece.King) {
		    //UpRight
		    retVal.Add(Directions.UpRight);
		    //UpLeft
		    retVal.Add(Directions.UpLeft);
		}
		return retVal;
	  }
	  /// <summary>
	  /// Returns the square at the specified side of the specified square.  If there is no square at that side (for example, if side isUpRight and the square is at the top of the board) then returns null.
	  /// </summary>
	  /// <param name="square"></param>
	  /// <param name="side"></param>
	  /// <returns></returns>
	  private static Square AdjacentSquare (Square[,] board, Square square, Directions side) {
		switch (side) {
		    case Directions.UpRight:
			  //If there is a square to the up right and square is not on the right or top sides of the board.  If not then returns null.
			  if (square.Left < 7 && square.Top > 0)
				return board[square.Left + 1, square.Top - 1];
			  break;
		    case Directions.UpLeft:
			  //If there is a square to the up left of square and square is not on the left side or the top side.
			  if (square.Left > 0 && square.Top > 0)
				return board[square.Left - 1, square.Top - 1];
			  break;
		    case Directions.DownRight:
			  //If there is a square to the down right of square and square is not on the right or bottom sides of the board.
			  if (square.Left < 7 && square.Top < 7)
				return board[square.Left + 1, square.Top + 1];
			  break;
		    case Directions.DownLeft:
			  //If there is a square to the down left of square and square is not on the left or bottom sides of the board.
			  if (square.Left > 0 && square.Top < 7)
				return board[square.Left - 1, square.Top + 1];
			  break;
		    //If there is no such square because square is on the wrong side of the board and the appropriate condition was false (otherwise, the function would no longer be running) then return null.
		}
		return null;
	  }
    }
}
