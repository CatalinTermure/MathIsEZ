using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl
    {
        public StartMenu()
        {
            InitializeComponent();
        }

        private void BtnLectii_Click(object sender, RoutedEventArgs e)
        {
            (Parent as MainWindow).Content = new LessonMenu();
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            (Parent as MainWindow).Close();
        }
    }
}
