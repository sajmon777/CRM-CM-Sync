using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Sync.Core;
using Sync.BL.Infrastructure;
using System.IO;
namespace Sync.Service
{
    public partial class CrmCmSyncService : ServiceBase
    {
        SyncThread syncTestThread;

        public CrmCmSyncService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLogManager.WriteMessage(new Message() { Description = "Service Start", Type = "Service", Time = DateTime.Now }, EventType.Test);
            try
            {
                syncTestThread = new SyncThread();
                syncTestThread.Run();

            }
            catch (Exception e)
            {
                EventLogManager.WriteMessage(new Message() { Description = "On Start Method: " + e.Message.ToString(), Type = "Service", Time = DateTime.Now }, EventType.Test);
            }
        }

        protected override void OnStop()
        {


            //EventLogManager.WriteMessage(new Message() { Description = "Service Start", Type = "Service", Time = DateTime.Now }, MessageType.Test);
            //try
            //{
            //    syncTestThread = new SyncThread();
            //    syncTestThread.Run();

            //}
            //catch (Exception e)
            //{
            //    EventLogManager.WriteMessage(new Message() { Description = "On Start Method: " + e.Message.ToString(), Type = "Service", Time = DateTime.Now }, MessageType.Test);
            //}



            EventLogManager.WriteMessage(new Message() { Description = "Service Stop", Type = "Service", Time = DateTime.Now }, EventType.Test);

            syncTestThread.Stop();
            try
            {
                syncTestThread.Stop();
            }
            catch (Exception e)
            {
                EventLogManager.WriteMessage(new Message() { Description = "On Stop Method: " + e.Message.ToString(), Type = "Service", Time = DateTime.Now }, EventType.Test);
            }
        }
    }
}
