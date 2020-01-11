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
        // variables/constants for converting database coordinates to draw coordinates
        private int WWidth, WHeight;
        private const int EWidth = 1920, EHeight = 1080;

        public DrawState currentlyDrawing = DrawState.NONE;

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            SToolbar.Visibility = Visibility.Visible;
            BtnShow.Visibility = Visibility.Collapsed;
        }

        private void LessonCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

        public LessonCreator()
        {
            InitializeComponent();

            Loaded += LectureCreator_Loaded;
        }

        private void LectureCreator_Loaded(object sender, RoutedEventArgs e)
        {
            WWidth = (int)(Parent as MainWindow).ActualWidth;
            WHeight = (int)(Parent as MainWindow).ActualHeight;
        }
    }
}
