using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Player_Class_Library;

namespace Square_Class_Library {
   
    public class Square {

        // Class Variables

        private string name;
        private int number;


        // Default Constructor
        public Square()
        {
            throw new ArgumentException("Access parameterless constructor");
        }

        // Main Constructor
        public Square(string name, int number)
        {
            this.name = name;
            this.number = number;
        }

        public string GetName()
        {
            return name;
        }

        public int GetNumber()
        {
            return number;
        }

        public virtual void EffectOnPlayer(Player who) 
        {
            
        }

    }
}
