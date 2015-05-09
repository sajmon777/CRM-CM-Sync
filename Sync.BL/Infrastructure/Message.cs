using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sync.BL.Infrastructure
{
    public class Message
    {
        public string   Description { get; set; }
        public DateTime Time { get; set; }
        public string   Type { get; set; }
        public string   ListName { get; set; }
        public string   System { get; set; }
        public int      SubscribersNumber { get; set; }
    }
}
