using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sync.BL.Infrastructure;
using System.Diagnostics;
using Microsoft.Win32;

namespace Sync.Core
{
    public class SyncThread
    {
        private readonly Thread syncThread;
        private long refreshRate = 2;

        private SyncCrmCm syncCrmCm;

        public SyncThread()
        {

            try
            {
                syncThread = new Thread(SyncList);
            }
            catch (Exception e)
            {

                EventLogManager.WriteMessage(new Message() { Description = e.ToString(), System = "Sync", Time = DateTime.Now }, EventType.Test);
            }
        }

        void SyncList()
        {
            //delete this sleep time 
            Thread.Sleep(30000);

            while (true)
            {
                //Read refresh rate from registry 
                try
                {
                    refreshRate = Convert.ToInt64(ServiceSettingsManager.GetRefreshRate()); //In minut
                    if (refreshRate == 0) throw new Exception();
                }
                catch
                {
                    refreshRate = 2;
                }
                refreshRate = refreshRate * 60000;
                Stopwatch watch = Stopwatch.StartNew();

                try
                {
                    syncCrmCm = new SyncCrmCm();
                    syncCrmCm.Sync();

                }
                catch (Exception e)
                {

                    EventLogManager.WriteMessage(new Message() { Description = e.Message.ToString(), System = "Sync", Time = DateTime.Now }, EventType.Error);
                }
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;
                long timeToSleep = refreshRate - elapsedMs;
                if (timeToSleep > 0)
                    Thread.Sleep(Convert.ToInt32(timeToSleep));
            }
        }

        public void Run()
        {
            syncThread.Start();
        }

        public void Stop()
        {
            syncThread.Abort();
        }
    }
}
