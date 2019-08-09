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
    }
}
