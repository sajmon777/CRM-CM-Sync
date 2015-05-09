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
using Sync.BL.Infrastructure;
using Sync.BL.CM;
using Sync.BL.CRM;
using Microsoft.Win32;
using Sync.BL.Entities;
using System.Threading;

namespace Sync_WPFBlend
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        CMRepository cmRepo;
        CMSettings cmSettings;
        CRMRepository crmRepo;
        CRMSettings crmSettings;

        public SettingsWindow()
        {
            this.InitializeComponent();
            readRegistry();
        }

        private void readRegistry()
        {
            CMSettings cmSettings = ConnectionSettingsManager.GetCMSettings();
            CRMSettings crmSettings = ConnectionSettingsManager.GetCRMSettings();
            int refreshRate = ServiceSettingsManager.GetRefreshRate();

            if (cmSettings != null)
            {
                CMUserName.Text = cmSettings.UserName;
                CMPassword.Password = cmSettings.Password;
                CMApiKey.Text = cmSettings.ApiKey;
            }
            if (crmSettings != null)
            {

                CRMUserName.Text = crmSettings.UserName;
                CRMPassword.Password = crmSettings.Password;
                CRMDomain.Text = crmSettings.Domain;
                CRMUrl.Text = crmSettings.Url;
                if (crmSettings.ProtocolSSH)
                    CRMProtocol.SelectedIndex = 1;
                else
                    CRMProtocol.SelectedIndex = 0;
            }

            RefreshRate.Text = refreshRate.ToString();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            CMSettings cmSettings;
            CRMSettings crmSettings;

            cmSettings = new CMSettings()
            {
                UserName = CMUserName.Text,
                Password = CMPassword.Password,
                ApiKey = CMApiKey.Text
            };

            bool protocilSsh;
            if (CRMProtocol.SelectedIndex == 0)
                protocilSsh = false;
            else
                protocilSsh = true;

            crmSettings = new CRMSettings()
            {
                UserName = CRMUserName.Text,
                Password = CRMPassword.Password,
                Domain = CRMDomain.Text,
                Url = CRMUrl.Text,
                ProtocolSSH = protocilSsh
            };

            ConnectionSettingsManager.SetCMSettings(cmSettings);
            ConnectionSettingsManager.SetCRMSettings(crmSettings);
            ServiceSettingsManager.SetRefreshRate(RefreshRate.Text);

            MessageBox.Show("Settings saved successfully");
            this.Close();
        }

        private void CMTestConnectionButton__Click(object sender, RoutedEventArgs e)
        {
            //Test Screen 
            TestingScreen.Visibility = System.Windows.Visibility.Visible;
            Thread cmTestThread = new Thread(TestCm);
            cmTestThread.IsBackground = true;
            cmTestThread.Start();
        }

        private void CRMTestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            //Test Screen 
            TestingScreen.Visibility = System.Windows.Visibility.Visible;
            Thread crmTestThread = new Thread(TestCrm);
            crmTestThread.IsBackground = true;
            crmTestThread.Start();
        }



        private void TestCm()
        {
            List<Sync.BL.Entities.List> cmList;

            this.Dispatcher.Invoke((Action)(() =>
                {
                    cmSettings = new CMSettings()
                    {
                        UserName = CMUserName.Text,
                        Password = CMPassword.Password,
                        ApiKey = CMApiKey.Text
                    };
                }));

            try
            {
                cmRepo = new CMRepository(cmSettings);
                Client cmClient = cmRepo.GetClients().FirstOrDefault();
                cmList = cmRepo.GetLists(cmClient.ClientId);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Connection is OK");
                    TestingScreen.Visibility = System.Windows.Visibility.Hidden;
                }));
            }
            catch
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Test failed!");
                    TestingScreen.Visibility = System.Windows.Visibility.Hidden;
                }));
            }
        }

        private void TestCrm()
        {
            List<Sync.BL.Entities.List> crmList;
            bool protocilSsh;


            this.Dispatcher.Invoke((Action)(() =>
                {
                    if (CRMProtocol.SelectedIndex == 0)
                        protocilSsh = false;
                    else
                        protocilSsh = true;

                    crmSettings = new CRMSettings()
                    {
                        UserName = CRMUserName.Text,
                        Password = CRMPassword.Password,
                        Domain = CRMDomain.Text,
                        Url = CRMUrl.Text,
                        ProtocolSSH = protocilSsh
                    };
                }));
            
            try
            {
                crmRepo = new CRMRepository(crmSettings);
                crmList = crmRepo.GetMarketingLists();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Connection is OK");
                    TestingScreen.Visibility = System.Windows.Visibility.Hidden;
                }));
            }
            catch
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Test failed!");
                    TestingScreen.Visibility = System.Windows.Visibility.Hidden;
                }));
            }
        }
    }
}