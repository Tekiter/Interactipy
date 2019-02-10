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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using Interactipy.Engine;
using System.IO;
using System.Diagnostics;

namespace Interactipy
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
        }

        Thread readOutputThread;
        Thread readinformationthread;
        PyEngine engine;

        private void txt_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //txt_output.Text += txt_input.Text + "\n";

                txt_output.AppendText(txt_input.Text + "\n");
                engine.Execute(txt_input.Text);
                
                txt_input.Clear();
            }
            
        }

        private void readOutputLoop()
        {
            while (true)
            {
                //if (engine.OutputStream.Peek() < 0)
                //    continue;

                char ch = (char)engine.OutputStream.Read();
                DispatcherOperation op = Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.txt_output.AppendText(ch.ToString());
                }));
            }
        }

        private void readInformationLoop()
        {
            while (true)
            {
                //if (engine.InformationStream.Peek() < 0)
                //    continue;
                char ch = (char)engine.InformationStream.Read();
                DispatcherOperation op = Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.txt_output.AppendText(ch.ToString());
                }));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            engine = new PyExeEngine(@"C:\Users\geon0\AppData\Local\Programs\Python\Python37-32\python.exe");
            engine.Start(Directory.GetCurrentDirectory());
            engine.WorkFinished += (s, ew) => {
                Debug.WriteLine("py>");
            };
            engine.ErrorOccured += (s, ew) => {
                Debug.WriteLine(ew.ErrorMessage);
            };

            readOutputThread = new Thread(new ThreadStart(readOutputLoop));
            readinformationthread = new Thread(new ThreadStart(readInformationLoop));
            //readinformationthread.Start();
            readOutputThread.Start();

            Debug.WriteLine("Loaded!");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            engine.Stop();
        }

        private void txt_output_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
