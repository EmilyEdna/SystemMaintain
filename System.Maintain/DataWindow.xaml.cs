using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace System.Maintain
{
    /// <summary>
    /// DataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow()
        {
            InitializeComponent();
        }

        private void Des(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(A2.Text))
            {
                A1.Text = LZStringCSharp.LZString.DecompressFromBase64(A2.Text);
            }
        }

        private void Enc(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(A1.Text))
            {
                A2.Text = LZStringCSharp.LZString.CompressToBase64(A1.Text);
            }
        }

        private void OpenDes(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "数据|*.sql"
            };
            if (dialog.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();
                using StreamReader sr = new StreamReader(dialog.FileName);
                string str = String.Empty;
                while ((str = sr.ReadLine()) != null)
                {
                    sb.Append($"{str}\n");
                }
                sr.Close();
                sr.Dispose();
                A1.Text = sb.ToString();
            }
        }

        private void OpenEnc(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "数据|*.sql"
            };
            if (dialog.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();
                using StreamReader sr = new StreamReader(dialog.FileName);
                string str = String.Empty;
                while ((str = sr.ReadLine()) != null)
                {
                    sb.Append(str);
                }
                sr.Close();
                sr.Dispose();
                A2.Text = sb.ToString();
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).CommandParameter.ToString() == "2")
            {
                if (!string.IsNullOrEmpty(A2.Text))
                {
                    //创建一个保存文件式的对话框
                    SaveFileDialog sfd = new SaveFileDialog();
                    //设置这个对话框的起始保存路径
                    sfd.InitialDirectory = @"C:\";
                    //设置保存的文件的类型，注意过滤器的语法
                    sfd.Filter = "数据|*.sql";
                    if (sfd.ShowDialog() == true)
                    {
                        StreamWriter streamWriter = new StreamWriter(sfd.FileName, true);
                        streamWriter.Write(this.A2.Text);
                        streamWriter.Close();
                    }
                }
            }
            else {
                if (!string.IsNullOrEmpty(A1.Text))
                {
                    //创建一个保存文件式的对话框
                    SaveFileDialog sfd = new SaveFileDialog();
                    //设置这个对话框的起始保存路径
                    sfd.InitialDirectory = @"C:\";
                    //设置保存的文件的类型，注意过滤器的语法
                    sfd.Filter = "数据|*.sql";
                    if (sfd.ShowDialog() == true)
                    {
                        StreamWriter streamWriter = new StreamWriter(sfd.FileName, true);
                        streamWriter.Write(this.A1.Text);
                        streamWriter.Close();
                    }
                }
            }
        }

        private void ClearBox(object sender, RoutedEventArgs e)
        {
            A1.Text = A2.Text = string.Empty;
        }
    }
}
