﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Sync.Service
{
   public  static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
     public static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new CrmCmSyncService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
