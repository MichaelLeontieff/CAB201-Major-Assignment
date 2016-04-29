using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_Class_Library
{
    public class Win_Square : Square
    {
        private const int ADD_AMOUNT = 15;

        public Win_Square(string name, int number) : base(name, number)
        {

        }

        public override void EffectOnPlayer(Player_Class_Library.Player who)
        {
            base.EffectOnPlayer(who);
            // apply effects
            who.Add(ADD_AMOUNT);
            // re-roll dice
            who.ReRollDice();

        }
    }
}
