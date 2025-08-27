using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class GameLogic
    {
        //All numbers from the real roulette table that are paired with the color black
        private static readonly int[] WinColorBlack =
            {15, 4, 2, 17, 6, 13, 11, 8, 10, 24, 33, 20, 31, 22, 29, 28, 35, 26 };

        //Random number gen
        private static readonly Random random = new Random();

        public (int Number, string Color) WinGenerator()
        {
            //Random number gen
            int winningNumber = random.Next(0, 37);
            string winningColor = "";
           

            //Choose the win color based on the generated number
            if (winningNumber == 0)
            {
                winningColor = "green";
            }
            else if (WinColorBlack.Contains(winningNumber))
            {
                winningColor = "black";
            }
            else
            {
                winningColor = "red";
            }

            return(winningNumber,  winningColor);
        }

        public bool CheckColorWin(string playerColor, string winningColor)
        {
            return playerColor == winningColor;
        }
        public bool CheckNumberWin(int playerNumber, int winningNumber)
        {
            return playerNumber == winningNumber;
        }
    }
}
