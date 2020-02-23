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
        private int startTime, currentTime = 0;
        private int gradations = 30;
        private const int scale = 10;
        private const double startX = 50, startY = 20;

        public int Gradations
        {
            get => gradations;
            set
            {
                if(value > 5 && value < 100 && CurrentTime + StartTime <= value * scale)
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
                if(value > 0 && CurrentTime - value <= Gradations * scale && CurrentTime >= value)
                {
                    startTime = value;
                    VisualCanvas.InvalidateVisual();
                }
            }
        }

        public int CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;
                VisualCanvas.InvalidateVisual();
                ((Parent as Grid).Parent as LessonCreator).LessonCanvas.InvalidateVisual();
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

            dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, 72, 72, 72)), new Pen(Brushes.Black, 0), new Rect(new Point(startX, startY), new Size(ActualWidth - 2 * startX, ActualHeight)));

            double gradationPosition = startX + ((double)(-StartTime % scale) / scale * step);
            if(gradationPosition < startX)
            {
                gradationPosition += startX;
            }
            double upLimit = ActualWidth - startX;
            for (int i = 0; gradationPosition <= upLimit; i++, gradationPosition += step)
            {
                dc.DrawText(new FormattedText($"{(i + StartTime / scale + (StartTime % scale != 0 ? 1 : 0)) * scale}", System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight, new Typeface("Futura"), (11 - Gradations / 30) * ActualWidth / 1570d, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                    new Point(gradationPosition - 10, startY - 15 + Gradations / 30));
                dc.DrawLine(linePen, new Point(gradationPosition, startY), 
                    new Point(gradationPosition, ActualHeight));
                if(gradationPosition + step / 2 <= upLimit)
                {
                    dc.DrawLine(smallLinePen, new Point(gradationPosition + step / 2, startY),
                       new Point(gradationPosition + step / 2, ActualHeight));
                }
            }

            if(CurrentTime % scale != 0)
            {
                dc.DrawText(new FormattedText(CurrentTime.ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight, new Typeface("Futura"), (11 - Gradations / 30) * ActualWidth / 1570d, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip),
                    new Point(startX - 5 + ((double)(CurrentTime - StartTime) / scale) * step, startY - 15));
            }

            dc.DrawLine(new Pen(Brushes.White, 1.5), new Point(startX + ((double)(CurrentTime - StartTime) / scale) * step, startY), new Point(startX + ((double)(CurrentTime - StartTime) / scale) * step, ActualHeight));
        }

        public Timeline()
        {
            InitializeComponent();

            VisualCanvas.ToDraw = DrawTimeline;

            Loaded += Timeline_Loaded;

            MouseWheel += Timeline_MouseWheel;
            MouseLeftButtonDown += Timeline_MouseLeftButtonDown;
            MouseDown += Timeline_MouseDown;
            MouseMove += Timeline_MouseMove;
            MouseUp += Timeline_MouseUp;
        }

        private void Timeline_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Middle)
            {
                middleMousePressed = false;
            }
        }

        private bool middleMousePressed = false;
        private Point startDragPoint;
        private int lastStartTime;

        private void Timeline_MouseMove(object sender, MouseEventArgs e)
        {
            if(middleMousePressed)
            {
                StartTime = lastStartTime + (int)(startDragPoint.X - e.GetPosition(this).X) * Gradations / 30;
            }
        }

        private void Timeline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Middle)
            {
                middleMousePressed = true;
                startDragPoint = e.GetPosition(this);
                lastStartTime = StartTime;
            }
        }

        private void Timeline_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(this);

            CurrentTime = StartTime + (int)Math.Round(Math.Min(ActualWidth - 2 * startX, Math.Max(0d, (location.X - startX))) / (ActualWidth - (2 * startX)) * Gradations * scale);
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
