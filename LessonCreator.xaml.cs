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
using System.Windows.Threading;

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

            DrawingCanvas.ToDraw = RenderEffects;
        }

        private void LessonCreator_Loaded(object sender, RoutedEventArgs e)
        {
            WWidth = (int)(Parent as MainWindow).ActualWidth;
            WHeight = (int)(Parent as MainWindow).ActualHeight;

            canvasRedrawTimer.Interval = TimeSpan.FromMilliseconds(20);
            canvasRedrawTimer.Tick += CanvasRedrawTimer_Tick;
            canvasRedrawTimer.Start();
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

        private void LessonCanvas_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Keyboard.Focus(LessonCanvas);
        }

        #endregion

        #region Adjusting to resolution
        // variables/constants for converting database coordinates to draw coordinates
        private int WWidth, WHeight;
        private const int EWidth = 1920, EHeight = 1080;

        

        #endregion

        // Logic for showing ShapeToolbar
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            SToolbar.Visibility = Visibility.Visible;
            BtnShow.Visibility = Visibility.Collapsed;
            LessonCanvas.Focus();
        }

        #region Drawing Shapes
        // Logic for inserting shapes


        public DrawState CurrentlyDrawing = DrawState.NONE;

        public double drawThickness = 5;

        private SolidColorBrush drawColor1 = new SolidColorBrush(Colors.White);
        private SolidColorBrush drawColor2 = new SolidColorBrush(Colors.Black);

        #region Logic for changing draw color

        public SolidColorBrush DrawColor1 { 
            get => drawColor1;
            set
            {
                SToolbar.Color1Btn.Foreground = value;
                drawColor1 = value;
                SToolbar.Color1Btn.InvalidateVisual();
            }
        }

        public SolidColorBrush DrawColor2
        {
            get => drawColor2;
            set
            {
                SToolbar.Color2Btn.Foreground = value;
                drawColor2 = value;
                SToolbar.Color2Btn.InvalidateVisual();
            }
        }

        #endregion

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        private double GetDistance(Point a, Point b) => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        private List<Shape> shapes = new List<Shape>();
        private List<Graph> graphs = new List<Graph>();
        private List<TextBlob> texts = new List<TextBlob>();

        private Point? startLocation;
        private readonly List<Point> vertices = new List<Point>();

        private const double VertexSpace = 8;

        #region Mouse events for editing shape attributes

        #endregion

        #region Helper functions for inserting shapes

        private void CreateEllipse(Point a, Point b)
        {
            shapes.Add(new Shape(ShapeType.ELLIPSE, new Point[2] { a, b }));
            Ellipse toAdd = new Ellipse
            {
                Stroke = drawColor1,
                Fill = drawColor2,
                Width = Math.Abs(b.X - a.X),
                Height = Math.Abs(b.Y - a.Y),
                StrokeThickness = drawThickness
            };
            ThicknessConverter converter = new ThicknessConverter();
            toAdd.Margin = (Thickness)converter.ConvertFrom($"{Math.Min(a.X, b.X).ToString()}, {Math.Min(a.Y, b.Y).ToString()}, 0, 0");
            LessonCanvas.Children.Add(toAdd);
        }

        private void CreateRectangle(Point a, Point b)
        {
            shapes.Add(new Shape(ShapeType.RECTANGLE, new Point[2] { a, b }));
            Rectangle toAdd = new Rectangle
            {
                Stroke = drawColor1,
                Fill = drawColor2,
                Width = Math.Abs(b.X - a.X),
                Height = Math.Abs(b.Y - a.Y),
                StrokeThickness = drawThickness
            };
            ThicknessConverter converter = new ThicknessConverter();
            toAdd.Margin = (Thickness)converter.ConvertFrom($"{Math.Min(a.X, b.X).ToString()}, {Math.Min(a.Y, b.Y).ToString()}, 0, 0");
            LessonCanvas.Children.Add(toAdd);
        }

        #endregion

        #region Mouse events for drawing shapes

        private bool mouseup = true;

        private void LessonCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (CurrentlyDrawing == DrawState.ELLIPSE || CurrentlyDrawing == DrawState.RECTANGLE)
                {
                    if (startLocation == null)
                    {
                        startLocation = e.GetPosition(this);
                    }
                }
                else if (mouseup && CurrentlyDrawing == DrawState.TRIANGLE)
                {
                    mouseup = false;
                    vertices.Add(e.GetPosition(this));
                    if (vertices.Count == 3)
                    {
                        shapes.Add(new Shape(ShapeType.TRIANGLE, vertices.ToArray()));
                        vertices.Clear();
                    }
                }
                else if (mouseup && CurrentlyDrawing == DrawState.POLYGON)
                {
                    // handle closing of the polygon
                    bool isClosed = false;
                    Point location = e.GetPosition(this);
                    foreach (Point vertex in vertices)
                    {
                        if (GetDistance(vertex, location) < VertexSpace && Keyboard.IsKeyUp(Key.LeftShift))
                        {
                            isClosed = true;
                        }
                    }

                    mouseup = false;

                    if (isClosed)
                    {
                        shapes.Add(new Shape(ShapeType.POLYGON, vertices.ToArray()));
                        PolygonAuxiliaryGeometry.Clear();
                        LessonCanvas.Children.Add(new Polygon()
                        {
                            Points = new PointCollection(vertices),
                            Fill = drawColor2,
                            StrokeThickness = drawThickness,
                            Stroke = drawColor1
                        });
                        vertices.Clear();
                    }
                    else
                    {
                        vertices.Add(e.GetPosition(this));
                        if (vertices.Count > 1)
                        {
                            PolygonAuxiliaryGeometry.Figures[0].Segments.Add(new LineSegment(vertices[vertices.Count - 1], true));
                        }
                        else if (vertices.Count == 1)
                        {
                            PolygonAuxiliaryGeometry.Figures.Add(new PathFigure() { StartPoint = vertices[0] });
                        }
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
            switch(CurrentlyDrawing)
            {
                case DrawState.ELLIPSE:
                    CreateEllipse(startLocation.Value, e.GetPosition(this));
                    break;
                case DrawState.RECTANGLE:
                    CreateRectangle(startLocation.Value, e.GetPosition(this));
                    break;
                default:
                    break;
            }

            startLocation = null;
            mouseup = true;
        }

        #endregion

        #region Additional effects when drawing shapes

        PathGeometry PolygonAuxiliaryGeometry = new PathGeometry();

        private void RenderEffects(DrawingContext dc)
        {
            switch (CurrentlyDrawing)
            {
                case DrawState.ELLIPSE:
                    if (startLocation.HasValue)
                    {
                        Point mouseloc = Mouse.GetPosition(this);
                        dc.DrawEllipse(drawColor2, new Pen(drawColor1, drawThickness), new Point((mouseloc.X + startLocation.Value.X) / 2, (mouseloc.Y + startLocation.Value.Y) / 2),
                            Math.Abs(mouseloc.X - startLocation.Value.X) / 2, Math.Abs(mouseloc.Y - startLocation.Value.Y) / 2);
                    }
                    break;
                case DrawState.RECTANGLE:
                    if (startLocation.HasValue)
                    {
                        Point mouseloc = Mouse.GetPosition(this);
                        dc.DrawRectangle(drawColor2, new Pen(drawColor1, drawThickness), new Rect(startLocation.Value, mouseloc));
                    }
                    break;
                case DrawState.POLYGON:
                    if (vertices.Count > 1)
                    {
                        foreach(Point vertex in vertices)
                        {
                            dc.DrawEllipse(null, new Pen(Brushes.White, 0.4), vertex, VertexSpace, VertexSpace);
                        }
                        dc.DrawGeometry(drawColor2, new Pen(drawColor1, drawThickness), PolygonAuxiliaryGeometry);
                    }
                    else if(vertices.Count == 1)
                    {
                        dc.DrawEllipse(null, new Pen(Brushes.White, 0.4), vertices[0], VertexSpace, VertexSpace);
                    }
                    break;
                default:
                    break;
            }
        }

        private DispatcherTimer canvasRedrawTimer = new DispatcherTimer();

        private void CanvasRedrawTimer_Tick(object sender, EventArgs e)
        {
            DrawingCanvas.InvalidateVisual();
        }

        #endregion

        #endregion
    }
}
