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
        public PokerBettingLogic bet;

        public PokerPage(MainWindow mainWindow)
        {
            InitializeComponent();
            main = mainWindow;
            checkwin = new PokerHandsCheck();
            chips = new ChipManagement(null, null);
            chips.LoadChips();
            chipDisplay.Text = chips.chipAmount.ToString();
            Console.WriteLine($"ChipDisplay set to {chips.chipAmount}");
            bet = new PokerBettingLogic(this);
            bet.betAmount = 0;
            LoadChipGif();

        // Initialize handResult using an instance of PokerHandsCheck
            var handsCheck = new PokerHandsCheck();
        }

        private void LoadChipGif()
        {
            var imageUri = new Uri("pack://application:,,,/Casino;component/Visuals/chip.gif", UriKind.Absolute);
            var image = new BitmapImage(imageUri);

            ImageBehavior.SetAnimatedSource(chipAnimation, image);
            ImageBehavior.SetRepeatBehavior(chipAnimation, System.Windows.Media.Animation.RepeatBehavior.Forever);
            ImageBehavior.SetAnimationSpeedRatio(chipAnimation, 1.5);
        }

        // Replace this line:
        // public string handResult = PokerHandsCheck.HandWinCheck();

        // With the following code to fix CS0120:

        //For force bet after the player hand draw 
        public bool firstRound = true;
        public bool betDone;

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
            betCount = 0;
            bet.betAmount = 0;
            //PopUp to check if user really wants to reset
            chips.SaveChips();
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
                chips.chipAmount = 1000;
                chips.SaveChips();
                chips.LoadChips();
                chipDisplay.Text = chips.chipAmount.ToString();
                Console.WriteLine("Updated chipDisplay to current chipAmount!");
                PokerDrawLogic.allCards = new List<string>(PokerDrawLogic.originalDeck);

                bet.betAmount = 0;

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

        public bool blockDraw = false;
        public bool firstBetDone = false;
        private async void Draw3DealerCards(object sender, RoutedEventArgs e)
        {
            if (firstBetDone == false)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "You have to bet at least 50 chips after getting your cards!";
                return;
            }
            ChipInput.IsEnabled = true;
            RaiseBet.IsEnabled = true;
            blockDraw = true;
            FirstDealerCards.IsEnabled = false;
            FirstDealerCards.Visibility = Visibility.Collapsed;
            for (int i = 0; i < 3; i++)
            {
                blockBetting = false;
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
            blockDraw = false;
        }

        bool firstClick = true;

        private void DrawDealerCard(object sender, RoutedEventArgs e)
        {
            ChipInput.IsEnabled = true;
            RaiseBet.IsEnabled = true;
            if (blockDraw) return;

            blockBetting = false;
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
                bet.WinCalculation(handResult);
            }
            firstClick = false;
        }

        

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            betDone = false;
            //checkedBet = false;

            ChipInput.IsEnabled = true;
            RaiseBet.IsEnabled = true;
            blockBetting = false;
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
            betCount = 0;
            bet.betAmount = 0;
            betDone = false;
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

        private void EmptyBetText(object sender, MouseButtonEventArgs e)
        {
            if (ChipInput.Text == "Bet")
            {
                ChipInput.Text = "";
            }

        }

        private void CheckForLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);

            if (e.Handled)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "Only numbers!";
            }
            else
            {
                InputCorrector.Visibility = Visibility.Hidden;
                InputCorrector.Text = "";
            }
        }

        //Make sure to draw cards before betting
        public bool blockBetting = true;
        public int intBetAmount;
        public int betCount = 0; 

        private void StartCalculation(object sender, RoutedEventArgs e)
        {
            chips.LoadChips();
            betDone = true;
            InputCorrector.Visibility = Visibility.Collapsed;


            //Check if the player entered something
            if (string.IsNullOrWhiteSpace(ChipInput.Text) || !int.TryParse(ChipInput.Text, out int betAmount))
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "Enter a valid number!";               
                return;
            }
            intBetAmount = int.Parse(ChipInput.Text);
            int checkResult = chips.chipAmount - intBetAmount - (int)bet.betAmount;
            Console.WriteLine($"chipAmount: {chips.chipAmount}");
            Console.WriteLine($"betAmount: {intBetAmount}");
            Console.WriteLine($"checkResult: {checkResult}");
            Console.WriteLine($"blockBetting: {blockBetting}");

            //Check if player tries to bet before cards were drawn
            if (blockBetting)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "Draw cards first!";
                return;
            }

            //Dont bet below minimum bet
            if (intBetAmount < 50)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "Minimum bet of 50 chips!";
                return;
            }

            //Check if the player has enough chips for his bet
            if (checkResult < 0)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "Can't bet that much!";
                ChipInput.Text = "";
                return;
            }

            if (betCount >= 2)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "You can only bet 2 times!";
                return;
            }

            if (blockBetting == false)
            {
                betCount++;
            }

            if (betCount == 1)
            {
                firstBetDone = true;
            }

            

            Console.WriteLine("intBetAmount = " + intBetAmount);
            blockBetting = true;
            bet.ChipCalculation(intBetAmount);
        }
    }
}

