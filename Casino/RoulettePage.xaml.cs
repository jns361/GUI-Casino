using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
namespace Casino
{
    /// <summary>
    /// Interaction logic for RoulettePage.xaml
    /// </summary>
    public partial class RoulettePage : UserControl
    {
        private GameLogicRoulette roulette;
        private ChipManagement chips;
        public int playerBetAmount;
        public MainWindow main;
        public Animations anim;

        public RoulettePage(MainWindow mainWindow)
        {
            InitializeComponent();
            textYour.Loaded += TextYour_Loaded;
            textBet.Loaded += TextBet_Loaded;
            textRolled.Loaded += TextRolled_Loaded;
            textResult.Loaded += TextResult_Loaded;

            main = mainWindow;

            // make everything gets shown correctly
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                SetupGame();
                LoadChipGif();
            }


            numberChoiceBox.Focusable = false;
            InputCorrector.Visibility = Visibility.Hidden;
        }

        private void TextYour_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FontAnim(textYour);
        }

        private void TextBet_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FontAnim(textBet);
        }

        private void TextRolled_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FontAnim(textRolled);
        }
        private void TextResult_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FontAnim(textResult);
        }

        private void SetupGame()

        {
            roulette = new GameLogicRoulette();
            chips = new ChipManagement(roulette, this);
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
                int chipAmount = chips.ChipAmount;
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

        public int chipFundsUsed;
        public bool ColorGame;
        public bool activeRound = false;
        private void Button_Click(object sender, RoutedEventArgs e) //SUBMIT button
        {
            ResultDisplay.BorderBrush = Brushes.Black;
            if (BetAmountInput.Text == "BET")
            {
                BetAmountInput.Text = "";
            }
            InputCorrector.Text = "";

            //Create all important variables for the upcoming codeblock
            string ColorChoice = "";
            var chipFund = (BetAmountInput.Text);
            int.TryParse(chipFund, out int chipFunds);
            chipFundsUsed = chipFunds;
            var PlayerNumber = (numberChoiceBox.Text);
            int.TryParse(PlayerNumber, out int PlayerNum);
            ColorGame = false;

            //Forbid to go below 0
            int checkResult = chips.chipAmount - chipFundsUsed;
            if (checkResult < 10)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "You can't go below the minimum bet of 10!";
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

            if (activeRound == true)
            {
                InputCorrector.Text = "Wait until the current round is over!";
                InputCorrector.Visibility = Visibility.Visible;
                return;
            }

            //Make sure there's a stake put on the line
            if (String.IsNullOrWhiteSpace(BetAmountInput.Text))
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = "Enter a bet!";
                return;
            }

            //Block submits without color or number
            if (String.IsNullOrWhiteSpace(PlayerNumber) && ColorGame == false)
            {
                InputCorrector.Text = "You have to choose what to bet on!";
                InputCorrector.Visibility = Visibility.Visible;
                return;
            }

            activeRound = true;
            chips.chipAmount -= chipFundsUsed;
            chipDisplay.Text = chips.chipAmount.ToString();
            DisplayUserColors(ColorChoice);

            ResultDisplayRed.Visibility = Visibility.Hidden;
            ResultDisplayGreen.Visibility = Visibility.Hidden;
            ResultDisplayGreen.Visibility = Visibility.Hidden;
            ResultDisplayBlack.Visibility = Visibility.Hidden;

            Random rnd = new Random();
            //Spin the wheel on click

            float repeats = (float)(rnd.NextDouble() * (6.5 - 3.5) + 3.5);
            RotateTransform rotateTransform = new RotateTransform(360);
            rouletteWheel.RenderTransform = rotateTransform;
            rouletteWheel.RenderTransformOrigin = new Point(0.5, 0.5);
            DoubleAnimation WheelSpin = new DoubleAnimation
            {
                From = 0,
                To = 360 * (repeats),
                Duration = TimeSpan.FromSeconds(2.5),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut },
            };



            WheelSpin.Completed += (s, e) =>
            {
                GetBetType();
                GameLoop(ColorGame, ColorChoice, PlayerNum, chipFundsUsed);// 
                activeRound = false;
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, WheelSpin);
        }

        public bool GetBetType()
        {
            return ColorGame;
        }

        public void GameLoop(bool ColorGame, string ColorChoice, int PlayerNumber, int chipFunds) //int playerBetAmount
        {
            bool gameWin;
            //Call the WinGenerator method to generate a number and color
            var result = roulette.WinGenerator();
            //If the user chose a color to play with, evaluate if he got the correct color or not
            if (ColorGame)
            {
                DisplayResultColors(result.Color, ColorChoice);
                if (roulette.CheckColorWin(ColorChoice, result.Color))
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
                if (roulette.CheckNumberWin(PlayerNumber, result.Number))
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
            //PopUp to check if user really wants to reset
            MessageBoxResult result = MessageBox.Show("Are you sure? Continuing will reset your chips back to 1000!",
                "Continue?",
                MessageBoxButton.YesNo);

            //If confirmed, reset chips to 1000, reset all animations (like hover effect) to normal
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

                BetAmountInput.Text = "BET";
                chipFundsUsed = 0;

                InputCorrector.Visibility = Visibility.Hidden;
                choiceAnnounce.Text = "Enter your desired stake in the beige box!";

                colorRed.IsChecked = false;
                colorGreen.IsChecked = false;
                colorBlack.IsChecked = false;
                NumberBetting.IsChecked = false;

                RedHover.Visibility = Visibility.Hidden;
                BlackHover.Visibility = Visibility.Hidden;
                GreenHover.Visibility = Visibility.Hidden;
                ResultDisplay.BorderBrush = Brushes.Black;
                UserDisplayRed.Visibility = Visibility.Hidden;
                UserDisplayGreen.Visibility = Visibility.Hidden;
                UserDisplayBlack.Visibility = Visibility.Hidden;
                ResultDisplayRed.Visibility = Visibility.Hidden;
                ResultDisplayGreen.Visibility = Visibility.Hidden;
                ResultDisplayBlack.Visibility = Visibility.Hidden;

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
            main.ChipRain.Visibility = Visibility.Visible;
            main.ChipRain.Opacity = 0;

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.55)
            };

            RouletteGrid.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.55)
            };

            fadeIn.Completed += (s, a) =>
            {
                RouletteGrid.Visibility = Visibility.Hidden;
                main.ChipRain.Visibility = Visibility.Visible;
            };
            main.ChipRain.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void DisplayUserColors(string colorChoice)
        {
            UserDisplayRed.Visibility = Visibility.Hidden;
            UserDisplayRed.Opacity = 0.0;
            UserDisplayGreen.Visibility = Visibility.Hidden;
            UserDisplayGreen.Opacity = 0.0;
            UserDisplayBlack.Visibility = Visibility.Hidden;
            UserDisplayBlack.Opacity = 0.0;

            Console.WriteLine("switched to Display");
            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(250)
            };

            if (colorChoice == "red")
            {
                UserDisplayRed.Visibility = Visibility.Visible;
                UserDisplayRed.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                //UserDisplayRed.Visibility = Visibility.Visible;
            }
            else if (colorChoice == "black")
            {
                UserDisplayBlack.Visibility = Visibility.Visible;
                UserDisplayBlack.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                //UserDisplayBlack.Visibility = Visibility.Visible;
            }
            else if (colorChoice == "green")
            {
                UserDisplayGreen.Visibility = Visibility.Visible;
                UserDisplayGreen.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                //UserDisplayGreen.Visibility = Visibility.Visible;
            }
        }

        private void DisplayResultColors(string resultColor, string colorChoice)
        {
            ResultDisplayRed.Visibility = Visibility.Hidden;
            ResultDisplayRed.Opacity = 0.0;
            ResultDisplayGreen.Visibility = Visibility.Hidden;
            ResultDisplayGreen.Opacity = 0.0;
            ResultDisplayBlack.Visibility = Visibility.Hidden;
            ResultDisplayBlack.Opacity = 0.0;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(250)
            };


            if (resultColor == "red")
            {
                ResultDisplayRed.Visibility = Visibility.Visible;
                ResultDisplayRed.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            else if (resultColor == "black")
            {
                ResultDisplayBlack.Visibility = Visibility.Visible;
                ResultDisplayBlack.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            else if (resultColor == "green")
            {
                ResultDisplayGreen.Visibility = Visibility.Visible;
                ResultDisplayGreen.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }

            if (resultColor == colorChoice)
            {
                ResultDisplay.BorderBrush = (Brush?)new BrushConverter().ConvertFromString("#34eb34") ?? Brushes.Green;
            }
            else
            {
                ResultDisplay.BorderBrush = (Brush?)new BrushConverter().ConvertFromString("#c90404") ?? Brushes.Red;
            }
        }

        private void chipDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
