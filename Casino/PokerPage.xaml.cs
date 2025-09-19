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
        
        public PokerPage(MainWindow mainWindow)
        {
            InitializeComponent();
            main = mainWindow;
            chips = new ChipManagement(roulette, null);

            // Initialize handResult using an instance of PokerHandsCheck
            var handsCheck = new PokerHandsCheck();
            handResult = handsCheck.HandWinCheck();

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
        public string handResult;

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

                PokerDrawLogic.ResetLists();
                CardPanel.Children.Clear();

                FirstDealerCards.IsEnabled = true;
                FirstDealerCards.Visibility = Visibility.Visible;
                NewDealerCard.IsEnabled = true;
                PlayerDraw.IsEnabled = true;

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

                TestText.Text += pickedCard + "\n";

                (string suit, string value) = PokerCardSetup.SuitValueAssignment(pickedCard);
                ImageSource source = PokerCardSetup.GetCardImage(suit, value);

                Image img = new Image
                {
                    Source = source,
                    Width = 48,
                    Height = 64,
                    Margin = new Thickness(5)
                };
                CardPanel.Children.Add(img);
                await Task.Delay(320);
            }
            FirstDealerCards.IsEnabled = false;
            FirstDealerCards.Visibility = Visibility.Collapsed;
        }

        private void DrawDealerCard(object sender, RoutedEventArgs e)
        {
            string pickedCard = draw.RandomCardDealer();
            if (pickedCard == "Reached max amount")
            {
                NewDealerCard.IsEnabled = false;
                MessageBox.Show("Reached the maximum of dealer cards (5)");
                return;
            }
            TestText.Text += pickedCard + "\n";

            (string suit, string value) = PokerCardSetup.SuitValueAssignment(pickedCard);
            ImageSource source = PokerCardSetup.GetCardImage(suit, value);

            Image img = new Image
            {
                Source = source,
                Width = 48,
                Height = 64,
                Margin = new Thickness(5)
            };
            CardPanel.Children.Add(img);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                string pickedCard = draw.RandomCardPlayer();
                if (pickedCard == "done")
                {
                    PlayerDraw.IsEnabled = false;
                    return;
                }
                TestText.Text += $"PlayerCard: {pickedCard}" + "\n";

                (string suit, string value) = PokerCardSetup.SuitValueAssignment(pickedCard);
                ImageSource source = PokerCardSetup.GetCardImage(suit, value);

                Image img = new Image
                {
                    Source = source,
                    Width = 48,
                    Height = 64,
                    Margin = new Thickness(5)
                };
                PlayerCardPanel.Children.Add(img);
                await Task.Delay(320);
            }
            PlayerDraw.IsEnabled = false;

            TestText.Text += handResult;
            
            //GEWINNERMITTLUNG FUNKTIONIERT NOCH NICHT RICHTIG
        }
     }
}

