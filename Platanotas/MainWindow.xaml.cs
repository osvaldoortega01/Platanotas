using Platanotas.Models;
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
        public enum DrawingTool
        {
            Pencil,
            Rectangle,
            FilledRectangle,
            Circle,
            FilledCircle,
            Line,
            Arrow,
            Text
        }

        private Shape? previewShape;
        private DrawingTool currentTool = DrawingTool.Pencil;
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            DrawingCanvas.MouseDown += DrawingCanvas_MouseDown;
            DrawingCanvas.MouseMove += DrawingCanvas_MouseMove;
            DrawingCanvas.MouseUp += DrawingCanvas_MouseUp;
        }
        private void PencilButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = DrawingTool.Pencil;
        }

        private void BrushConfigButton_Click(object sender, RoutedEventArgs e)
        {
            ColorSizePopup.IsOpen = !ColorSizePopup.IsOpen;
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = DrawingTool.Rectangle;
        }

        private void FilledRectangleButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = DrawingTool.FilledRectangle;
        }
        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = DrawingTool.Line;
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;
            startPoint = e.GetPosition(DrawingCanvas);

            if (currentTool == DrawingTool.Pencil)
            {
                // Initialize polyline for the pencil tool
                currentPolyline = new Polyline
                {
                    Stroke = currentBrush,
                    StrokeThickness = currentBrushSize,
                };
                DrawingCanvas.Children.Add(currentPolyline);
            }
            else if (currentTool == DrawingTool.Rectangle || currentTool == DrawingTool.FilledRectangle)
            {
                // Initialize a rectangle for preview
                previewShape = new Rectangle
                {
                    Stroke = currentBrush,
                    StrokeThickness = currentBrushSize,
                    Fill = currentTool == DrawingTool.FilledRectangle ? currentBrush : Brushes.Transparent
                };
                DrawingCanvas.Children.Add(previewShape);
            }
            else if (currentTool == DrawingTool.Line)
            {
                // Initialize a line for preview
                previewShape = new Line
                {
                    Stroke = currentBrush,
                    StrokeThickness = currentBrushSize,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y
                };
                DrawingCanvas.Children.Add(previewShape);
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            Point currentPoint = e.GetPosition(DrawingCanvas);

            // Update polyline for pencil drawing
            if (currentTool == DrawingTool.Pencil && currentPolyline != null)
            {
                currentPolyline.Points.Add(currentPoint);
            }
            // Update preview rectangle
            else if (currentTool == DrawingTool.Rectangle || currentTool == DrawingTool.FilledRectangle)
            {
                if (previewShape is Rectangle rectangle)
                {
                    rectangle.Width = Math.Abs(currentPoint.X - startPoint.X);
                    rectangle.Height = Math.Abs(currentPoint.Y - startPoint.Y);
                    Canvas.SetLeft(rectangle, Math.Min(currentPoint.X, startPoint.X));
                    Canvas.SetTop(rectangle, Math.Min(currentPoint.Y, startPoint.Y));
                }
            }
            // Update preview line
            else if (currentTool == DrawingTool.Line)
            {
                if (previewShape is Line line)
                {
                    line.X2 = currentPoint.X;
                    line.Y2 = currentPoint.Y;
                }
            }
        }


        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;

            // Finalize rectangle
            if (currentTool == DrawingTool.Rectangle || currentTool == DrawingTool.FilledRectangle)
            {
                // Remove the preview shape; the preview shape is already added, so no need to re-add it
                previewShape = null;
            }
            // Finalize line
            else if (currentTool == DrawingTool.Line)
            {
                // Remove the preview shape; the preview shape is already added, so no need to re-add it
                previewShape = null;
            }

            // Reset currentPolyline after pencil drawing is complete
            currentPolyline = null;
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
            if (sender is Button button && button.Tag is string colorTag)
            {
                string color = colorTag;
                // Aquí puedes establecer el color actual según el valor de 'color'
                Color selectedColor = (Color)ColorConverter.ConvertFromString(colorTag);
                currentBrush = new SolidColorBrush(selectedColor);
            }
        }

        private void SetBrushGradient(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string gradientTag)
            {
                switch (gradientTag)
                {
                    case "BlueToPurple":
                        currentBrush = new LinearGradientBrush(Colors.Blue, Colors.Purple, 45);
                        break;
                    case "RedToYellow":
                        currentBrush = new LinearGradientBrush(Colors.Red, Colors.Yellow, 45);
                        break;
                    case "GreenToBlue":
                        currentBrush = new LinearGradientBrush(Colors.Green, Colors.Blue, 45);
                        break;
                    default:
                        currentBrush = Brushes.Black;
                        break;
                }
            }
        }

        private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentBrushSize = e.NewValue;
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
        public ICommand SelectPencilCommand => new RelayCommand(_ => currentTool = DrawingTool.Pencil);
        public ICommand SelectRectangleCommand => new RelayCommand(_ => currentTool = DrawingTool.Rectangle);
        public ICommand SelectLineCommand => new RelayCommand(_ => currentTool = DrawingTool.Line);

    }
}