using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    
    internal class PokerHandsCheck
    {
        public PokerPage pkp;
        public static PokerDrawLogic draw;

        public static string pickedCard = draw.pickedCard;

        public string pSuit = "";
        public string pValue = "";
        public string dSuit = "";
        public string dValue = "";
        public string[] suits = { "hearts", "clubs", "diamonds", "spades" };
        
        public List<string> playerValues = new List<string>();
        public List<string> playerSuits = new List<string>();
        public List<string> dealerValues = new List<string>();
        public List<string> dealerSuits = new List<string>();
        
        public List<string> playerCards = new List<string>();
        public List<string> dealerCards = new List<string>();

        public string playerCard = "";
        public string dealerCard = "";

        public void ComparePreparation()
        {
            //player cards value and suit seperation
            foreach (var playerCard in PokerDrawLogic.drawnPlayerCards)
            {
                playerCards.Add(playerCard);
            }

            foreach (string suit in suits)
            {
                foreach (string playerCard in playerCards)
                {
                    if (playerCard.StartsWith(suit))
                    {
                        pSuit = suit;
                        playerSuits.Add(pSuit);
                        pValue = playerCard.Substring(suit.Length).ToLower();
                        playerValues.Add(pValue);
                    }
                }
            }

            foreach (var dealerCard in PokerDrawLogic.drawnDealerCards)
            {
                dealerCards.Add(dealerCard);
            }

            foreach (string suit in suits)
            {
                foreach (string dealerCard in dealerCards)
                {
                    if (dealerCard.StartsWith(suit))
                    {
                        dSuit = suit;
                        dealerSuits.Add(dSuit);
                        dValue = dealerCard.Substring(suit.Length).ToLower();
                        dealerValues.Add(dValue);
                    }
                }
            }
            if (playerSuits.Count == 0 || dealerSuits.Count == 0)
            {
                throw new ArgumentException("Invalid card name: " + pickedCard);
            }
        }

        public string HandWinCheck()
        {
            int playerHeartsAmount = 0;
            int playerClubsAmount = 0;
            int playerSpadesAmount = 0;
            int playerDiamondsAmount = 0;
            foreach (string suit in playerSuits)
            {
                switch (suit)
                {
                    case "hearts":
                        playerHeartsAmount += 1;
                        break;
                    case "clubs":
                        playerClubsAmount += 1;
                        break; ;
                    case "spades":
                        playerSpadesAmount += 1;
                        break;
                    case "diamonds":
                        playerDiamondsAmount += 1;
                        break;
                }
            }

            int dealerHeartsAmount = 0;
            int dealerClubsAmount = 0;
            int dealerSpadesAmount = 0;
            int dealerDiamondsAmount = 0;
            foreach (string suit in dealerSuits)
            {
                switch (suit)
                {
                    case "hearts":
                        dealerHeartsAmount += 1;
                        break;
                    case "clubs":
                        dealerClubsAmount += 1;
                        break;
                    case "spades":
                        dealerSpadesAmount += 1;
                        break;
                    case "diamonds":
                        dealerDiamondsAmount += 1;
                        break;
                }
            }

            int DealerPlayerHeartsSum = dealerHeartsAmount + playerHeartsAmount;
            int DealerPlayerClubsSum = dealerClubsAmount + playerClubsAmount;
            int DealerPlayerSpadesSum = dealerSpadesAmount + playerSpadesAmount;
            int DealerPlayerDiamondsSum = dealerDiamondsAmount + playerDiamondsAmount;

            string handResult = "";

            /*RoyalFlush*/
            /*StraightFlush*/
            if (DealerPlayerHeartsSum == 5 || DealerPlayerClubsSum == 5 || DealerPlayerSpadesSum == 5 || DealerPlayerDiamondsSum == 5)
            {
                handResult = "flush";
                return handResult;
            }


            else
            {
                handResult = "lose";
                return handResult;
            }

        }
    }
}
