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
    /// Interaction logic for ShapeToolbar.xaml
    /// </summary>
    public partial class ShapeToolbar : UserControl
    {
        public ShapeToolbar()
        {
            InitializeComponent();
        }

        #region Button click handlers

        private readonly SolidColorBrush DefaultBtnColor = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));
        private readonly SolidColorBrush HighlightBtnColor = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
        private Button highlightedButton;

        private void BtnHide_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).BtnShow.Visibility = Visibility.Visible;
            Visibility = Visibility.Collapsed;
        }

        private void HighlightButton(Button btn)
        {
            if(highlightedButton != null)
            {
                highlightedButton.Background = DefaultBtnColor;
            }
            btn.Background = HighlightBtnColor;
            highlightedButton = btn;
        }

        private void BtnEllipse_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).currentlyDrawing = DrawState.ELLIPSE;
            HighlightButton(sender as Button);
        }

        private void BtnRectangle_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).currentlyDrawing = DrawState.RECTANGLE;
            HighlightButton(sender as Button);
        }

        private void BtnPolygon_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).currentlyDrawing = DrawState.POLYGON;
            HighlightButton(sender as Button);
        }

        private void BtnTriangle_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).currentlyDrawing = DrawState.TRIANGLE;
            HighlightButton(sender as Button);
        }

        private void BtnGraph_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).currentlyDrawing = DrawState.GRAPH;
            HighlightButton(sender as Button);
        }

        private void BtnText_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).currentlyDrawing = DrawState.TEXT;
            HighlightButton(sender as Button);
        }

        #endregion
    }
}
