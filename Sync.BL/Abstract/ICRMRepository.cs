using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sync.BL.Entities; 

namespace Sync.BL.Abstract
{
    interface ICRMRepository
    {
        List<List> GetMarketingLists();
        List<Subscriber> GetSubscribers(string listId);
        List<Subscriber> GetSubscribers(string listId, DateTime fromDate);
        void AddSubscribers(string ListId, List<Subscriber> subscribers);
        void UnsubscribeSubscribers(string ListId, List<Subscriber> subscribers);
        void AddContact();
    }
}
