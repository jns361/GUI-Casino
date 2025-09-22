using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Casino
{
    
    public class PokerHandsCheck
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
        public List<string> combinedValues = new List<string>();
        public List<string> combinedSuits = new List<string>();
        public List<string> flushValues = new List<string>();
        public Dictionary<string, int> valueNumberAssign = new Dictionary<string, int>()
        {
            
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "10", 10 },
            { "j", 11 },
            { "q", 12 },
            { "k", 13 },
            { "a", 14 }
        };
        public List<int> assignedValueNums = new List<int>();
        public List<int> assignedValsNoDupes = new List<int>();

        public string HandWinCheck()
        {
            ComparePreparation();
            combinedValues.Clear();
            combinedSuits.Clear();
            combinedValues.AddRange(playerValues);
            combinedValues.AddRange(dealerValues);

            var valueCount = new Dictionary<string, int>();

            foreach (var value in combinedValues)
            {
                if (!valueCount.ContainsKey(value))
                {
                    valueCount[value] = 0;
                }
                valueCount[value]++;
                
                if (valueNumberAssign.ContainsKey(value.ToLower()))
                {
                    assignedValueNums.Add(valueNumberAssign[value.ToLower()]);
                }
                else
                {
                    throw new Exception("Unknown card value: " + value);
                }
            }
            assignedValsNoDupes.AddRange(assignedValueNums.Distinct().ToList());
            assignedValsNoDupes.Sort();

            combinedSuits.AddRange(playerSuits);
            combinedSuits.AddRange(dealerSuits);

            var suitCount = new Dictionary<string, int>();

            foreach (var suit in combinedSuits)
            {
                if (!suitCount.ContainsKey(suit))
                {
                    suitCount[suit] = 0;
                }
                suitCount[suit]++;
            }

            bool straightCheck = StraightCheck();

            string handResult = "";

            var triples = valueCount.Where(kv => kv.Value == 3).Select(kv => kv.Key).ToList();
            var pairs = valueCount.Where(kv => kv.Value == 2).Select(kv => kv.Key).ToList();

            bool fullHouse = false;

            foreach (var triple in triples)
            {
                if (pairs.Any(p => p != triple))
                {
                    fullHouse = true;
                    break;
                }
            }

            string flushSuit = null;
            foreach (var suit in suitCount.Keys)
            {
                if (suitCount[suit] >= 5)
                {
                    flushSuit = suit;
                    break;
                }
            }

            bool royalFlush = false;
            
            if (flushSuit != null)
            {
                for (int i = 0; i < combinedValues.Count; i++)
                {
                    if (combinedSuits[i] == flushSuit)
                    {
                        flushValues.Add(combinedValues[i].ToLower());
                    }
                    string[] royalValues = { "10", "j", "q", "k", "a" };
                    royalFlush = royalValues.All(val => flushValues.Contains(val));
                }
            }
            if (royalFlush)
            {
                handResult = "RoyalFlush";
                return handResult;
            }

            else if (suitCount.Values.Any(count => count >= 5) && straightCheck == true)
            {
                handResult = "StraightFlush";
                return handResult;
            }

            else if (suitCount.Values.Any(count => count >= 5) && straightCheck == true)
            {
                handResult = "StraightFlush";
                return handResult;
            }

            else if (valueCount.Values.Any(count => count >= 4))
            {
                handResult = "FourOfAKind";
                return handResult;
            }

            else if (fullHouse)
            {
                handResult = "FullHouse";
                return handResult;
            }

            else if (suitCount.Values.Any(count => count >= 5))
            {
                handResult = "Flush";
                return handResult;
            }

            else if (straightCheck == true)
            {
                handResult = "Straight";
                return handResult;
            }

            else if (valueCount.Values.Any(count => count >= 3))
            {
                handResult = "ThreeOfAKind";
                return handResult;
            }

            else if (valueCount.Values.Count(count => count >= 2) >= 2)
            {
                handResult = "TwoPair";
                return handResult;
            }

            else if (valueCount.Values.Any(count => count >= 2))
            {
                handResult = "Pair";
                return handResult;
            }

            else
            {
                handResult = "Highcard";
                return handResult;
            }

        }

        public bool StraightCheck()
        {
            bool straightCheck = false;

            int consecutiveCount = 1;
            for (int i = 1; i < assignedValsNoDupes.Count; i++)
            {
                consecutiveCount++;
                if (assignedValsNoDupes[i] == assignedValsNoDupes[i - 1 ] + 1)
                {
                    if (consecutiveCount >= 5)
                    {
                        straightCheck = true;
                        return straightCheck;
                    }
                    
                }
                else
                {
                    consecutiveCount = 1;
                }
            }
            return straightCheck;
        }
        public void ResetLists()
        {
            dealerCards.Clear();
            playerCards.Clear();
            combinedValues.Clear();
            combinedSuits.Clear();
            assignedValsNoDupes.Clear();
            assignedValueNums.Clear();
            flushValues.Clear();
            playerValues.Clear();
            dealerValues.Clear();
            playerSuits.Clear();
            dealerSuits.Clear();
        }
    }
}
