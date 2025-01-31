using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Platanotas.Models
{
    public static class BrushHelper
    {

        #region Gradient
        public static Brush GetGradientBrush(string gradientTag, double width = 100, double height = 100)
        {
            int offset = new Random().Next(0, 360); // Generate a random angle
            var gradientMap = GetGradientMap(offset, width, height);

            return gradientMap.TryGetValue(gradientTag, out var brush) ? brush : Brushes.Black;
        }

        // Generates a dictionary of gradients with a dynamic angle and customizable start-end points
        private static Dictionary<string, LinearGradientBrush> GetGradientMap(int offset, double width, double height)
        {
            return new Dictionary<string, LinearGradientBrush>
    {
        { "BlueToPurple", CreateGradientBrush(Colors.Blue, Colors.Purple, offset, width, height) },
        { "RedToYellow", CreateGradientBrush(Colors.Red, Colors.Yellow, offset, width, height) },
        { "GreenToBlue", CreateGradientBrush(Colors.Green, Colors.Blue, offset, width, height) },
        { "GreenToYellow", CreateGradientBrush(Colors.Green, Colors.Yellow, offset, width, height) }
    };
        }

        // Creates a gradient with better control over positioning
        private static LinearGradientBrush CreateGradientBrush(Color startColor, Color endColor, int angle, double width, double height)
        {
            // Convert angle to start and end points
            var (start, end) = GetGradientStartEnd(angle, width, height);

            return new LinearGradientBrush(startColor, endColor, 0) // 0 degrees (we manually set StartPoint & EndPoint)
            {
                StartPoint = start,
                EndPoint = end
            };
        }

        // Converts angle to normalized StartPoint & EndPoint
        private static (Point Start, Point End) GetGradientStartEnd(int angle, double width, double height)
        {
            double radians = angle * (Math.PI / 180);

            double x = Math.Cos(radians);
            double y = Math.Sin(radians);

            // Normalize based on width & height to ensure smooth transitions
            double startX = 0.5 - (x / 2);
            double startY = 0.5 - (y / 2);
            double endX = 0.5 + (x / 2);
            double endY = 0.5 + (y / 2);

            return (new Point(startX, startY), new Point(endX, endY));
        }

        // Adjusts the angle dynamically for existing brushes
        public static Brush ChangeGradientAngle(Brush brush)
        {
            int offset = new Random().Next(0, 360); // Generate a new angle

            if (brush is LinearGradientBrush linearGradientBrush)
            {
                var (start, end) = GetGradientStartEnd(offset, 100, 100);

                return new LinearGradientBrush(linearGradientBrush.GradientStops, 0) // Angle ignored; we use Start/EndPoint
                {
                    StartPoint = start,
                    EndPoint = end
                };
            }

            return brush;
        }


        #endregion

        #region Solid
        private static readonly Dictionary<string, Brush> ColorMap = new()
        {
            { "Red", Brushes.Red },
            { "Blue", Brushes.Blue },
            { "Green", Brushes.Green },
            { "Yellow", Brushes.Yellow },
            { "Pink", Brushes.Pink },
            { "Black", Brushes.Black } // Color por defecto
        };

        public static Brush GetBrush(string color)
        {
            return ColorMap.TryGetValue(color, out Brush? brush) ? brush : Brushes.Black;
        }
        #endregion

    }
}
