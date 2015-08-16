using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers {
    public enum Players {
	  Black, Red
    }
    /// <summary>
    /// A checkers piece.
    /// </summary>
    public class Piece {
	  private Players player;	  //Stores the value of the player property.
	  /// <summary>
	  /// Returns the player of this piece.  Is read-only.
	  /// </summary>
	  public Players Player {
		get {
		    return player;
		}
	  }
	  /// <summary>
	  /// Stores the value of the Location property.
	  /// </summary>
	  private Square location;
	  /// <summary>
	  /// Gets or sets the square that the piece is on.
	  /// </summary>
	  public Square Location {
		get {
		    return location;
		}
		set {
		    if (location != value) {	  //If the value of the property is being changed.
			  location = value;
			  location.Piece = this;		//Make sure that Location's Piece property equals this piece.
		    }
		}
	  }
	  private Boolean king = false;		//Stores the value of the King property.  It starts out false.
	  /// <summary>
	  /// Gets or sets whether this piece was kinged (by reaching the other side of the board).
	  /// </summary>
	  public bool King {
		get {
		    return king;
		}
		set {
		    king = value;
		}
	  }
	  /// <summary>
	  /// Initializes a piece of the specified player on the specified square.
	  /// </summary>
	  /// <param name="player">The player of the new piece.</param>
	  /// <param name="location">The square that the new piece should start out on.</param>
	  public Piece (Players player, Square location) {
		this.player = player;		//Assigns the private variable player to the parameter player.
		this.Location = location;	    //Assigns the private variable location to the parameter location.
	  }
    }
}
