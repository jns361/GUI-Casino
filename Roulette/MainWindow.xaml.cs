using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;
using System.IO;

namespace Roulette
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Make it possible to access GameLogic.cs
        private readonly GameLogic game;
        private readonly ChipManagement chips;
        public int playerBetAmount;
        
        public MainWindow()
        {
            InitializeComponent();
            //Disable number input field as long as the checkbox isn't ticked
            chips = new ChipManagement(game, this);
            //chips.SaveChips(); Debug and testing line
            numberChoiceBox.Focusable = false;
            game = new GameLogic();
            chipDisplay.Text = chips.chipAmount.ToString();
            
            //You can hover over the position with your mouse but it doesn't get highlighted
            InputCorrector.Visibility = Visibility.Hidden;

            var imageUri = new Uri("pack://application:,,,/Roulette;component/Visuals/chip.gif", UriKind.Absolute);
            var image = new BitmapImage(imageUri);

            ImageBehavior.SetAnimatedSource(chipAnimation, image);
            ImageBehavior.SetRepeatBehavior(chipAnimation, System.Windows.Media.Animation.RepeatBehavior.Forever);

            ImageBehavior.SetAnimationSpeedRatio(chipAnimation, 1.5);
        }
        private void Window_Loaded(object sender,RoutedEventArgs e)
        {
            DataObject.AddPastingHandler(numberChoiceBox, OnPaste);
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
            if(e.DataObject.GetDataPresent(typeof(string))) 
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
        //public int BetAmount;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InputCorrector.Text = "";
            //Create all important variables for the upcoming codeblock
            var ColorChoice = "empty";
            var chipFund= (BetAmountInput.Text);
            int.TryParse(chipFund, out int chipFunds);
            var PlayerNumber = (numberChoiceBox.Text);
            int.TryParse(PlayerNumber, out int PlayerNum);
            ColorGame = false;

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

            GameLoop(ColorGame, ColorChoice, PlayerNum, chipFunds, playerBetAmount);
            GetBetType();

            //chips.SetChipAmount(chipAmount, PlayerNum);
        }

        public bool GetBetType()
        {
            return ColorGame;
        }

        private void GetBetAmount(int betAmount)
        {
            chips.GetBetAmount(betAmount);
        }
        
        private void GameLoop(bool ColorGame, string ColorChoice, int PlayerNumber, int chipFunds, int playerBetAmount)
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

        //Tickbox behaviour for the number betting
        private void numberBetting_Checked(object sender, RoutedEventArgs e)
        {
            numberChoiceBox.Focusable = true;
            colorRed.IsEnabled = false;
            colorBlack.IsEnabled = false;
            colorGreen.IsEnabled = false;
            colorGreen.IsChecked = false;
            colorBlack.IsChecked = false;
            colorRed.IsChecked = false;

            numberChoiceBox.Background = Brushes.White;
        }

        //Tickbox behaviour for the number betting
        private void numberBetting_Unchecked(object sender, RoutedEventArgs e)
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
            
            numberChoiceBox.Clear();
            numberChoiceBox.Background = (Brush)new BrushConverter().ConvertFromString("#FFCAC6C6");
        }

        //Checkbox behavior for the color red checkbox
        private void colorRed_Checked(object sender, RoutedEventArgs e)
        {
            colorGreen.IsChecked = false;
            colorBlack.IsChecked = false;
        }

        //Checkbox behavior for the color green checkbox
        private void colorGreen_Checked(object sender, RoutedEventArgs e)
        {
            colorRed.IsChecked = false;
            colorBlack.IsChecked = false;
        }

        //Checkbox behavior for the color black checkbox
        private void colorBlack_Checked(object sender, RoutedEventArgs e)
        {
            colorRed.IsChecked = false;
            colorGreen.IsChecked = false;
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {

        }

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
                Console.WriteLine($"Reset successful! Chips are: displayed: {chipDisplay};" +
                    $" backend amount: {chips.chipAmount}");
                MessageBox.Show("Reset successful!", "Reset successful!");
            }
                
                
            else
            {
                MessageBox.Show("Reset cancelled!", "Reset cancelled!");
            }


        /*string savePath = chips.savePath;
        Console.WriteLine(savePath);
        using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII))
        {
            sw.Write("1000");
            chipDisplay.Text = "1000";
            chips.chipAmount = 1000;
        }
        Console.WriteLine("Reset successful!");         */
        }
    }
}
