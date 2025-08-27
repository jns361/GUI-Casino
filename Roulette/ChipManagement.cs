using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class ChipManagement
    {
        public readonly GameLogic game;
        public readonly MainWindow interactions;

        private int chipAmount = 100;

        public ChipManagement(GameLogic game, MainWindow interactions)
        {
            this.game = game;
            this.interactions = interactions;
        }

        public void SetChipAmount()
        {
            bool ColorGame = interactions.GetBetType();
            if (ColorGame)
            {

            }
        }

        public int GetChipAmount() 
        { 
            return chipAmount; 
        }

        /*public static BetAmountInput()
        {

        }*/

        /*
        public static ChipCalculationColor()
        {

        }*/
    }
}
