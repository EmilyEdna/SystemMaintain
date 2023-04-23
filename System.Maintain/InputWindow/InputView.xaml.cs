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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace System.Maintain.InputWindow
{
    /// <summary>
    /// InputView.xaml 的交互逻辑
    /// </summary>
    public partial class InputView : Window
    {

        public InputView(string Tip)
        {
            InitializeComponent();
            TipTitle.Text = Tip;
            MouseLeftButtonDown += (sender, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            };
        }

        private void HandlerEvent(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();

        }
    }
}
