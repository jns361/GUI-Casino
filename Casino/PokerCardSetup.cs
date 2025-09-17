using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
namespace Casino
{
    public class PokerCardSetup
    {
        public PokerPage pkp;

        public static (string suit, string value) SuitValueAssignment(string pickedCard)
        {
            if (pickedCard.StartsWith("hearts"))
                return ("h", pickedCard.Substring("hearts".Length).ToLower());
            
            if (pickedCard.StartsWith("clubs"))
                return ("c", pickedCard.Substring("clubs".Length).ToLower());

            if (pickedCard.StartsWith("spades"))
                return ("s", pickedCard.Substring("spades".Length).ToLower());

            if (pickedCard.StartsWith("diamonds"))
                return ("d", pickedCard.Substring("diamonds".Length).ToLower());

            throw new ArgumentException("Invalid card name: " + pickedCard);
        }

        public static ImageSource GetCardImage(string suit, string value)
        {
            var imageUri = new Uri("pack://application:,,,/Casino;component/Visuals/Pokercards/pokercardSprite.png", UriKind.Absolute);
            var CardSheet = new BitmapImage(imageUri);

            int row = 0;
            int column = 0;

            switch (suit)
            {
                case "h":
                    row = 0;
                    break;
                case "d":
                    row = 1;
                    break;
                case "c":
                    row = 2;
                    break;
                case "s":
                    row = 3;
                    break;
            }

            switch (value)
            {
                case "a":
                    column = 0;
                    break;
                case "2":
                    column = 1;
                    break;
                case "3":
                    column = 2;
                    break;
                case "4":
                    column = 3;
                    break;
                case "5":
                    column = 4;
                    break;
                case "6":
                    column = 5;
                    break;
                case "7":
                    column = 6;
                    break;
                case "8":
                    column = 7;
                    break;
                case "9":
                    column = 8;
                    break;
                case "10":
                    column = 9;
                    break;
                case "j":
                    column = 10;
                    break;
                case "q":
                    column = 11;
                    break;
                case "k":
                    column = 12;
                    break;
            }

            var rect = new Int32Rect(column * 48, row * 64, 48, 64);
            var cropped = new CroppedBitmap(CardSheet, rect);

            return cropped;
        }

        public void DisplayCards()
        {
            
        }
    }
}
