using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Casino
{
    public class Animations
    {
        public static void PopOut(TextBox target, double scale = 1.2, int durationMs = 145)
        {
            if (!(target.RenderTransform is ScaleTransform scaleTransform))
            {
                scaleTransform = new ScaleTransform(1, 1);
                target.RenderTransform = scaleTransform;
                target.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            }

            var animation = new DoubleAnimation
            {
                To = scale,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                AutoReverse = true
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
        }

        private readonly Random rnd = new Random();

        public static void StartChipRain(Canvas chipCanvas, int chipCount = 20)
        {
            Random rnd = new Random();
            string[] chipImages = new string[]
            {
            "pack://application:,,,/Casino;component/Visuals/BlackChip.svg.png",
            "pack://application:,,,/Casino;component/Visuals/BlueChip.png",
            "pack://application:,,,/Casino;component/Visuals/GreenChip.png",
            "pack://application:,,,/Casino;component/Visuals/RedChip.png",
            "pack://application:,,,/Casino;component/Visuals/DollarSign.png"
            };

            for (int i = 0; i < chipCount; i++)
            {
                Image chip = new Image
                {
                    Source = new BitmapImage(new Uri(chipImages[rnd.Next(chipImages.Length)])),
                    Width = rnd.Next(25, 66),
                    Height = rnd.Next(25, 66)
                };

                Canvas.SetLeft(chip, rnd.Next(0, (int)chipCanvas.ActualWidth));
                Canvas.SetTop(chip, -50);
                chipCanvas.Children.Add(chip);

                DoubleAnimation fallAnim = new DoubleAnimation
                {
                    From = -50,
                    To = chipCanvas.ActualHeight,
                    Duration = TimeSpan.FromSeconds(3 + rnd.NextDouble() * 2),
                    RepeatBehavior = RepeatBehavior.Forever
                };

                chip.BeginAnimation(Canvas.TopProperty, fallAnim);
            }
        }

    }
}
