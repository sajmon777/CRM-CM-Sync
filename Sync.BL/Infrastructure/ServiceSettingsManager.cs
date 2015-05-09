using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sync.BL.Infrastructure
{
    public class ServiceSettingsManager
    {

        public static int GetRefreshRate()
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                if ((rk.OpenSubKey("CM_CRM_Sync") == null) || (rk.GetValue("RefreshRate") == null))
                    return 2;
                return Convert.ToInt32(rk.GetValue("RefreshRate").ToString());
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        public static void SetRefreshRate(String  refreshRate)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                if (rk.OpenSubKey("CM_CRM_Sync") == null)
                    rk.CreateSubKey("CM_CRM_Sync");
                else
                    rk.OpenSubKey("CM_CRM_Sync");
                rk.SetValue("RefreshRate", refreshRate);
            }
            catch (Exception e)
            {
                throw e; 
            }
        }
    }
}
