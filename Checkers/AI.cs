using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers {
    public static class AI {
	  public static Move Go (Square[,] board) {
		Move retVal = new Move( );
		Dictionary<Players, List<Piece>> piecesOf = new Dictionary<Players, List<Piece>>( );	    //A dictionary of lists of pieces.  piecesOf[Colors.Red] contains a list of all red pieces.
		piecesOf.Add(Players.Black, new List<Piece>( ));	    //Initialize the list of black pieces in piecesOf. 
		piecesOf.Add(Players.Red, new List<Piece>( ));
		//Sort the pieces and add them into the respective list in PiecesOf.
		foreach (Square oItem in board) 	  //For each square.
		    if (oItem.Piece != null)	    //If the square has a piece in it.
			  piecesOf[oItem.Piece.Player].Add(oItem.Piece);		//Add the piece to the respective list in piecesOf.

		//Find the cmove which jumps the most pieces.
		//Evaluate the moves of each of the computer's pieces.
		foreach (Piece currentPiece in piecesOf[Players.Black])		//For each of the computer player's pieces (the computer player is black).
		    foreach (var item in Library.PossibleMoves(board, currentPiece)) {		  //For each of the squares that currentPiece can move to.
			  Move currentMove = new Move(currentPiece, item.Key, item.Value, 0);
			  currentMove.Rating = RateMove(currentMove);
			  //Determine the human player's best response after the proposed move.
			  Square[,] contingency = new Square[8, 8];
			  for (int row = 0; row < 8; row++) {
				for (int column = 0; column < 8; column++) {
				    contingency[row, column] = board[row, column].Clone( );
				}
			  }
			  contingency[item.Key.Left, item.Key.Top].Piece = contingency[currentPiece.Location.Left, currentPiece.Location.Top].Piece;	    //The piece being moved should be moved to the proposed destination.
			  contingency[currentPiece.Location.Left, currentPiece.Location.Top].Piece = null;	  //Remove currentPiece from it's old location.
			  Move bestResponse = new Move( );
			  foreach (Square iItem in contingency) {
				if (iItem.Piece != null && iItem.Piece.Player == Players.Red) {
				    foreach (var iiItem in Library.PossibleMoves(contingency, iItem.Piece)) {
					  int rating = RateMove(new Move(iItem.Piece, iiItem.Key, iiItem.Value, 0));
					  if (bestResponse.PieceMoved == null || rating > bestResponse.Rating)
						bestResponse = new Move(iItem.Piece, iiItem.Key, iiItem.Value, rating);
				    }
				}
			  }
			  currentMove.Rating -= bestResponse.Rating;
			  if (retVal.PiecesJumped == null || currentMove.Rating > retVal.Rating)		    //If retVal was never set before or currentMove jumps more pieces than the previously found best move.
				retVal = currentMove;
		    }
		return retVal;
	  }
	  private static int RateMove (Move move) {
		int retVal = 0;
		foreach (Piece item in move.PiecesJumped) {
		    retVal += 1;
		    if (item.King)
			  retVal += 1;
		}
		if ((move.PieceMoved.Player == Players.Black && move.Destination.Top == 7)	  //If the piece is black and it is on the bottom of the board
		    || (move.PieceMoved.Player == Players.Red && move.Destination.Top == 0))		//or if the piece is red and it is on the top of the board.
		    retVal += 2;

		if (move.PieceMoved.Location.Top == (move.PieceMoved.Player == Players.Black ? 0 : 7))
		    retVal -= 3;
		else
		    if (move.PieceMoved.Location.Left == 0 || move.PieceMoved.Location.Left == 7)
			  retVal -= 1;
		if (move.Destination.Left == 0 || move.Destination.Left == 7)
		    retVal += 1;
		return retVal;
	  }
    }
}
