using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

            ShapesCanvas.ToDraw = RenderShapes; // TODO: OPTIMIZE THIS
            AdditionalEffectsCanvas.ToDraw = RenderAdditionalEffects;
            EffectsRedrawTimer.Tick += RedrawTimer_Tick;
            EffectsRedrawTimer.Interval = TimeSpan.FromMilliseconds(10);
        }

        private void RedrawTimer_Tick(object sender, EventArgs e)
        {
            AdditionalEffectsCanvas.InvalidateVisual();
        }

        private readonly DispatcherTimer EffectsRedrawTimer = new DispatcherTimer();

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
                if(ShapeCount > 0)
                {
                    ShapeCount--;
                }
            }
            else if(e.Key == Key.Y && ctrlPressed)
            {
                if(ShapeCount < Shapes.Count)
                {
                    ShapeCount++;
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
            ShapesCanvas.Focus();
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

        private readonly List<Tuple<Shape, int>> Shapes = new List<Tuple<Shape, int>>();
        private readonly List<Tuple<Graph, int>> Graphs = new List<Tuple<Graph, int>>();
        private readonly List<Tuple<TextBlob, int>> Texts = new List<Tuple<TextBlob, int>>();

        private int shapeCount = 0;

        private int ShapeCount
        {
            get => shapeCount;
            set
            {
                shapeCount = value;
                ShapesCanvas.InvalidateVisual();
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

        /// <summary>
        /// Trims the oldest shapes so that only count shapes remain.
        /// </summary>
        /// <param name="count"> Number of shapes that should remain </param>
        private void TrimShapes(int count)
        {
            for(int i = 0; i < Shapes.Count; i++)
            {
                if(Shapes[i].Item2 > count)
                {
                    Shapes.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Creates ellipse defined by the rectangle defined by the two points
        /// </summary>
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
            if (ShapeCount < Shapes.Count)
            {
                TrimShapes(ShapeCount);
            }
            InsertInOrderedList(Shapes, new Tuple<Shape, int>(new Shape(ShapeType.ELLIPSE)
            {
                Points = new Point[] { new Point(minX, minY), new Point(maxX, maxY) },
                Stroke = DrawColor1,
                StrokeThickness = drawThickness,
                Fill = DrawColor2,
                Start = LessonTimeline.CurrentTime,
                End = -1
            }, ShapeCount + 1), ShapeComparer);
            ShapeCount++;
        }

        /// <summary>
        /// Creates polygon based on the vertices field
        /// </summary>
        private void CreatePolygon()
        {
            if(ShapeCount < Shapes.Count)
            {
                TrimShapes(ShapeCount);
            }
            InsertInOrderedList(Shapes, new Tuple<Shape, int>(new Shape(ShapeType.POLYGON)
            {
                Points = vertices.ToArray(),
                Fill = DrawColor2,
                Stroke = DrawColor1,
                StrokeThickness = drawThickness,
                Start = LessonTimeline.CurrentTime,
                End = -1,
            }, ShapeCount + 1), ShapeComparer);
            ShapeCount++;
            PolygonAuxiliaryGeometry.Clear();
            vertices.Clear();
        }

        /// <summary>
        /// Creates rectangle defined by two points
        /// </summary>
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
            if (ShapeCount < Shapes.Count)
            {
                TrimShapes(ShapeCount);
            }
            InsertInOrderedList(Shapes, new Tuple<Shape, int>(new Shape(ShapeType.RECTANGLE)
            {
                Points = new Point[] { new Point(minX, minY), new Point(maxX, maxY) },
                Stroke = DrawColor1,
                StrokeThickness = drawThickness,
                Fill = DrawColor2,
                Start = LessonTimeline.CurrentTime,
                End = -1
            }, ShapeCount + 1), ShapeComparer);
            ShapeCount++;
        }

        #endregion

        #region Mouse events for inserting shapes

        private bool mouseup = true;

        private void LessonCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EffectsRedrawTimer.Start();
                // Drawing Ellipses and Recntangles
                if (CurrentlyDrawing == DrawState.ELLIPSE || CurrentlyDrawing == DrawState.RECTANGLE)
                {
                    if (startLocation == null)
                    {
                        startLocation = e.GetPosition(this);
                    }
                }
                // Drawing Triangles
                else if (mouseup && CurrentlyDrawing == DrawState.TRIANGLE)
                {
                    mouseup = false;
                    vertices.Add(e.GetPosition(this));
                    if (vertices.Count == 3)
                    {
                        vertices.Clear();
                    }
                }
                // Drawing Polygons
                else if (mouseup && CurrentlyDrawing == DrawState.POLYGON)
                {
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
                        CreatePolygon();
                    }
                    else
                    {
                        vertices.Add(e.GetPosition(this));
                        if (vertices.Count > 1)
                        {
                            PolygonAuxiliaryGeometry.Figures[0].Segments.Add(new LineSegment(vertices[^1], true));
                        }
                        else if (vertices.Count == 1)
                        {
                            PolygonAuxiliaryGeometry.Figures.Add(new PathFigure() { StartPoint = vertices[0] });
                        }
                    }
                }
            }
            else if(e.ChangedButton == MouseButton.Right)
            {
                // TODO: Shape menu for animations and others
            }
        }

        public void LessonCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                switch (CurrentlyDrawing)
                {
                    case DrawState.ELLIPSE:
                        if(!startLocation.HasValue)
                        {
                            return;
                        }
                        CreateEllipse(startLocation.Value, e.GetPosition(this));
                        EffectsRedrawTimer.Stop();
                        break;
                    case DrawState.RECTANGLE:
                        if (!startLocation.HasValue)
                        {
                            return;
                        }
                        CreateRectangle(startLocation.Value, e.GetPosition(this));
                        EffectsRedrawTimer.Stop();
                        break;
                    default:
                        break;
                }

                startLocation = null;
                mouseup = true;
                AdditionalEffectsCanvas.InvalidateVisual();
            }
        }
        #endregion

        #region Rendering shapes and effects

        private readonly PathGeometry PolygonAuxiliaryGeometry = new PathGeometry();

        private void RenderShapes(DrawingContext dc)
        {
            int time = LessonTimeline.CurrentTime;
            // lower bound search for time
            int lo = -1, hi = Shapes.Count, mid;
            while (hi - lo > 1)
            {
                mid = (lo + hi) / 2;
                if (Shapes[mid].Item1.Start < time)
                    lo = mid;
                else
                    hi = mid;
            }
            if(hi < 0 || hi > Shapes.Count || Shapes.Count == 0)
            {
                return;
            }
            if(hi == Shapes.Count || Shapes[hi].Item1.Start > time)
            {
                hi--;
            }
            for(int i = 0; i <= hi; i++)
            {
                if(Shapes[i].Item2 > ShapeCount)
                {
                    continue;
                }
                Shape shape = Shapes[i].Item1;
                if(shape.End >= time || shape.End == -1)
                {
                    switch (shape.Type)
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
        }

        private void RenderAdditionalEffects(DrawingContext dc)
        {
            Point mouseloc = Mouse.GetPosition(this);
            Pen strokePen = new Pen(DrawColor1, drawThickness);

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
                        dc.DrawLine(strokePen, vertices[^1], mouseloc);
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
        }

        #endregion

        #endregion

        #region Helper functions

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        private double GetDistance(Point a, Point b) => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        #region Compare functions for ordering lists

        private class ShapeCompare : Comparer<Tuple<Shape, int>>
        {
            public override int Compare(Tuple<Shape, int> x, Tuple<Shape, int> y)
            {
                if (x.Item1.Start == y.Item1.Start)
                    return 0;
                else if (x.Item1.Start < y.Item1.Start)
                    return -1;
                else
                    return 1;
            }
        };

        private readonly ShapeCompare ShapeComparer = new ShapeCompare();

        #endregion

        private void InsertInOrderedList<T>(List<T> l, T item, Comparer<T> comp)
        {
            int lo = -1, hi = l.Count, mid;
            while (hi - lo > 1)
            {
                mid = (hi + lo) / 2;
                if (comp.Compare(l[mid], item) < 0)
                    lo = mid;
                else
                    hi = mid;
            }
            l.Insert(hi, item);
        }

        #endregion
    }
}
