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

        public int betInTotal = 0;
        public double winAmount;
        public double betAmount;
        public int pBettingAmount;
        public void ChipCalculation(int bettingAmount)
        {
            chips.LoadChips();
            pBettingAmount = bettingAmount;
            chips.chipAmount -= pBettingAmount;
            chips.SaveChips();
            betInTotal += pBettingAmount;
            pkp.TotalBetDisplay.Text = "Total bet: " + betInTotal.ToString();

            betAmount += Convert.ToDouble(bettingAmount);
            bettingAmount = 0;
            Console.WriteLine("Given double betAmount = " + betAmount);
            pkp.chipDisplay.Text = chips.chipAmount.ToString();
            Animations.PopOut(pkp.chipDisplay);
        }

        double multiplicator;

        public void WinCalculation(string handResult)
        {
            pkp.WinAnnounce.Text += "Final hand: " + handResult + "\n";
            switch (handResult)
            {
                case "RoyalFlush":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 7;
                    break;
                case "StraightFlush":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 5.3;
                    break;
                case "FourOfAKind":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 4.2;
                    break;
                case "FullHouse":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 3.6;
                    break;
                case "Flush":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 3.0;
                    break;
                case "Straight":
                    Console.WriteLine("HandResult: " + handResult);
                    winAmount = betAmount * 2.5;
                    multiplicator = 2.5;
                    break;
                case "ThreeOfAKind":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 2;
                    break;
                case "TwoPair":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 1.5;
                    break;
                case "Pair":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 0.85;
                    break;
                case "Highcard":
                    Console.WriteLine("HandResult: " + handResult);
                    multiplicator = 0.5;
                    break;
            }
            winAmount = betAmount * multiplicator;
            winAmount = Math.Round(winAmount);
            pkp.WinAnnounce.Text += "\n" + "Bet: " + betInTotal + "\n" + "Won multiplicator: " + multiplicator + "\n" + "\n" + "Total win: " + winAmount;
            Animations.PopOut(pkp.WinAnnounce);
            chips.chipAmount += Convert.ToInt32(winAmount);
            chips.SaveChips();

            Console.WriteLine($"Saved chips after Converting / l.82. ChipAmount is: " + chips.chipAmount);
            pkp.chipDisplay.Text = chips.chipAmount.ToString();
            Console.WriteLine("Changed chipdisplay immediatley after save in l. 82");

            if (chips.chipAmount < 50)
            {
                MessageBoxResult result = MessageBox.Show("GAME OVER!" + "\n" + "Reset or try your comeback in Roulette!" + "\n" + "If you want to reset, click yes. No will take you to the main menu!",
                    "GAME OVER",
                    MessageBoxButton.OK);
                if(result == MessageBoxResult.OK)
                {
                    MessageBoxResult newResult = MessageBox.Show("Do you want to reset?" + "\n" + "When clicking 'No' you will be sent to the home menu so you can try to comeback in Roulette!",
                        "GAME OVER",
                        MessageBoxButton.YesNo);
                    if (newResult == MessageBoxResult.Yes)
                    {
                        pkp.ResetGame();
                    }
                    else
                    {
                        pkp.HomeMenu();
                    }
                }
                
            }

        }
    }
}
