using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sync.Core;
using System.Threading;
using Sync.Service;

namespace Sync.Test
{
    [TestClass]
    public class SyncTest
    {
        [TestMethod]
        public void MainSyncTest()
        {

            Core.SyncCrmCm sync = new Core.SyncCrmCm();
            sync.Sync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ThreadSyncTest()
        {
            SyncThread syncTestThread = new SyncThread();
            syncTestThread.Run();
            Thread.Sleep(100000); 
        }

        [TestMethod]
        public void ServiceSyncTest()
        {
            Program.Main(); 
        }
    }
}
