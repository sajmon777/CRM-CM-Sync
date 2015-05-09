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

namespace Sync.WPF
{
    /// <summary>
    /// Interaction logic for MapTable.xaml
    /// </summary>
    public partial class MapTable : Window
    {
        CMRepository cmRepo;
        CMSettings cmSettings;
        CRMRepository crmRepo;
        CRMSettings crmSettings;
        List<CrmCmConnection> crmCmConnections;
        Thread readFromRepo;

        public MapTable()
        {
            InitializeComponent();
            readFromRepo = new Thread(RunInThread);
            readFromRepo.Start();
        }

        private void RunInThread()
        {
            try
            {
                InitializeRepository();
                InitializeComboBox();
                InitializeTable();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
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
            List<Sync.BL.Entities.List> cmList;
            List<Sync.BL.Entities.List> crmList;

            Client cmClient = cmRepo.GetClients().FirstOrDefault();
            cmList = cmRepo.GetLists(cmClient.ClientId);
            crmList = crmRepo.GetMarketingLists();

            this.Dispatcher.Invoke((Action)(() =>
            {
                Binding cmListBinding = new Binding();
                cmListBinding.Source = cmList;
                CmListComboBox.DisplayMemberPath = "Name";
                CmListComboBox.SelectedValuePath = "ListId";
                CmListComboBox.SetBinding(ComboBox.ItemsSourceProperty, cmListBinding);

                Binding crmListBinding = new Binding();
                crmListBinding.Source = crmList;
                CrmListComboBox.DisplayMemberPath = "Name";
                CrmListComboBox.SelectedValuePath = "ListId";
                CrmListComboBox.SetBinding(ComboBox.ItemsSourceProperty, crmListBinding);
            }));
        }

        private void InitializeTable()
        {
            crmCmConnections = ManageMapTable.LoadMapTable();
            if (crmCmConnections == null)
                crmCmConnections = new List<CrmCmConnection>();

            this.Dispatcher.Invoke((Action)(() =>
            {
                ListDataGrid.ItemsSource = crmCmConnections;
            }));
        }

        private void AddConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            Sync.BL.Entities.List crmList = (Sync.BL.Entities.List)CrmListComboBox.SelectedItem;
            Sync.BL.Entities.List cmList = (Sync.BL.Entities.List)CmListComboBox.SelectedItem;

            CrmCmConnection newConnection = new CrmCmConnection()
            {
                CmListName = cmList.Name,
                CmListId = cmList.ListId,
                CrmListName = crmList.Name,
                CrmListId = crmList.ListId,
                LastSync = new DateTime(1910, 1, 1)
            };
            crmCmConnections.Add(newConnection);
            ListDataGrid.Items.Refresh();
            ManageMapTable.SaveMapTable(crmCmConnections);
        }

        private void deleteConnection_Click(object sender, RoutedEventArgs e)
        {
            
            CrmCmConnection deleteConnection = (CrmCmConnection)ListDataGrid.SelectedItem;

            crmCmConnections.Remove(deleteConnection);
            ListDataGrid.Items.Refresh();
            ManageMapTable.SaveMapTable(crmCmConnections);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.ShowDialog(); 
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            readFromRepo.Abort();
            readFromRepo = new Thread(RunInThread);
            readFromRepo.Start();
        }

        private void Service_Click(object sender, RoutedEventArgs e)
        {
            ServiceControl serviceWindow = new ServiceControl();
            serviceWindow.ShowDialog(); 
        }

        private void Error_Click(object sender, RoutedEventArgs e)
        {
            ErrorList error = new ErrorList();
            error.ShowDialog(); 
        }

    }
}
