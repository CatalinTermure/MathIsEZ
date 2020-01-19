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
using System.Reflection;

namespace MathIsEZ
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();

            Loaded += ColorPicker_Loaded;
        }

        #region LoadColors

        /// <summary>
        /// Helper variable for loading colors
        /// </summary>
        const int nrCol = 3, nrRow = 4;
        readonly Color[] colors =
        {
            Colors.Black,
            Colors.White,
            Colors.Green,
            Colors.Red,
            Colors.Blue,
            Colors.Yellow,
            Colors.Purple,
            Colors.Pink,
            Colors.Orange,
            Colors.Brown,
            Colors.LimeGreen,
            Colors.LightBlue,
        };

        /// <summary>
        /// Loads all the colors for the ColorPicker
        /// </summary>
        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < nrRow; i++)
            {
                ColorPickerGrid.RowDefinitions.Add(new RowDefinition());
            }
            for(int j = 0; j < nrCol; j++)
            {
                ColorPickerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for(int i = 0; i < nrRow; i++)
            {
                for(int j = 0; j < nrCol; j++)
                {
                    Button item = new Button()
                    {
                        Foreground = new SolidColorBrush(colors[i * nrCol + j]),
                        Style = (Style)styles["ColorButton"],
                    };
                    item.PreviewMouseDown += BtnColor_PreviewMouseDown;
                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);
                    ColorPickerGrid.Children.Add(item);
                }
            }
        }

        private void BtnColor_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                (((((Parent as Grid).Parent as Grid).Parent as ShapeToolbar).Parent as Grid).Parent as LessonCreator).DrawColor1 = (SolidColorBrush)(sender as Button).Foreground;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                (((((Parent as Grid).Parent as Grid).Parent as ShapeToolbar).Parent as Grid).Parent as LessonCreator).DrawColor2 = (SolidColorBrush)(sender as Button).Foreground;
            }
        }

        #endregion
    }
}
