using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Othello {
    static class AI {
	  /// <summary>
	  /// This struct stores a square and the number of squares flipped by choosing it.
	  /// </summary>
	  struct Move {
		/// <summary>
		/// The square to flip to make this move.
		/// </summary>
		public Square Square;
		/// <summary>
		/// A number representing how good the move is.
		/// </summary>
		public int Quality;
		public Move (Square square, int results) {
		    Square = square;
		    Quality = results;
		}
	  }
	  /// <summary>
	  /// Determines which move the computer player will choose.
	  /// </summary>
	  /// <param name="board">A multidimesional array containing all the squares in to board.</param>
	  /// <returns>The square that the computer player choose.</returns>
	  public static Square Go (Square[,] board) {
		Dictionary<Players, List<Square>> squaresOf = OthelloLibrary.GroupSquares(board);
		//Choose the best move that will allow the human player to get the worst move.
		Move bestMove = new Move( );	    //Stores the best move found so far.  It is assigned so that it can be used.
		foreach (Square outerItem in squaresOf[Players.Empty]) {		//For each empty square
		    List<Square> squaresFlipped = OthelloLibrary.SquaresFlipped(board, outerItem, Players.White);
		    if (squaresFlipped.Count < 1)	  //If the move is illegal
			  continue;
		    //Determine how good a move the human player can choose if the computer player chooses this square
		    Square[,] boardAfterMove = new Square[8, 8];	  //The board after this move
		    //Assign all squares in boardAfterMove to a clone of the corresponding square in board
		    foreach (Square innerItem in board) {
			  boardAfterMove[innerItem.Left, innerItem.Top] = innerItem.Clone( );
		    }
		    //Flip the squares that would be flipped if the computer player chooses this square
		    foreach (Square innerItem in squaresFlipped) {
			  boardAfterMove[innerItem.Left, innerItem.Top].Player = innerItem.Player;
		    }
		    boardAfterMove[outerItem.Left, outerItem.Top].Player = outerItem.Player;	    //The square itself is not included in squaresFlipped
		    //Determine the rating of the best move the human player can choose if he chooses the best move.
		    //In order to do so, a list of empty squares in boardAfterMove is necessary.
		    List<Square> potentialResponses = new List<Square>( );
		    //Add all valid moves that the human player can choose to potentialResponses.
		    foreach (Square innerItem in boardAfterMove)
			  if (innerItem.Player == Players.Empty && OthelloLibrary.SquaresFlipped(boardAfterMove, innerItem, Players.Black).Count >= 1)
				potentialResponses.Add(innerItem);
		    Move bestResponse = new Move( );   //The best move that the human player can choose after the computer player chose OuterItem.  It is assigned so that it can be used.
		    //Find the best move that the human player can choose after the computer player choose outerItem.
		    foreach (Square innerItem in potentialResponses) {
			  Move currentResponse = new Move(innerItem, RateMove(boardAfterMove, Players.Black, innerItem));
			  //If the current move is better than the best move found so far then assign bestResponse to currentMove.
			  if (currentResponse.Quality > bestResponse.Quality)
				bestResponse = currentResponse;
		    }
		    Move currentMove = new Move(outerItem, RateMove(board, Players.White, outerItem) - bestResponse.Quality);
		    //If the current move will flip better squares or allow the human player to flip worse squares than the best move found so far will or no valid move was found so far then assign bestMove to the current move.
		    if (currentMove.Quality >= bestMove.Quality || bestMove.Square == null)
			  bestMove = currentMove;
		}
		//MessageBox.Show(bestMove.Quality.ToString( ), "Othello");
		return bestMove.Square;
	  }
	  /// <summary>
	  /// Rates a move by the quantity and the quality of the squares it flips.
	  /// </summary>
	  /// <param name="board">A 2-dimensional array of squares that represents to board that the move can be made in.</param>
	  /// <param name="player">The player who can make the move.</param>
	  /// <param name="squareChoosen">The square that the specified player can choose to make this move.</param>
	  /// <returns>The rating of the move.</returns>
	  private static int RateMove (Square[,] board, Players player, Square squareChoosen) {
		int retVal = 0;
		List<Square> squaresGained = OthelloLibrary.SquaresFlipped(board, squareChoosen, player);   //The total number of squares that will be gained by this move, including squareChoosen.
		squaresGained.Add(squareChoosen);
		//Rate each square in squareChoosen
		foreach (Square item in squaresGained) {
		    //If the square is a corner square then give it a very high rating
		    if ((item.Left == 0 || item.Left == 7) && (item.Top == 0 || item.Top == 7)) {		  //If Left and Top are at an edge.
			  retVal += 12;
			  continue;
		    }
		    //If the square is an X square and the nearest corner is empty and it will not be gained by the specified move (and therefore, the specified move and cause the opponent of player to gain the corner).
		    if ((item.Left == 1 || item.Left == 6) && (item.Top == 1 || item.Top == 6) && NearestCorner(board, item) != squareChoosen && NearestCorner(board, item).Player == Players.Empty) {
			  retVal -= 9;	  //Give the move a low rating because it can cause players's opponent to lose a corner.
			  continue;
		    }
		    //If item is an C square and the adjacent corner does is empty and will not be gained by the current move then give it a low rating.
		    if ((item.Left >= 6 || item.Left <= 1) && (item.Top >= 6 || item.Top <= 1) && NearestCorner(board, item).Player == Players.Empty && NearestCorner(board, item) != squareChoosen) {	  //If item is an X square or a corner square then the loop would have continued to the next item in squaresGained because of the previous statements.
			  retVal -= 8;
		    }
		    //If item is an edge square and all squares between it and a corner belong to player then the square is secure, so give it a high rating.
		    if ((item.Left == 0 || item.Left == 7 || item.Top == 0 || item.Top == 7) &&	    //If item is an edge square
			  OthelloLibrary.SquaresBetween(board, NearestCorner(board, item), item).All(s => s.Player == player || squaresGained.Contains(s))) {		//and all squares between it and the nearest corner belong to player or will be gained in the current turn
			  retVal += 7;
			  continue;
		    }
		    //If item is not a frontier square then give it a high rating.
		    //It is not considered a frontier square if at least six of the adjacent squares are not empty.
		    List<Square> adjacentSquares = AdjacentSquares(board, item);
		    if (adjacentSquares.Count(s => s.Player != Players.Empty) > 6) {
			  retVal += 5;
			  continue;
		    }
		    retVal += 1;		  //If item is a typical square that give it a rating of 1.
		}
		return retVal;
	  }
	  /// <summary>
	  /// Returns the corner that is closest to the specified square.
	  /// </summary>
	  /// <param name="board">A multi-dimensional array of squares representing the board.</param>
	  /// <param name="square">The specified square.</param>
	  /// <returns>The corner that is closest to the specified square.</returns>
	  private static Square NearestCorner (Square[,] board, Square square) {
		return board[(square.Left / 4) * 7, (square.Top / 4) * 7];	  //Left/4 would be between 0 and 1 if Left is below 4 and it would be rounded down to 0 in order to store it in an int.  Therefore, (square.Left/4)*7 would equal 0.  If Left is 4 or higher then Left/4 would be between 1 and 2 and would be rounded to 1.Therefore, (square.Left/4)*7 would equal 7.
	  }
	  /// <summary>
	  /// Returns a list of all the adjacent squares of the specified square.
	  /// </summary>
	  /// <param name="board">A 2-dimensional array of squares representing the board.</param>
	  /// <param name="square"></param>
	  /// <returns>A list of all the adjacent squares of the specified square.</returns>
	  private static List<Square> AdjacentSquares (Square[,] board, Square square) {
		List<Square> retVal = new List<Square>( );
		//If the square isn't in the top row then add the square above it to retVal.
		if (square.Top > 0)
		    retVal.Add(board[square.Left, square.Top - 1]);
		//If the square isn't in the bottom row then add the square below it to retVal.
		if (square.Top < 7)
		    retVal.Add(board[square.Left, square.Top + 1]);
		//If the square isn't in the first column then add the square to the left of it to retVal.
		if (square.Left > 0)
		    retVal.Add(board[square.Left - 1, square.Top]);
		//If the square isn't in the last column then add the square to the right of it to retVal/
		if (square.Left < 7)
		    retVal.Add(board[square.Left + 1, square.Top]);
		//If the square isn't in the first column and isn't in the first row then add the square left of the square above it to retVal.
		if (square.Left > 0 && square.Top > 0)
		    retVal.Add(board[square.Left - 1, square.Top - 1]);
		//If the square isn't in the first column and isn't in the last row then add the square left of the square below it to retVal.
		if (square.Left > 0 && square.Top < 7)
		    retVal.Add(board[square.Left - 1, square.Top + 1]);
		//If the square isn't in the last column and isn't in the first row then add the square right of the square above it to retVal.
		if (square.Left < 7 && square.Top > 0)
		    retVal.Add(board[square.Left + 1, square.Top - 1]);
		//If the square isn't in the last column and isn't in the last row then add the square right of the square below it to retVal.
		if (square.Left < 7 && square.Top < 7)
		    retVal.Add(board[square.Left + 1, square.Top + 1]);
		return retVal;
	  }
    }
}
