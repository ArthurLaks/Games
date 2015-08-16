using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Othello {
    /// <summary>
    /// The players that a square can belong to.
    /// </summary>
    internal enum Players {
	  Empty,
	  White,
	  Black
    }
    /// <summary>
    /// Represents one of the 64 squares on the board.
    /// </summary>
    class Square {
	  private int left;		//Stores the value of the Left propertry
	  /// <summary>
	  /// Gets the number of squares from the left of the of the board that this square is.
	  /// </summary>
	  public int Left {
		get {
		    return left;
		}
	  }
	  private int top;		//Stores the value of the Top propertry
	  ///<summary>Gets the number of squares from the top of the board that this square is.</summary>
	  public int Top {
		get {
		    return top;
		}
	  }
	  private Players player = Players.Empty;	//Stores the value of the Player property.  Starts out empty.
	  public delegate void PlayerChangedEventHandler (Square sender, Players oldValue);	  //OldValue is used to update the appropriate list in the method that subscribes to PlayerChanged
	  ///<summary>Occurs when the Player of a square changes.</summary>
	  public event PlayerChangedEventHandler PlayerChanged;
	  ///<summary>Gets or sets player that this square belongs to and calls the OnPlayerChanged event.</summary>
	  public Players Player {
		get {
		    return player;
		}
		set {
		    if (Player != value) {	    //If the old value equals the new value then do nothing
			  Players oldValue = player;
			  player = value;
			  if(PlayerChanged !=null)
			    PlayerChanged(this, oldValue);	    //Raise the OnPlayerChanged event so that the form can change the color of the square and update the appropriate list
		    }
		}
	  }
	  /// <summary>
	  /// Initializes an empty square at the specified location.
	  /// </summary>
	  /// <param name="left">The number of square from the left of the board that the new square should be.</param>
	  /// <param name="top">The number of squares from the top of the board that the new square should be.</param>
	  internal Square (int left, int top):this(left,top,Players.Empty) {
	  }
	  /// <summary>
	  /// Initializes a square of the specified player at the specified location.
	  /// </summary>
	  /// <param name="left">The number of square from the left of the board that the new square should be.</param>
	  /// <param name="top">The number of squares from the top of the board that the new square should be.</param>
	  /// <param name="player">The player that the specified square should belong to.</param>
	  internal Square (int left, int top, Players player) { 
		this.left = left;	    //Assign the left field to the left parameter (the Left property is read-only).
		this.top = top;	    //Assign the top field to the top parameter (the Top property is read-only).
		Player = player;	  //Assign the Player property to the player field.
	  }
	  /// <summary>
	  /// Creates a new square of the player and in the location as this square.
	  /// </summary>
	  /// <returns>A new square of the player and in the location as this square.</returns>
	  public Square Clone ( ) {
		return new Square(Left, Top, Player);
	  }

    }
}
