using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Othello {
    static class OthelloLibrary {
	  /// <summary>
	  /// Groups the squares by player.
	  /// </summary>
	  /// <param name="squares">A list of the squares in the board.</param>
	  /// <returns>A dictionary where the key is the player and the value is a list the squares belonging to that player.</returns>
	  public static Dictionary<Players, List<Square>> GroupSquares (Square[,] squares) {
		Dictionary<Players, List<Square>> retVal = new Dictionary<Players, List<Square>>( );
		//Initialize the lists.
		retVal[Players.Black] = new List<Square>( );
		retVal[Players.White] = new List<Square>( );
		retVal[Players.Empty] = new List<Square>( );
		//Add each square to the appropriate list.
		foreach (Square item in squares) {
		    retVal[item.Player].Add(item);
		}
		return retVal;
	  }
	  /// <summary>
	  /// Returns the squares that would be flipped had the specified player choosen the specified square.
	  /// </summary>
	  /// <param name="squareToTest">The square that would have caused the squares to be flipped.</param>
	  /// <param name="playerToTest"></param>
	  /// <returns>A list of the squares that would be flipped had the specified </returns>
	  public static List<Square> SquaresFlipped (Square[,] squares, Square squareToTest, Players playerToTest) {
		Dictionary<Players, List<Square>> squaresOf = GroupSquares(squares);
		List<Square> retVal = new List<Square>( );
		//For each square that playerToTest owns, add all squares between it and squareToTest if there are no squares that does not belong to his opponent among those squares.
		foreach (Square cSquare in squaresOf[playerToTest]) {
		    if (cSquare == squareToTest)
			  continue;
		    List<Square> squaresBetween = SquaresBetween(squares, squareToTest, cSquare);
		    if (squaresBetween.Any(square => square.Player != Opponent(playerToTest)))	    //If any of the squares in squaresBetween do not belong to playerToTest's opponent then do not add anything to retVal.  If squaresBetween contains squares that should be flipped then they will be added when the loop is up to the nearest square that belongs to playerToTest
			  continue;
		    else
			  retVal.AddRange(squaresBetween);
		}
		return retVal;
	  }
	  /// <summary>
	  /// Returns the squares between the two specified squares.
	  /// </summary>
	  /// <param name="squares">An array of the squares in the board</param>
	  /// <param name="firstSquare"></param>
	  /// <param name="secondSquare"></param>
	  /// <returns></returns>
	  public static List<Square> SquaresBetween (Square[,] squares, Square firstSquare, Square secondSquare) {
		List<Square> retVal = new List<Square>( );
		//If no squares can are between firstSquare and secondSquare because first square and secondSquare are not is the same row or column and are not across from eachother
		if (!(firstSquare.Left == secondSquare.Left || firstSquare.Top == secondSquare.Top	    //If both squares are not in the same row or column
		    || Math.Abs(firstSquare.Left - secondSquare.Left) == Math.Abs(firstSquare.Top - secondSquare.Top)))	//and are not across from eachother
		    return retVal;	  //Return retval, which is empty
		else {
		    //For every square between firstSquare and secondSquare, add it to retVal
		    for (int x = firstSquare.Left, y = firstSquare.Top;
			  x != secondSquare.Left || y != secondSquare.Top;	//Until leftCounter and rightCounter refer to secondSquare
			   x += Math.Sign(secondSquare.Left - firstSquare.Left),	//If secondSquare.Left is greater than firstSquare.Left then increments leftCounter and adds the square to the left (closer to secondSquare) to retVal.  If secondSquare.Left equals firstSquare.Left then leftCounter remains unchanged and the square above or below the previous square is added to retVal.  If secondSquare.Left is less than firstSquare.Left then letfCounter is decremented.
			   y += Math.Sign(secondSquare.Top - firstSquare.Top)) {
			  Square squareToAdd = squares[x, y];
			  if (squareToAdd != firstSquare && squareToAdd != secondSquare)
				retVal.Add(squares[x, y]);
		    }
		    return retVal;
		}
	  }
	  /// <summary>
	  /// Returns the opponent of the specified player.
	  /// </summary>
	  /// <param name="player"></param>
	  /// <returns>Black if white was passed and white if black was passed.</returns>
	  public static Players Opponent (Players player) {
		switch (player) {
		    case Players.White:
			  return Players.Black;
		    case Players.Black:
			  return Players.White;
		    default:	  //If player equals empty
			  throw new ArgumentException("Empty has no opponent", "player");
		}
	  }
    }
}
