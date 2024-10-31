using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Platanotas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDrawing = false;
        private Point startPoint;
        private Polyline? currentPolyline;
        private Brush currentBrush = Brushes.Red;
        private double currentBrushSize = 2;
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            DrawingCanvas.MouseDown += DrawingCanvas_MouseDown;
            DrawingCanvas.MouseMove += DrawingCanvas_MouseMove;
            DrawingCanvas.MouseUp += DrawingCanvas_MouseUp;
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;
            currentPolyline = new Polyline
            {
                Stroke = currentBrush,
                StrokeThickness = currentBrushSize,
            };
            DrawingCanvas.Children.Add(currentPolyline);
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || currentPolyline == null) return;

            Point currentPoint = e.GetPosition(DrawingCanvas);
            currentPolyline.Points.Add(currentPoint);
        }

        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }


        private void SetBrushColor(object sender, RoutedEventArgs e)
        {
            // Obtén el color del Tag del MenuItem
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                string color = menuItem.Tag.ToString();
                // Aquí puedes establecer el color actual según el valor de 'color'
                currentBrush = GetBrushByColor(color);
            }
        }

        private void SetBrushSize(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null && menuItem.Tag != null)
            {
                // Parse the Tag value to double
                if (double.TryParse(menuItem.Tag.ToString(), out double size))
                {
                    currentBrushSize = size;
                }
            }
        }

        private Brush GetBrushByColor(string color)
        {
            switch (color)
            {
                case "Red":
                    return Brushes.Red;
                case "Blue":
                    return Brushes.Blue;
                case "Green":
                    return Brushes.Green;
                default:
                    return Brushes.Black; // Color por defecto
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            }
            else
            {
                DragMove();
            }
        }
    }
}