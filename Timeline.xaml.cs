using System;
using System.Collections.Generic;
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

namespace MathIsEZ
{
    /// <summary>
    /// Interaction logic for Timeline.xaml
    /// </summary>
    public partial class Timeline : UserControl
    {
        private int startTime, currentTime = 10;
        private int gradations = 30;
        private const int scale = 10;
        private double startX = 50;

        public int Gradations
        {
            get => gradations;
            set
            {
                if(value > 5)
                {
                    gradations = value;
                    VisualCanvas.InvalidateVisual();
                }
            }
        }

        public int StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                VisualCanvas.InvalidateVisual();
            }
        }

        public int CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;
                VisualCanvas.InvalidateVisual();
            }
        }

        private void DrawTimeline(DrawingContext dc)
        {
            if(ActualWidth == 0 || ActualHeight == 0)
            {
                // If the Timeline isn't visibly rendered, we should not try to draw the canvas
                return;
            }

            Pen linePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 16, 16, 16)), 1.5);
            Pen smallLinePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 16, 16, 16)), 0.5);
            double step = (ActualWidth - 2 * startX) / Gradations;

            dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, 72, 72, 72)), new Pen(Brushes.Black, 0), new Rect(new Point(startX, 0), new Size(ActualWidth - 2 * startX, ActualHeight)));

            for(int i = 1; i <= Gradations; i++)
            {
                dc.DrawLine(linePen, new Point(startX + step * i, 0), new Point(startX + step * i, ActualHeight));
                dc.DrawLine(smallLinePen, new Point(startX + step * i - step / 2, 0), new Point(startX + step * i - step / 2, ActualHeight));
            }
        }

        public Timeline()
        {
            InitializeComponent();

            VisualCanvas.ToDraw = DrawTimeline;

            Loaded += Timeline_Loaded;

            MouseWheel += Timeline_MouseWheel;
            MouseLeftButtonDown += Timeline_MouseLeftButtonDown;
        }

        private void Timeline_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(this);
        }

        private void Timeline_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Gradations += e.Delta / 120;
        }

        private void Timeline_Loaded(object sender, RoutedEventArgs e)
        {
            VisualCanvas.InvalidateVisual();
        }


    }
}
