using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;


namespace Othello {
    public partial class Othello : Form {
	  private Square[,] squares = new Square[8, 8];		//Contians all the squares in the board.
	  private Button[,] buttons = new Button[8, 8];		//Contains all the buttons, which correspond to the squares.
	  private readonly Size buttonSize = new Size(50, 50);	    //The size of each button.
	  private Dictionary<Players, List<Square>> squaresOf = new Dictionary<Players, List<Square>>( );	//Contains three lists, one of which contains all empty squares, one of which contains all black squares, and one of which contains all white squares.
	  private Players turn = Players.Black;
	  private Players Turn {
		get {
		    return turn;
		}
		set {
		    turn = value;
		    Text = string.Format("Othello {0}'s turn", turn);
		}
	  }
	  public Othello ( ) {
		InitializeComponent( );
		//Initialize the lists in squaresOf
		squaresOf[Players.Black] = new List<Square>( );
		squaresOf[Players.White] = new List<Square>( );
		squaresOf[Players.Empty] = new List<Square>( );
		//Initialize the squares and the buttons and set their properties.
		for (int left = 0; left < 8; left++) {	  //For each column
		    for (int top = 0; top < 8; top++) {	//For each row
			  Square squareToCreate = new Square(left, top);
			  squares[left, top] = squareToCreate;		//Add square to squares
			  squareToCreate.PlayerChanged += OnPlayerChanged;	    //Set the PlayerChanged event to OnPlayerChanged
			  Button buttonToCreate = new Button( ) { 
				Size = buttonSize,
				//Position the button so that it is right of the square left of it and below the square on top of it.
				Left = left * buttonSize.Width,
				Top = top * buttonSize.Height,
				BackColor = Pens.Green.Color,   //Set the color to green
				Tag = squareToCreate	    //The tag of each button should be the corresponding square.
			  };
			  buttonToCreate.Paint += button_Paint;
			  buttonToCreate.Click += button_Click;
			  buttons[left, top] = buttonToCreate;	  //Add button to buttons
			  Controls.Add(buttonToCreate);
			  squaresOf[Players.Empty].Add(squareToCreate);
		    }
		}
		squares[3, 3].Player = squares[4, 4].Player = Players.White;	//The square to the top-left and bottom-right of the center of the board should start out white
		squares[4, 3].Player = squares[3, 4].Player = Players.Black;	    //The square to the top-right and bottom-left of the center of the board should start out black.
	  }

