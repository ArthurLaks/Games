using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace Checkers {
    public struct Move {
	  /// <summary>
	  /// The piece that was moved.
	  /// </summary>
	  public Piece PieceMoved;
	  /// <summary>
	  /// The square that the piece moved to.
	  /// </summary>
	  public Square Destination;
	  /// <summary>
	  /// The AI's rating of the current move.  For use by the AI only.
	  /// </summary>
	  public int Rating;
	  /// <summary>
	  /// The pieces jumped by the move.
	  /// </summary>
	  public List<Piece> PiecesJumped;
	  public Move (Piece pieceMoved, Square destination, List<Piece> piecesJumped, int rating) {
		PieceMoved = pieceMoved;
		Destination = destination;
		PiecesJumped = piecesJumped;
		Rating = rating;
	  }
    }
    public partial class Game : Form {
	  private Square[,] board = new Square[8, 8];		//A 2-dimensional array contianing all the squares in the board.
	  private Dictionary<Players, int> piecesOf = new Dictionary<Players, int>( );	    //A dictionary of the number of pieces of each player.
	  private Piece selectedPiece;	    //Stores the selected square.  If no square is selected then it is null.
	  /// <summary>
	  /// Contains all the squares that the piece on selectedPiece can move to as keys and the piece that would be jumped if that piece move there as values.
	  /// </summary>
	  private Dictionary<Square, List<Piece>> possibleMoves = new Dictionary<Square, List<Piece>>( );
	  public Game ( ) {
		InitializeComponent( );
		//Initialize the squares in Board.
		Colors cColor = Colors.Red;	    //This variable alternates between black and red, making every other square black.
		for (int column = 0; column < 8; column++) {	    //For each column
		    for (int row = 0; row < 8; row++) {	    //For each row.
			  board[column, row] = new Square(column, row, cColor);		  //Initialize the square of the current player in the current column and row.
			  cColor = (cColor == Colors.Black ? Colors.Red : Colors.Black);		//Reverses the value of cColor.  If cColor was black then the condition is true and the terciary operator returns red.
		    }
		    cColor = (cColor == Colors.Black ? Colors.Red : Colors.Black);		//Reverse the player of cColor again so that the first square in each column is the opposite player of the first square of previous column
		}
		CreatePieces( );	  //Create and arrange the pieces.
		MaximumSize = new Size(64 * 8, 64 * 8 + statusStrip.Height);		//Do not allow the images of pieces to be stretched if the user expands the form too much.
		statusStripLabel.Text = "It is your turn.  Click on the piece you wants to move to select it.";
	  }

	  private void panel_Resize (object sender, EventArgs e) {
		panel.Invalidate( );		//If the form is resized then redraw the panel at the current size.  (The panel is docked to the form.)
	  }

	  private void panel_Paint (object sender, PaintEventArgs e) {
		//Paint the squares.
		foreach (Square item in board) {
		    //If the square is black then paint a black square.  If the square is red then paint a red square.
		    Brush color;	    //The player of the square
		    RectangleF bounds = new RectangleF((panel.Width / 8f) * item.Left, (panel.Height / 8f) * item.Top, panel.Width / 8f, panel.Height / 8f);		//The location and size of the square.  It should be an eighth of the width of the panel wide and an eighth of the height of the panel tall.  Square.Left and Top are the number of squares from the top and right of the board respectively that the square is.  Therefore, the width of a square (panel.Width/8) times square.Left equals the number of pixels from the left of the panel that the square should be.
		    if (item.Color == Colors.Black) {
			  //If the square is selected then make it gold (it is only possible to select a black square).  If the piece on the selected square can move there then make it purple.  If it is neither condition is true then make it black.
			  if (item.Piece != null && item.Piece == selectedPiece)
				color = Brushes.Gold;
			  else
				if (possibleMoves.ContainsKey(item))
				    color = Brushes.Purple;
				else
				    color = Brushes.Black;
		    } else
			  color = Brushes.Red;
		    e.Graphics.FillRectangle(color, bounds);	    //Draw a square of the appropriate player at the appropriate location.
		    //If there is a piece on the square then draw an image of the appropriate piece on the square.
		    if (item.Piece != null) {	  //This statement is necessary because if item.Piece is null then item.Piece.Player will throw an exception.
			  if (item.Piece.Player == Players.Red) {	  //If the piece is red
				if (item.Piece.King)		//If the piece is a king then draw an image of a king.
				    e.Graphics.DrawImage(Properties.Resources.RedKing, bounds);
				else 	    //If it is not a king then draw an image of a regular piece.
				    e.Graphics.DrawImage(Properties.Resources.Red, bounds);
			  }
			  if (item.Piece.Player == Players.Black) {
				if (item.Piece.King)
				    e.Graphics.DrawImage(Properties.Resources.BlackKing, bounds);
				else
				    e.Graphics.DrawImage(Properties.Resources.Black, bounds);
			  }
		    }
		}
	  }
	  private void panel_MouseClick (object sender, MouseEventArgs e) {
		Square squareClicked = board[e.X / (panel.Width / 8+1), e.Y / (panel.ClientSize.Height / 8+1)];	    //e.X and e.Y are measured in pixels.  In order to convert them into coordinates (the number of squares from the left or top of the form), they are divided by the width or height of each square, which is an eighth of the width or height of the form.

		//If  the user tries to move the piece in selectedPiece to a square that it cannot move to then give an error.
		if (selectedPiece != null && !possibleMoves.ContainsKey(squareClicked)
		    && !(squareClicked.Piece != null && squareClicked.Piece.Player == Players.Red)) {	//If the user selected a square and then decided to select another of his squares then do not give an error.
		    SystemSounds.Beep.Play( );
		    statusStripLabel.Text = "The selected piece cannot move to that square.  Click on a highlighted square or select a piece that can move there";
		}

		//If it is the user tries to select one of the computer player's pieces then give an error.
		if (selectedPiece == null && squareClicked.Piece != null && squareClicked.Piece.Player == Players.Black) {
		    SystemSounds.Beep.Play( );
		    statusStripLabel.Text = "You cannot select the computer player's pieces.";
		}
		//If no square is selected or the user clicked on another of his squares then select the square that the user clicked on.
		//If the user is selecting a square.
		if (squareClicked.Piece != null	    //If squareClicked has a piece in it
		    && squareClicked.Piece.Player == Players.Red) {   // and  that piece belongs to the human player.
		    possibleMoves = Library.PossibleMoves(board, squareClicked.Piece);	    //Find all possible moves from squareClicked.
		    if (possibleMoves.Count == 0) {	    //If the piece on squareClicked cannot be moved then do not select it.
			  SystemSounds.Beep.Play( );
			  statusStripLabel.Text = "This piece cannot be moved.";
		    } else {		//If the piece on squareClicked can be moved
			  //Select the piece on squareClicked.
			  selectedPiece = squareClicked.Piece;
			  statusStripLabel.Text = "Click on the highlighted square that you want to move the selected piece to.";
		    }
		}

		//If a square is selected and the user clicked on a square that he can move the piece on selectedPiece to then move the piece to squareClicked and if he jumped a piece then remove it.
		if (selectedPiece != null && possibleMoves.ContainsKey(squareClicked)) {
		    selectedPiece.Location.Piece = null;		  //Delete the piece on selectedPiece's previos location so that it isn't copied.
		    squareClicked.Piece = selectedPiece;	  //Move the piece on selectedPiece to squareClicked.
		    List<Piece> piecesJumped = possibleMoves[squareClicked];		  //A list of the  pieces that would be jumped if the piece on selectedPiece is moved to squareClicked.
		    foreach (Piece item in piecesJumped) {	    //For each piece that would be jumped.  If no pieces are jumped then does nothing
			  piecesOf[Players.Black]--;	//Subtract 1 from piecesOf[Players.Black] to reflect the absence of one of the computer player's pieces.
			  item.Location.Piece = null;	  //Remove pieceJumped from the square that it is on.
		    }
		    //If the selected piece moved to the top of the board (the huan player's pieces start out on the bottom of the board) then make it a king.
		    if (squareClicked.Top == 0)
			  squareClicked.Piece.King = true;
		    //Unselect the selected piece and clear possibleMoves.
		    selectedPiece = null;
		    possibleMoves.Clear( );
		    //Determine if the human player won.
		    if (piecesOf[Players.Black] == 0) {	//If the computer player has no pieces left the the human player won.
			  Victory("Congratulations! You won.");
			  return;
		    }
		    statusStrip.Text = "Please wait while the computer player makes his move.";
		    panel.Invalidate( );
		    Thread.Sleep(500);
		    //Make the computer player move.
		    Move response = AI.Go(board);	    //The computer player's response to the human player's move.
		    response.PieceMoved.Location.Piece = null;			//Remove the pieces on the square that pieceMoved used to be so it is not copied.
		    response.PieceMoved.Location = response.Destination;		//Move the piece to its destination.
		    //If that move jumps pieces then delete those pieces.
		    foreach (Piece pieceJumped in response.PiecesJumped) {	    //For each of the pieces that the computer player's move would have jumped
			  pieceJumped.Location.Piece = null;		//Delete that piece.
			  piecesOf[Players.Red]--;			//Subtract 1 from piecesOf[Players.Red] to reflect the absence of one of the human player's pieces
		    }
		    //If response.PieceMoved moved to the bottom of the board (the computer player's pieces start out on the top of the board) then make it a king.
		    if (response.PieceMoved.Location.Top == 7)
			  response.PieceMoved.King = true;
		    if (piecesOf[Players.Red] == 0) {		//If the human player has no pieces left then the computer player won.
			  Victory("Ha Ha Ha! You lost!");
			  return;
		    }
		    statusStripLabel.Text = "It is your turn.  Click on the piece you want to move to select it.";
		}
		panel.Invalidate( );		//Repaint the form so that the changes will be visible.
	  }

	  /// <summary>
	  /// This function should be called when the specified player wins.
	  /// </summary>
	  /// <param name="text">The text of the dialog box before "Do you want to start a new game?"</param>
	  private void Victory (string text) {
		panel.Invalidate( );		//Repaint the panel so that the user can see the situation while the dialog box exists.
		//Inform the user that he won and ask him if the wants to start a new game.  If he does then start a new game.  If he does not then exit the application.
		switch (MessageBox.Show(text + "Do you want to start a new game?", "Victory!", MessageBoxButtons.YesNo)) {	    //Perform the switch on the dialogResult, which is the return value of MessageBox.Show.
		    case DialogResult.No:	  //If the user clicked no then quit.
			  Application.Exit( );
			  return;
		    case DialogResult.Yes:		//If the user click yes then start a new game.
			  CreatePieces( );	    //Reinitialize the pieces at their proper places.
			  break;
		}
	  }
	  /// <summary>
	  /// Creates all the pieces and arranges them like the way they are supposed to be at the beginning of the game
	  /// </summary>
	  private void CreatePieces ( ) {
		//Recreate piecesOf with twelve pieces for each player.
		piecesOf.Clear( );
		piecesOf.Add(Players.Red, 12);
		piecesOf.Add(Players.Black, 12);
		//For each sqaure, if it is black and in the right location then create a square in it.
		foreach (Square item in board) {
		    if (item.Color == Colors.Black) {	//If the square is red then it shouldn't have a piece on it.
			  item.Piece = null;	    //If this function is called the start a new game then it should delete all of the old pieces.
			  Players? player = null;	    //The player of the new piece.  It should start out null and if the square should have a piece in it then it should be assigned.
			  //If the square is above the fourth row then it should have a black piece.
			  if (item.Top <= 2)
				player = Players.Black;
			  //If the square is below the fifth row then it should have a red piece.
			  if (item.Top >= 5)
				player = Players.Red;
			  if (player.HasValue) 		//If player is not null (if the square should have a piece on it).
				item.Piece = new Piece(player.Value, board[item.Left, item.Top]);		//Create a new piece on the square.
		    }
		}
	  }
    }
}
