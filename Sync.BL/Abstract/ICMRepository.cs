using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sync.BL.Entities;

namespace Sync.BL.Abstract
{
    public interface ICMRepository
    {
        List<Subscriber> GetAciveSubscribers(string ListId);
        List<Subscriber> GetAciveSubscribers(string ListId, DateTime fromDate);
        List<Subscriber> GetBouncedSubscribers(string ListId);
        List<Subscriber> GetUnsubscribedSubscribers(string ListId);
        List<Subscriber> GetDeletedSubscribers(string ListId);
        void AddSubscribers(string ListId, List<Subscriber> subscribers);
        void DeleteSubscribers(string ListId, List<Subscriber> subscribers);
        void UnsubscribeSubscribers(string ListId, List<Subscriber> subscribers);
        List<Entities.List> GetLists(string ClientId);
        List<Client> GetClients();
       // DateTime GetSystemDate(); 
    }
}