	  private void OnPlayerChanged (Square sender, Players oldValue) {
		buttons[sender.Left, sender.Top].Invalidate( );	    //Invalidate the control so that the circle in the control is redrawn based on the new value of sender.Player
		//Modify the appropriate lists.
		squaresOf[oldValue].Remove(sender);	    //Remove the square from the list that it used to be in
		squaresOf[sender.Player].Add(sender);	    //Add the square to the list that it should now be in
		blackScore.Text = squaresOf[Players.Black].Count.ToString( );	    //Update the text of the status bar to show the number of squares that belong to each player.
		whiteScore.Text = squaresOf[Players.White].Count.ToString( );
	  }
	  /// <summary>
	  /// Paints a circle in the button that is color of the square.
	  /// </summary>
	  /// <param name="sender"></param>
	  /// <param name="e"></param>
	  void button_Paint (object sender, PaintEventArgs e) {
		Button buttonToPaint = (Button)sender;
		switch (((Square)(buttonToPaint).Tag).Player) {		//Casts sender to button and sender.Tag to square
		    case Players.Empty:	    //If the button is empty then don't draw anything
			  return;
		    case Players.White:	    //If the square is white then draw a white circle on that square
			  e.Graphics.FillEllipse(Brushes.White, buttonToPaint.ClientRectangle);
			  break;
		    case Players.Black:	    //If the square is black then draw a black circles on that square
			  e.Graphics.FillEllipse(Brushes.Black, buttonToPaint.ClientRectangle);
			  break;
		}
	  }
	  private void button_Click (object sender, EventArgs e) {
		//Determines which squares will be flipped by this move.  If any squares would be flipped then flip those squares and give the square the user clicked on to him.  Determines if the computer player can make a legal move the next turn.  If it can then switch turn to white (the computer player).  If it can't then determine if the user can make a legal move the next turn.  If he can then turn is not switched.  If he can't then deremines who won.
		//If it is the computer player's turn then do not allow the user to move.
		if (turn == Players.White) {
		    MessageBox.Show("It is the computer player's turn.  Click Go or press any key to make it go.", "Othello");
		    return;
		}
		Square squareClickedOn = (Square)((Button)sender).Tag;		//sender is casted to Button and sender.Button is casted to Square.
		if (squareClickedOn.Player != Players.Empty) { 	  //If the square was already taken.
		    MessageBox.Show("Illegal Move.  The square already belongs to a player.  Go again.", "Othello");
		    return;
		}
		List<Square> squaresToFlip = OthelloLibrary.SquaresFlipped(squares, squareClickedOn, Players.Black);
		if (squaresToFlip.Count == 0) {	  //If no squares would have been overtuned by this move.
		    MessageBox.Show("Illegal move.  You would not have overturned any tokens.  Go again", "Othello");
		    return;
		}
		//Flip the appropriate squares.
		foreach (Square cSquare in squaresToFlip) {
		    cSquare.Player = Players.Black;
		}
		//Give the square that the user clicked on to him.
		squareClickedOn.Player = Players.Black;
		//Modify the progress bar to reflect the fact that there is one fewer turn left.
		progress.PerformStep( );
		//Call NextTurn to determine whose turn it should be.
		switch (NextTurn(turn)) {
		    case Players.White:
			  //If white (the computer player) should go next then switch turn to White.
			  Turn = Players.White;
			  break;
		    case Players.Black:
			  //If NextTurn returned black then white reliquished a turn.  Inform the user but don't switch turn.
			  MessageBox.Show("The computer player cannot make a legal move and relinquished its turn.  Go again", "Othello");
			  break;
		    case Players.Empty:
			  //If NextTurn returned empty then no player can move and the game is over.
			  EndGame( );
			  break;
		}
	  }
	  /// <summary>
	  /// Makes the computer player move and flips the appropriate squares.
	  /// </summary>
	  private void MoveAI ( ) {
		if (Turn == Players.Black) {		//If it is not the computer player's turn then ask the user to move.
		    MessageBox.Show("It is your turn.  Choose a square", "Othello");
		    return;	  //Do not make the AI move.
		}
		Square squareAIChoose = AI.Go(squares);	//Make the AI choose a square.
		List<Square> squaresFlipped = OthelloLibrary.SquaresFlipped(squares, squareAIChoose, Players.White);		//Determine which squares will be flipped by this move.
		if (squaresFlipped.Count > 0 && squareAIChoose.Player == Players.Empty) {	    //If it is a valid move (if it choose an empty square that will flip at least one square).  If it is an invalid move then throw an exception.
		    //Flip the appropriate squares.
		    squareAIChoose.Player = Players.White;
		    foreach (Square item in squaresFlipped) {
			  item.Player = Players.White;
		    }
		    progress.PerformStep( );		//Update the progress bar to reflect the fact that the computer player went.
		} else	    //If the move is invalid then throw an exception.
		    throw new Exception("The AI choose an invalid move.");
		//Determine who should get the next turn and a player won.
		switch (NextTurn(Players.White)) {
		    case Players.Black:	    //If human player has a legal move then switch Turn to black.
			  Turn = Players.Black;
			  break;
		    case Players.White:	    //If the human player relinquished a turn then inform the user and do not change Turn.
			  MessageBox.Show("You have no legal moves and relinquished a turn.   Click Go or press any key to make the computer player move again.", "Othello");
			  break;
		    case Players.Empty:	    //If NextTurn returned empty then neither player has a legal move and the game is over.
			  EndGame( );
			  break;
		}
		progress.PerformStep( );
	  }
	  /// <summary>
	  /// Makes the computer player move.
	  /// </summary>
	  /// <param name="sender"></param>
	  /// <param name="e"></param>
	  private void Othello_KeyPress (object sender, KeyPressEventArgs e) {
		MoveAI( );
	  }
	  private void Go_Click (object sender, EventArgs e) {
		MoveAI( );
	  }

