using Interactipy.Engine;
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

namespace Interactipy
{
    /// <summary>
    /// TestWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestWindow : Window
    {

        public EnvironmentSelector Environ { get; set; }

        public TestWindow()
        {
            InitializeComponent();
            DataContext = this;
            Environ = new EnvironmentSelector();
            
        }

        private void PyExe_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Environ.Refresh();

            PyExeList.Items.Clear();

            foreach (var i in Environ.Environments)
            {
                PyExeList.Items.Add(i.Version);
            }
        }

        PyProcess curproc = null;
        private void Btn_go_Click(object sender, RoutedEventArgs e)
        {
            int selidx = PyExeList.SelectedIndex;
            if (selidx >= 0)
            {

                if (curproc != null)
                {
                    curproc.RawProcess.Refresh();
                    curproc.RawProcess.Close();
                    txt_content.Clear();
                }

                var environ = Environ.Environments[selidx];

                var proc = environ.Run("-i", Environment.CurrentDirectory);

                proc.RawProcess.OutputDataReceived += RawProcess_OutputDataReceived;
                proc.RawProcess.ErrorDataReceived += RawProcess_OutputDataReceived;
                proc.RawProcess.BeginOutputReadLine();
                proc.RawProcess.BeginErrorReadLine();

                curproc = proc;
            }
        }

        private void RawProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            txt_content.Dispatcher.BeginInvoke(new Action(() => {
                txt_content.Text += e.Data + "\n";
            }), null);
            
        }

        private void Txt_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                curproc.RawProcess.StandardInput.WriteLine(txt_input.Text);

                txt_input.Clear();
            }
        }
    }
}
