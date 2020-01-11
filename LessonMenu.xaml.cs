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
    /// Interaction logic for LectureMenu.xaml
    /// </summary>
    public partial class LessonMenu : UserControl
    {
        public LessonMenu()
        {
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            (Parent as MainWindow).Content = new LessonCreator();
        }
    }
}