	  ///<summary>
	  /// Determines which player should move after the specified player moved (or if a player relinquished a turn).
	  /// </summary>
	  /// <param name="justWent">The player who went most recently.</param>
	  /// <returns>The player who should move after the specified player went.  Returns empty if neither player can move and the game is over.</returns>
	  private Players NextTurn (Players justWent) {
		//Determine if the opponent of justWent can make a legal move the next turn.  If he can then return Opponent(justWent).
		foreach (Square cSquare in squaresOf[Players.Empty])
		    //If a square would be flipped by that move then return Opponent(justWent).
		    if (OthelloLibrary.SquaresFlipped(squares, cSquare, OthelloLibrary.Opponent(justWent)).Count > 0)
			  return OthelloLibrary.Opponent(justWent);

		//If the condition it the foreach loop was never true then Opponent(justWent) cannot move the next turn, so return justWent, if he has a legal move.
		//Determine justWent has a legal move.  If he does then the Opponent(justWent) relinquishes a turn, so return justWent.  If he does not then the game is over, so return Players.Empty.
		foreach (Square cSquare in squaresOf[Players.Empty])
		    if (OthelloLibrary.SquaresFlipped(squares, cSquare, justWent).Count > 0)
			  return justWent;
		//If neither player can move then return Players.Empty.
		return Players.Empty;
	  }
	  /// <summary>
	  /// Determines who won and displays a dialog box asking if the user wants to start a new game.
	  /// </summary>
	  private void EndGame ( ) {
		//Determine who won.  Display a dialog box displaying an appropriate message and asking the user if he wants to start the new game.  If he clicks Yes then starts a new game.  If he clicks no then closes the form.
		string message=null;	    //Stores the message that it should say in the dialog box.  The message depends on the outcome of the game.   It starts out null so that it will not be an "unassigned local variable".
		//Determine which player has more squares
		if (squaresOf[Players.Black].Count > squaresOf[Players.White].Count) 	  //If black has more squares than white then black won
		    message = "Congradulations!  You won.";

		if (squaresOf[Players.White].Count > squaresOf[Players.Black].Count)
		    message = "Ha Ha Ha! You lost.";

		//If Black and White have the same number of squares then the game is tied.
		if (squaresOf[Players.Black].Count==squaresOf[Players.White].Count)
		    message = "Tie game";
		switch (MessageBox.Show(String.Format("{0}  Do you want to start a new game?", message), "Othello", MessageBoxButtons.YesNo)) {
		    case DialogResult.No:	  //If the user clicked No
			  Close( );	    //Close the form and exit the game.
			  break;
		    case DialogResult.Yes:
			  //Start a new game
			  //Make every squares empty
			  foreach (Square cSquare in squares) {
				cSquare.Player = Players.Empty;
			  }
			  progress.Value = progress.Minimum;		//Resets the progress bar to reflect the fact the all squares but four are empty.
			  Turn = Players.Black;		//Black should move first in the new game.
			  //squares[3.3] and squares[4,4] should start out belonging to white and squares[3,4] and squares[4,3] should start out belonging to black
			  squares[3, 3].Player = squares[4, 4].Player = Players.White;
			  squares[3, 4].Player = squares[4, 3].Player = Players.Black;

			  break;
		}
	  }

      private void Othello_Load(object sender, EventArgs e)
      {

      }


    }
}