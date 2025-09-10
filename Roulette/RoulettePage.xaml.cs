using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using System.ComponentModel;
namespace Casino
{
    /// <summary>
    /// Interaction logic for RoulettePage.xaml
    /// </summary>
    public partial class RoulettePage : UserControl
    {
        private GameLogic game;
        private ChipManagement chips;
        public int playerBetAmount;
        public MainWindow main;

        public RoulettePage(MainWindow mainWindow)
        {
            InitializeComponent();
            main = mainWindow;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                SetupGame();
                LoadChipGif();
            }

            
            numberChoiceBox.Focusable = false;
            InputCorrector.Visibility = Visibility.Hidden;
        }

        private void SetupGame()
        { 
            game = new GameLogic();
            chips = new ChipManagement(game, this);
            chipDisplay.Text = chips.chipAmount.ToString();
        }

        private void LoadChipGif()
        {
            var imageUri = new Uri("pack://application:,,,/Casino;component/Visuals/chip.gif", UriKind.Absolute);
            var image = new BitmapImage(imageUri);

            ImageBehavior.SetAnimatedSource(chipAnimation, image);
            ImageBehavior.SetRepeatBehavior(chipAnimation, System.Windows.Media.Animation.RepeatBehavior.Forever);
            ImageBehavior.SetAnimationSpeedRatio(chipAnimation, 1.5);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check if input is a valid integer
            if (int.TryParse(numberChoiceBox.Text, out int value))
            {
                //Verify it's something from 0 to 36
                if (value < 0 || value > 36)
                {
                    InputCorrector.Visibility = Visibility.Visible;
                    InputCorrector.Text = "Only numbers from 0 to 36 allowed!";
                    numberChoiceBox.Text = "";
                }
            }
        }

