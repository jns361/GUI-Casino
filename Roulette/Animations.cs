using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
        private static DispatcherTimer timer;
        public static void StartChipRain(Canvas chipCanvas, int chipCount = 2)
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

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(250)
            };

            timer.Tick += (s, e) =>
            {
                for(int i = 0; i < 2 ; i++)
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
                        To = chipCanvas.ActualHeight + 50,
                        Duration = TimeSpan.FromSeconds(3 + rnd.NextDouble() * 2),
                        FillBehavior = FillBehavior.Stop,
                    };

                    RotateTransform rotateTransform = new RotateTransform(360);
                    

                    fallAnim.Completed += (senderAnim, argsAnim) =>
                    {
                        chipCanvas.Children.Remove(chip);
                    };

                    chip.RenderTransform = rotateTransform;
                    chip.RenderTransformOrigin = new System.Windows.Point  (0.5, 0.5);

                    DoubleAnimation RotateAnim = new DoubleAnimation
                    {
                        From = 0,
                        To = 360,
                        Duration = TimeSpan.FromSeconds(2),
                        RepeatBehavior = RepeatBehavior.Forever,
                    };
                    chip.BeginAnimation(Canvas.TopProperty, fallAnim);
                    rotateTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnim);


                }
            };
            timer.Start();
        }
        public static void FontAnim(TextBlock target)
        {
            var scale = new DoubleAnimation
            {
                From = 1.0,
                To = 1.2,
                Duration = TimeSpan.FromMilliseconds(1125),
                //Thread.Sleep(1130),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,                
            
            };
            ((ScaleTransform)target.RenderTransform).BeginAnimation(ScaleTransform.ScaleXProperty, scale);
            ((ScaleTransform)target.RenderTransform).BeginAnimation(ScaleTransform.ScaleYProperty, scale);
            //textYour.BeginAnimation(TextBlock.FontSizeProperty, PopOut);
            /*

            DoubleAnimation GoLow = new DoubleAnimation
            {
                From = 14,
                To = 11,
                Duration = TimeSpan.FromMilliseconds(1125),
                RepeatBehavior = RepeatBehavior.Forever
            };
            textYour.BeginAnimation(TextBlock.FontSizeProperty, GoLow);
            */
        }

    }
}
