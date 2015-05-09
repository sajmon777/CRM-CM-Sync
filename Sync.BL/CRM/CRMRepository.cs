using Sync.BL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel.Description;
using Microsoft.Xrm.Client;
using Xrm;
using Microsoft.Xrm.Sdk.Client;

namespace Sync.BL.CRM
{
    public class CRMRepository : ICRMRepository
    {
        private XrmServiceContext context;
        private CrmConnection crmConnection;

        public CRMRepository(CRMSettings crmSettings)
        {
            crmConnection = new CrmConnection();

            try
            {
                string uriString;
                if (!crmSettings.ProtocolSSH)
                {
                    uriString = "http://" + crmSettings.Url;
                }
                else
                {
                    uriString = "https://" + crmSettings.Url;
                }
                crmConnection.ServiceUri = new Uri(uriString);
                ClientCredentials crmCredentials = new ClientCredentials();
                crmCredentials.Windows.ClientCredential.UserName = crmSettings.UserName;
                crmCredentials.Windows.ClientCredential.Password = crmSettings.Password;
                crmCredentials.Windows.ClientCredential.Domain = crmSettings.Domain;
                crmConnection.ClientCredentials = crmCredentials;
                context = new XrmServiceContext(crmConnection);
                context.MergeOption = Microsoft.Xrm.Sdk.Client.MergeOption.NoTracking;
            }
            catch (Exception e)
            {

                throw e; 
            }

            //Object mapping 
            CRMMap map = new CRMMap();
            map.CreateMaps();
        }
        public List<Entities.List> GetMarketingLists()
        {
            try
            {
                List<List> marketingLists = context.ListSet.ToList();
                return (List<Entities.List>)Mapper.Map<IList<List>, IList<Entities.List>>(marketingLists);
            }
            catch (Exception e)
            {
                throw new Exception("Get marketing lists CRM (" + e.Message.ToString() + ")");
            }
        }
        public List<Entities.Subscriber> GetSubscribers(string listId)
        {
            try
            {
                List<Contact> subscribers = (from m in context.CreateQuery<ListMember>()
                                             join c in context.CreateQuery<Contact>() on m.EntityId.Id equals c.Id
                                             where m.ListId.Equals(listId)
                                             select c).ToList();

                return (List<Entities.Subscriber>)Mapper.Map<IList<Contact>, IList<Entities.Subscriber>>(subscribers);
            }
            catch (Exception e)
            {
                throw new Exception("Get marketing lists CRM (" + e.Message.ToString() + ")");
            }
        }
        public List<Entities.Subscriber> GetSubscribers(string listId, DateTime fromDate)
        {
            try
            {
                List<Contact> subscribers = (from m in context.CreateQuery<ListMember>()
                                             join c in context.CreateQuery<Contact>() on m.EntityId.Id equals c.Id
                                             where m.ListId.Equals(listId) && m.CreatedOn > fromDate
                                             select c).ToList();

                return (List<Entities.Subscriber>)Mapper.Map<IList<Contact>, IList<Entities.Subscriber>>(subscribers);
            }
            catch (Exception e)
            {

                throw new Exception("Get subscribers CRM (" + e.Message.ToString() + ")");
            }
        }
        public void AddSubscribers(string ListId, List<Entities.Subscriber> subscribers)
        {
            try
            {
                foreach (Entities.Subscriber subscriber in subscribers)
                {
                    Contact contact = (Contact)context.ContactSet.Where(x => x.EMailAddress1.Equals(subscriber.E_mail)).FirstOrDefault();
                    Guid listGuid = new Guid(ListId);
                    if (contact == null)
                    {
                        Guid contactGuid = context.Create(new Contact() { EMailAddress1 = subscriber.E_mail, FirstName = subscriber.Name });
                        var member = new AddMemberListRequest() { EntityId = contactGuid, ListId = listGuid };
                        context.Execute(member);
                    }
                    else
                    {
                        var member = new AddMemberListRequest() { EntityId = contact.Id, ListId = listGuid };
                        context.Execute(member);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Get subscribers CRM (" + e.Message.ToString() + ")");
            }
        }
        public void UnsubscribeSubscribers(string ListId, List<Entities.Subscriber> subscribers)
        {
            try
            {
                Guid listGuid = new Guid(ListId);
                RemoveMemberListRequest req = new RemoveMemberListRequest();
                req.ListId = listGuid;
                foreach (Entities.Subscriber subscriber in subscribers)
                {
                    Contact contact = (Contact)context.ContactSet.Where(x => x.EMailAddress1.Equals(subscriber.E_mail)).FirstOrDefault();
                    if (contact != null)
                    {
                        req.EntityId = contact.Id;
                        RemoveMemberListResponse resp = (RemoveMemberListResponse)context.Execute(req);
                    }
                }
            }
            catch (Exception e)
            {

                throw new Exception("Unsubscribe subscribers CRM (" + e.Message.ToString() + ")");
            }
        }
        public void AddContact()
        {
            throw new NotImplementedException();
            //   List<Contact> contacts= (List<Contact>)Mapper.Map<IList<Entities.Subscriber>, IList<Contact>>(subscribers);
            //foreach (Contact contact in contacts) {
            //    context.Create(11contact); 
        }

        //public void GetLocalZone() {
        //    int? _localeId;
        //    int? _timeZoneCode;
        //    string _timeZoneName;
        //    OrganizationServiceProxy _serviceProxy;
            
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(serverConfig))
        //    if (string.IsNullOrWhiteSpace(_timeZoneName) || !_localeId.HasValue)
        //        return;

        //    var request = new GetTimeZoneCodeByLocalizedNameRequest
        //    {
        //        LocaleId = _localeId.Value,
        //        LocalizedStandardName = _timeZoneName
        //    };

        //    var response = (GetTimeZoneCodeByLocalizedNameResponse)_serviceProxy.Execute(request);

        //}

    }

}
