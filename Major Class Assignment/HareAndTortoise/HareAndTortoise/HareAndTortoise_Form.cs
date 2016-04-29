using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Board_Class_Library;
using Square_Class_Library;
using Player_Class_Library;
using System.Diagnostics;

namespace HareAndTortoise {
    public partial class HareAndTortoise_Form : Form {

        const int NUM_OF_ROWS = 8; 
        const int NUM_OF_COLUMNS = 7;
        const int NUM_OF_PLAYERS = 6;
        const int START_SQUARE = 0;
        const int COMBOBOX_OFFSET = 2;
        const bool KEEP = true;
        const bool REMOVE = false;
        private Square Current_Square;
        private SquareControl Square_Box;



        public HareAndTortoise_Form() {
            InitializeComponent();
            HareAndTortoise_Game.SetUpGame();
            ResizeGameBoard();
            SetUpGuiGameBoard();
            DataGridView.DataSource = HareAndTortoise_Game.Players;
            UpdatePlayerSquares(KEEP);
            Trace.Listeners.Add(new ListBoxTraceListener(lbDebug));
            // Set default value of player count
            cbNumberOfPlayers.SelectedIndex = cbNumberOfPlayers.FindString("6");
        }

        /// <summary>
        /// loads the appropriate square control object into each cell of the table layout panel
        /// </summary>
        private void SetUpGuiGameBoard()
        {
            // for each row
            for (int row = 0; row < NUM_OF_ROWS; row++)
            {
                // for each column
                for (int column = 0; column < NUM_OF_COLUMNS; column++)
                {
                    Current_Square = Board.GetGameBoardSquare(column + (row * NUM_OF_COLUMNS));
                    Square_Box = new SquareControl(Current_Square, HareAndTortoise_Game.Players);
                    if (Current_Square.GetName() == "Start" || Current_Square.GetName() == "Finish")
                    {
                        Square_Box.BackColor = Color.BurlyWood;
                    }
                    // if row number is divisible by 2, orientate squares left to right
                    if (row % 2 == 0)
                    {
                        gameBoardPanel.Controls.Add(Square_Box, column, (NUM_OF_ROWS - 1) - row);
                    }
                    // if row number is divisible by 2, orientate squares right to left
                    else
                    {
                        gameBoardPanel.Controls.Add(Square_Box, (NUM_OF_COLUMNS - 1) - column, (NUM_OF_ROWS - 1) - row);
                    }
                }
            }
        }//end SetUpGuiGameBoard()

        

        private void ResizeGameBoard() {
            const int SQUARE_SIZE = SquareControl.SQUARE_SIZE;
            int currentHeight = gameBoardPanel.Size.Height;
            int currentWidth = gameBoardPanel.Size.Width;
            int desiredHeight = SQUARE_SIZE * NUM_OF_ROWS;
            int desiredWidth = SQUARE_SIZE * NUM_OF_COLUMNS;
            int increaseInHeight = desiredHeight - currentHeight;
            int increaseInWidth = desiredWidth - currentWidth;
            this.Size += new Size(increaseInWidth, increaseInHeight);
            gameBoardPanel.Size = new Size(desiredWidth, desiredHeight);
        } //end ResizeGameBoard

        
        /// <summary>
        /// Update each square object in which players reside
        /// </summary>
        /// <param name="clear">determines whether the update is clearing players or adding them</param>
        public void UpdatePlayerSquares(bool clear)
        {
            for (int player = 0; player < HareAndTortoise_Game.NumberOfPlayers; player++)
            {
                // Get location (square object) of corresponding player object
                Square _Player_location = HareAndTortoise_Game.Players[player].Location;
                // Get corresponding square number from Square
                int square_number = _Player_location.GetNumber();
                // Get the control object of square number by finding its row and column
                SquareControl _Control = (SquareControl)GetControlOfSquareNumber(square_number);
                // Set corresponding bool of ContainsPlayers to true
                if (clear)
                {
                    _Control.ContainsPlayers[player] = true;
                }
                else
                {
                    _Control.ContainsPlayers[player] = false;
                }
            }
            // Re-display game board
            gameBoardPanel.Invalidate(true);
        }
         

        /// <summary>
        /// Returns the control of the corresponding square number
        /// </summary>
        /// <param name="square_number">location of control respective of number not row/column</param>
        /// <returns>Control of respective square</returns>
        private Control GetControlOfSquareNumber(int square_number)
        {
            int square_counter = 0;

            for (int row = 0; row < NUM_OF_ROWS; row++)
            {
                for (int column = 0; column < NUM_OF_COLUMNS; column++)
                {                    
                    if (square_counter == square_number)
                    {
                        if (row % 2 == 0)
                        {
                            return gameBoardPanel.GetControlFromPosition(column, (NUM_OF_ROWS - 1) - row);
                        }
                        else
                        {
                            return gameBoardPanel.GetControlFromPosition((NUM_OF_COLUMNS - 1) - column, (NUM_OF_ROWS - 1) - row);
                        }      
                    }
                    square_counter++;
                }
            }
            throw new ArgumentOutOfRangeException("Corresponding Control Object Not Found!");
        }

        private void btnRollDice_Click(object sender, EventArgs e)
        {
            // update form elements
            cbNumberOfPlayers.Enabled = false;
            btnReset.Enabled = true;
            // Remove previous player tokens
            UpdatePlayerSquares(REMOVE);

            // Call method to play one round - if true game is over
            if (HareAndTortoise_Game.PlayOneRound())
            {
                HareAndTortoise_Game.CalculateWinners();
                btnReset.Enabled = true;
                btnRollDice.Enabled = false;
            }
            // Update squares
            UpdatePlayerSquares(KEEP);
            OutputPlayersDetails();
            UpdateDataGridView();


        }

        /// <summary>
        /// output all player details
        /// </summary>
        private void OutputPlayersDetails() 
        {
            HareAndTortoise_Game.OutputAllPlayerDetails();
            lbDebug.Items.Add("");
            lbDebug.SelectedIndex = lbDebug.Items.Count - 1;
        }

        private void UpdateDataGridView()
        {
            HareAndTortoise_Game.Players.ResetBindings();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you really want to Exit?", "Are you sure?", 
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

       
        /// <summary>
        /// Reset all players to their defaults for new game
        /// </summary>
        private void ResetPlayers()
        {
            lbDebug.Items.Clear();

            for (int player = 0; player < NUM_OF_PLAYERS; player++)
            {
                // reset game status
                HareAndTortoise_Game.GameOver = false;
                // reset money
                HareAndTortoise_Game.Players[player].Money = 100;
                // reset winner status
                HareAndTortoise_Game.Players[player].HasWon = false;
                // reset location
                HareAndTortoise_Game.Players[player].Location = Board.GetGameBoardSquare(START_SQUARE);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // re-enable combo box
            cbNumberOfPlayers.Enabled = true;
            // re-enable roll dice button
            btnRollDice.Enabled = true;
            // call worker methods
            UpdatePlayerSquares(REMOVE);
            ResetPlayers();
            UpdatePlayerSquares(KEEP);
            DataGridView.Refresh();
        }

        /// <summary>
        /// upon value change for combobox, modify active players
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbNumberOfPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlayerSquares(REMOVE);
            // determine the number of players in the round
            HareAndTortoise_Game.NumberOfPlayers = cbNumberOfPlayers.SelectedIndex + COMBOBOX_OFFSET;
            // render appropriate number of tokens based on selection
            UpdatePlayerSquares(KEEP);
        }

    }//end class 
} //end namespace
