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

        public int chipAmount = 100;

        public ChipManagement(GameLogic game, MainWindow interactions)
        {
            this.game = game;
            this.interactions = interactions;
        }
        
        //set amount after bet + amount after win
        public void SetChipAmount(bool gameWin, int betAmount, int PlayerNumber, string playerColor)               
        {
            chipAmount -= betAmount;
            interactions.chipDisplay.Text=chipAmount.ToString();
            Animations.PopOut(interactions.chipDisplay);
            //after color bet
            bool ColorGame = interactions.GetBetType();
            int prizeForWin = 0;
            if (gameWin)
            { 
                if (ColorGame)
                {
                    if (playerColor.ToLower() == "red" || playerColor.ToLower() == "black")
                    {
                        prizeForWin = betAmount * 2;
                    }
                    else if (playerColor.ToLower() == "green")
                    {
                        prizeForWin = betAmount * 36;
                    }
                }
                else if (!ColorGame)
                {
                    prizeForWin = betAmount * 36;    
                }
                chipAmount += prizeForWin;
                interactions.chipDisplay.Text= chipAmount.ToString();
            }
            else if (!gameWin)
            {
                prizeForWin = 0;
            }
        }
        public int GetBetAmount(int betAmount)
        {
            return betAmount;
        }

        public int GetChipAmount() 
        { 
            return chipAmount; 
        }
    }
}
