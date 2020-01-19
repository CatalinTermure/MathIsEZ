using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathIsEZ
{
    // enum for keeping track of what the user is trying to insert in a lesson
    public enum DrawState
    {
        NONE,
        ELLIPSE,
        RECTANGLE,
        TRIANGLE,
        POLYGON,
        GRAPH,
        TEXT,
    }

    public partial class LessonCreator : UserControl
    {
        public LessonCreator()
        {
            InitializeComponent();

            Loaded += LessonCreator_Loaded;
            KeyDown += LessonCreator_KeyDown;
            KeyUp += LessonCreator_KeyUp;
        }

        #region Keyboard shortcuts for editing shapes

        private void LessonCreator_KeyUp(object sender, KeyEventArgs e)
        {
            //
        }

        private void LessonCreator_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                try
                {
                    shapes.RemoveAt(shapes.Count - 1);
                    LessonCanvas.Children.RemoveAt(LessonCanvas.Children.Count - 1);
                } catch(ArgumentOutOfRangeException){}
                
            }
        }

        #endregion

        #region Adjusting to resolution
        // variables/constants for converting database coordinates to draw coordinates
        private int WWidth, WHeight;
        private const int EWidth = 1920, EHeight = 1080;

        private void LessonCreator_Loaded(object sender, RoutedEventArgs e)
        {
            WWidth = (int)(Parent as MainWindow).ActualWidth;
            WHeight = (int)(Parent as MainWindow).ActualHeight;
        }

        #endregion

        // Logic for collapsing ShapeToolbar
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            SToolbar.Visibility = Visibility.Visible;
            BtnShow.Visibility = Visibility.Collapsed;
        }

        #region Drawing Shapes
        // Logic for inserting shapes

        public DrawState currentlyDrawing = DrawState.NONE;

        public double drawThickness = 5;

        private SolidColorBrush drawColor1 = new SolidColorBrush(Colors.White);
        private SolidColorBrush drawColor2 = new SolidColorBrush(Colors.Black);

        #region Logic for changing draw color

        private void UpdateColor1(SolidColorBrush brush)
        {
            SToolbar.Color1Btn.Foreground = brush;
            drawColor1 = brush;
            SToolbar.Color1Btn.InvalidateVisual();
        }

        private void UpdateColor2(SolidColorBrush brush)
        {
            SToolbar.Color2Btn.Foreground = brush;
            drawColor2 = brush;
            SToolbar.Color2Btn.InvalidateVisual();
        }

        public SolidColorBrush DrawColor1 { 
            get => drawColor1;
            set => UpdateColor1(value);
        }

        public SolidColorBrush DrawColor2
        {
            get => drawColor2;
            set => UpdateColor2(value);
        }

        #endregion

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        private double GetDistance(Point a, Point b) => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        private List<Shape> shapes = new List<Shape>();
        private List<Graph> graphs = new List<Graph>();
        private List<TextBlob> texts = new List<TextBlob>();

        #region Mouse events for editing shape attributes

        #endregion

        #region Helper functions for inserting shapes

        private void CreateEllipse(Point a, Point b)
        {
            shapes.Add(new Shape(ShapeType.ELLIPSE, new Point[2] { a, b }));
            Ellipse toAdd = new Ellipse();
            toAdd.Stroke = drawColor1;
            toAdd.Fill = drawColor2;
            toAdd.Width = Math.Abs(b.X - a.X);
            toAdd.Height = Math.Abs(b.Y - a.Y);
            toAdd.StrokeThickness = drawThickness;
            ThicknessConverter converter = new ThicknessConverter();
            toAdd.Margin = (Thickness)converter.ConvertFrom($"{Math.Min(a.X, b.X).ToString()}, {Math.Min(a.Y, b.Y).ToString()}, 0, 0");
            LessonCanvas.Children.Add(toAdd);
            LessonCanvas.InvalidateVisual();
        }

        #endregion

        #region Mouse events for drawing shapes

        private Point? startLocation;
        private List<Point> vertices = new List<Point>();
        private bool mouseup = true;
        private void LessonCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (currentlyDrawing == DrawState.ELLIPSE || currentlyDrawing == DrawState.RECTANGLE)
                {
                    if (startLocation == null)
                    {
                        startLocation = e.GetPosition(this);
                    }
                }
                else if (mouseup && currentlyDrawing == DrawState.TRIANGLE)
                {
                    mouseup = false;
                    vertices.Add(e.GetPosition(this));
                    if (vertices.Count == 3)
                    {
                        shapes.Add(new Shape(ShapeType.TRIANGLE, vertices.ToArray()));
                        vertices.Clear();
                    }
                }
                else if (mouseup && currentlyDrawing == DrawState.POLYGON)
                {
                    // handle closing of the polygon
                    bool isClosed = false;
                    Point location = e.GetPosition(this);
                    foreach (Point vertex in vertices)
                    {
                        if (GetDistance(vertex, location) < 5 && Keyboard.IsKeyUp(Key.LeftShift))
                        {
                            isClosed = true;
                        }
                    }

                    mouseup = false;
                    vertices.Add(e.GetPosition(this));
                    if(isClosed)
                    {
                        shapes.Add(new Shape(ShapeType.POLYGON, vertices.ToArray()));
                        vertices.Clear();
                    }
                }
            }
        }

        private void LessonCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Right)
            {
                e.Handled = true;
                return;
            }
            switch(currentlyDrawing)
            {
                case DrawState.ELLIPSE:
                    CreateEllipse(startLocation.Value, e.GetPosition(this));
                    break;
                case DrawState.RECTANGLE:
                    shapes.Add(new Shape(ShapeType.RECTANGLE, new Point[2] { startLocation.GetValueOrDefault(), e.GetPosition(this) }));
                    break;
                case DrawState.POLYGON:
                    break;
                default:
                    break;
            }

            startLocation = null;
            mouseup = true;
        }

        #endregion

        #endregion
    }
}
