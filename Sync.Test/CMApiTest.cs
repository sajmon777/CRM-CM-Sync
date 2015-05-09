using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sync.BL.CM;
using Sync.BL.Abstract;
using Sync.BL.Entities;
using System.Collections.Generic;

namespace Sync.Test
{
    [TestClass]
    public class CMApiTest
    {
        [TestMethod]
        public void ConnectionTest()
        {
            
        }

        [TestMethod]
        public void ClientsTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> list = repo.GetClients();  
            Assert.IsNotNull(list.Find(x=>x.Name=="Hal")); 
        }

        [TestMethod]
        public void ListTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            Assert.IsNotNull(list.Find(x => x.Name == "Crm"));
        }

        [TestMethod]
        public void ActiveSubscribersTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            List<Subscriber> subscribers = repo.GetAciveSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString());
            repo.GetAciveSubscribers(ListId: "dsds", fromDate: DateTime.Now); 
            Assert.IsNotNull(subscribers.Find(x => x.Name == "Xxxxx Yyyyy"));
        }

        [TestMethod]
        public void ActiveSubscribersByTimeTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            List<Subscriber> subscribers = repo.GetAciveSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString(),DateTime.Now.AddHours(-1));
            repo.GetAciveSubscribers(ListId: "dsds", fromDate: DateTime.Now);
            Assert.IsNotNull(subscribers.Find(x => x.Name == "Xxxxx Yyyyy"));
        }


        [TestMethod]
        public void UnsubscribedSubscribersTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            List<Subscriber> subscribers = repo.GetUnsubscribedSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString());
            Assert.IsNull(subscribers.Find(x => x.Name == "Xxxxx Yyyyy"));
        }

        [TestMethod]
        public void AddSubscribersTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            //Create Test Subscribers
            List<Subscriber> importSubscribers = new List<Subscriber>() { 
                new Subscriber(){ E_mail="joze@jama.si", Name="Joze"},
                new Subscriber(){ E_mail="miha.koko@tovarna.si", Name="Miha"}, 
                new Subscriber(){ E_mail="mojca@trgovina.si", Name="Mojca"} 
            };
            //Import Subscribers
            repo.AddSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString(), importSubscribers);
            //Test inported subscribers
            List<Subscriber> subscribers = repo.GetAciveSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString());
            Assert.IsNotNull(subscribers.Find(x => x.E_mail== "mojca@trgovina.si"));
        }

        [TestMethod]
        public void DeleteSubscribersTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            //Create Test Subscribers
            List<Subscriber> DeleteSubscribers = new List<Subscriber>() { 
                new Subscriber(){ E_mail="joze@jama.si", Name="Joze"},
                new Subscriber(){ E_mail="miha.koko@tovarna.si", Name="Miha"} 
            };
            //Delete Subscribers
            repo.DeleteSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString(), DeleteSubscribers);
            //Test inported subscribers
            List<Subscriber> subscribers = repo.GetAciveSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString());
            Assert.IsNull(subscribers.Find(x => x.E_mail == "miha.koko@tovarna.si"));
      }

        [TestMethod]
        public void UnsubscribeTest()
        {
            CMSettings settings = new CMSettings();
            settings.ApiKey = "4a087dfc720e09d91e8cb5cae9db51d5"; //Hard Coded
            ICMRepository repo = new CMRepository(settings);
            List<Client> clientList = repo.GetClients();
            List<List> list = repo.GetLists(clientList.Find(x => x.Name == "Hal").ClientId.ToString());
            //Create Test Subscribers
            List<Subscriber> UnsubscribeSubscribers = new List<Subscriber>() { 
                new Subscriber(){ E_mail="joze@jama.si", Name="Joze"},
                new Subscriber(){ E_mail="miha.koko@tovarna.si", Name="Miha"} 
            };
            //Delete Subscribers
            repo.UnsubscribeSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString(), UnsubscribeSubscribers);
            //Test inported subscribers
            List<Subscriber> subscribers = repo.GetAciveSubscribers(list.Find(x => x.Name == "Crm").ListId.ToString());
            Assert.IsNull(subscribers.Find(x => x.E_mail == "miha.koko@tovarna.si"));
        }
    }
}
