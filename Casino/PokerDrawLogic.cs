using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class PokerDrawLogic
    {
        private Random rdm = new Random();
        public PokerCardSetup setcards;

        public string heart = "h";
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

        public string pickedCard = "";
        public List<string> pickedCards = new List<string>();

        public string RandomCard()
        {
            while (true)
            {
                List<string> values = new List<string>()
                {
                    "a","2","3","4","5","6","7","8","9","10","j","q","k"
                };

                List<string> suits = new List<string>()
                {
                    "h", "c", "d", "s"
                };

                int index = rdm.Next(values.Count);
                string pickedValue = values[index];

                index = rdm.Next(suits.Count);
                string pickedSuit = suits[index];

                switch (pickedSuit)
                {
                    case "h":
                        switch (pickedValue)
                        {
                            case "a":
                                pickedCard = "heartsA";
                                break;
                            case "2":
                                pickedCard = "hearts2";
                                break;
                            case "3":
                                pickedCard = "hearts3";
                                break;
                            case "4":
                                pickedCard = "hearts4";
                                break;
                            case "5":
                                pickedCard = "hearts5";
                                break;
                            case "6":
                                pickedCard = "hearts6";
                                break;
                            case "7":
                                pickedCard = "hearts7";
                                break;
                            case "8":
                                pickedCard = "hearts8";
                                break;
                            case "9":
                                pickedCard = "hearts9";
                                break;
                            case "10":
                                pickedCard = "hearts10";
                                break;
                            case "j":
                                pickedCard = "heartsJ";
                                break;
                            case "q":
                                pickedCard = "heartsQ";
                                break;
                            case "k":
                                pickedCard = "heartsK";
                                break;
                        }
                        break;
                    case "c":
                        switch (pickedValue)
                        {
                            case "a":
                                pickedCard = "clubsA";
                                break;
                            case "2":
                                pickedCard = "clubs2";
                                break;
                            case "3":
                                pickedCard = "clubs3";
                                break;
                            case "4":
                                pickedCard = "clubs4";
                                break;
                            case "5":
                                pickedCard = "clubs5";
                                break;
                            case "6":
                                pickedCard = "clubs6";
                                break;
                            case "7":
                                pickedCard = "clubs7";
                                break;
                            case "8":
                                pickedCard = "clubs8";
                                break;
                            case "9":
                                pickedCard = "clubs9";
                                break;
                            case "10":
                                pickedCard = "clubs10";
                                break;
                            case "j":
                                pickedCard = "clubsJ";
                                break;
                            case "q":
                                pickedCard = "clubsQ";
                                break;
                            case "k":
                                pickedCard = "clubsK";
                                break;
                        }
                        break;
                    case "s":
                        switch (pickedValue)
                        {
                            case "a":
                                pickedCard = "spadesA";
                                break;
                            case "2":
                                pickedCard = "spades2";
                                break;
                            case "3":
                                pickedCard = "spades3";
                                break;
                            case "4":
                                pickedCard = "spades4";
                                break;
                            case "5":
                                pickedCard = "spades5";
                                break;
                            case "6":
                                pickedCard = "spades6";
                                break;
                            case "7":
                                pickedCard = "spades7";
                                break;
                            case "8":
                                pickedCard = "spades8";
                                break;
                            case "9":
                                pickedCard = "spades9";
                                break;
                            case "10":
                                pickedCard = "spades10";
                                break;
                            case "j":
                                pickedCard = "spadesJ";
                                break;
                            case "q":
                                pickedCard = "spadesQ";
                                break;
                            case "k":
                                pickedCard = "spadesK";
                                break;
                        }
                        break;
                    case "d":
                        switch (pickedValue)
                        {
                            case "a":
                                pickedCard = "diamondsA";
                                break;
                            case "2":
                                pickedCard = "diamonds2";
                                break;
                            case "3":
                                pickedCard = "diamonds3";
                                break;
                            case "4":
                                pickedCard = "diamonds4";
                                break;
                            case "5":
                                pickedCard = "diamonds5";
                                break;
                            case "6":
                                pickedCard = "diamonds6";
                                break;
                            case "7":
                                pickedCard = "diamonds7";
                                break;
                            case "8":
                                pickedCard = "diamonds8";
                                break;
                            case "9":
                                pickedCard = "diamonds9";
                                break;
                            case "10":
                                pickedCard = "diamonds10";
                                break;
                            case "j":
                                pickedCard = "diamondsJ";
                                break;
                            case "q":
                                pickedCard = "diamondsQ";
                                break;
                            case "k":
                                pickedCard = "diamondsK";
                                break;
                        }
                        break;
                }
                
                if (pickedCards.Contains(pickedCard))
                {
                    continue;
                }
                else
                {
                    pickedCards.Add(pickedCard);
                    break;
                }
            }
            return pickedCard;
        }
    }
}