        private void BetAmountBox_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (BetAmountInput.Text == "BET")
            {
                BetAmountInput.Text = "";
            }
        }

        private void TextBoxChips_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check if entered bet amount is a valid integer
            if (int.TryParse(BetAmountInput.Text, out int value))
            {
                int chipAmount = chips.GetChipAmount();
                if (value > chipAmount)
                {
                    InputCorrector.Visibility = Visibility.Visible;
                    InputCorrector.Text = "Your bet can't be higher then your total chips!";
                    BetAmountInput.Text = "";
                }
                else if (value < 1)
                {
                    InputCorrector.Visibility = Visibility.Visible;
                    InputCorrector.Text = "Your bet has to be at least 1!";
                    BetAmountInput.Text = "";
                }
                else
                {
                    playerBetAmount = value;
                }
                //chipAmount = chipAmount - value;
                //GetBetAmount(value);
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            //Check if pasted content is a string
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                //Get the pasted content as string variable
                string pasteText = (string)e.DataObject.GetData(typeof(string));

                //Check if all chars of the paste are digits
                if (!pasteText.All(char.IsDigit))
                {
                    //If not every char is a digit, cancel the paste and send an error message
                    e.CancelCommand();
                    InputCorrector.Visibility = Visibility.Visible;
                    InputCorrector.Text = "You have to paste numbers ranging from 0 to 36!";
                }
            }
            //If the paste content isnt a string, cancel the paste immediately
            else
            {
                e.CancelCommand();
            }
        }

        private void CheckForLetters(object sender, TextCompositionEventArgs e)
        {
            //Check if user wants to write letters instead of digits
            e.Handled = !e.Text.All(char.IsDigit);

            if (e.Handled)
            {
                //If e.Handled is true (user doesnt only write digits), show an error message
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "You can only enter numbers!";
            }
            else
            {
                InputCorrector.Visibility = Visibility.Hidden;
                InputCorrector.Text = "";
            }

        }

        public bool ColorGame;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (BetAmountInput.Text == "BET")
            {
                BetAmountInput.Text = "";
            }
            InputCorrector.Text = "";
            //Create all important variables for the upcoming codeblock
            string ColorChoice = "";
            var chipFund = (BetAmountInput.Text);
            int.TryParse(chipFund, out int chipFunds);
            var PlayerNumber = (numberChoiceBox.Text);
            int.TryParse(PlayerNumber, out int PlayerNum);
            ColorGame = false;

            //Forbid to go below 0
            int checkResult = chips.chipAmount - chipFunds;
            if (checkResult < 0)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "You can't go below 0!";
                BetAmountInput.Text = "";
                return;
            }
            //Check what color got chosen and set the ColorChoice variable to it
            if (colorRed.IsChecked == true)
            {
                ColorChoice = "red";
                ColorGame = true;
            }
            else if (colorGreen.IsChecked == true)
            {
                ColorChoice = "green";
                ColorGame = true;
            }
            else if (colorBlack.IsChecked == true)
            {
                ColorChoice = "black";
                ColorGame = true;
            }

            GameLoop(ColorGame, ColorChoice, PlayerNum, chipFunds);// playerBetAmount
            GetBetType();
        }

        public bool GetBetType()
        {
            return ColorGame;
        }

        private void GetBetAmount(int betAmount)
        {
            chips.GetBetAmount(betAmount);
        }

        public void GameLoop(bool ColorGame, string ColorChoice, int PlayerNumber, int chipFunds) //int playerBetAmount
        {
            bool gameWin;
            //Call the WinGenerator method to generate a number and color
            var result = game.WinGenerator();
            //If the user chose a color to play with, evaluate if he got the correct color or not
            if (ColorGame)
            {
                //Hier chips iwie machen lol
                if (game.CheckColorWin(ColorChoice, result.Color))
                {
                    choiceAnnounce.Text = $"You win! You picked the color {ColorChoice}, " +
                        $"the color {result.Color} got rolled with the number {result.Number}";
                    gameWin = true;
                }
                else
                {
                    choiceAnnounce.Text = $"Unlucky, you lose! You picked the color {ColorChoice}, " +
                        $"the color {result.Color} got rolled with the number {result.Number}";
                    gameWin = false;
                }
            }

            //If the user chose a number to play with, evaluate if he got the correct number or not
            else
            {
                if (game.CheckNumberWin(PlayerNumber, result.Number))
                {
                    choiceAnnounce.Text = $"You win! You picked the number {PlayerNumber}, " +
                        $"the number {result.Number} got rolled with the color {result.Color}";
                    gameWin = true;
                }
                else
                {
                    choiceAnnounce.Text = $"Unlucky, you lose! You picked the number {PlayerNumber}, " +
                        $"the number {result.Number} got rolled with the color {result.Color}";
                    gameWin = false;
                }
            }
            chips.SetChipAmount(gameWin, chipFunds, PlayerNumber, ColorChoice);
            chips.SaveChips();
        }

        private void NumberBetting_Checked(object sender, RoutedEventArgs e)
        {
            RedHover.Visibility = Visibility.Hidden;
            BlackHover.Visibility = Visibility.Hidden;
            GreenHover.Visibility = Visibility.Hidden;
            numberChoiceBox.Focusable = true;
            colorRed.IsEnabled = false;
            colorBlack.IsEnabled = false;
            colorGreen.IsEnabled = false;
            colorGreen.IsChecked = false;
            colorBlack.IsChecked = false;
            colorRed.IsChecked = false;
            ColorRed.Background = Brushes.Gray;
            ColorBlack.Background = Brushes.Gray;
            ColorGreen.Background = Brushes.Gray;

            numberChoiceBox.Background = Brushes.White;
        }

        //Tickbox behaviour for the number betting
        private void NumberBetting_Unchecked(object sender, RoutedEventArgs e)
        {
            InputCorrector.Text = "";
            InputCorrector.Visibility = Visibility.Hidden;
            numberChoiceBox.Focusable = false;
            colorRed.IsEnabled = true;
            colorBlack.IsEnabled = true;
            colorGreen.IsEnabled = true;
            colorRed.Focusable = true;
            colorBlack.Focusable = true;
            colorGreen.Focusable = true;
            ColorRed.Background = Brushes.Red;
            ColorBlack.Background = Brushes.Black;
            ColorGreen.Background = Brushes.Green;

            numberChoiceBox.Clear();
            numberChoiceBox.Background = new SolidColorBrush(Color.FromRgb(202, 198, 198));
        }

        //Checkbox behavior for the color red checkbox
        private void ColorRed_Checked(object sender, RoutedEventArgs e)
        {
            RedHover.Visibility = Visibility.Visible;
            GreenHover.Visibility = Visibility.Hidden;
            BlackHover.Visibility = Visibility.Hidden;
            colorGreen.IsChecked = false;
            colorBlack.IsChecked = false;
        }

        //Checkbox behavior for the color green checkbox
        private void ColorGreen_Checked(object sender, RoutedEventArgs e)
        {
            RedHover.Visibility = Visibility.Hidden;
            GreenHover.Visibility = Visibility.Visible;
            BlackHover.Visibility = Visibility.Hidden;
            colorRed.IsChecked = false;
            colorBlack.IsChecked = false;
        }

        //Checkbox behavior for the color black checkbox
        private void ColorBlack_Checked(object sender, RoutedEventArgs e)
        {
            RedHover.Visibility = Visibility.Hidden;
            GreenHover.Visibility = Visibility.Hidden;
            BlackHover.Visibility = Visibility.Visible;
            colorRed.IsChecked = false;
            colorGreen.IsChecked = false;
        }

        //Reset chips to the beginning by just changing savefile content
        private void ResetChips(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure? Continuing will reset your chips back to 1000!",
                "Continue?",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                string savePath = chips.savePath;
                Console.WriteLine(savePath);
                using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII))
                {
                    sw.Write("1000");
                }
                chipDisplay.Text = "1000";
                chips.chipAmount = 1000;
                InputCorrector.Visibility = Visibility.Hidden;
                choiceAnnounce.Text = "Enter your desired stake in the beige box!";
                Console.WriteLine($"Reset successful! Chips are: displayed: {chipDisplay};" +
                    $" backend amount: {chips.chipAmount}");
                MessageBox.Show("Reset successful!", "Reset successful!");

            }

            else
            {
                MessageBox.Show("Reset cancelled!", "Reset cancelled!");
            }
        }

        public void NoChipsLeft()
        {
            InputCorrector.Visibility = Visibility.Visible;
            InputCorrector.Text = "You have no chips left, please press RESET to try again!";
            choiceAnnounce.Text = "Press 'RESET' to try again!";
            BetAmountInput.Text = "";
        }

        public void ColorHover(object sender, MouseEventArgs e)
        {
            if (colorBlack.IsMouseOver)
            {
                colorBlack.Background = Brushes.Transparent;
                colorBlack.Foreground = Brushes.Transparent;
                colorBlack.Opacity = 0;
                BlackHover.Visibility = Visibility.Visible;
            }
            else if (colorGreen.IsMouseOver)
            {
                colorGreen.Background = Brushes.Transparent;
                colorGreen.Foreground = Brushes.Transparent;
                colorGreen.Opacity = 0;
                GreenHover.Visibility = Visibility.Visible;
            }
            else if (colorRed.IsMouseOver)
            {
                RedHover.Visibility = Visibility.Visible;
            }
        }

        public void ColorNoHover(object sender, MouseEventArgs e)
        {
            if (colorBlack.IsChecked == false)
            {
                BlackHover.Visibility = Visibility.Hidden;
            }

            if (colorGreen.IsChecked == false)
            {
                GreenHover.Visibility = Visibility.Hidden;
            }
            if (colorRed.IsChecked == false)
            {
                RedHover.Visibility = Visibility.Hidden;
            }
        }

        private void HomeMenu_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("Starting window change to Home menu");
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.75)
            };

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.75)
            };

            fadeOut.Completed += (s, a) =>
            {
                RouletteGrid.Visibility = Visibility.Hidden;
                main.ChipRain.Visibility = Visibility.Visible;
            };

            RouletteGrid.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            //Debug.WriteLine("Starting fadeIn mainmenu");
            main.ChipRain.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            //Debug.WriteLine("Ended transition to main menu; ");
        }
    }
}
