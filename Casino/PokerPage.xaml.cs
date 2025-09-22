using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WpfAnimatedGif;
using System.ComponentModel;
namespace Casino
{
    /// <summary>
    /// Interaction logic for PokerPage.xaml
    /// </summary>
    public partial class PokerPage : UserControl
    {
        private GameLogicRoulette roulette;
        private ChipManagement chips;
        public int playerBetAmount;
        public MainWindow main;
        public Animations anim;
        public PokerCardSetup setcards;
        public PokerDrawLogic draw = new PokerDrawLogic();
        public PokerHandsCheck checkwin;

        public PokerPage(MainWindow mainWindow)
        {
            InitializeComponent();
            main = mainWindow;
            checkwin = new PokerHandsCheck();
            chips = new ChipManagement(null, null);
            chipDisplay.Text = chips.chipAmount.ToString();

        // Initialize handResult using an instance of PokerHandsCheck
            var handsCheck = new PokerHandsCheck();
            /*
            var hearts2 = PokerCardSetup.GetCardImage(PokerCardSetup.heart, PokerCardSetup.two);
            var spadesK = PokerCardSetup.GetCardImage(PokerCardSetup.spades, PokerCardSetup.king);
            var diamondsA = PokerCardSetup.GetCardImage(PokerCardSetup.diamonds, PokerCardSetup.ace);

            CardPanel.Children.Add(new Image
            {
                Source = hearts2,
                Width = 48,
                Height = 68,
                Margin = new Thickness(5)
            });
            CardPanel.Children.Add(new Image
            {
                Source = spadesK,
                Width = 48,
                Height = 68,
                Margin = new Thickness(5)
            });
            CardPanel.Children.Add(new Image
            {
                Source = diamondsA,
                Width = 48,
                Height = 68,
                Margin = new Thickness(5)
            });*/
        }


        // Replace this line:
        // public string handResult = PokerHandsCheck.HandWinCheck();

        // With the following code to fix CS0120:

