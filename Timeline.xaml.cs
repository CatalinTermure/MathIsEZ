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
        private const int startGradations = 30;
        private const int step = 10;
        private const int zoomSpeed = 120;
        private const double sideMargin = 50, upMargin = 20;
        private const double numberSize = 10;

        public int Gradations
        {
            get => gradations;
            set
            {
                if(value > 5 && value < 100)
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
                if(value >= 0 && CurrentTime - value <= Gradations * step && CurrentTime >= value)
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

            dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, 72, 72, 72)), new Pen(Brushes.Black, 0), new Rect(new Point(sideMargin, upMargin), new Size(ActualWidth - 2 * sideMargin, ActualHeight)));

            bool isSmallLine = false;
            double linePosition = sideMargin + (CurrentTime - StartTime) * (ActualWidth - 2 * sideMargin) / (step * Gradations);

            FormattedText txt = new FormattedText(CurrentTime.ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight, new Typeface("Futura"), numberSize, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);

            for (int t = (int)(Math.Ceiling(StartTime / 10.0) * 10); t <= StartTime + Gradations * step; isSmallLine = !isSmallLine, t += step / 2)
            {
                FormattedText txtTmp = new FormattedText(t.ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight, new Typeface("Futura"), numberSize, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                double linePositionTmp = sideMargin + (t - StartTime) * (ActualWidth - 2 * sideMargin) / (step * Gradations);

                if((Math.Abs(linePositionTmp - linePosition) > 1 + txt.Width / 2 + txtTmp.Width / 2) && !isSmallLine)
                {
                    dc.DrawText(txtTmp, new Point(linePositionTmp - txtTmp.Width / 2, upMargin - 15));
                }

                dc.DrawLine(isSmallLine ? smallLinePen : linePen, new Point(linePositionTmp, upMargin), new Point(linePositionTmp, ActualHeight));
            }

            dc.DrawText(txt, new Point(linePosition - txt.Width / 2, upMargin - 15));

            dc.DrawLine(new Pen(Brushes.White, 1.5), new Point(linePosition, upMargin), new Point(linePosition, ActualHeight));
        }

        public Timeline()
        {
            InitializeComponent();

            VisualCanvas.ToDraw = DrawTimeline;

            Loaded += Timeline_Loaded;

            MouseWheel += Timeline_MouseWheel;
            PreviewMouseLeftButtonDown += Timeline_PreviewMouseLeftButtonDown;
            MouseDown += Timeline_MouseDown;
            MouseMove += Timeline_MouseMove;
            MouseUp += Timeline_MouseUp;
            JumpButton.Click += JumpButton_Click;
        }

        private void JumpButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException("Implement jumping through right click menu :)");
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
                StartTime = lastStartTime + (int)((startDragPoint.X - e.GetPosition(this).X) * Gradations / startGradations);
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

        private void Timeline_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(this);
            
            if(location.X >= sideMargin && location.X + sideMargin < ActualWidth)
            {
                CurrentTime = (int)Math.Round(StartTime + (location.X - sideMargin) * (step * Gradations) / (ActualWidth - 2 * sideMargin));
            }

            e.Handled = true;
        }

        private void Timeline_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Gradations += e.Delta / zoomSpeed;
        }

        private void Timeline_Loaded(object sender, RoutedEventArgs e)
        {
            VisualCanvas.InvalidateVisual();
        }
    }
}
