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

        public PokerPage(MainWindow mainWindow)
        {
            InitializeComponent();

            main = mainWindow;
        }

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

        private void ResetChips(object sender, RoutedEventArgs e)
        {
            //PopUp to check if user really wants to reset
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
        }
    }
    }
