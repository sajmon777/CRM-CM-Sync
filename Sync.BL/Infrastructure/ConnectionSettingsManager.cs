using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Sync.BL.CM;
using Sync.BL.CRM;
namespace Sync.BL.Infrastructure
{
    public class ConnectionSettingsManager
    {
        public static CMSettings GetCMSettings()
        {

          RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            if (rk.OpenSubKey("CM_CRM_Sync") == null)
                return null;

            string UserName = rk.GetValue("CMUserName").ToString();
            string Password = rk.GetValue("CMPassword").ToString();
            string ApiKey = rk.GetValue("CMApiKey").ToString();


            return new CMSettings()
            {
                UserName = rk.GetValue("CMUserName").ToString(),
                Password = rk.GetValue("CMPassword").ToString(),
                ApiKey = rk.GetValue("CMApiKey").ToString()
            };        
        }

        //RegistryKey rk = LocalMachine.OpenSubKey(subkey, RegistryKeyPremissionsCheck.ReadWriteSubTree, RegistryRights.ChangePermissions | RegistryRights.ReadKey);//Get the registry key desired with ChangePermissions Rights.
        //RegistrySecurity rs = new RegistrySecurity();
        //rs.AddAccessRule(new RegistryAccessRule("Administrator", RegistryRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));//Create access rule giving full control to the Administrator user.
        //rk.SetAccessControl(rs); //Apply the new access rule to this Registry Key.
        //rk = LocalMachine.OpenSubKey(subkey, RegistryKeyPremissionsCheck.ReadWriteSubTree, RegistryRights.FullControl); // Opens the key again with full control.
        //rs.SetOwner(new NTAccount("Administrator"));// Set the securitys owner to be Administrator



        public static CRMSettings GetCRMSettings()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE",  true);
            if (rk.OpenSubKey("CM_CRM_Sync") == null)
                return null;

            bool protocolSSH = false;
            if (rk.GetValue("CRMProtocol").ToString().Equals("HTTPS"))
                protocolSSH = true;
            return new CRMSettings()
            {
                UserName = rk.GetValue("CRMUserName").ToString(),
                Password = rk.GetValue("CRMPassword").ToString(),
                Url = rk.GetValue("CRMUrl").ToString(),
                Domain = rk.GetValue("CRMDomain").ToString(),
                ProtocolSSH = protocolSSH
            };
        }

        public static void SetCMSettings(CMSettings settings)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            if (rk.OpenSubKey("CM_CRM_Sync") == null)
                rk.CreateSubKey("CM_CRM_Sync");
            else
                rk.OpenSubKey("CM_CRM_Sync");
            rk.SetValue("CMUserName", settings.UserName);
            rk.SetValue("CMPassword", settings.Password);
            rk.SetValue("CMApiKey", settings.ApiKey);
        }

        public static void SetCRMSettings(CRMSettings settings)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            if (rk.OpenSubKey("CM_CRM_Sync") == null)
                rk.CreateSubKey("CM_CRM_Sync");
            else
                rk.OpenSubKey("CM_CRM_Sync");
            rk.SetValue("CRMUserName", settings.UserName);
            rk.SetValue("CRMPassword", settings.Password);
            rk.SetValue("CRMDomain", settings.Domain);
            rk.SetValue("CRMUrl", settings.Url);
            if (!settings.ProtocolSSH)
                rk.SetValue("CRMProtocol", "HTTP");
            else
                rk.SetValue("CRMProtocol", "HTTPS");
        }
    }
}
