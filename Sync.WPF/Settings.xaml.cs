using Microsoft.Win32;
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

namespace Sync.WPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            readRegistry();
        }


        private void readRegistry()
        {
            CMSettings cmSettings = ConnectionSettingsManager.GetCMSettings();
            CRMSettings crmSettings = ConnectionSettingsManager.GetCRMSettings();

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

            //RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", true);
            //if (rk.OpenSubKey("CM_CRM_Sync") == null)
            //{
            //    rk.CreateSubKey("CM_CRM_Sync");
            //    rk.SetValue("CMUserName", "null");
            //    rk.SetValue("CMPassword", "null");
            //    rk.SetValue("CMApiKey", "null");
            //    rk.SetValue("CRMPassword", "null");
            //    rk.SetValue("CRMUserName", "null");
            //    rk.SetValue("CRMDomain", "null");
            //    rk.SetValue("CRMUrl", "null");
            //    rk.SetValue("CRMProtocol", "null");
            //}

            //if (!rk.GetValue("CMUserName").ToString().Equals("null"))
            //    CMUserName.Text = rk.GetValue("CMUserName").ToString();
            //if (!rk.GetValue("CMPassword").ToString().Equals("null"))
            //    CMPassword.Password = rk.GetValue("CMPassword").ToString();
            //if (!rk.GetValue("CMApiKey").ToString().Equals("null"))
            //    CMApiKey.Text = rk.GetValue("CMApiKey").ToString();
            //if (!rk.GetValue("CRMPassword").ToString().Equals("null"))
            //    CRMPassword.Password = rk.GetValue("CRMPassword").ToString();
            //if (!rk.GetValue("CRMUserName").ToString().Equals("null"))
            //    CRMUserName.Text = rk.GetValue("CRMUserName").ToString();
            //if (!rk.GetValue("CRMDomain").ToString().Equals("null"))
            //    CRMDomain.Text = rk.GetValue("CRMDomain").ToString();
            //if (!rk.GetValue("CRMUrl").ToString().Equals("null"))
            //    CRMUrl.Text = rk.GetValue("CRMUrl").ToString();
            //if (rk.GetValue("CRMProtocol").ToString().Equals("HTTPS"))
            //    CRMProtocol.SelectedIndex = 1;
            //else
            //    CRMProtocol.SelectedIndex = 0;

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



            //RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", true);
            //rk.OpenSubKey("CM_CRM_Sync");
            //rk.SetValue("CMUserName", CMUserName.Text);
            //rk.SetValue("CMPassword", CMPassword.Password);
            //rk.SetValue("CMApiKey", CMApiKey.Text);
            //rk.SetValue("CRMUserName", CRMUserName.Text);
            //rk.SetValue("CRMPassword", CRMPassword.Password);
            //rk.SetValue("CRMDomain", CRMDomain.Text);
            //rk.SetValue("CRMUrl", CRMUrl.Text);
            //if (CRMProtocol.SelectedIndex == 0)
            //    rk.SetValue("CRMProtocol", "HTTP");
            //else
            //    rk.SetValue("CRMProtocol", "HTTPS");

            MessageBox.Show("Settings saved successfully");
            SettingsWindow.Close();
        }
    }
}
