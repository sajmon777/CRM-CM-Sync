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
using System.Windows.Shapes;
using System.ServiceProcess;
using System.Threading; 

namespace Sync_WPFBlend
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        ServiceController controler;

        public ServiceWindow()
        {
            this.InitializeComponent();
            controler = new ServiceController("Sync Crm Cm Service");
            ControlLight();
        }
        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.Status == ServiceControllerStatus.Running)
            {
                controler.Stop();
                controler = new ServiceController("Sync Crm Cm Service");
                Thread.Sleep(500); 
                ControlLight();
            }
        }
        
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (controler.Status == ServiceControllerStatus.Stopped)
            {
                controler.Start();
                controler = new ServiceController("Sync Crm Cm Service");
                Thread.Sleep(500); 
                ControlLight();
            }
        }


        private void ControlLight()
        {
            if (controler.Status == ServiceControllerStatus.Running)
            {
                RunLight.Visibility = System.Windows.Visibility.Visible;
                StopLight.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {

                RunLight.Visibility = System.Windows.Visibility.Hidden;
                StopLight.Visibility = System.Windows.Visibility.Visible;

            }
        }
    }
}