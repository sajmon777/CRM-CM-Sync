using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sync.BL.CRM
{
    public class CRMSettings
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public bool ProtocolSSH { get; set; }
    }
}
