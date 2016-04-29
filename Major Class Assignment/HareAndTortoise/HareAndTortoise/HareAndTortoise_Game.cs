using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Class_Library;
using Player_Class_Library;
using Square_Class_Library;
using System.Drawing;
using Die_Class_Library;
using System.Diagnostics;

// CREATE PLAYERS IN THIS CLASS

namespace HareAndTortoise {
    public static class HareAndTortoise_Game {
        private static int numberOfPlayers = 6;
        // possible outcomes chance square
        public const int POSSIBLE_OUTCOMES = 2;
        // Game Dice
        private static Die dieOne = new Die();
        private static Die dieTwo = new Die();
        private static bool gameOver = false;
        private static  string[] playerName = {"One", "Two", "Three", "Four", "Five", "Six"};
        private static Brush[] playerColours = {Brushes.Black, Brushes.Red, Brushes.Gold, Brushes.GreenYellow, 
                                               Brushes.Fuchsia, Brushes.BlueViolet};
        private static BindingList<Player> players = new BindingList<Player>();
        public enum specialState {NORMAL, WIN, LOSE, CHANCE, START, FINISH };

        public static BindingList<Player> Players {
            get {
                return players;
            }
        }

        public static int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }


        public static bool GameOver
        {
            get
            {
                return gameOver;
            }
            set
            {
                gameOver = value;
            }
        }

        public static void SetUpGame(){

            Board.SetUpBoard();
            InitialisePlayers();
           
            //more code to be added later
        }// end SetUpGame

        /// <summary>
        /// Instantiate player objects
        /// </summary>
        public static void InitialisePlayers()
        {
            for (int player = 0; player < numberOfPlayers; player++)
            {
                //create player object and intialise with required variables
                Player _player = new Player(playerName[player], Board.GetGameBoardSquare(0));
                _player.PlayerTokenColour = playerColours[player];
                //load into binding list
                players.Add(_player);
            }
        }

        /// <summary>
        /// Play a single round of the game - if a player lands on an effect square then
        /// the effect of that square is conducted within the same round
        /// </summary>
        /// <returns>returns true or false based on whether the game has ended or not</returns>
        public static bool PlayOneRound()
        {
            // if number of players if less than six then adjust the number of players respectively
            for (int player = 0; player < numberOfPlayers; player++)
            {
                // roll dice, including the chanceDie - it will only be used when needed
                Players[player].RollDice(dieOne, dieTwo);
                // if resulting location is the finish square set gameOver to TRUE
                if (Players[player].Location.GetNumber() == Board.FINISH_SQUARE_NUMBER)
                {
                    // if any player reaches the end then set noRoll to true
                    gameOver = true;
                    return gameOver;
                }
            }
            
            return gameOver;

        }
        /// <summary>
        /// Calculate the winners after the final round by calling helper methods
        /// </summary>
        public static void CalculateWinners()
        {
            // output winners heading
            Trace.WriteLine(String.Format("And the Winner(s) is:\n\n"));
            // use max earning to find overall winner according to rules
            FindWinningPlayers(ReturnMostMoney());
        }

        /// <summary>
        /// find the highest amount of money earned through simple binary search
        /// </summary>
        /// <returns>highest amount of money earned</returns>
        private static int ReturnMostMoney()
        {
            int highestEarning = 0;

            for (int player = 0; player < numberOfPlayers; player++)
            {
                if (Players[player].Money > highestEarning)
                {
                    highestEarning = Players[player].Money;
                }
            }
            return highestEarning;
        }

       /* 
        private static void OutputPlayerState(Player who)
        {

                specialState currentState = specialState.NORMAL;

                if (who.CurrentState == Player.specialState.CHANCE)
                {
                    currentState = specialState.CHANCE;
                }
                else if (who.CurrentState == Player.specialState.WIN)
                {
                    currentState = specialState.WIN;
                }
                if (who.CurrentState == Player.specialState.LOSE)
                {
                    currentState = specialState.LOSE;
                }

                Trace.WriteLine(String.Format("Player {0} landed on {1}",
                    who.Name, currentState));
        }
        */
        
        /// <summary>
        /// Use the highest money value to find the corresponding players,
        /// then from these players find the fatherest on the board.
        /// </summary>
        /// <param name="highestEarning">highest earned</param>
        private static void FindWinningPlayers(int highestEarning)
        {
            int winnerLocation = 0;

            for (int player = 0; player < numberOfPlayers; player++)
            {
                // find players with the highest amount of money
                if (Players[player].Money == highestEarning)
                {
                    // determine the location of that player
                    // if more than one player has the highest amount then winnerlocation
                    // will represent the location of the furtherest player on the board
                    if (winnerLocation <= Players[player].Location.GetNumber())
                    {
                        winnerLocation = Players[player].Location.GetNumber();
                    }
                }          
            }

            // Output winners to tracebox
            for (int player = 0; player < numberOfPlayers; player++)
            {
                if (Players[player].Location.GetNumber() == winnerLocation && Players[player].Money == highestEarning)
                {
                    Players[player].HasWon = true;
                    Trace.WriteLine(String.Format("Player {0} with ${1}",
                               Players[player].Name, Players[player].Money));
                }
            }
        }


        public static void OutputAllPlayerDetails()
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                OutputIndividualDetails(Players[i]);
            }
        } // end OutputAllPlayerDetails

        /// <summary>
        /// Outputs a player's current location and amount of money
        /// pre:  player's object to display
        /// post: displayed the player's location and amount
        /// </summary>
        /// <param name="who">the player to display</param>
        public static void OutputIndividualDetails(Player who)
        {
            if (!gameOver)
            {
                Square playerLocation = who.Location;
                Trace.WriteLine(String.Format("Player {0} on square {1} with {2:C}",
                                               who.Name, playerLocation.GetName(), who.Money));
            }

        }// end OutputIndividualDetails

        
    }//end class
}//end namespace
