using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sync.BL.CM;
using Sync.BL.CRM;
using Sync.BL.Infrastructure;
using Sync.BL.Entities;

namespace Sync.Core
{
    public class SyncCrmCm
    {
        private CMRepository cmRepo;
        private CRMRepository crmRepo;
        private CMSettings cmSettings;
        private CRMSettings crmSettings;


        void SynCmCrm()
        {
            Initialization();
        }

        //Read from register (instalation path, connection settings)

        private void Initialization()
        {
            cmSettings = ConnectionSettingsManager.GetCMSettings();
            crmSettings = ConnectionSettingsManager.GetCRMSettings();
            try
            {
                cmRepo = new CMRepository(cmSettings);
                crmRepo = new CRMRepository(crmSettings);
            }
            catch (Exception)
            {
                EventLogManager.WriteMessage(new Message() { Description ="Connection settings error", Type = "Error", Time = DateTime.Now }, EventType.Error);
            }
        }

        public void Sync()
        {
            Initialization();
            List<List> cmList;
            List<List> crmList;
            Client cmClient = cmRepo.GetClients().FirstOrDefault();
            try
            {

                cmList = cmRepo.GetLists(cmClient.ClientId);
                crmList = crmRepo.GetMarketingLists();
            }
            catch (Exception e)
            {
                EventLogManager.WriteMessage(new Message() { Description = e.Message.ToString(), Type = "Error", Time = DateTime.Now }, EventType.Error);
                return;
            }
            string CmListID;
            string CmrListID;
            DateTime lastSync;


            List<CrmCmConnection> mapTable = ManageMapTable.LoadMapTable();


            foreach (CrmCmConnection connection in mapTable)
            {
                CmListID = connection.CmListId;
                CmrListID = connection.CrmListId;
                lastSync = connection.LastSync;

                List<Subscriber> cmSubscribers;
                List<Subscriber> crmSubscribers;

                try
                {
                    cmSubscribers = cmRepo.GetAciveSubscribers(CmListID);
                    crmSubscribers = crmRepo.GetSubscribers(CmrListID);
                }
                catch (Exception e)
                {
                    EventLogManager.WriteMessage(new Message() { Description = e.Message.ToString(), Type = "Error", Time = DateTime.Now }, EventType.Error);
                    break;
                }

                //Left Excluding JOIN using
                //A:CM B:CRM
                List<Subscriber> subscribersToCRM = (from a in cmSubscribers
                                                     join b in crmSubscribers on a.E_mail equals b.E_mail into j
                                                     from b in j.DefaultIfEmpty()
                                                     where b == null
                                                     select new Subscriber { Name = a.Name, E_mail = a.E_mail }).ToList();


                //A:CRM B:CM
                List<Subscriber> subscribersToCM = (from a in crmSubscribers
                                                    join b in cmSubscribers on a.E_mail equals b.E_mail into j
                                                    from b in j.DefaultIfEmpty()
                                                    where b == null
                                                    select new Subscriber { Name = a.Name, E_mail = a.E_mail }).ToList();
                //Delete or insert
                //new Subscribers from last sync
                List<Subscriber> newCmSubscribeers;
                List<Subscriber> newCrmSubscribers;

                try
                {
                    newCmSubscribeers = cmRepo.GetAciveSubscribers(CmListID, lastSync);
                    newCrmSubscribers = crmRepo.GetSubscribers(CmrListID, lastSync.AddHours(-2));
                    //newCrmSubscribers = crmRepo.GetSubscribers(CmrListID, lastSync.AddHours(-2));
                }
                catch (Exception e)
                {
                    EventLogManager.WriteMessage(new Message() { Description = e.Message.ToString(), Type = "Error", Time = DateTime.Now }, EventType.Error);
                    return;
                }


                //Delete
                List<Subscriber> subscribersDeleteToCM = (from a in subscribersToCM
                                                          join b in newCrmSubscribers on a.E_mail equals b.E_mail into j
                                                          from b in j.DefaultIfEmpty()
                                                          where b == null
                                                          select new Subscriber { Name = a.Name, E_mail = a.E_mail }).ToList();

                List<Subscriber> subscribersDeleteToCMR = (from a in subscribersToCRM
                                                           join b in newCmSubscribeers on a.E_mail equals b.E_mail into j
                                                           from b in j.DefaultIfEmpty()
                                                           where b == null
                                                           select new Subscriber { Name = a.Name, E_mail = a.E_mail }).ToList();
                try
                {
                    //Insert Delete
                    //CM
                    //ADD
                    if (newCrmSubscribers.Count > 0)
                    {
                        cmRepo.AddSubscribers(CmListID, newCrmSubscribers);
                        EventLogManager.WriteMessage(new Message() { Description = "Added", SubscribersNumber = newCrmSubscribers.Count, ListName = connection.CmListName, System = "CM", Time = DateTime.Now, Type = "Sync" }, EventType.Sync);
                    }
                    //DELETE
                    if (subscribersDeleteToCMR.Count > 0)
                    {
                        cmRepo.UnsubscribeSubscribers(CmListID, subscribersDeleteToCMR);
                        EventLogManager.WriteMessage(new Message() { Description = "Deleted", SubscribersNumber = subscribersDeleteToCMR.Count, ListName = connection.CmListName, System = "CM", Time = DateTime.Now, Type = "Sync" }, EventType.Sync); 
                    }
                    //CRM
                    //ADD
                    if (newCmSubscribeers.Count > 0)
                    {
                        crmRepo.AddSubscribers(CmrListID, newCmSubscribeers);
                        EventLogManager.WriteMessage(new Message() { Description = "Added", SubscribersNumber = newCmSubscribeers.Count, ListName = connection.CrmListName, System = "CRM", Time = DateTime.Now, Type = "Sync" }, EventType.Sync); 
                    }
                    //DELETE
                    if (subscribersDeleteToCM.Count > 0)
                    {
                        crmRepo.UnsubscribeSubscribers(CmrListID, subscribersDeleteToCM);
                        EventLogManager.WriteMessage(new Message() { Description = "Deleted", SubscribersNumber = subscribersDeleteToCM.Count, ListName = connection.CrmListName, System = "CRM", Time = DateTime.Now, Type = "Sync" }, EventType.Sync); 
                    }
                }
                catch (Exception e)
                {
                    EventLogManager.WriteMessage(new Message() { Description = e.Message.ToString(), Type = "Error", Time = DateTime.Now }, EventType.Error);
                    return;
                }

                //Save sync date
                connection.LastSync = DateTime.Now;
                ManageMapTable.SaveMapTable(mapTable);
            }
        }
    }
}
