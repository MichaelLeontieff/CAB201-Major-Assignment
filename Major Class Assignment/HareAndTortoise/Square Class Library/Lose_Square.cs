using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Class_Library;

namespace Square_Class_Library
{
    public class Lose_Square : Square
    {
        private const int DEDUCT_AMOUNT = 25;
        private const int DISTANCE_OFFSET = -3;
        public Lose_Square(string name, int number) : base(name, number)
        {

        }

        public override void EffectOnPlayer(Player_Class_Library.Player who)
        {
            base.EffectOnPlayer(who);
            // apply effects
            who.Deduct(DEDUCT_AMOUNT);
            who.OffsetDistance(DISTANCE_OFFSET);
        }
    }

    

}
