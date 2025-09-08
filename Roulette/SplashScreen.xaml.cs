using Roulette;
using System.Windows;

namespace RouletteNew
{
    /// <summary>
    /// Interaktionslogik für SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private async void RouletteButton_Click(object sender, RoutedEventArgs e)
        {
            var rouletteWindow = new RouletteWindow();
            rouletteWindow.Show();
            this.Close();
        }
    }
}