        private void HomeMenu_Click(object sender, RoutedEventArgs e)
        {
            main.ChipRain.Visibility = Visibility.Visible;
            main.ChipRain.Opacity = 0;
            //Debug.WriteLine("Starting window change to Home menu");
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.55)
            };

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.55)
            };

            fadeIn.Completed += (s, a) =>
            {
                PokerGrid.Visibility = Visibility.Hidden;
                main.ChipRain.Visibility = Visibility.Visible;
            };

            PokerGrid.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            //Debug.WriteLine("Starting fadeIn mainmenu");
            main.ChipRain.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            //Debug.WriteLine("Ended transition to main menu; ");
            }

        private void ResetGame(object sender, RoutedEventArgs e)
        {
            //PopUp to check if user really wants to reset
            MessageBoxResult result = MessageBox.Show("Are you sure? Continuing will reset your chips back to 1000!",
                "Continue?",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string chipSavePath = chips.savePath;
                Console.WriteLine(chipSavePath);
                using (StreamWriter sw = new StreamWriter(chipSavePath, false, Encoding.ASCII))
                {
                    sw.Write("1000");
                }
                chipDisplay.Text = "1000";
                chips.chipAmount = 1000;

                PokerDrawLogic.allCards = new List<string>(PokerDrawLogic.originalDeck);
                
                PokerDrawLogic.ResetLists();
                CardPanel.Children.Clear();
                PlayerCardPanel.Children.Clear();

                FirstDealerCards.IsEnabled = true;
                FirstDealerCards.Visibility = Visibility.Visible;
                NewDealerCard.IsEnabled = true;
                PlayerDraw.IsEnabled = true;
                draw.pickedCard = "";

                checkwin.ResetLists();
                TestText.Text = "";
                
                Console.WriteLine($"Reset successful! Chips are: displayed: {chipDisplay};" +
                    $" backend amount: {chips.chipAmount}");
                MessageBox.Show("Reset successful!", "Reset successful!");
            }

            else
            {
                MessageBox.Show("Reset cancelled!", "Reset cancelled!");
            }


        }

        private async void Draw3DealerCards(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                string pickedCard = draw.RandomCardDealer();
                if (pickedCard == "Reached max amount")
                {
                    NewDealerCard.IsEnabled = false;
                    FirstDealerCards.IsEnabled = false;
                    MessageBox.Show("Reached the maximum of dealer cards (5)");
                    return;
                }

                (string suit, string value) = PokerCardSetup.SuitValueAssignment(pickedCard);
                ImageSource source = PokerCardSetup.GetCardImage(suit, value);

                Image img = new Image
                {
                    Source = source,
                    Width = 48,
                    Height = 64,
                    Margin = new Thickness(5)
                };
                StackPanel cardWithLabel = new StackPanel();
                cardWithLabel.Children.Add(img);
                img.VerticalAlignment = VerticalAlignment.Top;
                img.Height = 64; img.Width = 48;

                TextBlock label = new TextBlock();
                label.Text = pickedCard;
                label.FontSize = 12;
                label.FontFamily = new FontFamily("Baskerville Old Face");
                label.TextAlignment = TextAlignment.Center;
                cardWithLabel.Children.Add(label);
                                
                CardPanel.Children.Add(cardWithLabel);
                await Task.Delay(320);
            }
            FirstDealerCards.IsEnabled = false;
            FirstDealerCards.Visibility = Visibility.Collapsed;
        }

        bool firstClick = true;

        private void DrawDealerCard(object sender, RoutedEventArgs e)
        {
            string pickedCard = draw.RandomCardDealer();
            if (pickedCard == "Reached max amount")
            {
                NewDealerCard.IsEnabled = false;
                MessageBox.Show("Reached the maximum of dealer cards (5)");
                return;
            }

            (string suit, string value) = PokerCardSetup.SuitValueAssignment(pickedCard);
            ImageSource source = PokerCardSetup.GetCardImage(suit, value);

            Image img = new Image
            {
                Source = source,
                Width = 48,
                Height = 64,
                Margin = new Thickness(5)
            };

            StackPanel cardWithLabel = new StackPanel();
            img.Width = 48; img.Height = 64;
            cardWithLabel.Children.Add(img);
            TextBlock label = new TextBlock();
            label.Text = pickedCard;
            label.FontSize = 12;
            label.FontFamily = new FontFamily("Baskerville Old Face");
            label.TextAlignment = TextAlignment.Center;
            cardWithLabel.Children.Add(label);

            CardPanel.Children.Add(cardWithLabel);

            if (firstClick == false)
            {
                string handResult = checkwin.HandWinCheck();
                TestText.TextAlignment = TextAlignment.Center;
                TestText.Text += handResult;
                NewRound.Visibility = Visibility.Visible;
            }
            firstClick = false;
        }

        

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                string pickedCard = draw.RandomCardPlayer();

                (string suit, string value) = PokerCardSetup.SuitValueAssignment(pickedCard);
                ImageSource source = PokerCardSetup.GetCardImage(suit, value);

                Image img = new Image
                {
                    Source = source,
                    Width = 48,
                    Height = 64,
                    Margin = new Thickness(5)
                };

                StackPanel cardWithLabel = new StackPanel();
                cardWithLabel.Orientation = Orientation.Vertical;

                cardWithLabel.Children.Add(img);
                img.VerticalAlignment = VerticalAlignment.Top;
                img.Height = 64; img.Width = 48;
                TextBlock label = new TextBlock();
                label.Text = pickedCard;
                label.FontSize = 12;
                cardWithLabel.Children.Add(label);

                label.FontFamily = new FontFamily("Baskerville Old Face");
                label.TextAlignment = TextAlignment.Center;

                PlayerCardPanel.Children.Add(cardWithLabel);
                await Task.Delay(300);
            }
            FirstDealerCards.IsEnabled = true;
            PlayerDraw.IsEnabled = false;
        }

        private void StartNewRound(object sender, RoutedEventArgs e)
        {
            PokerDrawLogic.allCards = new List<string>(PokerDrawLogic.originalDeck);
            NewRound.Visibility = Visibility.Collapsed;
            string chipSavePath = chips.savePath;
            Console.WriteLine(chipSavePath);

            FirstDealerCards.IsEnabled = false;
            FirstDealerCards.Visibility = Visibility.Visible;

            PokerDrawLogic.ResetLists();
            CardPanel.Children.Clear();
            PlayerCardPanel.Children.Clear();

            NewDealerCard.IsEnabled = true;
            PlayerDraw.IsEnabled = true;
            draw.pickedCard = "";

            firstClick = true;

            checkwin.ResetLists();
            TestText.Text = "";
        }
    }
}

