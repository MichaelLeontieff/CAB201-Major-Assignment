using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Square_Class_Library;

namespace Board_Class_Library {

    public static class Board {

        // Private cnostants
        public const int START_SQUARE_NUMBER = 0;
        public const int FINISH_SQUARE_NUMBER = 55;
        public const int CHANCE_SQUARE_FACTOR = 6;
        public const int WIN_SQUARE_FACTOR = 5;
        public const int LOSE_SQUARE_FACTOR = 10;
        public const int PLAYING_SQUARES = 54;
        public const int TOTAL_NUMBER_OF_SQUARES = 56;
        
        // Private variables
        private static Square[] gameBoard = new Square[TOTAL_NUMBER_OF_SQUARES];
        
        public static void SetUpBoard()
        {
            for (int square = 0; square < TOTAL_NUMBER_OF_SQUARES; square++)
            {
                if (square == START_SQUARE_NUMBER)
                {
                    gameBoard[square] = new Square("Start", square);
                }
                else if (square % LOSE_SQUARE_FACTOR == 0)
                {
                    gameBoard[square] = new Lose_Square(square.ToString(), square);
                }
                else if (square % CHANCE_SQUARE_FACTOR == 0)
                {
                    gameBoard[square] = new Chance_Square(square.ToString(), square);
                }
                else if (square % WIN_SQUARE_FACTOR == 0 && square % LOSE_SQUARE_FACTOR != 0 && square != FINISH_SQUARE_NUMBER)
                {
                    gameBoard[square] = new Win_Square(square.ToString(), square);
                }
                else if (square == FINISH_SQUARE_NUMBER)
                {
                    gameBoard[square] = new Square("Finish", square);
                }
                else
                {
                    gameBoard[square] = new Square(square.ToString(), square);
                }
            }
        }

        public static Square GetGameBoardSquare(int square_number)
        {
            if (square_number >= 0 && square_number < TOTAL_NUMBER_OF_SQUARES)
            {
                return gameBoard[square_number];
            }
            else
            {
                throw new Exception("Square number out of range");
            }
            
        }

        public static Square StartSquare()
        {
            return gameBoard[START_SQUARE_NUMBER];
        }

        public static Square NextSquare(int square_number)
        {
            if (square_number < FINISH_SQUARE_NUMBER)
            {
                return gameBoard[square_number + 1];
            }
            else
            {
                throw new Exception("Square not found!");
            }
        }

    }
}
