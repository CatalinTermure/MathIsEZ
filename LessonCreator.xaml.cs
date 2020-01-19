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
        GRAPH
    }

    public partial class LessonCreator : UserControl
    {
        public LessonCreator()
        {
            InitializeComponent();

            Loaded += LectureCreator_Loaded;
        }

        #region Drawing Shapes
        // Logic for inserting shapes

        public DrawState currentlyDrawing = DrawState.NONE;
        private SolidColorBrush drawColor1 = new SolidColorBrush(Colors.White);
        private SolidColorBrush drawColor2 = new SolidColorBrush(Colors.Black);

        #region Logic for changing draw color

        private void UpdateColor1(SolidColorBrush brush)
        {
            SToolbar.Color1Btn.Foreground = brush;
            SToolbar.Color1Btn.InvalidateVisual();
        }

        private void UpdateColor2(SolidColorBrush brush)
        {
            SToolbar.Color2Btn.Foreground = brush;
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

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            SToolbar.Visibility = Visibility.Visible;
            BtnShow.Visibility = Visibility.Collapsed;
        }

        private List<Shape> shapes;
        private List<Graph> graphs;

        private Point? startLocation;
        private List<Point> vertices;
        private bool mouseup = true;
        private void LessonCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(currentlyDrawing == DrawState.ELLIPSE || currentlyDrawing == DrawState.RECTANGLE)
            {
                if (startLocation == null)
                {
                    startLocation = e.GetPosition(this);
                }
            }
            else if(mouseup && currentlyDrawing == DrawState.TRIANGLE)
            {
                mouseup = false;
                vertices.Add(e.GetPosition(this));
                if (vertices.Count == 3)
                {
                    shapes.Add(new Shape(ShapeType.TRIANGLE, vertices.ToArray()));
                    vertices.Clear();
                }
            }
            else if(mouseup && currentlyDrawing == DrawState.POLYGON)
            {
                vertices.Add(e.GetPosition(this));
            }
        }

        private void LessonCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch(currentlyDrawing)
            {
                case DrawState.ELLIPSE:
                    break;
                case DrawState.RECTANGLE:
                    break;
                case DrawState.TRIANGLE:
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

        #region Adjusting to resolution
        // variables/constants for converting database coordinates to draw coordinates
        private int WWidth, WHeight;
        private const int EWidth = 1920, EHeight = 1080;

        private void LectureCreator_Loaded(object sender, RoutedEventArgs e)
        {
            WWidth = (int)(Parent as MainWindow).ActualWidth;
            WHeight = (int)(Parent as MainWindow).ActualHeight;
        }

        #endregion
    }
}
