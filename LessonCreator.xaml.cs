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

            LessonCanvas.ToDraw = RenderShapes; // TODO: OPTIMIZE THIS SHIT, IT'S HORRIBLE
            redrawTimer.Tick += RedrawTimer_Tick;
            redrawTimer.Interval = TimeSpan.FromMilliseconds(10);
            redrawTimer.Start();
        }

        private void RedrawTimer_Tick(object sender, EventArgs e)
        {
            LessonCanvas.InvalidateVisual();
        }

        private readonly DispatcherTimer redrawTimer = new DispatcherTimer();

        private void LessonCreator_Loaded(object sender, RoutedEventArgs e)
        {
            WWidth = (int)(Parent as MainWindow).ActualWidth;
            WHeight = (int)(Parent as MainWindow).ActualHeight;
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
                if(shapeCount > 0)
                {
                    shapeCount--;
                }
            }
            else if(e.Key == Key.Y && ctrlPressed)
            {
                if(shapeCount < shapes.Count)
                {
                    shapeCount++; // yes, this should be the field, not the property
                }
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

        #region Drawing shapes and graphs
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

        private readonly List<Shape> shapes = new List<Shape>();
        private readonly List<Graph> graphs = new List<Graph>();
        private readonly List<TextBlob> texts = new List<TextBlob>();

        /// <summary>
        /// DO NOT ACCESS THIS IF YOU DO NOT KNOW WHY THE PROPERTY EXISTS
        /// </summary>
        private int shapeCount = 0;

        private int ShapeCount
        {
            get => shapeCount;
            set
            {
                if(value > shapeCount)
                {
                    shapes.RemoveRange(shapeCount, shapes.Count - shapeCount);
                }
                shapeCount = value;
            }
        }

        #endregion

        private Point? startLocation;
        private readonly List<Point> vertices = new List<Point>();

        private const double VertexSpace = 8;

        #region Mouse events for editing shape attributes
        // TODO
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
            double minX = Math.Min(a.X, b.X);
            double maxX = Math.Max(a.X, b.X);
            double minY = Math.Min(a.Y, b.Y);
            double maxY = Math.Max(a.Y, b.Y);
            ShapeCount++;
            shapes.Add(new Shape(ShapeType.ELLIPSE) {
                Points = new Point[]{ new Point(minX, minY), new Point(maxX, maxY) }, 
                Stroke = DrawColor1,
                StrokeThickness = drawThickness,
                Fill = DrawColor2,
                Start = LessonTimeline.CurrentTime,
                End = -1
            });
        }

        private void CreateRectangle(Point a, Point b)
        {
            if(shiftPressed)
            {
                double sz = Math.Min(Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
                b.X = a.X + Math.Sign(b.X - a.X) * sz;
                b.Y = a.Y + Math.Sign(b.Y - a.Y) * sz;
            }
            double minX = Math.Min(a.X, b.X);
            double maxX = Math.Max(a.X, b.X);
            double minY = Math.Min(a.Y, b.Y);
            double maxY = Math.Max(a.Y, b.Y);
            ShapeCount++;
            shapes.Add(new Shape(ShapeType.RECTANGLE)
            {
                Points = new Point[] { new Point(minX, minY), new Point(maxX, maxY) },
                Stroke = DrawColor1,
                StrokeThickness = drawThickness,
                Fill = DrawColor2,
                Start = LessonTimeline.CurrentTime,
                End = -1
            });
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
                        ShapeCount++;
                        shapes.Add(new Shape(ShapeType.POLYGON)
                        {
                            Points = vertices.ToArray(),
                            Fill = DrawColor2,
                            Stroke = DrawColor1,
                            StrokeThickness = drawThickness,
                            Start = LessonTimeline.CurrentTime,
                            End = -1,
                        });
                        PolygonAuxiliaryGeometry.Clear();
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
            if(e.GetPosition(this).Y + LessonTimeline.ActualHeight > WHeight)
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

        #region Drawing shapes

        private readonly PathGeometry PolygonAuxiliaryGeometry = new PathGeometry();

        private void RenderShapes(DrawingContext dc)
        {
            Point mouseloc = Mouse.GetPosition(this);
            Pen strokePen = new Pen(DrawColor1, drawThickness);

            for(int i = 0; i < shapeCount; i++)
            {
                Shape shape = shapes[i];
                if(shape.Start <= LessonTimeline.CurrentTime && (shape.End <= LessonTimeline.CurrentTime || shape.End == -1))
                {
                    switch(shape.Type)
                    {
                        case ShapeType.ELLIPSE:
                            Point center = new Point((shape.Points[0].X + shape.Points[1].X) / 2, (shape.Points[0].Y + shape.Points[1].Y) / 2);
                            dc.DrawEllipse(shape.Fill, new Pen(shape.Stroke, shape.StrokeThickness), center, shape.Points[1].X - center.X, shape.Points[1].Y - center.Y);
                            break;
                        case ShapeType.RECTANGLE:
                            dc.DrawRectangle(shape.Fill, new Pen(shape.Stroke, shape.StrokeThickness), new Rect(shape.Points[0], shape.Points[1]));
                            break;
                        case ShapeType.POLYGON:
                            dc.DrawGeometry(shape.Fill, new Pen(shape.Stroke, shape.StrokeThickness), shape.CreateGeometry());
                            break;
                        default: break;
                    }
                }
            }

            #region Additional effects when drawing shapes

            switch (CurrentlyDrawing)
            {
                case DrawState.ELLIPSE:
                    if (startLocation.HasValue)
                    {
                        if (shiftPressed)
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
                        foreach (Point vertex in vertices)
                        {
                            dc.DrawEllipse(null, new Pen(Brushes.White, 0.4), vertex, VertexSpace, VertexSpace);
                        }
                        dc.DrawGeometry(DrawColor2, strokePen, PolygonAuxiliaryGeometry);
                        dc.DrawLine(strokePen, vertices[vertices.Count - 1], mouseloc);
                    }
                    else if (vertices.Count == 1)
                    {
                        dc.DrawEllipse(null, new Pen(Brushes.White, 0.4), vertices[0], VertexSpace, VertexSpace);
                        dc.DrawLine(strokePen, vertices[0], mouseloc);
                    }
                    break;
                default:
                    break;
            }

            #endregion
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
