using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Class_Library;

namespace Square_Class_Library
{
    public class Chance_Square : Square
    {
        private const int POSSIBLE_OUTCOMES = 2;
        private const int ADD_DEDUCT_AMOUNT = 50;
        private const int WIN_DISTANCE_OFFSET = 5;
        private const int LOSE_DISTANCE_OFFSET = 5;
        Die_Class_Library.Die chanceDie = new Die_Class_Library.Die(POSSIBLE_OUTCOMES);
        

        public Chance_Square(string name, int number) : base(name, number)
        {

        }
        /// <summary>
        /// Effect on player, rolls modified die and calls only player methods
        /// </summary>
        /// <param name="who">player</param>
        public override void EffectOnPlayer(Player_Class_Library.Player who)
        {
            base.EffectOnPlayer(who);
          
            // roll dice
            chanceDie.Roll();

            // if value is one then add else deduct
            if (chanceDie.FaceValue == 1)
            {
                // apply position offset
                who.Add(ADD_DEDUCT_AMOUNT);
                who.OffsetDistance(WIN_DISTANCE_OFFSET);
            }
            else 
            {
                // apply position offset
                who.Deduct(ADD_DEDUCT_AMOUNT);
                who.OffsetDistance(LOSE_DISTANCE_OFFSET);
            }

        }
    }
}
