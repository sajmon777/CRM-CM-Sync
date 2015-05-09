using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sync.BL.CRM;
using Sync.BL.Entities;
using System.Collections.Generic;

namespace Sync.Test
{
    [TestClass]
    public class CMRApiTest
    {
        [TestMethod]
        public void MarketingListTest()
        {
            CRMSettings cmrSettings = new CRMSettings()
            {
                UserName = "Rokp",
                Password = "SpremeniGeslo33",
                Url = "internalcrm.virtua-it.si/TestiranjeCRM/",
                Domain="it",
                ProtocolSSH=true
            };
            CRMRepository repo = new CRMRepository(cmrSettings);
            List<Sync.BL.Entities.List> marketnigLists = repo.GetMarketingLists();
            Assert.IsNotNull(marketnigLists);   
         }
        
        [TestMethod]
        public void CRMSubscribersTest()
        {
            CRMSettings cmrSettings = new CRMSettings()
            {
                UserName = "Rokp",
                Password = "SpremeniGeslo33",
                Url = "internalcrm.virtua-it.si/TestiranjeCRM/",
                Domain = "it",
                ProtocolSSH = true
            };
            CRMRepository repo = new CRMRepository(cmrSettings);
            List<Sync.BL.Entities.Subscriber> marketnigLists = repo.GetSubscribers("328ec6c8-2f1e-e411-9419-005056a91611");
            Assert.IsNotNull(marketnigLists);
        }

        [TestMethod]
        public void CRMSubscribersDateTest()
        {
            CRMSettings cmrSettings = new CRMSettings()
            {
                UserName = "Rokp",
                Password = "SpremeniGeslo33",
                Url = "internalcrm.virtua-it.si/TestiranjeCRM/",
                Domain = "it",
                ProtocolSSH = true
            };
            DateTime lastSync = DateTime.Now.AddHours(-3);  
            CRMRepository repo = new CRMRepository(cmrSettings);
            List<Sync.BL.Entities.Subscriber> marketnigLists = repo.GetSubscribers("328ec6c8-2f1e-e411-9419-005056a91611",lastSync);
            Assert.IsNotNull(marketnigLists);
        }


        
        [TestMethod]
        public void AddCRMSubscribersTest()
        {
            CRMSettings cmrSettings = new CRMSettings()
            {
                UserName = "Rokp",
                Password = "SpremeniGeslo33",
                Url = "https://internalcrm.virtua-it.si/TestiranjeCRM/",
                Domain = "it"
            };
            List<Subscriber> importSubscribers = new List<Subscriber>() { 
                new Subscriber(){ E_mail="joze@jama.si", Name="Joze"},
                new Subscriber(){ E_mail="miha.koko@tovarna.si", Name="Miha"}, 
                new Subscriber(){ E_mail="mojca@trgovina.si", Name="Mojca"} 
            };
            CRMRepository repo = new CRMRepository(cmrSettings);
            repo.AddSubscribers("328ec6c8-2f1e-e411-9419-005056a91611",importSubscribers);
            Assert.IsTrue(true); 
        }

        [TestMethod]
        public void UnsubscribeCRMSubscribersTest()
        {
            CRMSettings cmrSettings = new CRMSettings()
            {
                UserName = "Rokp",
                Password = "SpremeniGeslo33",
                Url = "https://internalcrm.virtua-it.si/TestiranjeCRM/",
                Domain = "it"
            };
            List<Subscriber> importSubscribers = new List<Subscriber>() { 
                new Subscriber(){ E_mail="joze@jama.si", Name="Joze"},
                new Subscriber(){ E_mail="miha.koko@tovarna.si", Name="Miha"}, 
                new Subscriber(){ E_mail="mojca@trgovina.si", Name="Mojca"} 
            };
            CRMRepository repo = new CRMRepository(cmrSettings);
            repo.UnsubscribeSubscribers("328ec6c8-2f1e-e411-9419-005056a91611", importSubscribers);
            Assert.IsTrue(true);
        } 
    }
}
