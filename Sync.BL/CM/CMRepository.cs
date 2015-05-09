using Sync.BL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using createsend_dotnet;
using AutoMapper;

namespace Sync.BL.CM
{
    public class CMRepository : ICMRepository
    {
        public CMSettings cmSettings { get; set; }


        public CMRepository(CMSettings theSettings)
        {
            cmSettings = theSettings;
            IMap map = new CMMap();
            map.CreateMaps();
        }

        //#region System
        //public DateTime GetSystemDate()
        //{
        //    ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
        //    Account acc= new Account(); 

        //    SystemDateResult result = new SystemDateResult(); 
        //    result.
        //} 
        
        //#endregion
        
        #region List

        public List<Entities.List> GetLists(string ClientId)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                Client client = new Client(auth, ClientId);
                List<BasicList> list = client.Lists().ToList();
                return (List<Entities.List>)Mapper.Map<IList<BasicList>, IList<Entities.List>>(list);

            }
            catch (Exception e)
            {
                throw new Exception("Reading CM list (" + e.Message.ToString() + ")");
            }
        }

        #endregion
        #region Client
        public List<Entities.Client> GetClients()
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                General genaral = new General(auth);
                List<BasicClient> clients = genaral.Clients().ToList();
                return (List<Entities.Client>)Mapper.Map<IList<BasicClient>, IList<Entities.Client>>(clients);
            }
            catch (Exception e)
            {
                throw new Exception("Get clients CM (" + e.Message.ToString() + ")");
            }
        }
        #endregion
        #region Subscriber
        public List<Entities.Subscriber> GetAciveSubscribers(string ListId)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                List list = new List(auth, ListId);
                List<SubscriberDetail> allSubscribers = new List<SubscriberDetail>();
                allSubscribers.AddRange(list.Active().Results);
                return (List<Entities.Subscriber>)Mapper.Map<IList<SubscriberDetail>, IList<Entities.Subscriber>>(allSubscribers);
            }
            catch (Exception e)
            {

                throw new Exception("Get active subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public List<Entities.Subscriber> GetAciveSubscribers(string ListId, DateTime fromDate)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                List list = new List(auth, ListId);
                List<SubscriberDetail> allSubscribers = new List<SubscriberDetail>();
                PagedCollection<SubscriberDetail> firstPage = list.Active(fromDate, 1, 100, "Email", "ASC");
                allSubscribers.AddRange(firstPage.Results);
                if (firstPage.NumberOfPages > 1)
                    for (int pageNumber = 2; pageNumber <= firstPage.NumberOfPages; pageNumber++)
                    {
                        PagedCollection<SubscriberDetail> subsequentPage = list.Active(fromDate, pageNumber, 100, "Email", "ASC");
                        allSubscribers.AddRange(subsequentPage.Results);
                    }
                return (List<Entities.Subscriber>)Mapper.Map<IList<SubscriberDetail>, IList<Entities.Subscriber>>(allSubscribers);
            }
            catch (Exception e)
            {

                throw new Exception("Get active subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public List<Entities.Subscriber> GetBouncedSubscribers(string ListId)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                List list = new List(auth, ListId);
                List<SubscriberDetail> allSubscribers = new List<SubscriberDetail>();
                allSubscribers.AddRange(list.Bounced().Results);
                return (List<Entities.Subscriber>)Mapper.Map<IList<SubscriberDetail>, IList<Entities.Subscriber>>(allSubscribers);
            }
            catch (Exception e)
            {

                throw new Exception("Get bounced subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public List<Entities.Subscriber> GetUnsubscribedSubscribers(string ListId)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                List list = new List(auth, ListId);
                List<SubscriberDetail> allSubscribers = new List<SubscriberDetail>();
                allSubscribers.AddRange(list.Unsubscribed().Results);
                return (List<Entities.Subscriber>)Mapper.Map<IList<SubscriberDetail>, IList<Entities.Subscriber>>(allSubscribers);
            }
            catch (Exception e)
            {

                throw new Exception("Get unsubscribed subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public List<Entities.Subscriber> GetDeletedSubscribers(string ListId)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                List list = new List(auth, ListId);
                List<SubscriberDetail> allSubscribers = new List<SubscriberDetail>();
                allSubscribers.AddRange(list.Deleted().Results);
                return (List<Entities.Subscriber>)Mapper.Map<IList<SubscriberDetail>, IList<Entities.Subscriber>>(allSubscribers);
            }
            catch (Exception e)
            {

                throw new Exception("Get deleted subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public void AddSubscribers(string ListId, List<Entities.Subscriber> subscribers)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                Subscriber subscriber = new Subscriber(auth, ListId);
                List<SubscriberDetail> importSubscribers = (List<SubscriberDetail>)Mapper.Map<IList<Entities.Subscriber>, IList<SubscriberDetail>>(subscribers);
                if (!subscribers.Any()) return;
                subscriber.Import(importSubscribers, true);
            }
            catch (Exception e)
            {

                throw new Exception("Add subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public void DeleteSubscribers(string ListId, List<Entities.Subscriber> subscribers)
        {
            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                Subscriber subscriber = new Subscriber(auth, ListId);
                foreach (Entities.Subscriber sub in subscribers)
                {
                    subscriber.Delete(sub.E_mail);
                }
            }
            catch (Exception e)
            {

                throw new Exception("Delete subscribers CM (" + e.Message.ToString() + ")");
            }
        }

        public void UnsubscribeSubscribers(string ListId, List<Entities.Subscriber> subscribers)
        {

            try
            {
                ApiKeyAuthenticationDetails auth = new ApiKeyAuthenticationDetails(cmSettings.ApiKey);
                Subscriber subscriber = new Subscriber(auth, ListId);
                foreach (Entities.Subscriber sub in subscribers)
                {
                    subscriber.Unsubscribe(sub.E_mail);
                }
            }
            catch (Exception e)
            {

                throw new Exception("Unsubscribe subscribers CM (" + e.Message.ToString() + ")");
            }

        }
        #endregion
    }
}
