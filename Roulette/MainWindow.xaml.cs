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

namespace Roulette
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            numberChoiceBox.Focusable = false;                                      //Deaktivierung des Zahleneingabefelds solange die Checkbox nicht gewählt ist
            InputCorrector.Visibility = Visibility.Hidden;                          //Dass man mit der Maus zwar drüberhovert, aber keine Outlines sieht
        }
        private void Window_Loaded(object sender,RoutedEventArgs e)
        {
            DataObject.AddPastingHandler(numberChoiceBox, OnPaste);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(numberChoiceBox.Text, out int value))
            {
                if (value < 0 || value > 36)
                {
                    InputCorrector.Visibility = Visibility.Visible;
                    InputCorrector.Text = ("Only numbers from 0 - 36 allowed!");
                    numberChoiceBox.Text = ("");
                }
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if(e.DataObject.GetDataPresent(typeof(string))) 
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));

                if (!pasteText.All(char.IsDigit))
                {
                    e.CancelCommand();
                    InputCorrector.Visibility = Visibility.Visible;
                    InputCorrector.Text = ("You have to paste numbers ranging from 0 - 36!");
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void CheckForLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);

            if (e.Handled)
            {
                InputCorrector.Visibility = Visibility.Visible;
                InputCorrector.Text = ("You can only enter numbers!");
            }
            else
            {
                InputCorrector.Visibility = Visibility.Hidden;
                InputCorrector.Text = ("");
            }
            //e.Handled = !e.Text.All(char.IsDigit);                                  //Nur Zahlen zulassen!
        }

        private void Button_Click(object sender, RoutedEventArgs e)                 //Übergabe des vom Spieler gewünschten Werts in Variablen
        {                                                                           //Entweder Farbe oder Zahl
            InputCorrector.Text = ("");
            var ColorChoice = "empty";
            var PlayerNumber = numberChoiceBox.Text;
            bool ColorGame = false;



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
            /*
            if (ColorGame == false)                                                 //Wird mit einer Zahl gespielt oder einer Farbe?
            {
                choiceAnnounce.Text = ($"Your choice is {PlayerNumber}; glhf");
            }
            else
            {
                choiceAnnounce.Text = ($"Your choice is {ColorChoice}; glhf");
            }*/

            var result = WinGenerator(ColorGame);
            string WinningNumber = result.Number.ToString();
            if (ColorGame == true)
            {
                if (ColorChoice == result.Color)
                {
                    choiceAnnounce.Text = ($"You win! You picked the color {ColorChoice}, " +
                        $"the color {result.Color} got rolled with the number {result.Number}");
                }
                else if (ColorChoice != result.Color)
                {
                    choiceAnnounce.Text = ($"Unlucky, you lose! You picked the color {ColorChoice}, " +
                        $"the color {result.Color} got rolled with the number {result.Number}");
                }
            }
            
            else
            {
                if (PlayerNumber == WinningNumber)
                { //result number umwandeln!!
                    choiceAnnounce.Text = ($"You win! You picked the number {PlayerNumber}, " +
                        $"the number {WinningNumber} got rolled with the color {result.Color}");
                }
                else
                {
                    choiceAnnounce.Text = ($"Unlucky, you lose! You picked the number {PlayerNumber}, " +
                        $"the number {WinningNumber} got rolled with the color {result.Color}");
                }
            }
        }

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

        private void numberBetting_Unchecked(object sender, RoutedEventArgs e)
        {
            InputCorrector.Text = ("");
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


        private void colorRed_Checked(object sender, RoutedEventArgs e)
        {
            colorGreen.IsChecked = false;
            colorBlack.IsChecked = false;
        }
 
        private void colorGreen_Checked(object sender, RoutedEventArgs e)
        {
            colorRed.IsChecked = false;
            colorBlack.IsChecked = false;
        }
        private void colorBlack_Checked(object sender, RoutedEventArgs e)
        {
            colorRed.IsChecked = false;
            colorGreen.IsChecked = false;
        }

        public static (int Number, string Color) WinGenerator(bool ColorGame)
        {
            int[] WinColorBlack = new int[] { 15, 4, 2, 17, 6, 13, 11, 8, 10, 24, 33, 20, 31, 22, 29, 28, 35, 26 };

            Random random = new Random();
            int WinningNumber = random.Next(0, 37);
            string WinningColor = "empty";

            if (WinningNumber == 0)
            {
                WinningColor = "green";
            }
            else if (WinColorBlack.Contains(WinningNumber))
            {
                WinningColor = "black";
            }
            else
            {
                WinningColor = "red";
            }
            return (WinningNumber, WinningColor);
        }


    } //NUMBERS 0-36
}
