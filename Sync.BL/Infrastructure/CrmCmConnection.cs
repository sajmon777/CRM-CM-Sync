using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sync.BL.Infrastructure
{
    public class CrmCmConnection
    {
        public DateTime LastSync { get; set; }
        public string CmListId { get; set; }
        public string CmListName { get; set; }
        public string CrmListId { get; set; }
        public string CrmListName { get; set; }
    }
}
