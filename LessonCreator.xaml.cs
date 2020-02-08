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

            canvasRedrawTimer.Interval = TimeSpan.FromMilliseconds(10);
            canvasRedrawTimer.Tick += CanvasRedrawTimer_Tick;
            canvasRedrawTimer.Start();
        }

        #region Keyboard shortcuts for editing shapes

        private bool ctrlPressed, shiftPressed;

        private void LessonCreator_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
            {
                shiftPressed = false;
            }
            else if (e.Key == Key.LeftCtrl)
            {
                ctrlPressed = false;
            }
        }

        private void LessonCreator_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Z && ctrlPressed)
            {
                try
                {
                    LessonCanvas.Children.RemoveAt(LessonCanvas.Children.Count - 1);
                } catch(ArgumentOutOfRangeException){}
            }
            else if (e.Key == Key.LeftShift)
            {
                shiftPressed = true;
            }
            else if (e.Key == Key.LeftCtrl)
            {
                ctrlPressed = true;
            }
        }

        private void LessonCanvas_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Keyboard.Focus(LessonCanvas);
        }

        #endregion

        #region Timeline handlers

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

        #region Fields and properties used for drawing

        public DrawState currentlyDrawing = DrawState.NONE;
        public DrawState CurrentlyDrawing
        {
            get => currentlyDrawing;
            set
            {
                vertices.Clear();
                startLocation = null;
                currentlyDrawing = value;
                PolygonAuxiliaryGeometry.Clear();
            }
        }

        public double drawThickness = 5;
        public bool doFill = false;

        private Brush drawColor1 = new SolidColorBrush(Colors.White);
        private Brush drawColor2 = new SolidColorBrush(Colors.Black);

        public Brush DrawColor1 { 
            get => drawColor1;
            set
            {
                SToolbar.Color1Btn.Foreground = value;
                drawColor1 = value;
                SToolbar.Color1Btn.InvalidateVisual();
            }
        }

        public Brush DrawColor2
        {
            get
            {
                return doFill ? drawColor2 : null;
            }
            set
            {
                SToolbar.Color2Btn.Foreground = value;
                drawColor2 = value;
                SToolbar.Color2Btn.InvalidateVisual();
            }
        }

        #endregion

        #region Fields and properties for storing lesson data 

        private List<Shape> shapes = new List<Shape>();
        private List<Graph> graphs = new List<Graph>();
        private List<TextBlob> texts = new List<TextBlob>();

        #endregion

        private Point? startLocation;
        private readonly List<Point> vertices = new List<Point>();

        private const double VertexSpace = 8;

        #region Mouse events for editing shape attributes
        /// TODO
        #endregion

        #region Helper functions for inserting shapes

        private void CreateEllipse(Point a, Point b)
        {
            if(shiftPressed)
            {
                double sz = Math.Min(Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
                b.X = a.X + Math.Sign(b.X - a.X) * sz;
                b.Y = a.Y + Math.Sign(b.Y - a.Y) * sz;
            }
            Ellipse toAdd = new Ellipse
            {
                Stroke = DrawColor1,
                Fill = DrawColor2,
                Width = Math.Abs(b.X - a.X) + drawThickness,
                Height = Math.Abs(b.Y - a.Y) + drawThickness,
                StrokeThickness = drawThickness
            };
            ThicknessConverter converter = new ThicknessConverter();
            toAdd.Margin = (Thickness)converter.ConvertFrom($"{(Math.Min(a.X, b.X) - drawThickness / 2).ToString()}, {(Math.Min(a.Y, b.Y) - drawThickness / 2).ToString()}, 0, 0");
            LessonCanvas.Children.Add(toAdd);
        }

        private void CreateRectangle(Point a, Point b)
        {
            if(shiftPressed)
            {
                double sz = Math.Min(Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
                b.X = a.X + Math.Sign(b.X - a.X) * sz;
                b.Y = a.Y + Math.Sign(b.Y - a.Y) * sz;
            }

            Rectangle toAdd = new Rectangle
            {
                Stroke = DrawColor1,
                Fill = DrawColor2,
                Width = Math.Abs(b.X - a.X) + drawThickness,
                Height = Math.Abs(b.Y - a.Y) + drawThickness,
                StrokeThickness = drawThickness
            };
            ThicknessConverter converter = new ThicknessConverter();
            toAdd.Margin = (Thickness)converter.ConvertFrom($"{(Math.Min(a.X, b.X) - drawThickness / 2).ToString()}, {(Math.Min(a.Y, b.Y) - drawThickness / 2).ToString()}, 0, 0");
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
                        PolygonAuxiliaryGeometry.Clear();
                        LessonCanvas.Children.Add(new Polygon()
                        {
                            Points = new PointCollection(vertices),
                            Fill = DrawColor2,
                            StrokeThickness = drawThickness,
                            Stroke = DrawColor1
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
            if(e.ChangedButton == MouseButton.Left)
            {
                switch (CurrentlyDrawing)
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
        }

        #endregion

        #region Additional effects when drawing shapes

        readonly PathGeometry PolygonAuxiliaryGeometry = new PathGeometry();

        private void RenderEffects(DrawingContext dc)
        {
            Point mouseloc = Mouse.GetPosition(this);
            Pen strokePen = new Pen(DrawColor1, drawThickness);
            switch (CurrentlyDrawing)
            {
                case DrawState.ELLIPSE:
                    if (startLocation.HasValue)
                    {
                        if(shiftPressed)
                        {
                            double radius = Math.Min(Math.Abs(mouseloc.X - startLocation.Value.X) / 2, Math.Abs(mouseloc.Y - startLocation.Value.Y) / 2);
                            dc.DrawEllipse(DrawColor2, strokePen,
                                new Point(startLocation.Value.X + radius * Math.Sign(mouseloc.X - startLocation.Value.X),
                                startLocation.Value.Y + radius * Math.Sign(mouseloc.Y - startLocation.Value.Y)), radius, radius);
                        }
                        else
                        {
                            dc.DrawEllipse(DrawColor2, strokePen,
                                new Point((mouseloc.X + startLocation.Value.X) / 2, (mouseloc.Y + startLocation.Value.Y) / 2),
                                Math.Abs(mouseloc.X - startLocation.Value.X) / 2, Math.Abs(mouseloc.Y - startLocation.Value.Y) / 2);
                        }
                    }
                    break;
                case DrawState.RECTANGLE:
                    if (startLocation.HasValue)
                    {
                        if (shiftPressed)
                        {
                            double sz = Math.Min(Math.Abs(mouseloc.X - startLocation.Value.X), Math.Abs(mouseloc.Y - startLocation.Value.Y));
                            dc.DrawRectangle(DrawColor2, strokePen, new Rect(startLocation.Value,
                                new Point(startLocation.Value.X + sz * Math.Sign(mouseloc.X - startLocation.Value.X),
                                startLocation.Value.Y + sz * Math.Sign(mouseloc.Y - startLocation.Value.Y))));
                        }
                        else
                        {
                            dc.DrawRectangle(DrawColor2, strokePen, new Rect(startLocation.Value, mouseloc));
                        }
                    }
                    break;
                case DrawState.POLYGON:
                    if (vertices.Count > 1)
                    {
                        foreach(Point vertex in vertices)
                        {
                            dc.DrawEllipse(null, new Pen(Brushes.White, 0.4), vertex, VertexSpace, VertexSpace);
                        }
                        dc.DrawGeometry(DrawColor2, strokePen, PolygonAuxiliaryGeometry);
                        dc.DrawLine(strokePen, vertices[vertices.Count - 1], mouseloc);
                    }
                    else if(vertices.Count == 1)
                    {
                        dc.DrawEllipse(null, new Pen(Brushes.White, 0.4), vertices[0], VertexSpace, VertexSpace);
                        dc.DrawLine(strokePen, vertices[0], mouseloc);
                    }
                    break;
                default:
                    break;
            }
        }

        private readonly DispatcherTimer canvasRedrawTimer = new DispatcherTimer();

        private void CanvasRedrawTimer_Tick(object sender, EventArgs e)
        {
            DrawingCanvas.InvalidateVisual();
        }

        #endregion

        #endregion

        #region Helper functions

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        private double GetDistance(Point a, Point b) => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        #endregion
    }
}
