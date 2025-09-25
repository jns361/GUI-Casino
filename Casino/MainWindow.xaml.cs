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
        public RoulettePage rlp;
        public int playerBetAmount;
        public MainWindow main;
        public PokerPage pkp;
        public ChipManagement chips;


        public MainWindow()
        {
            InitializeComponent();
            ChipRain.Loaded += ChipRain_Loaded;

            rlp = new RoulettePage(this);
            chips = new ChipManagement(null, null);

            rlp.Visibility = Visibility.Hidden;

            pkp = new PokerPage(this);
            pkp.Visibility = Visibility.Hidden;

            MainGrid.Children.Add(rlp);
            MainGrid.Children.Add(pkp);
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
            chips.LoadChips();
            rlp.chipDisplay.Text = chips.chipAmount.ToString();
            // RoulettePage selbst sichtbar
            rlp.Visibility = Visibility.Visible;
            rlp.RouletteGrid.Visibility = Visibility.Visible;
            rlp.RouletteGrid.Opacity = 0;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.55)
            };
            rlp.RouletteGrid.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.55)
            };
            
            fadeOut.Completed += (s, a) =>
            {
                ChipRain.Visibility = Visibility.Hidden;
            };
            ChipRain.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void PokerButton_Click(object sender, RoutedEventArgs e)
        {
            pkp.StartNewRound();
            chips.LoadChips();
            pkp.chipDisplay.Text = chips.chipAmount.ToString();
            pkp.Visibility = Visibility.Visible;
            pkp.PokerGrid.Visibility = Visibility.Visible;
            pkp.PokerGrid.Opacity = 0;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.55)
            };
            pkp.PokerGrid.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.55)
            };
            ChipRain.BeginAnimation (UIElement.OpacityProperty, fadeOut);

        }
    }
}