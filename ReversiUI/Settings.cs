using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReversiUI
{
    class Settings
    {
        static Settings instance;
        public static Settings GetInstance()
        {
            if (instance == null)
            {
                instance = new Settings();
            }
            return instance;
        }

        private int level;

        Settings()
        {
           level = 5;
        }

        public Player User;

        public HeuristicsEnum Heuristics = HeuristicsEnum.MobilityAndStability;

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Уровень должен натуральным числом");
                }
                level = value;
            }

        }
    }
}
       
 