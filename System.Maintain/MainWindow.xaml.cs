using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace System.Maintain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CommandLine.CmdBatch();
            InitializeComponent();
            Loaded += delegate
            {
                (this.DataContext as MainViewModel).RichBox = RichBox;
            };
        }

        private void RiseEvent(object sender, TextChangedEventArgs e)
        {
            RichBox.ScrollToEnd();
        }

        private void ShowAnimeEvent(object sender, RoutedEventArgs e)
        {
            ((Storyboard)FindResource("NavbarAnimeStyle")).Begin();
        }
    }
}
