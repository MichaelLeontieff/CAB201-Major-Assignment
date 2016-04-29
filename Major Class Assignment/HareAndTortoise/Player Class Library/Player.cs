using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Square_Class_Library;
using System.Drawing;
using Board_Class_Library;
using System.Windows.Forms;
using Die_Class_Library;

namespace Player_Class_Library {

    public class Player {

        // Private variables
        private string name;
        private int money;
        private bool hasWon;
        private Square location;
        private Square previousLocation;
        private Image playerTokenImage;
        private Brush playerTokenColour;
        public int rollNumber = 0;
        //public constants
        public const int MIN_DIE_VALUE = 1;
        public const int MAX_DIE_VALUE = 6;
        public const int POSSIBLE_OUTCOMES = 2;
        private bool Debug = false;
        public enum specialState { NONE, WIN, LOSE, CHANCE, START, FINISH };
        private specialState currentState;
        


        public Player()
        {
            throw new ArgumentException();
        }

        public Player(string name, Square location)
        {
            this.name = name;
            this.location = location;
            this.previousLocation = location;
            this.money = 100;
        }

        /// <summary>
        /// Not used in current implementation, set enum for active effect on player
        /// </summary>
        public specialState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
            }
        }

        /// <summary>
        /// offset player location by parameter amount
        /// </summary>
        /// <param name="distanceOffset">offset amount</param>
        public void OffsetDistance(int distanceOffset)
        {
            int new_location = this.Location.GetNumber() - distanceOffset;
            this.Location = Board.GetGameBoardSquare(new_location);
        }

        /// <summary>
        /// Re-Roll dice for win square
        /// </summary>
        public void ReRollDice()
        {
            Die DieOne = new Die();
            Die DieTwo = new Die();

            RollDice(DieOne, DieTwo);
        }

        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
            }
        }


        
        public bool HasWon
        {
            get
            {
                return hasWon;
            }
            set
            {
                hasWon = value;
            }
        }


        public string Name
        {
            set
            {
                name = value;
            }
            get 
            {
                return name;
            }
            
        }


        public Square Location
        {
            set
            {
                location = value;
            }
            get 
            {
                return location;
            }
        }

        public Square PreviousLocation
        {
            set
            {
                previousLocation = value;
            }
            get
            {
                return previousLocation;
            }
        }

        public Image PlayerTokenImage
        {
            get
            {
                return playerTokenImage;
            }
        }

        public Brush PlayerTokenColour
        {
            get
            {
                return playerTokenColour;
            }
            set
            {
                playerTokenColour = value;
                playerTokenImage = new Bitmap(1, 1);
                using (Graphics g = Graphics.FromImage(playerTokenImage))
                {
                    g.FillRectangle(playerTokenColour, 0, 0, 1, 1);
                }
            }
        }

        /// <summary>
        /// Call Roll Dice methods to roll the dice and load resulting
        /// value into the approprite player object
        /// </summary>
        /// <param name="dieOne">die object</param>
        /// <param name="dieTwo">die object</param>
        public void RollDice(Die dieOne, Die dieTwo)
        {
                int new_position = 0;
                // get current position
                int current_position = location.GetNumber();
                // get new from current + die values
                // I've implemented a simple debug flag which removes random die rolls allowing 
                // different win cases to be easily tested
                if (!Debug)
                {
                    rollNumber = dieOne.Roll() + dieTwo.Roll();
                    // append both random die values to current position
                    new_position = current_position + rollNumber;
                }
                //-------ELSE-------------// hard code die values
                // load location into object if it's over 56 then change location to finish
                if (new_position >= Board.FINISH_SQUARE_NUMBER)
                {
                    location = Board.GetGameBoardSquare(Board.FINISH_SQUARE_NUMBER);
                    new_position = Board.FINISH_SQUARE_NUMBER;
                }
                else
                {
                    location = Board.GetGameBoardSquare(new_position);
                }
                // determine effect
                location.EffectOnPlayer(this);
            
                
            
        }

        /// <summary>
        /// add amount to player money
        /// </summary>
        /// <param name="amount"></param>
        public void Add(int amount)
        {
            this.money += amount;
        }

        /// <summary>
        /// deduct amount from player money
        /// </summary>
        /// <param name="amount"></param>
        public void Deduct(int amount)
        {
            // if deduction results in less than 0
            if (this.money - amount <= 0)
            {
                this.money = 0;
            }
            else
            {
                this.money -= amount;
            }
        }
    }
}
