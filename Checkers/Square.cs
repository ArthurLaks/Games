using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers {
    /// <summary>
    /// The player of a token or square.
    /// </summary>
    public enum Colors {
	  Black = 0,
	  Red = 1
    }
    /// <summary>
    /// Represents all squares including red squares.
    /// </summary>
    public class Square : ICloneable {
	  private Colors color;		//Stores the value of the player property.
	  /// <summary>
	  /// This property is only sent in the constructor because the player of a square cannnot change in the middle of a game.
	  /// </summary>
	  public Colors Color {
		get {
		    return color;
		}
	  }
	  private int left;		//Stores the value of the Left property.
	  /// <summary>
	  /// The 0-based number of squares from the left of the board.  It is only set in the constructor.
	  /// </summary>
	  public int Left {
		get {
		    return left;
		}
	  }
	  private int top;		//Stores the value of the Top property.
	  /// <summary>
	  /// The 0-based number of squares from the top of the board.  It is only set in the constructor.
	  /// </summary>
	  public int Top {
		get {
		    return top;
		}
	  }
	  private Piece piece;	  //Stores the value of the property Piece.
	  /// <summary>
	  /// Gets or sets the piece on this square.
	  /// </summary>
	  public Piece Piece {
		get {
		    return piece;
		}
		set {
		    if (Color == Colors.Red)
			  throw new InvalidOperationException("A red square cannot have a piece on it");
		    if (piece != value) {	    //If the value of the property changed.
			  piece = value;
			  if (piece != null)		//If piece is null then do not refer to piece.Location.
				piece.Location = this;		//Make sure that the piece's Location property equals this square.
		    }
		}
	  }
	  /// <summary>
	  /// Constructs a square with the Left, Top, and Player properties set to the specified values.
	  /// </summary>
	  /// <param name="left">The 0-based number of squares from the left of the board that the new square should be.</param>
	  /// <param name="top">The 0-based number of squares from the top of the board that the new square should be.</param>
	  /// <param name="color">The color of the new square.</param>
	  public Square (int left, int top, Colors color) {
		this.left = left;
		this.top = top;
		this.color = color;
	  }
	  public Square Clone ( ) {
		Square retVal = (Square)base.MemberwiseClone( );
		retVal.piece = null;
		if (piece != null)
		    retVal.piece = new Piece(piece.Player, new Square(left, top, color));
		return retVal;
	  }

	  object ICloneable.Clone ( ) {
		return Clone( );
	  }

    }
}