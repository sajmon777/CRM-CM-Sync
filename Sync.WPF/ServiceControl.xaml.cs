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
using System.Windows.Shapes;
using System.ServiceProcess;

namespace Sync.WPF
{
    /// <summary>
    /// Interaction logic for ServiceControl.xaml
    /// </summary>
    public partial class ServiceControl : Window
    {
        ServiceController controler;


        public ServiceControl()
        {
            InitializeComponent();

            controler = new ServiceController("Sync Crm Cm Service");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controler.Start(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            controler.Stop(); 
        }

    }
}
