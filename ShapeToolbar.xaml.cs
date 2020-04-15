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

        // Logic for collapsing ShapeToolbar
        private void BtnHide_Click(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).BtnShow.Visibility = Visibility.Visible;
            Visibility = Visibility.Collapsed;
            ((Parent as Grid).Parent as LessonCreator).ShapesCanvas.Focus();
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
            if(highlightedButton == sender)
            {
                highlightedButton = null;
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.NONE;
                (sender as Button).Background = DefaultBtnColor;
            }
            else
            {
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.ELLIPSE;
                HighlightButton(sender as Button);
            }
        }

        private void BtnRectangle_Click(object sender, RoutedEventArgs e)
        {
            if (highlightedButton == sender)
            {
                highlightedButton = null;
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.NONE;
                (sender as Button).Background = DefaultBtnColor;
            }
            else
            {
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.RECTANGLE;
                HighlightButton(sender as Button);
            }
        }

        private void BtnPolygon_Click(object sender, RoutedEventArgs e)
        {
            if (highlightedButton == sender)
            {
                highlightedButton = null;
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.NONE;
                (sender as Button).Background = DefaultBtnColor;
            }
            else
            {
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.POLYGON;
                HighlightButton(sender as Button);
            }
        }

        private void BtnTriangle_Click(object sender, RoutedEventArgs e)
        {
            if (highlightedButton == sender)
            {
                highlightedButton = null;
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.NONE;
                (sender as Button).Background = DefaultBtnColor;
            }
            else
            {
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.TRIANGLE;
                HighlightButton(sender as Button);
            }
        }

        private void BtnGraph_Click(object sender, RoutedEventArgs e)
        {
            if (highlightedButton == sender)
            {
                highlightedButton = null;
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.NONE;
                (sender as Button).Background = DefaultBtnColor;
            }
            else
            {
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.GRAPH;
                HighlightButton(sender as Button);
            }
        }

        private void BtnText_Click(object sender, RoutedEventArgs e)
        {
            if (highlightedButton == sender)
            {
                highlightedButton = null;
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.NONE;
                (sender as Button).Background = DefaultBtnColor;
            }
            else
            {
                ((Parent as Grid).Parent as LessonCreator).CurrentlyDrawing = DrawState.TEXT;
                HighlightButton(sender as Button);
            }
        }

        #endregion

        #region Additional option logic

        private void FillCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).doFill = true;
        }

        private void FillCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Parent as Grid).Parent as LessonCreator).doFill = false;
        }

        #endregion
    }
}
