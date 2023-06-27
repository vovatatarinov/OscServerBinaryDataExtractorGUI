using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using System.IO;

namespace OscServerBinaryDataExtractorGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private String fileName;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                textBlock1.Text = "Открыт файл: ";
                textBlock1.Text += openFileDialog.SafeFileName;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if ((fileName == "") || (fileName == null))
            {
                textBlock1.Text = "Сначала открой файл.";
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = System.IO.Path.ChangeExtension(fileName, "csv");
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                textBlock1.Text = "Сохраняю файл...";
                FileStream fs_in = File.OpenRead(fileName);
                long in_size = fs_in.Length;
                FileStream fs_out = File.OpenWrite(saveFileDialog.FileName);
                fs_in.Seek(0x1E, SeekOrigin.Begin);
                for (long i = 0x1E; i < in_size; ++i)
                {
                    byte c = (byte) fs_in.ReadByte();
                    sbyte sc = (sbyte)c;
                    int val = sc;
                    string s_val = val.ToString() + Environment.NewLine;
                    byte[] b_val = new UTF8Encoding(true).GetBytes(s_val.ToCharArray());
                    fs_out.Write(b_val, 0, b_val.Length);


                }
                fs_in.Close();
                fs_out.Close();
                textBlock1.Text = "Файл сохранен.";
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fileName = ""; //Обнуляем строку с именем файла
        }

    }
}
