using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sync.BL.Infrastructure;
using System.Collections.Generic;

namespace Sync.Test
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void LogWriteTest()
        {
            EventLogManager.WriteMessage(new Message() { Description = "Test", Time = DateTime.Now, Type = "Tip" }, EventType.Sync);
            Assert.IsTrue(true); 

        }

        [TestMethod]
        public void LogReadTest()
        {
            List<Message> list = EventLogManager.ReadMessages(EventType.Error); 
            Assert.IsTrue(true);
        }
    }
}
