using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public double betAmount = 0;
        public void ChipCalculation(int bettingAmount)
        {
            chips.chipAmount = chips.chipAmount - bettingAmount;
            chips.SaveChips();
            betAmount += bettingAmount;
            pkp.TestText.Text += "Whole Bet: " + betAmount + "\n";
            pkp.chipDisplay.Text = chips.chipAmount.ToString();
            Animations.PopOut(pkp.chipDisplay);
        }

        public string handResult => hands.handResult;

        public void WinCalculation()
        {
            pkp.TestText.Text += "HAND RESULT: " + handResult + "\n";
            switch (handResult)
            {
                case "RoyalFlush":
                    winAmount = betAmount * 7;
                    break;
                case "StraightFlush":
                    winAmount = betAmount * 5.3;
                    break;
                case "FourOfAKind":
                    winAmount = betAmount * 4.2;
                    break;
                case "FullHouse":
                    winAmount = betAmount * 3.6;
                    break;
                case "Flush":
                    winAmount = betAmount * 3.0;
                    break;
                case "Straight":
                    winAmount = betAmount * 2.5;
                    break;
                case "ThreeOfAKind":
                    winAmount = betAmount * 2;
                    break;
                case "TwoPair":
                    winAmount = betAmount * 1.5;
                    break;
                case "Pair":
                    winAmount = betAmount * 0.85;
                    break;
                case "Highcard":
                    winAmount = betAmount * 0.5;
                    break;
            }
            pkp.TestText.Text += "Before round: " + winAmount.ToString() + "\n";
            winAmount = Math.Round(winAmount);
            pkp.TestText.Text += "After round: " + winAmount.ToString() + "\n";

            pkp.TestText.Text += "Chips before addition: " + chips.chipAmount.ToString() + "\n";
            chips.chipAmount += Convert.ToInt32(winAmount);
            pkp.TestText.Text += "Chips after addition: " + chips.chipAmount.ToString() + "\n";
        }
    }
}
