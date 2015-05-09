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
using Sync.BL.CM;
using Sync.BL.CRM;
using Sync.BL.Infrastructure;
using Sync.BL.Entities;
using System.Threading;
using System.ServiceProcess;

namespace Sync_WPFBlend
{
	/// <summary>
	/// Interaction logic for SyncWindow.xaml
	/// </summary>
	public partial class SyncWindow : Window
	{
        CMRepository cmRepo;
        CMSettings cmSettings;
        CRMRepository crmRepo;
        CRMSettings crmSettings;
        List<CrmCmConnection> crmCmConnections;
        Thread readFromRepo;
        List<Sync.BL.Entities.List> cmList;
        List<Sync.BL.Entities.List> crmList;
        ServiceController controler;

        public SyncWindow()
		{
			this.InitializeComponent();
            LoadingScreen.Visibility = System.Windows.Visibility.Visible; 
            readFromRepo = new Thread(LoadingData);
            readFromRepo.IsBackground = true;
            readFromRepo.Start();
            controler = new ServiceController("Sync Crm Cm Service");
            ControlLight();
        }

        private void LoadingData()
        {
            try
            {
                while (true)
                {
                    InitializeRepository();
                    InitializeComboBox();
                    InitializeTable();
                    Thread.Sleep(60000); 
                }
            
            }
            catch (Exception e)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Faild to load data" + "\n" + "Check your network connections");
                    LoadingScreen.Visibility = System.Windows.Visibility.Hidden;
                })); 
            
            }
        }

        private void InitializeRepository()
        {
            cmSettings = ConnectionSettingsManager.GetCMSettings();
            cmRepo = new CMRepository(cmSettings);
            crmSettings = ConnectionSettingsManager.GetCRMSettings();
            crmRepo = new CRMRepository(crmSettings);
        }

        private void InitializeComboBox()
        {
            Client cmClient = cmRepo.GetClients().FirstOrDefault();
            cmList = cmRepo.GetLists(cmClient.ClientId);
            crmList = crmRepo.GetMarketingLists();
        }

       

        private void InitializeTable()
        {
            crmCmConnections = ManageMapTable.LoadMapTable();
            if (crmCmConnections == null)
                crmCmConnections = new List<CrmCmConnection>();

            this.Dispatcher.Invoke((Action)(() =>
            {
                MapListDataGrid.ItemsSource = crmCmConnections;
                LoadingScreen.Visibility = System.Windows.Visibility.Hidden;
            }));          
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowInTaskbar = false;
            settingsWindow.Topmost = true; 
            settingsWindow.ShowDialog(); 
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Owner = this; 
            serviceWindow.ShowInTaskbar = false;
            serviceWindow.Topmost = true; 
            serviceWindow.ShowDialog(); 
        }

      

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddWindow addWindow = new AddWindow(cmList, crmList, crmCmConnections);
            addWindow.Owner = this;
            addWindow.ShowInTaskbar = false;
            addWindow.Topmost = true;
            addWindow.ShowDialog();
            MapListDataGrid.Items.Refresh();
        }

        private void StopServiceButton_Click(object sender, RoutedEventArgs e)
        {
            controler = new ServiceController("Sync Crm Cm Service");
            if (controler.Status == ServiceControllerStatus.Running)
            {
                controler.Stop();
                Thread.Sleep(1000);
                ControlLight();
            }
        }

        private void StartServiceButton_Click(object sender, RoutedEventArgs e)
        {
            controler = new ServiceController("Sync Crm Cm Service");
            if (controler.Status == ServiceControllerStatus.Stopped)
            { 
                controler.Start();
                Thread.Sleep(1000);
                ControlLight();
            }
        }


        private void ControlLight()
        {
            controler  = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "Sync Crm Cm Service");
            if (controler == null) {
                SyncText.Text = "Sync Service not Installed";
                return;
            }
                
            
            if (controler.Status == ServiceControllerStatus.Running)
            {
                RunLight.Visibility = System.Windows.Visibility.Visible;
                StopLight.Visibility = System.Windows.Visibility.Hidden;
                SyncText.Text = "Synchronization Running";
               
            }
            else
            {

                RunLight.Visibility = System.Windows.Visibility.Hidden;
                StopLight.Visibility = System.Windows.Visibility.Visible;
                SyncText.Text = "Synchronization Stopped";
            }
        }

        private void DeleteConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            CrmCmConnection deleteConnection = (CrmCmConnection)MapListDataGrid.SelectedItem;

            crmCmConnections.Remove(deleteConnection);
            MapListDataGrid.Items.Refresh();
            ManageMapTable.SaveMapTable(crmCmConnections);
        }
        
        private void ErrorLogButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorLogWindow errorLogWindow = new ErrorLogWindow(EventType.Error);
            errorLogWindow.Owner = this;
            errorLogWindow.ShowInTaskbar = false;
            errorLogWindow.Topmost = true;
            errorLogWindow.ShowDialog();
        }

        private void SyncLogButton_Click(object sender, RoutedEventArgs e)
        {
            SyncLogWindow syncLogWindow = new SyncLogWindow(EventType.Sync);
            syncLogWindow.Owner = this;
            syncLogWindow.ShowInTaskbar = false;
            syncLogWindow.Topmost = true;
            syncLogWindow.ShowDialog(); 
        }    
    }
}