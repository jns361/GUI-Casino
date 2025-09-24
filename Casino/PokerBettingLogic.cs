using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Casino
{
    public class PokerBettingLogic
    {
        public PokerHandsCheck hands;
        public ChipManagement chips;
        public Animations anim;
        public PokerPage pkp;

        public PokerBettingLogic(PokerPage pokerPage)
        {
            pkp = pokerPage;
            chips = new ChipManagement(null, null);
            anim = new Animations();
            hands = new PokerHandsCheck();
        }
        
        public double winAmount;
        public double betAmount;
        public int pBettingAmount;
        public void ChipCalculation(int bettingAmount)
        {
            pBettingAmount = bettingAmount;
            chips.chipAmount -= pBettingAmount;
            chips.SaveChips();

            betAmount += Convert.ToDouble(bettingAmount);
            bettingAmount = 0;
            Console.WriteLine("Given double betAmount = " + betAmount);
            pkp.TestText.Text += "Whole Bet: " + betAmount + "\n";
            pkp.chipDisplay.Text = chips.chipAmount.ToString();
            Animations.PopOut(pkp.chipDisplay);
        }

        //public string handResult => hands.handResult;

        public void WinCalculation(string handResult)
        {
            pkp.TestText.Text += "HAND RESULT: " + handResult + "\n";
            switch (handResult)
            {
                case "RoyalFlush":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 7;
                    break;
                case "StraightFlush":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 5.3;
                    break;
                case "FourOfAKind":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 4.2;
                    break;
                case "FullHouse":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 3.6;
                    break;
                case "Flush":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 3.0;
                    break;
                case "Straight":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 2.5;
                    break;
                case "ThreeOfAKind":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 2;
                    break;
                case "TwoPair":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 1.5;
                    break;
                case "Pair":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 0.85;
                    break;
                case "Highcard":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 0.5;
                    break;
            }
            pkp.TestText.Text += "Before round: " + winAmount.ToString() + "\n";
            winAmount = Math.Round(winAmount);
            pkp.TestText.Text += "After round: " + winAmount.ToString() + "\n";

            pkp.TestText.Text += "Chips before addition: " + chips.chipAmount.ToString() + "\n";
            chips.chipAmount += Convert.ToInt32(winAmount);
            pkp.TestText.Text += "Chips after addition: " + chips.chipAmount.ToString() + "\n";
            chips.SaveChips();

            Console.WriteLine($"Saved chips after Converting / l.82. ChipAmount is: " + chips.chipAmount);
            pkp.chipDisplay.Text = chips.chipAmount.ToString();
            Console.WriteLine("Changed chipdisplay immediatley after save in l. 82");

            if (chips.chipAmount < 50)
            {
                MessageBox.Show("GAME OVER!" + "\n" + "Reset or try your comeback in Roulette!" + "\n" + "If you want to reset, click yes. No will take you to the main menu!"
                    , "GAME OVER");
            }

        }
    }
}
