using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sync.BL.Infrastructure;
using System.Collections.Generic; 

namespace Sync.Test
{
    [TestClass]
    public class MapTableTest
    {
        [TestMethod]
        public void LoadMapTable()
        {
            List<CrmCmConnection> list = ManageMapTable.LoadMapTable(); 
        
        }
    }
}
