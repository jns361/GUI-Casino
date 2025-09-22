using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class PokerDrawLogic
    {
        public PokerPage pkp;
        private Random rnd = new Random();
        public PokerCardSetup setcards;

        /*public string heart = "h";
        public string diamonds = "d";
        public string clubs = "c";
        public string spades = "s";

        public string two = "2";
        public string three = "3";
        public string four = "4";
        public string five = "5";
        public string six = "6";
        public string seven = "7";
        public string eight = "8";
        public string nine = "9";
        public string ten = "10";
        public string jack = "j";
        public string queen = "q";
        public string king = "k";
        public string ace = "a";
        */
        public string pickedCard = "";

        public static readonly List<string> originalDeck= new List<string>() { "heartsA", "hearts2", "hearts3", "hearts4", "hearts5", "hearts6", "hearts7", "hearts8", "hearts9", "hearts10", "heartsJ", "heartsQ",
            "heartsK", "clubsA", "clubs2", "clubs3", "clubs4", "clubs5", "clubs6", "clubs7", "clubs8", "clubs9", "clubs10", "clubsJ", "clubsQ", "clubsK", "spadesA", "spades2", "spades3",
            "spades4", "spades5", "spades6", "spades7", "spades8", "spades9", "spades10", "spadesJ", "spadesQ", "spadesK", "diamondsA", "diamonds2", "diamonds3", "diamonds4", "diamonds5",
            "diamonds6", "diamonds7", "diamonds8", "diamonds9", "diamonds10", "diamondsJ", "diamondsQ", "diamondsK" };

        public static List<string> allCards = new List<string>(originalDeck);


        public int cardDeckAmount = allCards.Count;

        public static List<string> drawnDealerCards = new List<string>();
        public static List<string> drawnCardsGeneral = new List<string>();
        public static List<string> drawnPlayerCards = new List<string>();

        public static void ResetLists()
        {
            drawnDealerCards.Clear();
            drawnCardsGeneral.Clear();
            drawnPlayerCards.Clear();
        }

        public string RandomCardDealer()
        {
            int index = rnd.Next(allCards.Count);
            pickedCard = allCards[index];

            if (drawnDealerCards.Count < 5)
            {
                drawnDealerCards.Add(pickedCard);
                drawnCardsGeneral.Add(pickedCard);
                allCards.Remove(pickedCard);
                return pickedCard;
            }

            else
            {
                pickedCard = "Reached max amount";
                return pickedCard;
            }
        }

        public string RandomCardPlayer()
        {
            int index = rnd.Next(allCards.Count);
            pickedCard = allCards[index];


            if (drawnPlayerCards.Count < 2)
            {
                drawnPlayerCards.Add(pickedCard);
                drawnCardsGeneral.Add(pickedCard);
                allCards.Remove(pickedCard);
                return pickedCard;
            }

            else
            {
                pickedCard = "done";
                return pickedCard;
            }
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                