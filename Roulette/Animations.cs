using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Roulette
{
    public static class Animations
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
    }
}
