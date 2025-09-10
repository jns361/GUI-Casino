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
    /// Interaktionslogik für RouletteWindow.xaml

    public partial class MainWindow : Window
    {
        //Make it possible to access GameLogic.cs
        public RoulettePage rlp;
        public int playerBetAmount;
        public MainWindow main;
        

        public MainWindow()
        {
            InitializeComponent();
            ChipRain.Loaded += ChipRain_Loaded;

            rlp = new RoulettePage(this);
            rlp.Visibility = Visibility.Hidden;

            MainGrid.Children.Add(rlp);
            //System.Diagnostics.Debug.WriteLine("This is a log.");
            //Disable number input field as long as the checkbox isn't ticked
            //chips.SaveChips(); Debug and testing line
            //You can hover over the position with your mouse but it doesn't get           
        }

        private void ChipRain_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.StartChipRain(ChipRain, 20);
        }

        private void RouletteButton_Click(object sender, RoutedEventArgs e)
        { 
            /*if (rlp == null)
            {
                rlp = new RoulettePage(this);
                rlp.Visibility = Visibility.Hidden;
                MainGrid.Children.Add(rlp);
            }*/

            // RoulettePage selbst sichtbar
            rlp.Visibility = Visibility.Visible;
            rlp.RouletteGrid.Visibility = Visibility.Visible;
            rlp.RouletteGrid.Opacity = 0;

            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.75));
            rlp.RouletteGrid.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.75));
            ChipRain.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            
            
            fadeOut.Completed += (s, a) =>
            {
                ChipRain.Visibility = Visibility.Hidden;
            };
        }
    }
}